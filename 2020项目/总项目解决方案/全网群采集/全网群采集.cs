using MySql.Data.MySqlClient;
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
using ZXing;

namespace 全网群采集
{
    public partial class 全网群采集 : Form
    {
        public 全网群采集()
        {
            InitializeComponent();
        }
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "session-id=144-7450304-7580635; session-id-time=2082787201l; ubid-main=133-9411273-8184314; x-wl-uid=1AR+eCj1iY57TRhM7A2m5KF9SEb1ho13Om87l60jAFJAp10qHX8GNgnZcOFTknCbmNkftPnMho/k=; aws-priv=eyJ2IjoxLCJldSI6MCwic3QiOjB9; aws-target-static-id=1536650638823-915613; s_fid=16BD3861C3483809-386224FB67B4E94E; regStatus=pre-register; s_dslv=1536656308918; i18n-prefs=USD; lc-main=zh_CN; sp-cdn=\"L5Z9: CN\"; session-token=/8/yst6nJSzUghSOya1omO6MEhQ/Moyyq2FsFStf5zcm4cZPhl38RIpfC+UZyiw//J9HubG+McoZMSB4hRyykQZ0SH1X07eSi5nxcOjmHQshqSmCJD6tL8cgFOFCByRnF1EJMjmxRfVwTkZZ/4yLqjzBQ2Ik6WclU4tG1u7+4UCFeGDYa//WLb3fCGfB6RuU; csm-hit=tb:DT2JH7KAE9BTWY50PJA8+s-DT2JH7KAE9BTWY50PJA8|1585472314824&t:1585472314824&adb:adblk_no";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://www.amazon.com/s?k=6Q0+959+856&__mk_zh_CN=%E4%BA%9A%E9%A9%AC%E9%80%8A%E7%BD%91%E7%AB%99&ref=nb_sb_noss";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
               
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
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
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

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public string PostUrl(string url, string postData)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Post";
            request.ContentType = "application/x-www-form-urlencoded";
            // request.ContentType = "application/json";
            request.ContentLength = postData.Length;

            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.112 Safari/537.36";
            //request.Headers.Add("Cookie", COOKIE);

            StreamWriter sw = new StreamWriter(request.GetRequestStream());
            sw.Write(postData);
            sw.Flush();


            WebResponse response = request.GetResponse();
            Stream s = response.GetResponseStream();
            StreamReader sr = new StreamReader(s, Encoding.GetEncoding("utf-8"));
            string html = sr.ReadToEnd();

            sw.Dispose();
            sw.Close();
            sr.Dispose();
            sr.Close();
            s.Dispose();
            s.Close();
            return html;
        }

        #endregion

        #region  网络图片转Bitmap
        public static Bitmap UrlToBitmap(string url)
        {
            WebClient mywebclient = new WebClient();
            byte[] Bytes = mywebclient.DownloadData(url);
            using (MemoryStream ms = new MemoryStream(Bytes))
            {
                Image outputImg = Image.FromStream(ms);
                
                Bitmap map = new Bitmap(outputImg);
                return map;
            }
        }
        #endregion

        #region 识别图中二维码 Bitmap格式图片识别无二维码为空
        /// <summary>
        /// 识别图中二维码 Bitmap格式图片识别无二维码为空
        /// </summary>
        /// <param name="barcodeBitmap"></param>
        /// <returns></returns>
        private string DecodeQrCode(Bitmap barcodeBitmap)
        {
            BarcodeReader reader = new BarcodeReader();
            reader.Options.CharacterSet = "UTF-8";
            var result = reader.Decode(barcodeBitmap);
            return (result == null) ? null : result.Text;
        }

        #endregion

        private void 全网群采集_Load(object sender, EventArgs e)
        {

        }
        #region 识别二维码文字
        public string shibie(string picurl)
        {
            string url = "https://aip.baidubce.com/rest/2.0/ocr/v1/general_basic";
            string access_token = "24.7ab687deb57c3c8383bc71c22d54a57e.2592000.1590636917.282335-19639932";
            string postdata = "access_token=" + access_token + "&url="+picurl;


            string html = PostUrl(url, postdata);
            return html;
        }
        #endregion


        #region  插入数据库

        public string insert(string name, string picurl, string resource, string expiretime)
        {

            try
            {


                string constr = "Host =111.229.244.97;Database=acaiji;Username=root;Password=root";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO quns (name,pic_url,resource,expiretime)VALUES('" + name + " ', '" + picurl + " ', '" + resource + " ', '" + expiretime + " ')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {

                    
                    mycon.Close();
                    return "成功";

                }
                else
                {

                    mycon.Close();
                    return "失败";
                }


            }

            catch (System.Exception ex)
            {
                return(ex.ToString());
            }

        }

        #endregion

        ArrayList finishes = new ArrayList();
        #region 贴吧
        public void run(object tieba1)
        {
            string tieba = tieba1.ToString();
            try
            {
                
         
                        for (int i = 0; i < 1001; i = i + 50)
                        {
                            string url = "https://tieba.baidu.com/f?kw=" + System.Web.HttpUtility.UrlEncode(tieba) + "&ie=utf-8&pn=" + i;
                            string html = GetUrl(url);

                            MatchCollection pics = Regex.Matches(html, @"bpic=""([\s\S]*?)""");
                            for (int j = 0; j < pics.Count; j++)
                            {
                                try
                                {
                                    label1.Text = DateTime.Now.ToString() + pics[j].Groups[1].Value;
                                    string vxcode = DecodeQrCode(UrlToBitmap(pics[j].Groups[1].Value));
                                    if (vxcode != "" && vxcode != null)
                                    {
                                        if (vxcode.Contains("/g/"))
                                        {
                                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                            lv1.SubItems.Add(pics[j].Groups[1].Value);
                                            lv1.SubItems.Add(tieba);
                                            lv1.SubItems.Add(vxcode);
                                            string wenzi = shibie(pics[j].Groups[1].Value);
                                            Match name = Regex.Match(wenzi, @"""words"": ""([\s\S]*?)""");
                                            Match time = Regex.Match(wenzi, @"该二维码7天内\(([\s\S]*?)前");

                                            lv1.SubItems.Add(name.Groups[1].Value);
                                            lv1.SubItems.Add(time.Groups[1].Value);

                                            label1.Text=  insert(name.Groups[1].Value, pics[j].Groups[1].Value,tieba,time.Groups[1].Value);
                                        }
                                    }
                                }
                                catch
                                {

                                    continue;
                                }



                            }
                        }  
               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


        }

        #endregion

        private DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(Convert.ToDouble(timeStamp));


        }

        bool zanting = true;
        #region 同行
        public void tonghang()
        {

            try
            {

                for (int j = 1; j < 9999; j++)
                {


                    string url = "https://itui.yfdou.com/v1/api/qrcode/latest_ten?page=" + j;

                    string html = GetUrl(url);

                    MatchCollection names = Regex.Matches(html, @"name"": ""([\s\S]*?)""");
                    MatchCollection times = Regex.Matches(html, @"exp_time"":([\s\S]*?),");
                    MatchCollection titles = Regex.Matches(html, @"title"": ""([\s\S]*?)""");
                    MatchCollection images = Regex.Matches(html, @"pic_url"": ""([\s\S]*?)""");
                   
                    if (images.Count == 0)
                    {

                        return;
                    }
                    label1.Text = "正在抓取第" + j + "页";
                    for (int i = 0; i < images.Count; i++)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据  
                        lv1.SubItems.Add(images[i].Groups[1].Value);
                        lv1.SubItems.Add(titles[i].Groups[1].Value);
                        lv1.SubItems.Add("-");
                        lv1.SubItems.Add(names[i].Groups[1].Value);
                        lv1.SubItems.Add(ConvertStringToDateTime(times[i].Groups[1].Value).ToString());
                        



                        label1.Text = insert(names[i].Groups[1].Value, images[i].Groups[1].Value, titles[i].Groups[1].Value, times[i].Groups[1].Value);
                       

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                    }
                    Thread.Sleep(500);



                }


                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


        }

        #endregion
        string[] text = { };
        private void Button1_Click(object sender, EventArgs e)
        {

            Thread thread = new Thread(new ParameterizedThreadStart(run));
            string o = "拼多多";
            thread.Start((object)o);
            Control.CheckForIllegalCrossThreadCalls = false;

            Thread thread1 = new Thread(new ParameterizedThreadStart(run));
            string o1 = "宝妈微信群";
            thread1.Start((object)o1);

            Thread thread2 = new Thread(new ParameterizedThreadStart(run));
            string o2 = "宝妈群";
            thread2.Start((object)o2);

            Thread thread3= new Thread(new ParameterizedThreadStart(run));
            string o3 = "微商群";
            thread3.Start((object)o3);

            Thread thread4= new Thread(new ParameterizedThreadStart(run));
            string o4 = "拼多多砍价";
            thread4.Start((object)o4);

            Thread thread5 = new Thread(new ParameterizedThreadStart(run));
            string o5 = "王者荣耀开黑";
            thread5.Start((object)o5);


            Thread thread6 = new Thread(new ParameterizedThreadStart(run));
            string o6 = "交友";
            thread6.Start((object)o6);

            Thread thread7 = new Thread(new ParameterizedThreadStart(run));
            string o7 = "交友";
            thread7.Start((object)o7);

        }

        private void 复制网址ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(this.listView1.SelectedItems[0].SubItems[1].Text);
        }

        private void 复制串码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(this.listView1.SelectedItems[0].SubItems[2].Text);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(tonghang));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            MessageBox.Show(shibie("http://czt.sc.gov.cn/kj/captcha.jpg?randdom=0.8007734420064332"));
        }

        private void 全网群采集_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确认退出吗？", "退出询问"
           , MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK)
            {
                e.Cancel = true;//告诉窗体关闭这个任务取消

            }
            else
            {
              

                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
    }
}
