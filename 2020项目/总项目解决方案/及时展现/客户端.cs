using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace 及时展现
{
    public partial class 客户端 : Form
    {
        public 客户端()
        {
            InitializeComponent();
        }
        public static string keywords { get; set; }

        private void 客户端_Load(object sender, EventArgs e)
        {
            timer1.Start();

        }
        #region 导出文本文件
        /// <summary>
        /// 导出文本文件
        /// </summary>
        /// <param name="dgv">需要导出的表格</param>
        public static void Txt(DataGridView dgv) //另存新档按钮   导出成.txt文件
        {

            string path = AppDomain.CurrentDomain.BaseDirectory
;
            FileStream fs1 = new FileStream(path + "导出数据.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1, System.Text.Encoding.GetEncoding(-0));
                try
                {
                    //写内容
                    for (int j = 0; j < dgv.Rows.Count; j++)
                    {
                        string tempStr = string.Empty;
                        tempStr += dgv.Rows[j].Cells[0].Value+"   "+dgv.Rows[j].Cells[1].Value + "\r\n";  
                        sw.Write(tempStr);
                    }
                    sw.Close();
                    fs1.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
                finally
                {
                    sw.Close();
                fs1.Close();
            }
            
        }

        #endregion

        #region 获取公告

        public void getgg()
        {
            try
            {



                string constr = "Host =139.159.218.174;Database=data;Username=root;Password=123456";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select * from gonggao ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源


                if (reader.Read())
                {

                    textBox1.Text = reader["gonggao"].ToString().Trim();

                }
            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #region 获取内容
        public void run()
        {
            label1.Text = "正在获取，请稍后................";
            string[] keys = keywords.Split(new string[] { "," }, StringSplitOptions.None);
            string sql = "Select aname,bname From datas Where";
            foreach (string key in keys)
            {
                if (key != "")
                {
                    sql += " aname like \"" + key + "%\" or";
                }

            }
            sql = sql.Substring(0, sql.Length - 3);
            if (keywords == "全国")
            {
                sql = "Select aname,bname From datas";
            }

            string conn = "Host =139.159.218.174;Database=data;Username=root;Password=123456";
            // MySqlDataAdapter sda = new MySqlDataAdapter("Select aname,bname From datas Where aname like '" + keywords + "%' ", conn);
            MySqlDataAdapter sda = new MySqlDataAdapter(sql, conn);
            DataSet Ds = new DataSet();
            sda.Fill(Ds, "T_Class");
           
            this.dataGridView1.DataSource = Ds.Tables["T_Class"];
            label1.Text = "获取完成";
            
        }

        #endregion

        private void DataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //增加序号
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
         e.RowBounds.Location.Y,
        dataGridView1.RowHeadersWidth - 4,
        e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics,
            (e.RowIndex + 1).ToString(),
            dataGridView1.RowHeadersDefaultCellStyle.Font,
               rectangle,
                   dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                   TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
           
            //Thread thread = new Thread(new ThreadStart(run));
            //Control.CheckForIllegalCrossThreadCalls = false;
            //thread.Start();
            run();
            Thread thread1 = new Thread(new ThreadStart(getgg));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread1.Start();
            Txt(dataGridView1);
        }
    }
}
