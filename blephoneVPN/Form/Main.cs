using System;
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
        private static int searchRount = 0;

        public Main()
        {
            InitializeComponent();
            this.SizeChanged += new EventHandler(Form1_SizeChanged);
            this.tb_msg1.Text = "";
            this.tb_msg2.Text = "";
            mVPNConnectHelper = new VPNConnectHelper();
            this.tabControl1.TabPages.Remove(this.tabControl1.TabPages[1]);//移除第二个tabpage
            //logname = AppDomain.CurrentDomain.BaseDirectory + "log\\" + DateTime.Now.ToString("yyyyMMdd_HH-mm") + ".log";
            //Log.CreateDirectory(logname);
            key = des.pubKey();
            emptykey = des.MD5Encrypt("", key);
            //endTimeToDo();
            string ret = des.MD5Decrypt(emptykey, key);
            Log.console("emptykey:" + emptykey + ", emptyvalue:" + ret + "md5 key : " + key);
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
            getInputValue(but.Name);
            getConnectIP();
            try
            {
                if (connectIP != "" && connectUserName != "" && connectPWD != "")
                {
                    if (but.Name == "button_login1")
                    {
                        Log.debug(TAG, "button_login1 clicked");
                        this.button_login1.Enabled = false;
                        //this.tb_username1.Enabled = false;
                        //this.tb_pwd1.Enabled = false;
                        btn_clicked = 1;
                    }
                    else if (but.Name == "button_login2")
                    {
                        Log.debug(TAG, "button_login2 clicked");
                        //this.button_login2.Enabled = false;
                        //this.tb_username2.Enabled = false;
                        //this.tb_pwd2.Enabled = false;
                        btn_clicked = 2;
                    }
                    //mRASHelper = new RASHelper(connectIP, VPNNAME, connectUserName, connectPWD);
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
            catch (Exception ex)
            {
                this.button_login1.Enabled = true;
                MessageBox.Show("mVPNConnectHelper 拨号失败!将尝试rasdial拨号.\n error msg:" + ex.ToString());
                Log.debug(TAG, "login_Click mVPNConnectHelper error : " + ex.ToString());
                try
                {
                    mRASHelper = new RASHelper(connectIP, VPNNAME, connectUserName, connectPWD);
                    mRASHelper.CreateOrUpdateVPN();
                    mRASHelper.TryConnectVPN();
                    searchVPNs();
                    connCount = 2;
                }
                catch (Exception err)
                {
                    this.button_login1.Enabled = true;
                    MessageBox.Show("mRASHelper拨号失败!VPN无法连接!\n error msg:" + err.ToString());
                    Log.debug(TAG, "login_Click mRASHelper error : " + err.ToString());
                }
            }
            /*try
            {
                if (connectIP != "" && connectUserName != "" && connectPWD != "")
                {
                    if (but.Name == "button_login1")
                    {
                        Log.debug(TAG, "button_login1 clicked");
                        btn_clicked = 1;
                    }
                    else if (but.Name == "button_login2")
                    {
                        Log.debug(TAG, "button_login2 clicked");
                        btn_clicked = 2;
                    }
                    mRASHelper = new RASHelper(connectIP, VPNNAME, connectUserName, connectPWD);
                    mRASHelper.CreateOrUpdateVPN();
                    mRASHelper.TryConnectVPN();
                    searchVPNs();
                    connCount = 2;
                }
                else
                {
                    Log.debug(TAG, "输入有误!");
                    MessageBox.Show("输入有误!");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("拨号失败!mRASHelper error msg:" + err.ToString());
                Log.debug(TAG, "login_Click mRASHelper error : " + err);
            }*/
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
                    connectIP = dataReader.GetString(1);
                }
            }
            catch (Exception err)
            {
                Log.debug(TAG, "getConnectIP error : " + err.ToString());
                Log.Error(TAG, "getConnectIP failed!", err);
                MessageBox.Show("数据库无法访问,VPN IP设置失败!error msg:" + err.ToString());
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
            Log.console("InputValue ip = " + connectIP + ", username = " + connectUserName + ", password = " + connectPWD);
        }

        public void exit()
        {
            Log.debug(TAG, "application exit");
            if (endTime != null)
            {
                endTime.Stop();
            }
            if (connCount == 1)
            {
                Log.debug(TAG, "exit mVPNConnectHelper");
                mVPNConnectHelper.CancelDialAsync();
                mVPNConnectHelper.Disconnect();
            }
            else if (connCount == 2)
            {
                Log.debug(TAG, "exit mRASHelper");
                mRASHelper.TryDisConnectVPN();
                Thread.Sleep(2000);
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
                setUsers();
                this.vpnnotify.ShowBalloonTip(3000, "...", "连接成功", ToolTipIcon.Info);
                endTimeToDo();
                this.UIThread(delegate { this.button_login1.Enabled = false; });
                this.Hide();
                //new CreateNetDrive().Show();
                //PublicVar.isDriveOpen = true;
            }
            else if (e.Error != null)
            {
                this.UIThread(delegate { this.button_login1.Enabled = true; });
                Log.debug(TAG, "VPNConnectHelper_DialAsyncComplete连接失败 : " + e.Error.Message);
                this.vpnnotify.ShowBalloonTip(3000, "...", "连接失败" + "\r\n" + e.Error.Message, ToolTipIcon.Info);
            }
        }

        void VPNConnectHelper_DialStateChange(RasDialer dialer, StateChangedEventArgs e)
        {
            Log.debug(TAG, "VPNConnectHelper_DialStateChange ErrorCode : " + e.ErrorCode + ", State : " + e.State);
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

                this.UIThread(delegate { this.button_login1.Enabled = true; });
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
            Log.debug(TAG, "tsm_setting_Click PublicVar.isSettingOpen : " + PublicVar.isSettingOpen);
            string info = "PublicVar.isSettingOpen : " + PublicVar.isSettingOpen;
            if (!PublicVar.isSettingOpen)
            {
                PublicVar.isSettingOpen = true;
                new Setting().Show();
            }
        }

        private void tsm_drive_Click(object sender, EventArgs e)
        {
            Log.debug(TAG, "tsm_drive_Click PublicVar.isDriveOpen : " + PublicVar.isDriveOpen);
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
                Log.debug(TAG, "Main_Load cb_username.Text:" + this.cb_username.Text + ", tb_pwd1.Text:" + this.tb_pwd1.Text);
                if (encryptUsername != emptykey)
                {
                    if (users.ContainsKey(encryptUsername))
                    {
                        string decryptPassword = des.MD5Decrypt(users[encryptUsername].Password, key);
                        this.tb_pwd1.Text = decryptPassword;
                        this.cb_rem.Checked = true;
                        Log.debug(TAG, "Main_Load load password : '" + users[encryptUsername].Password + "' to tb_pwd1.Text\n" 
                            + "cb_username.Text:" + this.cb_username.Text + ", tb_pwd1.Text:" + this.tb_pwd1.Text);
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
                    Log.debug(TAG, "cb_username_SelectedValueChanged cb_username.Text:" + this.cb_username.Text + ", tb_pwd1.Text:" + this.tb_pwd1.Text);
                }
            }
        }

        private void getUsers()
        {
            FileStream fs = new FileStream("data.bin", FileMode.OpenOrCreate);
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                Log.debug(TAG, "get all users, filestream size : " + fs.Length);
                if (fs.Length > 0)
                {
                    users = bf.Deserialize(fs) as Dictionary<string, User>;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取保存用户信息失败!error msg:" + ex.ToString());
                Log.debug(TAG, "get all users failed Exception : " + ex.ToString());
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
                users.Add(user.Username, user);
                bf.Serialize(fs, users);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存用户信息失败!error msg:" + ex.ToString());
                Log.debug(TAG, "add user failed Exception : " + ex.ToString());
            }
            finally
            {
                fs.Close();
            }
        }

        public void endTimeToDo()
        {
            Log.debug(TAG, "endTimeToDo run");
            endTime = new System.Timers.Timer();
            endTime.Interval = intTime;
            endTime.Elapsed += new System.Timers.ElapsedEventHandler(theout);
            endTime.AutoReset = false;
            endTime.Enabled = true;
            endTime.Start();
        }

        private void theout(object source, System.Timers.ElapsedEventArgs e)
        {
            Log.debug(TAG, "theout run");
            this.UIThread(delegate { new Warning(this).Show(); });
        }

        private void tsm_warning_Click(object sender, EventArgs e)
        {
            new Warning(this).Show();
        }

        private void searchVPNs()
        {
            Log.debug(TAG, "searchVPNs run");
            searchTime = new System.Timers.Timer();
            searchTime.Interval = intSearchTime;
            searchTime.Elapsed += new System.Timers.ElapsedEventHandler(searchTime_Elapsed);
            searchTime.AutoReset = true;
            searchTime.Enabled = true;
            searchTime.Start();
        }

        private void searchTime_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            searchVPNName = new List<string>();
            searchVPNName = mRASHelper.GetCurrentConnectingVPNNames();
            string tmp = string.Join("\n", searchVPNName.ToArray());
            searchRount += 1;
            Log.debug(TAG, "searchTime_Elapsed run times : " + searchRount + ", searchVPNName : " + tmp);
            this.UIThread(delegate
            {
                if (searchRount <= 10)
                {
                    if (searchVPNName.Contains(VPNNAME))
                    {
                        this.vpnnotify.ShowBalloonTip(3000, "...", "连接成功", ToolTipIcon.Info);
                        Log.debug(TAG, "连接成功");
                        endTimeToDo();
                        this.button_login1.Enabled = false;
                        this.Hide();
                        searchTime.Elapsed -= new System.Timers.ElapsedEventHandler(searchTime_Elapsed);
                        searchVPNName = null;
                    }
                    else
                    {
                        Log.debug(TAG, "searchTime_Elapsed searchVPNName not Contains VPNNAME");
                    }
                }
                else if (searchRount > 10)
                {
                    if (!searchVPNName.Contains(VPNNAME))
                    {
                        this.vpnnotify.ShowBalloonTip(3000, "...", "连接失败", ToolTipIcon.Info);
                        Log.debug(TAG, "连接失败");
                        this.button_login1.Enabled = true;
                        searchTime.Elapsed -= new System.Timers.ElapsedEventHandler(searchTime_Elapsed);
                        searchVPNName = null;
                    }
                }
            });
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
