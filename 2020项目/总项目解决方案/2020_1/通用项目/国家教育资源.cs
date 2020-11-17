using System;
using System.Collections;
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
         string cookie= "UM_distinctid=1722b6d63256fa-0d957a4cc62be8-6373664-1fa400-1722b6d6326fc; c846d1371fa646f180641dc334a81239=WyI2MDE0ODY4MTQiXQ; ck=a8f77499cd06af5b23c3b1432d54b2991591697419; CNZZDATA5916476=cnzz_eid%3D1513833111-1591696647-%26ntime%3D1591696647; rpk_dc0ba9777c06d718b1a0e8afc6822986=aFFOVg2V9%2BxTMlDrRmoq%2BbUVfnWOe9FJMEJCCnGzm92mrgXqXQA; eduyun_sessionid=3b8c3a9b-00b5-4950-9b42-c14baa810212; eduyun_qpsflag=1; PHPSESSID=ko7d3375p2rstm6ev7q0than74; remember_account=emhvdTE0NzUyNDc5; remember_keys=WmhvdWthaWdlMDA%3D; pullTime=300";
        #endregion

        #region GET请求解决基础连接关闭无法获取HTML
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public  string GetUrl(string Url, string charset)
        {
            string outStr = "";
            string tmpStr = "";

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
               
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://n.eduyun.cn/index.php?r=center/person/index";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie",cookie);
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                try
                {//循环获取
                    while ((tmpStr = reader.ReadLine()) != null)
                    {
                        outStr += tmpStr;
                    }
                }
                catch
                {

                }
                reader.Close();
                response.Close();

                return outStr;

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return ex.ToString();

            }

        }
        #endregion
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {


            for (long i = Convert.ToInt64(textBox3.Text.Trim()); i < Convert.ToInt64(textBox4.Text.Trim()); i++)
            {
                try
                {

                    FileStream fs1 = new FileStream(path + "config.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1);
                    sw.WriteLine(i.ToString());
                    sw.Close();
                    fs1.Close();


                    string url = "http://1s1k.eduyun.cn/resource/resource/RedesignCaseView/showVersion1s1k.jspx?type=all&date=1568624403391&pageNo="+i+"&yearMark="+year+"&sessionKey=yVGaPfx0gjHEalQ9JOWr";

                   
                    string html = GetUrl(url, "utf-8");
                  
                    MatchCollection IDS = Regex.Matches(html, @"CASE_ID=([\s\S]*?),");
                    foreach (Match ID in IDS)
                    {

                        string aurl = "http://1s1k.eduyun.cn/resource/resource/RedesignCaseView/viewCaseBbs1s1k.jspx?date=1581939217784&code=-1&sdResIdCaseId=" + ID.Groups[1].Value + "&flags=&guideId=&sk=&sessionKey=OL9YmN5uZwuHbYhX9ZYL";
                        string ahtml = GetUrl(aurl, "utf-8");
                        Match title = Regex.Match(ahtml, @"<h1>([\s\S]*?)</h1>");
                        // Match zuohe = Regex.Match(ahtml, @"<dt>([\s\S]*?)</dt>");


                        MatchCollection DocIds1 = Regex.Matches(ahtml, @"class=""docCode"" value=""doc-([\s\S]*?)""");
                        MatchCollection DocIds2 = Regex.Matches(ahtml, @"resId=doc-([\s\S]*?)&([\s\S]*?)>([\s\S]*?)<");
                       
                        foreach (Match DocId in DocIds1)
                        {

                            string bt = title.Groups[1].Value;
                            string downUrl = GetUrl("http://1s1k.eduyun.cn/resource/resource/RedesignCaseView/getDownUrlByCode.jspx?code=doc-" + DocId.Groups[1].Value + "&resId=doc-" + DocId.Groups[1].Value + "&date=1581934769915", "utf-8");


                           
                            Match gs1 = Regex.Match(downUrl, @"download([\s\S]*?)\.([\s\S]*?)\?");
                            string gs = gs1.Groups[2].Value;
                            string counter = "1";

                            string dizhi = path + "下载文件\\" + removeValid(bt) + counter + "." + gs;
                            while (System.IO.File.Exists(dizhi))
                            {
                                counter = (Convert.ToInt32(counter) + 1).ToString();
                                dizhi = path + "下载文件\\" + removeValid(bt) + counter + "." + gs;
                            }
                            
                            method.downloadFile(downUrl, path + "下载文件\\", removeValid(bt) + counter + "." + gs, cookie);
                            
                            textBox1.Text += DateTime.Now.ToString() + i + "下载成功：" + bt + "\r\n";

                        }
                        foreach (Match DocId in DocIds2)
                        {

                            string bt = DocId.Groups[3].Value;
                            string downUrl = GetUrl("http://1s1k.eduyun.cn/resource/resource/RedesignCaseView/getDownUrlByCode.jspx?code=doc-" + DocId.Groups[1].Value + "&resId=doc-" + DocId.Groups[1].Value + "&date=1581934769915", "utf-8");
                            Match gs1 = Regex.Match(downUrl, @"download([\s\S]*?)\.([\s\S]*?)\?");
                            string gs = gs1.Groups[2].Value;
                            if (geshiList.Contains(gs))
                            {
                                method.downloadFile(downUrl, path + "下载文件\\", removeValid(bt) + "." + gs, cookie);

                                textBox1.Text += DateTime.Now.ToString() + i + "下载成功：" + bt + "\r\n";
                            }
                        }
                        while (zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }


                        if (status == false)
                        {
                            return;
                        }

                        Thread.Sleep(Convert.ToInt32(textBox2.Text) * 1000);





                    }

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                   // continue;
                }
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
        ArrayList geshiList = new ArrayList();
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

           
            if (checkBox1.Checked == true)
            {
                geshiList.Add("doc");
            }
            if (checkBox2.Checked == true)
            {
                geshiList.Add("docx");
            }
            if (checkBox3.Checked == true)
            {
                geshiList.Add("ppt");
            }
            if (checkBox4.Checked == true)
            {
                geshiList.Add("pptx");
            }
            if (checkBox5.Checked == true)
            {
                geshiList.Add("pdf");
            }
            if (checkBox6.Checked == true)
            {
                geshiList.Add("txt");
            }
            if (checkBox7.Checked == true)
            {
                geshiList.Add("rar");
            }
            if (checkBox8.Checked == true)
            {
                geshiList.Add("zip");
            }


            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"guojiajiaoyu"))
            {
                button1.Enabled = false;
                status = true;
                Thread thread = new Thread(new ThreadStart(run));
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
