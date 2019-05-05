using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections;
using MySql.Data.MySqlClient;
namespace _58
{
    

    public partial class meituan_search : Form
    {
        int index { get; set; }
        string cookie { get; set; }

        bool zanting = true;

        public meituan_search()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false; // 设置线程之间可以操作
        }

        #region 获取数据库美团城市名称
        public void getCityName()
        {
            ArrayList list = new ArrayList();
            try
            {
                string constr = "Host =47.99.68.92;Database=citys;Username=root;Password=zhoukaige00.@*.";
                string str = "SELECT name from meituan_province_city ";
                MySqlDataAdapter da = new MySqlDataAdapter(str, constr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString().Trim());
                }
            }
            catch (MySqlException ee)
            {
                MessageBox.Show(ee.Message.ToString());
            }
            comboBox1.DataSource = list;

        }
        #endregion
        private void meituan_search_Load(object sender, EventArgs e)
        {

         getCityName();

            this.cookie = "client-id=f66b7fc9-3f5b-4a1c-b702-fed6ff564ecf;";

        }
        //#region  radiobutton点击事件
        //public void radioclick()
        //{
        //    foreach (Control c in this.groupBox2.Controls)
        //    {
        //        if (c is RadioButton)
        //        {
        //            if (((RadioButton)c).Checked == true)
        //            {
        //                textBox2.Text = ((RadioButton)c).Text;

        //            }
        //        }

        //    }
        //}
        //#endregion



//private：只能在本类中使用
//protected：在本类中及其子类中可以使用
//internal：同一命名空间（程序集）中的类可以使用
//public：所有类均可使用
//从上到下，私有程度逐渐降低


        

        #region GET请求
        public static string meituan_GetUrl(string Url,string COOKIE)
        {
            try
            {
               

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.97 Safari/537.11";
              
                request.Headers.Add("Cookie",COOKIE);  


                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string content = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                ex.ToString();



            }
            return "";
        }
        #endregion

        #region  获取数据库中城市名称对应的拼音

        public string Getpinyin(string city)
        {

            try
            {
                string constr = "Host =47.99.68.92;Database=citys;Username=root;Password=zhoukaige00.@*.";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select meituan_city_pinyin from meituan_city where meituan_city_name='" + city + "'  ", mycon);         //SQL语句读取textbox的值'"+textBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                reader.Read();
   
                string citypinyin = reader["meituan_city_pinyin"].ToString().Trim();
                mycon.Close();
                reader.Close();
                return citypinyin;


            }

            catch (System.Exception ex)
            {
                return ex.ToString();
            }
            

        }

        #endregion

        public string Rxg0;
        public string Rxg1;
        public string Rxg2;

        private bool Condition = true;  //定义数据采集筛选条件，默认为真不筛选
        private Match tell;

        #region  所有分类搜索手机端主函数一
        public void Search1()
        {
            Method md = new Method();



            try
            {
                string[] citys = textBox1.Text.Trim().Split(new string[] { "," }, StringSplitOptions.None);

                int page = 50;

                if (textBox1.Text.Trim()== "")
                {
                    MessageBox.Show("请输入城市！");
                    return;
                }

                if (textBox2.Text.Trim() == "")
                {
                    MessageBox.Show("请输入关键字!");
                    return;
                }



                string[] keywords = textBox2.Text.Trim().Split(new string[] { "," }, StringSplitOptions.None);



                foreach (string city in citys)

                {
                    
                    foreach (string keyword in keywords)

                    {
                    
                        if (checkBox1.Checked==true)
                        {
                            this.Rxg0 = @"""name"":""([\s\S]*?)""";
                            this.Rxg1 = @"""phone"":""([\s\S]*?)""";
                            this.Rxg2 = @"""addr"":""([\s\S]*?)""";

                            for (int i = 1; i <= page; i++)

                            {


                                String Url = "http://i.meituan.com/s/"+ Getpinyin(city) + "-" + keyword + "?p=" + i;
                                string strhtml = meituan_GetUrl(Url,this.cookie);  //定义的GetRul方法 返回 reader.ReadToEnd()

                                string Rxg = @"data-href=""//i.meituan.com/poi/([\s\S]*?)"">";

                               

                                MatchCollection all = Regex.Matches(strhtml, Rxg);

                                ArrayList lists = new ArrayList();
                                foreach (Match NextMatch in all)
                                {


                                    lists.Add("http://i.meituan.com/poi/" + NextMatch.Groups[1].Value);



                                }

                                if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                    break;

                                string tm1 = DateTime.Now.ToString();  //获取系统时间

                                textBox3.Text = tm1 + "-->正在采集" + city + "" + keyword + "第" + i + "页" + "\r\n";

                                foreach (string list in lists)

                                {


                                    String Url1 = list;
                                    string strhtml1 = meituan_GetUrl(Url1, this.cookie);  //定义的GetRul方法 返回 reader.ReadToEnd()



                                    Match name = Regex.Match(strhtml1, Rxg0);
                                    this.tell = Regex.Match(strhtml1, Rxg1);
                                    Match addr = Regex.Match(strhtml1, Rxg2);

                                    int a = this.tell.Groups[1].Value.IndexOf("/");   //获取/的索引

                                    if (checkBox2.Checked == true)
                                    {
                                        this.Condition = this.tell.Groups[1].Value != "" && this.tell.Groups[1].Value.Remove(0, a + 1).IndexOf("-") == -1;
                                    }

                                    if (this.Condition)
                                    {
                                        this.index = this.dataGridView1.Rows.Add();    //利用dataGridView1.Rows.Add()事件为DataGridView控件增加新的行，该函数返回添加新行的索引号，即新行的行号，然后可以通过该索引号操作该行的各个单元格，如dataGridView1.Rows[index].Cells[0].Value = "1"。这是很常用也是很简单的方法。

                                        this.dataGridView1.Rows[index].Cells[0].Value = index;
                                        this.dataGridView1.Rows[index].Cells[1].Value = name.Groups[1].Value;


                                        this.dataGridView1.Rows[index].Cells[2].Value = this.tell.Groups[1].Value.Remove(0, a + 1);

                                        this.dataGridView1.Rows[index].Cells[3].Value = addr.Groups[1].Value;

                                        this.dataGridView1.Rows[index].Cells[4].Value = city;


                                        this.dataGridView1.CurrentCell = this.dataGridView1.Rows[index].Cells[0];  //让datagridview滚动到当前行


                                        while (this.zanting == false)
                                        {
                                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                        }

                                    }


                                   

                                    Application.DoEvents();
                                    Thread.Sleep(1000);


                                }

                            }
                        }
                        else
                        {
                            this.Rxg0 = @"<h1 class=""dealcard-brand"">([\s\S]*?)</h1>";
                            this.Rxg1 = @"data-tele=""([\s\S]*?)""";
                            this.Rxg2 = @"addr:([\s\S]*?)&";
                            string rxg4 = @"营业时间</h6><p>([\s\S]*?)</p>";
                            string rxg5 = @"poi_lng"": ""([\s\S]*?)""";
                            string rxg6 = @"poi_lat"": ""([\s\S]*?)""";



                            for (int i = 1; i <= page; i++)

                            {


                                String Url = "http://i.meituan.com/s/" + Getpinyin(city) + "-" + keyword + "?p=" + i;
                                string strhtml = meituan_GetUrl(Url,this.cookie);  //定义的GetRul方法 返回 reader.ReadToEnd()

                                string Rxg = @"data-href=""//i.meituan.com/poi/([\s\S]*?)"">";



                                MatchCollection all = Regex.Matches(strhtml, Rxg);

                                ArrayList lists = new ArrayList();
                                foreach (Match NextMatch in all)
                                {


                                    lists.Add("http://i.meituan.com/poi/" + NextMatch.Groups[1].Value);



                                }

                                if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                    break;

                                string tm1 = DateTime.Now.ToString();  //获取系统时间

                                textBox3.Text += tm1 + "-->正在采集" + city + "" + keyword + "第" + i + "页" + "\r\n";

                                foreach (string list in lists)

                                {


                                    String Url1 = list;
                                    string strhtml1 = meituan_GetUrl(Url1, this.cookie);  //定义的GetRul方法 返回 reader.ReadToEnd()

                                   

                                    Match name = Regex.Match(strhtml1, Rxg0);
                                    this.tell = Regex.Match(strhtml1, Rxg1);
                                    Match addr = Regex.Match(strhtml1, Rxg2);
         
                                    Match time = Regex.Match(strhtml1, rxg4);
                                    Match lng = Regex.Match(strhtml1, rxg5);
                                    Match lat = Regex.Match(strhtml1, rxg6);

                                    int a = this.tell.Groups[1].Value.IndexOf("/");   //获取/的索引

                                    if (checkBox2.Checked == true)
                                    {
                                        this.Condition = this.tell.Groups[1].Value != "" && this.tell.Groups[1].Value.Remove(0, a + 1).IndexOf("-") == -1;
                                    }

                                    if (this.Condition)
                                    {
                                        this.index = this.dataGridView1.Rows.Add();    //利用dataGridView1.Rows.Add()事件为DataGridView控件增加新的行，该函数返回添加新行的索引号，即新行的行号，然后可以通过该索引号操作该行的各个单元格，如dataGridView1.Rows[index].Cells[0].Value = "1"。这是很常用也是很简单的方法。

                                        this.dataGridView1.Rows[index].Cells[0].Value = index;
                                        this.dataGridView1.Rows[index].Cells[1].Value = name.Groups[1].Value;


                                        this.dataGridView1.Rows[index].Cells[2].Value = this.tell.Groups[1].Value.Remove(0, a + 1);

                                        this.dataGridView1.Rows[index].Cells[3].Value = addr.Groups[1].Value;

                                        this.dataGridView1.Rows[index].Cells[4].Value = city;
                                        this.dataGridView1.Rows[index].Cells[5].Value = list;
                                        this.dataGridView1.Rows[index].Cells[6].Value = time.Groups[1].Value.Trim();
                                        this.dataGridView1.Rows[index].Cells[7].Value = lng.Groups[1].Value.Trim();
                                        this.dataGridView1.Rows[index].Cells[8].Value = lat.Groups[1].Value.Trim();


                                        this.dataGridView1.CurrentCell = this.dataGridView1.Rows[index].Cells[0];  //让datagridview滚动到当前行
                                        while (this.zanting == false)
                                        {
                                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                        }

                                    }


                                   

                                    Application.DoEvents();
                                    Thread.Sleep(1000);


                                }



                            }
                        }



                      

                    }
                    

                }
                
            }

            catch (System.Exception ex)
            {
                textBox3.Text = ex.ToString();
            }
        }

        #endregion

        #region  所有分类搜索手机端主函数二(细分区域)
        public void Search2()
        {
            Method md = new Method();


          

            try
            {


                int page = 50;

              

                if (textBox5.Text.Trim() == "")
                {
                    MessageBox.Show("请输入关键字!");
                    return;
                }


                string city = comboBox1.SelectedItem.ToString();
                string[] keywords = textBox5.Text.Trim().Split(new string[] { "," }, StringSplitOptions.None);
                string[] areas = textBox4.Text.Trim().Split(new string[] { "," }, StringSplitOptions.None);




                foreach (string keyword in keywords)

                {
                    foreach (string area in areas)

                    {

                      //  string html1 = meituan_GetUrl("http://i.meituan.com/s/" + Getpinyin(city) + "-" + keyword +  "?p=1");

                        if (checkBox1.Checked == true)
                        {
                            this.Rxg0 = @"title:([\s\S]*?);";
                            this.Rxg1 = @"""phone"":""([\s\S]*?)""";

                        }
                        else
                        {
                            this.Rxg0 = @"<h1 class=""dealcard-brand"">([\s\S]*?)</h1>";
                            this.Rxg1 = @"data-tele=""([\s\S]*?)""";

                        }

                        for (int i = 1; i <= page; i++)

                        {


                            String Url = "http://i.meituan.com/s/" + Getpinyin(city) + "-" + keyword + "?bid=" + area+"&p="+i;
                            string strhtml = meituan_GetUrl(Url, this.cookie);  //定义的GetRul方法 返回 reader.ReadToEnd()

                            string Rxg = @"data-href=""//i.meituan.com/poi/([\s\S]*?)"">";



                            MatchCollection all = Regex.Matches(strhtml, Rxg);

                            ArrayList lists = new ArrayList();
                            foreach (Match NextMatch in all)
                            {


                                lists.Add("http://i.meituan.com/poi/" + NextMatch.Groups[1].Value);



                            }

                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;

                            string tm1 = DateTime.Now.ToString();  //获取系统时间

                            textBox3.Text = tm1 + "-->正在采集" + city + "" + keyword + "第" + i + "页" +"\r\n";

                            foreach (string list in lists)
                            {

                                int index = this.dataGridView1.Rows.Add();    //利用dataGridView1.Rows.Add()事件为DataGridView控件增加新的行，该函数返回添加新行的索引号，即新行的行号，然后可以通过该索引号操作该行的各个单元格，如dataGridView1.Rows[index].Cells[0].Value = "1"。这是很常用也是很简单的方法。

                                String Url1 = list;
                                string strhtml1 = meituan_GetUrl(Url1,this.cookie);  //定义的GetRul方法 返回 reader.ReadToEnd()



                                string Rxg2 = @"addr:([\s\S]*?)&";




                                Match name = Regex.Match(strhtml1, Rxg0);

                                Match tell = Regex.Match(strhtml1, Rxg1);


                                Match addr = Regex.Match(strhtml1, Rxg2);




                                this.dataGridView1.Rows[index].Cells[0].Value = index;
                                this.dataGridView1.Rows[index].Cells[1].Value = name.Groups[1].Value;
                               
                                this.dataGridView1.Rows[index].Cells[2].Value = tell.Groups[1].Value;
                              
                                this.dataGridView1.Rows[index].Cells[3].Value = addr.Groups[1].Value;
                   

                                this.dataGridView1.Rows[index].Cells[4].Value = city;


                                Method.EachToData(dataGridView1, index);


                                this.dataGridView1.CurrentCell = this.dataGridView1.Rows[index].Cells[0];  //让datagridview滚动到当前行



                                //if (this.skinDataGridView1.Rows[index].Cells[1].Value.ToString().Contains("-"))//如果表格值包含“-”则。。。
                                //{


                                //    skinDataGridView1.Rows.RemoveAt(index);

                                //}

                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }

                                Application.DoEvents();
                                Thread.Sleep(1000);

                               

                            }



                        }

                    }


                }
            }


            catch (System.Exception ex)
            {
                textBox3.Text = ex.ToString();
            }
        }

        #endregion




        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("IEXPLORE.EXE", "http://wpa.qq.com/msgrd?v=3&uin=852266010&site=qq&menu=yes");
            
            System.Diagnostics.Process.Start("IEXPLORE.EXE", "http://www.acaiji.com");

        }



        #region 右击鼠标事件
        public void clear(object sender, EventArgs e)  //清空表格
        {

            dataGridView1.Rows.Clear();

        }

        #region  去除固话

        public void RemoveTell(object sender, EventArgs e)

        {

            try
            {
                for (int i = 0; i <= dataGridView1.Rows.Count; i++)
                {
                    string Lpv = dataGridView1.Rows[i].Cells[1].Value.ToString();

                    if (Lpv.Contains("-") && !Lpv.Contains("/"))                //Contains()包含, indexof()返回字符在字符串中首次出现的位置，若没有返回 -1
                    {
                        dataGridView1.Rows.RemoveAt(i);
                    }
                }
            }

            catch (System.Exception ex)
            {
                ex.ToString();
            }
        }

        #endregion
        #endregion






       

        #region  其他事件

      
        private void meituan_search_FormClosing(object sender, FormClosingEventArgs e)
        {
           
            MessageBox.Show("您确定要关闭吗？");
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("IEXPLORE.EXE", "http://wpa.qq.com/msgrd?v=3&uin=852266010&site=qq&menu=yes");
        }

        #endregion

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text += comboBox1.SelectedItem.ToString() + ",";


           
        }

            

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text += comboBox2.SelectedItem.ToString() + ",";
        }

      

     

       

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox4.Text = null;

            string cityPinYin = Getpinyin(comboBox3.SelectedItem.ToString());
            try
            {
                string constr = "Host =116.62.62.62;Database=citys;Username=root;Password=zhoukaige";
                string str = "SELECT meituan_area_id from meituan_area Where meituan_area_citypinyin= '" + cityPinYin + "' ";
                MySqlDataAdapter da = new MySqlDataAdapter(str, constr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    textBox4.Text += dr[0].ToString().Trim() + ",";
                }
            }
            catch (MySqlException ee)
            {
                MessageBox.Show(ee.Message.ToString());
            }


           

        }

      
       

      

        #region 测试获取数据库美团城市名称返回集合
        public ArrayList getpinyin()
        {
            ArrayList list = new ArrayList();
            try
            {
                string constr = "Host =47.99.68.92;Database=citys;Username=root;Password=zhoukaige00.@*.";
                string str = "SELECT meituan_area_citypinyin from meituan_area ";
                MySqlDataAdapter da = new MySqlDataAdapter(str, constr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString().Trim());
                }
            }
            catch (MySqlException ee)
            {
                MessageBox.Show(ee.Message.ToString());
            }
            return list;
        }


        public ArrayList getid()
        {
            ArrayList list = new ArrayList();
            try
            {
                string constr = "Host =47.99.68.92;Database=citys;Username=root;Password=zhoukaige00.@*.";
                string str = "SELECT meituan_area_id from meituan_area ";
                MySqlDataAdapter da = new MySqlDataAdapter(str, constr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString().Trim());
                }
            }
            catch (MySqlException ee)
            {
                MessageBox.Show(ee.Message.ToString());
            }
            return list;
        }

        public void run() {

            ArrayList list = getpinyin();
            ArrayList list1 = getid();
            for (int i = 9254; i > 0; i--)
            {
                int index = this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[index].Cells[1].Value = "http://i.meituan.com/s/" + list[i] + "-%E7%BE%8E%E9%A3%9F?bid=" + list1[i];
            }
            Application.DoEvents();
            Thread.Sleep(100);
        }


        #endregion




        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu menu = new ContextMenu();   //初始化menu

                menu.MenuItems.Add("去除固话");
                menu.MenuItems.Add("清空数据");   //添加菜单项c1
                                              //menu.MenuItems.Add("添加城市");   //添加菜单项c2

                menu.Show(dataGridView1, new Point(e.X, e.Y));   //在点(e.X, e.Y)处显示menu


                menu.MenuItems[0].Click += new EventHandler(RemoveTell);
                menu.MenuItems[1].Click += new EventHandler(clear);

                textBox3.Text = "";

            }
        }

        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            #region 通用登录

            bool value = false;
            string html = Method.GetUrl("http://acaiji.com/success/ip.php");
            string localip = Method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == localip.Trim())
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {
                if (tabControl1.SelectedIndex == 0) //简单地级市采集
                {
                    Thread search_thread = new Thread(new ThreadStart(this.Search1));

                    search_thread.Start();

                }
                else if (tabControl1.SelectedIndex == 1)//地级市区县精准采集
                {
                    Thread Search_thread = new Thread(new ThreadStart(this.Search2));

                    Search_thread.Start();

                }

            }
            else
            {
                MessageBox.Show("请登录您的账号！");
                System.Diagnostics.Process.Start("http://www.acaiji.com");
                return;
            }
            #endregion


           
            
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox5.Text += comboBox2.SelectedItem.ToString() + ",";
        }


        private void button3_Click(object sender, EventArgs e)
        {
            this.zanting = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.zanting = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Method.DgvToTable(this.dataGridView1);
            Method.DataTableToExcel(Method.DgvToTable(this.dataGridView1), "Sheet1", true);
        }

        private void dataGridView1_MouseEnter(object sender, EventArgs e)
        {
            label1.Text = Method.User; //获取Method公共类的静态变量User的值
        }

        private void TabPage1_MouseEnter(object sender, EventArgs e)
        {

        }
    }
}
