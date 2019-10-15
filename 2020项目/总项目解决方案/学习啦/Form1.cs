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
        string path = AppDomain.CurrentDomain.BaseDirectory+"/data/";
        #region 爬虫列表页

        public ArrayList getLists(string url)
        {
            ArrayList lists = new ArrayList();
            string html = method.GetUrl(url, "gbk");
            MatchCollection urls = Regex.Matches(html, @"http://www.xuexila.com/[a-z]{2,}/");
            for (int i = 0; i < urls.Count; i++)
            {
                lists.Add(urls[i].Groups[0].Value);
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

        public void getDoc(string url)
        {
           
            string html = method.GetUrl(url, "gbk");
            Match title = Regex.Match(html, @"<title>([\s\S]*?)</title>");
            Match body = Regex.Match(html, @"<div class=""con_main"" id=""contentText"">([\s\S]*?)<div class=""pages"">");
            // string str = body.Groups[1].Value.Replace("</p>","\r\n");
           
            string value = Regex.Replace(body.Groups[1].Value.Replace("a(\"conten\");", ""), "<[^>]+>", ""); //去标签
            textBox1.Text = value;
            string path = AppDomain.CurrentDomain.BaseDirectory;
            FileStream fs1 = new FileStream(path + title.Groups[1].Value+".docx", FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1);
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
            return panduan;
        }


        public void run()
        {
            ArrayList lists = getLists("http://www.xuexila.com/");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < lists.Count; i++)
            {
                sb.Append(lists[i]+"\r\n");
            }
            System.IO.File.WriteAllText(path+"wan.txt", sb.ToString(), Encoding.UTF8);

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //run();
          bool a=  panduan("http://www.xuexila.com/hob/");

            MessageBox.Show(a.ToString());
        }
    }
}
