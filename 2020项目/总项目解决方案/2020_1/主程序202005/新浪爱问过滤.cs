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

namespace 主程序202005
{
    public partial class 新浪爱问过滤 : Form
    {
        public 新浪爱问过滤()
        {
            InitializeComponent();
        }

        private void 新浪爱问过滤_Load(object sender, EventArgs e)
        {
            webBrowser1.Url = new Uri("http://ishare.iask.sina.com.cn/");
            webBrowser1.ScriptErrorsSuppressed = true;
          //  webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(getTitle);
        }
        //private void getTitle(object sender, EventArgs e)
        //{

        //    System.IO.StreamReader getReader = new System.IO.StreamReader(this.webBrowser1.DocumentStream, System.Text.Encoding.GetEncoding("utf-8"));

        //    string html = getReader.ReadToEnd();


        //    if (html.Contains("touxiang"))
        //    {
        //        label4.Text = "登陆成功";
        //        return;
        //    }
        //    else
        //    {
        //        label4.Text = "登陆失败";
        //    }


        //}
        static string COOKIE = "SINAGLOBAL=121.234.247.218_1589341933.594065; UOR=www.baidu.com,news.sina.com.cn,; UM_distinctid=17220896f43221-052b0ab1620879-6373664-1fa400-17220896f44b13; __gads=ID=0fb2334157161892:T=1589683778:S=ALNI_Mac_yK3DS06bkrqUly0SUfAn8CIcQ; lxlrttp=1578733570; FSINAGLOBAL=121.234.247.218_1589341933.594065; ULV=1589716192314:3:3:3::1589683787262; Hm_lvt_ad29670c49e093f8aa6cbb0f672c1a81=1589802911; Hm_lvt_e81d7f51b76a84681a000c9162b4d13e=1589802911; Hm_lvt_08ef5961d6a494d8186b807e6509a52e=1589802912; Hm_lvt_a1a86043f4a1a06bbaab7086fb8b8423=1589802912; Hm_lvt_f78f25b92eebf21f3f6fcb3e35be1516=1589802912; ALF=1621387502; cid=15898871478100.03825662234755933; pgv_pvid=6909970046; gr_user_id=e5895e5e-422b-4ea2-be1f-0cbe349fadea; grwng_uid=b0260899-7209-463b-a9a0-00be5bc33335; _dpcid=15898871493870.4920147713954597; Hm_lvt_17cdd3f409f282dc0eeb3785fcf78a66=1589887156; cuk=cb6acad7689ed6234d0dbba2ebca3c2900f96220cdbf93cdb3c42a25878c0d30; lsdt=1560653375695; 9f367e704cdcd7bc_gr_last_sent_cs1=5abc4227e4b08fd141b4587f; l_u_id=5abc4227e4b08fd141b4587f; loginDomain=.sina.com.cn; terminalType=1; __qc_wId=445; 9f367e704cdcd7bc_gr_session_id=ccaf00c3-9826-4f86-9068-49d117665f7b; 9f367e704cdcd7bc_gr_last_sent_sid_with_cs1=ccaf00c3-9826-4f86-9068-49d117665f7b; 9f367e704cdcd7bc_gr_session_id_ccaf00c3-9826-4f86-9068-49d117665f7b=true; Qs_lvt_335601=1589887148%2C1589887584%2C1589942234; Hm_lvt_adb0f091db00ed439bf000f2c5cbaee7=1589887150,1589887585,1589942236; Hm_lvt_17cdd3f409f282dc0eeb3785fcf78a66=1589887156; JSESSIONID=08dac023-e275-47fa-8f76-87587698ed58; CNZZDATA1257780960=1471744885-1589881814-https%253A%252F%252Fishare.iask.sina.com.cn%252F%7C1589938231; Qs_pv_335601=1367208329437079300%2C1536886990981022500%2C810151279711193000%2C53886457114336536%2C2990549266834841000; CNZZDATA1257407067=193344473-1589887150-%7C1589942434; Hm_lpvt_17cdd3f409f282dc0eeb3785fcf78a66=1589942434; Hm_lpvt_adb0f091db00ed439bf000f2c5cbaee7=1589942434; 9f367e704cdcd7bc_gr_cs1=5abc4227e4b08fd141b4587f";
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

        public void getdocs()
        {

            try
            {
                for (int i = Convert.ToInt32(textBox1.Text.Trim()); i <= Convert.ToInt32(textBox2.Text.Trim()); i++)
                {
                    textBox3.Text += "正在筛选第" + i + "页" + "\r\n";
                    string url = "https://ishare.iask.sina.com.cn/ucenter/myuploads?subPage=a&pindex=" + i ;

                    string html = GetUrl(url);

                    MatchCollection ids = Regex.Matches(html, @"""fedit_id"" value=""([\s\S]*?)""");
                    MatchCollection uids = Regex.Matches(html, @"""fedit_uid"" value=""([\s\S]*?)""");
                    MatchCollection titles = Regex.Matches(html, @"""fedit_title"" value=""([\s\S]*?)""");

                    for (int j = 0; j < ids.Count; j++)
                    {
                        if (panduan(titles[j].Groups[1].Value))
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                            lv1.SubItems.Add(ids[j].Groups[1].Value);
                            lv1.SubItems.Add(uids[j].Groups[1].Value);
                            lv1.SubItems.Add(titles[j].Groups[1].Value);
                            lv1.SubItems.Add("--");
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
        public bool deletedocs(string id,string uid)
        {

            try
            {
                string url = "https://ishare.iask.sina.com.cn/ucenter/delFiles?fids="+id+"&showflag=y&uid="+uid;

                string html = GetUrl(url);
                if (html.Contains("1"))
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
                string uid = listView1.CheckedItems[i].SubItems[2].Text;

                textBox3.Text += id + "：删除状态" + deletedocs(id,uid) + "\r\n";
                Thread.Sleep(Convert.ToInt32(textBox4.Text) * 1000);
            }
            MessageBox.Show("删除结束");
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

        private void button1_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
           COOKIE = method.GetCookies("https://ishare.iask.sina.com.cn/ucenter/myuploads");
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

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 新浪爱问过滤_FormClosing(object sender, FormClosingEventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            //防止弹窗；
            e.Cancel = true;
            string url = this.webBrowser1.StatusText;
            this.webBrowser1.Url = new Uri(url);

        }
    }
}
