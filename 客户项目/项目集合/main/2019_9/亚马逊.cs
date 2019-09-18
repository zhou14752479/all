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

namespace main._2019_9
{
    public partial class 亚马逊 : Form
    {
        public 亚马逊()
        {
            InitializeComponent();
        }

        bool zanting = true;


        #region  亚马逊
      
        public void run()
        {
              
            try
            {
                string cookie = "session-id=143-0185990-5724411; session-id-time=2082787201l; i18n-prefs=USD; lc-main=zh_CN; sp-cdn=\"L5Z9:CN\"; x-wl-uid=1H0f9i1TwtBOaGZWHgNwGFhMtYB3tStDNALo7fgL98l6Ef94MBcomvkTbHwnD7wIB4QyIzNQ953M=; ubid-main=134-5759749-7449164; session-token=AXzUwYG1RynqG2FvtqiZABMDnzoqhB7zkspKgzIccY9EjB4GOdVJoZ7WtSziZ4i1Agko46WDhgkdrOWTtCAwaYxo7KgRlS6mpSWzQcdK4fDXPpdK+cK2/jKaEC8rpoRWyiGGD1IDY12EqKxreKpbx5XpX0Km/YZVoKpt5umUASSDn5o0IPHp76qemmpKWmc04SuncBhygN4++HQIbfXuYWk6Z02BGp56xTo3xZRG9iXpNujFsrifcmWkgCDs6XnI; csm-hit=tb:XBA71XNDZZWCA0DA5ZBM+s-K6GRPKFH0TTJYAK333XY|1568599234872&t:1568599234872&adb:adblk_no";
             string[] urls = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string URL in urls)
                {

                    for (int i = 1; i < 999; i++)
                    {

                        string URL1 = URL + "&pageNumber=" + i;
                        string strhtml = method.gethtml(URL1, cookie,"utf-8");

                        textBox3.Text = URL1;
                        MatchCollection  items = Regex.Matches(strhtml, @"<div id=""customer_review([\s\S]*?)</span></div></div></ul>");
                        MessageBox.Show(items.Count.ToString());
                        foreach (Match item in items)
                        {
                            if (item.Groups[1].Value.Contains(textBox2.Text.Trim()))
                            {
                                MatchCollection dates = Regex.Matches(item.Groups[1].Value, @"review-date"">([\s\S]*?)</span>");
                                for (int j = 0; j < dates.Count; j++)
                                {
                                    ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());

                                    //listViewItem.SubItems.Add(URL);
                                    listViewItem.SubItems.Add(dates[j].Groups[1].Value);
                                }
                               
                            }
                           
                        }


                     

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                        Thread.Sleep(1000);
                    }

                }




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        #endregion
        private void 亚马逊_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }
    }
}
