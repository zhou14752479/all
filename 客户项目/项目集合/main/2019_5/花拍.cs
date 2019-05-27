using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main._2019_5
{
    public partial class 花拍 : Form
    {
        public 花拍()
        {
            InitializeComponent();
        }

        public static string COOKIE = "PHPSESSID=ohvckqa066j1ubejgkpb77jt47; CNZZDATA1254168241=1079397366-1558943902-%7C1558943902; UM_distinctid=16af859acbc102-05e012748741a1c-69010762-1fa400-16af859acbd77e; Hm_lvt_e51dc33d7f97d2b32af34d847bc7164c=1558944793; Hm_lpvt_e51dc33d7f97d2b32af34d847bc7164c=1558944815";

        bool status = true;
        bool zanting = true;
        #region 
        public void run()
        {
            COOKIE = webBrowser.cookie;
            try
            {

                for (int i = Convert.ToInt32(textBox1.Text) * 10; i <= Convert.ToInt32(textBox2.Text) *10; i = i + 10)
                {

                    string Url = "https://seller.huapai.com/goodslist/index?tid=0&status=2&step_status=-1&oid=0&per_page=" + i ;

                    string html = method.GetUrlWithCookie(Url, COOKIE, "utf-8");

                    MatchCollection names = Regex.Matches(html, @"data-name=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection dingdans = Regex.Matches(html, @"uid([\s\S]*?)</a></td>([\s\S]*?)<td>([\s\S]*?)</td>([\s\S]*?)<td>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    for (int j = 0; j < names.Count; j++)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(names[j].Groups[1].Value.Trim());
                        lv1.SubItems.Add(dingdans[j].Groups[3].Value.Trim());
                        lv1.SubItems.Add(dingdans[j].Groups[5].Value.Trim());

                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }


                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        label1.Text = "当前正在抓取第" + i / 10 + "页";

                        if (this.status == false)
                            return;
                    }

                }

                Thread.Sleep(500);



            }




            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }
        private void Button4_Click(object sender, EventArgs e)
        {
            webBrowser web = new webBrowser("https://seller.huapai.com/");
            web.Show();
        }
    }
}
