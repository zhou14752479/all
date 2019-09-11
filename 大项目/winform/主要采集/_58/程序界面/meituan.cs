using System;
using System.Windows.Forms;
using System.Xml;
using System.Text.RegularExpressions;

using System.Threading;
using System.Collections;
using MySql.Data.MySqlClient;
using System.Data;
using System.Drawing;
using System.Net;
using System.IO;
using System.Text;

namespace _58
{
    public partial class meituan : Form
    {

        bool status = true;

        public meituan()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {


        }

        #region GET请求
        public static string GetUrl(string Url)
        {
            try
            {
                string COOKIE = "_lxsdk_cuid=1619d225de7ad-0c5551b9982ceb-3b60490d-1fa400-1619d225de8c8; iuuid=EE452628FFED0D7E2E8057602BBF1FB40975D7AC4634E49AA9A0FDF0EEA3FC2F; _lxsdk=EE452628FFED0D7E2E8057602BBF1FB40975D7AC4634E49AA9A0FDF0EEA3FC2F; webp=1; _hc.v=e24e6fb3-392f-efd0-1faa-23a60600128d.1522554246; _ga=GA1.2.1049379255.1526306541; __mta=46562712.1518765142494.1532784659235.1533443717244.73; UM_distinctid=16556dfabd2f5-0f889f94418bdc-762e6d31-ee67c-16556dfabd316e; CNZZDATA1261883731=256675398-1534760379-null%7C1534808979; Hm_lvt_dbeeb675516927da776beeb1d9802bd4=1535603808; oc=HHdE1pzMFrdyJAh4aEBuGv_bXE5LSe-aHksZ9tr_sc4-RplxzhG0a9w-vgyjGw4e7nLCFL_rT0P5voF0RmqlIA-s5fsbOkZgu6cxdlreV5nixPpfpB6Z_Xb-Z8LqqLKzcOpgTWVZjJApjKPMcAwtdt3vQlMNkzzEDdwYh0Ks_uE; __utma=74597006.1049379255.1526306541.1537842327.1537842327.1; __utmz=74597006.1537842327.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); cityname=%E4%B8%8A%E6%B5%B7; i_extend=E190210055551694398011363996492082145081_e5686200127472754479_v1044412679615575934_a%e7%81%ab%e9%94%85H__a; ci=20; rvct=20%2C60%2C10; uuid=c95baff372264304b21c.1539563885.1.0.0; client-id=c1254b9c-48c9-4a5a-a663-d338895a10dd; lat=23.123472; lng=113.266592; _lxsdk_s=16675289303-dfe-348-0de%7C%7C7";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.81 Safari/537.36";
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


        #region 美食类
        public void meishi()
        {
            string[] areas = skinTextBox3.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            int page = 32;
            string item = label8.Text;
            string city = label6.Text;
            if (city == "")
            {
                MessageBox.Show("请选择城市！");
                return;
            }

            foreach (string area in areas)

            {

                for (int i = 1; i <= page; i++)
                {
                    String Url = "http://" + city + ".meituan.com/"+item+"b"+area+"/pn" + i + "/";
                    string html = GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()


                    MatchCollection TitleMatchs = Regex.Matches(html, @"{""poiId"":([\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    ArrayList lists = new ArrayList();
                    foreach (Match NextMatch in TitleMatchs)
                    {

                        lists.Add("http://www.meituan.com/meishi/" + NextMatch.Groups[1].Value + "/");



                    }

                    if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;

                   

                    

                    Application.DoEvents();
                    Thread.Sleep(1001);



                    foreach (string list in lists)
                    {

                       


                        int index = this.skinDataGridView1.Rows.Add();

                        String Url1 = list;
                        string strhtml = GetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()

                        string Rxg = @"{""poiId"":([\s\S]*?),""name"":""([\s\S]*?)"",""avgScore"":([\s\S]*?),""address"":""([\s\S]*?)"",""phone"":""([\s\S]*?)"",";


                      

                        Match all = Regex.Match(strhtml, Rxg);
                       
                        this.skinDataGridView1.Rows[index].Cells[0].Value = all.Groups[2].Value;
                            this.skinDataGridView1.Rows[index].Cells[1].Value = all.Groups[5].Value;
                            this.skinDataGridView1.Rows[index].Cells[2].Value = all.Groups[4].Value;
                            this.skinDataGridView1.Rows[index].Cells[3].Value = label14.Text;

                            this.skinDataGridView1.CurrentCell = this.skinDataGridView1.Rows[index].Cells[0];  //让datagridview滚动到当前行


                        
                        string tm1 = DateTime.Now.ToString();  //获取系统时间
                        textBox3.Text += tm1 + "-->正在采集" + Url + "\r\n";

                        while (this.status == false)
                        {
                            Application.DoEvents();
                        }

                        if (visualButton2.Text == "已停止")  //停止事件触发
                        {

                            return;


                        }
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(1000);

                    }
                }

                
            }
        }

        #endregion


        #region 其他分类
        public void otherItems()
        {
            string[] areas = skinTextBox3.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            string city = label6.Text;
            string item = label8.Text;
            if (city == "")
            {
                MessageBox.Show("请选择城市！");
                return;
            }

            int page =33;

            foreach (string area in areas)

            {
                for (int i = 0; i <page; i++)

                {
                   
                String Url = "http://" + city + ".meituan.com/"+item+"b" + area + "/pn" + i + "/";
                    
                string html = GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()

                string Rxg = @"<a class=""abstract-pic grey"" href=""([\s\S]*?)""";

                

                 MatchCollection match = Regex.Matches(html, Rxg);

                 ArrayList lists = new ArrayList();

                    foreach (Match NextMatch in match)
                    {

                        lists.Add( "http:" + NextMatch.Groups[1].Value);
                    


                    }
                

                    if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;

                    Application.DoEvents();
                     Thread.Sleep(1001);


                    


                    foreach (string list in lists)
                    {

                        int index = this.skinDataGridView1.Rows.Add();

                        String Url1 = list;
                        string strhtml = GetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()

                        string Rxg1 = @"<h1 class=""seller-name"">([\s\S]*?)</h1>";
                        string Rxg2 = @"""address"":""([\s\S]*?)""";
                        string Rxg3 = @"""phone"":""([\s\S]*?)""";



                        Match name = Regex.Match(strhtml, Rxg1);
                        Match address = Regex.Match(strhtml, Rxg2);
                        Match tell = Regex.Match(strhtml, Rxg3);

                        this.skinDataGridView1.Rows[index].Cells[0].Value = name.Groups[1].Value;

                        this.skinDataGridView1.CurrentCell = this.skinDataGridView1.Rows[index].Cells[0];  //让datagridview滚动到当前行


                        this.skinDataGridView1.Rows[index].Cells[1].Value = tell.Groups[1].Value;


                        this.skinDataGridView1.Rows[index].Cells[2].Value = address.Groups[1].Value;

                        this.skinDataGridView1.Rows[index].Cells[3].Value = label14.Text;




                        string tm1 = DateTime.Now.ToString();  //获取系统时间
                        textBox3.Text += tm1 + "-->正在采集" + label14.Text + "" + item + "第" + i + "页" + "\r\n";

                        if (visualButton2.Text == "已停止")  //停止事件触发
                        {

                            return;


                        }

                        Application.DoEvents();
                        System.Threading.Thread.Sleep(1000);

                    }
                }
               
            }

        }

        #endregion


        #region 电脑端搜索采集
        public void search()
        {
            string[] areas = skinTextBox3.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            int page = 32;
            
            string city = label14.Text;
            if (city == "")
            {
                MessageBox.Show("请选择城市！");
                return;
            }
            string keyword = skinTextBox1.Text;

            string citycode = GetCityCode(city);

            foreach (string area in areas)

            {

                for (int i = 0; i <= page; i=i+32)
                {
                    String Url = "http://apimobile.meituan.com/group/v4/poi/pcsearch/"+citycode+"?uuid=f41ed1d1-736c-423d-a39f-7517d13203a2&userid=875973616&limit=32&offset="+i+"&cateId=-1&q="+keyword+ "&areaId="+area;
                    string html = GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()


                    MatchCollection TitleMatchs = Regex.Matches(html, @"""},{""id"":([\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    ArrayList lists = new ArrayList();
                    foreach (Match NextMatch in TitleMatchs)
                    {

                        lists.Add("http://www.meituan.com/meishi/" + NextMatch.Groups[1].Value + "/");



                    }

                    if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;





                    Application.DoEvents();
                    Thread.Sleep(1001);



                    foreach (string list in lists)
                    {


                        int index = this.skinDataGridView1.Rows.Add();

                        String Url1 = list;
                        string strhtml = GetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()
                                                        // skinTextBox3.Text = strhtml;
                                                        //return;

                        string Rxg = @"{""poiId"":([\s\S]*?),""name"":""([\s\S]*?)"",""avgScore"":([\s\S]*?),""address"":""([\s\S]*?)"",""phone"":""([\s\S]*?)"",";



                        MatchCollection all = Regex.Matches(strhtml, Rxg);




                        foreach (Match NextMatch in all)
                        {



                            this.skinDataGridView1.Rows[index].Cells[0].Value = NextMatch.Groups[2].Value;
                            this.skinDataGridView1.Rows[index].Cells[1].Value = NextMatch.Groups[5].Value;
                            this.skinDataGridView1.Rows[index].Cells[2].Value = NextMatch.Groups[4].Value;
                            this.skinDataGridView1.Rows[index].Cells[3].Value = label14.Text;

                            this.skinDataGridView1.CurrentCell = this.skinDataGridView1.Rows[index].Cells[0];  //让datagridview滚动到当前行


                        }
                        string tm1 = DateTime.Now.ToString();  //获取系统时间
                        textBox3.Text += tm1 + "-->正在采集" + city + "" + keyword + "第" + i + "页" + "\r\n";



                        if (visualButton2.Text == "已停止")  //停止事件触发
                        {

                            return;


                        }
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(1000);

                    }
                }


            }
        }

        #endregion


        #region  获取数据库中城市名称对应的拼音

        public string Getpinyin(string city)
        {

            try
            {



                string constr = "Host =116.62.62.62;Database=citys;Username=root;Password=zhoukaige";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select meituan_city_pinyin from meituan_city where meituan_city_name='" + city + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


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

        #region  获取数据库中City名称对应的ID

        public string GetCityCode(string city)
        {

            try
            {



                string constr = "Host =116.62.62.62;Database=citys;Username=root;Password=zhoukaige";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select meituan_cityid from meituan_pc_city where meituan_cityname='" + city + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                reader.Read();

                string cityid = reader["meituan_cityid"].ToString().Trim();
                mycon.Close();
                reader.Close();
                return cityid;


            }

            catch (System.Exception ex)
            {
                return ex.ToString();
            }


        }

        #endregion



        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void skinTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            skinTextBox3.Text ="";
            label6.Text = e.Node.Name;
            label14.Text = e.Node.Text;


            string cityPinYin = Getpinyin(label14.Text.Trim());
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
                    skinTextBox3.Text += dr[0].ToString().Trim() + "\r\n";
                }
            }
            catch (MySqlException ee)
            {
                MessageBox.Show(ee.Message.ToString());
            }
            return;


        }

        

       

        

        private void skinTreeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            

            label8.Text = e.Node.Name;
            
        }

      

    

        private void 登陆ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login lg = new Login();
            lg.Show();
        }

        private void meituan_MouseEnter(object sender, EventArgs e)
        {
            label18.Text = Method.User;
        }

      

        private void label11_Click(object sender, EventArgs e)
        {
            

            Method.ReadSqlite(skinDataGridView1);



        }

       

       
        private void label12_Click(object sender, EventArgs e)
        {
          
        }


        #region 右击鼠标事件
        public void clear(object sender, EventArgs e)  //清空表格
        {

            skinDataGridView1.Rows.Clear();

        }

        #region  去除固话

        public  void RemoveTell(object sender, EventArgs e)

        {
            
            try
            {
                for (int i = 0; i <= skinDataGridView1.Rows.Count; i++)
                {
                    string Lpv = skinDataGridView1.Rows[i].Cells[1].Value.ToString();

                    if (Lpv.Contains("-") && !Lpv.Contains("/"))                //Contains()包含, indexof()返回字符在字符串中首次出现的位置，若没有返回 -1
                    {
                        skinDataGridView1.Rows.RemoveAt(i);
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
        #region  表格右击事件
        private void skinDataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu menu = new ContextMenu();   //初始化menu

                menu.MenuItems.Add("去除固话");
                menu.MenuItems.Add("清空数据");   //添加菜单项c1
                


                menu.Show(skinDataGridView1, new Point(e.X, e.Y));   //在点(e.X, e.Y)处显示menu

                menu.MenuItems[0].Click += new EventHandler(RemoveTell);  //菜单第一项对应事件
                menu.MenuItems[1].Click += new EventHandler(clear); 
                

            }
        }
        #endregion

        private void label9_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("IEXPLORE.EXE", "http://wpa.qq.com/msgrd?v=3&uin=852266010&site=qq&menu=yes");
            System.Diagnostics.Process.Start("IEXPLORE.EXE", "http://www.acaiji.com");
        }

        private void skinDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void visualButton2_Click(object sender, EventArgs e)
        {
            visualButton2.Text = "已停止";

        }

        private void visualButton1_Click(object sender, EventArgs e)
        {
            visualButton2.Text = "停止采集";

            if (label18.Text == "测试用户" || label18.Text == "")
            {
                MessageBox.Show("请注册账号登陆！");
                return;
            }

            if (tabControl1.SelectedIndex == 0) 
            {

                if (label8.Text.Contains("meishi"))
                {
                    Thread search_thread = new Thread(new ThreadStart(meishi));
                    Control.CheckForIllegalCrossThreadCalls = false;
                    search_thread.Start();
                }

                else if (label8.Text != "请选择分类")
                {
                    Thread thread = new Thread(new ThreadStart(otherItems));
                    Control.CheckForIllegalCrossThreadCalls = false;
                    thread.Start();

                }

            }

            else if (tabControl1.SelectedIndex == 1) //简单地级市采集
            {
                Thread search_thread = new Thread(new ThreadStart(search));
                Control.CheckForIllegalCrossThreadCalls = false;
                search_thread.Start();

            }

            else
            {

                MessageBox.Show("请选择分类！");

            }



        }

        private void visualButton4_Click(object sender, EventArgs e)
        {
            Method.Txt(skinDataGridView1);
        }

        private void visualButton3_Click(object sender, EventArgs e)
        {
            Method.DgvToTable(this.skinDataGridView1);
            Method.DataTableToExcel(Method.DgvToTable(this.skinDataGridView1), "Sheet1", true);
            Method.importtodatabase(skinDataGridView1);   //采集数据导入数据库。
        }

        private void visualButton5_Click(object sender, EventArgs e)
        {
            this.status = false;
        }

        private void visualButton6_Click(object sender, EventArgs e)
        {
            this.status = true;
        }
    }
}
