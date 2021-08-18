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



            this.tabControl1.Region = new Region(new RectangleF(this.tabPage1.Left, this.tabPage1.Top, this.tabPage1.Width, this.tabPage1.Height));
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("https://www.sporttery.cn/jc/jsq/zqhhgg/");



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

             html = webBrowser1.Document.Body.OuterHtml;
            ahtml = webBrowser1.DocumentText;
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
            suiji = bianma_txt.Text + function.getsuijima();
            gethtml();
            jiexi jx = new jiexi();
            jx.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            #region 通用检测

            if (!function.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"FCUoF"))
            {
                return;
            }

            #endregion

            gethtml();
            fc.getdata(Report,html,ahtml);

            // Report.Print(true);
            //Report.PrintPreview(true);
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
    }
}
