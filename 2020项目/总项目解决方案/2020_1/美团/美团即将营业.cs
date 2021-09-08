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
using myDLL;
namespace 美团
{
    public partial class 美团即将营业 : Form
    {
        public 美团即将营业()
        {
            InitializeComponent();
        }


        #region GET请求
        public static string GetUrl(string Url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.97 Safari/537.11";
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 13_6_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/8.0.4(0x1800042c) NetType/WIFI Language/zh_CN";
                WebHeaderCollection headers = request.Headers;
                headers.Add("clientversion: 2.16.1");
                headers.Add("myLat: 118.24239");
                headers.Add("openId: oJVP50IRqKIIshugSqrvYE3OHJKQ");
                headers.Add("uuid: 17ac1d2f96bc8-21fe451de940f0-0-0-17ac1d2f96bc8");
                headers.Add("token: o12WUQ68y4BX_6EHZHILSxIDU9kAAAAADA4AAC5y1B7soOXKI6GymX-V4sc1jBnEIsZ8tpQ8XAQprXVstWD6oAP4VluxhaeWUHXMUw");
                headers.Add("openIdCipher:AwQAAABJAgAAAAEAAAAyAAAAPLgC95WH3MyqngAoyM/hf1hEoKrGdo0pJ5DI44e1wGF9AT3PH7Wes03actC2n/GVnwfURonD78PewMUppAAAADiKABSEHqf6ddkKfPmtaxPf7h5fT5I7TIBL1SNaE66D+vjeYYWFzWcStaSbcTncER5tI+u6RodKxw==");
                request.Referer = "https://servicewechat.com/wxde8ac0a21135c07d/766/page-frame.html";
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


        Dictionary<string, string> dics = new Dictionary<string, string>();

        #region 获取所有城市ID
        public ArrayList getallcitys()
        {
            dics.Clear();
            string Url = "https://www.meituan.com/changecity/";
          
            string html = GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()

            MatchCollection ids = Regex.Matches(html, @"{""id"":([\s\S]*?),""name"":""([\s\S]*?)""");
            ArrayList lists = new ArrayList();

            foreach (Match item in ids)
            {
                if(!dics.ContainsKey(item.Groups[1].Value))
                {
                    lists.Add(item.Groups[1].Value);
                    dics.Add(item.Groups[1].Value, item.Groups[2].Value);
                }
                
            }

            return lists;
        }

        #endregion
        #region  全国
        public void getall()
        {
            ArrayList cityids = getallcitys();
            
            foreach (string cityid in cityids)
            {
               
                string cityname = dics[cityid];
                
                try
                {
                   

                        string url = "https://apimobile.meituan.com/group/v1/deal/search/suggest/"+cityid+"?riskLevel=71&optimusCode=10&uuid=17b68a713bfc8-97c5aecd898ef0-0-0-17b68a713bfc8&input="+ System.Web.HttpUtility.UrlEncode(textBox1.Text.Trim())+"&version_name=0&cateId=undefined&locateCityId=184&client=ipad&position=33.93974304199219%2C118.25344848632812&entrance=1&wifi-name=Tenda_0A0C50&wifi-strength=0.7821524143218994&wifi-cur=&wifi-mac=c8%3A3a%3A35%3A0a%3A0c%3A50&utm_content=17b68a713bfc8-97c5aecd898ef0-0-0-17b68a713bfc8&utm_term=0&utm_medium=ipad&appType=wx_xiaochengxu&source=1&language=zh-Hans";

                        string html = GetUrl(url);  //定义的GetRul方法 返回 reader.ReadToEnd()

                    MatchCollection htmls = Regex.Matches(html, @"exData([\s\S]*?)ignore");


                    toolStripStatusLabel1.Text = "正在采集：" + cityname + "获取到待判断个数" + htmls.Count.ToString();

                    if (htmls.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            continue;

                    for (int j = 0; j < htmls.Count; j++)

                    {

                        string ahtml = htmls[j].Groups[1].Value;
                        if (ahtml.Contains("即将营业"))
                        {
                            string id = Regex.Match(ahtml, @"_id"":([\s\S]*?),").Groups[1].Value;
                            string name= Regex.Match(ahtml, @"editorWord"":""([\s\S]*?)""").Groups[1].Value;
                            ListViewItem listViewItem = listView1.Items.Add((listView1.Items.Count + 1).ToString());
                            listViewItem.SubItems.Add(name);
                            listViewItem.SubItems.Add(id);
                            listViewItem.SubItems.Add(cityname);
                            listViewItem.SubItems.Add("即将营业");
                            listViewItem.SubItems.Add(gettel(id));


                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)
                                return;


                        }
                    }





                    Thread.Sleep(100);



                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

        }
        #endregion


        public string gettel(string id)
        {
            try
            {
                Thread.Sleep(1000);
                string url = "https://i.meituan.com/wrapapi/allpoiinfo?riskLevel=71&optimusCode=10&poiId=" + id + "&isDaoZong=true";
                string strhtml1 = GetUrl(url);
                string tel = Regex.Match(strhtml1, @"phone"":""([\s\S]*?)""").Groups[1].Value;
                return tel;
            }
            catch (Exception)
            {

                return "";
            }
        }
        Thread thread;
        bool zanting = true;
        bool status = true;
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html","utf-8");

            if (!html.Contains(@"abc147258"))
            {
                MessageBox.Show("");
                return;
            }



            #endregion
            status = true;



            Thread thread = new Thread(getall);
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
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

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;
            System.Diagnostics.Process.Start("https://www.meituan.com/meishi/" + listView1.SelectedItems[0].SubItems[2].Text+"/");
           
        }
    }
}
