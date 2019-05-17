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
        public static string GetUrl(string Url, string charset, string COOKIE)
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.81 Safari/537.36";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                request.KeepAlive = false;
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
        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl()
        {
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = "https://trade.taobao.com/trade/itemlist/asyncSold.htm?event_submit_do_query=1&_input_charset=utf8",//URL     必需项  
                Method = "POST",//URL     可选项 默认为Get  
                Encoding = Encoding.GetEncoding("gb2312"),//编码格式（utf-8,gb2312,gbk）     可选项 默认类会自动识别  
                Timeout = 100000,//连接超时时间     可选项默认为100000  
                ReadWriteTimeout = 30000,//写入Post数据超时时间     可选项默认为30000  
                IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写  
                Cookie = "miid=1280586377935732790; thw=cn; cna=8QJMFUu4DhACATFZv2JYDtwd; t=549c883cd0d96a2361709464aafc2ef7; tg=0; hng=CN%7Czh-CN%7CCNY%7C156; _bl_uid=njj6hv4Xl4aqL0q7entvuUh8gtbv; enc=5BnyFFuDeqJnqQZxA6QyFlyN20GFeiRDc0kkddkfspydsa13DgSJd7cMSqH2bHy0qmM%2BsJvDUWBn%2Bfei0I78CA%3D%3D; _m_h5_tk=da06e51a2a981f23c980f3c5e7cde11a_1557828467488; _m_h5_tk_enc=3d04b61039a426004703a7797a9c54c1; _cc_=W5iHLLyFfA%3D%3D; mt=ci=41_1; cookie2=1e8a19072fad675b0e26975ca81971ac; _tb_token_=ee3eeeb918b3b; x=2992737907; uc3=id2=&nk2=&lg2=; skt=bc10876b3a7e3fb4; sn=%E8%81%94%E9%80%9A%E7%BF%BC%E5%BE%B7%E9%80%9A%E4%BF%A1%E4%B8%93%E5%8D%96%E5%BA%97%3A%E6%A1%94%E5%AD%90; unb=3247690276; tracknick=; csg=6ed74aed; v=0; uc1=cookie14=UoTZ7HUO4iQLBA%3D%3D&lng=zh_CN; l=bBreIJ14vWJ-OvB8BOfNiuIRGZ7T6IOb8sPzw4gGAICPO11wJNH1WZ9eQDTeC3GVa6U2R3oWYJ1uBuYrAyUIh; isg=BCkpA5FIUYbzOm01KxYP7DVpONVDXhVG3yF7yMsf3ZAukkukE0O1-NzEVHYBCrVg",//字符串Cookie     可选项  
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",//用户的浏览器类型，版本，操作系统     可选项有默认值  
                Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值  
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值  
                Referer = "",//来源URL     可选项  
                //Allowautoredirect = False,//是否根据３０１跳转     可选项  
                //AutoRedirectCookie = False,//是否自动处理Cookie     可选项  
                                           //CerPath = "d:\123.cer",//证书绝对路径     可选项不需要证书时可以不写这个参数  
                                           //Connectionlimit = 1024,//最大连接数     可选项 默认为1024  
                Postdata = "auctionType=0&close=0&pageNum=2&pageSize=15&queryMore=false&rxAuditFlag=0&rxElectronicAllFlag=0&rxElectronicAuditFlag=0&rxHasSendFlag=0&rxOldFlag=0&rxSendFlag=0&rxSuccessflag=0&rxWaitSendflag=0&tradeTag=0&useCheckcode=false&useOrderInfo=false&errorCheckcode=false&action=itemlist%2FSoldQueryAction&prePageNo=1",//Post数据     可选项GET时不需要写  
                                                                                                                                                                                                                                                                                                                                                   //ProxyIp = "192.168.1.105：2020",//代理服务器ID     可选项 不需要代理 时可以不设置这三个参数  
                                                                                                                                                                                                                                                                                                                                                   //ProxyPwd = "123456",//代理服务器密码     可选项  
                                                                                                                                                                                                                                                                                                                                                   //ProxyUserName = "administrator",//代理服务器账户名     可选项  
                ResultType = ResultType.String,//返回数据类型，是Byte还是String  
            };
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            string cookie = result.Cookie;
            return html;

        }

        #endregion






        public static string COOKIE = "miid=1280586377935732790; thw=cn; cna=8QJMFUu4DhACATFZv2JYDtwd; t=549c883cd0d96a2361709464aafc2ef7; tg=0; hng=CN%7Czh-CN%7CCNY%7C156; _bl_uid=njj6hv4Xl4aqL0q7entvuUh8gtbv; enc=5BnyFFuDeqJnqQZxA6QyFlyN20GFeiRDc0kkddkfspydsa13DgSJd7cMSqH2bHy0qmM%2BsJvDUWBn%2Bfei0I78CA%3D%3D; _cc_=VFC%2FuZ9ajQ%3D%3D; mt=ci=-1_0&np=; _m_h5_tk=da06e51a2a981f23c980f3c5e7cde11a_1557828467488; _m_h5_tk_enc=3d04b61039a426004703a7797a9c54c1; cookie2=1a953bd75316101f8b16733d6d1410bf; _tb_token_=83ee3731e750; x=2992737907; uc3=id2=&nk2=&lg2=; skt=b46d5bacaace540e; sn=%E8%81%94%E9%80%9A%E7%BF%BC%E5%BE%B7%E9%80%9A%E4%BF%A1%E4%B8%93%E5%8D%96%E5%BA%97%3A%E6%A1%94%E5%AD%90; unb=3247690276; tracknick=; csg=6feba747; v=0; uc1=cookie14=UoTZ48xqWqOx%2BQ%3D%3D&lng=zh_CN; l=bBreIJ14vWJ-OIgfKOfgNuIRGi7O6IOb8sPzw4gGxICP_rfMCiGfWZ9BZ4THC3GVZ66yR3oWYJ1uB28l6yCV.; isg=BBQUxEISNBowOaDSTj1Ko1ii5VJGxTA7Ilr276703B93mbXj1nhJ56RfmdGkYXCv";


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
        public void run()
        {
            if (COOKIE == "")
            {
                MessageBox.Show("请登录账号！");
                return;
            }
            listView1.Items.Clear();
            try
            {
                

                for (int i = 2; i < 100; i++)
                {
                    
                    int prei = i - 1;
                    string url = "https://trade.taobao.com/trade/itemlist/asyncSold.htm?event_submit_do_query=1&_input_charset=utf8";

                    string postdata = "auctionType=0&close=0&pageNum=2&pageSize=15&queryMore=false&rxAuditFlag=0&rxElectronicAllFlag=0&rxElectronicAuditFlag=0&rxHasSendFlag=0&rxOldFlag=0&rxSendFlag=0&rxSuccessflag=0&rxWaitSendflag=0&tradeTag=0&useCheckcode=false&useOrderInfo=false&errorCheckcode=false&action=itemlist%2FSoldQueryAction&prePageNo=1";

                    //string html = PostUrl(url,postdata ,COOKIE, "gb2312");
                    string html = "";
                    textBox1.Text = html;

                    MatchCollection IDs = Regex.Matches(html, @"tradeID=([\s\S]*?)&");
                    MatchCollection users = Regex.Matches(html, @"userID=([\s\S]*?)&");
                    MatchCollection times = Regex.Matches(html, @"createTime"":""([\s\S]*?)""");
                    //MatchCollection users = Regex.Matches(html, @"userID=([\s\S]*?)&");
                    //MatchCollection users = Regex.Matches(html, @"userID=([\s\S]*?)&");
                    if (IDs.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;

                 
                    for (int j = 0; j < IDs.Count; j++)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(IDs[j].Groups[1].Value);
                       
                       

                      
                    }
                    Thread.Sleep(3000);
                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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
            //webBrowser web = new webBrowser("http://crm.58.com/");
            //web.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //textBox1.Text = webBrowser.cookie;
            //COOKIE = textBox1.Text;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           // timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = PostUrl();
            #region   读取注册码信息才能运行软件！

            RegistryKey rsg = Registry.CurrentUser.OpenSubKey("zhucema"); //true表可修改                
            if (rsg != null && rsg.GetValue("mac") != null)  //如果值不为空
            {
                Thread thread = new Thread(new ThreadStart(run));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();

            }

            else
            {
                MessageBox.Show("请注册软件！");
                login lg = new login();
                lg.Show();
            }

            #endregion
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
           
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
