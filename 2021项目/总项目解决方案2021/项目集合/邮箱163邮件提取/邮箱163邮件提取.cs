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
using System.Web;
using System.Windows.Forms;
using myDLL;

namespace 邮箱163邮件提取
{
    public partial class 邮箱163邮件提取 : Form
    {
        public 邮箱163邮件提取()
        {
            InitializeComponent();
        }

        string mailcookie = "";
        string zpcookie = "";

        private bool Request_mail(string url, int start, out string content)
        {
            content = "";
            HttpWebResponse response = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.KeepAlive = true;
                request.Headers.Add("sec-ch-ua", @""" Not A;Brand"";v=""99"", ""Chromium"";v=""90"", ""Google Chrome"";v=""90""");
                request.Accept = "text/javascript";
                request.Headers.Add("sec-ch-ua-mobile", @"?0");
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.85 Safari/537.36";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("Origin", @"https://mail.163.com");
                request.Headers.Add("Sec-Fetch-Site", @"same-origin");
                request.Headers.Add("Sec-Fetch-Mode", @"cors");
                request.Headers.Add("Sec-Fetch-Dest", @"empty");
                request.Referer = "https://mail.163.com/js6/main.jsp?sid=tCrUgnrhwBFXSYcDEXhhfbkyIQYKuJeG&df=mail163_letter";
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "zh,sq;q=0.9,zh-CN;q=0.8,oc;q=0.7,de;q=0.6,en;q=0.5");
                request.Headers.Set(HttpRequestHeader.Cookie, mailcookie);

                request.Method = "POST";
                request.ServicePoint.Expect100Continue = false;

                string body = @"var=%3C%3Fxml%20version%3D%221.0%22%3F%3E%3Cobject%3E%3Cint%20name%3D%22fid%22%3E1%3C%2Fint%3E%3Cstring%20name%3D%22order%22%3Edate%3C%2Fstring%3E%3Cboolean%20name%3D%22desc%22%3Etrue%3C%2Fboolean%3E%3Cint%20name%3D%22limit%22%3E100%3C%2Fint%3E%3Cint%20name%3D%22start%22%3E" + start + "%3C%2Fint%3E%3Cboolean%20name%3D%22skipLockedFolders%22%3Efalse%3C%2Fboolean%3E%3Cstring%20name%3D%22topFlag%22%3Etop%3C%2Fstring%3E%3Cboolean%20name%3D%22returnTag%22%3Etrue%3C%2Fboolean%3E%3Cboolean%20name%3D%22returnTotal%22%3Etrue%3C%2Fboolean%3E%3Cstring%20name%3D%22mrcid%22%3E0868192c6ddd70cf35c7418b95b3c782%3C%2Fstring%3E%3C%2Fobject%3E";
                byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
                request.ContentLength = postBytes.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(postBytes, 0, postBytes.Length);
                stream.Close();
                response = (HttpWebResponse)request.GetResponse();
                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding("utf-8"));
                    content = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    content = reader.ReadToEnd();
                    reader.Close();
                }

            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
                else return false;
            }
            catch (Exception)
            {
                if (response != null) response.Close();
                return false;
            }

            return true;
        }

        public void run()
        {
            int count = 0;
            textBox4.Text = DateTime.Now.ToString("HH:MM:ss") + "：开始抓取...";
            mailcookie = method.GetCookies("https://mail.163.com/js6/main.jsp");
           zpcookie = method.GetCookies("https://www.jianzhimao.com/ctrlcommon/readResume?resumeid=OHJYQU4xd3BndW9GMDNYZHJrVW43dz09");

            string sid = Regex.Match(mailcookie, @"sid=([\s\S]*?);").Groups[1].Value;
            if (sid == "")
            {
                sid = Regex.Match(mailcookie, @"sid=.*").Groups[0].Value.Replace("sid=","");
            }
            string url = "https://mail.163.com/js6/s?sid="+sid+"&func=mbox:listMessages";
            for (int page = 0; page < (Convert.ToInt32(textBox1.Text)*100); page=page+100)
            {
                Request_mail(url, page, out string html);
                MatchCollection uids = Regex.Matches(html, @"'id':'([\s\S]*?)'");
                MatchCollection jops = Regex.Matches(html, @"'subject':'([\s\S]*?)'");
                MatchCollection senddates = Regex.Matches(html, @"sentDate':new Date\(([\s\S]*?)\)");
                if (uids.Count == 0 && page==0)
                {
                    MessageBox.Show("邮箱会话已过期");
                    return;
                }


                for (int i = 0; i < uids.Count; i++)
                {
                    string date = senddates[i].Groups[1].Value;
                    string[] dates = date.Split(new string[] { "," }, StringSplitOptions.None);
                    string newdate = date;
                    if (dates.Length > 5)
                    {
                        newdate = dates[0] + "-" +(Convert.ToInt32(dates[1])+1)+ "-" + dates[2]+" "+dates[3]+":"+ dates[4]+":"+ dates[5];
                     
                        if (Convert.ToDateTime(newdate) >= dateTimePicker1.Value && Convert.ToDateTime(newdate) <= dateTimePicker2.Value)
                        {

                        }
                        else
                        {
                            textBox4.Text = DateTime.Now.ToString("HH:MM:ss") + "：日期不符合...";
                            continue;
                        }
                    }
                   
                    
                    string uid = uids[i].Groups[1].Value;
                    count = count + 1;
                    string aurl = "https://mail.163.com/js6/read/readhtml.jsp?mid="+uid+"&userType=ud&font=15&color=064977";
                    string ahtml= method.GetUrlWithCookie(aurl, mailcookie, "utf-8");
                    string zpid= Regex.Match(ahtml, @"resumeid=([\s\S]*?)""").Groups[1].Value;

                    string zpurl = "https://www.jianzhimao.com/resumelib/getUserCompleteResume?resumeid="+zpid+"&user_type=";
                   
                    string zphtml = method.GetUrlWithCookie(zpurl,zpcookie,"utf-8");
                    if (zphtml.Contains("先登录"))
                    {
                        MessageBox.Show("招聘网站会话已过期");
                        return;
                    }

                    string name = Regex.Match(zphtml, @"""resume_name"":""([\s\S]*?)""").Groups[1].Value;
                    string sex = Regex.Match(zphtml, @"""sex"":""([\s\S]*?)""").Groups[1].Value;
                    string age = Regex.Match(zphtml, @"""age"":""([\s\S]*?)""").Groups[1].Value;
                    string birthday = Regex.Match(zphtml, @"""birthday"":""([\s\S]*?)""").Groups[1].Value;
                    string phone = Regex.Match(zphtml, @"""phone"":""([\s\S]*?)""").Groups[1].Value;
                    
                    string province = Regex.Match(zphtml, @"""province"":""([\s\S]*?)""").Groups[1].Value;
                    string city = Regex.Match(zphtml, @"""city"":""([\s\S]*?)""").Groups[1].Value;
                    string area = Regex.Match(zphtml, @"""area"":""([\s\S]*?)""").Groups[1].Value;
                    textBox4.Text = DateTime.Now.ToShortTimeString() + "：已抓取..." + count + "\r\n" +name+"  " +jops[i].Groups[1].Value;
                    if (name != "")
                    {
                        if (age != "")
                        {
                            if (Convert.ToInt32(age) >= Convert.ToInt32(textBox2.Text) && Convert.ToInt32(age) <= Convert.ToInt32(textBox3.Text))
                            {

                            }
                            else
                            {
                                textBox4.Text = DateTime.Now.ToString("HH:MM:ss") + "：年龄不符合...";
                                continue;  //不符合年龄
                            }
                        }


                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(name);
                        listViewItem.SubItems.Add(sex);
                        listViewItem.SubItems.Add(age);
                        listViewItem.SubItems.Add(birthday);
                        listViewItem.SubItems.Add(phone);
                        listViewItem.SubItems.Add(province + city + area);
                        listViewItem.SubItems.Add(jops[i].Groups[1].Value);
                        listViewItem.SubItems.Add(newdate);

                        string username = "ceshi";
                        string rukuUrl = string.Format("http://www.acaiji.com:8080/api/mt/mtjianliregister.html?name={0}&sex={1}&age={2}&birthday={3}&phone={4}&area={5}&job={6}&time={7}&username={8}&", HttpUtility.UrlEncode(name), HttpUtility.UrlEncode(sex), age, birthday, phone, HttpUtility.UrlEncode(area), HttpUtility.UrlEncode(jops[i].Groups[1].Value), newdate, HttpUtility.UrlEncode(username));
                      string rukuhtml=  method.GetUrl(rukuUrl,"utf-8");
                      
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                    }
                }
            }

            textBox4.Text = "完成";

        }
        private void 邮箱163邮件提取_Load(object sender, EventArgs e)
        {
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("https://mail.163.com/");
            webBrowser2.Navigate("https://www.jianzhimao.com/ctrlcity/newLogin");
        }
        Thread thread;
        bool zanting = true;
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html","utf-8");

            if (!html.Contains(@"N2nhD"))
            {
                MessageBox.Show("");
                return;
            }

            #endregion
            if (thread == null || !thread.IsAlive)
            {

                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ListviewToTxt(listView1);
        }

        private void button4_Click(object sender, EventArgs e)
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

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
        #region  listview导出文本TXT
        public  void ListviewToTxt(ListView listview)
        {

            //if (listview.Items.Count == 0)
            //{
            //    MessageBox.Show("列表为空!");
            //}

/*
 * 
 * 
姓名---性别---年龄
姓名---性别---年龄---号码
姓名---性别---年龄---号码---职位
姓名---性别---年龄---号码---职位---时间
*/
List<string> list = new List<string>();
foreach (ListViewItem item in listview.Items)
{
    switch (comboBox2.Text)
    {
        case "纯号码":
            list.Add(item.SubItems[5].Text);
            break;
        case "姓名---性别---年龄":
            list.Add(item.SubItems[1].Text + "---" + item.SubItems[2].Text + "---" + item.SubItems[3].Text);
            break;
        case "姓名---性别---年龄---号码":
            list.Add(item.SubItems[1].Text + "---" + item.SubItems[2].Text + "---" + item.SubItems[3].Text + "---" + item.SubItems[5].Text);
            break;
        case "姓名---性别---年龄---号码---职位":
                        list.Add(item.SubItems[1].Text + "---" + item.SubItems[2].Text + "---" + item.SubItems[3].Text + "---" + item.SubItems[5].Text + "---" + item.SubItems[7].Text);
            break;
        case "姓名---性别---年龄---号码---职位---时间":
            list.Add(item.SubItems[1].Text + "---" + item.SubItems[2].Text + "---" + item.SubItems[3].Text + "---" + item.SubItems[5].Text + "---" + item.SubItems[7].Text + "---" + item.SubItems[8].Text);
            break;

    }



}
SaveFileDialog sfd = new SaveFileDialog();

// string path = AppDomain.CurrentDomain.BaseDirectory + "导出_" + Guid.NewGuid().ToString() + ".txt";
string path = "";
if (sfd.ShowDialog() == DialogResult.OK)
{
    path = sfd.FileName + ".txt";
}
StringBuilder sb = new StringBuilder();
foreach (string tel in list)
{
    sb.AppendLine(tel);
}
System.IO.File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
MessageBox.Show("文件导出成功!文件地址:" + path);


}







#endregion


}
}
