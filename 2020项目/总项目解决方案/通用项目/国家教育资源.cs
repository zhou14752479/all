using System;
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

namespace 通用项目
{
    public partial class 国家教育资源 : Form
    {
        public 国家教育资源()
        {
            InitializeComponent();
        }
        
        string path = AppDomain.CurrentDomain.BaseDirectory;
        
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
        bool zanting = true;
        bool status = true;
        string year = "5";
        string cookie = "";
        #endregion
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {

            try
            {
                
                for (int i = Convert.ToInt32(textBox3.Text.Trim()); i < Convert.ToInt32(textBox4.Text.Trim()); i++)
                {
                    FileStream fs1 = new FileStream(path + "config.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1);
                    sw.WriteLine(i.ToString());
                    sw.Close();
                    fs1.Close();

                    
                    string url = "http://1s1k.eduyun.cn/resource/resource/RedesignCaseView/showVersion1s1k.jspx?type=all&date=1568624403391&pageNo=" + i + "&yearMark="+year+"&sessionKey=PKSpcCwnXGPjaVHtZZ2D=";
                    string html = method.GetUrl2(url, "utf-8");
                  
                    MatchCollection IDS = Regex.Matches(html, @"CASE_ID=([\s\S]*?),");
                    foreach (Match ID in IDS)
                    {

                        string aurl = "http://1s1k.eduyun.cn/resource/resource/RedesignCaseView/viewCaseBbs1s1k.jspx?date=1581939217784&code=-1&sdResIdCaseId=" + ID.Groups[1].Value + "&flags=&guideId=&sk=&sessionKey=OL9YmN5uZwuHbYhX9ZYL";
                        string ahtml = method.GetUrl2(aurl, "utf-8");
                        Match title = Regex.Match(ahtml, @"<h1>([\s\S]*?)</h1>");
                       // Match zuohe = Regex.Match(ahtml, @"<dt>([\s\S]*?)</dt>");


                        MatchCollection DocIds1 = Regex.Matches(ahtml, @"class=""docCode"" value=""doc-([\s\S]*?)""");
                        MatchCollection DocIds2 = Regex.Matches(ahtml, @"resId=doc-([\s\S]*?)&([\s\S]*?)>([\s\S]*?)<");
                        
                        foreach (Match DocId in DocIds1)
                        {
                         
                            string bt = title.Groups[1].Value ;
                            string downUrl = method.GetUrl2("http://1s1k.eduyun.cn/resource/resource/RedesignCaseView/getDownUrlByCode.jspx?code=doc-" + DocId.Groups[1].Value + "&resId=doc-" + DocId.Groups[1].Value + "&date=1581934769915", "utf-8");

                            Match gs1 = Regex.Match(downUrl, @"download([\s\S]*?)\.([\s\S]*?)\?");
                            string gs = gs1.Groups[2].Value;

                             method.downloadFile(downUrl, path + "下载文件\\", removeValid(bt) + "." + gs, cookie);

                            textBox1.Text += DateTime.Now.ToString() + i + "下载成功：" + bt + "\r\n";
                           
                        }
                        foreach (Match DocId in DocIds2)
                        {
                            
                            string bt = DocId.Groups[3].Value ;
                            string downUrl = method.GetUrl("http://1s1k.eduyun.cn/resource/resource/RedesignCaseView/getDownUrlByCode.jspx?code=doc-" + DocId.Groups[1].Value + "&resId=doc-" + DocId.Groups[1].Value + "&date=1581934769915", "utf-8");
                            Match gs1 = Regex.Match(downUrl, @"download([\s\S]*?)\.([\s\S]*?)\?");
                            string gs = gs1.Groups[2].Value;

                             method.downloadFile(downUrl, path + "下载文件\\", removeValid(bt) + "." + gs, cookie);

                            textBox1.Text += DateTime.Now.ToString() + i + "下载成功：" + bt + "\r\n";
                          
                        }
                        while (zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                        
                        if (status == false)
                        {
                            return;
                        }

                        Thread.Sleep(Convert.ToInt32(textBox2.Text)*1000);


                        


                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


        }

        private void 国家教育资源_Load(object sender, EventArgs e)
        {
            webBrowser1.Url = new Uri("http://n.eduyun.cn/index.php?r=portal/site/index");

            foreach (Control ctr in panel1.Controls)
            {

                if (ctr is TextBox)
                {

                    string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
                    if (File.Exists(path + ctr.Name + ".txt"))
                    {

                        StreamReader sr = new StreamReader(path + ctr.Name + ".txt", Encoding.GetEncoding("utf-8"));
                        //一次性读取完 
                        string texts = sr.ReadToEnd();
                        ctr.Text = texts;
                        sr.Close();
                    }
                }
            }

            if (File.Exists(path + "config.txt"))
            {

                StreamReader sr = new StreamReader(path + "config.txt", Encoding.GetEncoding("utf-8"));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                sr.Close();

                textBox3.Text = texts.Trim();

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (comboBox1.Text)
            {
                case "2014年度":
                    year = "1";
                    break;
                case "2015-2016年度":
                    year = "2";
                    break;
                case "2016-2017年度":
                    year = "3";
                    break;
                case "2018年度":
                    year = "4";
                    break;
                case "2019年度":
                    year = "5";
                    break;

            }

            button1.Enabled = false;
            status = true;
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {


            button1.Enabled = true;
            status = false;
        }

        private void 国家教育资源_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确认退出吗？", "退出询问"
           , MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK)
            {
                e.Cancel = true;//告诉窗体关闭这个任务取消

            }
            else
            {
                foreach (Control ctr in panel1.Controls)
                {
                    if (ctr is TextBox)
                    {


                        string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
                        FileStream fs1 = new FileStream(path + ctr.Name + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                        StreamWriter sw = new StreamWriter(fs1);
                        sw.WriteLine(ctr.Text.Trim());
                        sw.Close();
                        fs1.Close();

                    }
                }

             

                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.Text)
            {
                case "2014年度":
                    label7.Text = "4760";
                    break;
                case "2015-2016年度":
                    label7.Text = "432406";
                    break;
                case "2016-2017年度":
                    label7.Text = "559779";
                    break;
                case "2018年度":
                    label7.Text = "400062";
                    break;
                case "2019年度":
                    label7.Text = "310268";
                    break;

            }
        }
    }
}
