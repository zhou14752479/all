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
        public static string PostUrl(string url, string postData, string COOKIE, string charset)
        {
            
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
               request.ContentType = "application/x-www-form-urlencoded";
                 //request.ContentType = "application/json";
                request.Accept = "*/*";     
               request.ContentLength = postData.Length;
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.81 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();
                WebResponse response = request.GetResponse();
                Stream s = response.GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.GetEncoding(charset));
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






        public static string COOKIE = "t=db807e67e00d0d1d34419cc5686c31bc; cna=/h4DE4EwWXQCATFGW6Fbj0Jl; tg=0; ali_ab=121.234.247.249.1523710505053.9; miid=8025647021775416888; enc=Qvp4zg9jdtdHD1eYDtKtSf6ngTXgX08BdN7Dh2sE57Panvcro2PQTTCMYP8qEJPYaRtY87A48OPrPiBToNy7ng%3D%3D; UM_distinctid=166869b31df766-0fb021c488ef24-5701631-1fa400-166869b31e06de; thw=cn; hng=CN%7Czh-CN%7CCNY%7C156; cookie2=155ebcae8117344d5aaa01774b98ec12; _tb_token_=7e7333bf76e01; _m_h5_tk=ce4086de229a40aba186cc02932e6754_1555390578639; _m_h5_tk_enc=c604ff113d2a6ee9fd3109d69b24d615; _cc_=URm48syIZQ%3D%3D; mt=ci=42_1; x=4062482443; uc3=id2=&nk2=&lg2=; skt=acacbab3163ef025; sn=%E5%A7%BF%E5%BD%A9%E4%BC%98%E5%93%81%E5%BD%B1%E9%9F%B3%E4%B8%93%E8%90%A5%E5%BA%97%3A%E5%95%8A%E5%81%A5; unb=2200717632773; tracknick=; csg=cc6b51a5; v=0; _bl_uid=zdjqmuapjeC8pec5CuaIbI600qw5; uc1=cookie14=UoTZ4SbxpZochQ%3D%3D&lng=zh_CN; apush4cf74727f26295a45be226fa020c560c=%7B%22ts%22%3A1555404123245%2C%22heir%22%3A1555385893208%2C%22parentId%22%3A1555385486009%7D; l=bBQGJtKevGyHoP2wBOfwquI8arb93KAX1sPzw4_GVIB1t_XTodAuxHwpfAI293Q_E_5K2exPJiyvlRFeyzU38x1..; isg=BAEBal28OeWaCVXRnw2UE2eSEE3bhnVGXrwm6mNe74qUSi8cqXo08T0MLP6pwg1Y";
        
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
                string orderStatus = "PAID";
                string type = "waitSend";

                for (int i = 2; i < 100; i++)
                {
                    int prei = i - 1;
                    string url = "https://trade.taobao.com/trade/itemlist/asyncSold.htm?event_submit_do_query=1&_input_charset=utf8";
                    
                    string postdata= "action=itemlist%2FSoldQueryAction&auctionType=0&buyerNick=&close=0&dateBegin=0&dateEnd=0&logisticsService=&orderStatus="+ orderStatus + "&pageNum="+i+"&pageSize=15&queryMore=false&queryOrder=desc&rateStatus=&refund=&rxAuditFlag=0&rxElectronicAllFlag=0&rxElectronicAuditFlag=0&rxHasSendFlag=0&rxOldFlag=0&rxSendFlag=0&rxSuccessflag=0&rxWaitSendflag=0&sellerNick=&tabCode="+type+"&tradeTag=0&useCheckcode=false&useOrderInfo=false&errorCheckcode=false&prePageNo="+prei.ToString();
                    
                    string html = PostUrl(url,postdata ,COOKIE, "utf-8");

                    textBox1.Text = html;


                    string[] strhtmls = html.Split(new string[] { "\"buyer\"" }, StringSplitOptions.None);
                    MatchCollection IDs = Regex.Matches(html, @"&bizOrderId=([\s\S]*?)&");
                    if (IDs.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;

                    foreach (string strhtml in strhtmls)
                    {
                        if (strhtml.Contains("超级会员"))
                        {
                            
                            Match ID = Regex.Match(strhtml, @"&bizOrderId=([\s\S]*?)&");
                            Match titles = Regex.Match(strhtml, @"""nick"":""([\s\S]*?)""");
                            MessageBox.Show(titles.Groups[1].Value);
                        }
                       
                    }

                    

                    

                    //for (int j = 0; j < IDs.Count; j++)
                    //{
                    //    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    //    lv1.SubItems.Add(IDs[j].Groups[1].Value);
                    //    lv1.SubItems.Add(titles[j].Groups[1].Value);
                    //    while (this.zanting == false)
                    //    {
                    //        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    //    }
                    //    if (listView1.Items.Count - 1 > 1)
                    //    {
                    //        listView1.EnsureVisible(listView1.Items.Count - 1);
                    //    }

                    //    if (status == false)
                    //    {
                    //        return;
                    //    }
                    //}
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
            //webBrowser web = new webBrowser("https://login.taobao.com/member/login.jhtml");
            //web.Show();
            webBrowser web = new webBrowser("http://crm.58.com/");
            web.Show();
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
