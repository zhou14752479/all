using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MTHProject
{
    public partial class 工商企业查询 : Form
    {
        public 工商企业查询()
        {
            InitializeComponent();
        }
        #region POST默认请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public string PostUrlDefault(string url, string postData, string COOKIE)
        {
            try
            {

                string charset = "utf-8";
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                //   request.Proxy = null;//防止代理抓包
               


                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("version:TYC-XCX-WX");
              
                //request.ContentType = "application/x-www-form-urlencoded";
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                // request.ContentLength = postData.Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                // request.Proxy = null;//禁止抓包
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 17_5_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/8.0.50(0x1800323b) NetType/WIFI Language/zh_CN";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = url;
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");

                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }


                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                return ("企业大数据：异常");
            }


        }

        #endregion

        /// <summary>
        /// 时间转换 毫秒级别的时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string GetChinaTicks(DateTime dateTime)
        {
            //北京时间相差8小时
            DateTime startTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 8, 0, 0, 0), TimeZoneInfo.Local);
            long t = (dateTime.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位   
            return t.ToString();
        }

        /// <summary>
        /// 获取时间戳毫秒
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalMilliseconds);
            return a.ToString();
        }
        /// <summary>
        /// 时间转换 毫秒级别的时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
      
        public void tyc()
        {
            int count = 0;
            status = true;


            StringBuilder sb = new StringBuilder();

            if (comboBox1.Text != "不限")
            {
                sb.Append(",\"orgType\":\"" + comboBox1.Text + "\"");
            }


            if (comboBox2.Text != "不限")
            {
                switch (comboBox2.Text)
                {
                    case "0-100":
                        sb.Append(",\"moneyStart\":0,\"moneyEnd\":100");
                        break;
                    case "100-200":
                        sb.Append(",\"moneyStart\":100,\"moneyEnd\":200");
                        break;
                    case "200-500":
                        sb.Append(",\"moneyStart\":200,\"moneyEnd\":500");
                        break;
                    case "500-1000":
                        sb.Append(",\"moneyStart\":500,\"moneyEnd\":1000");
                        break;
                    case "1000-":
                        sb.Append(",\"moneyStart\":1000,\"moneyEnd\":null");
                        break;
                }
            }


            if (comboBox4.Text != "不限")
            {
                switch (comboBox4.Text)
                {
                    case "0-1年":
                        sb.Append(",\"estiblishTimeStart\":" + GetChinaTicks(DateTime.Now.AddDays(-365)) + ",\"estiblishTimeEnd\":" + GetTimeStamp() + "");
                        break;
                    case "1-2年":
                        sb.Append(",\"estiblishTimeStart\":" + GetChinaTicks(DateTime.Now.AddDays(-730)) + ",\"estiblishTimeEnd\":" + GetChinaTicks(DateTime.Now.AddDays(-365)) + "");
                        break;
                    case "2-3年":
                        sb.Append(",\"estiblishTimeStart\":" + GetChinaTicks(DateTime.Now.AddDays(-1095)) + ",\"estiblishTimeEnd\":" + GetChinaTicks(DateTime.Now.AddDays(-730)) + "");
                        break;
                    case "3-5年":
                        sb.Append(",\"estiblishTimeStart\":" + GetChinaTicks(DateTime.Now.AddDays(-1825)) + ",\"estiblishTimeEnd\":" + GetChinaTicks(DateTime.Now.AddDays(-1095)) + "");
                        break;
                    case "5-10年":
                        sb.Append(",\"estiblishTimeStart\":" + GetChinaTicks(DateTime.Now.AddDays(-3650)) + ",\"estiblishTimeEnd\":" + GetChinaTicks(DateTime.Now.AddDays(-1825)) + "");
                        break;
                    case "10年以上":
                        sb.Append(",\"estiblishTimeStart\":-4669997486139,\"estiblishTimeEnd\":" + GetChinaTicks(DateTime.Now.AddDays(-3650)) + "");
                        break;
                }
            }
            string areacode = "";
            ////获取区域code
            //if (comboBox6.Text != "全部")
            //{
            //    if (comboBox7.Text == "全部")
            //    {
            //        areacode = dicp[comboBox6.Text];
            //    }
            //    else if (comboBox8.Text == "全部")
            //    {
            //        areacode = dicc[comboBox7.Text];
            //    }
            //    else
            //    {
            //        areacode = dica[comboBox8.Text];
            //    }
            //    sb.Append(",\"customAreaCode\":\"" + areacode + "\"");
            //}




            try
            {
                for (int i = 1; i < 51; i++)
                {
                    try
                    {


                        string url = "https://capi.tianyancha.com/cloud-tempest/app/searchCompany";
                        string postdata = "{\"sortType\":0,\"pageSize\":100,\"pageNum\":" + i + ",\"word\":\"" + textBox1.Text.Trim() + "\",\"allowModifyQuery\":1" + sb.ToString() + "}";
                        string html2 = PostUrlDefault(url, postdata, "");

                   

                        string html = Regex.Match(html2, @"companyList([\s\S]*?)brandAndAgencyList").Groups[1].Value;
                        MatchCollection names = Regex.Matches(html, @"{""id"":([\s\S]*?),""name"":""([\s\S]*?)""");

                        MatchCollection legalPerson = Regex.Matches(html, @"""legalPersonName"":""([\s\S]*?)""");
                        MatchCollection regCap = Regex.Matches(html, @"""regCapital"":""([\s\S]*?)""");
                        MatchCollection StartDate = Regex.Matches(html, @"""estiblishTime"":""([\s\S]*?)""");
                        MatchCollection Address = Regex.Matches(html, @"""regLocation"":""([\s\S]*?)""");
                        MatchCollection businessScope = Regex.Matches(html, @"""businessScope"":""([\s\S]*?)""");
                        MatchCollection tel = Regex.Matches(html, @"phoneList([\s\S]*?)phoneInfoList");
                        if (names.Count == 0)
                        {
                            Thread.Sleep(1000);
                            continue;
                        }
                        //listView1.Items.Add("");
                        for (int j = 0; j < names.Count; j++)
                        {
                            try
                            {
                                Thread.Sleep(500);
                                // textBox1.Text = DateTime.Now.ToLongTimeString() + "正在提取：" + names[j].Groups[2].Value.Replace("<em>", "").Replace("</em>", "");


                                //ListViewItem lv1 = new ListViewItem((listView1.Items.Count + 1).ToString());
                                //lv1.SubItems.Add(names[j].Groups[2].Value.Replace("<em>", "").Replace("</em>", ""));
                                //lv1.SubItems.Add(legalPerson[j].Groups[1].Value);
                                //lv1.SubItems.Add(regCap[j].Groups[1].Value);
                                //lv1.SubItems.Add(StartDate[j].Groups[1].Value.Replace("00:00:00.0", ""));


                               
                                string telall = tel[j].Groups[1].Value.Replace("[", "").Replace("]", "").Replace("\"", "").Replace(":", "");

                                string tel1 = "";
                                string tel2 = "";

                                //MessageBox.Show(tel[j].Groups[1].Value);
                                string[] text = tel[j].Groups[1].Value.Replace("[", "").Replace("]", "").Replace("\"", "").Replace(":", "").Split(new string[] { "," }, StringSplitOptions.None);
                             


                                if (status == false)
                                    return;
                                count = count + 1;
                                label4.Text = count.ToString();
                               
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("企业大数据：异常");
                              
                                continue;
                            }
                        }


                        Thread.Sleep(1000);

                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("企业大数据：异常" + ex.ToString());
                    }

                }




            }
            catch (Exception ex)
            {

                MessageBox.Show("企业大数据：异常");
            }
        }

        Thread thread;
        bool status = true;
        private void nav_Monitor_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入行业");
                return;
            }

            status = true;

            if (thread == null || !thread.IsAlive)
            {

                thread = new Thread(tyc);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            else
            {
                status = false;
            }
        }
    }
}
