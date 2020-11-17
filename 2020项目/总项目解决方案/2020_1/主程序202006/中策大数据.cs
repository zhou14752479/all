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
    public partial class 中策大数据 : Form
    {
        public 中策大数据()
        {
            InitializeComponent();
        }

        string cookie = "mediav=%7B%22eid%22%3A%22628495%22%2C%22ep%22%3A%22%22%2C%22vid%22%3A%22)TCZU%23y5nR9*Q%23YaN0jp%22%2C%22ctn%22%3A%22%22%2C%22vvid%22%3A%22)TCZU%23y5nR9*Q%23YaN0jp%22%7D; _ga=GA1.3.339430833.1592791735; www_china0001_com_cn_mycity=17; _gid=GA1.3.1875224426.1593307322; Qs_lvt_289675=1592791735%2C1593307321%2C1593336583%2C1593409499; PHPSESSID=ai0piu145l5ksn9nntl6hc3pgv; Hm_lvt_d2634fb106dc720564524e04dfd88bec=1593307322,1593409500,1593427307,1593432458; www_china0001_com_cn_prolove=790733%2C790515%2C789981%2C789998%2C789999%2C789750%2C789770%2C789842%2C792483%2C779731; www_china0001_com_cn_username=13338703444; www_china0001_com_cn_userpwd=china0001; Hm_lpvt_d2634fb106dc720564524e04dfd88bec=1593433105; Qs_pv_289675=351266940976634200%2C2363216495513196000%2C3279605208580686000%2C822729104316573800%2C76753818617815460";
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
                string url = "https://www.china0001.com.cn/project/"+area+jieduan+"-"+time+"-no1-p"+i+"/";
                
                string html = method.GetUrlWithCookie(url, cookie,"utf-8");

                MatchCollection ids = Regex.Matches(html, @"id=""pro_([\s\S]*?)""");
                MessageBox.Show(ids.Count.ToString());
                if (ids.Count == 0)
                {
                    label4.Text = "结束";
                    return;
                }


                for (int j = 0; j < ids.Count; j++)

                {
                    string URL = "https://www.china0001.com.cn/project/"+ids[j].Groups[1].Value+".html";

                    try
                    {
                        string htmls = method.GetUrlWithCookie(URL, cookie, "utf-8");
                        Match title = Regex.Match(htmls, @"<title>([\s\S]*?)-");

                        Match ahtml = Regex.Match(htmls, @"<!-- 甲方单位联系方式 -->([\s\S]*?)<ul class=""rmktxt3"" >([\s\S]*?)<!--");
                        Match bhtml = Regex.Match(htmls, @"<!-- 设计单位联系方式 -->([\s\S]*?)<ul class=""rmktxt3"" >([\s\S]*?)</tr>");
                        Match chtml = Regex.Match(htmls, @"<!-- 施工方联系方式 -->([\s\S]*?)<ul class=""rmktxt3"" >([\s\S]*?)<!--");
                        Match dhtml = Regex.Match(htmls, @"<!-- 其他参建单位 -->([\s\S]*?)<ul class=""rmktxt3"" >([\s\S]*?)<!--");


                        MatchCollection acompany = Regex.Matches(ahtml.Groups[2].Value, @"data-title=""单位：([\s\S]*?)""");
                        MatchCollection aname = Regex.Matches(ahtml.Groups[2].Value, @"data-title=""姓名：([\s\S]*?)""([\s\S]*?)change-font"">([\s\S]*?)</a>");
                        


                        MatchCollection bcompany = Regex.Matches(bhtml.Groups[2].Value, @"data-title=""单位：([\s\S]*?)""");
                        MatchCollection bname = Regex.Matches(bhtml.Groups[2].Value, @"data-title=""姓名：([\s\S]*?)""([\s\S]*?)change-font"">([\s\S]*?)</a>");
                    

                        MatchCollection ccompany = Regex.Matches(chtml.Groups[2].Value, @"data-title=""单位：([\s\S]*?)""");
                        MatchCollection cname = Regex.Matches(chtml.Groups[2].Value, @"data-title=""姓名：([\s\S]*?)""([\s\S]*?)change-font"">([\s\S]*?)</a>");
                      

                        MatchCollection dcompany = Regex.Matches(dhtml.Groups[2].Value, @"data-title=""单位：([\s\S]*?)""");
                        MatchCollection dname = Regex.Matches(dhtml.Groups[2].Value, @"data-title=""姓名：([\s\S]*?)""([\s\S]*?)change-font"">([\s\S]*?)</a>");


                        if (checkBox1.Checked == true)
                        {
                            for (int a = 0; a < aname.Count; a++)
                            {
                                try
                                {
                                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                    lv1.SubItems.Add(title.Groups[1].Value);
                                    lv1.SubItems.Add("甲方单位");

                                    lv1.SubItems.Add(Regex.Replace(aname[a].Groups[1].Value, "<[^>]+>", "").Trim());
                                    lv1.SubItems.Add(Regex.Replace(aname[a].Groups[3].Value, "<[^>]+>", "").Trim());
                                    lv1.SubItems.Add(Regex.Replace(acompany[a].Groups[1].Value, "<[^>]+>", "").Trim());
                                }
                                catch (Exception ex)
                                {

                                    ex.ToString();
                                }
                            }
                        }
                        if (checkBox2.Checked == true)
                        {
                            for (int b = 0; b < bname.Count; b++)
                            {
                                try
                                {
                                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                    lv1.SubItems.Add(title.Groups[1].Value);
                                    lv1.SubItems.Add("设计单位");

                                    lv1.SubItems.Add(Regex.Replace(bname[b].Groups[1].Value, "<[^>]+>", "").Trim());
                                    lv1.SubItems.Add(Regex.Replace(bname[b].Groups[3].Value, "<[^>]+>", "").Trim());
                                    lv1.SubItems.Add(Regex.Replace(bcompany[b].Groups[1].Value, "<[^>]+>", "").Trim());
                                }
                                catch (Exception ex)
                                {

                                    ex.ToString();
                                }
                            }
                        }
                        if (checkBox3.Checked == true)
                        {
                            for (int c = 0; c < cname.Count; c++)
                            {
                                try
                                {
                                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                    lv1.SubItems.Add(title.Groups[1].Value);
                                    lv1.SubItems.Add("施工方");

                                    lv1.SubItems.Add(Regex.Replace(cname[c].Groups[1].Value, "<[^>]+>", "").Trim());
                                    lv1.SubItems.Add(Regex.Replace(cname[c].Groups[3].Value, "<[^>]+>", "").Trim());
                                    lv1.SubItems.Add(Regex.Replace(ccompany[c].Groups[1].Value, "<[^>]+>", "").Trim());
                                }
                                catch (Exception ex)
                                {

                                    ex.ToString();
                                }
                            }
                        }

                        if (checkBox4.Checked == true)
                        {

                            for (int d = 0; d < dname.Count; d++)
                            {
                                try
                                {
                                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                    lv1.SubItems.Add(title.Groups[1].Value);
                                    lv1.SubItems.Add("其他参建单位");

                                    lv1.SubItems.Add(Regex.Replace(dname[d].Groups[1].Value, "<[^>]+>", "").Trim());
                                    lv1.SubItems.Add(Regex.Replace(dname[d].Groups[3].Value, "<[^>]+>", "").Trim());
                                    lv1.SubItems.Add(Regex.Replace(dcompany[d].Groups[1].Value, "<[^>]+>", "").Trim());
                                }
                                catch (Exception ex)
                                {

                                    ex.ToString();
                                }
                            }
                        }



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

        private void button1_Click(object sender, EventArgs e)
        {
           // cookie = Form1.cookie;
            label4.Text = "正在搜索，请勿重复点击......";
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
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
            Form1 fm1 = new Form1();
            fm1.Show();
        }
    }
}
