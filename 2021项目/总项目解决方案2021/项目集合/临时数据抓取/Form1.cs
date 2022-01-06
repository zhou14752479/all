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
using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using myDLL;

namespace 临时数据抓取
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Thread thread;
        bool zanting = true;
        bool status = true;

        string COOKIE = "_gscu_2075282647=225902965kvn2d59; wIlwQR28aVgbS=5z.MH5j7dbLRhF4GMzL4b57NpM5uwV5k7_02wgiGN58Pd8xO6da2c_5dksr8j1ExukvT.YrTSZb9w1Gv8uJqkoq; WEB=20111132; _gscbrs_2075282647=1; _gscs_2075282647=26142813zt6ofv20|pv:15; wIlwQR28aVgbT=53iuT2Ck75waqqqm_vF63RGR4dGOGmEdyzOMgLtrGo1nWu.KYM81i_P6URcVnE97DjBEvmoTobrs2iGcRiMheaBAqB7FyJIae4uEuxKt7yC35LiNL8i1VJ88jyPUlRazzjWvKNNzzNRpIHlUwkyG1tCvzeifn4.pt2Rev.5nAO4ysi_FUTILj.k3__qATcWfZxqKFwzjYbG31bn..mNpxTTjHXF9wTRJsImMTWvb5kfR9rMhPaDObZ3rd.76Moum0n6bcMts0fD_.Ag7iwEV4Vj.Pvs7TUczdJ3P9JBurRHkA_0CouSKtGYF8CuC8KStzcKjr0iwadb7L.pzfnKw8aW";
        string[] agents = {
  "Mozilla/5.0 (Linux; U; Android 2.3.6; en-us; Nexus S Build/GRK39F) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1",
  "Avant Browser/1.2.789rel1 (http://www.avantbrowser.com)",
  "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/532.5 (KHTML, like Gecko) Chrome/4.0.249.0 Safari/532.5",
  "Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/532.9 (KHTML, like Gecko) Chrome/5.0.310.0 Safari/532.9",
  "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.7 (KHTML, like Gecko) Chrome/7.0.514.0 Safari/534.7",
  "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/534.14 (KHTML, like Gecko) Chrome/9.0.601.0 Safari/534.14",
  "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.14 (KHTML, like Gecko) Chrome/10.0.601.0 Safari/534.14",
  "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.20 (KHTML, like Gecko) Chrome/11.0.672.2 Safari/534.20",
  "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/534.27 (KHTML, like Gecko) Chrome/12.0.712.0 Safari/534.27",
  "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.24 Safari/535.1",
  "Mozilla/5.0 (Windows NT 6.0) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.120 Safari/535.2",
  "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.7 (KHTML, like Gecko) Chrome/16.0.912.36 Safari/535.7",
  "Mozilla/5.0 (Windows; U; Windows NT 6.0 x64; en-US; rv:1.9pre) Gecko/2008072421 Minefield/3.0.2pre",
  "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.0.10) Gecko/2009042316 Firefox/3.0.10",
  "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-GB; rv:1.9.0.11) Gecko/2009060215 Firefox/3.0.11 (.NET CLR 3.5.30729)",
  "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.1.6) Gecko/20091201 Firefox/3.5.6 GTB5",
  "Mozilla/5.0 (Windows; U; Windows NT 5.1; tr; rv:1.9.2.8) Gecko/20100722 Firefox/3.6.8 ( .NET CLR 3.5.30729; .NET4.0E)",
  "Mozilla/5.0 (Windows NT 6.1; rv:2.0.1) Gecko/20100101 Firefox/4.0.1",
  "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:2.0.1) Gecko/20100101 Firefox/4.0.1",
  "Mozilla/5.0 (Windows NT 5.1; rv:5.0) Gecko/20100101 Firefox/5.0",
  "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:6.0a2) Gecko/20110622 Firefox/6.0a2",
  "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:7.0.1) Gecko/20100101 Firefox/7.0.1",
  "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:2.0b4pre) Gecko/20100815 Minefield/4.0b4pre",
  "Mozilla/4.0 (compatible; MSIE 5.5; Windows NT 5.0 )",
  "Mozilla/4.0 (compatible; MSIE 5.5; Windows 98; Win 9x 4.90)",
  "Mozilla/5.0 (Windows; U; Windows XP) Gecko MultiZilla/1.6.1.0a",
  "Mozilla/2.02E (Win95; U)",
  "Mozilla/3.01Gold (Win95; I)",
  "Mozilla/4.8 [en] (Windows NT 5.1; U)",
  "Mozilla/5.0 (Windows; U; Win98; en-US; rv:1.4) Gecko Netscape/7.1 (ax)",
  "HTC_Dream Mozilla/5.0 (Linux; U; Android 1.5; en-ca; Build/CUPCAKE) AppleWebKit/528.5  (KHTML, like Gecko) Version/3.1.2 Mobile Safari/525.20.1",
  "Mozilla/5.0 (hp-tablet; Linux; hpwOS/3.0.2; U; de-DE) AppleWebKit/534.6 (KHTML, like Gecko) wOSBrowser/234.40.1 Safari/534.6 TouchPad/1.0",
  "Mozilla/5.0 (Linux; U; Android 1.5; en-us; sdk Build/CUPCAKE) AppleWebkit/528.5  (KHTML, like Gecko) Version/3.1.2 Mobile Safari/525.20.1",
  "Mozilla/5.0 (Linux; U; Android 2.1; en-us; Nexus One Build/ERD62) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17",
  "Mozilla/5.0 (Linux; U; Android 2.2; en-us; Nexus One Build/FRF91) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1",
  "Mozilla/5.0 (Linux; U; Android 1.5; en-us; htc_bahamas Build/CRB17) AppleWebKit/528.5  (KHTML, like Gecko) Version/3.1.2 Mobile Safari/525.20.1",
  "Mozilla/5.0 (Linux; U; Android 2.1-update1; de-de; HTC Desire 1.19.161.5 Build/ERE27) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17",
  "Mozilla/5.0 (Linux; U; Android 2.2; en-us; Sprint APA9292KT Build/FRF91) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1",
  "Mozilla/5.0 (Linux; U; Android 1.5; de-ch; HTC Hero Build/CUPCAKE) AppleWebKit/528.5  (KHTML, like Gecko) Version/3.1.2 Mobile Safari/525.20.1",
  "Mozilla/5.0 (Linux; U; Android 2.2; en-us; ADR6300 Build/FRF91) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1",
  "Mozilla/5.0 (Linux; U; Android 2.1; en-us; HTC Legend Build/cupcake) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17",
  "Mozilla/5.0 (Linux; U; Android 1.5; de-de; HTC Magic Build/PLAT-RC33) AppleWebKit/528.5  (KHTML, like Gecko) Version/3.1.2 Mobile Safari/525.20.1 FirePHP/0.3",
  "Mozilla/5.0 (Linux; U; Android 1.6; en-us; HTC_TATTOO_A3288 Build/DRC79) AppleWebKit/528.5  (KHTML, like Gecko) Version/3.1.2 Mobile Safari/525.20.1",
  "Mozilla/5.0 (Linux; U; Android 1.0; en-us; dream) AppleWebKit/525.10  (KHTML, like Gecko) Version/3.0.4 Mobile Safari/523.12.2",
  "Mozilla/5.0 (Linux; U; Android 1.5; en-us; T-Mobile G1 Build/CRB43) AppleWebKit/528.5  (KHTML, like Gecko) Version/3.1.2 Mobile Safari 525.20.1",
  "Mozilla/5.0 (Linux; U; Android 1.5; en-gb; T-Mobile_G2_Touch Build/CUPCAKE) AppleWebKit/528.5  (KHTML, like Gecko) Version/3.1.2 Mobile Safari/525.20.1",
  "Mozilla/5.0 (Linux; U; Android 2.0; en-us; Droid Build/ESD20) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17",
  "Mozilla/5.0 (Linux; U; Android 2.2; en-us; Droid Build/FRG22D) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1",
  "Mozilla/5.0 (Linux; U; Android 2.0; en-us; Milestone Build/ SHOLS_U2_01.03.1) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17",
  "Mozilla/5.0 (Linux; U; Android 2.0.1; de-de; Milestone Build/SHOLS_U2_01.14.0) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17",
  "Mozilla/5.0 (Linux; U; Android 3.0; en-us; Xoom Build/HRI39) AppleWebKit/525.10  (KHTML, like Gecko) Version/3.0.4 Mobile Safari/523.12.2",
  "Mozilla/5.0 (Linux; U; Android 0.5; en-us) AppleWebKit/522  (KHTML, like Gecko) Safari/419.3",
  "Mozilla/5.0 (Linux; U; Android 1.1; en-gb; dream) AppleWebKit/525.10  (KHTML, like Gecko) Version/3.0.4 Mobile Safari/523.12.2",
  "Mozilla/5.0 (Linux; U; Android 2.0; en-us; Droid Build/ESD20) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17",
  "Mozilla/5.0 (Linux; U; Android 2.1; en-us; Nexus One Build/ERD62) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17",
  "Mozilla/5.0 (Linux; U; Android 2.2; en-us; Sprint APA9292KT Build/FRF91) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1",
  "Mozilla/5.0 (Linux; U; Android 2.2; en-us; ADR6300 Build/FRF91) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1",
  "Mozilla/5.0 (Linux; U; Android 2.2; en-ca; GT-P1000M Build/FROYO) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1",
  "Mozilla/5.0 (Linux; U; Android 3.0.1; fr-fr; A500 Build/HRI66) AppleWebKit/534.13 (KHTML, like Gecko) Version/4.0 Safari/534.13",
  "Mozilla/5.0 (Linux; U; Android 3.0; en-us; Xoom Build/HRI39) AppleWebKit/525.10  (KHTML, like Gecko) Version/3.0.4 Mobile Safari/523.12.2",
  "Mozilla/5.0 (Linux; U; Android 1.6; es-es; SonyEricssonX10i Build/R1FA016) AppleWebKit/528.5  (KHTML, like Gecko) Version/3.1.2 Mobile Safari/525.20.1",
  "Mozilla/5.0 (Linux; U; Android 1.6; en-us; SonyEricssonX10i Build/R1AA056) AppleWebKit/528.5  (KHTML, like Gecko) Version/3.1.2 Mobile Safari/525.20.1",
};

        #region GET请求带COOKIE
        /// <summary>
        /// GET请求带COOKIE
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public string GetUrlWithCookie(string Url, string COOKIE, string charset)
        {
            try
            {

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                                                                                  //添加头部
                WebHeaderCollection headers = request.Headers;

                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.141 Safari/537.36";

                request.Referer = "";
                request.Headers.Add("Cookie", COOKIE);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                request.Accept = "*/*";
                request.Timeout = 10000;
                string content = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return "";


            }

        }
        #endregion

        private string GetHttp20210301174102(string url)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = url,//URL     必需项  
                Method = "GET",//URL     可选项 默认为Get  
                Timeout = 100000,//连接超时时间     可选项默认为100000  
                ReadWriteTimeout = 30000,//写入Post数据超时时间     可选项默认为30000  
                IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写  
                Cookie = "",//字符串Cookie     可选项  
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",//用户的浏览器类型，版本，操作系统     可选项有默认值  
                Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值  
                ContentType = "text/html",//返回类型    可选项有默认值  
                Referer = "http://www.sufeinet.com",//来源URL     可选项  

                //CerPath = "d:\123.cer",//证书绝对路径     可选项不需要证书时可以不写这个参数  
                //Connectionlimit = 1024,//最大连接数     可选项 默认为1024  
                Postdata = "",//Post数据     可选项GET时不需要写  
                ProxyIp = "tps115.kdlapi.com:15818",//代理服务器ID     可选项 不需要代理 时可以不设置这三个参数  
                                                    //ProxyPwd = "123456",//代理服务器密码     可选项  
                                                    //ProxyUserName = "administrator",//代理服务器账户名     可选项  
                ResultType = ResultType.String,//返回数据类型，是Byte还是String  
            };
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            return html;
        }



        #region tecalliance  检测UA
        public void tecalliance()
        {


            for (int i = 0; i < richTextBox1.Lines.Length; i++)
            {

                string n = richTextBox1.Lines[i].Trim();
                if (richTextBox1.Lines[i].Trim().Length < 5)
                {
                    n = "0" + richTextBox1.Lines[i].Trim();

                }

                string url = "https://www.tecalliance.cn/cn/search/1?q=" + n + "&lbid=101&base=lbid";



                string html = GetHttp20210301174102(url);
                if (html.Trim() == "")
                {
                    MessageBox.Show("验证");
                    i = i - 1;
                    continue;
                }
                Match num = Regex.Match(html, @"通过产品号查询:([\s\S]*?)</div>");

                Match html1 = Regex.Match(html, @"<div class=""part-detail-item-body"" >([\s\S]*?)<div class=""m-sec"">");
                Match html2 = Regex.Match(html, @"<div class=""m-sec"">([\s\S]*?)info-body end");
                Match img = Regex.Match(html, @"class=""brand-img"" src=""([\s\S]*?)""");

                MatchCollection oems = Regex.Matches(html1.Groups[1].Value, @"<a href=""([\s\S]*?)"">([\s\S]*?)</a>");
                MatchCollection canshus = Regex.Matches(html2.Groups[1].Value, @"<span>([\s\S]*?)</span>([\s\S]*?)<strong>([\s\S]*?)</strong>");

                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < oems.Count; j++)
                {

                    sb.Append(oems[j].Groups[2].Value.Trim() + "/");
                }

                StringBuilder sb1 = new StringBuilder();

                for (int a = 0; a < canshus.Count; a++)
                {

                    sb1.Append(canshus[a].Groups[1].Value.Trim() + canshus[a].Groups[3].Value.Trim() + "/");
                }

                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                lv1.SubItems.Add(n);
                lv1.SubItems.Add(num.Groups[1].Value.Trim());
                lv1.SubItems.Add(sb.ToString());
                lv1.SubItems.Add(sb1.ToString());
                lv1.SubItems.Add(img.Groups[1].Value.Trim());

                while (this.zanting == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }
                if (status == false)
                    return;
            }


        }

        #endregion


        #region rockauto.com
        public void rockauto()
        {
            // string ip = getip();
            string ip = "tps115.kdlapi.com:15818";
            for (int i = 0; i < richTextBox1.Lines.Length; i++)
            {
                string n = richTextBox1.Lines[i].Trim();


                string url = "https://www.rockauto.com/en/parts/mevotech," + n;

                //string html = method.GetUrlwithIP(url, ip, "", "utf-8");
                string html = GetHttp20210301174102(url);

                //string html = method.GetUrl(url, "utf-8");

                Match oem = Regex.Match(html, @"OE Part Numbers"">([\s\S]*?)</span>");
                Match img = Regex.Match(html, @"info\\\/([\s\S]*?)&");

                //if (oem.Groups[1].Value.Trim()==""|| html.Trim() == "")
                //{
                //    ip = getip();
                //    i = i - 1;
                //    continue;
                //}


                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                lv1.SubItems.Add(n);
                lv1.SubItems.Add(" ");
                lv1.SubItems.Add(oem.Groups[1].Value.Trim());
                lv1.SubItems.Add("");
                lv1.SubItems.Add("http://www.rockauto.com/info/" + img.Groups[1].Value.Replace("__ra_t", "").Replace("\\", "").Trim());

                while (this.zanting == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }
                if (status == false)
                    return;

            }


        }

        #endregion

        #region rockauto.com进入详情页
        public void rockauto1()
        {


            for (int i = 0; i < richTextBox1.Lines.Length; i++)
            {
                string n = richTextBox1.Lines[i].Trim();


                string url = "https://www.rockauto.com/en/parts/mevotech," + n;



                string html = method.GetUrl(url, "utf-8");

                Match uid = Regex.Match(html, @"moreinfo\.php([\s\S]*?)""");


                if (uid.Groups[1].Value != "")
                {
                    string aurl = "https://www.rockauto.com/en/moreinfo.php" + uid.Groups[1].Value;
                    string ahtml = GetUrlWithCookie(aurl, "", "utf-8");
                    Match oem = Regex.Match(ahtml, @"Number\(s\):([\s\S]*?)</td>");
                    Match img = Regex.Match(ahtml, @"info\\\/([\s\S]*?)&");
                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(n);
                    lv1.SubItems.Add(" ");
                    lv1.SubItems.Add(oem.Groups[1].Value.Trim());
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("http://www.rockauto.com/info/" + img.Groups[1].Value.Replace("__ra_t", "").Replace("\\", "").Trim());
                    Thread.Sleep(100);
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;
                }
                else
                {
                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(n);

                }
            }


        }

        #endregion



        public string getip()
        {
            // string html = method.GetUrl("http://47.106.170.4:8081/Index-generate_api_url.html?packid=7&fa=5&groupid=0&fetch_key=&qty=1&port=1&format=txt&ss=1&css=&pro=&city=&usertype=7", "utf-8");
            string html = method.GetUrl("http://47.106.170.4:8081/Index-generate_api_url.html?packid=7&fa=5&groupid=0&fetch_key=&qty=1&port=1&format=txt&ss=1&css=&pro=&city=&usertype=7", "utf-8");
            label1.Text = html;
            return html;
        }


        #region tecalliance去重
        public void tecalliancequchong()
        {

            for (int i = 0; i < richTextBox1.Lines.Length; i++)
            {
                string n = richTextBox1.Lines[i].Trim();
                string[] text = n.Split(new string[] { "/" }, StringSplitOptions.None);
                ArrayList list = new ArrayList();
                StringBuilder sb = new StringBuilder();
                foreach (var item in text)
                {
                    if (!list.Contains(item) && item != "")
                    {
                        list.Add(item);
                        sb.Append(item + "/");
                    }

                }

                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                lv1.SubItems.Add(sb.ToString());





            }


        }

        #endregion

        #region Dorman.com
        public void Dorman()
        {

            string ip = getip();
            for (int i = 0; i < richTextBox1.Lines.Length; i++)
            {
                string n = richTextBox1.Lines[i].Trim();


                string url = "https://www.dormanproducts.com/gsearch.aspx?type=oesearch&origin=oesearch&q=" + n;

                string html = method.GetUrlwithIP(url, ip, "", "utf-8");

                if (html.Contains("SecurityForm") || html.Trim() == "")
                {
                    ip = getip();
                    i = i - 1;
                    continue;
                }

                MatchCollection dormanpartnumbers = Regex.Matches(html, @"class=""item-name"">([\s\S]*?)</span>");
                Match nums = Regex.Match(html, @"<th scope=""row"">([\s\S]*?)</th>");
                MatchCollection SYDs = Regex.Matches(html, @"<td style=""width:50%"">S([\s\S]*?)</td>");


                Match descriptions = Regex.Match(html, @"<div style=""width:100%; display:block;"">([\s\S]*?)<h4>([\s\S]*?)</h4>");
                Match uid = Regex.Match(html, @"<h2 class=""item-headline"">([\s\S]*?)<a href=""([\s\S]*?)""");
                MatchCollection imgs = Regex.Matches(html, @"<div class=""searchItems-img"">([\s\S]*?)<img src=""([\s\S]*?)""");
                string wutu = " ";
                StringBuilder sb = new StringBuilder();
                // StringBuilder sb1 = new StringBuilder();
                StringBuilder sb2 = new StringBuilder();
                for (int j = 0; j < SYDs.Count; j++)
                {
                    try
                    {
                        sb.Append(dormanpartnumbers[j].Groups[1].Value + ",");
                        // sb1.Append(imgs[j].Groups[2].Value + ",");
                    }
                    catch (Exception)
                    {
                        wutu = "1";
                        continue;
                    }


                }

                //    string aurl = "https://www.dormanproducts.com/" + uid.Groups[2].Value;
                //    string ahtml = method.GetUrlwithIP(aurl, ip, "", "utf-8");
                //if (ahtml.Contains("SecurityForm") ||ahtml.Trim() == "")
                //{
                //    ip = getip();
                //    ahtml = method.GetUrlwithIP(aurl, ip, "", "utf-8");
                //}
                //MatchCollection oems = Regex.Matches(ahtml, @"<th scope=""row"">([\s\S]*?)</th>");

                //for (int a = 0; a < oems.Count; a++)
                //{
                //    sb2.Append(oems[a].Groups[1].Value + ",");

                //}

                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                lv1.SubItems.Add(n);
                lv1.SubItems.Add(nums.Groups[1].Value);
                lv1.SubItems.Add(sb.ToString());
                lv1.SubItems.Add(descriptions.Groups[2].Value);
                lv1.SubItems.Add(sb2.ToString());
                lv1.SubItems.Add(wutu);

                Thread.Sleep(100);
                while (this.zanting == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }
                if (status == false)
                    return;


            }


        }

        #endregion


        #region tecdoc.com
        public void tecdoc()
        {


            for (int i = 0; i < richTextBox1.Lines.Length; i++)
            {
                string n = richTextBox1.Lines[i].Trim();
                label1.Text = i.ToString();

                string url = "https://mx.tecdoc.net/search?q=" + n;
               
                string html = method.GetUrl(url, "utf-8");


                MatchCollection aurls = Regex.Matches(html, @"<h4>([\s\S]*?)<a href=""([\s\S]*?)""");

                for (int j = 0; j < aurls.Count; j++)
                {
                    try
                    {
                        string aurl = "https://mx.tecdoc.net" + aurls[j].Groups[2].Value;

                        string ahtml = method.GetUrl(aurl, "utf-8");
                        string title = Regex.Match(ahtml, @"<h1 itemprop=""name"" class=""media-heading"">([\s\S]*?)<").Groups[1].Value.Trim();
                        string Number = Regex.Match(ahtml, @"<span itemprop=""mpn"">([\s\S]*?)<").Groups[1].Value.Trim();
                        string oem = Regex.Match(ahtml, @"OE Number</th>([\s\S]*?)</b></td>([\s\S]*?)</td>").Groups[2].Value.Trim();

                   
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(n);
                        lv1.SubItems.Add(Number);
                        lv1.SubItems.Add(title);
                        lv1.SubItems.Add(Regex.Replace(oem, "<[^>]+>", ""));

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        Thread.Sleep(1000);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        continue;
                    }


                }


            }


        }

        #endregion

        #region tecdoc.com品牌下全部数据
        public void tecdoc_all()
        {


             
              
                string url = "https://mx.tecdoc.net/search?b=2100";

                string html = method.GetUrl(url, "utf-8");

         
            MatchCollection gas = Regex.Matches(Regex.Match(html, @"Part Type <small>([\s\S]*?)</select>").Groups[1].Value.Trim(), @"<option value=""([\s\S]*?)""");
          
                for (int j = 0; j <gas.Count; j++)
                {

                for (int page = 1; page < 101; page++)
                {
                    textBox2.Text = gas[j].Groups[1].Value;

                    if (gas[j].Groups[1].Value == "")
                        continue;
                    string aurl = "https://mx.tecdoc.net/search?b=2100&ga="+gas[j].Groups[1].Value+"&p="+page;
                    string ahtml = method.GetUrl(aurl, "utf-8");
                 
                    MatchCollection producturls= Regex.Matches(ahtml, @"<h4>([\s\S]*?)<a href=""([\s\S]*?)""");
                 
                    if (producturls.Count == 0)
                        break;

                    foreach (Match producturl in producturls)
                    {
                       
                            string purl = "https://mx.tecdoc.net" + producturl.Groups[2].Value + "&lang=en-US";
                        label1.Text = purl;
                        string producthtml = method.GetUrl(purl, "utf-8");
                        string Brand = Regex.Match(producthtml, @"<span itemprop=""brand"">([\s\S]*?)<").Groups[1].Value.Trim();
                        string partnum = Regex.Match(producthtml, @"<span itemprop=""mpn"">([\s\S]*?)<").Groups[1].Value.Trim();
                        string PartType = Regex.Match(producthtml, @"Part Type:</strong></small>([\s\S]*?)<").Groups[1].Value.Trim();
                        string partstatus = Regex.Match(producthtml, @"data-placement=""right"" title=""([\s\S]*?)""").Groups[1].Value.Trim();


                        StringBuilder oesb = new StringBuilder();



                        string[] oehtmls = producthtml.Split(new string[] { "td rowspan=" }, StringSplitOptions.None);
                        foreach (var oehtml in oehtmls)
                        {
                            MatchCollection oenums = Regex.Matches(oehtml, @"<a href=""/search\?q=([\s\S]*?)""");
                           string oe_pinpai = Regex.Match(oehtml, @"<b>([\s\S]*?)</b>").Groups[1].Value;
                            if(oenums.Count>0)
                            {
                                oesb.Append(oe_pinpai+":");
                                for (int i = 0; i < oenums.Count; i++)
                                {
                                    oesb.Append(oenums[i].Groups[1].Value.Trim() + ",");
                                }
                                oesb.Append("\r\n");
                            }
                        }
                        
                      




                        string Criteriahtml = Regex.Match(producthtml, @"Criteria</h2>([\s\S]*?)</table>").Groups[1].Value.Trim();

                        MatchCollection a1 = Regex.Matches(Criteriahtml, @"<td class=""col-xs-4""><b>([\s\S]*?)</b>");
                        MatchCollection a2 = Regex.Matches(Criteriahtml, @"<td class=""col-xs-8"">([\s\S]*?)</td>");
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < a1.Count; i++)
                        {
                            sb.Append(a1[i].Groups[1].Value.Trim() + ":"+a2[i].Groups[1].Value.Trim()+",");
                        }

                        string pchtml = Regex.Match(producthtml, @"<h3>PC</h3>([\s\S]*?)</div>").Groups[1].Value.Trim();

                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(Regex.Replace(Brand, "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(partnum, "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(PartType, "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(partstatus, "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(oesb.ToString(), "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(sb.ToString(), "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(pchtml.ToString(), "<[^>]+>", ""));

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        Thread.Sleep(100);
                    }
                   
                }
                  
                }

        }

        #endregion
        #region autozone.com
        public void autozone()
        {


            for (int i = 0; i < richTextBox1.Lines.Length; i++)
            {
                try
                {
                    string n = richTextBox1.Lines[i].Trim();
                    label1.Text = i.ToString();

                    string url = "https://www.autozone.com/searchresult?searchText=" + n;

                    string html = GetUrlWithCookie(url, COOKIE, "utf-8");


                    Match title = Regex.Match(html, @"<title>([\s\S]*?)</title>");
                    Match price = Regex.Match(html, @"""price"":""([\s\S]*?)""");
                    Match part = Regex.Match(html, @"Part #</span>([\s\S]*?)</span>");
                    Match sku = Regex.Match(html, @"""sku"":""([\s\S]*?)""");
                    Match weight = Regex.Match(html, @"""WEIGHT"":""([\s\S]*?)""");
                    Match img = Regex.Match(html, @"<img src=""([\s\S]*?)""");
                    if (title.Groups[1].Value != "")
                    {
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(n);
                        lv1.SubItems.Add(title.Groups[1].Value.Trim());
                        lv1.SubItems.Add(price.Groups[1].Value.Trim());
                        lv1.SubItems.Add(Regex.Replace(part.Groups[1].Value.Trim(), "<[^>]+>", ""));
                        lv1.SubItems.Add(sku.Groups[1].Value.Trim());
                        lv1.SubItems.Add(weight.Groups[1].Value.Trim());
                        lv1.SubItems.Add(img.Groups[1].Value.Trim());
                    }
                    else
                    {
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(n);
                        lv1.SubItems.Add("空");
                        lv1.SubItems.Add("空");
                        lv1.SubItems.Add("空");
                        lv1.SubItems.Add("空");
                        lv1.SubItems.Add("空");
                        lv1.SubItems.Add("空");

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());

                }




            }


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
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                // request.ContentType = "application/x-www-form-urlencoded";

                // 添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("BAIXING-SESSION:$2y$10$5jxzdMFEfh5a7CD9R.mSzu26JmWwbadWalXLNe5OjjyQmi1LPXuAO");
                //headers.Add("x-nike-visitid:5");
                //headers.Add("x-nike-visitorid:d03393ee-e42c-463e-9235-3ca0491475b4");
                //添加头部
                request.ContentType = "application/json";
                request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = "https://web.duanmatong.cn/";
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

        #region 专利查询
        public void zhuanli()
        {


            for (int i = 0; i < richTextBox1.Lines.Length; i++)
            {
                try
                {
                    string n = System.Web.HttpUtility.UrlEncode(richTextBox1.Lines[i].Trim());
                    label1.Text = i.ToString();
                    for (int year = 2008; year < 2021; i++)
                    {

                        for (int page = 1; page < 101; page++)
                        {

                            string url = "http://epub.cnipa.gov.cn/patentoutline.action?showType=1&strSources=&strWhere=OPD%3DBETWEEN%5B%27" + year + ".01.01%27%2C%27" + year + ".12.31%27%5D+and+PA%3D%27%25" + n + "%25%27&numSortMethod=&strLicenseCode=&numIp=&numIpc=&numIg=&numIgc=&numIgd=&numUg=&numUgc=&numUgd=&numDg=&numDgc=&pageSize=20&pageNow=" + page;
                            textBox2.Text = url;
                            string html = GetUrlWithCookie(url, COOKIE, "utf-8");


                            MatchCollection haos = Regex.Matches(html, @"<td height=""30""([\s\S]*?)>([\s\S]*?)</a>");


                            for (int a = 0; a < haos.Count / 4; a++)
                            {
                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(year.ToString());
                                lv1.SubItems.Add(haos[(4 * a) + 1].Groups[2].Value);
                                lv1.SubItems.Add(haos[(4 * a) + 2].Groups[2].Value);
                                lv1.SubItems.Add(haos[(4 * a) + 3].Groups[2].Value);

                            }




                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());

                }




            }


        }

        #endregion

        #region 百姓租房
        public void baixing()
        {


            for (int i = 0; i < richTextBox1.Lines.Length; i++)
            {
                try
                {
                    string n = richTextBox1.Lines[i].Trim();


                    for (int page = 1; page < 101; page++)
                    {

                        string url = "https://mpapi.baixing.com/v1.3.6/";
                        string postdata = "{\"listing.getAds\":{\"areaId\":\"" + n + "\",\"categoryId\":\"qiufang\",\"page\":" + page + ",\"notAllowChatOnly\":1}}";
                        string html = PostUrl(url, postdata, "", "utf-8").Replace("\"user\":{\"id", "");

                        MatchCollection haos = Regex.Matches(html, @"""id"":""([\s\S]*?)""");
                        MatchCollection tels = Regex.Matches(html, @"""mobile"":""([\s\S]*?)""");
                        MatchCollection citys = Regex.Matches(html, @"""cityCName"":""([\s\S]*?)""");

                        if (haos.Count == 0)
                        {
                            break;
                        }
                        for (int a = 0; a < haos.Count; a++)
                        {
                            try
                            {
                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据

                                lv1.SubItems.Add(haos[a].Groups[1].Value);
                                lv1.SubItems.Add(tels[a].Groups[1].Value);
                                lv1.SubItems.Add(citys[a].Groups[1].Value);
                                lv1.SubItems.Add(n);
                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                            }
                            catch (Exception)
                            {

                                continue;
                            }

                        }




                    }
                }

                catch (Exception ex)
                {
                    textBox2.Text = (ex.ToString());

                }

            }


        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {



            status = true;
            //tecalliance  检测UA
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(tecdoc_all);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
       


        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            status = false;
        }
    }
}
