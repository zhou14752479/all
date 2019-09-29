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


                //DataColumn column = new DataColumn();
                //column.ColumnName = "序号";
                //column.AutoIncrement = true;
                //column.AutoIncrementSeed = 1;
                //column.AutoIncrementStep = 1;
                //ds.Tables[0].Columns.Add(column);
                //dataGridView1.Columns["序号"].DisplayIndex = 0;//调整列顺序



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
                    string title = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    string  taobao = dataGridView1.Rows[i].Cells[0].Value.ToString();


                    string url = "https://m.1688.com/offer_search/-6D7033.html?keywords="+ System.Web.HttpUtility.UrlEncode(title); ;

                    string html = method.GetUrl(url,"utf-8");


                    //MatchCollection aids = Regex.Matches(html, @"data-offer-id=""([\s\S]*?)""");
                    MatchCollection aids = Regex.Matches(html, @"<span><font color=red>([\s\S]*?)</font>");

                    if (aids.Count <= Convert.ToInt32(textBox2.Text))
                    {
                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());

                        listViewItem.SubItems.Add(title);
                        listViewItem.SubItems.Add(taobao);
                        listViewItem.SubItems.Add(aids.Count.ToString());
                        listViewItem.SubItems.Add("符合条件");
                    }
                    else
                    {
                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());

                        listViewItem.SubItems.Add(title);
                        listViewItem.SubItems.Add(taobao);
                        listViewItem.SubItems.Add(aids.Count.ToString());
                        listViewItem.SubItems.Add("不符合");
                    }
            
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    Thread.Sleep(Convert.ToInt32(textBox3.Text));

                }

                MessageBox.Show("采集完成");

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
            button1.Enabled = false;

            #region 通用验证

            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php", "utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == "15.15.15.15")
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

        private void Button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            zanting = true;
            button1.Enabled = true;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Process.Start(this.listView1.SelectedItems[0].SubItems[2].Text);
        }

        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void M1688_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            { //点取消的代码 
            }
        }

        private void 复制标题ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(this.listView1.SelectedItems[0].SubItems[1].Text);
        }
    }
}
