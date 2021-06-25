using System;
using System.Collections;
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
            request.Headers.Add("X-XSRF-TOKEN", token);
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

                string html = "";
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                        request.KeepAlive = true;
                        request.Headers.Add("sec-ch-ua", @""" Not A;Brand"";v=""99"", ""Chromium"";v=""90"", ""Google Chrome"";v=""90""");
                        request.Accept = "application/json, text/plain, */*";
                        request.Headers.Add("X-XSRF-TOKEN", token);
                        request.Headers.Add("X-Requested-With", @"XMLHttpRequest");
                        request.Headers.Add("sec-ch-ua-mobile", @"?0");
                        request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.85 Safari/537.36";
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.Headers.Add("Origin", @"https://item.manager.taobao.com");
                        request.Headers.Add("Sec-Fetch-Site", @"same-origin");
                        request.Headers.Add("Sec-Fetch-Mode", @"cors");
                        request.Headers.Add("Sec-Fetch-Dest", @"empty");
                        request.Referer = "https://item.manager.taobao.com/taobao/manager/render.htm?pagination.current=1&pagination.pageSize=20&tab=in_stock&table.sort.endDate_m=desc";
                        request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
                        request.Headers.Set(HttpRequestHeader.AcceptLanguage, "zh,sq;q=0.9,zh-CN;q=0.8,oc;q=0.7,de;q=0.6,en;q=0.5");
                        request.Headers.Set(HttpRequestHeader.Cookie, cookie);

                        request.Method = "POST";
                        request.ServicePoint.Expect100Continue = false;

                        string body =postData;
                        byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
                        request.ContentLength = postBytes.Length;
                        Stream stream = request.GetRequestStream();
                        stream.Write(postBytes, 0, postBytes.Length);
                        stream.Close();

                       HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                   
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

                    return html;
                          
                
              
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion
        public static string cookie = "x-gpf-submit-trace-id=210596cf16245173200302788ebc4f; x-gpf-render-trace-id=212cbb2416245195445268833ed430; thw=cn; everywhere_tool_welcome=true; hng=CN%7Czh-CN%7CCNY%7C156; UM_distinctid=179f90a49dd5fa-09fb0140012713-d7e163f-1fa400-179f90a49deb3e; t=6af64ef8841da97b8e5718b98e10ae52; xlly_s=1; cookie2=274c2f54e4b35478fb96115bf1920b3d; _tb_token_=533058e366476; _samesite_flag_=true; XSRF-TOKEN=ec5d54d1-b873-4c5e-bab1-178faf0acfc0; cna=P4y2GM7SE3kCAXniuV3Hr4CT; sgcookie=E100VMiQSMH2Ld8ar58ZE6roS9XdyrLTIdOH%2BVWvA4rL3KACabQZyO%2B%2FGdJUx9mr%2F51pH7D13%2B2m2jOPSBIJLXa33g%3D%3D; unb=2662407709; uc3=vt3=F8dCuwzh2d0nQKisL7U%3D&nk2=0vH8pBB%2FyxZ3bOUK804%3D&id2=UU6nQ%2FmttD4ALA%3D%3D&lg2=VT5L2FSpMGV7TQ%3D%3D; csg=fd1682d9; lgc=%5Cu8D1D%5Cu53F0%5Cu946B%5Cu978B%5Cu670D%5Cu4E13%5Cu67DC; cookie17=UU6nQ%2FmttD4ALA%3D%3D; dnk=%5Cu8D1D%5Cu53F0%5Cu946B%5Cu978B%5Cu670D%5Cu4E13%5Cu67DC; skt=a1c972d16d235ad6; existShop=MTYyNDUxNjk0Mg%3D%3D; uc4=id4=0%40U2xqJxIgAHvLZiF%2B5uyWiiaZZ3nK&nk4=0%400EDErsspygisRovwGKK7wjcrjnhGHDYDJQ%3D%3D; tracknick=%5Cu8D1D%5Cu53F0%5Cu946B%5Cu978B%5Cu670D%5Cu4E13%5Cu67DC; _cc_=WqG3DMC9EA%3D%3D; _l_g_=Ug%3D%3D; sg=%E6%9F%9C95; _nk_=%5Cu8D1D%5Cu53F0%5Cu946B%5Cu978B%5Cu670D%5Cu4E13%5Cu67DC; cookie1=B0T5a8v9EEqpcSf30IlBalJmUSlLThW5HgZeKQOyOUc%3D; mt=ci=0_1; _m_h5_tk=e3a1157e4ae0bc643dc8aa54e34d765f_1624527388799; _m_h5_tk_enc=a41eb31f771ffc394338181a7e14aff5; v=0; enc=Td3xjytTYKygd6x15El8kajBY355vnyBBlaVeOpXGXKQFd5ttcpdJJDOYQnMnm3uqiYyb6uLrOBMkd%2Bp5DoVug%3D%3D; x5sec=7b22677066342d74616f62616f3b32223a223935393739353132386131656266333133373135663033363164373633633061434e446b30495947454f4875766f2f4e6d626d5949686f4d4d6a59324d6a51774e7a63774f5473784b414977315a697069506a2f2f2f2f2f41513d3d227d; uc1=existShop=true&cookie14=Uoe2ySLjEeU4bA%3D%3D&cookie16=UIHiLt3xCS3yM2h4eKHS9lpEOw%3D%3D&cookie15=W5iHLLyFOGW7aA%3D%3D&cookie21=Vq8l%2BKCLiv0MyZ1zjr80rw%3D%3D&pas=0; isg=BDU14G8wuSAQYN3kkdYmAgmCRLHvsunE0GRkQLda8az7jlWAfwL5lEMP2Fq4zgF8; l=eBMjXCQujngOQ_I1BOfwourza77OSIRAguPzaNbMiOCP_q5B5XjGW69JqFY6C3GNhsCvR3kEpCzgBeYBqQd-nxvtVSNxxDDmn; tfstk=cf5GB0D8OOJ64qJhF5O1SeS-ifNRaFh2qs5CTstoTM7Of55p7sx-LYyhtyxGjsHf.";

        static string token = "ec5d54d1-b873-4c5e-bab1-178faf0acfc0";
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


        /// <summary>
        /// 获取完整标题
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public string getfulltitle(string item)
        {
            Thread.Sleep(1000);
            //2020-07-29 02:02:02
            try
            {
                int resource = 0;

               

                if (resource==0)
                {
                    string url = "https://item.manager.taobao.com/taobao/manager/fastEdit.htm?optType=editTitle&action=render&opSource=6&itemId=" + item;

                    string html = PostUrl(url,"");
                  
                    Match title = Regex.Match(html, @"value"":{""title"":""([\s\S]*?)""");
                    if (title.Groups[1].Value != "")
                    {

                        return title.Groups[1].Value;
                    }
                    else
                    {
                        resource = 1;
                    }
                    
                }
                if(resource==1)
                {
                    string url = "https://item.upload.taobao.com/sell/publish.htm?itemId=" + item;

                    string html = getHtml(url);
                    Match title = Regex.Match(html, @",""title"":""([\s\S]*?)""");
                    if (title.Groups[1].Value != "")
                    {

                        return title.Groups[1].Value;
                    }
                    else
                    {
                        resource = 0;
                    }
                }
                return "";

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
                    if (i == 1 && items.Count == 0)
                    {
                        MessageBox.Show("获取仓库商品失败");
                    }

                    if (items.Count == 0)
                        return;

                    for (int j = 0; j < items.Count; j++)
                    {

                        
                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(items[j].Groups[1].Value);
                        if (checkBox1.Checked == true)
                        {
                            listViewItem.SubItems.Add(getfulltitle(items[j].Groups[1].Value));

                        }
                        else
                        {
                            listViewItem.SubItems.Add(titles[j].Groups[2].Value);
                        }
                        // listViewItem.SubItems.Add(titles[j].Groups[2].Value);
                       
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


        public void dodel()
        {
            textBox4.Text = "开始删除.......";
               DialogResult dr = MessageBox.Show("确定要删除吗？", "删除", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {


                //foreach (ListViewItem lv in listView1.CheckedItems)
                //{
                //    if (lv.Checked)
                //    {
                //        sb.Append("%22"+lv.SubItems[1].Text+ "%22%2C");


                //    }
                //}

                for (int i = 0; i < listView1.CheckedItems.Count; i++)
                {
                    textBox4.Text = "开始删除......."+ listView1.CheckedItems[i].SubItems[1].Text;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("%22" + listView1.CheckedItems[i].SubItems[1].Text + "%22%2C");
                    string postdata = "jsonBody=%7B%22auctionids%22%3A%5B" + sb.ToString().Remove(sb.ToString().Length - 3, 3) + "%5D%7D";

                    string html = delete(postdata);
                    textBox4.Text = html + "\r\n";
                    Thread.Sleep(200);
                }

                MessageBox.Show("删除结束");

                //删除宝贝
            }
            else
            {

            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Thread search_thread = new Thread(new ThreadStart(dodel));
            Control.CheckForIllegalCrossThreadCalls = false;
            search_thread.Start();



        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox4.Text = "开始执行";


            //cookie = Form1.cookie;
            cookie = textBox6.Text;
            Match tok = Regex.Match(cookie, @"XSRF-TOKEN=([\s\S]*?);");
            token = tok.Groups[1].Value;
            if (tok.Groups[1].Value == "")
            {
                tok = Regex.Match(cookie, @"XSRF-TOKEN=.*");
                token = tok.Groups[0].Value.Replace("XSRF-TOKEN=", "");
            }







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




                string  newtitle = textBox2.Text.Trim() + lists[value].ToString() + textBox3.Text.Trim();
               
               


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

            foreach (ListViewItem lv in listView1.CheckedItems)
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

        private void button9_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
