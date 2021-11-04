using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 主程序202104
{
    public partial class 发货易 : Form
    {
        public 发货易()
        {
            InitializeComponent();
        }

        private void 发货易_Load(object sender, EventArgs e)
        {

        }


        string cookie = "orderItemsDisplayType=title; orderTablePageSize=200; _ati=443554097427; acw_tc=2760822f16189648265152540e3ef93fb3f4b1f452205ec8cc59d0c74381d2; SESSION=86aefa82-093f-4402-bd39-ebc7d2dfdbd6";


        public Dictionary<string,string> gettels(string sb)
        {
           
            Dictionary<string, string> dics = new Dictionary<string, string>();
            string url = "https://a44.fahuoyi.com/orderDecrypt/decryptOrdersRest?isHistoryOrder=fals"+sb;
            string html = method.GetUrlWithCookie(url, cookie, "utf-8");
            MatchCollection ids = Regex.Matches(html, @"{""id"":([\s\S]*?),");
            MatchCollection names = Regex.Matches(html, @"""consignee_name"":""([\s\S]*?)""");
            MatchCollection tels = Regex.Matches(html, @"""mobile"":""([\s\S]*?)""");
            MatchCollection address = Regex.Matches(html, @"""street"":""([\s\S]*?)""");
            for (int a = 0; a < ids.Count; a++)
            {
                dics.Add(ids[a].Groups[1].Value,names[a].Groups[1].Value+"#"+tels[a].Groups[1].Value+"#"+address[a].Groups[1].Value);
            }
            return dics;
        }





        /// <summary>
        /// 订单信息
        /// </summary>
        public void run()
        {
          
                for (int page = 0; page <9999; page=page+200)
                {
                    try
                    {

                    string url = "https://a44.fahuoyi.com/order/listRest?shopId=1551861&shopType=TAOBAO&tab=5&orderSort=order&offset=" + page + "&max=200&timeType=order&sellerFlags=&orderMarks=";
                  

                    string html = method.GetUrlWithCookie(url,  cookie, "utf-8");

                    MatchCollection ids = Regex.Matches(html, @"{""id"":([\s\S]*?),");
                    MatchCollection titles = Regex.Matches(html, @"""buyerUsername"":""([\s\S]*?)""");
                    MatchCollection goods = Regex.Matches(html, @"""title"":""([\s\S]*?)""");
                   
                    MatchCollection danhaohtmls = Regex.Matches(html, @"ewaybillNos""([\s\S]*?)\]");

                    MatchCollection provinces = Regex.Matches(html, @"""province"":""([\s\S]*?)""");
                    MatchCollection citys = Regex.Matches(html, @"""city"":""([\s\S]*?)""");
                    MatchCollection districts = Regex.Matches(html, @"""district"":""([\s\S]*?)""");
                    MatchCollection beizhus = Regex.Matches(html, @"""sellerRemarks"":([\s\S]*?),");

                    if (titles.Count == 0)
                        break;


                    StringBuilder sb = new StringBuilder();
                    for (int a = 0; a < ids.Count/2; a++)
                    {
                        sb.Append("&orderIds="+ids[a*2].Groups[1].Value);
                    }

                    
                    Dictionary<string, string> dics = gettels(sb.ToString());

                    for (int j = 0; j < titles.Count; j++)
                    {
                        string value = dics[ids[j * 2].Groups[1].Value.Trim()];

                        string[] text = value.Split(new string[] { "#" }, StringSplitOptions.None);
                        string danhao= Regex.Match(danhaohtmls[j].Groups[1].Value, @"waybillNo"":""([\s\S]*?)""").Groups[1].Value;


                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(titles[j].Groups[1].Value.Trim());
                        lv1.SubItems.Add(goods[j].Groups[1].Value.Trim());
                        lv1.SubItems.Add(text[0]);
                        lv1.SubItems.Add(text[1]);
                        lv1.SubItems.Add(provinces[j].Groups[1].Value.Trim()+ citys[j].Groups[1].Value.Trim()+ districts[j].Groups[1].Value.Trim()+text[2]);
                        lv1.SubItems.Add(danhao);
                        lv1.SubItems.Add(beizhus[j].Groups[1].Value.Replace("\"",""));
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;


                    }
                    Thread.Sleep(1000);

                }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.ToString());
                    }

                }


            }


        Thread thread;
        bool zanting = true;
        bool status = true;
        string tab = "5";
        private void button6_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"Dwzy2DZ"))
            {

                return;
            }



            #endregion
           
            status = true;


            string path = AppDomain.CurrentDomain.BaseDirectory;

            try
            {
                StreamReader sr = new StreamReader(path + "cookie.txt", method.EncodingType.GetTxtType(path + "cookie.txt"));
                //一次性读取完 
                string texts = sr.ReadToEnd();

                cookie = Regex.Match(texts, @"cookie=([\s\S]*?)&").Groups[1].Value;
                textBox2.Text = cookie;
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存

            }

           
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            cookie = "acw_tc=2760776c16353887275355205ec86a6315a780207af8c76d60af780fa6145b; SESSION=cd3c25d9-157c-467f-945c-aa3087bf8161; _ati=133630949992";
            if (radioButton1.Checked == true)
            {
                tab = "2";
            }
            if (radioButton2.Checked == true)
            {
                tab = "5";
            }
            if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(run);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
          
        }
       



        private void button2_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            Process.Start(path + "helper.exe");
        }

        private void button1_Click(object sender, EventArgs e)
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
    }
}
