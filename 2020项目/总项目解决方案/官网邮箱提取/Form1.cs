using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;
using MySql.Data.MySqlClient;

namespace 官网邮箱提取
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {


        }
        OpenFileDialog Ofile = new OpenFileDialog();


        DataSet ds = new DataSet();

      
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();


            this.ds.Tables.Clear();
            this.Ofile.FileName = "";
            this.dataGridView1.DataSource = "";
            this.Ofile.ShowDialog();
            string fileName = this.Ofile.FileName;
            if (fileName != null && fileName != "")
            {
                string connectionString = " Provider = Microsoft.Jet.OLEDB.4.0 ; Data Source = " + fileName + "; Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
                OleDbConnection oleDbConnection = new OleDbConnection(connectionString);
                oleDbConnection.Open();
                DataTable oleDbSchemaTable = oleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[]
                {
                    null,
                    null,
                    null,
                    "TABLE"
                });
                string str = oleDbSchemaTable.Rows[0]["TABLE_NAME"].ToString();
                string selectCommandText = "select * from [" + str + "]";
                OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(selectCommandText, oleDbConnection);
                oleDbDataAdapter.Fill(this.ds, "temp");
                oleDbConnection.Close();
                this.dataGridView1.DataSource = this.ds.Tables[0];


                textBox1.Text = this.Ofile.FileName;
            }




        }

        bool zanting = true;

        public void run()
        {
           

            try
            {


                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    string coname = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    string name = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    string url = "http://"+dataGridView1.Rows[i].Cells[2].Value.ToString().Replace("#",".").Replace("http://","").Replace("/", "");
                    string html = method.GetUrl(url, "utf-8");
                   
                    Match lianxi = Regex.Match(html, @"<a.*>.*联系.*");
                    string lxUrl = lianxi.Groups[0].Value;
                    if(!lxUrl.Contains("www"))
                {
                    Match contact = Regex.Match(lianxi.Groups[0].Value, @"href=""([\s\S]*?)""");
                    lxUrl = url + contact.Groups[1].Value;
                }
                    


                    textBox3.Text += "正在抓取："+lxUrl+"\r\n";
                    string strhtml = method.GetUrl(lxUrl, "utf-8");
                    Match  tel = Regex.Match(strhtml, @"[1][3,4,5,7,8,9][0-9]{9}|0\d{2,3}-\d{7,8}");
                    Match tel2 = Regex.Match(strhtml, @"0\d{2,3}-\d{7,8}");
                   
                  
                    MatchCollection mails = Regex.Matches(strhtml, @"[0-9a-zA-Z_]{0,19}@[0-9a-zA-Z]{1,13}\.[com,cn,net]{1,3}");
                    ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());

                    StringBuilder sbmail = new StringBuilder();
                    foreach (Match mail in mails)
                    {
                        sbmail.Append(mail.Groups[0].Value+"，");
                    }





                      
                        listViewItem.SubItems.Add(coname);
                        listViewItem.SubItems.Add(name);
                    listViewItem.SubItems.Add(url);
                    listViewItem.SubItems.Add(tel.Groups[0].Value+","+ tel2.Groups[0].Value);
                    listViewItem.SubItems.Add(sbmail.ToString());



                    while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                      
                    
                }

                MessageBox.Show("采集完成");

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            string constr = "Host =47.99.68.92;Database=vip_database;Username=root;Password=zhoukaige00.@*.";
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();

            MySqlCommand cmd = new MySqlCommand("select * from vip where username='官网邮箱提取'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

            MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

            if (reader.Read())
            {

                string password = reader["password"].ToString().Trim();

                if (password != "官网邮箱提取")

                {
                    MessageBox.Show("验证失败");

                    Environment.Exit(0);
                }

                else
                {
                    Thread thread = new Thread(new ThreadStart(run));
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }


            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
