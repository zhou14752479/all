using System;
using System.Collections;
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
using Microsoft.VisualBasic;
using myDLL;

namespace 主程序202006
{
    public partial class 飞机票信息记录 : Form
    {
        public 飞机票信息记录()
        {
            InitializeComponent();
        }

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

        int yijing = 0;
        int hege = 0;
        ArrayList finish = new ArrayList();

        ArrayList lists = new ArrayList();

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
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                //headers.Add("sec-fetch-site:same-origin");
                //headers.Add("sec-fetch-user:?1");
                //headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                // request.ContentType = "application/x-www-form-urlencoded";
               
                request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                // request.ContentLength = postData.Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", "");

                request.Referer = "";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");

                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding("utf-8"));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }


                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {

            long min = Convert.ToInt64(textBox1.Text.Replace("\r\n", "").Trim());
            long max = Convert.ToInt64(textBox1.Text.Replace("\r\n", "").Trim()) +  Convert.ToInt64(textBox2.Text.Replace("\r\n", "").Trim()) ;

            label7.Text = textBox2.Text.Replace("\r\n","").Trim(); ;
            for (long i =min; i < max; i++)
            {
                if (!finish.Contains(i))
                {
                    finish.Add(i);
                    yijing = yijing + 1;
                    label8.Text = yijing.ToString();

                    try
                    {


                        string url = "http://api.camucz.com:889/pidservicedetr.asmx/DetrJsonQueryId?UserAct=" + textBox4.Text.Replace("\r\n", "").Trim() + "&UserPwd=" + textBox5.Text.Replace("\r\n", "").Trim() + "&UserKey=" + textBox6.Text.Replace("\r\n", "").Trim() + "&InstructionType=TN&InstructionValue=" + i+ "&QueryId=";
                        
                        string html = GetUrl(url, "utf-8");

                        Match a1 = Regex.Match(html, @"""PASSENGER"":""([\s\S]*?)""");
                        Match a2 = Regex.Match(html, @"""DETRTN"":""([\s\S]*?)""");
                        Match a3 = Regex.Match(html, @"""AirFromCode"":""([\s\S]*?)""");
                        Match a4 = Regex.Match(html, @"""AirToCode"":""([\s\S]*?)""");
                        Match a5 = Regex.Match(html, @"""AirCode"":""([\s\S]*?)""");
                        Match a6 = Regex.Match(html, @"""AirFlightNo"":""([\s\S]*?)""");
                        Match a7 = Regex.Match(html, @"""AirSeat"":""([\s\S]*?)""");
                        Match a8 = Regex.Match(html, @"""AirDate"":""([\s\S]*?)""");
                        Match a9 = Regex.Match(html, @"""AirTime"":""([\s\S]*?)""");
                        Match a10 = Regex.Match(html, @"""AirTicketStatus"":""([\s\S]*?)""");

                        if (a1.Groups[1].Value != "")
                        {
                            hege = hege + 1;
                            label9.Text = hege.ToString();
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count+1).ToString()); //使用Listview展示数据   
                            lv1.SubItems.Add(a1.Groups[1].Value.Replace(":", "").Trim());
                            lv1.SubItems.Add(a2.Groups[1].Value);
                            lv1.SubItems.Add(a3.Groups[1].Value);
                            lv1.SubItems.Add(a4.Groups[1].Value);
                            lv1.SubItems.Add(a5.Groups[1].Value + a6.Groups[1].Value);
                            lv1.SubItems.Add(a7.Groups[1].Value);
                            lv1.SubItems.Add(a8.Groups[1].Value);
                            lv1.SubItems.Add(a9.Groups[1].Value);
                            lv1.SubItems.Add(a10.Groups[1].Value);

                            //导出
                            string date = a8.Groups[1].Value;

                            if (date.Trim() == "")
                            {
                                date = "空";
                            }
                            string value = a1.Groups[1].Value.Replace(":", "").Trim() + "---"  + a2.Groups[1].Value + "---" + a3.Groups[1].Value + "---" + a4.Groups[1].Value + "---" + a5.Groups[1].Value + a6.Groups[1].Value + "---" + a7.Groups[1].Value + "---" + a8.Groups[1].Value + "---" + a9.Groups[1].Value + "---" + a10.Groups[1].Value;
                            if (!File.Exists(path + date + ".txt"))
                            {
                                FileStream fs1 = new FileStream(path + date + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                                StreamWriter sw = new StreamWriter(fs1);
                                sw.WriteLine(value);
                                sw.Close();
                                fs1.Close();
                            }
                            else
                            {
                                StreamWriter fs = new StreamWriter(path + date + ".txt", true);
                                fs.WriteLine(value);
                                fs.Close();
                            }

                            //导出
                        }
                    }
                    catch
                    {

                        continue;
                    }



                }


            }
            label10.Text = "查询完成";



        }
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
               
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
              
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
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



        /// <summary>
        /// 主程序1
        /// </summary>
        public void run1()
        {

            StreamReader streamReader = new StreamReader(filename, Encoding.Default);
            string text = streamReader.ReadToEnd();
            string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            label7.Text = array.Length.ToString();
            for (int i = 0; i < array.Length; i++)
            {

                if (!finish.Contains(array[i]))
                {
                    finish.Add(array[i]);
                
                    yijing = yijing + 1;
                    label8.Text = yijing.ToString();

                    try
                    {


                        string url = "http://api.camucz.com:889/pidservicedetr.asmx/DetrJsonQueryId?UserAct=" + textBox4.Text.Replace("\r\n","").Trim()+"&UserPwd="+textBox5.Text.Replace("\r\n", "").Trim()+"&UserKey="+textBox6.Text.Replace("\r\n", "").Trim()+"&InstructionType=TN&InstructionValue=" + array[i].Replace("\r\n", "").Trim()+ "&QueryId=";
                        
                        string html = GetUrl(url, "utf-8");
                      
                        Match a1 = Regex.Match(html, @"""PASSENGER"":""([\s\S]*?)""");
                        Match a2 = Regex.Match(html, @"""DETRTN"":""([\s\S]*?)""");
                        Match a3 = Regex.Match(html, @"""AirFromCode"":""([\s\S]*?)""");
                        Match a4 = Regex.Match(html, @"""AirToCode"":""([\s\S]*?)""");
                        Match a5 = Regex.Match(html, @"""AirCode"":""([\s\S]*?)""");
                        Match a6 = Regex.Match(html, @"""AirFlightNo"":""([\s\S]*?)""");
                        Match a7 = Regex.Match(html, @"""AirSeat"":""([\s\S]*?)""");
                        Match a8 = Regex.Match(html, @"""AirDate"":""([\s\S]*?)""");
                        Match a9 = Regex.Match(html, @"""AirTime"":""([\s\S]*?)""");
                        Match a10 = Regex.Match(html, @"""AirTicketStatus"":""([\s\S]*?)""");

                        if (a1.Groups[1].Value != "")
                        {
                            hege = hege + 1;
                            label9.Text = hege.ToString();
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                            lv1.SubItems.Add(a1.Groups[1].Value.Replace(":", "").Trim());
                            lv1.SubItems.Add(a2.Groups[1].Value);
                            lv1.SubItems.Add(a3.Groups[1].Value);
                            lv1.SubItems.Add(a4.Groups[1].Value);
                            lv1.SubItems.Add(a5.Groups[1].Value + a6.Groups[1].Value);
                            lv1.SubItems.Add(a7.Groups[1].Value);
                            lv1.SubItems.Add(a8.Groups[1].Value);
                            lv1.SubItems.Add(a9.Groups[1].Value);
                            lv1.SubItems.Add(a10.Groups[1].Value);

                            //导出
                            string date = a8.Groups[1].Value;

                            if (date.Trim() == "")
                            {
                                date = "空";
                            }
                            string value = a1.Groups[1].Value.Replace(":", "").Trim() + "---" + a2.Groups[1].Value + "---" + a3.Groups[1].Value + "---" + a4.Groups[1].Value + "---" + a5.Groups[1].Value + a6.Groups[1].Value + "---" + a7.Groups[1].Value + "---" + a8.Groups[1].Value + "---" + a9.Groups[1].Value + "---" + a10.Groups[1].Value;
                            if (!File.Exists(path + date + ".txt"))
                            {
                                FileStream fs1 = new FileStream(path + date + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                                StreamWriter sw = new StreamWriter(fs1);
                                sw.WriteLine(value);
                                sw.Close();
                                fs1.Close();
                            }
                            else
                            {
                                StreamWriter fs = new StreamWriter(path + date + ".txt", true);
                                fs.WriteLine(value);
                                fs.Close();
                            }

                            //导出
                        }
                    }
                    catch
                    {

                        continue;
                    }



                }


            }
            label10.Text = "查询完成";



        }



        /// <summary>
        /// 主程序2新2021-05-29
        /// </summary>
        public void run2()
        {

            long min = Convert.ToInt64(textBox1.Text.Replace("\r\n", "").Trim());
            long max = Convert.ToInt64(textBox1.Text.Replace("\r\n", "").Trim()) + Convert.ToInt64(textBox2.Text.Replace("\r\n", "").Trim());

            label7.Text = textBox2.Text.Replace("\r\n", "").Trim(); ;
            for (long i = min; i < max; i++)
            {
                if (!finish.Contains(i))
                {
                    finish.Add(i);
                    yijing = yijing + 1;
                    label8.Text = yijing.ToString();


                    try
                    {


                        string url = "http://47.104.234.89:9755/api/Action/Detr";
                        string postdata = "{\"header\": {\"pid\": \"SHALM\",\"safety\": \"SHALM2021052414\"},\"body\": {\"ticketNO\": \"" + i + "\"}}";
                        string html = PostUrl(url, postdata);
                       
                        Match a1 = Regex.Match(html, @"ETKD:([\s\S]*?) ");
                        Match a2 = Regex.Match(html, @"ORG/DST: ([\s\S]*?) ");
                        Match a3 = Regex.Match(html, @"PASSENGER: ([\s\S]*?) ");
                        Match a4 = Regex.Match(html, @"FM:([\s\S]*?)/");
                        Match a5 = Regex.Match(html, @"20K ([\s\S]*?) ");
                      
                        
                       
                        if (a1.Groups[1].Value != "")
                        {
                            hege = hege + 1;
                            label9.Text = hege.ToString();
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                            lv1.SubItems.Add(a1.Groups[1].Value.Replace("TN/", ""));
                            lv1.SubItems.Add(a2.Groups[1].Value.Trim());
                            lv1.SubItems.Add(a3.Groups[1].Value.Trim());
                            lv1.SubItems.Add(a4.Groups[1].Value.Trim());
                            lv1.SubItems.Add(a5.Groups[1].Value.Trim());
                          
                         

                            //导出
                            //string date = a8.Groups[1].Value;

                            //if (date.Trim() == "")
                            //{
                            //    date = "空";
                            //}
                            //string value = a1.Groups[1].Value.Replace(":", "").Trim() + "---" + a2.Groups[1].Value + "---" + a3.Groups[1].Value + "---" + a4.Groups[1].Value + "---" + a5.Groups[1].Value + a6.Groups[1].Value + "---" + a7.Groups[1].Value + "---" + a8.Groups[1].Value + "---" + a9.Groups[1].Value + "---" + a10.Groups[1].Value;
                            //if (!File.Exists(path + date + ".txt"))
                            //{
                            //    FileStream fs1 = new FileStream(path + date + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                            //    StreamWriter sw = new StreamWriter(fs1);
                            //    sw.WriteLine(value);
                            //    sw.Close();
                            //    fs1.Close();
                            //}
                            //else
                            //{
                            //    StreamWriter fs = new StreamWriter(path + date + ".txt", true);
                            //    fs.WriteLine(value);
                            //    fs.Close();
                            //}

                            //导出
                        }

                    }
                    catch
                    {

                        continue;
                    }
                }

            }
            label10.Text = "查询完成";



        }


        /// <summary>
        /// 主程序2新2021-05-29
        /// </summary>
        public void run3()
        {

            StreamReader streamReader = new StreamReader(filename, Encoding.Default);
            string text = streamReader.ReadToEnd();
            string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            label7.Text = array.Length.ToString();
            for (int i = 0; i < array.Length; i++)
            {

                if (!finish.Contains(array[i]))
                {
                    finish.Add(array[i]);

                    yijing = yijing + 1;
                    label8.Text = yijing.ToString();

                    try
                    {


                        string url = "http://47.104.234.89:9755/api/Action/Detr";
                        string postdata = "{\"header\": {\"pid\": \"SHALM\",\"safety\": \"SHALM2021052414\"},\"body\": {\"ticketNO\": \"" + array[i].Replace("\r\n", "").Trim() + "\"}}";
                        string html = PostUrl(url, postdata);

                        Match a1 = Regex.Match(html, @"ETKD:([\s\S]*?) ");
                        Match a2 = Regex.Match(html, @"ORG/DST: ([\s\S]*?) ");
                        Match a3 = Regex.Match(html, @"PASSENGER: ([\s\S]*?) ");
                        Match a4 = Regex.Match(html, @"FM:([\s\S]*?)/");
                        Match a5 = Regex.Match(html, @"20K ([\s\S]*?) ");



                        if (a1.Groups[1].Value != "")
                        {
                            hege = hege + 1;
                            label9.Text = hege.ToString();
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                            lv1.SubItems.Add(a1.Groups[1].Value.Trim().Replace("TN/",""));
                            lv1.SubItems.Add(a2.Groups[1].Value.Trim());
                            lv1.SubItems.Add(a3.Groups[1].Value.Trim());
                            lv1.SubItems.Add(a4.Groups[1].Value.Trim());
                            lv1.SubItems.Add(a5.Groups[1].Value.Trim());

                            //导出
                            //string date = a8.Groups[1].Value;

                            //if (date.Trim() == "")
                            //{
                            //    date = "空";
                            //}
                            //string value = a1.Groups[1].Value.Replace(":", "").Trim() + "---" + a2.Groups[1].Value + "---" + a3.Groups[1].Value + "---" + a4.Groups[1].Value + "---" + a5.Groups[1].Value + a6.Groups[1].Value + "---" + a7.Groups[1].Value + "---" + a8.Groups[1].Value + "---" + a9.Groups[1].Value + "---" + a10.Groups[1].Value;
                            //if (!File.Exists(path + date + ".txt"))
                            //{
                            //    FileStream fs1 = new FileStream(path + date + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                            //    StreamWriter sw = new StreamWriter(fs1);
                            //    sw.WriteLine(value);
                            //    sw.Close();
                            //    fs1.Close();
                            //}
                            //else
                            //{
                            //    StreamWriter fs = new StreamWriter(path + date + ".txt", true);
                            //    fs.WriteLine(value);
                            //    fs.Close();
                            //}

                            //导出
                        }

                    }
                    catch
                    {

                        continue;
                    }
                }

            }
            label10.Text = "查询完成";



        }


        private void 飞机票信息记录_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    if (ExistINIFile())
            //    {
            //        string key = IniReadValue("values", "key");
            //        string secret = IniReadValue("values", "secret");
            //        string[] value = secret.Split(new string[] { "asd" }, StringSplitOptions.None);
            //        if (Convert.ToInt32(value[1]) < Convert.ToInt32(myDLL.method.GetTimeStamp()))
            //        {
            //            MessageBox.Show("激活已过期");
            //            string str = Interaction.InputBox("您的机器码如下，请复制机器码提供到后台，输入激活码然后激活！", "激活软件", myDLL.method.GetMD5(myDLL.method.GetMacAddress()), -1, -1);
            //            string[] text = str.Split(new string[] { "asd" }, StringSplitOptions.None);

            //            if (text[0] == myDLL.method.GetMD5(myDLL.method.GetMD5(myDLL.method.GetMacAddress()) + "siyiruanjian"))
            //            {
            //                IniWriteValue("values", "key", myDLL.method.GetMD5(myDLL.method.GetMacAddress()));
            //                IniWriteValue("values", "secret", str);
            //                MessageBox.Show("激活成功");


            //            }
            //            else
            //            {
            //                MessageBox.Show("激活码错误");
            //                System.Diagnostics.Process.GetCurrentProcess().Kill();
            //                return;
            //            }

            //        }




            //        else if (value[0] != myDLL.method.GetMD5(myDLL.method.GetMD5(myDLL.method.GetMacAddress()) + "siyiruanjian") || key != myDLL.method.GetMD5(myDLL.method.GetMacAddress()))
            //        {

            //            string str = Interaction.InputBox("您的机器码如下，请复制机器码提供到后台，输入激活码然后激活！", "激活软件", myDLL.method.GetMD5(myDLL.method.GetMacAddress()), -1, -1);
            //            string[] text = str.Split(new string[] { "asd" }, StringSplitOptions.None);

            //            if (text[0] == myDLL.method.GetMD5(myDLL.method.GetMD5(myDLL.method.GetMacAddress()) + "siyiruanjian"))
            //            {
            //                IniWriteValue("values", "key", myDLL.method.GetMD5(myDLL.method.GetMacAddress()));
            //                IniWriteValue("values", "secret", str);
            //                MessageBox.Show("激活成功");


            //            }
            //            else
            //            {
            //                MessageBox.Show("激活码错误");
            //                System.Diagnostics.Process.GetCurrentProcess().Kill();
            //                return;
            //            }
            //        }

            //    }
            //    else
            //    {

            //        string str = Interaction.InputBox("您的机器码如下，请复制机器码提供到后台，输入激活码然后激活！", "激活软件", myDLL.method.GetMD5(myDLL.method.GetMacAddress()), -1, -1);
            //        string[] text = str.Split(new string[] { "asd" }, StringSplitOptions.None);
            //        if (text[0] == myDLL.method.GetMD5(myDLL.method.GetMD5(myDLL.method.GetMacAddress()) + "siyiruanjian"))
            //        {
            //            IniWriteValue("values", "key", myDLL.method.GetMD5(myDLL.method.GetMacAddress()));
            //            IniWriteValue("values", "secret", str);
            //            MessageBox.Show("激活成功");


            //        }
            //        else
            //        {
            //            MessageBox.Show("激活码错误");
            //            System.Diagnostics.Process.GetCurrentProcess().Kill();
            //            return;
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show("激活失败，请联系管理员");
            //    System.Diagnostics.Process.GetCurrentProcess().Kill();
            //}



            foreach (Control ctr in splitContainer1.Panel1.Controls)
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
        }

        private void button4_Click(object sender, EventArgs e)
        {
            myDLL.method.DataTableToExcel(myDLL.method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            #region 通用检测

            string html = myDLL.method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"fly517"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion


            if (filename=="")
            {
                for (int i = 0; i < Convert.ToInt32(textBox3.Text.Replace("\r\n", "").Trim()); i++)
                {

                    Thread thread = new Thread(new ThreadStart(run2));
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;

                }
            }
            else
            {
                for (int i = 0; i < Convert.ToInt32(textBox3.Text.Replace("\r\n", "").Trim()); i++)
                {

                    Thread thread = new Thread(new ThreadStart(run3));
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;

                }
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        string filename = "";
        private void button2_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                filename = this.openFileDialog1.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            filename = "";
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void 飞机票信息记录_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确认退出吗？", "退出询问"
           , MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK)
            {
                e.Cancel = true;//告诉窗体关闭这个任务取消

            }
            else
            {
                foreach (Control ctr in splitContainer1.Panel1.Controls)
                {
                    if (ctr is TextBox)
                    {


                        string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
                        FileStream fs1 = new FileStream(path + ctr.Name + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                        StreamWriter sw = new StreamWriter(fs1);
                        sw.WriteLine(ctr.Text);
                        sw.Close();
                        fs1.Close();

                    }
                }

  
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            myDLL.method.ListViewToCSV(listView1, true);
        }

        static string  path = AppDomain.CurrentDomain.BaseDirectory + "导出结果\\";

        #region 导出文本
        public static void expotTxt(ListView lv1)
        {

           


                foreach (ListViewItem item in lv1.Items)
                {
             
                    List<string> list = new List<string>();

                    string temp1 = item.SubItems[1].Text;
                    string temp2 = item.SubItems[2].Text;
                    string temp3 = item.SubItems[3].Text;
                    string temp4 = item.SubItems[4].Text;
                    string temp5 = item.SubItems[5].Text;
                    string temp6 = item.SubItems[6].Text;
                    string temp7 = item.SubItems[7].Text;
                    string temp8 = item.SubItems[8].Text;
                    string temp9 = item.SubItems[9].Text;
                    string value = temp1 + "---" + temp2 + "---" + temp3 + "---" + temp4 + "---" + temp5 + "---" + temp6 + "---" + temp7 + "---" + temp8 + "---" + temp9 ;
                    string date = temp7;

                if (date.Trim() == "")
                {
                    date = "空";
                }
                    if (!File.Exists(path + date + ".txt"))
                    {
                    FileStream fs1 = new FileStream(path + date + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1);
                    sw.WriteLine(value);
                    sw.Close();
                    fs1.Close();
                }
                    else
                    {
                        StreamWriter fs = new StreamWriter(path + date + ".txt", true);
                        fs.WriteLine(value);
                        fs.Close();
                    }

          
            }
                MessageBox.Show("导出完成");
            

        }
        #endregion
        private void button7_Click(object sender, EventArgs e)
        {
            expotTxt(listView1);
        }
    }
}
