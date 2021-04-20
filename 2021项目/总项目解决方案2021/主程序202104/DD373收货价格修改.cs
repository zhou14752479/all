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
                for (int a = 0; a < Identifys.Count / 5; a++)
                {

                    string priceurl = "https://www.dd373.com/s-" + Identifys[5 * a].Groups[1].Value + "-" + Identifys[(5 * a) + 1].Groups[1].Value + "-" + Identifys[(5 * a) + 2].Groups[1].Value + "-" + Identifys[(5 * a) + 3].Groups[1].Value + "-0-0-jk5sj0-0-0-recycle-0-0-1-0-0-0.html";


                    string newprice = getprice(priceurl);
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                    lv1.SubItems.Add(names[5 * a].Groups[1].Value.Trim() + names[(5 * a) + 1].Groups[1].Value.Trim() + names[(5 * a) + 2].Groups[1].Value.Trim() + names[(5 * a) + 3].Groups[1].Value.Trim() + names[(5 * a) + 4].Groups[1].Value.Trim());

                    lv1.SubItems.Add("设置完成");
                    lv1.SubItems.Add(newprice);
                    Thread.Sleep(1000);
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
            string postdata = "Goods%5B0%5D%5BShopNumber%5D="+ shopNo + "&Goods%5B0%5D%5BTotalQuantity%5D="+ TotalQuantity + "&Goods%5B0%5D%5BUnitPrice%5D="+ UnitPriceByMoney + "&Goods%5B0%5D%5BMinQuantity%5D="+ MinQuantity + "&Goods%5B0%5D%5BMaxQuantity%5D="+ MaxQuantity;
            string ahtml = method.PostUrl(aurl, postdata, cookies, "utf-8", "application/x-www-form-urlencoded; charset=UTF-8", "https://goods.dd373.com/");
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
            cookies = "acw_tc=76b20f7116171889186127464e4d83bff26aab1ee3aa926dac5fa945a88fb1; SERVERID=a7219fc38e119fffac9ed962aad2f2b1|1617189823|1617189822; loginToken=23121d9f-7cf4-411c-8b8f-4ade720a839c; clientId=43cc571a-70ed-4c52-b6f5-77dcbe048756; Hm_lvt_b1609ca2c0a77d0130ec3cf8396eb4d5=1617188886; member_session=P%2bHfgCJ3uFRV2UZJk8jK4sygEZEZB%2bOWihIRjNa8UcUlS1%2fk9nrbgTNxbLB%2bqvwJ1Au04qWVy0Ie%2b9tPAGjbPhu%2bGpCigAAYCDtUaGIkenXAKo50t7Q6hXy9M6Al2f4NZ6EJxxRjpZH3%2bkKnSLcnrsK1LRA5MfVSrYmdVhPvEf0Tg1onTbrZQP1Y3uojvu6Zl5Ak%2fGjomJSL9J86l3jyS3I23ftfNI1hIUkhW9TycUNBO7IbYl7KnZkoGAh7nOAPaUP%2f8g1DQLCZ0ttkS1aH1G4FV3TYvoA%2f0J%2bvOZQQs0u8HhQGUjiUYJ09%2fWUNE5Zp%2f3jsTqsMJPi%2fxQqW%2fQPzQA%3d%3d; refreshToken=974ff9cf-d1ff-457b-9d43-5fcfbad8b193; login.dd373.com=5f24b243-a78d-4dae-9826-b531748032b4; goods.dd373.com=41a40532-3a4b-4a50-9858-4625f7650998; hb.dd373.com=7b23212d-5fbe-4e1c-83a7-01bfc596b8ef; point.dd373.com=3f492953-b6ed-4abd-af20-79cd557121ed; newpay.dd373.com=27ad0aea-e717-4a83-a298-696ceaa527d6; newuser.dd373.com=1592936d-fb7b-4090-8103-a3bae760bc01; thirdbind.dd373.com=b6b5c5d1-352a-47e7-85c5-5935bcf6f820; mission.dd373.com=103c058b-164a-482c-8e0a-41cf3a2d2a9c; order.dd373.com=03ebb39c-e31b-4e3a-9355-fdb61b3bfb9c";

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
