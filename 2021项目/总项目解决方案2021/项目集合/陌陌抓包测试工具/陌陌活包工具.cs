using CsharpHttpHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 陌陌抓包测试工具
{
    public partial class 陌陌活包工具 : Form
    {
        public 陌陌活包工具()
        {
            InitializeComponent();
        }
        public static string getState(string URL)
        {
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = URL,//URL     必需项  
                Method = "HEAD",//URL     可选项 默认为Get  
                Timeout = 5000,
            };
      
            HttpResult result = http.GetHtml(item);
            return result.StatusCode.ToString();

        }

        #region 下载文件  【好用】
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="URLAddress">图片地址</param>
        /// <param name="subPath">图片所在文件夹</param>
        /// <param name="name">图片名称</param>
        public static void downloadFile(string URLAddress, string subPath, string name)
        {
            try
            {
                string path = System.IO.Directory.GetCurrentDirectory();

                WebClient client = new WebClient();
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
                //client.Headers.Add("Cookie", COOKIE);
                client.Headers.Add("Referer", "");
                if (false == System.IO.Directory.Exists(subPath))
                {
                    //创建pic文件夹
                    System.IO.Directory.CreateDirectory(subPath);
                }

                client.DownloadFile(URLAddress, subPath + "\\" + name);
            }
            catch (WebException ex)
            {

                ex.ToString();
            }
        }



        #endregion

        List<string> list = new List<string>();
        public void run()
        {
            try
            {
                for (int i = Convert.ToInt32(textBox5.Text); i <= Convert.ToInt32(textBox6.Text); i++)
                {
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    if (!list.Contains(i.ToString()))
                    {
                        list.Add(i.ToString());

                        string url = "";
                        if (comboBox1.Text == "两位版本")
                        {
                            url= string.Format("http://dl-ali.doki.ren/android/market/sem/momo_{0}.{1}_c{2}.apk", textBox2.Text.Trim(), textBox3.Text.Trim(), i);
                        }
                        if (comboBox1.Text == "三位版本")
                        {
                            url = string.Format("http://dl-ali.doki.ren/android/market/sem/momo_{0}.{1}.{2}_c{3}.apk", textBox2.Text.Trim(), textBox3.Text.Trim(), textBox4.Text.Trim(), i);
                        }
                        if (comboBox1.Text == "四位版本")
                        {
                            url = string.Format("http://dl-ali.doki.ren/android/market/sem/momo_{0}.{1}.{2}.{3}_c{4}.apk", textBox2.Text.Trim(), textBox3.Text.Trim(), textBox4.Text.Trim(), textBox7.Text.Trim(), i);
                        }

                    
                        string code = getState(url);
                        if (code == "OK")
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                            lv1.SubItems.Add(i.ToString());
                            lv1.SubItems.Add(url);
                            lv1.SubItems.Add("未下载");
                        }
                        textBox1.Text += url + "-->" + code + "\r\n";
                    }
                }
             


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString()); ;
            }
            button1.Text = "开始扫描";
        }
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (zanting == true)
            {
                if (thread == null || !thread.IsAlive)
                {
                    button1.Text = "停止扫描";
                    thread = new Thread(run);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                    Thread thread1 = new Thread(run);
                    thread1.Start();

                    Thread thread2= new Thread(run);
                    thread2.Start();
                }
                else
                {
                    button1.Text = "开始扫描";
                    zanting = false;
                }

                
            }
            else
            {
                button1.Text = "停止扫描";
                zanting = true;
            }
         
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.textBox1.SelectionStart = this.textBox1.Text.Length;
            this.textBox1.SelectionLength = 0;
            this.textBox1.ScrollToCaret();
        }


        bool zanting = true;
        private void button2_Click(object sender, EventArgs e)
        {
            Random rd = new Random(Guid.NewGuid().GetHashCode());

            Dictionary<string, string> dics = new Dictionary<string, string>(); ;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (!dics.ContainsKey(listView1.Items[i].SubItems[1].Text))
                {
                    dics.Add(listView1.Items[i].SubItems[1].Text, listView1.Items[i].SubItems[2].Text);
                }

            }

            listView1.Items.Clear();
            List<int> suijis = new List<int>();
            if (dics.Keys.Count > 2)
            {
                for (int j = 0; j < 5; j++)
                {

                    int suiji = rd.Next(1, dics.Keys.Count);
                    while (suijis.Contains(suiji))
                    {
                        suiji = rd.Next(1, dics.Keys.Count);
                        if (!suijis.Contains(suiji))
                        {
                            break;
                        }
                    }
                    suijis.Add(suiji);
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   

                    lv1.SubItems.Add(dics.ElementAt(suiji).Key);
                    lv1.SubItems.Add(dics.ElementAt(suiji).Value);
                    lv1.SubItems.Add("未下载");
                    lv1.Checked = true;
                }
            }
        }

        string path = AppDomain.CurrentDomain.BaseDirectory;
        public void download()
        {
            for (int i = 0; i < listView1.CheckedItems.Count; i++)
            {
                listView1.CheckedItems[i].SubItems[3].Text = "开始下载...";

                string url = listView1.Items[i].SubItems[2].Text;
                downloadFile(url, path+"//下载文件//", url.Replace("http://dl-ali.doki.ren/android/market/sem/", ""));

                listView1.CheckedItems[i].SubItems[3].Text = "下载完成";
            }

            
        }
        Thread t;
        private void button3_Click(object sender, EventArgs e)
        {
            if (t == null || !t.IsAlive)
            {
                t = new Thread(download);
                t.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
        private Label label = new Label();
        public string text = "北乔运营社-专注快速引流暴力变现";
        private void 陌陌抓包测试工具_Load(object sender, EventArgs e)
        {
          
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            string text = label8.Text;
            string text1 = text.Substring(1);
            string text2 = text1 + text[0];
            label8.Text = text2;
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Checked = true;

            }
        }
    }
}
