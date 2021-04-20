using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsharpHttpHelper;
using myDLL;

namespace 主程序202102
{
    public partial class 韵达快递查询 : Form
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.inipath);
        }

        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
            return temp.ToString();
        }

        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public bool ExistINIFile()
        {
            return File.Exists(inipath);
        }
        public 韵达快递查询()
        {
            InitializeComponent();
        }


        string cookie = "";


        public string geturlwithcookie(string url)
        {
            
              HttpHelper http = new HttpHelper();
               HttpItem item = new HttpItem()
                {
                    URL = url,
                    Method = "GET",
                    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.90 Safari/537.36",
                    Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9",
                    Cookie = cookie,
                    Host = "travel.yundasys.com:31432",
                };
                item.Header.Add("Accept-Encoding", "gzip, deflate");
                item.Header.Add("Accept-Language", "zh,sq;q=0.9,zh-CN;q=0.8,oc;q=0.7,de;q=0.6,en;q=0.5");
               HttpResult result = http.GetHtml(item);
                string html = result.Html;
                return html;
            

        }


        public string getinfos(string billid)
        {


            string html = geturlwithcookie("http://travel.yundasys.com:31432/interface/ld?txm=" + billid);
           
            MatchCollection areas = Regex.Matches(html, @"城区：([\s\S]*?)&");

            MatchCollection addrs = Regex.Matches(html, @">地址：([\s\S]*?)<");

           
            if (addrs.Count > 0)
            {
                string area = areas[areas.Count - 1].Groups[1].Value;
                string addr = addrs[addrs.Count - 1].Groups[1].Value;

                return area + "|" + addr;
            }
            else
            {
                return  "|";
            }



        }

        public string gethaoma(string billid)
        {

            string html = geturlwithcookie("http://travel.yundasys.com:31432/interface/orderPhone?txm=" + billid);
          
            string name = Regex.Match(html, @"<data>([\s\S]*?)</data>").Groups[1].Value;
          
            return name;

        }

        public string getsanduan(string billid)
        {

            string html = geturlwithcookie("http://travel.yundasys.com:31432/interface/tmfp?txm=" + billid);
            string three_codes = Regex.Match(html, @"<three_codes>([\s\S]*?)</three_codes>").Groups[1].Value;

            return three_codes;

        }

        #region 订单信息
        public void dingdan()
        {


            string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string item in text)
            {

                string html = getinfos(item.Trim());

                string ahtml = gethaoma(item.Trim());
                string bhtml = getsanduan(item.Trim());

                string[] text1 = html.Split(new string[] { "|" }, StringSplitOptions.None);
                string[] text2 = ahtml.Split(new string[] { "|" }, StringSplitOptions.None);
                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                try
                {
                   
                    lv1.SubItems.Add(item);
                    lv1.SubItems.Add(text1[0]);
                    lv1.SubItems.Add(text1[1]);
                    lv1.SubItems.Add(bhtml);

                    lv1.SubItems.Add(text2[0]); //姓名
                    lv1.SubItems.Add(text2[1]);  //电话
                   
                }
                catch (Exception)
                {

                    lv1.SubItems.Add("");

                }
                Thread.Sleep(1000);
                while (this.zanting == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }
                if (status == false)
                    return;

            }

        }

        #endregion

        Thread thread;

        bool zanting = true;
        bool status = true;
      
        private void button3_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;

            try
            {
                StreamReader sr = new StreamReader(path + "cookie.txt", method.EncodingType.GetTxtType(path + "cookie.txt"));
                //一次性读取完 
                string texts = sr.ReadToEnd();

                cookie = Regex.Match(texts, @"cookie=([\s\S]*?)&").Groups[1].Value;
                textBox2.Text = cookie;
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


          
           
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(dingdan);
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

        private void button6_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            listView1.Items.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            Process.Start(path + "helper.exe");
        }

        private void 韵达快递查询_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void 韵达快递查询_Load(object sender, EventArgs e)
        {

        }
    }
}
