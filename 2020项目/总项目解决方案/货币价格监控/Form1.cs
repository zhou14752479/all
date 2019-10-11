using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 货币价格监控
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;
        SoundPlayer player = new SoundPlayer();
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string COOKIE = "__cfduid=de89865284ab56987edf1e421357ee37c1570753767; _ga=GA1.3.2038574935.1570754133; _gid=GA1.3.1167036501.1570754133; __zlcmid=uiibOqXQFuWQgm";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.108 Safari/537.36";
                request.Referer = "https://www.huobi.br.com";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

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
        #region 主程序
        public void run()
        {
            try

            {
                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
                {
                    MessageBox.Show("请输入差值");
                    return;
                }

         
                string okHtml = method.GetUrl("https://www.okex.com/v3/c2c/tradingOrders/book?t=1570702053211&side=sell&baseCurrency=btc&quoteCurrency=cny&userType=certified&paymentMethod=all", "utf-8");
                string zbHtml = method.GetUrl("https://trans.zb.com/api/web/market/V1_0_0/getDishData?callback=jQuery34103271134449147446_1570701854280&market=btc_qc&depth=&length=5&_=1570701854281", "utf-8");
                string huobiHtml = GetUrl("https://otc-api.huobi.br.com/v1/data/trade-market?coinId=1&currency=1&tradeType=sell&currPage=1&payMethod=0&country=37&blockType=general&online=1&range=0&amount=", "utf-8");

                
                Match okPrice = Regex.Match(okHtml, @"""price"":""([\s\S]*?)""");
                Match zbPrice = Regex.Match(zbHtml, @"""currentPrice"":""([\s\S]*?)""");
                Match huobiPrice = Regex.Match(huobiHtml, @"""price"":([\s\S]*?),");

                

                if (okPrice.Groups[1].Value != "" && zbPrice.Groups[1].Value != "" && huobiPrice.Groups[1].Value != "")
                {
                    label6.Text = okPrice.Groups[1].Value;
                    label9.Text = zbPrice.Groups[1].Value;
                    label12.Text = huobiPrice.Groups[1].Value;


                    decimal ok = Convert.ToDecimal(okPrice.Groups[1].Value);
                    decimal zb = Convert.ToDecimal(zbPrice.Groups[1].Value);
                    decimal huobi = Convert.ToDecimal(huobiPrice.Groups[1].Value);

                    if ((ok - zb) > Convert.ToInt32(textBox1.Text))
                    {
                        textBox5.Text += DateTime.Now.ToString()+ " : okex的价格为" + okPrice.Groups[1].Value + "....zb的价格为" + zbPrice.Groups[1].Value + "...okex高于zb" + (ok-zb)+"\r\n";
                        player.SoundLocation = path + "bj.wav";
                        player.Load();
                        player.Play();
                    }

                    if ((zb - ok) > Convert.ToInt32(textBox2.Text))
                    {
                        textBox5.Text += DateTime.Now.ToString() + " : okex的价格为" + okPrice.Groups[1].Value + "....zb的价格为" + zbPrice.Groups[1].Value + "...zb高于okex" + (zb - ok)+"\r\n";
                        player.SoundLocation = path + "bj.wav";
                        player.Load();
                        player.Play();
                    }

                    if ((zb - huobi) > Convert.ToInt32(textBox3.Text))
                    {
                        textBox5.Text += DateTime.Now.ToString() + " : zb的价格为" + zbPrice.Groups[1].Value + "....火币的价格为" + huobiPrice.Groups[1].Value + "...zb高于火币" + (zb - huobi)+"\r\n";
                        player.SoundLocation = path + "bj.wav";
                        player.Load();
                        player.Play();
                    }

                    if ((ok - huobi) > Convert.ToInt32(textBox4.Text))
                    {
                        textBox5.Text += DateTime.Now.ToString() + " : ok的价格为" + okPrice.Groups[1].Value + "....火币的价格为" + huobiPrice.Groups[1].Value + "...ok高于火币" + (ok - huobi) + "\r\n";
                        player.SoundLocation = path + "bj.wav";
                        player.Load();
                        player.Play();
                    }

                }


          



            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
           
            timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        
        private void Button2_Click(object sender, EventArgs e)
        {
           
            timer1.Stop();
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           
            textBox5.Text = "";
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            player.Stop();
        }
    }
}
