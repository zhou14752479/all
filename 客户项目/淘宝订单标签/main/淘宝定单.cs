using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
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
    public partial class 淘宝定单 : Form
    {
        public 淘宝定单()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel1_MouseEnter(object sender, EventArgs e)
        {

        }

        private void 淘宝定单_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;

        }

        public static string COOKIE = "";
        bool zanting = true;
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

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string POSTdata)
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
                Postdata = POSTdata,//Post数据     可选项GET时不需要写  
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
        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }
        /// <summary>
        /// 等待发货
        /// </summary>
        public void run()
        {

           

                try
                {
                for (int a = 0; a < listView2.Items.Count; a++)
                {
                    COOKIE = listView2.Items[a].SubItems[1].Text;
                    if (COOKIE == "")
                    {
                        MessageBox.Show("请登录账号！");
                        return;
                    }
                    for (int i = 0; i < 20; i++)
                    {


                        string URL = "https://trade.taobao.com/trade/itemlist/asyncSold.htm?event_submit_do_query=1&_input_charset=utf8";
                        string postdata = "action=itemlist%2FSoldQueryAction&auctionType=0&buyerNick=&close=0&dateBegin=0&dateEnd=0&logisticsService=&orderStatus=PAID&pageNum=" + i + "&pageSize=15&queryMore=false&queryOrder=desc&rateStatus=&refund=&rxAuditFlag=0&rxElectronicAllFlag=0&rxElectronicAuditFlag=0&rxHasSendFlag=0&rxOldFlag=0&rxSendFlag=0&rxSuccessflag=0&rxWaitSendflag=0&sellerNick=&tabCode=waitSend&tradeTag=0&useCheckcode=false&useOrderInfo=false&errorCheckcode=false&prePageNo=" + i;
                        string html = PostUrl(URL, postdata); ;

                        MatchCollection IDs = Regex.Matches(html, @"tradeID=([\s\S]*?)&");

                        MatchCollection times = Regex.Matches(html, @"createTime"":""([\s\S]*?)""");
                        MatchCollection users = Regex.Matches(html, @"""actualFee"":""([\s\S]*?)""");
                        if (IDs.Count == 0)
                        {
                            break;
                        }


                        for (int j = 0; j < IDs.Count; j++)
                        {

                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(IDs[j].Groups[1].Value);
                            lv1.SubItems.Add(times[j].Groups[1].Value);
                            lv1.SubItems.Add(Unicode2String(users[j].Groups[1].Value));



                            while (this.zanting == false)
                            {
                                //label1.Text = "已暂停....";
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }

                            Thread.Sleep(100);

                        }
                    }

                }
                }
                


            catch (System.Exception ex)
            {
                ex.ToString();
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            webBrowser web = new webBrowser("https://login.taobao.com/member/login.jhtml");
            web.Show();
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count + 1).ToString()); //使用Listview展示数据
            lv2.SubItems.Add(webBrowser.cookie);
        }
    }
}
