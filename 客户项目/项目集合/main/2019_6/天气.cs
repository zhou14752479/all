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

namespace main._2019_6
{
    public partial class 天气 : Form
    {
        public 天气()
        {
            InitializeComponent();
        }

        #region 采集
        public void run()
        {
            
            try
            {
               
                StreamReader sr = new StreamReader(textBox1.Text, Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string[] months = {

                    "201712",
"201801",
"201802",
"201803",
"201804",
"201805",
"201806",
"201807",
"201808",
"201809",
"201810",
"201811",
"201812",
"201901",
"201902",
"201903",
"201904",
"201905",
"201906"

                };
                for (int i = 0; i < text.Length; i++)
                {
                    foreach (string month in months)
                    {
                        string Url = "http://tianqi.2345.com/t/wea_history/js/"+month+"/" + text[i] + "_"+month+".js";

                        string html = method.gethtml(Url, "", "gb2312");
                        if (html == null)
                            break;
                        Match city = Regex.Match(html, @"city:'([\s\S]*?)'");

                        MatchCollection a1 = Regex.Matches(html, @"ymd:'([\s\S]*?)'", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        MatchCollection a2 = Regex.Matches(html, @",bWendu:'([\s\S]*?)'", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        MatchCollection a3 = Regex.Matches(html, @",yWendu:'([\s\S]*?)'", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        MatchCollection a4 = Regex.Matches(html, @",tianqi:'([\s\S]*?)'", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        MatchCollection a5 = Regex.Matches(html, @",fengxiang:'([\s\S]*?)'", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        MatchCollection a6 = Regex.Matches(html, @",fengli:'([\s\S]*?)'", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        MatchCollection a7 = Regex.Matches(html, @",aqi:'([\s\S]*?)'", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        MatchCollection a8 = Regex.Matches(html, @",aqiInfo:'([\s\S]*?)'", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        StringBuilder sb = new StringBuilder();
                        for (int z = 0; z < a1.Count; z++)
                        {

                            sb.Append(city.Groups[1].Value+"#"+ a1[z].Groups[1].Value+"#" + a2[z].Groups[1].Value + "#" + a3[z].Groups[1].Value + "#" + a4[z].Groups[1].Value + "#" + a5[z].Groups[1].Value + "#" + a6[z].Groups[1].Value + "#" + a7[z].Groups[1].Value + "#" + a8[z].Groups[1].Value + "\r\n");
                          
                        }


                        string path = AppDomain.CurrentDomain.BaseDirectory;
                        FileStream fs1 = new FileStream(path + "天气.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                        StreamWriter sw = new StreamWriter(fs1); 
                        sw.Write(sb.ToString());
                        sw.Close();
                        fs1.Close();

                        //FileStream fs1 = new FileStream(path + text[i] + "//" + text[i] + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                        //StreamWriter sw = new StreamWriter(fs1);
                        //sw.Write(sb.ToString());
                        //sw.Close();
                        //fs1.Close();
                        Thread.Sleep(100);
                        textBox2.Text += Url + "\r\n";

                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }
                    }

                }
            }

            catch (System.Exception ex)
            {

             MessageBox.Show(  ex.ToString());
            }

        }

        #endregion

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void 天气_Load(object sender, EventArgs e)
        {

        }
    }
}
