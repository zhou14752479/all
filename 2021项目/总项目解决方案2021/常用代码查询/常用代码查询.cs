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

        /// <summary>
        /// 查询字段
        /// </summary>
        public string chaxunvalue(string sql)
        {
            try
            {
                string value = "";
                string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

                using (SQLiteConnection con = new SQLiteConnection("Data Source=" + path + "\\data.db"))
                {
                    con.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = string.Format(sql);
                        using (SQLiteDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (dr.Read())
                            {
                                value = dr["body"].ToString();
                               
                            }

                        }

                    }
                    con.Close();
                }
                
                return value;

            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.ToString());
                return null;


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

                int status = cmd.ExecuteNonQuery();  //执行sql语句
                if (status > 0)
                {
                    MessageBox.Show("添加成功");
                    title_text.Text = "";
                    body_text.Text = "";
                }
                mycon.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.ToString());

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
            string sql = "select * from datas where instr(title,'" + textBox1.Text.Trim() + "') > 0 ";
           
            getall(sql);
        }

        private void 常用代码查询_Load(object sender, EventArgs e)
        {
            string sql = "select * from datas";
            getall( sql);
        }

        private void add_btn_Click(object sender, EventArgs e)
        {
            if (title_text.Text != "" && body_text.Text != "")
            {
                string sql = "INSERT INTO datas(title,body)VALUES('" + @title_text.Text.Trim() + "','" + @body_text.Text.Trim() + "')";
                insertdata(sql);
            }
            else
            {
                MessageBox.Show("输入为空");
            }
        }

        private void all_btn_Click(object sender, EventArgs e)
        {
            string sql = "select * from datas";
            getall(sql);
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;
            string id = listView1.SelectedItems[0].SubItems[0].Text;
            string sql = "select * from datas where id= "+id;
          string value=  chaxunvalue(sql);
            result_text.Text = value;
            tabControl1.SelectedIndex = 0;
        }
    }
}
