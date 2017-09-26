namespace blephoneVPN
{
    partial class Setting
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
            this.tb_sql_ip = new System.Windows.Forms.TextBox();
            this.tb_sql_username = new System.Windows.Forms.TextBox();
            this.tb_sql_pwd = new System.Windows.Forms.TextBox();
            this.btn_save = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_sql_db = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_sql_table = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(24, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "数据库IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(24, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "数据库用户名:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(24, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "数据库密码:";
            // 
            // tb_sql_ip
            // 
            this.tb_sql_ip.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.tb_sql_ip.Location = new System.Drawing.Point(143, 19);
            this.tb_sql_ip.Name = "tb_sql_ip";
            this.tb_sql_ip.Size = new System.Drawing.Size(138, 24);
            this.tb_sql_ip.TabIndex = 3;
            // 
            // tb_sql_username
            // 
            this.tb_sql_username.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.tb_sql_username.Location = new System.Drawing.Point(143, 67);
            this.tb_sql_username.Name = "tb_sql_username";
            this.tb_sql_username.Size = new System.Drawing.Size(138, 24);
            this.tb_sql_username.TabIndex = 4;
            // 
            // tb_sql_pwd
            // 
            this.tb_sql_pwd.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.tb_sql_pwd.Location = new System.Drawing.Point(143, 113);
            this.tb_sql_pwd.Name = "tb_sql_pwd";
            this.tb_sql_pwd.PasswordChar = '*';
            this.tb_sql_pwd.Size = new System.Drawing.Size(138, 24);
            this.tb_sql_pwd.TabIndex = 5;
            // 
            // btn_save
            // 
            this.btn_save.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_save.Location = new System.Drawing.Point(50, 251);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(75, 34);
            this.btn_save.TabIndex = 8;
            this.btn_save.Text = "保存";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.btn_cancel.Location = new System.Drawing.Point(182, 251);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 34);
            this.btn_cancel.TabIndex = 9;
            this.btn_cancel.Text = "取消";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(24, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "数据库名称:";
            // 
            // tb_sql_db
            // 
            this.tb_sql_db.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.tb_sql_db.Location = new System.Drawing.Point(143, 157);
            this.tb_sql_db.Name = "tb_sql_db";
            this.tb_sql_db.Size = new System.Drawing.Size(138, 24);
            this.tb_sql_db.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(24, 205);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 15);
            this.label5.TabIndex = 11;
            this.label5.Text = "数据表:";
            // 
            // tb_sql_table
            // 
            this.tb_sql_table.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.tb_sql_table.Location = new System.Drawing.Point(143, 202);
            this.tb_sql_table.Name = "tb_sql_table";
            this.tb_sql_table.Size = new System.Drawing.Size(138, 24);
            this.tb_sql_table.TabIndex = 7;
            // 
            // Setting
            // 
            this.AcceptButton = this.btn_save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 311);
            this.Controls.Add(this.tb_sql_table);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tb_sql_db);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.tb_sql_pwd);
            this.Controls.Add(this.tb_sql_username);
            this.Controls.Add(this.tb_sql_ip);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Setting";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Setting";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Setting_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_sql_ip;
        private System.Windows.Forms.TextBox tb_sql_username;
        private System.Windows.Forms.TextBox tb_sql_pwd;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_sql_db;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_sql_table;
    }
}