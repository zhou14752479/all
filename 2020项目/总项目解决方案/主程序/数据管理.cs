using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序
{
    public partial class 数据管理 : Form
    {
        public 数据管理()
        {
            InitializeComponent();
        }
  

        private void Button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();//首先根据打开文件对话框，选择excel表格
            ofd.Filter = "表格|*.xls";//打开文件对话框筛选器
            string strPath;//文件完整的路径名
           
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    strPath = ofd.FileName;
                    textBox1.Text = strPath;
                    string strCon = "provider=microsoft.jet.oledb.4.0;data source=" + strPath + ";extended properties=excel 8.0";//关键是红色区域
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




        }

        private void Button3_Click(object sender, EventArgs e)
        {
            //if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    DataGridViewTextBoxColumn co0 = new DataGridViewTextBoxColumn();

            //    DataGridViewTextBoxColumn co1 = new DataGridViewTextBoxColumn();

            //    DataGridViewTextBoxColumn co2 = new DataGridViewTextBoxColumn();
            //    co0.HeaderText = "名称";
            //    co1.HeaderText = "电话";
            //    co2.HeaderText = "地址";

            //    dataGridView1.Columns.Add(co0);
            //    dataGridView1.Columns.Add(co1);
            //    dataGridView1.Columns.Add(co2);

            //    StreamReader sr = new StreamReader(openFileDialog1.FileName.Replace("xls","txt"), Encoding.Default);
            //    //一次性读取完 
            //    string texts = sr.ReadToEnd();
            //    string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            //    int index = 0;
            //    for (int i = 0; i < text.Length; i++)
            //    {
            //        MessageBox.Show(text[i]);
            //        string[] value = text[i].Split(new string[] { "----" }, StringSplitOptions.None);

            //        index = dataGridView1.Rows.Add();
            //        dataGridView1.Rows[index].Cells[0].Value = value[0];
            //        dataGridView1.Rows[index].Cells[1].Value = value[1];
            //        dataGridView1.Rows[index].Cells[2].Value = value[2];



            //    }
            //}

        }
        /// <summary>
        /// 读取数据库
        /// </summary>
        public void getdata()
        {
            try
            {

                string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\data58.db");
                mycon.Open();

                SQLiteCommand cmd = new SQLiteCommand("select tel from tels", mycon);
                SQLiteDataReader rdr = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(rdr);

                for (int i = 0; i < table.Rows.Count; i++) // 遍历行
                {

                    

                }
                mycon.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.ToString());

            }

        }
        /// <summary>
        /// 插入数据库
        /// </summary>
        public void insertdata(string sql)
        {
            try
            {

                string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\data58.db");
                mycon.Open();

                SQLiteCommand cmd = new SQLiteCommand(sql, mycon);

                cmd.ExecuteNonQuery();  //执行sql语句
                mycon.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.ToString());

            }

        }


        private void Button1_Click(object sender, EventArgs e)
        {

        }

        private void 数据管理_Load(object sender, EventArgs e)
        {

        }
    }
}
