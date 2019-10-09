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

namespace 淘宝搜索
{
    public partial class 华夏银行 : Form
    {
        public 华夏银行()
        {
            InitializeComponent();
        }
        #region 华夏银行
        public void run()
        {
            try

            {

                string url = "http://www.hxb.com.cn/grjr/lylc/zzfsdlccpxx/index.shtml";

                    string html = method.GetUrl(url, "utf-8");

                    MatchCollection  a1s = Regex.Matches(html, @"<p class=""box_title""><a href=""#"">([\s\S]*?)<");
                MatchCollection a2s = Regex.Matches(html, @"<p class=""box_num"">([\s\S]*?)</p>([\s\S]*?)<p>([\s\S]*?)</p>");
                MatchCollection a3s = Regex.Matches(html, @"<span class=""highlight"">([\s\S]*?)<");
                MatchCollection a4s = Regex.Matches(html, @"<span class=""riqi"">([\s\S]*?)<");
                MatchCollection a5s = Regex.Matches(html, @"<span class=""amt"">([\s\S]*?)</li>");







                for (int j = 0; j < a1s.Count; j++)
                {
                    ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                    listViewItem.SubItems.Add(a1s[j].Groups[1].Value.Trim());
                    listViewItem.SubItems.Add(a2s[j].Groups[3].Value.Trim());
                    listViewItem.SubItems.Add(a2s[j].Groups[1].Value.Trim());
                    listViewItem.SubItems.Add(a3s[j].Groups[1].Value.Trim());
                    listViewItem.SubItems.Add(a4s[j].Groups[1].Value.Trim());
                    listViewItem.SubItems.Add(a5s[j].Groups[1].Value.Replace("</span>","").Trim());

                }
               

                
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion
        private void 华夏银行_Load(object sender, EventArgs e)
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
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
