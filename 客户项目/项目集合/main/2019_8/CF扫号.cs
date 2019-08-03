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

namespace main._2019_8
{
    public partial class CF扫号 : Form
    {
        public CF扫号()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }
        bool zanting = true;
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
                label1.Text = "正在验证" + text[i];

                string URL = "https://api.unipay.qq.com/v1/r/1450000251/get_role_list?pf=mds_pay-__mds_webpay_iframe.pay-website&pfkey=pfkey&aid=pay.index.cf&from_h5=1&pc_st=3BE4D784-0E1A-41E2-A437-581321F6AC091564801388450&r=0.7985930536221353&openid=852266010&openkey=%40DexEZHrwK&session_id=uin&session_type=skey&sck=CAC52DB62D24551C98322A60A5F22A0B&anti_auto_script_token_id=E34436CBB13DFA9AD8585CD634A2339C87B32152C86AD0EA7ECE6B593DB5993C5B35E178A44A10FD96928BB84C36D5EA263702174537C0DF97DB0B1F530A4F4F&isusempaymode=1&zoneid=320&game_scene=dq_pay&provide_uin="+ text[i] + "&webversion=minipayv2&from_https=1&t=1564801427315&__refer=https%3A%2F%2Fpay.qq.com%2Fmidas%2Fminipay_v2%2Fviews%2Fpayindex%2Fpcgame.shtml%3Fcode%3Dcfdq%26aid%3Dpay.index.cf%26pf%3Dmds_pay-__mds_webpay_iframe.pay-website%26extend%3DeditableForTeam%253D0%26openid%3D%26openkey%3D%26session_id%3Duin%26session_type%3Dvask_27";

                string html = method.GetUrl(URL, "utf-8");

                if (html.Contains("未创建"))
                {

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据         
                    lv1.SubItems.Add(text[i]);   //比分
                    lv1.SubItems.Add("未创建");
                }
                else
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据         
                    lv1.SubItems.Add(text[i]);
                    lv1.SubItems.Add("已创建角色");   //比分
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
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
