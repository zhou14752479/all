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

namespace main._2019_5
{
    public partial class 网站发布项目 : Form
    {

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
        public static string COOKIE = "_ga=GA1.2.1344847034.1556672582; yunsuo_session_verify=23617dad39cdcf7aa32d1892602a6e11; srcurl=687474703a2f2f6d697373303330342e636f6d2f; security_session_mid_verify=27f97827335f437ebe4b86ea5023cf00; ASPSESSIONIDCABTQDBB=EANBDLLDLHBNIBEAAKFHHDME; _gid=GA1.2.350724619.1557106175; _gat=1; NewAspUsers=LastTime=2019%2F5%2F6+9%3A35%3A15&LastTimeDate=2019%2F5%2F1+11%3A36%3A46&LastTimeIP=183%2E135%2E206%2E214&userid=228062&usercookies=0&nickname=sunny369&username=sunny369&UserClass=0&password=12cb74d5e2c21fb6&UserGrade=3&UserLogin=60&usermail=my%40email%2Ecom&UserGroup=VIP+%BB%E1%D4%B1&userlastip=49%2E70%2E16%2E238&UserToday=0%2C0%2C0%2C0%2C0%2C0&RegDateTime=2019%2D03%2D04+16%3A31%3A36; usercookies%5F228062=dayarticlenum=0&daysoftnum=0&userip=49%2E70%2E16%2E238; __tins__18803298=%7B%22sid%22%3A%201557106523680%2C%20%22vd%22%3A%201%2C%20%22expires%22%3A%201557108323680%7D; __51cke__=; __51laig__=1";
        public void run()
        {
            string typeid = "4";
            string areaid = "2";

            try
            {
                for (int i = 1; i < 1000; i = i ++)
                {

                    string url = "http://miss0304.com/class.asp?page="+i+"&typeid="+typeid+"&areaid="+areaid;
                    string html = method.GetUrlWithCookie(url,COOKIE);

                    MatchCollection ids = Regex.Matches(html, @"show.asp\?id=([\s\S]*?)""");
                    ArrayList lists = new ArrayList();

                    foreach (Match id in ids)
                    {
                        lists.Add("http://miss0304.com/show.asp?id=" + id.Groups[1].Value);
                    }
                    

                    if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;


                    foreach (string list in lists)

                    {
                        string strhtml = method.GetUrlWithCookie(list, COOKIE);
                        
                        Match a1 = Regex.Match(strhtml, @"信息标题：([\s\S]*?)</td>");
                        Match a2 = Regex.Match(strhtml, @"信息分类：([\s\S]*?)</td>");
                        Match a3 = Regex.Match(strhtml, @"所属地区：([\s\S]*?)</td>");
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
                        if (a1.Groups[1].Value != "")
                        {
                            ListViewItem lv1 = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void 网站发布项目_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
        }
    }
}
