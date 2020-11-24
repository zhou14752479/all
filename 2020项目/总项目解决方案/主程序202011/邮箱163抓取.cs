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

namespace 主程序202011
{
    public partial class 邮箱163抓取 : Form
    {
        public 邮箱163抓取()
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
        public static string PostUrl(string url, string postData, string COOKIE, string charset)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";

                //request.ContentType = "application/json";
                request.ContentLength = postData.Length;
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = "https://mail.163.com/js6/main.jsp?sid=cBZNTotGUjWbvKFHDtGGQKnnrdhvIQXG&df=email163";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

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

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public  string GetUrl(string Url)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
              
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
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


        int total = 0;
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
            if (checkBox7.Checked == false)
            {
                dateTimePicker1.Value = DateTime.Now.AddYears(-99);
                dateTimePicker2.Value = DateTime.Now.AddYears(99);
            }


            for (int i = 0; i < Convert.ToInt32(textBox3.Text); i++)
            {
                int start = i * 30;
                string url = "https://mail.163.com/js6/s?sid="+sid+"&func=mbox:listMessages&mbox_pager_next=1";
                string postdata = "var=%3C%3Fxml%20version%3D%221.0%22%3F%3E%3Cobject%3E%3Cint%20name%3D%22fid%22%3E1%3C%2Fint%3E%3Cstring%20name%3D%22order%22%3Edate%3C%2Fstring%3E%3Cboolean%20name%3D%22desc%22%3Etrue%3C%2Fboolean%3E%3Cint%20name%3D%22limit%22%3E30%3C%2Fint%3E%3Cint%20name%3D%22start%22%3E" + start + "%3C%2Fint%3E%3Cboolean%20name%3D%22skipLockedFolders%22%3Efalse%3C%2Fboolean%3E%3Cstring%20name%3D%22topFlag%22%3Etop%3C%2Fstring%3E%3Cboolean%20name%3D%22returnTag%22%3Etrue%3C%2Fboolean%3E%3Cboolean%20name%3D%22returnTotal%22%3Etrue%3C%2Fboolean%3E%3C%2Fobject%3E";
                string html = PostUrl(url, postdata, COOKIE, "utf-8");
              
                MatchCollection aids = Regex.Matches(html, @"<string name=""id"">([\s\S]*?)</string>");
                MatchCollection titles = Regex.Matches(html, @"<string name=""subject"">([\s\S]*?)</string>");
                MatchCollection times = Regex.Matches(html, @"<date name=""receivedDate"">([\s\S]*?)</date>");
                MatchCollection yidus = Regex.Matches(html, @"<object name=""flags([\s\S]*?)</object>");
                if (aids.Count == 0)
                {
                    break;

                }
                


                for (int j = 0; j < aids.Count; j++)
                {
                    bool yiduweidu = true;

                    if (checkBox3.Checked == false && !yidus[j].Groups[1].Value.Contains("read"))
                    {
                        yiduweidu = false;
                    }
                    if (checkBox8.Checked == false && yidus[j].Groups[1].Value.Contains("read"))
                    {
                        yiduweidu = false;
                    }
                    try
                    {
                        if (yiduweidu)
                        {

                            string aurl = "https://mail.163.com/js6/read/readhtml.jsp?mid="+aids[j].Groups[1].Value+"&userType=ud";
                    string ahtml = GetUrl(aurl);

                    Match name = Regex.Match(ahtml, @"font-weight:normal;"">([\s\S]*?)<");
                    Match age = Regex.Match(ahtml, @">（([\s\S]*?)）");
                    Match tel = Regex.Match(ahtml, @"手机号码：([\s\S]*?)</span>");
                            if (name.Groups[1].Value != "")
                            {
                                
                                    string nianling = age.Groups[1].Value.Replace("女", "").Replace("男", "").Replace("，", "").Replace("岁", "");
                                    if (Convert.ToInt32(nianling) > Convert.ToInt32(textBox1.Text) && Convert.ToInt32(nianling) < Convert.ToInt32(textBox2.Text))
                                    {

                                        if (Convert.ToDateTime(times[j].Groups[1].Value) > dateTimePicker1.Value && Convert.ToDateTime(times[j].Groups[1].Value) < dateTimePicker2.Value)
                                        {
                                            if (comboBox1.Text == "全部性别")
                                            {
                                                total = total + 1;
                                                toolStripStatusLabel2.Text = total.ToString();
                                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                                lv1.SubItems.Add(name.Groups[1].Value);
                                                lv1.SubItems.Add(age.Groups[1].Value);
                                                lv1.SubItems.Add(Regex.Replace(tel.Groups[1].Value, "<[^>]+>", ""));
                                                lv1.SubItems.Add(titles[j].Groups[1].Value);
                                                lv1.SubItems.Add(times[j].Groups[1].Value);
                                            }
                                            else
                                            {
                                                if (age.Groups[1].Value.Contains(comboBox1.Text.Trim()))
                                                {
                                                    total = total + 1;
                                                    toolStripStatusLabel2.Text = total.ToString();
                                                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                                    lv1.SubItems.Add(name.Groups[1].Value);
                                                    lv1.SubItems.Add(age.Groups[1].Value);
                                                    lv1.SubItems.Add(Regex.Replace(tel.Groups[1].Value, "<[^>]+>", ""));
                                                    lv1.SubItems.Add(titles[j].Groups[1].Value);
                                                    lv1.SubItems.Add(times[j].Groups[1].Value);

                                                }
                                                else
                                                {
                                                    toolStripStatusLabel2.Text = "不符合性别要求";
                                                }
                                            }

                                        }
                                        else
                                        {
                                            toolStripStatusLabel2.Text = "不符合时间要求";
                                        }
                                    }
                                    else
                                    {
                                        toolStripStatusLabel2.Text = "不符合年龄要求";
                                    }
                                }
                                Thread.Sleep(1000);
                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                                if (status == false)
                                    return;
                            }

                        else
                        {
                            toolStripStatusLabel2.Text = "不符合已读未读筛选";
                        }
                    }
                    catch (Exception)
                    {

                        continue;
                    }


                }
                
            }
            MessageBox.Show("抓取结束");


        }

        bool zanting = true;
        Thread thread;
        string sid = "";
        string COOKIE = "";
        private void button1_Click(object sender, EventArgs e)
        {

            cookieBrowser cb = new cookieBrowser("https://mail.163.com/js6/s?sid=WCMjZlDUiyNqxrkjdXUUVOJCNlostwDI&func=mbox:listMessages&mbox_pager_next=1");
            cb.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            COOKIE = cookieBrowser.cookie;
            COOKIE = "_ntes_nnid=cd122851463bd744e423f21fad397a63,1590834443609; _ga=GA1.2.1033791233.1590834537; _ntes_nuid=cd122851463bd744e423f21fad397a63; UM_distinctid=173f54f7393232-0c87497a2723a2-6373664-1fa400-173f54f7394bb5; vjlast=1598164493.1598164493.30; vjuids=-1bbd9584b.1741a06e3f5.0.222433b9c83f6; locale=; face=js6; mail_psc_fingerprint=f9980414501707bee1035100250b2caf; vinfo_n_f_l_n3=9248c9d9cccd372d.1.6.1590834443622.1603713715708.1605315103536; gdxidpyhxdE=Vif%2F9lItVYjDH85LZC%5CzqsNbtgRRcTqvsryKy7OHb7lTQYhQbfp4zzMOwlhIbyvvjuSXvwyR2nOz%2F4cC5u3PZ5MUpHqYkP3dKNc02%2Fw6j%2Fkm2%2BHOez%2FQZT9u%2BPjjqYO8xMhOShXaVcUXk2D791MN2Dj4NDZN61upjUv2g2moj1SEolCx%3A1605842537545; _9755xjdesxxd_=32; starttime=; df=mail163_letter; mail_upx_nf=; mail_idc=\"\"; secu_info=1; mail_host=mail.163.com; mail_style=js6; NTES_SESS=HTAo_nnVud8Hld9.kLHvd.Eai1QNZXzkPM4mOLmWquuhqC5iqgFBt6mMg617sdXB83ieSgQGqhH0pu0Y4lPK32mnIvynFrXN3PAq6NM9M.T7CbhkljNjvftIJrWjHPBPyi5OENJvyFTotmOhPSz5OakN_zE4BZunv76Lz_JJcTsfNbZtDLI6KiVTom5BoQYDXVXDgTdXNVjz6; NTES_PASSPORT=LbYompA_wVrwEt9S7VF1iUnyWlyA1QQfEhJPa3VlNnhYNK67NXivTd_PXd.myx5v3q1sDCR2bOMDCxC4kZ2ANKvyNH8L5e4zTiLj9cHi5mGOEnrMtu85dBN2V52BpljeCHzriu2hidmPiU_5hzSV4Lj8K0d5FKtA1qV9aorNjm0UG6tURad9ICYwxX8Htpl8U; S_INFO=1605873606|0|3&80##|aa8898770; P_INFO=aa8898770@163.com|1605873606|1|mail163|00&99|gud&1605873301&mail163#jis&321300#10#0#0|&0|mail163|aa8898770@163.com; nts_mail_user=aa8898770@163.com:-1:1; mail_upx=t2hz.mail.163.com|t3hz.mail.163.com|t4hz.mail.163.com|t7hz.mail.163.com|t8hz.mail.163.com|t1hz.mail.163.com|t1bj.mail.163.com|t2bj.mail.163.com|t3bj.mail.163.com|t4bj.mail.163.com; Coremail=b7ee400a28731%nCJGMryVBfjkPqtTRTVVSUBkQIqejIRi%g1a123.mail.163.com; cm_last_info=dT1hYTg4OTg3NzAlNDAxNjMuY29tJmQ9aHR0cHMlM0ElMkYlMkZtYWlsLjE2My5jb20lMkZqczYlMkZtYWluLmpzcCUzRnNpZCUzRG5DSkdNcnlWQmZqa1BxdFRSVFZWU1VCa1FJcWVqSVJpJnM9bkNKR01yeVZCZmprUHF0VFJUVlZTVUJrUUlxZWpJUmkmaD1odHRwcyUzQSUyRiUyRm1haWwuMTYzLmNvbSUyRmpzNiUyRm1haW4uanNwJTNGc2lkJTNEbkNKR01yeVZCZmprUHF0VFJUVlZTVUJrUUlxZWpJUmkmdz1odHRwcyUzQSUyRiUyRm1haWwuMTYzLmNvbSZsPS0xJnQ9LTEmYXM9dHJ1ZQ==; MAIL_SESS=HTAo_nnVud8Hld9.kLHvd.Eai1QNZXzkPM4mOLmWquuhqC5iqgFBt6mMg617sdXB83ieSgQGqhH0pu0Y4lPK32mnIvynFrXN3PAq6NM9M.T7CbhkljNjvftIJrWjHPBPyi5OENJvyFTotmOhPSz5OakN_zE4BZunv76Lz_JJcTsfNbZtDLI6KiVTom5BoQYDXVXDgTdXNVjz6; MAIL_SINFO=1605873606|0|3&80##|aa8898770; MAIL_PINFO=aa8898770@163.com|1605873606|1|mail163|00&99|gud&1605873301&mail163#jis&321300#10#0#0|&0|mail163|aa8898770@163.com; MAIL_PASSPORT_INFO=aa8898770@163.com|1605873606|1; mail_entry_sess=10d48311a5ca245afe2d58ae75c8a298ba583fbd8f010dc49254f30c05a4ed2c39af8209e466312ab5264f42725a7955eb5873484ee40014fcca016986398912bf42f77cb1a8fdd9efaa7ff6514fd8babbe7f939042d93a5b17ebbc66fd4b540a92e6a57e231ddf799576411f09716c44bc93ae06a420a0b98fba0485094ab9076e69cb4d8f156ad6784d04039a084ea4149c2949d0ca0db2b19584abb5df4f6c6b07c8f689cc1bb191b026bcb3f0d9605fbfe9d629f879f8e73d01141d1c79c; JSESSIONID=1514DE6785FA4E501F03EF843833CA16; mail_uid=aa8898770@163.com; Coremail.sid=nCJGMryVBfjkPqtTRTVVSUBkQIqejIRi;";
            Match name = Regex.Match(COOKIE, @"Coremail\.sid=([\s\S]*?);");
            sid = name.Groups[1].Value;
         
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"163youxiang"))
            {
                MessageBox.Show("");
                return;
            }

            #endregion

            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
        bool status = true;
        private void button5_Click(object sender, EventArgs e)
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

        private void button6_Click(object sender, EventArgs e)
        {
            
            status = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            total = 0;
            listView1.Items.Clear();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            method.ListviewToTxt(listView1,1);
        }

        private void 邮箱163抓取_Load(object sender, EventArgs e)
        {

        }
    }
}
