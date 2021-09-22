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

namespace 主程序202006
{
    public partial class 中策大数据 : Form
    {
        public 中策大数据()
        {
            InitializeComponent();
        }

        string cookie = "mediav=%7B%22eid%22%3A%22628495%22%2C%22ep%22%3A%22%22%2C%22vid%22%3A%22DhAOCGWoI%3A8ch%5D7QwUQB%22%2C%22ctn%22%3A%22%22%2C%22vvid%22%3A%22DhAOCGWoI%3A8ch%5D7QwUQB%22%2C%22_mvnf%22%3A1%2C%22_mvctn%22%3A0%2C%22_mvck%22%3A0%2C%22_refnf%22%3A0%7D; Hm_lvt_d2634fb106dc720564524e04dfd88bec=1625556915,1625624161; Qs_lvt_289675=1622682637%2C1625556914%2C1625624161; PHPSESSID=tgemm7rikdm8rlbphh66a88pbl; Qs_lvt_314176=1625625079; mediav=%7B%22eid%22%3A%22628495%22%2C%22ep%22%3A%22%22%2C%22vid%22%3A%22DhAOCGWoI%3A8ch%5D7QwUQB%22%2C%22ctn%22%3A%22%22%2C%22vvid%22%3A%22DhAOCGWoI%3A8ch%5D7QwUQB%22%2C%22_mvnf%22%3A1%2C%22_mvctn%22%3A0%2C%22_mvck%22%3A0%2C%22_refnf%22%3A0%7D; wap_china0001_com_cn_username=13338703444; wap_china0001_com_cn_userpwd=china0001; wap_china0001_com_cn_lasturl=http%3A%2F%2Fwap.china0001.com.cn%2Fproject%2F933254.html; Hm_lpvt_d2634fb106dc720564524e04dfd88bec=1625625193; Qs_pv_289675=977771833730040700%2C3523361108074497000%2C54918524870182856%2C2681212953383196000%2C3920035525615057000; Hm_lvt_a2cf3e9240bd7552da95f5c95c4346b0=1625625079,1625625127,1625625130,1625627349; Hm_lpvt_a2cf3e9240bd7552da95f5c95c4346b0=1625627349; Qs_pv_314176=1181253322396840200%2C1750478188412803800%2C1724953541381945300%2C2971107667881820700%2C335888415443476100";
        bool zanting = true;
        string area = "d17"; //江苏省
        string time = "h0";
        string jieduan = "";
        public void run()
        {
            if (radioButton2.Checked == true)
            {
                jieduan = "-g2";
            }
            if (radioButton3.Checked == true)
            {
                jieduan = "-g3";
            }
            if (radioButton4.Checked == true)
            {
                jieduan = "-g4";
            }
            if (radioButton5.Checked == true)
            {
                jieduan = "-g5";
            }





            switch (comboBox1.Text.Trim())
            {
                case "江苏省":
                    area = "d17";
                    break;
                case "南京市":
                    area = "ct114";
                    break;
                case "无锡市":
                    area = "ct119";
                    break;
                case "苏州市":
                    area = "ct116";
                    break;
                case "常州市":
                    area = "ct111";
                    break;
                case "镇江市":
                    area = "ct123";
                    break;
                case "南通市":
                    area = "ct115";
                    break;
                case "盐城市":
                    area = "ct119";
                    break;
                case "泰州市":
                    area = "ct121";
                    break;
                case "连云港市":
                    area = "ct113";
                    break;
                case "宿迁市":
                    area = "ct117";
                    break;
                case "徐州市":
                    area = "ct120";
                    break;
                case "淮安市":
                    area = "ct112";
                    break;
                case "扬州市":
                    area = "ct122";
                    break;


            }



            switch (comboBox3.Text.Trim())
            {
                case "全部":
                    time = "h0";
                    break;
                case "一周":
                    time = "h1";
                    break;
                case "一个月":
                    time = "h2";
                    break;
                case "三个月":
                    time = "h3";
                    break;
                case "半年":
                    time = "h4";
                    break;
                case "一年":
                    time = "h5";
                    break;
                case "两年":
                    time = "h6";
                    break;

            }
           
            for (int i = 1; i < 9999; i++)
            {

                label4.Text = "正在搜索，第" + i + "页";
                string url = "https://wap.china0001.com.cn/project/" + area+jieduan+"-"+time+"-no1-p"+i+"/";
             
                string html = method.GetUrlWithCookie(url, cookie,"utf-8");
             
                MatchCollection ids = Regex.Matches(html, @""" id=""pro_([\s\S]*?)""");
              
                if (ids.Count == 0)
                {
                    label4.Text = "结束";
                    return;
                }


                for (int j = 0; j < ids.Count; j++)

                {
                    string URL = "https://wap.china0001.com.cn/project/" + ids[j].Groups[1].Value+".html";

                    try
                    {
                      
                        string htmls = method.GetUrlWithCookie(URL, cookie, "utf-8");
                       // Match title = Regex.Match(htmls, @"<title>([\s\S]*?)联系人");

                        Match ahtml = Regex.Match(htmls, @"<B>业主单位联系方式</B>([\s\S]*?)</section>");
                        Match bhtml = Regex.Match(htmls, @"<B>设计院联系方式</B>([\s\S]*?)</section>");
                        Match chtml = Regex.Match(htmls, @"施工单位联系方式</div>([\s\S]*?)</section>");
                        Match dhtml = Regex.Match(htmls, @"<!-- 其他参建单位 -->([\s\S]*?)</section>");
                    

                        MatchCollection acompany = Regex.Matches(ahtml.Groups[1].Value, @"dw=""([\s\S]*?)""");
                        MatchCollection aname = Regex.Matches(ahtml.Groups[1].Value, @"xm=""([\s\S]*?)""");
                        MatchCollection ajob = Regex.Matches(ahtml.Groups[1].Value, @"zw=""([\s\S]*?)""");
                        MatchCollection atel = Regex.Matches(ahtml.Groups[1].Value, @"sj=""([\s\S]*?)""");

                        MatchCollection bcompany = Regex.Matches(bhtml.Groups[1].Value, @"dw=""([\s\S]*?)""");
                        MatchCollection bname = Regex.Matches(bhtml.Groups[1].Value, @"xm=""([\s\S]*?)""");
                        MatchCollection bjob = Regex.Matches(bhtml.Groups[1].Value, @"zw=""([\s\S]*?)""");
                        MatchCollection btel = Regex.Matches(bhtml.Groups[1].Value, @"sj=""([\s\S]*?)""");

                        MatchCollection ccompany = Regex.Matches(chtml.Groups[1].Value, @"dw=""([\s\S]*?)""");
                        MatchCollection cname = Regex.Matches(chtml.Groups[1].Value, @"xm=""([\s\S]*?)""");
                        MatchCollection cjob = Regex.Matches(chtml.Groups[1].Value, @"zw=""([\s\S]*?)""");
                        MatchCollection ctel = Regex.Matches(chtml.Groups[1].Value, @"sj=""([\s\S]*?)""");

                        MatchCollection dcompany = Regex.Matches(dhtml.Groups[1].Value, @"dw=""([\s\S]*?)""");
                        MatchCollection dname = Regex.Matches(dhtml.Groups[1].Value, @"xm=""([\s\S]*?)""");
                        MatchCollection djob = Regex.Matches(dhtml.Groups[1].Value, @"zw=""([\s\S]*?)""");
                        MatchCollection dtel = Regex.Matches(dhtml.Groups[1].Value, @"sj=""([\s\S]*?)""");


                        if (checkBox1.Checked == true)
                        {
                            for (int a = 0; a < acompany.Count; a++)
                            {
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add("甲方");
                                lv1.SubItems.Add(acompany[a].Groups[1].Value);
                                lv1.SubItems.Add(aname[a].Groups[1].Value);
                                lv1.SubItems.Add(atel[a].Groups[1].Value);
                            }
                        }

                        if (checkBox2.Checked == true)
                        {
                            for (int a = 0; a < bcompany.Count; a++)
                            {
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add("设计方");
                                lv1.SubItems.Add(bcompany[a].Groups[1].Value);
                                lv1.SubItems.Add(bname[a].Groups[1].Value);
                                lv1.SubItems.Add(btel[a].Groups[1].Value);
                            }
                        }
                        if (checkBox3.Checked == true)
                        {

                            for (int a = 0; a < ccompany.Count; a++)
                            {
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add("施工方");
                                lv1.SubItems.Add(ccompany[a].Groups[1].Value);
                                lv1.SubItems.Add(cname[a].Groups[1].Value);
                                lv1.SubItems.Add(ctel[a].Groups[1].Value);

                            }
                        }


                      

                        Thread.Sleep(1000);

                        while (zanting == false)
                        {
                            Application.DoEvents();//等待本次加载完毕才执行下次循环.
                        }
                    }
                    catch(Exception ex)
                    {

                        MessageBox.Show(ex.ToString());
                    }



                }
               

            }
            label4.Text = "查询结束";
        }



        private void 中策大数据_Load(object sender, EventArgs e)
        {

        }
        Thread thread;

        private void button1_Click(object sender, EventArgs e)
        {
           //cookie = Form1.cookie;
            label4.Text = "正在搜索，请勿重复点击......";
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Form1 fm1 = new Form1();
            //fm1.Show();
        }
    }
}
