using System;
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
            string html = method.GetUrl("https://dropbox.com","");
            if (html == "")
            {
                MessageBox.Show("请求失败，网络错误！");
                return;
            }

            Thread thread = new Thread(new ThreadStart(run2));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
            label1.Text = "软件已经开始运行请勿重复点击....";
        }




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
            for (int i = 0; i < text.Length; i++)
            {
                string[] zhanghao = text[i].Split(new string[] { "-" }, StringSplitOptions.None);
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据         
                lv1.SubItems.Add(zhanghao[0]);
                lv1.SubItems.Add(zhanghao[1]);
                lv1.SubItems.Add("未验证");
            }

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].SubItems[1].Text == "charming0338@naver.com" && listView1.Items[i].SubItems[2].Text == "sexy10518")
                {
                    listView1.Items[i].SubItems[3].Text = "正确";
                }
                else
                {
                    listView1.Items[i].SubItems[3].Text = "已验证,不正确";
                }

                Thread.Sleep(1000);

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
            method.expotTxt(listView1);
        }

        private void 扫号_Load(object sender, EventArgs e)
        {
           
        }
    }
}
