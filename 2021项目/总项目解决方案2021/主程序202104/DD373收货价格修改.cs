using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
using Microsoft.VisualBasic;
namespace 主程序202104
{
    public partial class DD373收货价格修改 : Form
    {
        public DD373收货价格修改()
        {
            InitializeComponent();
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.inipath);
        }

        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
            return temp.ToString();
        }

        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public bool ExistINIFile()
        {
            return File.Exists(inipath);
        }

        public string cookies { get; set; }

        public string getprice(string url)
        {
            string html = method.GetUrlWithCookie(url, cookies, "utf-8");
            string price = Regex.Match(html, @"clear"">1元=([\s\S]*?)金").Groups[1].Value;
            return price;
        }

        public void getgoods()
        {
            for (int page = 1; page < 999; page++)
            {


                string url = "https://goods.dd373.com/Api/Receive/UserCenter/List?LastId=&GoodsType=&Status=-1&PageIndex=" + page + "&PageSize=80";
                string html = method.GetUrlWithCookie(url, cookies, "utf-8");
               
                MatchCollection uids = Regex.Matches(html, @"LastId"":""([\s\S]*?)""");
                MatchCollection types = Regex.Matches(html, @"GoodsType"":""([\s\S]*?)""");


                MatchCollection ShopNos = Regex.Matches(html, @"ShopNo"":""([\s\S]*?)""");
                MatchCollection TotalQuantitys = Regex.Matches(html, @"TotalQuantity"":([\s\S]*?)\.");
                MatchCollection MinQuantitys = Regex.Matches(html, @"MinQuantity"":([\s\S]*?)\.");
                MatchCollection MaxQuantitys = Regex.Matches(html, @"MaxQuantity"":([\s\S]*?)\.");

                if (uids.Count == 0)
                    break;


                StringBuilder sb = new StringBuilder();
                sb.Append("{\"GetInfoSearches\":[");
                for (int i = 0; i < uids.Count; i++)
                {
                    sb.Append("{\"LastId\":\"" + uids[i].Groups[1].Value + "\",\"GoodsTypeId\":\"" + types[i].Groups[1].Value + "\"},");

                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]}");
                string aurl = "https://game.dd373.com/Api/Game/GetGameInfoDataListByIds";
                string postdata = sb.ToString();
                string ahtml = method.PostUrl(aurl, postdata, cookies, "utf-8", "application/json; charset=UTF-8", "https://goods.dd373.com/");
                MatchCollection Identifys = Regex.Matches(ahtml, @"tify"":""([\s\S]*?)""");

              
                MatchCollection names = Regex.Matches(ahtml, @"Name"":""([\s\S]*?)""");
                for (int a = 0; a < Identifys.Count / 4; a++)
                {

                    //string priceurl = "https://www.dd373.com/s-" + Identifys[5 * a].Groups[1].Value + "-" + Identifys[(5 * a) + 1].Groups[1].Value + "-" + Identifys[(5 * a) + 2].Groups[1].Value + "-" + Identifys[(5 * a) + 3].Groups[1].Value + "-0-0-jk5sj0-0-0-recycle-0-0-1-0-0-0.html";
                    string priceurl = "https://www.dd373.com/s-7u2tcu-q6m7m8-"+ Identifys[(4 * a) + 2].Groups[1].Value + "-qqnsrh-0-0-qqnsrh-0-0-recycle-0-0-1-0-0-0.html";


                    //textBox1.Text = priceurl;
                    //MessageBox.Show(priceurl);
                    string newprice = getprice(priceurl);
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                    lv1.SubItems.Add(names[4 * a].Groups[1].Value.Trim() + names[(4 * a) + 1].Groups[1].Value.Trim() + names[(4* a) + 2].Groups[1].Value.Trim() + names[(4* a) + 3].Groups[1].Value.Trim() );

                    lv1.SubItems.Add("设置完成");
                    lv1.SubItems.Add(newprice);
                    Thread.Sleep(2000);
                    if (newprice != "")
                    {
                        textBox1.Text = changePrice(ShopNos[a].Groups[1].Value, TotalQuantitys[a].Groups[1].Value, newprice, MinQuantitys[a].Groups[1].Value, MaxQuantitys[a].Groups[1].Value);

                    }

                }

            }
          
        }


        public string changePrice(string shopNo,string TotalQuantity,string UnitPriceByMoney, string MinQuantity,string MaxQuantity)
        {
            string aurl = "https://goods.dd373.com/Api/Receive/UserCenter/Save";
            //string postdata = "Goods%5B0%5D%5BShopNumber%5D="+ shopNo + "&Goods%5B0%5D%5BTotalQuantity%5D="+ TotalQuantity + "&Goods%5B0%5D%5BUnitPrice%5D="+ UnitPriceByMoney + "&Goods%5B0%5D%5BMinQuantity%5D="+ MinQuantity + "&Goods%5B0%5D%5BMaxQuantity%5D="+ MaxQuantity;

            string postdata = "{\"Goods\":[{\"ShopNumber\":\"" + shopNo + "\",\"TotalQuantity\":" + TotalQuantity + ",\"UnitPrice\":" + UnitPriceByMoney + ",\"MinQuantity\":" + MinQuantity + ",\"MaxQuantity\":"+ MaxQuantity + "}]}";

           
            string ahtml = method.PostUrl(aurl, postdata, cookies, "utf-8", "application/json; charset=UTF-8", "https://goods.dd373.com/");
            //MessageBox.Show(ahtml);
            return ahtml;

           
        }


       
      
        private void DD373收货价格修改_Load(object sender, EventArgs e)
        {
            

        }


        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (ExistINIFile())
            {
                string key = IniReadValue("values", "key");
                string secret = IniReadValue("values", "secret");
           
                if (secret != method.GetMD5(method.GetMD5(method.GetMacAddress())+ "siyiruanjian") || key != method.GetMD5(method.GetMacAddress()))
                {
                    label5.Visible = true;
                    linkLabel1.Visible = true;
                    label5.Text = method.GetMD5(method.GetMacAddress());
                    string str = Interaction.InputBox("输入激活码", "激活软件", "输入激活码", -1, -1);
                    if (str == method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian"))
                    {
                        IniWriteValue("values", "key", method.GetMD5(method.GetMacAddress()));
                        IniWriteValue("values", "secret", method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian"));
                        MessageBox.Show("激活成功");
                        label5.Visible = false;
                        linkLabel1.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("激活码错误");
                        return;
                    }
                }
              
            }
            else
            {
                label5.Visible = true;
                linkLabel1.Visible = true;
                label5.Text = method.GetMD5(method.GetMacAddress());
                string str = Interaction.InputBox("输入激活码", "激活软件", "输入激活码", -1, -1);
                if (str == method.GetMD5(method.GetMD5(method.GetMacAddress()) +"siyiruanjian"))
                {
                    IniWriteValue("values", "key", method.GetMD5(method.GetMacAddress()));
                    IniWriteValue("values", "secret", method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian"));
                    MessageBox.Show("激活成功");
                    label5.Visible = false;
                    linkLabel1.Visible = false;
                }
                else
                {
                    MessageBox.Show("激活码错误");
                    return;
                }
            }
           






            timer1.Interval = Convert.ToInt32(textBox4.Text)*1000*60;
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"mf7VpZ"))
            {
                MessageBox.Show("");
                return;
            }



            #endregion

            string path = AppDomain.CurrentDomain.BaseDirectory;
            cookies = "Hm_lvt_b1609ca2c0a77d0130ec3cf8396eb4d5=1689387327; Hm_lpvt_b1609ca2c0a77d0130ec3cf8396eb4d5=1689387327; loginToken=c131c6b8-430d-45ea-95c9-6cd5b250fdad; refreshToken=dbd8b395-fa9b-40af-acd7-e11e4136d78e; clientId=7d20c02b-1134-b232-9aee-ad6755eb1516; new_pc_rememberPassword=1; login.dd373.com=fad0e5bd-66e2-4583-93cc-7b468953b022; userName_cc=laoyou1; newpay.dd373.com=4d0de003-caca-4e99-9649-041f57c0862d; newuser.dd373.com=bdeb9f64-78d3-400f-942f-3b0af99a46f1; point.dd373.com=5ea43e62-23d2-4793-a5bc-d59ef9c82a48; order.dd373.com=789d911d-b964-4714-8d5f-95227f890273; thirdbind.dd373.com=df6f7235-4dd7-410d-84e2-f0878ec7a879; menu.dd373.com=bd27ea97-9e31-4abe-9d3b-3e81740550b0; mission.dd373.com=c3cdb8f9-4375-4de6-8242-3ca482e8504a; goods.dd373.com=d72bcce2-b9f0-4ace-84a3-dacc3650ea92; hb.dd373.com=13ce7d0e-712a-488b-9e42-2abd72e55dcd; imservice.dd373.com=341f7904-9009-4621-ad8b-be73bbd9fbd7; RUNREADSUM=%7B%22orderNum%22%3A0%2C%22shopNum%22%3A0%2C%22serverNum%22%3A0%2C%22recentNum%22%3A0%2C%22allMum%22%3A0%7D";

            try
            {
                //cookie = IniReadValue("values", "cookie");
                //state = IniReadValue("values", "state");

                StreamReader sr = new StreamReader(path + "cookie.txt", method.EncodingType.GetTxtType(path + "cookie.txt"));
                //一次性读取完 
                string texts = sr.ReadToEnd();

                cookies = Regex.Match(texts, @"cookie=([\s\S]*?)&").Groups[1].Value;
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        

          

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getgoods);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            timer1.Start();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            Process.Start(path + "helper.exe");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getgoods);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void DD373收货价格修改_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(label5.Text); //复制
        }
    }
}
