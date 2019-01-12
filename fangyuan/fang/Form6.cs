using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

namespace fang
{
    public partial class Form6 : Form
    {

        bool status = true;



        public class JsonParser
        {
            public List<Rangfen> rangfen;
            public List<Daxiao> daxiao;
        }
        public class Rangfen
        {
            public string handicap;
            public string time_slot;
        }

        public class Daxiao
        {
            public string handicap;
            public string time_slot;
        }

        public string average(string[] lists)

        {
            decimal total = 0;

            for(int i=0;i<lists.Length;i++)
            {
                total = Convert.ToDecimal(lists[i]) + total;

            }

            return (total / lists.Length).ToString("0.00");

        }



        public Form6()
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
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;

                string COOKIE = "ds_session=of872m7n6fr0fnhe56ku4v6g22; uid=R-352871-8c9d3c9405ba07e9868b59; Hm_lvt_aab38eac43f4647b44ea5c594590abf8=1537244825,1537244869,1537245510,1537245949; Hm_lpvt_aab38eac43f4647b44ea5c594590abf8=1537245949";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.81 Safari/537.36";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);

                request.KeepAlive = false;

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string content = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
            return "";
        }
        #endregion

        private void Form6_Load(object sender, EventArgs e)
        {

        }

        public string[] ReadText()
        {

            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
            StreamReader sr = new StreamReader(textBox1.Text);
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            return text;


        }

        /// <summary>
        /// 时间戳转时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public string totime(long timeStamp)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            DateTime dt = startTime.AddSeconds(timeStamp);
            return( dt.ToString("yyyy/MM/dd HH:mm:ss"));
        }

        /// <summary>
        /// unicode转字符串
        /// </summary>
        /// <param name="unicode"></param>
        /// <returns></returns>
        private string UnicodeToStr(string str)
        {
            MatchCollection mc = Regex.Matches(str, "([\\w]+)|(\\\\u([\\w]{4}))");
            if (mc != null && mc.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Match m2 in mc)
                {
                    string v = m2.Value;
                    if (v.StartsWith("\\"))
                    {
                        string word = v.Substring(2);
                        byte[] codes = new byte[2];
                        int code = Convert.ToInt32(word.Substring(0, 2), 16);
                        int code2 = Convert.ToInt32(word.Substring(2), 16);
                        codes[0] = (byte)code2;
                        codes[1] = (byte)code;
                        sb.Append(Encoding.Unicode.GetString(codes));
                    }
                    else
                    {
                        sb.Append(v);
                    }
                }
                return sb.ToString();
            }
            else
            {
                return str;
            }


        }

        #region  体育数据抓取处理
        public void run()
        {

            try
            {

                string[] Urls = ReadText();


                foreach (string Url in Urls)
                {

                    String url = Url.Replace("race_sp", "ajax/race_data");


                    string html = GetUrl(url);

                MatchCollection duiwus = Regex.Matches(html, @"""name"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                Match time = Regex.Match(html, @"race_time"":""([\s\S]*?)""");
                MatchCollection zuos = Regex.Matches(html, @"host_q([\s\S]*?)"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                MatchCollection yous = Regex.Matches(html, @"guest_q([\s\S]*?)"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                Match zuo5 = Regex.Match(html, @"host_ot"":""([\s\S]*?)""");
                Match you5 = Regex.Match(html, @"guest_ot"":""([\s\S]*?)""");

                Match yachu = Regex.Match(html, @"rangfen_handicap"":""([\s\S]*?)""");
                Match daxiaochu = Regex.Match(html, @"daxiao_handicap"":""([\s\S]*?)""");

                Match json = Regex.Match(html, @"""sp"":([\s\S]*?),""chart_data");

                string jsons = json.Groups[1].Value;

                string z5 = zuo5.Groups[1].Value == "" ? "0" : zuo5.Groups[1].Value;  //三元运算符如果为空就为0，否则就为他的值
                string y5 = you5.Groups[1].Value == "" ? "0" : you5.Groups[1].Value;  //三元运算符如果为空就为0，否则就为他的值


                string[] htmls = html.Split(new string[] { "}],\"daxiao" }, StringSplitOptions.None);
                
               

                if (duiwus.Count > 2 && zuos.Count > 3)
                {
                   

                    JsonParser jsonParser = JsonConvert.DeserializeObject<JsonParser>(jsons);

                    //4
                    StringBuilder sb4y= new StringBuilder();
                    foreach (Rangfen rangfen in jsonParser.rangfen)
                    {
                        if (Convert.ToInt32(rangfen.time_slot) == 4)
                        {
                            sb4y.Append(rangfen.handicap + ",");
                        }
                       
                    }
                    string[] values4y = sb4y.ToString().Split(',');
                    values4y=values4y.Where(s => !string.IsNullOrEmpty(s)).ToArray();

                    StringBuilder sb4z = new StringBuilder();
                    foreach (Daxiao daxiao in jsonParser.daxiao)
                    {
                        if (Convert.ToInt32(daxiao.time_slot) == 4)
                        {
                            sb4z.Append(daxiao.handicap + ",");
                        }

                    }
                    string[] values4z = sb4z.ToString().Split(',');
                    values4z = values4z.Where(s => !string.IsNullOrEmpty(s)).ToArray();

                      
                    // lv1.SubItems.Add(values4y[0] + "|" + values4y[values4y.Length - 1] + "|" + average(values4y) + "|" + values4z[0] + "|" + values4z[values4z.Length - 1] + "|" + average(values4z));

                        

                    //3


                    StringBuilder sb3y = new StringBuilder();
                    foreach (Rangfen rangfen in jsonParser.rangfen)
                    {
                        if (Convert.ToInt32(rangfen.time_slot) == 3)
                        {
                            sb3y.Append(rangfen.handicap + ",");
                        }

                    }
                    string[] values3y = sb3y.ToString().Split(',');
                    values3y = values3y.Where(s => !string.IsNullOrEmpty(s)).ToArray();

                    StringBuilder sb3z = new StringBuilder();
                    foreach (Daxiao daxiao in jsonParser.daxiao)
                    {
                        if (Convert.ToInt32(daxiao.time_slot) == 3)
                        {
                            sb3z.Append(daxiao.handicap + ",");
                        }

                    }
                    string[] values3z = sb3z.ToString().Split(',');
                    values3z = values3z.Where(s => !string.IsNullOrEmpty(s)).ToArray();


                    //lv1.SubItems.Add(values3y[0] + "|" + values3y[values3y.Length - 1] + "|" + average(values3y) + "|" + values3z[0] + "|" + values3z[values3z.Length - 1] + "|" + average(values3z));



                    //2
                    StringBuilder sb2y = new StringBuilder();
                    foreach (Rangfen rangfen in jsonParser.rangfen)
                    {
                        if (Convert.ToInt32(rangfen.time_slot) == 2)
                        {
                            sb2y.Append(rangfen.handicap + ",");
                        }

                    }
                    string[] values2y = sb2y.ToString().Split(',');
                    values2y = values2y.Where(s => !string.IsNullOrEmpty(s)).ToArray();

                    StringBuilder sb2z = new StringBuilder();
                    foreach (Daxiao daxiao in jsonParser.daxiao)
                    {
                        if (Convert.ToInt32(daxiao.time_slot) == 2)
                        {
                            sb2z.Append(daxiao.handicap + ",");
                        }

                    }
                    string[] values2z = sb2z.ToString().Split(',');
                    values2z = values2z.Where(s => !string.IsNullOrEmpty(s)).ToArray();


                  //  lv1.SubItems.Add(values2y[0] + "|" + values2y[values2y.Length - 1] + "|" + average(values2y) + "|" + values2z[0] + "|" + values2z[values2z.Length - 1] + "|" + average(values2z));


                    //1
                    StringBuilder sb1y = new StringBuilder();
                    foreach (Rangfen rangfen in jsonParser.rangfen)
                    {
                        if (Convert.ToInt32(rangfen.time_slot) == 1)
                        {
                            sb1y.Append(rangfen.handicap + ",");
                        }

                    }
                    string[] values1y = sb1y.ToString().Split(',');
                    values1y = values1y.Where(s => !string.IsNullOrEmpty(s)).ToArray();

                    StringBuilder sb1z = new StringBuilder();
                    foreach (Daxiao daxiao in jsonParser.daxiao)
                    {
                        if (Convert.ToInt32(daxiao.time_slot) == 1)
                        {
                            sb1z.Append(daxiao.handicap + ",");
                        }

                    }
                    string[] values1z = sb1z.ToString().Split(',');
                    values1z = values1z.Where(s => !string.IsNullOrEmpty(s)).ToArray();


                   // lv1.SubItems.Add(values1y[0] + "|" + values1y[values1y.Length - 1] + "|" + average(values1y) + "|" + values1z[0] + "|" + values1z[values1z.Length - 1] + "|" + average(values1z));



                        if (values1y.Length > 0 && values1z.Length > 0&&values2y.Length > 0 && values2z.Length > 0&&values3y.Length > 0 && values3z.Length > 0 && values4y.Length > 0 && values4z.Length > 0)

                        {
                            ListViewItem lv1 = listView1.Items.Add(UnicodeToStr(duiwus[0].Groups[1].Value.Trim())); //使用Listview展示数据
                            lv1.SubItems.Add(totime(Convert.ToInt64(time.Groups[1].Value.Trim()) - 28800));
                            lv1.SubItems.Add(UnicodeToStr(duiwus[1].Groups[1].Value.ToString()));
                            lv1.SubItems.Add(UnicodeToStr(duiwus[2].Groups[1].Value.ToString()));
                            lv1.SubItems.Add(zuos[0].Groups[2].Value.ToString() + "," + zuos[1].Groups[2].Value.ToString() + "," + zuos[2].Groups[2].Value.ToString() + "," + zuos[3].Groups[2].Value.ToString() + "," + z5);
                            lv1.SubItems.Add(yous[0].Groups[2].Value.ToString() + "," + yous[1].Groups[2].Value.ToString() + "," + yous[2].Groups[2].Value.ToString() + "," + yous[3].Groups[2].Value.ToString() + "," + y5);
                            lv1.SubItems.Add(yachu.Groups[1].Value.ToString());
                            lv1.SubItems.Add(daxiaochu.Groups[1].Value.ToString());


                            lv1.SubItems.Add(values1y[0] + "|" + values1y[values1y.Length - 1] + "|" + average(values1y) + "|" + values1z[0] + "|" + values1z[values1z.Length - 1] + "|" + average(values1z));
                            lv1.SubItems.Add(values2y[0] + "|" + values2y[values2y.Length - 1] + "|" + average(values2y) + "|" + values2z[0] + "|" + values2z[values2z.Length - 1] + "|" + average(values2z));
                      
                            lv1.SubItems.Add(values3y[0] + "|" + values3y[values3y.Length - 1] + "|" + average(values3y) + "|" + values3z[0] + "|" + values3z[values3z.Length - 1] + "|" + average(values3z));
                            lv1.SubItems.Add(values4y[0] + "|" + values4y[values4y.Length - 1] + "|" + average(values4y) + "|" + values4z[0] + "|" + values4z[values4z.Length - 1] + "|" + average(values4z));

                        }
                        if (status == false)
                            return;
                        if (listView1.Items.Count > 1)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }





                    }
                






     

                Application.DoEvents();
                System.Threading.Thread.Sleep(Convert.ToInt32(3000));   //内容获取间隔，可变量                     

                }


            }





            catch (System.Exception ex)
            {

                MessageBox.Show( ex.ToString());
            }

        }

        #endregion

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请选择文件");
                return;
            }

            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.status = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
