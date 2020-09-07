using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 主程序202009
{
    public partial class 慧聪网 : Form
    {
        public 慧聪网()
        {
            InitializeComponent();
        }

        private void 慧聪网_Load(object sender, EventArgs e)
        {

        }

        bool zanting = true;
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {

            string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                for (int page = 1; page < 100; page++)
                {


                    string keyword = System.Web.HttpUtility.UrlEncode(text[i].ToString());
                    string url = "https://m.so.com/index.php?q=" + keyword + "&pn=" + page + "&psid=4d9d5dcc9b290c2a56eb9b1d3dba3237&src=srp_paging&fr=none";

                    string html = method.GetUrl(url, "utf-8");

                    MatchCollection urls = Regex.Matches(html, @"<a class=alink([\s\S]*?)u=([\s\S]*?)/");





                    foreach (Match yuming in urls)
                    {
                        try
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(yuming.Groups[2].Value);
                            lv1.SubItems.Add("1");


                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                        }

                        catch
                        {

                            continue;
                        }
                    }
                    Thread.Sleep(1000);
                }

            }
        }



    }
}
