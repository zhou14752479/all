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
        public static string COOKIE = "t=db807e67e00d0d1d34419cc5686c31bc; cna=/h4DE4EwWXQCATFGW6Fbj0Jl; tg=0; ali_ab=121.234.247.249.1523710505053.9; miid=8025647021775416888; thw=cn; hng=CN%7Czh-CN%7CCNY%7C156; UM_distinctid=16a5312a75437a-0215e3ab7f0a66-5701631-1fa400-16a5312a755cf6; tracknick=zkg852266010; lgc=zkg852266010; v=0; cookie2=1995b919fa8b0372e7abe74c03b14806; _tb_token_=e3e7eb33e577a; _m_h5_tk=f799aa5019b19bbe60b88246875fb6aa_1561261572648; _m_h5_tk_enc=ff65210cb947f1d3355681b669e49ad8; supportWebp=false; dnk=zkg852266010; mt=ci=-1_0&np=; ucn=center; unb=1052347548; uc1=cookie16=VFC%2FuZ9az08KUQ56dCrZDlbNdA%3D%3D&cookie21=W5iHLLyFfXVRCJf5lG0u7A%3D%3D&cookie15=VT5L2FSpMGV7TQ%3D%3D&existShop=true&pas=0&cookie14=UoTaGd%2FpQyonzw%3D%3D&tag=8&lng=zh_CN; sg=080; _l_g_=Ug%3D%3D; skt=a1b1b878424bacad; cookie1=Vvj8uMJubtxirKFtxaDmWPxYCP5sb7EKtrFe1w68JDk%3D; csg=a3b68857; uc3=vt3=F8dBy3kWEhD3nB9Z6Yc%3D&id2=UoH62EAv27BqSg%3D%3D&nk2=GcOvCmiKUSBXqZNU&lg2=WqG3DMC9VAQiUQ%3D%3D; existShop=MTU2MTI1NjQ3NA%3D%3D; _cc_=U%2BGCWk%2F7og%3D%3D; _nk_=zkg852266010; cookie17=UoH62EAv27BqSg%3D%3D; l=bBP3PwQlvvuNKvJdBOCwourza77OSIRAguPzaNbMi_5C-6T_EL7OkX-dEF96Vj5RsYLB4G2npwJ9-etkw; x5sec=7b2277736d3b32223a223130373363356261366232623163303732333830316363306634333630393433434a764c752b67464549474375372f55726f72672b414561444445774e54497a4e4463314e4467374d513d3d227d; linezing_session=vcQFGegz8kkdWXn81NMx5DIp_1561257375600l8WY_24; isg=BO7uNOuVP9cRg0pEv9q5KUTmP0RwR7KOEkedUBi3ZvGs-49VgXso-dq5t2cXI6oB";
        #region 宝贝监控
        public void run()
        {

            try
            {

                foreach (ListViewItem item in listView1.Items)
                {
                    string url = "http://shop.m.taobao.com/shop/shop_search.htm?q="+ item.SubItems[1].Text;
                  
                    Match shopid = Regex.Match(method.gethtml(url, COOKIE, "utf-8"), @"shop_id=([\s\S]*?)""");

                    string Url = "https://shop"+shopid.Groups[1].Value+".taobao.com/category.htm";
                   
                    string html = method.gethtml(Url, COOKIE, "gb2312");
                    if (html == null)
                        break;
                    //Match title = Regex.Match(html, @"<span class=""shop-name-title"" title=""([\s\S]*?)""");  
                    Match midid = Regex.Match(html, @"mid=w-([\s\S]*?)-");

                     string curl = "https://shop"+shopid.Groups[1].Value+".taobao.com/i/asynSearch.htm?_ksTS=1561163488418_160&callback=jsonp161&mid=w-"+midid.Groups[1].Value+ "-0&wid=" + midid.Groups[1].Value + "&path=/search.htm&search=y&orderType=newOn_desc&viewType=grid&keyword=null&lowPrice=null&highPrice=null";

                    textBox1.Text = curl;
                    Match count = Regex.Match(method.gethtml(curl, COOKIE, "utf-8"), @"共搜索到<span>([\s\S]*?)</span>");
                    

                   textBox1.Text += item.SubItems[1].Text + "共有" + count.Groups[1].Value + "个宝贝" + "\r\n";

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

            StreamReader sr = new StreamReader(label3.Text, Encoding.Default);
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据    
                lv1.SubItems.Add(text[i]);
                lv1.SubItems.Add("未监控");
                lv1.SubItems.Add("0");
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

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            StreamReader sr = new StreamReader(label3.Text, Encoding.Default);
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据    
                lv1.SubItems.Add(text[i]);
                lv1.SubItems.Add("未监控");
                lv1.SubItems.Add("0");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void Button6_Click(object sender, EventArgs e)
        {

        }
    }
}
