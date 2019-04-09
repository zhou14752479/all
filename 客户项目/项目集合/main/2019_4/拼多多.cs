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

                string url2 = "http://www.ciwuyou.com/fenci/?text=" + textBox9.Text + "&set_ignore=1&do_fork=1&Submit=%E5%88%86+%E8%AF%8D";
                string strhtml = method.GetUrl(url2, "utf-8");
                Random rd = new Random();
                MatchCollection keywords = Regex.Matches(strhtml, @"<td width=""70"">([\s\S]*?)</td>([\s\S]*?)<td width=""71"">([\s\S]*?)</td>");

                foreach (string id in ids)
                {

                    String Url = "http://mobile.yangkeduo.com/goods.html?goods_id=" + id;

                    string html = method.GetUrl(Url, "utf-8");


                    Match titles = Regex.Match(html, @"""goodsName"":""([\s\S]*?)""");

                    string title = titles.Groups[1].Value;
                    decimal a = (Convert.ToDecimal((100 - Convert.ToInt32(textBox2.Text))) / 100);
                    
                    string title1 = title.Substring(0,Convert.ToInt32(title.Length*a));
                    string title2 = title1.Replace(textBox4.Text,"");
                    string title3 = title2.Replace(textBox5.Text, textBox6.Text);
                    string title4 = textBox7.Text + title3 + textBox8.Text;
                    
                   
                    
                   
                    for (int i = 0; i < Convert.ToInt32(textBox3.Text); i++)
                    {
                        string title5 = title4;

                        while (title5.Length < 29)
                        {
                            
                            int suiji = rd.Next(0, keywords.Count);
                            title5 = title5 + keywords[suiji].Groups[3].ToString();
                        }
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(Url);
                        lv1.SubItems.Add(title5.Trim());

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
    }
}
