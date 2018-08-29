using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using blephoneVPN.Util;

namespace blephoneVPN
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Log.logname = AppDomain.CurrentDomain.BaseDirectory + "log\\" + DateTime.Now.ToString("yyyyMMdd_HH-mm") + ".log";
            Log.CreateDirectory();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
