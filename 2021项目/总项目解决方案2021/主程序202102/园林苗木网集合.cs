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

namespace 主程序202102
{
    public partial class 园林苗木网集合 : Form
    {
        public 园林苗木网集合()
        {
            InitializeComponent();
        }
        Thread thread;
        bool zanting = true;
        bool status = true;

        #region huamu.cn
        public void huamucn()
        {
         
             

                for (int i = 6684; i < 13135; i++)
                {
                  
                    string url = "http://www.huamu.cn/Company/S______"+i+".html";

                    string html = method.GetUrl(url, "gb2312");



                    MatchCollection titles = Regex.Matches(html, @"<div class=""shop-name"">([\s\S]*?)</div>");
                   
                    MatchCollection lxrs = Regex.Matches(html, @"<li>联 系 人：([\s\S]*?)<");
                MatchCollection tels = Regex.Matches(html, @"联系电话：([\s\S]*?)<");
                MatchCollection addrs = Regex.Matches(html, @"<li>联系地址：([\s\S]*?)<");


                MatchCollection contacturl = Regex.Matches(html, @"<div class=""shop-contact"">([\s\S]*?)<a href=""([\s\S]*?)""");

                if (titles.Count == 0)
                        break;
                    for (int j = 0; j < titles.Count; j++)
                    {
                        try
                        {
                        string tel = Regex.Replace(tels[j].Groups[1].Value, " <[^>]+>", "");
                        if (tel == "")
                        {

                            string ahtml = method.GetUrl(contacturl[j].Groups[2].Value, "gb2312");
                            Match tel2 = Regex.Match(ahtml, @"手机号码：([\s\S]*?)<br>");
                            Match tel3 = Regex.Match(ahtml, @"电话号码：([\s\S]*?)<br>");
                         
                            string tel22 = Regex.Replace(tel2.Groups[1].Value, "<[^>]+>", "").Trim();
                            if (tel22 != "")
                            {
                                tel = tel22;
                            }
                            else
                            {
                                tel = Regex.Replace(tel3.Groups[1].Value, "<[^>]+>", "").Trim();
                            }
                            

                        }






                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(Regex.Replace(titles[j].Groups[1].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(lxrs[j].Groups[1].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(tel);
                        lv1.SubItems.Add(Regex.Replace(addrs[j].Groups[1].Value, "<[^>]+>", ""));



                        while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                        if (status == false)
                            return;

                        }
                        catch (Exception)
                        {

                            continue;
                        }

                    }
                    

                }
            

        }

        #endregion

        #region cx987.cn
        public void cx987cn()
        {



            for (int i = 1; i <4935; i++)
            {

                string url = "http://www.cx987.cn/company/index-htm-page-"+i+".html";

                string html = method.GetUrl(url, "utf-8");



                MatchCollection urls = Regex.Matches(html, @"<td width=""100""><div><a href=""([\s\S]*?)""");
              


                if (urls.Count == 0)
                    break;
                for (int j = 0; j < urls.Count; j++)
                {
                    try
                    {

                        string aurl =urls[j].Groups[1].Value + "contact/";
                      
                        string ahtml = method.GetUrl(aurl, "utf-8");

                        Match title = Regex.Match(ahtml, @"<h1>([\s\S]*?)</h1>");
                        Match lxr = Regex.Match(ahtml, @"联 系 人：</td>([\s\S]*?)</td>");
                        Match tel = Regex.Match(ahtml, @"公司电话：</td>([\s\S]*?)</td>");
                        Match tel2 = Regex.Match(ahtml, @"手机号码：</td>([\s\S]*?)</td>");
                        Match addr = Regex.Match(ahtml, @"公司地址：</td>([\s\S]*?)</td>");


                        string phone = "";

                        if (Regex.Replace(tel.Groups[1].Value, "<[^>]+>", "").Trim() == Regex.Replace(tel2.Groups[1].Value, "<[^>]+>", "").Trim())
                        {
                            phone = Regex.Replace(tel.Groups[1].Value, "<[^>]+>", "").Trim();
                        }
                        else
                        {
                            phone = Regex.Replace(tel.Groups[1].Value, "<[^>]+>", "").Trim() + " "+Regex.Replace(tel2.Groups[1].Value, "<[^>]+>", "").Trim();
                        }
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(Regex.Replace(title.Groups[1].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(lxr.Groups[1].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(phone.Trim());
                        lv1.SubItems.Add(Regex.Replace(addr.Groups[1].Value, "<[^>]+>", ""));



                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;

                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                        continue;
                    }

                }


            }


        }

        #endregion

        #region xbmiaomu.com
        public void xbmiaomu()
        {



            for (int i = 1637; i < 3858; i++)
            {

                string url = "http://www.xbmiaomu.com/company/index-htm-page-" + i + ".html";

                string html = method.GetUrl(url, "utf-8");



                MatchCollection urls = Regex.Matches(html, @"<h3 style=""font-size: 18px;""><a href=""([\s\S]*?)""");



                if (urls.Count == 0)
                    break;
                for (int j = 0; j < urls.Count; j++)
                {
                    try
                    {

                        string aurl = urls[j].Groups[1].Value + "contact/";

                        string ahtml = method.GetUrl(aurl, "utf-8");

                        Match title = Regex.Match(ahtml, @"<h1>([\s\S]*?)</h1>");
                        Match lxr = Regex.Match(ahtml, @"联 系 人：</td>([\s\S]*?)</td>");
                        Match tel = Regex.Match(ahtml, @"手机号码：</td>([\s\S]*?)</td>");
                        Match addr = Regex.Match(ahtml, @"所在地区：</td>([\s\S]*?)</td>");
                        if (lxr.Groups[1].Value.Trim() == "")
                        {
                            lxr = Regex.Match(ahtml, @"联系人：([\s\S]*?)</li>");
                        }
                        if (tel.Groups[1].Value.Trim() == "")
                        {
                            tel= Regex.Match(ahtml, @"联系手机：([\s\S]*?)</li>");
                            if (tel.Groups[1].Value.Trim() == "")
                            {
                                tel = Regex.Match(ahtml, @"公司电话：</td>([\s\S]*?)</td>");
                                if (tel.Groups[1].Value.Trim() == "")
                                {
                                    tel = Regex.Match(ahtml, @"联系电话：([\s\S]*?)</li>");
                                }
                            }
                        }
                        if (addr.Groups[1].Value == "")
                        {
                            addr = Regex.Match(ahtml, @"公司地址：([\s\S]*?)</li>");
                        }


                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(Regex.Replace(title.Groups[1].Value, "<[^>]+>", "").Replace("&nbsp;",""));
                        lv1.SubItems.Add(Regex.Replace(lxr.Groups[1].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(tel.Groups[1].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(addr.Groups[1].Value, "<[^>]+>", ""));


                      
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;

                    }
                    catch (Exception)
                    {

                        continue;
                    }

                }


            }


        }

        #endregion


        #region 597mm.com
        public void mm597()
        {



            for (int i = 1; i < 9999; i++)
            {

                string url = "http://www.597mm.com/company/index.php?page="+i;

                string html = method.GetUrl(url, "utf-8");



                MatchCollection urls = Regex.Matches(html, @"<td width=""100""><div><a href=""([\s\S]*?)""");



                if (urls.Count == 0)
                    break;
                for (int j = 0; j < urls.Count; j++)
                {
                    try
                    {

                        string aurl = urls[j].Groups[1].Value + "&file=contact";

                        string ahtml = method.GetUrl(aurl, "gbk");

                        Match title = Regex.Match(ahtml, @"公司名称：</td>([\s\S]*?)</td>");
                        Match lxr = Regex.Match(ahtml, @"联 系 人：</td>([\s\S]*?)</td>");
                        Match tel = Regex.Match(ahtml, @"手机号码：</td>([\s\S]*?)</td>");
                        Match addr = Regex.Match(ahtml, @"公司地址：</td>([\s\S]*?)</td>");


                        if (tel.Groups[1].Value == "")
                        {
                            tel = Regex.Match(ahtml, @"公司电话：</td>([\s\S]*?)</td>");
                        }

                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(Regex.Replace(title.Groups[1].Value, "<[^>]+>", "").Replace("&nbsp;", ""));
                        lv1.SubItems.Add(Regex.Replace(lxr.Groups[1].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(tel.Groups[1].Value, "<[^>]+>", "").Replace(" ",""));
                        lv1.SubItems.Add(Regex.Replace(addr.Groups[1].Value, "<[^>]+>", ""));



                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;

                    }
                    catch (Exception)
                    {

                        continue;
                    }

                }


            }


        }

        #endregion

        #region 312green
        public void green312()
        {



            for (int i = Convert.ToInt32(textBox1.Text); i < 15282; i++)
            {

                string url = "http://m.312green.com/enterprise/view-c-s-t-v-p"+i+".html";

                string html = method.GetUrl(url, "utf-8");



                MatchCollection names = Regex.Matches(html, @"<div class='item'>([\s\S]*?)</a>");
                MatchCollection lxrs = Regex.Matches(html, @"><span class='zt1'>([\s\S]*?)<");
                MatchCollection tels = Regex.Matches(html, @"<a href='tel([\s\S]*?)>([\s\S]*?)</a>");
                MatchCollection addrs = Regex.Matches(html, @"<span class='gray zt1'>([\s\S]*?)<");
                if (names.Count == 0)
                {
                    i = i - 1;
                    continue;
                }
                   
                for (int j = 0; j < names.Count; j++)
                {
                    try
                    {

                                           

                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(Regex.Replace(names[j].Groups[1].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(lxrs[j].Groups[1].Value, "<[^>]+>", "").Replace("&nbsp;&nbsp;",""));
                        lv1.SubItems.Add(Regex.Replace(tels[j].Groups[2].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(addrs[j].Groups[1].Value, "<[^>]+>", ""));

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;

                    }
                    catch (Exception)
                    {

                        continue;
                    }

                }


            }


        }

        #endregion

        #region huamu.com
        public void huamucom()
        {



            for (int i = 4789; i < 5034; i++)
            {

                string url = "https://www.huamu.com/pinpai/"+i+".html";

                string html = method.GetUrl(url, "utf-8");



                MatchCollection titles = Regex.Matches(html, @"<div class=""newslist_top_p"">([\s\S]*?)"">([\s\S]*?)</a>");

                MatchCollection lxrs = Regex.Matches(html, @"联系人 : </span>([\s\S]*?)</span>");
                MatchCollection tels = Regex.Matches(html, @"电话 : </span>([\s\S]*?)</span>");
                MatchCollection addrs = Regex.Matches(html, @"地址 : </dt>([\s\S]*?)</dd>");


                if (titles.Count == 0)
                    continue;
               
                for (int j = 0; j < titles.Count; j++)
                {
                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    try
                    {
                      
                        lv1.SubItems.Add(Regex.Replace(titles[j].Groups[2].Value, "<[^>]+>", ""));
                        lv1.SubItems.Add(Regex.Replace(lxrs[j].Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(tels[j].Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(addrs[j].Groups[1].Value, "<[^>]+>", "").Replace(" ",""));



                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;

                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                        lv1.SubItems.Add("未填写");
                       
                    }

                }


            }


        }

        #endregion


        public string getip()
        {
            // string html = method.GetUrl("http://47.106.170.4:8081/Index-generate_api_url.html?packid=7&fa=5&groupid=0&fetch_key=&qty=1&port=1&format=txt&ss=1&css=&pro=&city=&usertype=7", "utf-8");
            string html = method.GetUrl("http://47.106.170.4:8081/Index-generate_api_url.html?packid=1&fa=0&groupid=0&fetch_key=&qty=1&port=1&format=txt&ss=1&css=&pro=&city=&usertype=7", "utf-8");
            label1.Text = html;
            return html;
        }

        string ip = "";
        #region yuanlin365
        public void yuanlin365()
        {
            ip = getip();
            List<string> lists = new List<string>();
            for (int i = Convert.ToInt32(textBox1.Text); i < 668; i++)
            {
                label1.Text = i.ToString();
                label2.Text = ip;
                // string url = "http://www.yuanlin365.com/supply/today-"+i+".shtml";
                string url = "http://www.yuanlin365.com/";
                string html = method.GetUrlwithIP(url,ip,"", "gb2312");

                if (html == "")
                {
                    Thread.Sleep(6000);
                    ip = getip();
                    i = i - 1;
                    continue;
                }

                //MatchCollection urls = Regex.Matches(html, @"<span>([\s\S]*?)a href=""([\s\S]*?)""");

                MatchCollection urls = Regex.Matches(html, @"<TD height=20>·<A href=([\s\S]*?)target");

                if (urls.Count == 0)
                    break;
                for (int j = 0; j < urls.Count; j++)
                {
                    try
                    {

                        //string aurl = urls[j].Groups[2].Value.Trim()+ "/contact.shtml";
                        string aurl = urls[j].Groups[1].Value.Trim() + "/contact.shtml";
                        string ahtml = method.GetUrlwithIP(aurl, ip, "", "gb2312");
                        if (ahtml == "")
                        {
                            Thread.Sleep(6000);
                            ip = getip();
                            j = j - 1;
                            continue;
                        }

                        Match title = Regex.Match(ahtml, @"class=""font-000-14"">([\s\S]*?)</td>");
                        Match lxr = Regex.Match(ahtml, @"class=""table-2"">([\s\S]*?)</td>");
                        Match tel = Regex.Match(ahtml, @"电　　话：([\s\S]*?)</td>");
                        Match tel2 = Regex.Match(ahtml, @"手　　机：([\s\S]*?)</td>");
                        Match addr = Regex.Match(ahtml, @"地　　址：([\s\S]*?)</td>");

                        if (title.Groups[1].Value == "")
                        {
                            title = Regex.Match(ahtml, @"<h1>([\s\S]*?)</h1>");
                        }
                      
                            lists.Add(Regex.Replace(tel.Groups[1].Value, "<[^>]+>", ""));
                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(Regex.Replace(title.Groups[1].Value, "<[^>]+>", ""));
                            lv1.SubItems.Add(Regex.Replace(lxr.Groups[1].Value, "<[^>]+>", "").Replace("&nbsp;", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(tel2.Groups[1].Value, "<[^>]+>", ""));
                            lv1.SubItems.Add(Regex.Replace(addr.Groups[1].Value, "<[^>]+>", ""));



                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)
                                return;
                        
                    }
                    catch (Exception)
                    {

                        continue;
                    }

                }


            }


        }

        #endregion

        private void 园林苗木网集合_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"RE72"))
            {
                MessageBox.Show("");
                return;
            }



            #endregion
            status = true;
          
            if (thread == null || !thread.IsAlive)
            {
                if (radioButton1.Checked == true)
                {
                    thread = new Thread(huamucn);
                    thread.Start();
                }
                if (radioButton2.Checked == true)
                {
                    thread = new Thread(yuanlin365);
                    thread.Start();
                }


                if (radioButton3.Checked == true)
                {
                    thread = new Thread(cx987cn);
                    thread.Start();
                }

                if (radioButton4.Checked == true)
                {
                    thread = new Thread(xbmiaomu);
                    thread.Start();
                }

                if (radioButton5.Checked == true)
                {
                    thread = new Thread(mm597);
                    thread.Start();
                }
                if (radioButton6.Checked == true)
                {
                    thread = new Thread(green312);
                    thread.Start();
                }
                if (radioButton7.Checked == true)
                {
                    thread = new Thread(huamucom);
                    thread.Start();
                }
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
