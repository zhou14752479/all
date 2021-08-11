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
using myDLL;

namespace 百姓二手车
{
    public partial class 百姓二手车 : Form
    {
        public 百姓二手车()
        {
            InitializeComponent();
        }
        Thread thread;
        bool zanting = true;
        bool status = true;

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
                headers.Add("BAIXING-SESSION:$2y$10$9gfzFoWpawABvFcguA7geOc6ZouzDAE.IJJWyJVLEOfyr/Gsn/G2a");
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


        Dictionary<string, string> dics = new Dictionary<string, string>();
        public void getcitys()
        {
            comboBox1.Items.Clear();
            dics.Clear();
            string url = "https://mpapi.baixing.com/v1.3.6/ ";
            string postdata = "{\"area.getAllCities\":{}}";
            string html = PostUrl(url, postdata, "", "utf-8");
        
            MatchCollection ids = Regex.Matches(html, @"""id"":""([\s\S]*?)"",""name"":""([\s\S]*?)""");
            foreach (Match item in ids)
            {
                if (!dics.ContainsKey(item.Groups[2].Value))
                {
                    comboBox1.Items.Add(item.Groups[2].Value);
                    dics.Add(item.Groups[2].Value, item.Groups[1].Value);
                }
            }
        }
        #region 百姓二手车
        public void baixing()
        {

            string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].Trim() == "")
                {
                    continue;
                }
                try
                {
                    string n = dics[text[i]];


                    for (int page = 1; page < 101; page++)
                    {

                        string url = "https://mpapi.baixing.com/v1.3.6/";
                        string postdata = "{\"listing.getAds\":{\"areaId\":\"" + n + "\",\"categoryId\":\"cheliang\",\"page\":" + page + ",\"notAllowChatOnly\":1}}";
                        string html = PostUrl(url, postdata, "", "utf-8").Replace("\"user\":{\"id", "");

                        MatchCollection ids = Regex.Matches(html, @"""id"":""([\s\S]*?)""");
                        MatchCollection titles = Regex.Matches(html, @"""title"":""([\s\S]*?)""");
                        MatchCollection tels = Regex.Matches(html, @"""mobile"":""([\s\S]*?)""");
                        MatchCollection citys = Regex.Matches(html, @"""cityCName"":""([\s\S]*?)""");

                        if (ids.Count == 0)
                        {
                           break;
                        }
                        for (int a = 0; a < ids.Count; a++)
                        {
                            try
                            {
                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据

                                lv1.SubItems.Add(ids[a].Groups[1].Value);
                                lv1.SubItems.Add(titles[a].Groups[1].Value);
                                lv1.SubItems.Add(tels[a].Groups[1].Value);
                                lv1.SubItems.Add(citys[a].Groups[1].Value);
                                lv1.SubItems.Add(n);
                                Thread.Sleep(50);
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
                  MessageBox.Show(ex.ToString());

                }

            }


        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            if (DateTime.Now > Convert.ToDateTime("2021-09-08"))
            {
                return;
            }
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(baixing);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
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

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void 百姓二手车_Load(object sender, EventArgs e)
        {
            getcitys();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
                textBox1.Text += comboBox1.Text + "\r\n";
            
        }
    }
}
