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
using CCWin;
using System.Threading;
using System.Collections;
using MySql.Data.MySqlClient;


namespace _58
{
    public partial class dianping : Form
    {
        public dianping()
        {
            InitializeComponent();
        }

        #region 根据城市拼音获取所有区域名称
        public void getarea(string citypinyin)

        {
            try
            {
                string constr = "Host =116.62.62.62;Database=citys;Username=root;Password=zhoukaige";
                string str = "SELECT area from dianping_area Where citypinyin= '" + citypinyin + "' ";
                MySqlDataAdapter da = new MySqlDataAdapter(str, constr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    textBox2.Text += dr[0].ToString().Trim() + "\r\n";
                }
            }
            catch (MySqlException ee)
            {
                MessageBox.Show(ee.Message.ToString());
            }
        }

        #endregion

        #region 根据区域名称转化为区域id
        public int getareaid(string area)

        {
            try
            {



                string constr = "Host =116.62.62.62;Database=citys;Username=root;Password=zhoukaige";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select areaid from dianping_area where area='" + area + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                reader.Read();

                int areaid = Convert.ToInt32(reader["areaid"].ToString().Trim());
                mycon.Close();
                reader.Close();
                return areaid;


            }

            catch (System.Exception ex)
            {
                return 1;
            }
        }

        #endregion


        #region 获取数据库点评城市名称
        public void getCityName()
        {
            ArrayList list = new ArrayList();
            try
            {
                string constr = "Host =116.62.62.62;Database=citys;Username=root;Password=zhoukaige";
                string str = "SELECT dianping_cityname from dianping_city ";
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

        #region  获取数据库中城市名称对应的拼音

        public string Getcitypinyin(string city)
        {

            try
            {



                string constr = "Host =116.62.62.62;Database=citys;Username=root;Password=zhoukaige";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select dianping_citypinyin from dianping_city where dianping_cityname='" + city + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                reader.Read();

                string citypinyin = reader["dianping_citypinyin"].ToString().Trim();
                mycon.Close();
                reader.Close();
                return citypinyin;


            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return "";
            }


        }

        #endregion

        string item;


        #region 点评GET请求
        public static string dianPingGetUrl(string Url)
        {
            try
            {
                string COOKIE = "_lxsdk_cuid=16233c680c7c8-081a8e5d136c39-3b60450b-1fa400-16233c680c7c8; _lxsdk=16233c680c7c8-081a8e5d136c39-3b60450b-1fa400-16233c680c7c8; _hc.v=8165c808-5cfd-45a0-0d36-3e7f9d78b4eb.1521287070; s_ViewType=10; switchcityflashtoast=1; _tr.u=jU5PhtbV9q3sLhwV; Hm_lvt_dbeeb675516927da776beeb1d9802bd4=1521289865; __mta=46068251.1521289881671.1521289895545.1521289900801.3; aburl=1; ua=dpuser_5678141658; ctu=90a81cde43e1e0934a456ec54b747c93d0e6b58b8c9732b3ed676c7795f37d7a; UM_distinctid=162f25ab3203f2-07621a0921189d-782c6036-2a490-162f25ab321924; CNZZDATA1261883731=1963307737-1524479289-null%7C1534814379; dper=52bfa729309c5043be1e1754564f188bc4a82bde7c2afce6b251a8a44a6622fb41a9a1dbe5ce4e5080e08f435f39ef1d6aa42f2923e27ce3546667fea02bdcbb870a434de1d973bb63aff45e3ab2156a2b0ec9f56ebfedcd50c7daf26ed4abb6; uamo=17606117606; _lx_utm=utm_source%3DBaidu%26utm_medium%3Dorganic; default_ab=shop%3AA%3A1%7CshopList%3AA%3A1; cityid=840; cy=57; cye=alashan; m_flash2=1; ll=7fd06e815b796be3df069dec7836c3df; pvhistory=6L+U5ZuePjo8L2Vycm9yL2Vycm9yX3BhZ2U+OjwxNTM1MDk1MDAzNzAzXV9b; _lxsdk_s=1656ac1a863-5a6-712-465%7C%7C102";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                Random r = new Random();

                int j = r.Next(6); //小于6的随机数,不包括6

                string[] lists =

                {
                 "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.2; Win64; x64; Trident/7.0)",
                 "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.97 Safari/537.11",
                 "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:20.0) Gecko/20100101 Firefox/20.0",
                 "Mozilla/5.0 (Linux; U; Android 2.3.7; en-us; Nexus One Build/FRF91) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1",
                 "Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_3_3 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8J2 Safari/6533.18.5",
                 "Mozilla/5.0 (iPad; U; CPU OS 4_3_3 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8J2 Safari/6533.18.5",

                };

                request.UserAgent = lists[j];

                request.Headers.Add("Cookie", COOKIE);

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string content = reader.ReadToEnd();
                reader.Close();
                reader.Dispose();
                return content;

            }
            catch (System.Exception ex)
            {
                ex.ToString();



            }
            return "";
        }
        #endregion

        #region  点评采集
        public void dianping_run()
        {
            Method md = new Method();

            if (this.item == "" || this.item == null)
            {
                MessageBox.Show("请选择分类");
                return;
            }


            visualButton2.Text = "停止采集";

            string[] areas = textBox2.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);

            try
            {


                int page = 50;


                string city = Getcitypinyin(comboBox1.SelectedItem.ToString());
                foreach (string area in areas)

                {
                    string areaid = getareaid(area).ToString();

                    for (int i = 1; i <= page; i++)

                    {


                        String Url = "http://www.dianping.com/" + city + "/" + item + "r" + areaid + "p" + i;

                        
                        string strhtml = dianPingGetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()


                        string Rxg = @"\'cl_to_s\',([\s\S]*?)\)";



                        MatchCollection all = Regex.Matches(strhtml, Rxg);

                        ArrayList lists = new ArrayList();
                        foreach (Match NextMatch in all)
                        {


                            lists.Add("http://m.dianping.com/shop/" + NextMatch.Groups[1].Value);



                        }



                        if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;

                        string tm1 = DateTime.Now.ToString();  //获取系统时间

                        textBox1.Text += tm1 + "-->正在采集" + city + "" + item + "第" + i + "页" + "\r\n";


                        Application.DoEvents();
                        Thread.Sleep(1000);


                        foreach (string list in lists)
                        {

                            int index = this.skinDataGridView1.Rows.Add();    //利用skinDataGridView1.Rows.Add()事件为DataGridView控件增加新的行，该函数返回添加新行的索引号，即新行的行号，然后可以通过该索引号操作该行的各个单元格，如skinDataGridView1.Rows[index].Cells[0].Value = "1"。这是很常用也是很简单的方法。

                            String Url1 = list;
                            string strhtml1 = dianPingGetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()


                            string Rxg0 = @"<h1 class=""shopName"">([\s\S]*?)</h1>";
                            string Rxg1 = @"href=""tel:([\s\S]*?)""";
                            string Rxg2 = @"address"":""([\s\S]*?)""";

                            Match name = Regex.Match(strhtml1, Rxg0);

                            Match tell = Regex.Match(strhtml1, Rxg1);


                            Match addr = Regex.Match(strhtml1, Rxg2);



                            this.skinDataGridView1.Rows[index].Cells[0].Value = name.Groups[1].Value;
                            this.skinDataGridView1.CurrentCell = this.skinDataGridView1.Rows[index].Cells[0];  //让datagridview滚动到当前行

                            this.skinDataGridView1.Rows[index].Cells[1].Value = tell.Groups[1].Value.Trim();

                            this.skinDataGridView1.Rows[index].Cells[2].Value = addr.Groups[1].Value;




                            this.skinDataGridView1.Rows[index].Cells[3].Value = area;
                            this.skinDataGridView1.Rows[index].Cells[4].Value = list;



                            if (visualButton2.Text == "已停止")  //停止事件触发
                            {
                                return;

                            }
                            if (skinDataGridView1.Rows[index].Cells[1].Value.ToString() == "" && skinDataGridView1.Rows[index].Cells[2].Value.ToString() == "" && skinDataGridView1.Rows[index].Cells[3].Value.ToString() == "")  //停止事件触发
                            {
                                MessageBox.Show("点评需要验证一次请在打开的网页中完成验证！");
                                //System.Diagnostics.Process.Start("IEXPLORE.EXE",Url1 );
                                System.Diagnostics.Process.Start(Url1);

                            }



                            Application.DoEvents();
                            Thread.Sleep(1000);
                        }


                    }

                }

            }






            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #region  点评搜索采集
        public void dianping_search()
        {
            Method md = new Method();


            visualButton2.Text = "停止采集";

            try
            {


                int page = 50;
                int cityId = 100;
                string keyword = "宠物店";


                for (int i = 1; i <= page; i++)

                {


                    String Url = " https://www.dianping.com/search/keyword/" + cityId + "/0_" + keyword + "/p" + i;
                    string strhtml = dianPingGetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()


                    string Rxg = @"\'cl_to_s\',([\s\S]*?)\)";



                    MatchCollection all = Regex.Matches(strhtml, Rxg);

                    ArrayList lists = new ArrayList();
                    foreach (Match NextMatch in all)
                    {

                        lists.Add("http://m.dianping.com/shop/" + NextMatch.Groups[1].Value);

                    }



                    if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;



                    foreach (string list in lists)
                    {

                        int index = this.skinDataGridView1.Rows.Add();    //利用skinDataGridView1.Rows.Add()事件为DataGridView控件增加新的行，该函数返回添加新行的索引号，即新行的行号，然后可以通过该索引号操作该行的各个单元格，如skinDataGridView1.Rows[index].Cells[0].Value = "1"。这是很常用也是很简单的方法。

                        String Url1 = list;
                        string strhtml1 = dianPingGetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()



                        string Rxg0 = @"<h1 class=""shop-name"">([\s\S]*?)</h1>";
                        string Rxg1 = @"tel:([\s\S]*?)""";
                        string Rxg2 = @"address"":""([\s\S]*?)""";





                        MatchCollection name = Regex.Matches(strhtml1, Rxg0);

                        MatchCollection tell = Regex.Matches(strhtml1, Rxg1);

                        MatchCollection addr = Regex.Matches(strhtml1, Rxg2);





                        foreach (Match match in name)
                        {

                            this.skinDataGridView1.Rows[index].Cells[0].Value = match.Groups[1].Value.Trim();
                        }
                        foreach (Match match in tell)
                        {
                            this.skinDataGridView1.Rows[index].Cells[1].Value = match.Groups[1].Value;
                        }

                        foreach (Match match in addr)
                        {
                            this.skinDataGridView1.Rows[index].Cells[2].Value = match.Groups[1].Value;
                        }




                        this.skinDataGridView1.Rows[index].Cells[3].Value = "";




                        this.skinDataGridView1.CurrentCell = this.skinDataGridView1.Rows[index].Cells[0];  //让datagridview滚动到当前行





                        if (visualButton2.Text == "已停止")  //停止事件触发
                        {

                            return;


                        }



                        Application.DoEvents();
                        Thread.Sleep(1000);



                    }

                }

            }






            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion


        private void dianping_Load(object sender, EventArgs e)
        {
            getCityName();
        }



        private void 注册ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            register rg = new register();
            rg.Show();

        }

        private void 登陆ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login lg = new Login();
            lg.Show();
        }



        private void visualButton1_Click(object sender, EventArgs e)
        {
            if (label2.Text == "测试用户" || label2.Text == "")
            {
                MessageBox.Show("请注册账号登陆！");
                register rg = new register();
                rg.Show();
                return;
            }

            Thread Search_thread = new Thread(new ThreadStart(dianping_run));
            Control.CheckForIllegalCrossThreadCalls = false;
            Search_thread.Start();
        }

        private void visualButton2_Click(object sender, EventArgs e)
        {
            visualButton2.Text = "已停止";
        }

        private void visualButton3_Click(object sender, EventArgs e)
        {
            Method.DgvToTable(this.skinDataGridView1);
            Method.DataTableToExcel(Method.DgvToTable(this.skinDataGridView1), "Sheet1", true);
            Method.importtodatabase(skinDataGridView1);   //采集数据导入数据库。
        }

        private void visualButton4_Click(object sender, EventArgs e)
        {
            Method.Txt(skinDataGridView1);
        }

        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            skinDataGridView1.Rows.Clear();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = null;


            string city = Getcitypinyin(comboBox1.SelectedItem.ToString());

            getarea(city);

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.item = e.Node.Name;
        }

        private void skinDataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu menu = new ContextMenu();   //初始化menu


                menu.MenuItems.Add("清空数据");   //添加菜单项c1
                                              //menu.MenuItems.Add("添加城市");   //添加菜单项c2

                menu.Show(skinDataGridView1, new Point(e.X, e.Y));   //在点(e.X, e.Y)处显示menu

                menu.MenuItems[0].Click += new EventHandler(clear);
            }

        }
        public void clear(object sender, EventArgs e)  //清空表格
        {

            skinDataGridView1.Rows.Clear();

        }

        private void 注册账号ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            register rg = new register();
            rg.Show();
        }

        private void 登陆账号ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login lg = new Login();
            lg.Show();
        }

        private void dianping_MouseEnter(object sender, EventArgs e)
        {
            label2.Text = Method.User;
        }
    }
}
