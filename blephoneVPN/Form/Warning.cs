using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using blephoneVPN.Util;

namespace blephoneVPN
{
    public partial class Warning : Form
    {
        static int intTime = 1000;
        static int endSecond = 60;
        private System.Timers.Timer time = new System.Timers.Timer();
        public static Type TAG = typeof(Warning);
        private Main m1;

        public Warning(Main main)
        {
            InitializeComponent();
            this.warning_msg.Text = "60秒后VPN将自动断开并退出!\n如需继续使用请点取消.";
            endTimeDo();
            m1 = main;
            Log.debug(TAG, "Warning init");
        }

        private void bt_msg_ok_Click(object sender, EventArgs e)
        {
            Log.debug(TAG, "bt_msg_ok_Click");
            Properties.Settings.Default.will_close = true;
            Application.Exit();
        }

        private void bt_msg_cancel_Click(object sender, EventArgs e)
        {
            Log.debug(TAG, "bt_msg_cancel_Click");
            Properties.Settings.Default.will_close = false;

            m1.endTimeToDo();
            time.Elapsed -= new System.Timers.ElapsedEventHandler(theout);
            time.Stop();
            endSecond = 60;
            this.Close();
        }

        private void endTimeDo()
        {
            Log.debug(TAG, "endTimeDo");
            time.Interval = intTime;
            time.Elapsed += new System.Timers.ElapsedEventHandler(theout);
            time.AutoReset = true;
            time.Enabled = true;
        }

        private void theout(object source, System.Timers.ElapsedEventArgs e)
        {
            endSecond = endSecond - 1;
            if (endSecond == 0)
            {
                Log.debug(TAG, "timeout exit, time : " + endSecond);
                time.Elapsed -= new System.Timers.ElapsedEventHandler(theout);
                time.Stop();
                Properties.Settings.Default.will_close = true;
                Application.Exit();
            }
            Log.debug(TAG, "theout : " + endSecond);
            //this.bt_msg_ok.Text = "确定(" + endSecond + ")";
            this.UIThread1(delegate { this.bt_msg_ok.Text = "确定(" + endSecond + ")"; });
        }

        private void Warning_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        
    }

    static class ControlExtensions1
    {
        static public void UIThread1(this Control control, Action code)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(code);
                return;
            }
            code.Invoke();
        }
    }
}
