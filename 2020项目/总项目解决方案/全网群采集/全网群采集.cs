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

        #region 识别二维码文字
        public string shibie(string picurl)
        {
            string url = "https://aip.baidubce.com/rest/2.0/ocr/v1/general_basic";
            string access_token = "24.7ab687deb57c3c8383bc71c22d54a57e.2592000.1590636917.282335-19639932";
            string postdata = "access_token=" + access_token + "&url=" + picurl;


            string html = PostUrl(url, postdata);
            return html;
        }
        #endregion

        private void 全网群采集_Load(object sender, EventArgs e)
        {

        }

        string constr = "Host =111.229.244.97;Database=acaiji;Username=root;Password=root";

        ArrayList vxcodelist = new ArrayList();
        #region 获取数据库vxcode集合
        public void getvxcodes()
        {
            
            try
            {
                
                string str = "SELECT vxcode from quns";
                MySqlDataAdapter da = new MySqlDataAdapter(str, constr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    vxcodelist.Add(dr[0].ToString().Trim());
                }
            }
            catch (MySqlException ee)
            {
                MessageBox.Show(ee.Message.ToString());
            }
            

        }
        #endregion
        #region  插入数据库

        public string insert(string name, string picurl, string resource, string expiretime,string vxcode)
        {

            try
            {
               
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO quns (name,pic_url,resource,expiretime,vxcode)VALUES('" + name + " ', '" + picurl + " ', '" + resource + " ', '" + expiretime + " ', '" + vxcode + " ')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


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

        string[] tiebas = { "拼多多微信群",
"拼多多",
"拼多多砍价",
"副业",
"优惠券",
"微商群",
"内部优惠券",
"购物券",
"褥羊毛",
"网上购物",
"购物狂",
"宝妈微信群",
"宝妈群",
"购物群",
"拼多多砍价群",
"宝妈微信群",
"宝妈微信群二维码",
"微商群聊",
"王者荣耀微信群",
            "交友"};
        #region 贴吧
        public void run()
        {
            while (true)
            {
                foreach (string tieba in tiebas)
                {
                    getvxcodes();
                    for (int i = 0; i < 51; i = i + 50)
                    {
                        string url = "https://tieba.baidu.com/f?kw=" + System.Web.HttpUtility.UrlEncode(tieba) + "&ie=utf-8&pn=" + i;
                        string html = GetUrl(url);

                        MatchCollection pics = Regex.Matches(html, @"bpic=""([\s\S]*?)""");
                        for (int j = 0; j < pics.Count; j++)
                        {
                            try
                            {

                                string vxcode = DecodeQrCode(UrlToBitmap(pics[j].Groups[1].Value));
                                if (vxcode != "" && vxcode != null)
                                {
                                    if (vxcode.Contains("/g/"))
                                    {
                                        if (!vxcodelist.Contains(vxcode))
                                        {

                                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                            lv1.SubItems.Add(pics[j].Groups[1].Value);
                                            lv1.SubItems.Add(tieba);
                                            lv1.SubItems.Add(vxcode);
                                            string wenzi = shibie(pics[j].Groups[1].Value);
                                            Match name = Regex.Match(wenzi, @"""words"": ""([\s\S]*?)""");
                                            Match time = Regex.Match(wenzi, @"7天内\(([\s\S]*?)前");
                                            if (time.Groups[1].Value.Contains("月") && time.Groups[1].Value.Contains("日"))
                                            {
                                                lv1.SubItems.Add(name.Groups[1].Value);
                                                lv1.SubItems.Add(time.Groups[1].Value);
                                                lv1.SubItems.Add(DateTime.Now.ToString());
                                                insert(name.Groups[1].Value, pics[j].Groups[1].Value, tieba, time.Groups[1].Value, vxcode);
                                            }

                                            else
                                            {
                                                lv1.SubItems.Add(name.Groups[1].Value);
                                                lv1.SubItems.Add(time.Groups[1].Value);
                                                lv1.SubItems.Add("false");
                                            }
                                        }
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
            }

        }
         
        #endregion



        private DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(Convert.ToDouble(timeStamp));


        }

        bool zanting = true;
        
 

        private void 复制网址ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(this.listView1.SelectedItems[0].SubItems[1].Text);
        }

        private void 复制串码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(this.listView1.SelectedItems[0].SubItems[2].Text);
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

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
