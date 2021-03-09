using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序202103
{
    public partial class 随机数 : Form
    {
        public 随机数()
        {
            InitializeComponent();
        }

        public bool panduan(string input)
        {
            string str = textBox3.Text.Trim();
            //char[] strCharArr = str.ToCharArray();

            string[] text = str.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (var item in text)
            {
                if (item.ToString() == input)
                {
                    return true;
                }
            }

            return false;
        }
        int cishu = 0;
        public void createShuLie()
        {
            if (listView1.Items.Count == 0)
            {
                for (int i = 0; i < Convert.ToInt32(textBox1.Text); i++)
                {
                    Random rd = new Random(Guid.NewGuid().GetHashCode()); //生成不重复的随机数，默认的话根据时间戳如果太快会相同
                    int suiji = rd.Next(1, 9);
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(suiji.ToString());
                }
            }
            else
            {
                cishu = cishu + 1;
                label6.Text = cishu.ToString();
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    Random rd = new Random(Guid.NewGuid().GetHashCode());
                    int suiji = rd.Next(1, 9);
                    string qishivalue = listView1.Items[i].SubItems[1].Text;
                    listView1.Items[i].SubItems[1].Text= qishivalue + " "+suiji;
                    if (panduan(qishivalue.Substring(qishivalue.Length-1,1)))
                    {
                        
                        listView1.Items[i].SubItems[1].Text= suiji.ToString();
                        listView1.Items[i].BackColor = Color.White;
                    }
                    if (listView1.Items[i].SubItems[1].Text.Length > Convert.ToInt32(textBox2.Text) + (Convert.ToInt32(textBox2.Text)-3))
                    {
                        label7.Text = "出现符合要求数列！";
                        label7.ForeColor = Color.Red;
                        listView1.Items[i].BackColor = Color.Yellow;
                    }
                }

            }

        }

        Thread t;
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            string html = "";
            string COOKIE = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 100000;

                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }

                response.Close();
                return html;



            }
            catch (System.Exception ex)
            {
                return ex.ToString();

            }



        }
        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"YOaO6W"))
            {
                MessageBox.Show("");
                return;
            }



            #endregion
            if (t == null || !t.IsAlive)
            {
                t = new Thread(createShuLie);
                t.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            // MessageBox.Show(Guid.NewGuid().ToString());  //结果是xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx结构的16进制数字
        }

        private void 随机数_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            label7.Text = "未出现符合要求数列";
            cishu = 0;
            listView1.Items.Clear();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           
            ListView listview = (ListView)sender;
            ListViewItem lstrow = listview.GetItemAt(e.X, e.Y);
            System.Windows.Forms.ListViewItem.ListViewSubItem lstcol = lstrow.GetSubItemAt(e.X, e.Y);
            string strText = lstcol.Text;
            try
            {
                Clipboard.SetDataObject(strText);
                string info = string.Format("内容【{0}】已经复制到剪贴板", strText);
                MessageBox.Show("复制成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
