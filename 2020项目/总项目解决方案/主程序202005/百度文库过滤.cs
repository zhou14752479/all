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
using CsharpHttpHelper;
using helper;

namespace 主程序202005
{
    public partial class 百度文库过滤 : Form
    {
        public 百度文库过滤()
        {
            InitializeComponent();
        }

        private void 百度文库过滤_Load(object sender, EventArgs e)
        {
            webBrowser1.Url = new Uri("https://passport.baidu.com/v2/?login&tpl=do&u=https%3A%2F%2Fwenku.baidu.com/nduc/browse/uc?_page=home&_redirect=1#/mydocsupload");
            webBrowser1.ScriptErrorsSuppressed = true;
            method.SetWebBrowserFeatures(method.IeVersion.IE10);
        }

        static string COOKIE = "murmur=undefined--Win32; BIDUPSID=A71B424CE88935CF0B85D5E3FE18C45D; PSTM=1589341639; BAIDUID=A71B424CE88935CFBE40D6274B822189:FG=1; BDORZ=B490B5EBF6F3CD402E515D22BCDA1598; _click_param_pc_rec_doc_2017_testid=1; _click_param_reader_query_ab=-1; BDUSS=nlLflBzZkdab0JKNnNoMmppSEtmbUtHUklLVlR-clI1Qmd6akUyOUIzVFR-dTllSVFBQUFBJCQAAAAAAAAAAAEAAADOMhPS1tDQodGnvqvGt73M0~0AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAANNxyF7TccheS; Hm_lvt_d8bfb560f8d03bbefc9bdecafc4a4bf6=1590134983,1590194411,1590221054,1590282665; isJiaoyuVip=1; H_PS_PSSID=1454_31670_21079_31590_31605_31271_31464_31322_30823_22157; delPer=0; PSINO=5; upLayerDoc=true; Hm_lpvt_d8bfb560f8d03bbefc9bdecafc4a4bf6=1590283476";

        string token = "";
        string folder = "1362699824";
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://ishare.iask.sina.com.cn/ucenter/myuploads?subPage=a&pindex=2";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("sec-fetch-mode:navigate");
                headers.Add("sec-fetch-site:same-origin");
                headers.Add("sec-fetch-user:?1");
                headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                // request.KeepAlive = true;
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
        public static string PostUrl(string url, string postData)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                      
                request.ContentType = "multipart/form-data; boundary=----WebKitFormBoundary6hBArAxW0nlIcAAV";
                request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
                
                request.Referer = "https://wenku.baidu.com/nduc/browse/uc?_page=mydocsupload&st=1&_redirect=1";
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

        public bool panduan(string title)

        {

            string[] text = textBox8.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != "")
                {
                    if (title.Contains(text[i]))
                    {
                        return true;
                    }
                }

            }
            return false;

        }
        bool zanting = true;
        /// <summary>
        /// 筛选文档
        /// </summary>
        public void getdocs()
        {
            string st = "5";
            //if (radioButton1.Checked == true)
            //{

            //}
            //else if (radioButton2.Checked == true)
            //{
            //    st = "9";
            //}

            try
            {
                for (int i = Convert.ToInt32(textBox1.Text.Trim()); i <= Convert.ToInt32(textBox2.Text.Trim()); i++)
                {
                    textBox3.Text += "正在筛选第" + i + "页" + "\r\n";
                    int pn = (i - 1) * 20;
                    string url = "https://wenku.baidu.com/nduc/api/user/interface/getcontribution?st="+st+"&pn="+pn;

                    string html = GetUrl(url);

                    MatchCollection ids = Regex.Matches(html, @"""doc_id"":""([\s\S]*?)""");
                    Match token = Regex.Match(html, @"""new_token"":""([\s\S]*?)""");
                    MatchCollection titles = Regex.Matches(html, @"""title"":""([\s\S]*?)""");

                    this.token = token.Groups[1].Value;
                    for (int j = 0; j < ids.Count; j++)
                    {
                        if (panduan(titles[j].Groups[1].Value))
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                            lv1.SubItems.Add(ids[j].Groups[1].Value);
                          
                            lv1.SubItems.Add(titles[j].Groups[1].Value);
                           
                        }

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                    }
                }

                MessageBox.Show("筛选结束");

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        public bool deletedocs(string doc)
        {

            try
            {
                string url = "https://wenku.baidu.com/user/submit/newdocDelete?doc_id_str="+doc+"&new_token="+token+"&token="+token+"&fold_id_str="+folder;

                
                string html = GetUrl(url);
               
                if (html.Contains("\"0\""))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {

                return false;
            }
        }
        public void del()
        {
            textBox3.Text = "";
            for (int i = 0; i < listView1.CheckedItems.Count; i++)
            {
                string id = listView1.CheckedItems[i].SubItems[1].Text;
               

                textBox3.Text = listView1.CheckedItems[i].SubItems[2].Text + "：删除状态" + deletedocs(id) + "\r\n";
                Thread.Sleep(Convert.ToInt32(textBox4.Text) * 1000);
            }
            MessageBox.Show("删除结束");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
            COOKIE = method.GetCookies("https://wenku.baidu.com/nduc/browse/uc?_page=home&_redirect=1#/mydocsupload");
            Thread thread = new Thread(new ThreadStart(getdocs));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
       

    private void button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(del));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = true;
            }
        }

        private void 取消全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = false;
            }
        }

        private void 百度文库过滤_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                StreamReader streamReader = new StreamReader(this.openFileDialog1.FileName, Encoding.Default);
                string text = streamReader.ReadToEnd();
                string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    textBox8.Text += array[i] + "\r\n";

                }

            }
        }
        #region listview转datable
        /// <summary>
        /// listview转datable
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        public static DataTable listViewToDataTable(ListView lv)
        {
            int i, j;
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Clear();
            dt.Columns.Clear();
            //生成DataTable列头
            for (i = 0; i < lv.Columns.Count; i++)
            {
                dt.Columns.Add(lv.Columns[i].Text.Trim(), typeof(String));
            }
            //每行内容
            for (i = 0; i < lv.CheckedItems.Count; i++)
            {
                dr = dt.NewRow();
                for (j = 0; j < lv.Columns.Count; j++)
                {
                    try
                    {
                        dr[j] = lv.CheckedItems[i].SubItems[j].Text.Trim();

                    }
                    catch
                    {

                        continue;
                    }

                }
                dt.Rows.Add(dr);
            }

            return dt;
        }
        #endregion
        private void button6_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
