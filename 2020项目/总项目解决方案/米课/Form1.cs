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

namespace 米课
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool zanting = true;
        
        public void run()
        {
            string cookie = textBox2.Text;
            try
            {
               
               
                string html = method.PostUrl("http://data.imiker.com/ajax_search", "m=hs&type=buy&country=all&content="+textBox1.Text+"&sort=0",cookie,"utf-8");
                Match datas = Regex.Match(html, @"data"":\[([\s\S]*?)\]");

                string[] data = datas.Groups[1].Value.Split(new string[] { "," }, StringSplitOptions.None);
                MessageBox.Show(data.Length.ToString());
                for (int i = 0; i < 100; i++)
                {
                    string postdata = "&list%5B"+5*i+"%5D="+data[5*i]+ "&list%5B"+ (5*i+1) + "%5D="+data[5*i+1] +"&list%5B"+ (5*i+2)+ "%5D="+data[5*i+2] +"&list%5B"+ (5*i+3)+ "%5D="+data[5*i+3] +"&list%5B"+(5*i+4)+"%5D="+data[5*i+4];
                    string postdata1 = "m=hs&type=buy&country=all&content="+textBox1.Text + postdata.Replace("\"", "").Replace(" ","+")+ "&page="+i;

                    string strhtml = method.PostUrl("http://data.imiker.com/ajax_list", postdata1, cookie, "utf-8");
                    
                    //textBox2.Text = strhtml;
                    //MessageBox.Show("1");
                    
                    MatchCollection matches = Regex.Matches(strhtml, @"<h2>([\s\S]*?)<a href=""/detailed/hs/buy/([\s\S]*?)""");
                    MatchCollection  countrys = Regex.Matches(strhtml, @"<span class=""country"">([\s\S]*?)</span>");
                    ArrayList lists = new ArrayList();
                    foreach (Match NextMatch in matches)
                    {

                        lists.Add( NextMatch.Groups[2].Value);

                    }

                    for (int j = 0; j <lists.Count; j++)
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
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用验证

            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php", "utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == "26.26.26.26")
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {

                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;


            }
            else
            {
                MessageBox.Show("IP不符");

            }
            #endregion
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
