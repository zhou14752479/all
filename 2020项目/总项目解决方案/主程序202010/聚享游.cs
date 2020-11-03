using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsharpHttpHelper;
using helper;

namespace 主程序202010
{
    public partial class 聚享游 : Form
    {

        

        public 聚享游()
        {
            InitializeComponent();
           
        }


        public void getyingkui()
        {

            string html = GetHttp();
          
            Match yingkui = Regex.Match(html, @"今日盈亏</p>([\s\S]*?)</p>");
            label23.Text = Regex.Replace(yingkui.Groups[1].Value, "<[^>]+>", "").Trim();

        
            if (label23.Text.Contains("+"))
            {
                if (Convert.ToInt64(label23.Text.Replace(",", "").Replace("+", "").Replace("-", "")) > Convert.ToInt64(textBox2.Text))
                {
                    timer1.Stop();
                    MessageBox.Show("达到止盈标准，停止运行");
                }
            }
            if (label23.Text.Contains("-"))
            {
                if (Convert.ToInt64(label23.Text.Replace(",", "").Replace("+", "").Replace("-", "")) > Convert.ToInt64(textBox3.Text))
                {
                    timer1.Stop();
                    MessageBox.Show("达到止亏标准，停止运行");
                }
            }

        }

        private string GetHttp()
        {
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = "http://www.juxiangyou.com/fun/play/crazy28/wdjc",
                Method = "GET",
                Host = "www.juxiangyou.com",
                Accept = "application/json, text/javascript, */*; q=0.01",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36",
                ContentType = "application/x-www-form-urlencoded; charset=UTF-8",
                Referer = "http://www.juxiangyou.com/fun/play/crazy28/jctz?id=1915220",
                Cookie = cookieBrowser.cookie,
                PostEncoding = Encoding.UTF8,
                Postdata = ""
            };
            item.Header.Add("X-Sign", "kgu60qx2");
            item.Header.Add("X-Requested-With", "XMLHttpRequest");
            item.Header.Add("Accept-Encoding", "gzip, deflate");
            item.Header.Add("Accept-Language", "zh-CN,zh;q=0.9");
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            return html;
        }
        private string GetHttp20201023110922(string postdata,string cookie,string xsign)
        {
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = "http://www.juxiangyou.com/fun/play/interaction",
                Method = "POST",
                Host = "www.juxiangyou.com",
                Accept = "application/json, text/javascript, */*; q=0.01",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36",
                ContentType = "application/x-www-form-urlencoded; charset=UTF-8",
                Referer = "http://www.juxiangyou.com/fun/play/crazy28/jctz?id=1915220",
                Cookie =cookie ,
                PostEncoding = Encoding.UTF8,
                Postdata = postdata
            };
            item.Header.Add("X-Sign", xsign);
            item.Header.Add("X-Requested-With", "XMLHttpRequest");
            item.Header.Add("Accept-Encoding", "gzip, deflate");
            item.Header.Add("Accept-Language", "zh-CN,zh;q=0.9");
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            return html;
        }

        public string Qihao= "";
        public string Content = "";
        public void getYuce()
        {
            try
            {
                string suanfaId = "1";
                if (radioButton1.Checked == true)
                {
                    suanfaId = "1";
                }
                if (radioButton2.Checked == true)
                {
                    suanfaId = "2";
                }
                if (radioButton3.Checked == true)
                {
                    suanfaId = "3";
                }
                if (radioButton4.Checked == true)
                {
                    suanfaId = "4";
                }
                if (radioButton5.Checked == true)
                {
                    suanfaId = "5";
                }

                string url = "https://www.28iq.com/api/yuce/indexYuce";
                string postdata = "gameId=32&yuceInfoId=4&suanfaId=" + suanfaId;
                string html = method.PostUrl(url, postdata, "", "utf-8");
                Match qihao = Regex.Match(html, @"""qihao"":([\s\S]*?),");
                Match content = Regex.Match(html, @"""content"":""([\s\S]*?)""");
                Qihao = qihao.Groups[1].Value.Trim();
                Content= content.Groups[1].Value.Trim();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
           
        }

        /// <summary>
        /// 获取时间戳毫秒
        /// </summary>
        /// <returns></returns>
        public long GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalMilliseconds);

            return a;
        }

       

        public void xiazhu()
        {
            getYuce();
           
            string v0 = "0", v1 = "0", v2 = "0", v3 = "0", v4 = "0", v5 = "0", v6 = "0", v7 = "0", v8 = "0", v9 = "0", v10 = "0", v11 = "0", v12 = "0", v13 = "0", v14 = "0", v15 = "0", v16 = "0", v17 = "0", v18 = "0", v19 = "0", v20 = "0", v21 = "0", v22 = "0", v23 = "0", v24 = "0", v25 = "0", v26 = "0", v27 = "0";
            try
            {
               
                switch (Content)
                {
                    case "杀3N0":
                        v0 = t0.Text.Trim();
                        v3 = t3.Text.Trim();
                        v6 = t6.Text.Trim();
                        v9 = t9.Text.Trim();
                        v12 = t12.Text.Trim();
                        v15 = t15.Text.Trim();
                        v18 = t18.Text.Trim();
                        v21 = t21.Text.Trim();
                        v24 = t24.Text.Trim();
                        v27= t27.Text.Trim();
                        break;
                    case "杀3N1":
                        v1 = t1.Text.Trim();
                        v4 = t4.Text.Trim();
                        v7 = t7.Text.Trim();
                        v10 = t10.Text.Trim();
                        v13 = t13.Text.Trim();
                        v16 = t16.Text.Trim();
                        v19 = t19.Text.Trim();
                        v22 = t22.Text.Trim();
                        v25 = t25.Text.Trim();
                        break;
                    case "杀3N2":
                        v2 = t2.Text.Trim();
                        v5 = t5.Text.Trim();
                        v8 = t8.Text.Trim();
                        v11 = t11.Text.Trim();
                        v14= t14.Text.Trim();
                        v17= t17.Text.Trim();
                        v20 = t20.Text.Trim();
                        v23 = t23.Text.Trim();
                        v26 = t26.Text.Trim();
                        break;
                }
                long nowtime = GetTimeStamp();
                string xsign = Jinzhi36(nowtime);
                StringBuilder sb = new StringBuilder();
                sb.Append("%5b"+v0);
                sb.Append("%2c" + v1);
                sb.Append("%2c" + v2);
                sb.Append("%2c" + v3);
                sb.Append("%2c" + v4);
                sb.Append("%2c" + v5);
                sb.Append("%2c" + v6);
                sb.Append("%2c" + v7);
                sb.Append("%2c" + v8);
                sb.Append("%2c" + v9);
                sb.Append("%2c" + v10);
                sb.Append("%2c" + v11);
                sb.Append("%2c" + v12);
                sb.Append("%2c" + v13);
                sb.Append("%2c" + v14);
                sb.Append("%2c" + v15);
                sb.Append("%2c" + v16);
                sb.Append("%2c" + v17);
                sb.Append("%2c" + v18);
                sb.Append("%2c" + v19);
                sb.Append("%2c" + v20);
                sb.Append("%2c" + v21);
                sb.Append("%2c" + v22);
                sb.Append("%2c" + v23);
                sb.Append("%2c" + v24);
                sb.Append("%2c" + v25);
                sb.Append("%2c" + v26);
                sb.Append("%2c" + v27+ "%5d");

                string inpudata = sb.ToString();
                //160387578541
                string postdata = "jxy_parameter=%7B%22fun%22%3A%22lottery%22%2C%22c%22%3A%22quiz%22%2C%22items%22%3A%22crazy28%22%2C%22lssue%22%3A"+Qihao+"%2C%22lotteryData%22%3A"+inpudata+ "%7D&timestamp="+nowtime;
               
                string html= GetHttp20201023110922(postdata, cookieBrowser.cookie,xsign);
                ListViewItem lv1 = listView1.Items.Add(Qihao); //使用Listview展示数据
              
                lv1.SubItems.Add(Content);
                lv1.SubItems.Add(html);
                lv1.SubItems.Add(DateTime.Now.ToString());
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void 聚享游_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            getyingkui();
            timer1.Start();
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(xiazhu);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

        }
        Thread thread;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(xiazhu);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            getyingkui();
           
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cookieBrowser cb = new cookieBrowser("http://www.juxiangyou.com/login/index");
            cb.Show();
        }

        /// <summary>
        /// 将十进制转换为指定的进制
       
        public static string Jinzhi36(long i)
        {
            string s = "";
            long j = 0;
            while (i > 36)
            {
                j = i % 36;
                if (j <= 9)
                    s += j.ToString();
                else
                    s += Convert.ToChar(j - 10 + 'a');
                i = i / 36;
            }
            if (i <= 9)
                s += i.ToString();
            else
                s += Convert.ToChar(i - 10 + 'a');
            Char[] c = s.ToCharArray();
            Array.Reverse(c);
            return new string(c);

        }


        private void button2_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(Jinzhi36(1603935897158));
            MessageBox.Show("已停止");
            timer1.Stop();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (Control ctr in splitContainer1.Panel2.Controls)
            {

                if (ctr is TextBox)
                {

                    ctr.Text = ctr.Text.Replace("000", "");
                }
            }


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            foreach (Control ctr in splitContainer1.Panel2.Controls)
            {

                if (ctr is TextBox)
                {
                    switch (comboBox1.Text)
                    {
                        case "除10":
                            ctr.Text = (Convert.ToInt32(ctr.Text) / 10).ToString();
                            break;
                        case "除100":
                            ctr.Text = (Convert.ToInt32(ctr.Text) / 100).ToString();
                            break;
                        case "除1000":
                            ctr.Text = (Convert.ToInt32(ctr.Text) / 1000).ToString();
                            break;
                        case "除10000":
                            ctr.Text = (Convert.ToInt32(ctr.Text) / 10000).ToString();
                            break;

                        case "乘2":
                            ctr.Text = (Convert.ToInt32(ctr.Text) * 2).ToString();
                            break;
                        case "乘3":
                            ctr.Text = (Convert.ToInt32(ctr.Text) * 3).ToString();
                            break;
                        case "乘5":
                            ctr.Text = (Convert.ToInt32(ctr.Text) * 5).ToString();
                            break;
                        case "乘10":
                            ctr.Text = (Convert.ToInt32(ctr.Text) * 10).ToString();
                            break;
                    }



                }
            }




        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
