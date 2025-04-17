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
using System.Web.UI;

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


           
            else
            {

                status = true;
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(run);
                    thread.Start();
                    System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
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
                        if (DateTime.Now > Convert.ToDateTime("2025-04-21"))
                        {
                            function.TestForKillMyself();
                            return;
                        }


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


                        label1.Text = "正在查询："+page;

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


                        if (facName.Count == 0)
                            break;
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
                                lv1.SubItems.Add(keyword);
                                lv1.SubItems.Add(facName[j].Groups[1].Value);
                                lv1.SubItems.Add(wangwang[j].Groups[1].Value);
                                try
                                {
                                    string item24 = getfahuo(userId[j].Groups[1].Value);
                                    lv1.SubItems.Add(item24);
                                }
                                catch (Exception)
                                {
                                    lv1.SubItems.Add("");


                                }


                                Thread.Sleep(100);
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




        public string getfahuo(string memberid)
        {
            string time = function.GetTimeStamp();

            string token = Regex.Match(cookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;


            string data = "{\"componentKey\":\"Wp_pc_common_offerlist\",\"params\":\"{\\\"memberId\\\":\\\""+memberid+"\\\",\\\"appdata\\\":{\\\"sortType\\\":\\\"tradenumdown\\\",\\\"sellerRecommendFilter\\\":false,\\\"mixFilter\\\":false,\\\"tradenumFilter\\\":false,\\\"quantityBegin\\\":null,\\\"pageNum\\\":1,\\\"count\\\":50}}\"}";
            string str = token + "&" + time + "&12574478&" + data;
            string sign = function.Md5_utf8(str);

            string url = "https://h5api.m.1688.com/h5/mtop.alibaba.alisite.cbu.server.moduleasyncservice/1.0/?jsv=2.7.0&appKey=12574478&t="+time+"&sign="+sign+"&api=mtop.alibaba.alisite.cbu.server.ModuleAsyncService&v=1.0&type=json&valueType=string&dataType=jsonp&timeout=10000";

            string postdata = System.Web.HttpUtility.UrlEncode(data);
            string html = function.PostUrlDefault(url, "data=" + postdata, cookie);

            //textBox2.Text = html;
          
            MatchCollection agentInfo = Regex.Matches(html, @"""agentInfo""([\s\S]*?),");
            MatchCollection id = Regex.Matches(html, @"object_id\@([\s\S]*?)\^");

            for (int i = 0; i < agentInfo.Count; i++)
            {

                string fahuotime = Regex.Match(agentInfo[i].Groups[1].Value, @"""fahuoTime"":""([\s\S]*?)""").Groups[1].Value;
                if(fahuotime=="24")
                {
                    return "https://detail.1688.com/offer/" + id[i].Groups[1].Value +".html";
                }

            }

            return "";

        }

        private void _1688_Load(object sender, EventArgs e)
        {

        }

       
        private void button6_Click_1(object sender, EventArgs e)
        {
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
          
            MessageBox.Show(getfahuo("b2b-220748602880961213"));
            listView1.Items.Clear();
           
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;

            try
            {
                System.Diagnostics.Process.Start(listView1.SelectedItems[0].SubItems[8].Text);
            } 
            catch (Exception)
            {

               
            }
           
        }
    }
}
