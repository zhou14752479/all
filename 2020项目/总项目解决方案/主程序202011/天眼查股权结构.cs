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

namespace 主程序202011
{
    public partial class 天眼查股权结构 : Form
    {
        public 天眼查股权结构()
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


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "TYCID=2c5864f01d0311ebacf21329746fdeb2; ssuid=6816360890; _ga=GA1.2.1085416821.1604318506; _gid=GA1.2.1962047875.1604318506; RTYCID=09dc411f40b64e8f9ee9e6971f9d4b85; CT_TYCID=1c68bfdecb0d416c8e52b622ea559164; aliyungf_tc=AQAAAGcOSx/FPwUAtbLieeHFEe9/MMUD; csrfToken=69uWxDB90EPWhe81qU9lpsYG; bannerFlag=true; Hm_lvt_e92c8d65d92d534b0fc290df538b4758=1604318506,1604363826; cloud_token=ff8cb8788b304da2950b0389537fc84b; token=35e87bbcc2144e81bf965224d6ab59d2; _utm=1b74864c03ea4f349755af8851727c69; tyc-user-info={%22claimEditPoint%22:%220%22%2C%22vipToMonth%22:%22false%22%2C%22explainPoint%22:%220%22%2C%22personalClaimType%22:%22none%22%2C%22integrity%22:%2210%25%22%2C%22state%22:%225%22%2C%22score%22:%222120%22%2C%22announcementPoint%22:%220%22%2C%22messageShowRedPoint%22:%220%22%2C%22bidSubscribe%22:%22-1%22%2C%22vipManager%22:%220%22%2C%22monitorUnreadCount%22:%2295%22%2C%22discussCommendCount%22:%221%22%2C%22onum%22:%22366%22%2C%22showPost%22:null%2C%22messageBubbleCount%22:%220%22%2C%22claimPoint%22:%220%22%2C%22token%22:%22eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiIxNzYwNjExNzYwNiIsImlhdCI6MTYwNDM2NzQ0NSwiZXhwIjoxNjM1OTAzNDQ1fQ.gGKYCfVr9H9s9W7kY2fEG2f3x40mYGjbL42SwpBJ9YpIIIbpxH5mA2ydvwfBntfdAvmE4oLkvSiFcqFwSMu1jw%22%2C%22schoolAuthStatus%22:%222%22%2C%22userId%22:%222343171%22%2C%22vipToTime%22:%221551920852530%22%2C%22scoreUnit%22:%22%22%2C%22redPoint%22:%220%22%2C%22myTidings%22:%220%22%2C%22companyAuthStatus%22:%222%22%2C%22originalScore%22:%222120%22%2C%22myAnswerCount%22:%220%22%2C%22myQuestionCount%22:%220%22%2C%22signUp%22:%220%22%2C%22privateMessagePointWeb%22:%220%22%2C%22nickname%22:%22%E9%A9%AC%E8%A5%BF%E8%8E%AB%C2%B7%E6%B3%B0%E6%AF%94%22%2C%22privateMessagePoint%22:%220%22%2C%22bossStatus%22:%222%22%2C%22isClaim%22:%220%22%2C%22yellowDiamondEndTime%22:%220%22%2C%22isExpired%22:%221%22%2C%22yellowDiamondStatus%22:%22-1%22%2C%22pleaseAnswerCount%22:%221%22%2C%22bizCardUnread%22:%220%22%2C%22vnum%22:%2220%22%2C%22mobile%22:%2217606117606%22}; tyc-user-info-save-time=1604367444604; auth_token=eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiIxNzYwNjExNzYwNiIsImlhdCI6MTYwNDM2NzQ0NSwiZXhwIjoxNjM1OTAzNDQ1fQ.gGKYCfVr9H9s9W7kY2fEG2f3x40mYGjbL42SwpBJ9YpIIIbpxH5mA2ydvwfBntfdAvmE4oLkvSiFcqFwSMu1jw; tyc-user-phone=%255B%252217606117606%2522%252C%2522135%25208168%25203953%2522%255D; Hm_lpvt_e92c8d65d92d534b0fc290df538b4758=1604367446; _gat_gtag_UA_123487620_1=1";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://www.tianyancha.com/";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
               
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("Authorization:IH8lYFNLk5XshG1StUfhs5PWf6g0My0roJE2Gwcz+2s2NF5/KxunoTrlugkVC8ujGiJ+71Q2VYulUaWjBhfbpWBfWXrKKmniF5lSEfSjPtgumNDryHnW7h4IfhI1rxNV4CJAV/U7xK08K7l36bY5czzs70KGsU/MPYGO1CB7B/o=");
                headers.Add("X-AUTH-TOKEN:eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiIxNzc3OTEwNDM0OSIsImlhdCI6MTYwNDM3MDUxNSwiZXhwIjoxNjY3NDQyNTE1fQ.dnjpREcE4Mt_ncRwS7GuKhRIrhGzhJZ7oxhvWghFLXCZG4YvFJa8rKhLbPDLNmTQSu7iano26oJZL5RJcXbNBw");
                headers.Add("version:iOS 12.17.1");
             
                //添加头部
                // request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
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


        #region  获取公司ID
        public void getid()
        {
            string[] keywords = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string keyword in keywords)
            {


                    string Url = "https://www.tianyancha.com/search?key="+ System.Web.HttpUtility.UrlEncode(keyword);

                    string html = GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()
                    Match aid = Regex.Match(html, @"data-id=""([\s\S]*?)""");
                Match name = Regex.Match(html, @"data-id=""([\s\S]*?)<em>([\s\S]*?)</em>");

                ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                listViewItem.SubItems.Add(keyword);
                listViewItem.SubItems.Add(aid.Groups[1].Value);
                listViewItem.SubItems.Add(name.Groups[2].Value);


                while (this.zanting == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }


                Application.DoEvents();
                    Thread.Sleep(1000);


               

            }
              
        }

        bool zanting = true;
        //string cateid = "1";

        #endregion

        #region 主程序
        public void run()
        {
            string[] aids= textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string aid in aids)
            {
               

                string Url = "https://capi.tianyancha.com/cloud-company-background/company/getShareHolderStructure?gid="+aid+"&rootGid="+aid+"&operateType=2";

                string html = GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()
                MatchCollection gids = Regex.Matches(html, @"""gid"":""([\s\S]*?)""");
                MatchCollection names = Regex.Matches(html, @"""name"":""([\s\S]*?)""");
                MatchCollection bilis = Regex.Matches(html, @"""percent"":""([\s\S]*?)""");
                MatchCollection child = Regex.Matches(html, @"""hasChild"":([\s\S]*?),");
                for (int i = 1; i < gids.Count; i++)
                {
                    if (child[i - 1].Groups[1].Value.Trim() == "false")
                    {
                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(names[0].Groups[1].Value);
                        listViewItem.SubItems.Add(gids[i].Groups[1].Value);
                        listViewItem.SubItems.Add(names[i].Groups[1].Value);
                        listViewItem.SubItems.Add(bilis[i - 1].Groups[1].Value);
                        listViewItem.SubItems.Add(child[i - 1].Groups[1].Value);
                    }
                    else
                    {
                        //string burl = "https://capi.tianyancha.com/cloud-company-background/company/getAllShareHoldingStructureInfo?gid="+ gids[i].Groups[1].Value + "&rootGid="+aid+"&operateType=2";

                        string burl = "https://capi.tianyancha.com/cloud-equity-provider/v4/equity/nextnode.json?id="+ gids[i].Groups[1].Value + "&indexId="+aid+"&direction=down";
                        string bhtml = GetUrl(burl);  //定义的GetRul方法 返回 reader.ReadToEnd()
                        //MatchCollection bgids = Regex.Matches(bhtml, @"""gid"":""([\s\S]*?)""");
                        //MatchCollection bnames = Regex.Matches(bhtml, @"""name"":""([\s\S]*?)""");
                        //MatchCollection bbilis = Regex.Matches(bhtml, @"""percent"":""([\s\S]*?)""");
                        MatchCollection bgids = Regex.Matches(bhtml, @"""id"":([\s\S]*?),");
                        MatchCollection bnames = Regex.Matches(bhtml, @"""name"":""([\s\S]*?)""");
                        MatchCollection bbilis = Regex.Matches(bhtml, @"""percent"":""([\s\S]*?)""");
                        for (int j = 0; j < bnames.Count; j++)
                        {
                            ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                            listViewItem.SubItems.Add(names[0].Groups[1].Value);
                            listViewItem.SubItems.Add(gids[i].Groups[1].Value);
                            listViewItem.SubItems.Add(names[i].Groups[1].Value);
                            listViewItem.SubItems.Add(bilis[i - 1].Groups[1].Value);
                            listViewItem.SubItems.Add(child[i - 1].Groups[1].Value);

                            //listViewItem.SubItems.Add(bgids[j+1].Groups[1].Value);
                            listViewItem.SubItems.Add(bgids[j].Groups[1].Value);
                            listViewItem.SubItems.Add(bnames[j].Groups[1].Value);
                            listViewItem.SubItems.Add(bbilis[j].Groups[1].Value);


                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            
                        }
                        Thread.Sleep(1000);
                    }
                }
              


              


                Application.DoEvents();
                Thread.Sleep(1000);




            }

        }

       

        #endregion

        private void 天眼查股权结构_Load(object sender, EventArgs e)
        {

        }
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }
    }
}
