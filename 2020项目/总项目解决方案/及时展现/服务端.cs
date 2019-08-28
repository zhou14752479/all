using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace 及时展现
{
    public partial class 服务端 : Form
    {
        public 服务端()
        {
            InitializeComponent();
        }

        #region 加载省份
        public ArrayList getP()
        {
            ArrayList list = new ArrayList();
            try
            {
                string constr = "Host =139.159.218.174;Database=data;Username=root;Password=123456";
                string str = "SELECT name from areas where length(code)=2  ";
                MySqlDataAdapter da = new MySqlDataAdapter(str, constr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString().Trim());
                }
            }
            catch (MySqlException ee)
            {
               textBox2.Text= ee.Message.ToString();
            }
            return list;

        }
        #endregion

        #region 获取省份下的城市
        public ArrayList getC(string pro)
        {
            ArrayList list = new ArrayList();
            try
            {
                string constr = "Host =139.159.218.174;Database=data;Username=root;Password=123456";
                string str = "SELECT name from areas where length(code)=4 and left(code,2)='" + pro + "'";  //2代表开头前两个字符
                MySqlDataAdapter da = new MySqlDataAdapter(str, constr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString().Trim());
                }
            }
            catch (MySqlException ee)
            {
                textBox2.Text = ee.Message.ToString();
            }
            return list;

        }
        #endregion

        #region 获取城市下的地区
        public ArrayList getA(string city)
        {
            ArrayList list = new ArrayList();
            try
            {
                string constr = "Host =139.159.218.174;Database=data;Username=root;Password=123456";
                string str = "SELECT name from areas where length(code)=6 and left(code,4)='" + city + "'";  //2代表开头前两个字符
                MySqlDataAdapter da = new MySqlDataAdapter(str, constr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString().Trim());
                }
            }
            catch (MySqlException ee)
            {
                textBox2.Text = ee.Message.ToString();
            }
            return list;

        }
        #endregion

        #region  获取code

        public string Getcode(string city)
        {

            try
            {
                string constr = "Host =139.159.218.174;Database=data;Username=root;Password=123456";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select code from areas where name='" + city + "'  ", mycon);         //SQL语句读取textbox的值'"+textBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                reader.Read();

                string code = reader["code"].ToString().Trim();
                mycon.Close();
                reader.Close();
                return code;


            }

            catch (System.Exception ex)
            {
               return( ex.ToString());
            }
           

        }

        #endregion

        

        private void 服务端_Load(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            list = getP();
            comboBox1.DataSource = list;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            ArrayList lists = new ArrayList();
            lists = getC(Getcode(comboBox1.Text));
            comboBox2.Items.Add("不限");
            foreach (string list in lists)
            {
                comboBox2.Items.Add(list);
            }
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            ArrayList lists = new ArrayList();
            lists = getA(Getcode(comboBox2.Text));
            comboBox3.Items.Add("不限");
            foreach (string list in lists)
            {
                comboBox3.Items.Add(list);
            }
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox1.Text = "全国";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = comboBox1.Text +"：" +comboBox2.Text + "：" + comboBox3.Text;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            //string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            string chars = "123456789abcdefghijkmnpqrstuvwxyz";
            Random randrom = new Random((int)DateTime.Now.Ticks);

            string user = "";
            string pass = "";
            for (int i = 0; i < 6; i++)
            {
                user += chars[randrom.Next(chars.Length)];
            }
            for (int i = 0; i < 6; i++)
            {
                pass += chars[randrom.Next(chars.Length)];
            }

            

            string constr = "Host =139.159.218.174;Database=data;Username=root;Password=123456";
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();
            string time = DateTime.Now.AddDays(Convert.ToUInt32(comboBox4.Text)).ToString();
            string keywords = "没有";


            MySqlCommand cmd = new MySqlCommand("INSERT INTO users (user,pass,time,keywords)VALUES('" + user + " ', '" + pass + " ','" + time + " ', '" + keywords + "')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

            int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
            if (count > 0)
            {
                textBox2.Text = "账号：" + user + "   密码：" + pass;
                
                mycon.Close();
                                
            }
            else
            {
                textBox2.Text = "生成失败";
            }
        }
    }
}
