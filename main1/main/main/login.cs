using Microsoft.Win32;
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

namespace main
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        #region  登陆函数，用户名或者手机号都可以登陆

        public void myLogin()
        {



            try
            {



                string constr = "Host =116.62.62.62;Database=vip;Username=root;Password=zhoukaige";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select * from vip_points where username='" + skinTextBox1.Text.Trim() + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源



                if (reader.Read())
                {

                    string username = reader["username"].ToString().Trim();
                    string password = reader["password"].ToString().Trim();


                    if (skinTextBox2.Text.Trim() == password)

                    {

                        skinButton1.Text = "正在连接服务器......";

                        Application.DoEvents();
                        System.Threading.Thread.Sleep(500);

                        skinButton1.Text = "正在验证用户名和密码......";
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(200);

                        mainForm mf = new mainForm();
                        Userdelegate ud = new Userdelegate(mf.getUsername);
                        ud(username);



                        //记住账号密码
                        if (checkBox1.Checked == true)
                        {
                            RegistryKey regkey = Registry.CurrentUser.CreateSubKey("acaiji");//要创建一个特殊的字符串，防止与系统或其他程序写注册表名称相同，
                            regkey.SetValue("UserName", skinTextBox1.Text.Trim());
                            regkey.SetValue("PassWord", skinTextBox2.Text.Trim());
                            regkey.Close();

                        }
                       
                        mf.Show();
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

                    reader.Close();
                    MySqlCommand cmd2 = new MySqlCommand("select * from vip_points where phone='" + skinTextBox1.Text.Trim() + "' ", mycon);
                    MySqlDataReader reader2 = cmd2.ExecuteReader();
                    if (reader2.Read())
                    {


                        string username = reader2["username"].ToString().Trim();
                        string password = reader2["password"].ToString().Trim();

                        if (skinTextBox2.Text.Trim() == password)

                        {

                            skinButton1.Text = "正在连接服务器......";

                            Application.DoEvents();
                            System.Threading.Thread.Sleep(500);

                            skinButton1.Text = "正在验证用户名和密码......";

                            Application.DoEvents();
                            System.Threading.Thread.Sleep(200);

                            mainForm mf = new mainForm();
                            //调用委托
                            Userdelegate ud = new Userdelegate(mf.getUsername);  
                            ud(username);

                            //记住账号密码
                            if (checkBox1.Checked == true)
                            {
                                RegistryKey regkey = Registry.CurrentUser.CreateSubKey("acaiji");//要创建一个特殊的字符串，防止与系统或其他程序写注册表名称相同，如果存在则更新值。
                                regkey.SetValue("UserName", skinTextBox1.Text.Trim());
                                regkey.SetValue("PassWord", skinTextBox2.Text.Trim());
                                regkey.Close();

                            }

                            
                            mf.Show();
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
                        MessageBox.Show("未查询到您的账户信息！请联系客服开通账号！");
                        return;
                    }

                }



            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }


        #endregion

        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (skinTextBox1.Text == "" || skinTextBox1.Text == "用户名或者手机号")
            {
                MessageBox.Show("请输入用户名！");
                return;
            }
            if (skinTextBox2.Text == "" || skinTextBox2.Text == "请输入密码")
            {
                MessageBox.Show("请输入密码！");
                return;
            }

            myLogin();



        }


       

        private void skinTextBox1_Enter(object sender, EventArgs e)
        {
            skinTextBox1.Text = "";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://acaiji.com/register.html");
        }

        private void login_Load(object sender, EventArgs e)
        {

           

            RegistryKey regkey = Registry.CurrentUser.OpenSubKey("acaiji"); //打开爱采集注册表

                if (regkey != null)

                {
                checkBox1.Checked = true;
                   skinTextBox1.Text = regkey.GetValue("UserName").ToString();
                   skinTextBox2.Text = regkey.GetValue("PassWord").ToString();
                   regkey.Close();
                }


        }
    }
}
