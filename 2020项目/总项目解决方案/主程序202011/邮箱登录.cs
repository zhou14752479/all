using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序202011
{
    public partial class 邮箱登录 : Form
    {
        public 邮箱登录()
        {
            InitializeComponent();
        }

        #region  获取32位MD5加密
        public string GetMD5(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        #endregion

        private int GetCreatetime(DateTime dt)
        {

            DateTime DateStart = new DateTime(1970, 1, 1, 8, 0, 0);

            return Convert.ToInt32((dt - DateStart).TotalSeconds);

        }
        #region  登陆函数

        public void myLogin()
        {


            try
            {
                string constr = "Host =143.92.45.176;Database=fastadmin;Username=fastadmin;Password=cFfJA2SH3JeFKtj7";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select * from fa_user where username='" + textBox1.Text.Trim() + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                if (reader.Read())
                {
                    string username = reader["username"].ToString().Trim();
                    string password = reader["password"].ToString().Trim();
                    string timestamp= reader["jointime"].ToString().Trim();

                    int nowtime = GetCreatetime(DateTime.Now.Date);
                    if (nowtime < Convert.ToInt32(timestamp))
                    {

                        //判断密码
                        if (GetMD5(GetMD5(textBox2.Text.Trim()) + "salt") == password)

                        {

                            button1.Text = "正在连接服务器......";

                            Application.DoEvents();
                            System.Threading.Thread.Sleep(500);

                            button1.Text = "正在验证用户名和密码....";
                            Application.DoEvents();
                            System.Threading.Thread.Sleep(500);

                            邮箱163抓取 fm1 = new 邮箱163抓取();
                            fm1.Show();
                            this.Hide();

                        }

                        else

                        {

                            MessageBox.Show("您的密码错误！");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("账号已过期");
                        return;
                    }


                }



                else
                {
                    MessageBox.Show("未查询到您的账户信息！请注册账号！");
                    return;
                }


            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }


        #endregion


        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入用户名！");
                return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("请输入密码！");
                return;
            }

            myLogin();
        }
    }
}
