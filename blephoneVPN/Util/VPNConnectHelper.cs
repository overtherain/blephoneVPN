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
        public static Type TAG = typeof(VPNConnectHelper);
        private RasPhoneBook allUsersPhoneBook ;
        private RasDialer dialer = new RasDialer();
        private readonly string VPNNAME = "blephoneVPN";

        public string CurrentDirectory { get; private set; }
        public string PhoneBookPath { get; private set; }

        public delegate void DialStateChangeHandler(RasDialer dialer, StateChangedEventArgs e);
        public event DialStateChangeHandler DialStateChange;

        public delegate void DialAsyncCompleteHandler(RasDialer dialer, DialCompletedEventArgs e);
        public event DialAsyncCompleteHandler DialAsyncComplete;

        public VPNConnectHelper()
        {
            CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            PhoneBookPath = Path.Combine(CurrentDirectory, @"Resource\rasphone.pbk");
            Console.WriteLine("VPNConnectHelper PhoneBookPath : " + PhoneBookPath);


            allUsersPhoneBook = new RasPhoneBook();
            allUsersPhoneBook.Open();
            dialer.EntryName = VPNNAME;
            //dialer.Timeout = 20000;
            dialer.PhoneBookPath = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.AllUsers);
            //allUsersPhoneBook.Open(dialer.PhoneBookPath);
            dialer.StateChanged += new EventHandler<StateChangedEventArgs>(dialer_StateChanged);
            dialer.DialCompleted += new EventHandler<DialCompletedEventArgs>(dialer_DialCompleted);
            Console.WriteLine("VPNConnectHelper init");
        }

        private void CreateOrUpdateVPNEntry(string ip, string user, string pwd)
        {
            Log.debug(TAG, "CreateOrUpdateVPNEntry ip:" + ip + ", user:" + user);
            Console.WriteLine("CreateOrUpdateVPNEntry ip:" + ip + ", user:" + user);
            try
            {
                RasEntry entry;
                if (!allUsersPhoneBook.Entries.Contains(VPNNAME))
                {
                    entry = RasEntry.CreateVpnEntry(VPNNAME, ip, RasVpnStrategy.L2tpOnly, RasDevice.GetDeviceByName("(L2TP)", RasDeviceType.Vpn));
                    entry.EncryptionType = RasEncryptionType.Optional;
                    allUsersPhoneBook.Entries.Add(entry);
                }
                else
                {
                    entry = allUsersPhoneBook.Entries[VPNNAME];
                    entry.PhoneNumber = ip;
                    IPAddress _ip;
                    IPAddress.TryParse(ip, out _ip);
                    entry.IPAddress = _ip;
                }
                NetworkCredential nc = new NetworkCredential(user, pwd);
                entry.UpdateCredentials(nc);
                entry.Update();
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
            Console.WriteLine("DialAsync ip:" + ip + ", user:" + user);
            CreateOrUpdateVPNEntry(ip, user, pwd);
            //Disconnect();
            dialer.DialAsync();
        }

        public void Dial(string ip, string user, string pwd)
        {
            Log.debug(TAG, "Dial ip:" + ip + ", user:" + user);
            Console.WriteLine("Dial ip:" + ip + ", user:" + user);
            CreateOrUpdateVPNEntry(ip, user, pwd);
            //Disconnect();
            dialer.Dial();
        }

        public void CancelDialAsync()
        {
            Log.debug(TAG, "CancelDialAsync");
            Console.WriteLine("CancelDialAsync");
            dialer.DialAsyncCancel();
        }

        public void Disconnect()
        {
            Console.WriteLine("Disconnect dialer.EntryName : " + dialer.EntryName);
            Log.debug(TAG, "Disconnect : " + dialer.EntryName);
            /*if (dialer.EntryName == VPNNAME)
            {
                Console.WriteLine("Disconnect dialer.EntryName : " + dialer.EntryName);
                dialer.DialAsyncCancel();
                RasConnection conn = dialer.CreateObjRef;
                dialer.Dispose();
                allUsersPhoneBook.Entries.Remove(VPNNAME);
                Log.debug(TAG, "Disconnect Remove PhoneBook");
            }*/
            //ReadOnlyCollection<RasConnection> conList = RasConnection.GetActiveConnections();
            foreach (RasConnection conn in dialer.GetActiveConnections())
            {
                Console.WriteLine("Disconnect conn.EntryName : " + conn.EntryName);
                if (conn.EntryName == VPNNAME)
                {
                    conn.HangUp();
                    allUsersPhoneBook.Entries.Remove(VPNNAME);
                    Log.debug(TAG, "Disconnect Remove PhoneBook");
                    return;
                }
            }
        }

        private void dialer_DialCompleted(object sender, DialCompletedEventArgs e)
        {
            Log.debug(TAG, "dialer_DialCompleted Error:" + e.Error);
            DialAsyncComplete((RasDialer)sender, e);
        }

        private void dialer_StateChanged(object sender, StateChangedEventArgs e)
        {
            Log.debug(TAG, "dialer_StateChanged State:" + e.State);
            DialStateChange((RasDialer)sender, e);
        }
    }
}
