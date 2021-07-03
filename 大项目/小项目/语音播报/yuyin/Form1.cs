using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using SpeechLib;

namespace yuyin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();

            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹      
            if (File.Exists(path + "//plans.txt"))
            {

                StreamReader sr = new StreamReader(path + "//plans.txt");
                //一次性读取完 
                string datas = sr.ReadToEnd();
                string[] data = datas.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < data.Length - 1; i++)
                {
                    string[] each = data[i].Split(new string[] { "#" }, StringSplitOptions.None);

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count+1).ToString());
                    lv1.SubItems.Add(each[0]);
                    lv1.SubItems.Add(each[1]);
                    lv1.SubItems.Add(each[2]);

                }
                sr.Close();
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            System.DateTime currentTime = new System.DateTime();

            currentTime = System.DateTime.Now;

            label3.Text = currentTime.ToString("HH:mm:ss");


                for (int i = 0; i < listView1.Items.Count; i++)
                {
                
                if (label3.Text == listView1.Items[i].SubItems[1].Text.ToString())
                {
                    for (int j = 0; j < Convert.ToInt32(textBox2.Text); j++)
                    {
                        SpeechVoiceSpeakFlags flag = SpeechVoiceSpeakFlags.SVSFlagsAsync;
                        SpVoice voice = new SpVoice();
                        string voice_txt = listView1.Items[i].SubItems[3].Text.ToString();
                        voice.Voice = voice.GetVoices(string.Empty, string.Empty).Item(0);
                        voice.Speak(voice_txt, flag);
                    }


                }

            }     
        }

        /// <summary>
        /// 软件开机自启动
        /// </summary>
        public static void StartRun()
        {
            //获得应用程序路径
            string strAssName = Application.StartupPath + @"\" + Application.ProductName + @".exe";
            //获得应用程序名 
            string ShortFileName = Application.ProductName; RegistryKey rgkRun = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (rgkRun == null)
            {
                rgkRun = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
            }
            rgkRun.SetValue(ShortFileName, strAssName);




            ////此删除注册表中启动项 
            ////获得应用程序名 
            //string ShortFileName = Application.ProductName; RegistryKey rgkRun = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            //if (rgkRun == null)
            //{
            //    rgkRun = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
            //}
            //rgkRun.DeleteValue(ShortFileName, false);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                StartRun();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count+1).ToString());
            lv1.SubItems.Add(dateTimePicker1.Value.ToString("HH:mm:ss"));
            lv1.SubItems.Add(textBox2.Text);
            lv1.SubItems.Add(textBox1.Text);

            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹      


            //for (int i = 0; i < listView1.Items.Count; i++)
            //{
            //    StreamWriter sw3 = File.AppendText(path + "//plans.txt");
            //    sw3.WriteLine(listView1.Items[i].SubItems[1].Text + "#" + listView1.Items[i].SubItems[2].Text + "#" + listView1.Items[i].SubItems[3].Text + "\r");
            //    sw3.Flush();
            //    sw3.Close();
            //}

            StreamWriter sw3 = File.AppendText(path + "//plans.txt");
            sw3.WriteLine(dateTimePicker1.Value.ToString("HH:mm:ss") + "#" + textBox2.Text.Trim() + "#" + textBox1.Text.Trim()+ "\r");
            sw3.Flush();
            sw3.Close();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹      
            System.IO.File.WriteAllText(path + "//plans.txt", string.Empty);
        }
    }
}
