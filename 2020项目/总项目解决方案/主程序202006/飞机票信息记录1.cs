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
    public partial class 飞机票信息记录1 : Form
    {
        public 飞机票信息记录1()
        {
            InitializeComponent();
        }

        private void 飞机票信息记录1_Load(object sender, EventArgs e)
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
            long max = Convert.ToInt64(textBox1.Text.Replace("\r\n", "").Trim()) + Convert.ToInt64(textBox2.Text.Replace("\r\n", "").Trim());

            label7.Text = textBox2.Text.Replace("\r\n", "").Trim(); ;
            for (long i = min; i < max; i++)
            {
                if (!finish.Contains(i))
                {
                    finish.Add(i);
                    yijing = yijing + 1;
                    label8.Text = yijing.ToString();

                    try
                    {


                        string url = "http://api.camucz.com:889/pidservicedetr.asmx/DetrJsonQueryId?UserAct=" + textBox4.Text.Replace("\r\n", "").Trim() + "&UserPwd=" + textBox5.Text.Replace("\r\n", "").Trim() + "&UserKey=" + textBox6.Text.Replace("\r\n", "").Trim() + "&InstructionType=TN&InstructionValue=" + i + "%2CF&QueryId=";

                        string html = GetUrl(url, "utf-8");

                        Match a1 = Regex.Match(html, @"""TKTN"":""([\s\S]*?)""");
                        Match a2 = Regex.Match(html, @"""NAME"":""([\s\S]*?)""");
                        Match a3 = Regex.Match(html, @"""RCPT"":""([\s\S]*?)""");
                       

                        if (a1.Groups[1].Value != "")
                        {
                            hege = hege + 1;
                            label9.Text = hege.ToString();
                            if (a3.Groups[1].Value == "")
                            {
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add(a1.Groups[1].Value.Replace(":", "").Trim());
                                lv1.SubItems.Add(a2.Groups[1].Value);
                                lv1.SubItems.Add(a3.Groups[1].Value);
                            }
                           
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


                        string url = "http://api.camucz.com:889/pidservicedetr.asmx/DetrJsonQueryId?UserAct=" + textBox4.Text.Replace("\r\n", "").Trim() + "&UserPwd=" + textBox5.Text.Replace("\r\n", "").Trim() + "&UserKey=" + textBox6.Text.Replace("\r\n", "").Trim() + "&InstructionType=TN&InstructionValue=" + array[i].Replace("\r\n", "").Trim() + "%2CF&QueryId=";

                        string html = GetUrl(url, "utf-8");

                        Match a1 = Regex.Match(html, @"""TKTN"":""([\s\S]*?)""");
                        Match a2 = Regex.Match(html, @"""NAME"":""([\s\S]*?)""");
                        Match a3 = Regex.Match(html, @"""RCPT"":""([\s\S]*?)""");

                        if (a1.Groups[1].Value != "")
                        {
                            hege = hege + 1;
                            label9.Text = hege.ToString();
                            if (a3.Groups[1].Value == "")
                            {
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add(a1.Groups[1].Value.Replace(":", "").Trim());
                                lv1.SubItems.Add(a2.Groups[1].Value);
                                lv1.SubItems.Add(a3.Groups[1].Value);
                            }
                         
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
        private void button3_Click(object sender, EventArgs e)
        {
            if (filename == "")
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
        string filename = "";
        private void button2_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                filename = this.openFileDialog1.FileName;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            method.ListViewToCSV(listView1, true);
        }
        #region 导出文本
        public static void expotTxt(ListView lv1)
        {

            string path = AppDomain.CurrentDomain.BaseDirectory + "导出结果\\";


            foreach (ListViewItem item in lv1.Items)
            {

                List<string> list = new List<string>();

                string temp1 = item.SubItems[1].Text;
                string temp2 = item.SubItems[2].Text;
                string temp3 = item.SubItems[3].Text;
             
                string value = temp1 + "---" + temp2 + "---" + temp3 ;

                if (temp3 == "")
                {

                    if (!File.Exists(path + DateTime.Now.ToString("yyyy-MM-dd") + "RP为空.txt"))
                    {
                        FileStream fs1 = new FileStream(path + DateTime.Now.ToString("yyyy-MM-dd")+"RP为空.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                        StreamWriter sw = new StreamWriter(fs1);
                        sw.WriteLine(value);
                        sw.Close();
                        fs1.Close();
                    }
                    else
                    {
                        StreamWriter fs = new StreamWriter(path + DateTime.Now.ToString("yyyy-MM-dd") + "RP为空.txt", true);
                        fs.WriteLine(value);
                        fs.Close();
                    }
                }
                else
                {
                    if (!File.Exists(path + DateTime.Now.ToString("yyyy-MM-dd") + "RP不为空.txt"))
                    {
                        FileStream fs1 = new FileStream(path + DateTime.Now.ToString("yyyy-MM-dd")+ "RP不为空.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                        StreamWriter sw = new StreamWriter(fs1);
                        sw.WriteLine(value);
                        sw.Close();
                        fs1.Close();
                    }
                    else
                    {
                        StreamWriter fs = new StreamWriter(path + DateTime.Now.ToString("yyyy-MM-dd") + "RP不为空.txt", true);
                        fs.WriteLine(value);
                        fs.Close();
                    }

                }


            }
            MessageBox.Show("导出完成");


        }
        #endregion
        private void button7_Click(object sender, EventArgs e)
        {
            expotTxt(listView1);
        }
    }
}
