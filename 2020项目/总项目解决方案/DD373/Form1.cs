using System;
using System.Collections;
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

namespace DD373
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool zanting = true;
        bool status = true;


        public static string GetUrl(string Url, string charset)
        {


            try
            {
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "1s1k453=ysyk_web62; JSESSIONID=C38B6D4A827F086C722EA8DAC0E55D26; UM_distinctid=1704d60505528a-0f30e2260ef312-2393f61-1fa400-1704d6050576ae; CNZZDATA1253333710=885277478-1581844031-%7C1581995363; CNZZDATA1253416210=1453523664-1581844817-%7C1581992732";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                string content = reader.ReadToEnd();


                reader.Close();
                response.Close();
                return content;



            }
            catch (System.Exception ex)
            {
                ex.ToString();

            }
            return "";
        }

       
 

        public void run()
        {

            try
            {


                foreach (string Url in dic.Values)
                {

                    string html = GetUrl(Url, "utf-8");
                    Match infos = Regex.Match(html, @"为您找到：([\s\S]*?)</a>");

                    MatchCollection ids = Regex.Matches(html, @"buy\/third-([\s\S]*?)\.");

                    MatchCollection jbsl = Regex.Matches(html, @"class=""titleText([\s\S]*?)title=""([\s\S]*?)金");
                    MatchCollection danjia1 = Regex.Matches(html, @"<p>0([\s\S]*?)</p>");
                    MatchCollection jianshu = Regex.Matches(html, @"<div class=""num left"">([\s\S]*?)</div>");
                    if (ids.Count == 0)
                    {
                        MessageBox.Show("抓取被屏蔽");
                        MessageBox.Show("点击继续运行");
                       html= GetUrl(Url, "utf-8");
                        infos = Regex.Match(html, @"为您找到：([\s\S]*?)</a>");
                        ids = Regex.Matches(html, @"buy\/third-([\s\S]*?)\.");
                        jbsl = Regex.Matches(html, @"class=""titleText([\s\S]*?)title=""([\s\S]*?)金");
                        danjia1 = Regex.Matches(html, @"<p>0([\s\S]*?)</p>");
                        jianshu = Regex.Matches(html, @"<div class=""num left"">([\s\S]*?)</div>");

                    }
                    
                    
                    
                    string fuwuqi = Regex.Replace(infos.Groups[1].Value, "<[^>]+>", "").Replace("魔兽世界怀旧服-游戏币-","").Replace("-","/");

                    bool tiaojian = false;
                    for (int j = 0; j < ids.Count; j++)
                    {
                        
                        if (!jianshu[j].Groups[1].Value.Trim().Contains("<"))

                        {

                            decimal jg = Convert.ToInt32(jbsl[j].Groups[2].Value.Trim());
                            decimal sl = Convert.ToInt32(jianshu[j].Groups[1].Value.Trim());

                            if (jg * sl >= Convert.ToInt32(textBox2.Text))
                            {
                                textBox1.Text = DateTime.Now.ToString() + "正在搜索：【" + Regex.Replace(infos.Groups[1].Value, "<[^>]+>", "") + "】";
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add(fuwuqi);
                                lv1.SubItems.Add(jbsl[j].Groups[2].Value);
                                lv1.SubItems.Add(jianshu[j].Groups[1].Value.Trim());
                                lv1.SubItems.Add("0" + danjia1[j].Groups[1].Value);

                                lv1.SubItems.Add("https://www.dd373.com/buy/third-" + ids[j].Groups[1].Value + ".html");
                                tiaojian = true;
                                break;
                            }
                            

                        }
                    }

                    if (tiaojian == false)
                    {
                        for (int j = 0; j < ids.Count; j++)
                        {

                            if (!jianshu[j].Groups[1].Value.Trim().Contains("<"))

                            {

                                decimal jg = Convert.ToInt32(jbsl[j].Groups[2].Value.Trim());
                                decimal sl = Convert.ToInt32(jianshu[j].Groups[1].Value.Trim());

                                if (jg * sl > Convert.ToInt32(textBox4.Text))
                                {
                                    textBox1.Text = DateTime.Now.ToString() + "正在搜索：【" + Regex.Replace(infos.Groups[1].Value, "<[^>]+>", "") + "】";
                                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                    lv1.SubItems.Add(fuwuqi);
                                    lv1.SubItems.Add(jbsl[j].Groups[2].Value);
                                    lv1.SubItems.Add(jianshu[j].Groups[1].Value.Trim());
                                    lv1.SubItems.Add("0" + danjia1[j].Groups[1].Value);

                                    lv1.SubItems.Add("https://www.dd373.com/buy/third-" + ids[j].Groups[1].Value + ".html");
                                    tiaojian = true;
                                    break;
                                }


                            }
                        }

                    }


                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }


                    if (status == false)
                    {
                        return;
                    }

                    Thread.Sleep(100);
                }
            }


            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }


        public void cha(string Url)
        {

            try
            {


   
                    string html = GetUrl(Url, "utf-8");
                    Match infos = Regex.Match(html, @"为您找到：([\s\S]*?)</a>");

                    MatchCollection ids = Regex.Matches(html, @"buy\/third-([\s\S]*?)\.");

                    MatchCollection jbsl = Regex.Matches(html, @"class=""titleText([\s\S]*?)title=""([\s\S]*?)金");
                    MatchCollection danjia1 = Regex.Matches(html, @"<p>0([\s\S]*?)</p>");
                    MatchCollection jianshu = Regex.Matches(html, @"<div class=""num left"">([\s\S]*?)</div>");
                    if (ids.Count == 0)
                    {
                        MessageBox.Show("抓取被屏蔽");
                        MessageBox.Show("点击继续运行");
                        html = GetUrl(Url, "utf-8");
                        infos = Regex.Match(html, @"为您找到：([\s\S]*?)</a>");
                        ids = Regex.Matches(html, @"buy\/third-([\s\S]*?)\.");
                        jbsl = Regex.Matches(html, @"class=""titleText([\s\S]*?)title=""([\s\S]*?)金");
                        danjia1 = Regex.Matches(html, @"<p>0([\s\S]*?)</p>");
                        jianshu = Regex.Matches(html, @"<div class=""num left"">([\s\S]*?)</div>");

                    }



                    string fuwuqi = Regex.Replace(infos.Groups[1].Value, "<[^>]+>", "").Replace("魔兽世界怀旧服-游戏币-", "").Replace("-", "/");

                    bool tiaojian = false;
                    for (int j = 0; j < ids.Count; j++)
                    {

                        if (!jianshu[j].Groups[1].Value.Trim().Contains("<"))

                        {

                            decimal jg = Convert.ToInt32(jbsl[j].Groups[2].Value.Trim());
                            decimal sl = Convert.ToInt32(jianshu[j].Groups[1].Value.Trim());

                            if (jg * sl >= Convert.ToInt32(textBox2.Text))
                            {
                                textBox1.Text = DateTime.Now.ToString() + "正在搜索：【" + Regex.Replace(infos.Groups[1].Value, "<[^>]+>", "") + "】";
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add(fuwuqi);
                                lv1.SubItems.Add(jbsl[j].Groups[2].Value);
                                lv1.SubItems.Add(jianshu[j].Groups[1].Value.Trim());
                                lv1.SubItems.Add("0" + danjia1[j].Groups[1].Value);

                                lv1.SubItems.Add("https://www.dd373.com/buy/third-" + ids[j].Groups[1].Value + ".html");
                                tiaojian = true;
                                break;
                            }


                        }
                    }

                    if (tiaojian == false)
                    {
                        for (int j = 0; j < ids.Count; j++)
                        {

                            if (!jianshu[j].Groups[1].Value.Trim().Contains("<"))

                            {

                                decimal jg = Convert.ToInt32(jbsl[j].Groups[2].Value.Trim());
                                decimal sl = Convert.ToInt32(jianshu[j].Groups[1].Value.Trim());

                                if (jg * sl > Convert.ToInt32(textBox4.Text))
                                {
                                    textBox1.Text = DateTime.Now.ToString() + "正在搜索：【" + Regex.Replace(infos.Groups[1].Value, "<[^>]+>", "") + "】";
                                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                    lv1.SubItems.Add(fuwuqi);
                                    lv1.SubItems.Add(jbsl[j].Groups[2].Value);
                                    lv1.SubItems.Add(jianshu[j].Groups[1].Value.Trim());
                                    lv1.SubItems.Add("0" + danjia1[j].Groups[1].Value);

                                    lv1.SubItems.Add("https://www.dd373.com/buy/third-" + ids[j].Groups[1].Value + ".html");
                                    tiaojian = true;
                                    break;
                                }


                            }
                        }

                    }


                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }


                    if (status == false)
                    {
                        return;
                    }

                    Thread.Sleep(100);
                
            }


            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        Dictionary<string, string> dic = new Dictionary<string, string>();
        private void Form1_Load(object sender, EventArgs e)
        {
            #region 1

            
            dic.Add("一区/奥罗/部落", "https://www.dd373.com/s/eja7u2-0r2mut-8fdgm6-fq0ud5-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/奥罗/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-8fdgm6-rhbcfn-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/沙尔图拉/部落", "https://www.dd373.com/s/eja7u2-0r2mut-8t9fke-awfduu-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/沙尔图拉/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-8t9fke-saejcw-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/寒脊山小径/部落", "https://www.dd373.com/s/eja7u2-0r2mut-xht6js-6uwwkd-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/寒脊山小径/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-xht6js-gdx49h-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/碧玉矿洞/部落", "https://www.dd373.com/s/eja7u2-0r2mut-vjg8bk-g5hgwg-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/碧玉矿洞/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-vjg8bk-9m9u8f-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/辛迪加/部落", "https://www.dd373.com/s/eja7u2-0r2mut-ac0ct4-qha9u5-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/辛迪加/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-ac0ct4-q7s0fm-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/哈霍兰/部落", "https://www.dd373.com/s/eja7u2-0r2mut-0acvkr-228nrn-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/哈霍兰/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-0acvkr-67492s-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/毁灭之刃/部落", "https://www.dd373.com/s/eja7u2-0r2mut-g6fjc1-pmv9k9-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/毁灭之刃/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-g6fjc1-tbb8j9-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/水晶之牙/部落", "https://www.dd373.com/s/eja7u2-0r2mut-0q7k7t-vq22w1-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/水晶之牙/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-0q7k7t-2p0ebu-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/灰烬使者/部落", "https://www.dd373.com/s/eja7u2-0r2mut-ucqwt4-91v9tc-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/灰烬使者/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-ucqwt4-ucb5n3-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/莫格莱尼/部落", "https://www.dd373.com/s/eja7u2-0r2mut-1sg5em-e2djat-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/莫格莱尼/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-1sg5em-3qumv0-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/诺格弗格/部落", "https://www.dd373.com/s/eja7u2-0r2mut-4cfm6d-en5h33-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/诺格弗格/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-4cfm6d-evedm2-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/霜语/部落", "https://www.dd373.com/s/eja7u2-0r2mut-5kp6tc-844kvm-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/霜语/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-5kp6tc-35dwxf-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/维克洛尔/部落", "https://www.dd373.com/s/eja7u2-0r2mut-9qupj9-9qjkds-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/维克洛尔/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-9qupj9-ut9nd1-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/希尔盖/部落", "https://www.dd373.com/s/eja7u2-0r2mut-up1xsk-6fst6q-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/希尔盖/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-up1xsk-detq27-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/伦鲁迪洛尔/部落", "https://www.dd373.com/s/eja7u2-0r2mut-b871w9-btdvkf-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/伦鲁迪洛尔/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-b871w9-7aw81b-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/奥金斧/部落", "https://www.dd373.com/s/eja7u2-0r2mut-1q75q3-kudwr6-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/奥金斧/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-1q75q3-xxa8ns-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/萨弗拉斯/部落", "https://www.dd373.com/s/eja7u2-0r2mut-dnw8wt-tpkwq1-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/萨弗拉斯/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-dnw8wt-ht6gax-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/骨火/部落", "https://www.dd373.com/s/eja7u2-0r2mut-xd9155-rw3eef-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/骨火/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-xd9155-bk7gcg-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/黑曜石之锋/部落", "https://www.dd373.com/s/eja7u2-0r2mut-r5bfdc-kx16fh-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/黑曜石之锋/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-r5bfdc-nm9vr8-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/末日之刃/部落", "https://www.dd373.com/s/eja7u2-0r2mut-w2gph5-jt3fk1-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/末日之刃/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-w2gph5-pgwpdj-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/祈福/部落", "https://www.dd373.com/s/eja7u2-0r2mut-bd0hgj-u8xm40-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/祈福/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-bd0hgj-ra3w34-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/辛洛斯/部落", "https://www.dd373.com/s/eja7u2-0r2mut-db2txm-pw7t1u-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/辛洛斯/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-db2txm-7tmg4s-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/怀特迈恩/部落", "https://www.dd373.com/s/eja7u2-0r2mut-1ng2h3-jeb34a-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/怀特迈恩/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-1ng2h3-c3wgdk-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/龙之召唤/部落", "https://www.dd373.com/s/eja7u2-0r2mut-x1sv94-wq1wd9-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/龙之召唤/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-x1sv94-rukfcb-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/加丁/部落", "https://www.dd373.com/s/eja7u2-0r2mut-u4312j-w6sr04-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/加丁/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-u4312j-pu0wj6-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/埃提耶什/部落", "https://www.dd373.com/s/eja7u2-0r2mut-wsq2pb-hqhg1m-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/埃提耶什/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-wsq2pb-a6482q-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/吉兹洛克/部落", "https://www.dd373.com/s/eja7u2-0r2mut-hudm34-06d8vb-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/吉兹洛克/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-hudm34-xnc2xs-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/怒炉/部落", "https://www.dd373.com/s/eja7u2-0r2mut-4vskce-d6havw-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/怒炉/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-4vskce-kaebv3-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/德姆塞卡尔/部落", "https://www.dd373.com/s/eja7u2-0r2mut-2fhtuc-x4k44p-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/德姆塞卡尔/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-2fhtuc-gkedkn-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/火锤/部落", "https://www.dd373.com/s/eja7u2-0r2mut-umahmf-c82ern-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/火锤/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-umahmf-4jrgba-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/艾隆纳亚/部落", "https://www.dd373.com/s/eja7u2-0r2mut-haf682-f6r14g-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/艾隆纳亚/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-haf682-vphv2u-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/沙顶/部落", "https://www.dd373.com/s/eja7u2-0r2mut-fggdq7-84m728-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/沙顶/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-fggdq7-g8ca3h-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/震地者/部落", "https://www.dd373.com/s/eja7u2-0r2mut-j22brf-mhcn12-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/震地者/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-j22brf-aw7w3g-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/席瓦莱恩/部落", "https://www.dd373.com/s/eja7u2-0r2mut-51w7ax-8umdke-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/席瓦莱恩/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-51w7ax-0hcxru-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/法拉克斯/部落", "https://www.dd373.com/s/eja7u2-0r2mut-kdwsv4-d7wfex-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/法拉克斯/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-kdwsv4-rvg2q9-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/乌洛克/部落", "https://www.dd373.com/s/eja7u2-0r2mut-0dwqmu-erw401-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/乌洛克/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-0dwqmu-peg8d6-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/光芒/部落", "https://www.dd373.com/s/eja7u2-0r2mut-hv0hsg-5pdhsh-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/光芒/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-hv0hsg-n4utnb-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/无畏/部落", "https://www.dd373.com/s/eja7u2-0r2mut-cgkvu7-3x2p9e-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/无畏/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-cgkvu7-22bq81-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/萨弗隆/部落", "https://www.dd373.com/s/eja7u2-0r2mut-1cqwwn-2gx5d4-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/萨弗隆/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-1cqwwn-k1h4nf-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/伊森迪奥斯/部落", "https://www.dd373.com/s/eja7u2-0r2mut-96kjqt-d57v54-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/伊森迪奥斯/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-96kjqt-kgpp8e-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/龙牙/部落", "https://www.dd373.com/s/eja7u2-0r2mut-phks8p-kcm46h-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/龙牙/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-phks8p-bd7x4m-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/法琳娜/部落", "https://www.dd373.com/s/eja7u2-0r2mut-cgnub8-a834g7-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/法琳娜/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-cgnub8-unxx62-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/克罗米/部落", "https://www.dd373.com/s/eja7u2-0r2mut-520s3q-huj0a9-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("一区/克罗米/联盟", "https://www.dd373.com/s/eja7u2-0r2mut-520s3q-btgkca-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/维希度斯/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-j2nu9r-m086u9-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/维希度斯/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-j2nu9r-a6reg4-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/匕首岭/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-ptugvs-kut4va-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/匕首岭/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-ptugvs-cv6hv0-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/木喉要塞/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-5n5au3-3vcjm4-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/木喉要塞/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-5n5au3-rrwu40-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/布鲁/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-jn7r4r-3ugdma-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/布鲁/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-jn7r4r-144jc3-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/范克瑞斯/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-pe5bdm-89fnqm-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/范克瑞斯/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-pe5bdm-1x48hs-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/帕奇维克/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-t3w66g-q6hsm6-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/帕奇维克/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-t3w66g-rqv63j-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/比斯巨兽/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-mwxuh6-p5w51v-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/比斯巨兽/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-mwxuh6-6se7h4-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/比格沃斯/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-psgfsv-uax61g-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/比格沃斯/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-psgfsv-mc61bj-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/狮心/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-tqb8p3-trpmr2-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/狮心/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-tqb8p3-3dx0xx-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/觅心者/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-f6dqw2-tuqa0j-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/觅心者/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-f6dqw2-ndvu82-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/巴罗夫/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-v6d025-he5qqc-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/巴罗夫/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-v6d025-rgh4b6-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/维克尼拉斯/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-t862e4-6xhfaf-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/维克尼拉斯/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-t862e4-qu32j5-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/秩序之源/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-d4jp31-1nb35a-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/秩序之源/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-d4jp31-1us7gu-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/灵风/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-5ks70q-abaf5v-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/灵风/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-5ks70q-nvxrp4-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/卓越/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-a34bu9-c14vg6-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/卓越/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-a34bu9-rev4u7-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/审判/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-5acd49-a063qn-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/审判/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-5acd49-hhtv1m-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/无尽风暴/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-9ds7xm-sttj99-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/无尽风暴/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-9ds7xm-389ru9-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/巨龙追猎者/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-ct2tmp-nw0m8f-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/巨龙追猎者/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-ct2tmp-ekv667-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/巨人追猎者/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-e5ph3g-599gdj-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/巨人追猎者/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-e5ph3g-hetr3m-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/狂野之刃/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-fhhafx-qu7mb1-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/狂野之刃/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-fhhafx-12wwp4-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/奎尔塞拉/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-2srrkx-6avegb-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/奎尔塞拉/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-2srrkx-tj002k-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/碧空之歌/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-vr5qqr-2sx5qh-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/碧空之歌/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-vr5qqr-0f5u28-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/阿什坎迪/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-36d61n-vsqjbk-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/阿什坎迪/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-36d61n-g9v7ds-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/雷霆之击/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-2udask-4xjxp2-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/雷霆之击/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-2udask-s5kpdw-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/赫洛德/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-01j91p-33u8fs-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/赫洛德/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-01j91p-0krma0-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/法尔班克斯/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-swu6xh-q46axf-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/法尔班克斯/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-swu6xh-c0qukc-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/拉姆斯登/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-tw8wuv-uk3nbb-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/拉姆斯登/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-tw8wuv-bjrubj-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/厄运之槌/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-0djqu5-8jtt0m-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/厄运之槌/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-0djqu5-mcr8g9-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/安娜丝塔丽/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-hn3p7w-fvuk68-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/安娜丝塔丽/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-hn3p7w-4wnaq2-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/娅尔罗/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-dt5vqj-kqp7q7-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/娅尔罗/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-dt5vqj-59ckg6-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/雷德/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-nurtuv-ruqjqc-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/雷德/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-nurtuv-507ggw-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/迈克斯纳/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-sq7huf-bte2tn-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/迈克斯纳/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-sq7huf-9vupdn-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/曼多基尔/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-vmkjx3-awm0d0-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/曼多基尔/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-vmkjx3-u66e8b-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/塞雷布拉斯/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-cc2hj1-1x1pe7-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/塞雷布拉斯/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-cc2hj1-a6w91b-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/诺克赛恩/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-6nk1mc-uqj11s-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/诺克赛恩/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-6nk1mc-jeumeu-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/塞卡尔/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-rj5ue9-0csf97-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/塞卡尔/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-rj5ue9-mscnsd-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/范沃森/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-tc8umm-5dxb9m-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/范沃森/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-tc8umm-67445b-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/维克托/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-8uth4p-n5guxe-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/维克托/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-8uth4p-tdvwn6-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/弗莱拉斯/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-bfgrxn-wxrdgd-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/弗莱拉斯/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-bfgrxn-m3vsg1-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/布劳缪克丝/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-njw5q4-5gv20m-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/布劳缪克丝/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-njw5q4-rx4fa1-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/寒冰之王/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-a2ehmt-xdb7ug-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/寒冰之王/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-a2ehmt-pe8vtt-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/阿鲁高/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-s363n4-91jt32-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/阿鲁高/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-s363n4-5afktx-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/湖畔镇/部落", "https://www.dd373.com/s/eja7u2-3fk9tg-dm98t9-w28na6-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            dic.Add("五区/湖畔镇/联盟", "https://www.dd373.com/s/eja7u2-3fk9tg-dm98t9-8qcm9u-0-0-jk5sj0-0-0-0-0-su-0-0-0.html");
            #endregion
        }

        private void button1_Click(object sender, EventArgs e)
        {
            status = true;
            button1.Enabled = false;
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void 查看详情ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                System.Diagnostics.Process.Start(listView1.SelectedItems[0].SubItems[5].Text);
            }
           
        }

        private void button5_Click(object sender, EventArgs e)
        {

            
            listView1.Items.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            status = false;
            button1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }
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

            File.WriteAllText(filePath, result.ToString());
            MessageBox.Show("导出成功");
        }

        private static void WriteCSVRow(StringBuilder result, int itemsCount, Func<int, bool> isColumnNeeded, Func<int, string> columnValue)
        {
            bool isFirstTime = true;
            for (int i = 0; i < itemsCount; i++)
            {
                if (i == 1 || i == 4)
                {
                    if (!isColumnNeeded(i))
                        continue;

                    if (!isFirstTime)
                        result.Append(",");
                    isFirstTime = false;

                    result.Append(String.Format("\"{0}\"", columnValue(i)));
                }
            }
            result.AppendLine();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            ListViewToCSV(listView1,true);
        }

       

        private void button7_Click(object sender, EventArgs e)
        {
            
            foreach (string ke in dic.Keys)
            {
                if (ke == textBox3.Text.Trim())
                {
                    cha(dic[ke]);
                    return;
                }


            }

            //for (int i = 0; i < listView1.Items.Count; i++)
            //{
            //    this.listView1.Items[i].Selected = false;
            //    listView1.Items[i].BackColor =Color.White;
            //}

            //    for (int i = 0; i < listView1.Items.Count; i++)
            //{
            //    if (listView1.Items[i].SubItems[1].Text.Contains(textBox3.Text.Trim()))
            //    {
                    
            //        this.listView1.Items[i].Selected = true;
            //        this.listView1.EnsureVisible(i);
                   
                   
            //        break;
            //    }
            //}


        }






    }
}
