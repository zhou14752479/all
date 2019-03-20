using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zhaopin_58
{
    public partial class meituan : Form
    {
        public meituan()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false; // 设置线程之间可以操作
                                                             
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle; //下一句用来禁止对窗口大小进行拖拽
        }

     
        #region 获取数据库美团城市名称
        public void getCityName()
        {
            ArrayList list = new ArrayList();
            try
            {
                string constr = "Host =116.62.62.62;Database=citys;Username=root;Password=zhoukaige";
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

        #region 获取数据库美团城市名称返回集合
        public ArrayList getCityNames()
        {
            ArrayList list = new ArrayList();
            try
            {
                string constr = "Host =116.62.62.62;Database=citys;Username=root;Password=zhoukaige";
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
            return list;

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

                MySqlCommand cmd = new MySqlCommand("select pinyin from meituan_province_city where name='" + city + "'  ", mycon);         //SQL语句读取textbox的值'"+textBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                reader.Read();

                string citypinyin = reader["pinyin"].ToString().Trim();
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

        #region  获取数据库中城市名称对应的缩写

        public string Getsuoxie(string city)
        {

            try
            {
                string constr = "Host =116.62.62.62;Database=citys;Username=root;Password=zhoukaige";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select suoxie from meituan_province_city where name='" + city + "'  ", mycon);         //SQL语句读取textbox的值'"+textBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                reader.Read();

                string citypinyin = reader["suoxie"].ToString().Trim();
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

        #region  获取数据库中城市名称对应的Id

        public string GetCityId(string city)
        {

            try
            {
                string constr = "Host =116.62.62.62;Database=citys;Username=root;Password=zhoukaige";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select meituan_cityid from meituan_pc_city where meituan_cityname='" + city + "'  ", mycon);         //SQL语句读取textbox的值'"+textBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                reader.Read();

                string meituan_cityid = reader["meituan_cityid"].ToString().Trim();
                mycon.Close();
                reader.Close();
                return meituan_cityid;


            }

            catch (System.Exception ex)
            {
                return ex.ToString();
                
            }


        }

        #endregion

        #region 获取城市名对应的区域ID
        public ArrayList getAreaId(string city)
        {
            //visualComboBox1.SelectedItem.ToString()
            ArrayList areas = new ArrayList();
            string cityPinYin = Getpinyin(city);
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
                    areas.Add(dr[0].ToString().Trim());
                }
            }
            catch (MySqlException ee)
            {
                ee.Message.ToString();
            }
            return areas;
        }

        #endregion


        /// <summary>
        /// 插入数据库配置
        /// </summary>
        /// <param name="values"></param>
        public void insertData(string[] values)
        {

            try
            {
                string constr = "Host =116.62.62.62;Database=vip;Username=root;Password=zhoukaige";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO meituan_datas (meituan_name,meituan_addr,meituan_tel,area,city,cate)VALUES('" + values[0] + " ','" + values[1] + " ','" + values[2] + " ','" + values[3] + " ','" + values[4] + " ','" + values[5] + " ')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.

            }

            catch (System.Exception ex)
            {
                ex.ToString();
            }
        }

        private void meituan_Load(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(getCityName));
            thread.Start();

        }
        bool zanting = true;
        bool status = true;

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string URL)
        {
            try
            {

                string COOKIE = "abt=1499071937.0%7CADE; _lxsdk_cuid=15d07a688f6c8-0b50ac5c472b18-333f5902-100200-15d07a688f79c; oc=xrmkO2nLZoY_IrhbS451igIl7hYw7IdDK6N_eVitVxW6WMxwN8yqI07hUj0vwzV57Iemuglzh4HS0E77JWewbxs2LOrDkniIWKv_go8Z8i77EVeAUCUejMdsEHZtdIDxMvg4fR4p53MxVNd2YZr8ZNhk_yZNN_hIE2VChkJKOJI; __mta=54360727.1499071941097.1518360971460.1518584371801.7; iuuid=F457EFC99EEB24DD0A17795BB1F8A91129848721BFF3EE82C45EF7C15E7C210E; _lxsdk=F457EFC99EEB24DD0A17795BB1F8A91129848721BFF3EE82C45EF7C15E7C210E; webp=1; __utmz=74597006.1547084235.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); _hc.v=125767d7-22b6-b361-e802-854d70351873.1547084296; __utma=74597006.1002021895.1547084235.1547426995.1547429915.3; i_extend=C145095553688078665527034436504084189858_b2_e4339319119865529162_v1084616022904540227_a%e8%bf%90%e5%8a%a8%e5%81%a5%e8%ba%ab_f179411730E015954189128616476612677712756937264803_e7765034795530822391_v1084625786170336172_a%e8%bf%90%e5%8a%a8%e5%81%a5%e8%ba%abGimthomepagesearchH__a100005__b3; uuid=42434825e14e46769143.1552287125.1.0.0; cityname=%E4%B9%8C%E9%B2%81%E6%9C%A8%E9%BD%90; ci=10; rvct=10%2C387%2C105%2C73%2C60%2C1%2C184%2C875%2C55%2C40";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Timeout = 10000;
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/16B92 MicroMessenger/7.0.3(0x17000321) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
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


        #region  主程序按照单个城市多个关键词

        public void run()
        {

            try
            {
                
                string[] keywords = textBox1.Text.Trim().Split(',');

                string city = comboBox1.SelectedItem.ToString();
              

                    ArrayList areaIds = getAreaId(city);
                    string cityId = GetCityId(city);

                    foreach (string keyword in keywords)

                    {

                        foreach (string area in areaIds)
                        {

                            string Url = "https://apimobile.meituan.com/group/v4/poi/pcsearch/"+cityId+"?cateId=-1&sort=default&userid=-1&offset=0&limit=1000&mypos=33.959859%2C118.279675&uuid=C693C857695CAE55399A30C25D9D05F8914E58638F1E750BFB40CACC3AD5AE9F&pcentrance=6&q="+keyword+"&requestType=filter&cityId="+cityId+"&areaId="+area;
                            
                            string html = GetUrl(Url);
                        
                            
                            MatchCollection TitleMatchs = Regex.Matches(html, @"false},{""id"":([\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                            ArrayList lists = new ArrayList();

                            foreach (Match NextMatch in TitleMatchs)
                            {

                                lists.Add("https://apimobile.meituan.com/group/v1/poi/"+ NextMatch.Groups[1].Value + "?fields=areaName,name,addr,phone" );
                            }

                        
                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;
                            foreach (string list in lists)
                            {
                                string strhtml = GetUrl(list);  //定义的GetRul方法 返回 reader.ReadToEnd()                             

                            
                                Match name = Regex.Match(strhtml, @"poiid([\s\S]*?)""name"":""([\s\S]*?)""");
                                Match addr = Regex.Match(strhtml, @"addr"":""([\s\S]*?)""");
                                Match tel = Regex.Match(strhtml, @"phone"":""([\s\S]*?)""");
                                Match areaName = Regex.Match(strhtml, @"areaName"":""([\s\S]*?)""");                        
                                if (name.Groups[2].Value != "")
                                {
                                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                                    lv1.SubItems.Add(name.Groups[2].Value.Trim());
                                    lv1.SubItems.Add(addr.Groups[1].Value.Trim());
                                    lv1.SubItems.Add(tel.Groups[1].Value.Trim());
                                    lv1.SubItems.Add(areaName.Groups[1].Value.Trim());
                                    lv1.SubItems.Add(city);
                                   
                                    string[] values = { name.Groups[1].Value.Trim(), addr.Groups[1].Value.Trim(), tel.Groups[1].Value.Trim(), areaName.Groups[1].Value.Trim(), city,"" };
                                    insertData(values);

                                    if (listView1.Items.Count - 1 > 1)
                                    {
                                        listView1.EnsureVisible(listView1.Items.Count - 1);
                                    }
                                    while (this.zanting == false)
                                    {
                                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                    }

                                if (status == false)
                                {
                                    return;
                                }
                                    
                                   Thread.Sleep(1000);   //内容获取间隔，可变量

                                }
                                
                            }

                        }
                    }
                }
            




            catch (System.Exception ex)
            {
              MessageBox.Show(  ex.ToString());
            }
        }

        #endregion


        

      

      

        private void 清空数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

      
     

        private void visualComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.zanting = false;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.zanting = true;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            status = false;
        }
    }
}
