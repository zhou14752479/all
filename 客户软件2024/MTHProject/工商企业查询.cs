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

namespace MTHProject
{
    public partial class 工商企业查询 : Form
    {
        public 工商企业查询()
        {
            InitializeComponent();
        }
        #region GET请求
        public  string GetUrl(string Url, string charset)
        {
           
            string result;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Proxy = null;
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = "https://www.aiqicha.com/s?q=%E8%A3%85%E9%A5%B0&t=0&p_type=2&p_tk=7953smCNriu4zn2kDf%2B76IgDAkPtg0%2F0EFkJl149Tec%2F7L3qkwMhPGD9vPLmzzoq0v33CHdc6AKQs0HEbTFZZ0gEZEgAWmcY4LpC4NX0nJ3WjcrkfcU24Pa8cXGBKVJnV2cHxgJMr5rz4tnbdu7m40CdgFjtEgMt6yGp0RcQqoBYUzI%3D&p_timestamp=1729301823&p_sign=06998284408c2b260fc66415f265c52d&p_signature=81fa7a69015b2f7e921d8a05305ee183&__pc2ps_ab=7953smCNriu4zn2kDf%2B76IgDAkPtg0%2F0EFkJl149Tec%2F7L3qkwMhPGD9vPLmzzoq0v33CHdc6AKQs0HEbTFZZ0gEZEgAWmcY4LpC4NX0nJ3WjcrkfcU24Pa8cXGBKVJnV2cHxgJMr5rz4tnbdu7m40CdgFjtEgMt6yGp0RcQqoBYUzI%3D|1729301823|81fa7a69015b2f7e921d8a05305ee183|06998284408c2b260fc66415f265c52d";
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("version:TYC-XCX-WX");
                request.Headers.Add("Cookie", cookie);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;
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

        string cookie = "log_guid=1d0c41d22a4c7efa5c0fc1fee3ae0c98; Hm_lvt_ad52b306e1ae4557f5d3534cce8f8bbf=1729301811; HMACCOUNT=B6526FFC7DC1CF5B; _j47_ka8_=57; ZX_UNIQ_UID=68933fee41aa2509207a12faa75a19c2; login_type=passport; BDUSS=UUtMFI1bVc2UGpaZHNNcklIVW9GVmZhYn4zbzB3Z1RWa0stOS1STlZ4VTNtenBuSVFBQUFBJCQAAAAAAAAAAAEAAACys-e7cTg1MjI2NjAxMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADcOE2c3DhNnT; BDPPN=e772eecabc7f809f088fc5995021e0d2; _t4z_qc8_=xlTM-TogKuTwQGQSrIRn1gIzlfrGMsHygwmd; ab172929960=64e22b236f39675adb46ca4fe76dab6f1729302074891; entry=1401; __jdg_yd=lTM-TogKuTwn0mXPcGT6ZW6nomllCXMeTlKdZ7eM-uvvLbbrU1HfScYuMObdvPdRA; ab172930320=66e22b236f39675adb46ca4fe76dab6f1729304006299; Hm_lpvt_ad52b306e1ae4557f5d3534cce8f8bbf=1729304006; ab_sr=1.0.1_M2RjZWI2YjZlODllMmE5YmQ5ODBjMGU2ZmFlMzdhZDUxY2Q2NDAwNWNmMTdmNjk0YzUzZDViZTFmOTVkMjM2ZjBlZjg5M2ZmYTRjNTNlYWJkMThhYTE5NWNlMjhiYTdjODU0MmYwN2FlMDI3YTlkNzg5YTZiMTdkOGViYTBjYzY5NDY3OTU5YmE3NjdhN2I5MzFhZDFiMTdlODgwNjc2Zg==; _s53_d91_=a54e017c2b1aedff3bc9cd0a953bbd6b45e989f0eb1350000ad775d0a8d52e6ff1eaf079f1c1ba26887413db7abb925a1d66926320a038052e88bbfaa5ea799cdc7d480a48f1b80b65b7b9391619f693128cf5d31fc58c8dfb177694bad8befa54805a54c4221af96539ea7400ff46919b9eeeff0c69c4543ed6495f004caa07776e22882087519107fa1116e3bf286c424882a05fe78ed87522bd2eee6635087e0a51acc73db684f03ba28057348b29a22ce7c6a9dcfa035e0be1906143be43665a03da02361b7bd9cec818ae142a4c919afdd2940fc5cc937fbc154e19d6c0; _y18_s21_=7808eb8e; RT=\"z=1&dm=aiqicha.com&si=bec3496b-9b00-41fc-b48c-19ddae353ab5&ss=m2fhnccw&sl=1b&tt=1n9r&bcn=https%3A%2F%2Ffclog.baidu.com%2Flog%2Fweirwood%3Ftype%3Dperf&ld=1bms5&nu=9y8m6cy&cl=1bz59\"";
        public void run()
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
                for (int p= 1; p < 999; p++)
                {
                    try
                    {


                        string url = "https://www.aiqicha.com/s/advanceFilterAjax?q=%E8%A3%85%E9%A5%B0&t=&p="+p+"&s=20&o=0&f=%7B%22regCap%22:[%7B%22start%22:10,%22end%22:50%7D],%22entType%22:[%221%22],%22provinceCode%22:[%22320100%22],%22startYear%22:[%7B%22start%22:%222022%22,%22end%22:%222023%22%7D]%7D";
                       string html= GetUrl(url, "utf-8");
                        // textBox1.Text= html;    
                        html = method.Unicode2String(html);
                        MatchCollection names = Regex.Matches(html, @"""titleName"":""([\s\S]*?)""");
                        MatchCollection legalPerson = Regex.Matches(html, @"""titleLegal"":""([\s\S]*?)""");
                        MatchCollection regCap = Regex.Matches(html, @"""regCap"":""([\s\S]*?)""");
                        MatchCollection StartDate = Regex.Matches(html, @"""validityFrom"":""([\s\S]*?)""");
                        MatchCollection Address = Regex.Matches(html, @"""titleDomicile"":""([\s\S]*?)""");
                        MatchCollection businessScope = Regex.Matches(html, @"""scope"":""([\s\S]*?)""");
                        MatchCollection tel = Regex.Matches(html, @"""telephone"":""([\s\S]*?)""");
                        if (names.Count == 0)
                        {
                            Thread.Sleep(1000);
                            continue;
                        }
                     
                        for (int j = 0; j < names.Count; j++)
                        {
                            try
                            {

                                // textBox1.Text = DateTime.Now.ToLongTimeString() + "正在提取：" + names[j].Groups[2].Value.Replace("<em>", "").Replace("</em>", "");

                                MessageBox.Show(names[j].Groups[1].Value);
                                DataTable dt = (DataTable)dataGridView1.DataSource;

                                // 创建新行并添加数据
                                DataRow newRow = dt.NewRow();
                                newRow[0]= names[j].Groups[1].Value; 
                                newRow[1] = 3; // Column2

                                // 将新行添加到DataTable
                                dt.Rows.Add(newRow);

                                // 更新DataGridView显示
                                dataGridView1.Refresh();

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

                thread = new Thread(run);
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
