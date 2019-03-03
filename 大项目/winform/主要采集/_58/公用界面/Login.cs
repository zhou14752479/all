using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Management;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using CCWin;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Win32;

namespace _58
{
    public partial class Login : Form
    {
        public Login()
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

                        label6.Text = "正在连接服务器......";

                        Application.DoEvents();
                        System.Threading.Thread.Sleep(200);

                        label6.Text = "正在验证用户名和密码......";

                        Method.User = username;
                        this.Close();

                    }

                    else

                    {

                        MessageBox.Show("您的密码错误！");
                        label6.Text = "请重新输入密码！";
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

                            label6.Text = "正在连接服务器......";

                            Application.DoEvents();
                            System.Threading.Thread.Sleep(200);

                            label6.Text = "正在验证用户名和密码......";

                            Method.User = username;
                            this.Close();

                        }

                        else

                        {

                            MessageBox.Show("您的密码错误！");
                            label6.Text = "请重新输入密码！";
                        }


                    }
                    else
                    {
                        MessageBox.Show("未查询到您的账户信息！请联系客服开通账号！");
                        this.Close();
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

        private void label5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label5_MouseHover(object sender, EventArgs e)
        {
            label5.ForeColor = Color.Blue;
        }
       
       

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            label5.ForeColor = Color.White;
        }
        
        private void skinTextBox1_Enter(object sender, EventArgs e)
        {
            //skinTextBox1.Text = "";
        }

        private void skinTextBox2_Enter(object sender, EventArgs e)
        {
            
        }

     

        private void skinPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {
            //RegistryKey regkey = Registry.CurrentUser.OpenSubKey("cc"); //打开cc注册表

            //if (regkey != null)

            //{

            //    string[] keys = Method.readKey();
            //    skinTextBox1.Text = keys[0];

            //    skinTextBox2.Text = keys[1];
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("IEXPLORE.EXE", "http://wpa.qq.com/msgrd?v=3&uin=852266010&site=qq&menu=yes");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("IEXPLORE.EXE", "https://amos.alicdn.com/getcid.aw?v=3&uid=zkg852266010&site=cnalichn&groupid=0&s=1&charset=gbk"); 
        }

        private void skinTextBox1_Enter_1(object sender, EventArgs e)
        {
            skinTextBox1.Text = "";
        }

        private void skinRadioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
            System.Diagnostics.Process.Start("IEXPLORE.EXE", "https://item.taobao.com/item.htm?scm=1007.13982.82927.0&id=566098255025&last_time=1521528109");
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.skinButton1_Click(sender, e);//触发button事件  
            }
        }
    }
}
