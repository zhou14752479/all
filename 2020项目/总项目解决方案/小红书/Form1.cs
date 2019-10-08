using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 小红书
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        
        bool zanting = true;

        public string IP = "";
        
        #region 小红书
        public void run()
        {
            try

            {

                for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
                {
                    string url = dataGridView1.Rows[i].Cells[2].Value.ToString();
                     
                    string html = method.GetUrlwithIP(url,IP);
                  
                    
                    if (html.Contains("小红书登录"))
                    {
                        getIp();
                        MessageBox.Show(IP);
                        html = method.GetUrlwithIP(url,this.IP);
                    }

                   // this.index = this.skinDataGridView1.Rows.Add();
                    Match a1 = Regex.Match(html, @"{""nickname"":""([\s\S]*?)""");
                    Match a2 = Regex.Match(html, @"发布于([\s\S]*?)<");
                    
                    Match a3 = Regex.Match(html, @"},""likes"":([\s\S]*?),");
                    Match a4 = Regex.Match(html, @",""comments"":([\s\S]*?),");
                    Match a5 = Regex.Match(html, @"""collects"":([\s\S]*?),");


                        dataGridView1.Rows[i].Cells[3].Value = a1.Groups[1].Value;
                    dataGridView1.Rows[i].Cells[4].Value = a2.Groups[1].Value;
                    dataGridView1.Rows[i].Cells[5].Value = a3.Groups[1].Value;
                    dataGridView1.Rows[i].Cells[6].Value = a4.Groups[1].Value;
                    dataGridView1.Rows[i].Cells[7].Value = a5.Groups[1].Value;

                    //this.dataGridView1.CurrentCell = this.dataGridView1.Rows[i].Cells[0];  //让datagridview滚动到当前行

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                   

                }
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用验证

            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php", "utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == "19.19.19.19")
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {

                button1.Enabled = false;
                getIp();

                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            }
            else
            {
                MessageBox.Show("IP不符");

            }
            #endregion
           
           
        }


        #region  代理iP

        public void getIp()
        {
            string ahtml = method.GetUrl(textBox3.Text, "utf-8");
            this.IP = ahtml.Trim();
           
        }
        #endregion

        private void button5_Click(object sender, EventArgs e)
        {
          
            method.DataTableToExcel(method.DgvToTable(this.dataGridView1), "Sheet1", true);
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            zanting = true;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
           dataGridView1.Rows.Clear();
        }
        OpenFileDialog Ofile = new OpenFileDialog();


        DataSet ds = new DataSet();

        private void Button6_Click(object sender, EventArgs e)
        {
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
            }

            textBox1.Text = fileName;
        }

        private void Button7_Click(object sender, EventArgs e)
        {   
            MessageBox.Show("选择网站下方【生成API链接】其他不变，然后复制链接");
            System.Diagnostics.Process.Start("http://h.zhimaruanjian.com/getapi/");
        }
    }
}
