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
using System.Security.Policy;

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


            if (DateTime.Now > Convert.ToDateTime("2025-04-20"))
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
        string tk = "_m_h5_tk=6c93e38ede6a50619ecf3b7910cd09c7_1744691106050; _m_h5_tk_enc=2000b9cdebd836e8a9b3e5b35e6d2cbe; ";
        string cookie = "";
        public string chuli(string shuzhi)
        {
            try
            {
                if (shuzhi != "-1")
                {
                    shuzhi = (Convert.ToDouble(shuzhi) * 100).ToString("F2") + "%";
                }
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

                   for (int page= 0; page < 3000; page=page+50)
                    {
                        cookie = tk + x5sec;
                        Thread.Sleep(1000); 
                        string keyword = text[i].Trim();
                        if (keyword.Trim() == "")
                            continue;
                        string time = function.GetTimeStamp();

                        string token = Regex.Match(cookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;


                        string data = "{\"appId\":32517,\"params\":\"{\\\"appName\\\":\\\"findFactoryWap\\\",\\\"pageName\\\":\\\"findFactory\\\",\\\"searchScene\\\":\\\"factoryMTopSearch\\\",\\\"method\\\":\\\"getFactories\\\",\\\"keywords\\\":\\\""+keyword+"\\\",\\\"startIndex\\\":"+page+",\\\"asyncCount\\\":50,\\\"_wvUseWKWebView\\\":\\\"true\\\",\\\"tabCode\\\":\\\"findFactoryTab\\\",\\\"verticalProductFlag\\\":\\\"wapfactory\\\",\\\"_layoutMode_\\\":\\\"noSort\\\",\\\"source\\\":\\\"search_input\\\",\\\"searchBy\\\":\\\"input\\\",\\\"sessionId\\\":\\\"3366512100ee47288cf652b08e0ad0cd\\\"}\"}";

                        string str = token + "&" + time + "&12574478&" + data;
                        string sign = function.Md5_utf8(str);

                        string url = "https://h5api.m.1688.com/h5/mtop.relationrecommend.wirelessrecommend.recommend/2.0/?jsv=2.5.1&appKey=12574478&t="+time+"&sign="+sign+"&api=mtop.relationrecommend.WirelessRecommend.recommend&v=2.0&jsonpIncPrefix=reqTppId_32517_getFactories&type=jsonp&dataType=jsonp&callback=mtopjsonpreqTppId_32517_getFactories1&data=" + System.Web.HttpUtility.UrlEncode(data);




                        string html = function.GetUrlWithCookie(url, cookie, "utf-8");
                       // textBox2.Text = html;
                        if (html.Contains("令牌过期"))
                        {

                            string cookiestr = function.getSetCookie(url);
                            string _m_h5_tk = "_m_h5_tk=" + Regex.Match(cookiestr, @"_m_h5_tk=([\s\S]*?);").Groups[1].Value;
                            string _m_h5_tk_enc = "_m_h5_tk_enc=" + Regex.Match(cookiestr, @"_m_h5_tk_enc=([\s\S]*?);").Groups[1].Value;
                            tk = _m_h5_tk + ";" + _m_h5_tk_enc + ";";

                           
                            page = page - 50;
                            continue;

                           
                        }



                       

                        if (html.Contains("被挤爆啦"))
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


                        MatchCollection facName = Regex.Matches(html, @"\\""facName\\"":\\""([\s\S]*?)\\""");
                        MatchCollection wangwang = Regex.Matches(html, @"\\""loginId\\"":\\""([\s\S]*?)\\""");
                        MatchCollection userId = Regex.Matches(html, @"\\""userId\\"":\\""([\s\S]*?)\\""");
                        MatchCollection complianceRate = Regex.Matches(html, @"\\""complianceRate\\"":\\""([\s\S]*?)\\"""); //履约率
                        MatchCollection repeatRate = Regex.Matches(html, @"\\""repeatRate\\"":\\""([\s\S]*?)\\"""); //回头率
                        MatchCollection wwResponseRate = Regex.Matches(html, @"\\""wwResponseRate\\"":\\""([\s\S]*?)\\""");  //响应率
                       
                        for (int j = 0; j < facName.Count; j++)
                        {
                           
                            
                            try
                            {
                                

                                if (list.Contains(userId[j].Groups[1].Value))
                                {
                                    label1.Text = "重复，跳过：" + facName[j].Groups[1].Value;
                                    continue;
                                }
                                list.Add(userId[j].Groups[1].Value);




                                if (complianceRate[j].Groups[1].Value != "")
                                {
                                    if (Convert.ToDouble(complianceRate[j].Groups[1].Value) > Convert.ToDouble(textBox3.Text))
                                    {
                                        label1.Text = "履约率大于，跳过：" + facName[j].Groups[1].Value + "履约率：" + complianceRate[j].Groups[1].Value;
                                        continue;
                                    }
                                   
                                }




                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据 
                              
                                lv1.SubItems.Add("https://sale.1688.com/factory/card.html?__existtitle__=1&__removesafearea__=1&memberId="+userId[j].Groups[1].Value);
                                lv1.SubItems.Add(chuli(complianceRate[j].Groups[1].Value));
                                lv1.SubItems.Add(chuli(repeatRate[j].Groups[1].Value));
                                lv1.SubItems.Add(chuli(wwResponseRate[j].Groups[1].Value));
                                lv1.SubItems.Add("");
                                lv1.SubItems.Add(keyword);
                                lv1.SubItems.Add(facName[j].Groups[1].Value);
                                lv1.SubItems.Add(wangwang[j].Groups[1].Value);
                              
                                if (status == false)
                                    return;
                                if (listView1.Items.Count > 2)
                                {
                                    this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                                }

                                
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
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

        string cookie_rate = "cna=frGEIFigJCoCAdpd3mcX7K5e; mtop_partitioned_detect=1; _m_h5_tk=6c93e38ede6a50619ecf3b7910cd09c7_1744691106050; _m_h5_tk_enc=2000b9cdebd836e8a9b3e5b35e6d2cbe; xlly_s=1; ali_apache_id=33.8.36.101.1744683916174.048159.7; __wapcsf__=1; cookie1=Vvj8uMJubtxirKFtxaDmWPxYCP5sb7EKtrFe1w68JDk%3D; cookie2=20c43c99f2de4ee8370741cde868573e; cookie17=UoH62EAv27BqSg%3D%3D; sgcookie=E100%2BuxZIQX1AjKMjbkAYoArRh0kZvgn3zM1SXTd0BBbiC1AYtjiAEbbDxm7k19VTRtSDTYntLFVw2gGVrwi2U%2BE4G7qWRVnDI42zlDxCo7iK6hSBewbfq%2Bds24hQwCi66DE; t=cf2c4353efc04c8936f92df16ea453d9; _tb_token_=37e3be37637b; sg=080; csg=aed87bf8; lid=zkg852266010; unb=1052347548; uc4=id4=0%40UOnlZ%2FcoxCrIUsehKuDBOOymzOgW&nk4=0%40GwrkntVPltPB9cR46GndxAc08hK48Fs%3D; _nk_=zkg852266010; __cn_logon__=true; __cn_logon_id__=zkg852266010; ali_apache_track=c_mid=b2b-1052347548|c_lid=zkg852266010|c_ms=1; ali_apache_tracktmp=c_w_signed=Y; last_mid=b2b-1052347548; _csrf_token=1744683922405; taklid=4931c7f0924a4c1293f2a7765d95ce0f; leftMenuLastMode=EXPEND; leftMenuModeTip=shown; isg=BGlpRlYnKmX0ZRbRuHMmZtk1eBXDNl1oX6oPlAte5dCP0onkU4ZtOFfRlnZkyvWg; _user_vitals_session_data_={\"user_line_track\":true,\"ul_session_id\":\"hsn69xg4vkh\",\"last_page_id\":\"mind.1688.com%2Fz4zvmwenwe\"}; tfstk=gns-5nNb1SVk7_tgquam-jsawke0jMBP3_WsxBAoRsCAi_Lhx9TSlx9yigAlE3tjlLJWtgxWxBIAas8lr3THGK5BF9WHzzP2hMCgFvvSxkGBCIWk-WyrdZKPmamHZ7-dd1xLs5qgjTWy0HNgs2u9PE-BdkAQoUa327Ybs5qgmTWyYHNGZx2shSp9CH9WR0gbHLJKNHTBAnwvdpcSR01Ch-d2LB9SADTjcp9-FHtCOtwvLIOIGTFpGYOnvNWlHJ9SGflisipJyBnHMDF7KLLRDTOY15sxAUd1FInQ_JMoyBdVfS4A3sb5ZK5LXSKfCZKWe6ZsSI_Ahg8MXPgR1tfPX3sTw0vDJQQfVEHIAtt2ZE_dw73yNafv7KTjplJcsI6PVZeEGT1Mw39XolwOhe_c4FSuG0K17TxlRsZx21srtGjOW6lM6p07H-3E8U9465vFzhnrDopvs8EK82843KdgH-3E8U92HC28p2ueu-5..";
        public string getrate(string memberid)
        {
            string time = function.GetTimeStamp();

            string token = Regex.Match(cookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;


            string data = "{\"componentKey\":\"wp_pc_shop_behavior\",\"params\":\"{\\\"memberId\\\":\\\""+memberid+"\\\"}\"}";
            string str = token + "&" + time + "&12574478&" + data;
            string sign = function.Md5_utf8(str);

            string url = "https://h5api.m.1688.com/h5/mtop.alibaba.alisite.cbu.server.pc.moduleasyncservice/1.0/?jsv=2.7.0&appKey=12574478&t=" + time + "&sign=" + sign + "&api=mtop.alibaba.alisite.cbu.server.pc.ModuleAsyncService&v=1.0&type=jsonp&dataType=jsonp&callback=mtopjsonp18&data=" + System.Web.HttpUtility.UrlEncode(data);

            string html = function.GetUrlWithCookie(url, cookie, "utf-8");


          
           
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
