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
    public partial class 高德地图企业查询 : Form
    {
        public 高德地图企业查询()
        {
            InitializeComponent();
        }
        bool zanting = true;
        Thread thread;
        bool status = true;

        #region 高德地图(上限17*50)
        public void gaode()
        {

            string[] keywords = textBox1.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None); ;
            if (keywords.Length == 0)
            {
                MessageBox.Show("请添加关键字");
                return;
            }


            try
            {

                foreach (string keyword in keywords)
                {

                    string url = "https://restapi.amap.com/v3/place/text?s=rsv3&children=&key=c51aeb4379b19d99f34f409cf5c57410&offset=50&page=1&extensions=all&city=&language=zh_cn&callback=jsonp_814675_&platform=JS&logversion=2.0&appname=about%3Ablank&csid=B222C126-1764-4373-AFB9-C0C4ADF1F546&sdkversion=1.4.16&keywords=" + System.Web.HttpUtility.UrlEncode(keyword);
                    string html = method.GetUrl(url, "utf-8");

                    string name = Regex.Match(html, @"""name"":""([\s\S]*?)""").Groups[1].Value;
                    string phone = Regex.Match(html, @"""tel"":""([\s\S]*?)""").Groups[1].Value;
                    string address = Regex.Match(html, @"""address"":""([\s\S]*?)""").Groups[1].Value;

                    string pname = Regex.Match(html, @"""pname"":([\s\S]*?)""").Groups[1].Value;
                    string city = Regex.Match(html, @"""cityname"":""([\s\S]*?)""").Groups[1].Value;
                    string adname = Regex.Match(html, @"""adname"":""([\s\S]*?)""").Groups[1].Value;

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                    lv1.SubItems.Add(name);
                    lv1.SubItems.Add(phone.Replace("[]",""));
                    lv1.SubItems.Add(pname + city + adname + address);

                    if (status == false)
                        return;
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    label1.Text = DateTime.Now.ToShortTimeString() + "正在查询："+keyword;
                    Thread.Sleep(800);
                }

              




            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }

        }

        #endregion



        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, method.EncodingType.GetTxtType(textBox1.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {
                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(text[i]);
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(gaode);
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
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void 高德地图企业查询_Load(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"HyWW"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion
        }

        private void 高德地图企业查询_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }
    }
}
