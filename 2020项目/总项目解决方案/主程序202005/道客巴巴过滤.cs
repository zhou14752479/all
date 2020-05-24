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
    public partial class 道客巴巴过滤 : Form
    {
        public 道客巴巴过滤()
        {
            InitializeComponent();
        }

         static string  COOKIE = "PHPSESSID=j2075ts5m5hqra9u0ja756mn77; cdb_sys_sid=j2075ts5m5hqra9u0ja756mn77; ShowSkinTip_1=1; cdb_back[login]=1; cdb_logined=1; cdb_change_message=1; cdb_back[u]=1; show_index=1; cdb_back[classify_id]=all; cdb_back[folder_id]=0; cdb_back[show_index]=1; cdb_RW_ID_1159859913=0; cdb_READED_PC_ID=%2C; cdb_H5R=1; showAnnotateTipIf=1; Page_9833935135590=1; cdb_back[n]=6; cdb_back[doctype]=0; cdb_RW_ID_1159859228=0; Page_6781683583995=1; cdb_back[pid]=9562968363388; cdb_RW_ID_1159858899=0; Page_9562968363388=1; cdb_back[p_name]=3%E3%80%80%E6%8E%A2%E6%B5%8B%E5%B0%84%E7%BA%BF%E7%9A%84%E6%96%B9%E6%B3%95; cdb_back[rel_p_id]=1159858899; cdb_back[srlid]=7e40lR9LzJYkn1bje2xio3uf2kXumzoo09MVU8OQ9wYUKV5CJUEudmsodv+ZOMx%2F4RlvBxeNmtUkM0cvsZLosDaQ0q75TEjjXOCvaXCUAg; cdb_back[pcode]=9562968363388; cdb_back[dlrand]=yTprdEEq; cdb_back[type]=1; cdb_back[order]=all; cdb_back[total]=all; menu=0; siftState=1; cdb_back[username]=zhou14752479; cdb_login_if=1; cdb_back[captchaCode]=1; cdb_back[t]=1; cdb_back[task_type]=1; cdb_back[e_class]=upload; cdb_back[title]=%E8%BD%AF%E4%BB%B6%E5%88%B6%E4%BD%9C; cdb_back[intro]=%E8%BD%AF%E4%BB%B6%E5%88%B6%E4%BD%9C; cdb_back[pcid]=8220; cdb_back[sharetodoc]=1; cdb_back[download]=2; cdb_back[p_price]=1500; cdb_back[p_default_points]=1; cdb_back[p_doc_format]=DOC; cdb_back[doccode]=1228688680; cdb_back[p_pagecount]=1; cdb_back[p_save]=undefined; cdb_back[list_num]=small; cdb_back[id]=1228688680; cdb_back[txtloginname]=mm3986574; cdb_back[txtPassword]=9981689mm; cdb_uid=30948364; c_login_name=mm3986574; cdb_back[image_type]=3; cdb_msg_num=5; cdb_back[module_type]=1; cdb_back[p_id]=1209901254; cdb_back[state]=myshare; cdb_back[menuIndex]=4; cdb_back[mid]=mm3986574; cdb_msg_time=1589678951; cdb_back[member_id]=30948364; cdb_back[show]=1; cdb_back[order_num]=1; cdb_back[folder_page]=0; cdb_back[act]=ajax_doc_list; cdb_pageType=2; cdb_back[curpage]=3";

       

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

                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("appid:orders");
                //headers.Add("x-nike-visitid:5");
                //headers.Add("x-nike-visitorid:d03393ee-e42c-463e-9235-3ca0491475b4");
                //添加头部
               
                request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
                //request.Headers.Add("origin","https://www.nike.com");
                request.Referer = "https://www.doc88.com/uc/doc_manager.php?act=doc_list&state=myshare";
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

      
        string menuindex = "4";
        string classifyid = "all";
        public void getdocs()
        {

            try
            {
                if (radioButton1.Checked == true)
                {
                    menuindex = "4";
                    classifyid = "all";

                }

                if (radioButton2.Checked == true)
                {
                    menuindex = "3";
                    classifyid = "private";

                }



                for (int i = Convert.ToInt32(textBox1.Text.Trim()); i <=Convert.ToInt32(textBox2.Text.Trim()); i++)
                {
                    textBox3.Text += "正在筛选第"+i+"页"+"\r\n";
                    string url = "https://www.doc88.com/uc/doc_manager.php?act=ajax_doc_list&curpage=" + i;
                    string postdata = "menuIndex="+menuindex+"&classify_id="+classifyid+"&folder_id=0&sort=&keyword=&show_index=1";
                   string html= PostUrl(url,postdata);
                   MatchCollection aids = Regex.Matches(html, @"<a href=""\/p-([\s\S]*?)\.");
                    MatchCollection uids = Regex.Matches(html, @"deleteDoc\('([\s\S]*?)'");
                    MatchCollection titles = Regex.Matches(html, @"class=""title"" title=""([\s\S]*?)""");
                    for (int j = 0; j < aids.Count; j++)
                    {
                        try
                        {
                            if (panduan(titles[j].Groups[1].Value))
                            {
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                            lv1.SubItems.Add(aids[j].Groups[1].Value);
                            lv1.SubItems.Add(uids[j].Groups[1].Value);
                            lv1.SubItems.Add(titles[j].Groups[1].Value);
                            lv1.SubItems.Add("--");
                            }
                        }
                        catch 
                        {

                            continue;
                        }
                       
                    }
                }

                MessageBox.Show("筛选结束");

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString()) ;
            }
        }


        public bool deletedocs(string id)
        {

            try
            {
                string url = "https://www.doc88.com/uc/index.php?act=del_doc";
                string postdata = "id=" + id;
                string html = PostUrl(url, postdata);
                if (html.Trim() == "1")
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
        private void 道客巴巴过滤_Load(object sender, EventArgs e)
        {
            webBrowser1.Url = new Uri("https://www.doc88.com/member.php?act=login");
           // webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(getTitle);
        }

        private void getTitle(object sender, EventArgs e)
        {

            System.IO.StreamReader getReader = new System.IO.StreamReader(this.webBrowser1.DocumentStream, System.Text.Encoding.GetEncoding("utf-8"));

            string html = getReader.ReadToEnd();

            Match count = Regex.Match(html, @"doc_count"">([\s\S]*?)</a>");
            
            if (html.Contains("个人中心"))
            {
                label4.Text =( Convert.ToInt32(count.Groups[1].Value.Trim())/15).ToString();
                button6.Text = "登陆成功";
                return;
            }
            else
            {
                button6.Text = "登陆失败";
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"doc88"))
            {
                textBox3.Text = "";
                COOKIE = method.GetCookies("https://www.doc88.com/uc/doc_manager.php?act=doc_list&state=all");
                Thread thread = new Thread(new ThreadStart(getdocs));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
           
        }

        public void del()
        {
            textBox3.Text = "";
            for (int i = 0; i < listView1.CheckedItems.Count; i++)
            {
                string uid = listView1.CheckedItems[i].SubItems[2].Text;

                textBox3.Text += uid + "：删除状态" + deletedocs(uid) + "\r\n";
                Thread.Sleep(Convert.ToInt32(textBox4.Text) * 1000);
            }
            MessageBox.Show("删除结束");
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Thread thread = new Thread(new ThreadStart(del));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

            //System.Diagnostics.Process.Start(listView1.SelectedItems[0].SubItems[5].Text);


        }

        private void 道客巴巴过滤_FormClosing(object sender, FormClosingEventArgs e)
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

        private void button6_Click(object sender, EventArgs e)
        {
            HtmlDocument dc = webBrowser1.Document;
            HtmlElementCollection es = dc.GetElementsByTagName("input");   //GetElementsByTagName返回集合
            foreach (HtmlElement e1 in es)
            {
                if (e1.GetAttribute("name") == "txtloginname")
                {
                    e1.SetAttribute("value", textBox6.Text.Trim());
                }
                if (e1.GetAttribute("name") == "txtPassword")
                {
                    e1.SetAttribute("value", textBox5.Text.Trim());
                }
            }

            //点击登陆

            HtmlElementCollection es2 = dc.GetElementsByTagName("button");   //GetElementsByTagName返回集合
            foreach (HtmlElement e1 in es2)
            {
                if (e1.GetAttribute("id") == "login-btn")
                {
                    e1.InvokeMember("click");
                }

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
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

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = true ;
            }
        }

        private void 取消全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = false;
            }
        }
    }
}
