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

namespace 主程序202305
{
    public partial class 越睡通 : Form
    {
        public 越睡通()
        {
            InitializeComponent();
        }
        public  string PostUrlDefault(string url, string postData, string COOKIE, string type)
        {
            string result;
            try
            {
                string charset = "utf-8";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
               request.Proxy = null;
                WebHeaderCollection headers = request.Headers;
                headers.Add("x-tif-did:HpnuilFzDJ");
                headers.Add("x-tif-sid:"+textBox2.Text.Trim());
                request.ContentType = type;
                //request.ContentType = "application/json";
                request.ContentLength = (long)Encoding.UTF8.GetBytes(postData).Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
                request.Referer = "";

                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                response.GetResponseHeader("Set-Cookie");
                bool flag = response.Headers["Content-Encoding"] == "gzip";
                string html;
                if (flag)
                {
                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader2 = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset));
                    html = reader2.ReadToEnd();
                    reader2.Close();
                }
                response.Close();
                result = html;
            }
            catch (WebException ex)
            {
                result = ex.ToString();
            }
            return result;
        }

        public void run()
        {


            for (int i = 0; i < listView1.Items.Count; i++)
            {

                try
                {

                    string texts = listView1.Items[i].SubItems[1].Text.ToString();

                    string[] text = texts.Split(new string[] { "," }, StringSplitOptions.None);

                    string url = "https://yst.guangdong.chinatax.gov.cn/app/yss/fpcy/fpcyForWx";
                    string postdata = "{\"fplb\":\"10\",\"fpdm\":\""+text[2]+ "\",\"fphm\":\"" + text[3] + "\",\"fpje\":\"" + text[4] + "\",\"kprq\":\"" + text[5] + "\",\"hsbz\":\"N\",\"timeOut\":\"20000\",\"ignore\":\"true\",\"wglx\":\"yss\",\"requestId\":\"2efc8bef-daaf-41e4-8655-fde4fe31c95c\"}";

                    string html = PostUrlDefault(url, postdata, "", "application/json");
                    html = System.Web.HttpUtility.UrlDecode(html);

               

                    string hjjexx = Regex.Match(html, @"""hjjexx"":([\s\S]*?),").Groups[1].Value;
                    string nsrmc = Regex.Match(html, @"""nsrmc"":""([\s\S]*?)""").Groups[1].Value;
                    string errMsg = Regex.Match(html, @"""errMsg"":""([\s\S]*?)""").Groups[1].Value;

                    listView1.Items[i].SubItems[2].Text = hjjexx;
                    listView1.Items[i].SubItems[3].Text = nsrmc;
                    listView1.Items[i].SubItems[4].Text = errMsg;

                    Thread.Sleep(1000/Convert.ToInt32(textBox1.Text));

                    if (listView1.Items.Count > 2)
                    {
                        this.listView1.Items[i].EnsureVisible();
                    }
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }
            }


        }
        private void button3_Click(object sender, EventArgs e)
        {

            #region 通用检测


            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"BsEFq"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
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

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            string filePath = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            StreamReader sr = new StreamReader(filePath, method.EncodingType.GetTxtType(filePath));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                lv1.SubItems.Add(text[i]);
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
            }
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存
        }

        private void 越睡通_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }
        Thread thread;

        bool zanting = true;
        bool status = true;
        private void button6_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
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

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
