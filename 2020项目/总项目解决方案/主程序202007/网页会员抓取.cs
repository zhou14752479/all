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
using helper;

namespace 主程序202007
{
    public partial class 网页会员抓取 : Form
    {
        public 网页会员抓取()
        {
            InitializeComponent();
        }

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/json";

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
                request.Headers.Add("Cookie", cookie);

                request.Referer = "http://dsbet8888.com/dsapiag/app/index";
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
      static  string cookie = "JSESSIONID=jB6P_B2yFZjnefEt7PwsyxSegBF_VDiSGuBp4z1a.dsapi-ag";

        bool zanting = true;
        bool status = true;

        
        public void run()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string[] text = textBox3.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int i = 0; i < text.Length; i++)
            {

                StringBuilder sb = new StringBuilder();
                label1.Text = "正在抓取第： " +text[i] + "，数据量大请稍后..."+i;
                string url = "https://kiosk.fruitfarm88.com/api/player/list?timeperiod=&startdate=2016-01-01&enddate=2020-08-07&viplevel=&kioskname=&page=1&perPage=100000&adminname=&sortby=signupdate" + text[i].Trim();

                string cookie = "kioskadmin_secret=62BE57BB77A80CAB39E364ECC05185D4; PHPSESSID=1a645b4aacaefe2074971464e6889d62; selected_kioskcode="+text[i];
                string html = method.GetUrlWithCookie(url,cookie,"utf-8");


                Match daili = Regex.Match(html, @"""KIOSKNAME"":""([\s\S]*?)""");

                MatchCollection zhanghus = Regex.Matches(html, @"""PLAYERNAME"":""([\s\S]*?)""");

                if (zhanghus.Count == 0)
                    continue;
                

                for (int j = 0; j < zhanghus.Count; j++)
                {

                    //ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                    //lv1.SubItems.Add(zhanghus[j].Groups[1].Value);
                    //lv1.SubItems.Add(nichengs[j].Groups[1].Value);
                    //lv1.SubItems.Add(yues[j].Groups[1].Value);
                    sb.Append(zhanghus[j].Groups[1].Value+" # "+daili.Groups[1].Value + "\r\n");

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                    {
                        return;
                    }
                }


                using (StreamWriter fs = new StreamWriter(path + "aaa.txt", true))
                {
                    fs.WriteLine(sb.ToString());
                }
                Thread.Sleep(2000);
            }
            
            
        }

        private void 网页会员抓取_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "开始查询......";
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"6666"))
            {
         
               MessageBox.Show("验证失败");
                return;
            }


            #endregion
           
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            cookieBrowser co = new cookieBrowser("http://dsbet8888.com/dsapiLoginAg/app/frontpage");
            co.Show();
        }
    }
}
