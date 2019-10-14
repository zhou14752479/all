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

namespace 商标查询
{
    public partial class 企查查 : Form
    {
        public 企查查()
        {
            InitializeComponent();
        }
        public static string COOKIE= "zg_did=%7B%22did%22%3A%20%2216c2bdc701a9fd-0cfda5fe614295-f353163-1fa400-16c2bdc701d70%22%7D; UM_distinctid=16c2bdc9cef480-089a8a3d063f-f353163-1fa400-16c2bdc9cf11d8; _uab_collina=156410397489894899860524; acw_tc=7518019a15708437013286366e44f9a842cdcc7173a88f0350e57ea423; QCCSESSID=spcn3mjv3gdbs3nmbogkb6lrp3; CNZZDATA1254842228=1203877086-1564099416-https%253A%252F%252Fwww.baidu.com%252F%7C1571012515; Hm_lvt_3456bee468c83cc63fb5147f119f1075=1569482634,1570843703,1571013810; hasShow=1; zg_de1d1a35bfa24ce29bbf2c7eb17e6c4f=%7B%22sid%22%3A%201571013808460%2C%22updated%22%3A%201571015408329%2C%22info%22%3A%201570843699939%2C%22superProperty%22%3A%20%22%7B%7D%22%2C%22platform%22%3A%20%22%7B%7D%22%2C%22utm%22%3A%20%22%7B%7D%22%2C%22referrerDomain%22%3A%20%22%22%2C%22cuid%22%3A%20%22b924eb29800901c1435f40aa0f0d2da5%22%2C%22zs%22%3A%200%2C%22sc%22%3A%200%7D; Hm_lpvt_3456bee468c83cc63fb5147f119f1075=1571015408";
        #region  中间量获取
        public void run()

        {


            try
            {
               
                string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int a = 0; a < text.Length; a++)
                {
                    if (text[a] == "")
                    {
                        continue;
                    }
                    label1.Text = "正在查询.."+ text[a];
                    string url = "https://www.qichacha.com/search?key=" + text[a];
                    string html = method.GetUrlWithCookie(url,COOKIE,"utf-8");
                    Match company = Regex.Match(html, @"data-name=""([\s\S]*?)""");
                    Match name = Regex.Match(html, @"法定代表人：([\s\S]*?)</a>");
                    Match tel = Regex.Match(html, @"电话：([\s\S]*?)</span>");
                    Match address = Regex.Match(html, @"地址：([\s\S]*?)</p>");

                    if (html.Contains("我不甘心"))
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据      
                        lv1.SubItems.Add(text[a]);
                        lv1.SubItems.Add("未查询到企业名");
                    }

                    else if (company.Groups[1].Value == "")
                    {
                        MessageBox.Show("需要验证");
                    }
                    else
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据      
                        lv1.SubItems.Add(Regex.Replace(company.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(name.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(tel.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(address.Groups[1].Value, "<[^>]+>", "").Trim());
                        

                    }

                    if (listView1.Items.Count > 2)
                    {
                        listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                    }
                    Thread.Sleep(300);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion
        private void 企查查_Load(object sender, EventArgs e)
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
                if (ip.Groups[1].Value.Trim() == "21.21.21.21")
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
    }
}
