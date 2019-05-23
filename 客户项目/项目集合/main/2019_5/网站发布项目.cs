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
using System.Web;
using System.Windows.Forms;

namespace main._2019_5
{
    public partial class 网站发布项目 : Form
    {
        bool status = true;

        public static string CleanHtml(string strHtml)
        {
            if (string.IsNullOrEmpty(strHtml)) return strHtml;
            //删除脚本
            //Regex.Replace(strHtml, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase)
            strHtml = Regex.Replace(strHtml, @"(\<script(.+?)\</script\>)|(\<style(.+?)\</style\>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            //删除标签
            var r = new Regex(@"</?[^>]*>", RegexOptions.IgnoreCase);
            Match m;
            for (m = r.Match(strHtml); m.Success; m = m.NextMatch())
            {
                strHtml = strHtml.Replace(m.Groups[0].ToString(), "");
            }
            return strHtml.Trim();
        }
        public 网站发布项目()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        bool zanting =true;
        static string aCOOKIE = "";

        public static string bCOOKIE = "";

        /// <summary>
        /// miss0304
        /// </summary>
        public void run()
        {
            aCOOKIE = textBox2.Text;
            bCOOKIE = textBox3.Text;
            int[] typeids = { 1,2,3,4,5,6};
            //int[] areaids = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };
            int[] areaids = { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };
            try
            {
                for (int i = 1; i < 1000; i = i ++)
                {
                    foreach (int areaid in areaids)
                    {

                        foreach (var typeid in typeids)
                        {


                            string url = "http://weike0403.com/class.asp?page=" + i + "&typeid=" + typeid + "&areaid=" + areaid;
                            string html = method.GetUrlWithCookie(url, aCOOKIE, "gb2312");

                            textBox1.Text = html;
                            MatchCollection ids = Regex.Matches(html, @"show.asp\?id=([\s\S]*?)""");
                            ArrayList lists = new ArrayList();

                            foreach (Match id in ids)
                            {
                                lists.Add("http://weike0403.com/show.asp?id=" + id.Groups[1].Value);
                            }


                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;

                            
                            foreach (string list in lists)

                            {
                                
                                label4.Text = "发布成功！";
                                string strhtml = method.GetUrlWithCookie(list, aCOOKIE, "gb2312");

                                Match a1 = Regex.Match(strhtml, @"信息标题：([\s\S]*?)</td>");
                                Match a2 = Regex.Match(strhtml, @"信息分类：<a href=""\/class\.asp\?typeid=([\s\S]*?)""><font color=""#FF00FF"">([\s\S]*?)</font>");
                                Match a3 = Regex.Match(strhtml, @"所属地区：<a href=""\/class\.asp\?areaid=([\s\S]*?)""><font color=""#FF6600"">([\s\S]*?)</font></a>&nbsp;-&nbsp;<a href=""\/class\.asp\?areaid=([\s\S]*?)""><font color=""#0066FF"">([\s\S]*?)</font>");
                                Match a4 = Regex.Match(strhtml, @"信息来源：([\s\S]*?)</td>");
                                Match a5 = Regex.Match(strhtml, @"小姐数量：([\s\S]*?)</td>");
                                Match a6 = Regex.Match(strhtml, @"小姐年龄：([\s\S]*?)</td>");
                                Match a7 = Regex.Match(strhtml, @"小姐素质：([\s\S]*?)</td>");
                                Match a8 = Regex.Match(strhtml, @"小姐外形：([\s\S]*?)</td>");
                                Match a9 = Regex.Match(strhtml, @"服务项目：([\s\S]*?)</td>");
                                Match a10 = Regex.Match(strhtml, @"价格一览：([\s\S]*?)</td>");
                                Match a11 = Regex.Match(strhtml, @"营业时间：([\s\S]*?)</td>");
                                Match a12 = Regex.Match(strhtml, @"环境设备：([\s\S]*?)</td>");
                                Match a13 = Regex.Match(strhtml, @"安全评估：([\s\S]*?)</td>");
                                Match a14 = Regex.Match(strhtml, @"详细地址：([\s\S]*?)</td>");
                                Match a15 = Regex.Match(strhtml, @"QQ/微信/电话：([\s\S]*?)</td>");
                                Match a16 = Regex.Match(strhtml, @"综合评价：([\s\S]*?)</td>");
                                Match a17 = Regex.Match(strhtml, @"详细介绍([\s\S]*?)<!-- 评论 begin-->");
                                MatchCollection a18 = Regex.Matches(strhtml, @"\d{14,}");  //图片地址

                                StringBuilder sb = new StringBuilder();

                                for (int s = 0; s < a18.Count; s++)
                                {
                                    sb.Append("<img src=\"http://weike0403.com/UploadFile/image/" + a18[s].Groups[0].Value + ".jpg \" />");
                                }

                                if (a1.Groups[1].Value != "" && a3.Groups[1].Value != "")
                                {
                                    ListViewItem lv1 = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());

                                    string title = HttpUtility.UrlEncode(CleanHtml(a1.Groups[1].Value), Encoding.GetEncoding("gb2312"));
                                    string laiyuan = HttpUtility.UrlEncode(CleanHtml(a4.Groups[1].Value), Encoding.GetEncoding("gb2312"));
                                    string shuliang = HttpUtility.UrlEncode(CleanHtml(a5.Groups[1].Value), Encoding.GetEncoding("gb2312"));
                                    string nianling = HttpUtility.UrlEncode(CleanHtml(a6.Groups[1].Value), Encoding.GetEncoding("gb2312"));
                                    string suzhi = HttpUtility.UrlEncode(CleanHtml(a7.Groups[1].Value), Encoding.GetEncoding("gb2312"));
                                    string waixing = HttpUtility.UrlEncode(CleanHtml(a8.Groups[1].Value), Encoding.GetEncoding("gb2312"));
                                    string xiangmu = HttpUtility.UrlEncode(CleanHtml(a9.Groups[1].Value), Encoding.GetEncoding("gb2312"));
                                    string jiage = HttpUtility.UrlEncode(CleanHtml(a10.Groups[1].Value), Encoding.GetEncoding("gb2312"));
                                    string yysj = HttpUtility.UrlEncode(CleanHtml(a11.Groups[1].Value), Encoding.GetEncoding("gb2312"));
                                    string hjsb = HttpUtility.UrlEncode(CleanHtml(a12.Groups[1].Value), Encoding.GetEncoding("gb2312"));
                                    string aqpg = HttpUtility.UrlEncode(CleanHtml(a13.Groups[1].Value), Encoding.GetEncoding("gb2312"));
                                    string adds = HttpUtility.UrlEncode(CleanHtml(a14.Groups[1].Value), Encoding.GetEncoding("gb2312"));
                                    string lxfs = HttpUtility.UrlEncode(CleanHtml(a15.Groups[1].Value), Encoding.GetEncoding("gb2312"));
                                    string zhpj = HttpUtility.UrlEncode(CleanHtml(a16.Groups[1].Value), Encoding.GetEncoding("gb2312"));
                                    string xxjs = CleanHtml(a17.Groups[1].Value);

                                    label3.Text = DateTime.Now.ToString(); 
                                    label4.Text = "正在采集.";
                                    label4.Text = "正在采集..";
                                    
                                    lv1.SubItems.Add(CleanHtml(a1.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a2.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a3.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a4.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a5.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a6.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a7.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a8.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a9.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a10.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a11.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a12.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a13.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a14.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a15.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a16.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a17.Groups[1].Value));


                                    string ftype = a2.Groups[2].Value.Trim() + "|" + a2.Groups[1].Value.Trim();
                                    string shengfen = a3.Groups[2].Value.Trim() + "|" + a3.Groups[1].Value.Trim();

                                    string diqu = a3.Groups[4].Value + "|" + a3.Groups[3].Value;
                                    string content = xxjs+sb.ToString() + "<p>管理约姐姐网【yuejj.net】提示:要求付定金 保证金的,总之提前要钱的都是骗子。</p><p>【必赢棋牌】真金游戏，真钱真刺激，不靠运气靠技术，技术好的一次赢几千，支持微信、支付宝、银行卡等多种方式上下分，想玩就玩，想走就走，安全可靠，注册就送18，可提现，点击下载：http://lol.wutgb6y.com:8899/?GameID=1&channelCode=1278</p>"+ "<a href=\"http://lol.wutgb6y.com:8899/?GameID=1&channelCode=1278\"<img src=\"http://yuejj.net/111.png\" /></a>" + "<p>【需要更新城市资源以及聊天的请添加3万人聊天技术讨论群，随便聊不踢人（乱发广告的除外），点击立刻加入</p><p>https://pt.im/joinchat/RmS8my3KbP_594rgWGFGEw</p><p>potato频道：https://pt.im/yuejj</p>";

                                    string q1 = HttpUtility.UrlEncode(ftype, Encoding.GetEncoding("gb2312"));
                                    string q2 = HttpUtility.UrlEncode(shengfen, Encoding.GetEncoding("gb2312"));
                                    string q3 = HttpUtility.UrlEncode(diqu, Encoding.GetEncoding("gb2312"));
                                    string q4 = HttpUtility.UrlEncode(content, Encoding.GetEncoding("gb2312"));
                                    string posturl = "http://yuejj.net/users/addinfo.asp?action=add";

                                    string postData = "ftype=" + q1 + "&shengfen=" + q2 + "&diqu=" + q3 + "&title=" + title + "&adds=" + adds + "&laiyuan=" + laiyuan + "&shuliang=" + shuliang + "&nianling=" + nianling + "&suzhi=" + suzhi + "&waixing=" + waixing + "&xiangmu=" + xiangmu + "&jiage=" + jiage + "&yysj=" + yysj + "&hjsb=" + hjsb + "&aqpg=" + aqpg + "&lxfs=" + lxfs + "&zhpj=" + zhpj + "&content=%3Cp%3E%26nbsp%3B" + q4 + "%3C%2Fp%3E&Submit=+%C8%B7+%C8%CF+%B7%A2+%B2%BC+";

                                    textBox1.Text = method.PostUrl(posturl, postData, bCOOKIE, "gb2312");

                                    label4.Text = "正在发布....";
                                    label4.Text = "正在发布......";

                                    if (status == false)
                                    {
                                        return;
                                    }
                                }


                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                                if (this.listView1.Items.Count > 2)
                                {
                                    this.listView1.EnsureVisible(this.listView1.Items.Count - 1);
                                }

                            }


                        }

                    }
                   
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }










        public void run1()
        {
            aCOOKIE = textBox2.Text;
            bCOOKIE = textBox3.Text;
            int[] typeids = { 1, 2, 3, 4, 5, 6 };
            //int[] areaids = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };
            int[] areaids = { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };
            try
            {
                for (int i = 1; i < 1000; i = i++)
                {
                    foreach (int areaid in areaids)
                    {

                        foreach (var typeid in typeids)
                        {


                            string url = "http://fenglouge123.com/showclass.asp?page="+i+"&typeid="+typeid+"&areaid="+areaid;
                            string html = method.GetUrlWithCookie(url, aCOOKIE, "utf-8");

                           
                            MatchCollection ids = Regex.Matches(html, @"ShowInfo.asp\?id=([\s\S]*?)""");
                            ArrayList lists = new ArrayList();

                            foreach (Match id in ids)
                            {
                                lists.Add("http://fenglouge123.com/ShowInfo.asp?id=" + id.Groups[1].Value);
                            }

                            textBox1.Text = lists[0].ToString();
                           
                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;


                            foreach (string list in lists)

                            {

                                label4.Text = "发布成功！";
                                string strhtml = method.GetUrlWithCookie(list, aCOOKIE, "utf-8");
                               
                                Match a1 = Regex.Match(strhtml, @"信息标题</td>([\s\S]*?)</td>");
                                Match a2 = Regex.Match(strhtml, @"信息分类</td>([\s\S]*?)</td>");
                                Match a3 = Regex.Match(strhtml, @"所属地区</td>([\s\S]*?)areaid=([\s\S]*?)&typeid=([\s\S]*?)"">([\s\S]*?)</a>-<a href=""\/ShowClass\.asp\?areaid=([\s\S]*?)&typeid=([\s\S]*?)"">([\s\S]*?)</a></td>");
                                Match a4 = Regex.Match(strhtml, @"信息来源</td>([\s\S]*?)</td>");
                                Match a5 = Regex.Match(strhtml, @"小姐数量</td>([\s\S]*?)</td>");
                                Match a6 = Regex.Match(strhtml, @"小姐年龄</td>([\s\S]*?)</td>");
                                Match a7 = Regex.Match(strhtml, @"小姐素质</td>([\s\S]*?)</td>");
                                Match a8 = Regex.Match(strhtml, @"小姐外形</td>([\s\S]*?)</td>");
                                Match a9 = Regex.Match(strhtml, @"服务项目</td>([\s\S]*?)</td>");
                                Match a10 = Regex.Match(strhtml, @"价格一览</td>([\s\S]*?)</td>");
                                Match a11 = Regex.Match(strhtml, @"营业时间</td>([\s\S]*?)</td>");
                                Match a12 = Regex.Match(strhtml, @"环境设备</td>([\s\S]*?)</td>");
                                Match a13 = Regex.Match(strhtml, @"安全评估</td>([\s\S]*?)</td>");
                                Match a14 = Regex.Match(strhtml, @"详细地址</td>([\s\S]*?)</td>");
                                Match a15 = Regex.Match(strhtml, @"tel=([\s\S]*?)""");
                                Match a16 = Regex.Match(strhtml, @"综合评价</td>([\s\S]*?)</td>");
                                Match a17 = Regex.Match(strhtml, @"详细介绍：([\s\S]*?)<!--投诉-->");
                                MatchCollection a18 = Regex.Matches(strhtml, @"\d{12,}");  //图片地址

                                StringBuilder sb = new StringBuilder();

                                for (int s = 0; s < a18.Count; s++)
                                {
                                    sb.Append("<img src=\"http://fenglouge123.com/allphoto/upload/" + a18[s].Groups[0].Value + ".jpg \" />");
                                }

                                textBox1.Text = list;
                               

                                if (a1.Groups[1].Value != "" && a3.Groups[1].Value != "")
                                {
                                    ListViewItem lv1 = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());

                                    string title = HttpUtility.UrlEncode(CleanHtml(a1.Groups[1].Value),Encoding.GetEncoding("gb2312"));
                                    string laiyuan = HttpUtility.UrlEncode(CleanHtml(a4.Groups[1].Value), Encoding.GetEncoding("gb2312"));
                                    string shuliang = HttpUtility.UrlEncode(CleanHtml(a5.Groups[1].Value),Encoding.GetEncoding("gb2312"));
                                    string nianling = HttpUtility.UrlEncode(CleanHtml(a6.Groups[1].Value), Encoding.GetEncoding("gb2312"));
                                    string suzhi = HttpUtility.UrlEncode(CleanHtml(a7.Groups[1].Value), Encoding.GetEncoding("gb2312"));
                                    string waixing = HttpUtility.UrlEncode(CleanHtml(a8.Groups[1].Value),Encoding.GetEncoding("gb2312"));
                                    string xiangmu = HttpUtility.UrlEncode(CleanHtml(a9.Groups[1].Value), Encoding.GetEncoding("gb2312"));
                                    string jiage = HttpUtility.UrlEncode(CleanHtml(a10.Groups[1].Value), Encoding.GetEncoding("gb2312"));
                                    string yysj = HttpUtility.UrlEncode(CleanHtml(a11.Groups[1].Value), Encoding.GetEncoding("gb2312"));
                                    string hjsb = HttpUtility.UrlEncode(CleanHtml(a12.Groups[1].Value), Encoding.GetEncoding("gb2312"));
                                    string aqpg = HttpUtility.UrlEncode(CleanHtml(a13.Groups[1].Value), Encoding.GetEncoding("gb2312"));
                                    string adds = HttpUtility.UrlEncode(CleanHtml(a14.Groups[1].Value), Encoding.GetEncoding("gb2312"));
                                    string lxfs = "<img src=\"http:////fenglouge123.com//img.aspx?tel="+a15.Groups[1].Value.Trim()+">";
                                    string zhpj = HttpUtility.UrlEncode(CleanHtml(a16.Groups[1].Value),Encoding.GetEncoding("gb2312"));
                                    string xxjs = CleanHtml(a17.Groups[1].Value);

                                    label3.Text = DateTime.Now.ToString();
                                    label4.Text = "正在采集.";
                                    label4.Text = "正在采集..";
                                   
                                    lv1.SubItems.Add(CleanHtml(a1.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a3.Groups[3].Value));
                                    lv1.SubItems.Add(CleanHtml(a3.Groups[2].Value));
                                    lv1.SubItems.Add(CleanHtml(a4.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a5.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a6.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a7.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a8.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a9.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a10.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a11.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a12.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a13.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a14.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a15.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a16.Groups[1].Value));
                                    lv1.SubItems.Add(CleanHtml(a17.Groups[1].Value));


                                    string ftype = CleanHtml(a2.Groups[1].Value).Trim() + "|" + CleanHtml(a3.Groups[3].Value).Trim();
                                    string shengfen = a3.Groups[4].Value.Trim() + "|" + a3.Groups[2].Value.Trim();
                                   
                                    string diqu = a3.Groups[7].Value + "|" + a3.Groups[5].Value;
                                    string content = xxjs + sb.ToString() + "<p>管理约姐姐网【yuejj.net】提示:要求付定金 保证金的,总之提前要钱的都是骗子。</p><p>【必赢棋牌】真金游戏，真钱真刺激，不靠运气靠技术，技术好的一次赢几千，支持微信、支付宝、银行卡等多种方式上下分，想玩就玩，想走就走，安全可靠，注册就送18，可提现，复制到浏览器下载：http://259955.com</p>" + "<a href=\"http://lol.wutgb6y.com:8899/?GameID=1&channelCode=1278\"<img src=\"http://yuejj.net/111.png\" /></a>" + "<p>【需要更新城市资源以及聊天的请添加3万人聊天技术讨论群，随便聊不踢人（乱发广告的除外），点击立刻加入</p><p>https://pt.im/joinchat/RmS8my3KbP_594rgWGFGEw</p><p>potato频道：https://pt.im/yuejj</p>";
                                    
                                    
                                    string q1 = HttpUtility.UrlEncode(ftype, Encoding.GetEncoding("gb2312"));
                                    string q2 = HttpUtility.UrlEncode(shengfen, Encoding.GetEncoding("gb2312"));
                                    string q3 = HttpUtility.UrlEncode(diqu, Encoding.GetEncoding("gb2312"));
                                    string q4 = HttpUtility.UrlEncode(content, Encoding.GetEncoding("gb2312"));
                                    string posturl = "http://yuejj.net/users/addinfo.asp?action=add";

                                    string postData = "ftype=" + q1 + "&shengfen=" + q2 + "&diqu=" + q3 + "&title=" + title + "&adds=" + adds + "&laiyuan=" + laiyuan + "&shuliang=" + shuliang + "&nianling=" + nianling + "&suzhi=" + suzhi + "&waixing=" + waixing + "&xiangmu=" + xiangmu + "&jiage=" + jiage + "&yysj=" + yysj + "&hjsb=" + hjsb + "&aqpg=" + aqpg + "&lxfs=" + lxfs + "&zhpj=" + zhpj + "&content=%3Cp%3E%26nbsp%3B" + q4 + "%3C%2Fp%3E&Submit=+%C8%B7+%C8%CF+%B7%A2+%B2%BC+";

                                    textBox1.Text = postData;
                                   
                                    textBox1.Text = method.PostUrl(posturl, postData, bCOOKIE, "gb2312");
                                  
                                    label4.Text = "正在发布....";
                                    label4.Text = "正在发布......";

                                    if (status == false)
                                    {
                                        return;
                                    }
                                }


                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                                if (this.listView1.Items.Count > 2)
                                {
                                    this.listView1.EnsureVisible(this.listView1.Items.Count - 1);
                                }

                            }


                        }

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 网站发布项目_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            status = true;
            label3.Text = "程序启动中...网站反应慢请稍等";
            label4.Text = "网站反应慢请稍等...";
            Thread thread = new Thread(new ThreadStart(run1));
            thread.Start();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            zanting = false;
            label3.Text = "暂停中...";
            label4.Text = "等待开始...";
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = true;
            label3.Text = "已开始...";
            label4.Text = "继续采集...";
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            label3.Text = "已停止...";
            label4.Text = "等待开始...";
            status = false;
        }
    }
}
