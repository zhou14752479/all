using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 模拟采集
{
    public partial class amazon : Form
    {
        public amazon()
        {
            InitializeComponent();
        }



        


        public void run()
        {
            
               

                MatchCollection asins = Regex.Matches(html, @"<div data-asin=""([\s\S]*?)""");
                MatchCollection titles = Regex.Matches(html, @"<span class=""a-size-base-plus a-color-base a-text-normal"">([\s\S]*?)</span>");
                MatchCollection ads = Regex.Matches(html, @"a-popover-sp-info-popover-([\s\S]*?)""");

            ArrayList asinads = new ArrayList();

            foreach (Match item in ads)
            {
                asinads.Add(item.Groups[1].Value);
            }


            ArrayList asinsList = new ArrayList();


            foreach (Match item in asins)
            {
                if (item.Groups[1].Value != "")
                {
                    asinsList.Add(item.Groups[1].Value);
                }
            }




            if (titles.Count == 0)
                {
                    shuju = false;
                    return;
                }

           
            for (int j = 0; j < titles.Count; j++)
                {

                    ListViewItem lv1 = listView1.Items.Add(DateTime.Now.ToString("yyyy-MM-dd")); //使用Listview展示数据   
                    try
                    {
                    int sponsored = 0;
                    if (asinads.Contains(asinsList[j].ToString()))
                    {
                        sponsored = 1;
                    }
                        
                        lv1.SubItems.Add(key);
                        lv1.SubItems.Add(xuhao.ToString());
                        lv1.SubItems.Add(titles[j].Groups[1].Value);
                        lv1.SubItems.Add(asinsList[j].ToString());
                        lv1.SubItems.Add(sponsored.ToString());
                        xuhao = xuhao + 1;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }


                }


           

        }

        bool shuju = true;  //判断页码是否有数据
        bool status = false;
        public static string html; //网页源码传值
        int xuhao = 1;

        ArrayList pageKeyList = new ArrayList();

        string key = "";

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"amazon"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion
            string domain = "com";
            switch (comboBox1.Text.Trim())
            {
                case "美国":
                    domain = "com";
                    break;

                case "加拿大":
                    domain = "ca";
                    break;
                case "英国":
                    domain = "co.uk";
                    break;
                case "法国":
                    domain = "fr";
                    break;
                case "西班牙":
                    domain = "es";
                    break;
                case "德国":
                    domain = "de";
                    break;
                case "意大利":
                    domain = "it";
                    break;
                case "日本":
                    domain = "ip";
                    break;


            }











            label1.Text = "正在查询......";

            string[] keywords = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string keyword in keywords)
            {
                xuhao = 1; //重置序号

                key = keyword;
                for (int i = 1; i < 11; i++)
                {
                    if (shuju == false)
                    {
                        return;
                    }

                    
                    status = false;
                    webBrowser1.Navigate("https://www.amazon."+domain+"/s?k=" + System.Web.HttpUtility.UrlEncode(keyword)+"&page=" + i);



                    while (this.status == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    Thread.Sleep(1000);
                }


            }

            label1.Text = "查询结束";
            MessageBox.Show("查询结束");
        }

        private void amazon_Load(object sender, EventArgs e)
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

            if (webBrowser1.DocumentText.Contains("</html>"))
            {
                
                html = webBrowser1.DocumentText;
               
                run();
                status = true;
            }
            else
            {
                Application.DoEvents();
            }
        }
       

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要删除吗？", "清空", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                listView1.Items.Clear();
            }
            else
            {
               
            }

            
        }

        private void amazon_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
                //System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }

        }
    }
}
