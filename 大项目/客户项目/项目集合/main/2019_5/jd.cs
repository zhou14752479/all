using System;
using System.Collections;
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

namespace main._2019_5
{
    public partial class jd : Form
    {
        public jd()
        {
            InitializeComponent();
        }

        public void run()
        {
            try
            {
                for (int i = 0; i < 400; i = i + 20)
                {

                    string url = "https://list.jd.com/list.html?cat=737,794,870";
                    string html = method.GetUrl(url, "utf-8");
                   
                    MatchCollection goodids = Regex.Matches(html, @"<img  data-sku=""([\s\S]*?)""");

                    
                    for (int j = 0; j < goodids.Count; j++)
                    {
                        string URL = "https://item.jd.com/" + goodids[j].Groups[1].Value + ".html";
                        string strhtml = method.GetUrl(URL, "utf-8");
                        Match  shopid = Regex.Match(strhtml, @"data-vid=""([\s\S]*?)""");
                        Match  hangye = Regex.Match(strhtml, @"mbNav-3"">([\s\S]*?)<");

                        MatchCollection items= Regex.Matches(strhtml, @"compare\/([\s\S]*?)-([\s\S]*?)-");

                        StringBuilder sb = new StringBuilder();
                        for (int a = 0; a < items.Count; a++)
                        {
                            sb.Append("J_"+items[a].Groups[2].Value+",");
                        }
                        string priceUrl = "https://p.3.cn/prices/mgets?skuIds="+sb.ToString()+"&type=1&callback=jsonp1557817529049&_=1557817529050";

                        MatchCollection pricees = Regex.Matches(strhtml, @"""p"":""([\s\S]*?)""");


                        for (int b = 0; b < pricees.Count; b++)
                        {
                            ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                            listViewItem.SubItems.Add(items[b].Groups[2].Value);
                            listViewItem.SubItems.Add(pricees[b].Groups[1].Value);
                        }




                    }

                   



                    //MatchCollection goodids = Regex.Matches(html, @"<img  data-sku=""([\s\S]*?)""");





                    //ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                    //listViewItem.SubItems.Add(URL);

                    //if (this.listView1.Items.Count > 2)
                    //{
                    //    this.listView1.EnsureVisible(this.listView1.Items.Count - 1);
                    //}

                    //Application.DoEvents();
                    //Thread.Sleep(1000);





                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void jd_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }
    }
}
