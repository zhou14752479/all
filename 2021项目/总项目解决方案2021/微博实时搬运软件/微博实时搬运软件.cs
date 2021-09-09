using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 微博实时搬运软件
{
    public partial class 微博实时搬运软件 : Form
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.inipath);
        }

        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
            return temp.ToString();
        }

        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public bool ExistINIFile()
        {
            return File.Exists(inipath);
        }
        public 微博实时搬运软件()
        {
            InitializeComponent();
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
        }

        #region API接口
        string access_token = "2.00GPLckFAjGwFE086a1b66242VLRVC";

        #region 获取accesstoken
        public void getaccesstoken()
        {
           
            string url = "https://api.weibo.com/oauth2/access_token";
            string code = Regex.Match(webBrowser1.Url.ToString(),@"code=.*").Groups[0].Value.Replace("code=","");
            string postdata = "client_id=3752261886&client_secret=cb55143f0a756ee5b171df38d3359328&grant_type=authorization_code&code="+code+"&redirect_uri=http://www.acaiji.com";
            string html = method.PostUrlDefault(url,postdata,"");
            string accesstoken = Regex.Match(html, @"access_token"":""([\s\S]*?)""").Groups[1].Value;
            access_token = accesstoken;
           
        }
        #endregion
   
        #region 发布头条
        public string publish()
        {

            string url = "https://api.weibo.com/proxy/article/publish.json";
            string title = System.Web.HttpUtility.UrlEncode("测试标题");
            string content = System.Web.HttpUtility.UrlEncode("正文内容");
            string cover = System.Web.HttpUtility.UrlEncode("https://imgheybox.max-c.com/bbs/2021/09/08/28fd2c5c886ab2cd9a76addeffa443fd.jpeg");
            string summary = System.Web.HttpUtility.UrlEncode("导语");
            string text = "aaa";
            string postdata = "title="+title+ "&content="+content+ "&cover="+ cover+ "&summary="+ summary+ "&text="+text+ "&access_token="+ access_token;
            string html = method.PostUrlDefault(url, postdata, "");
            textBox1.Text = postdata;
            MessageBox.Show(html);
            return html;
        }
        #endregion

        #endregion



        string COOKIE = "SINAGLOBAL=2631464369339.447.1613870261937; UOR=,,www.baidu.com; _s_tentry=-; Apache=9878414590865.965.1631144429159; ULV=1631144429165:7:2:2:9878414590865.965.1631144429159:1631074635475; SSOLoginState=1631144906; ALF=1662680919; SCF=Alslx_RaEKKXj8jzZBGXOQSLv6Lc3mi-TFwTiE0NdNW1Pk00Wni-ZtzaxkSQ6LnBP_G-gJnopkfexMRWRlzSbow.; SUB=_2A25MPTuJDeRhGeNM7VsV9yvPyzyIHXVvSypBrDV8PUNbmtANLVjDkW9NThXiEiRJtl9BGtN3ez72D3Gq5bvfaANq; SUBP=0033WrSXqPxfM725Ws9jqgMF55529P9D9WFAbTDTMuOyH2NWYqT8Dr.s5JpX5KzhUgL.Fo-ESo.XS0-0eh52dJLoI7LLqcyadsvc9P.t; wvr=6; webim_unReadCount=%7B%22time%22%3A1631164350718%2C%22dm_pub_total%22%3A0%2C%22chat_group_client%22%3A0%2C%22chat_group_notice%22%3A0%2C%22allcountNum%22%3A26%2C%22msgbox%22%3A0%7D";
        string uid = "5269475300";
        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public  string PostUrl(string url, string postData)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";

                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("appid:orders");
                //headers.Add("x-nike-visitid:5");
                //headers.Add("x-nike-visitorid:d03393ee-e42c-463e-9235-3ca0491475b4");
                //添加头部
               
                request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = "https://card.weibo.com/article/v3/editor";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

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


        #region 创建文章ID
        public string create()
        {
            try
            {
                string url = "https://card.weibo.com/article/v3/aj/editor/draft/create?uid=" + uid;
                string html = PostUrl(url, "uid=" + uid);
                string id = Regex.Match(html, @"""id"":""([\s\S]*?)""").Groups[1].Value;
                string date = Regex.Match(html, @"""updated"":""([\s\S]*?)""").Groups[1].Value;
                return "updated=" + date + "&id=" + id;
            }
            catch (Exception ex)
            {

                textBox3.Text = ex.ToString();
                return "";
            }
        }

        #endregion


        #region 创建文章正文
        public string createbody(string title,string content,string cover,string summary)
        {
            try
            {
                string dateid = create();
                string id = Regex.Match(dateid, @"id=.*").Groups[0].Value.Replace("id=", "");
                string url = "https://card.weibo.com/article/v3/aj/editor/draft/save?uid=" + uid + "&id=" + id;

           

                string postdata = "title=" + title + "&type=&summary=" + summary + "&writer=&cover=" + cover + "&content=" + content + "&collection=%5B%5D&" + dateid + "&subtitle=&extra=null&status=0&publish_at=&error_msg=&error_code=0&free_content=&is_word=0&article_recommend=%5B%5D&is_article_free=0&follow_to_read=1&follow_to_read_detail%5Bresult%5D=1&follow_to_read_detail%5Bx%5D=0&follow_to_read_detail%5By%5D=0&follow_to_read_detail%5Breadme_link%5D=http%3A%2F%2Ft.cn%2FA6UnJsqW&follow_to_read_detail%5Blevel%5D=&isreward=0&pay_setting=%7B%22ispay%22%3A0%2C%22isvclub%22%3A0%7D&source=0&action=2&content_type=0&save=1";
                string html = PostUrl(url, postdata);


                string tags = System.Web.HttpUtility.UrlEncode(textBox2.Text); 
                string text = System.Web.HttpUtility.UrlEncode("发布了头条文章：") + title + tags;

                return "id=" + id + "&rank=0&text=" + text + " &sync_wb=0&is_original=0&time=";
            }
            catch (Exception ex)
            {

                textBox3.Text=ex.ToString();
                return "";
            }
        }

        #endregion


        #region 创建文章标题
        public string createtitle(string postdata)
        {
            try
            {
                
                string id = Regex.Match(postdata, @"id=([\s\S]*?)&").Groups[1].Value;
                string url = "https://card.weibo.com/article/v3/aj/editor/draft/publish?uid=" + uid + "&id=" + id;
                string html = PostUrl(url, postdata);
                return html;
               

            }
            catch (Exception ex)
            {

                textBox3.Text = ex.ToString();
                return "";
            }
        }

        #endregion


        #region 处理文章图片
        public string  getnewpics(string content)
        {
            try
            {
               
                string body = "";
                MatchCollection picurls = Regex.Matches(content, @"src=\\""([\s\S]*?)\\""");
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < picurls.Count; i++)
                {
                    sb.Append(System.Web.HttpUtility.UrlEncode("urls[" +i+"]")+ "="+System.Web.HttpUtility.UrlEncode(picurls[i].Groups[1].Value)+"&");
                }
                if (sb.ToString().Length > 10)
                {
                    string url = "https://card.weibo.com/article/v3/aj/editor/plugins/asyncuploadimg?uid=" + uid;
                    string postdata = sb.ToString().Remove(sb.ToString().Length-1,1);
                   
                    string html = PostUrl(url, postdata);
                  
                    if (html.Contains("true"))
                    {
                      
                        string newurl = "https://card.weibo.com/article/v3/aj/editor/plugins/asyncimginfo?uid="+uid;
                      
                      
                       string infohtml= PostUrl(newurl, postdata);
                       
                        MatchCollection origin_urls = Regex.Matches(infohtml, @"""origin_url"":""([\s\S]*?)""");
                        MatchCollection urls = Regex.Matches(infohtml, @"""url"":""([\s\S]*?)""");
                        for (int j = 0; j < origin_urls.Count; j++)
                        {
                            body = content.Replace(origin_urls[j].Groups[1].Value, urls[j].Groups[1].Value);
                        }


                      

                    }


                }
               
                return body;
            }
            catch (Exception ex)
            {

                textBox3.Text = ex.ToString();
                return "";
            }
        }

        #endregion

        public void run()
        {
            try
            {
                string url = "https://news.maxjia.com/maxnews/app/news/with/topics/authors?tag=None&lang=zh-cn&os_type=iOS&os_version=13.6.1&_time=1631074415&version=4.4.41&device_id=6B8E0037-2842-4B1E-B506-A2F16A43714F&game_type=csgo&max__id=0&limit=20&offset=0";
                string html = method.GetUrl(url,"utf-8");
                MatchCollection uids = Regex.Matches(html, @"""newsid"": ""([\s\S]*?)""");
                for (int i = 0; i < 1; i++)  //监控两篇
                {
                    string uid = uids[i].Groups[1].Value;
                    string uidini = "";
                    if (ExistINIFile())
                    {
                        uidini = IniReadValue("values", "uids");


                    }
                    if (uidini.Contains(uid))
                    {
                        continue;
                    }


                    string detailUrl = "http://news.maxjia.com/maxnews/app/detail/csgo/"+uid+"?return_json=1";
                    string detailhtml = method.GetUrl(detailUrl, "utf-8");
                    detailhtml = method.Unicode2String(detailhtml);
                    string title = System.Web.HttpUtility.UrlEncode(Regex.Match(detailhtml, @"""title"": ""([\s\S]*?)""").Groups[1].Value);

                    string content = getnewpics(Regex.Match(detailhtml, @"""content"": ""([\s\S]*?)""content_type").Groups[1].Value.Trim().Replace("\",​​", "​​"));
                   
                    content = System.Web.HttpUtility.UrlEncode("<p>转自Max</p>" + content);

                    string cover = System.Web.HttpUtility.UrlEncode(Regex.Match(detailhtml, @"""thumb"": ""([\s\S]*?)""").Groups[1].Value);
                    string summary = System.Web.HttpUtility.UrlEncode(Regex.Match(detailhtml, @"<div class=""blockquote"">([\s\S]*?)</div>").Groups[1].Value);

                    string postdata = createbody(title,content,cover,summary);
                    string result = createtitle(postdata);
                    textBox3.Text = method.Unicode2String(result);
                    string articleurl = Regex.Match(result, @"""url"":""([\s\S]*?)""").Groups[1].Value;
                    if (articleurl != "")
                    {

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(Regex.Match(detailhtml, @"""title"": ""([\s\S]*?)""").Groups[1].Value);
                        lv1.SubItems.Add("发布成功");
                    }

                    Thread.Sleep(5000);
                }
            }
            catch (Exception ex)
            {

                textBox3.Text = ex.ToString();
            }

        }

        private void 微博实时搬运软件_Load(object sender, EventArgs e)
        {
           
        }

        Thread thread;

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"YVoWQ"))
            {

                return;
            }

            #endregion
            timer1.Start();
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
         
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
}
