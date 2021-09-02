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

namespace 工商企业采集
{
    public partial class 工商企业采集 : Form
    {
        public 工商企业采集()
        {
            InitializeComponent();
        }

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            string html = "";
            string COOKIE = "BAIDUID=3FD97B155EDA2A85DD4FCA0DA34D2350:FG=1";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "aiinquiry/2.3.2 (iPhone; iOS 13.6.1; Scale/3.00) aiqicha/2.3.2";
                request.Referer = "https://aiqicha.baidu.com/usercenter";
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("Cuid:72BAF9EA500309A310153C6EB6F523F4F1F0101B1FBGEHHGFGJ");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;

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
            catch (System.Exception ex)
            {
                return ex.ToString();

            }



        }
        #endregion

        public string gettel(string id)
        {
            StringBuilder sb = new StringBuilder();
            //string url = "https://aiqicha.baidu.com/company_detail_"+ id;
            string url = "https://aiqicha.baidu.com/appcompdata/headinfoAjax?pid="+id;
            string html= GetUrl(url, "utf-8");

            MatchCollection phones = Regex.Matches(html, @"""phone"":""([\s\S]*?)""");
            foreach (Match item in phones)
            {
                if(item.Groups[1].Value.Length>5)
                {
                    if (radioButton2.Checked == true)
                    {
                        if (!item.Groups[1].Value.Contains("-") && item.Groups[1].Value.Length>10 && item.Groups[1].Value.Substring(0,1)=="1")
                        {
                           
                            return item.Groups[1].Value;
                        }
                    }
                    else
                    {
                        sb.Append(item.Groups[1].Value + ",");
                    }
                   
                }
                
            }

            if (sb.ToString().Length > 2)
            {
                return sb.ToString().Remove(sb.ToString().Length - 1, 1);
            }
            else
            {
                return sb.ToString();
            }


            
        }
        string cookie = "PSTM=1613733764; BIDUPSID=634C389E425B06C8023560F0B66A7FC2; BDPPN=968b1e012a3c25214233887b063ead52; log_guid=e9c99c3994d0da11a5df342641ad63f7; __yjs_duid=1_0dfa7ccad66512bd9fc094f2cba61b1c1619085461575; H_WISE_SIDS=107315_110085_114551_127969_131423_154619_165136_166147_169066_170035_170816_170872_170935_171509_171707_171710_172226_172472_172828_172924_173088_173125_173412_173602_173610_173625_173774_173830_174180_174197_174357_174445_174681_174771_174806_175133_175212_175215_175364_8000052_8000103_8000131_8000135_8000150; MCITY=-%3A; BAIDUID=52C9E1D94235EF0D56844731CF8E7719:FG=1; BDORZ=B490B5EBF6F3CD402E515D22BCDA1598; _j47_ka8_=57; _j54_6ae_=xlTM-TogKuTwv4cyUEmLGrya98ei0L8Ssgmd; BDUSS=s1dDNSSzRHRW0tZkN4QXQ2TDlzT0h0emR4MHhMWEZOR0Q2VHB-VkFEOGZyMWRoSVFBQUFBJCQAAAAAAQAAAAEAAABwsQkdemhvdWthaWdlNjY2OAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB8iMGEfIjBhb0; BDUSS_BFESS=s1dDNSSzRHRW0tZkN4QXQ2TDlzT0h0emR4MHhMWEZOR0Q2VHB-VkFEOGZyMWRoSVFBQUFBJCQAAAAAAQAAAAEAAABwsQkdemhvdWthaWdlNjY2OAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB8iMGEfIjBhb0; H_PS_PSSID=34437_34532_34497_31660_34004_34092_26350_34323_34473; BAIDUID_BFESS=52C9E1D94235EF0D56844731CF8E7719:FG=1; _fb537_=xlTM-TogKuTwUK886dsO8eMQ7B4g-axI6d4dzsVxGJNlljC2klwQ8N8md; Hm_lvt_ad52b306e1ae4557f5d3534cce8f8bbf=1630544295,1630549932,1630549972,1630550082; Hm_lpvt_ad52b306e1ae4557f5d3534cce8f8bbf=1630550103; ab_sr=1.0.1_N2Q0MTBhNGQ0ZWE3ZTgyY2RjYmYyYWJiYjNjNDhkOWQxZDJiZGJjMTBiNWJkODMwNTY5NzMwNzIzNjgzMWZkNDk5N2UxYTIwOGRhMDU4N2Y4NGM5ZTE0NzNiYmIxZjg1MzljNmUwMGIxY2FhNmU5MmIxYjhlODUxYjk4MzY3ZmE5ZmU1ZGRjOGI1NTY5Y2Q1ODBhMmRiZDdiMjdlYzcwMQ==; __yjs_st=2_MDI0ODY5MmM0ZjBlYmRjYTIwNzVjMWNjMDA0OTI3MDUzZmI2NmJkMTg2NzdmYWRmNTBhMjExMTk1ZWE4ODc2M2Y2OGY4ODRlZTQ5MmU3MmQ4N2FkY2E5MmQyYjhhMmViZGVmZDRkNGM2NjY5Y2QyYmIzOTRkYTA3MDgyYTZjMjM2OGZjN2JhNGVhMjEyYWFmYTk2MjVkMTg5MDNkZDYzNTJhYjVmODA1NzliZTUxMjA3MzRkM2EzZTJhYjM5OGVlNzEwZDc1YzNmMzAyNjYzY2E1MjMzNGMzZDFjZGNlYTI0NzNkNzdjYjg5YmQzNzEwNTczNWUxZjdhZjQ3M2ZjNF83X2JkN2Y0MmNj; _s53_d91_=b905d1f221e72c9fbe6c5b75c71823024829ea664362d3b9a8b7f6014dacce7f7a49de59bfc92927f476223e34a94a5b7dee76bee8a3b216c3c1a0ea39a221dbffec93fbccde206f0fbcf962483ea6a8713de70086606e4673a4b11251a89a9f8394411605b14a1df1b6c381be83ea288b86b466a59a933f836c5318a8616d3003d39bbc8156d77bf84127fbec2b94cdceda024117e8616f43221d55d41c15e4fe97651ec6164f34043f01fad31bc7ad582258354aa0dcb34ca3687adeec8bbde0b3420f18592c7a8416e9b28ac0e0d281ef4c78660cd09ff2e84758c659b182; _y18_s21_=94e77033; RT=\"sl=d&ss=kt2bbssc&tt=i1c&bcn=https%3A%2F%2Ffclog.baidu.com%2Flog%2Fweirwood%3Ftype%3Dperf&z=1&dm=baidu.com&si=p16nepxjqz&ld=41o1&cl=4137\"";
        public void run()
        {

            StringBuilder sb = new StringBuilder();
            if (comboBox1.Text != "不限")
            {
                sb.Append(string.Format("\"entType\":[\"{0}\"],", dic[comboBox1.Text]));
            }
            if (comboBox2.Text != "不限")
            {
                string[] text = dic[comboBox2.Text].Split(new string[] { "-" }, StringSplitOptions.None);
                sb.Append("\"regCap\":[{\"start\":"+text[0]+ ",\"end\":" + text[1] + "}],");
            }
            if (comboBox3.Text != "不限")
            {
                sb.Append(string.Format("\"openStatus\":[\"{0}\"],", System.Web.HttpUtility.UrlEncode(dic[comboBox3.Text])));
            }
            if (comboBox4.Text != "不限")
            {
                string[] text = dic[comboBox4.Text].Split(new string[] { "-" }, StringSplitOptions.None);
                sb.Append("\"startYear\":[{\"start\":\""+text[0]+"\",\"end\":\""+text[1]+"\"}],");
            }
            if (comboBox5.Text != "不限")
            {
                sb.Append(string.Format("\"industryCode1\":[\"{0}\"],", dic[comboBox5.Text]));
            }

            string filter = "";
            if (sb.ToString().Length > 2)
            {
                filter = sb.ToString().Remove(sb.ToString().Length - 1, 1).Replace("\"", "%22").Replace("{", "%7B").Replace("}", "%7D");
            }
            try
            {
                for (int page= 1; page < 10001; page++)
                {

                    string url = "https://aiqicha.baidu.com/search/advanceFilterAjax?q=" + System.Web.HttpUtility.UrlEncode(textBox2.Text) + "&t=&p="+page+"&s=10&o=0&f=%7B"+filter+"%7D";
                    //string html = method.GetUrlWithCookie(url, cookie,"utf-8");
                    string html = GetUrl(url,"utf-8");
                   
                    html = method.Unicode2String(html);
                    //textBox1.Text = html;
                    MatchCollection uids = Regex.Matches(html, @"""pid"":""([\s\S]*?)""");
                    MatchCollection entName = Regex.Matches(html, @"""titleName"":""([\s\S]*?)""");
                    MatchCollection legalPerson = Regex.Matches(html, @"""legalPerson"":""([\s\S]*?)""");
                    MatchCollection regCap = Regex.Matches(html, @"""regCap"":""([\s\S]*?)""");
                    MatchCollection validityFrom = Regex.Matches(html, @"""validityFrom"":""([\s\S]*?)""");
                    MatchCollection domicile = Regex.Matches(html, @"""domicile"":""([\s\S]*?)""");
                    MatchCollection scope = Regex.Matches(html, @"""scope"":""([\s\S]*?)""");

                    for (int i = 0; i < uids.Count; i++)
                    {
                        Thread.Sleep(1000);
                        label7.Text =DateTime.Now.ToLongTimeString()+ "正在提取："+ entName[i].Groups[1].Value;
                        string tel = gettel(uids[i].Groups[1].Value);
                        if (radioButton2.Checked == true)
                        {
                            if (tel == "")
                            {
                                label7.Text = DateTime.Now.ToLongTimeString() + "正在提取：" + entName[i].Groups[1].Value+"--无手机号跳过";
                                continue;
                            }
                                
                        }
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        lv1.SubItems.Add(entName[i].Groups[1].Value);
                        lv1.SubItems.Add(legalPerson[i].Groups[1].Value);
                        lv1.SubItems.Add(regCap[i].Groups[1].Value);
                        lv1.SubItems.Add(validityFrom[i].Groups[1].Value);
                        lv1.SubItems.Add(domicile[i].Groups[1].Value);
                        lv1.SubItems.Add(scope[i].Groups[1].Value);
                        lv1.SubItems.Add(tel);
                         
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;
                    }
                }
            }
            catch (Exception ex)
            {

               label7.Text=ex.ToString();
            }
        }

        private void 工商企业采集_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("不限");
            comboBox2.Items.Add("不限");
            comboBox3.Items.Add("不限");
            comboBox4.Items.Add("不限");
            comboBox5.Items.Add("不限");


            getcates("qylx", comboBox1);
            getcates("zczb", comboBox2);
            getcates("qyzt", comboBox3);
            getcates("clnx", comboBox4);
            getcates("hy",comboBox5);
        }
        bool zanting = true;
        bool status = true;
        Thread thread;
        private void button7_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"PlSUh"))
            {

                return;
            }



            #endregion
            if (textBox2.Text == "")
            {
                MessageBox.Show("请输入关键字");
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


        string path = AppDomain.CurrentDomain.BaseDirectory;
        Dictionary<string, string> dic = new Dictionary<string, string>();
        public void getcates(string txtname,ComboBox cob)
        {
            StreamReader sr = new StreamReader(path+"data\\"+ txtname + ".txt", method.EncodingType.GetTxtType(path + "data\\"+ txtname + ".txt"));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            MatchCollection cates = Regex.Matches(texts, @"data-log-title=""([\s\S]*?)""([\s\S]*?)>([\s\S]*?)<");
            for (int i = 0; i < cates.Count; i++)
            {
                cob.Items.Add(cates[i].Groups[3].Value.Trim());
                dic.Add(cates[i].Groups[3].Value.Trim(), cates[i].Groups[1].Value.Trim());
            }
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存

        }

        public void creatVcf()

        {

            string text = method.GetTimeStamp() + ".vcf";
            if (File.Exists(text))
            {
                if (MessageBox.Show("“" + text + "”已经存在，是否删除它？", "确认", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;
                }
                File.Delete(text);
            }
            UTF8Encoding encoding = new UTF8Encoding(false);
            StreamWriter streamWriter = new StreamWriter(text, false, encoding);
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                string name = listView1.Items[i].SubItems[1].Text.Trim();
                string tel = listView1.Items[i].SubItems[7].Text.Trim();
                if (name != "" && tel != "")
                {
                    streamWriter.WriteLine("BEGIN:VCARD");
                    streamWriter.WriteLine("VERSION:3.0");

                    streamWriter.WriteLine("N;CHARSET=UTF-8:" + name);
                    streamWriter.WriteLine("FN;CHARSET=UTF-8:" + name);

                    streamWriter.WriteLine("TEL;TYPE=CELL:" + tel);



                    streamWriter.WriteLine("END:VCARD");

                }
            }
            streamWriter.Flush();
            streamWriter.Close();
            MessageBox.Show("生成成功！文件名是：" + text);



        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                Thread thread= new Thread(creatVcf);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            status = false;
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
    }
}
