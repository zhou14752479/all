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
using helper;

namespace 启动程序
{
    public partial class 广东快乐十分 : Form
    {
        public 广东快乐十分()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
            string date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            MessageBox.Show(date);
            try
            {
                string url = "https://www.1396r.com/gdkl10/KaiJiang?date="+date+"&_=1586348830631";

                        string html = method.GetUrl(url, "utf-8");

                        MatchCollection ahtmls = Regex.Matches(html, @"<div class='profile'>([\s\S]*?)<div class='toolLinks'>");


                        Match title = Regex.Match(html, @"<title>([\s\S]*?)-");



                        for (int i = 0; i < ahtmls.Count; i++)
                        {
                            try
                            {
                                Match location = Regex.Match(ahtmls[i].Groups[1].Value, @"<div class='location'>([\s\S]*?)</div>");
                                Match postdate = Regex.Match(ahtmls[i].Groups[1].Value, @"<div class='postDate'>([\s\S]*?)</div>");
                                Match body = Regex.Match(ahtmls[i].Groups[1].Value, @"<div class='postBody'>([\s\S]*?)<div class=""postTools"">");
                                Match person = Regex.Match(ahtmls[i].Groups[1].Value, @"forum""><span>([\s\S]*?)</span>");

                                MatchCollection posts = Regex.Matches(ahtmls[i].Groups[1].Value, @"<span class=""badgeText"">([\s\S]*?) ");

                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add(title.Groups[1].Value);
                                lv1.SubItems.Add(Regex.Replace(person.Groups[1].Value, "<[^>]+>", "").Trim());
                                lv1.SubItems.Add(location.Groups[1].Value);
                                lv1.SubItems.Add(Regex.Replace(body.Groups[1].Value, "<[^>]+>", "").Trim());
                                lv1.SubItems.Add(postdate.Groups[1].Value);

                                lv1.SubItems.Add(Regex.Replace(posts[0].Groups[1].Value, "<[^>]+>", "").Trim());
                                lv1.SubItems.Add(Regex.Replace(posts[1].Groups[1].Value, "<[^>]+>", "").Trim());
                                lv1.SubItems.Add(Regex.Replace(posts[2].Groups[1].Value, "<[^>]+>", "").Trim());


                            }
                            catch
                            {

                                continue;
                            }

                         
                }
                
            }

            catch (Exception)
            {

                throw;
            }
        }
        private void 广东快乐十分_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            run();
        }
    }
}
