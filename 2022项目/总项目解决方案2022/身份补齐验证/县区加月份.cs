using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 身份补齐验证
{
    public partial class 县区加月份 : Form
    {
        public 县区加月份()
        {
            InitializeComponent();
        }

        List<string> list=new List<string>();
        string path = AppDomain.CurrentDomain.BaseDirectory;
        public void getdata()
        {
            if(!File.Exists(path + "data.txt"))
            {
                MessageBox.Show("请将数据放入软件文件夹，命名data");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            StreamReader sr = new StreamReader(path+"data.txt", method.EncodingType.GetTxtType(path + "data.txt"));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存


            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != "")
                {

                    list.Add(text[i]);
                }
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "当前线程：" + count;
            count++;
            Thread thread = new Thread(run);
            thread.Name = "线程" + count.ToString();
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }



        private void 县区加月份_Load(object sender, EventArgs e)
        {
            getdata();
        }

        DateTime starttime;
        int successcount = 0;
        int threadcount = 0;
        int allcount = 0;

        int runcount = 0;
        string cookie = "";
        string yzm = "";
        bool status = true;

        private void button2_Click(object sender, EventArgs e)
        {
          

            status = true;

            cookie = method.getSetCookie("http://www.iy6.cn/?c=Public&action=verify");

            string acw_tc = Regex.Match(cookie, @"acw_tc=([\s\S]*?);").Groups[1].Value;
            string PHPSESSID = Regex.Match(cookie, @"PHPSESSID=([\s\S]*?);").Groups[1].Value;
            cookie = "acw_tc=" + acw_tc + ";PHPSESSID=" + PHPSESSID;

            yzm_ttshitu.cookie = cookie;
            yzm = (yzm_ttshitu.shibie("zhou14752479", "zhoukaige00", "http://www.iy6.cn/?c=Public&action=verify"));



          Thread  thread = new Thread(run);
            thread.Name = "线程" + count.ToString();
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

           
        }
        #region POST默认请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public string PostUrlDefault(string url, string postData, string COOKIE)
        {
            try
            {

                string charset = "utf-8";
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //WebProxy proxy = new WebProxy(ip);
                //request.Proxy = proxy;
                request.Method = "Post";

                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                //headers.Add("sec-fetch-site:same-origin");
                //headers.Add("sec-fetch-user:?1");
                //headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                request.ContentType = "application/x-www-form-urlencoded";
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                //request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                // request.ContentLength = postData.Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                //request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = url;
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");

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
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion


       Dictionary<string, string> dics = new Dictionary<string, string>();  

        public void getquxian(string id)
        {
            try
            {
                
                string url = "https://www.wapi.cn/icard/"+id+".html";
                string html = method.GetUrl(url,"utf-8");
                MatchCollection uids = Regex.Matches(html, @"href=""/icard/([\s\S]*?)\.");
              StringBuilder sb = new StringBuilder();   
                for (int i = 0; i < uids.Count; i++)
                {
                   
                    sb.Append(uids[i].Groups[1].Value.ToString().Substring(4,2)+",");   
                }
               if(!dics.ContainsKey(id))
                {
                    dics.Add(id, sb.ToString());
                }
            }
            catch (Exception ex)
            {

                textBox3.Text=ex.ToString();
            }
        }


        List<string> finish = new List<string>(); 


        private void run()
        {


            try
            {


                try
                {
                    foreach (string uid in list.ToArray())
                    {
                        if (finish.Contains(uid))
                            continue;
                        finish.Add(uid);    
                        list.Remove(uid);
                        bool jixu = false;
                        string[] value = uid.ToString().Split(new string[] { "----" }, StringSplitOptions.None);

                        if (value.Length < 2)
                        {
                            textBox3.Text = "格式错误";

                            return;
                        }

                        for (int month = 1; month <= 12; month++)
                        {
                            if (jixu == true)
                                break;
                            for (int day = 1; day <= 31; day++)
                            {
                                if (jixu == true)
                                    break;



                                if(!dics.ContainsKey(value[1].Trim().Substring(0,4)))
                                {
                                    getquxian(value[1].Trim().Substring(0, 4));
                                }
                               

                                string quxians = dics[value[1].Trim().Substring(0, 4)];

                                
                                string[] text = quxians.Split(new string[] { "," }, StringSplitOptions.None);

                                foreach (string qu in text)
                                {
                                    if (qu.Trim() == "")
                                        continue;
                                    

                                    if (jixu == true)
                                        break;
                                    if (textBox3.Text.Length > 500)
                                        textBox3.Text = "";


                                    string month2 = month.ToString();
                                    if (month < 10)
                                    {
                                        month2 = "0" + month;
                                    }
                                    string day2 = day.ToString();
                                    if (day < 10)
                                    {
                                        day2 = "0" + day;
                                    }

                                   


                                    string name = System.Web.HttpUtility.UrlEncode(value[0].Trim());
                                    string card = value[1].Trim().Replace("****", "abcd");
                                   
                                    card = card.Replace("**", "ef");
                                   
                                    card = card.Replace("abcd", month2 + day2);
                                   
                                    card = card.Replace("ef", qu);
                                   
                                    if (身份补齐验证.CheckIDCard18(card) == false)
                                    {

                                        continue;
                                    }



                                    string zimu = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz123456789";
                                    Random rd = new Random(Guid.NewGuid().GetHashCode());
                                    string suiji = "";
                                    for (int a = 0; a < 10; a++)
                                    {
                                        int suijizimu = rd.Next(0, 60);
                                        suiji = suiji + zimu[suijizimu];
                                    }



                                    string url = "http://www.iy6.cn/?c=Public&action=toregister";
                                    string postdata = "username=" + suiji + "&password=" + suiji + "&pwdconfirm=" + suiji + "&realname=" + name + "&email=943510630%40qq.com&identity_card=" + card + "&verify=" + yzm + "&url=http%3A%2F%2Fwww.iy6.cn%2F";
                                    string html = PostUrlDefault(url, postdata, cookie);


                                    if (html.Contains("验证码错误"))
                                    {
                                        cookie = method.getSetCookie("http://www.iy6.cn/?c=Public&action=verify");

                                        string acw_tc = Regex.Match(cookie, @"acw_tc=([\s\S]*?);").Groups[1].Value;
                                        string PHPSESSID = Regex.Match(cookie, @"PHPSESSID=([\s\S]*?);").Groups[1].Value;
                                        cookie = "acw_tc=" + acw_tc + ";PHPSESSID=" + PHPSESSID;

                                        yzm_ttshitu.cookie = cookie;
                                        yzm = (yzm_ttshitu.shibie("zhou14752479", "zhoukaige00", "http://www.iy6.cn/?c=Public&action=verify"));
                                    }



                                    string amsg = Regex.Match(html, @"""msg"":""([\s\S]*?)""").Groups[1].Value;
                                    string astatus = Regex.Match(html, @"""status"":([\s\S]*?),").Groups[1].Value;

                                    if (astatus == "0")
                                    {
                                        amsg = method.Unicode2String(amsg);
                                        if (amsg == "今日注册已达上限")
                                        {
                                            successcount++;
                                            threadcount--;
                                          
                                            System.TimeSpan t = DateTime.Now - starttime;
                                            label4.Text = "用时：" + t.Hours + "时" + t.Minutes + "分" + t.Seconds + "秒，导入数量：" + allcount + " 正在第" + runcount + "个，成功数量：" + successcount;
                                            textBox3.Text = DateTime.Now.ToString("HH:mm:ss") + " " + value[0] + "----" + card + "----成功" + "\r\n";


                                            FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\成功.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                                            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                                            sw.WriteLine(value[0] + "----" + card + "----成功");
                                            sw.Close();
                                            fs1.Close();
                                            sw.Dispose();
                                            jixu = true;
                                            break;
                                        }

                                        else
                                        {

                                            System.TimeSpan t = DateTime.Now - starttime;
                                            label4.Text = "用时：" + t.Hours + "时" + t.Minutes + "分" + t.Seconds + "秒，导入数量：" + allcount + " 正在第" + runcount + "个，成功数量：" + successcount;

                                            string[] arr = { "身份证姓名匹配失败", "身份证姓名不符合", "身份信息有误" };
                                            Random r = new Random();
                                            textBox3.Text += Thread.CurrentThread.Name +"-"+ DateTime.Now.ToString("HH:mm:ss") + "  " + value[0] + "----" + card + "----" + amsg.Replace("身份证与姓名不符", arr[rd.Next(3)]).Replace("身份证与姓名不符", "身份证与姓名未匹配") + "\r\n";
                                        }



                                    }
                                    if (astatus == "1")
                                    {
                                        successcount++;
                                        threadcount--;
                                   
                                        System.TimeSpan t = DateTime.Now - starttime;
                                        label4.Text = "用时：" + t.Hours + "时" + t.Minutes + "分" + t.Seconds + "秒，导入数据数量：" + allcount + " 正在验证第" + runcount + "个，成功数量：" + successcount;



                                        textBox3.Text = DateTime.Now.ToString("HH:mm:ss") + " " + value[0] + "----" + card + "----成功" + "\r\n";

                                        FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\成功.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                                        StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                                        sw.WriteLine(value[0] + "----" + card + "----成功");
                                        sw.Close();
                                        fs1.Close();
                                        sw.Dispose();

                                        jixu = true;
                                        break;

                                    }

                                    if (html.Contains("编码"))
                                    {
                                        successcount++;
                                        threadcount--;
                                       
                                        System.TimeSpan t = DateTime.Now - starttime;
                                        label4.Text = "运行用时：" + t.Hours + "时" + t.Minutes + "分" + t.Seconds + "秒，导入数据数量：" + allcount + " 正在验证第" + runcount + "个，成功数量：" + successcount;



                                        textBox3.Text = DateTime.Now.ToString("HH:mm:ss") + " " + value[0] + "----" + card + "----成功" + "\r\n";

                                        FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\编码占用.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                                        StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                                        sw.WriteLine(value[0] + "----" + card + "----成功");
                                        sw.Close();
                                        fs1.Close();
                                        sw.Dispose();

                                        jixu = true;
                                        break;

                                    }







                                }

                            }



                        }




                        FileStream fs11 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\失败.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                        StreamWriter sw1 = new StreamWriter(fs11, Encoding.GetEncoding("UTF-8"));
                        sw1.WriteLine(value[0] + "----" + uid + "----失败");
                        sw1.Close();
                        fs11.Close();
                        sw1.Dispose();

                        jixu = true;

                    }

                   

                }
                catch (Exception ex)
                {

                    threadcount--;
                    textBox3.Text = ex.ToString();
                }


            }
            catch (Exception ex)
            {

                threadcount--;
                textBox3.Text = ex.Message;
            }


        }

        private void 县区加月份_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        int count = 0;
      

        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < list.ToArray().Length; i++)
            {
                FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\data"+DateTime.Now.ToString("HHmmss")+".txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                sw.WriteLine(list[i]);
                sw.Close();
                fs1.Close();
                sw.Dispose();
            }
        }
    }
}
