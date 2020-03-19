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


namespace 启动程序
{
    public partial class 奇趣分分彩 : Form
    {
        public 奇趣分分彩()
        {
            InitializeComponent();
        }

        


        

        #region GET请求带COOKIE
        /// <summary>
        /// GET请求带COOKIE
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlWithCookie(string Url, string COOKIE)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                
                request.Headers.Add("Cookie", COOKIE);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
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

        public static string cookie="";


        

        private void 奇趣分分彩_Load(object sender, EventArgs e)
        {
           
        }

      
       
        private void Button1_Click(object sender, EventArgs e)
        {

            helper.Form1 fm1 = new helper.Form1();
            fm1.Show();
        }

        public void run()

        {
            listView1.Items.Clear();
            string url = "https://yx-668.com/APIV2/GraphQL?l=zh-cn";
            string postdata1 = "{\"operationName\":\"GetLotteryCycle\",\"variables\":{\"game_id\":190,\"row_count\":50},\"query\":\"query GetLotteryCycle($game_id: Int!, $row_count: Int) {\n LotteryGame(game_id: $game_id) {\n game_value\n game_id\n base_game\n start_number\n end_number\n number_count\n lottery_result_history(row_count: $row_count) {\n cycle_value\n game_result\n __typename\n    }\n trend_chart(row_count: $row_count) {\n titles\n __typename\n    }\n __typename\n  }\n}\n\"}";
            string postdata2 = "{\"operationName\":\"GetLotteryCycle\",\"variables\":{\"game_id\":237,\"row_count\":50},\"query\":\"query GetLotteryCycle($game_id: Int!, $row_count: Int) {\n LotteryGame(game_id: $game_id) {\n game_value\n game_id\n base_game\n start_number\n end_number\n number_count\n lottery_result_history(row_count: $row_count) {\n cycle_value\n game_result\n __typename\n    }\n trend_chart(row_count: $row_count) {\n titles\n __typename\n    }\n __typename\n  }\n}\n\"}";
            string postdata3 = "{\"operationName\":\"GetLotteryCycle\",\"variables\":{\"game_id\":191,\"row_count\":50},\"query\":\"query GetLotteryCycle($game_id: Int!, $row_count: Int) {\n LotteryGame(game_id: $game_id) {\n game_value\n game_id\n base_game\n start_number\n end_number\n number_count\n lottery_result_history(row_count: $row_count) {\n cycle_value\n game_result\n __typename\n    }\n trend_chart(row_count: $row_count) {\n titles\n __typename\n    }\n __typename\n  }\n}\n\"}";
            string postdata4 = "{\"operationName\":\"GetLotteryCycle\",\"variables\":{\"game_id\":192,\"row_count\":50},\"query\":\"query GetLotteryCycle($game_id: Int!, $row_count: Int) {\n LotteryGame(game_id: $game_id) {\n game_value\n game_id\n base_game\n start_number\n end_number\n number_count\n lottery_result_history(row_count: $row_count) {\n cycle_value\n game_result\n __typename\n    }\n trend_chart(row_count: $row_count) {\n titles\n __typename\n    }\n __typename\n  }\n}\n\"}";

            string html1 = method.PostUrl(url,postdata1, "","utf-8");
            string html2 = method.PostUrl(url, postdata2, "", "utf-8");
            string html3 = method.PostUrl(url, postdata3, "", "utf-8");
            string html4 = method.PostUrl(url, postdata4, "", "utf-8");


            MatchCollection result1 = Regex.Matches(html1, @"game_result"":\[([\s\S]*?)\]");
            MatchCollection result2 = Regex.Matches(html2, @"game_result"":\[([\s\S]*?)\]");
            MatchCollection result3 = Regex.Matches(html3, @"game_result"":\[([\s\S]*?)\]");
            MatchCollection result4 = Regex.Matches(html4, @"game_result"":\[([\s\S]*?)\]");

            MatchCollection qishu1 = Regex.Matches(html1, @"""cycle_value"":""([\s\S]*?)""");
            MatchCollection qishu2 = Regex.Matches(html2, @"""cycle_value"":""([\s\S]*?)""");
            MatchCollection qishu3 = Regex.Matches(html3, @"""cycle_value"":""([\s\S]*?)""");
            MatchCollection qishu4 = Regex.Matches(html4, @"""cycle_value"":""([\s\S]*?)""");


            for (int i = 0; i < result1.Count; i++)
            {
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count+1).ToString()); //使用Listview展示数据   

                string r1 = result1[i].Groups[1].Value.Trim().Replace("\"","");
                string r2 = result2[i].Groups[1].Value.Trim().Replace("\"", "");
                string r3 = result3[i].Groups[1].Value.Trim().Replace("\"", "");
                string r4 = result4[i].Groups[1].Value.Trim().Replace("\"", "");

                lv1.SubItems.Add(qishu1[i].Groups[1].Value);
                lv1.SubItems.Add(r1);
                lv1.SubItems.Add(qishu2[i].Groups[1].Value);
                lv1.SubItems.Add(r2);
                lv1.SubItems.Add(qishu3[i].Groups[1].Value);
                lv1.SubItems.Add(r3);
                lv1.SubItems.Add(qishu4[i].Groups[1].Value);
                lv1.SubItems.Add(r4);





                //textBox1.ForeColor = Color.Black;
                //textBox2.ForeColor = Color.Black;
                //textBox3.ForeColor = Color.Black;
                //textBox4.ForeColor = Color.Black;
                lv1.BackColor = Color.White;

                //if (r1 == r2)
                //{
                //    lv1.BackColor = Color.Red;

                //}
                //if (r1 == r3)
                //{

                //    lv1.BackColor = Color.Red;
                //}
                //if (r1 == r4)
                //{

                //    lv1.BackColor = Color.Red;
                //}
                //if (r2 == r3)
                //{

                //    lv1.BackColor = Color.Red;
                //}
                //if (r2 == r4)
                //{

                //    lv1.BackColor = Color.Red;
                //}
                //if (r3 == r4)
                //{

                //    lv1.BackColor = Color.Red;
                //}

            }



        }

        private void Button6_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"qqtxffc"))
            {
                MessageBox.Show("监控已开启");
                cookie = helper.Form1.cookie;


                //   timer1.Start();
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;


            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion

           
        }

      

        private void Timer1_Tick(object sender, EventArgs e)
        {
            
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("监控已关闭");
            timer1.Stop();
        }
    }
}
