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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序
{
    public partial class 工厂针织服装软件 : Form
    {
        public 工厂针织服装软件()
        {
            InitializeComponent();
        }

        public string image64;
        #region  image转base64
        public static string ImageToBase64(Image image)
        {
            try
            {
                Bitmap bmp = new Bitmap(image);
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region  base64转image
        public static Bitmap Base64ToImage(string strbase64)
        {
            try
            {
                byte[] arr = Convert.FromBase64String(strbase64);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);
                ms.Close();

                return bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        #endregion
        string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
        /// <summary>
        /// 插入数据库
        /// </summary>
        public void insertdata(string sql)
        {
            try
            {

               

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\fzdata.db");
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
        /// 读取数据库
        /// </summary>
        public void getdata(string uid)
        {
            try
            {

                

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\fzdata.db");
                mycon.Open();

                SQLiteCommand cmd = new SQLiteCommand("select * from data where uid='" + uid + "'", mycon);
                SQLiteDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    textBox1.Text = rdr["jgtz"].ToString();

                    textBox2.Text = rdr["fgtz"].ToString();
                    pictureBox1.Image = Base64ToImage(rdr["image"].ToString());
                }

                else
                {
                    textBox1.Text = "";
                    textBox2.Text = "";
                    pictureBox1.Image = null;

                }
                mycon.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.ToString());

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
                string COOKIE = "session-id=144-7450304-7580635; session-id-time=2082787201l; ubid-main=133-9411273-8184314; x-wl-uid=1AR+eCj1iY57TRhM7A2m5KF9SEb1ho13Om87l60jAFJAp10qHX8GNgnZcOFTknCbmNkftPnMho/k=; aws-priv=eyJ2IjoxLCJldSI6MCwic3QiOjB9; aws-target-static-id=1536650638823-915613; s_fid=16BD3861C3483809-386224FB67B4E94E; regStatus=pre-register; s_dslv=1536656308918; i18n-prefs=USD; lc-main=zh_CN; sp-cdn=\"L5Z9: CN\"; session-token=/8/yst6nJSzUghSOya1omO6MEhQ/Moyyq2FsFStf5zcm4cZPhl38RIpfC+UZyiw//J9HubG+McoZMSB4hRyykQZ0SH1X07eSi5nxcOjmHQshqSmCJD6tL8cgFOFCByRnF1EJMjmxRfVwTkZZ/4yLqjzBQ2Ik6WclU4tG1u7+4UCFeGDYa//WLb3fCGfB6RuU; csm-hit=tb:DT2JH7KAE9BTWY50PJA8+s-DT2JH7KAE9BTWY50PJA8|1585472314824&t:1585472314824&adb:adblk_no";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://www.amazon.com/s?k=6Q0+959+856&__mk_zh_CN=%E4%BA%9A%E9%A9%AC%E9%80%8A%E7%BD%91%E7%AB%99&ref=nb_sb_noss";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("sec-fetch-mode:navigate");
                headers.Add("sec-fetch-site:same-origin");
                headers.Add("sec-fetch-user:?1");
                headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                // request.KeepAlive = true;
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


        private void 工厂针织服装软件_Load(object sender, EventArgs e)
        {
            
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"gongchangfz"))
            {
                


            }

            else
            {
                MessageBox.Show("验证失败");
                Environment.Exit(0);
                return;
            }


            #endregion
            this.treeView1.ExpandAll();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string jgtz = textBox1.Text.Trim();
            string fgtz = textBox2.Text.Trim();

            string uid = treeView1.SelectedNode.Name ;
          

            insertdata("INSERT INTO data (jgtz,fgtz,image,uid) VALUES( '" + jgtz + "','" + fgtz + "','" + image64 + "','" + uid + "')");
            MessageBox.Show("保存成功");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "图片文件|*.jpg|BMP图片|*.bmp|Gif图片|*.gif";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            Image image = Image.FromFile(ofd.FileName);
            pictureBox1.Image = image;
            image64 = ImageToBase64(image);
            
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            getdata(treeView1.SelectedNode.Name);
            label5.Text = treeView1.SelectedNode.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string jgtz = textBox1.Text.Trim();
            string fgtz = textBox2.Text.Trim();
          
            image64 = ImageToBase64(pictureBox1.Image);
            string uid = treeView1.SelectedNode.Name;


            insertdata("UPDATE data SET jgtz='" + jgtz + "',fgtz='" + fgtz + "',image='" + image64 + "' where uid ='" + uid + "'");
            MessageBox.Show("更新成功");
        }
    }
}
