using System;
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
using myDLL;

namespace 天猫店铺采集
{
    public partial class 淘宝店铺采集 : Form
    {
        public 淘宝店铺采集()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {

            cookie = textBox4.Text.Trim();
            //if(textBox1.Text=="")
            //{
            //    MessageBox.Show("请导入关键词");
            //    return;
            //}

            if(DateTime.Now> Convert.ToDateTime("2024-08-01"))
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
        bool zanting = true;



        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 淘宝店铺采集_FormClosing(object sender, FormClosingEventArgs e)
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

        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show(function.Md5_utf8(textBox1.Text));

            登录 login = new 登录();
            login.Show();
        }



        //public string  getbaozhengjin()
        //{
        //    string cookiestr = function.getSetCookie2("https://h5api.m.taobao.com/h5/mtop.taobao.shop.simple.fetch/1.0/?jsv=2.6.2&appKey=12574478&t=1719823300449&sign=ba5a7cccfc68e3198ea0afbffc803fd7&api=mtop.taobao.shop.simple.fetch&type=originaljson&v=1.0&timeout=10000&dataType=json&sessionOption=AutoLoginAndManualLogin&needLogin=true&LoginRequest=true&jsonpIncPrefix=_1719823300447_&data=%7B%22shopId%22%3A%22542565393%22%2C%22sellerId%22%3A%223774665820%22%7D",reviewcookie);
        //    string _m_h5_tk = "_m_h5_tk=" + Regex.Match(cookiestr, @"_m_h5_tk=([\s\S]*?);").Groups[1].Value;
        //    string _m_h5_tk_enc = "_m_h5_tk_enc=" + Regex.Match(cookiestr, @"_m_h5_tk_enc=([\s\S]*?);").Groups[1].Value;
        //    reviewcookie = _m_h5_tk + ";" + _m_h5_tk_enc + ";";

        //    MessageBox.Show(cookiestr+"            "+reviewcookie);
        //    string token = Regex.Match(reviewcookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;

        //    string time = function.GetTimeStamp();
        //    string sign = function.Md5_utf8(token + " & " + time + "&12574478&{\"shopId\":\"542565393\",\"sellerId\":\"3774665820\"}");


        //    string url = "https://h5api.m.taobao.com/h5/mtop.taobao.shop.simple.fetch/1.0/?jsv=2.6.2&appKey=12574478&t="+time+"&sign="+sign+"&api=mtop.taobao.shop.simple.fetch&type=originaljson&v=1.0&timeout=10000&dataType=json&sessionOption=AutoLoginAndManualLogin&needLogin=true&LoginRequest=true&jsonpIncPrefix=_1719822459858_&data=%7B%22shopId%22%3A%22542565393%22%2C%22sellerId%22%3A%223774665820%22%7D";

        //    textBox1.Text = url;
        //    string html = method.GetUrlWithCookie(url, reviewcookie, "utf-8");

        //    MessageBox.Show(html);
        //    return "";
        //}









        string cookie = "";
        
        /// <summary>
        /// 在宝贝栏搜 店铺名称
        /// </summary>
        public void run()
        {

            List<string> list = new List<string>();
           // dics.Clear();
          

            try
            {
               // DataTable dt = method.ExcelToDataTable(textBox1.Text, true);
                for (int a = 0; a < 1; a++)
                {
                    //string keyword = dt.Rows[a][0].ToString().Trim();
                    //if (keyword.Trim() == "")
                    //    continue;



                    for (int i = 0; i < 2001; i=i+20)
                    {
                        //搜索店铺
                        //string url = "https://shopsearch.taobao.com/search?data-key=s&data-value="+i+"&ajax=true&_ksTS=1719834025032_542&callback=jsonp543&q="+keyword+"&js=1&initiative_id=staobaoz_20240701&ie=utf8&isb=0&shop_type=&ratesum=xin";

                        string time = function.GetTimeStamp();
                        string data = "{\"appId\":\"29859\",\"params\":\"{\\\"chituGroupAlias\\\":\\\"zhouzhou_liantiao_final\\\",\\\"_blendInfos\\\":\\\"true\\\",\\\"debug_rerankNewOpenCard\\\":\\\"false\\\",\\\"pvFeature\\\":\\\"654083998634;644832834668;668084343069;662334090942;665339768743;664390297378;664047381602\\\",\\\"tab\\\":\\\"shop\\\",\\\"grayHair\\\":\\\"false\\\",\\\"sversion\\\":\\\"13.7\\\",\\\"from\\\":\\\"input\\\",\\\"isBeta\\\":\\\"false\\\",\\\"brand\\\":\\\"HUAWEI\\\",\\\"info\\\":\\\"wifi\\\",\\\"client_for_bts\\\":\\\"client_android_view_preload:1000001\\\",\\\"ttid\\\":\\\"600000@taobao_android_10.8.0\\\",\\\"rainbow\\\":\\\"\\\",\\\"areaCode\\\":\\\"CN\\\",\\\"vm\\\":\\\"nw\\\",\\\"elderHome\\\":\\\"false\\\",\\\"style\\\":\\\"list\\\",\\\"page\\\":3,\\\"device\\\":\\\"HMA-AL00\\\",\\\"editionCode\\\":\\\"CN\\\",\\\"cityCode\\\":\\\"110100\\\",\\\"countryNum\\\":\\\"156\\\",\\\"newSearch\\\":\\\"false\\\",\\\"chituBiz\\\":\\\"TaobaoPhoneSearch\\\",\\\"utd_id\\\":\\\"XYDZLfLy3ZQDAKmnYOhvIwW4\\\",\\\"network\\\":\\\"wifi\\\",\\\"hasPreposeFilter\\\":\\\"false\\\",\\\"client_os\\\":\\\"Android\\\",\\\"gpsEnabled\\\":\\\"true\\\",\\\"apptimestamp\\\":\\\"" + time + "\\\",\\\"canP4pVideoPlay\\\":\\\"true\\\",\\\"homePageVersion\\\":\\\"v7\\\",\\\"searchElderHomeOpen\\\":\\\"false\\\",\\\"n\\\":\\\"10\\\",\\\"search_action\\\":\\\"initiative\\\",\\\"q\\\":\\\"软件开发\\\",\\\"tagSearchKeyword\\\":null,\\\"sort\\\":\\\"_coefp\\\",\\\"filterTag\\\":\\\"\\\",\\\"prop\\\":\\\"\\\"}\"}";


                        string token = Regex.Match(cookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;

                    










                        
                        string str = token + "&" + time + "&1257447&" + data;
                        string sign = function.Md5_utf8(str);

                        string url = "https://h5api.m.taobao.com/h5/mtop.relationrecommend.wirelessrecommend.recommend/2.0/?jsv=2.7.0&appKey=12574478&t="+time+"&sign="+sign+"&api=mtop.relationrecommend.WirelessRecommend.recommend&v=2.0&H5Request=true&preventFallback=true&type=jsonp&dataType=jsonp&callback=mtopjsonp2&data="+data;
                        string html = function.GetUrlWithCookie(url, cookie, "utf-8");




                        textBox4.Text = html;
                        MessageBox.Show(sign);

                        MatchCollection shopname = Regex.Matches(html, @"""rawTitle"":""([\s\S]*?)""");

                        MatchCollection shopurl = Regex.Matches(html, @"""shopUrl"":""([\s\S]*?)""");
                        MatchCollection startFee = Regex.Matches(html, @"""startFee"":""([\s\S]*?)""");


                        for (int j = 0; j < shopname.Count; j++)
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据 
                            lv1.SubItems.Add(shopname[j].Groups[1].Value);

                            lv1.SubItems.Add("https:"+shopurl[j].Groups[1].Value);
                           // lv1.SubItems.Add(keyword);
                            lv1.SubItems.Add("1000");
                            lv1.SubItems.Add("是");
                            lv1.SubItems.Add(startFee[j].Groups[1].Value);

                        }

                        while (zanting == false)
                        {
                            Application.DoEvents();//等待本次加载完毕才执行下次循环.
                        }
                        if (status == false)
                            return;
                        if (listView1.Items.Count > 2)
                        {
                            this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                        }
                    }

                   

                    Thread.Sleep(5000);
                }

                MessageBox.Show("完成");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void 淘宝店铺采集_Load(object sender, EventArgs e)
        {

        }
    }
}
