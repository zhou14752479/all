using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 商铺58
{
    public partial class 登陆 : Form
    {
        public 登陆()
        {
            InitializeComponent();
        }
        string path = AppDomain.CurrentDomain.BaseDirectory; //获取当前程序运行文件夹

        private void label5_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {

            }
        }
        #region 获取公网IP
        public static string GetIP()
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    webClient.Credentials = CredentialCache.DefaultCredentials;
                    byte[] pageDate = webClient.DownloadData("http://pv.sohu.com/cityjson?ie=utf-8");
                    String ip = Encoding.UTF8.GetString(pageDate);
                    webClient.Dispose();

                    Match rebool = Regex.Match(ip, @"\d{2,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
                    return rebool.Value;
                }
                catch (Exception e)
                {
                    return e.ToString();
                }

            }
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

                            FileStream fs1 = new FileStream(path + "config.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                            StreamWriter sw = new StreamWriter(fs1);
                            sw.WriteLine(skinTextBox1.Text);
                            sw.WriteLine(skinTextBox2.Text);
                            sw.Close();
                            fs1.Close();

                        }
                        Form1.username = skinTextBox1.Text;
                        Form1 fm1 = new Form1();
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

        private void skinButton2_Click(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            string time = DateTime.Now.ToString();
            string ip = GetIP();
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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.acaiji.com");
        }

        private void label1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.acaiji.com");
        }

        private void 登陆_Load(object sender, EventArgs e)
        {
            if (File.Exists(path + "config.txt"))
            {
                StreamReader sr = new StreamReader(path + "config.txt", Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                skinTextBox1.Text = text[0];
                skinTextBox2.Text = text[1];

            }
        }
    }
}
