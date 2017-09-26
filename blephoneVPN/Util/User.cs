using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace blephoneVPN.Util
{
    [Serializable]
    public class User
    {
        public static Type TAG = typeof(Main);
        private string userName;
        public string Username
        {
            get { return userName; }
            set { userName = value; }
        }

        private string passWord;
        public string Password
        {
            get { return passWord; }
            set { passWord = value; }
        }
    }
}
