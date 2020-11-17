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

namespace 模拟采集1
{
    public partial class 搜索采集文章 : Form
    {
        string[] keywords = { };

        string yuming = "www.51sole.com";
        bool zanting = true;
        bool status = true;
        string path = AppDomain.CurrentDomain.BaseDirectory+"txt\\";
        string cookie = "CXID=F873D12A84F66C63025A4B68E9A456FE; SUID=DAF7EA791824A00A5EC0A5DC000EF75E; ssuid=126331545; IPLOC=CN3213; SUV=1594956626184150; pgv_pvi=9442649088; usid=njfBMcqxCUQg55HY; wuid=AAHXqmBoMAAAAAqMGhzJjgAAZAM=; FREQUENCY=1596415196724_1; front_screen_resolution=750*1334; browerV=3; osV=1; sw_uuid=8218142946; ABTEST=8|1598932952|v17; SNUID=9A0C73EB92943E86B84809A6921199D6; ld=elllllllll2K9SHzlllllVdWswwlllllGUBNbkllllUlllllRklll5@@@@@@@@@@; sst0=918; LSTMV=683%2C388; LCLKINT=5685";
        public 搜索采集文章()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public string GetUrl(string Url)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://www.sogou.com/";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", cookie);
             
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
        

        /// <summary>
        /// 读取关键字
        /// </summary>
        public void getkeys()
        {
            StreamReader streamReader = new StreamReader(path + "key.txt", Encoding.Default);
            string text = streamReader.ReadToEnd();
            string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < array.Length; i++)
            {
                textBox1.Text += array[i] + "\r\n";

            }
            streamReader.Close();
            groupBox1.Text = "共读取到"+array.Length+"个关键字";
        }
        /// <summary>
        /// 保存文本
        /// </summary>
        /// <param name="key"></param>
        /// <param name="neirong"></param>
        public void baocun(string key,string neirong)
        {
            string path1 = AppDomain.CurrentDomain.BaseDirectory;
            FileStream fs1 = new FileStream(path1+ "jtn\\"+ key+".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1);
            sw.WriteLine(neirong);
            sw.Close();
            fs1.Close();

        }
        /// <summary>
        /// 谷歌翻译伪原创
        /// </summary>
        /// <param name="key"></param>
        public void googleTranslate(string key)
        {
            StringBuilder sb = new StringBuilder();
            string path1 = AppDomain.CurrentDomain.BaseDirectory;
            StreamReader sr = new StreamReader(path1 + "jtn\\" + key + ".txt", Encoding.GetEncoding("utf-8"));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != "" && !text[i].Contains("<"))
                {
                    string yingwen = method.translate("zh-CN", "en", text[i]);
                    Thread.Sleep(2000);
                    string result = method.translate("en", "zh-CN", yingwen);
                   
                    if (result == "")
                    {
                        result = text[i];
                    }

                    sb.AppendLine(result);
                }
                else
                {
                    sb.AppendLine(text[i]);
                }

            }
            sr.Close();

            baocun(key,sb.ToString());

            //neirong = method.translate("zh-CN", "en", neirong);

        }

        /// <summary>
        /// 替换品牌词
        /// </summary>
        /// <param name="key"></param>
        /// <param name="neirong"></param>
        public string tihuan(string neirong)
        {
            Random random = new Random();
            //品牌词
            StreamReader sr0 = new StreamReader(path + "jtn.txt", Encoding.Default);
            //一次性读取完 
            string jtns = sr0.ReadToEnd();
            string[] jtn = jtns.Split(new string[] { "\r\n" }, StringSplitOptions.None);


            StreamReader sr = new StreamReader(path+"pinpai.txt", Encoding.Default);
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int i = 0; i < text.Length; i++)
            {
               
                if (jtn.Length > 1)
                {
                   
                  neirong= neirong.Replace(text[i], jtn[random.Next(0, jtn.Length - 1)]);
                }
            }
            sr0.Close();
            sr.Close();
            return neirong;
        }

        /// <summary>
        /// 插入关键词
        /// </summary>
        public string charu()
        {
            Random random = new Random();
            string guanjianci = "";
            StreamReader sr = new StreamReader(path + "huojia.txt", Encoding.GetEncoding("utf-8"));
            //一次性读取完 
            string texts = sr.ReadToEnd();
           
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);


            sr.Close();
            if (text.Length > 1)
            {
              guanjianci=  text[random.Next(0, text.Length)];
            }
            return guanjianci;
        }


        /// <summary>
        /// 插入图片
        /// </summary>
        public string charuTupian()
        {
            Random random = new Random();
            string url = "";
            StreamReader sr = new StreamReader(path + "img.txt", Encoding.Default);
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);


            sr.Close();
            url = text[0] + random.Next(Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox4.Text)) + ".jpg";
            return url;
        }
        /// <summary>
        /// 搜狗搜索
        /// </summary>




        public void run(string keyword)
        {
           
            int num = 0;
            StringBuilder sb = new StringBuilder();
            
           

            MatchCollection links = Regex.Matches(html, @"&url=http([\s\S]*?)&did");
            foreach (Match link in links)
            {
                if (num > 7)
                {
                    textBox2.Text += DateTime.Now.ToString() + "正在合成文章：" + keyword + "\r\n";
                    baocun(keyword, sb.ToString());
                    googleTranslate(keyword);
                    break;
                }

                string aurl = "http" + System.Web.HttpUtility.UrlDecode(link.Groups[1].Value);

                string ahtml = GetUrl(aurl);
                Match body = Regex.Match(ahtml, @"<div class=""pd_d_t_info"">([\s\S]*?)<div");
                string neirong = Regex.Replace(body.Groups[1].Value, "<[^>]+>", "");


                neirong = tihuan(neirong);//替换品牌词

                string tupianName = charu();

                string tupian = "<p style=\"text-align: center;\"><img src=\"" + charuTupian() + "\" title=\"" + tupianName + "\" alt=\"" + tupianName + "\"></p>";

                if (neirong.Trim() != "")
                {
                    sb.Append(charu());  //插入关键词
                    sb.Append(Regex.Replace(neirong, "<(?!/?p)[^>]*>", ""));
                    sb.Append(charu());
                    if (num == 1 || num == 3 || num == 5 || num == 7)
                    {
                        sb.Append(tupian);  //插入图片
                    }
                }

             
                num = num + 1;
            }
        }


        string html = "";
        string keyword = "";

       
        private void 搜索采集文章_Load(object sender, EventArgs e)
        {
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser2.ScriptErrorsSuppressed = true;

            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WB_DocumentCompleted);
        }

        private void WB_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                return;
            if (e.Url.ToString() != webBrowser1.Url.ToString())
                return;


            if (webBrowser1.DocumentText.Contains("</html>"))
            {

                html = webBrowser1.DocumentText;
             
                run(keyword);
                status = true;
            }
            else
            {
                Application.DoEvents();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text =DateTime.Now.ToString()+ "：正在采集请稍后......"+"\r\n";
             keywords = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (var keyword in keywords)
            {
                if (keyword == "")
                {
                    continue;
                }
                this.keyword = keyword;
                status = false;
                string url = "https://www.sogou.com/web?query=" + System.Web.HttpUtility.UrlEncode(keyword).Trim() + "+site%3A" + yuming + "&page=1&ie=utf8";
                webBrowser1.Navigate(url);
                while (this.status == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }

                
            }

    }
        // neirong= method.translate("zh-CN", "en", neirong);
        Thread thread;
        private void button2_Click(object sender, EventArgs e)
        {
            thread = new Thread(getkeys);
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            
            zanting = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            zanting = true;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                webBrowser2.Navigate("https://www.sogou.com/");
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                webBrowser2.Navigate("https://www.baidu.com/");
            }
        }
    }
}
