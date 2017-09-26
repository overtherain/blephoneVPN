namespace blephoneVPN
{
    partial class Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.defaut = new System.Windows.Forms.TabPage();
            this.cb_username = new System.Windows.Forms.ComboBox();
            this.cb_rem = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tb_msg1 = new System.Windows.Forms.TextBox();
            this.button_exit1 = new System.Windows.Forms.Button();
            this.button_login1 = new System.Windows.Forms.Button();
            this.tb_pwd1 = new System.Windows.Forms.TextBox();
            this.tb_username1 = new System.Windows.Forms.TextBox();
            this.label_pwd1 = new System.Windows.Forms.Label();
            this.label_username1 = new System.Windows.Forms.Label();
            this.input = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tb_msg2 = new System.Windows.Forms.TextBox();
            this.tb_pwd2 = new System.Windows.Forms.TextBox();
            this.label_ip = new System.Windows.Forms.Label();
            this.button_exit2 = new System.Windows.Forms.Button();
            this.button_login2 = new System.Windows.Forms.Button();
            this.tb_username2 = new System.Windows.Forms.TextBox();
            this.tb_ip = new System.Windows.Forms.TextBox();
            this.label_pwd2 = new System.Windows.Forms.Label();
            this.label_username2 = new System.Windows.Forms.Label();
            this.vpnnotify = new System.Windows.Forms.NotifyIcon(this.components);
            this.cms1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsm_show = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm_exit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsm_setting = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm_drive = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm_warning = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.defaut.SuspendLayout();
            this.panel1.SuspendLayout();
            this.input.SuspendLayout();
            this.panel2.SuspendLayout();
            this.cms1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.defaut);
            this.tabControl1.Controls.Add(this.input);
            this.tabControl1.Location = new System.Drawing.Point(0, 28);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(256, 293);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 0;
            // 
            // defaut
            // 
            this.defaut.Controls.Add(this.cb_username);
            this.defaut.Controls.Add(this.cb_rem);
            this.defaut.Controls.Add(this.panel1);
            this.defaut.Controls.Add(this.button_exit1);
            this.defaut.Controls.Add(this.button_login1);
            this.defaut.Controls.Add(this.tb_pwd1);
            this.defaut.Controls.Add(this.tb_username1);
            this.defaut.Controls.Add(this.label_pwd1);
            this.defaut.Controls.Add(this.label_username1);
            this.defaut.Location = new System.Drawing.Point(4, 22);
            this.defaut.Name = "defaut";
            this.defaut.Padding = new System.Windows.Forms.Padding(3);
            this.defaut.Size = new System.Drawing.Size(248, 267);
            this.defaut.TabIndex = 0;
            this.defaut.Text = "默认IP";
            this.defaut.UseVisualStyleBackColor = true;
            // 
            // cb_username
            // 
            this.cb_username.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.cb_username.FormattingEnabled = true;
            this.cb_username.Location = new System.Drawing.Point(97, 37);
            this.cb_username.Name = "cb_username";
            this.cb_username.Size = new System.Drawing.Size(135, 23);
            this.cb_username.TabIndex = 2;
            this.cb_username.SelectedValueChanged += new System.EventHandler(this.cb_username_SelectedValueChanged);
            // 
            // cb_rem
            // 
            this.cb_rem.AutoSize = true;
            this.cb_rem.Location = new System.Drawing.Point(97, 116);
            this.cb_rem.Name = "cb_rem";
            this.cb_rem.Size = new System.Drawing.Size(72, 16);
            this.cb_rem.TabIndex = 7;
            this.cb_rem.Text = "记住密码";
            this.cb_rem.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.tb_msg1);
            this.panel1.Location = new System.Drawing.Point(7, 186);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(234, 79);
            this.panel1.TabIndex = 6;
            // 
            // tb_msg1
            // 
            this.tb_msg1.Location = new System.Drawing.Point(4, 4);
            this.tb_msg1.Multiline = true;
            this.tb_msg1.Name = "tb_msg1";
            this.tb_msg1.ReadOnly = true;
            this.tb_msg1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_msg1.Size = new System.Drawing.Size(227, 70);
            this.tb_msg1.TabIndex = 0;
            // 
            // button_exit1
            // 
            this.button_exit1.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.button_exit1.Location = new System.Drawing.Point(145, 138);
            this.button_exit1.Name = "button_exit1";
            this.button_exit1.Size = new System.Drawing.Size(75, 32);
            this.button_exit1.TabIndex = 5;
            this.button_exit1.Text = "退出";
            this.button_exit1.UseVisualStyleBackColor = true;
            this.button_exit1.Click += new System.EventHandler(this.button_exit1_Click);
            // 
            // button_login1
            // 
            this.button_login1.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.button_login1.Location = new System.Drawing.Point(35, 138);
            this.button_login1.Name = "button_login1";
            this.button_login1.Size = new System.Drawing.Size(75, 32);
            this.button_login1.TabIndex = 4;
            this.button_login1.Text = "登陆";
            this.button_login1.UseVisualStyleBackColor = true;
            this.button_login1.Click += new System.EventHandler(this.login_Click);
            // 
            // tb_pwd1
            // 
            this.tb_pwd1.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.tb_pwd1.Location = new System.Drawing.Point(97, 83);
            this.tb_pwd1.Name = "tb_pwd1";
            this.tb_pwd1.PasswordChar = '*';
            this.tb_pwd1.Size = new System.Drawing.Size(135, 24);
            this.tb_pwd1.TabIndex = 3;
            this.tb_pwd1.UseSystemPasswordChar = true;
            // 
            // tb_username1
            // 
            this.tb_username1.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.tb_username1.Location = new System.Drawing.Point(97, 6);
            this.tb_username1.Name = "tb_username1";
            this.tb_username1.Size = new System.Drawing.Size(135, 24);
            this.tb_username1.TabIndex = 2;
            this.tb_username1.Visible = false;
            // 
            // label_pwd1
            // 
            this.label_pwd1.AutoSize = true;
            this.label_pwd1.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.label_pwd1.Location = new System.Drawing.Point(32, 86);
            this.label_pwd1.Name = "label_pwd1";
            this.label_pwd1.Size = new System.Drawing.Size(59, 15);
            this.label_pwd1.TabIndex = 1;
            this.label_pwd1.Text = "密码 :";
            // 
            // label_username1
            // 
            this.label_username1.AutoSize = true;
            this.label_username1.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.label_username1.Location = new System.Drawing.Point(15, 40);
            this.label_username1.Name = "label_username1";
            this.label_username1.Size = new System.Drawing.Size(76, 15);
            this.label_username1.TabIndex = 0;
            this.label_username1.Text = "用户名 :";
            // 
            // input
            // 
            this.input.Controls.Add(this.panel2);
            this.input.Controls.Add(this.tb_pwd2);
            this.input.Controls.Add(this.label_ip);
            this.input.Controls.Add(this.button_exit2);
            this.input.Controls.Add(this.button_login2);
            this.input.Controls.Add(this.tb_username2);
            this.input.Controls.Add(this.tb_ip);
            this.input.Controls.Add(this.label_pwd2);
            this.input.Controls.Add(this.label_username2);
            this.input.Location = new System.Drawing.Point(4, 22);
            this.input.Name = "input";
            this.input.Padding = new System.Windows.Forms.Padding(3);
            this.input.Size = new System.Drawing.Size(248, 267);
            this.input.TabIndex = 1;
            this.input.Text = "手动IP";
            this.input.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.tb_msg2);
            this.panel2.Location = new System.Drawing.Point(7, 193);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(234, 72);
            this.panel2.TabIndex = 14;
            // 
            // tb_msg2
            // 
            this.tb_msg2.Location = new System.Drawing.Point(4, 4);
            this.tb_msg2.Multiline = true;
            this.tb_msg2.Name = "tb_msg2";
            this.tb_msg2.ReadOnly = true;
            this.tb_msg2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_msg2.Size = new System.Drawing.Size(227, 63);
            this.tb_msg2.TabIndex = 0;
            // 
            // tb_pwd2
            // 
            this.tb_pwd2.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.tb_pwd2.Location = new System.Drawing.Point(91, 115);
            this.tb_pwd2.Name = "tb_pwd2";
            this.tb_pwd2.PasswordChar = '*';
            this.tb_pwd2.Size = new System.Drawing.Size(135, 24);
            this.tb_pwd2.TabIndex = 10;
            // 
            // label_ip
            // 
            this.label_ip.AutoSize = true;
            this.label_ip.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.label_ip.Location = new System.Drawing.Point(42, 28);
            this.label_ip.Name = "label_ip";
            this.label_ip.Size = new System.Drawing.Size(43, 15);
            this.label_ip.TabIndex = 13;
            this.label_ip.Text = "IP :";
            // 
            // button_exit2
            // 
            this.button_exit2.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.button_exit2.Location = new System.Drawing.Point(143, 155);
            this.button_exit2.Name = "button_exit2";
            this.button_exit2.Size = new System.Drawing.Size(75, 32);
            this.button_exit2.TabIndex = 12;
            this.button_exit2.Text = "退出";
            this.button_exit2.UseVisualStyleBackColor = true;
            this.button_exit2.Click += new System.EventHandler(this.button_exit2_Click);
            // 
            // button_login2
            // 
            this.button_login2.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.button_login2.Location = new System.Drawing.Point(32, 155);
            this.button_login2.Name = "button_login2";
            this.button_login2.Size = new System.Drawing.Size(75, 32);
            this.button_login2.TabIndex = 11;
            this.button_login2.Text = "登陆";
            this.button_login2.UseVisualStyleBackColor = true;
            this.button_login2.Click += new System.EventHandler(this.login_Click);
            // 
            // tb_username2
            // 
            this.tb_username2.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.tb_username2.Location = new System.Drawing.Point(91, 70);
            this.tb_username2.Name = "tb_username2";
            this.tb_username2.Size = new System.Drawing.Size(135, 24);
            this.tb_username2.TabIndex = 9;
            // 
            // tb_ip
            // 
            this.tb_ip.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.tb_ip.Location = new System.Drawing.Point(91, 25);
            this.tb_ip.Name = "tb_ip";
            this.tb_ip.Size = new System.Drawing.Size(135, 24);
            this.tb_ip.TabIndex = 8;
            // 
            // label_pwd2
            // 
            this.label_pwd2.AutoSize = true;
            this.label_pwd2.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.label_pwd2.Location = new System.Drawing.Point(26, 118);
            this.label_pwd2.Name = "label_pwd2";
            this.label_pwd2.Size = new System.Drawing.Size(59, 15);
            this.label_pwd2.TabIndex = 7;
            this.label_pwd2.Text = "密码 :";
            // 
            // label_username2
            // 
            this.label_username2.AutoSize = true;
            this.label_username2.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.label_username2.Location = new System.Drawing.Point(9, 73);
            this.label_username2.Name = "label_username2";
            this.label_username2.Size = new System.Drawing.Size(76, 15);
            this.label_username2.TabIndex = 6;
            this.label_username2.Text = "用户名 :";
            // 
            // vpnnotify
            // 
            this.vpnnotify.ContextMenuStrip = this.cms1;
            this.vpnnotify.Icon = ((System.Drawing.Icon)(resources.GetObject("vpnnotify.Icon")));
            this.vpnnotify.Text = "vpn notify";
            this.vpnnotify.Visible = true;
            this.vpnnotify.Click += new System.EventHandler(this.vpnnotify_Click);
            this.vpnnotify.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.vpnnotify_MouseDoubleClick);
            // 
            // cms1
            // 
            this.cms1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsm_show,
            this.tsm_exit});
            this.cms1.Name = "cms1";
            this.cms1.Size = new System.Drawing.Size(101, 48);
            // 
            // tsm_show
            // 
            this.tsm_show.Name = "tsm_show";
            this.tsm_show.Size = new System.Drawing.Size(100, 22);
            this.tsm_show.Text = "显示";
            this.tsm_show.Click += new System.EventHandler(this.ToolStripMenuItem_show_Click);
            // 
            // tsm_exit
            // 
            this.tsm_exit.Name = "tsm_exit";
            this.tsm_exit.Size = new System.Drawing.Size(100, 22);
            this.tsm_exit.Text = "退出";
            this.tsm_exit.Click += new System.EventHandler(this.ToolStripMenuItem_exit_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsm_setting,
            this.tsm_drive,
            this.tsm_warning});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(255, 25);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsm_setting
            // 
            this.tsm_setting.Name = "tsm_setting";
            this.tsm_setting.Size = new System.Drawing.Size(44, 21);
            this.tsm_setting.Text = "设置";
            this.tsm_setting.Click += new System.EventHandler(this.tsm_setting_Click);
            // 
            // tsm_drive
            // 
            this.tsm_drive.Enabled = false;
            this.tsm_drive.Name = "tsm_drive";
            this.tsm_drive.Size = new System.Drawing.Size(44, 21);
            this.tsm_drive.Text = "映射";
            this.tsm_drive.Click += new System.EventHandler(this.tsm_drive_Click);
            // 
            // tsm_warning
            // 
            this.tsm_warning.Name = "tsm_warning";
            this.tsm_warning.Size = new System.Drawing.Size(66, 21);
            this.tsm_warning.Text = "warning";
            this.tsm_warning.Visible = false;
            this.tsm_warning.Click += new System.EventHandler(this.tsm_warning_Click);
            // 
            // Main
            // 
            this.AcceptButton = this.button_login1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(255, 322);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Main";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "blephoneVPN";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.tabControl1.ResumeLayout(false);
            this.defaut.ResumeLayout(false);
            this.defaut.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.input.ResumeLayout(false);
            this.input.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.cms1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage defaut;
        private System.Windows.Forms.TextBox tb_pwd1;
        private System.Windows.Forms.TextBox tb_username1;
        private System.Windows.Forms.Label label_pwd1;
        private System.Windows.Forms.Label label_username1;
        private System.Windows.Forms.TabPage input;
        private System.Windows.Forms.Button button_exit1;
        private System.Windows.Forms.Button button_login1;
        private System.Windows.Forms.Button button_exit2;
        private System.Windows.Forms.Button button_login2;
        private System.Windows.Forms.TextBox tb_username2;
        private System.Windows.Forms.TextBox tb_ip;
        private System.Windows.Forms.Label label_pwd2;
        private System.Windows.Forms.Label label_username2;
        private System.Windows.Forms.TextBox tb_pwd2;
        private System.Windows.Forms.Label label_ip;
        private System.Windows.Forms.NotifyIcon vpnnotify;
        private System.Windows.Forms.ContextMenuStrip cms1;
        private System.Windows.Forms.ToolStripMenuItem tsm_exit;
        private System.Windows.Forms.ToolStripMenuItem tsm_show;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox tb_msg1;
        private System.Windows.Forms.TextBox tb_msg2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsm_setting;
        private System.Windows.Forms.ToolStripMenuItem tsm_drive;
        private System.Windows.Forms.CheckBox cb_rem;
        private System.Windows.Forms.ComboBox cb_username;
        private System.Windows.Forms.ToolStripMenuItem tsm_warning;
    }
}

