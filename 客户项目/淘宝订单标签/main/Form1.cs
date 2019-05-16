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
        public static string PostUrl(string url, string data, string COOKIE, string charset)
        {
            try
            {
                Encoding myEncoding = Encoding.GetEncoding("gb2312");  //选择编码字符集
                
                byte[] bytesToPost = System.Text.Encoding.Default.GetBytes(data); //转换为bytes数据

                string responseResult = String.Empty;
                HttpWebRequest req = (HttpWebRequest)
                HttpWebRequest.Create("http://192.168.60.59:81/rpc/snBurn/insertBySN");   //创建一个有效的httprequest请求，地址和端口和指定路径必须要和网页系统工程师确认正确，不然一直通讯不成功
                req.Method = "POST";
                req.ContentType =
                "application/x-www-form-urlencoded;charset=gb2312";
                req.ContentLength = bytesToPost.Length;

                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(bytesToPost, 0, bytesToPost.Length);     //把要上传网页系统的数据通过post发送
                }
                HttpWebResponse cnblogsRespone = (HttpWebResponse)req.GetResponse();
                if (cnblogsRespone != null && cnblogsRespone.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader sr;
                    using (sr = new StreamReader(cnblogsRespone.GetResponseStream()))
                    {
                        responseResult = sr.ReadToEnd();  //网页系统的json格式的返回值，在responseResult里，具体内容就是网页系统负责工程师跟你协议号的返回值协议内容
                    }
                    sr.Close();
                }
                cnblogsRespone.Close();
           
                return responseResult;
            }
            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
                return "";
            }
               
           
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
                    
                    string html = PostUrl(url,postdata ,COOKIE, "gb2312");

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
            string url = "https://trade.taobao.com/trade/itemlist/asyncSold.htm?event_submit_do_query=1&_input_charset=utf8";
            string a = "t=db807e67e00d0d1d34419cc5686c31bc; cna=/h4DE4EwWXQCATFGW6Fbj0Jl; tg=0; ali_ab=121.234.247.249.1523710505053.9; miid=8025647021775416888; enc=Qvp4zg9jdtdHD1eYDtKtSf6ngTXgX08BdN7Dh2sE57Panvcro2PQTTCMYP8qEJPYaRtY87A48OPrPiBToNy7ng%3D%3D; UM_distinctid=166869b31df766-0fb021c488ef24-5701631-1fa400-166869b31e06de; thw=cn; hng=CN%7Czh-CN%7CCNY%7C156; cookie2=155ebcae8117344d5aaa01774b98ec12; _tb_token_=7e7333bf76e01; _m_h5_tk=ce4086de229a40aba186cc02932e6754_1555390578639; _m_h5_tk_enc=c604ff113d2a6ee9fd3109d69b24d615; _cc_=URm48syIZQ%3D%3D; mt=ci=42_1; x=4062482443; uc3=id2=&nk2=&lg2=; skt=acacbab3163ef025; sn=%E5%A7%BF%E5%BD%A9%E4%BC%98%E5%93%81%E5%BD%B1%E9%9F%B3%E4%B8%93%E8%90%A5%E5%BA%97%3A%E5%95%8A%E5%81%A5; unb=2200717632773; tracknick=; csg=cc6b51a5; v=0; _bl_uid=zdjqmuapjeC8pec5CuaIbI600qw5; uc1=cookie14=UoTZ4SbxpZochQ%3D%3D&lng=zh_CN; apush4cf74727f26295a45be226fa020c560c=%7B%22ts%22%3A1555404123245%2C%22heir%22%3A1555385893208%2C%22parentId%22%3A1555385486009%7D; l=bBQGJtKevGyHoP2wBOfwquI8arb93KAX1sPzw4_GVIB1t_XTodAuxHwpfAI293Q_E_5K2exPJiyvlRFeyzU38x1..; isg=BAEBal28OeWaCVXRnw2UE2eSEE3bhnVGXrwm6mNe74qUSi8cqXo08T0MLP6pwg1Y";
            string postdata = "action=itemlist%2FSoldQueryAction&auctionType=0&buyerNick=&close=0&dateBegin=0&dateEnd=0&logisticsService=&orderStatus=PAID&pageNum=6&pageSize=15&queryMore=false&queryOrder=desc&rateStatus=&refund=&rxAuditFlag=0&rxElectronicAllFlag=0&rxElectronicAuditFlag=0&rxHasSendFlag=0&rxOldFlag=0&rxSendFlag=0&rxSuccessflag=0&rxWaitSendflag=0&sellerNick=&tabCode=waitSend&tradeTag=0&useCheckcode=false&useOrderInfo=false&errorCheckcode=false&prePageNo=5";

            string html = PostUrl(url, postdata, a, "gb2312");
           


            textBox1.Text = html;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
