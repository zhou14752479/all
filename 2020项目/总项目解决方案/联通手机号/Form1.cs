using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;
using MySql.Data.MySqlClient;

namespace 联通手机号
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            foreach (Control ctl in tabPage2.Controls)
            {
                if (ctl is ListView)
                {

                    ctl.Click += new System.EventHandler(listviewClick);

                }

            }

            foreach (Control ctl in tabPage3.Controls)
            {
                if (ctl is ListView)
                {

                    ctl.Click += new System.EventHandler(listviewClick);

                }

            }
        }

        bool zanting = true;

        

       

        private void listviewClick(object sender, EventArgs e)
        {
            ListView listview = (ListView)sender;
           
            Clipboard.SetText(listview.SelectedItems[0].SubItems[0].Text);
        }

        public void run()
        {
            #region 判断套餐
            ArrayList goodIds = new ArrayList();
            string taocan = "全部套餐";
            string pid = "34";
            string cid = "350";

            taocan = listView30.SelectedItems[0].SubItems[2].Text;
            pid = listView30.SelectedItems[0].SubItems[4].Text;
            cid = listView30.SelectedItems[0].SubItems[5].Text;
            if (taocan == "全部套餐")
            {
                goodIds.Add("981610241535"); goodIds.Add("981702278573"); goodIds.Add("981802085690"); goodIds.Add("981711282733"); goodIds.Add("981711282734");
                goodIds.Add("981611177210"); goodIds.Add("981801174882"); goodIds.Add("981909236567"); goodIds.Add("981909126136"); goodIds.Add("981909126135");
                goodIds.Add("981712203626"); goodIds.Add("981712203627"); goodIds.Add("981804027821"); goodIds.Add("981909126138"); goodIds.Add("981802085680");
                goodIds.Add("981802085681"); goodIds.Add("981909045640");
            }
            if (taocan == "腾讯大王卡")
            {
                goodIds.Add("981610241535");
            }

            if (taocan == "腾讯天王卡")
            {
                goodIds.Add("981702278573");
            }
            if (taocan == "腾讯地王卡")
            {
                goodIds.Add("981802085690");
            }
            if (taocan == "阿里小宝卡")
            {
                goodIds.Add("981711282733");
            }
            if (taocan == "阿里大宝卡")
            {
                goodIds.Add("981711282734");
            }
            if (taocan == "蚂蚁大宝卡")
            {
                goodIds.Add("981611177210");
            }
            if (taocan == "蚂蚁国宝卡")
            {
                goodIds.Add("981801174882");
            }
            if (taocan == "中铁王卡")
            {
                goodIds.Add("981909236567");
            }
            if (taocan == "米粉卡Pro")
            {
                goodIds.Add("981909126136");
            }
            if (taocan == "米粉王卡")
            {
                goodIds.Add("981909126135");
            }
            if (taocan == "滴滴大王卡")
            {
                goodIds.Add("981712203626");
            }
            if (taocan == "滴滴小王卡")
            {
                goodIds.Add("981712203627");
            }
            if (taocan == "滴滴mini卡")
            {
                goodIds.Add("981804027821");
            }
            if (taocan == "天神卡")
            {
                goodIds.Add("981909126138");
            }
            if (taocan == "钉钉大宝卡")
            {
                goodIds.Add("981802085680");
            }
            if (taocan == "钉钉小宝卡")
            {
                goodIds.Add("981802085681");
            }
            if (taocan == "懂我卡畅享版")
            {
                goodIds.Add("981909045640");
            }
            #endregion







            try
            {
                foreach (string goodid in goodIds)
                {
                    string dqtaocan = "";
                    if (goodid == "981610241535")
                    {
                        dqtaocan = "腾讯大网卡";               
                    }

                    if (goodid == "981702278573")
                    {
                        dqtaocan = "腾讯天网卡";
                    }
                    if (goodid == "981802085690")
                    {
                        dqtaocan = "腾讯地网卡";
                    }
                    if (goodid == "981711282733")
                    {
                        dqtaocan = "阿里小宝卡";
                    }
                    if (goodid == "981711282734")
                    {
                        dqtaocan = "阿里大宝卡";
                    }
                    if (goodid == "981611177210")
                    {
                        dqtaocan = "蚂蚁大宝卡";
                    }
                    if (goodid == "981801174882")
                    {
                        dqtaocan = "蚂蚁国宝卡";
                        
                    }
                    if (goodid == "981909236567")
                    {
                        dqtaocan = "中铁王卡";
                       
                    }
                    if (goodid == "981909126136")
                    {
                        dqtaocan = "米粉卡Pro";
                        
                    }
                    if (goodid == "981909126135")
                    {
                        dqtaocan = "米粉王卡";
                        
                    }
                    if (goodid == "981712203626")
                    {
                        dqtaocan = "滴滴大王卡";
                        
                    }
                    if (goodid == "981712203627")
                    {
                        dqtaocan = "滴滴小王卡";
                       
                    }
                    if (goodid == "981804027821")
                    {
                        dqtaocan = "滴滴mini卡";
                        
                    }
                    if (goodid == "981909126138")
                    {
                        dqtaocan = "天神卡";
                        
                    }
                    if (goodid == "981802085680")
                    {
                        dqtaocan = "钉钉大宝卡";
                        
                    }
                    if (goodid == "981802085681")
                    {
                        dqtaocan = "钉钉小宝卡";
                        
                    }
                    if (goodid == "981909045640")
                    {
                        dqtaocan = "懂我卡畅享版";
                        
                    }


                    for (int i = 0; i < 9999; i++)
                    {

                        string url = "https://msgo.10010.com/NumApp/NumberCenter/qryNum?callback=jsonp_queryMoreNums&provinceCode="+pid+"&cityCode="+cid+"&monthFeeLimit=0&goodsId=" + goodid + "&searchCategory=3&net=01&amounts=200&codeTypeCode=&searchValue=&qryType=02&goodsNet=4&channel=msg-xsg&_=1578983826340";
                       
                        string html = method.GetUrl(url, "utf-8");

                        textBox3.Text += DateTime.Now.ToString() + ":正在采集"+ dqtaocan+"\r\n";
                        MatchCollection haomas = Regex.Matches(html, @"\d{11}");
                        if (haomas.Count == 0)
                            return;
                        for (int j = 0; j < haomas.Count; j++)
                        {
                            string haoma = haomas[j].Groups[0].Value+","+ dqtaocan;

                            ListViewItem listViewItem = this.listView1.Items.Add(haoma);

                            Match match3 = Regex.Match(haoma, @"\d([0-9])(?!\1)([0-9])\2{2}\d");  //三连
                            Match match7 = Regex.Match(haoma, @"\d{7}([0-9]012|[^0]123|[^1]234|[^2]345|[^3]456|[^4]567|[^5]678|[^6]789)");  // 尾号ABC
                            Match match6 = Regex.Match(haoma, @"\d{6}([0-9])(?!\1)([0-9])\2(?!\2)([0-9])\3");   //尾号AABB
                            Match match5 = Regex.Match(haoma, @"\d{7}([0-9])(?!\1)([0-9])\1\2");   //尾号ABAB
                            Match match10 = Regex.Match(haoma, @"\d{5}(\d{3})\1");   //尾号ABCABC
                            Match match8 = Regex.Match(haoma, @"\d{6}([\d])\1{2,}([\d])\2{0,}\1");   //尾号AAABA

                            //第二排
                            Match match19 = Regex.Match(haoma, @"\d{7}([0-9])(?!\1)([0-9])\2{2}");   //尾号AAA
                            Match match18 = Regex.Match(haoma, @"\d([0-9])(?!\1)([0-9])\2{3}\d");   //四连

                            Match match16 = Regex.Match(haoma, @"\d{7}0123|\d{7}1234|\d{7}2345|\d{7}3456|\d{7}4567|\d{7}5678|\d{7}6789");   //尾号ABCD
                            Match match15 = Regex.Match(haoma, @"\d{4}([0-9])(?!\1)([0-9])\2(?!\2)([0-9])\3(?!\3)([0-9])\4");   //尾号AABBCC
                            Match match13 = Regex.Match(haoma, @"(01234|12345|23456|34567|45678|56789)");   //中间ABCDE
                            Match match11 = Regex.Match(haoma, @"\d{4}([0-9])(?!\1)([0-9])\2(?!\2)([0-9])\3(?!\3)([0-9])\4");
                            //第三排

                            Match match28 = Regex.Match(haoma, @"\d{7}(0000)|\d{7}(1111)|\d{7}(2222)|\d{7}(3333)|\d{7}(4444)|\d{7}(5555)|\d{7}(6666)|\d{7}(7777)|\d{7}(8888)|\d{7}(9999)");
                            Match match27 = Regex.Match(haoma, @"00000|11111|22222|33333|44444|55555|66666|77777|88888|99999");
                            Match match25 = Regex.Match(haoma, @"\d{6}(01234|12345|23456|34567|45678|56789)");
                            Match match24 = Regex.Match(haoma, @"\d{3}([0-9])\1([0-9])\2([0-9])\3([0-9])\4");
                            Match match23 = Regex.Match(haoma, @"\d{3}(\d{4})\1");
                            Match match22 = Regex.Match(haoma, @"\d{3}(\d{4})\1");

                            if (textBox4.Text != "" )
                            {
                                if (haoma.Contains(textBox4.Text.Trim()))
                                {
                                    this.listView2.Items.Add(haoma);
                                }

                            }









                            if (match3.Groups[1].Value != "")
                            {
                                this.listView3.Items.Add(haoma);
                            }
                            if (match7.Groups[1].Value != "")
                            {
                                this.listView7.Items.Add(haoma);
                            }
                            if (match6.Groups[1].Value != "")
                            {
                                this.listView6.Items.Add(haoma);
                            }
                            if (match5.Groups[1].Value != "")
                            {
                                this.listView5.Items.Add(haoma);
                            }
                            if (match10.Groups[1].Value != "")
                            {
                                this.listView10.Items.Add(haoma);
                            }
                            if (match8.Groups[1].Value != "")
                            {
                                this.listView8.Items.Add(haoma);
                            }
                            if (match19.Groups[1].Value != "")
                            {
                                this.listView19.Items.Add(haoma);
                            }

                            if (match18.Groups[1].Value != "")
                            {
                                this.listView18.Items.Add(haoma);
                            }


                            if (match16.Groups[1].Value != "")
                            {
                                this.listView16.Items.Add(haoma);
                            }
                            if (match15.Groups[1].Value != "")
                            {
                                this.listView15.Items.Add(haoma);
                            }

                            if (match13.Groups[1].Value != "")
                            {
                                this.listView13.Items.Add(haoma);
                            }
                            if (match11.Groups[1].Value != "")
                            {
                                this.listView11.Items.Add(haoma);
                            }
                            if (match28.Groups[1].Value != "")
                            {
                                this.listView28.Items.Add(haoma);
                            }

                            if (match27.Groups[1].Value != "")
                            {
                                this.listView27.Items.Add(haoma);
                            }

                            if (match25.Groups[1].Value != "")
                            {
                                this.listView25.Items.Add(haoma);
                            }

                            if (match24.Groups[1].Value != "")
                            {
                                this.listView24.Items.Add(haoma);
                            }
                            if (match23.Groups[1].Value != "")
                            {
                                this.listView23.Items.Add(haoma);
                            }

                            if (match22.Groups[1].Value != "")
                            {
                                this.listView22.Items.Add(haoma);
                            }





                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            //if (listView1.Items.Count > 2)
                            //{
                            //    this.listView1.Items[j].EnsureVisible();
                            //}

                        }

                    }

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
           


            textBox3.Text = "软件初始化成功..."+"\r\n";


            listView29.Items.Add("全部套餐");
            listView29.Items.Add("腾讯大王卡");
            listView29.Items.Add("腾讯天王卡");
            listView29.Items.Add("腾讯地王卡");
            listView29.Items.Add("阿里小宝卡");
            listView29.Items.Add("阿里大宝卡");
            listView29.Items.Add("蚂蚁大宝卡");
            listView29.Items.Add("蚂蚁国宝卡");
            listView29.Items.Add("中铁王卡");
            listView29.Items.Add("米粉卡Pro");
            listView29.Items.Add("米粉王卡");
            listView29.Items.Add("滴滴大王卡");
            listView29.Items.Add("滴滴小王卡");
            listView29.Items.Add("滴滴mini卡");
            listView29.Items.Add("天神卡");
            listView29.Items.Add("钉钉大宝卡");
            listView29.Items.Add("钉钉小宝卡");

            listView29.Items[0].Selected = true;

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string constr = "Host =47.99.68.92;Database=vip_database;Username=root;Password=zhoukaige00.@*.";
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();

            MySqlCommand cmd = new MySqlCommand("select * from vip where username='联通手机卡'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

            MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

            if (reader.Read())
            {

                string password = reader["password"].ToString().Trim();

                if (password != "联通手机卡")

                {
                    MessageBox.Show("验证失败");

                    Environment.Exit(0);
                }




            }







            if (listView30.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选中需要开始的任务");
                return;
            }


            
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region 城市联动
            this.comboBox2.Items.Clear();
            this.comboBox2.Text = "";
            if (this.comboBox1.Text == "全国")
            {
               

            }
            else if (this.comboBox1.Text == "北京")
            {
                this.comboBox2.Items.Add("北京市");
            }
            else if (this.comboBox1.Text == "天津")
            {
                this.comboBox2.Items.Add("天津市");
            }
            else if (this.comboBox1.Text == "重庆")
            {
                this.comboBox2.Items.Add("重庆市");
            }
            else if (this.comboBox1.Text == "上海")
            {
                this.comboBox2.Items.Add("上海市");

            }
            else if (this.comboBox1.Text == "河北省")
            {
                comboBox2.Items.Add("全部城市"); comboBox2.Items.Add("石家庄市"); comboBox2.Items.Add("保定市"); comboBox2.Items.Add("沧州市"); comboBox2.Items.Add("廊坊市"); comboBox2.Items.Add("唐山市"); comboBox2.Items.Add("邢台市"); comboBox2.Items.Add("邯郸市"); comboBox2.Items.Add("衡水市"); comboBox2.Items.Add("秦皇岛市"); comboBox2.Items.Add("张家口市"); comboBox2.Items.Add("承德市");
            }
            else if (this.comboBox1.Text == "山西省")
            {
                comboBox2.Items.Add("全部城市"); comboBox2.Items.Add("太原市"); comboBox2.Items.Add("运城市"); comboBox2.Items.Add("临汾市"); comboBox2.Items.Add("大同市"); comboBox2.Items.Add("长治市"); comboBox2.Items.Add("晋城市"); comboBox2.Items.Add("吕梁市"); comboBox2.Items.Add("衡水市"); comboBox2.Items.Add("阳泉市"); comboBox2.Items.Add("忻州市"); comboBox2.Items.Add("朔州市"); comboBox2.Items.Add("晋中市");
            }
            else if (this.comboBox1.Text == "内蒙古自治区")
            {
                comboBox2.Items.Add("全部城市"); comboBox2.Items.Add("呼和浩特市"); comboBox2.Items.Add("包头市"); comboBox2.Items.Add("赤峰市"); comboBox2.Items.Add("呼伦贝尔市"); comboBox2.Items.Add("通辽市"); comboBox2.Items.Add("鄂尔多斯市"); comboBox2.Items.Add("巴彦淖尔盟市"); comboBox2.Items.Add("锡林郭勒市"); comboBox2.Items.Add("乌兰察布市"); comboBox2.Items.Add("兴安盟"); comboBox2.Items.Add("乌海市"); comboBox2.Items.Add("阿拉善盟市");
            }
            else if (this.comboBox1.Text == "辽宁省")
            {
                comboBox2.Items.Add("全部城市"); comboBox2.Items.Add("沈阳市"); comboBox2.Items.Add("大连市"); comboBox2.Items.Add("鞍山市"); comboBox2.Items.Add("锦州市"); comboBox2.Items.Add("营口市"); comboBox2.Items.Add("丹东市"); comboBox2.Items.Add("抚顺市"); comboBox2.Items.Add("朝阳市"); comboBox2.Items.Add("葫芦岛市"); comboBox2.Items.Add("铁岭市"); comboBox2.Items.Add("辽阳市"); comboBox2.Items.Add("盘锦市"); comboBox2.Items.Add("阜新市"); comboBox2.Items.Add("本溪市");
            }
            else if (this.comboBox1.Text == "吉林省")
            {
                comboBox2.Items.Add("全部城市"); comboBox2.Items.Add("长春市"); comboBox2.Items.Add("吉林市"); comboBox2.Items.Add("延边朝鲜族自治州"); comboBox2.Items.Add("通化市"); comboBox2.Items.Add("四平市"); comboBox2.Items.Add("白城市"); comboBox2.Items.Add("松原市"); comboBox2.Items.Add("白山市"); comboBox2.Items.Add("辽源市");

            }
            else if (this.comboBox1.Text == "黑龙江省")
            {
                comboBox2.Items.Add("全部城市"); comboBox2.Items.Add("哈尔滨市"); comboBox2.Items.Add("大庆市"); comboBox2.Items.Add("齐齐哈尔市"); comboBox2.Items.Add("佳木斯市"); comboBox2.Items.Add("伊春市"); comboBox2.Items.Add("牡丹江市"); comboBox2.Items.Add("鸡西市"); comboBox2.Items.Add("黑河市"); comboBox2.Items.Add("绥化市"); comboBox2.Items.Add("双鸭山市"); comboBox2.Items.Add("鹤岗市"); comboBox2.Items.Add("七台河市"); comboBox2.Items.Add("大兴安岭地区");
            }
            else if (this.comboBox1.Text == "江苏省")
            {
                comboBox2.Items.Add("全部城市"); comboBox2.Items.Add("苏州市"); comboBox2.Items.Add("南京市"); comboBox2.Items.Add("无锡市"); comboBox2.Items.Add("常州市"); comboBox2.Items.Add("徐州市"); comboBox2.Items.Add("南通市"); comboBox2.Items.Add("扬州市"); comboBox2.Items.Add("泰州市"); comboBox2.Items.Add("盐城市"); comboBox2.Items.Add("镇江市"); comboBox2.Items.Add("连云港市"); comboBox2.Items.Add("淮安市"); comboBox2.Items.Add("宿迁市");
            }
            else if (this.comboBox1.Text == "浙江省")
            {
                comboBox2.Items.Add("全部城市"); comboBox2.Items.Add("杭州市"); comboBox2.Items.Add("温州市"); comboBox2.Items.Add("宁波市"); comboBox2.Items.Add("金华市"); comboBox2.Items.Add("台州市"); comboBox2.Items.Add("嘉兴市"); comboBox2.Items.Add("绍兴市"); comboBox2.Items.Add("湖州市"); comboBox2.Items.Add("丽水市"); comboBox2.Items.Add("衢州市"); comboBox2.Items.Add("舟山市");
            }
            else if (this.comboBox1.Text == "安徽省")
            {
                comboBox2.Items.Add("全部城市"); comboBox2.Items.Add("合肥市"); comboBox2.Items.Add("淮北市"); comboBox2.Items.Add("安庆市"); comboBox2.Items.Add("芜湖市"); comboBox2.Items.Add("阜阳市"); comboBox2.Items.Add("滁州市"); comboBox2.Items.Add("蚌埠市"); comboBox2.Items.Add("马鞍山市"); comboBox2.Items.Add("六安市"); comboBox2.Items.Add("巢湖市"); comboBox2.Items.Add("宣城市"); comboBox2.Items.Add("淮南市"); comboBox2.Items.Add("亳州市"); comboBox2.Items.Add("黄山市"); comboBox2.Items.Add("池州市"); comboBox2.Items.Add("铜陵市"); comboBox2.Items.Add("宿州市");
            }
            else if (this.comboBox1.Text == "福建省")
            {
                comboBox2.Items.Add("全部城市"); comboBox2.Items.Add("厦门市"); comboBox2.Items.Add("泉州市"); comboBox2.Items.Add("福州市"); comboBox2.Items.Add("漳州市"); comboBox2.Items.Add("莆田市"); comboBox2.Items.Add("龙岩市"); comboBox2.Items.Add("宁德市"); comboBox2.Items.Add("三明市"); comboBox2.Items.Add("南平市");
            }
            else if (this.comboBox1.Text == "江西省")
            {
                comboBox2.Items.Add("全部城市"); comboBox2.Items.Add("南昌市"); comboBox2.Items.Add("抚州市"); comboBox2.Items.Add("赣州市"); comboBox2.Items.Add("九江市"); comboBox2.Items.Add("上饶市"); comboBox2.Items.Add("吉安市"); comboBox2.Items.Add("景德镇市"); comboBox2.Items.Add("萍乡市"); comboBox2.Items.Add("新余市"); comboBox2.Items.Add("宜春市"); comboBox2.Items.Add("鹰潭市");
            }
            else if (this.comboBox1.Text == "山东省")
            {
                comboBox2.Items.Add("全部城市"); comboBox2.Items.Add("济南市"); comboBox2.Items.Add("青岛市"); comboBox2.Items.Add("淄博市"); comboBox2.Items.Add("枣庄市"); comboBox2.Items.Add("东营市"); comboBox2.Items.Add("烟台市"); comboBox2.Items.Add("莱阳市"); comboBox2.Items.Add("潍坊市"); comboBox2.Items.Add("济宁市"); comboBox2.Items.Add("泰安市"); comboBox2.Items.Add("威海市"); comboBox2.Items.Add("日照市"); comboBox2.Items.Add("滨州市"); comboBox2.Items.Add("德州市"); comboBox2.Items.Add("聊城市"); comboBox2.Items.Add("临沂市"); comboBox2.Items.Add("菏泽市"); comboBox2.Items.Add("莱芜市");
            }
            else if (this.comboBox1.Text == "河南省")
            {
                comboBox2.Items.Add("全部城市"); comboBox2.Items.Add("郑州市"); comboBox2.Items.Add("洛阳市"); comboBox2.Items.Add("新乡市"); comboBox2.Items.Add("南阳市"); comboBox2.Items.Add("安阳市"); comboBox2.Items.Add("焦作市"); comboBox2.Items.Add("许昌市"); comboBox2.Items.Add("商丘市"); comboBox2.Items.Add("平顶山市"); comboBox2.Items.Add("周口市"); comboBox2.Items.Add("信阳市"); comboBox2.Items.Add("濮阳市"); comboBox2.Items.Add("开封市"); comboBox2.Items.Add("驻马店市"); comboBox2.Items.Add("鹤壁市"); comboBox2.Items.Add("三门峡市"); comboBox2.Items.Add("漯河市"); comboBox2.Items.Add("济源市");
            }
            else if (this.comboBox1.Text == "湖北省")
            {
                comboBox2.Items.Add("全部城市"); comboBox2.Items.Add("武汉市"); comboBox2.Items.Add("襄樊市"); comboBox2.Items.Add("宜昌市"); comboBox2.Items.Add("荆州市"); comboBox2.Items.Add("十堰市"); comboBox2.Items.Add("孝感市"); comboBox2.Items.Add("黄冈市"); comboBox2.Items.Add("恩施土家族苗族自治州"); comboBox2.Items.Add("黄石市"); comboBox2.Items.Add("荆门市"); comboBox2.Items.Add("随州市"); comboBox2.Items.Add("咸宁市"); comboBox2.Items.Add("鄂州市"); comboBox2.Items.Add("潜江市"); comboBox2.Items.Add("神农架林区市"); comboBox2.Items.Add("天门市"); comboBox2.Items.Add("仙桃市");
            }
            else if (this.comboBox1.Text == "湖南省")
            {
                comboBox2.Items.Add("全部城市"); comboBox2.Items.Add("长沙市"); comboBox2.Items.Add("湘潭市"); comboBox2.Items.Add("衡阳市"); comboBox2.Items.Add("株洲市"); comboBox2.Items.Add("郴州市"); comboBox2.Items.Add("常德市"); comboBox2.Items.Add("邵阳市"); comboBox2.Items.Add("岳阳市"); comboBox2.Items.Add("怀化市"); comboBox2.Items.Add("永州市"); comboBox2.Items.Add("娄底市"); comboBox2.Items.Add("益阳市"); comboBox2.Items.Add("张家界市");
            }
            else if (this.comboBox1.Text == "广东省")
            {
                comboBox2.Items.Add("全部城市"); comboBox2.Items.Add("深圳市"); comboBox2.Items.Add("广州市"); comboBox2.Items.Add("东莞市"); comboBox2.Items.Add("佛山市"); comboBox2.Items.Add("中山市"); comboBox2.Items.Add("惠州市"); comboBox2.Items.Add("珠海市"); comboBox2.Items.Add("汕头市"); comboBox2.Items.Add("江门市"); comboBox2.Items.Add("肇庆市"); comboBox2.Items.Add("揭阳市"); comboBox2.Items.Add("梅州市"); comboBox2.Items.Add("茂名市"); comboBox2.Items.Add("潮州市"); comboBox2.Items.Add("清远市"); comboBox2.Items.Add("韶关市"); comboBox2.Items.Add("潜江市"); comboBox2.Items.Add("河源市"); comboBox2.Items.Add("汕尾市"); comboBox2.Items.Add("云浮市"); comboBox2.Items.Add("阳江市");
            }
            else if (this.comboBox1.Text == "广西省")
            {
                comboBox2.Items.Add("全部城市"); comboBox2.Items.Add("南宁市"); comboBox2.Items.Add("桂林市"); comboBox2.Items.Add("柳州市"); comboBox2.Items.Add("玉林市"); comboBox2.Items.Add("贵港市"); comboBox2.Items.Add("百色市"); comboBox2.Items.Add("梧州市"); comboBox2.Items.Add("北海市"); comboBox2.Items.Add("钦州市"); comboBox2.Items.Add("河池市"); comboBox2.Items.Add("防城港市"); comboBox2.Items.Add("来宾市"); comboBox2.Items.Add("贺州市"); comboBox2.Items.Add("崇左市");
            }
            else if (this.comboBox1.Text == "海南省")
            {
                comboBox2.Items.Add("全部城市"); comboBox2.Items.Add("海口市"); comboBox2.Items.Add("三亚市"); comboBox2.Items.Add("文昌市");
            }
            else if (this.comboBox1.Text == "四川省")
            {
                comboBox2.Items.Add("全部城市"); comboBox2.Items.Add("成都市"); comboBox2.Items.Add("绵阳市"); comboBox2.Items.Add("德阳市"); comboBox2.Items.Add("南充市"); comboBox2.Items.Add("宜宾市"); comboBox2.Items.Add("乐山市"); comboBox2.Items.Add("泸州市"); comboBox2.Items.Add("达州市"); comboBox2.Items.Add("自贡市"); comboBox2.Items.Add("广元市"); comboBox2.Items.Add("广安市"); comboBox2.Items.Add("内江市"); comboBox2.Items.Add("攀枝花市");
            }
            else if (this.comboBox1.Text == "贵州省")
            {
                comboBox2.Items.Add("全部城市"); comboBox2.Items.Add("贵阳市"); comboBox2.Items.Add("遵义市"); comboBox2.Items.Add("六盘水市"); comboBox2.Items.Add("安顺市");
            }
            else if (this.comboBox1.Text == "云南省")
            {
                comboBox2.Items.Add("全部城市"); comboBox2.Items.Add("昆明市");
            }
            else if (this.comboBox1.Text == "西藏自治区")
            {
                comboBox2.Items.Add("全部城市");
            }
            else if (this.comboBox1.Text == "陕西省")
            {
                comboBox2.Items.Add("全部城市"); comboBox2.Items.Add("西安市"); comboBox2.Items.Add("榆林市"); comboBox2.Items.Add("宝鸡市"); comboBox2.Items.Add("汉中市"); comboBox2.Items.Add("咸阳市"); comboBox2.Items.Add("渭南市"); comboBox2.Items.Add("延安市"); comboBox2.Items.Add("安康市"); comboBox2.Items.Add("商洛市"); comboBox2.Items.Add("铜川市");
            }
            else if (this.comboBox1.Text == "甘肃省")
            {
                comboBox2.Items.Add("全部城市"); comboBox2.Items.Add("兰州市"); comboBox2.Items.Add("白银市"); comboBox2.Items.Add("天水市"); comboBox2.Items.Add("酒泉市"); comboBox2.Items.Add("庆阳市"); comboBox2.Items.Add("平凉市"); comboBox2.Items.Add("张掖市"); comboBox2.Items.Add("武威市"); comboBox2.Items.Add("陇南市"); comboBox2.Items.Add("定西市"); comboBox2.Items.Add("金昌市"); comboBox2.Items.Add("嘉峪关市");
            }

            else if (this.comboBox1.Text == "青海省")
            {
                comboBox2.Items.Add("全部城市"); comboBox2.Items.Add("西宁市");
            }
            else if (this.comboBox1.Text == "宁夏回族自治区")
            {
                comboBox2.Items.Add("全部城市"); comboBox2.Items.Add("银川市");
            }
            else if (this.comboBox1.Text == "新疆维吾尔自治区")
            {
                comboBox2.Items.Add("全部城市"); comboBox2.Items.Add("乌鲁木齐市");
            }

            #endregion
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            ListViewItem listViewItem = this.listView30.Items.Add((listView1.Items.Count + 1).ToString());
            listViewItem.SubItems.Add(comboBox1.Text + comboBox2.Text);
            listViewItem.SubItems.Add(listView29.SelectedItems[0].SubItems[0].Text);
            listViewItem.SubItems.Add("1");

            string path = AppDomain.CurrentDomain.BaseDirectory;
            StreamReader sr = new StreamReader(path + "citys.txt", Encoding.Default);
            //一次性读取完 
            string texts = sr.ReadToEnd();
            MatchCollection pnames = Regex.Matches(texts, @"PROVINCE_NAME"": ""([\s\S]*?)""");
            MatchCollection pcodes = Regex.Matches(texts, @"ESS_PROVINCE_CODE"": ""([\s\S]*?)""");

            MatchCollection cnames = Regex.Matches(texts, @"CITY_NAME"": ""([\s\S]*?)""");
            MatchCollection ccodes = Regex.Matches(texts, @"ESS_CITY_CODE"": ""([\s\S]*?)""");
            for (int i = 0; i < pnames.Count; i++)
            {
                if (comboBox1.Text.Replace("省", "") == pnames[i].Groups[1].Value)
                {
                    listViewItem.SubItems.Add(pcodes[i].Groups[1].Value);
                }
            }


            for (int j = 0; j< cnames.Count; j++)
            {
                if (comboBox2.Text == cnames[j].Groups[1].Value)
                {
                    listViewItem.SubItems.Add(ccodes[j].Groups[1].Value);
                }
            }








        }

        private void Button3_Click(object sender, EventArgs e)
        {

            listView1.Items.Clear(); listView13.Items.Clear();
            listView2.Items.Clear(); listView14.Items.Clear();
            listView3.Items.Clear(); listView15.Items.Clear();
            listView4.Items.Clear(); listView16.Items.Clear();
            listView5.Items.Clear(); listView17.Items.Clear();
            listView6.Items.Clear(); listView18.Items.Clear();
            listView7.Items.Clear(); listView19.Items.Clear();
            listView8.Items.Clear(); listView20.Items.Clear();
            listView9.Items.Clear(); listView21.Items.Clear();
            listView10.Items.Clear(); listView22.Items.Clear();
            listView11.Items.Clear(); listView23.Items.Clear();
            listView12.Items.Clear(); listView24.Items.Clear();
            listView25.Items.Clear(); listView26.Items.Clear();
            listView27.Items.Clear(); listView28.Items.Clear();
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            
            zanting = false;
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            method.ListviewToTxt(listView1,0);
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            宽带拨号 bh = new 宽带拨号();
            bh.Show();
        }

      
        private void ListView1_MouseClick(object sender, MouseEventArgs e)
        {
            Clipboard.SetText(listView1.SelectedItems[0].SubItems[0].Text);

        }
       
    }
}
