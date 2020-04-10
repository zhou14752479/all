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

        string month = "";
        #region HRB
        public void HRB()
        {

            string pinzhong = "HRB";
            ArrayList liushuis = getliushui(@"HRB','([\s\S]*?)'");
            string[] kuandus = {"700-899", "900-999", "1000-1800", "1801-2100", };
            try
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
                                lv1.SubItems.Add(pinzhong+"宝钢股份热轧");
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
                                lv1.SubItems.Add(pinzhong+"青山基地热轧");
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
            ArrayList liushuis = getliushui(@"'HRM','([\s\S]*?)'");

           
            try
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
                        lists.Add("1.50~1.79");
                        for (int i = 1; i < houdus.Count; i++)
                        { 
                
                            lists.Add(houdus[i].Groups[1].Value);
                        }
                        int j = 0;


                        for (int i = 0; i < lists.Count; i++)
                        {
                            foreach (Match kuandu in kuandus)
                            {

                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add(month);
                                lv1.SubItems.Add(pinzhong+"梅山、东山基地热轧");
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
            

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        #endregion


        #region M00
        public void M00()
        {

            string pinzhong = "M00";
            ArrayList liushuis = getliushui(@"'M00','([\s\S]*?)'");
            string[] kuandus = { "600-799", "800-999", "1000-1199", "1200-1499", "1500-1699", "1700-1850", "1851~ ", };

            try
            {
               
                    foreach (string liushui in liushuis)
                    {
                        string url = "http://ecommerce.ibaosteel.com/baosteel_csm/ne/productPriceQueryMainCas.cas?functionCode=productPriceQueryMainCas&month=" + month + "&prodCode=" + pinzhong + "&seqNum=" + liushui;

                        string html = method.GetUrl(url, "GBK");

                        Match paihao = Regex.Match(html, @">牌号：([\s\S]*?)</td>");

                        MatchCollection houdus = Regex.Matches(html, @"<td rowspan=2 height=([\s\S]*?)>([\s\S]*?)</td>");
                        MatchCollection prices = Regex.Matches(html, @"border-left:none' x:num>([\s\S]*?)</td>");


                       

                        ArrayList lists = new ArrayList();
                       
                        for (int i = 0; i < houdus.Count; i++)
                        {

                            lists.Add(houdus[i].Groups[2].Value);
                        }
                        


                        for (int i = 0; i < lists.Count; i++)
                        {
                            int j = 0;
                            foreach (string kuandu in kuandus)
                            {
                               
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add(month);
                                lv1.SubItems.Add(pinzhong+"热镀锌");
                                lv1.SubItems.Add(liushui);
                                lv1.SubItems.Add("DC51D+Z、CR1、DX51D、SGCC、St01Z、St02Z、St03Z ");
                                lv1.SubItems.Add(lists[i].ToString());
                                lv1.SubItems.Add(kuandu);
                                lv1.SubItems.Add(prices[14*i+j].Groups[1].Value.Trim());
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
            

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #region ML0
        public void ML0()
        {

            string pinzhong = "ML0";
            ArrayList liushuis = getliushui(@"'ML0','([\s\S]*?)'");
            string[] kuandus = { "700-799", "800-899", "900-999", "1000-1199", "1200-1300" };

            try
            {
              
                    foreach (string liushui in liushuis)
                    {
                        string url = "http://ecommerce.ibaosteel.com/baosteel_csm/ne/productPriceQueryMainCas.cas?functionCode=productPriceQueryMainCas&month=" + month + "&prodCode=" + pinzhong + "&seqNum=" + liushui;

                        string html = method.GetUrl(url, "GBK");

                        Match paihao = Regex.Match(html, @">牌号：([\s\S]*?)</td>");

                        MatchCollection houdus = Regex.Matches(html, @"<td rowspan=2 height=([\s\S]*?)>([\s\S]*?)</td>");
                        MatchCollection prices = Regex.Matches(html, @"border-left:none' x:num=""([\s\S]*?)"">([\s\S]*?)</td>");




                        ArrayList lists = new ArrayList();

                        for (int i = 0; i < houdus.Count; i++)
                        {

                            lists.Add(houdus[i].Groups[2].Value);
                        }



                        for (int i = 0; i < lists.Count; i++)
                        {
                            int j = 0;
                            foreach (string kuandu in kuandus)
                            {

                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add(month);
                                lv1.SubItems.Add(pinzhong+"镀铝锌");
                                lv1.SubItems.Add(liushui);
                                lv1.SubItems.Add("DC51D＋AZ   (Q/BQB 425-2009) ");
                                lv1.SubItems.Add(lists[i].ToString());
                                lv1.SubItems.Add(kuandu);
                                lv1.SubItems.Add(prices[20 * i + 2*j].Groups[2].Value.Trim());
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
            

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        #endregion







        #region N00
        public void N00()
        {

            string pinzhong = "N00";
            ArrayList liushuis = getliushui(@"'N00','([\s\S]*?)'");
            string[] kuandus = { "700-899", "900-1399", "1400-1599", "1600-1850"};

            try
            {
               
                    foreach (string liushui in liushuis)
                    {
                        string url = "http://ecommerce.ibaosteel.com/baosteel_csm/ne/productPriceQueryMainCas.cas?functionCode=productPriceQueryMainCas&month=" + month + "&prodCode=" + pinzhong + "&seqNum=" + liushui;

                        string html = method.GetUrl(url, "GBK");

                        Match paihao = Regex.Match(html, @">牌号：([\s\S]*?)</td>");

                        MatchCollection houdus = Regex.Matches(html, @"<td rowspan=2 height=([\s\S]*?)>([\s\S]*?)</td>");
                        MatchCollection prices = Regex.Matches(html, @"border-left:none' x:num>([\s\S]*?)</td>");




                        ArrayList lists = new ArrayList();

                        for (int i = 0; i < houdus.Count; i++)
                        {

                            lists.Add(houdus[i].Groups[2].Value);
                        }



                        for (int i = 0; i < lists.Count; i++)
                        {
                            int j = 0;
                            foreach (string kuandu in kuandus)
                            {

                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add(month);
                                lv1.SubItems.Add(pinzhong+"电镀锌");
                                lv1.SubItems.Add(liushui);
                                lv1.SubItems.Add("SECC,BLCE+Z,DC01E+Z,CR1 ");
                                lv1.SubItems.Add(lists[i].ToString());
                                lv1.SubItems.Add(kuandu);
                                lv1.SubItems.Add(prices[8* i +  j].Groups[1].Value.Trim());
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
            

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        #endregion


        #region O00
        public void O00()
        {

            string pinzhong = "O00";
            ArrayList liushuis = getliushui(@"'O00','([\s\S]*?)'");
            string[] kuandus = { "600-699", "700-799", "800-899", "900-999", "1000-1199", "1200-1299", "1300-1500" };

            try
            {
              
                    foreach (string liushui in liushuis)
                    {
                        string url = "http://ecommerce.ibaosteel.com/baosteel_csm/ne/productPriceQueryMainCas.cas?functionCode=productPriceQueryMainCas&month=" + month + "&prodCode=" + pinzhong + "&seqNum=" + liushui;

                        string html = method.GetUrl(url, "GBK");

                        Match paihao = Regex.Match(html, @">牌号：([\s\S]*?)</td>");

                        MatchCollection houdus = Regex.Matches(html, @"<td rowspan=2 height=([\s\S]*?)>([\s\S]*?)</td>");
                        MatchCollection prices = Regex.Matches(html, @"border-left:none' x:num=""([\s\S]*?)"">([\s\S]*?)</td>");




                        ArrayList lists = new ArrayList();

                        for (int i = 0; i < houdus.Count; i++)
                        {

                            lists.Add(houdus[i].Groups[2].Value);
                        }



                        for (int i = 0; i < lists.Count; i++)
                        {
                            int j = 0;
                            foreach (string kuandu in kuandus)
                            {

                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add(month);
                                lv1.SubItems.Add(pinzhong + "彩涂");
                                lv1.SubItems.Add(liushui);
                                lv1.SubItems.Add("TDC51D+Z   (Q/BQB 440-2009) ");
                                lv1.SubItems.Add(lists[i].ToString());
                                lv1.SubItems.Add(kuandu);
                                lv1.SubItems.Add(prices[28 * i + 2 * j].Groups[2].Value.Trim());
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

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        #endregion


        #region S00
        public void S00()
        {

            string pinzhong = "S00";
            ArrayList liushuis = getliushui(@"'S00','([\s\S]*?)'");
            string[] kuandus = { "800-899", "900-999", "1000-1099", "1100-1199", "1200-1250", "1251-1280" };

            try
            {
               
                    foreach (string liushui in liushuis)
                    {
                        string url = "http://ecommerce.ibaosteel.com/baosteel_csm/ne/productPriceQueryMainCas.cas?functionCode=productPriceQueryMainCas&month=" + month + "&prodCode=" + pinzhong + "&seqNum=" + liushui;

                        string html = method.GetUrl(url, "GBK");

                        MatchCollection paihao = Regex.Matches(html, @"width:51pt'>B([\s\S]*?)</td>");

                        
                        MatchCollection prices = Regex.Matches(html, @"border-left:none' x:num>([\s\S]*?)</td>");




                        ArrayList lists = new ArrayList();

                        for (int i = 0; i < paihao.Count; i++)
                        {

                            lists.Add(paihao[i].Groups[1].Value);
                        }



                        for (int i = 0; i < lists.Count; i++)
                        {
                            int j = 0;
                            foreach (string kuandu in kuandus)
                            {

                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add(month);
                                lv1.SubItems.Add(pinzhong + "无取向电工钢");
                                lv1.SubItems.Add(liushui );
                                lv1.SubItems.Add("B"+lists[i].ToString()); //牌号
                                lv1.SubItems.Add("无");  //厚度
                                lv1.SubItems.Add(kuandu);
                                lv1.SubItems.Add(prices[6* i +  j].Groups[1].Value.Trim());
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
            

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #region S0C
        public void S0C()
        {

            string pinzhong = "S0C";
            ArrayList liushuis = getliushui(@"'S0C','([\s\S]*?)'");
            string[] kuandus = { "800-899", "900-999", "1000-1099", "1100-1199", "1200-1250", "1251-1280" };

            try
            {
               
                    foreach (string liushui in liushuis)
                    {
                       
                        string url = "http://ecommerce.ibaosteel.com/baosteel_csm/ne/productPriceQueryMainCas.cas?functionCode=productPriceQueryMainCas&month=" + month + "&prodCode=" + pinzhong + "&seqNum=" + liushui;
                       
                        string html = method.GetUrl(url, "GBK");

                        MatchCollection paihao = Regex.Matches(html, @"width:50pt'>([\s\S]*?)</td>");


                        MatchCollection prices = Regex.Matches(html, @"border-left:none' x:num>([\s\S]*?)</td>");


                        MessageBox.Show(paihao.Count.ToString());

                        ArrayList lists = new ArrayList();

                        for (int i = 2; i < paihao.Count-2; i++)
                        {

                            lists.Add(paihao[i].Groups[1].Value);
                        }



                        for (int i = 0; i < lists.Count; i++)
                        {
                            int j = 0;
                            foreach (string kuandu in kuandus)
                            {

                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add(month);
                                lv1.SubItems.Add(pinzhong + "青山基地无取向电工钢");
                                lv1.SubItems.Add(liushui);
                                lv1.SubItems.Add( lists[i].ToString()); //牌号
                                lv1.SubItems.Add("无");  //厚度
                                lv1.SubItems.Add(kuandu);
                                lv1.SubItems.Add(prices[6 * i + j].Groups[1].Value.Trim());
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
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"baogangjiage"))
            {
                month = textBox1.Text.Trim();
                button1.Enabled = false;
                status = true;
                if (checkBox1.Checked == true)
                {
                    Thread thread1 = new Thread(new ThreadStart(HRB));
                    thread1.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
                if (checkBox2.Checked == true)
                {
                    Thread thread1 = new Thread(new ThreadStart(HRC));
                    thread1.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
                if (checkBox3.Checked == true)
                {
                    Thread thread1 = new Thread(new ThreadStart(HRM));
                    thread1.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
                if (checkBox5.Checked == true)
                {
                    Thread thread1 = new Thread(new ThreadStart(M00));
                    thread1.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
                if (checkBox6.Checked == true)
                {
                    Thread thread1 = new Thread(new ThreadStart(ML0));
                    thread1.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }

                if (checkBox7.Checked == true)
                {
                    Thread thread1 = new Thread(new ThreadStart(N00));
                    thread1.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }

                if (checkBox8.Checked == true)
                {
                    Thread thread1 = new Thread(new ThreadStart(O00));
                    thread1.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }

                if (checkBox9.Checked == true)
                {
                    Thread thread1 = new Thread(new ThreadStart(S00));
                    thread1.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }

                if (checkBox10.Checked == true)
                {
                    Thread thread1 = new Thread(new ThreadStart(S0C));
                    thread1.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }

            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
           
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            status = false;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = true; ;
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
