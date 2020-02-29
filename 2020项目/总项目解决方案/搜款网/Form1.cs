using MySql.Data.MySqlClient;
using System;
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

namespace 搜款网
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool zanting = true;
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url)
        {


            try
            {
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "1s1k453=ysyk_web62; JSESSIONID=C38B6D4A827F086C722EA8DAC0E55D26; UM_distinctid=1704d60505528a-0f30e2260ef312-2393f61-1fa400-1704d6050576ae; CNZZDATA1253333710=885277478-1581844031-%7C1581995363; CNZZDATA1253416210=1453523664-1581844817-%7C1581992732";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
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

       
        public string insert(string sql)
        {
            try
            {


                string constr = "Host ="+textBox7.Text+ ";Database=" + textBox8.Text+"; Username=" + textBox6.Text + ";Password=" + textBox5.Text;
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

             
                MySqlCommand cmd = new MySqlCommand(sql, mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {
                    mycon.Close();
                    return ("插入成功！");

                    

                }
                else
                {
                    mycon.Close();
                    return("连接失败！");
                }


            }

            catch (System.Exception ex)
            {
                return(ex.ToString());
            }
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
      
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {

            try
            {
                string[] array = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] != "")
                    {
                        label4.Text = ""; label12.Text = ""; label13.Text = "";

                        string html = GetUrl(array[i]);


                        Match title = Regex.Match(html, @"<title>([\s\S]*?)-");                             //标题
                        Match costprice = Regex.Match(html, @"<strong class=""d-sale"">([\s\S]*?)</strong>");  //批发价
                        Match huohao = Regex.Match(html, @"<div class=""value ff-arial"">([\s\S]*?)</div>");  //货号
                        Match addr = Regex.Match(html, @"地址：</div>([\s\S]*?)<a");

                        Match xq2 = Regex.Match(html, @"id=""descTemplate"">([\s\S]*?)</script>");  //详情带图片


                      
                        string price = (Convert.ToDecimal(textBox2.Text) * Convert.ToDecimal(costprice.Groups[1].Value.Trim())).ToString();
                        string card_price = (Convert.ToDecimal(textBox3.Text) * Convert.ToDecimal(costprice.Groups[1].Value.Trim())).ToString();
                        string productprice = (Convert.ToDecimal(textBox4.Text) * Convert.ToDecimal(costprice.Groups[1].Value.Trim())).ToString();
                        string supply_id = textBox9.Text.Trim();
                        string sales = "0";
                        string total = "9999";
                        string time = GetTimeStamp();
                        string video = "";
                        string bianma = Regex.Replace(addr.Groups[1].Value, "<[^>]+>", "").Trim() + "+" + huohao.Groups[1].Value.Trim();




                        MatchCollection colors = Regex.Matches(html, @"""color_name"":""([\s\S]*?)""");
                        MatchCollection chimas = Regex.Matches(html, @"<li title=""尺码:([\s\S]*?)""");


                        MatchCollection zhupics = Regex.Matches(html, @"mid=""([\s\S]*?)""");                           //主图 缺少http:
                        MatchCollection xqpics = Regex.Matches(html, @"<img align=""absmiddle"" src=""([\s\S]*?)""");  //详情图

                        MessageBox.Show(chimas.Count.ToString());




                         string sql = "INSERT INTO oscshop_lionfish_comshop_goods (id,goodsname,grounding,price,costprice,card_price,productprice,sales,total,addtime,codes)" +
                            "VALUES('" + time + " ','" + title.Groups[1].Value + " ','1', '" + price + " ','" + costprice.Groups[1].Value.Trim() + " ','" + card_price + " ','" + productprice + " ','" + sales+ " ','" + total + " ','" + time + " ','" + bianma + " ')";
                       label4.Text= insert(sql);

                        string sql2 = "INSERT INTO oscshop_lionfish_comshop_good_common (goods_id,supply_id,share_title,big_img,goods_share_image,content,video)" +
                            "VALUES('" + time + " ','" + supply_id + " ', '" + title.Groups[1].Value + " ', '" + "http:"+zhupics[0].Groups[1].Value + " ','" + "http:" + zhupics[0].Groups[1].Value + " ','" + xq2.Groups[1].Value + " ','" + video + " ')";
                        label12.Text = insert(sql2);

                        for (int j = 0; j < colors.Count; j++)
                        {
                            string sql3 = "INSERT INTO oscshop_lionfish_comshop_goods_option_item (goods_id,title,displayorder)" +
                               "VALUES('" + time + " ','" + colors[j].Groups[1].Value + " ', '" + j + 1 + " ')";
                            insert(sql3);

                            for (int a = 0; a < chimas.Count; a++)
                            {
                              

                                string sql31 = "INSERT INTO oscshop_lionfish_comshop_goods_option_item_value (goods_id,productprice,marketprice,card_price,stock,costprice,title,goodssn)" +
                               "VALUES('" + time + " ','" + productprice + " ''" + price + " ','" + card_price + " ','" + total + " ','" + costprice+ " ','" + colors[j].Groups[1].Value+"+"+chimas[a].Groups[1].Value + " ', '" + bianma + " ')";
                                insert(sql31);
                            }
                            

                        }


                        foreach (Match img in zhupics)
                        {
                            string sql4 = "INSERT INTO oscshop_lionfish_comshop_goods_images (goods_id,image,thumb)" +
                            "VALUES('" + time + " ','" + img.Groups[1].Value + " ', '" + img.Groups[1].Value + " ')";
                            label13.Text = insert(sql4);
                        }
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                    }





                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string constr = "Host =47.99.68.92;Database=vip_database;Username=root;Password=zhoukaige00.@*.";
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();

            MySqlCommand cmd = new MySqlCommand("select * from vip where username='搜款网'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

            MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

            if (reader.Read())
            {

                string password = reader["password"].ToString().Trim();

                if (password != "搜款网")

                {
                    MessageBox.Show("验证失败");

                    Environment.Exit(0);
                }

                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            }
           
        }
    }
}
