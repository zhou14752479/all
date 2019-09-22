using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 商标查询
{
    public partial class shangBiao : Form
    {
        public shangBiao()
        {
            InitializeComponent();
        }

        bool zanting = true;
        #region  主程序
        public void run()
        {
            try
            
{
                StreamReader sr = new StreamReader(textBox1.Text, Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int a = 0; a < text.Length - 1; a++)
                {

                    string url = "https://api.ipr.kuaifawu.com/xcx/tmsearch/index";

             

                string postdata=textBox2.Text;
                  
              postdata=  Regex.Replace(postdata, @"\d{6,}",text[a].Trim());
                    
                    string html =method.PostUrl(url,postdata,"","utf-8");
                    
                        Match a1 = Regex.Match(html, @"""MarkName"":""([\s\S]*?)""");
                        Match a2 = Regex.Match(html, @"UnionTypeCode"":([\s\S]*?),");
                        Match a3 = Regex.Match(html, @"""StateDate2017"":""([\s\S]*?)""");
                        Match a4 = Regex.Match(html, @"""AppPerson"":""([\s\S]*?)""");
                        Match a5 = Regex.Match(html, @"""Addr"":""([\s\S]*?)""");  // 地址


                    Match a51 = Regex.Match(html, @"""Addr"":""([\s\S]*?)省");  // 省
                    Match a52 = Regex.Match(html, @"省([\s\S]*?)市");  // 市

                        Match a6 = Regex.Match(html, @"""AppDate"":""([\s\S]*?)""");
                        Match a7 = Regex.Match(html, @"""AgentName"":""([\s\S]*?)""");


                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据      
                        lv1.SubItems.Add(text[a]);
                        lv1.SubItems.Add(a1.Groups[1].Value);
                        lv1.SubItems.Add(a2.Groups[1].Value);
                        lv1.SubItems.Add(a3.Groups[1].Value);
                        lv1.SubItems.Add(a4.Groups[1].Value);
                        lv1.SubItems.Add("中国");
                        lv1.SubItems.Add(a51.Groups[1].Value);
                        lv1.SubItems.Add(a52.Groups[1].Value);
                        lv1.SubItems.Add(a5.Groups[1].Value);
                        lv1.SubItems.Add(a6.Groups[1].Value);
                        lv1.SubItems.Add(a7.Groups[1].Value);
                        

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        Thread.Sleep(1000);


                    }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion
        private void Button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void shangBiao_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }
    }
}
