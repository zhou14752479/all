using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace 链家网
{
   


    public partial class 链家网 : Form
    {
        [DllImport("Kernel32.dll")] //引入命名空间 using System.Runtime.InteropServices;  
        public static extern bool Beep(int frequency, int duration);// 第一个参数是指频率的高低，越大越高，第二个参数是指响的时间多

        public 链家网()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        bool status = true;

        public void run()
        {
            try
            {


                for (int i = 1; i < 100; i++)
                {
                    string url = textBox1.Text + "pg" + i + "/";
                    
                    string html = method.GetHtmlSource(url);
                   
                    MatchCollection matches = Regex.Matches(html, @"data-id=""([\s\S]*?)""");
                    MatchCollection zuixin = Regex.Matches(html, @"<div class=""totalPrice""><span>([\s\S]*?)</span>");

                    ArrayList lists = new ArrayList();
                    foreach (Match NextMatch in matches)
                    {
                        lists.Add("https://cq.lianjia.com/ershoufang/co41c" + NextMatch.Groups[1].Value + "/");

                    }
                   
                    if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;

                    for (int j = 0; j < lists.Count; j++)
                    {

                        string strhtml = method.GetHtmlSource(lists[j].ToString());
                        Match xiaoqu = Regex.Match(strhtml, @"<span class=""name"">([\s\S]*?)</span>");
                        MatchCollection zuidi = Regex.Matches(strhtml, @"data-price=""([\s\S]*?)""");

                        Match zaishou = Regex.Match(strhtml, @"count: ([\s\S]*?),");

                        if (zuidi.Count >2)
                        {

                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(xiaoqu.Groups[1].Value);
                            lv1.SubItems.Add(zuidi[0].Groups[1].Value);
                            lv1.SubItems.Add(zuidi[1].Groups[1].Value);
                            lv1.SubItems.Add(zuixin[j].Groups[1].Value);
                            lv1.SubItems.Add(zaishou.Groups[1].Value);

                            



                            string url1 = "http://www.17gp.com/FangJia/GetCommunityList";
                            string postdata = "kw="+ xiaoqu.Groups[1].Value + "&num=8&code=";
                            string cookie = "VerifyToken=a5c6c1e375284296ad8e109365d92cf6; Hm_lvt_1799f23f2766063ed058651d23d7543d=1551768898; UM_distinctid=1694ca2805728-0c9405f52fafef-5d1f3b1c-100200-1694ca28059315; CNZZDATA1263082085=48898473-1551768855-null%7C1551768855; gr_user_id=2eeb5b35-caac-4b7d-b61d-84358dbbf01e; Hm_lpvt_1799f23f2766063ed058651d23d7543d=1551771217; gr_session_id_9d5a50541bffa263=d861bda6-e22b-46c7-9f69-202139bda1fd; gr_session_id_9d5a50541bffa263_d861bda6-e22b-46c7-9f69-202139bda1fd=true";
                            string ahtml = method.PostUrl(url1,postdata,cookie,"utf-8");

                            Match aid = Regex.Match(ahtml, @"guid"":""([\s\S]*?)""");


                            string bhtml = method.PostUrl("http://www.17gp.com/FangJia/GetRecentlyPrice/"+aid.Groups[1].Value, "", "VerifyToken=a5c6c1e375284296ad8e109365d92cf6; Hm_lvt_1799f23f2766063ed058651d23d7543d=1551768898; UM_distinctid=1694ca2805728-0c9405f52fafef-5d1f3b1c-100200-1694ca28059315; CNZZDATA1263082085=48898473-1551768855-null%7C1551768855; gr_user_id=2eeb5b35-caac-4b7d-b61d-84358dbbf01e; gr_session_id_9d5a50541bffa263=d861bda6-e22b-46c7-9f69-202139bda1fd; gr_session_id_9d5a50541bffa263_d861bda6-e22b-46c7-9f69-202139bda1fd=true; Hm_lpvt_1799f23f2766063ed058651d23d7543d=1551772518", "utf-8");

                            Match pgprice= Regex.Match(bhtml, @"avgPrice"":""([\s\S]*?)""");

                            lv1.SubItems.Add(pgprice.Groups[1].Value);

                            Double a = Convert.ToDouble(zuidi[0].Groups[1].Value);
                            Double b = Convert.ToDouble(zuidi[1].Groups[1].Value);
                            Double d = Convert.ToDouble(pgprice.Groups[1].Value);

                            if (a < b * Convert.ToDouble(textBox2.Text))
                            {
                                textBox3.Text += "ID"+listView1.Items.Count.ToString()+ xiaoqu.Groups[1].Value +"出现报警信息"+ "\r\n";
                                Beep(500, 700);
                                listView1.Items[listView1.Items.Count - 1].BackColor = Color.Red;
                            }


                            if (a < d * Convert.ToDouble(textBox5.Text))
                            {
                                textBox4.Text += "ID" + listView1.Items.Count.ToString() + xiaoqu.Groups[1].Value + "出现报警信息" + "\r\n";
                                Beep(500, 700);
                                listView1.Items[listView1.Items.Count-1].BackColor = Color.Red;
                            }



                            if (listView1.Items.Count - 1 > 1)
                            {
                                listView1.EnsureVisible(listView1.Items.Count - 1);
                            }
                        }
                        while (this.status == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        Thread.Sleep(500);
                    }



                }


            }
            catch (System.Exception ex)
            {

             MessageBox.Show(  ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = true;
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            listView1.FullRowSelect = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {



                string constr = "Host =122.114.13.236;Database=users;Username=admin111;Password=admin111";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select * from zhanghaos where username='" + textBox1.Text.Trim() + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'
                MySqlDataReader reader = cmd.ExecuteReader();



                if (reader.Read())
                {

                    string username = reader["username"].ToString().Trim();
                    string password = reader["password"].ToString().Trim();
                    string status = reader["status"].ToString().Trim();


                    if (status == "1")
                    {
                        MessageBox.Show("您的账号在别处登录！");
                        return;
                    }
                    //判断密码
                    if (textBox2.Text.Trim() == password)
                    {

                        MessageBox.Show("登陆成功！");
                        textBox2.Visible = false;
                        denglu = true;
                        reader.Close();
                        MySqlCommand cmd1 = new MySqlCommand("UPDATE zhanghaos SET status='1' where username='" + textBox1.Text.Trim() + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'
                        cmd1.ExecuteReader();

                    }


                    else

                    {
                        MessageBox.Show("您的密码错误！");
                        return;
                    }

                }

                else
                {
                    MessageBox.Show("未查询到您的账户信息！");
                    return;
                }


            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}
