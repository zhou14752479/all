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
    public partial class 移动套餐查询 : Form
    {
        public 移动套餐查询()
        {
            InitializeComponent();
        }
        bool zanting = true;
        string pubArgs = "YwHWsAlvxZzWTl3U2U/wipedbGUGw8CTdgNevSM48IR9wRbEu/XkQ2ic58+Wz6q7jkvPZlpokS6m1fZHKLHTICPzj71z5Cpr7Db1810h1+cMg86SuuMgSANYktusnes8XDAjyG9ckRRlZ1lXfchhlS99C8Aw/KJ2EWw/8LoS3znvmYnm7LCRe/0gkQy9B4BNf60XXcRsDaxLC1KundVi/4ePw5YIveVxLSsFDF3A+Cs9O0l/j9lS/Bpm5j21bI1Ul3HlpDkVGqU7qhNJA80cwYBfTO59bDIp5jFXi91xFCM=";
        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public string PostUrl(string url, string postData)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
            

                request.ContentType = "application/json";
                request.ContentLength = postData.Length;
               // request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("pubArgs",pubArgs);
              

               // request.Referer = "";
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
        public void run()
        {
            long start = Convert.ToInt64(textBox1.Text);
            long end = Convert.ToInt64(textBox2.Text);
            for (long i = start; i <= end; i++)
            {
                string url = "https://h5.sd.chinamobile.com/cmtj/openingCard/consumer/campnInfo/getWorkBenchView";
                string postdata = "{\"appId\" : \"100000056\", \"userId\" : \"1097395671845093378\",\"serialNumber\" : \""+i+"\"}";
                string html = PostUrl(url,postdata);
               textBox3.Text = html;
                Match chengxiao = Regex.Match(html, @"承消([\s\S]*?)元");
                MatchCollection taocans = Regex.Matches(html, @"""campaignName"":""([\s\S]*?)""");
                label3.Text = "正在查询：" + i;
                StringBuilder sb = new StringBuilder();
                foreach (Match taocan in taocans)
                {
                    sb.Append(taocan.Groups[1].Value+"    ");
                }
                if (chengxiao.Groups[1].Value != "" && chengxiao.Groups[1].Value != "20" && chengxiao.Groups[1].Value != "15" && chengxiao.Groups[1].Value != "10")
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(i.ToString());
                    lv1.SubItems.Add(chengxiao.Groups[1].Value);
                    lv1.SubItems.Add(sb.ToString());

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                }
                else
                {
                    label3.Text = "正在查询：" + i + " 不符合";
                }

                Thread.Sleep(500);
            }

            MessageBox.Show("查询完成");

        }
        private void 移动套餐查询_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
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
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void 移动套餐查询_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
                //System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
