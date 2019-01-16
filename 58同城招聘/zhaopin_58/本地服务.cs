using MySql.Data.MySqlClient;
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

namespace zhaopin_58
{
    public partial class 本地服务 : Form
    {
        public 本地服务()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false; // 设置线程之间可以操作
        }

        private void bendifuwu_Load(object sender, EventArgs e)
        {

        }
        bool zanting = true;

        public void insertData(string[] values)
        {

            try
            {
                string constr = "Host =116.62.62.62;Database=vip;Username=root;Password=zhoukaige";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO wuba_bendi (title,company,contacts,phone,city,addr,time)VALUES('" + values[0] + " ','" + values[1] + " ','" + values[2] + " ','" + values[3] + " ','" + values[4] + " ','" + values[5] + " ','" + values[6] + " ')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.

            }

            catch (System.Exception ex)
            {
                ex.ToString();
            }
        }

        #region  主程序

        public void zhaopin()
        {

            try
            {

                string[] citys = textBox1.Text.Trim().Split(',');
                string[] keywords = textBox2.Text.Trim().Split(',');

                foreach (string city in citys)
                {
                    if (city == "")
                    {
                        MessageBox.Show("请选择城市！");
                        return;
                    }
                    foreach (string keyword in keywords)
                    {

                        if (keyword == "")
                        {
                            MessageBox.Show("请输入采集行业或者关键词！");
                            return;
                        }

                        string ahtml = method.GetUrl("https://suggest.58.com/searchsuggest_14.do?inputbox="+keyword+"&https=1&cityid=2&catid=0&nonstop=1&callback=callback9737");
                        //获取关键字对应的分类字母如艺术对应techang
                        MatchCollection items = Regex.Matches(ahtml, @"""l"": ""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        for (int i = 1; i < 71; i++)
                        {

                            string Url = "https://"+city+".58.com/"+items[1].Groups[1].Value.Trim()+"/pn"+i+"/?key="+keyword;
                            string html = method.GetUrl(Url);

                           
                            
                            MatchCollection TitleMatchs = Regex.Matches(html, @"https://[a-z]+.58.com/[a-z]+/[0-9]+x.shtml", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                            ArrayList lists = new ArrayList();
                           
                            foreach (Match NextMatch in TitleMatchs)
                            {

                                if (!lists.Contains(NextMatch.Groups[0].Value))
                                {
                                    lists.Add(NextMatch.Groups[0].Value);
                                }
                                

                            }

                            
                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;

                            
                            foreach (string list in lists)

                            {
                                string strhtml = method.GetUrl(list);  //定义的GetRul方法 返回 reader.ReadToEnd()

                                

                                string rxg = @"<title>([\s\S]*?)</title>";
                                string rxg1 = @"""name"":""([\s\S]*?)""";    //公司                            
                                string rxg2 = @"linkman:'([\s\S]*?)'";
                                string rxg3 = @"<div class='nums'>([\s\S]*?)</div>";
                                string rxg4 = @"pubDate"": ""([\s\S]*?)""";
                                string rxg5 = @"cityName"":""([\s\S]*?)""";
                                string rxg6 = @"bsName"":""([\s\S]*?)""";
                                string rxg8 = @"infoid:'([\s\S]*?)'";
                                Match id = Regex.Match(strhtml, rxg8);



                                string tellHtml = method.GetUrl("http://app.58.com/api/windex/scandetail/car/" + id.Groups[1].Value.ToString().Trim() + "/?pid=801");
                               



                                Match title = Regex.Match(strhtml, rxg);
                                Match company = Regex.Match(strhtml, rxg1);
                                Match lxr = Regex.Match(strhtml, rxg2);
                                Match  tell= Regex.Match(tellHtml, rxg3);
                                Match time = Regex.Match(strhtml, rxg4);
                                Match area = Regex.Match(strhtml, rxg5);
                                Match addr = Regex.Match(strhtml, rxg6);

                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                                    lv1.SubItems.Add(title.Groups[1].Value.Trim());
                                    lv1.SubItems.Add(company.Groups[1].Value.Trim());
                                lv1.SubItems.Add(lxr.Groups[1].Value.Trim());
                                lv1.SubItems.Add(tell.Groups[1].Value.Trim().Replace("-", ""));
                                lv1.SubItems.Add(area.Groups[1].Value.Trim());
                                lv1.SubItems.Add(addr.Groups[1].Value.Trim());
                                
                                    lv1.SubItems.Add(time.Groups[1].Value.Trim());

                                string[] values = { title.Groups[1].Value.Trim(), company.Groups[1].Value.Trim(), lxr.Groups[1].Value.Trim(),  tell.Groups[1].Value.Trim(), area.Groups[1].Value.Trim(), addr.Groups[1].Value.Trim(), DateTime.Now.ToString(),};
                                insertData(values);

                                if (listView1.Items.Count - 1 > 1)
                                {
                                    listView1.EnsureVisible(listView1.Items.Count - 1);
                                }

                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                                Application.DoEvents();
                                System.Threading.Thread.Sleep(3000);   //内容获取间隔，可变量

                            }

                        }
                    }
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
            Thread thread = new Thread(new ThreadStart(zhaopin));
            thread.Start();
        }

   

        private void button2_Click(object sender, EventArgs e)
        {
            this.zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.zanting = true;
        }


        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void skinTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            textBox1.Text += e.Node.Name + ",";
        }
    }
}
