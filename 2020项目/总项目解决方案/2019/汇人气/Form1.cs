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
using helper;

namespace 汇人气
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

      
        bool zanting = true;
        public static string COOKIE="";


        string vip = "-1";
        string startTime = "";
        string endTime = "";

        public int getPage()
        {
            if (checkBox1.Checked == false)
            {
                startTime = dateTimePicker1.Value.ToString("yyyy-MM-dd")+" 00:00:00";
                endTime = dateTimePicker2.Value.ToString("yyyy-MM-dd")+" 23:59:59";
            }
            string url = "http://boss.tao-liang.cn/api/member/boss/mbmge/list";
            string postdata = "{\"id\":\"\",\"vip\":\"" + vip + "\",\"weixin\":\"" + textBox13.Text + "\",\"serverWeixin\":\"" + textBox6.Text + "\",\"mobile\":\"" + textBox11.Text + "\",\"qq\":\"" + textBox12.Text + "\",\"registerIP\":\"" + textBox7.Text + "\",\"loginIP\":\"" + textBox8.Text + "\",\"startTime\":\"" + startTime + "\",\"endTime\":\"" + endTime + "\",\"page\":1,\"limit\":"+textBox10.Text+"}";
           
            string html = method.PostUrl(url, postdata, COOKIE, "utf-8");
            Match page = Regex.Match(html, @"totalPage"":([\s\S]*?)}");
            return Convert.ToInt32(page.Groups[1].Value);
        }


        #region  主程序
        public void run()

        {
            
            
            if (COOKIE == "")
            {
                MessageBox.Show("请先登陆");
                return;
            }
            int page = getPage();
            if (checkBox1.Checked == false)
            {
                startTime = dateTimePicker1.Value.ToString("yyyy-MM-dd") + " 00:00:00";
                endTime = dateTimePicker2.Value.ToString("yyyy-MM-dd") + " 23:59:59";
            }
            
            try
            {
              

                for (int i = 1; i <page+1; i++)
                {
                   
                    string url = "http://boss.tao-liang.cn/api/member/boss/mbmge/list";
                    string postdata = "{\"id\":\"\",\"vip\":\""+vip+"\",\"weixin\":\""+textBox13.Text+"\",\"serverWeixin\":\""+textBox6.Text+"\",\"mobile\":\""+textBox11.Text+"\",\"qq\":\""+textBox12.Text+"\",\"registerIP\":\""+textBox7.Text+"\",\"loginIP\":\""+textBox8.Text+"\",\"startTime\":\""+startTime+"\",\"endTime\":\""+endTime+"\",\"page\":"+i+",\"limit\":"+textBox10.Text+"}";
                   
                    string html = method.PostUrl(url,postdata, COOKIE, "utf-8");

                    MatchCollection tels = Regex.Matches(html, @"""username"":""([\s\S]*?)""");
                    MatchCollection QQs = Regex.Matches(html, @"""qq"":""([\s\S]*?)""");
                    MatchCollection coins = Regex.Matches(html, @"""coin"":([\s\S]*?),");
                    MatchCollection chongzhis = Regex.Matches(html, @"""totalRecharge"":([\s\S]*?),");
                    MatchCollection atime = Regex.Matches(html, @"""registerTime"":""([\s\S]*?)""");
                    MatchCollection btime = Regex.Matches(html, @"""loginTime"":""([\s\S]*?)""");
                    MatchCollection aip = Regex.Matches(html, @"""registerIp"":""([\s\S]*?)""");
                    MatchCollection bip = Regex.Matches(html, @"""loginIp"":""([\s\S]*?)""");
                    if (tels.Count == 0)
                    {
                        break;
                    }
                    for (int j = 0; j < tels.Count; j++)
                    {
                        if (Convert.ToDecimal(coins[j].Groups[1].Value) < Convert.ToDecimal(textBox4.Text))
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count+1).ToString()); //使用Listview展示数据 
                            if (tels[j].Groups[1].Value.Length > 7 && QQs[j].Groups[1].Value.Length > 5)
                            {
                                lv1.SubItems.Add("*******" + tels[j].Groups[1].Value.Substring(7));
                                lv1.SubItems.Add("*****" + QQs[j].Groups[1].Value.Substring(QQs[j].Groups[1].Length - 4));
                            }
                            else
                            {
                                lv1.SubItems.Add( tels[j].Groups[1].Value);
                                lv1.SubItems.Add( QQs[j].Groups[1].Value);
                            }
                           
                            lv1.SubItems.Add(coins[j].Groups[1].Value);
                            lv1.SubItems.Add((Convert.ToInt32(chongzhis[j].Groups[1].Value)/100).ToString());
                            lv1.SubItems.Add(atime[j].Groups[1].Value);
                            lv1.SubItems.Add(btime[j].Groups[1].Value);
                            lv1.SubItems.Add(aip[j].Groups[1].Value);
                            lv1.SubItems.Add(bip[j].Groups[1].Value);

                        }

                    }




                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    if (listView1.Items.Count > 2)
                    {
                        listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                    }
                    Thread.Sleep(Convert.ToInt32(textBox9.Text));
                }

                MessageBox.Show("抓取完成");

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString()); 
            }
        }

        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
             COOKIE = method.getheader("http://boss.tao-liang.cn/api/member/boss/auth/login", "username="+textBox1.Text.Trim()+ "&password=" + textBox2.Text.Trim() + "&secret=boss.tao-liang.cn&");
            MessageBox.Show("登陆成功");
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
            MessageBox.Show(getPage().ToString());
            zanting = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (textBox3.Text == "hpx")
            {
                groupBox1.Visible = true;

            }

            else
            {
                MessageBox.Show("密码错误");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "普通会员")
            {
                vip = "0";
            }

            if (comboBox1.Text == "VIP会员")
            {
                vip = "1";
            }
        }
    }
}
