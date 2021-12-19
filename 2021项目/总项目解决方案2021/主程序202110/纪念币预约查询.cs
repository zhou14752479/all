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
using myDLL;

namespace 主程序202110
{
    public partial class 纪念币预约查询 : Form
    {
        public 纪念币预约查询()
        {
            InitializeComponent();
        }

        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text=="")
            {
                MessageBox.Show("请导入数据");
                return;
            }

            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        public void run()
        {
            StreamReader sr = new StreamReader(openFileDialog1.FileName, method.EncodingType.GetTxtType(textBox1.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存
            for (int i = 0; i < text.Length; i++)
            {
                try
                {
                    string[] value = text[i].Split(new string[] { "#" }, StringSplitOptions.None);

                    string card = value[0].Trim();
                    string phone = value[1].Trim();
                    string url = "https://eapply.abchina.com/coin/coin/CoinSearchResult?issueid=I105";
                    string postdata = "certNo=" + card + "&phoneNo=" + phone + "&txtCaptchaCode=t2qm";
                    string html = method.PostUrlDefault(url, postdata, "");
                    string yuyue = "查询失败";
                    string xingming = "";
                    string name = "";
                    string shuliang = "";
                    string wangdian = "";
                    string shoujihao = "";
                    string addr = "";
                    if (html.Contains("预约成功"))
                    {
                        yuyue = "预约成功";

                        xingming = Regex.Match(html, @"name"" type=""text"" value=""([\s\S]*?)""").Groups[1].Value; ;
                        xingming = Regex.Replace(xingming, "<[^>]+>", "").Trim();


                        name = Regex.Match(html, @"名称：([\s\S]*?)</td>([\s\S]*?)</td>").Groups[2].Value; ;
                        name = Regex.Replace(name, "<[^>]+>", "").Trim();

                        shuliang = Regex.Match(html, @"数量：([\s\S]*?)</td>([\s\S]*?)</td>").Groups[2].Value; ;
                        shuliang = Regex.Replace(shuliang, "<[^>]+>", "").Trim();

                        wangdian = Regex.Match(html, @"兑换网点：([\s\S]*?)</td>([\s\S]*?)</td>").Groups[2].Value; ;
                        wangdian = Regex.Replace(wangdian, "<[^>]+>", "").Trim();

                        shoujihao = Regex.Match(html, @"手机号码：([\s\S]*?)</td>([\s\S]*?)</td>").Groups[2].Value; ;
                        shoujihao = Regex.Replace(shoujihao, "<[^>]+>", "").Trim();

                        addr = Regex.Match(html, @"网点地址：([\s\S]*?)</td>([\s\S]*?)</td>").Groups[2].Value; ;
                        addr = Regex.Replace(addr, "<[^>]+>", "").Trim();

                    }
                    if (html.Contains("没有该预约"))
                    {
                        yuyue = "没有该预约";
                    }
                    if (status == false)
                        return;
                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(card);
                    lv1.SubItems.Add(phone);
                    lv1.SubItems.Add(yuyue);
                    lv1.SubItems.Add(name);
                    lv1.SubItems.Add(shuliang);
                    lv1.SubItems.Add(wangdian);
                    lv1.SubItems.Add(xingming);
                    lv1.SubItems.Add(shoujihao);
                    lv1.SubItems.Add(addr);
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {

                    continue;
                }
            }
            MessageBox.Show("完成");
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                textBox1.Text = openFileDialog1.FileName;
               
            }
        }
        bool status = true;
        private void button7_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 纪念币预约查询_Load(object sender, EventArgs e)
        {

        }
    }
}
