using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using helper;

namespace 主程序202008
{
    public partial class 百度搜索 : Form
    {
        public 百度搜索()
        {
            InitializeComponent();
        }

        bool zanting = true;

        


        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "CXID=F873D12A84F66C63025A4B68E9A456FE; SUID=DAF7EA791824A00A5EC0A5DC000EF75E; ssuid=126331545; sw_uuid=9269334666; IPLOC=CN3213; SUV=1594956626184150; browerV=3; osV=1; ABTEST=7|1596247376|v17; SNUID=F1BFC05A232689E56EF8708623CC1C36; pgv_pvi=9442649088; usid=njfBMcqxCUQg55HY; wuid=AAHXqmBoMAAAAAqMGhzJjgAAZAM=; FREQUENCY=1596415196724_1; front_screen_resolution=750*1334; sgwtype=3; ld=Oyllllllll2K9SHzlllllVDXBTUlllllGUBNbkllllollllllZlll5@@@@@@@@@@; sst0=581; LSTMV=142%2C391; LCLKINT=6307";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://www.sogou.com/web?query=%E5%8C%BA%E5%9D%97%E9%93%BE&_asf=www.sogou.com&_ast=&w=01015002&p=40040108&ie=utf8&from=index-nologin&s_from=index&oq=&ri=0&sourceid=sugg&suguuid=&sut=0&sst0=1596417909581&lkt=0%2C0%2C0&sugsuv=1594956626184150&sugtime=1596417909581/";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                // request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 3000;
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

        #region GET使用代理IP请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlwithIP(string Url, string ip)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";
                WebProxy proxy = new WebProxy(ip);
                request.Proxy = proxy;

                //request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //request.KeepAlive = true;
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
                MessageBox.Show(ex.ToString());

            }
            return "";
        }
        #endregion

        public string baohan(string html)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (html!=""&& html.Contains(text[i]))
                {
                    return text[i];
                }
            }
            return "";

        }

        ArrayList finishes = new ArrayList();
        public static string cookie = "";
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {

            string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                for (int page = 1; page < 100; page++)
                {
                  
                    
                    string keyword = System.Web.HttpUtility.UrlEncode(text[i].ToString());
                    string url = "https://m.so.com/index.php?q="+keyword+"&pn="+page+"&psid=4d9d5dcc9b290c2a56eb9b1d3dba3237&src=srp_paging&fr=none";

                    //  string html = GetUrlwithIP(url, ips[a].ToString());
                    string html = GetUrl(url, "utf-8");

                    textBox2.Text = html;
                    MessageBox.Show("1");
                    MatchCollection urls = Regex.Matches(html, @"<a class=alink([\s\S]*?)u=([\s\S]*?)/");
                   
                  



                    foreach (Match yuming in urls)
                    {
                        try
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(yuming.Groups[2].Value );
                            lv1.SubItems.Add("1");




                            //string yum = "http://"+yuming.Groups[1].Value;
                            //if (!finishes.Contains(yum) && !yum.Contains("360") && !yum.Contains("sogou"))
                            //{
                            //    finishes.Add(yum);

                            //        string strhtml = GetUrl(yum + "/robots.txt", "utf-8");

                            //        string key = baohan(strhtml);
                            //        if (key != "")
                            //        {
                            //            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            //            lv1.SubItems.Add(yum + "/" + key);
                            //            lv1.SubItems.Add("1");
                            //        }
                            //        else
                            //        {
                            //            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            //            lv1.SubItems.Add(yum);
                            //            lv1.SubItems.Add("0");

                            //        }
                            //}
                            if (listView1.Items.Count - 1 > 1)
                            {
                                listView1.EnsureVisible(listView1.Items.Count - 1);
                            }


                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                        }
                        
                        catch 
                        {

                            continue;
                        }
                    }
                    Thread.Sleep(1000);
                }

            }
        }

        #region 获取ip文本
        ArrayList ips = new ArrayList();
        public void getIps()
        {

            string path = AppDomain.CurrentDomain.BaseDirectory;
                StreamReader streamReader = new StreamReader(path+"ip.txt", Encoding.Default);
                string text = streamReader.ReadToEnd();
                string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                ips.Add(array[i].Trim());

                }

        }
        #endregion

        private void 百度搜索_Load(object sender, EventArgs e)
        {
            getIps();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                StreamReader streamReader = new StreamReader(this.openFileDialog1.FileName, Encoding.Default);
                string text = streamReader.ReadToEnd();
                string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    textBox1.Text += array[i] + "\r\n";

                }

            }
        }
        string[] text = { };
        private void button2_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"jdwl"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion

            //cookie = cookieBrowser.cookie;
             text = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            //string[] keys = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            //for (int i = 0; i < keys.Length; i++)
            //{

            //    Thread thread = new Thread(new ParameterizedThreadStart(run));
            //    string o = keys[i];
            //    thread.Start((object)o);
            //    Control.CheckForIllegalCrossThreadCalls = false;
            //}


            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        #region 获取页面跳转后Url
        static string fanhuiurl(string originalAddress)
        {
            //string redirectUrl;
            //WebRequest myRequest = WebRequest.Create(originalAddress);
            //myRequest.Timeout = 5000;

            //WebResponse myResponse = myRequest.GetResponse();
            //redirectUrl = myResponse.ResponseUri.ToString();

            //myResponse.Close();
            //return redirectUrl;

            string url = "";
                HttpWebRequest request = WebRequest.Create(originalAddress) as HttpWebRequest;
                request.AllowAutoRedirect = false;
            request.Timeout = 3000;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                url = response.Headers["Location"];
                 response.Close();
             
               request = WebRequest.Create(originalAddress) as HttpWebRequest;
                 request.AllowAutoRedirect = false;
                 request.Referer = originalAddress;
                 response = request.GetResponse() as HttpWebResponse;
            
          
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
            string content = reader.ReadToEnd();
            Match fanhuiurl = Regex.Match(content, @"URL='([\s\S]*?)'");

            response.Close();
            return fanhuiurl.Groups[1].Value;

        }

        #endregion
        private void button6_Click(object sender, EventArgs e)
        {
           
            listView1.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        #region 导出文本
        public static void expotTxt(ListView lv1)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {

                StringBuilder sb = new StringBuilder();
                foreach (ListViewItem item in lv1.Items)
                {
                    try
                    {
                        List<string> list = new List<string>();
                        string temp = item.SubItems[1].Text;
                        string temp1 = item.SubItems[2].Text;
                        if (temp1.Trim() == "1")
                        {
                            list.Add(temp);
                            foreach (string tel in list)
                            {

                                sb.AppendLine(tel);
                            }

                            string path = "";

                            path = dialog.SelectedPath + "\\导出结果.txt";

                            System.IO.File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
                MessageBox.Show("导出完成");
            }

        }
        #endregion

        private void button4_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            expotTxt(listView1);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            cookieBrowser bro = new cookieBrowser("https://passport.baidu.com/v2/?login");
            bro.Show();
        }
    }
}
