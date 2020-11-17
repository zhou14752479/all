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

namespace 主程序202007
{
    public partial class 安居客房价 : Form
    {
        public 安居客房价()
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


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "id58=e87rkF7Avuhk3KttMnJOAg==; sessid=078E9654-5BD1-5C12-76F1-127BF768709C; aQQ_ajkguid=FE727045-2366-4DC3-951F-EF154BFF845D; _ga=GA1.2.57638740.1591174134; 58tj_uuid=fce4cf3b-3044-430a-9e0b-ff27040ee0a2; als=0; wmda_uuid=156ffb055b64faeb189e857f1f10e130; wmda_new_uuid=1; wmda_visited_projects=%3B6145577459763; Ref=https%3A%2F%2Fm.anjuke.com%2Fxm%2Fmap%2F%3Fadr%3D%25E5%2585%25B4%25E6%25B8%25AF%25E4%25B8%2580%25E9%2587%258C176-279%25E5%258F%25B7%26lng%3D118.0324%26lat%3D24.492611%26tp%3Dcomm%26id%3D401355%26from%3Dxqdy_map; propertys=2dgr54t-qe263m_; lps=\" / xm / community / haicang /? p = 3 | \"; wmda_session_id_6145577459763=1595834026864-6a4e8d14-5538-e22d; new_uv=11; init_refer=; new_session=0; twe=2; _gid=GA1.2.1350016763.1595834539; ctid=195; __xsptplus8=8.9.1595834539.1595834548.3%232%7Cwww.baidu.com%7C%7C%7C%25E5%25AE%2589%25E5%25B1%2585%25E5%25AE%25A2%7C%23%23yjxS-ggwtji-K279gMlRCkkEU3NFuHon%23; xxzl_cid=eba0f015e41b4ec38117cacbe3ab4f54; xzuid=190e8c1e-a1f1-458c-9af4-33305910e1f6";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                //request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = false;
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

        ArrayList uids = new ArrayList();
        

        public void run()
        {
            
         

                for (int i = 1; i < 99999; i++)
                {

                    string url = "https://m.anjuke.com/bj/community/?p=" + i;
                    
                    label1.Text = url;
                    string html = GetUrl(url, "utf-8");
                    Match ahtml = Regex.Match(html, @"<!-- 小区列表 start -->([\s\S]*?)<!-- 小区列表 end -->");

                    MatchCollection urls = Regex.Matches(ahtml.Groups[1].Value, @"<a href=""([\s\S]*?)""");
                    MatchCollection areas = Regex.Matches(ahtml.Groups[1].Value, @"display:block;"">([\s\S]*?) ");
                    MatchCollection names = Regex.Matches(ahtml.Groups[1].Value, @"<strong class=""g-overflow"">([\s\S]*?)</strong>");
                MatchCollection buildates = Regex.Matches(ahtml.Groups[1].Value, @"<label class=""date"">([\s\S]*?)</label>");
                if (urls.Count == 0)
                    {
                       
                        break;
                    }

                    for (int j = 0; j < urls.Count; j++)
                    {

                        Match uid = Regex.Match(urls[j].Groups[1].Value, @"\d{5,7}");

                    if (uids.Contains(uid.Groups[0].Value))
                    {
                        MessageBox.Show("查询完成");
                        return;
                    }

                    else
                    {
                        uids.Add(uid.Groups[0].Value);
                    }
                        string purl = "https://m.anjuke.com/ajax/trendency/price/all?comm_id=" + uid.Groups[0].Value;
                        string phtml = GetUrl(purl, "utf-8");


                        string strhtml= GetUrl(urls[j].Groups[1].Value, "utf-8");
                        string[] text = phtml.Split(new string[] { "}," }, StringSplitOptions.None);

                        MatchCollection times = Regex.Matches(text[0], @"""20([\s\S]*?)"":([\s\S]*?),");
                        Match addr = Regex.Match(strhtml, @"<p>地址：([\s\S]*?)<");
                        Match lng = Regex.Match(strhtml, @"&lng=([\s\S]*?)&");
                        Match lat = Regex.Match(strhtml, @"&lat=([\s\S]*?)&");

                        for (int a = 0; a < times.Count; a++)
                        {
                            try
                            {
                                ListViewItem lv1 = listView1.Items.Add("20" + times[a].Groups[1].Value); //使用Listview展示数据

                                lv1.SubItems.Add(areas[j].Groups[1].Value);
                                lv1.SubItems.Add(names[j].Groups[1].Value);
                                double pp = Math.Round(Convert.ToDouble(times[a].Groups[2].Value), 4) * 10000;
                                lv1.SubItems.Add(pp.ToString());
                                lv1.SubItems.Add(addr.Groups[1].Value.Trim().Replace(" ","区"));
                                lv1.SubItems.Add(lng.Groups[1].Value);
                                lv1.SubItems.Add(lat.Groups[1].Value);
                            lv1.SubItems.Add(buildates[j].Groups[1].Value);
                        }
                            catch (Exception)
                            {


                            }

                        }

                        Thread.Sleep(1000);
                    }
                    Thread.Sleep(1000);

                }
              

               
            

            MessageBox.Show("查询完成");

        }
        private void 安居客房价_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
