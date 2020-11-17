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

namespace 主程序202008
{
    public partial class 华强美妆网 : Form
    {
        public 华强美妆网()
        {
            InitializeComponent();
        }


        string areaId = "47";
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            switch (comboBox1.Text)
            {

                case "明通":
                    comboBox2.Items.Add("1F");
                    comboBox2.Items.Add("2F");
                    comboBox2.Items.Add("3F");
                    comboBox2.Items.Add("4F");
                    comboBox2.Items.Add("5F");
                    break;
                case "曼哈":
                    comboBox2.Items.Add("1F");
                    comboBox2.Items.Add("2F");
                    break;
                case "紫荆城":
                    comboBox2.Items.Add("1F");
                    comboBox2.Items.Add("2F");
                    break;
                case "明通B区":
                    comboBox2.Items.Add("1F");
                    comboBox2.Items.Add("2F");
                    comboBox2.Items.Add("3F");
                    comboBox2.Items.Add("4F");
                    break;
                case "龙胜":
                    comboBox2.Items.Add("1F");
                    comboBox2.Items.Add("2F");
                   
                    break;
                case "远望":
                    comboBox2.Items.Add("1F");
                    comboBox2.Items.Add("2F");
                    break;



            }



        }
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
            string url = "https://cytkj.cn/index.php?_m=diy/market&id="+areaId;

            string html = method.GetUrl(url, "utf-8");

            MatchCollection urls = Regex.Matches(html, @"store&id=([\s\S]*?)""");


            foreach (Match uid in urls)
            {
                try
                {
                    string URL = "https://cytkj.cn/index.php?_m=diy/store&id=" + uid.Groups[1].Value;
                    string telhtml = method.GetUrl(URL + "&type=info", "utf-8");
                    Thread.Sleep(1000);
                    string strhtml = method.GetUrl(URL, "utf-8");

                    Match title = Regex.Match(telhtml, @"<div class=""info-shop-name"">([\s\S]*?)</div>");
                    MatchCollection tels = Regex.Matches(telhtml, @"<span class=""info-item-value"">([\s\S]*?)</span>");



                    Match price= Regex.Match(strhtml, @"<li style=""line-height:20px;height:auto;"">([\s\S]*?)</li>");
                    Match image = Regex.Match(telhtml, @"<div style=""text-align:center;padding-bottom:150px;""><img src=""([\s\S]*?)""");

                    string xiazai = "";
                    if (image.Groups[1].Value == "")
                    {
                        xiazai = "无";

                    }
                    else
                    {
                        method.downloadFile("https://cytkj.cn" + image.Groups[1].Value, AppDomain.CurrentDomain.BaseDirectory+"二维码\\",title.Groups[1].Value+".png","");
                        xiazai = "已下载";
                    }

                   

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(title.Groups[1].Value);
                    lv1.SubItems.Add(Regex.Replace(tels[0].Groups[1].Value, "<[^>]+>", ""));
                    lv1.SubItems.Add(tels[1].Groups[1].Value.Replace("<span>", "").Replace("，", ""));
                    lv1.SubItems.Add(tels[2].Groups[1].Value);
                    lv1.SubItems.Add(price.Groups[1].Value);
                    lv1.SubItems.Add(xiazai);

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                }
                catch (Exception)
                {

                    continue;
                }
                

                Thread.Sleep(1000);

            }

        }

        bool zanting = true;


        private void button1_Click(object sender, EventArgs e)
        {
            
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"ceshi1111111"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion
            switch (comboBox1.Text)
            {

                case "明通":
                    switch (comboBox2.Text)
                    {
                        case "1F":
                            areaId = "47";
                            break;
                        case "2F":
                            areaId = "48";
                            break;
                        case "3F":
                            areaId = "49";
                            break;
                        case "4F":
                            areaId = "60";
                            break;
                        case "5F":
                            areaId = "63";
                            break;
                    }
                    break;
                case "曼哈":
                    switch (comboBox2.Text)
                    {
                        case "1F":
                            areaId = "58";
                            break;
                        case "2F":
                            areaId = "59";
                            break;
                     
                    }
                    break;
                case "紫荆城":
                    switch (comboBox2.Text)
                    {
                        case "1F":
                            areaId = "50";
                            break;
                        case "2F":
                            areaId = "62";
                            break;

                    }
                    break;
                case "明通B区":
                    switch (comboBox2.Text)
                    {
                        case "1F":
                            areaId = "65";
                            break;
                        case "2F":
                            areaId = "66";
                            break;
                        case "3F":
                            areaId = "67";
                            break;
                        case "4F":
                            areaId = "71";
                            break;
                      
                    }
                    break;
                case "龙胜":
                    switch (comboBox2.Text)
                    {
                        case "1F":
                            areaId = "69";
                            break;
                        case "2F":
                            areaId = "70";
                            break;

                    }

                    break;
                case "远望":
                    switch (comboBox2.Text)
                    {
                        case "1F":
                            areaId = "73";
                            break;
                        case "2F":
                            areaId = "74";
                            break;

                    }
                    break;



            }

           
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
            button1.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
           method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void 华强美妆网_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确认退出吗？", "退出询问"
          , MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK)
            {
                e.Cancel = true;//告诉窗体关闭这个任务取消

            }
            else
            {
         

                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        
    }

        private void button5_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
           listView1.Items.Clear();
        }

        private void 华强美妆网_Load(object sender, EventArgs e)
        {

        }
    }
}
