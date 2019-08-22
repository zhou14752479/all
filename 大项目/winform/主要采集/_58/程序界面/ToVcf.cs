using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;


namespace _58
{
    public partial class ToVcf : Form
    {
        public ToVcf()
        {
            InitializeComponent();
        }

        OpenFileDialog Ofile=new OpenFileDialog();
        

        DataSet ds = new DataSet();

        public void run()

        {
            if (this.Ofile.FileName == null || this.Ofile.FileName == "")
            {
                MessageBox.Show("没有选择EXCEL文件！");
                return;
            }
            if (this.ds.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("你选择的文件是空文件！");
                return;
            }


            int num = 0;
            for (int i = 0; i < this.ds.Tables[0].Columns.Count; i++)
            {
                if (this.ds.Tables[0].Columns[i].ColumnName == "姓名")
                {
                    num++;
                }
                if (this.ds.Tables[0].Columns[i].ColumnName == "电话")
                {
                    num++;
                }
                if (this.ds.Tables[0].Columns[i].ColumnName == "电话1")
                {
                    num++;
                }

            }
            if (num < 3)
            {
                MessageBox.Show("excel文件不符合要求，具体请参阅帮助！");
            }
            else
            {
                string text = this.Ofile.FileName + ".vcf";
                if (File.Exists(text))
                {
                    if (MessageBox.Show("“" + text + "”已经存在，是否删除它？", "确认", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        return;
                    }
                    File.Delete(text);
                }
                UTF8Encoding encoding = new UTF8Encoding(false);
                StreamWriter streamWriter = new StreamWriter(text, false, encoding);
                for (int i = 0; i < this.ds.Tables[0].Rows.Count; i++)
                {
                    streamWriter.WriteLine("BEGIN:VCARD");
                    streamWriter.WriteLine("VERSION:3.0");
                    if (this.ds.Tables[0].Rows[i]["姓名"].ToString().Trim() != "")
                    {
                        streamWriter.WriteLine("N;CHARSET=UTF-8:" + this.ds.Tables[0].Rows[i]["姓名"].ToString().Trim());
                        streamWriter.WriteLine("FN;CHARSET=UTF-8:" + this.ds.Tables[0].Rows[i]["姓名"].ToString().Trim());
                        if (this.ds.Tables[0].Columns.IndexOf("所在分组") != -1)
                        {
                            if (this.ds.Tables[0].Rows[i]["所在分组"].ToString().Trim() != "")
                            {
                                streamWriter.WriteLine("CATEGORIES:" + this.ds.Tables[0].Rows[i]["所在分组"].ToString().Trim());
                            }
                        }
                        if (this.ds.Tables[0].Rows[i]["电话"].ToString().Trim() != "")
                        {
                            streamWriter.WriteLine("TEL;TYPE=CELL:" + this.ds.Tables[0].Rows[i]["电话"].ToString().Trim());
                        }
                        if (this.ds.Tables[0].Rows[i]["电话1"].ToString().Trim() != "")
                        {
                            streamWriter.WriteLine("TEL;TYPE=CELL:" + this.ds.Tables[0].Rows[i]["电话1"].ToString().Trim());
                        }

                        streamWriter.WriteLine("END:VCARD");
                    }
                }
                streamWriter.Flush();
                streamWriter.Close();
                MessageBox.Show("生成成功！文件名是：" + text);
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {

            #region 通用验证

            bool value = false;
            string html = Method.GetUrl("http://acaiji.com/success/ip.php");
            string localip = Method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == "9.9.9.9")
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {
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


        private void button2_Click(object sender, EventArgs e)
        {
            //dataGridView1.DataSource = null; //每次打开清空内容
            //DataTable dt = Method.getData().Tables[0];
            //dataGridView1.DataSource = dt;

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

        private void ToVcf_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://wpa.qq.com/msgrd?v=3&uin=852266010&site=qq&menu=yes");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start( "http://wpa.qq.com/msgrd?v=3&uin=852266010&site=qq&menu=yes");
        }
    }
}
