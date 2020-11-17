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
using helper;

namespace 主程序202006
{
    public partial class 工程项目 : Form
    {
        public 工程项目()
        {
            InitializeComponent();
        }



        public string getcode(string name)
        {
            string url = "http://citycode.blacklife.cn/search/search.php";
            string postdata = "search=" + System.Web.HttpUtility.UrlEncode(name);

            string html = method.PostUrl(url, postdata, "", "utf-8");
            
            Match code = Regex.Match(html, @"编码：([\s\S]*?)</div>");
            return code.Groups[1].Value;
        }

        bool zanting = true;
        string province = "0";
        string city = "0";
        string keyword = "";
        string type1 = "0100";
        string time = "02";
        /// <summary>
        public void run()
        {
            if (textBox1.Text != "")
            {
                keyword = System.Web.HttpUtility.UrlEncode(textBox1.Text);
            }
            if (radioButton2.Checked == true)
            {
                 type1 = "0101";
            }
            if (radioButton3.Checked == true)
            {
                type1 = "0102";
            }
            if (radioButton4.Checked == true)
            {
                type1 = "0104";
            }

            switch (comboBox3.Text.Trim())
            {
                case "当天":
                    time = "01";
                    break;
                case "近三天":
                    time = "02";
                    break;
                case "近十天":
                    time = "03";
                    break;
                case "近一月":
                    time = "04";
                    break;
                case "近三月":
                    time = "05";
                    break;


            }



            string begindate = DateTime.Now.AddDays(-9).ToString("yyyy-mm-dd");
            string enddate = DateTime.Now.AddDays(1).ToString("yyyy-mm-dd");

            for (int i = 1; i <Convert.ToInt32(textBox2.Text)+1; i++)
            {

                label4.Text = "正在搜索，第"+i+"页";
                string url = "http://deal.ggzy.gov.cn/ds/deal/dealList_find.jsp";
                string postdata = "TIMEBEGIN_SHOW="+begindate+ "&TIMEEND_SHOW=" + enddate + "&TIMEBEGIN=" + begindate + "&TIMEEND=" + enddate + "&SOURCE_TYPE=1&DEAL_TIME="+time+"&DEAL_CLASSIFY=01&DEAL_STAGE="+type1+"&DEAL_PROVINCE="+province+"&DEAL_CITY="+city+"&DEAL_PLATFORM=0&BID_PLATFORM=0&DEAL_TRADE=0&isShowAll=1&PAGENUMBER="+i+"&FINDTXT="+keyword;
               
                string html = method.PostUrl(url,postdata,"", "utf-8");

                MatchCollection titles = Regex.Matches(html, @"""title"":""([\s\S]*?)""");
                MatchCollection diqu = Regex.Matches(html, @"""districtShow"":""([\s\S]*?)""");
                MatchCollection type = Regex.Matches(html, @"""classifyShow"":""([\s\S]*?)""");
                MatchCollection stageShow = Regex.Matches(html, @"""stageShow"":""([\s\S]*?)""");
                MatchCollection timeShow = Regex.Matches(html, @"""timeShow"":""([\s\S]*?)""");
                MatchCollection urls = Regex.Matches(html, @"""url"":""([\s\S]*?)""");
              


                for (int a = 0; a < titles.Count; a++)

                {

                    try
                    {



                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(titles[a].Groups[1].Value);
                        lv1.SubItems.Add(diqu[a].Groups[1].Value);
                        lv1.SubItems.Add(type[a].Groups[1].Value);
                        lv1.SubItems.Add(stageShow[a].Groups[1].Value);
                        lv1.SubItems.Add(timeShow[a].Groups[1].Value);
                        lv1.SubItems.Add(urls[a].Groups[1].Value);

                        while (zanting == false)
                        {
                            Application.DoEvents();//等待本次加载完毕才执行下次循环.
                        }
                    }
                    catch
                    {

                        continue;
                    }

                   

                }
                Thread.Sleep(1000);



            }
            label4.Text = "查询结束";
        }
        private void 工程项目_Load(object sender, EventArgs e)
        {
            method.SetWebBrowserFeatures(method.IeVersion.IE10);
            webBrowser1.ScriptErrorsSuppressed = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label4.Text = "正在搜索，请勿重复点击......";
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.comboBox2.Items.Clear();
            this.comboBox2.Text = "";
            if (this.comboBox1.Text == "全国")
            {
                this.comboBox2.Items.Add("中国");

            }
            else if (this.comboBox1.Text == "北京")
            {
                this.comboBox2.Items.Add("北京市");
                this.comboBox2.Items.Add("北京市");
            }
            else if (this.comboBox1.Text == "天津")
            {
                this.comboBox2.Items.Add("天津市");
                this.comboBox2.Items.Add("天津市");
            }
            else if (this.comboBox1.Text == "重庆")
            {
                this.comboBox2.Items.Add("重庆市");
                this.comboBox2.Items.Add("重庆市");
            }
            else if (this.comboBox1.Text == "上海")
            {
                this.comboBox2.Items.Add("上海市");
                this.comboBox2.Items.Add("上海市");
            }
            else if (this.comboBox1.Text == "河北省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("石家庄市"); comboBox2.Items.Add("保定市"); comboBox2.Items.Add("沧州市"); comboBox2.Items.Add("廊坊市"); comboBox2.Items.Add("唐山市"); comboBox2.Items.Add("邢台市"); comboBox2.Items.Add("邯郸市"); comboBox2.Items.Add("衡水市"); comboBox2.Items.Add("秦皇岛市"); comboBox2.Items.Add("张家口市"); comboBox2.Items.Add("承德市");
            }
            else if (this.comboBox1.Text == "山西省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("太原市"); comboBox2.Items.Add("运城市"); comboBox2.Items.Add("临汾市"); comboBox2.Items.Add("大同市"); comboBox2.Items.Add("长治市"); comboBox2.Items.Add("晋城市"); comboBox2.Items.Add("吕梁市"); comboBox2.Items.Add("衡水市"); comboBox2.Items.Add("阳泉市"); comboBox2.Items.Add("忻州市"); comboBox2.Items.Add("朔州市"); comboBox2.Items.Add("晋中市");
            }
            else if (this.comboBox1.Text == "内蒙古自治区")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("呼和浩特市"); comboBox2.Items.Add("包头市"); comboBox2.Items.Add("赤峰市"); comboBox2.Items.Add("呼伦贝尔市"); comboBox2.Items.Add("通辽市"); comboBox2.Items.Add("鄂尔多斯市"); comboBox2.Items.Add("巴彦淖尔盟市"); comboBox2.Items.Add("锡林郭勒市"); comboBox2.Items.Add("乌兰察布市"); comboBox2.Items.Add("兴安盟"); comboBox2.Items.Add("乌海市"); comboBox2.Items.Add("阿拉善盟市");
            }
            else if (this.comboBox1.Text == "辽宁省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("沈阳市"); comboBox2.Items.Add("大连市"); comboBox2.Items.Add("鞍山市"); comboBox2.Items.Add("锦州市"); comboBox2.Items.Add("营口市"); comboBox2.Items.Add("丹东市"); comboBox2.Items.Add("抚顺市"); comboBox2.Items.Add("朝阳市"); comboBox2.Items.Add("葫芦岛市"); comboBox2.Items.Add("铁岭市"); comboBox2.Items.Add("辽阳市"); comboBox2.Items.Add("盘锦市"); comboBox2.Items.Add("阜新市"); comboBox2.Items.Add("本溪市");
            }
            else if (this.comboBox1.Text == "吉林省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("长春市"); comboBox2.Items.Add("吉林市"); comboBox2.Items.Add("延边朝鲜族自治州"); comboBox2.Items.Add("通化市"); comboBox2.Items.Add("四平市"); comboBox2.Items.Add("白城市"); comboBox2.Items.Add("松原市"); comboBox2.Items.Add("白山市"); comboBox2.Items.Add("辽源市");

            }
            else if (this.comboBox1.Text == "黑龙江省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("哈尔滨市"); comboBox2.Items.Add("大庆市"); comboBox2.Items.Add("齐齐哈尔市"); comboBox2.Items.Add("佳木斯市"); comboBox2.Items.Add("伊春市"); comboBox2.Items.Add("牡丹江市"); comboBox2.Items.Add("鸡西市"); comboBox2.Items.Add("黑河市"); comboBox2.Items.Add("绥化市"); comboBox2.Items.Add("双鸭山市"); comboBox2.Items.Add("鹤岗市"); comboBox2.Items.Add("七台河市"); comboBox2.Items.Add("大兴安岭地区");
            }
            else if (this.comboBox1.Text == "江苏省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("苏州市"); comboBox2.Items.Add("南京市"); comboBox2.Items.Add("无锡市"); comboBox2.Items.Add("常州市"); comboBox2.Items.Add("徐州市"); comboBox2.Items.Add("南通市"); comboBox2.Items.Add("扬州市"); comboBox2.Items.Add("泰州市"); comboBox2.Items.Add("盐城市"); comboBox2.Items.Add("镇江市"); comboBox2.Items.Add("连云港市"); comboBox2.Items.Add("淮安市"); comboBox2.Items.Add("宿迁市");
            }
            else if (this.comboBox1.Text == "浙江省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("杭州市"); comboBox2.Items.Add("温州市"); comboBox2.Items.Add("宁波市"); comboBox2.Items.Add("金华市"); comboBox2.Items.Add("台州市"); comboBox2.Items.Add("嘉兴市"); comboBox2.Items.Add("绍兴市"); comboBox2.Items.Add("湖州市"); comboBox2.Items.Add("丽水市"); comboBox2.Items.Add("衢州市"); comboBox2.Items.Add("舟山市");
            }
            else if (this.comboBox1.Text == "安徽省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("合肥市"); comboBox2.Items.Add("淮北市"); comboBox2.Items.Add("安庆市"); comboBox2.Items.Add("芜湖市"); comboBox2.Items.Add("阜阳市"); comboBox2.Items.Add("滁州市"); comboBox2.Items.Add("蚌埠市"); comboBox2.Items.Add("马鞍山市"); comboBox2.Items.Add("六安市"); comboBox2.Items.Add("巢湖市"); comboBox2.Items.Add("宣城市"); comboBox2.Items.Add("淮南市"); comboBox2.Items.Add("亳州市"); comboBox2.Items.Add("黄山市"); comboBox2.Items.Add("池州市"); comboBox2.Items.Add("铜陵市"); comboBox2.Items.Add("宿州市");
            }
            else if (this.comboBox1.Text == "福建省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("厦门市"); comboBox2.Items.Add("泉州市"); comboBox2.Items.Add("福州市"); comboBox2.Items.Add("漳州市"); comboBox2.Items.Add("莆田市"); comboBox2.Items.Add("龙岩市"); comboBox2.Items.Add("宁德市"); comboBox2.Items.Add("三明市"); comboBox2.Items.Add("南平市");
            }
            else if (this.comboBox1.Text == "江西省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("南昌市"); comboBox2.Items.Add("抚州市"); comboBox2.Items.Add("赣州市"); comboBox2.Items.Add("九江市"); comboBox2.Items.Add("上饶市"); comboBox2.Items.Add("吉安市"); comboBox2.Items.Add("景德镇市"); comboBox2.Items.Add("萍乡市"); comboBox2.Items.Add("新余市"); comboBox2.Items.Add("宜春市"); comboBox2.Items.Add("鹰潭市");
            }
            else if (this.comboBox1.Text == "山东省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("济南市"); comboBox2.Items.Add("青岛市"); comboBox2.Items.Add("淄博市"); comboBox2.Items.Add("枣庄市"); comboBox2.Items.Add("东营市"); comboBox2.Items.Add("烟台市"); comboBox2.Items.Add("莱阳市"); comboBox2.Items.Add("潍坊市"); comboBox2.Items.Add("济宁市"); comboBox2.Items.Add("泰安市"); comboBox2.Items.Add("威海市"); comboBox2.Items.Add("日照市"); comboBox2.Items.Add("滨州市"); comboBox2.Items.Add("德州市"); comboBox2.Items.Add("聊城市"); comboBox2.Items.Add("临沂市"); comboBox2.Items.Add("菏泽市"); comboBox2.Items.Add("莱芜市");
            }
            else if (this.comboBox1.Text == "河南省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("郑州市"); comboBox2.Items.Add("洛阳市"); comboBox2.Items.Add("新乡市"); comboBox2.Items.Add("南阳市"); comboBox2.Items.Add("安阳市"); comboBox2.Items.Add("焦作市"); comboBox2.Items.Add("许昌市"); comboBox2.Items.Add("商丘市"); comboBox2.Items.Add("平顶山市"); comboBox2.Items.Add("周口市"); comboBox2.Items.Add("信阳市"); comboBox2.Items.Add("濮阳市"); comboBox2.Items.Add("开封市"); comboBox2.Items.Add("驻马店市"); comboBox2.Items.Add("鹤壁市"); comboBox2.Items.Add("三门峡市"); comboBox2.Items.Add("漯河市"); comboBox2.Items.Add("济源市");
            }
            else if (this.comboBox1.Text == "湖北省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("武汉市"); comboBox2.Items.Add("襄樊市"); comboBox2.Items.Add("宜昌市"); comboBox2.Items.Add("荆州市"); comboBox2.Items.Add("十堰市"); comboBox2.Items.Add("孝感市"); comboBox2.Items.Add("黄冈市"); comboBox2.Items.Add("恩施土家族苗族自治州"); comboBox2.Items.Add("黄石市"); comboBox2.Items.Add("荆门市"); comboBox2.Items.Add("随州市"); comboBox2.Items.Add("咸宁市"); comboBox2.Items.Add("鄂州市"); comboBox2.Items.Add("潜江市"); comboBox2.Items.Add("神农架林区市"); comboBox2.Items.Add("天门市"); comboBox2.Items.Add("仙桃市");
            }
            else if (this.comboBox1.Text == "湖南省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("长沙市"); comboBox2.Items.Add("湘潭市"); comboBox2.Items.Add("衡阳市"); comboBox2.Items.Add("株洲市"); comboBox2.Items.Add("郴州市"); comboBox2.Items.Add("常德市"); comboBox2.Items.Add("邵阳市"); comboBox2.Items.Add("岳阳市"); comboBox2.Items.Add("怀化市"); comboBox2.Items.Add("永州市"); comboBox2.Items.Add("娄底市"); comboBox2.Items.Add("益阳市"); comboBox2.Items.Add("张家界市");
            }
            else if (this.comboBox1.Text == "广东省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("深圳市"); comboBox2.Items.Add("广州市"); comboBox2.Items.Add("东莞市"); comboBox2.Items.Add("佛山市"); comboBox2.Items.Add("中山市"); comboBox2.Items.Add("惠州市"); comboBox2.Items.Add("珠海市"); comboBox2.Items.Add("汕头市"); comboBox2.Items.Add("江门市"); comboBox2.Items.Add("肇庆市"); comboBox2.Items.Add("揭阳市"); comboBox2.Items.Add("梅州市"); comboBox2.Items.Add("茂名市"); comboBox2.Items.Add("潮州市"); comboBox2.Items.Add("清远市"); comboBox2.Items.Add("韶关市"); comboBox2.Items.Add("潜江市"); comboBox2.Items.Add("河源市"); comboBox2.Items.Add("汕尾市"); comboBox2.Items.Add("云浮市"); comboBox2.Items.Add("阳江市");
            }
            else if (this.comboBox1.Text == "广西省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("南宁市"); comboBox2.Items.Add("桂林市"); comboBox2.Items.Add("柳州市"); comboBox2.Items.Add("玉林市"); comboBox2.Items.Add("贵港市"); comboBox2.Items.Add("百色市"); comboBox2.Items.Add("梧州市"); comboBox2.Items.Add("北海市"); comboBox2.Items.Add("钦州市"); comboBox2.Items.Add("河池市"); comboBox2.Items.Add("防城港市"); comboBox2.Items.Add("来宾市"); comboBox2.Items.Add("贺州市"); comboBox2.Items.Add("崇左市");
            }
            else if (this.comboBox1.Text == "海南省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("海口市"); comboBox2.Items.Add("三亚市"); comboBox2.Items.Add("文昌市");
            }
            else if (this.comboBox1.Text == "四川省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("成都市"); comboBox2.Items.Add("绵阳市"); comboBox2.Items.Add("德阳市"); comboBox2.Items.Add("南充市"); comboBox2.Items.Add("宜宾市"); comboBox2.Items.Add("乐山市"); comboBox2.Items.Add("泸州市"); comboBox2.Items.Add("达州市"); comboBox2.Items.Add("自贡市"); comboBox2.Items.Add("广元市"); comboBox2.Items.Add("广安市"); comboBox2.Items.Add("内江市"); comboBox2.Items.Add("攀枝花市");
            }
            else if (this.comboBox1.Text == "贵州省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("贵阳市"); comboBox2.Items.Add("遵义市"); comboBox2.Items.Add("六盘水市"); comboBox2.Items.Add("安顺市");
            }
            else if (this.comboBox1.Text == "云南省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("昆明市");
            }
            else if (this.comboBox1.Text == "西藏自治区")
            {
                comboBox2.Items.Add("全省");
            }
            else if (this.comboBox1.Text == "陕西省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("西安市"); comboBox2.Items.Add("榆林市"); comboBox2.Items.Add("宝鸡市"); comboBox2.Items.Add("汉中市"); comboBox2.Items.Add("咸阳市"); comboBox2.Items.Add("渭南市"); comboBox2.Items.Add("延安市"); comboBox2.Items.Add("安康市"); comboBox2.Items.Add("商洛市"); comboBox2.Items.Add("铜川市");
            }
            else if (this.comboBox1.Text == "甘肃省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("兰州市"); comboBox2.Items.Add("白银市"); comboBox2.Items.Add("天水市"); comboBox2.Items.Add("酒泉市"); comboBox2.Items.Add("庆阳市"); comboBox2.Items.Add("平凉市"); comboBox2.Items.Add("张掖市"); comboBox2.Items.Add("武威市"); comboBox2.Items.Add("陇南市"); comboBox2.Items.Add("定西市"); comboBox2.Items.Add("金昌市"); comboBox2.Items.Add("嘉峪关市");
            }

            else if (this.comboBox1.Text == "青海省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("西宁市");
            }
            else if (this.comboBox1.Text == "宁夏回族自治区")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("银川市");
            }
            else if (this.comboBox1.Text == "新疆维吾尔自治区")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("乌鲁木齐市");
            }
            province = getcode(comboBox1.Text);
            if (province == "")
            {
                province = "0";
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text != "全省")
            {
                city = getcode(comboBox2.Text);
            }
            if (city == "" || province=="0")
            {
                city = "0";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
           
           
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                if (listView1.SelectedItems[0].SubItems[6].Text.Contains("/a/"))
                {
                    string url = listView1.SelectedItems[0].SubItems[6].Text.Replace("/a/", "/b/");

                    webBrowser1.Navigate(url);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
