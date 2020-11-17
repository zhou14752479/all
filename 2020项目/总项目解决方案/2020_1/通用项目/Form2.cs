using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 通用项目
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {


            string strhtml = method.GetUrl("https://server.dailiantong.com/API/AppService.ashx?Action=LevelOrderList&callback=callback&IsPub=7&GameID=121&ZoneID=0&ServerID=0&SearchStr=&Sort_Str=&PageIndex=1&PageSize=20&Price_Str=&PubCancel=0&SettleHour=0&FilterType=0&PGType=0&Focused=-1&TimeStamp=1581816887&Ver=1.0&AppVer=2.0.0&AppOS=webapp&AppID=webapp&Sign=7c58ad60aa322ba607fd1aa0a4cd23a2", "utf-8");
            
            Match title = Regex.Match(strhtml, @"""Title"":""([\s\S]*?)""");
            if (title.Groups[1].Value == "")
            {
                textBox1.Text += DateTime.Now.ToString() + ":" + "未发现定单..." + "\r\n";
            }
            else
            {

                string textToSpeak = "你好,发现定单，请及时查看";
                SpeechSynthesizer synthes = new SpeechSynthesizer();
                //synthes.Speak(textToSpeak);//同步
                synthes.SpeakAsync(textToSpeak);//异步
                textBox1.Text += DateTime.Now.ToString() + ":" + "【发现定单】" + "\r\n";

            }



        }
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            run();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            MessageBox.Show("已停止监控");
            button1.Enabled = true;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            timer1.Start();
        }
    }
}
