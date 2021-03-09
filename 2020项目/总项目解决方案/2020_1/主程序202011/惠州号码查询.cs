using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 主程序202011
{
    public partial class 惠州号码查询 : Form
    {
        public 惠州号码查询()
        {
            InitializeComponent();
        }
        Thread thread;
        private void button6_Click(object sender, EventArgs e)
        {
            status = true;
            if (thread == null || !thread.IsAlive)
            {
               // timer1.Start();
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
        bool zanting = true;
        bool status = true;
        string ip = "";


        public void changeIp()
        {
            string html = method.GetUrl("http://47.106.170.4:8081/Index-generate_api_url.html?packid=7&fa=5&groupid=0&fetch_key=&qty=1&port=1&format=txt&ss=1&css=&pro=&city=&usertype=7", "utf-8");
            ip = html.Trim();
            label4.Text = ip;
        }
        public void run()
        {
            
            if (textBox1.Text == "")
            {
                MessageBox.Show("请选择手机号");
                return;
            }
            StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();

            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != "")
                {
                    label3.Text = "正在查询：" + text[i];
                    string url = "https://www.hzydbsp.com:8085/WinSvcInterface/CL/GetCLProjectSearchQYWX.ashx?phone="+ text[i].Trim();
                    string cookie = "bsp.hz.gmcc.net=080875A6965A7AD944CE103F797516AF7CB02D058FB84DBE7C8CE3882D695E431E0A2591BF4C222CD4372F848EA701152F1E359C904693F7F5A11A3C5645D9C170736C10043DE8F17FD49569517F8ECC4C32B224CCF4E08507B1CCC859E81D6D9F9EAD1E19B11DD0D82981C3459180EA7D25759E; ASP.NET_SessionId=1vfb3x11hgsrvqmefk3aqgis";
                    string html = method.GetUrlWithCookie(url,cookie ,"utf-8");
                   
                    Match name = Regex.Match(html, @"""RetMsg"":""([\s\S]*?)""");
                    label4.Text = "运行中";
                    if (name.Groups[1].Value.Contains("上限"))
                    {
                        label4.Text = "暂停中";
                        Thread.Sleep(20000);
                        
                    }

                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(text[i]);
                        lv1.SubItems.Add("请求成功");
                        lv1.SubItems.Add(name.Groups[1].Value.Replace("<br/>", "  "));

                        Thread.Sleep(Convert.ToInt32(textBox2.Text) * 1000);
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;
                    
                }
            }



        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {
                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        int jishi = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
           
            if (zanting == true)
            {
                label4.Text = "查询中:" + jishi;
                jishi = jishi + 1;
                if (jishi > 60)
                {
                    zanting = false;
                }
            }
            if(zanting==false)
            {
                label4.Text = "暂停中:" + (jishi - 50);
                jishi = jishi - 1;
                if (jishi <=50)
                {
                    zanting = true;
                    jishi = 0;
                    
                }
                
            }

        }

        private void 惠州号码查询_Load(object sender, EventArgs e)
        {

        }
    }
}
