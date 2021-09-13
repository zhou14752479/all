using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _91porn视频下载
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.inipath);
        }

        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
            return temp.ToString();
        }

        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public bool ExistINIFile()
        {
            return File.Exists(inipath);
        }





        
    public Form1()
        {
            InitializeComponent();
        }

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            string html = "";
            string COOKIE = "CLIPSHARE=6hgbrkadf37ucm9sb1lciu85k6; __utma=63181224.1165478261.1631491585.1631491585.1631491585.1; __utmb=63181224.0.10.1631491585; __utmc=63181224; __utmz=63181224.1631491585.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); covid19=c51cQ%2B6uZlcHf66r9eyq7ihHSiuxVmauLito8JT9;language=cn_CN;";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = "https://91porn.com/view_video_hd.php?viewkey=1c1ab153be74399b3fc2";
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;

                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }

                response.Close();
                return html;



            }
            catch (System.Exception ex)
            {
                return ex.ToString();

            }



        }
        #endregion


        #region 下载文件  【好用】
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="URLAddress">图片地址</param>
        /// <param name="subPath">图片所在文件夹</param>
        /// <param name="name">图片名称</param>
        public static void downloadFile(string URLAddress, string subPath, string name, string COOKIE)
        {
            try
            {
                string path = System.IO.Directory.GetCurrentDirectory();
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                WebClient client = new WebClient();
                client.Proxy = null;
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
                client.Headers.Add("Cookie", COOKIE);
                client.Headers.Add("Referer", "https://m.mm131.net/chemo/89_5.html");
                if (false == System.IO.Directory.Exists(subPath))
                {
                    //创建pic文件夹
                    System.IO.Directory.CreateDirectory(subPath);
                }

                client.DownloadFile(URLAddress, subPath + "\\" + name);
            }
            catch (WebException ex)
            {

               ex.ToString();
            }
        }


        #endregion
        #region 去掉路径中非法字符
        public string removeValid(string illegal)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                illegal = illegal.Replace(c.ToString(), "");
            }

            string str = illegal;
            str = str.Replace("\\", string.Empty);
            str = str.Replace("/", string.Empty);
            str = str.Replace(":", string.Empty);
            str = str.Replace("*", string.Empty);
            str = str.Replace("?", string.Empty);
            str = str.Replace("\"", string.Empty);
            str = str.Replace("<", string.Empty);
            str = str.Replace(">", string.Empty);
            str = str.Replace("|", string.Empty);
            str = str.Replace(" ", string.Empty);    //前面的替换会产生空格,最后将其一并替换掉
            return str;
           
        }

        #endregion
        /// <summary>
        /// 将某一文件夹下的ts文件 合并为一个完整版  BY GJW
        /// </summary>
        /// <param name="nameList">需要合并的视频地址集合</param>
        /// <param name="savePath">保存地址</param>
        /// <param name="fileNmae">合成的文件名</param>
        public static void MergeVideo(List<string> nameList, string savePath, string fileNmae)
        {
            if (nameList.Count == 0 || string.IsNullOrEmpty(savePath)) throw new Exception("文件路径不能为空");
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
            p.Start();//启动程序
            //向cmd窗口发送输入信息
            //拼接命令
            //string cmdstr = string.Format(@"copy /b {0}\*.ts  {1}\{2}_完整版.ts",videoPath,savePath,fileNmae);
            string cmdstr = string.Format(@"copy /b  ""{0}""  ""{1}\{2}""", string.Join(@"""+""", nameList), savePath, fileNmae);

            p.StandardInput.WriteLine(cmdstr + "&exit");
            p.StandardInput.AutoFlush = true;

            //获取cmd窗口的输出信息
            string output = p.StandardOutput.ReadToEnd();

            p.WaitForExit();//等待程序执行完退出进程
            p.Close();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (ExistINIFile())
            {
                textBox1.Text = IniReadValue("values", "time");
                textBox2.Text = IniReadValue("values", "url");
                textBox7.Text = IniReadValue("values", "maxuid");
            }
        }

        public void run()
        {
            string mailurl = textBox2.Text;
            if (!textBox2.Text.Contains("http"))
            {
                mailurl = "https://" + mailurl;
            }
            if (mailurl.Substring(mailurl.Length-1,1)!="/")
            {
                mailurl =  mailurl+"/";
            }

            for (int page = 1; page < 6379; page++)
            {
                string url = mailurl + "v.php?next=watch&page=" + page;
                string html = GetUrl(url, "utf-8");

                MatchCollection uids = Regex.Matches(html, @"id=""playvthumb_([\s\S]*?)""");
                MatchCollection viewkeys = Regex.Matches(html, @"viewkey=([\s\S]*?)&");
                MatchCollection hds = Regex.Matches(html, @"id=""playvthumb_([\s\S]*?)</div>");


                MatchCollection titles = Regex.Matches(html, @"m-t-5"">([\s\S]*?)<");
                MatchCollection authors = Regex.Matches(html, @"作者:</span>([\s\S]*?)<");


                for (int i = 0; i < uids.Count; i++)
                {
                    List<string> list = new List<string>();
                    string uid = uids[i].Groups[1].Value.Trim();
                    string title = removeValid(titles[i].Groups[1].Value.Trim().Replace(" ", ""));
                    string author = authors[i].Groups[1].Value.Trim().Replace(" ", "");
                   
                    string ts_url = "https://fdc.91p49.com//m3u8hd/" + uid + "/" + uid + ".m3u8";

                    if (!hds[i].Groups[1].Value.Contains("HD"))
                    {
                        ts_url = "https://fdc.91p49.com//m3u8/" + uid + "/" + uid + ".m3u8";
                    }

                    string uidini = "";
                    if (ExistINIFile())
                    {
                        uidini = IniReadValue("values", "uids");


                    }
                    if (uidini.Contains(uid))
                    {
                        continue;
                    }


                    string infoUrl = mailurl + "view_video.php?viewkey=" + viewkeys[i].Groups[1].Value.Trim() + "&page=&viewtype=&category=";

                    string infohtml = GetUrl(infoUrl, "utf-8");
                    string info = Regex.Match(infohtml, @"<span class=""more title"">([\s\S]*?)</span>").Groups[1].Value;
                    info = Regex.Replace(info, "<[^>]+>", "").Trim();
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(uid);
                    lv1.SubItems.Add(title);
                    lv1.SubItems.Add(author);
                    lv1.SubItems.Add(info);


                    string path2 = "C:\\" + uid + "\\";
                    if (!System.IO.Directory.Exists("C:\\"))
                    {
                        path2 = "D:\\" + uid + "\\";
                    }


                    string tshtml = GetUrl(ts_url, "utf-8");
                    
                    string[] text = tshtml.Split(new string[] { "\n" }, StringSplitOptions.None);
                    progressBar1.Value = 0;
                    progressBar1.Maximum = text.Length;

                    foreach (var item in text)
                    {
                        double nowvalue = progressBar1.Value;
                        double nowpercent = (nowvalue / progressBar1.Maximum) * 100;
                        nowpercent = Math.Round(nowpercent, 2);
                        label6.Text = "正在下载：" + uid + "------ " + nowpercent + "%";
                        progressBar1.Value = progressBar1.Value + 1;
                        if (item.Contains("ts"))
                        {

                            string tsurl = "https://cdn.91p07.com//m3u8/" + uid + "/" + item.Replace(" ", "").Trim();

                            try
                            {
                                downloadFile(tsurl, path2, item, "");
                            }
                            catch (Exception)
                            {
                                try
                                {
                                    downloadFile(tsurl, path2, item, "");
                                }
                                catch (Exception)
                                {

                                    downloadFile(tsurl, path2, item, "");
                                }

                            }


                            list.Add(path2 + item);
                        }
                    }

                    MergeVideo(list, path, author + "-" + title + ".mp4");
                    Directory.Delete(path2, true);


                    lv1.SubItems.Add("下载成功");
                    IniWriteValue("values", "uids", uidini + "," + uid);
                    label6.Text = uid + "----" + "下载成功";
                    FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\data.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                    sw.WriteLine(author + "-" + title + "-" + info);
                    sw.Close();
                    fs1.Close();
                    sw.Dispose();


                }
            }

        }



        public void run1()
        {
            string mailurl = textBox2.Text;
            if (!textBox2.Text.Contains("http"))
            {
                mailurl = "https://" + mailurl;
            }
            if (mailurl.Substring(mailurl.Length - 1, 1) != "/")
            {
                mailurl = mailurl + "/";
            }

            for (int page = Convert.ToInt32(textBox5.Text); page <= Convert.ToInt32(textBox6.Text); page++)
            {
                string url = mailurl + "v.php?next=watch&page=" + page;
                string html = GetUrl(url, "utf-8");

                MatchCollection uids = Regex.Matches(html, @"id=""playvthumb_([\s\S]*?)""");
                MatchCollection viewkeys = Regex.Matches(html, @"viewkey=([\s\S]*?)&");
                MatchCollection hds = Regex.Matches(html, @"id=""playvthumb_([\s\S]*?)</div>");


                MatchCollection titles = Regex.Matches(html, @"m-t-5"">([\s\S]*?)<");
                MatchCollection authors = Regex.Matches(html, @"作者:</span>([\s\S]*?)<");


                for (int i = 0; i < uids.Count; i++)
                {
                    string uid = uids[i].Groups[1].Value.Trim();

                    if (radioButton1.Checked == true && checkBox1.Checked == true)
                    {
                        if (textBox7.Text == "")
                        {
                            MessageBox.Show("未输入ID");
                            return;
                        }
                        else
                        {
                            if (Convert.ToInt32(uid) < Convert.ToInt32(textBox7.Text))
                            {
                                label6.Text = "UID不符合跳过：" + uid + ".......";
                                continue;
                            }
                        }
                    }



                    List<string> list = new List<string>();
                   
                    string title = removeValid(titles[i].Groups[1].Value.Trim().Replace(" ", ""));
                    string author = authors[i].Groups[1].Value.Trim().Replace(" ", "");

                    string mp4url = "https://fdc.91p49.com//mp4hd/" + uid+".mp4";

                    if (!hds[i].Groups[1].Value.Contains("HD"))
                    {
                        mp4url= "https://fdc.91p49.com//mp43/" + uid + ".mp4";
                    }

                    string uidini = "";
                    if (ExistINIFile())
                    {
                        uidini = IniReadValue("values", "uids");


                    }
                    if (uidini.Contains(uid))
                    {
                        continue;
                    }


                    string infoUrl = mailurl + "view_video.php?viewkey=" + viewkeys[i].Groups[1].Value.Trim() + "&page=&viewtype=&category=";

                    string infohtml = GetUrl(infoUrl, "utf-8");
                    string info = Regex.Match(infohtml, @"<span class=""more title"">([\s\S]*?)</span>").Groups[1].Value;
                    info = Regex.Replace(info, "<[^>]+>", "").Trim();
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(uid);
                    lv1.SubItems.Add(title);
                    lv1.SubItems.Add(author);
                    lv1.SubItems.Add(info);




                    label6.Text = "正在下载：" + uid + ".......";

                    downloadFile(mp4url, path, uid+"-"+author + "-" + title + ".mp4", "");



                    lv1.SubItems.Add("下载成功");
                    IniWriteValue("values", "uids", uidini + "," + uid);
                    label6.Text = uid + "----" + "下载成功";
                    FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\data.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                    sw.WriteLine(author + "-" + title + "-" + info);
                    sw.Close();
                    fs1.Close();
                    sw.Dispose();


                }
            }

        }
        Thread thread;
        string path = AppDomain.CurrentDomain.BaseDirectory+"mp4\\";
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"Wigbkf"))
            {

                return;
            }



            #endregion

            if(radioButton1.Checked==true)
            {
                timer1.Start();
                timer1.Interval = Convert.ToInt32(textBox1.Text) * 1000 * 60;
            }
            if (radioButton2.Checked == true)
            {
                timer1.Stop();
            }

            IniWriteValue("values", "time", textBox1.Text.ToString());
            IniWriteValue("values", "url", textBox2.Text.ToString());
            IniWriteValue("values", "maxuid", textBox7.Text.ToString());


            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run1);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run1);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }
    }
}
