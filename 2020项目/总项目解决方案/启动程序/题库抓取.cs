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
        public void run()
        {
            try
            {

                string src = System.Web.HttpUtility.UrlEncode(comboBox1.Text);
                if (comboBox1.Text == "全部")
                {
                    src = "";
                }

   
                    string url = "https://manfenzhengzhi.ixunke.cn/api/question?app=true&token=VTJGc2RHVmtYMS9aQlpMZmJsV0ZjdWN0U0F5T0FIbXRaREhyWnB5RkREUTNRY0YwR1NWMjdjT1YvamdCVVIwelJDWE9aVmNBWU5MVFVvWFdUT2tyS2c9PSMxNTg1NzI2NDU3MzYw&qBankId=8&chapterId=83&practise=1&studentAnswer=true";


                    string html = method.GetUrl(url, "utf-8");
                    MatchCollection questions = Regex.Matches(html, @"""stem"":""([\s\S]*?)""\,");

                    MatchCollection options = Regex.Matches(html, @"""options"":\[([\s\S]*?)\]");
                    MatchCollection analysis = Regex.Matches(html, @"""analysis"":""([\s\S]*?)""\,");
                    MatchCollection anwsers = Regex.Matches(html, @"""answer"":\[([\s\S]*?)\]");
                    for (int j = 0; j < questions.Count; j++)
                    {
                        string[] option = options[j].Groups[1].Value.Split(new string[] { "," }, StringSplitOptions.None);


                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(questions[j].Groups[1].Value);
                        lv1.SubItems.Add(option[0]);
                        lv1.SubItems.Add(option[1]);
                        lv1.SubItems.Add(option[2]);
                        lv1.SubItems.Add(option[3]);
                        lv1.SubItems.Add(anwsers[j].Groups[1].Value);
                        lv1.SubItems.Add(analysis[j].Groups[1].Value);

                     }


            }
            catch (Exception)
            {

                throw;
            }


        }
        private void 题库抓取_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
