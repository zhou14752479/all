using System;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
              
                //设置支持的ssl协议版本，这里我们都勾选上常用的几个
                //ServicePointManager.SecurityProtocol =  SecurityProtocolType.Ssl3;
               

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
               
                string COOKIE = "zlan=cn; zloginStatus=; zJSESSIONID=6FE4FD3AF4111330B5DAF4564C1C0DDC";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.108 Safari/537.36";
                request.Referer = "";
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
                MessageBox.Show(ex.ToString());

            }
            return "";
        }
        #endregion

        bool status = false;

        #region 主程序
        public void run()
        {
            status = true;
            try

            {
                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
                {
                    MessageBox.Show("请输入差值");
                    return;
                }

         
                string okHtml = GetUrl("https://www.okex.com/v3/c2c/tradingOrders/book?t=1570702053211&side=sell&baseCurrency=btc&quoteCurrency=cny&userType=certified&paymentMethod=all", "utf-8");
                string zbHtml = GetUrl("https://trans.zb.live/api/web/market/V1_0_0/getGroupTicker?market=btcqc&callback=jsonp16", "utf-8");
                string huobiHtml = GetUrl("https://otc-api-hk.eiijo.cn/v1/data/trade-market?coinId=1&currency=1&tradeType=sell&currPage=1&payMethod=0&country=37&blockType=general&online=1&range=0&amount=", "utf-8");

                
                Match okPrice = Regex.Match(okHtml, @"""price"":""([\s\S]*?)""");
                Match zbPrice = Regex.Match(zbHtml, @"""btc_qc""([\s\S]*?)""([\s\S]*?)""");
                Match huobiPrice = Regex.Match(huobiHtml, @"""price"":([\s\S]*?),");

                string zbp = zbPrice.Groups[2].Value;



                if (okPrice.Groups[1].Value != "" && zbp != "" && huobiPrice.Groups[1].Value != "")
                {
                    label6.Text = okPrice.Groups[1].Value;
                    label9.Text = zbp;
                    label12.Text = huobiPrice.Groups[1].Value;


                    decimal ok = Convert.ToDecimal(okPrice.Groups[1].Value);
                    decimal zb = Convert.ToDecimal(zbp);
                    decimal huobi = Convert.ToDecimal(huobiPrice.Groups[1].Value);

                    if ((ok - zb) > Convert.ToInt32(textBox1.Text))
                    {
                        textBox5.Text += DateTime.Now.ToString()+ " : okex的价格为" + okPrice.Groups[1].Value + "....zb的价格为" + zbp + "...okex高于zb" + (ok-zb)+"\r\n";
                        player.SoundLocation = path + "bj.wav";
                        player.Load();
                        player.Play();
                    }

                    if ((zb - ok) > Convert.ToInt32(textBox2.Text))
                    {
                        textBox5.Text += DateTime.Now.ToString() + " : okex的价格为" + okPrice.Groups[1].Value + "....zb的价格为" + zbp + "...zb高于okex" + (zb - ok)+"\r\n";
                        player.SoundLocation = path + "bj.wav";
                        player.Load();
                        player.Play();
                    }

                    if ((zb - huobi) > Convert.ToInt32(textBox3.Text))
                    {
                        textBox5.Text += DateTime.Now.ToString() + " : zb的价格为" + zbp + "....火币的价格为" + huobiPrice.Groups[1].Value + "...zb高于火币" + (zb - huobi)+"\r\n";
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

            status = false;
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
            if (status == false)
            {
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
        
        private void Button2_Click(object sender, EventArgs e)
        {
            //string zbHtml = GetUrl("https://trans.zb.live/api/web/market/V1_0_0/getGroupTicker?market=btcqc&callback=jsonp16", "utf-8");

            //textBox5.Text = zbHtml;


            status = false;
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
                //System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }
    }
}
