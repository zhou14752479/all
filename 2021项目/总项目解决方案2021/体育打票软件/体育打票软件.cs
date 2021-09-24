using gregn6Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 体育打票软件
{
    public partial class 体育打票软件 : Form
    {
        public 体育打票软件()
        {
            InitializeComponent();
        }



        #region ini读取
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

        #endregion








        string path = AppDomain.CurrentDomain.BaseDirectory;
        private GridppReport Report = new GridppReport();
        private void 体育打票软件_Load(object sender, EventArgs e)
        {
            if (ExistINIFile())
            {
                address_txt.Text = IniReadValue("values", "address");
                haoma_txt.Text = IniReadValue("values", "haoma");
                bianma_txt.Text = IniReadValue("values", "bianma");
            }



            //this.tabControl1.Region = new Region(new RectangleF(this.tabPage1.Left, this.tabPage1.Top, this.tabPage1.Width, this.tabPage1.Height));
            webBrowser1.ScriptErrorsSuppressed = true;
            //webBrowser1.Navigate("https://www.sporttery.cn/jc/jsq/zqhhgg/");
            webBrowser1.Navigate("https://trade.500.com/jczq/");


            //Report.LoadFromFile(@"C:\Grid++Report 6\Samples\Reports\1a.简单表格.grf");
            //Report.DetailGrid.Recordset.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;" +
            //    @"User ID=Admin;Data Source=C:\Grid++Report 6\\Samples\Data\Northwind.mdb";
        }


        function fc = new function();

        public static string html;
        public static string ahtml;
        public static string suiji;

        public void gethtml()
        {
            //var btns = webBrowser1.Document.GetElementsByTagName("input");
            //foreach (HtmlElement btn in btns)
            //{
            //    if (btn.GetAttribute("id") == "detailBtn")
            //    {
            //        btn.InvokeMember("click");
            //    }
            //}

            
        
            html = webBrowser1.Document.Body.OuterHtml;
            ahtml = webBrowser1.DocumentText;
            if (webBrowser1.Url.ToString().Contains("500.com"))
            {
                StreamReader sr = new StreamReader(webBrowser1.DocumentStream, Encoding.GetEncoding(("gb2312")));
                ahtml = sr.ReadToEnd();
            }
            textBox1.Text = html;
        }


   

        private void button1_Click(object sender, EventArgs e)
        {
            button2.BackColor = Color.LightGray;
            button1.BackColor = Color.White;
            tabControl1.SelectedIndex = 0;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.BackColor = Color.LightGray;
            button2.BackColor=Color.White;
            tabControl1.SelectedIndex = 1;

        }

        private void button7_Click(object sender, EventArgs e)
        {
           
           
           

            webBrowser1.Refresh();
        }
        
        private void button4_Click(object sender, EventArgs e)
        {
           
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://www.sporttery.cn/jc/jsq/zqhhgg/");

            //List<Task> TaskList = new List<Task>();

            //for (int i = 0; i < 100; i++)
            //{
            //    TaskList.Add(
            //        Task.Factory.StartNew(() =>
            //        {
            //            BeginInvoke(new Action(() =>
            //            {
            //                textBox1.Text += i + "\r\n";

            //            }));
            //        })
            //    );
            //}
          
            
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://trade.500.com/jczq/");
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://trade.500.com/jclq/index.php?playid=274&g=2");
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://www.sporttery.cn/jc/jsq/zqhhgg/");
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://www.sporttery.cn/jc/jsq/lqsf/");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            #region 通用检测

            if (!function.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"FCUoF"))
            {
                return;
            }

            #endregion
            suiji = bianma_txt.Text + function.getsuijima();
            gethtml();
            if (webBrowser1.Url.ToString().Contains("500"))
            {
                string fangshi = "解析模式：500网";
                jiexi jx = new jiexi();
                jx.Show();
                jx.Text = fangshi;
               
                   
                
            }
            else
            {

                string fangshi = "解析模式：竞彩" + Regex.Match(ahtml, @"<title>([\s\S]*?)</title>").Groups[1].Value;
                if (!fangshi.Contains("混合过关"))
                {
                    MessageBox.Show("请将页面切换至混合过关");
                }
                else
                {
                    jiexi jx = new jiexi();
                    jx.Text = fangshi;
                    jx.Show();
                }
            }

        }

        public void cishi(string value)
        {
            MessageBox.Show(value);
        }

        private void button5_Click(object sender, EventArgs e)
        { // Report.Print(true);
            //Report.PrintPreview(true);

            #region 通用检测

            if (!function.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"FCUoF"))
            {
                return;
            }

            #endregion

           

            gethtml();
            if (webBrowser1.Url.ToString().Contains("500"))
            {
                try
                {
                    HtmlElement element2 = webBrowser1.Document.CreateElement("script");
                    element2.SetAttribute("type", "text/javascript");
                    element2.SetAttribute("text", "function _func(){return document.getElementById('buy_bs').value}");   //这里写JS代码
                    HtmlElement head = webBrowser1.Document.Body.AppendChild(element2);
                    fc.beishu_500 = webBrowser1.Document.InvokeScript("_func").ToString();
                }
                catch (Exception)
                {
                    HtmlElement element2 = webBrowser1.Document.CreateElement("script");
                    element2.SetAttribute("type", "text/javascript");
                    element2.SetAttribute("text", "function _func(){k=document.getElementsByTagName('input');for (l = 0; l < k.length; l++){if (k[l].type == 'text') { return k[l].value; }}}");   //这里写JS代码
                    HtmlElement head = webBrowser1.Document.Body.AppendChild(element2);
                    fc.beishu_500 = webBrowser1.Document.InvokeScript("_func").ToString();

                   
                }
                fc.getdata_500(Report, html, ahtml);
            }

            else
            {
                string fangshi = Regex.Match(ahtml, @"<title>([\s\S]*?)</title>").Groups[1].Value;
                if (fangshi == "足球混合过关")
                {
                    fc.getdata(Report, html, ahtml);
                }
                if (fangshi == "足球胜平负" || fangshi == "足球半全场胜平负" || fangshi == "足球总进球数" || fangshi == "足球比分")
                {
                    fc.getdata_shengpingfu(Report, html, ahtml);
                }
            }

           




            PreviewForm theForm = new PreviewForm();
            theForm.AttachReport(Report);
            theForm.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            IniWriteValue("values", "address", address_txt.Text.ToString());
            IniWriteValue("values", "haoma", haoma_txt.Text.ToString());
            IniWriteValue("values", "bianma", bianma_txt.Text.ToString());
            MessageBox.Show("保存成功","保存提示");

        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void button5_MouseHover(object sender, EventArgs e)
        {
           
        }

        private void button5_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                var btn = webBrowser1.Document.GetElementById("detailBtn");
                btn.InvokeMember("click");

              
            }
            catch (Exception)
            {
                var btn2 = webBrowser1.Document.GetElementById("panelSelectBtn");
                btn2.InvokeMember("click");
            }
        }
    }
}
