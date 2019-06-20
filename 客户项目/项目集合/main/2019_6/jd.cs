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

namespace main._2019_6
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

               // string url = "https://mall.jd.com/index-168080.html";
               string url = "https://mall.jd.com/index-145893.html";
                    string html = method.GetUrl(url, "utf-8");

                    Match match = Regex.Match(html, @"search-"" \+ ([\s\S]*?) ");

                string URL = "https://mall.jd.com/view_search-"+ match.Groups[1].Value + "-0-99-1-24-1.html" ;

                string html2= method.gethtml(URL,"", "utf-8");
                Match id1 = Regex.Match(html2, @"m_render_pageInstance_id=""([\s\S]*?)""");
                Match id2 = Regex.Match(html2, @"m_render_layout_instance_id=""([\s\S]*?)""");
                Match id3 = Regex.Match(html2, @"SearchList-([\s\S]*?) ");

                Match shopid = Regex.Match(html2, @"shopId = ""([\s\S]*?)""");
                Match id5 = Regex.Match(html2, @"m_render_app_id=""([\s\S]*?)""");
                Match id6 = Regex.Match(html2, @"vender_id"" value=""([\s\S]*?)""");
               // string zurl = "https://module-jshop.jd.com/module/allGoods/goods.html?callback=jQuery4333181&sortType=0&appId=" + match.Groups[1].Value + "&pageInstanceId=" + id1.Groups[1].Value + "&searchWord=&pageNo=2&direction=1&instanceId=" + id2.Groups[1].Value + "&modulePrototypeId=55555&moduleTemplateId="+ id3.Groups[1].Value;


                string ZURL = "https://module-jshop.jd.com/module/getModuleHtml.html?orderBy=99&direction=1&pageNo=1&categoryId=0&pageSize=24&pagePrototypeId=8&pageInstanceId=" + id1.Groups[1].Value + "&moduleInstanceId=" + id1.Groups[1].Value + "&prototypeId=68&templateId=" + id3.Groups[1].Value + "&appId=" + id5.Groups[1].Value + "&layoutInstanceId=" + id2.Groups[1].Value + "&origin=0&shopId=" + shopid.Groups[1].Value + "&venderId=" + id6.Groups[1].Value + "&callback=jshop_module_render_callback";
                textBox1.Text = ZURL;



                        //MatchCollection items = Regex.Matches(strhtml, @"compare\/([\s\S]*?)-([\s\S]*?)-");

                        //StringBuilder sb = new StringBuilder();
                        //for (int a = 0; a < items.Count; a++)
                        //{
                        //    sb.Append("J_" + items[a].Groups[2].Value + ",");
                        //}
                        //string priceUrl = "https://p.3.cn/prices/mgets?skuIds=" + sb.ToString() + "&type=1&callback=jsonp1557817529049&_=1557817529050";

                        //MatchCollection pricees = Regex.Matches(strhtml, @"""p"":""([\s\S]*?)""");


                        //for (int b = 0; b < pricees.Count; b++)
                        //{
                        //    ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        //    listViewItem.SubItems.Add(items[b].Groups[2].Value);
                        //    listViewItem.SubItems.Add(pricees[b].Groups[1].Value);
                        //}




                    

                }

            
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void Jd_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
