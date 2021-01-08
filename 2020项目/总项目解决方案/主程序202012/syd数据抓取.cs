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
using helper;

namespace 主程序202012
{
    public partial class syd数据抓取 : Form
    {
        public syd数据抓取()
        {
            InitializeComponent();
        }

        private void syd数据抓取_Load(object sender, EventArgs e)
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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "__cfduid=dac4bdb62e19ea788a64352698521822e1608523955; _ga=GA1.3.1085192139.1608523958; _gid=GA1.3.1373397225.1609724850; _gat_gtag_UA_122646694_1=1; XSRF-TOKEN=eyJpdiI6IkRHRUJcL2JSaUc5VTBUUEtFQVNnaGZnPT0iLCJ2YWx1ZSI6ImhVd2s3OWdFVjBvTk5RMFl0RUVaTFZtYXQzMWJaSXhETnVCajRJbmU5VVh3Y2VtT1YwU3lkQ1lKTUtKXC9yOWQ5aVh0aDlPZkhRSnlQc24yWEhNZlIxdz09IiwibWFjIjoiMmEyOGEwNjcwNWEzZjAwYjQzZWIyZjQ0N2I0OTE3ZmU0YzY3NGU0NDI4MWNmYTA5ZTQ0MmExODJlZDRkN2E1NSJ9; laravel_session=eyJpdiI6Ik52UnF4ait5aXBHNjhIWWNiNXVjTkE9PSIsInZhbHVlIjoielwvOCtweTdSNXZUTjdZTTc2MDRwRnlvNnJrQmNncExqdjFjYmRZamZTTFlZa29ENFJkMk5rWExmaHpaZHlZbmM3Q2NoeXdvd1JWUUxFUmZEcGJzZ1pBPT0iLCJtYWMiOiJhYmNjZjA3OWIxMjMxYTVmODZlMGQ0MGE5NjhkNmU1OTFiNjAzYTM2MzQ4M2FhZDc4NjVhMzMxYjYwYTBkMGEyIn0%3D";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://www.syd.com.mx/Catalogo/";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
               
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                WebHeaderCollection headers = request.Headers;
                headers.Add("X-Requested-With:XMLHttpRequest");
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
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
        bool zanting = true;

        int num = 1;
        








        public void run()
        {


            for (int i = 1; i < 464; i++)
            {

                string url = "https://www.syd.com.mx/Catalogo/getArticlesLnAzul?page=" + i;

                string html = GetUrl(url);
                MatchCollection aids = Regex.Matches(html, @"alt=\\""([\s\S]*?)\\""");
                textBox7.Text = i.ToString();
                for (int j = 0; j < aids.Count; j++)
                {
                    try
                    {

                        string uid = aids[j].Groups[1].Value.Replace("\\", "");
                        string aurl = "https://www.syd.com.mx/Catalogo/showArticle?idArticle=" + uid + "&_=1609727780997";
                        string ahtml = GetUrl(aurl);

                        Match title = Regex.Match(ahtml, @"data-title=\\""([\s\S]*?)\\""");
                        MatchCollection a2 = Regex.Matches(ahtml, @"#collapse([\s\S]*?)>([\s\S]*?)-([\s\S]*?)<");

                      

                        string[] texthtml = ahtml.Split(new string[] { "#collapse" }, StringSplitOptions.None);
                        for (int z = 0; z < a2.Count; z++)
                        {
                            MatchCollection a4 = Regex.Matches(texthtml[z+1], @"\\n\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t([\s\S]*?)\\n");

                            StringBuilder sb = new StringBuilder();
                            foreach (Match item in a4)
                            {
                                sb.Append(item.Groups[1].Value+",");
                            }
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(uid);
                            lv1.SubItems.Add(num.ToString());
                            lv1.SubItems.Add(Regex.Replace(title.Groups[1].Value, "<[^>]+>", "").Trim() + " " + uid);
                            lv1.SubItems.Add("http://www.syd.com.mx/Catalogo/fotos/" + uid.Replace("/", "_") + ".jpg");
                            lv1.SubItems.Add(a2[z].Groups[2].Value);
                            lv1.SubItems.Add(a2[z].Groups[3].Value);
                            lv1.SubItems.Add(sb.ToString());
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }

                        }

                        num = num + 1;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        continue;
                    }
                }



            }



        }


        public ArrayList getbrandId(int i)
        {
            ArrayList list = new ArrayList();
            string url = "https://www.syd.com.mx/Catalogo/getBrands/" + i;

            string html = GetUrl(url);
            MatchCollection aids = Regex.Matches(html, @"""IdBrand"":([\s\S]*?)}");

            for (int j = 0; j < aids.Count; j++)
            {
                list.Add(aids[j].Groups[1].Value);
            }
            return list;

        }


        public void run1()
        {
            //ArrayList lists1 = getbrandId(1);
             ArrayList lists2 = getbrandId(4);
            foreach (string brandId in lists2)
            {

                for (int i = 1; i < 9999; i++)
                {


                    string url = "https://www.syd.com.mx/Catalogo/getArticles/0/1/" + brandId + "?page=" + i;

                    string html = GetUrl(url);
                    MatchCollection aids = Regex.Matches(html, @"alt=\\""([\s\S]*?)\\""");
                    if (aids.Count == 0)
                    {
                        break;
                    }
                    textBox7.Text = brandId + "  " + i.ToString();
                    for (int j = 0; j < aids.Count; j++)
                    {
                        try
                        {

                            string uid = aids[j].Groups[1].Value.Replace("\\", "");
                            string aurl = "https://www.syd.com.mx/Catalogo/showArticle?idArticle=" + uid + "&_=1609727780997";
                            string ahtml = GetUrl(aurl);

                            Match title = Regex.Match(ahtml, @"data-title=\\""([\s\S]*?)\\""");
                            MatchCollection a2 = Regex.Matches(ahtml, @"#collapse([\s\S]*?)>([\s\S]*?)-([\s\S]*?)<");



                            string[] texthtml = ahtml.Split(new string[] { "#collapse" }, StringSplitOptions.None);
                            for (int z = 0; z < a2.Count; z++)
                            {
                                MatchCollection a4 = Regex.Matches(texthtml[z + 1], @"\\n\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t\\t([\s\S]*?)\\n");

                                StringBuilder sb = new StringBuilder();
                                foreach (Match item in a4)
                                {
                                    sb.Append(item.Groups[1].Value + ",");
                                }
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(uid);
                                lv1.SubItems.Add(num.ToString());
                                lv1.SubItems.Add(Regex.Replace(title.Groups[1].Value, "<[^>]+>", "").Trim() + " " + uid);
                                lv1.SubItems.Add("http://www.syd.com.mx/Catalogo/fotos/" + uid.Replace("/", "_") + ".jpg");
                                lv1.SubItems.Add(a2[z].Groups[2].Value);
                                lv1.SubItems.Add(a2[z].Groups[3].Value);
                                lv1.SubItems.Add(sb.ToString());
                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }

                            }

                            num = num + 1;

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                            continue;
                        }
                    }


                }

            }
        }

        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run1);
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
    }
}
