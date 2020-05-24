using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;


namespace link114
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
    

  
     


        public void qichacah()
        {
            try
            {
                for (int i = 1; i < dataGridView1.Rows.Count - 1; i++)
                {
                    string name = dataGridView1.Rows[i].Cells[0].Value.ToString().Trim();

                    string value = dataGridView1.Rows[i].Cells[1].Value.ToString().Trim();



                        string Html = method.GetUrl("https://xcx.qichacha.com/wxa/v1/finance/getTaxesList?unique=" + value + "&token="+textBox2.Text.Trim(), "utf-8");

                    if (Html.Contains("失效"))
                    {
                        MessageBox.Show("失效");
                    }


                        MatchCollection y = Regex.Matches(Html, @"""Year"":""([\s\S]*?)""");
                        MatchCollection c = Regex.Matches(Html, @"""Name"":""([\s\S]*?)""");
                        MatchCollection n = Regex.Matches(Html, @"""No"":""([\s\S]*?)""");




                    if (y.Count > 0)
                        {
                            for (int j = 0; j < y.Count; j++)
                            {


                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据 
                                lv1.SubItems.Add(y[j].Groups[1].Value);
                                lv1.SubItems.Add(c[j].Groups[1].Value);
                                lv1.SubItems.Add(n[j].Groups[1].Value);
                                lv1.SubItems.Add("1");

                            }

                        }

                        else
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据 
                            lv1.SubItems.Add("年份");
                            lv1.SubItems.Add("税号");
                            lv1.SubItems.Add(name);
                            lv1.SubItems.Add("0");
                        }



                        Thread.Sleep(1000);



                    
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


        }


        public string chauxun(string y,string n)
        {
           
            for (int j = 0; j < dataGridView2.Rows.Count-1; j++)
            {
                string year = dataGridView2.Rows[j].Cells[0].Value.ToString().Trim();
                string code = dataGridView2.Rows[j].Cells[1].Value.ToString().Trim();
                string name = dataGridView2.Rows[j].Cells[2].Value.ToString().Trim();

                if (year == y && name == n)
                {
                    
                    return code;
                }
            }
            return "";

        }
        /// <summary>
        /// 对比取值
        /// </summary>
        public void duibi()
        {
            try
            {
                for (int i = 1; i < dataGridView1.Rows.Count - 1; i++)
                {
                    string year = dataGridView1.Rows[i].Cells[7].Value.ToString().Trim();
                    string code = dataGridView1.Rows[i].Cells[8].Value.ToString().Trim();
                    string name = dataGridView1.Rows[i].Cells[9].Value.ToString().Trim();

                    if (chauxun(year, name) != "")
                    {
                        dataGridView1.Rows[i].Cells[8].Value = chauxun(year, name);
                        dataGridView1.Rows[i].Cells[11].Value = "1";
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells[11].Value = "0";
                    }
                  

                    
                       



                    
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


        }


        OpenFileDialog Ofile = new OpenFileDialog();
        DataSet ds = new DataSet();
        private void button5_Click(object sender, EventArgs e)
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
        #region  中间量获取
        public void run2()

        {
            try
            {

                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                   
                    string name = dataGridView1.Rows[i].Cells[0].Value.ToString().Trim();
                    string NAME= System.Web.HttpUtility.UrlEncode(dataGridView1.Rows[i].Cells[0].Value.ToString().Trim());
   
                    string url = "https://xcx.qichacha.com/wxa/v1/base/advancedSearchNew?searchKey="+NAME + "&token=3ebe551f6dbb16241cc62fda6aafc0da";
                    string html = method.GetUrl(url,"utf-8");

                   
                    Match company = Regex.Match(html, @"""KeyNo"":""([\s\S]*?)""");
                    if (company.Groups[1].Value == "")
                    {
                        MessageBox.Show("需要验证");
                    }

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(name);
                    lv1.SubItems.Add(company.Groups[1].Value);
                    lv1.SubItems.Add("1");
                    lv1.SubItems.Add("2");




                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    if (listView1.Items.Count > 2)
                    {
                        listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                    }
                    Thread.Sleep(1000);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion


        bool zanting = true;
        private void button1_Click(object sender, EventArgs e)
        {
            //Thread thread = new Thread(new ThreadStart(run2));
            //thread.Start();
            //Control.CheckForIllegalCrossThreadCalls = false;

            Thread thread = new Thread(new ThreadStart(duibi));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
            method.DataTableToExcel(method.DgvToTable(this.dataGridView1), "Sheet1", true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
           
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView2.Columns.Clear();


            this.ds.Tables.Clear();
            this.Ofile.FileName = "";
            this.dataGridView2.DataSource = "";
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
                this.dataGridView2.DataSource = this.ds.Tables[0];
                textBox2.Text = this.Ofile.FileName;
            }
        }
    }
}
