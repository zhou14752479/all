using System;
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
using myDLL;

namespace QQ群采集
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string COOKIE { get; set; }
        public string bkn { get; set; }
        bool zanting = true;
        bool status = true;


        long GetBkn()
        {
            string skey = Regex.Match(COOKIE, @"skey=([\s\S]*?);").Groups[1].Value;
            if (skey == "")
            {
                skey = Regex.Match(COOKIE, @"skey=.*").Groups[0].Value.Replace("skey=", "").Trim();
            }

            var hash = 5381;
            for (int i = 0, len = skey.Length; i < len; ++i)
                hash += (hash << 5) + (int)skey[i];
            return hash & 2147483647;
        }

        /// <summary>
        /// 主程序
        /// </summary>
        public void QQqun()
        {
            try
            {
                string[] array = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] != "")
                    {


                        string url = "https://qun.qq.com/cgi-bin/group_search/group_search?retype=2&keyword="+ array[i] + "&page=0&wantnum=20&city_flag=0&distance=1&ver=1&from=9&bkn=765861501&style=1";
                        string html =method.GetUrlWithCookie(url,COOKIE, "utf-8");

                      
                        Match Userid = Regex.Match(html, @"encryptedUserId\\"":\\""([\s\S]*?)\\");



                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据  
                        lv1.SubItems.Add(array[i]);





                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
