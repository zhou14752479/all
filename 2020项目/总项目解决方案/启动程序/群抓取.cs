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
    public partial class 群抓取 : Form
    {
        public 群抓取()
        {
            InitializeComponent();
        }

        private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        bool zanting = true;
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
            try
            {
                string[] array = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] != "")
                    {


                        string url = "https://shopsearch.taobao.com/search?q=" + array[i] + "&js=1&initiative_id=staobaoz_20200328&ie=utf8";
                        string html =method.GetUrl(url, "utf-8");
                        Match Userid = Regex.Match(html, @"encryptedUserId\\"":\\""([\s\S]*?)\\");
                        Match goods = Regex.Match(html, @"""procnt"":([\s\S]*?),");


                        Match userid = Regex.Match(html, @"do\?userid=([\s\S]*?)""");
                        Match uid = Regex.Match(html, @"shop_id=([\s\S]*?)""");

                        Match sold = Regex.Match(html, @"""totalsold"":([\s\S]*?),");


                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据  
                        lv1.SubItems.Add(array[i]);
                        lv1.SubItems.Add(sold.Groups[1].Value);
                        lv1.SubItems.Add(goods.Groups[1].Value);
                       




                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        Thread.Sleep(2000);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }


        private void Button1_Click(object sender, EventArgs e)
        {

        }
    }
}
