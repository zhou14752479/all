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

namespace 启动程序
{
    public partial class 兴盛优选 : Form
    {
        public 兴盛优选()
        {
            InitializeComponent();
        }

        bool status = true;
        public void run()
        {

            try
            {

                string keyword = System.Web.HttpUtility.UrlEncode(textBox1.Text);


                for (int i = 1; i < 9999; i++)
                {

                    string url = "https://mall-store.xsyxsc.com/mall-store/store/queryStoreList?page="+i+"&rows=100&storeName="+System.Web.HttpUtility.UrlEncode(textBox1.Text)+"&userKey=a2422d77-69a1-4f7b-aa1a-39bec2b2db44";


                    string html = method.GetUrl(url, "utf-8");
                    MatchCollection titles = Regex.Matches(html, @"""storeName"":""([\s\S]*?)""");

                    MatchCollection names = Regex.Matches(html, @"""contacts"":""([\s\S]*?)""");
                    MatchCollection tels = Regex.Matches(html, @"""contactsTel"":""([\s\S]*?)""");
                    MatchCollection address = Regex.Matches(html, @"""detailAddress"":""([\s\S]*?)""");

                    if (titles.Count == 0)
                        return;

                    for (int j = 0; j < titles.Count; j++)
                    {
                        try
                        {



                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                            lv1.SubItems.Add(titles[j].Groups[1].Value);
                            lv1.SubItems.Add(names[j].Groups[1].Value);
                            lv1.SubItems.Add(tels[j].Groups[1].Value);
                            lv1.SubItems.Add(address[j].Groups[1].Value);
                            if (status == false)
                                return;

                        }
                        catch
                        {

                            continue;
                        }



                    }


                }
            }
            catch (Exception)
            {

                throw;
            }




        }
        private void 兴盛优选_Load(object sender, EventArgs e)
        {
         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"xsyx"))
            {
                button1.Enabled = false;
                status = true;
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            status = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
