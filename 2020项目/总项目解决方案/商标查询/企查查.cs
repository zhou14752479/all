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
        bool zanting = true;
       
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
                    string html = method.GetUrlWithCookie(url,登录.COOKIE,"utf-8");

                    textBox1.Text = html;
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

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
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

        private void Button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            登录 dl = new 登录();
            dl.Show();
        }
    }
}
