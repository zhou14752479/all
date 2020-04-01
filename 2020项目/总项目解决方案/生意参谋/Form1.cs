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
        ArrayList oids = new ArrayList();
        string token = "";
        public void run()
        {
            string  src= System.Web.HttpUtility.UrlEncode(comboBox1.Text);
            if (comboBox1.Text == "全部")
            {
                src = "";
            }

            for (int i = 1; i < 99; i++)
            {
                
                string url = "https://sycm.taobao.com/ipoll/live/visitor/getRtVisitor.json?_=" + time + "&device=2&limit=20&page="+i+"&srcgrpname=" + src + "&token=" + token + "&type=Y";
               

                string html = GetUrlWithCookie(url, cookie, "utf-8");
                

            string[] ahtml = html.Split(new string[] { "\"},{\"" }, StringSplitOptions.None);

          
            if (ahtml.Length <2)
                    return;
               

                for (int j = 0; j < ahtml.Length; j++)
                {
                    


                    Match laiyuan = Regex.Match(ahtml[j]+"\"", @"srcGrpName"":""([\s\S]*?)""");
                    Match time = Regex.Match(ahtml[j] + "\"", @"visitTime"":""([\s\S]*?)""");
                    Match key = Regex.Match(ahtml[j] + "\"", @"preSeKeyword"":""([\s\S]*?)""");
                    Match oid = Regex.Match(ahtml[j] + "\"", @"oid"":""([\s\S]*?)""");

                    



                    DateTime nowtime = DateTime.Now;
                    DateTime time1 = Convert.ToDateTime(time.Groups[1].Value);
                    TimeSpan timeSpan = nowtime - time1;
                    if (timeSpan.TotalMinutes <= minutes)
                    {

                        if (!oids.Contains(oid.Groups[1].Value))
                        {
                            if (checkBox1.Checked == true)
                            {
                                oids.Add(oid.Groups[1].Value);
                            }



                            if (laiyuan.Groups[1].Value == "")
                            {
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add(time.Groups[1].Value);
                                lv1.SubItems.Add("其他来源");
                                lv1.SubItems.Add(oid.Groups[1].Value);
                            }
                            else
                            {
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add(time.Groups[1].Value);
                                lv1.SubItems.Add(laiyuan.Groups[1].Value + key.Groups[1].Value);
                                lv1.SubItems.Add(oid.Groups[1].Value);
                            }
                        }
                    }
                }

                Thread.Sleep(2000);
            }


        }

        private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        int minutes = 9999999;

        private void Button1_Click(object sender, EventArgs e)
        {
            
            if (radioButton1.Checked == true)
            {
                timer1.Interval = 600000;
                minutes = 10;
            }

            if (radioButton2.Checked == true)
            {
                timer1.Interval = 1800000;
                minutes = 30;
            }

            if (radioButton3.Checked == true)
            {
                timer1.Interval = 3600000;
                minutes = 60;
            }



            timer1.Start();

            token = getToken();
            cookie = "thw=cn; ali_ab=49.94.92.171.1563332665663.4; x=e%3D1%26p%3D*%26s%3D0%26c%3D0%26f%3D0%26g%3D0%26t%3D0%26__ll%3D-1%26_ato%3D0; hng=CN%7Czh-CN%7CCNY%7C156; enc=rY0GpAFgrh5bXXfBXutSHaQSm6aOCly2Ov5qI2xvmmRLzA74CWx0R1R%2FH4RXdUTECCRII572ywPqHDXt8ypRKg%3D%3D; t=027e7e2bc53b51842bd6d63b5b90ab8a; cna=8QJMFUu4DhACATFZv2JYDtwd; lgc=zkg852266010; tracknick=zkg852266010; tg=0; _euacm_ac_l_uid_=1052347548; 1052347548_euacm_ac_c_uid_=1052347548; 1052347548_euacm_ac_rs_uid_=1052347548; cc_gray=1; cookie2=1f6531a4fd93f6e9d02880217ffba454; v=0; _tb_token_=ea3ed371fa41e; _samesite_flag_=true; sgcookie=EReFThaf0L%2BNDHqzm6%2Fd1; unb=1052347548; uc3=lg2=Vq8l%2BKCLz3%2F65A%3D%3D&nk2=GcOvCmiKUSBXqZNU&vt3=F8dBxdAR%2B5k0M6RYGH8%3D&id2=UoH62EAv27BqSg%3D%3D; csg=0d6aac58; cookie17=UoH62EAv27BqSg%3D%3D; dnk=zkg852266010; skt=2bcdb8e7dcffabd2; existShop=MTU4NTcwODY2OQ%3D%3D; uc4=id4=0%40UOnlZ%2FcoxCrIUsehKGOnwB8wL3Ij&nk4=0%40GwrkntVPltPB9cR46GnfGp2kZlFXQ6s%3D; _cc_=URm48syIZQ%3D%3D; _l_g_=Ug%3D%3D; sg=080; _nk_=zkg852266010; cookie1=Vvj8uMJubtxirKFtxaDmWPxYCP5sb7EKtrFe1w68JDk%3D; tfstk=cofRBb95FoqkTB1De_eDOnwJhsEGZHrpc4tn9y_0YCEZ83cdih1G_6v3FeiJMrC..; _euacm_ac_rs_sid_=145672826; mt=ci=62_1; _m_h5_tk=4bc70155c1b8fe62f695c9ce32c08ac4_1585729795142; _m_h5_tk_enc=4a65bab378f43bd47d7a84729eead8d7; uc1=cookie16=VT5L2FSpNgq6fDudInPRgavC%2BQ%3D%3D&cookie21=UIHiLt3xSixwH1aenGUFEQ%3D%3D&cookie15=UtASsssmOIJ0bQ%3D%3D&existShop=true&pas=0&cookie14=UoTUP2WAJWxT4w%3D%3D&tag=8&lng=zh_CN; l=dBTc_4AlQLoCcdb8BOfMS42gwx7twLRXcsPrE67l5ICPO96hlAZVWZfJ1HxMCnGVnsCpW3oWYJ1uB58Thy4EhtikBBrsDOsI2dTh.; isg=BNraYy3gUFFNndx31nvUcrNzK4D8C17liETo_eRZMG3sV2qRyZkb9JfhJyNLh9Z9";
            run();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
           
            
        }


     
        private void Timer1_Tick(object sender, EventArgs e)
        {
            run();


        }

        private void Button2_Click_1(object sender, EventArgs e)
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                string value = listView1.Items[i].SubItems[2].Text.Trim();
                if (!dic.ContainsKey(value))
                {
                    dic.Add(value, 1);   //1代表只有1个

                }
                else
                { 
                    dic[value]++;       //包含了则增加1
                }
                                                             
            }

            foreach (KeyValuePair<string, int> item in dic)
            {
                textBox1.Text += item.Key + " " + item.Value+"\r\n";
                   
            }

            

            timer1.Stop();
        }
    }
}
