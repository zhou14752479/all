using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pdf管理
{
    public partial class pdf管理 : Form
    {
        public pdf管理()
        {
            InitializeComponent();
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;//获取当前程序运行文件夹
        #region 绑定数据
        public void getdata()
        {
            listView1.Items.Clear();
            using (SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\data.db"))
            {
                DataSet ds = new DataSet();
                try
                {
                    mycon.Open();
                    SQLiteDataAdapter command = new SQLiteDataAdapter("SELECT * from datas", mycon);
                    command.Fill(ds, "T_Class");
                }
                catch (System.Data.SQLite.SQLiteException ex)
                {
                    throw new Exception(ex.Message);
                }
                DataTable dt = ds.Tables["T_Class"];　　　//返回的是一个DataSet
                int rowCount = dt.Rows.Count;
                for (int i = 0; i < rowCount; i++)
                {
                    string id = dt.Rows[i]["id"].ToString();
                    string a1 = dt.Rows[i]["pdfname"].ToString();
                    string a2 = dt.Rows[i]["path"].ToString();
                    string a3 = dt.Rows[i]["qjname"].ToString();
                    string a4 = dt.Rows[i]["gongneng"].ToString();
                    string a5 = dt.Rows[i]["type"].ToString();
                    string a6 = dt.Rows[i]["yinzi"].ToString();
                    string a7 = dt.Rows[i]["beizhu"].ToString();
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(a1);
                    lv1.SubItems.Add(a2);
                    lv1.SubItems.Add(a3);
                    lv1.SubItems.Add(a4);
                    lv1.SubItems.Add(a5);
                    lv1.SubItems.Add(a6);
                    lv1.SubItems.Add(a7);
                    lv1.SubItems.Add(id);


                }
            }
        }

        #endregion


        /// <summary>
        /// 获取指定字段
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string getkey(int id)
        {
            try
            {
               
                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\data.db");

                string sql = "SELECT * from datas where id=" + id;
                mycon.Open();

                SQLiteCommand cmd = new SQLiteCommand(sql, mycon);

                SQLiteDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源
                reader.Read();
                string keyword = reader["path"].ToString().Trim();
                reader.Close();
                mycon.Close();
                return keyword;
            }
            catch (SQLiteException ex)
            {
                ex.ToString();
                return "";
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
                MessageBox.Show("添加成功");
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.ToString());

            }

        }

        /// <summary>
        /// 删除
        /// </summary>
        public void deletedata(string sql)
        {
            try
            {

                string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\data.db");
                mycon.Open();

                SQLiteCommand cmd = new SQLiteCommand(sql, mycon);

                cmd.ExecuteNonQuery();  //执行sql语句
                mycon.Close();
                MessageBox.Show("删除成功");
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.ToString());

            }

        }

        /// <summary>
        /// 修改
        /// </summary>
        public void updatedata(string sql)
        {
            try
            {

                string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\data.db");
                mycon.Open();

                SQLiteCommand cmd = new SQLiteCommand(sql, mycon);

                cmd.ExecuteNonQuery();  //执行sql语句
                mycon.Close();
                MessageBox.Show("更新成功");
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.ToString());

            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
           
            this.Width = 850;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string a1 = textBox2.Text.Trim();
            string a2 = textBox3.Text.Trim();
            string a3 = textBox4.Text.Trim();
            string a4 = textBox5.Text.Trim();
            string a5 = textBox6.Text.Trim();
            if (button2.Text == "确认添加")
            {
                string filename = System.IO.Path.GetFileName(textBox1.Text);

                string filepath = textBox1.Text;
 
                string sql = "INSERT INTO datas(pdfname, path, qjname,gongneng,type,yinzi,beizhu) VALUES('" + filename + "','" + filepath + "','" + a1 + "','" + a2 + "','" + a3 + "','" + a4 + "','" + a5 + "')";
                insertdata(sql);
            }
            else if (button2.Text == "确认修改")
            {
                string id = listView1.SelectedItems[0].SubItems[8].Text;

                string sql = "UPDATE datas SET qjname= '" + a1 + "',gongneng= '" + a2 + "',type= '" + a3 + "',yinzi= '" + a4 + "',beizhu= '" + a5 + "' where id=" + id;
                updatedata(sql);
                this.Width = 530;
            }
            if (thread == null || !thread.IsAlive)
            {

                thread = new Thread(getdata);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

        }

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
            
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                string content = reader.ReadToEnd();


                reader.Close();
                response.Close();
                return content;



            }
            catch (System.Exception ex)
            {
                ex.ToString();

            }
            return "";
        }
        #endregion

        Thread thread;
        private void pdf管理_Load(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"pdfguanli"))
            {
                MessageBox.Show("");
                Environment.Exit(0);
                return;
            }

            #endregion
            this.Width = 530;
            if (thread == null || !thread.IsAlive)
            {
               
                thread = new Thread(getdata);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
           
           
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string id = listView1.SelectedItems[0].SubItems[8].Text;
            string sql = "DELETE FROM datas where id="+id;
            deletedata(sql);

            if (thread == null || !thread.IsAlive)
            {
               
                thread = new Thread(getdata);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            button2.Text = "确认添加";
            this.Width = 530;
        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           textBox2.Text = listView1.SelectedItems[0].SubItems[3].Text;
            textBox3.Text = listView1.SelectedItems[0].SubItems[4].Text;
            textBox4.Text = listView1.SelectedItems[0].SubItems[5].Text;
            textBox5.Text = listView1.SelectedItems[0].SubItems[6].Text;
            textBox6.Text = listView1.SelectedItems[0].SubItems[7].Text;
            button2.Text = "确认修改";
            this.Width = 850;
        }

        private void 打开文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filepath = listView1.SelectedItems[0].SubItems[2].Text;
           System.Diagnostics.Process.Start(filepath);
        }

        private void 打开所在文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filepath = listView1.SelectedItems[0].SubItems[2].Text;
           string dic= System.IO.Path.GetDirectoryName(filepath);
            System.Diagnostics.Process.Start(dic);
        }

        private void pdf管理_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
               // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }
    }
}
