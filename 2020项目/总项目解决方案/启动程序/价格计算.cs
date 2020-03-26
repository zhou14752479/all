using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 启动程序
{
    public partial class 价格计算 : Form
    {

       







        public 价格计算()
        {
            InitializeComponent();
        }
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
                // System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "Hm_lvt_fc2b90cbec55323dbc64f2b6400d86c7=1584760539; Hm_lpvt_fc2b90cbec55323dbc64f2b6400d86c7=1584761623";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://www.jzj9999.com/news/index.aspx";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);

                WebHeaderCollection headers = request.Headers;
                headers.Add("Sec-Fetch-Mode: cors");
                headers.Add("Sec-Fetch-Site: same-origin");
                headers.Add("X-Requested-With: XMLHttpRequest");

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
                MessageBox.Show(ex.ToString());

            }
            return "";
        }
        #endregion

        public static string time;




        public static Double hjPrice;
        public static Double byPrice;
        public static Double bojPrice;
        public static Double bajPrice;


        public static Double price1;
        public static Double price2;
        public static Double price3;
        public static Double price4;
        public static Double price5;
        public static Double price6;


        public static Double jieguo1;
        public static Double jieguo2;
        public static Double jieguo3;
        public static Double jieguo4;
        public static Double jieguo5;
        public static Double jieguo6;

        #region 获取网站价格
        public void getPrice()
        {
            string url = "https://www.jzj9999.com/pricedata/getdata.aspx?tmp=93861584761821632";
            string html = GetUrl(url);

            Match time = Regex.Match(html, @"pubtime"":""([\s\S]*?)""");
            Match a1 = Regex.Match(html, @"JZJ_au"",""askprice"":([\s\S]*?),");
            Match a2 = Regex.Match(html, @"JZJ_ag"",""askprice"":([\s\S]*?),");
            Match a3 = Regex.Match(html, @"JZJ_pt"",""askprice"":([\s\S]*?),");
            Match a4 = Regex.Match(html, @"JZJ_pd"",""askprice"":([\s\S]*?),");

            textBox1.Text = time.Groups[1].Value;
            textBox2.Text = a1.Groups[1].Value;
            textBox3.Text = a2.Groups[1].Value;
            textBox4.Text = a3.Groups[1].Value;
            textBox5.Text = a4.Groups[1].Value;

            hjPrice = Convert.ToDouble(textBox2.Text);
            
            byPrice = Convert.ToDouble(textBox3.Text);
            bojPrice = Convert.ToDouble(textBox4.Text);
            bajPrice = Convert.ToDouble(textBox5.Text);

            价格计算.time = time.Groups[1].Value;
        }
            #endregion


            private void 价格计算_Load(object sender, EventArgs e)
        {
           



            foreach (Control ctr in groupBox2.Controls)
            {

                if (ctr is TextBox)
                {

                    string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
                    if (File.Exists(path + ctr.Name + ".txt"))
                    {

                        StreamReader sr = new StreamReader(path + ctr.Name + ".txt", Encoding.GetEncoding("utf-8"));
                        //一次性读取完 
                        string texts = sr.ReadToEnd();
                        ctr.Text = texts;
                        sr.Close();
                    }
                }
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {

            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html");

            if (html.Contains(@"jiagejisuan"))
            {
                getPrice();
                timer1.Start();
                timer1.Interval = Convert.ToInt32(textBox6.Text) * 1000;


            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
          
        }

        private void 价格计算_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }


        public void jisuan()

        {
            





            if (comboBox1.Text.Trim() == "黄金") 
            {
                price1 = hjPrice;
            }
            else if (comboBox1.Text.Trim() == "白银")
            {
                price1 = byPrice;
            }
            else if (comboBox1.Text.Trim() == "铂金")
            {
                price1 = bojPrice;
            }

            else if (comboBox1.Text.Trim() == "钯金")
            {
                price1 = bajPrice;
            }


            if (comboBox2.Text.Trim() == "黄金")
            {
                price2 = hjPrice;
            }
            else if (comboBox2.Text.Trim() == "白银")
            {
                price2 = byPrice;
            }
            else if (comboBox2.Text.Trim() == "铂金")
            {
                price2 = bojPrice;
            }

            else if (comboBox2.Text.Trim() == "钯金")
            {
                price2 = bajPrice;
            }


            if (comboBox3.Text.Trim() == "黄金")
            {
                price3 = hjPrice;
            }
            else if (comboBox3.Text.Trim() == "白银")
            {
                price3 = byPrice;
            }
            else if (comboBox3.Text.Trim() == "铂金")
            {
                price3 = bojPrice;
            }

            else if (comboBox3.Text.Trim() == "钯金")
            {
                price3 = bajPrice;
            }



            if (comboBox4.Text.Trim() == "黄金")
            {
                price4 = hjPrice;
            }
            else if (comboBox4.Text.Trim() == "白银")
            {
                price4 = byPrice;
            }
            else if (comboBox4.Text.Trim() == "铂金")
            {
                price4 = bojPrice;
            }

            else if (comboBox4.Text.Trim() == "钯金")
            {
                price4 = bajPrice;
            }


            if (comboBox5.Text.Trim() == "黄金")
            {
                price5 = hjPrice;
            }
            else if (comboBox5.Text.Trim() == "白银")
            {
                price5 = byPrice;
            }
            else if (comboBox5.Text.Trim() == "铂金")
            {
                price5 = bojPrice;
            }

            else if (comboBox5.Text.Trim() == "钯金")
            {
                price5 = bajPrice;
            }





            if (comboBox6.Text.Trim() == "黄金")
            {
                price6 = hjPrice;
            }
            else if (comboBox6.Text.Trim() == "白银")
            {
                price6 = byPrice;
            }
            else if (comboBox6.Text.Trim() == "铂金")
            {
                price6 = bojPrice;
            }

            else if (comboBox6.Text.Trim() == "钯金")
            {
                price6 = bajPrice;
            }









            double jisuan1 = (price1 + Convert.ToDouble(textBox7.Text)) * Convert.ToDouble(textBox8.Text) + Convert.ToDouble(textBox9.Text);
            double jisuan2 = (price2 + Convert.ToDouble(textBox15.Text)) * Convert.ToDouble(textBox14.Text) + Convert.ToDouble(textBox13.Text);
            double jisuan3 = (price3 + Convert.ToDouble(textBox20.Text)) * Convert.ToDouble(textBox19.Text) + Convert.ToDouble(textBox18.Text);
            double jisuan4 = (price4 + Convert.ToDouble(textBox24.Text)) * Convert.ToDouble(textBox23.Text) + Convert.ToDouble(textBox22.Text);
            double jisuan5 = (price5 + Convert.ToDouble(textBox28.Text)) * Convert.ToDouble(textBox27.Text) + Convert.ToDouble(textBox26.Text);
            double jisuan6 = (price6 + Convert.ToDouble(textBox32.Text)) * Convert.ToDouble(textBox31.Text) + Convert.ToDouble(textBox30.Text);


            //判断人工输入框是否为空
            if (jisuan1> Convert.ToDouble(textBox10.Text))
            {
                jieguo1 = jisuan1;
            }
            else
            {
                jieguo1 = Convert.ToDouble(textBox10.Text);
            }


            if (jisuan2 > Convert.ToDouble(textBox11.Text))
            {
                jieguo2 = jisuan2;
            }
            else
            {
                jieguo2 = Convert.ToDouble(textBox11.Text);
            }


            if (jisuan3> Convert.ToDouble(textBox17.Text))
            {
                jieguo3 = jisuan3;
            }
            else
            {
                jieguo3 = Convert.ToDouble(textBox17.Text);
            }


            if (jisuan4 > Convert.ToDouble(textBox21.Text))
            {
                jieguo4 = jisuan4;
            }
            else
            {
                jieguo4 = Convert.ToDouble(textBox21.Text);
            }

            if (jisuan5 > Convert.ToDouble(textBox25.Text))
            {
                jieguo5 = jisuan5; 
            }
            else
            {
                jieguo5 = Convert.ToDouble(textBox25.Text);
            }


            if (jisuan6 > Convert.ToDouble(textBox29.Text))
            {
                jieguo6 = jisuan6;
            }
            else
            {
                jieguo6 = Convert.ToDouble(textBox29.Text);
            }




            //保留小数
            //保留小数
            //保留小数
            //if (comboBox7.Text == "0")
            //{
            //    jieguo1 = Math.Round(jieguo1, 0);
            //}
            //else if (comboBox7.Text == "1")
            //{
            //    jieguo1 = Math.Round(jieguo1, 1);
            //}
            //else if (comboBox7.Text == "2")
            //{
            //    jieguo1 = Math.Round(jieguo1, 2);
            //}









        }



        private void button2_Click(object sender, EventArgs e)
        {
            
            timer2.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            getPrice();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            jisuan();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (Control ctr in groupBox2.Controls)
            {
                if (ctr is TextBox)
                {


                    string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
                    FileStream fs1 = new FileStream(path + ctr.Name + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1);
                    sw.WriteLine(ctr.Text.Trim());
                    sw.Close();
                    fs1.Close();

                }
            }
            this.Hide();
        }
    }
}
