using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 常用代码查询
{
    public partial class 常用代码查询 : Form
    {
        public 常用代码查询()
        {
            InitializeComponent();
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
        public DataTable chaxundata(string sql)
        {
            try
            {
                string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\data.db");
                mycon.Open();

                SQLiteDataAdapter mAdapter = new SQLiteDataAdapter(sql, mycon);
                DataTable dt = new DataTable();
                mAdapter.Fill(dt);
                mycon.Close();
                return dt;
               
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
                

            }

        }

        public void getall(string sql)
        {
            listView1.Items.Clear();
            try
            {
                
                DataTable dt = chaxundata(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    string id = dr[0].ToString().Trim();
                    string title = dr[1].ToString().Trim();
                    ListViewItem lv1 = listView1.Items.Add(id); //使用Listview展示数据
                    lv1.SubItems.Add(title);

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void search_btn_Click(object sender, EventArgs e)
        {
            string sql = "select * from datas instr(title, '" + textBox1.Text.Trim() + "') > 0 ";
            getall(sql);
        }

        private void 常用代码查询_Load(object sender, EventArgs e)
        {
            string sql = "select * from datas";
            getall( sql);
        }

        private void add_btn_Click(object sender, EventArgs e)
        {

        }

        private void all_btn_Click(object sender, EventArgs e)
        {
            string sql = "select * from datas";
            getall(sql);
        }
    }
}
