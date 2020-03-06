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

namespace 通用项目
{
    public partial class 抓数据用 : Form
    {
        public 抓数据用()
        {
            InitializeComponent();
        }

        bool zanting = true;
        public static string COOKIE = "";

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

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Post";
            request.ContentType = "application/json";
            request.ContentLength = postData.Length;

            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.112 Safari/537.36";
            request.Headers.Add("Cookie", COOKIE);

            StreamWriter sw = new StreamWriter(request.GetRequestStream());
            sw.Write(postData);
            sw.Flush();


            WebResponse response = request.GetResponse();
            Stream s = response.GetResponseStream();
            StreamReader sr = new StreamReader(s, Encoding.GetEncoding("utf-8"));
            string html = sr.ReadToEnd();

            sw.Dispose();
            sw.Close();
            sr.Dispose();
            sr.Close();
            s.Dispose();
            s.Close();
            return html;
        }
        
        private DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long mTime = long.Parse(timeStamp+"0000" );
            TimeSpan toNow = new TimeSpan(mTime);
            return startTime.Add(toNow);

        }

        #endregion

     

        public void qichacha()
        {
            StreamReader streamReader = new StreamReader(this.textBox1.Text,Encoding.GetEncoding("utf-8"));
            string text = streamReader.ReadToEnd();
            string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int i = 0; i < array.Length; i++)
            {
                try
                {
                    string url = "https://xcx.qichacha.com/wxa/v1/base/advancedSearchNew?searchKey=" + System.Web.HttpUtility.UrlEncode(array[i].Trim())+"&searchIndex=&sortField=&isSortAsc=false&province=&cityCode=&countyCode=&industryCode=&subIndustryCode=&industryV3=&token=" + textBox2.Text + "&startDateBegin=&startDateEnd=&registCapiBegin=&registCapiEnd=&insuredCntStart=&insuredCntEnd=&coyType=&statusCode=&hasPhone=&hasMobilePhone=&hasEmail=&hasTM=&hasPatent=&hasSC=&hasShiXin=&hasFinance=&hasIPO=&pageIndex=1&searchType=0";

                    string html = method.GetUrl(url, "utf-8");

                    if (html.Contains("已失效"))
                    {
                        ListViewItem lv1 = listView2.Items.Add((listView2.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(array[i]);
                        lv1.SubItems.Add("失效");
                        MessageBox.Show("已失效");
                    }

                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        Match tellit = Regex.Match(html, @"TelList"":""\[([\s\S]*?)\]");
                        MatchCollection tel = Regex.Matches(tellit.Groups[1].Value, @"t\\"":\\""([\s\S]*?)\\");

                        foreach (Match t in tel)
                        {
                            sb.Append(t.Groups[1].Value + "#");
                        }

                        ListViewItem lv1 = listView2.Items.Add((listView2.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(array[i]);
                        lv1.SubItems.Add(sb.ToString());
                    }

                    while (zanting == false)
                    {
                        Application.DoEvents();//等待本次加载完毕才执行下次循环.
                    }

                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    // continue;
                }
               
            }

        }


        
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
           
                    for (int j = 0; j < 247; j++)
                    {
                        string url = "http://gs.amac.org.cn/amac-infodisc/api/pof/manager?rand=0.3952054052459051&page=" + j + "&size=100";

                        string html = PostUrl(url, "{}");
                        MatchCollection a1 = Regex.Matches(html, @"""officeProvince"":""([\s\S]*?)""");
                        MatchCollection a2 = Regex.Matches(html, @"""managerName"":""([\s\S]*?)""");
                        MatchCollection a3 = Regex.Matches(html, @"""artificialPersonName"":""([\s\S]*?)""");
                        MatchCollection a4 = Regex.Matches(html, @"""registerDate"":([\s\S]*?),");
                        MatchCollection a5 = Regex.Matches(html, @"""establishDate"":([\s\S]*?),");
                        MatchCollection a6 = Regex.Matches(html, @"""primaryInvestType"":""([\s\S]*?)""");

                        MatchCollection ids = Regex.Matches(html, @"""url"":""([\s\S]*?)""");

                        for (int a = 0; a < ids.Count; a++)

                        {

                    try
                    {
                        string strhtml = method.GetUrl("http://gs.amac.org.cn/amac-infodisc/res/pof/manager/" + ids[a].Groups[1].Value, "utf-8");
                        Match zhuce = Regex.Match(strhtml, @"注册资本\(万元\)\(人民币\)</td>([\s\S]*?)</td>");
                        Match bili = Regex.Match(strhtml, @"注册资本实缴比例</td>([\s\S]*?)</td>");

                        //FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "新文档.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                        //StreamWriter sw = new StreamWriter(fs1);
                        //string value = a1[a].Groups[1].Value + "#" + a2[a].Groups[1].Value + "#" + a3[a].Groups[1].Value + "#" + ConvertStringToDateTime(a4[a].Groups[1].Value).ToString() + "#" + ConvertStringToDateTime(a5[a].Groups[1].Value).ToString() + "#" + zhuce.Groups[1].Value.Replace("<td colspan=\"2\">", "").Trim() + "#" + bili.Groups[1].Value.Replace("<td colspan=\"2\">", "").Trim() +"#"+ a6[a].Groups[1].Value;
                        //sw.WriteLine(value);
                        //sw.Close();
                        //fs1.Close();

                        
                        //label1.Text = zhi.ToString();
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(a1[a].Groups[1].Value);
                        lv1.SubItems.Add(a2[a].Groups[1].Value);
                        lv1.SubItems.Add(a3[a].Groups[1].Value);
                        lv1.SubItems.Add(ConvertStringToDateTime(a4[a].Groups[1].Value).ToString());
                        lv1.SubItems.Add(ConvertStringToDateTime(a5[a].Groups[1].Value).ToString());
                        lv1.SubItems.Add(zhuce.Groups[1].Value.Replace("<td colspan=\"2\">", "").Trim());
                        lv1.SubItems.Add(bili.Groups[1].Value.Replace("<td colspan=\"2\">", "").Trim());
                        lv1.SubItems.Add(a6[a].Groups[1].Value);
                    }
                    catch 
                    {

                        continue;
                    }
                            

                            

                        }

                        
                     }
                }
            
            
           
        
        private void button1_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Thread thread = new Thread(new ThreadStart(run));
            //thread.Start();
            //Control.CheckForIllegalCrossThreadCalls = false;

            Thread thread = new Thread(new ThreadStart(qichacha));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.expotTxt(listView1);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            method.ListViewToCSV(listView2,true);

        }

        private void button7_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            zanting = true;
        }
    }
}
