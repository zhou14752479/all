using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main._2019_7
{
    public partial class jianli : Form
    {
        public jianli()
        {
            InitializeComponent();
        }
        bool jihuo = false;
        bool zanting = true;
        int index { get; set; }
        public void select()
        {

           
            try
            {
                string connetStr = "server=49.232.128.185;user=root;password=root;database=datas;"; //localhost不支持ssl连接时，最后一句一定要加！！！sslMode=none;
                MySqlConnection conn = new MySqlConnection(connetStr);
                conn.Open(); //连接数据库
                for (int i = 0; i < 200; i++)
                {
                   
                    
                //MessageBox.Show("数据库连接成功", "提示", MessageBoxButtons.OK);
                string searchStr = "SELECT * FROM jianli ORDER BY rand() LIMIT 1 ";   //student表中数据
                //MySqlDataAdapter adapter = new MySqlDataAdapter(searchStr, conn);
                //DataTable a = new DataTable();
                //adapter.Fill(a);
                //this.dataGridView1.DataSource = a;
                MySqlCommand cmd = new MySqlCommand(searchStr, conn);       
                MySqlDataReader reader = cmd.ExecuteReader();  
              


                if (reader.Read())
                {

                    string nicheng = reader["nicheng"].ToString().Trim();
                    string name = reader["name"].ToString().Trim();
                    string phone = reader["phone"].ToString().Trim();
                    string tel = reader["tel"].ToString().Trim();
                    string province = reader["province"].ToString().Trim();
                    string city = reader["city"].ToString().Trim();
                    string area = reader["area"].ToString().Trim();
                    string address = reader["address"].ToString().Trim();

                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(nicheng);
                        listViewItem.SubItems.Add(name);
                        listViewItem.SubItems.Add(phone);
                        listViewItem.SubItems.Add(tel);
                        listViewItem.SubItems.Add(province);
                        listViewItem.SubItems.Add(city);
                        listViewItem.SubItems.Add(area);
                        listViewItem.SubItems.Add(address);
                        //dataGridView1.Columns[1].HeaderText = "用户名";
                        //dataGridView1.Columns[2].HeaderText = "真实姓名";
                        //dataGridView1.Columns[3].HeaderText = "电话";
                        //dataGridView1.Columns[4].HeaderText = "手机号";
                        //dataGridView1.Columns[5].HeaderText = "省份";
                        //dataGridView1.Columns[6].HeaderText = "城市";
                        //dataGridView1.Columns[7].HeaderText = "区域";
                        //dataGridView1.Columns[8].HeaderText = "地址";
                        reader.Close();
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }
                    }
                }

                MessageBox.Show("采集结束");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK);     //显示错误信息
            }

        }

        public void select1()
        {
            try
            {
                string connetStr = "server=49.232.128.185;user=root;password=root;database=datas;"; //localhost不支持ssl连接时，最后一句一定要加！！！sslMode=none;
                MySqlConnection conn = new MySqlConnection(connetStr);
                conn.Open(); //连接数据库
                for (int i = 0; i < 99999; i++)
                {
                  

                    //MessageBox.Show("数据库连接成功", "提示", MessageBoxButtons.OK);
                    string searchStr = "SELECT * FROM jianli ORDER BY rand() LIMIT 1 ";   //student表中数据
                                                                                         
                    MySqlCommand cmd = new MySqlCommand(searchStr, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();



                    if (reader.Read())
                    {

                        string nicheng = reader["nicheng"].ToString().Trim();
                        string name = reader["name"].ToString().Trim();
                        string phone = reader["phone"].ToString().Trim();
                        string tel = reader["tel"].ToString().Trim();
                        string province = reader["province"].ToString().Trim();
                        string city = reader["city"].ToString().Trim();
                        string area = reader["area"].ToString().Trim();
                        string address = reader["address"].ToString().Trim();

                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(nicheng);
                        listViewItem.SubItems.Add(name);
                        listViewItem.SubItems.Add(phone);
                        listViewItem.SubItems.Add(tel);
                        listViewItem.SubItems.Add(province);
                        listViewItem.SubItems.Add(city);
                        listViewItem.SubItems.Add(area);
                        listViewItem.SubItems.Add(address);

                        reader.Close();
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }
                    }
                }
                MessageBox.Show("采集结束");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK);     //显示错误信息
            }


        }




        private void Jianli_Load(object sender, EventArgs e)
        {
            MessageBox.Show("未激活软件一次限制采集200个");
          
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

           

            if (jihuo == false)
            {

                string path = AppDomain.CurrentDomain.BaseDirectory;
                StreamReader sr = new StreamReader(path + "config.txt", Encoding.Default);
                string texts = sr.ReadToEnd();

                if (texts == "")
                {
                    sr.Close();
                    FileStream fs1 = new FileStream(path + "config.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1);
                    sw.WriteLine("1");
                    sw.Close();
                    fs1.Close();

                    Thread thread = new Thread(new ThreadStart(select));
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;

                }
                else 
                {
                    MessageBox.Show("未激活软件，试用次数已达上限,请激活后使用！联系QQ 583504945");
                }
              
                sr.Close();

                
            }
            
            else if (jihuo == true)
            {
                Thread thread = new Thread(new ThreadStart(select1));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
                //string path = AppDomain.CurrentDomain.BaseDirectory;
                //FileStream fs1 = new FileStream(path + "config.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                //StreamWriter sw = new StreamWriter(fs1);
                //sw.WriteLine("2");
                //sw.Close();
                //fs1.Close();
                //sw.Close();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(listView1), "Sheet1", true);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            string connetStr = "server=49.232.128.185;user=root;password=root;database=datas;"; //localhost不支持ssl连接时，最后一句一定要加！！！sslMode=none;
            MySqlConnection conn = new MySqlConnection(connetStr);
            try
            {
                conn.Open(); //连接数据库

                string Str = "select * from jhm where jihuoma='" + textBox1.Text.Trim() + "'  ";   //student表中数据
                MySqlCommand cmd = new MySqlCommand(Str, conn);
                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源



                if (reader.Read())
                {

                    // string jihuoma = reader["jihuoma"].ToString().Trim();
                    MessageBox.Show("激活成功");
                    jihuo = true;
                    textBox1.Text = "";
                }
                else
                {
                    MessageBox.Show("激活码错误");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK);     //显示错误信息
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            zanting = true;
        }
    }
}
