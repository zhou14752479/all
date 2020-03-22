using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 启动程序
{
    public partial class YY检测 : Form
    {
        public YY检测()
        {
            InitializeComponent();
        }
        bool zanting = true;
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
            StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.Default);
            string text = streamReader.ReadToEnd();
            string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != "")
                {
                    int a = 0;
                   
                        string html =method.GetUrl("https://aq.yy.com/p/pwd/fgt/mnew/dpch.do?account="+array[i].Trim()+"&busifrom=&appid=1&yyapi=false", "utf-8" );


                       // Match key = Regex.Match(html, @"key"" value=""([\s\S]*?)""");

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(array[i]);

                    if (html.Contains("LEVEL2"))
                    {
                        lv1.SubItems.Add("手机类型");

                    }
                    if (html.Contains("账号不存在"))
                    {
                        lv1.SubItems.Add("账号不存在");

                    }

                    if (html.Contains("未设置密保"))
                    {
                        lv1.SubItems.Add("未设置密保");

                    }

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }


                }




            }
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string pathname = AppDomain.CurrentDomain.BaseDirectory + ts.TotalSeconds.ToString() + ".xlsx";
            method.DataTableToExcelTime(method.listViewToDataTable(this.listView1), true, pathname);
        }


        private void YY检测_Load(object sender, EventArgs e)
        {

        }
    }
}
