using MySql.Data.MySqlClient;
using Newtonsoft.Json;
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

        bool status = true;
        bool zanting = true;

        public class Info
        {

            public List<recordList> recordList;
        }

        public class recordList
        {
            public string companyname { get; set; }
            public string linkman { get; set; }
            public string linkmp { get; set; }
            public string address { get; set; }
            public string cityname { get; set; }
        }

        #region  慧聪网

        public void huicong()
        {
            listView2.Visible = false;
            if (comboBox1.Text == "")
            {
                MessageBox.Show("请选择地区");
                return;
            }

            try
            {

                string[] keywords = textBox3.Text.Trim().Split(',');

               
                string city = System.Web.HttpUtility.UrlEncode("中国:"+comboBox1.Text+":"+comboBox2.Text);
                if (comboBox2.Text == "全省" || comboBox2.Text == "")
                {
                    city = System.Web.HttpUtility.UrlEncode("中国:" + comboBox1.Text );
                }
                if (comboBox1.Text == "全国")
                {
                    city = System.Web.HttpUtility.UrlEncode("中国" );
                }

                textBox1.Text = city;
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

                        string Url = "https://esapi.org.hc360.com/interface/getinfos.html?pnum="+i+"&psize=100&kwd="+keyword+"&z="+city+"&index=companyinfo&collapsef=providerid";
                       
                        string strhtml = method.GetUrl(Url,"utf-8");  //定义的GetRul方法 返回 reader.ReadToEnd()


                        Info jsonParser = JsonConvert.DeserializeObject<Info>(strhtml);



                        foreach (recordList recordList1 in jsonParser.recordList)
                        {

                            if (recordList1.linkmp != "null" && recordList1.linkmp !="")
                            {
                               
                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                                lv1.SubItems.Add(recordList1.companyname);
                                lv1.SubItems.Add(recordList1.linkman);

                                lv1.SubItems.Add(recordList1.linkmp);
                                lv1.SubItems.Add(recordList1.address);
                                lv1.SubItems.Add(recordList1.cityname);
                                if (listView2.Items.Count - 1 > 1)
                                {
                                    listView2.EnsureVisible(listView2.Items.Count - 1);
                                }
                                if (status == false)
                                {
                                    return;
                                }

                                while (this.zanting == false)
                                {
                                  Application.DoEvents();
                              }

                            }

                            
                        }
                        Application.DoEvents();
                        Thread.Sleep(1000);   //内容获取间隔，可变量

                        //MatchCollection names = Regex.Matches(strhtml, @"companyname"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        //MatchCollection tels = Regex.Matches(strhtml, @"linkmp"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        //MatchCollection areas = Regex.Matches(strhtml, @"cityname"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        //MatchCollection address = Regex.Matches(strhtml, @"address"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        //MatchCollection contacts = Regex.Matches(strhtml, @"linkman"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        //int count = names.Count < address.Count ? names.Count : address.Count;

                        //if (names.Count == 0)

                        //    break;

                        //for (int j = 0; j < count; j++)
                        //{

                        //    if (names.Count > 0 && tels[j].Groups[1].Value != "")
                        //    {
                        //        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                        //        lv1.SubItems.Add(names[j].Groups[1].Value.Trim());
                        //        lv1.SubItems.Add(contacts[j].Groups[1].Value.Trim());
                        //        lv1.SubItems.Add(tels[j].Groups[1].Value.Trim());
                        //        lv1.SubItems.Add(address[j].Groups[1].Value.Trim());
                        //        lv1.SubItems.Add(areas[j].Groups[1].Value.Trim());
                        //        toolStripStatusLabel2.Text = listView1.Items.Count.ToString();
                        //        while (this.zanting == false)
                        //        {
                        //            Application.DoEvents();
                        //        }

                        //        if (status == false)
                        //        {
                        //            return;
                        //        }

                        //        if (listView1.Items.Count - 1 > 1)
                        //        {
                        //            listView1.EnsureVisible(listView1.Items.Count - 1);
                        //        }
                        //    }

                    }




                        Application.DoEvents();
                        System.Threading.Thread.Sleep(1000);   //内容获取间隔，可变量

                    }


                

                button2.Enabled = true;
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

                            if (phone.Groups[1].Value!="-")
                            {
                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                                lv1.SubItems.Add(name.Groups[1].Value.Trim());
                                lv1.SubItems.Add(contacts.Groups[1].Value.Trim());
                                lv1.SubItems.Add(phone.Groups[1].Value.Trim() + "  " + tell.Groups[1].Value.Trim());
                                lv1.SubItems.Add(addr.Groups[1].Value.Trim());
                                lv1.SubItems.Add(title.Groups[1].Value.Trim());
                            }
                            if (status == false)
                            {
                                return;
                            }

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


        public class JsonParser
        {
            public List<Content> Content;

        }

        public class Content
        {
            public string name;
            public string tel;
            public string addr;
        }

        #region 获取百度citycode
        public int getcityId(string cityName)
        {
            try

            {

                String Url = "https://map.baidu.com/?newmap=1&reqflag=pcmap&biz=1&from=webmap&da_par=direct&pcevaname=pc4.1&qt=s&da_src=searchBox.button&wd=" + cityName + "&c=289&src=0&wd2=&pn=0&sug=0&l=12&b=(13461858.87,3636969.979999999;13584738.87,3670185.979999999)&from=webmap&biz_forward={%22scaler%22:1,%22styles%22:%22pl%22}&sug_forward=&tn=B_NORMAL_MAP&nn=0&u_loc=13166533,3998088&ie=utf-8";

                string html = method.GetUrl(Url,"utf-8");


                MatchCollection Matchs = Regex.Matches(html, @"""code"":([\s\S]*?),", RegexOptions.IgnoreCase);




                int cityId = Convert.ToInt32(Matchs[0].Groups[1].Value);
                return cityId;

            }
            catch (System.Exception ex)
            {
                ex.ToString();
                return 1;
            }




        }
        #endregion

        ArrayList finishes = new ArrayList();

        #region  百度地图采集

        public void baidu()

        {

            try

            {
                // string[] citys = textBox1.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                if (textBox3.Text == "")
                {
                    MessageBox.Show("请输入关键字");
                    return;
                }
                ArrayList citys = new ArrayList();
                foreach (var item in comboBox2.Items)
                {                
                        citys.Add(item);

                }
                 if (comboBox1.Text == "全国")
                {
                    citys.Add("北京");  //获取 citycode;
                    citys.Add("上海");
                    citys.Add("天津");
                    citys.Add("重庆");
                    citys.Add("广州");
                    citys.Add("深圳");
                    citys.Add("杭州");
                }


                citys.RemoveAt(0);

                string[] keywords = textBox3.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);

                int pages = 200;


                foreach (string city in citys)

                {
                    int cityid = getcityId(city);  //获取 citycode;
                   
                    if (comboBox1.Text.Trim() == "北京")
                    {
                        
                        cityid = getcityId("北京市");  //获取 citycode;
                        
                    }
                   else if (comboBox1.Text == "上海")
                    {
                        cityid = getcityId("上海市");  //获取 citycode;
                    }
                    else if (comboBox1.Text == "天津")
                    {
                        cityid = getcityId("天津市");  //获取 citycode;
                    }
                    else if (comboBox1.Text== "重庆")
                    {
                        cityid = getcityId("重庆市");  //获取 citycode;
                    }

                    else if (comboBox2.Text != "全省")
                    {
                        cityid = getcityId(comboBox2.Text);  //获取 citycode;
                    }
                   

                    foreach (string keyword in keywords)

                    {

                        for (int i = 0; i <= pages; i++)

                        {

                            
                            int j = i - 1 > 0 ? i - 1 : 0;

                            String Url = "https://map.baidu.com/?newmap=1&reqflag=pcmap&biz=1&from=webmap&da_par=direct&pcevaname=pc4.1&qt=con&from=webmap&c=" + cityid + "&wd=" + keyword + "&wd2=&pn=" + i + "&nn=" + j + "0&db=0&sug=0&addr=0&pl_data_type=cater&pl_price_section=0%2C%2B&pl_sort_type=data_type&pl_sort_rule=0&pl_discount2_section=0%2C%2B&pl_groupon_section=0%2C%2B&pl_cater_book_pc_section=0%2C%2B&pl_hotel_book_pc_section=0%2C%2B&pl_ticket_book_flag_section=0%2C%2B&pl_movie_book_section=0%2C%2B&pl_business_type=cater&pl_business_id=&da_src=pcmappg.poi.page&on_gel=1&src=7&gr=3&l=12";



                            string html = method.GetUrl(Url,"utf-8");


                            MatchCollection TitleMatchs = Regex.Matches(html, @"""primary_uid"":""([\s\S]*?)""", RegexOptions.IgnoreCase);

                            ArrayList lists = new ArrayList();

                            foreach (Match NextMatch in TitleMatchs)
                            {


                                lists.Add(NextMatch.Groups[1].Value);

                            }
                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;

                            string tm1 = DateTime.Now.ToString();  //获取系统时间

                          

                            JsonParser jsonParser = JsonConvert.DeserializeObject<JsonParser>(html);



                            foreach (Content content in jsonParser.Content)
                            {

                                if (content.tel !=null&&!finishes.Contains(content.name))
                                {
                                    finishes.Add(content.name);
                                    ListViewItem lv1 = listView2.Items.Add(listView2.Items.Count.ToString());
                                    lv1.SubItems.Add(content.name);
                                    lv1.SubItems.Add(content.tel);
                                  
                                    lv1.SubItems.Add(content.addr);
                                    lv1.SubItems.Add(keyword.Trim());
                                    if (listView2.Items.Count - 1 > 1)
                                    {
                                        listView2.EnsureVisible(listView2.Items.Count - 1);
                                    }
                                    if (status == false)
                                    {
                                        return;
                                    }
                                }

                                Application.DoEvents();
                                Thread.Sleep(10);   //内容获取间隔，可变量
                            }
                        }

                    }
                }
                button2.Enabled = true;
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

                        textBox1.Text = Url;

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

                        MessageBox.Show(lists.Count.ToString());
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
               MessageBox.Show( ex.ToString());
            }
        }
        #endregion 
        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            status = true;
            if (denglu == false)
            {
                MessageBox.Show("请先登录您的账号！");
                return;
            }
            Thread thread = new Thread(new ThreadStart(huicong));
            thread.Start();


            //for (int i = 0; i <5; i++)
            //{
            //    Thread thread = new Thread(new ThreadStart(baidu));
            //    thread.Start();
            //}



            //#region 慧聪网通用登录

            //bool value = false;
            //string html = method.GetUrl("http://acaiji.com/success/ip.php","utf-8");
            //string localip = method.GetIP();
            //MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            //foreach (Match ip in ips)
            //{
            //    if (ip.Groups[1].Value.Trim() == localip.Trim())
            //    {
            //        value = true;
            //        break;
            //    }

            //}
            //if (value == true)
            //{
            //    //--------登陆函数------------------
            //    Thread thread2 = new Thread(new ThreadStart(huicong));
            //    thread2.Start();

            //}
            //else
            //{
            //    MessageBox.Show("请登录您的账号！");
            //    System.Diagnostics.Process.Start("http://www.acaiji.com");
            //    return;
            //}
            //#endregion

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



                string constr = "Host =47.99.68.92;Database=vip_database;Username=root;Password=zhoukaige00.@*.";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select * from vip where username='" + textBox1.Text.Trim() + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'
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
            if (listView2.Visible == true)
            {
                method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
                return;
            }

            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);

        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            listView2.Items.Clear();
                button2.Enabled = true;
           
        }

        private void button7_Click(object sender, EventArgs e)
        {
            status = false;
            button2.Enabled = true;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
           

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.comboBox2.Items.Clear();
            this.comboBox2.Text="";
            if (this.comboBox1.Text == "全国")
            {
                this.comboBox2.Items.Add("中国");

            }
            else if (this.comboBox1.Text == "北京")
            {
                this.comboBox2.Items.Add("北京市");
                this.comboBox2.Items.Add("北京市");
            }
            else if (this.comboBox1.Text == "天津")
            {
                this.comboBox2.Items.Add("天津市");
                this.comboBox2.Items.Add("天津市");
            }
            else if (this.comboBox1.Text == "重庆")
            {
                this.comboBox2.Items.Add("重庆市");
                this.comboBox2.Items.Add("重庆市");
            }
            else if (this.comboBox1.Text == "上海")
            {
                this.comboBox2.Items.Add("上海市");
                this.comboBox2.Items.Add("上海市");
            }
            else if (this.comboBox1.Text == "河北省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("石家庄市"); comboBox2.Items.Add("保定市"); comboBox2.Items.Add("沧州市"); comboBox2.Items.Add("廊坊市"); comboBox2.Items.Add("唐山市"); comboBox2.Items.Add("邢台市"); comboBox2.Items.Add("邯郸市"); comboBox2.Items.Add("衡水市"); comboBox2.Items.Add("秦皇岛市"); comboBox2.Items.Add("张家口市"); comboBox2.Items.Add("承德市");
            }
            else if (this.comboBox1.Text == "山西省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("太原市"); comboBox2.Items.Add("运城市"); comboBox2.Items.Add("临汾市"); comboBox2.Items.Add("大同市"); comboBox2.Items.Add("长治市"); comboBox2.Items.Add("晋城市"); comboBox2.Items.Add("吕梁市"); comboBox2.Items.Add("衡水市"); comboBox2.Items.Add("阳泉市"); comboBox2.Items.Add("忻州市"); comboBox2.Items.Add("朔州市"); comboBox2.Items.Add("晋中市");
            }
            else if (this.comboBox1.Text == "内蒙古自治区")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("呼和浩特市"); comboBox2.Items.Add("包头市"); comboBox2.Items.Add("赤峰市"); comboBox2.Items.Add("呼伦贝尔市"); comboBox2.Items.Add("通辽市"); comboBox2.Items.Add("鄂尔多斯市"); comboBox2.Items.Add("巴彦淖尔盟市"); comboBox2.Items.Add("锡林郭勒市"); comboBox2.Items.Add("乌兰察布市"); comboBox2.Items.Add("兴安盟"); comboBox2.Items.Add("乌海市"); comboBox2.Items.Add("阿拉善盟市");
            }
            else if (this.comboBox1.Text == "辽宁省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("沈阳市"); comboBox2.Items.Add("大连市"); comboBox2.Items.Add("鞍山市"); comboBox2.Items.Add("锦州市"); comboBox2.Items.Add("营口市"); comboBox2.Items.Add("丹东市"); comboBox2.Items.Add("抚顺市"); comboBox2.Items.Add("朝阳市"); comboBox2.Items.Add("葫芦岛市"); comboBox2.Items.Add("铁岭市"); comboBox2.Items.Add("辽阳市"); comboBox2.Items.Add("盘锦市"); comboBox2.Items.Add("阜新市"); comboBox2.Items.Add("本溪市");
            }
            else if (this.comboBox1.Text == "吉林省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("长春市"); comboBox2.Items.Add("吉林市"); comboBox2.Items.Add("延边朝鲜族自治州"); comboBox2.Items.Add("通化市"); comboBox2.Items.Add("四平市"); comboBox2.Items.Add("白城市"); comboBox2.Items.Add("松原市"); comboBox2.Items.Add("白山市"); comboBox2.Items.Add("辽源市");

            }
            else if (this.comboBox1.Text == "黑龙江省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("哈尔滨市"); comboBox2.Items.Add("大庆市"); comboBox2.Items.Add("齐齐哈尔市"); comboBox2.Items.Add("佳木斯市"); comboBox2.Items.Add("伊春市"); comboBox2.Items.Add("牡丹江市"); comboBox2.Items.Add("鸡西市"); comboBox2.Items.Add("黑河市"); comboBox2.Items.Add("绥化市"); comboBox2.Items.Add("双鸭山市"); comboBox2.Items.Add("鹤岗市"); comboBox2.Items.Add("七台河市"); comboBox2.Items.Add("大兴安岭地区");
            }
            else if (this.comboBox1.Text == "江苏省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("苏州市"); comboBox2.Items.Add("南京市"); comboBox2.Items.Add("无锡市"); comboBox2.Items.Add("常州市"); comboBox2.Items.Add("徐州市"); comboBox2.Items.Add("南通市"); comboBox2.Items.Add("扬州市"); comboBox2.Items.Add("泰州市"); comboBox2.Items.Add("盐城市"); comboBox2.Items.Add("镇江市"); comboBox2.Items.Add("连云港市"); comboBox2.Items.Add("淮安市"); comboBox2.Items.Add("宿迁市"); 
            }
            else if (this.comboBox1.Text == "浙江省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("杭州市"); comboBox2.Items.Add("温州市"); comboBox2.Items.Add("宁波市"); comboBox2.Items.Add("金华市"); comboBox2.Items.Add("台州市"); comboBox2.Items.Add("嘉兴市"); comboBox2.Items.Add("绍兴市"); comboBox2.Items.Add("湖州市"); comboBox2.Items.Add("丽水市"); comboBox2.Items.Add("衢州市"); comboBox2.Items.Add("舟山市"); 
            }
            else if (this.comboBox1.Text == "安徽省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("合肥市"); comboBox2.Items.Add("淮北市"); comboBox2.Items.Add("安庆市"); comboBox2.Items.Add("芜湖市"); comboBox2.Items.Add("阜阳市"); comboBox2.Items.Add("滁州市"); comboBox2.Items.Add("蚌埠市"); comboBox2.Items.Add("马鞍山市"); comboBox2.Items.Add("六安市"); comboBox2.Items.Add("巢湖市"); comboBox2.Items.Add("宣城市"); comboBox2.Items.Add("淮南市"); comboBox2.Items.Add("亳州市"); comboBox2.Items.Add("黄山市"); comboBox2.Items.Add("池州市"); comboBox2.Items.Add("铜陵市"); comboBox2.Items.Add("宿州市");
            }
            else if (this.comboBox1.Text == "福建省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("厦门市"); comboBox2.Items.Add("泉州市"); comboBox2.Items.Add("福州市"); comboBox2.Items.Add("漳州市"); comboBox2.Items.Add("莆田市"); comboBox2.Items.Add("龙岩市"); comboBox2.Items.Add("宁德市"); comboBox2.Items.Add("三明市"); comboBox2.Items.Add("南平市"); 
            }
            else if (this.comboBox1.Text == "江西省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("南昌市"); comboBox2.Items.Add("抚州市"); comboBox2.Items.Add("赣州市"); comboBox2.Items.Add("九江市"); comboBox2.Items.Add("上饶市"); comboBox2.Items.Add("吉安市"); comboBox2.Items.Add("景德镇市"); comboBox2.Items.Add("萍乡市"); comboBox2.Items.Add("新余市"); comboBox2.Items.Add("宜春市"); comboBox2.Items.Add("鹰潭市"); 
            }
            else if (this.comboBox1.Text == "山东省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("济南市"); comboBox2.Items.Add("青岛市"); comboBox2.Items.Add("淄博市"); comboBox2.Items.Add("枣庄市"); comboBox2.Items.Add("东营市"); comboBox2.Items.Add("烟台市"); comboBox2.Items.Add("莱阳市"); comboBox2.Items.Add("潍坊市"); comboBox2.Items.Add("济宁市"); comboBox2.Items.Add("泰安市"); comboBox2.Items.Add("威海市"); comboBox2.Items.Add("日照市"); comboBox2.Items.Add("滨州市"); comboBox2.Items.Add("德州市"); comboBox2.Items.Add("聊城市"); comboBox2.Items.Add("临沂市"); comboBox2.Items.Add("菏泽市"); comboBox2.Items.Add("莱芜市");
            }
            else if (this.comboBox1.Text == "河南省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("郑州市"); comboBox2.Items.Add("洛阳市"); comboBox2.Items.Add("新乡市"); comboBox2.Items.Add("南阳市"); comboBox2.Items.Add("安阳市"); comboBox2.Items.Add("焦作市"); comboBox2.Items.Add("许昌市"); comboBox2.Items.Add("商丘市"); comboBox2.Items.Add("平顶山市"); comboBox2.Items.Add("周口市"); comboBox2.Items.Add("信阳市"); comboBox2.Items.Add("濮阳市"); comboBox2.Items.Add("开封市"); comboBox2.Items.Add("驻马店市"); comboBox2.Items.Add("鹤壁市"); comboBox2.Items.Add("三门峡市"); comboBox2.Items.Add("漯河市"); comboBox2.Items.Add("济源市");
            }
            else if (this.comboBox1.Text == "湖北省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("武汉市"); comboBox2.Items.Add("襄樊市"); comboBox2.Items.Add("宜昌市"); comboBox2.Items.Add("荆州市"); comboBox2.Items.Add("十堰市"); comboBox2.Items.Add("孝感市"); comboBox2.Items.Add("黄冈市"); comboBox2.Items.Add("恩施土家族苗族自治州"); comboBox2.Items.Add("黄石市"); comboBox2.Items.Add("荆门市"); comboBox2.Items.Add("随州市"); comboBox2.Items.Add("咸宁市"); comboBox2.Items.Add("鄂州市"); comboBox2.Items.Add("潜江市"); comboBox2.Items.Add("神农架林区市"); comboBox2.Items.Add("天门市"); comboBox2.Items.Add("仙桃市");
            }
            else if (this.comboBox1.Text == "湖南省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("长沙市"); comboBox2.Items.Add("湘潭市"); comboBox2.Items.Add("衡阳市"); comboBox2.Items.Add("株洲市"); comboBox2.Items.Add("郴州市"); comboBox2.Items.Add("常德市"); comboBox2.Items.Add("邵阳市"); comboBox2.Items.Add("岳阳市"); comboBox2.Items.Add("怀化市"); comboBox2.Items.Add("永州市"); comboBox2.Items.Add("娄底市"); comboBox2.Items.Add("益阳市"); comboBox2.Items.Add("张家界市");
            }
            else if (this.comboBox1.Text == "广东省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("深圳市"); comboBox2.Items.Add("广州市"); comboBox2.Items.Add("东莞市"); comboBox2.Items.Add("佛山市"); comboBox2.Items.Add("中山市"); comboBox2.Items.Add("惠州市"); comboBox2.Items.Add("珠海市"); comboBox2.Items.Add("汕头市"); comboBox2.Items.Add("江门市"); comboBox2.Items.Add("肇庆市"); comboBox2.Items.Add("揭阳市"); comboBox2.Items.Add("梅州市"); comboBox2.Items.Add("茂名市"); comboBox2.Items.Add("潮州市"); comboBox2.Items.Add("清远市"); comboBox2.Items.Add("韶关市"); comboBox2.Items.Add("潜江市"); comboBox2.Items.Add("河源市"); comboBox2.Items.Add("汕尾市"); comboBox2.Items.Add("云浮市"); comboBox2.Items.Add("阳江市");
            }
            else if (this.comboBox1.Text == "广西省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("南宁市"); comboBox2.Items.Add("桂林市"); comboBox2.Items.Add("柳州市"); comboBox2.Items.Add("玉林市"); comboBox2.Items.Add("贵港市"); comboBox2.Items.Add("百色市"); comboBox2.Items.Add("梧州市"); comboBox2.Items.Add("北海市"); comboBox2.Items.Add("钦州市"); comboBox2.Items.Add("河池市"); comboBox2.Items.Add("防城港市"); comboBox2.Items.Add("来宾市"); comboBox2.Items.Add("贺州市"); comboBox2.Items.Add("崇左市");
            }
            else if (this.comboBox1.Text == "海南省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("海口市"); comboBox2.Items.Add("三亚市"); comboBox2.Items.Add("文昌市");
            }
            else if (this.comboBox1.Text == "四川省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("成都市"); comboBox2.Items.Add("绵阳市"); comboBox2.Items.Add("德阳市"); comboBox2.Items.Add("南充市"); comboBox2.Items.Add("宜宾市"); comboBox2.Items.Add("乐山市"); comboBox2.Items.Add("泸州市"); comboBox2.Items.Add("达州市"); comboBox2.Items.Add("自贡市"); comboBox2.Items.Add("广元市"); comboBox2.Items.Add("广安市"); comboBox2.Items.Add("内江市"); comboBox2.Items.Add("攀枝花市");
            }
            else if (this.comboBox1.Text == "贵州省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("贵阳市"); comboBox2.Items.Add("遵义市"); comboBox2.Items.Add("六盘水市"); comboBox2.Items.Add("安顺市"); 
            }
            else if (this.comboBox1.Text == "云南省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("昆明市"); 
            }
            else if (this.comboBox1.Text == "西藏自治区")
            {
                comboBox2.Items.Add("全省");
            }
            else if (this.comboBox1.Text == "陕西省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("西安市"); comboBox2.Items.Add("榆林市"); comboBox2.Items.Add("宝鸡市"); comboBox2.Items.Add("汉中市"); comboBox2.Items.Add("咸阳市"); comboBox2.Items.Add("渭南市"); comboBox2.Items.Add("延安市"); comboBox2.Items.Add("安康市"); comboBox2.Items.Add("商洛市"); comboBox2.Items.Add("铜川市"); 
            }
            else if (this.comboBox1.Text == "甘肃省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("兰州市"); comboBox2.Items.Add("白银市"); comboBox2.Items.Add("天水市"); comboBox2.Items.Add("酒泉市"); comboBox2.Items.Add("庆阳市"); comboBox2.Items.Add("平凉市"); comboBox2.Items.Add("张掖市"); comboBox2.Items.Add("武威市"); comboBox2.Items.Add("陇南市"); comboBox2.Items.Add("定西市"); comboBox2.Items.Add("金昌市"); comboBox2.Items.Add("嘉峪关市"); 
            }
            
            else if (this.comboBox1.Text == "青海省")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("西宁市"); 
            }
            else if (this.comboBox1.Text == "宁夏回族自治区")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("银川市"); 
            }
            else if (this.comboBox1.Text == "新疆维吾尔自治区")
            {
                comboBox2.Items.Add("全省"); comboBox2.Items.Add("乌鲁木齐市"); 
            }
























        }
    }
}


