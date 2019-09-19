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

        #region 获取Mac地址
        /// <summary>
        /// 获取Mac地址
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            try
            {
                string strMac = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        strMac = mo["MacAddress"].ToString();
                    }
                }
                moc = null;
                mc = null;
                return strMac;
            }
            catch
            {
                return "unknown";
            }
        }

        #endregion

        #region 注册码随机生成函数
        /// <summary>
        /// 注册码随机生成函数
        /// </summary>
        /// <returns></returns>
        public static string Random()

        {


            //string[] array = {"A","B","C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};
            string Hour = DateTime.Now.Hour.ToString();                    //获取当前小时
            string Hour2 = Math.Pow(Convert.ToDouble(Hour), 2).ToString();  //获取当前小时的平方
            //string zimu = array[DateTime.Now.Hour];                        //获取当前小时作为数组索引的字母
            string key = Hour + "1475" + (Hour2) + "2479" + Hour;

            return key;



        }


        #endregion

        #region  登陆函数

        public void myLogin()
        {



            try
            {



                string constr = "Host =47.99.68.92;Database=vip_database;Username=root;Password=zhoukaige00.@*.";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select * from vip where username='" + skinTextBox1.Text.Trim() + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源



                if (reader.Read())
                {

                    string username = reader["username"].ToString().Trim();
                    string password = reader["password"].ToString().Trim();
                    string mac = reader["mac"].ToString().Trim();


                    //判断MAC地址

                    if (GetMacAddress().ToString().Trim() != mac)
                    {
                        MessageBox.Show("您使用的此台电脑未开通，如需开通此台电脑请联系客服购买！VX：17606117606");
                        return;
                    }



                    //判断密码
                    if (skinTextBox2.Text.Trim() == password)

                    {

                        skinButton1.Text = "正在连接服务器......";

                        Application.DoEvents();
                        System.Threading.Thread.Sleep(200);

                        skinButton1.Text = "正在验证用户名和密码......";
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(200);



                        //记住账号密码
                        if (checkBox1.Checked == true)
                        {
                            RegistryKey regkey = Registry.CurrentUser.CreateSubKey("acaiji");//要创建一个特殊的字符串，防止与系统或其他程序写注册表名称相同，
                            regkey.SetValue("UserName", skinTextBox1.Text.Trim());
                            regkey.SetValue("PassWord", skinTextBox2.Text.Trim());
                            regkey.Close();

                        }

                        //meituan mt = new meituan();
                        //mt.Show();
                        fang_58 fang = new fang_58();
                        fang.Show();

                        //Map mp = new Map();
                        //mp.Show();
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

        private void SkinButton2_Click(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            string time = DateTime.Now.ToString();
            string ip = Method.GetIp();
            string mac = GetMacAddress();


            if (skinTextBox3.Text == "" || skinTextBox4.Text == "" || skinTextBox5.Text == "")
            {
                MessageBox.Show("请完善账号信息！");
                return;
            }
            if (skinTextBox3.Text.Length < 11)
            {
                MessageBox.Show("请输入十一位有效手机号！");
                return;
            }

            if (skinTextBox5.Text.Trim() != Random())
            {
                MessageBox.Show("您的注册码错误！");
                return;
            }


            try
            {


                string constr = "Host =47.99.68.92;Database=vip_database;Username=root;Password=zhoukaige00.@*.";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                string username = skinTextBox3.Text.Trim();
                string password = skinTextBox4.Text.Trim();


                MySqlCommand cmd = new MySqlCommand("INSERT INTO vip (username,password,register_t,ip,mac)VALUES('" + username + " ', '" + password + " ', '" + time + " ', '" + ip + " ', '" + mac + " ')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {
                    MessageBox.Show("注册成功！");

                    mycon.Close();

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

        private void Label5_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
        private Point mPoint = new Point();
        private void SkinPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void SkinPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
            }
        }
    }
}
