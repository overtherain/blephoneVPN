using DotRas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using System.Net;

namespace blephoneVPN.Util
{
    class RASHelper
    {
        public static Type TAG = typeof(RASHelper);
        // 系统路径 C:\windows\system32\
        private static string WinDir = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\";
        // rasdial.exe
        private static string RasDialFileName = "rasdial.exe";
        // VPN路径 C:\windows\system32\rasdial.exe
        private static string VPNPROCESS = WinDir + RasDialFileName;
        // VPN地址
        public string IPToPing { get; set; }
        // VPN名称
        public string VPNName { get; set; }
        // VPN用户名
        public string UserName { get; set; }
        // VPN密码
        public string PassWord { get; set; }

        public string CurrentDirectory { get; private set; }
        public string PhoneBookPath { get; private set; }

        public RASHelper()
        {
            Log.debug(TAG, "RASHelper init");
            CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            PhoneBookPath = Path.Combine(CurrentDirectory, @"Resource\rasphone.pbk");
        }

        /// <summary>
        /// 带参构造函数
        /// </summary>
        /// <param name="_vpnIP"></param>
        /// <param name="_vpnName"></param>
        /// <param name="_userName"></param>
        /// <param name="_passWord"></param>
        public RASHelper(string _vpnIP, string _vpnName, string _userName, string _passWord)
        {
            Log.debug(TAG, "RASHelper input vars _vpnIP : " + _vpnIP + ", _vpnName : " + _vpnName + ", _userName : " + _userName);
            this.IPToPing = _vpnIP;
            this.VPNName = _vpnName;
            this.UserName = _userName;
            this.PassWord = _passWord;
        }

        /// <summary>
        /// 尝试连接VPN(默认VPN)
        /// </summary>
        /// <returns></returns>
        public void TryConnectVPN()
        {
            this.TryConnectVPN(this.VPNName,this.UserName,this.PassWord);
        }

        /// <summary>
        /// 尝试断开连接(默认VPN)
        /// </summary>
        /// <returns></returns>
        public void TryDisConnectVPN()
        {
            this.TryDisConnectVPN(this.VPNName);
        }

        /// <summary>
        /// 创建或更新一个默认的VPN连接
        /// </summary>
        public void CreateOrUpdateVPN()
        {
            this.CreateOrUpdateVPN(this.VPNName, this.IPToPing);
        }

        /// <summary>
        /// 尝试删除连接(默认VPN)
        /// </summary>
        /// <returns></returns>
        public void TryDeleteVPN()
        {
            this.TryDeleteVPN(this.VPNName);
        }
        /// <summary>
        /// 尝试连接VPN(指定VPN名称，用户名，密码)
        /// </summary>
        /// <returns></returns>
        public void TryConnectVPN(string connVpnName,string connUserName,string connPassWord)
        {
            Log.debug(TAG, "TryConnectVPN");
            try
            {
                string args = string.Format("{0} {1} {2}", connVpnName, connUserName, connPassWord);
                ProcessStartInfo myProcess = new ProcessStartInfo(VPNPROCESS, args);
                myProcess.CreateNoWindow = true;
                myProcess.UseShellExecute = false;
                Process.Start(myProcess);
            }
            catch (Exception Ex)
            {
                Log.debug(TAG, "TryConnectVPN error, msg : " + Ex.ToString());
                Debug.Assert(false, Ex.ToString());
            }
        }

        /// <summary>
        /// 尝试断开VPN(指定VPN名称)
        /// </summary>
        /// <returns></returns>
        public void TryDisConnectVPN(string disConnVpnName)
        {
            Log.debug(TAG, "TryDisConnectVPN");
            try
            {
                string args = string.Format(@"{0} /d", disConnVpnName);
                ProcessStartInfo myProcess = new ProcessStartInfo(VPNPROCESS, args);
                myProcess.CreateNoWindow = true;
                myProcess.UseShellExecute = false;
                Process.Start(myProcess);
            }
            catch (Exception Ex)
            {
                Log.debug(TAG, "TryDisConnectVPN error msg : " + Ex.ToString());
                Debug.Assert(false, Ex.ToString());
            }
        }

        /// <summary>
        /// 创建或更新一个VPN连接(指定VPN名称，及IP)
        /// </summary>
        public void CreateOrUpdateVPN(string updateVPNname,string updateVPNip)
        {
            Log.debug(TAG, "CreateOrUpdateVPN");
            RasDialer dialer = new RasDialer();
            RasPhoneBook allUsersPhoneBook = new RasPhoneBook();
            allUsersPhoneBook.Open();

            RasEntry entry = null;
            if (allUsersPhoneBook.Entries.Contains(updateVPNname))
            {
                entry = allUsersPhoneBook.Entries[updateVPNname];
                entry.EncryptionType = RasEncryptionType.Optional;
                entry.PhoneNumber = updateVPNip;
                IPAddress _ip;
                IPAddress.TryParse(updateVPNip, out _ip);
                entry.IPAddress = _ip;
                entry.Options.UsePreSharedKey = true;
                entry.UpdateCredentials(RasPreSharedKey.Client, "123456");
                entry.Update();
            }
            else
            {
                entry = RasEntry.CreateVpnEntry(updateVPNname, updateVPNip, RasVpnStrategy.L2tpOnly, RasDevice.GetDeviceByName("(L2TP)", RasDeviceType.Vpn));
                entry.EncryptionType = RasEncryptionType.Optional;
                entry.Options.UsePreSharedKey = true;
                allUsersPhoneBook.Entries.Add(entry);
                entry.UpdateCredentials(RasPreSharedKey.Client, "123456");
            }
        }

        /// <summary>
        /// 删除指定名称的VPN
        /// 如果VPN正在运行，一样会在电话本里删除，但是不会断开连接，所以，最好是先断开连接，再进行删除操作
        /// </summary>
        /// <param name="delVpnName"></param>
        public void TryDeleteVPN(string delVpnName)
        {
            Log.debug(TAG, "TryDeleteVPN");
            RasPhoneBook allUsersPhoneBook = new RasPhoneBook();
            allUsersPhoneBook.Open();
            if (allUsersPhoneBook.Entries.Contains(delVpnName))
            {
                allUsersPhoneBook.Entries.Remove(delVpnName);
            }
        }

        /// <summary>
        /// 获取当前正在连接中的VPN名称
        /// </summary>
        public List<string> GetCurrentConnectingVPNNames()
        {
            Log.debug(TAG, "GetCurrentConnectingVPNNames in");
            List<string> ConnectingVPNList = new List<string>();
            Process proIP = new Process();
            string strResult = null;

            try 
            {
                proIP.StartInfo.FileName = "cmd.exe ";
                proIP.StartInfo.UseShellExecute = false;
                proIP.StartInfo.RedirectStandardInput = true;
                proIP.StartInfo.RedirectStandardOutput = true;
                proIP.StartInfo.RedirectStandardError = true;
                proIP.StartInfo.CreateNoWindow = true;
                proIP.Start();

                proIP.StandardInput.WriteLine(RasDialFileName);
                proIP.StandardInput.WriteLine("exit");

                strResult = proIP.StandardOutput.ReadToEnd();
                proIP.Close();
                Log.debug(TAG, "GetCurrentConnectingVPNNames run process result : \n" + strResult);
            }
            catch(Exception ex)
            {
                Log.debug(TAG, "GetCurrentConnectingVPNNames error msg : " + ex.ToString());
            }

            int begin = strResult.IndexOf("已连接") + 3;
            int end = strResult.IndexOf("命令已完成");
            string getVpnNames = null;
            getVpnNames = strResult.Substring(begin, end - begin);
            Log.debug(TAG, "GetCurrentConnectingVPNNames getVpnNames : \n" + getVpnNames);

            string[] sArray = getVpnNames.Split('\n');
            foreach (string e in sArray)
            {
                Log.debug(TAG, "GetCurrentConnectingVPNNames sArray : " + e);
            }
            for (int index = 0; index < sArray.Length; index++)
            {
                if (sArray[index] != string.Empty)
                    ConnectingVPNList.Add(sArray[index].Replace("\r", ""));
            }

            string tmp = string.Join("\n", ConnectingVPNList.ToArray());
            Log.debug(TAG, "GetCurrentConnectingVPNNames ConnectingVPNList : \n" + tmp);

            return ConnectingVPNList;
        }
    }
}
