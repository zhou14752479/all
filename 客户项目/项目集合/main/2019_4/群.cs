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

        public static string COOKIE = " XWINDEXGREY=1; main_login=qq; pac_uid=1_852266010; pgv_info=ssid=s742373320; pgv_pvid=8532151720; skey=MCjHmnyKqc; uin=o0852266010; traceid=b5e5d0424b";

        private void 群_Load(object sender, EventArgs e)
        {
            
        }

        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
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

                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }


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

            if (textBox3.Text == "")
            {
                MessageBox.Show("请输入QQ群搜索关键字");
                return;
            }

            try
            {
                for (int i = 0; i < 100; i++)
                {
                    string keyword = System.Web.HttpUtility.UrlDecode(textBox3.Text);

                    String Url = "https://qun.qq.com/cgi-bin/group_search/group_search?retype=2&keyword="+keyword+"&page="+i+"&wantnum=20&city_flag=0&distance=1&ver=1&from=9&bkn=1767690426&style=1";

                    string html = method.GetUrlWithCookie(Url, COOKIE);

                    string html2 = Unicode2String(html);

                    MatchCollection names = Regex.Matches(html2, @",""name"":""([\s\S]*?)""");
                    MatchCollection codes = Regex.Matches(html2, @"""code"":([\s\S]*?),");


                    if (names.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;
                    for (int j = 0; j< names.Count; j++)

                    {

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(names[j].Groups[1].Value.Trim());
                        lv1.SubItems.Add(codes[j].Groups[1].Value.Trim());

                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }
                        

                    }

                    Thread.Sleep(Convert.ToInt32(2000));   //内容获取间隔，可变量        
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
                if (radioButton1.Checked == true)
                {
                    Thread thread = new Thread(new ThreadStart(run));
                    Control.CheckForIllegalCrossThreadCalls = false;
                    thread.Start();
                }
                else if (radioButton2.Checked == true)
                {
                    Thread thread = new Thread(new ThreadStart(run1));
                    Control.CheckForIllegalCrossThreadCalls = false;
                    thread.Start();
                }
            }

            else
            {
                MessageBox.Show("请注册软件！");
                register lg = new register();
                lg.Show();
            }

            #endregion
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            webBrowser web1 = new webBrowser("https://qun.qq.com/member.html");
            web1.Show();

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
