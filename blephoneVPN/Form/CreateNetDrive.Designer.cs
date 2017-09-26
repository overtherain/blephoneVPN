namespace blephoneVPN
{
    partial class CreateNetDrive
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_drive_remotename = new System.Windows.Forms.TextBox();
            this.tb_drive_localname = new System.Windows.Forms.TextBox();
            this.tb_drive_username = new System.Windows.Forms.TextBox();
            this.tb_drive_pwd = new System.Windows.Forms.TextBox();
            this.btn_drive_login = new System.Windows.Forms.Button();
            this.btn_drive_cancel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "网络磁盘地址:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(46, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "映射盘符:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(63, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "用户名:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(80, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "密码:";
            // 
            // tb_drive_remotename
            // 
            this.tb_drive_remotename.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.tb_drive_remotename.Location = new System.Drawing.Point(136, 18);
            this.tb_drive_remotename.Name = "tb_drive_remotename";
            this.tb_drive_remotename.Size = new System.Drawing.Size(172, 24);
            this.tb_drive_remotename.TabIndex = 4;
            // 
            // tb_drive_localname
            // 
            this.tb_drive_localname.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.tb_drive_localname.Location = new System.Drawing.Point(136, 62);
            this.tb_drive_localname.Name = "tb_drive_localname";
            this.tb_drive_localname.Size = new System.Drawing.Size(172, 24);
            this.tb_drive_localname.TabIndex = 5;
            // 
            // tb_drive_username
            // 
            this.tb_drive_username.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.tb_drive_username.Location = new System.Drawing.Point(136, 103);
            this.tb_drive_username.Name = "tb_drive_username";
            this.tb_drive_username.Size = new System.Drawing.Size(172, 24);
            this.tb_drive_username.TabIndex = 6;
            // 
            // tb_drive_pwd
            // 
            this.tb_drive_pwd.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.tb_drive_pwd.Location = new System.Drawing.Point(136, 138);
            this.tb_drive_pwd.Name = "tb_drive_pwd";
            this.tb_drive_pwd.PasswordChar = '*';
            this.tb_drive_pwd.Size = new System.Drawing.Size(172, 24);
            this.tb_drive_pwd.TabIndex = 7;
            // 
            // btn_drive_login
            // 
            this.btn_drive_login.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.btn_drive_login.Location = new System.Drawing.Point(83, 190);
            this.btn_drive_login.Name = "btn_drive_login";
            this.btn_drive_login.Size = new System.Drawing.Size(75, 33);
            this.btn_drive_login.TabIndex = 8;
            this.btn_drive_login.Text = "映射";
            this.btn_drive_login.UseVisualStyleBackColor = true;
            this.btn_drive_login.Click += new System.EventHandler(this.btn_drive_login_Click);
            // 
            // btn_drive_cancel
            // 
            this.btn_drive_cancel.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.btn_drive_cancel.Location = new System.Drawing.Point(233, 190);
            this.btn_drive_cancel.Name = "btn_drive_cancel";
            this.btn_drive_cancel.Size = new System.Drawing.Size(75, 33);
            this.btn_drive_cancel.TabIndex = 9;
            this.btn_drive_cancel.Text = "取消";
            this.btn_drive_cancel.UseVisualStyleBackColor = true;
            this.btn_drive_cancel.Click += new System.EventHandler(this.btn_drive_cancel_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(314, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(137, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "例如:\\\\192.168.1.1\\xxx";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(314, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "例如:G:";
            // 
            // CreateNetDrive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 253);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btn_drive_cancel);
            this.Controls.Add(this.btn_drive_login);
            this.Controls.Add(this.tb_drive_pwd);
            this.Controls.Add(this.tb_drive_username);
            this.Controls.Add(this.tb_drive_localname);
            this.Controls.Add(this.tb_drive_remotename);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "CreateNetDrive";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CreateNetDrive";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_drive_remotename;
        private System.Windows.Forms.TextBox tb_drive_localname;
        private System.Windows.Forms.TextBox tb_drive_username;
        private System.Windows.Forms.TextBox tb_drive_pwd;
        private System.Windows.Forms.Button btn_drive_login;
        private System.Windows.Forms.Button btn_drive_cancel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}