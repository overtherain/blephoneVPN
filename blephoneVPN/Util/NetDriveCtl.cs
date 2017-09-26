using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;

namespace blephoneVPN.Util
{
    public class NetDriveCtl
    {
        ArrayList NDList;

        public NetDriveCtl()
        {
            NDList = new ArrayList();
        }

        public int CreateDrive(string LocalName, string RemoteName, string UserName, string Password)
        {
            NETRESOURCE NetDrive = new NETRESOURCE();
            NetDrive.dwScope = RESOURCE_SCOPE.RESOURCE_GLOBALNET;//2
            NetDrive.dwType = RESOURCE_TYPE.RESOURCETYPE_DISK;//1 disk
            NetDrive.dwDisplayType = RESOURCE_DISPLAYTYPE.RESOURCEDISPLAYTYPE_SHARE;//3
            NetDrive.dwUsage = RESOURCE_USAGE.RESOURCEUSAGE_CONNECTABLE;//1
            NetDrive.LocalName = LocalName;
            NetDrive.RemoteName = RemoteName;

            NDList.Add(NetDrive);
            return ConnectDrive(NetDrive, UserName, Password);
        }

        public Boolean DeleteDrive(string LocalName, string RemoteName)
        {
            foreach (NETRESOURCE NetDrive in NDList)
            {
                if ((NetDrive.LocalName == LocalName) && (NetDrive.RemoteName == RemoteName))
                {
                    DisconnectDrive(NetDrive);
                    NDList.Remove(NetDrive);
                    return true;
                }
            }
            return false;
        }

        private int ConnectDrive(NETRESOURCE NetDrive, string UserName, string Password)
        {
            DisconnectDrive(NetDrive);
            int ret = WNetAddConnection2(NetDrive, Password, UserName, 0);

            return ret;
        }

        private string DisconnectDrive(NETRESOURCE NetDrive)
        {
            string LocalName = NetDrive.LocalName;
            return WNetCancelConnection2(LocalName, 1, true).ToString();
        }

        public string DisconnectDrive(string LocalName)
        {
            return WNetCancelConnection2(LocalName, 1, true).ToString();
        }

        //这两个是系统API函数
        [DllImport("mpr.dll", EntryPoint = "WNetAddConnection2")]
        private static extern int WNetAddConnection2([In] NETRESOURCE lpNetResource, string lpPassword, string lpUsername, int dwFlags);
        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2(string lpName, int dwFlags, bool fForce);
    }
}
