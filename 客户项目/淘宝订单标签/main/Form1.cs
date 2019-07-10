using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using Microsoft.Win32;
using System;
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

namespace main
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string COOKIE)
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.81 Safari/537.36";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                request.KeepAlive = false;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gbk")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

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




        public static string COOKIE = "t=792ea994957bef8e4a71539f91876594; tg=0; _m_h5_tk=80b9d40156f454dcf701dfcad347c4ef_1562227700260; _m_h5_tk_enc=5e99c10aec1a43efcffd88a184daf3ca; thw=cn; _cc_=UtASsssmfA%3D%3D; cookie2=1e8205cf8c8f6ed20bdad5fc8418df4c; _tb_token_=5758e797758e8; mt=ci=0_0; x=2992737907; uc3=id2=&nk2=&lg2=; sn=%E8%81%94%E9%80%9A%E7%BF%BC%E5%BE%B7%E9%80%9A%E4%BF%A1%E4%B8%93%E5%8D%96%E5%BA%97%3A%E5%85%A8%E8%87%AA%E5%8A%A8%E5%AF%BC%E8%B4%AD01; unb=3300260619; tracknick=; cna=8QJMFUu4DhACATFZv2JYDtwd; _bl_uid=10jnOxj9tagx5qodm8jwuOwx3FX0; skt=09efb2d4079ea14b; csg=f2fb0d75; v=0; uc1=cookie14=UoTaGqj6bYDcrA%3D%3D&lng=zh_CN; l=cB_Zr19mqxALdELEBOfNVQhfh87O5IOb8sPP2FyMGICP_-fp7jSNWZn29_T9CnGVLsCDJ3oWYJ1uBu87ty4el8pnv6n7oc-C.; isg=BK6u9Pltf7ta64tf73Kiz3eu_wTcu3ir3Lgc-dh2aLF4u08VQTvbuH35c2fyY2rB";
        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url,string POSTdata)
        {
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = url,//URL     必需项  
                Method = "POST",//URL     可选项 默认为Get  
                Timeout = 100000,//连接超时时间     可选项默认为100000  
                ReadWriteTimeout = 30000,//写入Post数据超时时间     可选项默认为30000  
                IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写  
                Cookie = COOKIE,
                Referer = "https://trade.taobao.com/trade/itemlist/list_sold_items.htm?action=itemlist/SoldQueryAction&event_submit_do_query=1&auctionStatus=PAID&tabCode=waitSend",
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",//用户的浏览器类型，版本，操作系统     可选项有默认值  
                Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值  
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值  
                //Allowautoredirect = False,//是否根据３０１跳转     可选项  
                //AutoRedirectCookie = False,//是否自动处理Cookie     可选项  
                //                           //CerPath = "d:\123.cer",//证书绝对路径     可选项不需要证书时可以不写这个参数  
                                           //Connectionlimit = 1024,//最大连接数     可选项 默认为1024  
                Postdata =POSTdata ,//Post数据     可选项GET时不需要写  
                                                                                                                                                                                                                                                                                                                                                   //ProxyIp = "192.168.1.105：2020",//代理服务器ID     可选项 不需要代理 时可以不设置这三个参数  
                                                                                                                                                                                                                                                                                                                                                   //ProxyPwd = "123456",//代理服务器密码     可选项  
                                                                                                                                                                                                                                                                                                                                                   //ProxyUserName = "administrator",//代理服务器账户名     可选项  
                ResultType = ResultType.String,//返回数据类型，是Byte还是String  
            };
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            return html;

        }

        #endregion

        string path = AppDomain.CurrentDomain.BaseDirectory;

        bool zanting = true;


        public static string getToken()
        {
            string token = "";
            string[] keys = COOKIE.Split(new string[] { ";" }, StringSplitOptions.None);
            foreach (string key in keys)
            {

                if (key.Contains("token"))
                {
                    token = key;
                }
            }
            return token;

        }

        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }
        public void run()
        {
            COOKIE = textBox1.Text;
            if (COOKIE == "")
            {
                MessageBox.Show("请登录账号！");
                return;
            }
            listView1.Items.Clear();
            try
            {

                for (int i = 0; i < 20; i++)
                {

                    int a = i + 1;

                    //string URL = "https://trade.taobao.com/trade/itemlist/list_sold_items.htm?action=itemlist/SoldQueryAction&event_submit_do_query=1&auctionStatus=PAID&tabCode=waitSend";
                    string URL = "https://trade.taobao.com/trade/itemlist/asyncSold.htm?event_submit_do_query=1&_input_charset=utf8";
                    string postdata = "action=itemlist%2FSoldQueryAction&auctionType=0&buyerNick=&close=0&dateBegin=0&dateEnd=0&logisticsService=&orderStatus=PAID&pageNum="+a+"&pageSize=15&queryMore=false&queryOrder=desc&rateStatus=&refund=&rxAuditFlag=0&rxElectronicAllFlag=0&rxElectronicAuditFlag=0&rxHasSendFlag=0&rxOldFlag=0&rxSendFlag=0&rxSuccessflag=0&rxWaitSendflag=0&sellerNick=&tabCode=waitSend&tradeTag=0&useCheckcode=false&useOrderInfo=false&errorCheckcode=false&prePageNo="+i;
                    string html = PostUrl(URL, postdata); ;

                    MatchCollection IDs = Regex.Matches(html, @"tradeID=([\s\S]*?)&");
                    MatchCollection bianmas = Regex.Matches(html, @"商家编码"",""value"":""([\s\S]*?)""");

                    MatchCollection taocans = Regex.Matches(html, @"skuText"":([\s\S]*?)priceInfo");
                    MatchCollection times = Regex.Matches(html, @"createTime"":""([\s\S]*?)""");
                    MatchCollection users = Regex.Matches(html, @"""nick"":""([\s\S]*?)""");
                    if (IDs.Count == 0)
                    {
                        break;
                    }


                    for (int j = 0; j < IDs.Count; j++)
                    {

                       
                        string strhtml = GetUrl("https://trade.tmall.com/detail/orderDetail.htm?bizOrderId=" + IDs[j].Groups[1].Value, COOKIE);

                        if (!strhtml.Contains("备忘："))
                        {
                            MatchCollection xs = Regex.Matches(strhtml, @"\[{""text"":""([\s\S]*?)""");
                            Match card = Regex.Match(strhtml, @"名""}\,{""content"":\[{""text"":""([\s\S]*?)""");

                            string pichtml = GetUrl("https://wt.taobao.com/trade/order/detail_info.do?m=Query&order_id=" + IDs[j].Groups[1].Value + "&mds=authPics,realName,certNo&_input_charset=UTF-8&callback=jsonp22", COOKIE);

                            MatchCollection pics = Regex.Matches(pichtml, @"url"":""([\s\S]*?)""");
                            Match name = Regex.Match(pichtml, @"realName"":""([\s\S]*?)""");

                            if (textBox3.Text == "无")
                            {
                                if (!Directory.Exists(path + bianmas[j].Groups[1].Value))
                                {
                                    Directory.CreateDirectory(path + "图片\\" + bianmas[j].Groups[1].Value + "\\" + IDs[j].Groups[1].Value); //创建文件夹
                                }
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(IDs[j].Groups[1].Value);
                                lv1.SubItems.Add(bianmas[j].Groups[1].Value);
                                lv1.SubItems.Add(Unicode2String(taocans[j].Groups[1].Value));
                                lv1.SubItems.Add(times[j].Groups[1].Value);
                                lv1.SubItems.Add(Unicode2String(users[j].Groups[1].Value));
                               // lv1.SubItems.Add(xs[0].Groups[1].Value);
                                lv1.SubItems.Add(xs[1].Groups[1].Value);
                                lv1.SubItems.Add(name.Groups[1].Value);
                                lv1.SubItems.Add(card.Groups[1].Value);

                                string[] text = xs[0].Groups[1].Value.Split(new string[]{" "}, StringSplitOptions.None);
                               
                                string[] names = text[0].Split(new string[] { "," }, StringSplitOptions.None);
                               
                                lv1.SubItems.Add(names[0]);
                                lv1.SubItems.Add(names[1]);
                                lv1.SubItems.Add(names[2]);
                                lv1.SubItems.Add(text[1]);
                                lv1.SubItems.Add(text[2]);
                                lv1.SubItems.Add(text[3]);
                                lv1.SubItems.Add(text[4]);


                                for (int z = 0; z < pics.Count; z++)
                                {
                                    method.downloadFile(pics[z].Groups[1].Value, path+"图片\\"+ bianmas[j].Groups[1].Value + "\\" + IDs[j].Groups[1].Value + "\\", name.Groups[1].Value + z + ".jpg");
                                }
                                while (this.zanting == false)
                                {
                                    label1.Text = "已暂停....";
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                            }
                            else if (bianmas[j].Groups[1].Value == textBox3.Text.Trim())
                            {
                                if (!Directory.Exists(path + "图片\\" + textBox3.Text + "\\" + IDs[j].Groups[1].Value))
                                {
                                    Directory.CreateDirectory(path + IDs[j].Groups[1].Value); //创建文件夹
                                }
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(IDs[j].Groups[1].Value);
                                lv1.SubItems.Add(bianmas[j].Groups[1].Value);
                                lv1.SubItems.Add(Unicode2String(taocans[j].Groups[1].Value));
                                lv1.SubItems.Add(times[j].Groups[1].Value);
                                lv1.SubItems.Add(Unicode2String(users[j].Groups[1].Value));
                                lv1.SubItems.Add(xs[0].Groups[1].Value);
                                lv1.SubItems.Add(xs[1].Groups[1].Value);
                                lv1.SubItems.Add(name.Groups[1].Value);
                                lv1.SubItems.Add(card.Groups[1].Value);
                                for (int z = 0; z < pics.Count; z++)
                                {
                                    method.downloadFile(pics[z].Groups[1].Value, path + "图片\\" + textBox3.Text + "\\" + IDs[j].Groups[1].Value + "\\", name.Groups[1].Value + z + ".jpg");
                                }
                                while (this.zanting == false)
                                {
                                    label1.Text = "已暂停....";
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                            }

                            biaoji(IDs[j].Groups[1].Value,textBox4.Text);
                        }
                        Thread.Sleep(100);

                    }
                }

             }

            
            catch (System.Exception ex)
            {
             ex.ToString();
            }
        }



        public void biaoji(string orderid,string text)
        {


            string gb2312text= System.Web.HttpUtility.UrlEncode(text, Encoding.GetEncoding("GB2312")); 
            string url = "https://trade.taobao.com/trade/memo/update_sell_memo.htm?spm=a1z09.1.0.0.15c636065NK8kq&seller_id=2992737907&biz_order_id="+ orderid + "&user_type=1&pageNum=1&auctionTitle=null&dateBegin=0&dateEnd=0&commentStatus=&buyerNick=&auctionStatus=PAID&logisticsService=";
           COOKIE = textBox1.Text;
            Match token = Regex.Match(COOKIE, @"_tb_token_=([\s\S]*?);");

            string postdata = "_tb_token_="+token.Groups[1].Value+"&event_submit_do_query=1&action=memo%2FUpdateSellMemoAction&user_type=1&pageNum=1&auctionTitle=&dateBegin=0&dateEnd=0&commentStatus=&buyerNick=&auctionStatus=PAID&returnUrl=&logisticsService=&from_flag=&biz_order_id="+orderid+"&flag=3&memo=" + gb2312text;
         label8.Text= PostUrl(url,postdata);

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_MouseEnter(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            webBrowser web = new webBrowser("https://login.taobao.com/member/login.jhtml");
            web.Show();
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            timer1.Interval = Convert.ToInt32(textBox2.Text)*1000;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("点击了开始");
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }
        public string GetResponseString(HttpWebResponse webresponse)
        {
            using (Stream s = webresponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(s, Encoding.GetEncoding("gb2312"));
                return reader.ReadToEnd();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(getToken());
            zanting = false;
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            
            listView1.Items.Clear();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
