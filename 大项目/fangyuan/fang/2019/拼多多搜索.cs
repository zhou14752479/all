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
    public partial class 拼多多搜索 : Form
    {
        public 拼多多搜索()
        {
            InitializeComponent();
        }
        public static string COOKIE = "api_uid=rBQRpFvJizBVvw8WqeZ3Ag==; _nano_fp=XpdqlpUqnp9xn5ToXo_ke8~lQxmpH7B0sj51EhI7; ua=Mozilla%2F5.0%20(Windows%20NT%2010.0%3B%20Win64%3B%20x64)%20AppleWebKit%2F537.36%20(KHTML%2C%20like%20Gecko)%20Chrome%2F69.0.3497.81%20Safari%2F537.36; webp=1; msec=1800000; rec_list_catgoods=rec_list_catgoods_DSg1TO; pdd_user_uin=7BT7HSLWTMIXDJYZTOHT4Q45MQ_GEXDA; pdd_user_id=7312500755985; PDDAccessToken=N4ADUU4BJHRIWTYK46NGVVW2V4MK2CMYPVUOFEPDOYQGEMOEK7UQ102118b; rec_list_orders=rec_list_orders_K5JiGI; rec_list_index=rec_list_index_mMjl3a; rec_list_personal=rec_list_personal_HEFmke; goods_detail=goods_detail_TbEJdF; goods_detail_mall=goods_detail_mall_VrqfFl; JSESSIONID=5084011DF44EE7994BCE9DAAE640566A";
        private void timer1_Tick(object sender, EventArgs e)
        {

            //textBox3.Text = webBrowser.cookie;
            //COOKIE = textBox3.Text;
        }
        private void splitContainer1_Panel1_MouseEnter(object sender, EventArgs e)
        {

        }
        bool zanting = true;

        public string[] ReadText()
        {
            string currentDirectory = Environment.CurrentDirectory;
            StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.Default);
            string text = streamReader.ReadToEnd();
            return text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
        }
        public void run()
        {
            try
            {
                string[] array = this.ReadText();
                foreach (string keyword in array)
                {


                    string url = "http://mobile.yangkeduo.com/search_result.html?search_key="+keyword+"&sort_type=_sales";
                    string html = method.GetUrlWithCookie(url, "utf-8",COOKIE);
                    //textBox3.Text = html;
                    //return;
                    if (!html.Contains("没有找到")&& !html.Contains("搜索结果"))
                    {
                        Match match = Regex.Match(html, @"""goodsID"":([\s\S]*?),");

                      
                        string URL = "http://mobile.yangkeduo.com/goods.html?goods_id=" + match.Groups[1].Value.Trim();
                        
                        string strhtml = method.GetUrlWithCookie(URL, "utf-8",COOKIE);

                        Match counts = Regex.Match(strhtml, @"已拼([\s\S]*?)<");
                        Match price = Regex.Match(strhtml, @"minOnSaleGroupPrice"":""([\s\S]*?)""");
                        Match commentCount = Regex.Match(strhtml, @"commentsAmount"":([\s\S]*?),");

                        
                        Match name = Regex.Match(strhtml, @"mallName"":""([\s\S]*?)""");
                        Match shopcounts = Regex.Match(strhtml, @"已拼([\s\S]*?)<");

                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(keyword);
                        listViewItem.SubItems.Add(counts.Groups[1].Value.ToString().Replace(":","").Trim());
                        listViewItem.SubItems.Add(price.Groups[1].Value.ToString());
                        listViewItem.SubItems.Add(commentCount.Groups[1].Value.ToString());
                        listViewItem.SubItems.Add(URL);
                        listViewItem.SubItems.Add(name.Groups[1].Value.ToString());
                        listViewItem.SubItems.Add(shopcounts.Groups[1].Value.ToString());

                        if (this.listView1.Items.Count > 2)
                        {
                            this.listView1.EnsureVisible(this.listView1.Items.Count - 1);
                        }

                        Application.DoEvents();
                        Thread.Sleep(Convert.ToInt32(textBox2.Text));

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                    }

                    else
                    {
                        Match sousuo = Regex.Match(html, @"“<!-- -->([\s\S]*?)<!-- -->");

                       
                        ListViewItem listViewItem = this.listView2.Items.Add((listView2.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(keyword);
                        listViewItem.SubItems.Add(sousuo.Groups[1].Value.ToString());                 
                        if (this.listView2.Items.Count > 2)
                        {
                            this.listView2.EnsureVisible(this.listView2.Items.Count - 1);
                        }
                    }



                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(this.run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.zanting = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.zanting = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }

        private void 拼多多搜索_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            webBrowser web = new webBrowser("http://mobile.yangkeduo.com/");
            web.Show();

        }

       
    }
}
