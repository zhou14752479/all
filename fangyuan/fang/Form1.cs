using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
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

namespace fang
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url,string charset)
        {
            try
            {
                string COOKIE = "id58=c5/nn1qC89pTY5VqGmdcAg==; 58tj_uuid=c3d04c48-dd4d-49b3-816c-4633755f76cb; als=0; xxzl_deviceid=H3uwjdpchUrszAFFwEXWI5UaNJCSrPvfwBqXEZzrZl2NJTBICdPpn5Oz382zj99d; wmda_uuid=70c7fd7fd7b308b2e6a6206def584118; wmda_new_uuid=1; gr_user_id=7ec08bb8-eca4-45e4-9423-142f24f58994; Hm_lvt_d32bebe8de17afd6738ef3ad3ffa4be3=1518609564; UM_distinctid=161d0fb312c473-0f0b11be7fbcac-3b60490d-1fa400-161d0fb312d34b; wmda_visited_projects=%3B1731916484865%3B1409632296065%3B1732038237441%3B1732039838209%3B2385390625025; __utma=253535702.1777428119.1523013867.1523013867.1523013867.1; __utmz=253535702.1523013867.1.1.utmcsr=sh.58.com|utmccn=(referral)|utmcmd=referral|utmcct=/shangpuqg/0/; _ga=GA1.2.1777428119.1523013867; Hm_lvt_3bb04d7a4ca3846dcc66a99c3e861511=1526113480,1528285922; Hm_lvt_e15962162366a86a6229038443847be7=1526113481,1528285922; Hm_lvt_e2d6b2d0ec536275bb1e37b421085803=1526113565,1528285939; Hm_lvt_4d4cdf6bc3c5cb0d6306c928369fe42f=1530434006; final_history=33904487894826%2C33904487533008%2C34202955671375%2C34457504313538%2C34457504172715; mcity=sh; mcityName=%E4%B8%8A%E6%B5%B7; nearCity=%5B%7B%22cityName%22%3A%22%E5%8D%97%E4%BA%AC%22%2C%22city%22%3A%22nj%22%7D%2C%7B%22cityName%22%3A%22%E5%AE%BF%E8%BF%81%22%2C%22city%22%3A%22suqian%22%7D%2C%7B%22cityName%22%3A%22%E4%B8%8A%E6%B5%B7%22%2C%22city%22%3A%22sh%22%7D%5D; cookieuid1=mgjwFVtJtsaIVFa/CEaJAg==; Hm_lvt_5a7a7bfd6e7dfd9438b9023d5a6a4a96=1531557573; city=cq; 58home=cq; new_uv=41; utm_source=; spm=; init_refer=; f=n; new_session=0; qz_gdt=; _house_detail_show_time_=2; ppStore_fingerprint=FCCDFD5888AF9A96AB5621ECA8D12A45C2DA9181ABE0A5F0%EF%BC%BF1532790344974";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";

                request.Headers.Add("Cookie", COOKIE);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

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
        #endregion

        #region  58个人二手房
        public void ershoufang()
        {

            try
            {
                string city = "zoucheng";


                    for (int i = 1; i < 71; i++)
                    {
                        String Url = "http://"+city+".58.com/ershoufang/0/pn" + i + "/";

                        string html = GetUrl(Url,"utf-8");


                        MatchCollection TitleMatchs = Regex.Matches(html, @"<li logr=""([\s\S]*?)_0_([\s\S]*?)_([\s\S]*?)_", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        ArrayList lists = new ArrayList();
                        foreach (Match NextMatch in TitleMatchs)
                        {
                            lists.Add("http://" + city + ".58.com/ershoufang/" + NextMatch.Groups[3].Value + "x.shtml");

                        }

                        if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;


                        foreach (string list in lists)

                        {

                        textBox1.Text += DateTime.Now.ToString() + list + "\r\n";
                        if (button2.Text == "已停止")
                            return;

                        string Url2 = "http://m.58.com/" + city + "/ershoufang/" + list.Substring(list.Length - 21);                       //获取二手房手机端的网址

                        string strhtml = GetUrl(list,"utf-8");                                                                                //定义的GetRul方法 返回 reader.ReadToEnd()

                        string strhtml2 = GetUrl(Url2, "utf-8");                                                                               //请求手机端网址

                        string title = @"<h1 class=""c_333 f20"">([\s\S]*?)</h1>";  //标题
                        string Rxg = @"<h2 class=""agent-title"">([\s\S]*?)</h2>";  //手机端正则匹配联系人
                        string Rxg1 = @"<p class='phone-num'>([\s\S]*?)<";   //电话
                        string Rxg3 = @"小区：([\s\S]*?)</h2>";//手机端小区
                        string Rxg4 = @"面积</p>([\s\S]*?)</p>"; //手机端面积去除标签
                        string Rxg5 = @"MinPrice':'([\s\S]*?)'"; 
                       
                        string Rxg7 = @"楼层：<span class=""detail-value"">([\s\S]*?)</span>";
                        string Rxg8 = @"朝向：<span class=""detail-value"">([\s\S]*?)</span>";
                        string Rxg9 = @"类型：<span class=""detail-value"">([\s\S]*?)</span>";
                        string Rxg10 = @"装修：<span class=""detail-value"">([\s\S]*?)</span>";
                        string Rxg11 = @"产权：([\s\S]*?)</span>";
                        string Rxg12 = @"元（([\s\S]*?)）"; //单价
                        string Rxg13 = @"发布：<span class=""detail-value"">([\s\S]*?)</span>";
                        string Rxg14 = @"<h3>核心卖点</h3>([\s\S]*?)</p>";

                        string Rxg15 = @"户型</p>([\s\S]*?)室([\s\S]*?)厅([\s\S]*?)卫"; //手机端户型去除标签 
                        

                        Match titles = Regex.Match(strhtml, title);
                            Match contacts = Regex.Match(strhtml2, Rxg);                                                        //手机端正则匹配联系人
                            Match tell = Regex.Match(strhtml, Rxg1);
                            Match xiaoqu = Regex.Match(strhtml2, Rxg3);
                            Match mianji = Regex.Match(strhtml2, Rxg4);
                            Match price = Regex.Match(strhtml, Rxg5);

                       
                        Match louceng = Regex.Match(strhtml2, Rxg7);
                        Match chaoxiang= Regex.Match(strhtml2, Rxg8);
                        Match leiixng = Regex.Match(strhtml2, Rxg9);
                        Match zhuangxiu = Regex.Match(strhtml2, Rxg10);
                        Match chanquan = Regex.Match(strhtml2, Rxg11);
                        Match danjia = Regex.Match(strhtml, Rxg12);
                        Match fabu = Regex.Match(strhtml2, Rxg13);
                        Match xiangqing = Regex.Match(strhtml2, Rxg14);

                        Match huxing = Regex.Match(strhtml2, Rxg15);

                       





                        ListViewItem lv1 = listView1.Items.Add(titles.Groups[1].Value.Trim()); //使用Listview展示数据
                        lv1.SubItems.Add(contacts.Groups[1].Value);
                        lv1.SubItems.Add(tell.Groups[1].Value);
                        lv1.SubItems.Add(xiaoqu.Groups[1].Value);
                        string temp = Regex.Replace(mianji.Groups[1].Value, "<[^>]*>", "");
                        lv1.SubItems.Add(temp.Trim());
                        string temp1 = Regex.Replace(price.Groups[1].Value+"万", "<[^>]*>", "");
                        lv1.SubItems.Add(temp1.Trim());

                        
                        
                        lv1.SubItems.Add(louceng.Groups[1].Value);
                        lv1.SubItems.Add(chaoxiang.Groups[1].Value);
                        lv1.SubItems.Add(leiixng.Groups[1].Value);
                        lv1.SubItems.Add(zhuangxiu.Groups[1].Value);
                        string temp3 = Regex.Replace(chanquan.Groups[1].Value, "<[^>]*>", "");
                        lv1.SubItems.Add(temp3);
                        lv1.SubItems.Add(danjia.Groups[1].Value.Replace("</i>","").Trim());
                        lv1.SubItems.Add(fabu.Groups[1].Value);
                        lv1.SubItems.Add(xiangqing.Groups[1].Value.Replace("<p>","").Trim());
                        string temp4 = Regex.Replace(huxing.Groups[1].Value, "<[^>]*>", "");
                        string temp5 = Regex.Replace(huxing.Groups[2].Value, "<[^>]*>", "");
                        string temp6 = Regex.Replace(huxing.Groups[3].Value, "<[^>]*>", "");
                        lv1.SubItems.Add(temp4.Trim());
                        lv1.SubItems.Add(temp5.Trim());
                        lv1.SubItems.Add(temp6.Trim());
                        



                        Application.DoEvents();
                            System.Threading.Thread.Sleep(Convert.ToInt32(1000));   //内容获取间隔，可变量                     

                        }


                    }
                
            }


            catch (System.Exception ex)
            {

                ex.ToString();
            }

        }

        #endregion

        #region  58中介二手房
        public void ershoufang1()
        {

            try
            {
                string city = "zoucheng";


                for (int i = 1; i < 71; i++)
                {
                    String Url = "http://" + city + ".58.com/ershoufang/1/pn" + i + "/";

                    string html = GetUrl(Url, "utf-8");


                    MatchCollection TitleMatchs = Regex.Matches(html, @"<li logr=""([\s\S]*?)_2_([\s\S]*?)_([\s\S]*?)_", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    ArrayList lists = new ArrayList();
                    foreach (Match NextMatch in TitleMatchs)
                    {
                        lists.Add("http://" + city + ".58.com/ershoufang/" + NextMatch.Groups[3].Value + "x.shtml");

                    }

                    if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;


                    foreach (string list in lists)

                    {

                        textBox1.Text += DateTime.Now.ToString() + list + "\r\n";
                        if (button2.Text == "已停止")
                            return;

                        string Url2 = "http://m.58.com/" + city + "/ershoufang/" + list.Substring(list.Length - 21);                       //获取二手房手机端的网址

                        string strhtml = GetUrl(list, "utf-8");                                                                                //定义的GetRul方法 返回 reader.ReadToEnd()

                        string strhtml2 = GetUrl(Url2, "utf-8");                                                                               //请求手机端网址

                        string title = @"<h1 class=""c_333 f20"">([\s\S]*?)</h1>";  //标题
                        string Rxg = @"<h2 class=""agent-title"">([\s\S]*?)</h2>";  //手机端正则匹配联系人
                        string Rxg1 = @"<p class='phone-num'>([\s\S]*?)<";   //电话
                        string Rxg3 = @"小区：([\s\S]*?)</h2>";//手机端小区
                        string Rxg4 = @"面积</p>([\s\S]*?)</p>"; //手机端面积去除标签
                        string Rxg5 = @"MinPrice':'([\s\S]*?)'";

                        string Rxg7 = @"楼层：<span class=""detail-value"">([\s\S]*?)</span>";
                        string Rxg8 = @"朝向：<span class=""detail-value"">([\s\S]*?)</span>";
                        string Rxg9 = @"类型：<span class=""detail-value"">([\s\S]*?)</span>";
                        string Rxg10 = @"装修：<span class=""detail-value"">([\s\S]*?)</span>";
                        string Rxg11 = @"产权：([\s\S]*?)</span>";
                        string Rxg12 = @"元（([\s\S]*?)）"; //单价
                        string Rxg13 = @"发布：<span class=""detail-value"">([\s\S]*?)</span>";
                        string Rxg14 = @"<h3>核心卖点</h3>([\s\S]*?)</p>";

                        string Rxg15 = @"户型</p>([\s\S]*?)室([\s\S]*?)厅([\s\S]*?)卫"; //手机端户型去除标签 


                        Match titles = Regex.Match(strhtml, title);
                        Match contacts = Regex.Match(strhtml2, Rxg);                                                        //手机端正则匹配联系人
                        Match tell = Regex.Match(strhtml, Rxg1);
                        Match xiaoqu = Regex.Match(strhtml2, Rxg3);
                        Match mianji = Regex.Match(strhtml2, Rxg4);
                        Match price = Regex.Match(strhtml, Rxg5);


                        Match louceng = Regex.Match(strhtml2, Rxg7);
                        Match chaoxiang = Regex.Match(strhtml2, Rxg8);
                        Match leiixng = Regex.Match(strhtml2, Rxg9);
                        Match zhuangxiu = Regex.Match(strhtml2, Rxg10);
                        Match chanquan = Regex.Match(strhtml2, Rxg11);
                        Match danjia = Regex.Match(strhtml, Rxg12);
                        Match fabu = Regex.Match(strhtml2, Rxg13);
                        Match xiangqing = Regex.Match(strhtml2, Rxg14);

                        Match huxing = Regex.Match(strhtml2, Rxg15);
                        Match gongsi = Regex.Match(strhtml2, @"所属公司：([\s\S]*?)</p>");






                        ListViewItem lv1 = listView1.Items.Add(titles.Groups[1].Value.Trim()); //使用Listview展示数据
                        lv1.SubItems.Add(contacts.Groups[1].Value);
                        lv1.SubItems.Add(tell.Groups[1].Value);
                        lv1.SubItems.Add(xiaoqu.Groups[1].Value);
                        string temp = Regex.Replace(mianji.Groups[1].Value, "<[^>]*>", "");
                        lv1.SubItems.Add(temp.Trim());
                        string temp1 = Regex.Replace(price.Groups[1].Value + "万", "<[^>]*>", "");
                        lv1.SubItems.Add(temp1.Trim());



                        lv1.SubItems.Add(louceng.Groups[1].Value);
                        lv1.SubItems.Add(chaoxiang.Groups[1].Value);
                        lv1.SubItems.Add(leiixng.Groups[1].Value);
                        lv1.SubItems.Add(zhuangxiu.Groups[1].Value);
                        string temp3 = Regex.Replace(chanquan.Groups[1].Value, "<[^>]*>", "");
                        lv1.SubItems.Add(temp3);
                        lv1.SubItems.Add(danjia.Groups[1].Value.Replace("</i>", "").Trim());
                        lv1.SubItems.Add(fabu.Groups[1].Value);
                        lv1.SubItems.Add(xiangqing.Groups[1].Value.Replace("<p>", "").Trim());
                        string temp4 = Regex.Replace(huxing.Groups[1].Value, "<[^>]*>", "");
                        string temp5 = Regex.Replace(huxing.Groups[2].Value, "<[^>]*>", "");
                        string temp6 = Regex.Replace(huxing.Groups[3].Value, "<[^>]*>", "");
                        lv1.SubItems.Add(temp4.Trim());
                        lv1.SubItems.Add(temp5.Trim());
                        lv1.SubItems.Add(temp6.Trim());
                        lv1.SubItems.Add(gongsi.Groups[1].Value.Trim());


                        Application.DoEvents();
                        System.Threading.Thread.Sleep(Convert.ToInt32(1000));   //内容获取间隔，可变量                     

                    }


                }

            }


            catch (System.Exception ex)
            {

                ex.ToString();
            }

        }

        #endregion

        #region 邹城个人房产
        public void zoucheng()
        {

            try
            {
                


                for (int i = 1; i <40; i++)
                {
                    String Url = "http://www.zcfcw.cn/sale/search/airallpage_ly%E4%B8%AA%E4%BA%BA_qy_fx_jg_mj_hx_zx_lc_dd_page" + i+".html";

                    string html = GetUrl(Url,"utf-8");


                    MatchCollection TitleMatchs = Regex.Matches(html, @"<li class=fylifw0><a href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    ArrayList lists = new ArrayList();
                    foreach (Match NextMatch in TitleMatchs)
                    {
                        lists.Add("http://www.zcfcw.cn"+ NextMatch.Groups[1].Value);

                    }

                    if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;


                    foreach (string list in lists)

                    {
                        if (button2.Text == "已停止")
                            return;

                        textBox1.Text += list+"\r\n";
                        
                        string strhtml = GetUrl(list,"utf-8");                                                                                //定义的GetRul方法 返回 reader.ReadToEnd()

                        
                        string Rxg = @"联 系 人：</li><li class=li2_1>([\s\S]*?)&"; 
                        string Rxg1 = @"手机号码：</li><li class=li2_1>([\s\S]*?)&";   //电话
                        string Rxg2 = @"区域方位：([\s\S]*?)&";
                        string Rxg3 = @"小区地段：([\s\S]*?)&";//手机端小区
                        string Rxg4 = @"建筑面积：([\s\S]*?)</font>";
                        string Rxg5 = @"交易价格：([\s\S]*?)&"; 
                        string Rxg6 = @"户型结构：([\s\S]*?)室([\s\S]*?)厅([\s\S]*?)卫&"; 
                        string Rxg7 = @"楼层总数：([\s\S]*?)&";
                        string Rxg8 = @"房屋朝向：([\s\S]*?)&";
                        string Rxg9 = @"房屋类型：([\s\S]*?)&";
                        string Rxg10 = @"装修程度：([\s\S]*?)&";
                        string Rxg11 = @"契证年数：([\s\S]*?)&";
                        string Rxg12 = @"每平单价：([\s\S]*?)&"; //单价
                        string Rxg13 = @"更新时间：([\s\S]*?)&";
                        
                        string Rxg14 = @"备注说明：</li>([\s\S]*?)</li>";
                        string Rxg15 = @"浏览：<span>([\s\S]*?)</span>";
                        string Rxg16 = @"小学学区：([\s\S]*?)&";
                        string Rxg17 = @"中学学区：([\s\S]*?)&";
                        string Rxg18 = @"<a href=""http://ip.yimao.com/([\s\S]*?).html";
                        string Rxg19 = @"来源：([\s\S]*?)</li>";
                        string Rxg20= @"所在楼层：([\s\S]*?)&";
                        string Rxg21 = @"车库情况：([\s\S]*?)&";
                        string Rxg22 = @"建成年份：([\s\S]*?)&";
                        string Rxg23 = @"房源编号：([\s\S]*?)&";
                        string Rxg24 = @"土地性质：([\s\S]*?)&";
                        string Rxg25 = @"证件情况：([\s\S]*?)&";
                        string Rxg26 = @"配套设施：([\s\S]*?)&";
                        string Rxg27 = @"电话/短号：</li><li class=li2_1>([\s\S]*?)&";

                        Match contacts = Regex.Match(strhtml, Rxg);                                                      
                        Match tell = Regex.Match(strhtml, Rxg1);
                        Match quyu = Regex.Match(strhtml, Rxg2);
                        Match xiaoqu = Regex.Match(strhtml, Rxg3);
                        Match mianji = Regex.Match(strhtml, Rxg4);
                        Match price = Regex.Match(strhtml, Rxg5);

                        Match huxing = Regex.Match(strhtml, Rxg6);
                        Match louceng = Regex.Match(strhtml, Rxg7);
                        Match chaoxiang = Regex.Match(strhtml, Rxg8);
                        Match leiixng = Regex.Match(strhtml, Rxg9);
                        Match zhuangxiu = Regex.Match(strhtml, Rxg10);
                        Match chanquan = Regex.Match(strhtml, Rxg11);
                        Match danjia = Regex.Match(strhtml, Rxg12);
                        Match time = Regex.Match(strhtml, Rxg13);
                        Match beizhu = Regex.Match(strhtml, Rxg14);
                        Match liulan = Regex.Match(strhtml, Rxg15);
                        Match xiaoxue = Regex.Match(strhtml, Rxg16);
                        Match zhongxue = Regex.Match(strhtml, Rxg17);
                        Match ip = Regex.Match(strhtml, Rxg18);
                        Match laiyuan = Regex.Match(strhtml, Rxg19);
                        Match suozailc= Regex.Match(strhtml, Rxg20);
                        Match cheku = Regex.Match(strhtml, Rxg21);
                        Match nianfen = Regex.Match(strhtml, Rxg22);
                        Match bianhao = Regex.Match(strhtml, Rxg23);
                        Match tudi = Regex.Match(strhtml, Rxg24);
                        Match zhengjian = Regex.Match(strhtml, Rxg25);
                        Match sheshi = Regex.Match(strhtml, Rxg26);
                        Match dianhua = Regex.Match(strhtml, Rxg27);



                        string temp = Regex.Replace(ip.Groups[1].Value, "<[^>]*>", "");
                        string temp1 = Regex.Replace(bianhao.Groups[1].Value, "<[^>]*>", "");
                        string temp2 = Regex.Replace(mianji.Groups[1].Value, "<[^>]*>", "");
                        string temp3 = Regex.Replace(price.Groups[1].Value, "<[^>]*>", "");
                        string temp4 = Regex.Replace(beizhu.Groups[1].Value, "<[^>]*>", "");




                        ListViewItem lv2 = listView2.Items.Add(time.Groups[1].Value.Replace("</li><li class=li2>","")); //使用Listview展示数据

                        lv2.SubItems.Add(temp);
                        lv2.SubItems.Add(liulan.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(laiyuan.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(temp1);
                        lv2.SubItems.Add(quyu.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(xiaoqu.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(leiixng.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        
                        lv2.SubItems.Add(suozailc.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(zhuangxiu.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(louceng.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(temp2);
                        lv2.SubItems.Add(cheku.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(chanquan.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(xiaoxue.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(zhongxue.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(chaoxiang.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(tudi.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(nianfen.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(danjia.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(zhengjian.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(temp3);
                        lv2.SubItems.Add(sheshi.Groups[1].Value.Replace("</li><li class=li5>", ""));
                        lv2.SubItems.Add(temp4);
                        lv2.SubItems.Add(contacts.Groups[1].Value.Replace("<li class=li2>", ""));
                        lv2.SubItems.Add(tell.Groups[1].Value.Replace("</li><li class=li2><b>", ""));
                        lv2.SubItems.Add(dianhua.Groups[1].Value.Replace("<li class=li2>", ""));

                        lv2.SubItems.Add(huxing.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(huxing.Groups[2].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(huxing.Groups[3].Value.Replace("</li><li class=li2>", ""));


                        Application.DoEvents();
                        System.Threading.Thread.Sleep(Convert.ToInt32(500));   //内容获取间隔，可变量                     

                    }


                }

            }


            catch (System.Exception ex)
            {

                ex.ToString();
            }

        }

        #endregion

        #region 邹城中介房产
        public void zoucheng1()
        {

            try
            {



                for (int i = 1; i < 40; i++)
                {
                    String Url = "http://www.zcfcw.cn/sale/search/airallpage_ly%e4%b8%ad%e4%bb%8b_qy_fx_jg_mj_hx_zx_lc_dd_page" + i + ".html";

                    string html = GetUrl(Url, "utf-8");


                    MatchCollection TitleMatchs = Regex.Matches(html, @"<li class=fylifw0><a href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    ArrayList lists = new ArrayList();
                    foreach (Match NextMatch in TitleMatchs)
                    {
                        lists.Add("http://www.zcfcw.cn" + NextMatch.Groups[1].Value);

                    }

                    if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;


                    foreach (string list in lists)

                    {
                        if (button2.Text == "已停止")
                            return;

                        textBox1.Text += list + "\r\n";

                        string strhtml = GetUrl(list, "utf-8");                                                                                //定义的GetRul方法 返回 reader.ReadToEnd()


                        string Rxg = @"联 系 人：</li><li class=li2_1>([\s\S]*?)&";
                        string Rxg1 = @"手机号码：</li><li class=li2_1>([\s\S]*?)&";   //电话
                        string Rxg2 = @"区域方位：([\s\S]*?)&";
                        string Rxg3 = @"小区地段：([\s\S]*?)&";//手机端小区
                        string Rxg4 = @"建筑面积：([\s\S]*?)</font>";
                        string Rxg5 = @"交易价格：([\s\S]*?)&";
                        string Rxg6 = @"户型结构：([\s\S]*?)室([\s\S]*?)厅([\s\S]*?)卫&";
                        string Rxg7 = @"楼层总数：([\s\S]*?)&";
                        string Rxg8 = @"房屋朝向：([\s\S]*?)&";
                        string Rxg9 = @"房屋类型：([\s\S]*?)&";
                        string Rxg10 = @"装修程度：([\s\S]*?)&";
                        string Rxg11 = @"契证年数：([\s\S]*?)&";
                        string Rxg12 = @"每平单价：([\s\S]*?)&"; //单价
                        string Rxg13 = @"更新时间：([\s\S]*?)&";

                        string Rxg14 = @"备注说明：</li>([\s\S]*?)</li>";
                        string Rxg15 = @"浏览：<span>([\s\S]*?)</span>";
                        string Rxg16 = @"小学学区：([\s\S]*?)&";
                        string Rxg17 = @"中学学区：([\s\S]*?)&";
                        string Rxg18 = @"<a href=""http://ip.yimao.com/([\s\S]*?).html";
                        string Rxg19 = @"来源：([\s\S]*?)</li>";
                        string Rxg20 = @"所在楼层：([\s\S]*?)&";
                        string Rxg21 = @"车库情况：([\s\S]*?)&";
                        string Rxg22 = @"建成年份：([\s\S]*?)&";
                        string Rxg23 = @"房源编号：([\s\S]*?)&";
                        string Rxg24 = @"土地性质：([\s\S]*?)&";
                        string Rxg25 = @"证件情况：([\s\S]*?)&";
                        string Rxg26 = @"配套设施：([\s\S]*?)&";
                        string Rxg27 = @"电话/短号：</li><li class=li2_1>([\s\S]*?)&";

                        Match contacts = Regex.Match(strhtml, Rxg);
                        Match tell = Regex.Match(strhtml, Rxg1);
                        Match quyu = Regex.Match(strhtml, Rxg2);
                        Match xiaoqu = Regex.Match(strhtml, Rxg3);
                        Match mianji = Regex.Match(strhtml, Rxg4);
                        Match price = Regex.Match(strhtml, Rxg5);

                        Match huxing = Regex.Match(strhtml, Rxg6);
                        Match louceng = Regex.Match(strhtml, Rxg7);
                        Match chaoxiang = Regex.Match(strhtml, Rxg8);
                        Match leiixng = Regex.Match(strhtml, Rxg9);
                        Match zhuangxiu = Regex.Match(strhtml, Rxg10);
                        Match chanquan = Regex.Match(strhtml, Rxg11);
                        Match danjia = Regex.Match(strhtml, Rxg12);
                        Match time = Regex.Match(strhtml, Rxg13);
                        Match beizhu = Regex.Match(strhtml, Rxg14);
                        Match liulan = Regex.Match(strhtml, Rxg15);
                        Match xiaoxue = Regex.Match(strhtml, Rxg16);
                        Match zhongxue = Regex.Match(strhtml, Rxg17);
                        Match ip = Regex.Match(strhtml, Rxg18);
                        Match laiyuan = Regex.Match(strhtml, Rxg19);
                        Match suozailc = Regex.Match(strhtml, Rxg20);
                        Match cheku = Regex.Match(strhtml, Rxg21);
                        Match nianfen = Regex.Match(strhtml, Rxg22);
                        Match bianhao = Regex.Match(strhtml, Rxg23);
                        Match tudi = Regex.Match(strhtml, Rxg24);
                        Match zhengjian = Regex.Match(strhtml, Rxg25);
                        Match sheshi = Regex.Match(strhtml, Rxg26);
                        Match dianhua = Regex.Match(strhtml, Rxg27);
                        Match qiye= Regex.Match(strhtml, @"企业名称：([\s\S]*?)</font>");
                        Match dizhi = Regex.Match(strhtml, @"企业地址：</li>([\s\S]*?)</li>");



                        string temp = Regex.Replace(ip.Groups[1].Value, "<[^>]*>", "");
                        string temp1 = Regex.Replace(bianhao.Groups[1].Value, "<[^>]*>", "");
                        string temp2 = Regex.Replace(mianji.Groups[1].Value, "<[^>]*>", "");
                        string temp3 = Regex.Replace(price.Groups[1].Value, "<[^>]*>", "");
                        string temp4 = Regex.Replace(beizhu.Groups[1].Value, "<[^>]*>", "");




                        ListViewItem lv2 = listView2.Items.Add(time.Groups[1].Value.Replace("</li><li class=li2>", "")); //使用Listview展示数据

                        lv2.SubItems.Add(temp);
                        lv2.SubItems.Add(liulan.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(laiyuan.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(temp1);
                        lv2.SubItems.Add(quyu.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(xiaoqu.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(leiixng.Groups[1].Value.Replace("</li><li class=li2>", ""));

                        lv2.SubItems.Add(suozailc.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(zhuangxiu.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(louceng.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(temp2);
                        lv2.SubItems.Add(cheku.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(chanquan.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(xiaoxue.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(zhongxue.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(chaoxiang.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(tudi.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(nianfen.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(danjia.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(zhengjian.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(temp3);
                        lv2.SubItems.Add(sheshi.Groups[1].Value.Replace("</li><li class=li5>", ""));
                        lv2.SubItems.Add(temp4);
                        lv2.SubItems.Add(contacts.Groups[1].Value.Replace("<li class=li2>", ""));
                        lv2.SubItems.Add(tell.Groups[1].Value.Replace("</li><li class=li2><b>", ""));
                        lv2.SubItems.Add(dianhua.Groups[1].Value.Replace("<li class=li2>", ""));

                        lv2.SubItems.Add(huxing.Groups[1].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(huxing.Groups[2].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(huxing.Groups[3].Value.Replace("</li><li class=li2>", ""));
                        lv2.SubItems.Add(Regex.Replace(qiye.Groups[1].Value, "<[^>]*>", ""));
                        lv2.SubItems.Add(Regex.Replace(dizhi.Groups[1].Value, "<[^>]*>", ""));


                        
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(Convert.ToInt32(500));   //内容获取间隔，可变量                     

                    }


                }

            }


            catch (System.Exception ex)
            {

                ex.ToString();
            }

        }

        #endregion



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button2.Text = "停止采集";
            listView1.Items.Clear();
            listView2.Items.Clear();

            if (radioButton1.Checked == true)
            {
                listView1.Visible = false;
                listView2.Visible = true;
                Thread thread = new Thread(new ThreadStart(zoucheng));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();
            }

            else if (radioButton2.Checked == true)
            {
                listView2.Visible = false;
                listView1.Visible = true;
                Thread thread = new Thread(new ThreadStart(ershoufang));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();

            }
            else if (radioButton3.Checked == true)
            {
                listView1.Visible = false;
                listView2.Visible = true;
                Thread thread = new Thread(new ThreadStart(zoucheng1));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();

            }
            else if (radioButton4.Checked == true)
            {
                listView2.Visible = false;
                listView1.Visible = true;
                Thread thread = new Thread(new ThreadStart(ershoufang1));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();

            }
            else
            {
                MessageBox.Show("请选择需要查找的网站！");
            }

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Text = "已停止";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            listView2.Items.Clear();
        }


        
        

        private void button4_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
            }
            if (radioButton2.Checked == true)
            {
                method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
            }



        }
    }
}
