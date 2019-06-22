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

namespace main._2019_6
{
    public partial class 淘宝宝贝上架监控 : Form
    {
        public 淘宝宝贝上架监控()
        {
            InitializeComponent();
        }
        bool status = true;
        bool zanting = true;
        private void 淘宝宝贝上架监控_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        }
        #region 宝贝监控
        public void run()
        {

            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                StreamReader sr = new StreamReader(label3.Text, Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {
                   
                    string Url = text[i] + "/category.htm";

                    string html = method.gethtml(Url, "", "utf-8");
                    if (html == null)
                        break;
                    Match title = Regex.Match(html, @"<span class=""shop-name-title"" title=""([\s\S]*?)""");  
                    Match midid = Regex.Match(html, @"mid=w-([\s\S]*?)-");

                //    string url = "https://fancys.taobao.com/i/asynSearch.htm?_ksTS=1561163488418_160&callback=jsonp161&mid=w-9841339055-0&wid=9841339055&path=/search.htm&search=y&orderType=newOn_desc&viewType=grid&keyword=null&lowPrice=null&highPrice=null";
                    //for (int j = 0; j < bodys.Count; j++)
                    //{
                    //    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据    
                    //    StringBuilder sb = new StringBuilder();
                    //    Match BODY = Regex.Match(bodys[j].Groups[2].Value, @">([^<]+)<");
                    //    sb.Append(BODY.Groups[0].Value.Replace(">", "").Replace("<", "").Trim());
                    //}

                    //lv1.SubItems.Add(title.Groups[1].Value);
                    //lv1.SubItems.Add(sb.ToString());
                   

                    if (listView1.Items.Count > 2)
                    {
                        listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                    }
                    Thread.Sleep(500);

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }


                    if (this.status == false)
                        return;





                }
            }

            catch (System.Exception ex)
            {

                ex.ToString();
            }

        }

        #endregion
        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                label3.Text = openFileDialog1.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            status = false;
        }
    }
}
