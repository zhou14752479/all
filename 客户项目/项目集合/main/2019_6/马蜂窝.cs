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

namespace main._2019_6
{
    public partial class 马蜂窝 : Form
    {
        public 马蜂窝()
        {
            InitializeComponent();
        }

        private void 马蜂窝_Load(object sender, EventArgs e)
        {

        }
        public static string CleanHtml(string strHtml)
        {
            if (string.IsNullOrEmpty(strHtml)) return strHtml;
            //删除脚本
            //Regex.Replace(strHtml, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase)
            strHtml = Regex.Replace(strHtml, @"(\<script(.+?)\</script\>)|(\<style(.+?)\</style\>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            //删除标签
            var r = new Regex(@"</?[^>]*>", RegexOptions.IgnoreCase);
            Match m;
            for (m = r.Match(strHtml); m.Success; m = m.NextMatch())
            {
                strHtml = strHtml.Replace(m.Groups[0].ToString(), "");
            }
            return strHtml.Trim();
        }
        bool status = true;
        bool zanting = true;
        #region 游记采集
        public void run()
        {

            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                StreamReader sr = new StreamReader(textBox1.Text, Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {
                    if (!Directory.Exists(path+text[i]))
                    {
                        Directory.CreateDirectory(path + text[i]); //创建文件夹
                    }
                    string Url = "http://www.mafengwo.cn/i/"+text[i]+".html";

                    string html = method.gethtml(Url, "", "utf-8");
                    if (html == null)
                        break;
                    Match title = Regex.Match(html, @"lh80"">([\s\S]*?)</h1>");
                    MatchCollection bodys = Regex.Matches(html, @"<p class=""_j_note_content _j_seqitem"" data-seq=""([\s\S]*?)"">([\s\S]*?)</p>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    Match pic = Regex.Match(html, @"data-src=""([\s\S]*?)""");
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据    
                    StringBuilder sb = new StringBuilder();
                    for (int j = 0; j < bodys.Count; j++)
                    {
                        
                        sb.Append(Regex.Replace(bodys[j].Groups[2].Value, "<.*?>", "").Trim());
                    }

                    lv1.SubItems.Add(title.Groups[1].Value);
                    lv1.SubItems.Add(sb.ToString());
                    method.downloadFile(pic.Groups[1].Value, path + text[i], title.Groups[1].Value+".jpg");

                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }
                        Thread.Sleep(500);

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }


                        if (this.status == false)
                            return;

                    



                }
            }

            catch (System.Exception ex)
            {

                ex.ToString();
            }

        }

        #endregion

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
