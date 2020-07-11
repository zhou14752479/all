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
using helper;

namespace 主程序202006
{
    public partial class 飞机票信息记录 : Form
    {
        public 飞机票信息记录()
        {
            InitializeComponent();
        }

        int yijing = 0;
        int hege = 0;
        ArrayList finish = new ArrayList();

        ArrayList lists = new ArrayList();
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {

            long min = Convert.ToInt64(textBox1.Text.Replace("\r\n", "").Trim());
            long max = Convert.ToInt64(textBox1.Text.Replace("\r\n", "").Trim()) +  Convert.ToInt64(textBox2.Text.Replace("\r\n", "").Trim()) ;

            label7.Text = textBox2.Text.Replace("\r\n","").Trim(); ;
            for (long i =min; i < max; i++)
            {
                if (!finish.Contains(i))
                {
                    finish.Add(i);
                    yijing = yijing + 1;
                    label8.Text = yijing.ToString();

                    try
                    {


                        string url = "http://api.fly517.com:889/pidservicedetr.asmx/DetrJsonQueryId?UserAct=" + textBox4.Text.Replace("\r\n", "").Trim() + "&UserPwd=" + textBox5.Text.Replace("\r\n", "").Trim() + "&UserKey=" + textBox6.Text.Replace("\r\n", "").Trim() + "&InstructionType=TN&InstructionValue=" + i+ "&QueryId=";

                        string html = GetUrl(url, "utf-8");

                        Match a1 = Regex.Match(html, @"""PASSENGER"":""([\s\S]*?)""");
                        Match a2 = Regex.Match(html, @"""DETRTN"":""([\s\S]*?)""");
                        Match a3 = Regex.Match(html, @"""AirFromCode"":""([\s\S]*?)""");
                        Match a4 = Regex.Match(html, @"""AirToCode"":""([\s\S]*?)""");
                        Match a5 = Regex.Match(html, @"""AirCode"":""([\s\S]*?)""");
                        Match a6 = Regex.Match(html, @"""AirFlightNo"":""([\s\S]*?)""");
                        Match a7 = Regex.Match(html, @"""AirSeat"":""([\s\S]*?)""");
                        Match a8 = Regex.Match(html, @"""AirDate"":""([\s\S]*?)""");
                        Match a9 = Regex.Match(html, @"""AirTime"":""([\s\S]*?)""");
                        Match a10 = Regex.Match(html, @"""AirTicketStatus"":""([\s\S]*?)""");

                        if (a1.Groups[1].Value != "")
                        {
                            hege = hege + 1;
                            label9.Text = hege.ToString();
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count+1).ToString()); //使用Listview展示数据   
                            lv1.SubItems.Add(a1.Groups[1].Value.Replace(":", "").Trim());
                            lv1.SubItems.Add(a2.Groups[1].Value);
                            lv1.SubItems.Add(a3.Groups[1].Value);
                            lv1.SubItems.Add(a4.Groups[1].Value);
                            lv1.SubItems.Add(a5.Groups[1].Value + a6.Groups[1].Value);
                            lv1.SubItems.Add(a7.Groups[1].Value);
                            lv1.SubItems.Add(a8.Groups[1].Value);
                            lv1.SubItems.Add(a9.Groups[1].Value);
                            lv1.SubItems.Add(a10.Groups[1].Value);
                        }
                    }
                    catch
                    {

                        continue;
                    }



                }


            }
            label10.Text = "查询完成";



        }
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
               
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
              
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
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
        /// 主程序1
        /// </summary>
        public void run1()
        {

            StreamReader streamReader = new StreamReader(filename, Encoding.Default);
            string text = streamReader.ReadToEnd();
            string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            label7.Text = array.Length.ToString();
            for (int i = 0; i < array.Length; i++)
            {

                if (!finish.Contains(array[i]))
                {
                    finish.Add(array[i]);
                
                    yijing = yijing + 1;
                    label8.Text = yijing.ToString();

                    try
                    {


                        string url = "http://api.fly517.com:889/pidservicedetr.asmx/DetrJsonQueryId?UserAct=" + textBox4.Text.Replace("\r\n","").Trim()+"&UserPwd="+textBox5.Text.Replace("\r\n", "").Trim()+"&UserKey="+textBox6.Text.Replace("\r\n", "").Trim()+"&InstructionType=TN&InstructionValue=" + array[i].Replace("\r\n", "").Trim()+ "&QueryId=";
                        
                        string html = GetUrl(url, "utf-8");
                      
                        Match a1 = Regex.Match(html, @"""PASSENGER"":""([\s\S]*?)""");
                        Match a2 = Regex.Match(html, @"""DETRTN"":""([\s\S]*?)""");
                        Match a3 = Regex.Match(html, @"""AirFromCode"":""([\s\S]*?)""");
                        Match a4 = Regex.Match(html, @"""AirToCode"":""([\s\S]*?)""");
                        Match a5 = Regex.Match(html, @"""AirCode"":""([\s\S]*?)""");
                        Match a6 = Regex.Match(html, @"""AirFlightNo"":""([\s\S]*?)""");
                        Match a7 = Regex.Match(html, @"""AirSeat"":""([\s\S]*?)""");
                        Match a8 = Regex.Match(html, @"""AirDate"":""([\s\S]*?)""");
                        Match a9 = Regex.Match(html, @"""AirTime"":""([\s\S]*?)""");
                        Match a10 = Regex.Match(html, @"""AirTicketStatus"":""([\s\S]*?)""");

                        if (a1.Groups[1].Value != "")
                        {
                            hege = hege + 1;
                            label9.Text = hege.ToString();
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                            lv1.SubItems.Add(a1.Groups[1].Value.Replace(":", "").Trim());
                            lv1.SubItems.Add(a2.Groups[1].Value);
                            lv1.SubItems.Add(a3.Groups[1].Value);
                            lv1.SubItems.Add(a4.Groups[1].Value);
                            lv1.SubItems.Add(a5.Groups[1].Value + a6.Groups[1].Value);
                            lv1.SubItems.Add(a7.Groups[1].Value);
                            lv1.SubItems.Add(a8.Groups[1].Value);
                            lv1.SubItems.Add(a9.Groups[1].Value);
                            lv1.SubItems.Add(a10.Groups[1].Value);
                        }
                    }
                    catch
                    {

                        continue;
                    }



                }


            }
            label10.Text = "查询完成";



        }
        private void 飞机票信息记录_Load(object sender, EventArgs e)
        {
            foreach (Control ctr in splitContainer1.Panel1.Controls)
            {

                if (ctr is TextBox)
                {

                    string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
                    if (File.Exists(path + ctr.Name + ".txt"))
                    {

                        StreamReader sr = new StreamReader(path + ctr.Name + ".txt", Encoding.GetEncoding("utf-8"));
                        //一次性读取完 
                        string texts = sr.ReadToEnd();
                        ctr.Text = texts;
                        sr.Close();
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"fly517"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion


            if (filename=="")
            {
                for (int i = 0; i < Convert.ToInt32(textBox3.Text.Replace("\r\n", "").Trim()); i++)
                {

                    Thread thread = new Thread(new ThreadStart(run));
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;

                }
            }
            else
            {
                for (int i = 0; i < Convert.ToInt32(textBox3.Text.Replace("\r\n", "").Trim()); i++)
                {

                    Thread thread = new Thread(new ThreadStart(run1));
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;

                }
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        string filename = "";
        private void button2_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                filename = this.openFileDialog1.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            filename = "";
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void 飞机票信息记录_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确认退出吗？", "退出询问"
           , MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK)
            {
                e.Cancel = true;//告诉窗体关闭这个任务取消

            }
            else
            {
                foreach (Control ctr in splitContainer1.Panel1.Controls)
                {
                    if (ctr is TextBox)
                    {


                        string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
                        FileStream fs1 = new FileStream(path + ctr.Name + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                        StreamWriter sw = new StreamWriter(fs1);
                        sw.WriteLine(ctr.Text);
                        sw.Close();
                        fs1.Close();

                    }
                }

  
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            method.ListViewToCSV(listView1, true);
        }
    }
}
