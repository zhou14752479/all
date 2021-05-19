using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;


namespace 汽车之家论坛
{
    public partial class 汽车之家论坛 : Form
    {
        public 汽车之家论坛()
        {
            InitializeComponent();
        }
        #region GET请求带COOKIE
        /// <summary>
        /// GET请求带COOKIE
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlWithCookie(string Url, string COOKIE, string charset)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = false;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.85 Safari/537.36";
                request.Referer = Url;
                request.Headers.Add("Cookie", COOKIE);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                request.Accept = "*/*";
                request.Timeout = 100000;
                string content = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                return (ex.ToString());



            }

        }
        #endregion


        public int autohomegetpage(string url)
        {
            
            string html2 = GetUrlWithCookie(url, "", "gb2312");
            string page = Regex.Match(html2, @"<span class=""fr"">共([\s\S]*?)页").Groups[1].Value;
            return Convert.ToInt32(page)+1;
        }

        public int yichegetpage(string url)
        {

            string html2 = GetUrlWithCookie(url, "", "utf-8");
            string page = Regex.Match(html2, @"data-pages=""([\s\S]*?)""").Groups[1].Value;
            return Convert.ToInt32(page) + 1;
        }

        public int pcautogetpage(string url)
        {

            string html2 = GetUrlWithCookie(url, "", "GBK");
            string page = Regex.Match(html2, @">\.\.\.([\s\S]*?)<").Groups[1].Value;
            if (page != "")
            {
                return Convert.ToInt32(page) + 1;
            }
            else
            {
                return 10;
            }
           
        }

        string xcarcookie = "TY_SESSION_ID=cb444e47-8a9e-4c4c-8214-eda6402ac9e6; nguv=c_16193174932884344823461721897756308; __jsluid_s=c0e6f58bf0b66681dd8c3d8c046a5583; _Xdwuv=6084d2f5d1594; _Xdwnewuv=1; _PVXuv=6084d2f5699df; bbs_abtest=a; bbs_visitedfid=493D542; iwt_uuid=6ed099db-84d8-4543-8d9e-2f38dc5a2257; Hm_lvt_53eb54d089f7b5dd4ae2927686b183e0=1619317494,1619488210,1619596604,1619748123; uv_firstv_refers=https%3A//www.xcar.com.cn/bbs/forumdisplay.php%3Ffid%3D493%26orderby%3Ddateline%26filter%3D%26ondigest%3D0; fw_slc=1%3A1619748159%3B1%3A1619748174%3B1%3A1619748175%3B1%3A1619748177%3B1%3A1619748203; orderby=1; fw_clc=1%3A1619748161%3B1%3A1619748201%3B1%3A1619748236; _Xdwstime=1619748261; zg_did=%7B%22did%22%3A%20%2217906d80eee993-032adcf45170d8-d7e163f-1fa400-17906d80eef2ac%22%7D; zg_8f3d0255011c4bc5bae66beca6584825=%7B%22sid%22%3A%201619748122800%2C%22updated%22%3A%201619748259128%2C%22info%22%3A%201619317493496%2C%22superProperty%22%3A%20%22%7B%5C%22platform_type%5C%22%3A%20%5C%22PC%5C%22%2C%5C%22login_id%5C%22%3A%20null%2C%5C%22project_name%5C%22%3A%20%5C%22XCAR%5C%22%2C%5C%22login_status%5C%22%3A%200%7D%22%2C%22platform%22%3A%20%22%7B%7D%22%2C%22utm%22%3A%20%22%7B%7D%22%2C%22referrerDomain%22%3A%20%22%22%2C%22zs%22%3A%200%2C%22sc%22%3A%200%2C%22firstScreen%22%3A%201619748122800%7D; fw_pvc=1%3A1619748122%3B1%3A1619748162%3B1%3A1619748202%3B1%3A1619748237%3B1%3A1619748259; Hm_lpvt_53eb54d089f7b5dd4ae2927686b183e0=1619748259; fw_exc=1%3A1619748161%3B1%3A1619748201%3B1%3A1619748236%3B1%3A1619748237%3B1%3A1619748307";
        public int xcargetpage(string url)
        {

            string html2 = GetUrlWithCookie(url, xcarcookie, "utf-8");
            string page = Regex.Match(html2, @"value\)<=([\s\S]*?)\)").Groups[1].Value;
           
            return Convert.ToInt32(page) + 1;
        }

        public string getvideoviews(string id)
        {
            string url = "https://club.autohome.com.cn/common/api/ClubObjectCounter/GetCount?type=2&idList="+id;
            string html = GetUrlWithCookie(url, "", "gb2312");
            string view = Regex.Match(html, @"views"":([\s\S]*?)\}").Groups[1].Value;
            return view;
        }



        #region 汽车之家
        public void autohome()
        {

            
            for (int i = 0; i <textBox1.Lines.Length;i++)
            {
               
                string startUrl = textBox1.Lines[i].ToString().Trim();
                int totalpage = autohomegetpage(startUrl);

                for (int page = 1; page < totalpage; page++)
                {

                  string url=  Regex.Replace(startUrl, @"\d\.html", page.ToString()+".html");

                   
                    string html2 = GetUrlWithCookie(url, "", "gb2312");
                    string html = Regex.Match(html2, @"<ul class=""list_dlsubtitle"">([\s\S]*?)<div class=""pagearea"">").Groups[1].Value;
             

                    string name = Regex.Match(html2, @"<h1 title=""([\s\S]*?)""").Groups[1].Value;

                    MatchCollection uids = Regex.Matches(html, @"<dd class=""cli_dd"" lang=""([\s\S]*?)""");
                    MatchCollection videois = Regex.Matches(html, @"<dd class=""cli_dd"" ([\s\S]*?)<span");

                    MatchCollection titles = Regex.Matches(html, @"<a class=""a_topic""([\s\S]*?)href=""([\s\S]*?)"">([\s\S]*?)</a>");
                    MatchCollection dates = Regex.Matches(html, @"<span class=""tdate"">([\s\S]*?)</span>");

                    MatchCollection anames = Regex.Matches(html, @"<span class=""tcount"">([\s\S]*?)linkblack"">([\s\S]*?)</a>");
                    MatchCollection adates = Regex.Matches(html, @"<span class=""ttime"">([\s\S]*?)</span>");

                    MatchCollection dds = Regex.Matches(html, @"<dl class=""list_dl""   lang=""([\s\S]*?)""");
                    if (uids.Count == 0)
                        break;

                    StringBuilder sb = new StringBuilder();

                    foreach (Match uid in uids)
                    {
                        sb.Append(uid.Groups[1].Value+"%2C");
                    }


                  

                    Dictionary<string, string> videoidsdic = new Dictionary<string, string>();
                    for (int a = 0; a < videois.Count; a++)
                    {
                     
                        Match uid = Regex.Match(videois[a].Groups[1].Value, @"lang=""([\s\S]*?)""");
                        Match videoid = Regex.Match(videois[a].Groups[1].Value, @"data-videoid=([\s\S]*?)>");
                        if (videoid.Groups[1].Value != "")
                        {
                   
                            videoidsdic.Add(uid.Groups[1].Value, videoid.Groups[1].Value);
                        }


                       
                    }

                    Thread.Sleep(2000);
                    Dictionary<string, string> dics = new Dictionary<string, string>();
                    string aurl = "https://clubajax.autohome.com.cn/topic/rv?fun=jsonprv&callback=jsonprv&ids="+sb.ToString()+"&r=Thu+Apr+01+2021+12%3A27%3A32+GMT%2B0800+(%E4%B8%AD%E5%9B%BD%E6%A0%87%E5%87%86%E6%97%B6%E9%97%B4)&callback=jsonprv&_=1617251252573";
                    string ahtml = GetUrlWithCookie(aurl, "", "gb2312");
                    MatchCollection values = Regex.Matches(ahtml, @"""topicid"":([\s\S]*?),");
                    MatchCollection values2 = Regex.Matches(ahtml, @"""views"":([\s\S]*?),");

                    for (int a = 0; a < values.Count; a++)
                    {
                        dics.Add(values[a].Groups[1].Value,values2[a].Groups[1].Value);

                    }
                    for (int j = 0; j < uids.Count; j++)
                    {
                        try
                        {
                            string[] text = dds[j].Groups[1].Value.Split(new string[] { "|" }, StringSplitOptions.None);

                            string biaozhi = text[7] + text[8] + text[9];

                      
                            string liulanl = dics[uids[j].Groups[1].Value];
                            string icon = "";
                            switch (biaozhi)
                            {
                                case "0181":
                                    icon = "问";
                                    break;
                                case "301":
                                    icon = "精";
                                    break;
                                case "001":
                                    icon = "图";
                                    break;
                                case "0180":
                                    icon = "问";
                                    break;
                                case "101":
                                    icon = "荐";
                                    break;
                            }

                           

                            if (videoidsdic.ContainsKey(uids[j].Groups[1].Value))
                            {
                                icon = "视频";
                                liulanl = getvideoviews(videoidsdic[uids[j].Groups[1].Value]);
                            }
                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据

                            lv1.SubItems.Add(titles[j].Groups[3].Value);
                            lv1.SubItems.Add("https://club.autohome.com.cn" + titles[j].Groups[2].Value);
                            lv1.SubItems.Add(liulanl);
                            lv1.SubItems.Add(text[3]);

                            lv1.SubItems.Add(dates[j].Groups[1].Value.Trim());
                            lv1.SubItems.Add(icon);
                            lv1.SubItems.Add(name);

                            lv1.SubItems.Add(text[10]);
                            lv1.SubItems.Add(anames[j].Groups[2].Value.Trim());
                            lv1.SubItems.Add(adates[j].Groups[1].Value.Trim());
                            lv1.SubItems.Add("汽车之家");
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)
                                return;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                           continue;
                        }
                    }

                    Thread.Sleep(3000);
                }
            }


        }

        #endregion

        #region 易车
        public void yiche()
        {


            for (int i = 0; i < textBox1.Lines.Length; i++)
            {

                string startUrl = textBox1.Lines[i].ToString().Trim();
                int totalpage = yichegetpage(startUrl);

                for (int page = 1; page < totalpage; page++)
                {

                    string url = Regex.Replace(startUrl, @"\d\.html", page.ToString() + ".html");
                    string html = GetUrlWithCookie(url, "", "utf-8");
                    MatchCollection ahtmls = Regex.Matches(html, @"<a target=""_blank"" data-([\s\S]*?)</a>");

                    if (ahtmls.Count == 0)
                        break;

                    for (int j = 5; j < ahtmls.Count; j++)
                    {
                        try
                        {
                            string title = Regex.Match(ahtmls[j].Groups[1].Value, @"<span class=""title"">([\s\S]*?)</span>").Groups[1].Value;
                            string link = "https://baa.yiche.com" + Regex.Match(ahtmls[j].Groups[1].Value, @"href=""([\s\S]*?)""").Groups[1].Value;
                            string huifu= Regex.Match(ahtmls[j].Groups[1].Value, @"<div class=""tz-item-txt item-top repNum"">([\s\S]*?)</div>").Groups[1].Value;
                            string liulan = Regex.Match(ahtmls[j].Groups[1].Value, @"<div class=""tz-item-txt repNum"">([\s\S]*?)</div>").Groups[1].Value;
                           MatchCollection  fabutime= Regex.Matches(ahtmls[j].Groups[1].Value, @"<div class=""tz-item-txt item-bot"">([\s\S]*?)</div>");
                            string type = Regex.Match(ahtmls[j].Groups[1].Value, @"<span class=""icon([\s\S]*?)"">([\s\S]*?)</span>").Groups[2].Value;
                            string cartype = Regex.Match(html, @"【([\s\S]*?)】").Groups[1].Value;
                            MatchCollection fatieren = Regex.Matches(ahtmls[j].Groups[1].Value, @"data-uid=""([\s\S]*?)"">([\s\S]*?)</div>");
                            string lastfatieren = fatieren[1].Groups[2].Value;
                            string lastfatietime = fabutime[1].Groups[1].Value;


                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据

                            lv1.SubItems.Add(Regex.Replace(title.Trim(), "<[^>]+>", ""));
                            lv1.SubItems.Add(link);
                            lv1.SubItems.Add(liulan.Trim());
                            lv1.SubItems.Add(huifu.Trim());
                           
                            lv1.SubItems.Add(fabutime[0].Groups[1].Value.Trim());
                            lv1.SubItems.Add(type.Trim());
                            lv1.SubItems.Add(cartype.Trim());
                            lv1.SubItems.Add(fatieren[0].Groups[2].Value.Trim());
                            lv1.SubItems.Add(lastfatieren.Trim());
                            lv1.SubItems.Add(lastfatietime.Trim());
                            lv1.SubItems.Add("易车");
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)
                                return;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                            continue;
                        }
                    }

                    Thread.Sleep(2000);
                }
            }


        }

        #endregion

        #region 太平洋汽车
        public void pcauto()
        {


            for (int i = 0; i < textBox1.Lines.Length; i++)
            {

                string startUrl = textBox1.Lines[i].ToString().Trim();
                string fid = Regex.Match(startUrl, @"\d{3,}").Groups[0].Value;

                int totalpage = pcautogetpage(startUrl);

                for (int page = 1; page < totalpage; page++)
                {

                    string url = "https://bbs.pcauto.com.cn/forum-"+fid+"-"+page+"_postat.html";
                  
                    string html = GetUrlWithCookie(url,"","GBK");
                    MatchCollection ahtmls = Regex.Matches(html, @"<tr onmouseout([\s\S]*?)</tbody>");
                    MatchCollection uids = Regex.Matches(html, @"<th class=""title checkbox_title2"" tid=""([\s\S]*?)""");
                    if (ahtmls.Count == 0)
                        break;
                    StringBuilder sb = new StringBuilder();
                    Dictionary<string, string> dics = new Dictionary<string, string>();
                   
                    foreach (Match uid in uids)
                    {
                        sb.Append(uid.Groups[1].Value + "%2C");
                    }
                    string aurl = "https://bbs.pcauto.com.cn/forum/loadStaticInfos.ajax?isBrandForum=true&tids="+sb.ToString().Substring(0, sb.ToString().Length-3) +"&fid="+ fid;
                 
                    string ahtml = GetUrlWithCookie(aurl, "", "GBK");


                    MatchCollection values = Regex.Matches(ahtml, @"""tid"":([\s\S]*?),""view"":([\s\S]*?)\}");
                  
                    for (int a = 0; a < values.Count; a++)
                    {
                        if (!dics.ContainsKey(values[a].Groups[1].Value))
                        {
                            dics.Add(values[a].Groups[1].Value, values[a].Groups[2].Value);
                        }

                    }


                   

                    for (int j = 0; j < ahtmls.Count; j++)
                    {
                        try
                        {
                            string tid = Regex.Match(ahtmls[j].Groups[1].Value, @"tid=""([\s\S]*?)""").Groups[1].Value;
                            string title = Regex.Match(ahtmls[j].Groups[1].Value, @"<span class=""checkbox_title"" >([\s\S]*?)</span>").Groups[1].Value;
                            string link = "https:" + Regex.Match(ahtmls[j].Groups[1].Value, @"href=""([\s\S]*?)""").Groups[1].Value;
                           
                            string liulan =dics[tid];
                           Match huifu = Regex.Match(ahtmls[j].Groups[1].Value, @"<td class=""nums"">([\s\S]*?)</cite>");
                            MatchCollection fabutime = Regex.Matches(ahtmls[j].Groups[1].Value, @"<em>([\s\S]*?)</em>");
                            string type = Regex.Match(ahtmls[j].Groups[1].Value, @"title=""([\s\S]*?)""").Groups[1].Value;
                            string cartype = Regex.Match(html, @"<title>([\s\S]*?)_").Groups[1].Value;
                            MatchCollection fatieren = Regex.Matches(ahtmls[j].Groups[1].Value, @"rel=""nofollow"">([\s\S]*?)</a>");
                            string lastfatieren = fatieren[1].Groups[1].Value;
                            string lastfatietime = fabutime[1].Groups[1].Value;


                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据

                            lv1.SubItems.Add(Regex.Replace(title.Trim(), "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(link);
                            lv1.SubItems.Add(liulan.Trim());
                            lv1.SubItems.Add(Regex.Replace(huifu.Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(fabutime[0].Groups[1].Value.Trim());
                            lv1.SubItems.Add(type.Trim());
                            lv1.SubItems.Add(cartype.Trim());
                            lv1.SubItems.Add(fatieren[0].Groups[1].Value.Trim());
                            lv1.SubItems.Add(lastfatieren.Trim());
                            lv1.SubItems.Add(lastfatietime.Trim());
                            lv1.SubItems.Add("太平洋汽车");
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)
                                return;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                            continue;
                        }
                    }

                    Thread.Sleep(2000);
                }
            }


        }

        #endregion

        #region 爱卡汽车
        public void xcar()
        {


            for (int i = 0; i < textBox1.Lines.Length; i++)
            {

                string startUrl = textBox1.Lines[i].ToString().Trim();
                int totalpage = xcargetpage(startUrl);

                for (int page = 1; page < totalpage; page++)
                {

                    string url = startUrl + "&orderby=dateline&filter=&ondigest=0&page=" + page;
                  
                    string html = GetUrlWithCookie(url, xcarcookie, "utf-8");
                    MatchCollection ahtmls = Regex.Matches(html, @"<dl class=""list_dl"">([\s\S]*?)</dl>");

                    if (ahtmls.Count == 0)
                        break;

                    for (int j = 0; j < ahtmls.Count; j++)
                    {
                        try
                        {
                            string title = Regex.Match(ahtmls[j].Groups[1].Value, @"<a class=""titlink""([\s\S]*?)>([\s\S]*?)</a>").Groups[2].Value;
                            string link = "https://www.xcar.com.cn/" + Regex.Match(ahtmls[j].Groups[1].Value, @"href=""([\s\S]*?)""").Groups[1].Value;
                            string liulan = Regex.Match(ahtmls[j].Groups[1].Value, @"<span class=""tcount"">([\s\S]*?)</span>").Groups[1].Value;
                            string huifu = Regex.Match(ahtmls[j].Groups[1].Value, @"<span class=""fontblue"">([\s\S]*?)</span>").Groups[1].Value;
                            string fabutime = Regex.Match(ahtmls[j].Groups[1].Value, @"<span class=""tdate"">([\s\S]*?)</span>").Groups[1].Value;
                            string type = Regex.Match(ahtmls[j].Groups[1].Value, @"<i class=""icon icon-([\s\S]*?)""").Groups[1].Value;
                            string cartype = Regex.Match(html, @"【([\s\S]*?)】").Groups[1].Value;
                            MatchCollection fatieren = Regex.Matches(ahtmls[j].Groups[1].Value, @"class=""linkblack"">([\s\S]*?)</a>");
                            string lastfatieren = fatieren[1].Groups[1].Value;
                            string lastfatietime = Regex.Match(ahtmls[j].Groups[1].Value, @"<span class=""ttime"">([\s\S]*?)</span>").Groups[1].Value;


                            string type1 = "";
                            switch (type.Trim())
                            {
                                case "pic":
                                    type1 = "图";
                                    break;
                                case "essence":
                                    type1 = "精";
                                    break;
                                case "flags":
                                    type1 = "活";
                                    break;
                                case "phb":
                                    type1 = "投";
                                    break;
                            }

                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据

                            lv1.SubItems.Add(Regex.Replace(title.Trim(), "<[^>]+>", ""));
                            lv1.SubItems.Add(link);
                            lv1.SubItems.Add(liulan.Trim());
                            lv1.SubItems.Add(huifu.Trim());
                           
                            lv1.SubItems.Add(fabutime.Trim());
                            lv1.SubItems.Add(type1);
                            lv1.SubItems.Add(cartype.Trim());
                            lv1.SubItems.Add(fatieren[0].Groups[1].Value.Trim());
                            lv1.SubItems.Add(lastfatieren.Trim());
                            lv1.SubItems.Add(lastfatietime.Trim());
                            lv1.SubItems.Add("爱卡汽车");
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)
                                return;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                            continue;
                        }
                    }

                    Thread.Sleep(2000);
                }
            }


        }

        #endregion



        #region 查询帖子
        public void autohometiezi()
        {

            for (int i = 0; i < textBox2.Lines.Length; i++)
            {
                string url = textBox2.Lines[i].ToString().Trim();
                string id= Regex.Match(url, @"\d{5,}").Groups[0].Value;
                string html = GetUrlWithCookie(url, "", "utf-8");

                string ahtml= GetUrlWithCookie("https://club.autohome.com.cn/frontapi/getclicksandreplys?topicids="+id, "", "utf-8");

                if (html.Contains("主楼已被删除"))
                {
                    ListViewItem lv= listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv.SubItems.Add("被删除");
                    lv.SubItems.Add(url);
                    continue;
                }
                string title = Regex.Match(html, @"<title>([\s\S]*?)</title>").Groups[1].Value;
                string type = Regex.Match(html, @"<span class=""post-title-mark"">([\s\S]*?)</span>").Groups[1].Value;
                string liulan = Regex.Match(ahtml, @"""views"":([\s\S]*?),").Groups[1].Value;
                string huifu = Regex.Match(ahtml, @"""replys"":([\s\S]*?),").Groups[1].Value;
                string fabutime = Regex.Match(html, @"发表于<strong>([\s\S]*?)</strong>").Groups[1].Value;


                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据

                lv1.SubItems.Add(title);
                lv1.SubItems.Add(url);
                lv1.SubItems.Add(liulan.Trim());
                lv1.SubItems.Add(huifu.Trim());
                lv1.SubItems.Add(fabutime.Trim());
                lv1.SubItems.Add(type.Trim());
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("汽车之家");
                while (this.zanting == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }
                if (status == false)
                    return;


                Thread.Sleep(1000);
                }
            }

        public void yichetiezi()
        {

            for (int i = 0; i < textBox2.Lines.Length; i++)
            {
                string url = textBox2.Lines[i].ToString().Trim();
                string id = Regex.Match(url, @"\d{5,}").Groups[0].Value;
                string html = GetUrlWithCookie(url, "", "utf-8");

               
                string title = Regex.Match(html, @"<title>([\s\S]*?)_").Groups[1].Value;
                string type = Regex.Match(html, @"<i class=""post-tiwen"">([\s\S]*?)</i>").Groups[1].Value;
                string liulan = Regex.Match(html, @"<span class=""view-num"">浏览([\s\S]*?)</span>").Groups[1].Value;
                string huifu = Regex.Match(html, @"<i id=""huiNumber"">([\s\S]*?)</i>").Groups[1].Value;
                string fabutime = Regex.Match(html, @"<span class=""post-time"">发表于([\s\S]*?)</span>").Groups[1].Value;


                if (title=="")
                {
                    ListViewItem lv = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv.SubItems.Add("被删除");
                    lv.SubItems.Add(url);
                    continue;
                }


                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据

                lv1.SubItems.Add(title);
                lv1.SubItems.Add(url);
                lv1.SubItems.Add(liulan.Trim());
                lv1.SubItems.Add(huifu.Trim());
                lv1.SubItems.Add(fabutime.Trim());
                lv1.SubItems.Add(type.Trim());
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
            
                lv1.SubItems.Add("易车");
                while (this.zanting == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }
                if (status == false)
                    return;


                Thread.Sleep(1000);
            }
        }

        public void pcautotiezi()
        {

            for (int i = 0; i < textBox2.Lines.Length; i++)
            {
                string url = textBox2.Lines[i].ToString().Trim();
                string id = Regex.Match(url, @"\d{5,}").Groups[0].Value;
                string html = GetUrlWithCookie(url, "", "GBK");
              
                string ahtml = GetUrlWithCookie("https://bbs.pcauto.com.cn/intf/topic/counter.ajax?tid=" + id, "u=4118ym2c; u4ad=417x48ct; pcsuv=1618476965044.a.175645191; pcLocate=%7B%22proCode%22%3A%22320000%22%2C%22pro%22%3A%22%E6%B1%9F%E8%8B%8F%E7%9C%81%22%2C%22cityCode%22%3A%22321300%22%2C%22city%22%3A%22%E5%AE%BF%E8%BF%81%E5%B8%82%22%2C%22dataType%22%3A%22ipJson%22%2C%22expires%22%3A1619772964958%7D; PClocation=390; pcautoLocate=%7B%22proId%22%3A1%2C%22cityId%22%3A390%2C%22url%22%3A%22%2F%2Fwww.pcauto.com.cn%2Fqcbj%2Fsuqian%2F%22%2C%22dataTypeAuto%22%3A%22region_ipArea%22%7D; favCar=%E5%9D%A6%E5%85%8B300_26541; sensorsdata2015jssdkcross=%7B%22distinct_id%22%3A%2217906df8ac15a-0a8094cfae8018-d7e163f-2073600-17906df8ac2b68%22%2C%22first_id%22%3A%22%22%2C%22props%22%3A%7B%22%24latest_traffic_source_type%22%3A%22%E7%9B%B4%E6%8E%A5%E6%B5%81%E9%87%8F%22%2C%22%24latest_search_keyword%22%3A%22%E6%9C%AA%E5%8F%96%E5%88%B0%E5%80%BC_%E7%9B%B4%E6%8E%A5%E6%89%93%E5%BC%80%22%2C%22%24latest_referrer%22%3A%22%22%7D%2C%22%24device_id%22%3A%2217906df8ac15a-0a8094cfae8018-d7e163f-2073600-17906df8ac2b68%22%7D; visitedfid=16040; ivy_look_number_475946_688267=1; iwt_uuid=90f3ba2f-e423-4d4f-8169-08664dd8f91f; __v24d714fb2336ae1477023d674cbf81fd=1; ivy_look_number_208004_700160=1; ivy_look_number_503894_667468=1; channel=9630; ivy_look_number_509339_650703=1; __v704088=2; __v708856=1; __v707639=1; __v708786=3; __v707559=1; __v704001=3; __v708787=1; __v708859=3; ivy_look_number_106349_658972=2; ivy_look_number_222090_692657=2; pcuvdata=lastAccessTime=1619596160506|visits=11", "utf-8");
                string title = Regex.Match(html, @"<title>([\s\S]*?)_").Groups[1].Value;
                string type = Regex.Match(html, @"<div class=""post_r_tit"">([\s\S]*?)</a>").Groups[1].Value;
                string liulan = Regex.Match(ahtml, @"""views"":([\s\S]*?)}").Groups[1].Value;
                string huifu = Regex.Match(html, @"<span class=""yh"">([\s\S]*?)</span>").Groups[1].Value;
                string fabutime = Regex.Match(html, @"发表于([\s\S]*?)</div>").Groups[1].Value;
                if (title == "")
                {
                    ListViewItem lv = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv.SubItems.Add("被删除");
                    lv.SubItems.Add(url);
                    continue;
                }

                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据

                lv1.SubItems.Add(title.Trim());
                lv1.SubItems.Add(url);
                lv1.SubItems.Add(liulan.Trim());
                lv1.SubItems.Add(huifu.Trim());
                lv1.SubItems.Add(fabutime.Trim());
                lv1.SubItems.Add(Regex.Replace(type.Trim(), "<[^>]+>", ""));
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("太平洋汽车");
                while (this.zanting == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }
                if (status == false)
                    return;


                Thread.Sleep(1000);
            }
        }

        public void xcartiezi()
        {

            for (int i = 0; i < textBox2.Lines.Length; i++)
            {
                string url = textBox2.Lines[i].ToString().Trim();
                string id = Regex.Match(url, @"\d{5,}").Groups[0].Value;
                string html = GetUrlWithCookie(url, xcarcookie, "utf-8");

                if (html.Contains("被删除"))
                {
                    ListViewItem lv = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv.SubItems.Add("被删除");
                    lv.SubItems.Add(url);
                    continue;
                }
                string title = Regex.Match(html, @"<title>([\s\S]*?)-").Groups[1].Value;
               
                string liulan = Regex.Match(html, @"<span>查看([\s\S]*?)<").Groups[1].Value;
                string huifu = Regex.Match(html, @"</i>回复([\s\S]*?)<").Groups[1].Value;
                string fabutime = Regex.Match(html, @"发表于([\s\S]*?)<").Groups[1].Value;


                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据

                lv1.SubItems.Add(title);
                lv1.SubItems.Add(url);
                lv1.SubItems.Add(liulan.Trim());
                lv1.SubItems.Add(huifu.Trim());
                lv1.SubItems.Add(fabutime.Trim());
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("爱卡汽车");
                while (this.zanting == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }
                if (status == false)
                    return;


                Thread.Sleep(1000);
            }
        }


        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrlWithCookie("http://www.acaiji.com/index/index/vip.html","", "utf-8");

            if (!html.Contains(@"yIepkOu"))
            {
                MessageBox.Show("");
                return;
            }



            #endregion

            status = true;
            if (tabControl1.SelectedIndex == 0)
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("论坛地址为空");
                    return;
                }
                if (textBox1.Text.Contains("autohome"))
                {
                    if (thread == null || !thread.IsAlive)
                    {
                        thread = new Thread(autohome);
                        thread.Start();
                        Control.CheckForIllegalCrossThreadCalls = false;
                    }
                }
                else if (textBox1.Text.Contains("yiche"))
                {

                    if (thread == null || !thread.IsAlive)
                    {
                        thread = new Thread(yiche);
                        thread.Start();
                        Control.CheckForIllegalCrossThreadCalls = false;
                    }
                }

                else if (textBox1.Text.Contains("pcauto"))
                {
                    if (thread == null || !thread.IsAlive)
                    {
                        thread = new Thread(pcauto);
                        thread.Start();
                        Control.CheckForIllegalCrossThreadCalls = false;
                    }
                }
                else if (textBox1.Text.Contains("xcar"))
                {
                    if (thread == null || !thread.IsAlive)
                    {
                        thread = new Thread(xcar);
                        thread.Start();
                        Control.CheckForIllegalCrossThreadCalls = false;
                    }
                }
            }
            if (tabControl1.SelectedIndex == 1)
            {
                if (textBox2.Text == "")
                {
                    MessageBox.Show("帖子地址为空");
                    return;
                }
                
                if (textBox2.Text.Contains("autohome"))
                {
                    if (thread == null || !thread.IsAlive)
                    {
                        thread = new Thread(autohometiezi);
                        thread.Start();
                        Control.CheckForIllegalCrossThreadCalls = false;
                    }
                }
                else if (textBox2.Text.Contains("yiche"))
                {

                    if (thread == null || !thread.IsAlive)
                    {
                        thread = new Thread(yichetiezi);
                        thread.Start();
                        Control.CheckForIllegalCrossThreadCalls = false;
                    }
                }

                else if (textBox2.Text.Contains("pcauto"))
                {
                    if (thread == null || !thread.IsAlive)
                    {
                        thread = new Thread(pcautotiezi);
                        thread.Start();
                        Control.CheckForIllegalCrossThreadCalls = false;
                    }
                }
                else if (textBox2.Text.Contains("xcar"))
                {
                    if (thread == null || !thread.IsAlive)
                    {
                        thread = new Thread(xcartiezi);
                        thread.Start();
                        Control.CheckForIllegalCrossThreadCalls = false;
                    }
                }
            }
           
        }

        Thread thread;
        bool status = true;
        bool zanting = true;

        #region  listView导出CSV
        /// <summary>
        /// 导出CSV
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="includeHidden"></param>
        /// 
        public static void ListViewToCSV(ListView listView, bool includeHidden)
        {
            //make header string
            SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "xlsx|*.xls|xlsx|*.xlsx";

            //sfd.Title = "Excel文件导出";
            string filePath = "";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                filePath = sfd.FileName + ".csv";
            }
            StringBuilder result = new StringBuilder();
            WriteCSVRow(result, listView.Columns.Count, i => includeHidden || listView.Columns[i].Width > 0, i => listView.Columns[i].Text);

            //export data rows
            foreach (ListViewItem listItem in listView.Items)
                WriteCSVRow(result, listView.Columns.Count, i => includeHidden || listView.Columns[i].Width > 0, i => listItem.SubItems[i].Text);

            File.WriteAllText(filePath, result.ToString(), Encoding.Default);
            MessageBox.Show("导出成功");
        }


        private static void WriteCSVRow(StringBuilder result, int itemsCount, Func<int, bool> isColumnNeeded, Func<int, string> columnValue)
        {
            bool isFirstTime = true;
            for (int i = 0; i < itemsCount; i++)
            {
                try
                {

                    if (!isColumnNeeded(i))
                        continue;

                    if (!isFirstTime)
                        result.Append(",");
                    isFirstTime = false;

                    result.Append(String.Format("\"{0}\"", columnValue(i)));
                }
                catch
                {
                    continue;
                }
            }

            result.AppendLine();
        }

        #endregion

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
            //ListViewToCSV(listView1,true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 汽车之家论坛_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void 汽车之家论坛_Load(object sender, EventArgs e)
        {

        }
    }
}
