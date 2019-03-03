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

namespace fang.临时软件
{
    public partial class ganji_keywords : Form
    {
        public ganji_keywords()
        {
            InitializeComponent();
        }

        bool status = true;

        #region  赶集本地服务
        public void run()
        {

            
          

            try
            {


                for (int i = 1; i < 12; i++)
                {




                string url = "http://iyouhui.org:808/Promoter/GetTradeList";
                string postdata = "pageSize=10&pageIndex="+i+"&Type=1&KeyWord=&StartDate=2018-09-01+00%3A00%3A00&EndDate=2019-02-21+00%3A00%3A00&TradeType=-1&OrderID=";

                    string cookie = "ASP.NET_SessionId=kqrwpmwif5ivknozldcn53nl; RYQIPAI=RYCacheKey=3006&RYCacheKey_ETS=2019-02-20+14%3a15%3a32&VS=c2c568d42ea46d87";
                    string charset = "utf-8";

                string html = method.PostUrl(url,postdata,cookie,charset);
                    textBox2.Text = html;

                    MatchCollection ass = Regex.Matches(html, @"Accounts\\"": \\""([\s\S]*?)\\", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection bss = Regex.Matches(html, @"RtnFee\\"":([\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                   



                    for (int j = 0; j < ass.Count; j++)
                    {

                     
                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                                lv1.SubItems.Add(ass[j].Groups[1].Value.Trim());
                        lv1.SubItems.Add(bss[j].Groups[1].Value.Trim());
                        

                        while (this.status == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (listView1.Items.Count > 1)
                                {
                                    listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                                }

                               
                        }



                    }

                }
            


            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.status = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void 删除此行ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
