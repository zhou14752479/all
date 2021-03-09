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
using myDLL;
namespace 同城58
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        

       

        public ArrayList getCitys(string city)
        {
          
            ArrayList list = new ArrayList();
            if (city.Contains("市"))
            {
                city = city.Replace("市","");
            }
            string url = "http://www.hao828.com/chaxun/zhongguochengshijingweidu/index.asp?key=" + System.Web.HttpUtility.UrlEncode(city, Encoding.GetEncoding("GB2312"))+ "&submit=%B2%E9%D1%AF";
            string html = method.GetUrl(url,"gb2312");
           
            MatchCollection areas = Regex.Matches(html, @"<td align=""center"">([\s\S]*?)</td>");
           
            for (int i = 0; i < areas.Count/8; i++)
            {

                list.Add(areas[(8*i)+2].Groups[1].Value+"#"+ areas[(8 * i) + 6].Groups[1].Value+"#"+ areas[(8 * i) + 7].Groups[1].Value);
               
            }
            return list;
        }

        public void run()
        {
            try
            {
                ArrayList citys = getCitys(textBox1.Text);

                foreach (string city in citys)
                {
                    Random rd1 = new Random();
                    double lagAdd = rd1.NextDouble();
                  


                   textBox3.Text += DateTime.Now.ToShortTimeString() + "：正在抓取" + textBox1.Text + "房产信息" + "\r\n";
                    string[] text = city.Split(new string[] { "#" }, StringSplitOptions.None);
                    if (text.Length > 2)
                    {
                        string url = "https://yaofa.58.com/search/card?xei4i=undefined&latitude=" + text[2] + "&longitude=" + text[1] + "&size=5555";
                        string html = method.GetUrl(url, "utf-8");

                        MatchCollection names = Regex.Matches(html, @"""realName"":""([\s\S]*?)""");
                        MatchCollection phones = Regex.Matches(html, @"""mobile"":""([\s\S]*?)""");
                        MatchCollection address = Regex.Matches(html, @"""address"":""([\s\S]*?)""");
                        Random rd = new Random();
                        int suiji = rd.Next(5,25);



                        for (int i = suiji; i < names.Count; i++)
                        {

                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(names[i].Groups[1].Value.Replace("经理","先生").Replace("师傅", "先生").Replace("老师", "先生"));
                            lv1.SubItems.Add(phones[i].Groups[1].Value);
                            lv1.SubItems.Add(address[i].Groups[1].Value);
                            lv1.SubItems.Add(textBox1.Text);

                            lv1.SubItems.Add(text[0]);

                            Thread.Sleep(1000);
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)
                                return;
                        }



                    }
                    else
                    {
                        textBox3.Text += DateTime.Now.ToShortTimeString() + "：城市输入错误";
                    }
                }
            }
            catch (Exception ex)
            {

                textBox3.Text=ex.ToString();
            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        Thread thread;
        bool zanting = true;
        bool status = true;

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"58shangjia"))
            {
                MessageBox.Show("");
                return;
            }



            #endregion


            status = true;
            if (thread == null || !thread.IsAlive)
            {

                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {
                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
