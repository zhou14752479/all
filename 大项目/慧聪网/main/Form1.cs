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
namespace main
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
       bool  denglu=false;


        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        //http://168s.mobile.hc360.com/get168.cgi?fc=0&e=100&n=0&z=%E4%B8%AD%E5%9B%BD:%E6%B1%9F%E8%8B%8F%E7%9C%81%3A%E5%AE%BF%E8%BF%81%E5%B8%82&v=609&s_id=001%3B003&gs=37&w=%E5%A9%9A%E7%BA%B1
        bool zanting = true;

        #region  慧聪

        public void huicong()
        {

            try
            {

                string[] keywords = textBox3.Text.Trim().Split(',');

                string city = System.Web.HttpUtility.UrlEncode("中国:江苏省");
                foreach (string keyword in keywords)
                {

                    if (keyword == "")
                    {
                        MessageBox.Show("请输入采集行业或者关键词！");
                        return;
                    }
                    string key = System.Web.HttpUtility.UrlEncode(keyword);

                    for (int i = 1; i < 9999; i++)
                    {

                        string Url = "http://168s.mobile.hc360.com/get168.cgi?fc=0&e=100&n=" + i + "00&z=" + city + "&v=609&s_id=001%3B003&gs=37&w=" + key;
                       
                        string strhtml = method.GetUrl(Url,"gb2312");  //定义的GetRul方法 返回 reader.ReadToEnd()
                       
                        MatchCollection names = Regex.Matches(strhtml, @"searchResultfoTitle"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        MatchCollection tels = Regex.Matches(strhtml, @"searchResultfoText"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        MatchCollection areas = Regex.Matches(strhtml, @"searchResultfoZone"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        MatchCollection address = Regex.Matches(strhtml, @"searchResultfoAddress"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        MatchCollection tips = Regex.Matches(strhtml, @"searchResultfoTp"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        if (names.Count == 0)

                            break;

                        for (int j = 0; j < names.Count; j++)
                        {

                            if (names.Count > 0)
                            {
                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                                lv1.SubItems.Add(names[j].Groups[1].Value.Trim());
                                lv1.SubItems.Add(tels[j].Groups[1].Value.Trim());
                                lv1.SubItems.Add(areas[j].Groups[1].Value.Trim());
                                lv1.SubItems.Add(address[j].Groups[1].Value.Trim());
                                lv1.SubItems.Add(tips[j].Groups[1].Value.Trim());
                                toolStripStatusLabel2.Text = listView1.Items.Count.ToString();
                                while (this.zanting == false)
                                {
                                    Application.DoEvents();
                                }
                            }

                        }

                        if (listView1.Items.Count - 1 > 1)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);
                        }

                      
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);   //内容获取间隔，可变量

                    }
                }
                

            }
            catch (System.Exception ex)
            {
               MessageBox.Show(  ex.ToString());
            }
        }


        #endregion

        #region 51搜了网
        public void sole51()
        {
            
            try
            {
                if (textBox3.Text == "")
                {
                    MessageBox.Show("请输入关键词！");
                }

               
                string[] keywords = textBox3.Text.Split(new string[] { "," }, StringSplitOptions.None);
            

                    foreach (string keyword in keywords)

                    {

                        string keywordutf8 = System.Web.HttpUtility.UrlEncode(keyword, System.Text.Encoding.GetEncoding("utf-8"));

                        for (int i = 1; i < 51; i++)
                        {
                            String Url = "https://s.51sole.com/search.aspx?q="+keyword+"&page="+i;

                            string strhtml = method.GetUrl(Url,"utf-8");  //定义的GetRul方法 返回 reader.ReadToEnd()

                            string Rxg = @"<p class=""t1_tit"">([\s\S]*?)<a href=""([\s\S]*?)""";


                            MatchCollection all = Regex.Matches(strhtml, Rxg);


                        ArrayList lists = new ArrayList();
                        ArrayList lists1 = new ArrayList();
                        foreach (Match NextMatch in all)
                        {
                            if (NextMatch.Groups[2].Value.Contains("detail"))
                            {
                                lists1.Add(NextMatch.Groups[2].Value);
                            }
                            else
                            {
                                lists.Add(NextMatch.Groups[2].Value);
                            }


                        }
                        if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;


                            foreach (string list in lists)
                            {
                                
                                string strhtml1 = method.GetUrl(list,"utf-8");  //定义的GetRul方法 返回 reader.ReadToEnd()

                            
                                Match name = Regex.Match(strhtml1, @"<li><b>([\s\S]*?)</b>");
                                Match contacts = Regex.Match(strhtml1, @"联系人：</i><span>([\s\S]*?)</span>");
                                Match phone = Regex.Match(strhtml1, @"电话：</i><span>([\s\S]*?)</span>");
                                Match tell = Regex.Match(strhtml1, @"手机：</i><span>([\s\S]*?)</span>");
                                Match addr = Regex.Match(strhtml1, @"地址：</i><span>([\s\S]*?)</span>");
                                Match title = Regex.Match(strhtml1, @"description""  content=""([\s\S]*?)""");


                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                                lv1.SubItems.Add(name.Groups[1].Value.Trim());
                                lv1.SubItems.Add(contacts.Groups[1].Value.Trim());
                                lv1.SubItems.Add(phone.Groups[1].Value.Trim()+"  " + tell.Groups[1].Value.Trim());
                                lv1.SubItems.Add(addr.Groups[1].Value.Trim());
                                lv1.SubItems.Add(title.Groups[1].Value.Trim());
                            while (this.zanting == false)
                            {
                                Application.DoEvents();
                            }
                            if (listView1.Items.Count - 1 > 1)
                            {
                                listView1.EnsureVisible(listView1.Items.Count - 1);
                            }

                            Application.DoEvents();
                                System.Threading.Thread.Sleep(100);



                            }

                        }

                    }

                }
            




            catch (System.Exception ex)
            {
               ex.ToString();
            }
        }
        #endregion 

        #region 一呼百应
        public void yihubaiying()
        {

            try
            {

                string[] keywords = textBox3.Text.Split(new string[] { "," }, StringSplitOptions.None);


                foreach (string keyword in keywords)

                {

                    string keywordutf8 = System.Web.HttpUtility.UrlEncode(keyword, System.Text.Encoding.GetEncoding("utf-8"));

                    for (int i = 1; i < 51; i++)
                    {
                        String Url = "https://s.51sole.com/search.aspx?q=" + keyword + "&page=" + i;

                        string strhtml = method.GetUrl(Url, "utf-8");  //定义的GetRul方法 返回 reader.ReadToEnd()

                        string Rxg = @"<p class=""t1_tit"">([\s\S]*?)<a href=""([\s\S]*?)""";


                        MatchCollection all = Regex.Matches(strhtml, Rxg);


                        ArrayList lists = new ArrayList();
                        ArrayList lists1 = new ArrayList();
                        foreach (Match NextMatch in all)
                        {
                            if (NextMatch.Groups[2].Value.Contains("detail"))
                            {
                                lists1.Add(NextMatch.Groups[2].Value);
                            }
                            else
                            {
                                lists.Add(NextMatch.Groups[2].Value);
                            }


                        }
                        if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;


                        foreach (string list in lists)
                        {

                            string strhtml1 = method.GetUrl(list, "utf-8");  //定义的GetRul方法 返回 reader.ReadToEnd()


                            Match name = Regex.Match(strhtml1, @"<li><b>([\s\S]*?)</b>");
                            Match contacts = Regex.Match(strhtml1, @"联系人：</i><span>([\s\S]*?)</span>");
                            Match phone = Regex.Match(strhtml1, @"电话：</i><span>([\s\S]*?)</span>");
                            Match tell = Regex.Match(strhtml1, @"手机：</i><span>([\s\S]*?)</span>");
                            Match addr = Regex.Match(strhtml1, @"地址：</i><span>([\s\S]*?)</span>");
                            Match title = Regex.Match(strhtml1, @"description""  content=""([\s\S]*?)""");


                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                            lv1.SubItems.Add(name.Groups[1].Value.Trim());
                            lv1.SubItems.Add(contacts.Groups[1].Value.Trim());
                            lv1.SubItems.Add(phone.Groups[1].Value.Trim() + "  " + tell.Groups[1].Value.Trim());
                            lv1.SubItems.Add(addr.Groups[1].Value.Trim());
                            lv1.SubItems.Add(title.Groups[1].Value.Trim());
                            while (this.zanting == false)
                            {
                                Application.DoEvents();
                            }
                            if (listView1.Items.Count - 1 > 1)
                            {
                                listView1.EnsureVisible(listView1.Items.Count - 1);
                            }

                            Application.DoEvents();
                            System.Threading.Thread.Sleep(100);



                        }

                    }

                }

            }





            catch (System.Exception ex)
            {
                ex.ToString();
            }
        }
        #endregion 

        #region 黄页88
        public void hy88()
        {

            try
            {

                string[] keywords = textBox3.Text.Split(new string[] { "," }, StringSplitOptions.None);


                foreach (string keyword in keywords)

                {

                    string keywordutf8 = System.Web.HttpUtility.UrlEncode(keyword, System.Text.Encoding.GetEncoding("utf-8"));

                    for (int i = 1; i < 51; i++)
                    {
                        String Url = "http://www.huangye88.com/search.html?kw="+ keywordutf8 + "&type=company&page="+i+"/";

                        string strhtml = method.GetUrl(Url, "utf-8");  //定义的GetRul方法 返回 reader.ReadToEnd()
                       
                        string Rxg = @"<p class=""p-title"">([\s\S]*?)<a href=""([\s\S]*?)""";


                        MatchCollection all = Regex.Matches(strhtml, Rxg);

                        ArrayList lists = new ArrayList();
                        foreach (Match NextMatch in all)
                        {
                          
                                lists.Add(NextMatch.Groups[2].Value);
                            

                        }

                        
                        if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;


                        foreach (string list in lists)
                        {

                            string strhtml1 = method.GetUrl(list, "utf-8");  //定义的GetRul方法 返回 reader.ReadToEnd()


                            Match name = Regex.Match(strhtml1, @"<h1 class=""big"">([\s\S]*?)</h1>");
                            Match contacts = Regex.Match(strhtml1, @"联系人：</i><span>([\s\S]*?)</span>");
                            Match phone = Regex.Match(strhtml1, @"电话：</label>([\s\S]*?)</li>");
                            Match tell = Regex.Match(strhtml1, @"手机：</label>([\s\S]*?)</li>");
                            Match addr = Regex.Match(strhtml1, @"地址:([\s\S]*?);");
                            Match title = Regex.Match(strhtml1, @"description""  content=""([\s\S]*?)""");


                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                            lv1.SubItems.Add(name.Groups[1].Value.Trim());
                            lv1.SubItems.Add(contacts.Groups[1].Value.Trim());
                            lv1.SubItems.Add(phone.Groups[1].Value.Trim() + "  " + tell.Groups[1].Value.Trim());
                            lv1.SubItems.Add(addr.Groups[1].Value.Trim());
                            lv1.SubItems.Add(title.Groups[1].Value.Trim());
                            while (this.zanting == false)
                            {
                                Application.DoEvents();
                            }
                            if (listView1.Items.Count - 1 > 1)
                            {
                                listView1.EnsureVisible(listView1.Items.Count - 1);
                            }

                            Application.DoEvents();
                            System.Threading.Thread.Sleep(100);



                        }

                    }

                }

            }





            catch (System.Exception ex)
            {
                ex.ToString();
            }
        }
        #endregion 
        private void button2_Click(object sender, EventArgs e)
        {
            if (denglu == false)
            {
                MessageBox.Show("请先登录您的账号！");
                return;
            }

            Thread thread = new Thread(new ThreadStart(sole51));
            thread.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {



                string constr = "Host =116.62.62.62;Database=vip;Username=root;Password=zhoukaige";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select * from wuba2019 where username='" + textBox1.Text.Trim() + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'
                MySqlDataReader reader = cmd.ExecuteReader();



                if (reader.Read())
                {

                    string username = reader["username"].ToString().Trim();
                    string password = reader["password"].ToString().Trim();
                   
                    //判断密码
                    if (textBox2.Text.Trim() == password)
                    {

                        MessageBox.Show("登陆成功！");
                       
                        denglu = true;
                        reader.Close();
                       

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

      

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}


