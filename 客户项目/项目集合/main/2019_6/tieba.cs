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

namespace main._2019_6
{
    public partial class tieba : Form
    {
        public tieba()
        {
            InitializeComponent();
        }

        private void Tieba_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        bool zanting = true;

        #region 主程序
        public void run()
        {

            try
            {
                string[] keywords = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                foreach (string keyword in keywords)
                {
                    string key= System.Web.HttpUtility.UrlEncode(keyword, Encoding.GetEncoding("GB2312"));

                    string Url = "http://tieba.baidu.com/f?kw="+key;

                    string html = method.GetUrl(Url, "utf-8");
                    if (html == null)
                        break;
                    MatchCollection ids = Regex.Matches(html, @"data-tid='([\s\S]*?)'", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    ArrayList lists = new ArrayList();

                    foreach (Match id in ids)
                    {
                        lists.Add("http://tieba.baidu.com/p/" + id.Groups[1].Value);
                    }
                   
                    foreach (string list in lists)

                    {

                        string strhtml = method.GetUrl(list, "utf-8");
                        string[] Keys = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(list);

                       
                        foreach (string Key in Keys)
                        {
                           
                            if (strhtml.Trim().Contains(key.Trim()))
                            {
                                lv1.SubItems.Add("是");
                            }
                            else
                            {
                                lv1.SubItems.Add("否");

                            }
                        }

                       
                           
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }

                    }

                }
            }

            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }
    }
}
