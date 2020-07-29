using System;
using System.Collections;
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

namespace 主程序202007
{
    public partial class 仓库宝贝管理 : Form
    {
        public 仓库宝贝管理()
        {
            InitializeComponent();
        }
        public string getHtml(string url)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.186 Safari/537.36");
            headers.Add("Cookie", cookie);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.UserAgent = headers["user-agent"];
            request.Timeout = 5000;
            request.KeepAlive = true;
            request.Headers["Cookie"] = headers["Cookie"];
            HttpWebResponse Response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(Response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
            string content = reader.ReadToEnd();
            return content;

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
        public static string PostUrl(string url, string postData)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";

                WebHeaderCollection headers = request.Headers;
                headers.Add("x-xsrf-token:"+token);

                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", cookie);

                request.Referer = "https://item.manager.taobao.com/taobao/manager/render.htm?tab=in_stock&table.sort.endDate_m=desc&spm=a217wi.openworkbeanchtmall";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

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
        public static string cookie = "x-gpf-render-trace-id=0b5205c315960111589843703e5802; thw=cn; hng=CN%7Czh-CN%7CCNY%7C156; _fbp=fb.1.1590825258546.1202985694; x=e%3D1%26p%3D*%26s%3D0%26c%3D0%26f%3D0%26g%3D0%26t%3D0; UM_distinctid=1727ee9bc7d468-09c5e317d04876-6373664-1fa400-1727ee9bc7e656; everywhere_tool_welcome=true; t=1e2cacb3ee45011b73c59f8f0cffef97; cna=b2RCFw6lRXICAXnq99rJ24nX; lgc=%5Cu8D1D%5Cu53F0%5Cu946B%5Cu978B%5Cu670D%5Cu4E13%5Cu67DC; tracknick=%5Cu8D1D%5Cu53F0%5Cu946B%5Cu978B%5Cu670D%5Cu4E13%5Cu67DC; mt=ci=0_1; enc=9yO8%2B3i5RXgiX6gMxPD9p4ClY8bsIxzIV10YGhUjiT1wJz3awDwqGz8jfptqGIFLLOX6woCCBl981LW1r4MFqg%3D%3D; cookie2=19ff5f06b46920fda3c7f4449cffa042; _tb_token_=35351fb0e0e34; _samesite_flag_=true; tfstk=cARNB2MrtfhZUYfje1141y-uzKWOZUTMoW75ILEqgp_mIZ6GilrA-Vvl8Zs-tOf..; sgcookie=EO7TLJEvfrlq7Cb1wRyYn; unb=2662407709; uc3=id2=UU6nQ%2FmttD4ALA%3D%3D&vt3=F8dBxGynV7eZCJc%2Fvk4%3D&nk2=0vH8pBB%2FyxZ3bOUK804%3D&lg2=W5iHLLyFOGW7aA%3D%3D; csg=dc85d2a0; cookie17=UU6nQ%2FmttD4ALA%3D%3D; dnk=%5Cu8D1D%5Cu53F0%5Cu946B%5Cu978B%5Cu670D%5Cu4E13%5Cu67DC; skt=8456803ed7ad89c8; existShop=MTU5NjAxMTEzNw%3D%3D; uc4=nk4=0%400EDErsspygisRovwGKK7wjSkBWfDEofokA%3D%3D&id4=0%40U2xqJxIgAHvLZiF%2B5d2vJrlFUvIW; _cc_=UIHiLt3xSw%3D%3D; _l_g_=Ug%3D%3D; sg=%E6%9F%9C95; _nk_=%5Cu8D1D%5Cu53F0%5Cu946B%5Cu978B%5Cu670D%5Cu4E13%5Cu67DC; cookie1=B0T5a8v9EEqpcSf30IlBalJmUSlLThW5HgZeKQOyOUc%3D; _m_h5_tk=8aac99bd3c383886ef71c745d59ca24a_1596019778894; _m_h5_tk_enc=79a31ca16b4c4b88112938b520b4b3fb; v=0; XSRF-TOKEN=66071665-3bb0-4bd8-be79-17e66f7e77ba; uc1=cookie16=V32FPkk%2FxXMk5UvIbNtImtMfJQ%3D%3D&existShop=true&cookie14=UoTV6huNeFMKlA%3D%3D&cookie15=UIHiLt3xD8xYTw%3D%3D&cookie21=UtASsssmfaCOMId3WBW%2Btw%3D%3D&pas=0; l=eBTLsRAPOEHCm9c3BO5aPurza77TUIRb8sPzaNbMiInca6phsp3TxNQqcxD2-dtjgt5jdeKyIQLleRUyrVa38xi3ebPBTHKlde968e1..; isg=BEpKJ8VEIaTCsK0INqSrO587mzDsO86VZqCmutSDLB1gh-tBvc4XpM-xl_Nbd0Yt";

        static string token = "24ed8b44-e287-448c-a20f-7ce816255b0f";
        /// <summary>
        /// 修改标题
        /// </summary>
        /// <param name="itemid"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public string xiugaiTitle(string itemid,string title)
        {
            try
            {
                string url = "https://item.manager.taobao.com/taobao/manager/fastEdit.htm?optType=editTitle&action=submit";
                string postdata = "value=%7B%22title%22%3A%22" + System.Web.HttpUtility.UrlEncode(title, Encoding.GetEncoding("utf-8")) + "%22%7D&itemId=" + itemid;
                
                string html = PostUrl(url,postdata);
                return html;
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// 定时上架
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string shangjia(string time,string item)
        {
            //2020-07-29 02:02:02
            try
            {
                string url = "https://item.manager.taobao.com/taobao/manager/fastEdit.htm?optType=onTimeShelf&action=submit";
                //string postdata = "value=%7B%22onTimeShelfDate%22%3A%222020-07-29+02%3A02%3A02%22%2C%22itemStatus_m%22%3A%7B%22itemStatus%22%3A-2%2C%22start%22%3A1585362463000%2C%22end%22%3A1595776672000%7D%7D&itemId=614057812417";
                string postdata = "value=%7b%22onTimeShelfDate%22%3a%22"+System.Web.HttpUtility.UrlEncode(time, Encoding.GetEncoding("utf-8"))+"%22%7d&itemId=" +item;

                string html = PostUrl(url, postdata);
                
                return html;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public ArrayList getNewtitles()
        {
            ArrayList titles = new ArrayList();
            StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.GetEncoding("utf-8"));
            string text = streamReader.ReadToEnd();
            string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int i = 0; i < array.Length; i++)
            {
                titles.Add(array[i].Trim());
            }
            return titles;
        }

        public void getitems()
        {
            ArrayList lists = getNewtitles();
            if (lists.Count == 0)
            {
                MessageBox.Show("新标题为空");
                return;
            }

            int value = 0;
            try
            {
                for (int i = 0; i < 999; i++)
                {
                    string url = "https://item.manager.taobao.com/taobao/manager/table.htm";
                    string postdata = "jsonBody=%7B%22filter%22%3A%7B%7D%2C%22pagination%22%3A%7B%22current%22%3A"+i+"%2C%22pageSize%22%3A20%7D%2C%22table%22%3A%7B%22sort%22%3A%7B%22endDate_m%22%3A%22desc%22%7D%7D%2C%22tab%22%3A%22in_stock%22%7D";

                    string html = PostUrl(url, postdata);
                    MatchCollection items = Regex.Matches(html, @"""itemId"":""([\s\S]*?)""");
                    MatchCollection titles = Regex.Matches(html, @"""desc"":\[([\s\S]*?)""text"":""([\s\S]*?)""");
                    if (items.Count == 0)
                        return;

                    for (int j = 0; j < items.Count; j++)
                    {
                       

                        if (value >= lists.Count)
                        {
                            MessageBox.Show("新标题不足");
                            return;

                        }
                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(items[j].Groups[1].Value);
                        listViewItem.SubItems.Add(titles[j].Groups[2].Value);
                        if (lists[value].ToString() == "")
                        {
                            value = value + 1;
                            continue;
                        }


                            string newtitle = textBox2.Text.Trim()+ lists[value].ToString()+ textBox3.Text.Trim();

                        string zhuangtai = xiugaiTitle(items[j].Groups[1].Value.Trim(),CutStringByte(newtitle,60));
                        
                        if (zhuangtai.Contains("成功"))
                        {
                            listViewItem.SubItems.Add("成功");
                        }
                        else
                        {
                            listViewItem.SubItems.Add("失败");
                            textBox4.Text += zhuangtai + "\r\n";

                        }

                        string time = dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        if (checkBox1.Checked == true)
                        {
                            string zhuangtai2 = shangjia(time, items[j].Groups[1].Value.Trim());
                            if (zhuangtai2.Contains("成功"))
                            {
                                listViewItem.SubItems.Add("成功");
                            }
                            else
                            {
                                listViewItem.SubItems.Add("失败");
                                textBox4.Text += zhuangtai2 + "\r\n";

                            }
                        }

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                        value = value + 1;
                        Thread.Sleep(1000);
                    }

                    Thread.Sleep(2000);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要删除吗？", "删除", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                //删除宝贝
            }
            else
            {
               
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"cangkubaobei"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion

            cookie = Form1.cookie;

            Match tok = Regex.Match(cookie, @"XSRF-TOKEN=([\s\S]*?);");
            token = tok.Groups[1].Value;
           

            if (textBox1.Text == "")
            {
                MessageBox.Show("请导入新标题文件");
                return;
            }
            Thread search_thread = new Thread(new ThreadStart(getitems));
            Control.CheckForIllegalCrossThreadCalls = false;
            search_thread.Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }

        private void 仓库宝贝管理_Load(object sender, EventArgs e)
        {

        }
        bool zanting = true;
        private void button4_Click(object sender, EventArgs e)
        {
            Form1 fm1 = new Form1();
            fm1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
           zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        /// <summary>
        /// 按指定(字节)长度截取字符串
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <returns>string</returns>
        private string CutStringByte(string str,int value)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            if (System.Text.Encoding.Default.GetByteCount(str) <= value)
            {
                return str;
            }
            int i = 0;//字节数
            int j = 0;//实际截取长度
            foreach (char newChar in str)
            {
                if ((int)newChar > 127)
                {
                    //汉字
                    i += 2;
                }
                else
                {
                    i++;
                }

                if (i <= value)
                    j++;
                else
                    break;
            }
            //str = str.Substring(0, j) + "...";
            str = str.Substring(0, j);
            return str;
        }


    }
}
