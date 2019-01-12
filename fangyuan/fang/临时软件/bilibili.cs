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

namespace fang.临时软件
{
    public partial class bilibili : Form
    {
        public bilibili()
        {
            InitializeComponent();
        }
        bool status = true;
        private void bilibili_Load(object sender, EventArgs e)
        {

        }
        #region  如果之前的请求获取不到源码就用这个去获取
        public static string GetHtmlSource(string url)
        {
            try
            {
                Uri uri = new Uri(url);
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(uri);
                myReq.UserAgent = "User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705";
                myReq.Accept = "*/*";
                myReq.KeepAlive = true;
                myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
                HttpWebResponse result = (HttpWebResponse)myReq.GetResponse();
                Stream receviceStream = result.GetResponseStream();
                StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding("utf-8"));
                string strHTML = readerOfStream.ReadToEnd();
                readerOfStream.Close();
                receviceStream.Close();
                result.Close();

                return strHTML;
            }
            catch (Exception ex)
            {
                throw new Exception("采集指定网址异常，" + ex.Message);
            }
        }
        #endregion
        #region  哔哩哔哩
        public void run()
        {



            if (textBox2.Text == "")
            {
                MessageBox.Show("请输入网址！");
                return;
            }

            try
            {
                string[] URLs = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                foreach (string URL in URLs)
                {

           
                        string strhtml = GetHtmlSource(URL);

                   

                    MatchCollection mulus = Regex.Matches(strhtml, @"""part"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    Match aid = Regex.Match(strhtml, @"""aid"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    Match cid = Regex.Match(strhtml, @"""cid"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    for (int i = 1; i < mulus.Count+1; i++)
                    {

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count+1).ToString());
                        lv1.SubItems.Add(mulus[i-1].Groups[1].Value.Trim());
                        lv1.SubItems.Add("http://player.bilibili.com/player.html?aid="+aid.Groups[1].Value+"&cid="+cid.Groups[1].Value+"&page="+i);
                       

                    }

                    Application.DoEvents();
                        Thread.Sleep(Convert.ToInt32(1000));

                        if (listView1.Items.Count > 1)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }

                        while (this.status == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }





                    

                }
            }


            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.status = false; 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.status =true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();
        }
    }
}
