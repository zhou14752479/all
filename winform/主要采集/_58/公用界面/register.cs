using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using IWshRuntimeLibrary;
using System.Runtime.InteropServices;

namespace _58
{
    public partial class register : Form
    {
        public register()
        {
            InitializeComponent();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {

            DateTime dt = DateTime.Now;
            string time = DateTime.Now.ToString();
            string ip = Method.GetIp();
            string mac = Method.GetMacAddress();


            if (skinTextBox1.Text == "" || skinTextBox2.Text == "" || skinTextBox3.Text == "" || skinTextBox4.Text == "")
            {
                MessageBox.Show("请完善账号信息！");
                return;
            }
            if (skinTextBox3.Text.Length < 11)
            {
                MessageBox.Show("请输入十一位有效手机号！");
                return;
            }

            if (skinTextBox4.Text.Trim() != Method.RandomKey())
            {
                MessageBox.Show("您的注册码错误！");
                return;
            }


            try
            {


                string constr = "Host =116.62.62.62;Database=vip;Username=root;Password=zhoukaige";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                string username = skinTextBox1.Text.Trim();
                string password = skinTextBox2.Text.Trim();
                string phone = skinTextBox3.Text.Trim();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO vip_points (username,password,phone,register_t,ip,mac)VALUES('" + username + " ', '" + password + " ','" + phone + " ', '" + time + " ', '" + ip + " ', '" + mac + " ')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

              //  MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，注册不需要读取，直接执行SQL语句即可

                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {
                    MessageBox.Show("注册成功！");
                    this.Close();
                    mycon.Close();
                    // reader.Close();                  
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

        private void register_Load(object sender, EventArgs e)
        {

        }



        private void skinTextBox1_Enter(object sender, EventArgs e)
        {
            skinTextBox1.Text = "";
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Method.clearkey();
            System.Diagnostics.Process.Start("IEXPLORE.EXE", "https://item.taobao.com/item.htm?scm=1007.13982.82927.0&id=566098255025&last_time=1521528109");
        }

        #region  创建桌面快捷方式

        public  void creatdesktop()

        {
           
           
            string app = Environment.CurrentDirectory + @"\_58.exe";

            try
            {
                // Create a Windows Script Host Shell class
                IWshShell_Class shell = new IWshShell_Class();
                // Define the shortcut file
                IWshShortcut_Class shortcut = shell.CreateShortcut(app + ".lnk") as IWshShortcut_Class;
                // Set all its properties
                shortcut.Description = "Smart sample of creating shell shortcut";
                shortcut.TargetPath = app;
                shortcut.IconLocation = app + ",0";
                // Save it
                shortcut.Save();
            }
            catch (COMException ex)
            {
                Console.WriteLine(ex.Message);
            }


        }

        #endregion
    }
}
