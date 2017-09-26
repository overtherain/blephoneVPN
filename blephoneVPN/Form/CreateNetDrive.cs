using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using blephoneVPN.Util;

namespace blephoneVPN
{
    public partial class CreateNetDrive : Form
    {
        NetDriveCtl ndc = new NetDriveCtl();
        public CreateNetDrive()
        {
            InitializeComponent();
        }

        private void btn_drive_login_Click(object sender, EventArgs e)
        {
            PublicVar.localName = this.tb_drive_localname.Text.ToString();
            PublicVar.remoteName = this.tb_drive_remotename.Text.ToString();
            PublicVar.driveUserName = this.tb_drive_username.Text.ToString();
            PublicVar.driveUserPwd = this.tb_drive_pwd.Text.ToString();
#if DEBUG
            Console.WriteLine("CreateNetDrive input items, localName : {0}, remoteName : {1}, driveUserName : {2}, driveUserPwd : {3}"
                                , PublicVar.localName, PublicVar.remoteName, PublicVar.driveUserName, PublicVar.driveUserPwd);
#endif
            string msg = "CreateNetDrive input items, localName : '" + PublicVar.localName
                        + "', remoteName : '" + PublicVar.remoteName
                        + "', driveUserName : '" + PublicVar.driveUserName
                        + "', driveUserPwd : '" + PublicVar.driveUserPwd;
            //MessageBox.Show(msg);
            if (PublicVar.localName != null && PublicVar.localName != ""
                && PublicVar.remoteName != null && PublicVar.remoteName != ""
                && PublicVar.driveUserName != null && PublicVar.driveUserName != ""
                && PublicVar.driveUserPwd != null && PublicVar.driveUserPwd != "")
            {
                int ret = ndc.CreateDrive(PublicVar.localName, PublicVar.remoteName, PublicVar.driveUserName, PublicVar.driveUserPwd);
#if DEBUG
                Console.WriteLine("ndc.CreateDrive ret:{0}, code:{1}", ret, (ERROR_ID)ret);
#endif
                if (ret != 0)
                {
                    MessageBox.Show("映射失败! ret:" + (ERROR_ID)ret);
                    return ;
                }
                PublicVar.isDriveOpen = false;
                try 
                {
                    if (Directory.Exists(PublicVar.localName))
                    {
                        System.Diagnostics.Process.Start("Explorer.exe", PublicVar.localName);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("此路径不存在 : '" + PublicVar.localName.ToString() + "'");
                    }
                }
                catch(Exception err_msg)
                {
                    Console.WriteLine("open fold error:{0}", err_msg.ToString());
                    MessageBox.Show(err_msg.ToString());
                }
            }
            else 
            {
                MessageBox.Show("输入有误!请重新输入.");
            }
        } 

        private void btn_drive_cancel_Click(object sender, EventArgs e)
        {
            if (PublicVar.localName != null && PublicVar.localName != "" 
                && PublicVar.remoteName != null && PublicVar.remoteName != "")
            {
                ndc.DeleteDrive(PublicVar.localName, PublicVar.remoteName);
            }
            PublicVar.isDriveOpen = false;
            this.Close();
        }
    }
}
