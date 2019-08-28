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
    public partial class 军队采购 : Form
    {
        public 军队采购()
        {
            InitializeComponent();
        }
       bool  zanting=true;
        ArrayList finishes = new ArrayList();
        #region  主程序
        public void run()
        {

            try
            {




                for (int i = 1; i < 20000; i++)
                {
                    if(!finishes.Contains(i))
                    {
                        finishes.Add(i);
                    String Url = "http://mall.plap.cn/npc/products.html?commit=%E6%9F%A5%E8%AF%A2&page=" + i;

                    string html = method.GetUrl(Url, "utf-8");


                    MatchCollection IDs = Regex.Matches(html, @"<td align=""center"" width=""30px"">([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection ass = Regex.Matches(html, @"<td align=""center"" width=""80px"" title=""([\s\S]*?)"">([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection bss = Regex.Matches(html, @"<td width=""90px"" align=""right"" name=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection css = Regex.Matches(html, @"<td width=""100px"" class=""no_break"">([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection dss = Regex.Matches(html, @"<td width=""100px"" align=""right"">([\s\S]*?)<", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        MatchCollection ess = Regex.Matches(html, @" class=""edit_in_place([\s\S]*?)>([\s\S]*?)<", RegexOptions.IgnoreCase | RegexOptions.Multiline);



                        if (IDs.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;



                        for (int j = 0; j < IDs.Count; j++)
                        {


                            ListViewItem lv1 = listView1.Items.Add(IDs[j].Groups[1].Value.Trim()); //使用Listview展示数据

                            lv1.SubItems.Add(ass[j].Groups[2].Value.Trim());
                            lv1.SubItems.Add(bss[j].Groups[1].Value.Trim());
                            lv1.SubItems.Add(css[j].Groups[1].Value.Trim());
                            lv1.SubItems.Add(dss[j].Groups[1].Value.Trim());
                            lv1.SubItems.Add(ess[j].Groups[2].Value.Trim());

                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置

                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                        }

                    }


                }
            }




            catch (System.Exception ex)
            {

                textBox1.Text = ex.ToString();
            }

        }

        #endregion
        private void 军队采购_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "已开始......";
            #region 通用登录

            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php","utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == localip.Trim())
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {
                //--------登陆函数------------------
                for (int i = 0; i < 5; i++)
                {
                    Thread thread = new Thread(new ThreadStart(run));
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
                

            }
            else
            {
                MessageBox.Show("请登录您的账号！");
                System.Diagnostics.Process.Start("http://www.acaiji.com");
                return;
            }
            #endregion
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
