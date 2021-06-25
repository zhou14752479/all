using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 订单匹配
{
    public partial class 订单表格匹配 : Form
    {
        public 订单表格匹配()
        {
            InitializeComponent();
        }
        #region 读取datatable到ListView2
        public static void ShowDataInListView2(DataTable dt, ListView lst)
        {

            lst.Clear();
            //   lst.View=System.Windows.Forms.View.Details;
            lst.AllowColumnReorder = true;//用户可以调整列的位置
            lst.GridLines = true;

            int RowCount, ColCount, i, j;
            DataRow dr = null;

            if (dt == null) return;
            RowCount = dt.Rows.Count;
            ColCount = dt.Columns.Count;
         
            for (i = 0; i < ColCount; i++)
            {
                lst.Columns.Add(dt.Columns[i].Caption.Trim(), lst.Width / ColCount, System.Windows.Forms.HorizontalAlignment.Left);
            }

            if (RowCount == 0) return;
            for (i = 0; i < RowCount; i++)
            {
                dr = dt.Rows[i];
                lst.Columns[0].Width = -2;
                lst.Columns[1].Width = -2;
                lst.Columns[2].Width = -2;
                // lst.Items.Add(dr[0].ToString().Trim());


                lst.Items.Add("");
                //lst.Items[i].SubItems.Add("");

                for (j = 1; j < ColCount; j++)
                {

                    lst.Columns[j].Width = -2;
                    lst.Items[i].SubItems.Add((string)dr[j].ToString().Trim());
                }
            }
        }

        #endregion
        #region 读取datatable到ListView
        public static void ShowDataInListView(DataTable dt, ListView lst)
        {
          
            lst.Clear();
            //   lst.View=System.Windows.Forms.View.Details;
            lst.AllowColumnReorder = true;//用户可以调整列的位置
            lst.GridLines = true;

            int RowCount, ColCount, i, j;
            DataRow dr = null;

            if (dt == null) return;
            RowCount = dt.Rows.Count;
            ColCount = dt.Columns.Count;
            //添加列标题名
            lst.Columns.Add("是否刷单", lst.Width / ColCount, System.Windows.Forms.HorizontalAlignment.Left);
            lst.Columns.Add("是否退款", lst.Width / ColCount, System.Windows.Forms.HorizontalAlignment.Left);
            for (i = 0; i < ColCount; i++)
            {
                lst.Columns.Add(dt.Columns[i].Caption.Trim(), lst.Width / ColCount, System.Windows.Forms.HorizontalAlignment.Left);
            }

            if (RowCount == 0) return;
            for (i = 0; i < RowCount; i++)
            {
                dr = dt.Rows[i];
                lst.Columns[0].Width = -2;
                lst.Columns[1].Width = -2;
                lst.Columns[2].Width = -2;
                // lst.Items.Add(dr[0].ToString().Trim());


                lst.Items.Add("");
                lst.Items[i].SubItems.Add("");
              
                for (j = 0; j < ColCount; j++)
                {
                 
                    lst.Columns[j].Width = -2;
                    lst.Items[i].SubItems.Add((string)dr[j].ToString().Trim());
                }
            }
        }

        #endregion
        DataTable dt1;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            //  openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox1.Text = openFileDialog1.FileName;
              dt1 = method.ExcelToDataTable(textBox1.Text, true);
                ShowDataInListView(dt1, listView1);
            }
        }
        public bool panduan(string orderno)
        {
            try
            {
                string sql = "select * from datas where 订单编号=" + orderno;
                string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\data.db");
                mycon.Open();

                SQLiteCommand cmd = new SQLiteCommand(sql, mycon);

                SQLiteDataReader reader = cmd.ExecuteReader();
              
                bool result = false;
                while (reader.Read())
                {
                    result = true;
                }
                mycon.Close();
              
                return result;
              
 
                   
            }
            catch (SQLiteException ex)
            {
               
                MessageBox.Show(ex.ToString());
                return false;
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

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\data.db");
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

        /// <summary>
        /// 查询数据库
        /// </summary>
        public void  chaxundata()
        {
            try
            {
                listView1.Items.Clear();
                listView1.Columns.Clear();

                string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\data.db");
                mycon.Open();

                SQLiteDataAdapter mAdapter = new SQLiteDataAdapter("select * from datas", mycon);
                DataTable dt = new DataTable();
                mAdapter.Fill(dt);
                mycon.Close();
                ShowDataInListView2(dt,listView1);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.ToString());

            }

        }


        public void run2()
        {
            try
            {
                if (textBox2.Text == "")
                {
                    MessageBox.Show("请导入表格");
                    return;
                }
               
                DataTable dt2 = method.ExcelToDataTable(textBox2.Text, true);
                List<string> lists = new List<string>();
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    DataRow dr = dt2.Rows[i];
                    lists.Add(dr[1].ToString().Trim());
                }

                for (int a = 0; a < listView1.Items.Count; a++)
                {
                    if (lists.Contains(listView1.Items[a].SubItems[2].Text.Trim()))
                    {
                        listView1.Items[a].SubItems[0].Text = "刷";
                    }

                }
                MessageBox.Show("完成");
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        public void run3()
        {
            try
            {
                if (textBox3.Text == "")
                {
                    MessageBox.Show("请导入表格");
                    return;
                }

                DataTable dt3= method.ExcelToDataTable(textBox3.Text, true);
               Dictionary<string,string> dics = new Dictionary<string, string>();
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    DataRow dr = dt3.Rows[i];
                    if (!dics.ContainsKey(dr[1].ToString()))
                    {
                        dics.Add(dr[1].ToString().Trim(), dr[3].ToString().Trim());
                    }
                }

                for (int a = 0; a < listView1.Items.Count; a++)
                {
                    if (dics.ContainsKey(listView1.Items[a].SubItems[33].Text.Trim()))
                    {
                        listView1.Items[a].SubItems[8].Text = dics[listView1.Items[a].SubItems[33].Text.Trim()];
                    }

                }
                MessageBox.Show("完成");

            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        public void run4()
        {
            try
            {
                if (textBox4.Text == "")
                {
                    MessageBox.Show("请导入表格");
                    return;
                }

                DataTable dt4 = method.ExcelToDataTable(textBox4.Text, true);
                List<string> lists = new List<string>();
                for (int i = 0; i < dt4.Rows.Count; i++)
                {
                    DataRow dr = dt4.Rows[i];
                    lists.Add(dr[1].ToString().Trim());
                }

                for (int a = 0; a < listView1.Items.Count; a++)
                {
                    if (lists.Contains(listView1.Items[a].SubItems[2].Text.Trim()))
                    {
                        listView1.Items[a].SubItems[1].Text = "退";
                    }

                }
                MessageBox.Show("完成");
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        Thread thread;
        private void button5_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run2);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            //  openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox2.Text = openFileDialog1.FileName;
               
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            //  openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox4.Text = openFileDialog1.FileName;

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run4);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run3);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            //  openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox3.Text = openFileDialog1.FileName;

            }
        }

        public void savedata()
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("列表数据为空");
                return;
            }


            for ( int i = 0; i < listView1.Items.Count; i++)
            {
               string a0=listView1.Items[i].SubItems[0].Text;
                string a1 = listView1.Items[i].SubItems[1].Text.Trim();
                string a2 = listView1.Items[i].SubItems[2].Text.Trim();
                string a3 = listView1.Items[i].SubItems[3].Text.Trim();
                string a4 = listView1.Items[i].SubItems[4].Text.Trim();
                string a5 = listView1.Items[i].SubItems[5].Text.Trim();
                string a6 = listView1.Items[i].SubItems[6].Text.Trim();
                string a7 = listView1.Items[i].SubItems[7].Text.Trim();
                string a8 = listView1.Items[i].SubItems[8].Text.Trim();
                string a9 = listView1.Items[i].SubItems[9].Text.Trim();
                string a10 = listView1.Items[i].SubItems[10].Text.Trim();
                string a11 = listView1.Items[i].SubItems[11].Text.Trim();
                string a12 = listView1.Items[i].SubItems[12].Text.Trim();
                string a13 = listView1.Items[i].SubItems[13].Text.Trim();
                string a14 = listView1.Items[i].SubItems[14].Text.Trim();
                string a15 = listView1.Items[i].SubItems[15].Text.Trim();
                string a16 = listView1.Items[i].SubItems[16].Text.Trim();
                string a17 = listView1.Items[i].SubItems[17].Text.Trim();
                string a18 = listView1.Items[i].SubItems[18].Text.Trim();
                string a19 = listView1.Items[i].SubItems[19].Text.Trim();
                string a20 = listView1.Items[i].SubItems[20].Text.Trim();
                string a21 = listView1.Items[i].SubItems[21].Text.Trim();
                string a22 = listView1.Items[i].SubItems[22].Text.Trim();
                string a23 = listView1.Items[i].SubItems[23].Text.Trim();
                string a24 = listView1.Items[i].SubItems[24].Text.Trim();
                string a25 = listView1.Items[i].SubItems[25].Text.Trim();
                string a26 = listView1.Items[i].SubItems[26].Text.Trim();
                string a27 = listView1.Items[i].SubItems[27].Text.Trim();
                string a28 = listView1.Items[i].SubItems[28].Text.Trim();
                string a29 = listView1.Items[i].SubItems[29].Text.Trim();
                string a30 = listView1.Items[i].SubItems[30].Text.Trim();
                string a31 = listView1.Items[i].SubItems[31].Text.Trim();
                string a32 = listView1.Items[i].SubItems[32].Text.Trim();
                string a33 = listView1.Items[i].SubItems[33].Text.Trim();
                string a34 = listView1.Items[i].SubItems[34].Text.Trim();
                string a35 = listView1.Items[i].SubItems[35].Text.Trim();
                string a36 = listView1.Items[i].SubItems[36].Text.Trim();
                string a37 = listView1.Items[i].SubItems[37].Text.Trim();
                string a38 = listView1.Items[i].SubItems[38].Text.Trim();
                string a39 = listView1.Items[i].SubItems[39].Text.Trim();
                string a40 = listView1.Items[i].SubItems[40].Text.Trim();
                string a41 = listView1.Items[i].SubItems[41].Text.Trim();
                string a42 = listView1.Items[i].SubItems[42].Text.Trim();
                string a43 = listView1.Items[i].SubItems[43].Text.Trim();
                string a44 = listView1.Items[i].SubItems[44].Text.Trim();

                if (panduan(a2) == true)
                {
                    label5.Text = "保数据库已经包含跳过此条数据：" + a2;
                    continue;
                }

                label5.Text = "保存数据到数据库成功："+a2;
                string sql = "INSERT INTO datas VALUES( '" + a0+ "'" +
                    ",'" + a1 + "'" +
                    ",'" + a2 + "'" +
                    ",'" + a3 + "'" +
                    ",'" + a4 + "'" +
                      ",'" +a5 + "'" +
                    ",'" + a6 + "'" +
                    ",'" + a7 + "'" +
                    ",'" + a8 + "'" +
                      ",'" +a9 + "'" +
                    ",'" + a10 + "'" +
                    ",'" + a11 + "'" +
                    ",'" + a12 + "'" +
                      ",'" + a13 + "'" +
                    ",'" + a14 + "'" +
                    ",'" + a15 + "'" +
                    ",'" + a16 + "'" +
                      ",'" + a17 + "'" +
                    ",'" + a18 + "'" +
                    ",'" + a19 + "'" +
                    ",'" + a20 + "'" +
                      ",'" + a21 + "'" +
                    ",'" + a22 + "'" +
                    ",'" + a23 + "'" +
                    ",'" + a24+ "'" +
                      ",'" + a25 + "'" +
                    ",'" + a26+ "'" +
                    ",'" + a27 + "'" +
                    ",'" + a28 + "'" +
                      ",'" + a29 + "'" +
                    ",'" + a30 + "'" +
                    ",'" + a31 + "'" +
                    ",'" + a32 + "'" +
                      ",'" + a33 + "'" +
                    ",'" + a34 + "'" +
                    ",'" + a35 + "'" +
                    ",'" + a36 + "'" +
                      ",'" + a37 + "'" +
                    ",'" + a38 + "'" +
                    ",'" + a39 + "'" +
                    ",'" + a40 + "'" +
                      ",'" + a41 + "'" +
                    ",'" + a42 + "'" +
                    ",'" + a43 + "'" +
                 
            ", '" + a44+ "')";
            insertdata(sql);
            }
            
            

        }


       

        private void button8_Click(object sender, EventArgs e)
        {
          
            Thread thread = new Thread(savedata);
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;


        }

        private void 订单表格匹配_Load(object sender, EventArgs e)
        {
          Thread  thread = new Thread(chaxundata);
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(chaxundata);
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
