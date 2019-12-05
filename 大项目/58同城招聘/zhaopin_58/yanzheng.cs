using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zhaopin_58
{
    public partial class yanzheng : Form
    {
        public yanzheng()
        {
            InitializeComponent();
        }
        
       
        #region  卡密注册

        public void doyanzheng()
        {
            try
            {
                string mac = method.GetMacAddress();
                string time = DateTime.Now.AddDays(365).ToString();

                string constr = "Host =47.99.68.92;Database=acaiji;Username=root;Password=zhoukaige00.@*.";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select * from kami where userkey='" + textBox1.Text.Trim() + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

              
                if (reader.Read())
                {
                    string isuse = reader["isuse"].ToString().Trim();
  
                    mycon.Close();
                    reader.Close();
                    if (isuse == "0")
                    {
                   

                        string constr1 = "Host =47.99.68.92;Database=acaiji;Username=root;Password=zhoukaige00.@*.";
                        MySqlConnection mycon1 = new MySqlConnection(constr1);
                        mycon1.Open();
                        MySqlCommand cmd1 = new MySqlCommand("UPDATE kami SET isuse = '1',mac = '" + mac + " ' ,time='" + time + " ' where userkey='" + textBox1.Text.Trim() + "'", mycon1);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'
                        int count = cmd1.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                        if (count > 0)
                        {
                            MessageBox.Show("激活成功请重启软件！");


                        }
                        else
                        {
                            MessageBox.Show("激活失败，请检查网络！");
                            
                        }
                        mycon1.Close();
                    }
                    else
                    {
                        MessageBox.Show("您的卡密已经使用！");
                        
                    }

                }
                else
                {
                    MessageBox.Show("您的卡密错误！");
                   
                }
                mycon.Close();



            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }


        #endregion

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(doyanzheng));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void Yanzheng_Load(object sender, EventArgs e)
        {

        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int j = 0; j < 1000; j++)
            {

            string chars = "123456789ABCDEFGHIJKLMNPQRSTUVWXYZ";

            Random randrom = new Random((int)DateTime.Now.Ticks);

            string str = "";
            for (int i = 0; i < 30; i++)
            {
                str += chars[randrom.Next(chars.Length)];
            }
            textBox2.Text += str + "\r\n";
            string constr = "Host =47.99.68.92;Database=acaiji;Username=root;Password=zhoukaige00.@*.";
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();

            MySqlCommand cmd = new MySqlCommand("INSERT INTO kami  (userkey) VALUES('" + str.Trim() + "') ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


            MySqlDataReader reader = cmd.ExecuteReader();
            mycon.Close();
            reader.Close();
                Thread.Sleep(100);

            }
        }
    }
}
