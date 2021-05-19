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
using MySql.Data.MySqlClient;

namespace 临时数据抓取
{
    public partial class 饿了么 : Form
    {

        string cookie = "JSESSIONID=960DFE0B425EA25DFD7CF1468455D970; Hm_lvt_7c0ab5b0ae856f8774afb3952e172f5d=1618274809; acw_tc=2760821916182767899946687ea4d33b42c671822bcb2bf18d9fccc6b13399; Hm_lpvt_7c0ab5b0ae856f8774afb3952e172f5d=1618276813";
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

        Dictionary<string, string> dics = new Dictionary<string, string>();

        #region 上海律师
        public void run()
        {

            for (int page = Convert.ToInt32(textBox2.Text); page < Convert.ToInt32(textBox3.Text); page++)
            {

                string url = "http://credit.lawyers.org.cn/lawyer-list.jsp?page=" + page;

                string html = method.GetUrlWithCookie(url, cookie, "utf-8");

                MatchCollection uids = Regex.Matches(html, @"id=([\s\S]*?)""");
            
                if (uids.Count == 0)
                    continue;
                for (int j = 0; j < uids.Count; j++)
                {


                    try
                    {
                        string aurl = "http://credit.lawyers.org.cn/lawyer.jsp?id=" + uids[j].Groups[1].Value.Trim();
                        string ahtml = GetUrlWithCookie(aurl, cookie, "utf-8");

                        Match name = Regex.Match(ahtml, @"<dd class=""name"">([\s\S]*?)</dd>");
                        Match zyzh = Regex.Match(ahtml, @"执业证号：</span>([\s\S]*?)</dd>");
                        Match xb = Regex.Match(ahtml, @"性别：</label>([\s\S]*?)</li>");
                        Match nl = Regex.Match(ahtml, @"年龄：</label>([\s\S]*?)</li>");
                        Match sf = Regex.Match(ahtml, @"所内身份：</label>([\s\S]*?)</li>");
                        Match email = Regex.Match(ahtml, @"email：</label>([\s\S]*?)</li>");
                        MatchCollection zyzts = Regex.Matches(ahtml, @"<div class=""credit-level([\s\S]*?)>([\s\S]*?)</div>");

                        string bhtml = "";
                        string bid = Regex.Match(ahtml, @"执业机构：</span><a href=""([\s\S]*?)""").Groups[1].Value;
                        if (dics.ContainsKey(bid))
                        {
                            bhtml = dics[bid];
                        }
                        else
                        {
                            string burl = "http://credit.lawyers.org.cn/" + bid;
                            bhtml = GetUrlWithCookie(burl, cookie, "utf-8");
                            dics.Add(bid, bhtml);
                        }





                        Match area = Regex.Match(bhtml, @"主管司法局：</label>([\s\S]*?)</li>");
                        Match bname = Regex.Match(bhtml, @"<dd class=""name"">([\s\S]*?)</dd>");
                        MatchCollection bzyzts = Regex.Matches(bhtml, @"<div class=""credit-level([\s\S]*?)>([\s\S]*?)</div>");
                        Match addr = Regex.Match(bhtml, @"办公地址：</span>([\s\S]*?)</dd>");
                        Match tel = Regex.Match(bhtml, @"联系电话：</label>([\s\S]*?)</li>");


                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(area.Groups[1].Value.Trim());
                        lv1.SubItems.Add(bname.Groups[1].Value.Trim());
                        lv1.SubItems.Add(bzyzts[0].Groups[2].Value.Trim());
                        lv1.SubItems.Add(bzyzts[1].Groups[2].Value.Trim());
                        lv1.SubItems.Add(addr.Groups[1].Value.Trim());
                        lv1.SubItems.Add(tel.Groups[1].Value.Trim());

                        lv1.SubItems.Add(name.Groups[1].Value.Trim());
                        lv1.SubItems.Add(zyzh.Groups[1].Value.Trim());
                        lv1.SubItems.Add(xb.Groups[1].Value.Trim());
                        lv1.SubItems.Add(nl.Groups[1].Value.Trim());
                        lv1.SubItems.Add(sf.Groups[1].Value.Trim());
                        lv1.SubItems.Add(email.Groups[1].Value.Trim());
                        lv1.SubItems.Add(zyzts[0].Groups[2].Value.Trim());
                        lv1.SubItems.Add(zyzts[1].Groups[2].Value.Trim());
                        lv1.SubItems.Add(page.ToString());



                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;
                        Thread.Sleep(100);
                    }
                    catch (Exception ex)
                    {
                        textBox1.Text += uids[j].Groups[1].Value + "\r\n";
                       continue;
                    }

                }
            }


        }

        #endregion

        public void insert(string a1, string a2, string a3, string a4, string a5, string a6)
        {

            try
            {
                string constr = "Host =localhost;Database=yanxiu_user;Username=root;Password=c#kaifa6668.";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();


                MySqlCommand cmd = new MySqlCommand("INSERT INTO datas (name,card,phone,uid,nickname,json)VALUES('" + a1 + " ', '" + a2 + " ', '" + a3 + " ', '" + a4 + " ', '" + a5 + " ', '" + a6 + " ')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {

                    mycon.Close();

                }



            }

            catch (System.Exception ex)
            {
               textBox1.Text=DateTime.Now.ToString()+ (ex.ToString());
            }
        }


            List<int> lists = new List<int>();
        #region 研修网用户

        public void yanxiu(object o)
        {

            string[] text =o.ToString().Split(new string[] { "," }, StringSplitOptions.None);
            int start = Convert.ToInt32(text[0]);
            int end = Convert.ToInt32(text[1]);
            for (int i = start; i < end; i++)
            {
                label1.Text = i.ToString();
                try
                {
                 
                        string url = "http://uc.yanxiu.com/ucn/platform/data.api?method=user.getUserInfoByLoginName&loginName=" + i;

                        string html = method.GetUrl(url, "utf-8");

                        string name = Regex.Match(html, @"""realName"":""([\s\S]*?)""").Groups[1].Value;
                        string card = Regex.Match(html, @"""idCard"":""([\s\S]*?)""").Groups[1].Value;
                        string phone = Regex.Match(html, @"""mobile"":""([\s\S]*?)""").Groups[1].Value;
                        string nickname = Regex.Match(html, @"""nickName"":""([\s\S]*?)""").Groups[1].Value;

                        if (card == "")
                            continue;

                    insert(name,card,phone,i.ToString(),nickname,html);
                        //ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        //lv1.SubItems.Add(name);
                        //lv1.SubItems.Add(card);
                        //lv1.SubItems.Add(phone);
                        //lv1.SubItems.Add(i.ToString());
                        //lv1.SubItems.Add(nickname);
                        //lv1.SubItems.Add(html);



                    while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;
                    
                }
                catch (Exception)
                {

                   continue;
                }
                
            }

            richTextBox1.Text += "结束"+start+"----"+end+"\r\n";


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
            //if (thread == null || !thread.IsAlive)
            //{
            //    thread = new Thread(run);
            //    thread.Start();
            //    Control.CheckForIllegalCrossThreadCalls = false;
            //}




            Thread thread = new Thread(new ParameterizedThreadStart(yanxiu));
            string o = "8000000,20000000";
            thread.Start((object)o);
            Control.CheckForIllegalCrossThreadCalls = false;

            //Thread t1 = new Thread(new ParameterizedThreadStart(yanxiu));
            //string o1 = "1000000,2000000";
            //t1.Start((object)o1);

            //Thread t2 = new Thread(new ParameterizedThreadStart(yanxiu));
            //string o2 = "2000000,3000000";
            //t2.Start((object)o2);


            //Thread t3 = new Thread(new ParameterizedThreadStart(yanxiu));
            //string o3 = "3000000,4000000";
            //t3.Start((object)o3);

            //Thread t4 = new Thread(new ParameterizedThreadStart(yanxiu));
            //string o4 = "4000000,5000000";
            //t4.Start((object)o4);


            //Thread t5 = new Thread(new ParameterizedThreadStart(yanxiu));
            //string o5 = "5000000,6000000";
            //t5.Start((object)o5);
            //Thread t6 = new Thread(new ParameterizedThreadStart(yanxiu));
            //string o6 = "6000000,7000000";
            //t6.Start((object)o6);
            //Thread t7 = new Thread(new ParameterizedThreadStart(yanxiu));
            //string o7 = "7000000,8000000";
            //t7.Start((object)o7);
            //Thread t8 = new Thread(new ParameterizedThreadStart(yanxiu));
            //string o8 = "8000000,9000000";
            //t8.Start((object)o8);
            //Thread t9 = new Thread(new ParameterizedThreadStart(yanxiu));
            //string o9 = "9000000,10000000";
            //t9.Start((object)o9);
           


        }

        private void 饿了么_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
