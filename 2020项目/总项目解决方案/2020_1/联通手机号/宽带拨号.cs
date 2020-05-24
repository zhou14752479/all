using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 联通手机号
{
    public partial class 宽带拨号 : Form
    {
        public 宽带拨号()
        {
            InitializeComponent();
        }
        private static Mutex mutex = new Mutex();
        private Process dailer = new Process();
        private void Button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void BtnMini_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            IsConnectedToInternet();
            StopDailer();
            this.notifyIcon1.Icon = new Icon("close.ico");
            MessageBox.Show("程序已经暂停！");
        }
        #region Functions
        int Desc;
        //Creating the extern function...
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        //Creating a function that uses the API function...
        //if out parameter returns 18 then fail,if 81 then success 
        public void IsConnectedToInternet()
        {
            InternetGetConnectedState(out Desc, 0);
        }

        private void StopDailer()
        {
            //Process dailer = new Process();
            //dailer.StartInfo.FileName = "close.bat";
            //dailer.Start();
            //dailer.Close();  
            while (Desc == 81)
            {
                lock (dailer)
                {
                    if (!IsAlive("rundll32"))
                    {
                        mutex.WaitOne();
                        dailer.StartInfo.FileName = "rundll32.exe";
                        dailer.StartInfo.Arguments = "iedkcs32.dll CloseRASConnections";
                        dailer.Start();
                        //Thread.Sleep(1000);
                        mutex.ReleaseMutex();
                    }
                    //dailer.WaitForExit(1000);
                    //dailer.Close();
                }
                IsConnectedToInternet();
            }
            dailer.Close();
        }

        private void StartDailer()
        {
            //Process dailer = new Process();
            //dailer.StartInfo.FileName = "connect.bat";
            //dailer.Start();
            //dailer.Close();
            while (Desc != 81)
            {
                lock (dailer)
                {
                    if (!IsAlive("rasdial"))
                    {
                        mutex.WaitOne();
                        dailer.StartInfo.FileName = "rasdial.exe";
                        dailer.StartInfo.Arguments = txtDail.Text.Trim() + " " + txtName.Text.Trim() + " " + txtPWD.Text.Trim();
                        dailer.Start();
                        mutex.ReleaseMutex();
                    }
                    //dailer.WaitForExit(1000);
                    //dailer.Close();
                    //Thread.Sleep(1000);
                }
                IsConnectedToInternet();
            }
            dailer.Close();
        }

        private bool IsAlive(string name)
        {
            Process[] ps = Process.GetProcessesByName(name);
            if (ps.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(txtDail.Text.Trim()))
            {
                MessageBox.Show("请输入宽带拨号名称！");
                return false;
            }
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                MessageBox.Show("请输入宽带拨号的用户名！");
                return false;
            }
            if (string.IsNullOrEmpty(txtPWD.Text.Trim()))
            {
                MessageBox.Show("请输入宽带拨号的密码！");
                return false;
            }
            return true;
        }
        #endregion
        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                this.Visible = false;
                this.notifyIcon1.Icon = new Icon("open.ico");
                IsConnectedToInternet();
                this.StartDailer();
            }
        }
    }
}
