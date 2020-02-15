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

namespace main._2019_7
{
    public partial class 上海企业 : Form
    {
        public 上海企业()
        {
            InitializeComponent();
        }


        bool status = true;
        private void Button1_Click(object sender, EventArgs e)
        {
            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
            lv1.SubItems.Add(textBox1.Text);
            textBox1.Text = "";
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Browser web = new Browser("http://yct.sh.gov.cn/namedeclare/check/check_biz1?self_id=&check_items=NAME&self_name=&check_nameApp=&check_nameRegOrgan=000000&check_nameApoOrgan=000000&check_nameDistrict=%E4%B8%8A%E6%B5%B7&check_nameTrad=%E9%A5%B0&check_tradPiny=&check_nameIndDesc=%E6%9C%89&check_indPhy=&check_industryCode=&check_ifBranch=");
            web.Show();
        }

      

        ArrayList finishes = new ArrayList();
        public void run()
        {
            
   

            for (int a= 0;a < listView1.Items.Count; a++)
            {
                string url = listView1.Items[a].SubItems[1].Text;

                if (!finishes.Contains(url))
                {
                    finishes.Add(url);

                    label2.Text = "正在抓取......第" + a + "个网址";
                    
                    string COOKIE = textBox2.Text;
                    string html = method.gethtml(url, COOKIE, "utf-8");

                    MatchCollection ids = Regex.Matches(html, @"""id"":""([\s\S]*?)""");
                    MatchCollection nameTrads = Regex.Matches(html, @"""nameTrad"":""([\s\S]*?)""");
                    MatchCollection etpsNames = Regex.Matches(html, @"""etpsName"":""([\s\S]*?)""");


                    if (ids.Count > 0)
                    {
                        for (int i = 0; i < ids.Count; i++)
                        {
                            if (!ids[i].Groups[1].Value.Contains("sh") && !ids[i].Groups[1].Value.Contains("SH"))
                            {
                                ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv2.SubItems.Add(ids[i].Groups[1].Value);
                                lv2.SubItems.Add(nameTrads[i].Groups[1].Value);
                                lv2.SubItems.Add(etpsNames[i].Groups[1].Value);

                                if (status == false)
                                    return;
                            }
                        }
                    }

                    else
                    {
                        ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv2.SubItems.Add("cookie失效");
                        lv2.SubItems.Add("cookie失效");
                        lv2.SubItems.Add("cookie失效");

                    }




                   
                }
            }

           

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")

            {
                MessageBox.Show("请先输入COOKIE");
                return;
            }
            status = true;
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("请添加网址");
                return;
            }
            label2.Text = "正在抓取......";
            timer1.Interval = (Convert.ToInt32(textBox3.Text)) * 1000;
            timer1.Start();
        }

       

        private void 上海企业_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread(new ThreadStart(run));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();
            }

            //抓取完成导出
            StringBuilder sb = new StringBuilder();
            foreach (ListViewItem item in listView2.Items)
            {
                List<string> list = new List<string>();
                string temp = item.SubItems[1].Text;
                string temp1 = item.SubItems[2].Text;
                string temp2 = item.SubItems[3].Text;
                list.Add(temp + "-----" + temp1 + "-----" + temp2);
                foreach (string tel in list)
                {
                    sb.AppendLine(tel);
                }
            }
            string path = AppDomain.CurrentDomain.BaseDirectory + "data" + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            string path1 = AppDomain.CurrentDomain.BaseDirectory + "data" + "\\总数据.txt";

            System.IO.File.WriteAllText(path, sb.ToString(), Encoding.UTF8);

            FileStream fs = new FileStream(path1, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw = new StreamWriter(fs);
            sw.Write(sb.ToString());
            //记得要关闭！不然里面没有字！
            sw.Close();
            fs.Close();
            listView2.Items.Clear();
            finishes.Clear();
        }

        private void 删除此行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int Index = 0;
            if (this.listView1.SelectedItems.Count > 0)//判断listview有被选中项
            {
                Index = this.listView1.SelectedItems[0].Index;//取当前选中项的index,SelectedItems[0]这必须为0
                listView1.Items[Index].Remove();
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {

                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(text[i]);


                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
