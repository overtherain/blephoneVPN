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
    public partial class Setting : Form
    {
        public static Type TAG = typeof(Setting);
        public Setting()
        {
            InitializeComponent();
            this.tb_sql_ip.Text = Properties.Settings.Default.sql_ip;
            this.tb_sql_db.Text = Properties.Settings.Default.sql_db;
            this.tb_sql_username.Text = Properties.Settings.Default.sql_username;
            this.tb_sql_pwd.Text = Properties.Settings.Default.sql_pwd;
            this.tb_sql_table.Text = Properties.Settings.Default.sql_table;
            this.tb_vpn_ip.Text = Main.connectIP;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            Log.debug(TAG, "btn_save_Click");
            saveSqlItems();
            this.Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            Log.debug(TAG, "btn_cancel_Click");
            PublicVar.isSettingOpen = false;
            this.Close();
        }

        private void saveSqlItems()
        {
            Log.debug(TAG, "saveSqlItems sql_ip:" + this.tb_sql_ip.Text + ", sql_db:" + this.tb_sql_db.Text);
            Properties.Settings.Default.sql_ip = this.tb_sql_ip.Text.ToString();
            Properties.Settings.Default.sql_db = this.tb_sql_db.Text.ToString();
            Properties.Settings.Default.sql_username = this.tb_sql_username.Text.ToString();
            Properties.Settings.Default.sql_pwd = this.tb_sql_pwd.Text.ToString();
            Properties.Settings.Default.sql_table = this.tb_sql_table.Text.ToString();
            Main.connectIP = this.tb_vpn_ip.Text.ToString();
            
            Properties.Settings.Default.Save();
        }

        private void Setting_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.debug(TAG, "Setting_FormClosing");
            PublicVar.isSettingOpen = false;
        }
    }
}
