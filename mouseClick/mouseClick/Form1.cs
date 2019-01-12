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

namespace mouseClick
{
    public partial class Form1 : Form {
        public Form1()
        {
            InitializeComponent();
            webBrowser1.ScriptErrorsSuppressed = true;
            Control.CheckForIllegalCrossThreadCalls = false;

        }


        private void Form1_Load(object sender, EventArgs e)
    {

    }        
#region GET请求
/// <summary>
/// GET请求
/// </summary>
/// <param name="Url">网址</param>
/// <returns></returns>
public static string GetUrl(string Url)
        {
            try
            {
                string COOKIE = "zh_choose=n; firstvisit_backurl=http%3A//www.wanfangdata.com.cn; Hm_lvt_838fbc4154ad87515435bf1e10023fab=1535951667,1536046919,1536054022; WFKS.Auth=%7b%22Context%22%3a%7b%22AccountIds%22%3a%5b%5d%2c%22Data%22%3a%5b%5d%2c%22SessionId%22%3a%225f3bc5e7-db98-4ebf-a91d-c2cc82472118%22%2c%22Sign%22%3a%22hi+authserv%22%7d%2c%22LastUpdate%22%3a%222018-09-04T09%3a41%3a41Z%22%2c%22TicketSign%22%3a%22fMj4xckmwU5AYCPwYpHvCg%3d%3d%22%7d; JSESSIONID=6AAAAD40508AC0629AB8613FF462AA70; Hm_lpvt_838fbc4154ad87515435bf1e10023fab=1536064366";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36";

                request.Headers.Add("Cookie", COOKIE);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string content = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                MessageBox.Show( ex.ToString());

            }
            return "";
        }
        #endregion


        private void button1_Click(object sender, EventArgs e)
        {
            //www.baidu.com来做测试
            HtmlDocument doc = this.webBrowser1.Document;
            //HtmlElement keyword = doc.GetElementById("kw");
            //keyword.InnerText = "冰川时代";

            //www.baidu.com来做测试

            //doc.GetElementById("ddownb").InvokeMember("click");

            //if (e1.GetAttribute("name").ToLower() == "j_username")
            //{
            //    //e1.SetAttribute("value", textBox3.Text.Trim());
            //    e1.InvokeMember("click");
            //}


            geturllists();



        }




        public void geturllists()
        {
            try
            {
                for (int i = 2; i < 3; i++)
                {

                    string html = GetUrl("http://www.wanfangdata.com.cn/perio/articleList.do?page=" + i + "&pageSize=10&issue_num=4&publish_year=2017&article_start=&article_end=2018&title_article=&perio_id=lztdxyxb");
                    textBox1.Text = html;

                    MatchCollection TitleMatchs = Regex.Matches(html, @"perioartical([\s\S]*?)""");

                    ArrayList lists = new ArrayList();
                    foreach (Match NextMatch in TitleMatchs)
                    {

                        lists.Add("http://www.wanfangdata.com.cn/details/detail.do?_type=perio&id=" + NextMatch.Groups[1].Value);

                    }



                    if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;


                    foreach (string list in lists)

                    {

                        webBrowser1.Url = new Uri(list);

                        Thread.Sleep(10000);

                        HtmlDocument doc = this.webBrowser1.Document;
                        doc.GetElementById("ddownb").InvokeMember("click");

                    }

                }

            }
            catch ( System.Exception ex)
            
            {

                MessageBox.Show(ex.ToString());
            }

           
        }

        public void click()
        {
            webBrowser1.Url = new Uri("https://xwxy.fanzhoutech.com/admin/applyform/list");

            HtmlDocument doc = this.webBrowser1.Document;
            HtmlElementCollection es = doc.GetElementsByTagName("a"); //GetElementsByTagName返回集合

            foreach (HtmlElement e1 in es)
            {
                if (e1.GetAttribute("name").ToLower() == "j_username")
                {
                    //e1.SetAttribute("value", textBox3.Text.Trim());
                    e1.InvokeMember("click");
                }

            }
        }





    }
}
