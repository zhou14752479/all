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
        string[] citys = {
            "hf",
"wuhu",
"bengbu",
"fy",
"hn",
"anqing",
"suzhou",
"la",
"huaibei",
"chuzhou",
"mas",
"tongling",
"xuancheng",
"bozhou",
"huangshan",
"chizhou",
"ch",
"hexian",
"hq",
"tongcheng",
"ningguo",
"tianchang",
"dongzhi",
"wuweixian",
"sz",
"gz",
"dg",
"fs",
"zs",
"zh",
"huizhou",
"jm",
"st",
"zhanjiang",
"zq",
"mm",
"jy",
"mz",
"qingyuan",
"yj",
"sg",
"heyuan",
"yf",
"sw",
"chaozhou",
"taishan",
"yangchun",
"sd",
"huidong",
"boluo",
"haifengxian",
"kaipingshi",
"lufengshi",
"nn",
"liuzhou",
"gl",
"yulin",
"wuzhou",
"bh",
"gg",
"qinzhou",
"baise",
"hc",
"lb",
"hezhou",
"fcg",
"chongzuo",
"guipingqu",
"beiliushi",
"bobaixian",
"cenxi",
"gy",
"zunyi",
"qdn",
"qn",
"lps",
"bijie",
"tr",
"anshun",
"qxn",
"renhuaishi",
"qingzhen",
"lz",
"tianshui",
"by",
"qingyang",
"pl",
"jq",
"zhangye",
"wuwei",
"dx",
"jinchang",
"ln",
"linxia",
"jyg",
"gn",
"dunhuang",
"haikou",
"sanya",
"wzs",
"sansha",
"qh",
"wenchang",
"wanning",
"tunchang",
"qiongzhong",
"lingshui",
"df",
"da",
"cm",
"baoting",
"baish",
"danzhou",
"zz",
"luoyang",
"xx",
"ny",
"xc",
"pds",
"ay",
"jiaozuo",
"sq",
"kaifeng",
"puyang",
"zk",
"xy",
"zmd",
"luohe",
"smx",
"hb",
"jiyuan",
"mg",
"yanling",
"yuzhou",
"changge",
"lingbaoshi",
"qixianqu",
"ruzhou",
"xiangchengshi",
"yanshiqu",
"changyuan",
"huaxian",
"linzhou",
"qinyang",
"mengzhou",
"wenxian",
"weishixian",
"lankaoxian",
"tongxuxian",
"lyxinan",
"yichuan",
"mengjinqu",
"lyyiyang",
"wugang",
"yongcheng",
"suixian",
"luyi",
"yingchixian",
"shenqiu",
"taikang",
"shangshui",
"qixianq",
"junxian",
"fanxian",
"gushixian",
"huaibinxian",
"dengzhou",
"xinye",
"hrb",
"dq",
"qqhr",
"mdj",
"suihua",
"jms",
"jixi",
"sys",
"hegang",
"heihe",
"yich",
"qth",
"dxal",
"shanda",
"shzhaodong",
"zhaozhou",
"wh",
"yc",
"xf",
"jingzhou",
"shiyan",
"hshi",
"xiaogan",
"hg",
"es",
"jingmen",
"xianning",
"ez",
"suizhou",
"qianjiang",
"tm",
"xiantao",
"snj",
"yidou",
"hanchuan",
"zaoyang",
"wuxueshi",
"zhongxiangshi",
"jingshanxian",
"shayangxian",
"songzi",
"guangshuishi",
"chibishi",
"laohekou",
"gucheng",
"yichengshi",
"nanzhang",
"yunmeng",
"anlu",
"dawu",
"xiaochang",
"dangyang",
"zhijiang",
"jiayuxian",
"suixia",
"cs",
"zhuzhou",
"yiyang",
"changde",
"hy",
"xiangtan",
"yy",
"chenzhou",
"shaoyang",
"hh",
"yongzhou",
"ld",
"xiangxi",
"zjj",
"liling",
"lixian",
"czguiyang",
"zixing",
"yongxing",
"changningshi",
"qidongxian",
"hengdong",
"lengshuijiangshi",
"lianyuanshi",
"shuangfengxian",
"shaoyangxian",
"shaodongxian",
"yuanjiangs",
"nanxian",
"qiyang",
"xiangyin",
"huarong",
"cilixian",
"zzyouxian",
"sjz",
"bd",
"ts",
"lf",
"hd",
"qhd",
"cangzhou",
"xt",
"hs",
"zjk",
"chengde",
"dingzhou",
"gt",
"zhangbei",
"zx",
"zd",
"qianan",
"renqiu",
"sanhe",
"wuan",
"xionganxinqu",
"lfyanjiao",
"zhuozhou",
"hejian",
"huanghua",
"cangxian",
"cixian",
"shexian",
"bazhou",
"xianghe",
"lfguan",
"zunhua",
"qianxixian",
"yutianxian",
"luannanxian",
"shaheshi",
"nc",
"ganzhou",
"jj",
"yichun",
"ja",
"sr",
"px",
"fuzhou",
"jdz",
"xinyu",
"yingtan",
"yxx",
"lepingshi",
"jinxian",
"fenyi",
"fengchengshi",
"zhangshu",
"gaoan",
"yujiang",
"nanchengx",
"fuliangxian",
"cc",
"jl",
"sp",
"yanbian",
"songyuan",
"bc",
"th",
"baishan",
"liaoyuan",
"gongzhuling",
"meihekou",
"fuyuxian",
"changlingxian",
"huadian",
"panshi",
"lishu",
"sy",
"dl",
"as",
"jinzhou",
"fushun",
"yk",
"pj",
"cy",
"dandong",
"liaoyang",
"benxi",
"hld",
"tl",
"fx",
"pld",
"wfd",
"dengta",
"fengcheng",
"beipiao",
"kaiyuan",
"yinchuan",
"wuzhong",
"szs",
"zw",
"guyuan",
"hu",
"bt",
"chifeng",
"erds",
"tongliao",
"hlbe",
"bycem",
"wlcb",
"xl",
"xam",
"wuhai",
"alsm",
"hlr",
"xn",
"hx",
"haibei",
"guoluo",
"haidong",
"huangnan",
"ys",
"hainan",
"geermushi",
"qd",
"jn",
"yt",
"wf",
"linyi",
"zb",
"jining",
"ta",
"lc",
"weihai",
"zaozhuang",
"dz",
"rizhao",
"dy",
"heze",
"bz",
"lw",
"zhangqiu",
"kl",
"zc",
"shouguang",
"longkou",
"caoxian",
"shanxian",
"feicheng",
"gaomi",
"guangrao",
"huantaixian",
"juxian",
"laizhou",
"penglai",
"qingzhou",
"rongcheng",
"rushan",
"tengzhou",
"xintai",
"zhaoyuan",
"zoucheng",
"zouping",
"linqing",
"chiping",
"hzyc",
"boxing",
"dongming",
"juye",
"wudi",
"qihe",
"weishan",
"yuchengshi",
"linyixianq",
"leling",
"laiyang",
"ningjin",
"gaotang",
"shenxian",
"yanggu",
"guanxian",
"pingyi",
"tancheng",
"yiyuanxian",
"wenshang",
"liangshanx",
"lijin",
"yinanxian",
"qixia",
"ningyang",
"dongping",
"changyishi",
"anqiu",
"changle",
"linqu",
"juancheng",
"ty",
"linfen",
"dt",
"yuncheng",
"jz",
"changzhi",
"jincheng",
"yq",
"lvliang",
"xinzhou",
"shuozhou",
"linyixian",
"qingxu",
"liulin",
"gaoping",
"zezhou",
"xiangyuanxian",
"xiaoyi",
"xa",
"xianyang",
"baoji",
"wn",
"hanzhong",
"yl",
"yanan",
"ankang",
"sl",
"tc",
"shenmu",
"hancheng",
"fugu",
"jingbian",
"dingbian",
"cd",
"mianyang",
"deyang",
"nanchong",
"yb",
"zg",
"ls",
"luzhou",
"dazhou",
"scnj",
"suining",
"panzhihua",
"ms",
"ga",
"zy",
"liangshan",
"guangyuan",
"ya",
"bazhong",
"ab",
"ganzi",
"anyuexian",
"guanghanshi",
"jianyangshi",
"renshouxian",
"shehongxian",
"dazu",
"xuanhan",
"qux",
"changningx",
"xj",
"changji",
"bygl",
"yili",
"aks",
"ks",
"hami",
"klmy",
"betl",
"tlf",
"ht",
"shz",
"kzls",
"ale",
"wjq",
"tmsk",
"kel",
"alt",
"tac",
"lasa",
"rkz",
"sn",
"linzhi",
"changdu",
"nq",
"al",
"rituxian",
"gaizexian",
"km",
"qj",
"dali",
"honghe",
"yx",
"lj",
"ws",
"cx",
"bn",
"zt",
"dh",
"pe",
"bs",
"lincang",
"diqing",
"nujiang",
"milexian",
"anningshi",
"xuanwushi"
        };
        private void Zufang_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
           
        }
        #region 房天下租房
        public void fang1()
        {

            try
            {

                foreach (string city in citys)
                {
                   
                   

                    string Url = "https://" + city + ".zu.fang.com/house/a21-h316/";
                    

                    string html = method.gethtml(Url, "", "gb2312");

                    MatchCollection ids = Regex.Matches(html, @"""houseid"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    if (ids.Count <1)
                        break;

                    ArrayList lists = new ArrayList();

                    foreach (Match id in ids)
                    {
                        lists.Add("https://" + city + ".zu.fang.com/chuzu/1_" + id.Groups[1].Value + "_-1.htm");
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

                foreach (string city in citys)
                {


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

                foreach (string city in citys)
                {

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

                foreach (string city in citys)
                {


                    string Url = "http://" + city + ".baixing.com/zhengzu/?grfy=1&sortKey=createdTime";

                    string html = method.GetUrl(Url, "utf-8");

                    MatchCollection ids = Regex.Matches(html, @"data-aid='([\s\S]*?)'", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    ArrayList lists = new ArrayList();

                    foreach (Match id in ids)
                    {
                        lists.Add("http://" + city + ".baixing.com/zhengzu/a" + id.Groups[1].Value + ".html");
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
                foreach (string city in citys)
                {


                    String Url = "http://" + city + ".58.com/shangpucz/0/pn1/";
                    string html = method.GetUrl(Url, "utf-8");


                    MatchCollection TitleMatchs = Regex.Matches(html, @"https://[a-z]+.58.com/[a-z]+/[0-9]+x.shtml", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                    ArrayList lists = new ArrayList();

                    foreach (Match NextMatch in TitleMatchs)
                    {

                        if (!lists.Contains(NextMatch.Groups[0].Value))
                        {
                            lists.Add(NextMatch.Groups[0].Value);
                        }
                    }

                    foreach (string list in lists)
                    {

                        string strhtml = method.GetUrl(list, "utf-8");  //定义的GetRul方法 返回 reader.ReadToEnd()


                        string title = @"<h1 class=""c_000 f20"">([\s\S]*?)</h1>";
                        string Rxg = @"<a class=""c_000 agent-name-txt""([\s\S]*?)>([\s\S]*?)</a>";
                        string Rxg1 = @"<p class='phone-num'>([\s\S]*?)</p>";
                        string Rxg2 = @"xxdz-des"">([\s\S]*?)</span>";

                        Match titles = Regex.Match(strhtml, title);
                        Match contacts = Regex.Match(strhtml, Rxg);
                        Match tell = Regex.Match(strhtml, Rxg1);
                        Match region = Regex.Match(strhtml, Rxg2);
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(titles.Groups[1].Value.Trim());
                        lv1.SubItems.Add(contacts.Groups[2].Value.Trim());
                        lv1.SubItems.Add(tell.Groups[1].Value.Trim());
                        lv1.SubItems.Add(region.Groups[1].Value.Trim());

                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }

                        Thread.Sleep(1000);

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

                ex.ToString();
            }

        }

        #endregion

        #region 安居客商铺出售
        public void anjuke3()
        {

            try
            {
                foreach (string city in citys)
                {

                    String Url = "http://" + city + ".58.com/shangpucs/0/pn1/";
                    string html = method.GetUrl(Url, "utf-8");


                    MatchCollection TitleMatchs = Regex.Matches(html, @"https://[a-z]+.58.com/[a-z]+/[0-9]+x.shtml", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                    ArrayList lists = new ArrayList();

                    foreach (Match NextMatch in TitleMatchs)
                    {

                        if (!lists.Contains(NextMatch.Groups[0].Value))
                        {
                            lists.Add(NextMatch.Groups[0].Value);
                        }
                    }

                    foreach (string list in lists)
                    {

                        string strhtml = method.GetUrl(list, "utf-8");  //定义的GetRul方法 返回 reader.ReadToEnd()


                        string title = @"<h1 class=""c_000 f20"">([\s\S]*?)</h1>";
                        string Rxg = @"<a class=""c_000 agent-name-txt""([\s\S]*?)>([\s\S]*?)</a>";
                        string Rxg1 = @"<p class='phone-num'>([\s\S]*?)</p>";
                        string Rxg2 = @"xxdz-des"">([\s\S]*?)</span>";

                        Match titles = Regex.Match(strhtml, title);
                        Match contacts = Regex.Match(strhtml, Rxg);
                        Match tell = Regex.Match(strhtml, Rxg1);
                        Match region = Regex.Match(strhtml, Rxg2);
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(titles.Groups[1].Value.Trim());
                        lv1.SubItems.Add(contacts.Groups[2].Value.Trim());
                        lv1.SubItems.Add(tell.Groups[1].Value.Trim());
                        lv1.SubItems.Add(region.Groups[1].Value.Trim());
                        Thread.Sleep(1000);
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

                ex.ToString();
            }

        }

        #endregion

        #region 赶集商铺出租
        public void ganji2()
        {

            try
            {

                foreach (string city in citys)
                {


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

                foreach (string city in citys)
                {


                    string Url = "http://" + city + ".ganji.com/shangpucs/0/";

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
                Thread thread = new Thread(new ThreadStart(anjuke2));
                thread.Start();
            }
            else if (comboBox1.Text == "安居客" && comboBox2.Text == "个人商铺出售")
            {
                Thread thread = new Thread(new ThreadStart(anjuke3));
                thread.Start();
            }

            else if (comboBox1.Text == "赶集商铺出租")
            {
                Thread thread = new Thread(new ThreadStart(ganji2));
                thread.Start();
            }

            else if (comboBox1.Text == "赶集商铺出售")
            {
                Thread thread = new Thread(new ThreadStart(ganji3));
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
