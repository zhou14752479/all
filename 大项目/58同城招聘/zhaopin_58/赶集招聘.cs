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
    public partial class 赶集招聘 : Form
    {
        public 赶集招聘()
        {
            InitializeComponent();
        }

        #region 获取数据库赶集城市名称
        
        public static void ganjiCityName(ComboBox cob)
        {
            ArrayList list = new ArrayList();
            try
            {
                string constr = "Host =116.62.62.62;Database=citys;Username=root;Password=zhoukaige";
                string str = "SELECT cityName from ganji_city ";
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
            cob.DataSource = list;

        }
        #endregion


        #region  赶集获取数据库中城市名称对应的拼音

        public static string ganjicitypinyin(string city)
        {

            try
            {



                string constr = "Host =116.62.62.62;Database=citys;Username=root;Password=zhoukaige";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select citycode from ganji_city where cityName='" + city + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                reader.Read();

                string citypinyin = reader["citycode"].ToString().Trim();
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


        #region 获取数据库赶集招聘分类名称

        public static void ganjizpName(ComboBox cob)
        {
            ArrayList list = new ArrayList();
            try
            {
                string constr = "Host =116.62.62.62;Database=citys;Username=root;Password=zhoukaige";
                string str = "SELECT name from ganji_zhaopin ";
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
            cob.DataSource = list;

        }
        #endregion

        #region  赶集获取数据库中招聘分类名称对应的拼音

        public static string ganjizppinyin(string item)
        {

            try
            {



                string constr = "Host =116.62.62.62;Database=citys;Username=root;Password=zhoukaige";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select pinyin from ganji_zhaopin where name='" + item + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


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

        bool zanting = true;

        #region  获取

        public void zhaopin1()
        {

          
          try
            {

                string[] citys = textBox1.Text.Trim().Split(','); ;
                string[] keywords = textBox2.Text.Trim().Split(','); ;

                foreach (string city1 in citys)
                {
                    string city = ganjicitypinyin(city1);

                    foreach (string keyword1 in keywords)
                    {
                        string keyword = ganjizppinyin(keyword1);
                        if (keyword == "")
                        {
                            MessageBox.Show("请输入采集行业或者关键词！");
                            return;
                        }

                        for (int i = 1; i < 71; i++)
                        {

                            string Url = "http://"+city+".ganji.com/" + keyword + "/o" + i + "/";
                            string html = method.GetUrl(Url);

                            MatchCollection TitleMatchs = Regex.Matches(html, @"post_id=([\s\S]*?)puid=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                            ArrayList lists = new ArrayList();

                            foreach (Match NextMatch in TitleMatchs)
                            {

                                lists.Add(NextMatch.Groups[2].Value);
                            }
                            if (lists.Count == 0)

                                break;


                            foreach (string list in lists)

                            {
                                string URL = "https://3g.ganji.com/sh_" + keyword + "/" + list + "x";
                                string strhtml = method.GetUrl(URL); ;  //定义的GetRul方法 返回 reader.ReadToEnd()


                                string rxg = @"class=""title"">([\s\S]*?)</h1>";
                                string rxg1 = @"content=""【([\s\S]*?)】";    //公司                              
                                string rxg2 = @"业</th><td>([\s\S]*?)</a>";
                                string rxg3 = @"地点</th><td>([\s\S]*?)</td>";
                                string rxg4 = @"联系人</th><td>([\s\S]*?)</td>";
                                string rxg5 = @"&phone=([\s\S]*?)&";




                                Match name = Regex.Match(strhtml, rxg);
                                Match company = Regex.Match(strhtml, rxg1);
                                Match hangye = Regex.Match(strhtml, rxg2);
                                Match addr = Regex.Match(strhtml, rxg3);
                                Match lxr = Regex.Match(strhtml, rxg4);
                                Match tel = Regex.Match(strhtml, rxg5);

                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                                lv1.SubItems.Add(name.Groups[1].Value.Trim());
                                lv1.SubItems.Add(company.Groups[1].Value.Trim());
                                lv1.SubItems.Add(Regex.Replace(hangye.Groups[1].Value, "<a.*>", ""));
                                lv1.SubItems.Add(addr.Groups[1].Value.Trim());


                                lv1.SubItems.Add(lxr.Groups[1].Value.Trim());
                                lv1.SubItems.Add(tel.Groups[1].Value.Trim());
                                lv1.SubItems.Add(city1);


                                if (listView1.Items.Count - 1 > 1)
                                {
                                    listView1.EnsureVisible(listView1.Items.Count - 1);
                                }

                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }


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

        private void 赶集招聘_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;

            ganjiCityName(comboBox1);
             ganjizpName(comboBox2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(zhaopin1));
            thread.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text += comboBox1.Text + ",";

        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text += comboBox2.Text + ",";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.zanting = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.zanting = true;
        }
    }
}
