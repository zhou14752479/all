using System;
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
using helper;

namespace 启动程序
{
    public partial class 论坛抓取 : Form
    {
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "TADCID=bjdFx3Rn3p6ys_X2ABQCjnFE8vTET66GHuEzPi7KfVwHepkChn3rpRApldXZCj93wuCZlB5vL-y-6QkGPOSaXOLqrgyYWaDoFfY; TAUnique=%1%enc%3A%2Bn6cLXD5tKm5oykdeM8slV6pCrLgmtvxyTYqwi3TgUnusqy1Uf8ERw%3D%3D; TASSK=enc%3AAMOW3x3HOqGUje9ABam9Y6f53RzW0rLDEGQunOC84Ke3qDEBdAE7GjvyKblvFOntF0F3uq%2B7SvnqLJabWtnoty0%2F0JnksuFx1toM0sJZP9zBnUcGu%2F%2B365tD4jQXfNaCHw%3D%3D; ServerPool=C; TART=%1%enc%3AuaMpHXjPLJVtqjvEr2exZgsRUdvwW4%2FrzdgnCJ8yskdbvh4Qgp%2BhTvRUj5IZVYxjQueMH861kz4%3D; TATravelInfo=V2*A.2*MG.-1*HP.2*FL.3*RS.1; TAReturnTo=%1%%2FShowTopic-g1-i12290-k10842388-Hyatt_Best_Rate_Guarantee_BRG_Scam_2017-Bargain_Travel.html; __gads=ID=7f0bfd853ba07e30:T=1583887597:S=ALNI_MZ9Z5BGcOAtOR1_nv2xuQK4Vt_fUA; OB-USER-TOKEN=951a2ee6-6e27-4186-945a-17fb10a32773; CM=%1%PremiumMobSess%2C%2C-1%7Ct4b-pc%2C%2C-1%7CRestAds%2FRPers%2C%2C-1%7CRCPers%2C%2C-1%7CMobSess%2C1%2C-1%7CWShadeSeen%2C%2C-1%7CTheForkMCCPers%2C%2C-1%7CHomeASess%2C%2C-1%7CPremiumMCSess%2C%2C-1%7CUVOwnersSess%2C%2C-1%7CRestPremRSess%2C%2C-1%7CRepTarMCSess%2C%2C-1%7CCCSess%2C%2C-1%7CCYLSess%2C%2C-1%7CPremRetPers%2C%2C-1%7CViatorMCPers%2C%2C-1%7Csesssticker%2C%2C-1%7CPremiumORSess%2C%2C-1%7Ct4b-sc%2C%2C-1%7CRestAdsPers%2C%2C-1%7CMC_IB_UPSELL_IB_LOGOS2%2C%2C-1%7Cb2bmcpers%2C%2C-1%7CPremMCBtmSess%2C%2C-1%7CMC_IB_UPSELL_IB_LOGOS%2C%2C-1%7CLaFourchette+Banners%2C%2C-1%7Csess_rev%2C%2C-1%7Csessamex%2C%2C-1%7CPremiumRRSess%2C%2C-1%7CTADORSess%2C%2C-1%7CAdsRetPers%2C%2C-1%7CTARSWBPers%2C%2C-1%7CListMCSess%2C%2C-1%7CSPMCSess%2C%2C-1%7CTheForkORSess%2C%2C-1%7CTheForkRRSess%2C%2C-1%7Cpers_rev%2C%2C-1%7CSPACMCSess%2C%2C-1%7CRBAPers%2C%2C-1%7CMobPers%2C%2C-1%7CRestAds%2FRSess%2C%2C-1%7CHomeAPers%2C%2C-1%7CPremiumMobPers%2C%2C-1%7CRCSess%2C%2C-1%7CLaFourchette+MC+Banners%2C%2C-1%7CRestAdsCCSess%2C%2C-1%7CRestPremRPers%2C%2C-1%7CRevHubRMPers%2C%2C-1%7CUVOwnersPers%2C%2C-1%7Csh%2C%2C-1%7Cpssamex%2C%2C-1%7CTheForkMCCSess%2C%2C-1%7CCYLPers%2C%2C-1%7CCCPers%2C%2C-1%7CRepTarMCPers%2C%2C-1%7CShownMobilePopup%2Ctrue%2C-1%7Cb2bmcsess%2C%2C-1%7CSPMCPers%2C%2C-1%7CRevHubRMSess%2C%2C-1%7CPremRetSess%2C%2C-1%7CViatorMCSess%2C%2C-1%7CPremiumMCPers%2C%2C-1%7CAdsRetSess%2C%2C-1%7CPremiumRRPers%2C%2C-1%7CRestAdsCCPers%2C%2C-1%7CTADORPers%2C%2C-1%7CSPACMCPers%2C%2C-1%7CTheForkORPers%2C%2C-1%7CPremMCBtmPers%2C%2C-1%7CTheForkRRPers%2C%2C-1%7CTARSWBSess%2C%2C-1%7CPremiumORPers%2C%2C-1%7CRestAdsSess%2C%2C-1%7CRBASess%2C%2C-1%7CSPORPers%2C%2C-1%7Cperssticker%2C%2C-1%7CListMCPers%2C%2C-1%7C; PAC=AAn6d3kzDNvVNyL_zNakaCUGX_JROOTCmkHRt8kX0UC-KBFrsr_BD7tiKJ4aaadGkL8MJ32SH9I9g3_Zeh1HXDJj-vDT3YbuewEAU5g64tOH3hA4bxVfEKr1ao7Ie5DhyfXoJgMeQowOk6ObYKRzm0s1FAiGQl_yH97ilXQOlybrmpHMW_NXvl7Sb5Ppl5JzbYKPp9zokqMHY9ar4KsehUV8WRRa8JENRxqAVflGglaC6mCSjggZTKyAORT7_i7R0g%3D%3D; PMC=V2*MS.53*MD.20200310*LD.20200311; ak_bmsc=913F57CD00F448067C80E5F0CE2C6F4D172E3005D63A0000D688685ED398E932~plMJtVPHFmW3byXFWLc+idd0UhMPb4JfjGHl99PVjKeUyBcyke/2rtg8yfCEMpn69MMbu+AgFx61SeHW3k3q3ZCa9lLcUMaPUEQD3/34dFJCt1lEID3Qpik3BOXzCAHjyFs37+tJB1Tle4XEbhcbqRU0/4/9R7IbOsdSOKENv2Be/X/RGz7bbj3QLKorzEQOIeDSlOBLeW/jLuCLeRg/MfdsDC5uG+mK0h1NqEn5liSbQ=; roybatty=TNI1625!AN1yDDk3Qw2TRaUHcPTwXrRHTbWv4gD9S5hyatDqZWwHmdKoYcQAzXUBMGhnHOwuCQCjKvT2wEPltRuBIRzR1%2BP90iTn3FwkC5V1VeBLWuIiSXKkPAjVYK4Qe6zW7RaZSWBvcCi4CPpOTSAlUy%2B0pXsYjrkxjwogxv6Q%2FnE%2Fo3hp%2C1; TASession=V2ID.3590108AFCBF20273B794FCD7A62391C*SQ.34*LS.DemandLoadAjax*GR.35*TCPAR.8*TBR.54*EXEX.46*ABTR.57*PHTB.89*FS.56*CPU.39*HS.recommended*ES.popularity*DS.5*SAS.popularity*FPS.oldFirst*FA.1*DF.0*TRA.true*LD.1*EAU._; TAUD=LA-1583887570147-1*RDD-1-2020_03_11*LG-26340799-2.1.F.*LD-26340800-.....; __vt=O2_FKKjnJ6kNyr-dABQCq4R_VSrMTACwWFvfTfL3vwvm544fepIaD5l--5p6mzIGXiTdkPlMuS2LRbX3ZAfaRdnCfUX13-EaNoKvR1plIouho2dR362DDZE7hIulJxVPLq9hEYbjDivivx3HrWFTlGBtv3U";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                //request.Referer = "https://cn.bing.com/search?q=%e9%a6%99%e6%b8%af%e5%85%ad%e5%90%88%e5%bd%a9&qs=n&sp=-1&first=01&FORM=PORE";
              
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.108 Safari/537.36";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("appid:orders");
                //headers.Add("x-nike-visitid:5");
                //headers.Add("x-nike-visitorid:d03393ee-e42c-463e-9235-3ca0491475b4");
                //添加头部
                request.KeepAlive = false;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
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
        public 论坛抓取()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {

            try
            {
                string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string url in text)
                {

                    for (int j = 0; j < 100; j++)
                    {
                        string URL = url.Replace("-Hyatt", "-o"+j+"0-Hyatt");
                        string html = GetUrl(URL, "utf-8");
                       
                        MatchCollection ahtmls = Regex.Matches(html, @"<div class=""postBadge badge"">([\s\S]*?)<div class='postBody'>");
                        if (ahtmls.Count==0)
                            break;
                        MatchCollection persons = Regex.Matches(html, @"Person""\,""name"":""([\s\S]*?)""");

                        Match title = Regex.Match(html, @"""text"":""([\s\S]*?)"",""");
                        MatchCollection bodys = Regex.Matches(html, @"""Answer"",""text"":""([\s\S]*?)"",""");



                        for (int i = 0; i < ahtmls.Count; i++)
                        {
                            try
                            {

                                MatchCollection posts = Regex.Matches(ahtmls[i].Groups[1].Value, @"<span class=""badgeText"">([\s\S]*?) ");

                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add(Regex.Replace(persons[i].Groups[1].Value, "<[^>]+>", "").Trim());
                                if (i == 0)
                                {
                                    lv1.SubItems.Add(Regex.Replace(title.Groups[1].Value, "<[^>]+>", "").Trim());
                                }
                                else
                                {
                                    lv1.SubItems.Add(bodys[i - 1].Groups[1].Value.Trim());
                                }
                                lv1.SubItems.Add(Regex.Replace(posts[0].Groups[1].Value, "<[^>]+>", "").Trim());
                                lv1.SubItems.Add(Regex.Replace(posts[1].Groups[1].Value, "<[^>]+>", "").Trim());
                                lv1.SubItems.Add(Regex.Replace(posts[2].Groups[1].Value, "<[^>]+>", "").Trim());


                            }
                            catch
                            {

                                continue;
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
        private void 论坛抓取_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread1 = new Thread(new ThreadStart(run));
            thread1.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
