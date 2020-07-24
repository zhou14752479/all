using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsharpHttpHelper;
using helper;

namespace 主程序202007
{
    public partial class 聚享游监测 : Form
    {
        public 聚享游监测()
        {
            InitializeComponent();
        }


        string cookie = "UM_distinctid=1735b96c46615a-02d3c61005ab07-6373664-1fa400-1735b96c46743f; __root_domain_v=.juxiangyou.com; _qddaz=QD.yozua3.pr0ki3.kcpvn6ue; _ga=GA1.2.1707749721.1594969475; cookie_userip=49.70.93.110; cookie_connect=s%3A0iORPiQpsyQFTcu6u3fcl2QcAxurcapp.crQ7jYs9eCVPiTtvC7AZLsc0Rbd%2BY8EL%2FBqW%2FsBQBW4; CNZZDATA5330249=cnzz_eid%3D290660917-1594969068-%26ntime%3D1595136476; Hm_lvt_f37751c35528926bb0c6f2ecd1b3c648=1594969474,1594974672,1595028238,1595138397; _gid=GA1.2.1156006068.1595138398; cookie_userid=23901979; cookie_autokey=; cookie_tokenkey=83ee8dd31bc41522a9a5369aa6f122373ddb538da0030abf6c577dca350b0c93; play_ad_fc=1; Hm_lpvt_f37751c35528926bb0c6f2ecd1b3c648=1595138408; _qdda=3-1.2ciyy3; _qddab=3-kafosg.kcso80v1; _qddamta_2852150146=3-0";
        Dictionary<string,string> dics=new Dictionary<string,string>();
        bool zhixing = false;

        private string GetHttp(string url)
        {
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = url,
                Method = "GET",
                Accept = "*/*",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36",
                Referer = "http://www.juxiangyou.com/fun/play/fun16/index",
                Host = "www.juxiangyou.com",
                Cookie = cookie,
            };
            item.Header.Add("Accept-Encoding", "");
            item.Header.Add("Accept-Language", "zh-cn,zh,en");
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            return html;
        }

        public bool panduan(string value)
        {

            string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == value)
                {

                    return true;

                }

            }

            return false;

        }

        string path = AppDomain.CurrentDomain.BaseDirectory;



        public void run()
        {
           
            zhixing = true;
            string url = "http://www.juxiangyou.com/fun/play/fun16/index";
            string html = GetHttp(url);
            
            
            Match values = Regex.Match(html, @"开心60秒第<span class=""clr-red"">([\s\S]*?)</span>期开奖结果：([\s\S]*?)<span class=""ball-num"">([\s\S]*?)</span>");
            string qishu = values.Groups[1].Value.Trim();
            string zhi1 = values.Groups[2].Value.Trim();
            string zhi2 = values.Groups[3].Value.Trim();

            if (!dics.ContainsKey(qishu))
            {
                
                dics.Add(qishu, zhi2);
                textBox3.Text += "期数：" + qishu + " 结果：" + zhi1 + "" + zhi2 + "\r\n";

                if (panduan(zhi2))
                {
                    
                    textBox3.Text += "【开出号码】：" + zhi2 + "\r\n";


                    SoundPlayer player = new SoundPlayer();
                    player.SoundLocation = path+"vedio.wav";
                    player.Load();
                    player.Play();
                    MessageBox.Show("【开出号码】：" + zhi2);
                }


                cishus[zhi2] = cishus[zhi2] + 1;
                if (zhi2 == textBox2.Text.Trim() && cishus[zhi2].ToString() == textBox4.Text.Trim())
                {
                   
                    textBox3.Text += "【开出号码】：" + zhi2 + "达到指定次数：" + cishus[zhi2] + "\r\n";

                    SoundPlayer player = new SoundPlayer();
                    player.SoundLocation = path + "vedio.wav";
                    player.Load();
                    player.Play();
                    MessageBox.Show("【开出号码】：" + zhi2 + "达到指定次数：" + cishus[zhi2]);
                }
                this.textBox3.Focus();
                this.textBox3.Select(this.textBox3.TextLength, 0);
                this.textBox3.ScrollToCaret();
            }


            zhixing = false;

        }
        Dictionary<string, int> cishus = new Dictionary<string, int>();
        private void 聚享游监测_Load(object sender, EventArgs e)
        {
            
            cishus.Add("3",0);
            cishus.Add("4", 0);
            cishus.Add("5", 0);
            cishus.Add("6", 0);
            cishus.Add("7", 0);
            cishus.Add("8", 0);
            cishus.Add("9", 0);
            cishus.Add("10", 0);
            cishus.Add("11", 0);
            cishus.Add("12", 0);
            cishus.Add("13", 0);
            cishus.Add("14", 0);
            cishus.Add("15", 0);
            cishus.Add("16", 0);
            cishus.Add("17", 0);
            cishus.Add("18", 0);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"kaixin60"))
            {
                button1.Enabled = false;
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
                timer1.Start();
            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (zhixing == false)
            {
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            timer1.Stop();
        }
    }
}
