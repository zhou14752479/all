using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 淘宝访客
{
    public partial class 淘宝访客 : Form
    {
        public 淘宝访客()
        {
            InitializeComponent();
        }
        

        private void button2_Click(object sender, EventArgs e)
        {
            登录 login=new 登录(); 
            login.ShowDialog();
        }
        public static string Md5_utf8(string str)
        {
            //将输入字符串转换成字节数组
            var buffer = Encoding.GetEncoding("utf-8").GetBytes(str);
            //接着，创建Md5对象进行散列计算
            var data = MD5.Create().ComputeHash(buffer);

            //创建一个新的Stringbuilder收集字节
            var sb = new StringBuilder();

            //遍历每个字节的散列数据 
            foreach (var t in data)
            {
                //格式每一个十六进制字符串
                sb.Append(t.ToString("X2"));
            }

            //返回十六进制字符串
            return sb.ToString().ToLower();
        }
        Thread thread;

        string token = "";
        string shopname = "";
        string shopurl = "";
        public void getshopall()
        {

            string reviewcookie = 登录.cookie;
            token = Regex.Match(登录.cookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;
           
            string time = method.GetTimeStamp();
            string str = token + "&" + time + "&12574478&{\"input\":\"{\\\"accessCode\\\":\\\"tbopenshop_personal\\\",\\\"client\\\":\\\"pc\\\",\\\"qnshopdomain\\\":\\\"https://myseller.taobao.com\\\"}\",\"serviceName\":\"positionRenderService\"}";
            string sign = Md5_utf8(str);

            string aurl = "https://h5api.m.taobao.com/h5/mtop.alibaba.shop.tbopenshop.gateway/1.0/?jsv=2.6.1&appKey=12574478&t="+time+"&sign="+sign+"&api=mtop.alibaba.shop.tbopenshop.gateway&v=1.0&ttid=11320%40taobao_WEB_9.9.99&dataType=json&type=originaljson";
            string postdata = "data=%7B%22input%22%3A%22%7B%5C%22accessCode%5C%22%3A%5C%22tbopenshop_personal%5C%22%2C%5C%22client%5C%22%3A%5C%22pc%5C%22%2C%5C%22qnshopdomain%5C%22%3A%5C%22https%3A%2F%2Fmyseller.taobao.com%5C%22%7D%22%2C%22serviceName%22%3A%22positionRenderService%22%7D";
            string html = method.PostUrlDefault(aurl,postdata, reviewcookie);

           
            if (html.Contains("令牌过期"))
            {
                string cookiestr = method.getSetCookie(aurl);
                string _m_h5_tk = "_m_h5_tk=" + Regex.Match(cookiestr, @"_m_h5_tk=([\s\S]*?);").Groups[1].Value;
                string _m_h5_tk_enc = "_m_h5_tk_enc=" + Regex.Match(cookiestr, @"_m_h5_tk_enc=([\s\S]*?);").Groups[1].Value;
                reviewcookie = _m_h5_tk + ";" + _m_h5_tk_enc + ";";

                token = Regex.Match(登录.cookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;
               time = method.GetTimeStamp();
                 str = token + "&" + time + "&12574478&{\"input\":\"{\\\"accessCode\\\":\\\"tbopenshop_personal\\\",\\\"client\\\":\\\"pc\\\",\\\"qnshopdomain\\\":\\\"https://myseller.taobao.com\\\"}\",\"serviceName\":\"positionRenderService\"}";
                sign = Md5_utf8(str);

                aurl = "https://h5api.m.taobao.com/h5/mtop.alibaba.shop.tbopenshop.gateway/1.0/?jsv=2.6.1&appKey=12574478&t=" + time + "&sign=" + sign + "&api=mtop.alibaba.shop.tbopenshop.gateway&v=1.0&ttid=11320%40taobao_WEB_9.9.99&dataType=json&type=originaljson";
                postdata = "data=%7B%22input%22%3A%22%7B%5C%22accessCode%5C%22%3A%5C%22tbopenshop_personal%5C%22%2C%5C%22client%5C%22%3A%5C%22pc%5C%22%2C%5C%22qnshopdomain%5C%22%3A%5C%22https%3A%2F%2Fmyseller.taobao.com%5C%22%7D%22%2C%22serviceName%22%3A%22positionRenderService%22%7D";
                html =method.PostUrlDefault(aurl, postdata, reviewcookie);
            }
            //MessageBox.Show(html);
            shopname = Regex.Match(html, @"""shopName"":""([\s\S]*?)""").Groups[1].Value;
           // shopurl = Regex.Match(html, @"""loginId"":""([\s\S]*?)""").Groups[1].Value;


        }


        public void getshop()
        {

            string reviewcookie = 登录.cookie;
            token = Regex.Match(登录.cookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;

            string time = method.GetTimeStamp();
            string str = token + "&" + time + "&12574478&{}";
            string sign = Md5_utf8(str);

            string aurl = "https://h5api.m.taobao.com/h5/mtop.taobao.jdy.resource.shop.info.get/1.0/?jsv=2.6.1&appKey=12574478&t="+time+"&sign="+sign+"&ttid=11320%40taobao_WEB_9.9.99&v=1.0&type=originaljsonp&timeout=20000&dataType=originaljsonp&api=mtop.taobao.jdy.resource.shop.info.get&callback=mtopjsonp2&data=%7B%7D";
           
            string html = method.GetUrlWithCookie(aurl, reviewcookie,"utf-8");

           
            if (html.Contains("令牌过期"))
            {
                string cookiestr = method.getSetCookie(aurl);
              
                string _m_h5_tk = "_m_h5_tk=" + Regex.Match(cookiestr, @"_m_h5_tk=([\s\S]*?);").Groups[1].Value;
                string _m_h5_tk_enc = "_m_h5_tk_enc=" + Regex.Match(cookiestr, @"_m_h5_tk_enc=([\s\S]*?);").Groups[1].Value;
                reviewcookie = _m_h5_tk + ";" + _m_h5_tk_enc + ";";

                token = Regex.Match(登录.cookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;
                time = method.GetTimeStamp();
                str = token + "&" + time + "&12574478&{}";
                sign = Md5_utf8(str);

                aurl = "https://h5api.m.taobao.com/h5/mtop.taobao.jdy.resource.shop.info.get/1.0/?jsv=2.6.1&appKey=12574478&t=" + time + "&sign=" + sign + "&ttid=11320%40taobao_WEB_9.9.99&v=1.0&type=originaljsonp&timeout=20000&dataType=originaljsonp&api=mtop.taobao.jdy.resource.shop.info.get&callback=mtopjsonp2&data=%7B%7D";

                html = method.GetUrlWithCookie(aurl, reviewcookie, "utf-8");
            }
            
            shopname = Regex.Match(html, @"""shopName"":""([\s\S]*?)""").Groups[1].Value;
            shopurl = Regex.Match(html, @"""shopDomainUrl"":""([\s\S]*?)""").Groups[1].Value;

            
        }


        #region 主程序
        public void run()
        {
            
            //登录.cookie = "t=854f718885cef3201bc094b3149ced04; thw=cn; enc=AYBtf5zHavWFAAAAAHnitl0Bdv39%2FSn9%2FUAm%2Ff1J%2FVEBc0Lm9PK2XHjDcs1vMHOUaS5jaanHt2pAE3PIafFe6lW3; cna=me/qGmJS/VYCAXniuY+bEuiw; lgc=zkg852266010; tracknick=zkg852266010; _m_h5_tk=4a285df6646d551d2a6ee0e7e088f94f_1651549567632; _m_h5_tk_enc=3b6a3def8f29682ca046499b0b7b1f90; _tb_token_=eef7f40e39343; xlly_s=1; _samesite_flag_=true; cookie2=2efb8b6b464fb5c4b660d79cacb5f05a; sgcookie=E100vmgNjkxIWgGQWlLwMzEzV7rWnB6cHG2DRi%2Fez2eCBjbUacRjwNgoDi%2FwY7mkXrReWI71%2ByKcVz%2BLs8GesWolavBjjXU0C8Y4CViZxG%2BMPlbrLZmBNB0CKziYNOr48d7%2F; unb=1052347548; uc3=nk2=GcOvCmiKUSBXqZNU&vt3=F8dCvC6CP6%2F6R1ZuFd8%3D&lg2=VFC%2FuZ9ayeYq2g%3D%3D&id2=UoH62EAv27BqSg%3D%3D; csg=62720f76; cancelledSubSites=empty; cookie17=UoH62EAv27BqSg%3D%3D; dnk=zkg852266010; skt=6bca334cb2eb3113; existShop=MTY1MTU0MTY1Mw%3D%3D; uc4=id4=0%40UOnlZ%2FcoxCrIUsehK6kNMyDrLgx1&nk4=0%40GwrkntVPltPB9cR46GncA5X9qudDESM%3D; _cc_=Vq8l%2BKCLiw%3D%3D; _l_g_=Ug%3D%3D; sg=080; _nk_=zkg852266010; cookie1=Vvj8uMJubtxirKFtxaDmWPxYCP5sb7EKtrFe1w68JDk%3D; mt=ci=84_1; uc1=cookie21=VFC%2FuZ9ajCbF8%2BYBpbBdiw%3D%3D&cookie15=V32FPkk%2Fw0dUvg%3D%3D&existShop=true&pas=0&cookie16=VT5L2FSpNgq6fDudInPRgavC%2BQ%3D%3D&cookie14=UoexMyYhRgt2TQ%3D%3D; v=0; tfstk=ci3GB0ZvwcrsG0rnVFa6lUgRxqlNarjaeqor8Cxdu94o5NEY_scAYgzQHBVvFdSf.; l=eBTM4C2ILF2sgZnfBO5w-urza77tCKOflsPzaNbMiInca6OFD36lJOC3kKrMedtjgtfj6eKyhayveRekJYULRxZvKT5ghvLY61p6-; isg=BOvrk_riDULLolGARgwuPPd3eg_VAP-C1XP89V1hoiqN_AFe5NAn0nsaVjySXFd6";
            getshop();
            int zuoric = 0;
            int c = 0;
            try
            {

                
                string aurl = "https://sycm.taobao.com/flow/new/live/monitor/item/tops/rank.json?dateRange=" + DateTime.Now.ToString("yyyy-MM-dd") + "%7C" + DateTime.Now.ToString("yyyy-MM-dd") + "&dateType=today&pageSize=999&page=1&order=desc&orderBy=uv&device=2&indexCode=uv%2CpayOrderByrCnt%2CpayRate&_=1651113494138&token=d68aae184";
               
              
                string burl = "https://sycm.taobao.com/flow/new/monitor/item/tops/rank.json?dateRange="+DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")+ "%7C" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "&dateType=recent1&pageSize=999&page=1&order=desc&orderBy=uv&device=2&indexCode=uv%2CpayOrderByrCnt%2CpayRate&_=1651112420539&token=d68aae184";
               
                //实时访客
                string ahtml = method.GetUrlWithCookie(aurl, 登录.cookie, "utf-8");
                MatchCollection title = Regex.Matches(ahtml, @"""title"":""([\s\S]*?)""");
                MatchCollection itemid = Regex.Matches(ahtml, @"{""itemId"":([\s\S]*?),");
                MatchCollection count = Regex.Matches(ahtml, @"""uv"":([\s\S]*?)value"":([\s\S]*?),");
                for (int i = 0; i < title.Count; i++)
                {

                    c = c + Convert.ToInt32(count[i].Groups[2].Value);

                }





                //昨日访客数据
                Dictionary<string, string> dict = new Dictionary<string, string>();
                string bhtml = method.GetUrlWithCookie(burl, 登录.cookie, "utf-8");
                MatchCollection btitle = Regex.Matches(bhtml, @"""title"":""([\s\S]*?)""");
                MatchCollection bitemid = Regex.Matches(bhtml, @"{""itemId"":([\s\S]*?),");
                MatchCollection bcount = Regex.Matches(bhtml, @"""uv"":([\s\S]*?)value"":([\s\S]*?),");

             
                for (int i = 0; i < btitle.Count; i++)
                {
                    dict.Add(btitle[i].Groups[1].Value,bcount[i].Groups[2].Value);
                    zuoric = zuoric + Convert.ToInt32(bcount[i].Groups[2].Value);
                   
                }


               








                //店铺进店关键词
                string keyurl = "https://sycm.taobao.com/flow/new/live/shop/source/detail.json?dateRange=" + DateTime.Now.ToString("yyyy-MM-dd") + "%7C" + DateTime.Now.ToString("yyyy-MM-dd") + "&dateType=today&pageSize=9999&page=1&order=desc&orderBy=uv&device=2&belong=all&pageId=23.s1150&pPageId=23&childPageType=se_keyword&indexCode=uv&_=1651115311634&token=d68aae184";

                //if (radioButton2.Checked == true)
                //{
                //    url = "https://sycm.taobao.com/flow/v3/shop/source/detail.json?dateRange=" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "%7C" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "&dateType=recent1&pageSize=999&page=1&order=desc&orderBy=crtRate&device=2&belong=all&pageId=23.s1150&pPageId=23&childPageType=se_keyword&indexCode=uv%2CcrtByrCnt%2CcrtRate&_=1651129487796&token=883f8dad0";
                //}


                string keyhtml = method.GetUrlWithCookie(keyurl, 登录.cookie, "utf-8");
                MatchCollection key = Regex.Matches(keyhtml, @"pageName"":{""value"":""([\s\S]*?)""");
                //MatchCollection keycount = Regex.Matches(html, @"uv"":{""value"":([\s\S]*?),");
                StringBuilder sb = new StringBuilder();
                int keycount = key.Count;
                if(keycount>7)
                {
                    keycount = 7;
                }
                for (int i = 0; i < keycount; i++)
                {

                    sb.Append(key[i].Groups[1].Value + System.Environment.NewLine);

                }


                string shebei = textBox1.Text;

                //退款
                string tuiurl = "https://trade.taobao.com/trade/itemlist/asyncSold.htm?event_submit_do_query=1&_input_charset=utf8";
                string tuihtml = method.PostUrlDefault(tuiurl, "prePageNo=1&sifg=0&action=itemlist%2FSoldQueryAction&tabCode=refunding&rateStatus=&orderStatus=REFUNDING&queryMore=false&close=0&pageNum=1&isQnNew=true", 登录.cookie);
                string tuicount = Regex.Match(tuihtml, @"""totalNumber"":([\s\S]*?),").Groups[1].Value;
               
                
                if(title.Count>0)
                {
                    del(shebei);
                }


                for (int i = 0; i < title.Count; i++)
                {
                    string zuoridangefangke = "0";
                   if(dict.ContainsKey(title[i].Groups[1].Value))
                    {
                        zuoridangefangke = dict[title[i].Groups[1].Value];
                    }
                    dataGridView1.Rows.Add(new object[] {shebei,shopname, shopurl, title[i].Groups[1].Value, "https://item.taobao.com/item.htm?id="+ itemid[i].Groups[1].Value,tuicount, zuoridangefangke,zuoric.ToString(), count[i].Groups[2].Value, c.ToString(), sb.ToString() });

                    string postdatas = "method=insert&shebei=" + shebei + "&shopname=" + shopname + "&shopurl=" + shopurl + "&item=" + title[i].Groups[1].Value + "&itemurl=" + "https://item.taobao.com/item.htm?id=" + itemid[i].Groups[1].Value + "&shouhou=" + tuicount + "&z_fk=" + zuoridangefangke + "&z_all_fk=" + zuoric.ToString() + "&s_fk=" + count[i].Groups[2].Value + "&s_all_fk=" + c.ToString() + "&keys=" + sb.ToString();

                    insert(postdatas);
                }








              




            

            }
            catch (Exception ex)
            {

              MessageBox.Show(ex.ToString())  ;
            }


        }

        #endregion



        string sqlurl = "http://120.24.252.181/kehutaobao/do.php";

        public void del(string shebei)
        {
            string postdata = "method=del&shebei="+shebei;
            string html = method.PostUrlDefault(sqlurl,postdata,"");
           
        }
        public string getall()
        {
            string postdata = "method=getall";
            string html = method.PostUrlDefault(sqlurl, postdata, "");
            return html;
        }
        public void insert(string postdata)
        {
            
            string html = method.PostUrlDefault(sqlurl, postdata, "");
           
        }


        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            if (登录.cookie == "")
            {
                MessageBox.Show("请先登录");
            }
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

      

        #region  程序关闭删除自身
        public static void TestForKillMyself()
        {
            string bat = @"@echo off
                           :tryagain
                           del %1
                           if exist %1 goto tryagain
                           del %0";
            File.WriteAllText("killme.bat", bat);//写bat文件
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "killme.bat";
            psi.Arguments = "\"" + Environment.GetCommandLineArgs()[0] + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
        }


        #endregion

        private void 淘宝访客_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoResizeColumns();

            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;


            #region 通用检测


            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"uHtj"))
            {
                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();

                return;
            }

            #endregion
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.DgvToTable(dataGridView1),"Sheet1",true);
        }


      
       
    }
}
