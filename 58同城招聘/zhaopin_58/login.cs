using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zhaopin_58
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }
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
            string key = Hour + "1475" + (Hour2) +"2479" + Hour;

            return key;



        }


        #endregion

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

        #region  登陆函数，用户名或者手机号都可以登陆

        public void myLogin()
        {



            try
            {



                string constr = "Host =116.62.62.62;Database=vip;Username=root;Password=zhoukaige";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select * from wuba2019 where phone='" + skinTextBox1.Text.Trim() + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源



                if (reader.Read())
                {

                    string username = reader["username"].ToString().Trim();
                    string password = reader["password"].ToString().Trim();
                    string mac = reader["mac"].ToString().Trim();


                    //判断MAC地址

                    if (GetMacAddress().ToString().Trim()!= mac)
                    {
                        MessageBox.Show("您使用的此台电脑未开通，如需开通此台电脑请联系客服购买！VX：17606117606");
                        return;
                    }



                    //判断密码
                    if (skinTextBox2.Text.Trim() == password)

                    {

                        skinButton1.Text = "正在连接服务器......";

                        Application.DoEvents();
                        System.Threading.Thread.Sleep(500);

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
                        main mf = new main();
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

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }


        #endregion
        

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

        private void skinPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            register rg = new register();
            rg.Show();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("联系微信17606117606，提供您的手机号！");
        }
    }
}
