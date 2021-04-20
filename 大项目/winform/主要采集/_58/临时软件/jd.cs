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


namespace _58.临时软件
{
    public partial class jd : Form
    {
        public jd()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false; // 设置线程之间可以操作
        }

        OpenFileDialog Ofile = new OpenFileDialog();


        DataSet ds = new DataSet();


        public string run(string keyword)
        {
            string url = "https://search.jd.com/Search?keyword="+keyword+"&enc=utf-8";
            string html = Method.GetUrl(url);

            string rxg = @"result_count:'([\s\S]*?)'";

            Match match = Regex.Match(html,rxg);

            string value = match.Groups[1].Value.ToString();
            return value;

        }

        private void button2_Click(object sender, EventArgs e)
        {

            this.ds.Tables.Clear();
            this.Ofile.FileName = "";
            this.dataGridView1.DataSource = "";
            this.Ofile.ShowDialog();
            string fileName = this.Ofile.FileName;
            if (fileName != null && fileName != "")
            {
                OpenFileDialog ofd = new OpenFileDialog();//首先根据打开文件对话框，选择excel表格
                //ofd.Filter = "表格|*.xls|xlsx";//打开文件对话框筛选器
                string strPath;//文件完整的路径名
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        strPath = ofd.FileName;
                        string strCon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "data source=" + strPath + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'";//关键是红色区域
                        OleDbConnection Con = new OleDbConnection(strCon);//建立连接
                        string strSql = "select * from [Sheet1$]";//表名的写法也应注意不同，对应的excel表为sheet1，在这里要在其后加美元符号$，并用中括号
                        OleDbCommand Cmd = new OleDbCommand(strSql, Con);//建立要执行的命令
                        OleDbDataAdapter da = new OleDbDataAdapter(Cmd);//建立数据适配器
                        DataSet ds = new DataSet();//新建数据集
                        da.Fill(ds, "shyman");//把数据适配器中的数据读到数据集中的一个表中（此处表名为shyman，可以任取表名）
                                              //指定datagridview1的数据源为数据集ds的第一张表（也就是shyman表），也可以写ds.Table["shyman"]
                        dataGridView1.DataSource = ds.Tables[0];
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);//捕捉异常
                    }
                }
                textBox1.Text = this.Ofile.FileName;

            }
        }

        private void jd_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Method.DgvToTable(this.dataGridView1);
            Method.DataTableToExcel(Method.DgvToTable(this.dataGridView1), "Sheet1", true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label3.Text="程序已启动.......请勿重新点击！";
            Thread thread = new Thread(new ThreadStart(main));

            thread.Start();

            
        }


        public void main()
        {
            int i = 0;

            foreach (DataGridViewRow dgr in dataGridView1.Rows)
            {
                if (dgr.Cells[7].Value == null)
                {
                    break;
                }

                if (dgr.Cells[7].Value.ToString() == "-1")
                {
                    i++;
                }

               
            }
            int j = i - 1;

            foreach (DataGridViewRow dgr in dataGridView1.Rows)
            {
                if (dgr.Cells[7].Value == null)
                {
                    break;
                }
                //MessageBox.Show( dgr.Cells[7].Value.ToString());
                

                if (dgr.Cells[7].Value.ToString() == "-1")
                {
                    dgr.Cells[7].Value = run(dgr.Cells[0].Value.ToString());
                    label3.Text = "当前表格共有"+i+"个需要查询的数据-正在查询第" + dgr.Index+ "行-剩余"+j+"个需要查询！";
                    j--;

                    this.dataGridView1.CurrentCell = this.dataGridView1.Rows[dgr.Index].Cells[0];
                }
               

                Thread.Sleep(100);
            }

            MessageBox.Show("抓取完成！请点击导出结果");
        }

        //处理dataGridview的DataError事件，让它不做任何弹窗。
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
          //  MessageBox.Show("1");
        }
    }




}
