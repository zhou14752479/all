using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsharpHttpHelper;
using helper;

namespace 主程序202007
{
    public partial class nike监控 : Form
    {
        public nike监控()
        {
            InitializeComponent();
        }

       
        private string GetHttp(string url)
        {
            HttpHelper http = new HttpHelper();
           HttpItem item = new HttpItem()
            {
                URL = url,
                Method = "GET",
                Host = "www.nike.com",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36",
                Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9",
                Cookie = Form1.cookie,
                Referer= "https://www.nike.com/cn/",
           };
            item.Header.Add("Sec-Fetch-Site", "none");
            item.Header.Add("Sec-Fetch-Mode", "navigate");
            item.Header.Add("Sec-Fetch-User", "?1");
            item.Header.Add("Sec-Fetch-Dest", "document");
            item.Header.Add("Accept-Encoding", "gzip, deflate, br");
            item.Header.Add("Accept-Language", "zh-CN,zh;q=0.9");
           HttpResult result = http.GetHtml(item);
            string html = result.Html;
           
           
            return html;
        }

        bool zhixing = false;

        Dictionary<string, string> dic = new Dictionary<string, string>();
        public void run()
        {
            zhixing = true;
            string[] text = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
        
            foreach (string url in text)
            {
                try
                {

   

                //string url = "https://www.nike.com/cn/t/dbreak-%E5%A5%B3%E5%AD%90%E8%BF%90%E5%8A%A8%E9%9E%8B-BbTxmx/CK2351-007";

                string html = GetHttp(url);

                Match title = Regex.Match(html, @"""title"":""([\s\S]*?)""");
                Match price = Regex.Match(html, @"currentPrice"":([\s\S]*?),");

                Match skuHtml = Regex.Match(html, @"""skus""([\s\S]*?)\]");
                Match availableHtml = Regex.Match(html, @"""availableSkus"":\[([\s\S]*?)\]");



                MatchCollection skus = Regex.Matches(skuHtml.Groups[1].Value, @"""skuId"":""([\s\S]*?)"",""localizedSize"":""([\s\S]*?)""");

                MatchCollection askus = Regex.Matches(availableHtml.Groups[1].Value, @"""id"":""([\s\S]*?)""");
                for (int i = 0; i < skus.Count; i++)
                {
                    if (!dic.ContainsKey(skus[i].Groups[1].Value))
                    {
                        dic.Add(skus[i].Groups[1].Value, skus[i].Groups[2].Value);  //key为skuid
                    }
                }

                StringBuilder sb = new StringBuilder();
                StringBuilder sb2 = new StringBuilder();
                for (int j = 0; j < askus.Count; j++)
                {
                    if (dic.ContainsKey(askus[j].Groups[1].Value))
                    {
                        sb.Append(dic[askus[j].Groups[1].Value] + "  ");
                    }

                }

                    textBox1.Text += title.Groups[1].Value + ": 价格：" + price.Groups[1].Value + " 有货尺码：" + "\r\n" + sb.ToString()+"\r\n";
                }
                catch 
                {

                    continue;
                }

            }
            zhixing = false;

        }
        private void nike监控_Load(object sender, EventArgs e)
        {
           
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"yidong"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion

            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
            timer1.Start();
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (zhixing == false)
            {
                textBox1.Text = "";
               
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            }
            else
            {
                //textBox1.Text += "函数未执行结束" + "\r\n";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 fm1 = new Form1();
            fm1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }
    }
}
