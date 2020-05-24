using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Media;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 保罗看水
{
    public partial class 登录 : Form
    {
        public 登录()
        {
            InitializeComponent();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
               
            }
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            textBox1.Text = "";
        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "请输入卡密";
            }
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

        #region  登陆函数

        public void login(string zhucema, string mac)
        {
         
            try
            {
                string constr = "Host =111.229.244.97;Database=kanshui;Username=root;Password=root";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select * from users where mac='" + mac + "'  ", mycon);         //SQL语句读取textbox的值'"+textBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                if (reader.Read())
                {
                    string zhuce = reader["zhucema"].ToString().Trim();
                    string time = reader["expiretime"].ToString().Trim();
                   
                    if (zhuce.Trim() == zhucema)
                    {
                        if (Convert.ToDateTime(time) > DateTime.Now)
                        {
                            main mn = new main();
                            mn.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("账号已过期");
                        }

                    }
                    else
                    {
                        MessageBox.Show("激活码错误");
                    }
                }
                else
                {
                    MessageBox.Show("此电脑未绑定激活码，请先绑定");
                }

                mycon.Close();
                reader.Close();



            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
          
            login(textBox1.Text.Trim(),GetMacAddress());
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
                string constr = "Host =111.229.244.97;Database=kanshui;Username=root;Password=root";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("update users set mac='" + GetMacAddress() + "' where zhucema='" + textBox1.Text.Trim()+ "'  ", mycon);         //SQL语句读取textbox的值'"+textBox1.Text+"'


            int count = cmd.ExecuteNonQuery();  //读取数据库数据信息，这个方法不需要绑定资源

            if (count>0)
            {
                MessageBox.Show("绑定成功");
                mycon.Close();
            }
            else
            {
                MessageBox.Show("激活码错误");
                mycon.Close();

            }

         }



    }
}
