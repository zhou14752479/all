using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 货币价格监控
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        string path = AppDomain.CurrentDomain.BaseDirectory + "bj.wav";

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


        #region 主程序
        public void run()
        {
          
            try

            {
                if (textBox1.Text == "" || textBox2.Text == "" )
                {
                    MessageBox.Show("请输入差值");
                    return;
                }


                string huobiHtml = GetUrl("https://www.okex.com/v3/c2c/tradingOrders/book?t=1570702053211&side=sell&baseCurrency=btc&quoteCurrency=cny&userType=certified&paymentMethod=all", "utf-8");
                string binanceHtml = GetUrl("https://www.binance.com/api/v3/depth?symbol=MDXUSDT&limit=1000", "utf-8");



              string huobiPrice = Regex.Match(huobiHtml, @"""price"":""([\s\S]*?)""").Groups[1].Value;
                string binancePrice = Regex.Match(binanceHtml, @"\[\[""([\s\S]*?)""").Groups[1].Value;
             


                if (huobiPrice != "" && binancePrice != "")
                {
                    label6.Text = huobiPrice;
                    label9.Text = binancePrice;
               

                    decimal  huobi = Convert.ToDecimal(huobiPrice);
                    decimal binance = Convert.ToDecimal(binancePrice);
                 
                    if ((huobi - binance) > Convert.ToInt32(textBox1.Text))
                    {
                        textBox5.Text += DateTime.Now.ToString() + " : 火币的价格为" + huobi+ "....币安的价格为" + binance + "..火币高于币安" + (huobi - binance) + "\r\n";
                        player.SoundLocation = path;
                        player.Load();
                        player.Play();
                    }

                    if ((binance - huobi) > Convert.ToInt32(textBox2.Text))
                    {
                        textBox5.Text += DateTime.Now.ToString() + " : 火币的价格为" + huobi + "....币安的价格为" + binance + "..币安高于火币" + (binance - huobi) + "\r\n";
                        player.SoundLocation = path ;
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


        async static void ttt()
        {
            try
            {
                ClientWebSocket webSocket = new ClientWebSocket();
                CancellationToken cancellation = new CancellationToken();
               
                  //建立连接
                  await webSocket.ConnectAsync(new Uri("wss://www.huobi.ci/-/s/pro/ws"), cancellation);
                
                byte[] bsend = Encoding.Default.GetBytes("\"{\"sub\":\"market.mdxusdt.kline.1min\",\"id\":\"id10\"}\"");
                //发送数据
                await webSocket.SendAsync(new ArraySegment<byte>(bsend), WebSocketMessageType.Binary, true, cancellation);
              
                //await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "1", cancellation);
                //释放资源
                // webSocket.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        async static void run1()
        {
            ClientWebSocket cln = new ClientWebSocket();
            cln.ConnectAsync(new Uri("wss://www.huobi.ci/-/s/pro/ws"), new CancellationToken()).Wait();
            byte[] bytess = Encoding.Default.GetBytes("{\"sub\":\"market.mdxusdt.kline.1min\",\"id\":\"id10\"}");
            cln.SendAsync(new ArraySegment<byte>(bytess), WebSocketMessageType.Text, true, new CancellationToken()).Wait();
            // string returnValue = await GetAsyncValue(cln);//异步方法
            string returnValue = await GetAsyncValue(cln);//异步方法
            MessageBox.Show(returnValue);
        }

        public static async Task<string> GetAsyncValue(ClientWebSocket clientWebSocket)
        {
            string returnValue = null;
            ArraySegment<Byte> buffer = new ArraySegment<byte>(new Byte[8192]);
            WebSocketReceiveResult result = null;
            using (var ms = new MemoryStream())
            {
                do
                {
                    result = await clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);
                ms.Seek(0, SeekOrigin.Begin);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    using (var reader = new StreamReader(ms, Encoding.UTF8))
                    {
                        returnValue = reader.ReadToEnd();
                        //Console.WriteLine(returnValue);
                    }
                }
            }
            return returnValue;
        }
    




    Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            //if (thread == null || !thread.IsAlive)
            //{
            //    thread = new Thread(run);
            //    thread.Start();
            //    Control.CheckForIllegalCrossThreadCalls = false;
            //}
            //timer1.Start();

            run1();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
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

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            player.Stop();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox5.Text = "";
        }
    }
}
