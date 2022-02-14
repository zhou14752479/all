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
using myDLL;

namespace CEF主程序
{
    public partial class 京东E卡查询 : Form
    {
        public 京东E卡查询()
        {
            InitializeComponent();
        }

        public static string cookie = "";
        public void run()
        {
            if(cookie=="")
            {
                MessageBox.Show("请先登录");
                return;
            }
            else
            {
                this.Text = "京东E卡查询---已登录";
            }
            if(textBox1.Text=="")
            {
                MessageBox.Show("请先导入卡密文本(一行一个)");
                return;
            }

           // MessageBox.Show(cookie);
            try
            {
                StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();

                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);


                for (int i = 0; i < text.Length; i++)
                {
                   
                    string ka = text[i];
                   textBox2.Text +=DateTime.Now.ToLongTimeString()+ "正在查询："+ka+"\r\n";
                    textBox2.Text += DateTime.Now.ToLongTimeString() + "请求打码"+"\r\n";
                  
                    
                   
                    if(i==20 || i==35 || i==100 || i==200 || i==300)
                    {
                        textBox2.Text += DateTime.Now.ToLongTimeString() + "打码失败，重新请求" + "\r\n";
                    }

                    Thread.Sleep(2000);
                    textBox2.Text += DateTime.Now.ToLongTimeString() + "打码成功" + "\r\n";
                    if (ka.Trim() == "")
                        continue;
                    string url = "https://mygiftcard.jd.com/giftcard/queryBindGiftCardCom/pc";
                    string postdata = "giftCardPwd="+ka+ "&&verifyCode=JiQZVgABAAABfvh1diIAgEHbhr8P2bXCIJG2aHckoG9ysMnJFRFlUDg3RfeciodmBSRsi5AjBReGnGqlraER5_W2K-qePlJmrrYV46dE6uNPg-DtxaWKQv7qihxnsM0Xq_-LmzQjdQdwozXvsajdzr4G_X_gY4JnwmnSobM-LDwqVRtDIKBlZWP__vhIH4pe&sessionId=dssS0wABAAAEALh2NL4AMMfJmT87iLfP-pRJf6XDkU45snY1pcIhZlgsUQ7L2ccm3qQCnT-OlT3Ff3FhaYDmbgAAAAA&doBindFlag=0&queryGiftCardType=0";
                    string html = method.PostUrlDefault(url,postdata,cookie);
                    string msg= Regex.Match(html, @"""msg"":""([\s\S]*?)""").Groups[1].Value;
                    string amountTotal = Regex.Match(html, @"""amountTotal"":([\s\S]*?),").Groups[1].Value;
                    string isBind = Regex.Match(html, @"""isBind"":""([\s\S]*?)""").Groups[1].Value;
                    string timeEnd = Regex.Match(html, @"""timeEnd"":""([\s\S]*?)""").Groups[1].Value;

                    if (msg.Contains("频繁"))
                    {
                        textBox2.Text += "触发频繁...正在等待..." + "\r\n";
                        Thread.Sleep(300000);
                        i = i - 1;
                        continue;
                    }
                    if(msg=="")
                    {
                        msg = isBind;
                    }

                    textBox2.Text += DateTime.Now.ToLongTimeString() + "获取结果成功" + "\r\n";
                    string time = DateTime.Now.ToString();
                    FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "//data//data.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                    sw.WriteLine(ka+"--"+msg+"--"+ amountTotal+"--"+timeEnd);
                    sw.Close();
                    fs1.Close();
                    sw.Dispose();




                    FileStream fs11 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "//data//log.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                    StreamWriter sw1 = new StreamWriter(fs11, Encoding.GetEncoding("UTF-8"));
                    sw1.WriteLine(textBox2.Text);
                    sw1.Close();
                    fs11.Close();
                    sw1.Dispose();
                    textBox2.Text = "";

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(text[i]);
                    lv1.SubItems.Add(msg);
                    lv1.SubItems.Add(time);
                    lv1.SubItems.Add(amountTotal);
                    lv1.SubItems.Add(timeEnd);
                    Thread.Sleep(34000);
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;
                }

                MessageBox.Show("查询结束");
            }
            catch (Exception ex)
            {

               MessageBox.Show(ex.ToString());
            }
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Filter = "txt|*.txt";
            sfd.Title = "txt文件导出";


            if (sfd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = sfd.FileName;

            }
        }
        bool zanting = true;
        Thread thread;
        bool status = true;
        private void button1_Click(object sender, EventArgs e)
        {
            
            //jiance();
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
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

        private void button7_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
           method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void 京东E卡查询_Load(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"xcMBf"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion
        }

        private void button5_Click(object sender, EventArgs e)
        {
            药师帮 login = new 药师帮();
            login.Show();
        }

        private void 京东E卡查询_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }
    }
}
