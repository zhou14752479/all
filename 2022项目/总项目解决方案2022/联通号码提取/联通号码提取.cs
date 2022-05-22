using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

namespace 联通号码提取
{
    public partial class 联通号码提取 : Form
    {
        public 联通号码提取()
        {
            InitializeComponent();
        }
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url)
        {
            string charset = "utf-8";
            string html = "";
            string COOKIE = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("appid:orders");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 100000;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈


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

        #region  程序关闭删除自身
        public static void TestForKillMyself()
        {
            string bat = @"@echo off
                           :tryagain
                           del %1
                           if exist %1 goto tryagain
                           del %0";
            File.WriteAllText("killme.bat", bat);//写bat文件
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "killme.bat";
            psi.Arguments = "\"" + Environment.GetCommandLineArgs()[0] + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
        }
        #endregion
        private void 联通号码提取_Load(object sender, EventArgs e)
        {
            #region 通用检测


            if (!GetUrl("http://acaiji.com/index/index/vip.html").Contains(@"Uodf"))
            {
                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();

                return;
            }

            #endregion
            textBox2.Text = AppDomain.CurrentDomain.BaseDirectory;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择所在文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }

                textBox2.Text = dialog.SelectedPath;
            }
        }


        Dictionary<string, string> citynamedic = new Dictionary<string, string>();
        Dictionary<string, string> cityprodic = new Dictionary<string, string>();

        public void getcitycode()
        {

            Dictionary<string, string> prodic = new Dictionary<string, string>();
            string url = "https://10010.8cardmall.com/themes/home_themes/lib/js/napply/areaInfo.js";
            string html = GetUrl(url);
            MatchCollection pro = Regex.Matches(html, @"ESS_PROVINCE_CODE"":""([\s\S]*?)"",""PROVINCE_CODE"":""([\s\S]*?)""");

            MatchCollection city = Regex.Matches(html, @"CITY_NAME"":""([\s\S]*?)"",""ESS_CITY_CODE"":""([\s\S]*?)"",""PROVINCE_CODE"":""([\s\S]*?)""");

            for (int i = 0; i < pro.Count; i++)
            {
                if (!prodic.ContainsKey(pro[i].Groups[2].Value))
                {
                    prodic.Add(pro[i].Groups[2].Value, pro[i].Groups[1].Value);
                }
            }



            for (int i = 0; i < city.Count; i++)
            {
                try
                {

                    citynamedic.Add(city[i].Groups[1].Value, city[i].Groups[2].Value);
                    cityprodic.Add(city[i].Groups[1].Value, prodic[city[i].Groups[3].Value]);
                }
                catch (Exception ex)
                {

                    textBox1.Text = ex.ToString();
                }

            }


        }


        bool status = true;
        public void run()
        {
            try
            {
                getcitycode();
                foreach (string cityname in citynamedic.Keys)
                {
                    List<string> lists = new List<string>();
                    int count = 0;
                    string citycode = citynamedic[cityname];
                    string procode = cityprodic[cityname];
                    
                    while(true)
                    {
                        if (status == false)
                            return;
                        int chongfucount = 0;
                        string url = "https://msgo.10010.com/NumApp/NumberCenter/qryNum?callback=jsonp_queryMoreNums&provinceCode=" + procode + "&cityCode=" + citycode + "&monthFeeLimit=0&goodsId=511610241535&searchCategory=3&net=01&amounts=200&codeTypeCode=&searchValue=&exn=51068003&qryType=02&goodsNet=4&channel=msg-xsg&_=1653109443961";
                        string html = GetUrl(url);
                        //textBox1.Text = "";
                        MatchCollection tels = Regex.Matches(html, @"\d{11}");
                        for (int i = 0; i < tels.Count; i++)
                        {
                           
                            if (!lists.Contains(tels[i].Groups[0].Value))
                            {
                                lists.Add(tels[i].Groups[0].Value);
                                count = count + 1;
                                FileStream fs1 = new FileStream(textBox2.Text + "\\" + cityname + ".txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                                StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                                sw.WriteLine(tels[i].Groups[0].Value);
                                sw.Close();
                                fs1.Close();
                                sw.Dispose();
                                textBox1.Text = DateTime.Now.ToString() + "--正在提取：" + cityname + "当前城市总数：" + count + "\r\n";
                            }
                            else
                            {
                                chongfucount = chongfucount + 1;
                            }
                            
                        }

                        if(chongfucount==100)
                        {
                            break;
                        }
                        Thread.Sleep(1000);
                    }
                    //MessageBox.Show(tels.Count.ToString());
                }
            }
            catch (Exception ex)
            {

                textBox1.Text = ex.ToString();
            }
        }


        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测


            if (!GetUrl("http://acaiji.com/index/index/vip.html").Contains(@"Uodf"))
            {
                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();

                return;
            }

            #endregion

            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            status = false;

        }
    }
}
