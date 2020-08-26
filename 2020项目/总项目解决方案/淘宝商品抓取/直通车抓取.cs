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

namespace 淘宝商品抓取
{
    public partial class 直通车抓取 : Form
    {
        public 直通车抓取()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            webbrowser web = new webbrowser();
            web.Show();
        }
        bool status = true;
        bool zanting = true;
        ArrayList wangwangList = new ArrayList();
        public string cookie = "";
        public string getHtml(string url)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.186 Safari/537.36");
            headers.Add("Cookie", cookie);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.UserAgent = headers["user-agent"];
            request.Timeout = 5000;
            request.KeepAlive = true;
            request.Headers["Cookie"] = headers["Cookie"];
            HttpWebResponse Response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(Response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
            string content = reader.ReadToEnd();
            return content;

        }

        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = getHtml("http://www.acaiji.com/index/index/vip.html");

            if (!html.Contains(@"zhitongche"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion

            cookie = webbrowser.COOKIE;
            //cookie = "thw=cn; _fbp=fb.1.1590825258546.1202985694; x=e%3D1%26p%3D*%26s%3D0%26c%3D0%26f%3D0%26g%3D0%26t%3D0; UM_distinctid=1727ee9bc7d468-09c5e317d04876-6373664-1fa400-1727ee9bc7e656; enc=xoO7%2BHYNBa0jFGMMnLKZzcoPElaNVz405AW3D17MDCLUtC%2F8eSYq0gdPSdKqruMeTpsJD0HIRv4Q2LostA15Fw%3D%3D; cna=b2RCFw6lRXICAXnq99rJ24nX; lgc=zkg852266010; tracknick=zkg852266010; hng=CN%7Czh-CN%7CCNY%7C156; mt=ci=61_1; t=6978df018d7b96da4409b11cf631a6e4; _m_h5_tk=714165eb82ee4118321794d99d1b1971_1598006528097; _m_h5_tk_enc=fdf7df73fcac2dd5a4640be0dd43e279; _samesite_flag_=true; cookie2=1a267df117dadaf77d628c74e2687d9a; sgcookie=EXdUyoXOwotAzCmHvScZw; unb=1052347548; uc3=lg2=VFC%2FuZ9ayeYq2g%3D%3D&vt3=F8dCufTDD06VG%2BhJIr4%3D&nk2=GcOvCmiKUSBXqZNU&id2=UoH62EAv27BqSg%3D%3D; csg=93248d64; cookie17=UoH62EAv27BqSg%3D%3D; dnk=zkg852266010; skt=bf1d563d6e991b9d; existShop=MTU5Nzk5ODg1Ng%3D%3D; uc4=id4=0%40UOnlZ%2FcoxCrIUsehKGIWtJ03TJ4R&nk4=0%40GwrkntVPltPB9cR46GnfG8%2F8S9%2BVUe4%3D; _cc_=URm48syIZQ%3D%3D; _l_g_=Ug%3D%3D; sg=080; _nk_=zkg852266010; cookie1=Vvj8uMJubtxirKFtxaDmWPxYCP5sb7EKtrFe1w68JDk%3D; v=0; uc1=cookie16=VT5L2FSpNgq6fDudInPRgavC%2BQ%3D%3D&pas=0&existShop=true&cookie21=V32FPkk%2Fhodroid0QSjisQ%3D%3D&cookie15=UtASsssmOIJ0bQ%3D%3D&cookie14=UoTV5Or8s%2B9QlQ%3D%3D; alitrackid=www.taobao.com; lastalitrackid=www.taobao.com; JSESSIONID=ED7267A78111A768A0343BDAA6C8551F; isg=BBgYtFZP88Qdn99eMK6ZdXER6UaqAXyLeHq0jFIIItM17bvX-hA2GtKFISVdfTRj; tfstk=c7GVB7veZId2ENV6pSNN1l5TpPEAaegmS_zQnAmm8R9CG1y0YsxVXzYmKzz_vQ2c.; l=eBTLsRAPOEHCm4zsBO5whurza77tEIOf1sPzaNbMiInca6_1TUsp9NQq-MFk7dtjgtfFZetyIQLledhW-AzU-jDDBeYBRs5mpxv9-; _tb_token_=e167965e36b4b";
            if (cookie == "")
            {
                MessageBox.Show("请先登陆");
               
                return;
            }
            status = true;


            Thread search_thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            search_thread.Start();
            button1.Enabled = false;
        }

        public void getInfos(string html)
        {

           
            MatchCollection shops = Regex.Matches(html, @"SHOPNAME\\"":\\""([\s\S]*?)\\""");
            MatchCollection wangwangs = Regex.Matches(html, @"WANGWANGID\\"":\\""([\s\S]*?)\\""");
            MatchCollection grades = Regex.Matches(html, @"GRADE\\"":\\""([\s\S]*?)\\""");
            MatchCollection ismalls = Regex.Matches(html, @"ISMALL\\"":\\""([\s\S]*?)\\""");
            MatchCollection sells = Regex.Matches(html, @"SELL\\"":\\""([\s\S]*?)\\""");


            for (int i = 0; i < shops.Count; i++)
            {


                string sell = Unicode2String(sells[i].Groups[1].Value).Replace("\\", "");
                string xinyu = Unicode2String(grades[i].Groups[1].Value).Replace("\\", "");
                string ismall = Unicode2String(ismalls[i].Groups[1].Value).Replace("\\", "").Trim();
                string wangwang = Unicode2String(wangwangs[i].Groups[1].Value).Replace("\\", "");

                try
                {
                    if (Convert.ToInt32(sell) > Convert.ToInt32(textBox2.Text) && Convert.ToInt32(xinyu) < Convert.ToInt32(textBox3.Text) && ismall=="0")
                    {
                        if (!wangwangList.Contains(wangwang))
                        {
                            wangwangList.Add(wangwang);
                            ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                            listViewItem.SubItems.Add(wangwang);
                            listViewItem.SubItems.Add(Unicode2String(shops[i].Groups[1].Value).Replace("\\", ""));

                            listViewItem.SubItems.Add(xinyu);
                            listViewItem.SubItems.Add(ismall);
                            listViewItem.SubItems.Add(sell);

                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                        }
                    }
                }
                catch 
                {

                    continue;
                }

                

                if (status == false)
                {
                    return;
                }
            }


        }

        public void run()
        {

            string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string keyword in text)
            {


                string url = "https://s.taobao.com/search?q=" + keyword;

                for (int i = 0; i < 20; i++)
                {
                    label4.Text = "正在查询" + keyword + " 第" + (i + 1) + "页";

                    int p = i * 44;
                    string URL = url + "&s=" + p;
                    string html = getHtml(URL);

                    getInfos(html);
                    Thread.Sleep(2000);
                    if (status == false)
                    {
                        return;
                    }
                }
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            button1.Enabled = true;
        }
  
        private void 直通车抓取_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
            button1.Enabled = true;
        }
    }
}
