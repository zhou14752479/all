using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace titan007
{
    public partial class 平均值 : Form
    {
        public 平均值()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
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

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }
        #region  程序关闭删除自身
        public static void TestForKillMyself()
        {
            string bat = @"@echo off
                           :tryagain
                           del %1
                           if exist %1 goto tryagain
                           del %0";
            File.WriteAllText("killme.bat", bat);//写bat文件
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "killme.bat";
            psi.Arguments = "\"" + Environment.GetCommandLineArgs()[0] + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
        }


        #endregion


        Thread thread;

        bool zanting = true;
        bool status = true;

        #region 主程序
        public void run()
        {





            string html = method.GetUrl("http://live.titan007.com/vbsxml/bfdata_ut.js?r=0071657803475000", "utf-8");




            MatchCollection ids = Regex.Matches(html, @"A\[([\s\S]*?)\]=""([\s\S]*?)\^");

            foreach (Match id in ids)
            {
            

                try
                {
                  
                    string URL = "https://op1.titan007.com/oddslist/" + id.Groups[2].Value + ".htm";
                   
                    webBrowser1.Stop();
                    webBrowser1.Navigate(URL);






                    Thread.Sleep(2000);

                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);


                }

                while (this.zanting == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }
                if (status == false)
                    return;
            }




        }
        #endregion

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 平均值_Load(object sender, EventArgs e)
        {
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WB_DocumentCompleted);
            

       
    }

        private void WB_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                return;
            if (e.Url.ToString() != webBrowser1.Url.ToString())
                return;
            if (webBrowser1.IsBusy == true)
                return;
            if (webBrowser1.DocumentText.Contains("</html>"))
            {

                html = webBrowser1.Document.Body.OuterHtml;
                run22();
                status = true;
            }
            else
            {
                Application.DoEvents();
            }
        }

        public string html = "";


        public void run22()
        {

            try
            {
                string home = Regex.Match(html, @"<div class=""home"">([\s\S]*?)title=""([\s\S]*?)""").Groups[2].Value.Replace(" ", "");
                string guest = Regex.Match(html, @"<div class=""guest"">([\s\S]*?)title=""([\s\S]*?)""").Groups[2].Value.Replace(" ", "");
                string liansai = Regex.Match(html, @"LName([\s\S]*?)>([\s\S]*?)<").Groups[2].Value.Replace(" ", "");
                string time = Regex.Match(html, @"2022-([\s\S]*?)<").Groups[1].Value.Replace(" ", "").Trim().Replace("&nbsp;", "");
                string chu = Regex.Match(html, @"初盘平均值([\s\S]*?)即时平均值").Groups[1].Value;

                string[] text = chu.Replace("<td>", "").Replace("<td class=\"rb\">", "").Replace("<td>", "").Split(new string[] { "</td>" }, StringSplitOptions.None);

                string jishi = Regex.Match(html, @"即时平均值([\s\S]*?)</tr>").Groups[1].Value;

                string[] text2 = jishi.Replace("<td>", "").Replace("<td class=\"rb\">", "").Replace("<span class=\"o_green\">", "").Replace("<span class=\"o_red\">", "").Replace("</span>", "").Split(new string[] { "</td>" }, StringSplitOptions.None);
                if (home != "")
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(liansai);
                    lv1.SubItems.Add(home);
                    lv1.SubItems.Add(guest);
                    lv1.SubItems.Add("2022-" + time);
                    lv1.SubItems.Add(text[1].Trim());
                    lv1.SubItems.Add(text[2].Trim());
                    lv1.SubItems.Add(text[3].Trim());
                    lv1.SubItems.Add(text2[1].Trim());
                    lv1.SubItems.Add(text2[2].Trim());
                    lv1.SubItems.Add(text2[3].Trim());

                }


            }
            catch (Exception)
            {
;
            }

        }

    }
}
