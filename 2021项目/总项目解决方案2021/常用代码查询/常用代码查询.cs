using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 常用代码查询
{
    public partial class 常用代码查询 : Form
    {
        [DllImport("ocr.dll")]
        public static extern int init();

        [DllImport("ocr.dll")]
        public static extern int ocr(byte[] bin, int binlength);

      

        public 常用代码查询()
        {
            InitializeComponent();
            
        }


        #region 根据图片地址获取图片的二进制流
        /// <summary>
        /// 根据图片地址获取图片的二进制流
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public static byte[] Getbyte(string imageUrl, string COOKIE)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(imageUrl);
            request.Proxy = null;
            request.Accept = "*/*";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Referer = "";
            request.Timeout = 30000;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0)";
            request.Method = "GET";
            request.Headers.Add("Cookie", COOKIE);
            request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version10;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK)
                return null;
            Stream resStream = response.GetResponseStream();
            Bitmap bmp = new Bitmap(resStream);
            response.Close();
            request.Abort();

            using (MemoryStream curImageStream = new MemoryStream())
            {
                bmp.Save(curImageStream, System.Drawing.Imaging.ImageFormat.Png);
                curImageStream.Flush();
                byte[] bmpBytes = curImageStream.ToArray();
                return bmpBytes;
            }
        }

        #endregion

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


       // public OCRUtils() { Dll.instance.init(); }
       // public interface Dll extends Library { Dll instance = (Dll) Native.load("ocr", Dll.class); 
        //声明 dll中方法有哪些 public void init(); 
        /** * @param z_bin 图片的字节集 * @param ok 文件长度 * @return */ 
        //public String ocr(byte[] z_bin, int ok); }
    //public String ocr(String base64) { byte[] decode = Base64.getDecoder().decode(base64); String code = Dll.instance.ocr(decode, decode.length); return code; }
       // public String ocrByUrl(String url, String cookie)
       // { String base64 = this.requestUrlToBase64(url, cookie); return this.ocr(base64); }


        //    public String ocr(String base64)
        //{ byte[] decode = Base64.getDecoder().decode(base64); String code = ocr(decode, decode.Length); return code; }
        private void button2_Click(object sender, EventArgs e)
        {
            //init();
            byte[] bytes = Getbyte("https://img1.baidu.com/it/u=4160572592,2121035394&fm=26&fmt=auto&gp=0.jpg","");
         int value=  ocr(bytes,bytes.Length);
           
            MessageBox.Show(value.ToString());
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
               search_btn.Focus();
                search_btn_Click(this, new EventArgs());


                if (this.listView1.Items.Count == 0)
                    return;
                string id = listView1.Items[0].SubItems[0].Text;
                string sql = "select * from datas where id= " + id;
                string value = chaxunvalue(sql);
                result_text.Text = value;
                tabControl1.SelectedIndex = 0;
                return;
            }

        }
    }
}
