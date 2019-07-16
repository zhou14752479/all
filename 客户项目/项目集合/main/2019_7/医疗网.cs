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

namespace main._2019_7
{
    public partial class 医疗网 : Form
    {
        public 医疗网()
        {
            InitializeComponent();
        }

        bool zanting = true;

        public string NCRtoString(string str)
        {
            return "";
         
        }
        #region 主程序
        public void run()
        {

            try
            {
                for (int i = 0; i < 9999; i++)
                {

                    string Url = "https://3g.kq36.cn/m/companylist.aspx?keyw=&typeid=1&proviceid=0&cityid=0&areaid=0&minying=&guimo=&xingzhi=&offerid=&ddlmedicare=&pageindex="+i;

                    string html = method.GetUrl(Url, "utf-8");
                   
                    MatchCollection ids = Regex.Matches(html, @"data-userid='([\s\S]*?)'", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                   
                    if (ids.Count == 0)
                        return;
                    ArrayList lists = new ArrayList();

                    foreach (Match id in ids)
                    {
                        lists.Add("https://3g.kq36.com/jobs/" + id.Groups[1].Value);
                    }

                    foreach (string list in lists)

                    {

                        string strhtml = method.GetUrl(list, "gb2312");

                        Match a1 = Regex.Match(strhtml, @"<title>([\s\S]*?)诚");
                        Match a2 = Regex.Match(strhtml, @"成立</li>([\s\S]*?)</li>");
                        Match a3 = Regex.Match(strhtml, @"员工</li>([\s\S]*?)</li>");
                        Match a4 = Regex.Match(strhtml, @"性质</li>([\s\S]*?)</li>");
                        Match a5 = Regex.Match(strhtml, @"联系</li>([\s\S]*?)</li>");
                        Match a6 = Regex.Match(strhtml, @"电话</li>([\s\S]*?)</li>");
                        Match a7 = Regex.Match(strhtml, @"手机</li>([\s\S]*?)</li>");
                        Match a8 = Regex.Match(strhtml, @"QQ</li>([\s\S]*?)</li>");
                        Match a9 = Regex.Match(strhtml, @"邮箱</li>([\s\S]*?)</li>");
                        Match a10 = Regex.Match(strhtml, @"地址</li>([\s\S]*?)</li>");

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(Regex.Replace(a1.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a2.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a3.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a4.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(NCRtoString(Regex.Replace(a5.Groups[1].Value, "<[^>]+>", "")).Trim());
                        lv1.SubItems.Add(Regex.Replace(a6.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a7.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a8.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a9.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(NCRtoString(Regex.Replace(a10.Groups[1].Value, "<[^>]+>", "")).Trim());


                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        Thread.Sleep(2000);
                    }

                }
            }

            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        private void 医疗网_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(NCRtoString("&#x5E7F;&#x4E1C;&#x6DF1;&#x5733;&#x5B9D;&#x5B89;&#x533A;&#x798F;&#x6C38;&#x8857;&#x9053;&#x6C38;&#x548C;&#x8DEF;&#x53CC;&#x91D1;&#x60E0;&#x5DE5;&#x4E1A;&#x533A;&#x44;&#x680B;&#x35;&#x697C"));
        }
    }
}
