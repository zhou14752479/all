using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
namespace 浙江号码匹配
{
    public partial class 湖南 : Form
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


        public 湖南()
        {
            InitializeComponent();
        }
        Thread thread;
        bool zanting = true;
        bool status = true;

        string codes = "";

        public string getcode()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < listView2.Items.Count; i++)
                {
                    if (listView2.Items[i].SubItems[1].Text != "")
                    {
                        sb.Append(listView2.Items[i].SubItems[1].Text+",");
                    }

                }

                string html= webBrowser1.Document.InvokeScript("getaes", new object[] { sb.ToString()}).ToString();
             
                return html;
            }
            catch (Exception ex)
            {
              
                return "";
            }
          
        }

        int key = 0;
        public void run()
        {
            if (ExistINIFile())
            {

                key = Convert.ToInt32(IniReadValue("values", "key"));

            }


            textBox1.Text = DateTime.Now.ToLongTimeString() + ": 开始查询";
            string[] text = codes.Split(new string[] { "," }, StringSplitOptions.None);
            for (int i = 0; i < text.Length-1; i++)
            {
                key = key - 1;
                if (key < 0)
                {
                    MessageBox.Show("打码积分不足");
                    return;
                }
                IniWriteValue("values", "key", key.ToString());
                try
                {
                    string name = listView2.Items[i].SubItems[0].Text;
                    string idcard = listView2.Items[i].SubItems[1].Text;
                    textBox1.Text = "正在查询：" + idcard;
                    string code = text[i];
                 
                    string url = "https://auth.zwfw.hunan.gov.cn/oauth2/yzCertificatenodo.jsp?certificate_no_input="+code+"&type=1";
                    string html = method.GetUrl(url,"utf-8");
                  
                    string tel = Regex.Match(html, @"""phone"":""([\s\S]*?)""").Groups[1].Value;

                    if (tel != "")
                    {

                        tel = method.Base64Decode(Encoding.Default, tel);
                    }
                    else
                    {
                        tel = html;
                    }
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(name);
                    lv1.SubItems.Add(idcard);
                    lv1.SubItems.Add(tel);
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    if (status == false)
                        return;
                   

                    Thread.Sleep(20);

                }
                catch (Exception ex)
                {
                   ex.ToString();
                    continue;

                }
            }    
        }
      
        private void button6_Click(object sender, EventArgs e)
        {
            //OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //openFileDialog1.Title = "打开excel文件";
            //// openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            //openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            //openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            //openFileDialog1.RestoreDirectory = true;
            //if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //{

            //    //打开文件对话框选择的文件
            //    textBox1.Text = "正在读取表格数据.......";
            //    DataTable dt = method.ExcelToDataTable(openFileDialog1.FileName, true);
            //    thread = new Thread(delegate () { method.ShowDataInListView(dt, listView2); });
            //    thread.Start();
            //    Control.CheckForIllegalCrossThreadCalls = false;

            //    textBox1.Text = "读取表格数据成功！";
            //    button1.Enabled = true;
            //}

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            // openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                //打开文件对话框选择的文件
                textBox1.Text = "正在读取表格数据.......";
                DataTable dt = method.ExcelToDataTable(openFileDialog1.FileName, true);

                method.ShowDataInListView(dt, listView2);

                textBox1.Text = "读取表格数据成功！";
                button1.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com:8080/api/vip.html", "utf-8");

            if (!html.Contains(@"SUhHyWW"))
            {
                MessageBox.Show("\"error:\":\"账户余额不足\"");
                return;
            }

            #endregion

         
            status = true;
            codes = getcode();
          
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
            //webBrowser1.Document.InvokeScript("getaes");
            //textBox1.Text= webBrowser1.Document.InvokeScript("getaes", new object[] { "430111198706162118,430111198706162118" }).ToString();

        }

        private void 湖南_Load(object sender, EventArgs e)
        {
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("http://47.102.145.207/hunan_tel/index.html");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 湖南_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

     

        private void button7_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", "http://www.houfaka.com/links/6277329890EA79FE");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string k1 = method.Base64Decode(Encoding.Default, textBox2.Text.Trim());
            string key22 = Regex.Match(k1, @"\d{5}").Groups[0].Value;

            if (key22 != "")
            {
                IniWriteValue("values", "key", key22.ToString());
                MessageBox.Show("成功");
                textBox2.Text = "";
            }
        }
    }
}
