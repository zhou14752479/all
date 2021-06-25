using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 客户美团
{
    public partial class gongshang : Form
    {
        public gongshang()
        {
            InitializeComponent();
        }

        string cookie = "";


        #region GET请求
        public string GetUrl(string Url)
        {
            try
            {


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                string COOKIE = cookie;
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.11(0x17000b21) NetType/4G Language/zh_CN";
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("openIdCipher", "AwQAAABJAgAAAAEAAAAyAAAAPLgC95WH3MyqngAoyM/hf1hEoKrGdo0pJ5DI44e1wGF9AT3PH7Wes03actC2n/GVnwfURonD78PewMUppAAAADhS4d+zREPZw1PQNF/0Zp8SLSbtYsmCKZFYbIjL5Ty7FJZwQ/bkMIGOGHHGqk1Nld5+rcxtuifNmA==");
                request.Referer = "https://servicewechat.com/wxde8ac0a21135c07d/350/page-frame.html";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string content = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                ex.ToString();



            }
            return "";
        }
        #endregion

        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
            string[] areas = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None); ;
            string[] keywords = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None); ;
            if (keywords.Length == 0)
            {
                MessageBox.Show("请添加关键字");
                return;
            }

            int count = 0;
            try
            {


                for (int a = 0; a < areas.Length; a++)
                {

                    foreach (string keyword in keywords)
                    {
                     

                        for (int page = 1; page < 1000; page++)
                        {

                            string url = "https://aiqicha.baidu.com/s/l?q=%E5%AE%BF%E8%BF%81%E5%85%AC%E5%8F%B8&t=&p="+page+"&s=10&o=0&f=%7B%7D";
                            string html = GetUrl(url);

                            MatchCollection names = Regex.Matches(html, @"""name"":""([\s\S]*?)""");
                            MatchCollection phones = Regex.Matches(html, @"""tel"":([\s\S]*?),");
                            MatchCollection address = Regex.Matches(html, @"""address"":([\s\S]*?),");
                            MatchCollection citys = Regex.Matches(html, @"""cityname"":([\s\S]*?),");
                            MatchCollection locations = Regex.Matches(html, @"""location"":([\s\S]*?),");
                            if (names.Count == 0)
                                break;

                            for (int i = 0; i < names.Count; i++)
                            {


                                string shouji = "";
                                string guhua = "";
                                string[] tels = phones[i].Groups[1].Value.Split(new string[] { ";" }, StringSplitOptions.None);
                                if (tels.Length == 1)
                                {
                                    if (phones[i].Groups[1].Value.Contains("-"))
                                    {
                                        guhua = phones[i].Groups[1].Value;
                                    }
                                    else
                                    {
                                        shouji = phones[i].Groups[1].Value;
                                    }
                                }
                                if (tels.Length == 2)
                                {
                                    if (phones[i].Groups[1].Value.Contains("-"))
                                    {
                                        if (tels[0].Contains("-"))
                                        {
                                            guhua = tels[0];
                                            shouji = tels[1];
                                        }
                                        else
                                        {
                                            guhua = tels[1];
                                            shouji = tels[0];
                                        }
                                    }
                                    else
                                    {
                                        guhua = "";
                                        shouji = tels[0];
                                    }
                                }
                                if (phones[i].Groups[1].Value == "[]")
                                {
                                    shouji = "";
                                }



                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                                lv1.SubItems.Add(names[i].Groups[1].Value);
                                lv1.SubItems.Add(shouji.Replace("\"", ""));
                                lv1.SubItems.Add(guhua.Replace("\"", ""));
                                lv1.SubItems.Add(address[i].Groups[1].Value.Replace("\"", ""));
                                lv1.SubItems.Add(citys[i].Groups[1].Value.Replace("\"", ""));
                                lv1.SubItems.Add(keyword);
                                lv1.SubItems.Add(locations[i].Groups[1].Value.Replace("\"", ""));
                                if (status == false)
                                    return;
                                Thread.Sleep(300);
                                count = count + 1;
                                label4.Text = count.ToString();
                            }

                            Thread.Sleep(1000);
                        }
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }

        }




        private void Form1_Load(object sender, EventArgs e)
        {
            ProvinceCity.ProvinceCity.BindProvince(comboBox1);
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {

            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProvinceCity.ProvinceCity.BindCity(comboBox1, comboBox2);
        }




        bool status = true;
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html");

            if (!html.Contains(@"YVoWQ"))
            {
                MessageBox.Show("");
                return;
            }



            #endregion
            #region 通用检测

            string ahtml = GetUrl("http://139.129.92.113/");

            if (!ahtml.Contains(@"siyisoft"))
            {

                return;
            }



            #endregion
            status = true;
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("区域或行业为空");
                return;
            }

            if (thread == null || !thread.IsAlive)
            {

                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            else
            {
                status = false;
            }


        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {



        }


      




        private void button2_Click(object sender, EventArgs e)
        {
            method.ListviewToTxt(listView1, 2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Contains(comboBox2.Text))
            {
                MessageBox.Show(comboBox2.Text + "：请勿重复添加", "重复添加错误");
                return;
            }
            textBox1.Text += comboBox2.Text + "\r\n";
        }

   
        private Point mPoint = new Point();
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }
    }
}
