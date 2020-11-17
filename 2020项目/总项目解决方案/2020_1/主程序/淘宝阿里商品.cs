using System;
using System.Collections;
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

namespace 主程序
{
    public partial class 淘宝阿里商品 : Form
    {
        public 淘宝阿里商品()
        {
            InitializeComponent();
        }
        public static string COOKIE = "";
        #region 淘宝
        public void taobao(object url)
        {

            try
            {

                string html = method.GetUrlWithCookie(url.ToString(), COOKIE, "gbk");
                Match company = Regex.Match(html, @"请进入([\s\S]*?)的([\s\S]*?)实力");
                Match name = Regex.Match(html, @"<title>([\s\S]*?)-");

                MatchCollection main = Regex.Matches(html, @"<dl class=""J_Prop([\s\S]*?)</dl>");
                ArrayList xxs = new ArrayList();
 
                if (main.Count > 1)
                {
                    string zhu = main[0].Groups[1].Value;
                    string zhu1 = main[1].Groups[1].Value;

                    MatchCollection xuanxiangs1 = Regex.Matches(zhu, @"<span>([\s\S]*?)</span>");
                    MatchCollection xuanxiangs2 = Regex.Matches(zhu1, @"<span>([\s\S]*?)</span>");
                    foreach (Match match1 in xuanxiangs1)
                    {
                        foreach (Match match2 in xuanxiangs2)
                        {
                            xxs.Add(match1.Groups[1].Value+","+ match2.Groups[1].Value);
                        }
                    }

                }


                




                MatchCollection prices = Regex.Matches(html, @"\{""price"":""([\s\S]*?)""");

                MatchCollection skus = Regex.Matches(html, @"""skuId"":""([\s\S]*?)""");



                for (int i = 0; i < skus.Count; i++)
                {
                    try
                    {


                        string[] text = xxs[i].ToString().Split(new string[] { "," }, StringSplitOptions.None);

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(Regex.Replace(company.Groups[2].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(name.Groups[1].Value);
                        lv1.SubItems.Add(skus[i].Groups[1].Value);
                     
                        lv1.SubItems.Add(text[0]);
                        lv1.SubItems.Add(text[1]);

                        lv1.SubItems.Add(prices[i].Groups[1].Value);
                    }
                    catch
                    {

                        continue;
                    }
                }





            }


            catch (System.Exception ex)
            {

              MessageBox.Show( ex.ToString());
            }

        }

        #endregion

        #region 天猫
        public void tmall(object url)
        {

            try
            {

                string html = method.GetUrlWithCookie(url.ToString(), COOKIE,"gbk");
                Match company = Regex.Match(html, @"<strong>([\s\S]*?)</strong>");
                Match name = Regex.Match(html, @"<title>([\s\S]*?)-");

                Match main = Regex.Match(html, @"skuList([\s\S]*?)salesProp");
                string zhu = main.Groups[1].Value;


                MatchCollection xuanxiangs = Regex.Matches(zhu, @"""names"":""([\s\S]*?)""");
                MatchCollection prices = Regex.Matches(zhu, @"""price"":""([\s\S]*?)""");

                MatchCollection skus = Regex.Matches(zhu, @"""skuId"":""([\s\S]*?)""");



                for (int i = 0; i < xuanxiangs.Count; i++)
                {
                    try
                    {
                        string[] text = xuanxiangs[i].Groups[1].Value.Split(new string[] { " " }, StringSplitOptions.None);



                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(company.Groups[1].Value);
                        lv1.SubItems.Add(name.Groups[1].Value);
                    lv1.SubItems.Add(skus[i].Groups[1].Value);
                    lv1.SubItems.Add(text[0]);
                    lv1.SubItems.Add(text[1]);
                    
                    lv1.SubItems.Add(prices[i].Groups[1].Value);
                    }
                    catch
                    {

                        continue;
                    }
                }





            }


            catch (System.Exception ex)
            {

                ex.ToString();
            }

        }

        #endregion

        #region 阿里巴巴
        public void alibab(object url)
        {

            try
            {

                string html = method.GetUrlWithCookie(url.ToString(), COOKIE,"gb2312");
                textBox1.Text = html;
                Match company = Regex.Match(html, @"company-name"">([\s\S]*?)<");
                Match name = Regex.Match(html, @"<h1 class=""d-title"">([\s\S]*?)</h1>");

                Match  main = Regex.Match(html, @"skuMap([\s\S]*?)\{([\s\S]*?)end");
                string zhu = "}," + main.Groups[2].Value;
              
              
                MatchCollection xuanxiangs = Regex.Matches(zhu, @"\},""([\s\S]*?)""");
                MatchCollection prices = Regex.Matches(zhu,@"""price"":""([\s\S]*?)""");
                
                MatchCollection skus = Regex.Matches(zhu, @"""skuId"":([\s\S]*?)\}");
              

              
                for (int i = 0; i < xuanxiangs.Count; i++)
                {
                    try
                    {

                        string[] text = xuanxiangs[i].Groups[1].Value.Split(new string[] { "&gt;" }, StringSplitOptions.None);



                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(company.Groups[1].Value);
                        lv1.SubItems.Add(name.Groups[1].Value);
                        lv1.SubItems.Add(skus[i].Groups[1].Value);
                        lv1.SubItems.Add(text[0]);
                        lv1.SubItems.Add(text[1]);
                        
                        lv1.SubItems.Add(prices[i].Groups[1].Value);
                       
                    }
                    catch
                    {

                        continue;
                    }

                }





            }


            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion
        private void 淘宝阿里商品_Load(object sender, EventArgs e)
        {

        }

        public void run()
        {
            COOKIE = helper.Form1.cookie;
            string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                if (radioButton1.Checked == true)
                {
                    tmall(text[i]);

                }
                else if (radioButton2.Checked == true)
                {
                    alibab(text[i]);

                }
                else if (radioButton3.Checked == true)
                {
                    taobao(text[i]);
                }

                Thread.Sleep(3000);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            
            listView1.Items.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            helper.Form1 fm1 = new helper.Form1();
            fm1.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                StreamReader streamReader = new StreamReader(this.openFileDialog1.FileName, Encoding.Default);
                string text = streamReader.ReadToEnd();
                string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    textBox1.Text += array[i] + "\r\n";

                }

            }
        }
    }
}
