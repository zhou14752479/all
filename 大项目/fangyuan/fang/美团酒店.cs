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


        ArrayList finishes = new ArrayList();

        #region  获取数据库中城市名称对应的拼音

        public string Getpinyin(string id)
        {

            try
            {
                string constr = "Host =116.62.62.62;Database=citys;Username=root;Password=zhoukaige";
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
        #region  主函数
        public void run()

        {
            
            string[] ids = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            try

            {

                foreach (string id in ids)
                {
                    ArrayList areaIds = getAreaId(id);

                    string html = method.GetUrl("https://apimobile.meituan.com/group/v4/poi/pcsearch/" + id + "?cateId=-1&sort=defaults&userid=-1&offset=0&limit=1000&mypos=33.959478%2C118.27953&uuid=C693C857695CAE55399A30C25D9D05F8914E58638F1E750BFB40CACC3AD5AE9F&pcentrance=6&cityId=184&q=%E9%85%92%E5%BA%97", "utf-8");

                    MatchCollection matchs = Regex.Matches(html, @"false},{""id"":([\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    ArrayList lists = new ArrayList();

                    foreach (Match NextMatch in matchs)
                    {
                        lists.Add( NextMatch.Groups[1].Value);

                    }

                    if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;
                  

                    foreach (string list in lists)
                    {

                        if (!finishes.Contains(list))
                        {
                            finishes.Add(list);


                            string strhtml = method.GetUrl("https://hotel.meituan.com/" + list + "/", "utf-8");
                            string pricehtml = method.GetUrl("https://ihotel.meituan.com/productapi/v2/prepayList?type=1&poiId=" + list + "&start=1553529600000&end=1553616000000&uuid=EE452628FFED0D7E2E8057602BBF1FB40975D7AC4634E49AA9A0FDF0EEA3FC2F&_token=eJxVjdtugkAURf9l+iiRmWG4+Ua9olJFUVubPoyIgCAiDCA0/fcOrX1ocpK9srJ3zifIzCPoIQihjATAcs6yLMkaUSWZICwA979TW3fItgPQe0c6hgLB8KM1Ky5+DYIaV39MOGPCr22ZvAQCxtK8J4rBlXlx9+KFrKBJ171eRKTICpSQoopPYXK6AgEAvro47UrSNUFGGneSTjjJnLCmC5L2Qyp/oShtPWrrPOkj2SPz0E9AD3jTKq7P0axqDNt+GavFyNivWGnOYsdEfh3VFbaqdbrxrPHOyGLdL5fzplPWzF+l2YZO3b63KJ27OKhxPy7Ow63nNVL+dhOhF56uxE3HAUqOVIvndIzqgM5g7lsxDZQ0Su3XWXQzTtnwxa7yhfPsT1xcOD5FJrXO+mhCrJUxGuyWYb5RE3TaM3w5oHvj2k3H71TT9WB9R+DrG7Lcf0c=&X-FOR-WITH=pGos92QTElLqcCWAvP6YQkfNNuhR6YuxIjiNfCcTY3U03tYLKjNimVRh%2B5JxlGK%2B6GX2pHOlKHGvc6mget1zVjnc8rEdG0JQUXxy7Vh0ev%2F%2FDMqYPcCna7DZDqKnSi1QcccV1%2B%2BB9MY7rjlslJkF0g%3D%3D", "utf-8");
                            Match titles = Regex.Match(strhtml, @"<title>([\s\S]*?)_");
                            Match addr = Regex.Match(strhtml, @"""addr"":""([\s\S]*?)""");
                            Match zhuangxiu = Regex.Match(strhtml, @"装修时间"",""attrValue"":""([\s\S]*?)""");
                            Match fangjian = Regex.Match(strhtml, @"房间总数"",""attrValue"":""([\s\S]*?)""");
                            Match phone = Regex.Match(strhtml, @"""phone"":""([\s\S]*?)""");
                            Match area = Regex.Match(strhtml, @"""areaName"":""([\s\S]*?)""");
                            Match type = Regex.Match(strhtml, @"""hotelStar"":""([\s\S]*?)""");
                            Match city = Regex.Match(strhtml, @"""cityName"":""([\s\S]*?)""");
                            Match price= Regex.Match(pricehtml, @"averagePrice"":([\s\S]*?),");





                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(titles.Groups[1].Value);

                            lv1.SubItems.Add(addr.Groups[1].Value);
                            lv1.SubItems.Add(zhuangxiu.Groups[1].Value);
                            lv1.SubItems.Add(fangjian.Groups[1].Value);
                            lv1.SubItems.Add(phone.Groups[1].Value);
                            lv1.SubItems.Add(area.Groups[1].Value);
                            lv1.SubItems.Add(type.Groups[1].Value);
                            lv1.SubItems.Add(city.Groups[1].Value);
                            lv1.SubItems.Add(price.Groups[1].Value);





                            if (listView1.Items.Count - 1 > 1)
                            {
                                listView1.EnsureVisible(listView1.Items.Count - 1);
                            }
                            //如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。


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
            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread(new ThreadStart(run));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();
            }
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
