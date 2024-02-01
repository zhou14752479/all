using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using myDLL;

namespace 主程序202401
{
    public partial class 百度酒店 : Form
    {
        public 百度酒店()
        {
            InitializeComponent();
        }
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测


            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"u7WSe"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

           

            #endregion
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }



        bool zanting = true;
        bool status = true;
        public void run()
        {
            try
            {
              
                for (int i = 0; i < 100; i++)
                {

                    string code = citycode[comboBox1.Text];
                    string url = "https://hotel.pae.baidu.com/hotel/xcx/list?req_from=fw_xcx&xcx_from=fw_xcx&hotel_sid=&source=&courierId=&promotionCom=&xfrom=&qid=&fr=&platform=web&pn="+i+"&rn=10&startDate="+DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")+ "&endDate="+DateTime.Now.AddDays(2).ToString("yyyy-MM-dd")+"&cityId="+ code + "&realCityId="+ code + "&query=%E9%85%92%E5%BA%97&bound=&_pfName=HOTEL_LIST_MAP";
                 
                    string html = method.GetUrl(url, "utf-8");
                   
           
                
                    MatchCollection uids = Regex.Matches(html, @"""uid"":""([\s\S]*?)""");
                    if (uids.Count == 0)
                    {
                        continue;
                    }

                    for (int a = 0; a < uids.Count; a++)
                    {
                        try
                        {
                            string aurl = "https://hotel.pae.baidu.com/hotel/xcx/detail?req_from=fw_xcx&xcx_from=fw_xcx&hotel_sid=&source=1081000910019156&courierId=&promotionCom=&xfrom=1081000910019156&qid=4471760522&fr=&platform=web&center_x=&center_y=&cityId=277&endDate=2024-01-08&realCityId=277&sfrom=&startDate=2024-01-07&uid=" + uids[a].Groups[1].Value +"&enterT=1704599055601&act=hotel_detail&type=hotel_detail&_pfName=HOTEL_DETAIL";
                            string ahtml = method.GetUrl(aurl,"utf-8");
                           
                           string name= Regex.Match(ahtml, @"""title"":""([\s\S]*?)""").Groups[1].Value;
                            string openYear = Regex.Match(ahtml, @"""openYear"":""([\s\S]*?)""").Groups[1].Value;
                            string phone = Regex.Match(ahtml, @"""phone"":""([\s\S]*?)""").Groups[1].Value;
                            string text = Regex.Match(ahtml, @"""text"":""([\s\S]*?)""").Groups[1].Value;
                            string price = Regex.Match(ahtml, @"""price"":([\s\S]*?),").Groups[1].Value;
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(name);
                            lv1.SubItems.Add(openYear);
                            lv1.SubItems.Add(phone);
                            lv1.SubItems.Add(text);
                            lv1.SubItems.Add(price);
                            if (listView1.Items.Count > 2)
                            {
                                this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                            }
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



                    Thread.Sleep(Convert.ToInt32(textBox1.Text) * 1000);

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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

        Dictionary<string, string> citycode= new Dictionary<string, string>();

        #region  获取城市ID

        public void GetcityId()
        {

            try
            {
                string url = "https://hotel.pae.baidu.com/hotel/xcx/city-list?req_from=fw_xcx&xcx_from=fw_xcx&hotel_sid=&source=1081000910019156&courierId=&promotionCom=&xfrom=1081000910019156&qid=8743974103&fr=&platform=web&act=hotel_city_list&_pfName=HOTEL_CITY_LIST";
                string html =method.GetUrl(url,"utf-8");
                MatchCollection cityIds = Regex.Matches(html, @"""code"":""([\s\S]*?)""");
                MatchCollection names = Regex.Matches(html, @"""name"":""([\s\S]*?)""");
                for (int i = 0; i < cityIds.Count; i++)
                {
                    comboBox1.Items.Add(names[i].Groups[1].Value);
                    citycode.Add(names[i].Groups[1].Value, cityIds[i].Groups[1].Value);
                }


            }

            catch (System.Exception ex)
            {
                ex.ToString();
            }

        }

        #endregion

        private void 百度酒店_Load(object sender, EventArgs e)
        {
            GetcityId();
        }
    }
}
