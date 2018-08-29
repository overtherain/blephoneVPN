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
        public static string connectIP = "";
        private static string connectUserName = "";
        private static string connectPWD = "";
        private static string VPNNAME = "blephoneVPN";
        private static string sqlcmd = "";
        private static string key = null;
        private static string emptykey = null;
        private static int btn_clicked = 0;
        private static int connCount = 0;
        private static int searchRount = 0;
        private System.Timers.Timer endTime;
        private System.Timers.Timer searchTime;
        private bool canClose = Properties.Settings.Default.will_close;

        private static Dictionary<string, User> users = new Dictionary<string, User>();
        private static List<string> searchVPNName = null;
        private DES des = null;
        private MySqlConnection conn = null;
        private MySqlDataReader dataReader = null;
        private MySqlCommand command = null;
        private VPNConnectHelper mVPNConnectHelper = null;
        private RASHelper mRASHelper = null;

        public static int intSearchTime = 500;
        public static int intTime = 1000 * 60 * 60;
        public static string mysqlConnectStr = "";
        public static string logname = null;
        public static Type TAG = typeof(Main);

        public Main()
        {
            InitializeComponent();
            Log.debug(TAG, "init main");
            getConnectIP();

            this.SizeChanged += new EventHandler(Form1_SizeChanged);
            this.tb_msg1.Text = "";
            this.tb_msg2.Text = "";
            this.tabControl1.TabPages.Remove(this.tabControl1.TabPages[1]);//移除第二个tabpage

            mVPNConnectHelper = new VPNConnectHelper();
            des = new DES();
            key = des.pubKey();

            emptykey = des.MD5Encrypt("", key);
            string ret = des.MD5Decrypt(emptykey, key);
            Log.console("emptykey : " + emptykey + ", emptyvalue : '" + ret + "', md5 key : " + key);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Log.debug(TAG, "Main_Load");
            getUsers();
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
            string encryptUsername = des.MD5Encrypt(this.cb_username.Text, key);
            for (int i = 0; i < users.Count; i++)
            {
                Log.debug(TAG, "Main_Load cb_username.Text : " + this.cb_username.Text + ", encryptUsername : " + encryptUsername);
                if (encryptUsername != emptykey)
                {
                    if (users.ContainsKey(encryptUsername))
                    {
                        string decryptPassword = des.MD5Decrypt(users[encryptUsername].Password, key);
                        this.tb_pwd1.Text = decryptPassword;
                        this.cb_rem.Checked = true;
                        Log.debug(TAG, "Main_Load load password : '" + users[encryptUsername].Password + "' to tb_pwd1.Text, " + "cb_username.Text:" + this.cb_username.Text);
                    }
                }
            }
        }

        void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Log.debug(TAG, "Form1_SizeChanged windows hide");
                this.Hide();
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.debug(TAG, "Main_FormClosing");
            this.vpnnotify.Dispose();
            exit();
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

        private void connectVPNConnectHelper()
        {
            try
            {
                mVPNConnectHelper.DialAsyncComplete += new VPNConnectHelper.DialAsyncCompleteHandler(VPNConnectHelper_DialAsyncComplete);
                mVPNConnectHelper.DialStateChange += new VPNConnectHelper.DialStateChangeHandler(VPNConnectHelper_DialStateChange);
                mVPNConnectHelper.DialAsync(connectIP, connectUserName, connectPWD);
            }
            catch(Exception ex)
            {
                this.button_login1.Enabled = true;
                MessageBox.Show("mVPNConnectHelper 拨号失败!将尝试rasdial拨号.\n error msg:" + ex.ToString());
            }
            Log.debug(TAG, "connectVPNConnectHelper connCount = " + connCount);
        }

        private void connectRASHelper()
        {
            try
            {
                mRASHelper = new RASHelper(connectIP, VPNNAME, connectUserName, connectPWD);
                mRASHelper.CreateOrUpdateVPN();
                mRASHelper.TryConnectVPN();
                searchVPNs();
            }
            catch(Exception ex)
            {
                this.button_login1.Enabled = true;
                MessageBox.Show("mRASHelper拨号失败!VPN无法连接!\n error msg:" + ex.ToString());
                Log.debug(TAG, "login_Click mRASHelper error : " + ex.ToString());
            }
            Log.debug(TAG, "connectRASHelper connCount = " + connCount);
        }

        private void login_Click(object sender, EventArgs e)
        {
            Button but = (Button)sender;
            getInputValue(but.Name);
            //getConnectIP();

            if (connectIP != "" && connectUserName != "" && connectPWD != "")
            {
                connCount = 0;
                if (but.Name == "button_login1")
                {
                    Log.debug(TAG, "button_login1 clicked");
                    this.button_login1.Enabled = false;
                    btn_clicked = 1;
                }
                else if (but.Name == "button_login2")
                {
                    Log.debug(TAG, "button_login2 clicked");
                    this.button_login2.Enabled = false;
                    btn_clicked = 2;
                }
                connectVPNConnectHelper();
                Log.debug(TAG, "login_Click end connCount = " + connCount);
            }
            else
            {
                Log.debug(TAG, "输入有误!");
                MessageBox.Show("输入有误!");
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

        void VPNConnectHelper_DialAsyncComplete(RasDialer dialer, DialCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Log.debug(TAG, "Cancelled!");
                this.UIThread(delegate { this.tb_msg1.Text += "连接状态:Cancelled!\r\n"; });
            }
            else if (e.TimedOut)
            {
                Log.debug(TAG, "Connection attempt timed out!");
                this.UIThread(delegate { this.tb_msg1.Text += "连接状态:Connection attempt timed out!\r\n"; });
            }
            else if (e.Error != null)
            {
                Log.debug(TAG, "VPNConnectHelper_DialAsyncComplete连接失败 : " + e.Error.Message);
                this.UIThread(delegate { this.button_login1.Enabled = true; });
                this.vpnnotify.ShowBalloonTip(1000, "...", "连接失败" + "\r\n" + e.Error.Message, ToolTipIcon.Info);
            }
            else if (e.Connected)
            {
                connCount = 1;

                Log.debug(TAG, "VPNConnectHelper_DialAsyncComplete===========connect!==========");
                this.UIThread(delegate { this.tb_msg1.Text += "连接状态:Connection successful!\r\n"; });
                this.UIThread(delegate { this.button_login1.Enabled = false; });
                this.vpnnotify.ShowBalloonTip(1000, "...", "连接成功", ToolTipIcon.Info);

                setUsers();
                endTimeToDo();
                this.Hide();
                //new CreateNetDrive().Show();
                //PublicVar.isDriveOpen = true;
            }

            if (btn_clicked == 1)
            {
                this.UIThread(delegate { this.tb_msg1.Focus(); });
                this.UIThread(delegate { this.tb_msg1.Select(this.tb_msg1.TextLength, 0); });
                this.UIThread(delegate { this.tb_msg1.ScrollToCaret(); });
            }
            else if (btn_clicked == 2)
            {
                this.UIThread(delegate { this.tb_msg2.Focus(); });
                this.UIThread(delegate { this.tb_msg2.Select(this.tb_msg2.TextLength, 0); });
                this.UIThread(delegate { this.tb_msg2.ScrollToCaret(); });
            }

            if (!e.Connected)
            {
                connectRASHelper();
            }
            Log.debug(TAG, "VPNConnectHelper_DialAsyncComplete connCount = " + connCount);
        }

        void VPNConnectHelper_DialStateChange(RasDialer dialer, StateChangedEventArgs e)
        {
            Log.debug(TAG, "VPNConnectHelper_DialStateChange ErrorCode : " + e.ErrorCode + ", State : " + e.State);
            if (e.ErrorCode == 0)
            {
                if (btn_clicked == 1)
                {
                    this.UIThread(delegate { this.tb_msg1.Text += "连接状态:" + e.State.ToString() + "\r\n"; });
                }
                else if (btn_clicked == 2)
                {
                    this.UIThread(delegate { this.tb_msg2.Text += "连接状态:" + e.State.ToString() + "\r\n"; });
                }
            }
            else
            {
                this.UIThread(delegate { this.button_login1.Enabled = true; });
                Log.debug(TAG, "VPNConnectHelper_DialStateChange错误信息:" + e.ErrorMessage);
                if (btn_clicked == 1)
                {
                    this.UIThread(delegate { this.tb_msg1.Text += "连接状态:" + e.State.ToString() + "\r\n"; });
                    this.UIThread(delegate { this.tb_msg1.Text += "错误代码:" + e.ErrorCode.ToString() + "\r\n"; });
                    this.UIThread(delegate { this.tb_msg1.Text += "错误信息:" + e.ErrorMessage.ToString() + "\r\n"; });
                }
                else if (btn_clicked == 2)
                {
                    this.UIThread(delegate { this.tb_msg2.Text += "连接状态:" + e.State.ToString() + "\r\n"; });
                    this.UIThread(delegate { this.tb_msg2.Text += "错误代码:" + e.ErrorCode.ToString() + "\r\n"; });
                    this.UIThread(delegate { this.tb_msg2.Text += "错误信息:" + e.ErrorMessage.ToString() + "\r\n"; });
                }
            }

            if (btn_clicked == 1)
            {
                this.UIThread(delegate { this.tb_msg1.Focus(); });
                this.UIThread(delegate { this.tb_msg1.Select(this.tb_msg1.TextLength, 0); });
                this.UIThread(delegate { this.tb_msg1.ScrollToCaret(); });
            }
            else if (btn_clicked == 2)
            {
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
                    Log.debug(TAG, "cb_username_SelectedValueChanged cb_username.Text:" + this.cb_username.Text + ", tb_pwd1.Text:" + users[encryptUsername].Password);
                }
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
            endTime.Elapsed -= new System.Timers.ElapsedEventHandler(theout);
            endTime.Stop();
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
                        connCount = 2;
                        Log.debug(TAG, "连接成功");
                        this.vpnnotify.ShowBalloonTip(1000, "...", "连接成功", ToolTipIcon.Info);
                        this.button_login1.Enabled = false;
                        this.Hide();

                        endTimeToDo();
                        searchTime.Elapsed -= new System.Timers.ElapsedEventHandler(searchTime_Elapsed);
                        searchTime.Stop();
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
                        connCount = 0;
                        Log.debug(TAG, "连接失败");
                        this.vpnnotify.ShowBalloonTip(1000, "...", "连接失败", ToolTipIcon.Info);
                        this.button_login1.Enabled = true;

                        searchTime.Elapsed -= new System.Timers.ElapsedEventHandler(searchTime_Elapsed);
                        searchTime.Stop();
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
