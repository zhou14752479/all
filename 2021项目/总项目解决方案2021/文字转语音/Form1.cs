using SpeechLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace 文字转语音
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private SpeechSynthesizer speech;
        /// <summary>
        /// 音量
        /// </summary>
        private int value = 100;
        /// <summary>
        /// 语速
        /// </summary>
        private int rate = 1;


        private void Speak()
        {
            rate = Int32.Parse(comboBox1.Text);
            speech.Rate = rate;
            speech.SelectVoice("Microsoft Huihui Desktop");//设置播音员（中文）
                                                           //speech.SelectVoice("Microsoft Anna"); //英文
            speech.Volume = value;
            speech.SpeakAsync(textBox1.Text);//语音阅读方法
            speech.SpeakCompleted += speech_SpeakCompleted;//绑定事件
        }

        void speech_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            button1.Text = "语音试听";
        }



        private void button1_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            string text = textBox1.Text;

            if (text.Trim().Length == 0)
            {
                MessageBox.Show("不能阅读空内容!", "错误提示");
                return;
            }

            if (button1.Text == "语音试听")
            {

                speech = new SpeechSynthesizer();

                new Thread(Speak).Start();

                button1.Text = "停止试听";

            }
            else if (button1.Text == "停止试听")
            {

                speech.SpeakAsyncCancelAll();//停止阅读

                button1.Text = "语音试听";
            }


        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //因为trackBar1的值为（0-10）之间而音量值为（0-100）所以要乘10；
            value = trackBar1.Value * 10;
        }

        private void button2_Click(object sender, EventArgs e)
        {


            string text = textBox1.Text;

            if (text.Trim().Length == 0)
            {
                MessageBox.Show("空内容无法生成!", "错误提示");
                return;
            }

            this.SaveFile(text);

        }



        /// <summary>
        /// 生成语音文件的方法
        /// </summary>
        /// <param name="text"></param>
        private void SaveFile(string text)
        {
            speech = new SpeechSynthesizer();
            var dialog = new SaveFileDialog();
            dialog.Filter = "*.wav|*.wav|*.mp3|*.mp3";
            dialog.ShowDialog();

            string path = dialog.FileName;
            if (path.Trim().Length == 0)
            {
                return;
            }
            speech.SetOutputToWaveFile(path);
            speech.Volume = value;
            speech.Rate = rate;
            speech.Speak(text);
            speech.SetOutputToNull();
            MessageBox.Show("生成成功!在" + path + "路径中！", "提示");

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            rate = Int32.Parse(comboBox1.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }
    }
}
