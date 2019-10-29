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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        int objType =-1;
        bool zanting = true;
        public static string COOKIE = "";
        string startTime = "";
        string endTime = "";
        string RstartTime = "";
        string RendTime = "";
        int state = -1;
        int type = -1;




        public int getPage()
        {
            if (checkBox1.Checked == false)
            {
                startTime = dateTimePicker1.Value.ToString("yyyy-MM-dd") + " 00:00:00";
                endTime = dateTimePicker2.Value.ToString("yyyy-MM-dd") + " 23:59:59";
                RstartTime = dateTimePicker3.Value.ToString("yyyy-MM-dd") + " 00:00:00";
                RendTime = dateTimePicker4.Value.ToString("yyyy-MM-dd") + " 23:59:59";

            }
            string url = "http://boss.tao-liang.cn/api/fund/cashier/pay/boss/list?objType="+objType+"&objKey=&moneyBeg="+textBox7.Text+"&moneyEnd="+textBox8.Text+"&state="+state+"&type="+type+"&startTime="+startTime+"&endTime="+endTime+"&startRegisterTime="+ RstartTime + "&endRegisterTime="+ RendTime + "&page=1&limit="+textBox6.Text;
           
            string html = method.GetUrlWithCookie(url,COOKIE,"utf-8");
            Match page = Regex.Match(html, @"""pages"":([\s\S]*?),");
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
            
            if (checkBox2.Checked== true)
            {
                objType =0;
            }
            else  if (checkBox3.Checked == true)
            {
                objType = 1;
            }

            if (checkBox1.Checked == false)
            {
                startTime = dateTimePicker1.Value.ToString("yyyy-MM-dd") + " 00:00:00";
                endTime = dateTimePicker2.Value.ToString("yyyy-MM-dd") + " 23:59:59";
                RstartTime = dateTimePicker3.Value.ToString("yyyy-MM-dd") + " 00:00:00";
                RendTime = dateTimePicker4.Value.ToString("yyyy-MM-dd") + " 23:59:59";

            }

            try
            {


                for (int i = 1; i < page+1; i++)
                {

                    string url = "http://boss.tao-liang.cn/api/fund/cashier/pay/boss/list?objType=" + objType + "&objKey=&moneyBeg=" + textBox7.Text + "&moneyEnd=" + textBox8.Text + "&state=" + state + "&type=" + type + "&startTime=" + startTime + "&endTime=" + endTime + "&startRegisterTime=" + RstartTime + "&endRegisterTime=" + RendTime + "&page="+i+"&limit=" + textBox6.Text;
                    
                    string html = method.GetUrlWithCookie(url, COOKIE, "utf-8");

                    MatchCollection a1s = Regex.Matches(html, @"""createTime"":""([\s\S]*?)""");
                    MatchCollection a2s = Regex.Matches(html, @"""username"":""([\s\S]*?)""");
                    MatchCollection a3s = Regex.Matches(html, @"""registerTime"":""([\s\S]*?)""");
                    MatchCollection a4s = Regex.Matches(html, @"""orderno"":""([\s\S]*?)""");
                    MatchCollection a5s = Regex.Matches(html, @"""money"":([\s\S]*?),");
                    MatchCollection a6s = Regex.Matches(html, @"""state"":([\s\S]*?),");
                    MatchCollection a7s = Regex.Matches(html, @"""type"":([\s\S]*?),");
                    MatchCollection a8s = Regex.Matches(html, @"""coin"":([\s\S]*?),");

                    string fukuan = "";
                    string chongzhi = "";
                
                    if (a1s.Count == 0)
                    {
                        break;
                    }
                   
                    for (int j = 0; j < a1s.Count; j++)
                    {
                        if (a6s[j].Groups[1].Value == "0")
                        {
                            fukuan = "待支付";

                        }
                        else if (a6s[j].Groups[1].Value == "1")
                        {
                            fukuan = "支付成功";

                        }
                        if (a7s[j].Groups[1].Value == "-1")
                        {
                            chongzhi = "支付宝";
                        }
                        if (a7s[j].Groups[1].Value == "0")
                        {
                            chongzhi = "微信";
                        }
                        if (a7s[j].Groups[1].Value == "1")
                        {
                            chongzhi = "个人支付宝";
                        }
                        if (a7s[j].Groups[1].Value == "2")
                        {
                            chongzhi = "个人微信";
                        }
                        else
                        {
                            chongzhi = "个人支付宝";
                        }

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count+1).ToString()); //使用Listview展示数据      
                        lv1.SubItems.Add(a1s[j].Groups[1].Value);
                        lv1.SubItems.Add("*******"+a2s[j].Groups[1].Value.Substring(7));
                        lv1.SubItems.Add(a3s[j].Groups[1].Value);
                        lv1.SubItems.Add(a4s[j].Groups[1].Value);
                        lv1.SubItems.Add(a5s[j].Groups[1].Value);
                        lv1.SubItems.Add(fukuan);
                        lv1.SubItems.Add(chongzhi);
                        lv1.SubItems.Add(a8s[j].Groups[1].Value);




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
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
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

        private void button7_Click(object sender, EventArgs e)
        {
            COOKIE = method.getheader("http://boss.tao-liang.cn/api/member/boss/auth/login", "username=" + textBox1.Text.Trim() + "&password=" + textBox2.Text.Trim() + "&secret=boss.tao-liang.cn&");
            MessageBox.Show("登陆成功");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            zanting = true;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text == "全部")
            {
                state = -1;
            }
            if (comboBox2.Text == "待支付")
            {
                state = 0;
            }
            if (comboBox2.Text == "支付成功")
            {
                state = 1;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "全部")
            {
                type = -1;
            }
            if (comboBox1.Text == "支付宝")
            {
                type = 0;
            }
            if (comboBox1.Text == "微信")
            {
                type = 1;
            }
            if (comboBox1.Text == "个人支付宝")
            {
                type = 2;
            }
            if (comboBox1.Text == "个人微信")
            {
                type = 3;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
