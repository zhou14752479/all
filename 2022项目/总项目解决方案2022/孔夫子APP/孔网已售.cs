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

namespace 孔夫子APP
{
    public partial class 孔网已售 : Form
    {
        public 孔网已售()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入ISBN");
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

        private void button1_Click(object sender, EventArgs e)
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


        string COOKIE = "Hm_lvt_33be6c04e0febc7531a1315c9594b136=1713963270; shoppingCartSessionId=bf96c8533a0bdd06e8e47556884cf211; reciever_area=1001000000; _c_WBKFRo=U3qserpq0pualfM8Oe81fuT0H32buIxGel5Yg9Vm; Hm_lvt_bca7840de7b518b3c5e6c6d73ca2662c=1722939174,1723463095; acw_tc=1a0c660217261443094761967e01365625fb7ed75cb352bbe4aafeda0a115b; kfz_uuid=4b4174a1-ffe1-40b4-867b-f1a1220efbdf; PHPSESSID=48f588ffb721ab4e30f9dd39bd803769a02e199a; kfz_trace=4b4174a1-ffe1-40b4-867b-f1a1220efbdf|16134930|5ac1e115e633cb46|-";
        public void run()
        {

            try
            {




                StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {

                    try
                    {

                        string isbn = text[i].ToString();

                        if (isbn.Trim() == "")
                            break;
                        string url = "https://search.kongfz.com/pc-gw/search-web/client/pc/product/keyword/list?dataType=1&keyword="+isbn+"&sortType=10&page=1&actionPath=sortType&userArea=1001000000";


                        string html = method.GetUrlWithCookie(url, COOKIE, "utf-8");

                        string sale = Regex.Match(html, @"""totalFoundText"":""([\s\S]*?)""").Groups[1].Value;
                        MatchCollection times = Regex.Matches(html, @"""showTimeText"":""([\s\S]*?)""");
                       
                       string time1= times.Count > 0 ?  times[0].Groups[1].Value.ToString().Replace("已售","") : "无";
                        string time2 = times.Count > 1 ? times[1].Groups[1].Value.ToString().Replace("已售", "") : "无";
                        string time3 = times.Count > 2 ? times[2].Groups[1].Value.ToString().Replace("已售", "") : "无";

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(isbn);
                        lv1.SubItems.Add(sale);
                        lv1.SubItems.Add(time1);
                        lv1.SubItems.Add(time2);
                        lv1.SubItems.Add(time3);



                        if(times.Count==0)
                        {
                            Thread.Sleep(60000);
                            continue;
                        }

                        if (listView1.Items.Count > 2)
                        {
                            this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                        }
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;
                        label2.Text = "正在查询：" + isbn;
                        Random rand = new Random();
                        Thread.Sleep(rand.Next(800, 1200));

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        continue;

                    }
                }
                label2.Text = ("查询结束");
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.ToString());
            }
        }
        Thread thread;
        bool zanting = true;
        bool status = true;

        private void 孔网已售_FormClosing(object sender, FormClosingEventArgs e)
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

        private void 孔网已售_Load(object sender, EventArgs e)
        {

        }
    }
}
