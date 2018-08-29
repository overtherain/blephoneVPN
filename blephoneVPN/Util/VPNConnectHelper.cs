using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using DotRas;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Collections.ObjectModel;

namespace blephoneVPN.Util
{
    public class VPNConnectHelper
    {
        private static string VPNNAME = "blephoneVPN";
        private RasPhoneBook allUsersPhoneBook = null;
        private RasDialer dialer = null;
        private RasHandle handle = null;

        public static Type TAG = typeof(VPNConnectHelper);
        public string CurrentDirectory { get; private set; }
        public string PhoneBookPath { get; private set; }

        public delegate void DialStateChangeHandler(RasDialer dialer, StateChangedEventArgs e);
        public event DialStateChangeHandler DialStateChange;
        public delegate void DialAsyncCompleteHandler(RasDialer dialer, DialCompletedEventArgs e);
        public event DialAsyncCompleteHandler DialAsyncComplete;

        public VPNConnectHelper()
        {
            Log.debug(TAG, "VPNConnectHelper init end");

            allUsersPhoneBook = new RasPhoneBook();
            allUsersPhoneBook.Open();

            dialer = new RasDialer();
            dialer.EntryName = VPNNAME;
            dialer.Timeout = 20000;
            dialer.PhoneBookPath = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.AllUsers);
            dialer.StateChanged += new EventHandler<StateChangedEventArgs>(dialer_StateChanged);
            dialer.DialCompleted += new EventHandler<DialCompletedEventArgs>(dialer_DialCompleted);
        }

        private void CreateOrUpdateVPNEntry(string ip)
        {
            Log.debug(TAG, "CreateOrUpdateVPNEntry ip:" + ip);
            try
            {
                RasEntry entry = null;
                if (allUsersPhoneBook.Entries.Contains(VPNNAME))
                {
                    entry = allUsersPhoneBook.Entries[VPNNAME];
                    entry.EncryptionType = RasEncryptionType.Optional;
                    entry.PhoneNumber = ip;
                    IPAddress _ip;
                    IPAddress.TryParse(ip, out _ip);
                    entry.IPAddress = _ip;
                    entry.Options.UsePreSharedKey = true;
                    entry.UpdateCredentials(RasPreSharedKey.Client, "123456");
                    entry.Update();
                }
                else
                {
                    entry = RasEntry.CreateVpnEntry(VPNNAME, ip, RasVpnStrategy.L2tpOnly, RasDevice.GetDeviceByName("(L2TP)", RasDeviceType.Vpn));
                    entry.EncryptionType = RasEncryptionType.Optional;
                    entry.Options.UsePreSharedKey = true;
                    allUsersPhoneBook.Entries.Add(entry);
                    entry.UpdateCredentials(RasPreSharedKey.Client, "123456");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("创建VPN失败!error msg:" + ex.ToString());
                Log.debug(TAG, "CreateOrUpdateVPNEntry error:" + ex.ToString());
            }
        }

        public void DialAsync(string ip, string user, string pwd)
        {
            Log.debug(TAG, "DialAsync ip:" + ip + ", user:" + user);
            CreateOrUpdateVPNEntry(ip);
            dialer.Credentials = new NetworkCredential(user, pwd);
            handle = dialer.DialAsync();
        }

        public void Dial(string ip, string user, string pwd)
        {
            Log.debug(TAG, "Dial ip:" + ip + ", user:" + user);
            CreateOrUpdateVPNEntry(ip);
            dialer.Credentials = new NetworkCredential(user, pwd);
            handle = dialer.Dial();
        }

        public void Disconnect()
        {
            Log.debug(TAG, "Disconnect dialer.EntryName : " + dialer.EntryName);
            if (dialer.IsBusy)
            {
                dialer.DialAsyncCancel();
            }
            else
            {
                RasConnection connection = RasConnection.GetActiveConnectionByHandle(handle);
                if (connection != null)
                {
                    connection.HangUp();
                }
            }
            allUsersPhoneBook.Entries.Remove(VPNNAME);
        }

        private void dialer_DialCompleted(object sender, DialCompletedEventArgs e)
        {
            DialAsyncComplete((RasDialer)sender, e);
        }

        private void dialer_StateChanged(object sender, StateChangedEventArgs e)
        {
            DialStateChange((RasDialer)sender, e);
        }
    }
}
