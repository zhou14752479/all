using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;
using ZXing;

namespace 主程序202012
{
    public partial class 转转商品监控 : Form
    {
        public 转转商品监控()
        {
            InitializeComponent();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData, string COOKIE, string charset)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = "";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion


        ArrayList lists = new ArrayList();


        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <returns></returns>

        public Bitmap creatQcode(string path)
        {
            try
            {
                int width = 150; //图片宽度
                int height = 150;//图片长度
                BarcodeWriter barCodeWriter = new BarcodeWriter();
                barCodeWriter.Format = BarcodeFormat.QR_CODE; // 生成码的方式(这里设置的是二维码),有条形码\二维码\还有中间嵌入图片的二维码等
                barCodeWriter.Options.Hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");// 支持中文字符串
                barCodeWriter.Options.Hints.Add(EncodeHintType.ERROR_CORRECTION, ZXing.QrCode.Internal.ErrorCorrectionLevel.H);
                barCodeWriter.Options.Height = height;
                barCodeWriter.Options.Width = width;
                barCodeWriter.Options.Margin = 0; //设置的白边大小
                ZXing.Common.BitMatrix bm = barCodeWriter.Encode(path);  //DNS为要生成的二维码字符串
                Bitmap result = barCodeWriter.Write(bm);
                Bitmap Qcbmp = result.Clone(new Rectangle(Point.Empty, result.Size), PixelFormat.Format1bppIndexed);//位深度
                                                                                                                    // SaveImg(currentPath, Qcbmp); //图片存储自己写的函数
                                                                                                                    //Qcbmp=WhiteUp(Qcbmp,10);
                return Qcbmp;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                return null;

            }
        }
        private DateTime ConvertStringToDateTime(string timeStamp)
        {
            //DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            //long mTime = long.Parse(timeStamp + "0000");
            //TimeSpan toNow = new TimeSpan(mTime);
            //return startTime.Add(toNow);

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(Convert.ToDouble(timeStamp));

        }
        /// <summary>
        /// 获取时间戳毫秒
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalMilliseconds);
            return a.ToString();
        }


        // string cookie = "uid=50023136438031;PPU=\"TT=fc7b939760a82716403dbef38dcfb54a7e408240&UID=50023136438031&SF=ZHUANZHUAN&SCT=1609296034842&V=1&ET=1611884434842\";tk=wt-oHQwN0aLWTb_qMJBboVR5pISwDrA;t=103;version=7.0.94;v=7.0.94;model=iPhone%207%20Plus%3CiPhone9%2C2%3E;channelid=none;osType=ios;id58=c5/nR1/r2pIPNX6AI+oKAg==;csrfToken=qb-O_XRiRAVO3KDIyYet79pC;hasGotWXRankData=1;";
        string cookie = "";

        public void xiaochengxu()
        {
            textBox7.Text = DateTime.Now.ToString("hh:MM:ss") + "：" +"程序已启动";

            try
            {
                string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (var key in text)
                {
                    for (int page = 1; page < 5; page++)
                    {


                        string keyword = System.Web.HttpUtility.UrlEncode(key).Replace("+", "%20");
                        string price = textBox5.Text.Trim() + "_" + textBox6.Text.Trim();
                        string url = "https://app.zhuanzhuan.com/zz/v2/zzinfoshow/getfeedflowinfo"; //小程序

                        string postdata = "lat=&lon=&areaId=0&areaid=0&pageNum="+ page + "&metricType=&pageSize=20&searchFrom=1&from=3&osType=ios&feedFlowType=WxSearch&filterParams=%7B%22st8%22%3A%5B%7B%22value%22%3A%22pr2%22%2C%22cmd%22%3A%22now_price%3D%3F%22%2C%22type%22%3A%22priceInput%22%2C%22supplement%22%3A%22" + price + "%22%7D%5D%7D&tableId=1&filteritemids=%7C%7C&keyword=" + keyword + "&pgSwitch=1&pgCateParam=%7B%7D&entChanl=&transparentParam=&pushcode=3&searcfilterhmove2zzsearch=1&searchPageSource=1&searchfrom=3&searchsuggestcate=1&usePgParam=1&v7abtest=1&requestmark=1600755698000"; //小程序

                        string html = PostUrl(url, postdata, cookie, "utf-8");


                        MatchCollection titles = Regex.Matches(html, @"""infoDesc"":""([\s\S]*?)""");
                        MatchCollection prices = Regex.Matches(html, @"""infoPrice"":([\s\S]*?),");
                        MatchCollection times = Regex.Matches(html, @"""creatTime"":""([\s\S]*?)""");
                        MatchCollection urls = Regex.Matches(html, @"""infoId"":""([\s\S]*?)""");
                        for (int i = 0; i < titles.Count; i++)
                        {
                            if (!lists.Contains(urls[i].Groups[1].Value))
                            {
                                lists.Add(urls[i].Groups[1].Value);
                                textBox7.Text += DateTime.Now.ToString("hh:MM:ss") + "：" + titles[i].Groups[1].Value + "\r\n";
                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(titles[i].Groups[1].Value);
                                lv1.SubItems.Add(prices[i].Groups[1].Value.Substring(0, prices[i].Groups[1].Value.Length - 2));

                                lv1.SubItems.Add(ConvertStringToDateTime(times[i].Groups[1].Value).ToString());
                                lv1.SubItems.Add("https://m.zhuanzhuan.com/u/zyservice/detail-yyj?infoId=" + urls[i].Groups[1].Value);
                                if (textBox7.TextLength > 10000)
                                {
                                    textBox7.Text = "";
                                }

                                //if (listView1.Items.Count > Convert.ToInt32(textBox2.Text))
                                //{
                                //    listView1.Items.Clear();
                                //}

                                if (Convert.ToInt64(GetTimeStamp()) - Convert.ToInt64(times[i].Groups[1].Value) < 100000)
                                {
                                    lv1.BackColor = Color.Red;
                                    MessageBox.Show("出现新上架商品");
                                }
                            }
                        }

                        Thread.Sleep(1000);
                    }
                }

              
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
               
            }
        }



        public void app()
        {
            textBox7.Text = DateTime.Now.ToString("hh:MM:ss") + "：" + "程序已启动";

            try
            {
                string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (var key in text)
                {
                    
                        string keyword = System.Web.HttpUtility.UrlEncode(key).Replace("+", "%20");
                        string price = textBox5.Text.Trim() + "_" + textBox6.Text.Trim();

                        string url = "https://app.zhuanzhuan.com/zz/transfer/search"; //APP

                        string dates = DateTime.Now.ToString("yyyyMMddhh") + "_" + DateTime.Now.AddDays(1).ToString("yyyyMMddhh");

                        string postdata = "filterParams=%7B%223%22%3A%5B%7B%22value%22%3A%221%22%2C%22cmd%22%3A%22timestamp%3D" + dates + "%22%7D%5D%2C%22st8%22%3A%5B%7B%22value%22%3A%22pr2%22%2C%22supplement%22%3A%22" + price + "%22%2C%22cmd%22%3A%22now_price%3D?%22%2C%22type%22%3A%22priceInput%22%7D%5D%7D&keyword=" + keyword + "&pagenum=1&pagesize=20&pushcode=1&requestmark=1609471541000&searcfilterhmove2zzsearch=1&searchPageSource=1&searchfrom=1&searchsuggestcate=1&tabId=1&usePgParam=1&v7abtest=1&zpm=1%5EV1008%5E1%5E0%5E0";//APP
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    //string postdata = "filterParams=%7B%22st8%22%3A%5B%7B%22value%22%3A%22pr2%22%2C%22supplement%22%3A%22"+price+"%22%2C%22cmd%22%3A%22now_price%3D?%22%2C%22type%22%3A%22priceInput%22%7D%5D%7D&keyword="+keyword+"&pagenum=1&pagesize=20&pushcode=131&requestmark=1609469741000&searcfilterhmove2zzsearch=1&searchPageSource=14&searchfrom=131&searchsuggestcate=1&tabId=1&usePgParam=1&v7abtest=1&zpm=1%5EV1008%5E5%5E1%5E131";
                        string html = PostUrl(url, postdata, cookie, "utf-8");
                        // textBox1.Text = html;


                        MatchCollection titles = Regex.Matches(html, @"""strInfoId"":""([\s\S]*?)""([\s\S]*?)""title"":""([\s\S]*?)""");
                        MatchCollection prices = Regex.Matches(html, @"""price_f"":""([\s\S]*?)""");
                        MatchCollection times = Regex.Matches(html, @"""pubTime"":""([\s\S]*?)""");

                        for (int i = 0; i < titles.Count; i++)
                        {
                            if (!lists.Contains(titles[i].Groups[1].Value))
                            {
                                lists.Add(titles[i].Groups[1].Value);
                                textBox7.Text += DateTime.Now.ToString("hh:MM:ss") + "：" + titles[i].Groups[3].Value + "\r\n";
                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(titles[i].Groups[3].Value);
                                lv1.SubItems.Add(prices[i].Groups[1].Value.Substring(0, prices[i].Groups[1].Value.Length - 2));

                                lv1.SubItems.Add(ConvertStringToDateTime(times[i].Groups[1].Value).ToString());
                                lv1.SubItems.Add("https://m.zhuanzhuan.com/u/zyservice/detail-yyj?infoId=" + titles[i].Groups[1].Value);
                                if (textBox7.TextLength > 10000)
                                {
                                    textBox7.Text = "";
                                }



                                if (Convert.ToInt64(GetTimeStamp()) - Convert.ToInt64(times[i].Groups[1].Value) < 100000)
                                {
                                    lv1.BackColor = Color.Red;
                                    MessageBox.Show("出现新上架商品");
                                }
                            }
                        }

                        Thread.Sleep(1000);
                    }
                

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());

            }
        }



        public void xiaochengxulist()
        {
            textBox7.Text = DateTime.Now.ToString("hh:MM:ss") + "：" + "程序已启动";

            try
            {
                string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (var key in text)
                {
                    for (int page = 1; page < 5; page++)
                    {

                        string keyword = System.Web.HttpUtility.UrlEncode(key).Replace("+", "%20");
                        string price = textBox5.Text.Trim() + "_" + textBox6.Text.Trim();

                        string url = " https://app.zhuanzhuan.com/zzopen/ypmall/listData";

                        string dates = DateTime.Now.ToString("yyyyMMddhh") + "_" + DateTime.Now.AddDays(1).ToString("yyyyMMddhh");

                        string postdata = "param=%7B%22filter%22%3A%225461%3A101%3B%3Bkeyword%3A" + keyword + "%3Btab%3A1%22%2C%22listInfoType%22%3A%22fullInfo%22%2C%22initFrom%22%3A%22undefined%22%2C%22secondFrom%22%3A%22%22%2C%22pageIndex%22%3A"+page+"%2C%22filterItems%22%3A%7B%221%22%3A%5B%7B%22value%22%3A%22101%2C0%2C0%22%2C%22cmd%22%3A%22pg_cate_brand_model%3D101%2C0%2C0%22%7D%5D%2C%223%22%3A%5B%7B%22value%22%3A%221%22%2C%22cmd%22%3A%22timestamp%3D" + dates + "%22%7D%5D%2C%224%22%3A%5B%7B%22value%22%3A%22pr2%22%2C%22cmd%22%3A%22now_price%3D%3F%22%2C%22supplement%22%3A%22" + price + "%22%7D%2C%7B%22value%22%3A%22pr2%22%2C%22cmd%22%3A%22now_price%3D%3F%22%7D%5D%7D%2C%22listSearchType%22%3A%22baseList%22%2C%22pageSize%22%3A16%2C%22rstmark%22%3A1609554626736%2C%22firstFrom%22%3A%22yp_list_search_101%2C10530%2C0%22%2C%22otherParams%22%3A%7B%22pgCate%22%3A%22101%2C0%2C0%22%2C%22abLayer%22%3A%22personalSupply%22%2C%22invokeFrom%22%3A%22%22%7D%7D";
                        string html = PostUrl(url, postdata, cookie, "utf-8");
                        // textBox1.Text = html;

                        MatchCollection ids = Regex.Matches(html, @"""infoId"":""([\s\S]*?)""");
                        MatchCollection titles = Regex.Matches(html, @"""title"":""([\s\S]*?)""([\s\S]*?)""salePrice"":([\s\S]*?),");



                        for (int i = 0; i < ids.Count; i++)
                        {
                            if (!lists.Contains(ids[i].Groups[1].Value))
                            {


                                lists.Add(ids[i].Groups[1].Value);


                                string strhtml = method.GetUrl("https://app.zhuanzhuan.com/zz/transfer/getgoodsdetaildata?infoId=" + ids[i].Groups[1].Value, "utf-8");

                                Match pvCount = Regex.Match(strhtml, @"""pvCount"":""([\s\S]*?)""");
                                Match time = Regex.Match(strhtml, @"""createTime"":""([\s\S]*?)""");

                                if (time.Groups[1].Value != "")
                                {
                                    textBox7.Text += DateTime.Now.ToString("hh:MM:ss") + "：" + titles[i].Groups[1].Value + "\r\n";
                                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                                    lv1.SubItems.Add(titles[i].Groups[1].Value);
                                    lv1.SubItems.Add(titles[i].Groups[3].Value);

                                    lv1.SubItems.Add(ConvertStringToDateTime(time.Groups[1].Value).ToString());
                                    lv1.SubItems.Add("https://m.zhuanzhuan.com/u/zyservice/detail-yyj?infoId=" + ids[i].Groups[1].Value);
                                    if (textBox7.TextLength > 10000)
                                    {
                                        textBox7.Text = "";
                                    }




                                    if (Convert.ToInt64(GetTimeStamp())/1000 - Convert.ToInt64(time.Groups[1].Value) < 200)
                                    {
                                        lv1.BackColor = Color.Red;
                                        MessageBox.Show("出现新上架商品");
                                    }
                                }
                            }
                        }

                        Thread.Sleep(1000);
                    }
                }


            }
            catch (Exception ex)
            {

                ex.ToString();

            }
        }
        private void 转转商品监控_Load(object sender, EventArgs e)
        {

        }
        Thread thread;
     

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"zhuanzhuan"))
            {
                MessageBox.Show("");
                return;
            }



            #endregion
            if (thread == null || !thread.IsAlive)
            {

                thread = new Thread(xiaochengxulist);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            timer1.Start();
            timer1.Interval = Convert.ToInt32(textBox4.Text)*1000;
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                pictureBox1.Image = creatQcode(listView1.SelectedItems[0].SubItems[4].Text);
                textBox7.Text = listView1.SelectedItems[0].SubItems[4].Text;
            }
        }

        private void 清空所有ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lists.Clear();
            listView1.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
            if (thread == null || !thread.IsAlive)
            {

                thread = new Thread(xiaochengxulist);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            if (listView1.Items.Count > Convert.ToInt32(textBox2.Text))
            {
                method.DataTableToExcelTime(method.listViewToDataTable(this.listView1), true); ;
            }

        }

        private void 转转商品监控_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void 保存数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
