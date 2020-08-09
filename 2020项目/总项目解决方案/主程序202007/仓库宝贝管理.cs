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
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
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
        public static string cookie = "x-gpf-render-trace-id=0b521ce215960724879376506ed809; thw=cn; hng=CN%7Czh-CN%7CCNY%7C156; _fbp=fb.1.1590825258546.1202985694; x=e%3D1%26p%3D*%26s%3D0%26c%3D0%26f%3D0%26g%3D0%26t%3D0; UM_distinctid=1727ee9bc7d468-09c5e317d04876-6373664-1fa400-1727ee9bc7e656; everywhere_tool_welcome=true; t=1e2cacb3ee45011b73c59f8f0cffef97; cna=b2RCFw6lRXICAXnq99rJ24nX; mt=ci=0_1; enc=9yO8%2B3i5RXgiX6gMxPD9p4ClY8bsIxzIV10YGhUjiT1wJz3awDwqGz8jfptqGIFLLOX6woCCBl981LW1r4MFqg%3D%3D; _m_h5_tk=e3e8610c8b6ac7e53d0afbe5ae311143_1596081360409; _m_h5_tk_enc=2068e642d1f0284f5b6fd538ee9b3a57; cookie2=1b2c5e04ec1c8572423efec3e4a2fcfa; _tb_token_=57a91f55b7663; _samesite_flag_=true; sgcookie=EJ9qTr2p56fbJ8A5HrUO%2B; unb=2662407709; uc3=id2=UU6nQ%2FmttD4ALA%3D%3D&lg2=UIHiLt3xD8xYTw%3D%3D&nk2=0vH8pBB%2FyxZ3bOUK804%3D&vt3=F8dBxGynURaoyOn8E2U%3D; csg=7b24c377; lgc=%5Cu8D1D%5Cu53F0%5Cu946B%5Cu978B%5Cu670D%5Cu4E13%5Cu67DC; cookie17=UU6nQ%2FmttD4ALA%3D%3D; dnk=%5Cu8D1D%5Cu53F0%5Cu946B%5Cu978B%5Cu670D%5Cu4E13%5Cu67DC; skt=aa984f2ee9f32ec1; existShop=MTU5NjA3MjQ3OA%3D%3D; uc4=nk4=0%400EDErsspygisRovwGKK7wjSkBWfFuFNRzw%3D%3D&id4=0%40U2xqJxIgAHvLZiF%2B5d2vJr%2BnnDdN; tracknick=%5Cu8D1D%5Cu53F0%5Cu946B%5Cu978B%5Cu670D%5Cu4E13%5Cu67DC; _cc_=WqG3DMC9EA%3D%3D; _l_g_=Ug%3D%3D; sg=%E6%9F%9C95; _nk_=%5Cu8D1D%5Cu53F0%5Cu946B%5Cu978B%5Cu670D%5Cu4E13%5Cu67DC; cookie1=B0T5a8v9EEqpcSf30IlBalJmUSlLThW5HgZeKQOyOUc%3D; tfstk=cENCBOvjGHxIhxXyTy_ZYFOwX4llZeLjP9i3RRys9yldT2aCi6Oqc2FUE3dtHV1..; v=0; XSRF-TOKEN=492c0c9a-c1c0-488d-9b27-9fe844b2963a; uc1=existShop=true&cookie15=U%2BGCWk%2F75gdr5Q%3D%3D&pas=0&cookie16=W5iHLLyFPlMGbLDwA%2BdvAGZqLg%3D%3D&cookie21=U%2BGCWk%2F7owY3j65jYY1yBA%3D%3D&cookie14=UoTV6huL5AUf4g%3D%3D; l=eBTLsRAPOEHCmT_FBO5Zhurza77OWQAfcsPzaNbMiInca6K1_n43iNQq0jd98dtjgt5E2eKyIQLleRe6rvULRETJnk3HTHKldAJpre1..; isg=BCMjHEgKeCjDmTQfd-fihC4IsmfNGLdaV7MfDVWCAALHlEu23O1hqvuCjmSaNA9S";

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
        /// 删除宝贝
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string delete( string postdata)
        {
           
            try
            {
                string url = "https://item.manager.taobao.com/taobao/manager/batchFastEdit.htm?optType=batchDeleteItem&action=submit";


                string html = PostUrl(url, postdata);

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
            StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.Default);
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
            

            
            try
            {
                for (int i = 1; i < 99; i++)
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

                        
                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(items[j].Groups[1].Value);
                        listViewItem.SubItems.Add(titles[j].Groups[2].Value);
                        listViewItem.SubItems.Add("-");
                        listViewItem.SubItems.Add("-");




                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        } 
                        
                    }

                    Thread.Sleep(1000);
                }
                MessageBox.Show("仓库宝贝获取完成");
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
                StringBuilder sb = new StringBuilder();
                foreach (ListViewItem lv in listView1.Items)
                {
                    if (lv.Checked)
                    {
                        sb.Append("%22"+lv.SubItems[1].Text+ "%22%2C");
                    
                       
                    }
                }

                string postdata = "jsonBody=%7B%22auctionids%22%3A%5B" + sb.ToString().Remove(sb.ToString().Length-3 ,3) + "%5D%7D";
               
                string html = delete(postdata);
                if (html.Contains("成功"))
                {
                    MessageBox.Show("删除成功");
                }
                else
                {
                    textBox4.Text += html + "\r\n";
                }

                //删除宝贝
            }
            else
            {
               
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox4.Text = "开始执行";


            cookie = Form1.cookie;
            Match tok = Regex.Match(cookie, @"XSRF-TOKEN=.*");
            token = tok.Groups[0].Value.Replace("XSRF-TOKEN=", "");


            if (cookie == "")
            {
                MessageBox.Show("请先登录");
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

        private void 仓库宝贝管理_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
                //System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }
        //修改标题

        int qianzhuigeshu = 0;

        public void xiugai()
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请导入新标题文件");
                return;
            }
            ArrayList lists = getNewtitles();
            int value = 0;

            if (lists.Count == 0)
            {
                MessageBox.Show("新标题为空");
                return;
            }
            foreach (ListViewItem lv in listView1.Items)
            {

                if (value >= lists.Count)
                {
                    MessageBox.Show("新标题不足");
                    return;

                }
                if (lists[value].ToString() == "")
                {
                    value = value + 1;
                    continue;
                }




                string newtitle = "";

                if (qianzhuigeshu <= numericUpDown1.Value)
                {
                    newtitle = textBox2.Text.Trim() + lists[value].ToString() + textBox3.Text.Trim();
                }
                else
                {
                    newtitle =  lists[value].ToString();
                }
               


                string zhuangtai = xiugaiTitle(lv.SubItems[1].Text, CutStringByte(newtitle, 60));

                if (zhuangtai.Contains("成功"))
                {
                    lv.SubItems[3].Text = "成功";
                }
                else
                {
                    lv.SubItems[3].Text = "失败";
                    textBox4.Text += zhuangtai + "\r\n";

                }
                value = value + 1;
                Thread.Sleep(1000);

                qianzhuigeshu = qianzhuigeshu + 1;
            }

            MessageBox.Show("标题全部修改成功");

        }
        private void button6_Click(object sender, EventArgs e)
        {
            Thread search_thread = new Thread(new ThreadStart(xiugai));
            Control.CheckForIllegalCrossThreadCalls = false;
            search_thread.Start();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string time = dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss");

            foreach (ListViewItem lv in listView1.Items)
            {

                
               
                    string zhuangtai2 = shangjia(time, lv.SubItems[1].Text);
                    if (zhuangtai2.Contains("成功"))
                    {
                        lv.SubItems[4].Text="成功";
                    }
                    else
                    {
                        lv.SubItems[4].Text = "失败";
                        textBox4.Text += zhuangtai2 + "\r\n";

                    }
                time=Convert.ToDateTime(time).AddSeconds(Convert.ToInt32(textBox5.Text)).ToString("yyyy-MM-dd HH:mm:ss");
                Thread.Sleep(1000);

            }
            MessageBox.Show("定时上架全部修改成功");

        }

        private void button8_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (ListViewItem lv in listView1.Items)
            {
                lv.Checked = true;
            }
        }
    }
}
