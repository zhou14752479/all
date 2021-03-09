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
using myDLL;

namespace 临时数据抓取
{
    public partial class 饿了么 : Form
    {

        string cookie = "SID=IQAAAAARpFEJ6AYKAACejBoMOqRxU8o8jX08xdkMzf_XUX5AgMZIL5ll; USERID=295981321";
        public 饿了么()
        {
            InitializeComponent();
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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)";
                request.Referer = Url;
                request.Headers.Add("Cookie", COOKIE);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                request.Accept = "*/*";
                request.Timeout = 100000;
                string content = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                return (ex.ToString());



            }

        }
        #endregion

        #region 饿了么
        public void ele()
        {


           
                
            for (double i = 120.5; i <122.1; i=i+0.05)
            {
                label1.Text = i.ToString();
                for (int page = 0; page < 1000; page = page + 10)
                {

                    string url = "https://mainsite-restapi.ele.me/pizza/v1/restaurants?category_name=%E7%BE%8E%E9%A3%9F&city_id=1&latitude=30.86&longitude="+i+"&keyword=&extras=%5B%22activities%22%5D&order_by=0&restaurant_category_ids=%5B770%2C514%2C1026%2C1282%2C263%2C266%2C778%2C522%2C1034%2C1290%2C267%2C268%2C269%2C786%2C530%2C1042%2C1298%2C794%2C538%2C1050%2C1306%2C802%2C546%2C1058%2C1314%2C810%2C554%2C1066%2C1322%2C818%2C562%2C1074%2C1330%2C826%2C570%2C1082%2C1338%2C834%2C578%2C1090%2C1346%2C842%2C586%2C1098%2C1354%2C850%2C594%2C1106%2C1362%2C858%2C602%2C346%2C1114%2C354%2C866%2C610%2C1122%2C362%2C874%2C618%2C1130%2C370%2C882%2C626%2C1138%2C378%2C890%2C634%2C1146%2C386%2C898%2C642%2C1154%2C394%2C906%2C650%2C1162%2C402%2C914%2C658%2C1170%2C410%2C922%2C666%2C1178%2C418%2C930%2C674%2C1186%2C426%2C938%2C682%2C1194%2C434%2C946%2C690%2C1202%2C442%2C954%2C698%2C1210%2C450%2C706%2C962%2C1218%2C458%2C970%2C714%2C1226%2C209%2C466%2C978%2C722%2C1234%2C212%2C214%2C474%2C218%2C986%2C730%2C221%2C222%2C223%2C224%2C225%2C482%2C226%2C994%2C738%2C1250%2C227%2C228%2C232%2C490%2C746%2C234%2C1002%2C1258%2C236%2C240%2C241%2C498%2C754%2C1010%2C242%2C1266%2C249%2C762%2C506%2C1018%2C250%2C1274%5D&category_schema=%7B%22complex_category_ids%22%3A%5B770%2C514%2C1026%2C1282%2C263%2C266%2C778%2C522%2C1034%2C1290%2C267%2C268%2C269%2C786%2C530%2C1042%2C1298%2C794%2C538%2C1050%2C1306%2C802%2C546%2C1058%2C1314%2C810%2C554%2C1066%2C1322%2C818%2C562%2C1074%2C1330%2C826%2C570%2C1082%2C1338%2C834%2C578%2C1090%2C1346%2C842%2C586%2C1098%2C1354%2C850%2C594%2C1106%2C1362%2C858%2C602%2C346%2C1114%2C354%2C866%2C610%2C1122%2C362%2C874%2C618%2C1130%2C370%2C882%2C626%2C1138%2C378%2C890%2C634%2C1146%2C386%2C898%2C642%2C1154%2C394%2C906%2C650%2C1162%2C402%2C914%2C658%2C1170%2C410%2C922%2C666%2C1178%2C418%2C930%2C674%2C1186%2C426%2C938%2C682%2C1194%2C434%2C946%2C690%2C1202%2C442%2C954%2C698%2C1210%2C450%2C706%2C962%2C1218%2C458%2C970%2C714%2C1226%2C209%2C466%2C978%2C722%2C1234%2C212%2C214%2C474%2C218%2C986%2C730%2C221%2C222%2C223%2C224%2C225%2C482%2C226%2C994%2C738%2C1250%2C227%2C228%2C232%2C490%2C746%2C234%2C1002%2C1258%2C236%2C240%2C241%2C498%2C754%2C1010%2C242%2C1266%2C249%2C762%2C506%2C1018%2C250%2C1274%5D%7D&restaurant_category_id=%5B770%2C514%2C1026%2C1282%2C263%2C266%2C778%2C522%2C1034%2C1290%2C267%2C268%2C269%2C786%2C530%2C1042%2C1298%2C794%2C538%2C1050%2C1306%2C802%2C546%2C1058%2C1314%2C810%2C554%2C1066%2C1322%2C818%2C562%2C1074%2C1330%2C826%2C570%2C1082%2C1338%2C834%2C578%2C1090%2C1346%2C842%2C586%2C1098%2C1354%2C850%2C594%2C1106%2C1362%2C858%2C602%2C346%2C1114%2C354%2C866%2C610%2C1122%2C362%2C874%2C618%2C1130%2C370%2C882%2C626%2C1138%2C378%2C890%2C634%2C1146%2C386%2C898%2C642%2C1154%2C394%2C906%2C650%2C1162%2C402%2C914%2C658%2C1170%2C410%2C922%2C666%2C1178%2C418%2C930%2C674%2C1186%2C426%2C938%2C682%2C1194%2C434%2C946%2C690%2C1202%2C442%2C954%2C698%2C1210%2C450%2C706%2C962%2C1218%2C458%2C970%2C714%2C1226%2C209%2C466%2C978%2C722%2C1234%2C212%2C214%2C474%2C218%2C986%2C730%2C221%2C222%2C223%2C224%2C225%2C482%2C226%2C994%2C738%2C1250%2C227%2C228%2C232%2C490%2C746%2C234%2C1002%2C1258%2C236%2C240%2C241%2C498%2C754%2C1010%2C242%2C1266%2C249%2C762%2C506%2C1018%2C250%2C1274%5D&offset=" + page + "&limit=10&terminal=weapp&user_id=295981321";



                    string html = method.GetUrlWithCookie(url, cookie, "utf-8");

                    MatchCollection uids = Regex.Matches(html, @"has_story([\s\S]*?)""id"":""([\s\S]*?)""");
                    MatchCollection names = Regex.Matches(html, @"has_story([\s\S]*?)""name"":""([\s\S]*?)""");
                
                    if (uids.Count == 0)
                        break;
                    for (int j = 0; j < uids.Count; j++)
                    {
                     

                        string aurl = "https://restapi.ele.me/giraffe/restaurant/phone?shopId=" + uids[j].Groups[2].Value.Trim();
                        string ahtml =GetUrlWithCookie(aurl, cookie, "utf-8");
                     
                        Match tel = Regex.Match(ahtml, @"numbers"":\[""([\s\S]*?)""\]");

                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(names[j].Groups[2].Value);
                        lv1.SubItems.Add(uids[j].Groups[2].Value);
                        lv1.SubItems.Add(tel.Groups[1].Value.Trim());
                    
                   
                        Thread.Sleep(1000);
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;
                    }
                }
            }


        }

        #endregion


        Thread thread;
        bool status = true;
        bool zanting = true;
        private void 饿了么_Load(object sender, EventArgs e)
        {
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(ele);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
}
