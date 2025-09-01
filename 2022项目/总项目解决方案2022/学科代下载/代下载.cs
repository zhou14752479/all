using myDLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace 学科代下载
{
    public partial class 代下载 : Form
    {
        public 代下载()
        {
            InitializeComponent();
        }

        #region 获取重定向网址
        public static string GetRedirectUrl(string url)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            //req.Method = "HEAD";
            req.KeepAlive = true;
            req.Headers.Add("Accept-Encoding", "gzip");
            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/138.0.0.0 Safari/537.36";
            //req.AllowAutoRedirect = false;
            HttpWebResponse myResp = (HttpWebResponse)req.GetResponse();
            bool flag = myResp.StatusCode == HttpStatusCode.Found;
            if (flag)
            {
                url = myResp.GetResponseHeader("Location");
            }
            return url;
        }
        #endregion

        Thread thread;
        bool zanting = true;
        bool status = true;
        private void button1_Click(object sender, EventArgs e)
        {

            MessageBox.Show(GetRedirectUrl("https://www.zxxk.com/soft/46669538.html"));
            //if (thread == null || !thread.IsAlive)
            //{
            //    thread = new Thread(run);
            //    thread.Start();
            //    Control.CheckForIllegalCrossThreadCalls = false;
            //}
        }

        string path = AppDomain.CurrentDomain.BaseDirectory;

        public void run()
        {
            try
            {




                string url = textBox1.Text.Trim();

                string html = method.GetUrl(url, "utf-8");

                string name = Regex.Match(html, @"<title>([\s\S]*?)</title>").Groups[1].Value;

                string sPath = path + "/" + name + "/";
                if (!Directory.Exists(sPath))
                {
                    Directory.CreateDirectory(sPath); //创建文件夹
                }
                MatchCollection aids = Regex.Matches(html, @"<i name=""([\s\S]*?)""");

                label3.Text = "共获取到文档数量：" + aids.Count.ToString();
                for (int a = 0; a < aids.Count; a++)
                {

                    label3.Text = "共获取到文档数量：" + aids.Count.ToString() + "正在下载：" + (a + 1);

                    string aurl = "http://113.250.184.46:8888/api.aspx?method=getfile&key=asdasdasd&link=" + aids[a].Groups[1].Value;
                    string ahtml = method.GetUrl(aurl, "utf-8");
                    string filename = Regex.Match(ahtml, @"""filename"":""([\s\S]*?)""").Groups[1].Value;
                    string fileurl = Regex.Match(ahtml, @"""fileurl"":""([\s\S]*?)""").Groups[1].Value;

                    method.downloadFile(fileurl, sPath, filename, "");
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(filename);


                    if (listView1.Items.Count > 2)
                    {
                        this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                    }
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;

                    Thread.Sleep(1000);

                }





                MessageBox.Show("下载完成");



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        private void 代下载_Load(object sender, EventArgs e)
        {

        }
    }
}
