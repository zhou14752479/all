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

namespace 模拟采集
{
    public partial class 广东5G查询 : Form
    {
        public 广东5G查询()
        {
            InitializeComponent();
        }

        private void 广东5G查询_Load(object sender, EventArgs e)
        {
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            
            webBrowser1.Navigate("https://apis.17wo.cn/flowq-web/#/personelLink/?peakChannel=PVD6erRqKSY%3D&peakCode=029227efda0324f67e83c3d339605e93");
        }
        private void webBrowser1_StatusTextChanged(object sender, EventArgs e)
        {
            textBox2.Text = webBrowser1.Document.Body.OuterHtml;
        }
       
        public void run()
        {
            
           
        }

        string nowtel = "";
        Thread thread;
        bool zanting = true;
        private void button1_Click(object sender, EventArgs e)
        {
            string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
           
            for (int i = 0; i < text.Length; i++)
            {
                HtmlDocument dc = webBrowser1.Document;
                HtmlElementCollection es = dc.GetElementsByTagName("input");   //GetElementsByTagName返回集合
                foreach (HtmlElement e1 in es)
                {
                    if (e1.GetAttribute("type") == "tel")
                    {

                        e1.Focus();
                        SendKeys.Send(text[i]);
                        nowtel = text[i];
                        MessageBox.Show(nowtel);
                        timer1.Enabled = true;
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        e1.SetAttribute("value", "");
                    }


                }

            }
            //Thread.Sleep(2000);
            //if (thread == null || !thread.IsAlive)
            //{
            //    thread = new Thread(run);
            //    thread.Start();
            //    Control.CheckForIllegalCrossThreadCalls = false;
            //}

        }

        private void button4_Click(object sender, EventArgs e)
        {
           
        }

        ArrayList lists = new ArrayList();
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (webBrowser1.StatusText == "完成")
            {
                zanting = false;
                timer1.Enabled = false;
                //页面加载完成,做一些其它的事
               string html = webBrowser1.Document.Body.OuterHtml;
                MatchCollection name = Regex.Matches(html, @"<p class=""shop_title"" data-v-46db6401="""">([\s\S]*?)</p>");
                for (int i = 0; i < name.Count; i++)
                {
                    if (!lists.Contains(nowtel))
                    {
                        lists.Add(nowtel);
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据  
                        lv1.SubItems.Add(nowtel);
                        lv1.SubItems.Add(name[i].Groups[1].Value);
                        //webBrowser1.DocumentText 注意不要用这个，这个和查看源文件一样的
                    }
                }
                zanting = true;
                timer1.Enabled = false;
                
            }
            

        }
    }
}
