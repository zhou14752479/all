using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 主程序202103
{
    public partial class 裕丰ERP : Form
    {
        public 裕丰ERP()
        {
            InitializeComponent();
        }
        string areaid = "62";
        string tags = "";
        string cookie = "";
        

        public string getimgs(string id)
        {
            try
            {
                string url = "http://121.14.195.7/house/house/getImgRows?house_code=" + id + "&page=1&limit=20";
                string html = method.GetUrlWithCookie(url, cookie, "utf-8");
              
                MatchCollection imgs = Regex.Matches(html, @"""url_original"": ""([\s\S]*?)""");

                StringBuilder sb = new StringBuilder();
                foreach (Match img in imgs)
                {
                    sb.Append(img.Groups[1].Value + "  \r\n");
                }

                return sb.ToString();
            }
            catch (Exception)
            {

                return "";
            }

        }
        #region 主程序
        public void run()
        {
            /*
             各区楼盘
本区成功激活盘
近3天激活盘
近2天新收盘
附近的激活盘
*/

            switch (comboBox1.Text)
            {
                case "各区楼盘":
                    tags = "";
                    break;
                case "本区成功激活盘":
                    tags = "12";
                    break;
                case "近3天激活盘":
                    tags = "7";
                    break;
                case "近2天新收盘":
                    tags = "8";
                    break;
                case "附近的激活盘":
                    tags = "9";
                    break;
            }

            areaid = textBox3.Text.Trim();

            for (int i = Convert.ToInt32(textBox1.Text); i < Convert.ToInt32(textBox2.Text); i++)
            {

                string keyword = System.Web.HttpUtility.UrlEncode(textBox5.Text);

                string url = "http://121.14.195.7/house/house/searchDo";

                string postdata = "page="+i+"&limit=20&number_code=4221&search%5Bfloor%5D%5B0%5D%5Bfield_key_name%5D=floor&search%5Bfloor%5D%5B0%5D%5Boption_value%5D=0%E5%B1%82%E8%87%B30%E5%B1%82&search%5Bfloor%5D%5B0%5D%5Bid%5D=0~0&search%5Broom%5D%5B0%5D%5Bfield_key_name%5D=room&search%5Broom%5D%5B0%5D%5Bid%5D=-1&search%5Broom%5D%5B0%5D%5Boption_value%5D=-1&search%5Bis_formal%5D%5B0%5D%5Bfield_key_name%5D=is_formal&search%5Bis_formal%5D%5B0%5D%5Bid%5D=1&search%5Bis_formal%5D%5B0%5D%5Boption_value%5D=is_formal&type_id=&city_id=16&phone_search=&like_search%5Btype%5D=1&like_search%5Boption_value%5D="+keyword+"&district_id="+ areaid + "&plate=&road=&plot_id=&tags="+tags+"&time_type%5Btime_search%5D%5Bfield_key_name%5D=create_time&time_type%5Btime_search%5D%5Boption_value%5D=+~+&font_file_index=0&is_perfect=&jz_city_id=";
                
                string html = method.PostUrl(url,postdata,cookie ,"utf-8", "application/x-www-form-urlencoded;", "http://121.14.195.7/house/house/search?tags=");
               
                MatchCollection codes = Regex.Matches(html, @"""house_code"":""([\s\S]*?)""");
                MatchCollection address = Regex.Matches(html, @"""full_address"":""([\s\S]*?)""");
                MatchCollection address2 = Regex.Matches(html, @"""address_text"":""([\s\S]*?)""");


                MatchCollection rooms = Regex.Matches(html, @"""room"":""([\s\S]*?)""");
                MatchCollection halls = Regex.Matches(html, @"""hall"":""([\s\S]*?)""");
                MatchCollection toilets = Regex.Matches(html, @"""toilet"":""([\s\S]*?)""");
                MatchCollection mianjis = Regex.Matches(html, @"""actually_area"":([\s\S]*?),");
                for (int j = 0; j < codes.Count; j++)
                {

                    try
                    {
                        if (!address2[j].Groups[1].Value.Contains("封盘"))
                        {
                            string aurl = "http://121.14.195.7/house/house/detailDoCopy?house_code=" + codes[j].Groups[1].Value + "&font_file_index=8";
                           
                            string ahtml = method.GetUrlWithCookie(aurl,cookie, "utf-8");
                          
                            Match price1 = Regex.Match(ahtml, @"""price"":([\s\S]*?),");
                            Match price2 = Regex.Match(ahtml, @"""rental_price"":([\s\S]*?),");
                            Match yezhuname = Regex.Match(ahtml, @"""name"":([\s\S]*?),");
                            Match yezhutel = Regex.Match(ahtml, @"""mobile_number"":([\s\S]*?),");

                            Match active_record = Regex.Match(ahtml, @"""active_record"":([\s\S]*?)\]");
                            Match remark_list = Regex.Match(ahtml, @"""remark_list"":([\s\S]*?)\]");
                           
                           
                            MatchCollection jihuos= Regex.Matches(active_record.Groups[1].Value, @"""remark"":([\s\S]*?),");
                            MatchCollection remarks = Regex.Matches(remark_list.Groups[1].Value, @"""remarks"":""([\s\S]*?)""");
                            MatchCollection create_dates = Regex.Matches(remark_list.Groups[1].Value, @"""create_date"":""([\s\S]*?)""");

                            StringBuilder remarksb = new StringBuilder();
                            foreach (Match jihuo in jihuos)
                            {
                                remarksb.Append(method.Unicode2String(jihuo.Groups[1].Value.Replace("\"", ""))+"  \r\n");
                            }


                            StringBuilder remarksb2 = new StringBuilder();
                            for (int a = 0; a < remarks.Count; a++)
                            {
                                try
                                {
                                    remarksb2.Append( method.Unicode2String(create_dates[a].Groups[1].Value.Replace("\"", "")) +"："+ method.Unicode2String(remarks[a].Groups[1].Value.Replace("\"", "")) + "  \r\n");
                                }
                                catch (Exception ex)
                                {
                                   
                                    continue;
                                }
                               
                            }

                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count+1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(method.Unicode2String(address[j].Groups[1].Value));
                            lv1.SubItems.Add(price1.Groups[1].Value.Replace("\"",""));
                            lv1.SubItems.Add(price2.Groups[1].Value);
                            lv1.SubItems.Add(rooms[j].Groups[1].Value+"室"+ halls[j].Groups[1].Value+"厅"+ toilets[j].Groups[1].Value+"卫");
                            lv1.SubItems.Add(mianjis[j].Groups[1].Value);
                            lv1.SubItems.Add(method.Unicode2String(yezhuname.Groups[1].Value.Replace("\"", "")));
                            lv1.SubItems.Add(yezhutel.Groups[1].Value.Replace("\"", ""));
                            lv1.SubItems.Add(remarksb.ToString());
                            lv1.SubItems.Add(remarksb2.ToString());
                            Thread.Sleep(500);
                            lv1.SubItems.Add(getimgs(codes[j].Groups[1].Value));
                            lv1.SubItems.Add("");
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)
                                return;
                            Thread.Sleep(500);
                        }
                    }
                    catch (Exception ex)
                    {
                       

                        continue;
                    }


                }
                Thread.Sleep(1000);
            }

        }

        #endregion

        Thread thread;
        bool zanting = true;
        bool status = true;
        private void 裕丰ERP_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            cookie = textBox4.Text.Trim();
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
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

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            Process.Start(path+ "获取cookie.exe");
        }

        private void 裕丰ERP_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();

        }
    }
}
