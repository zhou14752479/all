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

namespace 网络游戏定单
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool zanting = true;
        #region 淘宝搜索
        public void run()
        {
            try

            {
                for (int a = 1; a < 999; a++)
                {


                    string url = "https://order.dd373.com/Api/Order/UserCenter/ListOrders?TabStatus=-1&Status=-1&StartDate=" + textBox1.Text + "-" + textBox2.Text + "-" + textBox3.Text + "+00%3A00%3A00&EndDate=" + textBox4.Text + "-" + textBox5.Text + "-" + textBox6.Text + "+23%3A59%3A59&Keyword=&LastId=&GoodsType=&DealType=-1&OrderBy=1&PageIndex="+a+"&PageSize=80&RoleType=1&IsRecycle=0";

                    string html = method.GetUrlWithCookie(url, 登录.COOKIE, "utf-8");




                    MatchCollection IDs = Regex.Matches(html, @"""Id"":""([\s\S]*?)""");
                    MatchCollection times = Regex.Matches(html, @"""CreateDate"":""([\s\S]*?)""");
                    MatchCollection LastOtherIds = Regex.Matches(html, @"""LastOtherId"":""([\s\S]*?)""");
                    MatchCollection ShopTypes = Regex.Matches(html, @"""ShopType"":""([\s\S]*?)""");
                    MatchCollection jinbis = Regex.Matches(html, @"""Title"":""([\s\S]*?)金");
                    MatchCollection prices = Regex.Matches(html, @"金=([\s\S]*?)元");
                    MatchCollection counts = Regex.Matches(html, @"""GoodsCount"":([\s\S]*?),");
                    MatchCollection status = Regex.Matches(html, @"""Status"":([\s\S]*?),");
                    if (IDs.Count == 0)
                    {
                        break;
                    }

                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < LastOtherIds.Count; i++)
                    {
                        sb.Append("{\"LastId\":\"" + LastOtherIds[i].Groups[1].Value + "\",\"GoodsTypeId\":\"" + ShopTypes[i].Groups[1].Value + "\"},");
                    }


                    string postdata = "[" + sb.ToString() + "]";

                    string strhtml = method.PostUrl("https://game.dd373.com/Api/Game/GetGameInfoListByIds", postdata.Replace(",]", "]"), 登录.COOKIE, "utf-8");

                    MatchCollection names = Regex.Matches(strhtml, @"""Name"":""([\s\S]*?)""");

                    for (int j = 0; j < IDs.Count; j++)
                    {

                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(IDs[j].Groups[1].Value);
                        listViewItem.SubItems.Add(times[j].Groups[1].Value);
                        listViewItem.SubItems.Add(names[5 * j].Groups[1].Value + "/" + names[(5 * j) + 1].Groups[1].Value + "/" + names[(5 * j) + 2].Groups[1].Value + "/" + names[(5 * j) + 3].Groups[1].Value);
                        listViewItem.SubItems.Add((Convert.ToDecimal(jinbis[j].Groups[1].Value) * Convert.ToInt32(counts[j].Groups[1].Value)).ToString());
                        listViewItem.SubItems.Add((Convert.ToDecimal(prices[j].Groups[1].Value) * Convert.ToInt32(counts[j].Groups[1].Value)).ToString());
                        if (status[j].Groups[1].Value == "1")
                        {
                            listViewItem.SubItems.Add("交易成功");
                        }
                        else
                        {
                            listViewItem.SubItems.Add("交易取消");
                        }

                    }


                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }



                }
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            #region 通用验证

            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php", "utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == "20.20.20.20")
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {

                button1.Enabled = false;
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;


            }
            else
            {
                MessageBox.Show("IP不符");

            }
            #endregion
           
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            登录 dl = new 登录();
            dl.Show();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            button1.Enabled = true;
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
