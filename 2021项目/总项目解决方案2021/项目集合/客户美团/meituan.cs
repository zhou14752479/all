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

namespace 客户美团
{
    public partial class meituan : Form
    {
        public meituan()
        {
            InitializeComponent();
        }

        string cookie = "";
        string cateid = "1";
        #region GET请求2
        public string GetUrl(string Url)
        {
            try
            {


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                string COOKIE = cookie;
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.11(0x17000b21) NetType/4G Language/zh_CN";
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("openIdCipher", "AwQAAABJAgAAAAEAAAAyAAAAPLgC95WH3MyqngAoyM/hf1hEoKrGdo0pJ5DI44e1wGF9AT3PH7Wes03actC2n/GVnwfURonD78PewMUppAAAADhS4d+zREPZw1PQNF/0Zp8SLSbtYsmCKZFYbIjL5Ty7FJZwQ/bkMIGOGHHGqk1Nld5+rcxtuifNmA==");
                request.Referer = "https://servicewechat.com/wxde8ac0a21135c07d/350/page-frame.html";
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

        #region 获取区域
        public ArrayList getareas(string cityid)
        {
            ArrayList lists = new ArrayList();
            string Url = "https://i.meituan.com/wrapapi/search/filters?riskLevel=71&optimusCode=10&ci=" + cityid;

            string html = GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()
            //]},{"id":2,"name":"徐汇区"
            MatchCollection areas = Regex.Matches(html, @"\]\},\{""id"":([\s\S]*?),([\s\S]*?)""name"":""([\s\S]*?)""");

            for (int i = 0; i < areas.Count; i++)
            {

                if (areas[i].Groups[2].Value.Contains("areaId"))

                {

                    lists.Add(areas[i].Groups[1].Value);
                }

            }

            return lists;
        }

        #endregion
        #region  获取城市ID

        public string GetcityId(string city)
        {

            try
            {
                string url = "https://apimobile.meituan.com/group/v1/area/search/" + System.Web.HttpUtility.UrlEncode(city.Replace("市", ""));
                string html = GetUrl(url);
                string cityId = Regex.Match(html, @"""cityId"":([\s\S]*?),").Groups[1].Value;

                return cityId;
            }

            catch (System.Exception ex)
            {
                return ex.ToString();
            }

        }

        #endregion

        #region  主程序
        public void run()
        {
            int count = 0;

            try
            {
                string[] citys = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string city in citys)
                {


                    string cityId = GetcityId(city);

                    ArrayList areas = getareas(cityId);

                    foreach (string areaId in areas)
                    {
                   
                            string[] catenames = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                            foreach (string catename in catenames)
                            {

                                switch (catename)
                                {
                                    case "餐饮美食":
                                        cateid = "1";
                                        break;
                                    case "小吃快餐":
                                        cateid = "36";
                                        break;
                                    case "丽人":
                                        cateid = "22";
                                        break;
                                    case "休闲娱乐":
                                        cateid = "2";
                                        break;
                                    case "饮品":
                                        cateid = "21329";
                                        break;
                                    case "蛋糕甜点":
                                        cateid = "11";
                                        break;
                                    case "美发":
                                        cateid = "74";
                                        break;
                                    case "美容美体":
                                        cateid = "76";
                                        break;
                                    case "婚纱摄影":
                                        cateid = "20178";
                                        break;
                                    case "汽车":
                                        cateid = "27";
                                        break;
                                    case "教育":
                                        cateid = "20285";
                                        break;
                                    case "KTV":
                                        cateid = "10";
                                        break;
                                    case "足疗":
                                        cateid = "52";
                                        break;
                                    case "洗浴汗蒸":
                                        cateid = "112";
                                        break;
                                    case "宠物医院":
                                        cateid = "20691";
                                        break;
                                    case "瑜伽舞蹈":
                                        cateid = "220";
                                        break;
                                    case "咖啡酒吧":
                                        cateid = "41";
                                        break;
                                    case "医疗":
                                        cateid = "20274";
                                        break;
                                }
                                for (int i = 0; i < 1001; i = i + 100)

                                {
                                    string Url = "https://m.dianping.com/mtbeauty/index/ajax/shoplist?token=&cityid=" + cityId + "&cateid=22&categoryids=" + cateid + "&lat=33.94114303588867&lng=118.2479019165039&userid=&uuid=&utm_source=meituan-wxapp&utmmedium=&utmterm=&utmcontent=&versionname=&utmcampaign=&mock=0&openid=oJVP50IRqKIIshugSqrvYE3OHJKQ&mtlite=false&start=" + i + "&limit=100&areaid=" + areaId+ "&distance=&subwaylineid=&subwaystationid=&sort=2";

                                    string html = method.GetUrl(Url, "utf-8");  //定义的GetRul方法 返回 reader.ReadToEnd()

                                    MatchCollection names = Regex.Matches(html, @"""shopName"":""([\s\S]*?)""");
                                    MatchCollection address = Regex.Matches(html, @"""address"":""([\s\S]*?)""");
                                    MatchCollection phone = Regex.Matches(html, @"""phone"":""([\s\S]*?)""");
                                    MatchCollection cate = Regex.Matches(html, @"mainCategoryName"":""([\s\S]*?)""");
                                    MatchCollection ranks = Regex.Matches(html, @"""shopPower"":([\s\S]*?),");

                                    if (names.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                        break;

                                    for (int j = 0; j < names.Count; j++)

                                    {

                                        string shouji = "";
                                        string guhua = "";
                                        string[] tels = phone[j].Groups[1].Value.Split(new string[] { "/" }, StringSplitOptions.None);
                                        if (tels.Length == 1)
                                        {
                                            if (phone[j].Groups[1].Value.Contains("-"))
                                            {
                                                guhua = phone[j].Groups[1].Value;
                                            }
                                            else
                                            {
                                                shouji = phone[j].Groups[1].Value;
                                            }
                                        }
                                        if (tels.Length == 2)
                                        {
                                            if (phone[j].Groups[1].Value.Contains("-"))
                                            {
                                                if (tels[0].Contains("-"))
                                                {
                                                    guhua = tels[0];
                                                    shouji = tels[1];
                                                }
                                                else
                                                {
                                                    guhua = tels[1];
                                                    shouji = tels[0];
                                                }
                                            }
                                            else
                                            {
                                                guhua = "";
                                                shouji = tels[0];
                                            }
                                        }

                                        ListViewItem listViewItem = listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                        listViewItem.SubItems.Add(names[j].Groups[1].Value);
                                        listViewItem.SubItems.Add(shouji);
                                        listViewItem.SubItems.Add(guhua);
                                        listViewItem.SubItems.Add(address[j].Groups[1].Value);

                                        listViewItem.SubItems.Add(comboBox2.Text);
                                        listViewItem.SubItems.Add(ranks[j].Groups[1].Value);
                                        listViewItem.SubItems.Add(cate[j].Groups[1].Value);

                                        //Thread.Sleep(300);
                                        count = count + 1;
                                        label4.Text = count.ToString();
                                        if (status == false)
                                            return;
                                    }



                                    Thread.Sleep(1000);
                                }
                            }

                        }
                    }
                }

            catch (System.Exception ex)
            {
                ex.ToString();
            }



        }



        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            ProvinceCity.ProvinceCity.BindProvince(comboBox1);
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
               
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProvinceCity.ProvinceCity.BindCity(comboBox1, comboBox2);
        }

       


        bool status = true;
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html");

            if (!html.Contains(@"YVoWQ"))
            {
                MessageBox.Show("");
                return;
            }



            #endregion

            #region 通用检测

            string ahtml = GetUrl("http://139.129.92.113/");

            if (!ahtml.Contains(@"siyisoft"))
            {

                return;
            }



            #endregion
            status = true;
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("区域或行业为空");
                return;
            }

            if (thread == null || !thread.IsAlive)
            {
              
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            else
            {
                status = false;
            }
            
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
           
        }

        private void button6_Click(object sender, EventArgs e)
        {
           

          
        }

      
       


       
        private void button2_Click(object sender, EventArgs e)
        {
           method.ListviewToTxt(listView1, 2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text.Contains("上海"))
            {
                textBox1.Text += "上海";
                return;
            }


            if (textBox1.Text.Contains(comboBox2.Text))
            {
                MessageBox.Show(comboBox2.Text + "：请勿重复添加", "重复添加错误");
                return;
            }
            textBox1.Text += comboBox2.Text + "\r\n";
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Contains(comboBox3.Text))
            {
                MessageBox.Show(comboBox3.Text + "：请勿重复添加", "重复添加错误");
                return;
            }
            textBox2.Text += comboBox3.Text + "\r\n";
        }
        private Point mPoint = new Point();
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            
        }
    }
}
