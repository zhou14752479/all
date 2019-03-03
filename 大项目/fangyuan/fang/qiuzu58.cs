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

namespace fang
{
    public partial class qiuzu58 : Form
    {
        bool status = true;
        public qiuzu58()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void qiuzu58_Load(object sender, EventArgs e)
        {
            method.get58CityName(comboBox1);
        }

        public void qiuzu()
        {

            string[] citys = textBox1.Text.Split(new string[] { "," }, StringSplitOptions.None);
            try
            {

                foreach (string cityname in citys)
                {
                    string city = method.Get58pinyin(cityname);

                    for (int i = 1; i < 71; i++)
                    {
                        String Url = string.Format("http://{0}.58.com/qiuzu/pn{1}/",city,i);

                      
                        string html = method.GetUrl(Url,"utf-8");

                        
                        MatchCollection TitleMatchs = Regex.Matches(html, @"logr=""([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        ArrayList lists = new ArrayList();
                        foreach (Match NextMatch in TitleMatchs)
                        {

                            lists.Add("http://m.58.com/"+city+"/qiuzu/" + NextMatch.Groups[4].Value + "x.shtml");

                        }
                        if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;
                        foreach (string list in lists)

                        {

                            string strhtml = method.GetUrl(list,"utf-8");  //定义的GetRul方法 返回 reader.ReadToEnd()

                            string title = @"<title>([\s\S]*?)</title>";
                            string Rxg = @"<p class=""llname"">([\s\S]*?)</p>";
                            string Rxg1 = @"<span id=""number"">([\s\S]*?)</span>";
                            string Rxg2 = @"<span class=""date""><i class=""ico""></i>([\s\S]*?)</span>";

                            Match titles = Regex.Match(strhtml, title);
                            Match contacts = Regex.Match(strhtml, Rxg);
                            Match tel = Regex.Match(strhtml, Rxg1);
                            Match time = Regex.Match(strhtml, Rxg2);

                 
                            //存入数据库结束

                            ListViewItem lv1 = listView1.Items.Add(titles.Groups[1].Value.Trim()); //使用Listview展示数据
                            lv1.SubItems.Add(contacts.Groups[1].Value);
                            lv1.SubItems.Add(tel.Groups[1].Value);
                            lv1.SubItems.Add(time.Groups[1].Value);

                            if (this.status == false)
                                return;
                            Application.DoEvents();
                            Thread.Sleep(Convert.ToInt32(1000));   //内容获取间隔，可变量

                        }

                    }

                }
            }

            catch (System.Exception ex)
            { ex.ToString();

            }


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text += comboBox1.SelectedItem.ToString() + ",";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(qiuzu);
            thread.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.status = false;
        }
    }
}
