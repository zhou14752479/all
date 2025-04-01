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
using myDLL;

namespace 主程序202401
{
    public partial class 电影评价 : Form
    {
        public 电影评价()
        {
            InitializeComponent();
        }

        string cookie = "ll=\"118171\"; bid=735qtE8E3cU; _pk_ref.100001.4cf6=%5B%22%22%2C%22%22%2C1738239792%2C%22https%3A%2F%2Fwww.baidu.com%2Fs%3Fie%3DUTF-8%26wd%3D%E8%B1%86%E7%93%A3%22%5D; _pk_id.100001.4cf6=1cb32108a9340b4a.1738239792.; _pk_ses.100001.4cf6=1; ap_v=0,6.0; __utma=30149280.640905292.1738239792.1738239792.1738239792.1; __utmb=30149280.0.10.1738239792; __utmc=30149280; __utmz=30149280.1738239792.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); __utma=223695111.1974849468.1738239792.1738239792.1738239792.1; __utmb=223695111.0.10.1738239792; __utmc=223695111; __utmz=223695111.1738239792.1.1.utmcsr=baidu|utmccn=(organic)|utmcmd=organic|utmctr=%E8%B1%86%E7%93%A3; __yadk_uid=6JKtOO40JjljpEFrs0jsVjGiynJ2iPgK; _vwo_uuid_v2=DDC215AE198F0C91161F32D8D999BAE5B|b0e0c7d742ed1f4731256b59fa06b7da; dbcl2=\"184268736:URXk/ABbwfQ\"; ck=A27F; push_noty_num=0; push_doumail_num=0; frodotk_db=\"40fa7959ec5fa450c2ef6c0dc51eecf0\"";
        public void run()
        {
            try
            {

                label1.Text = "电影评分（数据来源于豆瓣电影）"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    string url = "https://movie.douban.com/cinema/nowplaying/suqian/";

                    string html = method.GetUrlWithCookie(url, cookie,"utf-8");

                    MatchCollection uids = Regex.Matches(html, @"data-subject=""([\s\S]*?)""");

                    for (int a = 0; a < uids.Count; a++)
                    {

                    try
                    {
                        string aurl = "https://movie.douban.com/subject/" + uids[a].Groups[1].Value + "/?from=playing_poster";
                        string ahtml = method.GetUrlWithCookie(aurl, cookie, "utf-8");

                        string name = Regex.Match(ahtml, @"<strong>([\s\S]*?)</strong>").Groups[1].Value;
                        string votes = Regex.Match(ahtml, @"votes"">([\s\S]*?)<").Groups[1].Value;
                        MatchCollection ratings = Regex.Matches(ahtml, @"<span class=""rating_per"">([\s\S]*?)</span>");


                        double vote1 = (Convert.ToDouble(ratings[0].Groups[1].Value.TrimEnd('%')) + Convert.ToDouble(ratings[1].Groups[1].Value.TrimEnd('%'))) / 100;
                        double vote2 = (Convert.ToDouble(ratings[3].Groups[1].Value.TrimEnd('%')) + Convert.ToDouble(ratings[4].Groups[1].Value.TrimEnd('%'))) / 100;

                        ListViewItem lv1 = listView1.Items.Add(name); //使用Listview展示数据

                        lv1.SubItems.Add(votes);

                        lv1.SubItems.Add(Math.Round(vote1 * Convert.ToInt32(votes)).ToString() );
                        lv1.SubItems.Add(Math.Round(vote2 * Convert.ToInt32(votes)).ToString());

                        lv1.SubItems.Add((vote1 * 100).ToString("F2") + "%");
                        lv1.SubItems.Add((vote2 * 100).ToString("F2") + "%");
                    }
                    catch (Exception)
                    {

                       continue;
                    }

                    }


                }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }


        Thread thread;
        private void 电影评价_Load(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = "电影评分（数据来源于豆瓣电影）" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
