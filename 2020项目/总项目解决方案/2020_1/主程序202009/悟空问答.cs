using System;
using System.Collections;
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
using NPOI.HSSF.UserModel;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace 主程序202009
{
    public partial class 悟空问答 : Form
    {
        public 悟空问答()
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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://www.wukong.com/search/?keyword=%E9%BB%91%E5%B8%BDSEO";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

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

        bool zanting = true;
        public void run()
        {

            int page = Convert.ToInt32(textBox2.Text)*10;

            StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.GetEncoding("utf-8"));
            string text = streamReader.ReadToEnd();
            string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int a = 0; a < array.Length; a++)
            {
                string keyword = array[a].ToString();
                
                for (int i = 0; i <=page; i=i+10)
            {

                string url = "https://www.wukong.com/wenda/wapshare/search/loadmore/?search_text="+ System.Web.HttpUtility.UrlEncode(keyword) +"&offset="+i;
                string html = GetUrl(url) ;

                    MatchCollection ids = Regex.Matches(html, @"""qid"": ""([\s\S]*?)""");
                    MatchCollection titles = Regex.Matches(html, @"""title"": ""([\s\S]*?)""");
                   
                 

                    for (int j = 0; j < ids.Count; j++)
                    {
                         string aurl = "https://www.wukong.com/wenda/web/question/loadmorev1/?qid="+ids[j].Groups[1].Value+"&count="+ Convert.ToInt32(textBox3.Text) + "&req_type=1&offset=0";
                        string ahtml = GetUrl(aurl);
                        MatchCollection answers = Regex.Matches(ahtml, @"""abstract_text"": ""([\s\S]*?)content"": ""([\s\S]*?)"",");

                        for (int z = 0; z < answers.Count; z++)
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                            lv1.SubItems.Add(ids[j].Groups[1].Value);
                            lv1.SubItems.Add(keyword);
                            lv1.SubItems.Add(Unicode2String(titles[j].Groups[1].Value));
                            lv1.SubItems.Add("https://www.wukong.com/question/" + ids[j].Groups[1].Value + "/");

                            string aser = answers[z].Groups[2].Value.Replace("<br/>", "").Replace("<span style=\\\"font-weight: bold;\\\">", "").Replace("<blockquote>", "").Replace("</blockquote>", "").Replace("</span>", "");
                            lv1.SubItems.Add(Unicode2String(aser).Replace("\\", ""));
                            

                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                        }
                        Thread.Sleep(1000);
                    }
                   



                }
            }

        }


        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }

        private void 悟空问答_Load(object sender, EventArgs e)
        {

        }

        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"wukongwenda"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
            }
        }
        string path = AppDomain.CurrentDomain.BaseDirectory + "\\data\\";
       
        private void button4_Click(object sender, EventArgs e)
        {
           
           
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }
    }
}
