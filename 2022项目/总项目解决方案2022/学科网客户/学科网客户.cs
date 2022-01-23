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

namespace 学科网客户
{
    public partial class 学科网客户 : Form
    {
        public 学科网客户()
        {
            InitializeComponent();
        }



        List<string> lists = new List<string>();
        public string cookie= "SESSION=MWU5NTE4YjMtOWJlMy00ZjIxLThhODgtZmVkZjNiNDQwNzg2";
        public void run()
        {

            try
            {
                string url = "http://sale.xkw.cn/api/school/query-temp";
                string html = method.PostUrlDefault(url, "dateType=0&mySelf=false&orderColumn=addTime&desc=true&pageNo=1&pageSize=20&_t=1642755316698", cookie);
                MatchCollection schoolNames = Regex.Matches(html, @"""schoolName"":""([\s\S]*?)""");
                MatchCollection htmls = Regex.Matches(html, @"addTime([\s\S]*?)syncUser");
                //MatchCollection schoolSites = Regex.Matches(html, @"""schoolSite"":""([\s\S]*?)""");

                MessageBox.Show(schoolNames.Count.ToString());
                for (int i = 0; i < schoolNames.Count; i++)
                {
                    string linkMan=  Regex.Match(htmls[i].Groups[1].Value, @"""schoolName"":""([\s\S]*?)""").Groups[1].Value;
                    string provinceId =Regex.Match(htmls[i].Groups[1].Value, @"""provinceId"":""([\s\S]*?)""").Groups[1].Value;
                    string cityId = Regex.Match(htmls[i].Groups[1].Value, @"""cityId"":""([\s\S]*?)""").Groups[1].Value;
                    string districtId = Regex.Match(htmls[i].Groups[1].Value, @"""districtId"":""([\s\S]*?)""").Groups[1].Value;
                    string linkMobile = Regex.Match(htmls[i].Groups[1].Value, @"""linkMobile"":""([\s\S]*?)""").Groups[1].Value;
                    string schoolAddress = Regex.Match(htmls[i].Groups[1].Value, @"""schoolAddress"":""([\s\S]*?)""").Groups[1].Value;

                    string schoolSite = Regex.Match(htmls[i].Groups[1].Value, @"""schoolSite"":""([\s\S]*?)""").Groups[1].Value;
                    string linkPhone = Regex.Match(htmls[i].Groups[1].Value, @"""linkPhone"":""([\s\S]*?)""").Groups[1].Value;
                    string schoolType = Regex.Match(htmls[i].Groups[1].Value, @"""schoolType"":([\s\S]*?),").Groups[1].Value;
                    if (linkMobile=="")
                    {
                        linkMobile = linkPhone;
                    }
                    if(schoolSite=="")
                    {
                        schoolSite = "http://www.baidu.com";
                    }

                    try
                    {
                        if (!lists.Contains(schoolNames[i].Groups[1].Value))
                        {



                            lists.Add(schoolNames[i].Groups[1].Value);
                            string aurl = "http://sale.xkw.cn/api/school/add-temp";
                            string apostdata = "schoolName=" + schoolNames[i].Groups[1].Value + "&schoolType="+ schoolType + "&stage=2&linkMan=" + linkMan + "&schoolAddress=" + schoolAddress + "&provinceId=" + provinceId + "&cityId=" + cityId + "&districtId=" + districtId + "&linkManJob=%E6%95%99%E5%B8%88&attachment=&priorityLevel=0&schoolSite="+ schoolSite + "&linkMobile=" + linkMobile + "&schoolLevel=0&_t=1642752791742";
                            string ahtml = method.PostUrlDefault(aurl, apostdata, cookie);
                            string mes = Regex.Match(ahtml, @"""message"":""([\s\S]*?)""").Groups[1].Value;
                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(schoolNames[i].Groups[1].Value);
                            lv1.SubItems.Add(mes);
                            Thread.Sleep(1000);

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        continue;
                    }
                }
            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }

        }
        private void 学科网客户_Load(object sender, EventArgs e)
        {

        }

        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测


            string html = method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"YYOa"))
            {

                return;
            }

            #endregion
            timer1.Start();
            timer1.Interval = Convert.ToInt32(textBox1.Text) * 1000 * 60;
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

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }
    }
}
