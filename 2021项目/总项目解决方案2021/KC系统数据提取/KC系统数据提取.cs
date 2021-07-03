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

namespace KC系统数据提取
{
    public partial class KC系统数据提取 : Form
    {
        public KC系统数据提取()
        {
            InitializeComponent();
        }

        string Authorization = "[object Object]";
        string token = "";
        string id= "";

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public string GetUrl(string Url,string parama)
        {
       
            string html = "";
            string COOKIE = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("Authorization: "+ parama);
                //headers.Add("x-nike-visitid:5");
                //headers.Add("x-nike-visitorid:d03393ee-e42c-463e-9235-3ca0491475b4");
                //添加头部
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 100000;

                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding("utf-8"));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }

                response.Close();
                return html;



            }
            catch (System.Exception ex)
            {
                return ex.ToString();

            }



        }
        #endregion

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public  string PostUrl(string url, string postData)
        {
            try
            {
                string charset = "utf-8";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                // request.ContentType = "application/x-www-form-urlencoded";

                WebHeaderCollection headers = request.Headers;
                headers.Add("Authorization: " + Authorization);
                //headers.Add("x-nike-visitid:5");
                //headers.Add("x-nike-visitorid:d03393ee-e42c-463e-9235-3ca0491475b4");
                //添加头部
                request.ContentType = "application/json";
                request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", "");

                request.Referer = "https://web.duanmatong.cn/";
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

        #region base64编码的字符串转为图片
        public Image Base64StringToImage(string strbase64)
        {

            try
            {
                byte[] arr = Convert.FromBase64String(strbase64);
                MemoryStream ms = new MemoryStream(arr);
               Image img = System.Drawing.Image.FromStream(ms);
                return img;
               

              
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion


      
        public void getcaptcha()
        {
            string url = "https://easy-kcapi.hydbest.com/captcha";
            string html = GetUrl(url, Authorization);
           string imgbase64= Regex.Match(html, @"base64,([\s\S]*?)""").Groups[1].Value;
          
            
            Image img = Base64StringToImage(imgbase64);
            if (img!=null)
            {

                pictureBox1.Image = img;
            }
            
        }


        public void login()
        {
            string url = "https://easy-kcapi.hydbest.com/admin/login/";
            string postdata = "{\"account\":\""+ textBox1.Text.Trim() + "\",\"password\":\""+ textBox2.Text.Trim() + "\",\"captcha\":\""+textBox3.Text.Trim()+"\"}";
            string html = PostUrl(url,postdata);
            token = Regex.Match(html, @"""token"":""([\s\S]*?)""").Groups[1].Value;
           id = Regex.Match(html, @"""id"":([\s\S]*?),").Groups[1].Value;

            if (token != "")
            {
                MessageBox.Show("登录成功");
            }
            else
            {
                MessageBox.Show("登录失败");
            }

            
        }


        public StringBuilder getOld(string OrderId,string type)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                string url = "https://easy-kcapi.hydbest.com/admin/kc/tickets/oldContacts?page=1&size=100&contactsType=" + type + "&internalOrderId=" + OrderId;
                string html = GetUrl(url, token);

                MatchCollection phonesa = Regex.Matches(html, @"""phoneNum"":""([\s\S]*?)""");
                MatchCollection namesa = Regex.Matches(html, @"""callName"":([\s\S]*?),");
                MatchCollection callCntsa = Regex.Matches(html, @"""callCnt"":([\s\S]*?),");
                MatchCollection callTimesa = Regex.Matches(html, @"""callTime"":([\s\S]*?),");

                for (int a = 0; a < phonesa.Count; a++)
                {
                    sb.Append(namesa[a].Groups[1].Value.Replace("\"", "") + "," + phonesa[a].Groups[1].Value + "," + callCntsa[a].Groups[1].Value + "," + callTimesa[a].Groups[1].Value + "\n");
                }

                return sb;
            }
            catch (Exception ex)
            {


                return null;
            }

        }



        public void run()
        {
            try
            {
                for (int i = 1; i < 999; i++)
                {
                    string url = "https://easy-kcapi.hydbest.com/admin/kc/tickets/Overduetickets?page="+i+"&size=25&statusForDisplay=TICKET_ALL&currentAgent="+id+"&status=TICKET_STATUS_FOLLOWING_UP";
                    string html= GetUrl(url,token);
                    MatchCollection uids = Regex.Matches(html, @"""id"":([\s\S]*?),");
                    if (uids.Count == 0)
                    {
                        MessageBox.Show("完成");
                        return;
                    }
                    foreach (Match uid in uids)
                    {
                        string ahtml = GetUrl("https://easy-kcapi.hydbest.com/admin/kc/tickets/ticketDetail/"+uid.Groups[1].Value, token);
                        string orderId = Regex.Match(ahtml, @"""orderId"":""([\s\S]*?)""").Groups[1].Value;
                        string username=Regex.Match(ahtml, @"""username"":""([\s\S]*?)""").Groups[1].Value;
                        string idcard = Regex.Match(ahtml, @"""idcard"":""([\s\S]*?)""").Groups[1].Value;
                        string decryptPhone = Regex.Match(ahtml, @"""decryptPhone"":""([\s\S]*?)""").Groups[1].Value;
                        string overdueDays = Regex.Match(ahtml, @"""overdueDays"":([\s\S]*?),").Groups[1].Value;
                        string displayAppName = Regex.Match(ahtml, @"""displayAppName"":""([\s\S]*?)""").Groups[1].Value;
                        string displayProductName = Regex.Match(ahtml, @"""displayProductName"":""([\s\S]*?)""").Groups[1].Value;
                        string lendingAmounts = Regex.Match(ahtml, @"""lendingAmounts"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");


                        string notRepaymentPrincipal = Regex.Match(ahtml, @"""notRepaymentPrincipal"":([\s\S]*?),").Groups[1].Value.Replace("\"","");
                        string dueInterest = Regex.Match(ahtml, @"""dueInterest"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        string dueInterestPenalty = Regex.Match(ahtml, @"""dueInterestPenalty"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                        double qiankuanzonge = Convert.ToDouble(notRepaymentPrincipal) + Convert.ToDouble(dueInterest) + Convert.ToDouble(dueInterestPenalty);
                        string bank = Regex.Match(ahtml, @"""bank"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");



                        string htmla = Regex.Match(ahtml, @"contactFamily([\s\S]*?)\]").Groups[1].Value;  //亲密
                        string htmlb = Regex.Match(ahtml, @"contactUrgent([\s\S]*?)\]").Groups[1].Value;   //紧急
                        string htmlc = Regex.Match(ahtml, @"contactHighFerq([\s\S]*?)\]").Groups[1].Value;  //高频


                        MatchCollection phonesa = Regex.Matches(htmla, @"""phoneNum"":""([\s\S]*?)""");
                        MatchCollection namesa = Regex.Matches(htmla, @"""callName"":([\s\S]*?),");
                        MatchCollection callCntsa = Regex.Matches(htmla, @"""callCnt"":([\s\S]*?),");
                        MatchCollection callTimesa = Regex.Matches(htmla, @"""callTime"":([\s\S]*?),");

                        MatchCollection namesb= Regex.Matches(htmlb, @"""callName"":([\s\S]*?),");
                        MatchCollection phonesb = Regex.Matches(htmlb, @"""phoneNum"":""([\s\S]*?)""");

                        MatchCollection phonesc = Regex.Matches(htmlc, @"""phoneNum"":""([\s\S]*?)""");
                        MatchCollection namesc = Regex.Matches(htmlc, @"""callName"":([\s\S]*?),");
                        MatchCollection callCntsc = Regex.Matches(htmlc, @"""callCnt"":([\s\S]*?),");
                        MatchCollection callTimesc = Regex.Matches(htmlc, @"""callTime"":([\s\S]*?),");

                        StringBuilder sba = new StringBuilder();
                        StringBuilder sbb = new StringBuilder();
                        StringBuilder sbc = new StringBuilder();





                        for (int a= 0; a <phonesa.Count; a++)
                        {
                            sba.Append(namesa[a].Groups[1].Value.Replace("\"", "") +","+phonesa[a].Groups[1].Value+","+callCntsa[a].Groups[1].Value+","+callTimesa[a].Groups[1].Value+"\n");
                        }
                        for (int b = 0; b < phonesb.Count;b++)
                        {
                            sbb.Append(namesb[b].Groups[1].Value.Replace("\"","") + "," + phonesb[b].Groups[1].Value + "\n");
                        }

                        for (int c = 0; c < phonesc.Count; c++)
                        {
                            sbc.Append(namesc[c].Groups[1].Value.Replace("\"", "") +","+phonesc[c].Groups[1].Value + "," + callCntsc[c].Groups[1].Value + "," + callTimesc[c].Groups[1].Value + "\n");
                        }


                        if (phonesa.Count == 0)
                        {
                            sba = getOld(orderId,"2");
                        }

                        if (phonesb.Count == 0)
                        {
                            sbb = getOld(orderId, "0");
                        }
                        if (phonesc.Count == 0)
                        {
                            sbc = getOld(orderId, "1");
                        }

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(username);
                        lv1.SubItems.Add(idcard);
                        lv1.SubItems.Add(decryptPhone);
                        lv1.SubItems.Add(overdueDays);
                        lv1.SubItems.Add(displayAppName);
                        lv1.SubItems.Add(displayProductName);
                        lv1.SubItems.Add(lendingAmounts);
                        lv1.SubItems.Add(qiankuanzonge.ToString());
                        lv1.SubItems.Add(notRepaymentPrincipal);
                        lv1.SubItems.Add(bank);

                        lv1.SubItems.Add(sba.ToString());
                        lv1.SubItems.Add(sbb.ToString());
                        lv1.SubItems.Add(sbc.ToString());
                    }

                   
              
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }
        private void KC系统数据提取_Load(object sender, EventArgs e)
        {
            getcaptcha();
            //method.SetFeatures(11000);
            //webBrowser1.ScriptErrorsSuppressed = true;
            //webBrowser1.Navigate("https://easy-kcadmin.hydbest.com/#/login");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            getcaptcha();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            login();
        }
        Thread thread;
        bool zanting = true;
        bool status = true;
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
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

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
