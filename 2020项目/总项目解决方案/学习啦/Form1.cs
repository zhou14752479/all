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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            getDoc("http://www.xuexila.com/yundong/yundong/c72193.html");
        }
    }
}
