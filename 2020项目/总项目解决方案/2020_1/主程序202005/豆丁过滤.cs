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

namespace 主程序202005
{
    public partial class 豆丁过滤 : Form
    {
        public 豆丁过滤()
        {
            InitializeComponent();
        }

        private void 豆丁过滤_Load(object sender, EventArgs e)
        {
            webBrowser1.Url = new Uri("https://www.docin.com/app/login?forward=forward:/my/upload/myUpload.do");
            webBrowser1.ScriptErrorsSuppressed = true;
            method.SetWebBrowserFeatures(method.IeVersion.IE9);
        }
        static string COOKIE = "mobilefirsttip=tip; docin_session_id=9a39fcdd-027f-47b4-a405-b6322043f339; cookie_id=C8EAE1E92040000161E014101D70138B; time_id=202052414435; _ga=GA1.2.836054535.1590302586; _gid=GA1.2.1617250182.1590302586; login_email=2938161de5; user_password=HIhjRNxVcJsUBdYW14m4%2F9lAPb4M61xpH_TyincObWtmEdzTDVQn3ceECNQGNYyFo4u0%2FQs7UMrFSDmdidmHSufoA%3D%3D; today_first_in=1; refererfunction=https%3A%2F%2Fwww.baidu.com%2Flink%3Furl%3DnyT0UyLb4hJNJq3yXiWiU32ob9SFMT6BJKEfOjbhL4q%26wd%3D%26eqid%3Defb106f500685da1000000045ecb17cd; userchoose=usertags172_999_171_170_169_102_104_165_129_105_000_; JSESSIONID=B2FFFBE4185D395154A5AF98DB180F60-n2; J_welcome=1; page_length=25; netfunction=\" / my / upload / myUpload.do\"";
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

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
             
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

        public bool panduan(string title)

        {

            string[] text = textBox8.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != "")
                {
                    if (title.Contains(text[i]))
                    {
                        return true;
                    }
                }

            }
            return false;

        }
        bool zanting = true;

        public void getdocs()
        {

            try
            {
                for (int i = Convert.ToInt32(textBox1.Text.Trim()); i <= Convert.ToInt32(textBox2.Text.Trim()); i++)
                {
                    textBox3.Text += "正在筛选第" + i + "页" + "\r\n";
                    string url = "https://www.docin.com/my/upload/myUpload.do?onlypPublic=1&currentPage=" + i;

                    string html = GetUrl(url);

                    MatchCollection ids = Regex.Matches(html, @"<label for=""product([\s\S]*?)""");
                   
                    MatchCollection titles = Regex.Matches(html, @"class=""font14sl"">([\s\S]*?)</a>");

                    for (int j = 0; j < ids.Count; j++)
                    {
                        if (radioButton1.Checked == true)
                        {

                 
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add(ids[j].Groups[1].Value);

                                lv1.SubItems.Add(titles[j].Groups[1].Value);

                            
                        }
                        else
                        {
                            if (panduan(titles[j].Groups[1].Value))
                            {
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add(ids[j].Groups[1].Value);

                                lv1.SubItems.Add(titles[j].Groups[1].Value);

                            }
                        }

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                       
                    }

                    Thread.Sleep(1000);
                }

                MessageBox.Show("筛选结束");

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        public bool deletedocs(string id)
        {

            try
            {
                string url = "https://www.docin.com/app/my/msg/appdeleteById?type=1&productId="+id+"&delproductDesc="+ System.Web.HttpUtility.UrlEncode(textBox5.Text.Trim())+ "&delReasonType=5&delReason=%E5%85%B6%E4%BB%96&date=0.3876018777571839";

                string html = GetUrl(url);
                
                if (html!="")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {

                return false;
            }
        }
        public void del()
        {
            textBox3.Text = "";
            for (int i = 0; i < listView1.CheckedItems.Count; i++)
            {
                string id = listView1.CheckedItems[i].SubItems[1].Text;
              

                textBox3.Text += id + "：删除状态" + deletedocs(id) + "\r\n";
                Thread.Sleep(Convert.ToInt32(textBox4.Text) * 1000);
            }
            MessageBox.Show("删除结束");
        }
        private void button7_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                StreamReader streamReader = new StreamReader(this.openFileDialog1.FileName, Encoding.Default);
                string text = streamReader.ReadToEnd();
                string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    textBox8.Text += array[i] + "\r\n";

                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
            COOKIE = method.GetCookies("https://www.docin.com/my/upload/myUpload.do?onlypPublic=1&currentPage=10");
            Thread thread = new Thread(new ThreadStart(getdocs));
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
            listView1.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(del));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false; 
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = true;
            }
        }

        private void 取消全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = false;
            }
        }
    }
}
