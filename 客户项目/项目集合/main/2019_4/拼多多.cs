using Microsoft.Win32;
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

namespace main._2019_4
{
    public partial class 拼多多 : Form
    {
        public 拼多多()
        {
            InitializeComponent();
        }

        private void 拼多多_Load(object sender, EventArgs e)
        {

        }


        #region  拼多多
        public void run()
        {

            try
            {
                string[] ids = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

              

                foreach (string id in ids)
                {

                    String Url = "http://mobile.yangkeduo.com/goods.html?goods_id=" + id;

                    string html = method.GetUrl(Url, "utf-8");


                    Match titles = Regex.Match(html, @"""goodsName"":""([\s\S]*?)""");

                    string title = titles.Groups[1].Value;
                   
       
                    for (int i = 0; i < Convert.ToInt32(textBox3.Text); i++)
                    {
                       
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(Url);
                        lv1.SubItems.Add(title.Trim());

                        listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
             
                    }


                    Thread.Sleep(Convert.ToInt32(500));   //内容获取间隔，可变量        
                }


            }

            


            catch (System.Exception ex)
            {

                textBox1.Text = ex.ToString();
            }

        }

        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            //#region   读取注册码信息才能运行软件！

            //RegistryKey rsg = Registry.CurrentUser.OpenSubKey("zhucema"); //true表可修改                
            //if (rsg != null && rsg.GetValue("mac") != null)  //如果值不为空
            //{
            //    Thread thread = new Thread(new ThreadStart(run));
            //    Control.CheckForIllegalCrossThreadCalls = false;
            //    thread.Start();

            //}

            //else
            //{
            //    MessageBox.Show("请注册软件！");
            //    register lg = new register();
            //    lg.Show();
            //}

            //#endregion
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.ListviewToTxt(listView1);

            //MessageBox.Show((100 - Convert.ToInt32(textBox2.Text)).ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.SubItems[2].Text = item.SubItems[2].Text.Replace(textBox4.Text.Trim(), ""); 
                
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.SubItems[2].Text = textBox7.Text + item.SubItems[2].Text + textBox8.Text;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                decimal a = (Convert.ToDecimal((100 - Convert.ToInt32(textBox2.Text))) / 100);
                item.SubItems[2].Text = item.SubItems[2].Text.Substring(0, Convert.ToInt32(item.SubItems[2].Text.Length * a)); 

            }
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string url2 = "http://www.ciwuyou.com/fenci/?text=" + textBox9.Text + "&set_ignore=1&do_fork=1&Submit=%E5%88%86+%E8%AF%8D";
            string strhtml = method.GetUrl(url2, "utf-8");
            
            MatchCollection keywords = Regex.Matches(strhtml, @"<td width=""70"">([\s\S]*?)</td>([\s\S]*?)<td width=""71"">([\s\S]*?)</td>");

            for (int i = 0; i < keywords.Count; i++)
            {
                textBox10.Text += keywords[i].Groups[3].ToString()+"\r\n";
            }
            
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string[] keys = textBox10.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            Random rd = new Random();
            foreach (ListViewItem item in listView1.Items)
            {
               
                while (item.SubItems[2].Text.Length < 29)
                {

                    int suiji = rd.Next(0, keys.Length);
                    item.SubItems[2].Text = item.SubItems[2].Text + keys[suiji].ToString();
                }
            }
           

        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.SubItems[2].Text= item.SubItems[2].Text.Replace(textBox5.Text, textBox6.Text); ;

                
            }
        }
    }
}
