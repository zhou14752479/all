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

namespace main._2019_9
{
    public partial class m1688 : Form
    {
        public m1688()
        {
            InitializeComponent();
        }

        OpenFileDialog Ofile = new OpenFileDialog();


        DataSet ds = new DataSet();
        private void button5_Click(object sender, EventArgs e)
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

                textBox1.Text = this.Ofile.FileName;
            }
        }
        bool zanting = true;
        #region  主程序
        public void run()
        {
            try
            {



                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    string title = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    string  taobao = dataGridView1.Rows[i].Cells[1].Value.ToString();


                    string url = "https://m.1688.com/offer_search/-6D7033.html?keywords="+ System.Web.HttpUtility.UrlEncode(title); ;

                    string html = method.GetUrl(url,"utf-8");


                    MatchCollection aids = Regex.Matches(html, @"data-offer-id=""([\s\S]*?)""");

                    if (aids.Count < 6)
                    {
                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());

                        listViewItem.SubItems.Add(title);
                        listViewItem.SubItems.Add(taobao);
                    }
                    else
                    {
                        int all = 0;
                        MatchCollection alts = Regex.Matches(html, @"data-offer-id=""([\s\S]*?)alt=""([\s\S]*?)""");

                        int yuan = 0;
                        foreach (char c in title)
                        {
                            yuan++;
                        }

                            for (int j = 0; j < alts.Count; j++)
                        {
                           
                            int geshu = 0;
                            foreach (char c in title)
                            {
                                if (alts[j].Groups[2].Value.IndexOf(c) > -1)
                                    geshu++;

                            }


                            if ((yuan - geshu) < 3)
                            {
                                all++;
                            }


                        }

                        if (all < 6)
                        {
                            ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());

                            listViewItem.SubItems.Add(title);
                            listViewItem.SubItems.Add(taobao);
                        }
                       

                    }

                   
   


                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    Thread.Sleep(1000);

                }

            }




            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }





        #endregion

        private void m1688_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
