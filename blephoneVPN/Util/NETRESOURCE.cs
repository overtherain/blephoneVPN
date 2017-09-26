using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace blephoneVPN.Util
{
    public enum ERROR_ID
    {
        ERROR_SUCCESS = 0,  // Success 
        ERROR_ACCESS_DENIED = 5,
        ERROR_NOT_ENOUGH_MEMORY = 8,
        ERROR_READ_FAULT = 30,
        Windows_cannot_find_the_network_path = 51,
        ERROR_BAD_NETPATH = 53,
        ERROR_NETWORK_BUSY = 54,
        ERROR_NETWORK_ACCESS_DENIED = 65,
        ERROR_BAD_DEV_TYPE = 66,
        ERROR_BAD_NET_NAME = 67,
        ERROR_REQ_NOT_ACCEP = 71,
        ERROR_ALREADY_ASSIGNED = 85,
        ERROR_INVALID_PASSWORD = 86,
        ERROR_INVALID_PARAMETER = 87,
        ERROR_OPEN_FAILED = 110,
        ERROR_INVALID_LEVEL = 124,
        ERROR_BUSY = 170,
        ERROR_MORE_DATA = 234,
        ERROR_NO_BROWSER_SERVERS_FOUND = 6118,
        ERROR_NO_NETWORK = 1222,
        ERROR_INVALID_HANDLE_STATE = 1609,
        ERROR_EXTENDED_ERROR = 1208,
        ERROR_DEVICE_ALREADY_REMEMBERED = 1202,
        ERROR_NO_NET_OR_BAD_PATH = 1203,
        the_user_has_not_been_granted_the_requested_logon_type_at_this_computer = 1385,
        unknown_user_name_or_bad_password = 1326,
        ERROR_ACCOUNT_RESTRICTION = 1327,
        ERROR_INVALID_WORKSTATION = 1329,
        logon_request_contained_an_invalid_logon_type_value = 1367,
    }

    [StructLayout(LayoutKind.Sequential)]
    public class NETRESOURCE
    {
        public RESOURCE_SCOPE dwScope;//只能取2
        public RESOURCE_TYPE dwType;//0为打印机或驱动器，1为驱动器，2为打印机
        public RESOURCE_DISPLAYTYPE dwDisplayType;//取0，自动设置
        public RESOURCE_USAGE dwUsage;//取1

        [MarshalAs(UnmanagedType.LPStr)]
        public string LocalName;//本地盘符或名称
        [MarshalAs(UnmanagedType.LPStr)]
        public string RemoteName;//远程地址
        [MarshalAs(UnmanagedType.LPStr)]
        public string Comment;//NULL即可，A pointer to a NULL-terminated string that contains a comment supplied by the network provider.
        [MarshalAs(UnmanagedType.LPStr)]
        public string Provider;//NULL即可，A pointer to a NULL-terminated string that contains the name of the provider that owns the resource. This member can be NULL if the provider name is unknown. 
    }

    public enum RESOURCE_SCOPE
    {
        RESOURCE_CONNECTED = 1,
        RESOURCE_GLOBALNET = 2,
        RESOURCE_REMEMBERED = 3,
        RESOURCE_RECENT = 4,
        RESOURCE_CONTEXT = 5
    }

    public enum RESOURCE_TYPE
    {
        RESOURCETYPE_ANY = 0,
        RESOURCETYPE_DISK = 1,
        RESOURCETYPE_PRINT = 2,
        RESOURCETYPE_RESERVED = 8,
    }

    public enum RESOURCE_USAGE
    {
        RESOURCEUSAGE_CONNECTABLE = 1,
        RESOURCEUSAGE_CONTAINER = 2,
        RESOURCEUSAGE_NOLOCALDEVICE = 4,
        RESOURCEUSAGE_SIBLING = 8,
        RESOURCEUSAGE_ATTACHED = 16,
        RESOURCEUSAGE_ALL = (RESOURCEUSAGE_CONNECTABLE | RESOURCEUSAGE_CONTAINER | RESOURCEUSAGE_ATTACHED),
    }

    public enum RESOURCE_DISPLAYTYPE
    {
        RESOURCEDISPLAYTYPE_GENERIC = 0,
        RESOURCEDISPLAYTYPE_DOMAIN = 1,
        RESOURCEDISPLAYTYPE_SERVER = 2,
        RESOURCEDISPLAYTYPE_SHARE = 3,
        RESOURCEDISPLAYTYPE_FILE = 4,
        RESOURCEDISPLAYTYPE_GROUP = 5,
        RESOURCEDISPLAYTYPE_NETWORK = 6,
        RESOURCEDISPLAYTYPE_ROOT = 7,
        RESOURCEDISPLAYTYPE_SHAREADMIN = 8,
        RESOURCEDISPLAYTYPE_DIRECTORY = 9,
        RESOURCEDISPLAYTYPE_TREE = 10,
        RESOURCEDISPLAYTYPE_NDSCONTAINER = 11
    }
}
