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

namespace main._2019_6
{
    public partial class zufang : Form
    {
        public zufang()
        {
            InitializeComponent();
        }
        bool status = true;
        bool zanting = true;
        private void Zufang_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        #region 房天下租房
        public void fang1()
        {

            try
            {

                string city = textBox1.Text.Trim();

                string Url = "https://"+city+".zu.fang.com/house/a21-h316/";

                string html = method.gethtml(Url, "","gb2312");
                
                MatchCollection ids = Regex.Matches(html, @"""houseid"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                ArrayList lists = new ArrayList();

                foreach (Match id in ids)
                {
                    lists.Add("https://"+city+".zu.fang.com/chuzu/1_" + id.Groups[1].Value + "_-1.htm");
                }


                foreach (string list in lists)

                {
                    string strhtml = method.gethtml(list, "", "gb2312");


                    Match title = Regex.Match(strhtml, @"<h1 class=""title "">([\s\S]*?)</h1>");
                    Match linkman = Regex.Match(strhtml, @"agentName: '([\s\S]*?)'");
                    Match phone = Regex.Match(strhtml, @"agentMobile: '([\s\S]*?)'");
                    Match cityname = Regex.Match(strhtml, @"cityName: '([\s\S]*?)'");

                    if (!cityname.Groups[1].Value.Contains("400"))
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(title.Groups[1].Value.Trim());
                        lv1.SubItems.Add(linkman.Groups[1].Value.Trim());
                        lv1.SubItems.Add(phone.Groups[1].Value.Trim());
                        lv1.SubItems.Add(cityname.Groups[1].Value.Trim());

                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }


                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }


                        if (this.status == false)
                            return;
                    }
                }

            }

            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion


        #region 房天下商铺出租
        public void fang2()
        {

            try
            {

                string city = textBox1.Text.Trim();

                string Url = "https://"+city+".shop.fang.com/zu/house/a21-h316/";

                string html = method.gethtml(Url, "", "gb2312");

                MatchCollection ids = Regex.Matches(html, @"""houseid"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                ArrayList lists = new ArrayList();

                foreach (Match id in ids)
                {
                    lists.Add("https://"+city+".shop.fang.com/zu/1_"+ id.Groups[1].Value + ".html" );
                }


                foreach (string list in lists)

                {
                   
                        string strhtml = method.gethtml(list, "", "gb2312");


                        Match title = Regex.Match(strhtml, @"<h3 class=""cont_tit"" title=""([\s\S]*?)""");
                        Match linkman = Regex.Match(strhtml, @"<span class=""zf_mfname"">([\s\S]*?)</span>");
                        Match phone = Regex.Match(strhtml, @"<span class=""zf_mftel"">([\s\S]*?)</span>");
                        Match cityname = Regex.Match(strhtml, @"址</b><span>([\s\S]*?)</span>");
                    if (!cityname.Groups[1].Value.Contains("-"))
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(title.Groups[1].Value.Trim());
                        lv1.SubItems.Add(linkman.Groups[1].Value.Trim());
                        lv1.SubItems.Add(phone.Groups[1].Value.Trim());
                        lv1.SubItems.Add(cityname.Groups[1].Value.Trim());

                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }


                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }


                        if (this.status == false)
                            return;
                    }
                }

            }

            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        #region 房天下商铺出售
        public void fang3()
        {

            try
            {

                string city = textBox1.Text.Trim();

                string Url = "https://" + city + ".shop.fang.com/shou/house/a21-h316/";

                string html = method.gethtml(Url, "", "gb2312");

                MatchCollection ids = Regex.Matches(html, @"""houseid"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                ArrayList lists = new ArrayList();

                foreach (Match id in ids)
                {
                    lists.Add("https://" + city + ".shop.fang.com/shou/1_" + id.Groups[1].Value + ".html");
                }


                foreach (string list in lists)

                {

                    string strhtml = method.gethtml(list, "", "gb2312");


                    Match title = Regex.Match(strhtml, @"<h3 class=""cont_tit"" title=""([\s\S]*?)""");
                    Match linkman = Regex.Match(strhtml, @"<span class=""zf_mfname"">([\s\S]*?)</span>");
                    Match phone = Regex.Match(strhtml, @"<span class=""zf_mftel"">([\s\S]*?)</span>");
                    Match cityname = Regex.Match(strhtml, @"址</b><span>([\s\S]*?)</span>");
                    if (!cityname.Groups[1].Value.Contains("-"))
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(title.Groups[1].Value.Trim());
                        lv1.SubItems.Add(linkman.Groups[1].Value.Trim());
                        lv1.SubItems.Add(phone.Groups[1].Value.Trim());
                        lv1.SubItems.Add(cityname.Groups[1].Value.Trim());

                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }


                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }


                        if (this.status == false)
                            return;
                    }
                }

            }

            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion
       
        #region 安居客整租
        public void anjuke1()
        {

            try
            {

                string city = textBox1.Text.Trim();

                string Url = "http://"+city+".baixing.com/zhengzu/?grfy=1&sortKey=createdTime";

                string html = method.GetUrl(Url, "utf-8");

                MatchCollection ids = Regex.Matches(html, @"data-aid='([\s\S]*?)'", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                ArrayList lists = new ArrayList();

                foreach (Match id in ids)
                {
                    lists.Add("http://"+city+".baixing.com/zhengzu/a"+id.Groups[1].Value+".html");
                }


                foreach (string list in lists)

                {

                    string strhtml = method.GetUrl(list, "utf-8");


                    Match title = Regex.Match(strhtml, @"<h1>([\s\S]*?)</h1>");
                    Match linkman = Regex.Match(strhtml, @"poster-name'>([\s\S]*?)'");
                    Match phone = Regex.Match(strhtml, @"<strong>([\s\S]*?)</strong>");
                    Match cityname = Regex.Match(strhtml, @"<i class='icon-ditu'></i><label>([\s\S]*?)</label>");
                    if (!cityname.Groups[1].Value.Contains("-"))
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(title.Groups[1].Value.Trim());
                        lv1.SubItems.Add(linkman.Groups[1].Value.Trim());
                        lv1.SubItems.Add(phone.Groups[1].Value.Trim());
                        lv1.SubItems.Add(cityname.Groups[1].Value.Trim());

                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }


                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }


                        if (this.status == false)
                            return;
                    }
                }

            }

            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        #region 安居客商铺出租
        public void anjuke2()
        {

            try
            {

                string city = textBox1.Text.Trim();

                string Url = "https://" + city + ".shop.fang.com/zu/house/a21-h316/";

                string html = method.gethtml(Url, "", "gb2312");

                MatchCollection ids = Regex.Matches(html, @"""houseid"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                ArrayList lists = new ArrayList();

                foreach (Match id in ids)
                {
                    lists.Add("https://" + city + ".shop.fang.com/zu/1_" + id.Groups[1].Value + ".html");
                }


                foreach (string list in lists)

                {

                    string strhtml = method.gethtml(list, "", "gb2312");


                    Match title = Regex.Match(strhtml, @"<h3 class=""cont_tit"" title=""([\s\S]*?)""");
                    Match linkman = Regex.Match(strhtml, @"<span class=""zf_mfname"">([\s\S]*?)</span>");
                    Match phone = Regex.Match(strhtml, @"<span class=""zf_mftel"">([\s\S]*?)</span>");
                    Match cityname = Regex.Match(strhtml, @"址</b><span>([\s\S]*?)</span>");
                    if (!cityname.Groups[1].Value.Contains("-"))
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(title.Groups[1].Value.Trim());
                        lv1.SubItems.Add(linkman.Groups[1].Value.Trim());
                        lv1.SubItems.Add(phone.Groups[1].Value.Trim());
                        lv1.SubItems.Add(cityname.Groups[1].Value.Trim());

                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }


                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }


                        if (this.status == false)
                            return;
                    }
                }

            }

            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion


        #region 赶集商铺出租
        public void ganji2()
        {

            try
            {

                string city = textBox1.Text.Trim();

                string Url = "http://" + city + ".ganji.com/shangpucz/0/";

                string html = method.GetUrl(Url, "utf-8");

                MatchCollection ids = Regex.Matches(html, @"<dd class=""dd-item title"">([\s\S]*?)<a href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                ArrayList lists = new ArrayList();

                foreach (Match id in ids)
                {
                    lists.Add("http:" + id.Groups[2].Value);
                }


                foreach (string list in lists)

                {

                    string strhtml = method.GetUrl(list, "utf-8");


                    Match title = Regex.Match(strhtml, @"<title>([\s\S]*?)</title>");
                    Match linkman = Regex.Match(strhtml, @"<div class=""name"">([\s\S]*?)_blank"">([\s\S]*?)</a>");
                    Match phone = Regex.Match(strhtml, @"phone"" gjalog=""([\s\S]*?)>([\s\S]*?)</a>");
                    Match cityname = Regex.Match(strhtml, @"com"" title="""">([\s\S]*?)</a>");
                    if (!cityname.Groups[1].Value.Contains("-"))
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(title.Groups[1].Value.Trim());
                        lv1.SubItems.Add(linkman.Groups[2].Value.Trim());
                        lv1.SubItems.Add(phone.Groups[2].Value.Trim());
                        lv1.SubItems.Add(cityname.Groups[1].Value.Trim());

                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }


                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }


                        if (this.status == false)
                            return;
                    }
                }

            }

            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        #region 赶集商铺出售
        public void ganji3()
        {

            try
            {

                string city = textBox1.Text.Trim();

                string Url = "http://"+city+".ganji.com/shangpucs/0/";

                string html = method.GetUrl(Url,  "utf-8");

                MatchCollection ids = Regex.Matches(html, @"<dd class=""dd-item title"">([\s\S]*?)<a href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                ArrayList lists = new ArrayList();

                foreach (Match id in ids)
                {
                    lists.Add("http:"+id.Groups[2].Value);
                }


                foreach (string list in lists)

                {

                    string strhtml = method.GetUrl(list, "utf-8");


                    Match title = Regex.Match(strhtml, @"<title>([\s\S]*?)</title>");
                    Match linkman = Regex.Match(strhtml, @"<div class=""name"">([\s\S]*?)_blank"">([\s\S]*?)</a>");
                    Match phone = Regex.Match(strhtml, @"phone"" gjalog=""([\s\S]*?)>([\s\S]*?)</a>");
                    Match cityname = Regex.Match(strhtml, @"com"" title="""">([\s\S]*?)</a>");
                    if (!cityname.Groups[1].Value.Contains("-"))
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(title.Groups[1].Value.Trim());
                        lv1.SubItems.Add(linkman.Groups[2].Value.Trim());
                        lv1.SubItems.Add(phone.Groups[2].Value.Trim());
                        lv1.SubItems.Add(cityname.Groups[1].Value.Trim());

                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }


                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }


                        if (this.status == false)
                            return;
                    }
                }

            }

            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        private void Button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "房天下" && comboBox2.Text == "个人房源出租")
            {
                Thread thread = new Thread(new ThreadStart(fang1));
                thread.Start();
            }

            else if (comboBox1.Text == "房天下" && comboBox2.Text == "个人商铺出租")
            {
                Thread thread = new Thread(new ThreadStart(fang2));
                thread.Start();
            }
            else if (comboBox1.Text == "房天下" && comboBox2.Text == "个人商铺出售")
            {
                Thread thread = new Thread(new ThreadStart(fang3));
                thread.Start();
            }
            else if (comboBox1.Text == "安居客" && comboBox2.Text == "个人房源出租")
            {
                Thread thread = new Thread(new ThreadStart(fang1));
                thread.Start();
            }
            else if (comboBox1.Text == "安居客" && comboBox2.Text == "个人商铺出租")
            {
                Thread thread = new Thread(new ThreadStart(fang2));
                thread.Start();
            }

            else if (comboBox1.Text == "赶集商铺出租")
            {
                Thread thread = new Thread(new ThreadStart(ganji2));
                thread.Start();
            }

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
    }
}
