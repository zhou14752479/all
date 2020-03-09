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

namespace 通用项目
{
    public partial class 一师一课 : Form
    {
        public 一师一课()
        {
            InitializeComponent();
        }

        private void 一师一课_Load(object sender, EventArgs e)
        {

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
       
        string cookie = "";
        #endregion
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {


            for (int i = Convert.ToInt32(textBox3.Text.Trim()); i < Convert.ToInt32(textBox4.Text.Trim()); i++)
            {
                try
                {

                    FileStream fs1 = new FileStream(path + "config.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1);
                    sw.WriteLine(i.ToString());
                    sw.Close();
                    fs1.Close();


                    string url = "http://yj.eduyun.cn/jiaosiziyuan/jiaoxueziyuan/index_"+i+".html";
                    string html = method.GetUrl2(url, "utf-8");

                    MatchCollection IDS = Regex.Matches(html, @"dd><a href=""([\s\S]*?)""");
                    foreach (Match aurl in IDS)
                    {

                      
                        string ahtml = method.GetUrl2(aurl.Groups[1].Value, "utf-8");
                        Match title = Regex.Match(ahtml, @"<title>([\s\S]*?)</title>");

                        Match down = Regex.Match(ahtml, @"#0066cc([\s\S]*?)href=""([\s\S]*?)""");
                        string bt = title.Groups[1].Value;
                        if (down.Groups[2].Value != "")
                        {
                            string downUrl = "http://yj.eduyun.cn" + down.Groups[2].Value;

                            Match gs1 = Regex.Match(ahtml, @"icon_([\s\S]*?)\.");
                            string gs = gs1.Groups[1].Value;
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
                catch
                {

                    continue;
                }
            }



        }


        ArrayList geshiList = new ArrayList();

        private void Button1_Click(object sender, EventArgs e)
        {
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

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            zanting = true;
        }
    }
}
