using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 足球网站数据对比
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString()+"000";
        }
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()

        {
           
                string url = "https://www.biying16.mobi/bv_in_play/v2/zh-cn/6/in_play/600?marketTypeIds%5B600%5D=68&pIds%5B600%5D=113" ;
                string html = method.GetUrl(url, "utf-8"); //获取比赛ID
           
            MatchCollection matches = Regex.Matches(html, @"\{""eId"":([\s\S]*?),");
            

                for (int i = 0; i< matches.Count; i++)
                {
                try
                {
                    string saipanUrl = "https://www.biying16.mobi/bv_event_level/zh-cn/6/coupons/" + matches[i].Groups[1].Value + "/4894?t=" + GetTimeStamp();
                    string saiguoUrl = "https://www.biying16.mobi/bv_event_level/zh-cn/6/coupons/" + matches[i].Groups[1].Value + "/4892?t=" + GetTimeStamp();

                    //赛果
                    string saiguohtml = method.GetUrl(saiguoUrl, "utf-8");
                    MatchCollection sgvalues = Regex.Matches(saiguohtml, @"""pr"":([\s\S]*?),");


                    //赛盘
                    string saipanhtml = method.GetUrl(saipanUrl, "utf-8");
                    MatchCollection spvalues = Regex.Matches(saipanhtml, @"""pr"":([\s\S]*?),");

                    Match duiwu = Regex.Match(saiguohtml, @"""event_desc"":""([\s\S]*?)v([\s\S]*?)""");


                    ListViewItem lv = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    lv.SubItems.Add(duiwu.Groups[1].Value);
                    lv.SubItems.Add(sgvalues[0].Groups[1].Value);
                    lv.SubItems.Add(spvalues[0].Groups[1].Value);
                    lv.SubItems.Add(spvalues[2].Groups[1].Value);


                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(duiwu.Groups[2].Value);
                    lv1.SubItems.Add(sgvalues[1].Groups[1].Value);
                    lv1.SubItems.Add(spvalues[1].Groups[1].Value);
                    lv1.SubItems.Add(spvalues[3].Groups[1].Value);

                }
                catch 
                {

                    continue;
                }




            }


          

        }
              
               

            

        
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            Thread thread1 = new Thread(new ThreadStart(run));
            thread1.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
