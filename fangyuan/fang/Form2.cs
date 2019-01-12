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

namespace fang
{
    public partial class Form2 : Form
    {
        string city { get; set; }
        string cityName { get; set; }

        string cookie { get; set; }

        public Form2()
        {
            InitializeComponent();
        }
        #region  链家网
        public void lianjia()
        {


            try
            {


                if (radioButton1.Checked == true)
                {
                    this.city ="bj";
                    this.cityName = "北京";
                    this.cookie = "UM_distinctid=16565884475100-0199d2e8c28032-37664109-1fa400-16565884476234; gr_user_id=0654b09d-02dc-4d28-aef7-db257832338a; HF_CITY_PINYIN=8ieAYXD6mDJxO1YvR89T1A%3d%3d; HF_CITY_PINYIN_SIGN=4b0c1f017e276cfcc9ed1e0d683db8f3; VerifyToken=e385080a01194ce3bca987d690e3410c; CNZZDATA1263082085=1948197884-1535005247-%7C1536794656; Hm_lvt_1799f23f2766063ed058651d23d7543d=1535283874,1535333605,1535682839,1536795715; gr_session_id_9d5a50541bffa263=6a078d08-5e46-4dc6-9025-1821c36383f0; gr_session_id_9d5a50541bffa263_6a078d08-5e46-4dc6-9025-1821c36383f0=true; HF_PROVINCE_GUID=7T7KrG0a9wGASMhu0NBqi33gF26jU9PeUX40OjnWQZ8sDoK5%2bM3l8dTGk53Kgsvx; HF_PROVINCE_GUID_SIGN=c87bcfd49ec55a13bfa8b5373be6949d; HF_CITY_GUID=DekvKOeX1MoAODFihTZ8gcZygqv3ZQ9jKXZ4O%2b5SozCF3R8H0BeEVGS9fE0Xvrqn; HF_CITY_GUID_SIGN=415d64bc813471ce88ea69b4039d1b36; HF_CITY_NAME=13oE%2fbIOw7tzFdK7KhPSuA%3d%3d; HF_CITY_NAME_SIGN=2fe4bf63fcddde74647881b3224957c7; Hm_lpvt_1799f23f2766063ed058651d23d7543d=1536795720";
                }
                if (radioButton2.Checked == true)
                {
                    this.city = "sh";
                    this.cityName = "上海";
                    this.cookie = "UM_distinctid=16565884475100-0199d2e8c28032-37664109-1fa400-16565884476234; gr_user_id=0654b09d-02dc-4d28-aef7-db257832338a; HF_CITY_PINYIN=8ieAYXD6mDJxO1YvR89T1A%3d%3d; HF_CITY_PINYIN_SIGN=4b0c1f017e276cfcc9ed1e0d683db8f3; VerifyToken=e385080a01194ce3bca987d690e3410c; CNZZDATA1263082085=1948197884-1535005247-%7C1536794656; Hm_lvt_1799f23f2766063ed058651d23d7543d=1535283874,1535333605,1535682839,1536795715; gr_session_id_9d5a50541bffa263=6a078d08-5e46-4dc6-9025-1821c36383f0; gr_session_id_9d5a50541bffa263_6a078d08-5e46-4dc6-9025-1821c36383f0=true; HF_PROVINCE_GUID=IluM9UBXcm0QF30T6FDbTdNL%2fXSU786Qgxupy7m%2fJ7vHgkKk0oDGnu2bRn8x9vdW; HF_PROVINCE_GUID_SIGN=b969671400a2f964bdbfde7ae80aded5; HF_CITY_GUID=x4cne32tXS9P%2bJB5%2fWdWi01J%2fYV9ez%2f9jCvbN2YzEoI3pI%2bM4mgj6%2fWEJbFGkrLc; HF_CITY_GUID_SIGN=ae875dcc74844869783bfe9dabf08072; HF_CITY_NAME=8fMmHVQT3KzUrRc7Z8foiA%3d%3d; HF_CITY_NAME_SIGN=e51d6d95833778ec296918e7922e26b0; Hm_lpvt_1799f23f2766063ed058651d23d7543d=1536796986";
                }
                if (radioButton3.Checked == true)
                {
                    this.city = "sz";
                    this.cityName = "深圳";
                    this.cookie = "UM_distinctid=16565884475100-0199d2e8c28032-37664109-1fa400-16565884476234; gr_user_id=0654b09d-02dc-4d28-aef7-db257832338a; HF_CITY_PINYIN=8ieAYXD6mDJxO1YvR89T1A%3d%3d; HF_CITY_PINYIN_SIGN=4b0c1f017e276cfcc9ed1e0d683db8f3; VerifyToken=e385080a01194ce3bca987d690e3410c; CNZZDATA1263082085=1948197884-1535005247-%7C1536794656; Hm_lvt_1799f23f2766063ed058651d23d7543d=1535283874,1535333605,1535682839,1536795715; gr_session_id_9d5a50541bffa263=6a078d08-5e46-4dc6-9025-1821c36383f0; gr_session_id_9d5a50541bffa263_6a078d08-5e46-4dc6-9025-1821c36383f0=true; HF_PROVINCE_GUID=X77sbvARFH%2bBXZjhagYf%2fD8pmyt6ywGuhnILl7Crmr9GOVV5cRnJj7zhBgVGJlF2; HF_PROVINCE_GUID_SIGN=7cea0ba34a470a9d76e292a377a3323b; HF_CITY_GUID=%2bhB%2bMbTuqs3TaoVPBO1AcbVHEa8rQzt8swR2fUzwfmKtaE0L26695vDiSSIxHn5Y; HF_CITY_GUID_SIGN=8933acc894f8d0ad1c33256b3fc0e53a; HF_CITY_NAME=Ipk5JqnFlvcekc%2fl0Wzy6A%3d%3d; HF_CITY_NAME_SIGN=1abce4e38d65db11e6549e2210b0ba4a; Hm_lpvt_1799f23f2766063ed058651d23d7543d=1536797067";
                }
                if (radioButton4.Checked == true)
                {
                    this.city = "hz";
                    this.cityName = "杭州";
                    this.cookie = "UM_distinctid=16565884475100-0199d2e8c28032-37664109-1fa400-16565884476234; gr_user_id=0654b09d-02dc-4d28-aef7-db257832338a; HF_CITY_PINYIN=8ieAYXD6mDJxO1YvR89T1A%3d%3d; HF_CITY_PINYIN_SIGN=4b0c1f017e276cfcc9ed1e0d683db8f3; VerifyToken=e385080a01194ce3bca987d690e3410c; CNZZDATA1263082085=1948197884-1535005247-%7C1536794656; Hm_lvt_1799f23f2766063ed058651d23d7543d=1535283874,1535333605,1535682839,1536795715; gr_session_id_9d5a50541bffa263=6a078d08-5e46-4dc6-9025-1821c36383f0; gr_session_id_9d5a50541bffa263_6a078d08-5e46-4dc6-9025-1821c36383f0=true; HF_PROVINCE_GUID=ByB%2bDMCFn%2bKzQ1piSUyAh5%2f%2b2Sr4EM8SxXRQ6v41e%2fuFjJZPZid7HQT1%2fHlpIRN4; HF_PROVINCE_GUID_SIGN=a8b8b9fa5c2460ec75c05e52e7986b57; HF_CITY_GUID=i75Ft%2fot%2f3GSnuJc1uXTOPPIDRED%2bDU1DPN5QSXgvhroVKf52vyyaKVxg9i%2fuCgI; HF_CITY_GUID_SIGN=a6e975a707fbcbccbb1cf4cab65abc77; HF_CITY_NAME=qALolmlE1N4KivYc0kV3Zg%3d%3d; HF_CITY_NAME_SIGN=2473d5aa144091b4865143ee33eba44f; Hm_lpvt_1799f23f2766063ed058651d23d7543d=1536797101";
                }
                if (radioButton5.Checked == true)
                {
                    this.city = "sy";
                    this.cityName = "沈阳";
                    this.cookie = "UM_distinctid=16565884475100-0199d2e8c28032-37664109-1fa400-16565884476234; gr_user_id=0654b09d-02dc-4d28-aef7-db257832338a; HF_CITY_PINYIN=8ieAYXD6mDJxO1YvR89T1A%3d%3d; HF_CITY_PINYIN_SIGN=4b0c1f017e276cfcc9ed1e0d683db8f3; VerifyToken=e385080a01194ce3bca987d690e3410c; CNZZDATA1263082085=1948197884-1535005247-%7C1536794656; Hm_lvt_1799f23f2766063ed058651d23d7543d=1535283874,1535333605,1535682839,1536795715; gr_session_id_9d5a50541bffa263=6a078d08-5e46-4dc6-9025-1821c36383f0; gr_session_id_9d5a50541bffa263_6a078d08-5e46-4dc6-9025-1821c36383f0=true; HF_PROVINCE_GUID=Ev0MpOha8JWZMoMvE77yJabKlcYpNagxnuoAyhQpiT3SH4w9F1xrHlmhnu5p6Lmy; HF_PROVINCE_GUID_SIGN=3006b01dcbb1f587f1b0f899b895bdd1; HF_CITY_GUID=CQu3LR7UsOiu9w7XXhYSOGYFVfgCSv8bda0%2fKgEX70s%2b1FsZAiiBs%2bKUaz70zHsY; HF_CITY_GUID_SIGN=0a2271ca411f61add742edee58093634; HF_CITY_NAME=vj7ncIUrHd0tYZeshzwdAw%3d%3d; HF_CITY_NAME_SIGN=d79b3dfdd81cf2708869a1b0fb488d48; Hm_lpvt_1799f23f2766063ed058651d23d7543d=1536797136";
                }
                if (radioButton6.Checked == true)
                {
                    this.city = "cq";
                    this.cityName = "重庆";
                    this.cookie = "UM_distinctid=16565884475100-0199d2e8c28032-37664109-1fa400-16565884476234; gr_user_id=0654b09d-02dc-4d28-aef7-db257832338a; HF_CITY_PINYIN=8ieAYXD6mDJxO1YvR89T1A%3d%3d; HF_CITY_PINYIN_SIGN=4b0c1f017e276cfcc9ed1e0d683db8f3; VerifyToken=e385080a01194ce3bca987d690e3410c; CNZZDATA1263082085=1948197884-1535005247-%7C1536794656; Hm_lvt_1799f23f2766063ed058651d23d7543d=1535283874,1535333605,1535682839,1536795715; gr_session_id_9d5a50541bffa263=6a078d08-5e46-4dc6-9025-1821c36383f0; gr_session_id_9d5a50541bffa263_6a078d08-5e46-4dc6-9025-1821c36383f0=true; HF_PROVINCE_GUID=FtoigvXB5EuKhQgoHcEtLsgGiClmZohoykyIQADlaA3A9hjOEcndXLeWYgI3MjFb; HF_PROVINCE_GUID_SIGN=72333ab4698679e7f74213904dc41c7e; HF_CITY_GUID=snTCaWFTIo5K9TFs0nXFB5ubePNhZyo7mZT9%2f1VbkQnYNN6cS3VJC%2bkj%2bG6J67nx; HF_CITY_GUID_SIGN=4bb64e42700869fdc304ebe8125d88f3; HF_CITY_NAME=lWfEOF%2fTfWOD0JG1FRKFRA%3d%3d; HF_CITY_NAME_SIGN=d4c1d3466e29f9e3eadf9d24c0bef9aa; Hm_lpvt_1799f23f2766063ed058651d23d7543d=1536797171";
                }



                for (int i = 1; i < 31; i++)
                {
                    String Url = "https://" + city + ".lianjia.com/xiaoqu/pg" + i + "/";

                    string strhtml = method.GetUrl(Url, "utf-8");

                    string rxg1 = @"resblock_name=""([\s\S]*?)""";
                    string rxg2 = @"<span class=""houseIcon""></span>([\s\S]*?)>([\s\S]*?)</a>";
                    string rxg3 = @"class=""district"" title=""([\s\S]*?)"">([\s\S]*?)</a>";
                    string rxg4 = @"class=""bizcircle"" title=""([\s\S]*?)""";
                    string rxg5 = @"<div class=""totalPrice""><span>([\s\S]*?)</span>";
                    string rxg6 = @"class=""totalSellCount""><span>([\s\S]*?)</span>";




                    string total = @"""ljweb_el"": ""([\s\S]*?)""";

                    MatchCollection xiaoqu = Regex.Matches(strhtml, rxg1);
                    MatchCollection yishou = Regex.Matches(strhtml, rxg2);
                    MatchCollection quyu = Regex.Matches(strhtml, rxg3);
                    MatchCollection addr = Regex.Matches(strhtml, rxg4);
                    MatchCollection jiage = Regex.Matches(strhtml, rxg5);
                    MatchCollection zaishou = Regex.Matches(strhtml, rxg6);


                    Match Total = Regex.Match(strhtml, total);

                    for (int j = 0; j < xiaoqu.Count; j++)
                    {
                        if (button3.Text == "已停止")
                            return;


                        ListViewItem lv1 = listView1.Items.Add(xiaoqu[j].Groups[1].Value.Trim()); //使用Listview展示数据

                        lv1.SubItems.Add(yishou[j].Groups[2].Value.Trim());
                        lv1.SubItems.Add(quyu[j].Groups[2].Value.Trim());
                        lv1.SubItems.Add(addr[j].Groups[1].Value.Trim());
                        lv1.SubItems.Add(jiage[j].Groups[1].Value.Trim());
                        lv1.SubItems.Add(zaishou[j].Groups[1].Value.Trim());
                        lv1.SubItems.Add(cityName);
                        lv1.SubItems.Add(DateTime.Now.ToString());


                        listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置


                        string rxg7 = @"data-id=""([\s\S]*?)""";
                        MatchCollection ids = Regex.Matches(strhtml, rxg7);
                        ArrayList lists = new ArrayList();
                        foreach (Match id in ids)
                        {
                            lists.Add("https://" + city + ".lianjia.com/xiaoqu/" + id.Groups[1].Value + "/huxing/");
                        }
                        string html = method.GetUrl(lists[j].ToString(), "utf-8");
                        Match lowest = Regex.Match(html, @"<div class=""frameItemTotalPrice""><span>([\s\S]*?)</span>");
                        Match lowestmianji = Regex.Match(html, @"frameItemTitle"">([\s\S]*?)卫 ([\s\S]*?)㎡");
                        lv1.SubItems.Add(lowest.Groups[1].Value + "万");

                       
                     
                        //评估网开始
                        string url = "http://www.17gp.com/FangJia/GetCommunityList";
                        string xiaoquName= System.Web.HttpUtility.UrlEncode(xiaoqu[j].Groups[1].Value.Trim(), System.Text.Encoding.GetEncoding("utf-8"));
                        string data = "kw="+ xiaoquName + "&num=8&code=";


                        string html2 = method.PostUrl(url,data,this.cookie, "UTF-8");
                        Match uid = Regex.Match(html2, @"guid"":""([\s\S]*?)""");
                        string url2 = "http://www.17gp.com/FangJia/GetRecentlyPrice/"+uid.Groups[1].Value;
                        string html3 = method.PostUrl(url2,"", this.cookie, "UTF-8");
                        Match avgprice = Regex.Match(html3, @"avgPrice"":""([\s\S]*?)""");


                        lv1.SubItems.Add(avgprice.Groups[1].Value+ "元/㎡");
                        lv1.SubItems.Add(DateTime.Now.ToString());


                        
                        int a = Convert.ToInt32(lowest.Groups[1].Value.Trim());
                        int b = Convert.ToInt32(avgprice.Groups[1].Value.Trim());
                        int c = Convert.ToInt32(lowestmianji.Groups[2].Value.Trim());

                        int d =(int)(b*c*0.7);
                        int e = a*10000 - d;
                             
                        int chengshu = (int)(e/a/1000);
                        lv1.SubItems.Add(chengshu.ToString());


                        Thread.Sleep(500);
                    }




                    Application.DoEvents();
                    Thread.Sleep(2000);   //内容获取间隔，可变量      
                }

            }


            catch (System.Exception ex)
            {

                textBox1.Text = ex.ToString();
            }

        }

        #endregion
        private void Form2_Load(object sender, EventArgs e)
        {

            this.tabControl1.SelectedIndex = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button3.Text = "停止采集";
            Thread thread = new Thread(new ThreadStart(lianjia));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Text = "已停止";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
