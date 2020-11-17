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

namespace 常用软件非客户
{
    public partial class VIP转付费 : Form
    {
        public VIP转付费()
        {
            InitializeComponent();
        }
        string COOKIE = "";


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
                request.Referer = "https://wenku.baidu.com/user/interface/getcontribution?st=7&pn=20";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
              
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

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public string PostUrl(string url, string postData)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
            
                request.ContentType = "application/json";
                //request.ContentLength = postData.Length;
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = "https://wenku.baidu.com/user/interface/getcontribution?st=7&pn=20";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                string Tracecode = response.GetResponseHeader("Tracecode");

                return Tracecode;
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion

        public string gaijiaAPI(string id,string name)
        {
            string price = textBox1.Text;

                string url = "https://wenku.baidu.com/ndecommtob/api/doc/upload/updatepublicdoc";
            string postdata = "{\"need_process_doc_info_list\":{\""+id+"\":{\"title\":\""+name+"\",\"summary\":null,\"tag_str\":\"\",\"flag\":10,\"cid1\":1,\"cid2\":9,\"cid3\":30,\"cid4\":0,\"fold_id\":\"1358364423\",\"old_cid1\":1,\"old_cid2\":9,\"old_cid3\":30,\"old_cid4\":0,\"new_cid1\":1,\"adoptStatus\":0,\"free_page\":999,\"downloadable\":1,\"pay_price\":"+price.Trim()+"00}}}";
            string Tracecode = PostUrl(url,postdata);

            return Tracecode;

        }
        public string zhuanhuanAPI(string id, string name)
        {
            string price = textBox1.Text;

            string url = "https://wenku.baidu.com/ndecommtob/api/doc/upload/updatepublicdoc";
            string postdata = "{\"need_process_doc_info_list\":{\"" + id + "\":{\"title\":\"" + name + "\",\"summary\":null,\"tag_str\":\"\",\"flag\":10,\"cid1\":1,\"cid2\":9,\"cid3\":30,\"cid4\":0,\"fold_id\":\"1358364423\",\"old_cid1\":1,\"old_cid2\":9,\"old_cid3\":30,\"old_cid4\":0,\"new_cid1\":1,\"adoptStatus\":0,\"free_page\":999,\"downloadable\":1,\"pay_price\":" + price.Trim() + "00}}}";
            string Tracecode = PostUrl(url, postdata);

            return Tracecode;

        }

        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }
        /// <summary>
        /// 付费改价
        /// </summary>
        public void run()
        {
            for (int i = (Convert.ToInt32(textBox2.Text)*20)-20; i >0; i=i-20)
            {

                string url = "https://wenku.baidu.com/user/interface/getuserpaydocs?range_time=1&pn=" + i;
                string html = GetUrl(url);
              
                MatchCollection docids = Regex.Matches(html, @"""doc_id"":""([\s\S]*?)""");
                MatchCollection names = Regex.Matches(html, @"""title"":""([\s\S]*?)""");
               
                for (int j = 0; j < docids.Count; j++)
                {
                    try
                    {
                        string docid = docids[j].Groups[1].Value.Trim();
                        string name = Unicode2String(names[j].Groups[1].Value).Trim();
                        string zhuangtai = gaijiaAPI(docid, name);
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据    
                        lv1.SubItems.Add(docid);
                        lv1.SubItems.Add(name);
                        lv1.SubItems.Add(zhuangtai);
                        Thread.Sleep(5000);
                    }
                    catch (Exception)
                    {

                        continue;
                    }
                 
                }

            }



        }

        /// <summary>
        /// VIP转付费
        /// </summary>
        public void viptofufei()
        {
            for (int i = 0; i < Convert.ToInt32(textBox2.Text) * 20; i=i+20)

            {

                string url = "https://wenku.baidu.com/user/interface/getcontribution?st=7&pn=" + i;
                string html = GetUrl(url);

                MatchCollection docids = Regex.Matches(html, @"""doc_id"":""([\s\S]*?)""");
                MatchCollection names = Regex.Matches(html, @"""title"":""([\s\S]*?)""");

                for (int j = 0; j < docids.Count; j++)
                {
                    try
                    {
                        string docid = docids[j].Groups[1].Value.Trim();
                        string name = Unicode2String(names[j].Groups[1].Value).Trim();
                        string zhuangtai = zhuanhuanAPI(docid, name);
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据    
                        lv1.SubItems.Add(docid);
                        lv1.SubItems.Add(name);
                        lv1.SubItems.Add(zhuangtai);
                        Thread.Sleep(5000);
                    }
                    catch (Exception)
                    {

                        continue;
                    }

                }

            }



        }

     
        private void VIP转付费_Load(object sender, EventArgs e)
        {

        }
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            COOKIE = textBox3.Text;

        
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(viptofufei);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void VIP转付费_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}
