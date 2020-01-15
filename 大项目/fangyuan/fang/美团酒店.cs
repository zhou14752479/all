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

namespace fang
{
    public partial class 美团酒店 : Form
    {
        public 美团酒店()
        {
            InitializeComponent();
        }

        bool status = true;
        ArrayList finishes = new ArrayList();

        #region  获取数据库中城市名称对应的拼音

        public string Getpinyin(string id)
        {

            try
            {
                string constr = "Host =47.99.68.92;Database=citys;Username=root;Password=zhoukaige00.@*.";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select pinyin from meituan_province_city where uid='" + id + "'  ", mycon);         //SQL语句读取textbox的值'"+textBox1.Text+"'


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
     
        #region 获取城市名对应的区域ID
        public ArrayList getAreaId(string cityid)
        {
            //visualComboBox1.SelectedItem.ToString()
            ArrayList areas = new ArrayList();
            string cityPinYin = Getpinyin(cityid);
            try
            {
                string constr = "Host =47.99.68.92;Database=citys;Username=root;Password=zhoukaige00.@*.";
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
       
        #region  主函数
        public void run()

        {
            
            string[] cityids = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            try

            {

                foreach (string cityid in cityids)
                {
                    ArrayList areaIds = getAreaId(cityid);

                    foreach (string areaId in areaIds)
                    {
                        for (int i = 0; i < 1001; i=i+20)
                        {


                        string html = method.GetUrl("https://ihotel.meituan.com/hbsearch/HotelSearch?utm_medium=pc&version_name=999.9&cateId=20&attr_28=129&cityId="+cityid+"&areaId="+areaId+"&offset="+i+"&limit=20&startDay="+ DateTime.Now.ToString("yyyyMMdd") + "&endDay=" + DateTime.Now.ToString("yyyyMMdd") + "&q=&sort=defaults", "utf-8");

                        MatchCollection matchs = Regex.Matches(html, @"""realPoiId"":([\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        MatchCollection prices = Regex.Matches(html, @"""lowestPrice"":([\s\S]*?),");

                        ArrayList lists = new ArrayList();

                        foreach (Match NextMatch in matchs)
                        {
                            lists.Add(NextMatch.Groups[1].Value);

                        }

                        if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;


                        for (int j = 0; j < lists.Count; j++)
                        {

           


                                    string strhtml = method.GetUrl("https://hotel.meituan.com/" + lists[j] + "/", "utf-8");

                                    Match titles = Regex.Match(strhtml, @"<title>([\s\S]*?)_");
                                    Match addr = Regex.Match(strhtml, @"""addr"":""([\s\S]*?)""");
                                    Match zhuangxiu = Regex.Match(strhtml, @"装修时间"",""attrValue"":""([\s\S]*?)""");
                                    Match fangjian = Regex.Match(strhtml, @"客房总量"",""attrValue"":""([\s\S]*?)""");
                                    Match phone = Regex.Match(strhtml, @"""phone"":""([\s\S]*?)""");
                                    Match area = Regex.Match(strhtml, @"""areaName"":""([\s\S]*?)""");
                                    Match type = Regex.Match(strhtml, @"""hotelStar"":""([\s\S]*?)""");
                                    Match city = Regex.Match(strhtml, @"""cityName"":""([\s\S]*?)""");




                                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                    lv1.SubItems.Add(titles.Groups[1].Value);

                                    lv1.SubItems.Add(addr.Groups[1].Value);
                                    lv1.SubItems.Add(zhuangxiu.Groups[1].Value);
                                    lv1.SubItems.Add(fangjian.Groups[1].Value);
                                lv1.SubItems.Add(phone.Groups[1].Value.Replace("u002F"," "));
                                    lv1.SubItems.Add(area.Groups[1].Value);
                                    lv1.SubItems.Add(type.Groups[1].Value);
                                    lv1.SubItems.Add(city.Groups[1].Value);
                                    lv1.SubItems.Add(prices[j].Groups[1].Value);



                                    if (listView1.Items.Count - 1 > 1)
                                    {
                                        listView1.EnsureVisible(listView1.Items.Count - 1);
                                    }
                                    if (this.status == false)

                                    {
                                        return;
                                    }
                                
                                
                            }
                        }

                    }
                }
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        private void skinButton2_Click(object sender, EventArgs e)
        {
            skinButton2.Enabled = false;
             status = true;
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();




        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void skinButton7_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void skinButton6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 美团酒店_Load(object sender, EventArgs e)
        {

        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            skinButton2.Enabled = true;
        }
    }
}
