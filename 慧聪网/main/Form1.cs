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

namespace main
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //http://168s.mobile.hc360.com/get168.cgi?fc=0&e=100&n=0&z=%E4%B8%AD%E5%9B%BD:%E6%B1%9F%E8%8B%8F%E7%9C%81%3A%E5%AE%BF%E8%BF%81%E5%B8%82&v=609&s_id=001%3B003&gs=37&w=%E5%A9%9A%E7%BA%B1
        bool zanting = true;

        #region  主程序

        public void run()
        {

            try
            {
           
                string[] keywords = textBox3.Text.Trim().Split(',');

               string city= System.Web.HttpUtility.UrlEncode("中国:江苏省");
                foreach (string keyword in keywords)
                    {

                    if (keyword == "")
                    {
                        MessageBox.Show("请输入采集行业或者关键词！");
                        return;
                    }

                    string key = System.Web.HttpUtility.UrlEncode(keyword);

                    textBox2.Text = key;

                        for (int i = 1; i < 9999; i++)
                        {

                        string Url = "http://168s.mobile.hc360.com/get168.cgi?fc=0&e=100&n=" + i + "00&z="+city+"&v=609&s_id=001%3B003&gs=37&w=" + key;
                        textBox1.Text = Url;


                        string strhtml = method.GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()

                        MatchCollection names = Regex.Matches(strhtml, @"searchResultfoTitle"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        MatchCollection tels = Regex.Matches(strhtml, @"searchResultfoText"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        MatchCollection areas = Regex.Matches(strhtml, @"searchResultfoZone"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        MatchCollection address = Regex.Matches(strhtml, @"searchResultfoAddress"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        MatchCollection tips = Regex.Matches(strhtml, @"searchResultfoTp"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        if (names.Count == 0)

                            break;

                        for (int j = 0; j < 100; j++)
                        {

                            if (names.Count > 0)
                            {
                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                                lv1.SubItems.Add(names[j].Groups[1].Value.Trim());


                            }

                        }

                        if (listView1.Items.Count - 1 > 1)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);
                        }

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);   //内容获取间隔，可变量




                    }
                }
                

            }
            catch (System.Exception ex)
            {
                ex.ToString();
            }
        }


        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
        }
    }
}
