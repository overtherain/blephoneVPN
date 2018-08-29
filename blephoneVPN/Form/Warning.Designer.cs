namespace blephoneVPN
{
    partial class Warning
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
            this.warning_msg = new System.Windows.Forms.Label();
            this.bt_msg_ok = new System.Windows.Forms.Button();
            this.bt_msg_cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // warning_msg
            // 
            this.warning_msg.AutoSize = true;
            this.warning_msg.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.warning_msg.Location = new System.Drawing.Point(39, 20);
            this.warning_msg.Name = "warning_msg";
            this.warning_msg.Size = new System.Drawing.Size(43, 15);
            this.warning_msg.TabIndex = 0;
            this.warning_msg.Text = "text";
            // 
            // bt_msg_ok
            // 
            this.bt_msg_ok.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.bt_msg_ok.Location = new System.Drawing.Point(28, 75);
            this.bt_msg_ok.Name = "bt_msg_ok";
            this.bt_msg_ok.Size = new System.Drawing.Size(99, 42);
            this.bt_msg_ok.TabIndex = 1;
            this.bt_msg_ok.Text = "确定";
            this.bt_msg_ok.UseVisualStyleBackColor = true;
            this.bt_msg_ok.Click += new System.EventHandler(this.bt_msg_ok_Click);
            // 
            // bt_msg_cancel
            // 
            this.bt_msg_cancel.Font = new System.Drawing.Font("幼圆", 11.25F, System.Drawing.FontStyle.Bold);
            this.bt_msg_cancel.Location = new System.Drawing.Point(171, 75);
            this.bt_msg_cancel.Name = "bt_msg_cancel";
            this.bt_msg_cancel.Size = new System.Drawing.Size(98, 42);
            this.bt_msg_cancel.TabIndex = 2;
            this.bt_msg_cancel.Text = "取消";
            this.bt_msg_cancel.UseVisualStyleBackColor = true;
            this.bt_msg_cancel.Click += new System.EventHandler(this.bt_msg_cancel_Click);
            // 
            // Warning
            // 
            this.AcceptButton = this.bt_msg_ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 149);
            this.Controls.Add(this.bt_msg_cancel);
            this.Controls.Add(this.bt_msg_ok);
            this.Controls.Add(this.warning_msg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Warning";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Warning";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Warning_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label warning_msg;
        private System.Windows.Forms.Button bt_msg_ok;
        private System.Windows.Forms.Button bt_msg_cancel;
    }
}