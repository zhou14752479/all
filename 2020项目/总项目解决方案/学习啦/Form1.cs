using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 学习啦
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        #region 去掉路径中非法字符
        public string removeValid(string illegal)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                illegal = illegal.Replace(c.ToString(), "");
            }
            return illegal;
        }

        #endregion

        string path = AppDomain.CurrentDomain.BaseDirectory+"/data/";
        bool zanting = true;
        #region 爬虫列表页

        public ArrayList getLists(string url)
        {
            ArrayList lists = new ArrayList();
            string html = method.GetUrl(url, "gbk");
            MatchCollection urls = Regex.Matches(html, @"http://www.xuexila.com/[a-z]{2,}/");
            MatchCollection url2s = Regex.Matches(html, @"http://www.xuexila.com/[a-z]{2,}/[a-z]{2,}");
            for (int i = 0; i < urls.Count; i++)
            {
                lists.Add(urls[i].Groups[0].Value);
            }
            for (int i = 0; i < url2s.Count; i++)
            {
                lists.Add(url2s[i].Groups[0].Value);
            }

            return lists;
        }
        #endregion

        #region 列表页匹配文章地址

        public ArrayList getArticleUrls(string url)
        {
            ArrayList lists = new ArrayList();
            string html = method.GetUrl(url, "gbk");
            MatchCollection uids = Regex.Matches(html, @"http://www.xuexila.com.*\d{4,}\.html");
            for (int i = 0; i < uids.Count; i++)
            {
                lists.Add(uids[i].Groups[0].Value);
            }

            return lists;
        }
        #endregion

        #region 下载文章到doc

        public void getDoc(string url,string dic)
        {
           
            string html = method.GetUrl(url, "gbk");
            Match title = Regex.Match(html, @"<title>([\s\S]*?)</title>");
            Match body = Regex.Match(html, @"<div class=""con_main"" id=""contentText"">([\s\S]*?)<div class=""pages"">");
            // string str = body.Groups[1].Value.Replace("</p>","\r\n");
           
            string value = Regex.Replace(body.Groups[1].Value.Replace("a(\"conten\");", ""), "<[^>]+>", ""); //去标签
           
            FileStream fs1 = new FileStream(path + dic+"/"+ removeValid(title.Groups[1].Value)+".docx", FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1);
            sw.WriteLine("                              " + title.Groups[1].Value);
            sw.WriteLine(value);
            sw.Close();
            fs1.Close();

        }
        #endregion

        public bool panduan(string url)
        {
            bool panduan = false;
            StreamReader sr = new StreamReader(path + "wan.txt", Encoding.UTF8);
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == url)
                {
                    panduan = true;
                }
               
            }
            sr.Close();
            return panduan;
        }

        public void baocun(string path, string list)
        {
            FileStream fs1 = new FileStream(path, FileMode.Append, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1);
            sw.WriteLine(list);
            sw.Close();
            fs1.Close();
        }


        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
            try
            {
                ArrayList lists = getLists("http://www.xuexila.com/");
                foreach (string list in lists)
                {
                    if (panduan(list) == false)
                    {
                        textBox1.Text +=DateTime.Now.ToString()+ "正在抓取" + list + "\r\n";
                        Match item = Regex.Match(list, @"com/([\s\S]*?)/");

                        if (!Directory.Exists(path + item.Groups[1].Value))
                        {
                            Directory.CreateDirectory(path + item.Groups[1].Value); //创建文件夹
                        }

                        ArrayList urls = getArticleUrls(list);
                        foreach (string url in urls)
                        {
                            getDoc(url, removeValid(item.Groups[1].Value));
                        }
                        baocun(path + "wan.txt", list);
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                        ArrayList alists = getLists(list);
                        foreach (string alist in alists)
                        {
                            if (panduan(alist) == false)
                            {
                                textBox1.Text += DateTime.Now.ToString() + "正在抓取" + alist + "\r\n";
                                Match aitem = Regex.Match(alist, @"com/([\s\S]*?)/");

                                if (!Directory.Exists(path + aitem.Groups[1].Value))
                                {
                                    Directory.CreateDirectory(path + aitem.Groups[1].Value); //创建文件夹
                                }

                                ArrayList aurls = getArticleUrls(alist);
                                foreach (string aurl in aurls)
                                {
                                    getDoc(aurl, removeValid(aitem.Groups[1].Value));
                                }

                                baocun(path + "wan.txt", alist);
                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                            }
                        }

                    }
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

    

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
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

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "已开始请勿重复点击"+"\r\n";
            
            #region 通用验证

            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php", "utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == "22.22.22.22")
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {

                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;


            }
            else
            {
                MessageBox.Show("IP不符");

            }
            #endregion

        }

        private void ToolStripButton2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void ToolStripButton3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void ToolStripButton4_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要重新开始吗？", "确定", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                System.IO.File.WriteAllText(path + "wan.txt", "", Encoding.UTF8);
                MessageBox.Show("重置成功，请点击开始");
            }
            else
            {
               
            }
            
        }
    }
}
