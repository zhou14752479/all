using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main._2019_7
{
    public partial class fang58 : Form
    {
        public fang58()
        {
            InitializeComponent();
        }
        bool status = true;
        bool zanting = true;
        int shuliang = 0;
        string IP = "";
        int PORT = 0;
        #region GET使用代理IP请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string ip, int port)
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                WebProxy proxy = new WebProxy(ip, port);
                request.Proxy = proxy;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";
                request.AllowAutoRedirect = true;
                //request.Headers.Add("Cookie",);
                request.KeepAlive = false;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

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
        #region 主程序1
        public void run1()
        {



            try
            {

                string Url = textBox1.Text;

                    string html = GetUrl(Url, IP, PORT);
               
                    Match city= Regex.Match(Url, @"https:\/\/([\s\S]*?)\.");
                MatchCollection ids = Regex.Matches(html, @"esf_id:([\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                

                if (ids.Count == 0)
                        return;
                    ArrayList lists = new ArrayList();

                    foreach (Match id in ids)
                    {
                        lists.Add(id.Groups[1].Value);
                    }

                    foreach (string list in lists)

                    {
                    string purl = "https://" + city.Groups[1].Value + ".58.com/ershoufang/" + list + "x.shtml";
                    string murl = "https://m.58.com/"+ city.Groups[1].Value + "/ershoufang/"+list+"x.shtml";
                    string lurl = "https://jst1.58.com/counter?infoid="+list+"&userid=0&uname=&sid=0&lid=0&px=0&cfpath=";
                    

                    string strhtml = GetUrl(purl,IP,PORT);
                    string mhtml = GetUrl(murl, IP, PORT);
                    string lhtml = method.gethtml(lurl,"", "utf-8");
                    textBox33.Text += DateTime.Now.ToString() + "----" + "正在获取网页源码..." + "\r\n";
                    Match a0 = Regex.Match(lhtml, @"total=\d*");
                    Match a1 = Regex.Match(strhtml, @"id=''>([\s\S]*?)</span>");
                        Match a2 = Regex.Match(strhtml, @"<title>([\s\S]*?)-");
                        Match a3 = Regex.Match(strhtml, @"<span class='c_000 mr_10'>([\s\S]*?)</span>");
                        Match a4 = Regex.Match(strhtml, @"房本面积</span>([\s\S]*?)</span>");
                        Match a5 = Regex.Match(strhtml, @"售价：([\s\S]*?)\（");
                        Match a6 = Regex.Match(strhtml, @"\（([\s\S]*?)\）"); //单价
                        Match a7 = Regex.Match(strhtml, @"quyu'\)"">([\s\S]*?)</a>");
                        Match a8 = Regex.Match(strhtml, @"房屋户型</span>([\s\S]*?)</span>");
                        Match a9 = Regex.Match(strhtml, @"房屋朝向</span>([\s\S]*?)</span>");
                        Match a10 = Regex.Match(strhtml, @"所在楼层</span>([\s\S]*?)</span>");
                    Match a11 = Regex.Match(strhtml, @"建筑年代([\s\S]*?)</span>");
                    Match a12 = Regex.Match(strhtml, @"装修情况</span>([\s\S]*?)</span>");
                    Match a13 = Regex.Match(strhtml, @"<p class='pic-desc-word'>([\s\S]*?)</p>");
                    Match a14 = Regex.Match(strhtml, @"<p class='phone-num'>([\s\S]*?)</p>");
                    Match a15 = Regex.Match(mhtml, @"<h2 class=""agent-title"">([\s\S]*?)</h2>");



                    if (Convert.ToInt32(a0.Groups[0].Value.Replace("total=", "")) < Convert.ToInt32(textBox25.Text))
                    {
                        String area = Regex.Replace(a7.Groups[1].Value, "<[^>]+>", "").Trim();
                        if (area != textBox34.Text.Replace("区", "") && area != textBox35.Text.Replace("区", "") && area != textBox36.Text.Replace("区", "") && area != textBox37.Text.Replace("区", "") && area != textBox38.Text.Replace("区", ""))
                        {
                            textBox33.Text += DateTime.Now.ToString() + "----" + "浏览量符合正在抓取..." + purl + "\r\n";
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据         
                            lv1.SubItems.Add(a0.Groups[0].Value.Replace("total=", "") + "/" + Regex.Replace(a1.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a2.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a3.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a4.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a5.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a6.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a7.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a8.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a9.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a10.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a11.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a12.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a13.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a14.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a15.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(purl);
                            shuliang = shuliang + 1;
                            label22.Text = shuliang.ToString();

                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)

                            {
                                return;
                            }
                            string[] text = textBox5.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                            foreach (string mail in text)
                            {
                                send(textBox2.Text, textBox3.Text, textBox4.Text, mail, a2.Groups[1].Value + purl);

                            }
                            
                        }

                        else
                        {
                            textBox33.Text += DateTime.Now.ToString() + "----" + "地区被排除跳过：" + purl + "\r\n";
                            textBox33.SelectionStart = this.textBox33.Text.Length;
                            textBox33.SelectionLength = 0;
                            textBox33.ScrollToCaret();
                        }

                    }
                    else
                    {
                        textBox33.Text += DateTime.Now.ToString() + "----" + "浏览量不符合跳过：" + purl+"\r\n";
                        textBox33.SelectionStart = this.textBox33.Text.Length;
                        textBox33.SelectionLength = 0;
                        textBox33.ScrollToCaret();
                    }

                }
            }

            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        #region 主程序2
        public void run2()
        {



            try
            {

                string Url = textBox10.Text;

                string html = GetUrl(Url, IP, PORT);

                Match city = Regex.Match(Url, @"https:\/\/([\s\S]*?)\.");
                MatchCollection ids = Regex.Matches(html, @"esf_id:([\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline);



                if (ids.Count == 0)
                    return;
                ArrayList lists = new ArrayList();

                foreach (Match id in ids)
                {
                    lists.Add(id.Groups[1].Value);
                }

                foreach (string list in lists)

                {
                    string purl = "https://" + city.Groups[1].Value + ".58.com/ershoufang/" + list + "x.shtml";
                    string murl = "https://m.58.com/" + city.Groups[1].Value + "/ershoufang/" + list + "x.shtml";
                    string lurl = "https://jst1.58.com/counter?infoid=" + list + "&userid=0&uname=&sid=0&lid=0&px=0&cfpath=";


                    string strhtml = GetUrl(purl, IP, PORT);
                    string mhtml = GetUrl(murl, IP, PORT);
                    string lhtml = method.gethtml(lurl, "", "utf-8");

                    Match a0 = Regex.Match(lhtml, @"total=\d*");
                    Match a1 = Regex.Match(strhtml, @"id=''>([\s\S]*?)</span>");
                    Match a2 = Regex.Match(strhtml, @"<title>([\s\S]*?)-");
                    Match a3 = Regex.Match(strhtml, @"<span class='c_000 mr_10'>([\s\S]*?)</span>");
                    Match a4 = Regex.Match(strhtml, @"房本面积</span>([\s\S]*?)</span>");
                    Match a5 = Regex.Match(strhtml, @"售价：([\s\S]*?)\（");
                    Match a6 = Regex.Match(strhtml, @"\（([\s\S]*?)\）"); //单价
                    Match a7 = Regex.Match(strhtml, @"quyu'\)"">([\s\S]*?)</a>");
                    Match a8 = Regex.Match(strhtml, @"房屋户型</span>([\s\S]*?)</span>");
                    Match a9 = Regex.Match(strhtml, @"房屋朝向</span>([\s\S]*?)</span>");
                    Match a10 = Regex.Match(strhtml, @"所在楼层</span>([\s\S]*?)</span>");
                    Match a11 = Regex.Match(strhtml, @"建筑年代([\s\S]*?)</span>");
                    Match a12 = Regex.Match(strhtml, @"装修情况</span>([\s\S]*?)</span>");
                    Match a13 = Regex.Match(strhtml, @"<p class='pic-desc-word'>([\s\S]*?)</p>");
                    Match a14 = Regex.Match(strhtml, @"<p class='phone-num'>([\s\S]*?)</p>");
                    Match a15 = Regex.Match(mhtml, @"<h2 class=""agent-title"">([\s\S]*?)</h2>");



                    if (Convert.ToInt32(a0.Groups[0].Value.Replace("total=", "")) < Convert.ToInt32(textBox25.Text))
                    {
                        String area = Regex.Replace(a7.Groups[1].Value, "<[^>]+>", "").Trim();
                        if (area != textBox34.Text.Replace("区", "") && area != textBox35.Text.Replace("区", "") && area != textBox36.Text.Replace("区", "") && area != textBox37.Text.Replace("区", "") && area != textBox38.Text.Replace("区", ""))
                        {
                            textBox33.Text += DateTime.Now.ToString() + "----" + "浏览量符合正在抓取..." + purl + "\r\n";
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据         
                            lv1.SubItems.Add(a0.Groups[0].Value.Replace("total=", "") + "/" + Regex.Replace(a1.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a2.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a3.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a4.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a5.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a6.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a7.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a8.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a9.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a10.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a11.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a12.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a13.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a14.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a15.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(purl);
                            shuliang = shuliang + 1;
                            label22.Text = shuliang.ToString();

                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)

                            {
                                return;
                            }
                            string[] text = textBox6.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                            foreach (string mail in text)
                            {
                                send(textBox9.Text, textBox8.Text, textBox7.Text, mail, a2.Groups[1].Value + purl);

                            }
                            
                        }

                        else
                        {
                            textBox33.Text += DateTime.Now.ToString() + "----" + "地区被排除跳过：" + purl + "\r\n";
                            textBox33.SelectionStart = this.textBox33.Text.Length;
                            textBox33.SelectionLength = 0;
                            textBox33.ScrollToCaret();
                        }

                    }
                    else
                    {
                        textBox33.Text += DateTime.Now.ToString() + "----" + "浏览量不符合跳过：" + purl + "\r\n";
                        textBox33.SelectionStart = this.textBox33.Text.Length;
                        textBox33.SelectionLength = 0;
                        textBox33.ScrollToCaret();
                    }

                }
            }

            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        #region 主程序3
        public void run3()
        {



            try
            {

                string Url = textBox15.Text;

                string html = GetUrl(Url, IP, PORT);

                Match city = Regex.Match(Url, @"https:\/\/([\s\S]*?)\.");
                MatchCollection ids = Regex.Matches(html, @"esf_id:([\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline);



                if (ids.Count == 0)
                    return;
                ArrayList lists = new ArrayList();

                foreach (Match id in ids)
                {
                    lists.Add(id.Groups[1].Value);
                }

                foreach (string list in lists)

                {
                    string purl = "https://" + city.Groups[1].Value + ".58.com/ershoufang/" + list + "x.shtml";
                    string murl = "https://m.58.com/" + city.Groups[1].Value + "/ershoufang/" + list + "x.shtml";
                    string lurl = "https://jst1.58.com/counter?infoid=" + list + "&userid=0&uname=&sid=0&lid=0&px=0&cfpath=";


                    string strhtml = GetUrl(purl, IP, PORT);
                    string mhtml = GetUrl(murl, IP, PORT);
                    string lhtml = method.gethtml(lurl, "", "utf-8");

                    Match a0 = Regex.Match(lhtml, @"total=\d*");
                    Match a1 = Regex.Match(strhtml, @"id=''>([\s\S]*?)</span>");
                    Match a2 = Regex.Match(strhtml, @"<title>([\s\S]*?)-");
                    Match a3 = Regex.Match(strhtml, @"<span class='c_000 mr_10'>([\s\S]*?)</span>");
                    Match a4 = Regex.Match(strhtml, @"房本面积</span>([\s\S]*?)</span>");
                    Match a5 = Regex.Match(strhtml, @"售价：([\s\S]*?)\（");
                    Match a6 = Regex.Match(strhtml, @"\（([\s\S]*?)\）"); //单价
                    Match a7 = Regex.Match(strhtml, @"quyu'\)"">([\s\S]*?)</a>");
                    Match a8 = Regex.Match(strhtml, @"房屋户型</span>([\s\S]*?)</span>");
                    Match a9 = Regex.Match(strhtml, @"房屋朝向</span>([\s\S]*?)</span>");
                    Match a10 = Regex.Match(strhtml, @"所在楼层</span>([\s\S]*?)</span>");
                    Match a11 = Regex.Match(strhtml, @"建筑年代([\s\S]*?)</span>");
                    Match a12 = Regex.Match(strhtml, @"装修情况</span>([\s\S]*?)</span>");
                    Match a13 = Regex.Match(strhtml, @"<p class='pic-desc-word'>([\s\S]*?)</p>");
                    Match a14 = Regex.Match(strhtml, @"<p class='phone-num'>([\s\S]*?)</p>");
                    Match a15 = Regex.Match(mhtml, @"<h2 class=""agent-title"">([\s\S]*?)</h2>");



                    if (Convert.ToInt32(a0.Groups[0].Value.Replace("total=", "")) < Convert.ToInt32(textBox25.Text))
                    {
                        String area = Regex.Replace(a7.Groups[1].Value, "<[^>]+>", "").Trim();
                        if (area != textBox34.Text.Replace("区", "") && area != textBox35.Text.Replace("区", "") && area != textBox36.Text.Replace("区", "") && area != textBox37.Text.Replace("区", "") && area != textBox38.Text.Replace("区", ""))
                        {
                            textBox33.Text += DateTime.Now.ToString() + "----" + "浏览量符合正在抓取..." + purl + "\r\n";
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据         
                            lv1.SubItems.Add(a0.Groups[0].Value.Replace("total=", "") + "/" + Regex.Replace(a1.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a2.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a3.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a4.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a5.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a6.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a7.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a8.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a9.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a10.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a11.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a12.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a13.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a14.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a15.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(purl);
                            shuliang = shuliang + 1;
                            label22.Text = shuliang.ToString();

                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)

                            {
                                return;
                            }
                            string[] text = textBox16.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                            foreach (string mail in text)
                            {
                                send(textBox19.Text, textBox18.Text, textBox17.Text, mail, a2.Groups[1].Value + purl);

                            }
                           
                        }

                        else
                        {
                            textBox33.Text += DateTime.Now.ToString() + "----" + "地区被排除跳过：" + purl + "\r\n";
                            textBox33.SelectionStart = this.textBox33.Text.Length;
                            textBox33.SelectionLength = 0;
                            textBox33.ScrollToCaret();
                        }

                    }
                    else
                    {
                        textBox33.Text += DateTime.Now.ToString() + "----" + "浏览量不符合跳过：" + purl + "\r\n";
                        textBox33.SelectionStart = this.textBox33.Text.Length;
                        textBox33.SelectionLength = 0;
                        textBox33.ScrollToCaret();
                    }

                }
            }

            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        #region 主程序4
        public void run4()
        {



            try
            {

                string Url = textBox20.Text;

                string html = GetUrl(Url, IP, PORT);

                Match city = Regex.Match(Url, @"https:\/\/([\s\S]*?)\.");
                MatchCollection ids = Regex.Matches(html, @"esf_id:([\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline);



                if (ids.Count == 0)
                    return;
                ArrayList lists = new ArrayList();

                foreach (Match id in ids)
                {
                    lists.Add(id.Groups[1].Value);
                }

                foreach (string list in lists)

                {
                    string purl = "https://" + city.Groups[1].Value + ".58.com/ershoufang/" + list + "x.shtml";
                    string murl = "https://m.58.com/" + city.Groups[1].Value + "/ershoufang/" + list + "x.shtml";
                    string lurl = "https://jst1.58.com/counter?infoid=" + list + "&userid=0&uname=&sid=0&lid=0&px=0&cfpath=";


                    string strhtml = GetUrl(purl, IP, PORT);
                    string mhtml = GetUrl(murl, IP, PORT);
                    string lhtml = method.gethtml(lurl, "", "utf-8");

                    Match a0 = Regex.Match(lhtml, @"total=\d*");
                    Match a1 = Regex.Match(strhtml, @"id=''>([\s\S]*?)</span>");
                    Match a2 = Regex.Match(strhtml, @"<title>([\s\S]*?)-");
                    Match a3 = Regex.Match(strhtml, @"<span class='c_000 mr_10'>([\s\S]*?)</span>");
                    Match a4 = Regex.Match(strhtml, @"房本面积</span>([\s\S]*?)</span>");
                    Match a5 = Regex.Match(strhtml, @"售价：([\s\S]*?)\（");
                    Match a6 = Regex.Match(strhtml, @"\（([\s\S]*?)\）"); //单价
                    Match a7 = Regex.Match(strhtml, @"quyu'\)"">([\s\S]*?)</a>");
                    Match a8 = Regex.Match(strhtml, @"房屋户型</span>([\s\S]*?)</span>");
                    Match a9 = Regex.Match(strhtml, @"房屋朝向</span>([\s\S]*?)</span>");
                    Match a10 = Regex.Match(strhtml, @"所在楼层</span>([\s\S]*?)</span>");
                    Match a11 = Regex.Match(strhtml, @"建筑年代([\s\S]*?)</span>");
                    Match a12 = Regex.Match(strhtml, @"装修情况</span>([\s\S]*?)</span>");
                    Match a13 = Regex.Match(strhtml, @"<p class='pic-desc-word'>([\s\S]*?)</p>");
                    Match a14 = Regex.Match(strhtml, @"<p class='phone-num'>([\s\S]*?)</p>");
                    Match a15 = Regex.Match(mhtml, @"<h2 class=""agent-title"">([\s\S]*?)</h2>");



                    if (Convert.ToInt32(a0.Groups[0].Value.Replace("total=", "")) < Convert.ToInt32(textBox25.Text))
                    {
                        String area = Regex.Replace(a7.Groups[1].Value, "<[^>]+>", "").Trim();
                        if ((area != textBox34.Text.Replace("区", "") && area != textBox35.Text.Replace("区", "") && area != textBox36.Text.Replace("区", "") && area != textBox37.Text.Replace("区", "") && area != textBox38.Text.Replace("区", "")) || area=="")
                        {
                            textBox33.Text += DateTime.Now.ToString() + "----" + "浏览量符合正在抓取..." + purl + "\r\n";
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据         
                            lv1.SubItems.Add(a0.Groups[0].Value.Replace("total=", "") + "/" + Regex.Replace(a1.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a2.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a3.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a4.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a5.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a6.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a7.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a8.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a9.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a10.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a11.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a12.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a13.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a14.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(a15.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(purl);
                            shuliang = shuliang + 1;
                            label22.Text = shuliang.ToString();

                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)

                            {
                                return;
                            }
                            string[] text = textBox11.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                            foreach (string mail in text)
                            {
                                send(textBox14.Text, textBox13.Text, textBox12.Text, mail, a2.Groups[1].Value + purl);

                            }
                           
                        }

                        else
                        {

                            textBox33.Text += DateTime.Now.ToString() + "----" + "地区被排除跳过：" + purl + "\r\n";
                            textBox33.SelectionStart = this.textBox33.Text.Length;
                            textBox33.SelectionLength = 0;
                            textBox33.ScrollToCaret();
                        }

                    }
                    else
                    {
                        textBox33.Text += DateTime.Now.ToString() + "----" + "浏览量不符合跳过：" + purl + "\r\n";
                        textBox33.SelectionStart = this.textBox33.Text.Length;
                        textBox33.SelectionLength = 0;
                        textBox33.ScrollToCaret();
                    }

                }
            }

            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion


        public static void send(string smtp, string fa,string ma, string address, string body)
        {
            //实例化一个发送邮件类。
            MailMessage mailMessage = new MailMessage();
            //发件人邮箱地址，方法重载不同，可以根据需求自行选择。
            mailMessage.From = new MailAddress(fa);
            //收件人邮箱地址。
            mailMessage.To.Add(new MailAddress(address));
            //邮件标题。
            mailMessage.Subject = "58房产提醒"+DateTime.Now.ToString();
            //邮件内容。
            mailMessage.Body = body;

            //实例化一个SmtpClient类。
            SmtpClient client = new SmtpClient();
            //在这里我使用的是qq邮箱，所以是smtp.qq.com，如果你使用的是126邮箱，那么就是smtp.126.com。
            client.Host = smtp;
            //使用安全加密连接。
            client.EnableSsl = true;
            //不和请求一块发送。
            client.UseDefaultCredentials = false;
            //验证发件人身份(发件人的邮箱，邮箱里的生成授权码);
            client.Credentials = new NetworkCredential(fa, ma);   //这里的密码用授权码
            //发送
            client.Send(mailMessage);
            // MessageBox.Show("发送成功");

        }
        private void fang58_Load(object sender, EventArgs e)
        {  
            timer1.Start();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox26.Text == "")
            {
                MessageBox.Show("请输入代理IP地址");
                return;
            }
            string html = method.GetUrl(textBox26.Text, "utf-8");
            string[] text = html.Split(new string[] { ":" }, StringSplitOptions.None);
            IP = text[0];
            PORT = Convert.ToInt32(text[1]);
            timer2.Start();
            timer2.Interval = Convert.ToInt32(textBox30.Text) * 1000;

            textBox33.Text = DateTime.Now.ToString() + "----" + "程序已启动"+"\r\n";
            Control.CheckForIllegalCrossThreadCalls = false;
            Thread thread = new Thread(new ThreadStart(run1));
            thread.Start();
            timer3.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {
                zanting = true;
            }
            else if(zanting==true)
            {
                zanting = false;
            }

           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox26.Text == "")
            {
                MessageBox.Show("请输入代理IP地址");
                return;
            }

            timer2.Start();
            timer2.Interval = Convert.ToInt32(textBox30.Text) * 1000;

            textBox33.Text = DateTime.Now.ToString() + "----" + "程序已启动" + "\r\n";
            Control.CheckForIllegalCrossThreadCalls = false;
            Thread thread = new Thread(new ThreadStart(run2));
            thread.Start();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (textBox26.Text == "")
            {
                MessageBox.Show("请输入代理IP地址");
                return;
            }

            timer2.Start();
            timer2.Interval = Convert.ToInt32(textBox30.Text) * 1000;

            textBox33.Text = DateTime.Now.ToString() + "----" + "程序已启动" + "\r\n";
            Control.CheckForIllegalCrossThreadCalls = false;
            Thread thread = new Thread(new ThreadStart(run3));
            thread.Start();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox26.Text == "")
            {
                MessageBox.Show("请输入代理IP地址");
                return;
            }

            timer2.Start();
            timer2.Interval = Convert.ToInt32(textBox30.Text) * 1000;

            textBox33.Text = DateTime.Now.ToString() + "----" + "程序已启动" + "\r\n";
            Control.CheckForIllegalCrossThreadCalls = false;
            Thread thread = new Thread(new ThreadStart(run4));
            thread.Start();
        }
        #region NPOI导出表格
        public static int DataTableToExcel(DataTable data, string sheetName, bool isColumnWritten)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            int i = 0;
            int j = 0;
            int count = 0;
            ISheet sheet = null;
            IWorkbook workbook = null;
            FileStream fs = null;
            // bool disposed;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "xlsx|*.xls|xlsx|*.xlsx";
            sfd.Title = "Excel文件导出";
            string fileName = path + DateTime.Now.ToShortDateString().Replace("/","-")+ ".xlsx";

          

            fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                workbook = new XSSFWorkbook();
            else if (fileName.IndexOf(".xls") > 0) // 2003版本
                workbook = new HSSFWorkbook();

            try
            {
                if (workbook != null)
                {
                    sheet = workbook.CreateSheet(sheetName);
                    ICellStyle style = workbook.CreateCellStyle();
                    style.FillPattern = FillPattern.SolidForeground;

                }
                else
                {
                    return -1;
                }

                if (isColumnWritten == true) //写入DataTable的列名
                {
                    IRow row = sheet.CreateRow(0);
                    for (j = 0; j < data.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data.Columns[j].ColumnName);

                    }
                    count = 1;
                }
                else
                {
                    count = 0;
                }

                for (i = 0; i < data.Rows.Count; ++i)
                {
                    IRow row = sheet.CreateRow(count);
                    for (j = 0; j < data.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
                    }
                    ++count;
                }
                workbook.Write(fs); //写入到excel
                workbook.Close();
                fs.Close();
                System.Diagnostics.Process[] Proc = System.Diagnostics.Process.GetProcessesByName("");
               
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return -1;
            }
        }

        #endregion
        private void Timer1_Tick(object sender, EventArgs e)
        {
            int h = Convert.ToInt32(DateTime.Now.Hour); int m = Convert.ToInt32(DateTime.Now.Minute); int s = Convert.ToInt32(DateTime.Now.Second);
            if (Convert.ToInt32(textBox27.Text) == h && Convert.ToInt32(textBox28.Text) == m && Convert.ToInt32(textBox29.Text) == s)
            {
                zanting = false;
                DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
                listView1.Items.Clear();
                shuliang = 0;
            }

            zanting = true;
           


        }

        private void Button10_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {
                zanting = true;
            }
            else if (zanting == true)
            {
                zanting = false;
            }
        }

        private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {
                zanting = true;
            }
            else if (zanting == true)
            {
                zanting = false;
            }
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {
                zanting = true;
            }
            else if (zanting == true)
            {
                zanting = false;
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {
                zanting = true;
            }
            else if (zanting == true)
            {
                zanting = false;
            }
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            textBox5.Text = textBox32.Text;
            textBox32.Visible = false;
        }

        private void Button12_Click(object sender, EventArgs e)
        {
            textBox6.Text = textBox32.Text;
            textBox32.Visible = false;
        }

        private void Button13_Click(object sender, EventArgs e)
        {
            textBox16.Text = textBox32.Text;
            textBox32.Visible = false;
        }

        private void Button14_Click(object sender, EventArgs e)
        {
            textBox11.Text = textBox32.Text;
            textBox32.Visible = false;
        }

        private void Button15_Click(object sender, EventArgs e)
        {
            if (textBox32.Visible == true)
            {
                textBox32.Visible = false;
            }
           else if (textBox32.Visible == false)
            {
                textBox32.Visible = true;
            }

        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            string html = method.GetUrl(textBox26.Text, "utf-8");
            string[] text = html.Split(new string[] { ":" }, StringSplitOptions.None);
            IP = text[0];
            PORT = Convert.ToInt32(text[1]);
            label32.Text = IP + ":" + PORT;
        }

        private void Timer3_Tick(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            Thread thread = new Thread(new ThreadStart(run1));
            thread.Start();
        }
    }
}
