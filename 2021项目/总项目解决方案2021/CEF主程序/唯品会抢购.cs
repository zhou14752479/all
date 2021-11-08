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

namespace CEF主程序
{
    public partial class 唯品会抢购 : Form
    {
        public 唯品会抢购()
        {
            InitializeComponent();
        }

        string spuId = "";
        string stock = "";
        string vSkuId = "";


        List<string> vidlist = new List<string>();

        public void getinfo()
        {
            if(comboBox1.Text=="")
            {
                MessageBox.Show("请先选择尺码");
                return;
            }
            try
            {

                string mid= Regex.Match(textBox1.Text, @"\d{15,}").Groups[0].Value;
                string brandid= Regex.Match(textBox1.Text, @"detail-([\s\S]*?)-").Groups[1].Value;
                string url = "https://mapi.vip.com/vips-mobile/rest/shopping/wap2/vendorSkuList/v4?app_name=shop_wap&app_version=4.0&api_key=8cec5243ade04ed3a02c5972bcda0d3f&mobile_platform=2&source_app=yd_wap&warehouse=VIP_NH&fdc_area_id=104104101&province_id=104104&mars_cid=1635604234160_a30c4d60039dffe8c915c82669aa5881&mobile_channel=mobiles-%7C%7C&standby_id=nature&vendorProductId=&mid="+mid+"&brandid="+brandid+"&device=3&salePriceVer=2&functions=svipAsSalePrice%2CfallingInfo%2Cannouncement%2CmidSupportServices%2CuserContext%2CbuyLimit%2CforeShowActive%2CpanelView%2CfuturePriceView%2CshowSingleColor%2CnoProps%2Csku_price%2Cactive_price%2Cprepay_sku_price%2Creduced_point_desc%2CsurprisePrice%2CbusinessCode%2CpromotionTips%2Cinvisible%2Cflash_sale_stock%2CrestrictTips%2CfavStatus%2CbanInfo%2CfuturePrice%2CpriceChart%2CpriceView%2CquotaInfo%2CexclusivePrice%2CextraDetailImages%2CfloatingView&prepayMsgType=1&promotionTipsVer=5&priceViewVer=8&supportSquare=1&panelViewVer=2&isUseMultiColor=1&couponInfoVer=2&freightTipsVer=3&serviceTagVer=1&supportAllPreheatTipsTypes=1&salePriceTypeVer=2&wap_consumer=&_=1636080246";
                string price = "";
              
                string color = "";
                string html = method.GetUrl(url,"utf-8");






                //获取款式颜色

                MatchCollection productIds = Regex.Matches(html, @"""productId"":""([\s\S]*?)""");
                MatchCollection names = Regex.Matches(html, @"},""id"":([\s\S]*?)name"":""([\s\S]*?)""");

                for (int i = 0; i < names.Count; i++)
                {
                    if(productIds[i].Groups[1].Value==mid)
                    {
                        color = names[i].Groups[2].Value;
                    }

                }
                ///获取颜色结束


            

                //textBox3.Text = html;
                string ahtml = Regex.Match(html, @"""skus""([\s\S]*?)prefer_trans").Groups[1].Value;
                MatchCollection prices = Regex.Matches(ahtml, @"""finalPrice"":{""price"":""([\s\S]*?)""");
                MatchCollection productCouponKey = Regex.Matches(ahtml, @"""mid"":""([\s\S]*?)""");

               MatchCollection vids = Regex.Matches(ahtml, @";\d{2,}:\d{2,}");
                MatchCollection vSkuIds = Regex.Matches(ahtml, @"""vSkuId"":""([\s\S]*?)""");

                for (int a= 0; a < vids.Count; a++)
                {
                    string[] text = vids[a].Groups[0].Value.Split(new string[] { ":" }, StringSplitOptions.None);
                  
                    if(text[1]== vidlist[comboBox1.SelectedIndex])
                    {
                        getchima_id_stock(vSkuIds[a].Groups[1].Value);
                    }
                }
                




                string title = Regex.Match(html, @"""longTitle"":""([\s\S]*?)""").Groups[1].Value;

                
                for (int i = 0; i < productCouponKey.Count; i++)
                {
                    if(productCouponKey[i].Groups[1].Value==mid)
                    {
                        price = prices[i].Groups[1].Value;
                      

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(title);
                        lv1.SubItems.Add(color);
                        lv1.SubItems.Add(vSkuId);
                        lv1.SubItems.Add(price);
                        lv1.SubItems.Add(stock);
                        lv1.SubItems.Add(textBox2.Text);
                        lv1.SubItems.Add(DateTime.Now.ToLongTimeString());
                        lv1.SubItems.Add(brandid);
                        lv1.SubItems.Add(spuId);
                        return;
                    }

                }
                MessageBox.Show("此商品已下架或不存在");
            }


            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }


        public void  run()
        {

            try
            {
                for (int i = 0; i <listView1.Items.Count; i++)
                {
                    string brandid = listView1.Items[i].SubItems[8].Text;
                    string spuId = listView1.Items[i].SubItems[9].Text;
                    string skuid= listView1.Items[i].SubItems[3].Text;

                   
                    string url = "https://mapi-rp.vip.com/vips-mobile/rest/shop/goods/stock/singleSpu/get?app_name=shop_wap&app_version=4.0&api_key=8cec5243ade04ed3a02c5972bcda0d3f&mobile_platform=2&source_app=yd_wap&warehouse=VIP_NH&fdc_area_id=104104101&province_id=104104&mars_cid=1635604234160_a30c4d60039dffe8c915c82669aa5881&mobile_channel=mobiles-%7C%7C&standby_id=nature&brand_id=" + brandid + "&spu_id=" + spuId + "&_=1636163278";

                    string html = method.GetUrl(url, "utf-8");
                    string ahtml = Regex.Match(html, @"""sizes""([\s\S]*?)\]").Groups[1].Value;
                    MatchCollection v_sku_ids = Regex.Matches(ahtml, @"""v_sku_id"":""([\s\S]*?)""");
                    MatchCollection stocks = Regex.Matches(ahtml, @"""stock"":([\s\S]*?),");
                    for (int j = 0;j < v_sku_ids.Count; j++)
                    {
                        if (v_sku_ids[j].Groups[1].Value == skuid)
                        {
                       
                           string  stock = stocks[i].Groups[1].Value;
                            listView1.Items[i].SubItems[5].Text = stock;
                            listView1.Items[i].SubItems[7].Text = DateTime.Now.ToLongTimeString(); 
                        }

                    }

                }
            }
            catch (Exception ex)
            {

                ex.ToString();
            }
        }
        private void 唯品会抢购_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            getinfo();
        }

        
        public void getchima()
        {
            vidlist.Clear();
            comboBox1.Items.Clear();
            //获取尺码
            string mid = Regex.Match(textBox1.Text, @"\d{15,}").Groups[0].Value;
            string brandid = Regex.Match(textBox1.Text, @"detail-([\s\S]*?)-").Groups[1].Value;
            string url = "https://mapi.vip.com/vips-mobile/rest/shopping/wap2/vendorSkuList/v4?app_name=shop_wap&app_version=4.0&api_key=8cec5243ade04ed3a02c5972bcda0d3f&mobile_platform=2&source_app=yd_wap&warehouse=VIP_NH&fdc_area_id=104104101&province_id=104104&mars_cid=1635604234160_a30c4d60039dffe8c915c82669aa5881&mobile_channel=mobiles-%7C%7C&standby_id=nature&vendorProductId=&mid=" + mid + "&brandid=" + brandid + "&device=3&salePriceVer=2&functions=svipAsSalePrice%2CfallingInfo%2Cannouncement%2CmidSupportServices%2CuserContext%2CbuyLimit%2CforeShowActive%2CpanelView%2CfuturePriceView%2CshowSingleColor%2CnoProps%2Csku_price%2Cactive_price%2Cprepay_sku_price%2Creduced_point_desc%2CsurprisePrice%2CbusinessCode%2CpromotionTips%2Cinvisible%2Cflash_sale_stock%2CrestrictTips%2CfavStatus%2CbanInfo%2CfuturePrice%2CpriceChart%2CpriceView%2CquotaInfo%2CexclusivePrice%2CextraDetailImages%2CfloatingView&prepayMsgType=1&promotionTipsVer=5&priceViewVer=8&supportSquare=1&panelViewVer=2&isUseMultiColor=1&couponInfoVer=2&freightTipsVer=3&serviceTagVer=1&supportAllPreheatTipsTypes=1&salePriceTypeVer=2&wap_consumer=&_=1636080246";
           
            string html = method.GetUrl(url, "utf-8");
            
            MatchCollection sizenames = Regex.Matches(html, @"detailImages"":\[\]([\s\S]*?)name"":""([\s\S]*?)""([\s\S]*?)vid"":""([\s\S]*?)""");
            spuId = Regex.Match(html, @"""spuId"":""([\s\S]*?)""").Groups[1].Value;
            for (int i = 0; i < sizenames.Count; i++)
            {
                comboBox1.Items.Add(sizenames[i].Groups[2].Value);
                vidlist.Add(sizenames[i].Groups[4].Value);
            }
            ///获取尺码结束

        }


        public void getchima_id_stock(string skuid)
        {

            string mid = Regex.Match(textBox1.Text, @"\d{15,}").Groups[0].Value;
            string brandid = Regex.Match(textBox1.Text, @"detail-([\s\S]*?)-").Groups[1].Value;
            string url = "https://mapi-rp.vip.com/vips-mobile/rest/shop/goods/stock/singleSpu/get?app_name=shop_wap&app_version=4.0&api_key=8cec5243ade04ed3a02c5972bcda0d3f&mobile_platform=2&source_app=yd_wap&warehouse=VIP_NH&fdc_area_id=104104101&province_id=104104&mars_cid=1635604234160_a30c4d60039dffe8c915c82669aa5881&mobile_channel=mobiles-%7C%7C&standby_id=nature&brand_id="+brandid+"&spu_id="+spuId+"&_=1636163278";

            string html = method.GetUrl(url, "utf-8");
            string ahtml = Regex.Match(html, @"""sizes""([\s\S]*?)\]").Groups[1].Value;
            MatchCollection v_sku_ids = Regex.Matches(ahtml, @"""v_sku_id"":""([\s\S]*?)""");
            MatchCollection stocks = Regex.Matches(ahtml, @"""stock"":([\s\S]*?),");
            for (int i = 0; i < v_sku_ids.Count; i++)
            {
                if(v_sku_ids[i].Groups[1].Value==skuid)
                {
                    vSkuId = v_sku_ids[i].Groups[1].Value;
                    stock = stocks[i].Groups[1].Value;
                }

            }
           
        

        }
        private void comboBox1_Click(object sender, EventArgs e)
        {
            getchima();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
        }


        Thread thread;
        private void button2_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"fRdukY"))
            {

                return;
            }



            #endregion

            timer1.Start();
            timer1.Interval = 2000;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            药师帮 login = new 药师帮();
            login.Show();
        }
    }
}
