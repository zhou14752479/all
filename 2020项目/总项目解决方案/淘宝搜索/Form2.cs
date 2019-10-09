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

namespace 淘宝搜索
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public static string COOKIE;
        bool zanting = true;
        #region 淘宝搜索
        public void run()
        {
            try

            {

                for (int i = 0; i < 4401; i=i+44)
                {
                    string url = "";

                    string html = method.GetUrlWithCookie(url,COOKIE, "utf-8");


                    if (html.Contains("小红书登录"))
                    {
                        
                       
                    }

                 
                    MatchCollection titles = Regex.Matches(html, @"{""rawtitle"":""([\s\S]*?)""");

                    for (int j = 0; j < titles.Count; j++)
                    {
                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(titles[j].Groups[1].Value);

                    }


                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }


                }
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {

        }
    }
}
