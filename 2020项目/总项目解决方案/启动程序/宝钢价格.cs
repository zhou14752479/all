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
using helper;

namespace 启动程序
{
    public partial class 宝钢价格 : Form
    {
        public 宝钢价格()
        {
            InitializeComponent();
        }

        bool status = true;
        bool zanting = true;

        string[] months = {"202001", "202002", "202003", "202004" };

     
        public ArrayList getliushui(string patten)
        {

            string html = method.GetUrl("http://ecommerce.ibaosteel.com/baosteel_csm/ne/productPriceQueryMainCas.cas?functionCode=productPriceQueryMainCas&month=202004&prodCode=HRB&seqNum=HRB0001", "GBK");
            MatchCollection liushuis = Regex.Matches(html, patten);

            ArrayList lists = new ArrayList();
            for (int i = 0; i < liushuis.Count-1; i++)
            {
                lists.Add(liushuis[i].Groups[1].Value);

            }

            return lists;
        }







        #region HRB
        public void HRB()
        {

            string pinzhong = "HRB";
            ArrayList liushuis = getliushui(@"HRB','([\s\S]*?)'");
            string[] kuandus = {"700-899", "900-999", "1000-1800", "1801-2100", };
            try
            {
                foreach (string month in months)
                {
                    foreach (string liushui in liushuis)
                    {
                        string url = "http://ecommerce.ibaosteel.com/baosteel_csm/ne/productPriceQueryMainCas.cas?functionCode=productPriceQueryMainCas&month=" + month + "&prodCode="+pinzhong+"&seqNum="+liushui;
                        
                        string html = method.GetUrl(url, "GBK");

                        Match paihao = Regex.Match(html, @">牌号：([\s\S]*?)</td>");

                        MatchCollection houdus = Regex.Matches(html, @"border-top:none'>([\s\S]*?)</td>");
                        MatchCollection prices = Regex.Matches(html, @"x:num=([\s\S]*?)>([\s\S]*?)</td>");


                        ArrayList lists = new ArrayList();
                        lists.Add("1.50~1.79");
                        foreach (Match houdu in houdus)
                        {
                            lists.Add(houdu.Groups[1].Value);
                        }
                        int j = 0;


                        for (int i = 0; i < lists.Count; i++)
                        {
                            foreach (string kuandu in kuandus)
                            {
                               
                                

                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count+1).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add(month);
                                lv1.SubItems.Add(pinzhong);
                                lv1.SubItems.Add(liushui);
                                lv1.SubItems.Add(paihao.Groups[1].Value.Trim());
                                lv1.SubItems.Add(lists[i].ToString());
                                lv1.SubItems.Add(kuandu);
                                lv1.SubItems.Add(prices[2*j].Groups[2].Value.Trim());
                                lv1.SubItems.Add("表列价格厚度公差为PT.A交货价格，按PT.B交货的加价50元/吨,按PT.C交货的加价200元/吨。表列价格宽度公差为PW.A交货价格，按PW.B交货的加价50元 / 吨（仅针对毛边板)。仅针对钢板不平度为PF.A交货不加价，按PF.B交货的加价100元 / 吨。");
                                lv1.SubItems.Add("BQB标准+α加价100元/吨。精整卷、平整卷加价100元 / 吨毛边板、商品卷加价100元 / 吨切边板、切边卷加价200元 / 吨");
                                //lv1.SubItems.Add(Regex.Replace(a1.Groups[1].Value, "<[^>]+>", "").Trim());
                                //lv1.SubItems.Add(Regex.Replace(a2.Groups[1].Value, "<[^>]+>", "").Trim());
                                j=j+1;

                                while (zanting == false)
                                {
                                    Application.DoEvents();//等待本次加载完毕才执行下次循环.
                                }
                                if (status == false)
                                    return;

                            }
                        }

                    }
                }
            }

            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region HRC
        public void HRC()
        {

            string pinzhong = "HRC";
            ArrayList liushuis = getliushui(@"HRC','([\s\S]*?)'");
            string[] kuandus = { "700-799", "800-899", "900-999", "1000-1800", "1801-2100", };
            try
            {
                foreach (string month in months)
                {
                    foreach (string liushui in liushuis)
                    {
                        string url = "http://ecommerce.ibaosteel.com/baosteel_csm/ne/productPriceQueryMainCas.cas?functionCode=productPriceQueryMainCas&month=" + month + "&prodCode=" + pinzhong + "&seqNum=" + liushui;

                        string html = method.GetUrl(url, "GBK");

                        Match paihao = Regex.Match(html, @">牌号：([\s\S]*?)</td>");

                        MatchCollection houdus = Regex.Matches(html, @"border-top:none'>([\s\S]*?)</td>");
                        MatchCollection prices = Regex.Matches(html, @"x:num([\s\S]*?)>([\s\S]*?)</td>");


                        ArrayList lists = new ArrayList();
                       
                        foreach (Match houdu in houdus)
                        {
                            lists.Add(houdu.Groups[1].Value);
                        }
                        int j = 0;


                        for (int i = 0; i < lists.Count; i++)
                        {
                            foreach (string kuandu in kuandus)
                            {



                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add(month);
                                lv1.SubItems.Add(pinzhong);
                                lv1.SubItems.Add(liushui);
                                lv1.SubItems.Add(paihao.Groups[1].Value.Trim());
                                lv1.SubItems.Add(lists[i].ToString());
                                lv1.SubItems.Add(kuandu);
                                lv1.SubItems.Add(prices[j].Groups[2].Value.Trim());
                                lv1.SubItems.Add("表列价格厚度公差为PT.A交货价格，按PT.B交货的加价50元/吨,按PT.C交货的加价200元/吨。表列价格宽度公差为PW.A交货价格，按PW.B交货的加价50元 / 吨（仅针对毛边板)。仅针对钢板不平度为PF.A交货不加价，按PF.B交货的加价100元 / 吨。");
                                lv1.SubItems.Add("BQB标准+α加价100元/吨。精整卷、平整卷加价100元 / 吨毛边板、商品卷加价100元 / 吨切边板、切边卷加价200元 / 吨");
                                //lv1.SubItems.Add(Regex.Replace(a1.Groups[1].Value, "<[^>]+>", "").Trim());
                                //lv1.SubItems.Add(Regex.Replace(a2.Groups[1].Value, "<[^>]+>", "").Trim());
                                j = j + 1;

                                while (zanting == false)
                                {
                                    Application.DoEvents();//等待本次加载完毕才执行下次循环.
                                }
                                if (status == false)
                                    return;

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

        #endregion


        #region HRM
        public void HRM()
        {

            string pinzhong = "HRM";
            ArrayList liushuis = getliushui(@"HRM','([\s\S]*?)'");

           
            try
            {
                foreach (string month in months)
                {
                    foreach (string liushui in liushuis)
                    {
                        string url = "http://ecommerce.ibaosteel.com/baosteel_csm/ne/productPriceQueryMainCas.cas?functionCode=productPriceQueryMainCas&month=" + month + "&prodCode=" + pinzhong + "&seqNum=" + liushui;

                        string html = method.GetUrl(url, "GBK");

                        Match paihao = Regex.Match(html, @">牌号：([\s\S]*?)</td>");

                        MatchCollection houdus = Regex.Matches(html, @"border-top:none'>([\s\S]*?)</td>");
                        MatchCollection prices = Regex.Matches(html, @"x:num([\s\S]*?)>([\s\S]*?)</td>");


                        MatchCollection kuandus = Regex.Matches(html, @"<td colspan=2([\s\S]*?)>([\s\S]*?)</td>");

                        ArrayList lists = new ArrayList();
                        
                        foreach (Match houdu in houdus)
                        {
                            lists.Add(houdu.Groups[1].Value);
                        }
                        int j = 0;


                        for (int i = 0; i < lists.Count; i++)
                        {
                            foreach (Match kuandu in kuandus)
                            {



                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add(month);
                                lv1.SubItems.Add(pinzhong);
                                lv1.SubItems.Add(liushui);
                                lv1.SubItems.Add(paihao.Groups[1].Value.Trim());
                                lv1.SubItems.Add(lists[i].ToString());
                                lv1.SubItems.Add(kuandu.Groups[2].Value);
                                lv1.SubItems.Add(prices[2*j].Groups[2].Value.Trim());
                                lv1.SubItems.Add("表列价格厚度公差为PT.A交货价格，按PT.B交货的加价50元/吨,按PT.C交货的加价200元/吨。表列价格宽度公差为PW.A交货价格，按PW.B交货的加价50元 / 吨（仅针对毛边板)。仅针对钢板不平度为PF.A交货不加价，按PF.B交货的加价100元 / 吨。");
                                lv1.SubItems.Add("BQB标准+α加价100元/吨。精整卷、平整卷加价100元 / 吨毛边板、商品卷加价100元 / 吨切边板、切边卷加价200元 / 吨");
                                //lv1.SubItems.Add(Regex.Replace(a1.Groups[1].Value, "<[^>]+>", "").Trim());
                                //lv1.SubItems.Add(Regex.Replace(a2.Groups[1].Value, "<[^>]+>", "").Trim());
                                j = j + 1;

                                while (zanting == false)
                                {
                                    Application.DoEvents();//等待本次加载完毕才执行下次循环.
                                }
                                if (status == false)
                                    return;

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

        #endregion

        private void 宝钢价格_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread1 = new Thread(new ThreadStart(HRM));
            thread1.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            status = false;
        }
    }
}
