using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using myDLL;
using System.IO;
using System.Web.UI.WebControls;

namespace 主程序2025
{
    public partial class _1688 : Form
    {
        public _1688()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请导入关键词");
                return;
            }


            if (DateTime.Now > Convert.ToDateTime("2025-04-15"))
            {
                function.TestForKillMyself();
            }
            else
            {

                status = true;
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(run);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }


        }



        Thread thread;
        bool status = true;

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void _1688_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }


        string x5sec = "";
        string tk = "_m_h5_tk=2a5c26761171f14e313e414f7f7c6ef7_1744645665900; _m_h5_tk_enc=633e3be064aea1de169049d9c39e41be;";
        string cookie = "";
        public string chuli(string shuzhi)
        {
            try
            {
                shuzhi = (Convert.ToDouble(shuzhi) * 100).ToString("F2") + "%";
            }
            catch (Exception)
            {

                
            }
        return shuzhi;  
        }

       

        List<string> list = new List<string>();

        #region 主程序


        public void run()
        {

          
            try
            {

                StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);



                for (int i = 0; i < text.Length; i++)
                {

                   for (int page= 1; page < 101; page++)
                    {
                        cookie = tk + x5sec;
                        Thread.Sleep(1000); 
                        string keyword = text[i].Trim();
                        if (keyword.Trim() == "")
                            continue;
                        string time = function.GetTimeStamp();

                        string token = Regex.Match(cookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;


                        string data = "{\"appId\":32517,\"params\":\"{\\\"beginPage\\\":\\\"" + page + "\\\",\\\"pageSize\\\":60,\\\"method\\\":\\\"getOfferList\\\",\\\"pageId\\\":\\\"kIRfUzgXbK4LwWrK8gRLo8KA7Cz4IsxD3Oiusu2Lxwk090zp\\\",\\\"verticalProductFlag\\\":\\\"pcmarket\\\",\\\"searchScene\\\":\\\"pcOfferSearch\\\",\\\"charset\\\":\\\"GBK\\\",\\\"spm\\\":\\\"a260k.home2024.searchbox.0\\\",\\\"filtOfferTags\\\":\\\"\\\",\\\"keywords\\\":\\\"" + keyword + "\\\"}\"}";

                        string str = token + "&" + time + "&12574478&" + data;
                        string sign = function.Md5_utf8(str);

                        string url = "https://h5api.m.taobao.com/h5/mtop.relationrecommend.wirelessrecommend.recommend/2.0/?jsv=2.7.2&appKey=12574478&t=" + time + "&sign=" + sign + "&api=mtop.relationrecommend.wirelessrecommend.recommend&v=2.0&type=jsonp&dataType=jsonp&callback=mtopjsonp18&data=" + System.Web.HttpUtility.UrlEncode(data);


                        // https://h5api.m.1688.com/h5/mtop.relationrecommend.wirelessrecommend.recommend/2.0/?jsv=2.7.2&appKey=12574478&t=1744443681882&sign=07ed661318bc1001527daa513339aaa0&api=mtop.relationrecommend.WirelessRecommend.recommend&v=2.0&jsonpIncPrefix=reqTppId_32517_getOfferList&type=jsonp&dataType=jsonp&callback=mtopjsonpreqTppId_32517_getOfferList6&data=
                        string html = function.GetUrlWithCookie(url, cookie, "utf-8");

                        if(html.Contains("被挤爆啦"))
                        {
                            string capurl = Regex.Match(html, @"""url"":""([\s\S]*?)""").Groups[1].Value;
                           
                            string caphtml = function.GetUrlWithCookie(capurl, cookie, "utf-8");
                            string action = Regex.Match(caphtml, @"""action"": ""([\s\S]*?)""").Groups[1].Value;
                          
                         
                          
                            if (action != "")
                            {
                                x5sec = function.getx5(action, capurl);
                                continue;
                            }

                        }




                       // textBox2.Text = html;
                     
                        
                        if (html.Contains("令牌过期"))
                        {

                            string cookiestr = function.getSetCookie(url);
                            string _m_h5_tk = "_m_h5_tk=" + Regex.Match(cookiestr, @"_m_h5_tk=([\s\S]*?);").Groups[1].Value;
                            string _m_h5_tk_enc = "_m_h5_tk_enc=" + Regex.Match(cookiestr, @"_m_h5_tk_enc=([\s\S]*?);").Groups[1].Value;
                            cookie = _m_h5_tk + ";" + _m_h5_tk_enc + ";"+x5sec;
                           
                            Thread.Sleep(5000);
                            continue;
                          
                        }
                       

                        MatchCollection ahtmls = Regex.Matches(html, @"""isSearchShow"":""([\s\S]*?)""sub_object_type""");
                     

                        for (int j = 0; j < ahtmls.Count; j++)
                        {
                            cookie = tk + x5sec;
                            Thread.Sleep(1000);
                            try
                            {
                                string memberId = Regex.Match(ahtmls[j].Groups[1].Value, @"""memberId"":""([\s\S]*?)""").Groups[1].Value;


                               
                                string shop = Regex.Match(ahtmls[j].Groups[1].Value, @"""shop""([\s\S]*?)""text"":""([\s\S]*?)""").Groups[2].Value;
                                string wangwang = Regex.Match(ahtmls[j].Groups[1].Value, @"""loginIdOfUtf8"":""([\s\S]*?)""").Groups[1].Value;

                                if (list.Contains(memberId))
                                {
                                    label1.Text = "重复，跳过：" +shop;
                                    continue;
                                }
                                list.Add(memberId); 

                                string ratehtml = getrate(memberId);


                              


                                string shop_nps_pay_ord_cnt_1m_001 = Regex.Match(ratehtml, @"""shop_nps_pay_ord_cnt_1m_001"":""([\s\S]*?)""").Groups[1].Value;
                                string shop_nps_lgt_48h_got_rate_30d = Regex.Match(ratehtml, @"""shop_nps_lgt_48h_got_rate_30d"":""([\s\S]*?)""").Groups[1].Value; //揽收率
                                string shop_lgt_fulfill_got_rate_30d = Regex.Match(ratehtml, @"""shop_lgt_fulfill_got_rate_30d"":""([\s\S]*?)""").Groups[1].Value;  //履约率
                                string shop_sns_ww_response_rate_30d = Regex.Match(ratehtml, @"""shop_sns_ww_response_rate_30d"":""([\s\S]*?)""").Groups[1].Value;  //响应率

                               // MessageBox.Show(ratehtml);
                                //if (shop_nps_lgt_48h_got_rate_30d!="")
                                //{
                                //    if(Convert.ToDouble(shop_nps_lgt_48h_got_rate_30d)>0.3)
                                //    {
                                //        label1.Text = "揽收率大于30%，跳过：" + shop + "揽收率：" + shop_nps_lgt_48h_got_rate_30d;
                                //    }
                                //    continue;
                                //}

                                string fahuohtml = getfahuo(memberId);


                             

                                string fahuoTime = Regex.Match(fahuohtml, @"""fahuoTime"":([\s\S]*?),").Groups[1].Value.Replace("\"", "");
                                string offer_id = Regex.Match(fahuohtml, @"""id"":""([\s\S]*?)""").Groups[1].Value;
                                string title = Regex.Match(fahuohtml, @"""intelligentTitle"":""([\s\S]*?)""").Groups[1].Value;


                                
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据 
                                lv1.SubItems.Add("https://detail.1688.com/offer/" + offer_id + ".html");
                                lv1.SubItems.Add(shop_nps_pay_ord_cnt_1m_001);
                                lv1.SubItems.Add(chuli(shop_nps_lgt_48h_got_rate_30d));
                                lv1.SubItems.Add(chuli(shop_lgt_fulfill_got_rate_30d));

                                lv1.SubItems.Add(fahuoTime);
                                lv1.SubItems.Add(keyword);
                                lv1.SubItems.Add(title);
                                lv1.SubItems.Add(shop);
                                lv1.SubItems.Add(System.Web.HttpUtility.UrlDecode(wangwang));
                                // textBox2.Text = "https://detail.1688.com/offer/" + offer_id + ".html";

                                if (status == false)
                                    return;
                                if (listView1.Items.Count > 2)
                                {
                                    this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                                }

                                
                            }
                            catch (Exception)
                            {

                                continue;
                            }
                        }

                    }




                }

                MessageBox.Show("完成");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());

            }

        }

        #endregion


        string x5sec_rate = "";
        
        string cookie_rate = "cna=D9B/IPCPU0kCAXLv8NwMVmh0; lid=zkg852266010; taklid=e35b74a058804607a46ae69c8452d218; ali_apache_track=c_mid=b2b-1052347548|c_lid=zkg852266010|c_ms=1; keywordsHistory=%E5%AE%B6%E5%B1%85%E6%8B%96%E9%9E%8B%3B%E5%84%BF%E7%AB%A5%E5%86%85%E8%A3%A4%E7%94%B7; callClientScheme=ali1688im%3Asendmsg; cookie1=Vvj8uMJubtxirKFtxaDmWPxYCP5sb7EKtrFe1w68JDk%3D; cookie2=1f1bc657bdf4de77633620c567866696; cookie17=UoH62EAv27BqSg%3D%3D; sgcookie=E100enwrW4RIFIXK5AJBg4NNxiWaePQ1znFgbH5HQTvQL%2Ftfs4trd3i8lGId99kWp6Hm9TN687DZmQbTHayRT7Jq1cGxTvFp5GZaJXHVjWagTT3caZSempvxnrExPRhcS%2B1g; t=d5978997e68e6bcbcdae638e8120847f; _tb_token_=e3618305b5a7b; sg=080; csg=fd042ca7; unb=1052347548; uc4=nk4=0%40GwrkntVPltPB9cR46GndxAcxTpGRO8I%3D&id4=0%40UOnlZ%2FcoxCrIUsehKuDBPTW9BRob; _nk_=zkg852266010; __cn_logon__=true; __cn_logon_id__=zkg852266010; is_identity=buyer; inquiry_preference=webIM; leftMenuLastMode=EXPEND; mtop_partitioned_detect=1; _m_h5_tk=2a5c26761171f14e313e414f7f7c6ef7_1744645665900; _m_h5_tk_enc=633e3be064aea1de169049d9c39e41be; xlly_s=1; leftMenuModeTip=shown; _user_vitals_session_data_={\"user_line_track\":true,\"ul_session_id\":\"2ay2lukxzjt\",\"last_page_id\":\"s.1688.com%2Fyz1mi8u4mj\"}; isg=BJqaND7BqWjSsiXBNpOsPxl360C8yx6lCM8cIaQTRi34FzpRjFtutWBl5-OLx5Y9; tfstk=gSXi3sA-Z1RsDtb-WwJ_Z3RuXUPKfc9X-ZHvkKL4Te8IBIRw0Doe-ZT96d8tnobCRsCYfIQhoabR6ne6WxvcmZ_vgtLAnc6hrxHT55sqnKp4yze8eGZ6hKz8hLmk2FtfYErp_wch7DJ4yzeK9DR_WK7T9IjgKX-W8h-q_ErH8htD3V8NueRe433w3K74xe-WVj-wbEkeYet23E723kvENrGwyt5PLyT3WcVMuUWHjCYPx7MqnOHJsUSw-xzPKh2XzG8n3x8tncthxGUiFMp1EaxRWJkGrg6C-H7znyvfRs72mwySkd1R5TOhW7kFYdYeasR3bRSHIFAkfQmm_dfV5tA1smZ5Yd7CNUOaOythBTdDPB0U7MIH7QfcWyM2W_jF-QBKJYpfRs72mwuG4BlEafeqhHrALjGX_HtHyU4loOOTL1Q8xkcRcC-BVUE3xjGX_HtHykqnw1OwA3TR.";

        public string getrate(string memberid)
        {
            string time = function.GetTimeStamp();

            string token = Regex.Match(cookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;


            string data = "{\"componentKey\":\"wp_pc_shop_behavior\",\"params\":\"{\\\"memberId\\\":\\\""+memberid+"\\\"}\"}";
            string str = token + "&" + time + "&12574478&" + data;
            string sign = function.Md5_utf8(str);

            string url = "https://h5api.m.1688.com/h5/mtop.alibaba.alisite.cbu.server.pc.moduleasyncservice/1.0/?jsv=2.7.0&appKey=12574478&t=" + time + "&sign=" + sign + "&api=mtop.alibaba.alisite.cbu.server.pc.ModuleAsyncService&v=1.0&type=jsonp&dataType=jsonp&callback=mtopjsonp18&data=" + System.Web.HttpUtility.UrlEncode(data);

            string html = function.GetUrlWithCookie(url, cookie, "utf-8");

            textBox2.Text = html;   
           // MessageBox.Show(html);  
            if (html.Contains("被挤爆啦"))
            {
                string capurl = Regex.Match(html, @"""url"":""([\s\S]*?)""").Groups[1].Value;

                string caphtml = function.GetUrlWithCookie(capurl, cookie, "utf-8");
                string action = Regex.Match(caphtml, @"""action"": ""([\s\S]*?)""").Groups[1].Value;



                if (action != "")
                {
                    x5sec_rate = function.getx5(action, capurl);
                 
                    
                    cookie_rate = token + x5sec_rate;


                    html = function.GetUrlWithCookie(url, cookie_rate, "utf-8");
                }

            }
            return html;    

        }


        public string getfahuo(string memberid)
        {
            string time = function.GetTimeStamp();

            string token = Regex.Match(cookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;


            string data = "{\"dataType\":\"moduleData\",\"argString\":\"{\\\"appName\\\":\\\"offerlistAlisite\\\",\\\"memberId\\\":\\\""+memberid+"\\\",\\\"appdata\\\":{\\\"sortType\\\":\\\"ninetytradenumdowndesc\\\",\\\"count\\\":2,\\\"showMode\\\":\\\"single\\\"}}\"}";
            string str = token + "&" + time + "&12574478&" + data;
            string sign = function.Md5_utf8(str);

            string url = "https://h5api.m.1688.com/h5/mtop.1688.shop.data.get/1.0/?jsv=2.7.4&appKey=12574478&t=" + time + "&sign=" + sign + "&api=mtop.alibaba.alisite.cbu.server.pc.ModuleAsyncService&v=1.0&type=jsonp&dataType=jsonp&callback=mtopjsonp18&data=" + System.Web.HttpUtility.UrlEncode(data);

            string html = function.GetUrlWithCookie(url, cookie, "utf-8");
            return html;

        }

        private void _1688_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            cookie = method.getSetCookie("https://h5api.m.1688.com/");
            MessageBox.Show(cookie);
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
           
        }
    }
}
