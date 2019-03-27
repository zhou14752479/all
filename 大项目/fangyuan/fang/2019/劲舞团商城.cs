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

namespace fang._2019
{
    public partial class 劲舞团商城 : Form
    {
        public 劲舞团商城()
        {
            InitializeComponent();
            webBrowser1.ScriptErrorsSuppressed = true;
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        bool zanting = true;

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {
                    textBox1.Text += text[i] + "\r\n";             
                }
            }
        }

        #region  主函数
        public void run()

        {
            string[] users = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            try

            {
                foreach (string user in users)
                {


                    webBrowser1.Url = new Uri("http://pay.9you.com/");

                    HtmlDocument doc = this.webBrowser1.Document;
                    HtmlElementCollection es = doc.GetElementsByTagName("input"); //GetElementsByTagName返回集合

                    foreach (HtmlElement e1 in es)
                    {

                        if (e1.GetAttribute("name").ToLower() == "userName")
                        {
                            e1.SetAttribute("value", user);

                        }

                        if (e1.GetAttribute("name").ToLower() == "password")
                        {
                            e1.SetAttribute("value", "qgmm123");

                        }
                        HtmlElementCollection ds = doc.GetElementsByTagName("a");
                        foreach (HtmlElement d1 in ds)
                        {
                            if (d1.GetAttribute("class").ToLower() == "loginbtn")
                            {
                                d1.InvokeMember("click");

                            }

                        }
                    }









                    //        string url = "http://shop.9you.com/NewDaily/";
                    //MatchCollection titles = Regex.Matches(method.GetUrl(url, "utf-8"), @"<div class=""describe"">([\s\S]*?)</p>");


                    //ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    //lv1.SubItems.Add(url);
                    //lv1.SubItems.Add(titles[0].Groups[1].Value);
                    //lv1.SubItems.Add(titles[1].Groups[1].Value);
                    //lv1.SubItems.Add(titles[2].Groups[1].Value);

                    //if (listView1.Items.Count - 1 > 1)
                    //{
                    //    listView1.EnsureVisible(listView1.Items.Count - 1);
                    //}
                    ////如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。

                    //while (this.zanting == false)
                    //{
                    //    Application.DoEvents();
                    //}


                }
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }
    }
}
