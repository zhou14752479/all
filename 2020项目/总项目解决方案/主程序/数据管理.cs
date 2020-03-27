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
using System.Threading;
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

    







        OpenFileDialog Ofile = new OpenFileDialog();

        DataSet ds = new DataSet();
        private void Button5_Click(object sender, EventArgs e)
        {
            if (suo == false)
            {
                MessageBox.Show("软件已锁定，请解锁");
                return;
            }



            dataGridView1.Columns.Clear();


            this.ds.Tables.Clear();
            this.Ofile.FileName = "";
            this.dataGridView1.DataSource = "";
            this.Ofile.ShowDialog();
            string fileName = this.Ofile.FileName;
            textBox1.Text = fileName;
            if (fileName.Trim().ToUpper().EndsWith("xls") || fileName.Trim().ToUpper().EndsWith("XLS"))//判断所要的?展名?型；
            {
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

                    string csvDir = openFileDialog1.FileName.ToString();
                }
            }






        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (suo == false)
            {
                MessageBox.Show("软件已锁定，请解锁");
                return;
            }
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                DataGridViewTextBoxColumn co0 = new DataGridViewTextBoxColumn();

                DataGridViewTextBoxColumn co1 = new DataGridViewTextBoxColumn();

                DataGridViewTextBoxColumn co2 = new DataGridViewTextBoxColumn();
                DataGridViewTextBoxColumn co3 = new DataGridViewTextBoxColumn();
                co0.HeaderText = "名称";
                co1.HeaderText = "手机";
                co2.HeaderText = "电话";
                co3.HeaderText = "地址";
               

                dataGridView1.Columns.Add(co0);
                dataGridView1.Columns.Add(co1);
                dataGridView1.Columns.Add(co2);
                dataGridView1.Columns.Add(co3);

                textBox2.Text = openFileDialog1.FileName;
                StreamReader sr = new StreamReader(openFileDialog1.FileName, Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                int index = 0;
                for (int i = 0; i < text.Length; i++)
                {

                    try
                    {
                        string[] value = text[i].Split(new string[] { "----" }, StringSplitOptions.None);

                        index = dataGridView1.Rows.Add();
                        dataGridView1.Rows[index].Cells[0].Value = value[0];
                        dataGridView1.Rows[index].Cells[1].Value = value[1];
                        dataGridView1.Rows[index].Cells[2].Value = value[2];
                        dataGridView1.Rows[index].Cells[3].Value = value[3];
                    }
                    catch 
                    {

                        continue;
                    }



                }
            }

        }
        /// <summary>
        /// 读取数据库
        /// </summary>
        /// 
        public void getdata()
        {
            try
            {

       

                string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\data58.db");
                mycon.Open();

                SQLiteDataAdapter sda = new SQLiteDataAdapter("Select * From tels group by tel", mycon);
                DataSet Ds = new DataSet();
                sda.Fill(Ds, "T_Class");

                this.dataGridView1.DataSource = Ds.Tables["T_Class"];
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

        public void run()
        {


            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                try
                {
                    string name = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    string tel = dataGridView1.Rows[i].Cells[2].Value.ToString();
                    string addr = dataGridView1.Rows[i].Cells[4].Value.ToString();

                    insertdata("INSERT INTO tels (name,tel,addr) VALUES( '" + name + "','" + tel + "','" + addr + "')");
                }
                catch 
                {
                    continue;
                }
            }

        }


        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

        }

        bool suo = false;

        private void 数据管理_Load(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (suo == false)
            {
                MessageBox.Show("软件已锁定，请解锁");
                return;
            }
            getdata();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                try
                {
                    string name = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    string tel = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    string addr = dataGridView1.Rows[i].Cells[3].Value.ToString();

                    insertdata("INSERT INTO tels (name,tel,addr) VALUES( '" + name + "','" + tel + "','" + addr + "')");
                }
                catch
                {
                    continue;
                }
            }
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            suo = false;
            MessageBox.Show("已锁定");
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "qwe147258")
            {
                MessageBox.Show("已成功解锁");
                suo = true;
            }
            else
            {
                MessageBox.Show("密码错误");
            }
        }
    }
}
