using System;
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

namespace 主程序202012
{
    public partial class 网址JSON检测 : Form
    {
        public 网址JSON检测()
        {
            InitializeComponent();
        }

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url)
        {


            try
            {
                string outStr = "";
                string tmpStr = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                request.KeepAlive = true;

                // request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip");
                // request.Headers.Set(HttpRequestHeader.AcceptLanguage, "zh-cn,zh,en");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                                                                                                                     // request.Accept = "*/*";

                try
                {//循环获取
                    while ((tmpStr = reader.ReadLine()) != null)
                    {
                        outStr += tmpStr;
                    }
                }
                catch
                {

                }
                reader.Close();
                response.Close();
                return outStr;



            }
            catch (System.Exception ex)
            {
                ex.ToString();

            }
            return "";
        }
        #endregion

        string cookie = "CGIC=IocBdGV4dC9odG1sLGFwcGxpY2F0aW9uL3hodG1sK3htbCxhcHBsaWNhdGlvbi94bWw7cT0wLjksaW1hZ2UvYXZpZixpbWFnZS93ZWJwLGltYWdlL2FwbmcsKi8qO3E9MC44LGFwcGxpY2F0aW9uL3NpZ25lZC1leGNoYW5nZTt2PWIzO3E9MC45; NID=204=OdpZQ9gANpvjnRZFWet5Tkww_IY8qk_lT63aGKwkl9nnSAGOR9l4t-oz088Ut55EJLoWd9nBfB54qxAM1lT1sqtSTOfBR7ptIDwGxZVFN-8acKoY4N32WZ1aAFE3ynDUUiy08nQuTkXI7xIvjkziSgdvXt6kESpVbcA3-OWURwis4y89ZD0atmRCMQ; ANID=AHWqTUk2Ed6c_pjHYsrzJoRiFNPrmLbvIrT-9ujmGZoJdf__BASq9n4M9DAsVozO; 1P_JAR=2020-12-14-07; DV=o0TEfyqmFr0l4OgbEydFW6hJYhIEZpeKpsw9cz05AgIAAAA";
        private void 网址JSON检测_Load(object sender, EventArgs e)
        {

        }
        Thread thread;
        bool zanting = true;
        bool status = true;
        public void run()
        {
           
            if (textBox1.Text == "")
            {
                MessageBox.Show("请导入csv");
                return;
            }
            StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();

            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int i = 1; i < text.Length; i++)
            {

                if (text[i] != "")
                {
                    string keyword = text[i].Replace(" ", "+");
                    label3.Text = "正在查询：" + text[i];
                   
                    string url = "https://www.google.com.hk/search?q=" + keyword.Trim() + "&safe=strict&source=lnms&tbm=shop&ved=2ahUKEwjg6tWP5cXtAhXMl54KHYk3DPUQ_AUoA3oECCcQBQ";
                    string html = method.GetUrlWithCookie(url, cookie, "utf-8");
                   // string html = GetUrl(url);
                    MatchCollection aids = Regex.Matches(html, @"<a class=""shntl sh-np__click-target"" data-merchant-id=([\s\S]*?)ai=([\s\S]*?)&");
                   
                    foreach (Match aid in aids)
                    {


                        string aurl = "https://www.google.com.hk/aclk?sa=L&ai=" + aid.Groups[2].Value;
                        string ahtml = method.GetUrl(aurl, "utf-8");
                       // string ahtml = GetUrl(aurl);
                        Match ym = Regex.Match(ahtml, @"<a href=""([\s\S]*?)//([\s\S]*?)/");
                        string yuming = ym.Groups[1].Value + "//" + ym.Groups[2].Value + "/products.json";
                        string yumingwithoutjson = ym.Groups[1].Value + "//" + ym.Groups[2].Value;
                        string strhtml = GetUrl(yuming);
                        if (strhtml.Contains("\":\"") && !strhtml.Contains("<html"))
                        {
                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据

                            lv1.SubItems.Add(yumingwithoutjson);
                            lv1.SubItems.Add("1");
                        }
                        else
                        {
                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据

                            lv1.SubItems.Add(yumingwithoutjson);
                            lv1.SubItems.Add("2");
                        }

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;
                    }

                }
            }
            MessageBox.Show("查询结束");



        }



        public void run1()
        {
            
            if (textBox2.Text == "")
            {
                MessageBox.Show("请导入表格");
                return;
            }
            DataTable dt = method.ExcelToDataTable(textBox2.Text,true);



            foreach (DataRow row in dt.Rows)
            {

                try
                {

   

                string url = row[0].ToString();
               
                if (url != "")
                {
                   
                    label3.Text = "正在查询：" + url;


                        Match ym = null;
                        string yuming = "";
                        string yumingwithoutjson = "";
                        if (url.Contains("https"))
                        {
                            ym = Regex.Match(url, @"https://([\s\S]*?)/");
                            yuming = "https://"+ym.Groups[1].Value + "/products.json";

                            yumingwithoutjson = "https://" + ym.Groups[1].Value;
                        }
                        else
                        {
                            ym = Regex.Match(url, @"http://([\s\S]*?)/");
                            yuming = "https://" + ym.Groups[1].Value + "/products.json";
                            yumingwithoutjson = "http://" + ym.Groups[1].Value;

                        }

                       
                        
                        string strhtml = GetUrl(yuming);
                        if (strhtml.Contains("\":\"") && !strhtml.Contains("<html"))
                        {
                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据

                            lv1.SubItems.Add(yumingwithoutjson);
                            lv1.SubItems.Add("1");
                        }
                        else
                        {
                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据

                            lv1.SubItems.Add(yumingwithoutjson);
                            lv1.SubItems.Add("2");
                        }

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;
                    

                }

                }
                catch (Exception ex)
                {

                    continue;
                }
            }
            MessageBox.Show("查询结束");



        }
        private void button6_Click(object sender, EventArgs e)
        {
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                // timer1.Start();
                if (textBox1.Text != "")
                {
                    thread = new Thread(run);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }

                else if (textBox2.Text != "")
                {
                    thread = new Thread(run1);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {
                zanting = true;
                button1.Text = "暂停";
            }
            else
            {
                zanting = false;
                button1.Text = "继续";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
            //method.ListViewToCSV(listView1,true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
                textBox2.Text = "";
            }
        }

        private void 网址JSON检测_FormClosing(object sender, FormClosingEventArgs e)
        {
            JSON检测登录.xiugai(JSON检测登录.user, "0");
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(listView1.SelectedItems.Count>0)
            {
                System.Diagnostics.Process.Start(listView1.SelectedItems[0].SubItems[1].Text);
            }
           
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要退出吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                JSON检测登录.xiugai(JSON检测登录.user, "0");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
              
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
           
            if (flag)
            {
              
                this.textBox2.Text = this.openFileDialog1.FileName;
                textBox1.Text = "";
            }
        }
    }
}
