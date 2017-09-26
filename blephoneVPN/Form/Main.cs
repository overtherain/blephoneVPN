﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using DotRas;
using DotRas.Design;
using System.Diagnostics;
using blephoneVPN.Util;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace blephoneVPN
{
    public partial class Main : Form
    {
        private static string connectIP = "";
        private static string connectUserName = "";
        private static string connectPWD = "";
        private readonly string VPNNAME = "blephoneVPN";
        //public static string mysqlConnectStr = @"Server=121.43.183.196;Database=vpn;Uid=vpn;Pwd=vpn@162534";
        public static string mysqlConnectStr = "";
        //private static string sqlcmd = "SELECT * FROM vpn_address";
        private static string sqlcmd = "";
        private static int btn_clicked = 0;

        MySqlConnection conn = null;
        MySqlDataReader dataReader = null;
        MySqlCommand command = null;
        VPNConnectHelper mVPNConnectHelper = null;
        RASHelper mRASHelper = null;
        public static string logname = null;
        public static Type TAG = typeof(Main);
        private static Dictionary<string, User> users = new Dictionary<string, User>();
        DES des = new DES();
        string key = null;
        string emptykey = null;

        public static int intTime = 1000 * 60 * 60;
        private System.Timers.Timer endTime;
        //public static int intTime = 3000;
        bool canClose = Properties.Settings.Default.will_close;

        private System.Timers.Timer searchTime;
        public static int intSearchTime = 500;
        private static List<string> searchVPNName = null;
        private static int connCount = 0;

        public Main()
        {
            InitializeComponent();
            this.SizeChanged += new EventHandler(Form1_SizeChanged);
            this.tb_msg1.Text = "";
            this.tb_msg2.Text = "";
            mVPNConnectHelper = new VPNConnectHelper();
            this.tabControl1.TabPages.Remove(this.tabControl1.TabPages[1]);//移除第二个tabpage
            logname = AppDomain.CurrentDomain.BaseDirectory + "log\\" + DateTime.Now.ToString("yyyyMMdd_HH-mm") + ".log";
            Log.CreateDirectory(logname);
            key = des.pubKey();
            emptykey = des.MD5Encrypt("", key);
            //endTimeToDo();
#if DEBUG
            string ret = des.MD5Decrypt(emptykey, key);
            Console.WriteLine("emptykey:" + emptykey + ", emptyvalue:" + ret + "md5 key : " + key);
#endif
            Log.debug(TAG, "init main end");
        }

        void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Log.debug(TAG, "Form1_SizeChanged windows hide");
                this.Hide();
            }
        }

        private void login_Click(object sender, EventArgs e)
        {
            Button but = (Button)sender;
            try
            { 
                getInputValue(but.Name);
                getConnectIP();
                if (connectIP != "" && connectUserName != "" && connectPWD != "")
                {
                    if (but.Name == "button_login1")
                    {
                        Log.debug(TAG, "button_login1");
#if DEBUG
                        Console.WriteLine("button_login1 clicked");
#endif
                        //this.button_login1.Enabled = false;
                        //this.tb_username1.Enabled = false;
                        //this.tb_pwd1.Enabled = false;
                        btn_clicked = 1;
                    }
                    else if (but.Name == "button_login2")
                    {
                        Log.debug(TAG, "button_login2");
#if DEBUG
                    Console.WriteLine("button_login2 clicked");
#endif
                        //this.button_login2.Enabled = false;
                        //this.tb_username2.Enabled = false;
                        //this.tb_pwd2.Enabled = false;
                        btn_clicked = 2;
                    }
                    mRASHelper = new RASHelper(connectIP, VPNNAME, connectUserName, connectPWD);
                    mVPNConnectHelper.DialAsyncComplete += new VPNConnectHelper.DialAsyncCompleteHandler(VPNConnectHelper_DialAsyncComplete);
                    mVPNConnectHelper.DialStateChange += new VPNConnectHelper.DialStateChangeHandler(VPNConnectHelper_DialStateChange);
                    mVPNConnectHelper.DialAsync(connectIP, connectUserName, connectPWD);
                }
                else
                {
                    Log.debug(TAG, "输入有误!");
                    MessageBox.Show("输入有误!");
                }
                connCount = 1;
            }
            catch(Exception ex)
            {
                MessageBox.Show("拨号失败!mVPNConnectHelper error msg:" + ex.ToString());
                Log.debug(TAG, "login_Click error : " + ex);
                try
                {
                    mRASHelper.CreateOrUpdateVPN();
                    mRASHelper.TryConnectVPN();
                    connCount = 2;
                }
                catch(Exception err)
                {
                    MessageBox.Show("拨号失败!mRASHelper error msg:" + err.ToString());
                }
            }
            
        }

        private void button_exit1_Click(object sender, EventArgs e)
        {
            Log.debug(TAG, "exit1 click");
            exit();
        }

        private void button_exit2_Click(object sender, EventArgs e)
        {
            Log.debug(TAG, "exit2 click");
            exit();
        }

        private void getConnectIP() 
        {
            mysqlConnectStr = "Server=" + Properties.Settings.Default.sql_ip
                           + ";Database=" + Properties.Settings.Default.sql_db
                           + ";Uid=" + Properties.Settings.Default.sql_username
                           + ";Pwd=" + Properties.Settings.Default.sql_pwd;
            sqlcmd = "SELECT * FROM " + Properties.Settings.Default.sql_table;
            Log.debug(TAG, "mysqlConnectStr Server=" + Properties.Settings.Default.sql_ip + ", Database=" + Properties.Settings.Default.sql_db);
#if DEBUG
            Console.WriteLine("mysqlConnectStr : {0}", mysqlConnectStr);
#endif
            try
            {
                conn = new MySqlConnection(mysqlConnectStr);
                conn.Open();
                command = conn.CreateCommand();
                command.CommandText = sqlcmd;
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    string dataReadResult = "VPN login id=" + dataReader.GetInt16(0) + ", ip=" + dataReader.GetString(1) + ", update=" + dataReader.GetString(2);
                    Log.debug(TAG, dataReadResult);
#if DEBUG
                    Console.WriteLine(dataReadResult);
#endif
                    connectIP = dataReader.GetString(1);
                }
            }
            catch (Exception err)
            {
                Log.debug(TAG,err.ToString());
                Log.Error(TAG, "getConnectIP failed!", err);
                MessageBox.Show("数据库无法访问,VPN IP设置失败!error msg:" + err.ToString());
#if DEBUG
                Console.WriteLine(err.ToString());
#endif
                Debug.Assert(false, err.ToString());
            }
            finally
            {
                Log.debug(TAG, "close sql connection");
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void getInputValue(string btn)
        {
            if (btn == "button_login1")
            {
                connectUserName = this.cb_username.Text.ToString();
                connectPWD = this.tb_pwd1.Text.ToString();
                string encPwd = des.MD5Encrypt(connectPWD, key);
                Log.debug(TAG, "getInputValue connectUserName : " + connectUserName + ", connectPWD : " + encPwd);
            }
            else if (btn == "button_login2")
            {
                connectIP = this.tb_ip.Text.ToString();
                connectUserName = this.tb_username2.Text.ToString();
                connectPWD = this.tb_pwd2.Text.ToString();
                string encPwd = des.MD5Encrypt(connectPWD, key);
                Log.debug(TAG, "getInputValue connectIP : " + connectIP + ", connectUserName : " + connectUserName + ", connectPWD : " + encPwd);
            }
#if DEBUG
            Console.WriteLine("InputValue ip={0},  username={1},  password={2}", connectIP, connectUserName, connectPWD);
#endif
        }

        public void exit()
        {
            Console.WriteLine("application exit");
            Log.debug(TAG, "application exit");
            if (endTime != null)
            {
                endTime.Stop();
            }
            if (connCount == 1)
            {
                Console.WriteLine("exit mVPNConnectHelper");
                mVPNConnectHelper.CancelDialAsync();
                mVPNConnectHelper.Disconnect();
            }
            else if (connCount == 2)
            {
                Console.WriteLine("exit mRASHelper");
                mRASHelper.TryDisConnectVPN();
                mRASHelper.TryDeleteVPN();
            }
            connCount = 0;
            this.vpnnotify.Dispose();
            Properties.Settings.Default.will_close = false;
            //System.Environment.Exit(0);
            Application.Exit();
        }

        void VPNConnectHelper_DialAsyncComplete(RasDialer dialer, DialCompletedEventArgs e)
        {
            if (e.Connected)
            {
                Log.debug(TAG, "VPNConnectHelper_DialAsyncComplete===========connect!==========");
#if DEBUG
                Console.WriteLine("===========connect!==========");
#endif
                setUsers();
                this.vpnnotify.ShowBalloonTip(3000, "...", "连接成功", ToolTipIcon.Info);
                endTimeToDo();
                this.Hide();
                //new CreateNetDrive().Show();
                //PublicVar.isDriveOpen = true;
            }
            else if (e.Error != null)
            {
                Log.debug(TAG, "VPNConnectHelper_DialAsyncComplete连接失败 : " + e.Error.Message);
                this.vpnnotify.ShowBalloonTip(3000, "...", "连接失败" + "\r\n" + e.Error.Message, ToolTipIcon.Info);
            }
        }

        void VPNConnectHelper_DialStateChange(RasDialer dialer, StateChangedEventArgs e)
        {
            Log.debug(TAG, "VPNConnectHelper_DialStateChange ErrorCode : " + e.ErrorCode + ", State : " + e.State);
#if DEBUG
            Console.WriteLine("===========VPNConnectHelper_DialStateChange===========");
            Console.WriteLine("State : {0}", e.State);
#endif
            if (e.ErrorCode == 0)
            {
                if (btn_clicked == 1)
                {
                    //this.tb_msg1.Text += "连接状态:" + e.State.ToString() + "\r\n";
                    this.UIThread(delegate { this.tb_msg1.Text += "连接状态:" + e.State.ToString() + "\r\n"; });
                }
                else if (btn_clicked == 2)
                {
                    //this.tb_msg2.Text += "连接状态:" + e.State.ToString() + "\r\n";
                    this.UIThread(delegate { this.tb_msg2.Text += "连接状态:" + e.State.ToString() + "\r\n"; });
                }
            }
            else
            {
                Log.debug(TAG, "VPNConnectHelper_DialStateChange错误信息:" + e.ErrorMessage);
                if (btn_clicked == 1)
                {
                    //this.tb_msg1.Text += "连接状态:" + e.State.ToString() + "\r\n";
                    //this.tb_msg1.Text += "错误代码:" + e.ErrorCode.ToString() + "\r\n";
                    //this.tb_msg1.Text += "错误信息:" + e.ErrorMessage.ToString() + "\r\n";
                    this.UIThread(delegate { this.tb_msg1.Text += "连接状态:" + e.State.ToString() + "\r\n"; });
                    this.UIThread(delegate { this.tb_msg1.Text += "错误代码:" + e.ErrorCode.ToString() + "\r\n"; });
                    this.UIThread(delegate { this.tb_msg1.Text += "错误信息:" + e.ErrorMessage.ToString() + "\r\n"; });
                }
                else if (btn_clicked == 2)
                {
                    //this.tb_msg2.Text += "连接状态:" + e.State.ToString() + "\r\n";
                    //this.tb_msg2.Text += "错误代码:" + e.ErrorCode.ToString() + "\r\n";
                    //this.tb_msg2.Text += "错误信息:" + e.ErrorMessage.ToString() + "\r\n";
                    this.UIThread(delegate { this.tb_msg2.Text += "连接状态:" + e.State.ToString() + "\r\n"; });
                    this.UIThread(delegate { this.tb_msg2.Text += "错误代码:" + e.ErrorCode.ToString() + "\r\n"; });
                    this.UIThread(delegate { this.tb_msg2.Text += "错误信息:" + e.ErrorMessage.ToString() + "\r\n"; });
                }
            }
            if (btn_clicked == 1)
            {
                //this.tb_msg1.Focus();
                //this.tb_msg1.Select(this.tb_msg1.TextLength, 0);
                //this.tb_msg1.ScrollToCaret();
                this.UIThread(delegate { this.tb_msg1.Focus(); });
                this.UIThread(delegate { this.tb_msg1.Select(this.tb_msg1.TextLength, 0); });
                this.UIThread(delegate { this.tb_msg1.ScrollToCaret(); });
            }
            else if (btn_clicked == 2)
            {
                //this.tb_msg2.Focus();
                //this.tb_msg2.Select(this.tb_msg1.TextLength, 0);
                //this.tb_msg2.ScrollToCaret();
                this.UIThread(delegate { this.tb_msg2.Focus(); });
                this.UIThread(delegate { this.tb_msg2.Select(this.tb_msg2.TextLength, 0); });
                this.UIThread(delegate { this.tb_msg2.ScrollToCaret(); });
            }
        }

        private void vpnnotify_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Log.debug(TAG, "vpnnotify_MouseDoubleClick");
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
        }

        void vpnnotify_Click(object sender, System.EventArgs e)
        {
            //this.Visible = true;
            //this.WindowState = FormWindowState.Normal;
        }

        private void ToolStripMenuItem_exit_Click(object sender, EventArgs e)
        {
            Log.debug(TAG, "ToolStripMenuItem_exit_Click");
            exit();
        }

        private void ToolStripMenuItem_show_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            Log.debug(TAG, "ToolStripMenuItem_show_Click");
        }

        private void tsm_setting_Click(object sender, EventArgs e)
        {
            Log.debug(TAG, "tsm_setting_Click");
#if DEBUG
            Console.WriteLine("PublicVar.isSettingOpen : {0}", PublicVar.isSettingOpen);
#endif
            string info = "PublicVar.isSettingOpen : " + PublicVar.isSettingOpen;
            if (!PublicVar.isSettingOpen)
            {
                PublicVar.isSettingOpen = true;
                new Setting().Show();
            }
        }

        private void tsm_drive_Click(object sender, EventArgs e)
        {
            Log.debug(TAG, "tsm_drive_Click");
#if DEBUG
            Console.WriteLine("PublicVar.isDriveOpen : {0}", PublicVar.isDriveOpen);
#endif
            if (!PublicVar.isDriveOpen)
            {
                PublicVar.isDriveOpen = true;
                new CreateNetDrive().Show();
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.debug(TAG, "Main_FormClosing");
            this.vpnnotify.Dispose();
            exit();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Log.debug(TAG, "Main_Load");
            getUsers();
            string encryptUsername = des.MD5Encrypt(this.cb_username.Text, key);
            foreach (User user in users.Values)
            {
                string decryptUsername = des.MD5Decrypt(user.Username, key);
                this.cb_username.Items.Add(decryptUsername);
                Log.debug(TAG, "load username : '" + decryptUsername + "' to cb_username.Items");
            }
            if (this.cb_username.Items.Count > 0)
            {
                this.cb_username.SelectedIndex = this.cb_username.Items.Count - 1;
            }
            for (int i = 0; i < users.Count; i++)
            {
#if DEBUG
                Console.WriteLine("Main_Load cb_username.Text:" + this.cb_username.Text + ", tb_pwd1.Text:" + this.tb_pwd1.Text);
#endif
                if (encryptUsername != emptykey)
                {
                    if (users.ContainsKey(encryptUsername))
                    {
                        string decryptPassword = des.MD5Decrypt(users[encryptUsername].Password, key);
                        this.tb_pwd1.Text = decryptPassword;
                        this.cb_rem.Checked = true;
                        Log.debug(TAG, "load password : '" + users[encryptUsername].Password + "' to tb_pwd1.Text");
#if DEBUG
                        Console.WriteLine("Main_Load cb_username.Text:" + this.cb_username.Text + ", tb_pwd1.Text:" + this.tb_pwd1.Text);
#endif
                    }
                }
            }
        }

        private void cb_username_SelectedValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < users.Count; i++)
            {
                if (this.cb_username.Text != "")
                {
                    string encryptUsername = des.MD5Encrypt(this.cb_username.Text, key);
                    string decryptPassword = des.MD5Decrypt(users[encryptUsername].Password, key);
                    Log.debug(TAG, "select username : " + this.cb_username.Text + ", input password : " + users[encryptUsername].Password);
                    if (users.ContainsKey(encryptUsername) && users[encryptUsername].Password != emptykey)
                    {
                        this.tb_pwd1.Text = decryptPassword;
                        this.cb_rem.Checked = true;
                    }
                    else
                    {
                        this.tb_pwd1.Text = "";
                        this.cb_rem.Checked = false;
                    }
#if DEBUG
                    Console.WriteLine("cb_username_SelectedValueChanged cb_username.Text:" + this.cb_username.Text + ", tb_pwd1.Text:" + this.tb_pwd1.Text);
#endif
                }
            }
        }

        private void getUsers()
        {
            FileStream fs = new FileStream("data.bin", FileMode.OpenOrCreate);
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
#if DEBUG
                Console.WriteLine("getUsers fs.Length:" + fs.Length);
#endif
                Log.debug(TAG, "get all users, filestream size : " + fs.Length);
                if (fs.Length > 0)
                {
                    users = bf.Deserialize(fs) as Dictionary<string, User>;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取保存用户信息失败!error msg:" + ex.ToString());
                Log.debug(TAG, "get all users failed Exception : " + ex);
#if DEBUG
                Console.WriteLine(ex);
#endif
            }
            finally
            {
                fs.Close();
            }
        }

        private void setUsers()
        {
            string username = des.MD5Encrypt(connectUserName, key);
            string password = des.MD5Encrypt(connectPWD, key);
            User user = new User();
            FileStream fs = new FileStream("data.bin", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();

            try
            {
                //users = bf.Deserialize(fs) as Dictionary<string, User>;
                user.Username = username;
                if (this.cb_rem.Checked)
                {
                    user.Password = password;
                }
                else
                {   
                    user.Password = "";
                }
                if (users.ContainsKey(user.Username))
                {
                    users.Remove(user.Username);
                }
                Log.debug(TAG, "add user username : " + user.Username + ", password : " + user.Password);
#if DEBUG
                Console.WriteLine("setUsers user.Username:" + user.Username + ", user.Password:" + user.Password);
#endif
                users.Add(user.Username, user);
                bf.Serialize(fs, users);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存用户信息失败!error msg:" + ex.ToString());
                Log.debug(TAG, "add user failed Exception : " + ex);
#if DEBUG
                Console.WriteLine(ex);
#endif
            }
            finally
            {
                fs.Close();
            }
        }

        public void endTimeToDo()
        {
            endTime = new System.Timers.Timer();
            endTime.Interval = intTime;
            endTime.Elapsed += new System.Timers.ElapsedEventHandler(theout);
            endTime.AutoReset = false;
            endTime.Enabled = true;
            endTime.Start();
        }

        private void theout(object source, System.Timers.ElapsedEventArgs e)
        {
            this.UIThread(delegate { new Warning(this).Show(); });
        }

        private void tsm_warning_Click(object sender, EventArgs e)
        {
            new Warning(this).Show();
        }

        private void searchVPNs()
        {
            searchTime = new System.Timers.Timer();
            searchTime.Interval = intSearchTime;
            searchTime.Elapsed += new System.Timers.ElapsedEventHandler(searchTime_Elapsed);
            searchTime.AutoReset = true;
            searchTime.Enabled = true;
            searchTime.Start();
        }

        private void searchTime_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            searchVPNName = mRASHelper.GetCurrentConnectingVPNNames();
            foreach(string name in searchVPNName)
            {
                Console.WriteLine("search vpn name : " + name);
                if (name == VPNNAME)
                {
                    this.UIThread(delegate
                    {
                        this.vpnnotify.ShowBalloonTip(3000, "...", "连接成功", ToolTipIcon.Info);
                        endTimeToDo();
                        this.Hide();
                    });
                    searchTime.Elapsed -= new System.Timers.ElapsedEventHandler(searchTime_Elapsed);
                    searchVPNName = null;
                }
            }
        }
    }

    //解决跨线程安全
    static class ControlExtensions
    {
        static public void UIThread(this Control control, Action code)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(code);
                return;
            }
            code.Invoke();
        }

        static public void UIThreadInvoke(this Control control, Action code)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(code);
                return;
            }
            code.Invoke();
        }
    }
}