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

namespace fang
{
    public partial class tianyancha : Form
    {
        public tianyancha()
        {
            InitializeComponent();
        }

        bool status = true;
        int page;
        #region  天眼查
        public void run()
        {

            

            if (textBox2.Text == "")
            {
                MessageBox.Show("请输入二级网址！");
                return;
            }

            try
            {
                string[] URLs = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                foreach (string URL in URLs)
                {


                    for (int i = 1; i < 251; i++)
                    {


                        string[] urls=URL.Split(new string[] { "?" }, StringSplitOptions.None);

                        string url = urls[0] + "p"+i+"?" + urls[1];

    
                        string strhtml = method.GetUrlWithCookie(url,"utf-8",textBox3.Text.Trim()); ;
                        
                        Match name = Regex.Match(strhtml, @""" target = '_blank'([\s\S]*?)</em></a>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        if(name.Groups[1].Value.Trim() == "")  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;
                        Match lxr = Regex.Match(strhtml, @"法定代表人：([\s\S]*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        Match tel= Regex.Match(strhtml, @"联系电话：</span><span>([\s\S]*?)</span>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        

                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                        lv1.SubItems.Add(Regex.Replace(name.Groups[1].Value.Trim(), "<[^>]*>", ""));
                        lv1.SubItems.Add(Regex.Replace(lxr.Groups[1].Value.Trim(), "<[^>]*>", ""));
                        lv1.SubItems.Add(Regex.Replace(tel.Groups[1].Value.Trim(), "<[^>]*>", ""));
                        


                        Application.DoEvents();
                        Thread.Sleep(Convert.ToInt32(1000));

                        if (listView1.Items.Count > 1)
                            {
                                listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                            }

                            while (this.status == false)
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

        private void tianyancha_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
