using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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
                        美团 mt = new 美团();
                            mt.Show();
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

        private Point mPoint = new Point();
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

   

      

        private void label5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("联系微信17606117606，提供您的手机号！");
        }

        private void Label2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("IExplore.exe", "http://www.acaiji.com/");
        }

        private void Label1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("IExplore.exe", "http://www.acaiji.com/");
        }

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

        private void Login_Resize(object sender, EventArgs e)
        {
            SetWindowRegion();
        }
        public void SetWindowRegion()
        {
            System.Drawing.Drawing2D.GraphicsPath FormPath;
            FormPath = new System.Drawing.Drawing2D.GraphicsPath();
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            FormPath = GetRoundedRectPath(rect, 10);
            this.Region = new Region(FormPath);

        }
        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            int diameter = radius;
            Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));
            GraphicsPath path = new GraphicsPath();

            // 左上角
            path.AddArc(arcRect, 180, 90);

            // 右上角
            arcRect.X = rect.Right - diameter;
            path.AddArc(arcRect, 270, 90);

            // 右下角
            arcRect.Y = rect.Bottom - diameter;
            path.AddArc(arcRect, 0, 90);

            // 左下角
            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90, 90);
            path.CloseFigure();//闭合曲线
            return path;
        }

        private void SkinButton2_Click(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            string time = DateTime.Now.ToString();
            string ip = method.GetIp();
            string mac = login.GetMacAddress();


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

            if (skinTextBox5.Text.Trim() != login.Random())
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

      

      

        private void Label1_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(label1,"点击访问");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.acaiji.com");
        }
    }
}
