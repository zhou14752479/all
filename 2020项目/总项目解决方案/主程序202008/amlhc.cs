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
using helper;

namespace 主程序202008
{
    public partial class amlhc : Form
    {
        public amlhc()
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

                WebHeaderCollection headers = request.Headers;
                

                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", "");

                request.Referer = "";
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


        string[] shengxiaos = { "鼠" ,"牛 " ,"虎" ,"兔" ,"龙" ,"蛇" ,"马" ,"羊" ,"猴" ,"鸡" ,"狗" ,"猪 " };

        ArrayList lists = new ArrayList();



        public void run()
        {



            //try
            //{

            //    string url = "http://wwwamlhc.com/api/HistoryOpenInfo";
            //    string nowdate = DateTime.Now.ToString("yyyy-MM-dd");
            //    string postdata = "{\"lotteryId\":2032,\"issueNum\":\"" + nowdate + "\"}";

            //    string html = PostUrl(url, postdata);


            //    MatchCollection issues = Regex.Matches(html, @"""issue"":""([\s\S]*?)""");
            //    MatchCollection items = Regex.Matches(html, @"""openCode"":""([\s\S]*?)""");
            //    MatchCollection times = Regex.Matches(html, @"""openTime"":""([\s\S]*?)""");

            //    if (items.Count == 0)
            //        return;

            //    for (int j = 0; j < items.Count; j++)
            //    {


            //        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
            //        listViewItem.SubItems.Add(issues[j].Groups[1].Value);
            //        listViewItem.SubItems.Add(items[j].Groups[1].Value);
            //        listViewItem.SubItems.Add(times[j].Groups[1].Value);



            //    }


            //}


            //catch (Exception ex)
            //{

            //    MessageBox.Show(ex.ToString());
            //}

            while (true)
            {
                List<int> values = new List<int>();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 999999999; i++)
                {
                    int a = new Random().Next(0, 12);
                    if (!values.Contains(a))
                    {
                        values.Add(a);
                    }

                    if (values.Count == 6)
                        break;
                }
                values = values.OrderBy(s => s).ToList();

                for (int j = 0; j < values.Count; j++)
                {
                    sb.Append(shengxiaos[values[j]]);

                }

                string jieguo = sb.ToString().Replace(" ", "");
              
                if (!lists.Contains(jieguo))
                {
                    lists.Add(jieguo);

                    ListViewItem lv1 = listView2.Items.Add((listView2.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(jieguo);
                }
            }
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;
        private void amlhc_Load(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader(path+"data.txt", Encoding.GetEncoding("utf-8"));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int i = 0; i < text.Length; i++)
            {

                ListViewItem lv2 = listView2.Items.Add(text[i]); //使用Listview展示数据
               

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread search_thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            search_thread.Start();




        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
        }
    }
}
