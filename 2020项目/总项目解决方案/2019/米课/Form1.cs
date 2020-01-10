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
using helper;
using MySql.Data.MySqlClient;

namespace 米课
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool zanting = true;

       

        private void Form1_Load(object sender, EventArgs e)
        {

        }

      

        public void run()
        {    string Country = "imiker_ua";
            switch (comboBox1.Text)
            {
                case "全球":
                    Country = "all";
                    break;
                case "美国":
                    Country = "imiker_us";
                    break;
                case "哥伦比亚":
                    Country = "imiker_co";
                    break;
                case "秘鲁":
                    Country = "imiker_pe";
                    break;
                case "巴拉圭":
                    Country = "imiker_py";
                    break;
                case "厄瓜多尔":
                    Country = "imiker_ec";
                    break;
                case "俄罗斯":
                    Country = "imiker_ru";
                    break;
                case "乌克兰":
                    Country = "imiker_ua";
                    break;
                case "巴基斯坦":
                    Country = "imiker_pk";
                    break;
                case "阿根廷":
                    Country = "ar";
                    break;
                case "智利":
                    Country = "imiker_cl";
                    break;
                case "乌拉圭":
                    Country = "imiker_uy";
                    break;
                case "委内瑞拉":
                    Country = "ve";
                    break;
                case "哥斯达黎加":
                    Country = "cr";
                    break;
                case "巴拿马":
                    Country = "pa";
                    break;
                case "印度":
                    Country = "imiker_in";
                    break;
                case "韩国":
                    Country = "ko";
                    break;
                case "英国":
                    Country = "uk";
                    break;
                case "德国":
                    Country = "de";
                    break;
                case "法国":
                    Country = "fr";
                    break;
                case "日本":
                    Country = "jp";
                    break;

            }
            try
            {
                

                string cookie = textBox2.Text;
                string html = method.PostUrl("http://data.imiker.com/ajax_search", "m=des&type=buy&country="+Country+"&content=" + textBox1.Text + "&sort=0", cookie, "utf-8");
                Match datas = Regex.Match(html, @"data"":\[([\s\S]*?)\]");
               
                string[] data = datas.Groups[1].Value.Split(new string[] { "\"," }, StringSplitOptions.None);
              
                for (int i = 0; i < 100; i++)
                {
                    string postdata = "&list%5B" + 5 * i + "%5D=" + data[5 * i].Replace("&", "%26") + "&list%5B" + (5 * i + 1) + "%5D=" + data[5 * i + 1].Replace("&", "%26") + "&list%5B" + (5 * i + 2) + "%5D=" + data[5 * i + 2].Replace("&", "%26") + "&list%5B" + (5 * i + 3) + "%5D=" + data[5 * i + 3].Replace("&", "%26") + "&list%5B" + (5 * i + 4) + "%5D=" + data[5 * i + 4].Replace("&", "%26");
                    string postdata1 = "m=des&type=buy&country=" + Country + "&content=" + textBox1.Text + postdata.Replace("\\/", "%2F").Replace("\"", "").Replace(" ", "+").Replace(",", "%2C").Replace("\\u6797", "%E6%9E%97").Replace("\\u2019", "%E2%80%99") + "&page=" + i;
                    
                   
                    string strhtml = method.PostUrl("http://data.imiker.com/ajax_list", postdata1, cookie, "utf-8");

                    //textBox2.Text = strhtml;
                    //MessageBox.Show("1");

                   // MatchCollection matches = Regex.Matches(strhtml, @"<h2>([\s\S]*?)<a href=""/detailed/hs/buy/([\s\S]*?)""");
                    MatchCollection matches = Regex.Matches(strhtml, @"""buy"",""([\s\S]*?)""");
                    MatchCollection countrys = Regex.Matches(strhtml, @"<span class=""country"">([\s\S]*?)</span>");
                    ArrayList lists = new ArrayList();
                    foreach (Match NextMatch in matches)
                    {

                        lists.Add(NextMatch.Groups[1].Value+"/"+textBox1.Text.Trim());

                    }

                    for (int j = 0; j < lists.Count; j++)
                    {

                        string ahtml = method.GetUrlWithCookie("http://data.imiker.com/detailed_ajax?m=hs&type=buy&content=" + lists[j].ToString().Replace("/", "&des="), cookie, "utf-8");

                        Match a1 = Regex.Match(ahtml, @"""importer"":""([\s\S]*?)""");

                        Match a3 = Regex.Match(ahtml, @"""related_total"":([\s\S]*?),");
                        Match a4 = Regex.Match(ahtml, @"""total"":([\s\S]*?),");
                        Match a5 = Regex.Match(ahtml, @"""related_total"":([\s\S]*?)percent"":([\s\S]*?),");
                        Match a6 = Regex.Match(ahtml, @"""end"":""([\s\S]*?)""");

                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(a1.Groups[1].Value);
                        listViewItem.SubItems.Add(countrys[j].Groups[1].Value);
                        listViewItem.SubItems.Add(a3.Groups[1].Value);
                        listViewItem.SubItems.Add(a4.Groups[1].Value);
                        listViewItem.SubItems.Add(a5.Groups[2].Value);
                        listViewItem.SubItems.Add(a6.Groups[1].Value);
                        listViewItem.SubItems.Add("http://data.imiker.com/detailed/hs/buy/" + lists[j]);

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {

                ex.ToString();
            }
        }

     
        private void button1_Click(object sender, EventArgs e)
        {
           
            string constr = "Host =47.99.68.92;Database=vip_database;Username=root;Password=zhoukaige00.@*.";
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();

            MySqlCommand cmd = new MySqlCommand("select * from vip where username='米课'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

            MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

            if (reader.Read())
            {

                string password = reader["password"].ToString().Trim();

                if (password != "米课")

                {
                    MessageBox.Show("验证失败");

                    Environment.Exit(0);
                }


                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
