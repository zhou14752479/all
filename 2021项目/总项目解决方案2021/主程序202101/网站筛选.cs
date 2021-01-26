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

namespace 主程序202101
{
    public partial class 网站筛选 : Form
    {
        public 网站筛选()
        {
            InitializeComponent();
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
                HttpWebRequest httpWebRequest = null;
                HttpWebResponse httpWebRespones = null;
                string strHtml = string.Empty;
                if (!Regex.IsMatch(Url, @"^https?://", RegexOptions.IgnoreCase))
                    Url = "http://" + Url;
                httpWebRequest = WebRequest.Create(Url) as HttpWebRequest;
                httpWebRequest.Timeout = 3000;
                httpWebRequest.ReadWriteTimeout = 60000;
                httpWebRequest.AllowAutoRedirect = true;
                httpWebRequest.UserAgent =
                    "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; Maxthon 2.0)";
                httpWebRespones = (HttpWebResponse)httpWebRequest.GetResponse();
                if (httpWebRespones.ContentType.ToLower().IndexOf("text/html") == -1)
                {
                    httpWebRespones.Close();
                    return string.Empty;
                }
                using (Stream stream = httpWebRespones.GetResponseStream())
                {
                    List<byte> lst = new List<byte>();
                    int nRead = 0;
                    while ((nRead = stream.ReadByte()) != -1) lst.Add((byte)nRead);
                    byte[] byHtml = lst.ToArray();
                    //utf8的编码比较多 所以默认先用他解码
                    strHtml = Encoding.UTF8.GetString(byHtml, 0, byHtml.Length);
                    //就算编码没对也不会影响英文和数字的显示 然后匹配真正编码
                    string strCharSet =
                        Regex.Match(strHtml, @"<meta.*?charset=""?([a-z0-9-]+)\b", RegexOptions.IgnoreCase)
                        .Groups[1].Value;
                    //如果匹配到了标签并且不是utf8 那么重新解码一次
                    if (strCharSet != "" && (strCharSet.ToLower().IndexOf("utf") == -1))
                    {
                        try
                        {
                            strHtml = Encoding.GetEncoding(strCharSet).GetString(byHtml, 0, byHtml.Length);
                        }
                        catch { }
                    }
                }
                return strHtml;




            }
            catch (System.Exception ex)
            {
                ex.ToString();

            }
            return "";
        }
        #endregion

        List<int> lists = new List<int>();

        public void run()
        {
            int start = Convert.ToInt32(textBox1.Text);
            int end = start + 999999;
            for (int i = start; i < end; i++)
            {
                if (!lists.Contains(i))
                {
                    lists.Add(i);

                    try
                    {
                        string url = "http://www." + i + ".com";
                        label3.Text = url;
                        string html = GetUrl(url);
                        if (html == "")
                            continue;

                        Match title = Regex.Match(html, @"<title>([\s\S]*?)</title>");

                        if (title.Groups[1].Value != "")
                        {

                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(url);
                            lv1.SubItems.Add(title.Groups[1].Value);

                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)
                                return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        continue;
                    }
                }

            }



        }

       
            bool zanting = true;
            bool status = true;
            private void button1_Click(object sender, EventArgs e)
        {
            if (DateTime.Now > Convert.ToDateTime("2021-01-14 21:00:00"))
            {
                return;
            }

            for (int i = 0; i < 50; i++)
            {
               
                    Thread thread = new Thread(run);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                
            }
        }

        private void 网站筛选_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
