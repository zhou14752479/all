using Microsoft.Win32;
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

namespace main._2019_4
{
    public partial class 群 : Form
    {
        public 群()
        {
            InitializeComponent();
        }
        static string path = "";
        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择保存路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                path = textBox1.Text = dialog.SelectedPath;
            }
        }

        private void 群_Load(object sender, EventArgs e)
        {

        }

        #region  微信群二维码下载主程序

        public void run()
        {
            if (path == "")
            {
                MessageBox.Show("请选择微信群二维码保存地址");
                return;
            }

            try
            {
                for (int i = 0; i <3300 ; i++)
                {


                    String Url = "https://www.weixinqun.com/group?p=" + i;

                    string html = method.GetUrl(Url, "utf-8");


                    MatchCollection urls = Regex.Matches(html, @"<p class=""goods_name ellips"">([\s\S]*?)<a href=""([\s\S]*?)""");

                    ArrayList lists = new ArrayList();

                    foreach (Match url in  urls)
                    {
                        lists.Add("https://www.weixinqun.com"+url.Groups[2].Value);
                    }

                    MessageBox.Show(lists.Count.ToString());
                    if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;
                    foreach (string list in lists)

                    {

                        
                        string strhtml = method.GetUrl(list, "utf-8");
                        Match title = Regex.Match(strhtml, @"<title>([\s\S]*?),");
                        Match downurl = Regex.Match(strhtml, @"<span class=""shiftcode"" style=""display: none;""><img src=""([\s\S]*?)""");
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(title.Groups[1].Value.Trim());
                        lv1.SubItems.Add("微信群请查看二维码，无群号");

                        method.downloadFile(downurl.Groups[1].Value,path,title.Groups[1].Value+".jpg");

                        listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置

                    }


                    Thread.Sleep(Convert.ToInt32(500));   //内容获取间隔，可变量        
                }


            }




            catch (System.Exception ex)
            {

                textBox1.Text = ex.ToString();
            }

        }

        #endregion

        #region  QQ群搜索主程序

        public void run1()
        {
           
            try
            {
                for (int i = 0; i < 3300; i++)
                {


                    String Url = "https://www.weixinqun.com/group?p=" + i;

                    string html = method.GetUrl(Url, "utf-8");


                    MatchCollection urls = Regex.Matches(html, @"<p class=""goods_name ellips"">([\s\S]*?)<a href=""([\s\S]*?)""");

                    ArrayList lists = new ArrayList();

                    foreach (Match url in urls)
                    {
                        lists.Add("https://www.weixinqun.com" + url.Groups[2].Value);
                    }

                    MessageBox.Show(lists.Count.ToString());
                    if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;
                    foreach (string list in lists)

                    {


                        string strhtml = method.GetUrl(list, "utf-8");
                        Match title = Regex.Match(strhtml, @"<title>([\s\S]*?),");
                        Match downurl = Regex.Match(strhtml, @"<span class=""shiftcode"" style=""display: none;""><img src=""([\s\S]*?)""");
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(title.Groups[1].Value.Trim());
                        lv1.SubItems.Add("微信群请查看二维码，无群号");

                        method.downloadFile(downurl.Groups[1].Value, path, title.Groups[1].Value + ".jpg");

                        listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置

                    }


                    Thread.Sleep(Convert.ToInt32(500));   //内容获取间隔，可变量        
                }


            }




            catch (System.Exception ex)
            {

                textBox1.Text = ex.ToString();
            }

        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            #region   读取注册码信息才能运行软件！

            RegistryKey rsg = Registry.CurrentUser.OpenSubKey("zhucema"); //true表可修改                
            if (rsg != null && rsg.GetValue("mac") != null)  //如果值不为空
            {
                Thread thread = new Thread(new ThreadStart(run));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();

            }

            else
            {
                MessageBox.Show("请注册软件！");
                register lg = new register();
                lg.Show();
            }

            #endregion
        }
    }
}
