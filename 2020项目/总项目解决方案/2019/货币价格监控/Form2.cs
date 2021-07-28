
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
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


      
　　public static uint SND_ASYNC = 0x0001;
    public static uint SND_FILENAME = 0x00020000;
    [DllImport("winmm.dll")]
    public static extern uint mciSendString(string lpstrCommand,
    string lpstrReturnString, uint uReturnLength, uint hWndCallback);
    public void Play()
    {
       mciSendString(@"close temp_alias", null, 0, 0);
        mciSendString(@"open ""9378.mp3"" alias temp_alias", null, 0, 0);
        mciSendString("play temp_alias repeat", null, 0, 0);
           
    }




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


        
        public void runpy()
        {
            Process p = new Process();
            p.StartInfo.FileName = "huobi.exe";    //填写exe的具体路径
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "abc 123";    //参数
            p.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            p.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
            p.Start();
            p.BeginOutputReadLine();
            p.WaitForExit();
            p.Close();


        }

        string huobiHtml = "";
       void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            huobiHtml=(outLine.Data);
           
        }

        bool playing = false;


        #region 主程序
        public  void run()
        {
           
            try

            {
                if (textBox1.Text == "" || textBox2.Text == "" )
                {
                    MessageBox.Show("请输入差值");
                    return;
                }


              
                string binanceHtml = GetUrl("https://www.binance.com/api/v3/depth?symbol=MDXUSDT&limit=1000", "utf-8");


 
                    string huobiPrice = Regex.Match(huobiHtml, @"""close"":([\s\S]*?),").Groups[1].Value;
                    string binancePrice = Regex.Match(binanceHtml, @"\[\[""([\s\S]*?)""").Groups[1].Value;


              
                if (huobiPrice != "" && binancePrice != "")
                {
                    label6.Text = huobiPrice;
                    label9.Text = binancePrice;
               

                    decimal  huobi = Convert.ToDecimal(huobiPrice);
                    decimal binance = Convert.ToDecimal(binancePrice);
                 
                    if ((huobi - binance) > Convert.ToDecimal(textBox1.Text))
                    {
                        
                        textBox5.Text = DateTime.Now.ToString() + " : 火币的价格为" + huobi+ "....币安的价格为" + binance + "..火币高于币安" + (huobi - binance) + "\r\n";
                      
                        if(playing==false)
                        {
                            playing = true;
                             Play();
                        }
                           
                         
                        
                       
                    }

                    if ((binance - huobi) > Convert.ToDecimal(textBox2.Text))
                    {
                      
                        textBox5.Text = DateTime.Now.ToString() + " : 火币的价格为" + huobi + "....币安的价格为" + binance + "..币安高于火币" + (binance - huobi) + "\r\n";
                        if (playing == false)
                        {
                            playing = true;
                            Play();
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



        Thread thread;
        Thread thread1;




        private void button1_Click(object sender, EventArgs e)
        {
            if (thread1 == null || !thread1.IsAlive)
            {
                Thread thread1 = new Thread(runpy);
                thread1.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            timer1.Start();
          
           
        }
       
        private void button2_Click(object sender, EventArgs e)
        {
            Play();


            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (thread == null || !thread.IsAlive)
            //{
            //    thread = new Thread(run);
            //    thread.Start();
            //    Control.CheckForIllegalCrossThreadCalls = false;
            //}

            run();
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
            
            mciSendString(@"close temp_alias", null, 0, 0);
            playing = false;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
            textBox5.Text = "";
        }

        private void Form2_Load(object sender, EventArgs e)
        {
          
        }

       
    }
}
