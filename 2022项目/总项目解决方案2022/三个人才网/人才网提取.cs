using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 三个人才网
{
    public partial class 人才网提取 : Form
    {
        public 人才网提取()
        {
            InitializeComponent();
        }

        #region  程序关闭删除自身
        public static void TestForKillMyself()
        {
            string bat = @"@echo off
                           :tryagain
                           del %1
                           if exist %1 goto tryagain
                           del %0";
            File.WriteAllText("killme.bat", bat);//写bat文件
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "killme.bat";
            psi.Arguments = "\"" + Environment.GetCommandLineArgs()[0] + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
        }


        #endregion

        private void 人才网提取_Load(object sender, EventArgs e)
        {
            #region 通用检测


            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"D2B5"))
            {
                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();

                return;
            }

            #endregion
           // dateTimePicker1.Value = DateTime.Now.AddDays(-30);
        }


        Thread thread;
        bool zanting = true;
        bool status = true;

        #region POST默认请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrlDefault(string url, string postData, string COOKIE)
        {
            try
            {

                string charset = "utf-8";
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                //request.Proxy = null;//防止代理抓包
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("xcxCode: eCGA4JW243xUe/HPeaJY4Lx+N9dVnva/OQpTSp4dXF0=");
                //headers.Add("sec-fetch-site:same-origin");
                //headers.Add("sec-fetch-user:?1");
                //headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                request.ContentType = "application/x-www-form-urlencoded";
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                //request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                // request.ContentLength = postData.Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = url;
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");

                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }


                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion


        List<string> lists=new List<string>();
       

        private string Request_www_caige_com(string id)
        {
            string charset = "utf-8";
           HttpWebResponse response = null;
            string html = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.yiwucaige.com/box.php?part=seecontact_tel&infoid="+id);

                request.KeepAlive = true;
                request.Headers.Add("sec-ch-ua", @""" Not A;Brand"";v=""99"", ""Chromium"";v=""100"", ""Google Chrome"";v=""100""");
                request.Accept = "*/*";
                request.Headers.Add("X-Requested-With", @"XMLHttpRequest");
                request.Headers.Add("sec-ch-ua-mobile", @"?0");
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.127 Safari/537.36";
                request.Headers.Add("sec-ch-ua-platform", @"""Windows""");
                request.Headers.Add("Sec-Fetch-Site", @"same-origin");
                request.Headers.Add("Sec-Fetch-Mode", @"cors");
                request.Headers.Add("Sec-Fetch-Dest", @"empty");
                request.Referer = "https://www.yiwucaige.com/information-id-129763.html";
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.9");
                request.Headers.Set(HttpRequestHeader.Cookie, @"awfn__s_uid=17606117606; awfn__s_pwd=BAZRVANVUg9XAAEEVgNVCFsBAlYAAAdRWlNQVFBcVlk%3Db64e64106f");

                response = (HttpWebResponse)request.GetResponse();
                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
                else return html;
            }
            catch (Exception)
            {
                if (response != null) response.Close();
                return html;
            }

            return html;
        }

        


        private string Request_www_vyuan8_com(string zid)
        {
            HttpWebResponse response = null;
            string html = "";
            try
            {
                string charset = "utf-8";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.vyuan8.com/hr/plugin.php?id=vyuan_zhaopin&model=view&act=tel_authorization&pid=113324&zid="+zid);

                request.KeepAlive = true;
                request.Accept = "application/json, text/javascript, */*; q=0.01";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36 NetType/WIFI MicroMessenger/7.0.20.1781(0x6700143B) WindowsWechat(0x63060012)";
                request.Headers.Add("X-Requested-With", @"XMLHttpRequest");
                request.Headers.Set(HttpRequestHeader.Cookie, @"logintime=1652256950; searchTime=1652198400; r8t7_2132_saltkey=u90jij05; r8t7_2132_lastvisit=1652253311; PHPSESSID=10o4or5ulur1quoqqrm97f2355; r8t7_2132_open_pid=113324; r8t7_2132_zhaopin_openid=oxP46xGaK1TUqVkIyF5LYtBf_-HI; r8t7_2132_openid=oxP46xGaK1TUqVkIyF5LYtBf_-HI; r8t7_2132_referer_url=https%3A%2F%2Fwww.vyuan8.com%2Fhr%2Fplugin.php%3Fid%3Dvyuan_zhaopin%26pid%3D113324%26model%3Dsearch; r8t7_2132_zhaopin_openid_request=1; Hm_lvt_188b90c06165ede59725450aee2d2bdc=1652256916; Hm_lpvt_188b90c06165ede59725450aee2d2bdc=1652256975; r8t7_2132_sid=B50eBc; r8t7_2132_lastact=1652256974%09plugin.php%09");
                request.Headers.Add("Sec-Fetch-Site", @"same-origin");
                request.Headers.Add("Sec-Fetch-Mode", @"cors");
                request.Headers.Add("Sec-Fetch-Dest", @"empty");
                request.Referer = "https://www.vyuan8.com/hr/plugin.php?id=vyuan_zhaopin&model=view&pid=113324&zid=993446&transfer=ok";
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.9,en-US;q=0.8,en;q=0.7");

                response = (HttpWebResponse)request.GetResponse();
                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }


                response.Close();
                return html;

            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
                else return "";
            }

            return html;
        }
        private DateTime ConvertStringToDateTime(string timeStamp)
        {

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(Convert.ToDouble(timeStamp));
        }

        #region 和众人才网
        public void hezhong()
        {



            for (int page = 1; page < 9999; page++)
            {

                string url = "https://www.0579work.com/api/wxapp/index.php?m=job&c=list";
                string postdata = "page=" + page + "&limit=100&joblist=1&webid=11327&provider=weixin&systemInfo=%7B%22model%22%3A%22iPhone%2013%3CiPhone14%2C5%3E%22%2C%22system%22%3A%22iOS%2015.4.1%22%2C%22platform%22%3A%22ios%22%7D&source=13";
                string html = PostUrlDefault(url, postdata, "");
                html = method.Unicode2String(html);
                //textBox1.Text = html;
                MatchCollection ids = Regex.Matches(html, @"""id"":""([\s\S]*?)""");
                MatchCollection names = Regex.Matches(html, @"""name"":""([\s\S]*?)""");
                MatchCollection lastupdate_dates = Regex.Matches(html, @"""lastupdate_date"":""([\s\S]*?)""");
                if (ids.Count == 0)
                {
                    break;
                }
                for (int a = 0; a < ids.Count; a++)
                {
                    try
                    {
                        string date = lastupdate_dates[a].Groups[1].Value;
                        if (lastupdate_dates[a].Groups[1].Value.Contains(":"))  //当天的时间
                        {
                            date = DateTime.Now.ToString("yyyy-MM-dd");
                        }
                        if (lastupdate_dates[a].Groups[1].Value == "昨天")
                        {
                            date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                        }

                      
                       
                        if (Convert.ToDateTime(date) > Convert.ToDateTime(dateTimePicker1.Value.AddDays(-1)) && Convert.ToDateTime(date) < Convert.ToDateTime(dateTimePicker2.Value.AddDays(1)))
                        {
                            string aurl = "https://www.0579work.com/api/wxapp/index.php?m=job&c=jobShowOther";
                            string apostdata = "id=" + ids[a].Groups[1].Value + "&webid=11327&provider=weixin&systemInfo=%7B%22model%22%3A%22iPhone%2013%3CiPhone14%2C5%3E%22%2C%22system%22%3A%22iOS%2015.4.1%22%2C%22platform%22%3A%22ios%22%7D&source=13&uid=8427&token=4f01cbb645601d6780727ab88b9d4a3a";
                            string ahtml = PostUrlDefault(aurl, apostdata, "");




                            string tel = Regex.Match(ahtml, @"linktel"":""([\s\S]*?)""").Groups[1].Value;

                            if (!lists.Contains(tel))
                            {
                                if(checkBox4.Checked==true)
                                {
                                    lists.Add(tel);
                                }    
                               
                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据


                                lv1.SubItems.Add(tel.Trim());
                                lv1.SubItems.Add(method.Unicode2String(names[a].Groups[1].Value).Trim());

                                Thread.Sleep(1000);
                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                                if (status == false)
                                    return;
                            }
                        }
                        else
                        {
                            label1.Text = "时间不符合跳过";
                        }
                    }
                    catch (Exception)
                    {

                        continue;
                    }

                }




            }


        }

        #endregion

        #region 领航人才网
        public void linghang()
        {



            for (int page = 1; page < 9999; page++)
            {

                string url = "https://www.vyuan8.com/hr/plugin.php?id=vyuan_zhaopin&model=search&pid=113324&ajax=ajax&page="+page+"&area=&salaryint=&edu=&industry=&searchField=&time=0&type=&label=";
            
                string html = method.GetUrl(url,"gb2312");
               
                MatchCollection titles = Regex.Matches(html, @"""z_name"":""([\s\S]*?)""");
                MatchCollection ids = Regex.Matches(html, @"""id"":""([\s\S]*?)""");
                MatchCollection lastupdate_dates = Regex.Matches(html, @"""z_addtime"":""([\s\S]*?)""");
               // textBox1.Text = html;
                if (ids.Count == 0)
                {
                    label1.Text = "无符合数据页码："+page;
                    break;
                }
                for (int a = 0; a < ids.Count; a++)
                {
                    try
                    {
                        string date ="2022-"+lastupdate_dates[a].Groups[1].Value.Replace("&#x6708;", "-").Replace("&#x65E5;", "");
                       
                        if (Convert.ToDateTime(date) >= Convert.ToDateTime(dateTimePicker1.Value.AddDays(-1)) && Convert.ToDateTime(date) <= Convert.ToDateTime(dateTimePicker2.Value.AddDays(1)))
                        {
                            string ahtml = Request_www_vyuan8_com(ids[a].Groups[1].Value);
                          
                            string tel = Regex.Match(ahtml, @"tel"":""([\s\S]*?)""").Groups[1].Value;

                            if (!lists.Contains(tel))
                            {
                                if (checkBox4.Checked == true)
                                {
                                    lists.Add(tel);
                                }
                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(tel.Trim());
                                lv1.SubItems.Add(titles[a].Groups[1].Value.Trim());

                                Thread.Sleep(1000);
                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                                if (status == false)
                                    return;
                            }
                        }
                    }
                    catch (Exception)
                    {

                        continue;
                    }

                }




            }


        }

        #endregion


        #region 才哥人才网
        public void caige()
        {



            for (int page = 1; page < 9999; page++)
            {

                string url = "https://www.yiwucaige.com/category-catid-1-page-"+page+".html";
         
                string html = method.GetUrl(url, "utf-8");

                MatchCollection titles = Regex.Matches(html, @"bold;"">([\s\S]*?)</a>");
                MatchCollection ids = Regex.Matches(html, @"<a href=""https://www.yiwucaige.com/information-id-([\s\S]*?)\.");
                MatchCollection lastupdate_dates = Regex.Matches(html, @"pull-right'>([\s\S]*?)<");
               
                if (ids.Count == 0)
                {
                    break;
                }
                for (int a = 0; a < ids.Count; a++)
                {
                    try
                    {
                        string date = lastupdate_dates[a].Groups[1].Value;
                        if (!lastupdate_dates[a].Groups[1].Value.Contains("-") && !lastupdate_dates[a].Groups[1].Value.Contains("天"))
                        {
                            date = DateTime.Now.ToString("yyyy-MM-dd");
                        }
                        if (lastupdate_dates[a].Groups[1].Value.Contains("昨天"))
                        {
                            date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                        }
                        if (lastupdate_dates[a].Groups[1].Value.Contains("前天"))
                        {
                            date = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");
                        }

                        if (lastupdate_dates[a].Groups[1].Value.Contains("3天"))
                        {
                            date = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd");
                        }

                        if (lastupdate_dates[a].Groups[1].Value.Contains("4天"))
                        {
                            date = DateTime.Now.AddDays(-4).ToString("yyyy-MM-dd");
                        }
                        if (lastupdate_dates[a].Groups[1].Value.Contains("5天"))
                        {
                            date = DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd");
                        }
                        if (lastupdate_dates[a].Groups[1].Value.Contains("6天"))
                        {
                            date = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
                        }
                        if (Convert.ToDateTime(date) >= Convert.ToDateTime(dateTimePicker1.Value.AddDays(-1)) && Convert.ToDateTime(date) <= Convert.ToDateTime(dateTimePicker2.Value.AddDays(1)))
                        {
                            string aurl = "https://www.yiwucaige.com/box.php?part=seecontact_tel&infoid="+ ids[a].Groups[1].Value;

                            string ahtml = Request_www_caige_com(ids[a].Groups[1].Value) ;

                         
                            string tel = Regex.Match(ahtml, @"btn-txt'>([\s\S]*?)<").Groups[1].Value.Replace("-","");

                            if (!lists.Contains(tel))
                            {
                                if (checkBox4.Checked == true)
                                {
                                    lists.Add(tel);
                                }
                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据


                                lv1.SubItems.Add(tel.Trim());
                                lv1.SubItems.Add(titles[a].Groups[1].Value.Trim());

                                Thread.Sleep(1000);
                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                                if (status == false)
                                    return;
                            }
                        }
                    }
                    catch (Exception)
                    {

                        continue;
                    }

                }




            }


        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            lists.Clear();
           status = true;
            if (checkBox1.Checked == true)
            {
                Thread thread = new Thread(hezhong);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            }
            if (checkBox2.Checked == true)
            {

                Thread thread = new Thread(linghang);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            if (checkBox3.Checked == true)
            {
                Thread thread = new Thread(caige);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.ListviewToTxt(listView1,1);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            status=false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
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

        private void 人才网提取_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }
    }
}
