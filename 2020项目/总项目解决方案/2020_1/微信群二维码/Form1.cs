using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;
using helper;

namespace 微信群二维码
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool zanting = true;
        private void Form1_Load(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader(path + "finish.txt", Encoding.Default);
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int i = 0; i < text.Length; i++)
            {
                finishes.Add(text[i]);
             

            }
            sr.Close();

        }
        #region GET请求
        /// <summary>
        /// GET请求带COOKIE
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            try
            {
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.108 Safari/537.36";
                request.Referer = "https://xy2.cbg.163.com/cgi-bin/equipquery.py?act=query&server_id=115&areaid=3&page=1&kind_id=45&query_order=selling_time+DESC";
                request.Headers.Add("Cookie", COOKIE);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

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
        #region 获取数据流
        public static Stream getStream(string Url)
        {

            string COOKIE = "shshshfpa=1325c19d-7317-26c8-e006-872a4999a27c-1526910507; shshshfpb=04f1e443ad4a14669b330d37d1758accda7f9d43dafa36bed5b0a7393d; __jdu=1746186544; user-key=ac09ee9c-3a85-4f62-b422-bd2f5436c54c; cn=2; PCSYCityID=933; __jda=122270672.1746186544.1539856564.1557561034.1557561069.9; __jdv=122270672|baidu|-|organic|not set|1557561069043; areaId=12; __jdc=122270672; ipLoc-djd=12-933-3407-40385; mt_xid=V2_52007VwMUV1pYW10bTBxsDGFRRlQPXgdGT0ERDxliUxtUQVBUXR9VEFhSb1QaAFsNUw4deRpdBW8fE1ZBWFJLH04SWA1sARBiXWhSahpNGlUCYQMWVm1aVlsb; _gcl_au=1.1.2120468405.1557565190; shshshfp=3d9d5b5ee4df1d426f7790085e66ff99; 3AB9D23F7A4B3C9B=W6QBEF5Y5EEHHOXWYSZC2W573XDNDCKCNKNFR2WTIFWNLSYRADQCTJY3QJNDM2XXCICM7SW4MLBVF7SE2WASP7S7CU; shshshsID=71cf11e2b286360f93326c3a6f5654ec_69_1557566178401; _jshop_vd_=NTZWCV6WKKGBA; __jdb=122270672.97.1746186544|9.1557561069; JSESSIONID=011256AB80718214DBBA8CC218158A96.s1";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
            request.Timeout = 10000;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.81 Safari/537.36";
            request.AllowAutoRedirect = true;
            request.Headers.Add("Cookie", COOKIE);
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
            return response.GetResponseStream();

        }
        #endregion

        #region 下载文件
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="URLAddress">图片地址</param>
        /// <param name="subPath">图片所在文件夹</param>
        /// <param name="name">图片名称</param>
        public static void downloadFile(string URLAddress, string subPath, string name)
        {
            string path = System.IO.Directory.GetCurrentDirectory();

            WebClient client = new WebClient();

            if (false == System.IO.Directory.Exists(subPath))
            {
                //创建pic文件夹
                System.IO.Directory.CreateDirectory(subPath);
            }
            client.DownloadFile(URLAddress, subPath + "\\" + name);
        }

        #endregion
        ArrayList finishes = new ArrayList();
        string path = AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
            try
            {
                for (int i = 0; i < 5; i++)
                {

                    string url = "https://weixinqun.com/group?p=" + i;
                    string html = GetUrl(url, "utf-8");


                    MatchCollection uids = Regex.Matches(html, @"<p class=""goods_name ellips"">([\s\S]*?)id=([\s\S]*?)""");



                    for (int j = 0; j < uids.Count; j++)
                    {
                        if (!finishes.Contains(uids[j].Groups[2].Value))
                        {
                            FileStream fs1 = new FileStream(path + "finish.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                            StreamWriter sw = new StreamWriter(fs1);
                            sw.WriteLine(uids[j].Groups[2].Value);
                            sw.Close();
                            fs1.Close();





                            string URL = "https://weixinqun.com/group?id=" + uids[j].Groups[2].Value;
                            string strhtml = GetUrl(URL, "utf-8");
                            Match pic = Regex.Match(strhtml, @"<span class=""shiftcode""><img src=""([\s\S]*?)""");
                            if (pic.Groups[1].Value != "")
                            {
                                label3.Text = "正在下载："+ pic.Groups[1].Value;

                                string value = jiexi(pic.Groups[1].Value);

                                string avalue= Regex.Replace(value, @".*/", "");
                                ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                listViewItem.SubItems.Add(avalue);

                                // listViewItem.SubItems.Add(pic.Groups[1].Value);
                                if (value.Contains("/g/"))
                                {

                                    if (!Directory.Exists(textBox1.Text + "二维码1\\"))
                                    {
                                        Directory.CreateDirectory(textBox1.Text + "\\二维码1");
                                    }

                                    downloadFile(pic.Groups[1].Value, textBox1.Text + "\\二维码1", avalue + ".jpg");


                                }
                            }
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            Thread.Sleep(1000);
                        }
                    }
                }
            }

            catch (Exception ex)
            {

                textBox1.Text = ex.ToString();
            }
        }

        #region 解码
        private string jiexi(string picurl)
        {
            try
            {
                Image image = Image.FromStream(getStream(picurl));
                QRCodeDecoder decoder = new QRCodeDecoder();
                String decodedString = decoder.decode(new QRCodeBitmapImage(new Bitmap(image)));
                return (decodedString);
            }
            catch (Exception ex)
            {

                return "解析失败";
            }
          


        }
        #endregion
        private void Button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(listView1), "Sheet1", true);

        }

        private void Button5_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择所在文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }

                textBox1.Text = dialog.SelectedPath;
            }

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请选择二维码保存文件夹");
                return;
            }
            label3.Text = "已开始....请勿重复点击";
            #region 通用验证

            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php", "utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == "25.25.25.25")
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {

                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

                timer1.Start();
                timer1.Interval = Convert.ToInt32(textBox3.Text) * 1000 * 60;

            }
            else
            {
                MessageBox.Show("IP不符");

            }
            #endregion

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
