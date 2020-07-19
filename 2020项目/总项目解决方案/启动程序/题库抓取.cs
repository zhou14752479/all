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
    public partial class 题库抓取 : Form
    {
        public 题库抓取()
        {
            InitializeComponent();
        }

        private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public string token = "VTJGc2RHVmtYMTlyQlN2RGJ5eE9xQnZzclNRRFcyK2NOTWdGN000bDl5TDJPT1B6L0RQTnpDd3Y1a3NVNCsxYWJaZXdIQ0wxYnNIUWdtMzdLdm96NGc9PSMxNTkyMjcwMDEzODY0";

        Dictionary<string, string> dic = new Dictionary<string, string>();
        public void getitems()
        {
            comboBox1.Items.Clear();
            dic.Clear();
            string url = "https://manfenzhengzhi.ixunke.cn/api/questions_member?myQBank=true&page=1&pageSize=10&app=true&token=" + token;   //改变pagesize获取全部，目前是10不是全部题库
            string html = method.GetUrl(url, "utf-8");

            MatchCollection items = Regex.Matches(html, @"\{""id"":([\s\S]*?)\,""title"":""([\s\S]*?)""");


            for (int i = 0; i < items.Count; i++)
            {

                comboBox1.Items.Add(items[i].Groups[2].Value);
                dic.Add(items[i].Groups[2].Value, items[i].Groups[1].Value);
            }

            //每次付费增加的题库
            dic.Add("刷千题---马原", "21");
            dic.Add("刷千题---史纲", "22");
            dic.Add("刷千题---思修", "23");
            dic.Add("刷千题---毛中特", "24");
            comboBox1.Items.Add("刷千题---马原");
            comboBox1.Items.Add("刷千题---史纲");
            comboBox1.Items.Add("刷千题---思修");
            comboBox1.Items.Add("刷千题---毛中特");


            dic.Add("30天70分马原", "33");
            dic.Add("30天70分毛中特", "34");
            dic.Add("30天70分思修", "35");
            dic.Add("30天70分史纲", "36");
            dic.Add("30天70分专项与综合训练", "37");
            comboBox1.Items.Add("30天70分马原");
            comboBox1.Items.Add("30天70分毛中特");
            comboBox1.Items.Add("30天70分思修");
            comboBox1.Items.Add("30天70分史纲");
            comboBox1.Items.Add("30天70分专项与综合训练");


        }

        Dictionary<string, string> dic2 = new Dictionary<string, string>();
        public void getitems2(string BankID)
        {
           
            comboBox2.Items.Clear();
            dic2.Clear();
            string url = "https://manfenzhengzhi.ixunke.cn/api/chapter?qBankId=" + BankID + "&app=true&token=" + token;
            string html = method.GetUrl(url, "utf-8");

            MatchCollection items = Regex.Matches(html, @"\{""id"":([\s\S]*?)\,""qBankId"":([\s\S]*?)\,""title"":""([\s\S]*?)""");


            for (int i = 0; i < items.Count; i++)
            {
                if (!dic2.ContainsKey(items[i].Groups[3].Value))
                {
                    comboBox2.Items.Add(items[i].Groups[3].Value);
                    dic2.Add(items[i].Groups[3].Value, items[i].Groups[1].Value);
                }
            }
        }






        public void run()
        {

            string bankId = dic[comboBox1.Text.Trim()];
            string chapterId = dic2[comboBox2.Text.Trim()];

            try
            {

                string src = System.Web.HttpUtility.UrlEncode(comboBox1.Text);
                if (comboBox1.Text == "全部")
                {
                    src = "";
                }


                string url = "https://manfenzhengzhi.ixunke.cn/api/question?app=true&token=" + token + "&qBankId=" + bankId + "&chapterId=" + chapterId + "&practise=1&studentAnswer=true";

               
                string html = method.GetUrl(url, "utf-8");
                MatchCollection questions = Regex.Matches(html, @"""stem"":""([\s\S]*?)""\,");

                MatchCollection options = Regex.Matches(html, @"""options"":\[([\s\S]*?)""\]");
                MatchCollection analysis = Regex.Matches(html, @"""analysis"":""([\s\S]*?)""\,");
                MatchCollection anwsers = Regex.Matches(html, @"""answer"":\[([\s\S]*?)\]");
                for (int j = 0; j < questions.Count; j++)
                {
                    try
                    {



                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(questions[j].Groups[1].Value);
                        lv1.SubItems.Add(anwsers[j].Groups[1].Value);
                        lv1.SubItems.Add(analysis[j].Groups[1].Value);

                        string[] option = options[j].Groups[1].Value.Split(new string[] { "\"," }, StringSplitOptions.None);
                        lv1.SubItems.Add(option[0]);
                        lv1.SubItems.Add(option[1]);
                        lv1.SubItems.Add(option[2]);
                        lv1.SubItems.Add(option[3]);
                    }
                    catch
                    {

                        continue;
                    }



                }


            }
            catch (Exception)
            {

                throw;
            }


        }
        private void 题库抓取_Load(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(getitems));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"tikuzhuaqu"))
            {
                button1.Enabled = false;
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion

        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            getitems2(dic[comboBox1.SelectedItem.ToString()]);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            listView1.Items.Clear();


        }

        private void Button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        public static void ceshi()
        {

            method.GetUrl("http://www.baidu.com", "utf-8");

            MessageBox.Show("1");

        }


        private void Timer1_Tick(object sender, EventArgs e)
        {
            //Thread thread = new Thread(new ThreadStart(ceshi));
            //thread.Start();
            //Control.CheckForIllegalCrossThreadCalls = false;
            ceshi();


        }
    }
}
