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

namespace 美团
{
    public partial class 美团附近 : Form
    {
        public 美团附近()
        {
            InitializeComponent();
        }

        #region GET请求
        public static string GetUrl(string Url)
        {
            try
            {


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.97 Safari/537.11";

                request.Headers.Add("Cookie", "");

                request.Referer = "https://i.meituan.com/wrapapi/poiinfo?poiId=150177929";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

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


        bool zanting = true;
        #region  主程序
        public void run()
        {

            try
            {


                for (int i = 0; i < 10001; i=i+100)

                {

                    string Url = "https://www.meituan.com/meishi/api/poi/getNearPoiList?offset="+i+ "&limit=100&cityId=184&lat=33.949188&lng=118.327505";

                    string html = GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()


                    MatchCollection names = Regex.Matches(html, @"""name"":""([\s\S]*?)""");
                    MatchCollection scores = Regex.Matches(html, @"""score"":([\s\S]*?),");
                    MatchCollection avgPrices = Regex.Matches(html, @"""avgPrice"":([\s\S]*?),");
                    MatchCollection address = Regex.Matches(html, @"""address"":""([\s\S]*?)""");
                    MatchCollection phone = Regex.Matches(html, @"""phone"":""([\s\S]*?)""");
                    MatchCollection sold = Regex.Matches(html, @"""sold"":([\s\S]*?),");
                   


                    if (names.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;

                    for (int j = 0; j < names.Count; j++)
 

                    {

                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(names[j].Groups[1].Value);
                        listViewItem.SubItems.Add(scores[j].Groups[1].Value);
                        listViewItem.SubItems.Add(avgPrices[j].Groups[1].Value);
                        listViewItem.SubItems.Add(address[j].Groups[1].Value);
                        listViewItem.SubItems.Add(phone[j].Groups[1].Value);
                        listViewItem.SubItems.Add(sold[j].Groups[1].Value);



                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                    }
                    Application.DoEvents();
                    Thread.Sleep(1000);
                }

               
            }
            catch (System.Exception ex)
            {
                 ex.ToString();
            }
         

        }


          

        #endregion
        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://api.map.baidu.com/lbsapi/getpoint/index.html");
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }
    }
}
