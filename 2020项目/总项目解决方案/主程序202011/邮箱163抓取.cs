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


            for (int i = 0; i < Convert.ToInt32(textBox3.Text); i++)
            {
                int start = i * 30;
                string url = "https://mail.163.com/js6/s?sid="+sid+"&func=mbox:listMessages&mbox_pager_next=1";
                string postdata = "var=%3C%3Fxml%20version%3D%221.0%22%3F%3E%3Cobject%3E%3Cint%20name%3D%22fid%22%3E1%3C%2Fint%3E%3Cstring%20name%3D%22order%22%3Edate%3C%2Fstring%3E%3Cboolean%20name%3D%22desc%22%3Etrue%3C%2Fboolean%3E%3Cint%20name%3D%22limit%22%3E30%3C%2Fint%3E%3Cint%20name%3D%22start%22%3E" + start + "%3C%2Fint%3E%3Cboolean%20name%3D%22skipLockedFolders%22%3Efalse%3C%2Fboolean%3E%3Cstring%20name%3D%22topFlag%22%3Etop%3C%2Fstring%3E%3Cboolean%20name%3D%22returnTag%22%3Etrue%3C%2Fboolean%3E%3Cboolean%20name%3D%22returnTotal%22%3Etrue%3C%2Fboolean%3E%3C%2Fobject%3E";
                string html = PostUrl(url, postdata, COOKIE, "utf-8");

                MatchCollection aids = Regex.Matches(html, @"<string name=""id"">([\s\S]*?)</string>");
                MatchCollection titles = Regex.Matches(html, @"<string name=""subject"">([\s\S]*?)</string>");
                MatchCollection times = Regex.Matches(html, @"<date name=""receivedDate"">([\s\S]*?)</date>");

                if (aids.Count == 0)
                {
                    break;

                }

                for (int j = 0; j < aids.Count; j++)
                {
                    total = total + 1;
                    toolStripStatusLabel2.Text = total.ToString();
                    string aurl = "https://mail.163.com/js6/read/readhtml.jsp?mid="+aids[j].Groups[1].Value+"&userType=ud";
                    string ahtml = GetUrl(aurl);

                    Match name = Regex.Match(ahtml, @"font-weight:normal;"">([\s\S]*?)<");
                    Match age = Regex.Match(ahtml, @">（([\s\S]*?)）");
                    Match tel = Regex.Match(ahtml, @"手机号码：([\s\S]*?)</span>");
                    if (name.Groups[1].Value != "")
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(name.Groups[1].Value);
                        lv1.SubItems.Add(age.Groups[1].Value);
                        lv1.SubItems.Add(Regex.Replace(tel.Groups[1].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(titles[j].Groups[1].Value);
                        lv1.SubItems.Add(times[j].Groups[1].Value);

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;
                    }
                    Thread.Sleep(1000);
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
            //COOKIE = "_ntes_nnid=cd122851463bd744e423f21fad397a63,1590834443609; _ga=GA1.2.1033791233.1590834537; _ntes_nuid=cd122851463bd744e423f21fad397a63; UM_distinctid=173f54f7393232-0c87497a2723a2-6373664-1fa400-173f54f7394bb5; vjlast=1598164493.1598164493.30; vjuids=-1bbd9584b.1741a06e3f5.0.222433b9c83f6; locale=; face=js6; mail_psc_fingerprint=f9980414501707bee1035100250b2caf; vinfo_n_f_l_n3=9248c9d9cccd372d.1.6.1590834443622.1603713715708.1605315103536; NTES_hp_textlink1=old; starttime=; mail_upx_nf=; mail_idc=\"\"; secu_info=1; mail_host=mail.163.com; mail_style=js6; nts_mail_user=kao377632@163.com:-1:1; df=mail163_letter; mail_uid=kao377632@163.com; NTES_SESS=z7HRpkXG7.ZuTfLkbte.8f0YtceN1y1KrdDlWX_fpW0QpvFRpWZ9_iEFwjXeZ7EB9nekq0i.ptVTBTG.r9xjyfyiQ7nEyCfOx5nLqK0cgWxDuZfzFGm.sXHC1e5ipHGeNLpoTl_3YzdaXahYgb9cGw9mcWDIamQq1pZe2N4TeKc9jMIp63WyAoiihKu_.BKH0WHVagvPjcOvbHEvTcs_mLaRz1RQ3Zn28; S_INFO=1605499319|0|2&70##|kao377632; P_INFO=kao377632@163.com|1605499319|0|mail163|00&99|jis&1605499242&mail163#jis&321300#10#0#0|156499&0|mail163|kao377632@163.com; mail_upx=t2hz.mail.163.com|t3hz.mail.163.com|t4hz.mail.163.com|t7hz.mail.163.com|t8hz.mail.163.com|t1hz.mail.163.com|t4bj.mail.163.com|t1bj.mail.163.com|t2bj.mail.163.com|t3bj.mail.163.com; Coremail=187a9c6d2650d%WCMjZlDUiyNqxrkjdXUUVOJCNlostwDI%g1a113.mail.163.com; cm_last_info=dT1rYW8zNzc2MzIlNDAxNjMuY29tJmQ9aHR0cHMlM0ElMkYlMkZtYWlsLjE2My5jb20lMkZqczYlMkZtYWluLmpzcCUzRnNpZCUzRFdDTWpabERVaXlOcXhya2pkWFVVVk9KQ05sb3N0d0RJJnM9V0NNalpsRFVpeU5xeHJramRYVVVWT0pDTmxvc3R3REkmaD1odHRwcyUzQSUyRiUyRm1haWwuMTYzLmNvbSUyRmpzNiUyRm1haW4uanNwJTNGc2lkJTNEV0NNalpsRFVpeU5xeHJramRYVVVWT0pDTmxvc3R3REkmdz1odHRwcyUzQSUyRiUyRm1haWwuMTYzLmNvbSZsPS0xJnQ9LTEmYXM9dHJ1ZQ==; MAIL_SESS=z7HRpkXG7.ZuTfLkbte.8f0YtceN1y1KrdDlWX_fpW0QpvFRpWZ9_iEFwjXeZ7EB9nekq0i.ptVTBTG.r9xjyfyiQ7nEyCfOx5nLqK0cgWxDuZfzFGm.sXHC1e5ipHGeNLpoTl_3YzdaXahYgb9cGw9mcWDIamQq1pZe2N4TeKc9jMIp63WyAoiihKu_.BKH0WHVagvPjcOvbHEvTcs_mLaRz1RQ3Zn28; MAIL_SINFO=1605499319|0|2&70##|kao377632; MAIL_PINFO=kao377632@163.com|1605499319|0|mail163|00&99|jis&1605499242&mail163#jis&321300#10#0#0|156499&0|mail163|kao377632@163.com; mail_entry_sess=4c1ea39c08e18ca1d077896d862bb14ea7a45f246760d3d0f03a8508159efde5aca8b4cb38a18f7eb65f82ec8a923d8ac48262bd237e69e3f1044530c8f934547bb3c82f170e318687f57f597d50325457499d7c3ed9f30d02fc191716f9b3b7efa11295a2e08319e6e975a9081b83aa416a62d120df0871c0633b850ec3ae25a5c3d5914632c51b2816c28894a0cef24a5257955a5bc18e6a75e268f712588603f32e19cadbe56d2ac7332fab8ee48e91fc3d4c8118e16f006de7e42f5769fa; JSESSIONID=215EDD9CE56B0D1BB4C4B11820E5BC22; Coremail.sid=WCMjZlDUiyNqxrkjdXUUVOJCNlostwDI";
            Match name = Regex.Match(COOKIE, @"Coremail\.sid=([\s\S]*?);");
            sid = name.Groups[1].Value;
            MessageBox.Show(sid);
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
            listView1.Items.Clear();
        }
    }
}
