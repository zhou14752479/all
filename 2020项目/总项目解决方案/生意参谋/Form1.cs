using System;
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

namespace 生意参谋
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 获取时间戳毫秒
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalMilliseconds);
            return a.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            time = GetTimeStamp();
        }
        #region GET请求带COOKIE
        /// <summary>
        /// GET请求带COOKIE
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlWithCookie(string Url, string COOKIE, string charset)
        {
            try
            {
               // System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Referer = "https://sycm.taobao.com/ipoll/visitor.htm?spm=a21ag.7623863.LeftMenu.d181.2955d27bxx8vL5";
                request.Headers.Add("Cookie", COOKIE);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
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
        public static string cookie;
        public string time;

        public string getToken()
        {
            string url = "https://sycm.taobao.com/ipoll/visitor.htm";
            string html = GetUrlWithCookie(url,cookie,"utf-8");
            Match token= Regex.Match(html, @"legalityToken=([\s\S]*?);");
            return token.Groups[1].Value;

        }

        string token = "";
        public void run()
        {
            for (int i = 1; i < 99; i++)
            {
                string src = "";
                 
                string url = "https://sycm.taobao.com/ipoll/live/visitor/getRtVisitor.json?_=" + time + "&device=2&limit=20&page="+i+"&srcgrpname=" + src + "&token=" + token + "&type=Y";
               

                string html = GetUrlWithCookie(url, cookie, "utf-8");
               
                MatchCollection ahtml = Regex.Matches(html, @"cityName([\s\S]*?)oldVisitor");

                if (ahtml.Count == 0)
                    return;


                for (int j = 0; j < ahtml.Count; j++)
                {
                    Match laiyuan = Regex.Match(ahtml[j].Groups[1].Value, @"srcGrpName"":""([\s\S]*?)""");
                    Match time = Regex.Match(ahtml[j].Groups[1].Value, @"visitTime"":""([\s\S]*?)""");
                    Match key = Regex.Match(ahtml[j].Groups[1].Value, @"preSeKeyword"":""([\s\S]*?)""");
                    if (laiyuan.Groups[1].Value== "")
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(time.Groups[1].Value);
                        lv1.SubItems.Add("其他来源");
                    }
                    else
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(time.Groups[1].Value);
                        lv1.SubItems.Add(laiyuan.Groups[1].Value+key.Groups[1].Value);
                    }
                }

                Thread.Sleep(2000);
            }


        }

        private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            token = getToken(); 
            cookie = "thw=cn; ali_ab=49.94.92.171.1563332665663.4; x=e%3D1%26p%3D*%26s%3D0%26c%3D0%26f%3D0%26g%3D0%26t%3D0%26__ll%3D-1%26_ato%3D0; hng=CN%7Czh-CN%7CCNY%7C156; enc=rY0GpAFgrh5bXXfBXutSHaQSm6aOCly2Ov5qI2xvmmRLzA74CWx0R1R%2FH4RXdUTECCRII572ywPqHDXt8ypRKg%3D%3D; t=027e7e2bc53b51842bd6d63b5b90ab8a; cookie2=13a4b64b6f740bc9665ec84985402c45; _tb_token_=7e7eb555e8655; _samesite_flag_=true; _m_h5_tk=2427c9a20a1de7f1006472f11d13a478_1585628904353; _m_h5_tk_enc=cffcce3eb750260eea5c2e0120ab14b0; mt=ci=0_0; v=0; JSESSIONID=6CD31D89DD8DB4896C70EE79C6497DE6; cna=8QJMFUu4DhACATFZv2JYDtwd; sgcookie=EvTjd1HeMMx1D8IU%2B9G9g; unb=1052347548; uc1=pas=0&cookie15=WqG3DMC9VAQiUQ%3D%3D&cookie14=UoTUP2R%2FDh6zLw%3D%3D&existShop=true&lng=zh_CN&cookie21=VT5L2FSpdet1EftGlDZ1Vg%3D%3D&tag=8&cookie16=URm48syIJ1yk0MX2J7mAAEhTuw%3D%3D; uc3=id2=UoH62EAv27BqSg%3D%3D&lg2=VFC%2FuZ9ayeYq2g%3D%3D&vt3=F8dBxdAQyswnsOovSyQ%3D&nk2=GcOvCmiKUSBXqZNU; csg=a733c053; lgc=zkg852266010; cookie17=UoH62EAv27BqSg%3D%3D; dnk=zkg852266010; skt=264b2b26f9a0ba1a; existShop=MTU4NTYzMzQzOQ%3D%3D; uc4=nk4=0%40GwrkntVPltPB9cR46GnfGp2l8RYv%2FbU%3D&id4=0%40UOnlZ%2FcoxCrIUsehKGOnwWPT4l2n; tracknick=zkg852266010; _cc_=UtASsssmfA%3D%3D; tg=0; _l_g_=Ug%3D%3D; sg=080; _nk_=zkg852266010; cookie1=Vvj8uMJubtxirKFtxaDmWPxYCP5sb7EKtrFe1w68JDk%3D; _euacm_ac_l_uid_=1052347548; 1052347548_euacm_ac_c_uid_=1052347548; 1052347548_euacm_ac_rs_uid_=1052347548; _euacm_ac_rs_sid_=145672826; _portal_version_=new; cc_gray=1; tfstk=ceFdB0DQdGjnabNipWBiNlHtbtscZWj-feikwaZo0qXH7DdRioN0g7Dld4tK6NC..; XSRF-TOKEN=169283bd-2b9a-4aee-8613-08b82f0353c8; l=dBTc_4AlQLoCcEQ2BOfi-42gwx7TiIRb8sPrE67l5ICPOLf9PKtGWZf4QqTpCnGVnsi6J3oWYJ1uBSTtry4EhtikBBrsDOsCHdTh.; isg=BGlpQul9ExrvZi8iubY3LyQ2eBXDNl1on-G7iAtebtCV0ojkUoTTOQaAlHZkyvWg";
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
