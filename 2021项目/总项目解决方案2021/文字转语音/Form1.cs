using SpeechLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 文字转语音
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SpeechVoiceSpeakFlags flag = SpeechVoiceSpeakFlags.SVSFlagsAsync;
            SpVoice voice = new SpVoice();
            string voice_txt =textBox1.Text.Trim();
            voice.Voice = voice.GetVoices(string.Empty, string.Empty).Item(0);
            voice.Speak(voice_txt, flag);
        }
    }
}
