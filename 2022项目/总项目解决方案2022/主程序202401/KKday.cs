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
using myDLL;

namespace 主程序202401
{
    public partial class KKday : Form
    {
        public KKday()
        {
            InitializeComponent();
        }

        public string COOKIE = "KKUD=SCMWEBc08870d4df1e9ef962335e2061647632; lang_ui=zh-hk; _ga=GA1.3.128564545.1710585220; _ga=GA1.2.128564545.1710585220; _gid=GA1.3.1922370155.1711804319; XSRF-TOKEN=eyJpdiI6Ik5JOVJTdW80b0FpdDQyUTVTUCs2NUE9PSIsInZhbHVlIjoidXdYZG5zKzVwaktxb3UxSnRHalZUWm5EcHo3akpiMDhtTjByYjJLQU90ZGtKV1N3SVdFeUV3V1BHTUp4OXY4VHRHeTFQYzhocWJQRXV2R05XalFhemJ3ZlpYZDNmMFdiWTkvekVMdnZYR0w4dXkyamRHcVVxZ0xHQktmTW5NRy8iLCJtYWMiOiJlMWFmZWI2NTMyNWY3NTgxMzFmNzhlMTgxOGM5MGY1ODVmYTBhYWRhOTBkMTQyNzFlYzhlOGQzZGIzYzBjMTY0IiwidGFnIjoiIn0%3D; kkday_supplier_web_session=eyJpdiI6IlhPaTQ0YXY4RkN2Ujg1RjYwU2F0L3c9PSIsInZhbHVlIjoibmdFOUJjcHlYOEFPQkt1Sy9xalcrdVZrR25VTyt3OHRCUXdma0hnVEQxT3o2MzBUZEV1a05PUjRwWmJkZkw2bkFhL29ySWp2b0h4MTQyTEJsWm1oZTBhWjhyOXBVZVAyNWxRQU9wL1h3QStKMGNLSlFSNUJGR1FSRHRIK3doWUMiLCJtYWMiOiJhYTA3MDk3MzJhYTFjYzY5MWM0M2M5MTg4MTgzY2Q4NWJjY2UxNzBiMGVjZGJiNzc4NDk5OWE3NTA5OTRlYTAyIiwidGFnIjoiIn0%3D; _dc_gtm_UA-49763723-4=1; _ga_7M6SQZX2NH=GS1.3.1711890421.7.1.1711890557.60.0.0; mp_c541d5678a900f15e53d17de65b43ec0_mixpanel=%7B%22distinct_id%22%3A%20%2224346ad5-8d03-495e-9835-141eae03e986%22%2C%22%24device_id%22%3A%20%2218e46d30d0ddd5-0264a0ba2488fa-26001b51-384000-18e46d30d0ddd6%22%2C%22DisplayLang%22%3A%20%22zh-hk%22%2C%22Platform%22%3A%20%22www%22%2C%22%24initial_referrer%22%3A%20%22%24direct%22%2C%22%24initial_referring_domain%22%3A%20%22%24direct%22%2C%22__mps%22%3A%20%7B%7D%2C%22__mpso%22%3A%20%7B%7D%2C%22__mpus%22%3A%20%7B%7D%2C%22__mpa%22%3A%20%7B%7D%2C%22__mpu%22%3A%20%7B%7D%2C%22__mpr%22%3A%20%5B%5D%2C%22__mpap%22%3A%20%5B%5D%2C%22%24user_id%22%3A%20%2224346ad5-8d03-495e-9835-141eae03e986%22%2C%22SupplierOid%22%3A%20%227579%22%7D; s_ci_sessions=a%3A4%3A%7Bs%3A10%3A%22session_id%22%3Bs%3A32%3A%22f90e4dc6833046baf1b496f91fb31c72%22%3Bs%3A10%3A%22ip_address%22%3Bs%3A15%3A%22121.226.183.163%22%3Bs%3A10%3A%22user_agent%22%3Bs%3A111%3A%22Mozilla%2F5.0%20%28Windows%20NT%2010.0%3B%20Win64%3B%20x64%29%20AppleWebKit%2F537.36%20%28KHTML%2C%20like%20Gecko%29%20Chrome%2F123.0.0.0%20Safari%2F537.36%22%3Bs%3A13%3A%22last_activity%22%3Bi%3A1711890574%3B%7D77495257a51f280aa261fa55fe4e6f59; _gid=GA1.2.1922370155.1711804319; _gat_UA-49763723-4=1;";
        public string UA= "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/123.0.0.0 Safari/537.36";
        #region GET请求
        public string GetUrl(string Url, string charset)
        {
           
            string result;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Proxy = null;
                request.AllowAutoRedirect = true;
                request.UserAgent = UA;
                //request.Referer = Url;
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("version:TYC-XCX-WX");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                //request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                bool flag = response.Headers["Content-Encoding"] == "gzip";
                string html;
                if (flag)
                {
                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader2 = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset));
                    html = reader2.ReadToEnd();
                    reader2.Close();
                }
                response.Close();
                result = html;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "//cookie.txt", textBox1.Text.Trim(), Encoding.UTF8);
            COOKIE = textBox1.Text.Trim();

            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        Thread thread;
        bool zanting = true;
        bool status = true;
        public void run()
        {
            try
            {

                for (int i = 1; i < 9999; i++)
                {

                    string startdate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                    string enddate = dateTimePicker2.Value.ToString("yyyy-MM-dd");

                    string url = "https://scm.kkday.com/api/v1/zh-hk/report/get_orderok_list?orderMid=&orderStatus=ALL&begCrtDt="+startdate+"&endCrtDt="+enddate+"&begLstGoDt=&endLstGoDt=&begUseDate=&endUseDate=&begCancelDt=&endCancelDt=&currentPage="+i+"&pageSize=20";

                    string html = GetUrl(url, "utf-8");



                    MatchCollection uids = Regex.Matches(html, @"""orderMid"":""([\s\S]*?)""");

                   // MessageBox.Show(uids.Count.ToString());
                    if (uids.Count == 0)
                    {
                        MessageBox.Show("完成");
                        return;
                    }

                    for (int a = 0; a < uids.Count; a++)
                    {
                        try
                        {
                            string aurl = "https://scm.kkday.com/v1/zh-hk/order/index/"+uids[a].Groups[1].Value;
                            string ahtml = GetUrl(aurl, "utf-8");

                            string orderMid = Regex.Match(ahtml, @"""orderMid"":""([\s\S]*?)""").Groups[1].Value;
                            string productNameMaster = Regex.Match(ahtml, @"""productNameMaster"":""([\s\S]*?)""").Groups[1].Value;
                            string userCrtDtGMT = Regex.Match(ahtml, @"""userCrtDtGMT"":""([\s\S]*?)""").Groups[1].Value;
                          
                            string contactFirstname = Regex.Match(ahtml, @"""contactFirstname"":""([\s\S]*?)""").Groups[1].Value;
                            string contactLastname = Regex.Match(ahtml, @"""contactLastname"":""([\s\S]*?)""").Groups[1].Value;

                            string contactTel = Regex.Match(ahtml, @"""contactTel"":""([\s\S]*?)""").Groups[1].Value;
                            string telCountryCd = Regex.Match(ahtml, @"""telCountryCd"":""([\s\S]*?)""").Groups[1].Value;

                            string contactEmail = Regex.Match(ahtml, @"""contactEmail"":""([\s\S]*?)""").Groups[1].Value;
                            string contactCountryCd = Regex.Match(ahtml, @"""contactCountryCd"":""([\s\S]*?)""").Groups[1].Value;
                          
                            string cusFirstname = Regex.Match(ahtml, @"""cusFirstname"":""([\s\S]*?)""").Groups[1].Value;
                            string cusLastname = Regex.Match(ahtml, @"""cusLastname"":""([\s\S]*?)""").Groups[1].Value;
                            string cusBirthday = Regex.Match(ahtml, @"""cusBirthday"":""([\s\S]*?)""").Groups[1].Value;

                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(orderMid);
                            lv1.SubItems.Add(method.Unicode2String(productNameMaster));
                            lv1.SubItems.Add(userCrtDtGMT);
                            lv1.SubItems.Add(method.Unicode2String(contactFirstname) + " "+ method.Unicode2String(contactLastname));
                            lv1.SubItems.Add(telCountryCd+"-"+contactTel);
                            lv1.SubItems.Add(contactEmail);
                            lv1.SubItems.Add(method.Unicode2String(contactCountryCd));
                            lv1.SubItems.Add(cusFirstname+" "+ cusLastname);
                            lv1.SubItems.Add(cusBirthday);
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
                        }
                        catch (Exception)
                        {

                            continue;
                        }
                    }



                    Thread.Sleep(1000);

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {

            MessageBox.Show(myDLL.StupidOcr.OCR("http://222.187.200.202:8001/ValidateCode/ValidateCode?examid=13240226095949&r=2037362032", ""));
            listView1.Items.Clear();
        }

        private void KKday_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }


        public void login()
        {

            string url = "";

        }
        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void KKday_Load(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "//cookie.txt", method.EncodingType.GetTxtType(AppDomain.CurrentDomain.BaseDirectory + "//cookie.txt"));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            textBox1.Text = texts;
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存

        }
    }
}
