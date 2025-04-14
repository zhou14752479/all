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

        string cookie = "\r\ncna=oRKCIBIeazUCAXLv8NxDPZ2A; xlly_s=1; _csrf_token=1744512163538; leftMenuModeTip=shown; isg=BMTEs4dUf0wZCMsAqvXdsZdilUK23ehHJXYHkt5lUA9SCWTTBu241_qqSaHRESCf; tfstk=gfaoEXi9juo7Cmy9e-u5jx2upsjYNQgI9JLKp2HF0xkXp7Fp2Jq3QxhyYe8Umv24nYz-v_Gh-5MjevC79Wu3QRDzwWBB-Bk0Uen8-WTnTWyCNvC5D7NSR2WOBJbTN7OOBG7NH25Ug2hH4k3xR7NSRI6OBNQTNpAzRIer8Juq0XGZ8XkULqJqhXGeUX8PiSkj3HJrLXu20bh942uULS5mOxlrZEtrByrautGZdD7rVOXiw0DaaxY3Nrcrq3Nrne8UoomoQpDD8ezmZSKb0a8GvArsecHa3ZLo-Sl3exeN7E0qjlNKt8bkz4E4czg8kOvKqJDrbrmDU3lirYmazm7CNSoUmr08z9t_MSDrjqEAcTnKr8qsC0SfhJVmeJzo0LXxplFYzm2NHE2IxlNKt8bkzRSPbn-w2w8BFaFMAHirGjDt9Pq2wznv1x5cihFI4jGsBsfDAHirGjDOisxTV0lj1AC..; _user_vitals_session_data_={\"user_line_track\":true,\"ul_session_id\":\"y3sloim32xh\",\"last_page_id\":\"shop06532883t1350.1688.com%2Ftawwz3ekctf\"}; mtop_partitioned_detect=1; _m_h5_tk=db31e9f9e2ce7df308c93b148bf00238_1744528826404; _m_h5_tk_enc=9cd1b5b69f1d6966fa39064cd3444101";
        
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
                      

                        //if (html.Contains("令牌过期"))
                        //{

                        //    string cookiestr = function.getSetCookie(url);
                        //    string _m_h5_tk = "_m_h5_tk=" + Regex.Match(cookiestr, @"_m_h5_tk=([\s\S]*?);").Groups[1].Value;
                        //    string _m_h5_tk_enc = "_m_h5_tk_enc=" + Regex.Match(cookiestr, @"_m_h5_tk_enc=([\s\S]*?);").Groups[1].Value;
                        //   cookie = _m_h5_tk + ";" + _m_h5_tk_enc + ";";
                        //    token = Regex.Match(cookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;

                        //    continue;
                        //}
                        //textBox2.Text = html;

                        MatchCollection ahtmls = Regex.Matches(html, @"""isSearchShow"":""([\s\S]*?)""sub_object_type""");
                     

                        for (int j = 0; j < ahtmls.Count; j++)
                        {
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




        public string getrate(string memberid)
        {
            string time = function.GetTimeStamp();

            string token = Regex.Match(cookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;


            string data = "{\"componentKey\":\"wp_pc_shop_behavior\",\"params\":\"{\\\"memberId\\\":\\\""+memberid+"\\\"}\"}";
            string str = token + "&" + time + "&12574478&" + data;
            string sign = function.Md5_utf8(str);

            string url = "https://h5api.m.1688.com/h5/mtop.alibaba.alisite.cbu.server.pc.moduleasyncservice/1.0/?jsv=2.7.0&appKey=12574478&t=" + time + "&sign=" + sign + "&api=mtop.alibaba.alisite.cbu.server.pc.ModuleAsyncService&v=1.0&type=jsonp&dataType=jsonp&callback=mtopjsonp18&data=" + System.Web.HttpUtility.UrlEncode(data);

            string html = function.GetUrlWithCookie(url, cookie, "utf-8");
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
    }
}
