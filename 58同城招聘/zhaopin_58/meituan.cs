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
    public partial class meituan : Form
    {
        public meituan()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false; // 设置线程之间可以操作
                                                             
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle; //下一句用来禁止对窗口大小进行拖拽
        }

        private void visualButton4_Click(object sender, EventArgs e)
        {

        }

        #region 获取数据库美团城市名称
        public void getCityName()
        {
            ArrayList list = new ArrayList();
            try
            {
                string constr = "Host =116.62.62.62;Database=citys;Username=root;Password=zhoukaige";
                string str = "SELECT meituan_city_name from meituan_city ";
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
            visualComboBox1.DataSource = list;

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
        public ArrayList getAreaId()
        {
            ArrayList areas = new ArrayList();
            string cityPinYin = Getpinyin(visualComboBox1.SelectedItem.ToString());
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

        private void meituan_Load(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(getCityName));
            thread.Start();
            
        }
        bool zanting = true;

        #region  主程序

        public void run()
        {

            try
            {

                string[] keywords = visualTextBox1.Text.Trim().Split(',');

                if (visualComboBox1.Text == "")
                {
                    MessageBox.Show("请选择城市！");
                    return;
                }
                string cityId = GetCityId(visualComboBox1.Text);
                ArrayList areaIds = getAreaId();

                foreach (string keyword in keywords)

                {

                    if (keyword == "")
                    {
                        MessageBox.Show("请输入采集行业或者关键词！");
                        return;
                    }

                    foreach (string area in areaIds)
                    {

                        string Url = "https://apimobile.meituan.com/group/v4/poi/pcsearch/" + cityId + "?limit=10&q=" + keyword + "&areaId="+ area + "&uuid=C693C857695CAE55399A30C25D9D05F8914E58638F1E750BFB40CACC3AD5AE9F";
                        string html = method.GetUrl(Url);

                        MatchCollection TitleMatchs = Regex.Matches(html, @"false},{""id"":([\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        ArrayList lists = new ArrayList();

                        foreach (Match NextMatch in TitleMatchs)
                        {

                            if (!lists.Contains(NextMatch.Groups[0].Value))
                            {
                                lists.Add("https://apimobile.meituan.com/group/v1/poi/" + NextMatch.Groups[1].Value);
                            }


                        }


                        if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;


                        foreach (string list in lists)

                        {
                            string strhtml = method.GetUrl(list);  //定义的GetRul方法 返回 reader.ReadToEnd()                             
                            string rxg1 = @"name"":""([\s\S]*?)""";    //公司                            
                            string rxg2 = @"addr"":""([\s\S]*?)""";
                            string rxg3 = @"phone"":""([\s\S]*?)""";
                            string rxg4 = @"areaName"":""([\s\S]*?)""";


                            Match name = Regex.Match(strhtml, rxg1);
                            Match addr = Regex.Match(strhtml, rxg2);
                            Match tel = Regex.Match(strhtml, rxg3);
                            Match areaName = Regex.Match(strhtml, rxg4);
                            if (name.Groups[1].Value != "")
                            {
                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                                lv1.SubItems.Add(name.Groups[1].Value.Trim());
                                lv1.SubItems.Add(addr.Groups[1].Value.Trim());
                                lv1.SubItems.Add(tel.Groups[1].Value.Trim());
                                lv1.SubItems.Add(areaName.Groups[1].Value.Trim());
                                lv1.SubItems.Add(list);
                                //string[] values = { title.Groups[1].Value.Trim(), company.Groups[1].Value.Trim(), lxr.Groups[1].Value.Trim(), tell.Groups[1].Value.Trim(), area.Groups[1].Value.Trim(), addr.Groups[1].Value.Trim(), DateTime.Now.ToString(), };
                                //insertData(values);

                                if (listView1.Items.Count - 1 > 1)
                                {
                                    listView1.EnsureVisible(listView1.Items.Count - 1);
                                }

                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }

                                
                                Application.DoEvents();
                                System.Threading.Thread.Sleep(2000/(visualTrackBar1.Value+1));   //内容获取间隔，可变量
                                MessageBox.Show((2000/(visualTrackBar1.Value+1)).ToString());
                            }


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

        private void visualButton1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
        }

        private void visualButton2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void 清空数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
