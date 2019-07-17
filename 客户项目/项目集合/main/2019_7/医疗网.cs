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

namespace main._2019_7
{
    public partial class 医疗网 : Form
    {
        public 医疗网()
        {
            InitializeComponent();
        }

       

        public string NCRtoString(string str)
        {
            string outStr = "";
            if (!string.IsNullOrEmpty(str))
            {
                string[] strlist = str.Replace("&#", "").Replace(";", "").Split('x');
                try
                {
                    for (int i = 1; i < strlist.Length; i++)
                    {
                        //将unicode字符转为10进制整数，然后转为char中文字符  
                        outStr += (char)int.Parse(strlist[i], System.Globalization.NumberStyles.HexNumber);
                    }
                }
                catch (FormatException ex)
                {
                    outStr = ex.Message;
                }
            }
            
            return outStr;
         
        }

        string typeid = "1";
        bool status = true;
        bool zanting = true;
        #region 主程序
        public void run()
        {

            try
            {
                for (int i = 0; i < 9999; i++)
                {

                    string Url = "https://3g.kq36.cn/m/companylist.aspx?keyw=&typeid="+typeid+"&proviceid=0&cityid=0&areaid=0&minying=&guimo=&xingzhi=&offerid=&ddlmedicare=&pageindex="+i;

                    string html = method.GetUrl(Url, "utf-8");
                   
                    MatchCollection ids = Regex.Matches(html, @"data-userid='([\s\S]*?)'", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                   
                    if (ids.Count == 0)
                        return;
                    ArrayList lists = new ArrayList();

                    foreach (Match id in ids)
                    {
                        lists.Add("https://3g.kq36.com/jobs/" + id.Groups[1].Value);
                    }

                    foreach (string list in lists)

                    {

                        string strhtml = method.GetUrl(list, "gb2312");

                        Match a1 = Regex.Match(strhtml, @"<title>([\s\S]*?)诚");
                        Match a2 = Regex.Match(strhtml, @"成立</li>([\s\S]*?)</li>");
                        Match a3 = Regex.Match(strhtml, @"员工</li>([\s\S]*?)</li>");
                        Match a4 = Regex.Match(strhtml, @"性质</li>([\s\S]*?)</li>");
                        Match a5 = Regex.Match(strhtml, @"联系</li>([\s\S]*?)&nbsp");
                        Match a6 = Regex.Match(strhtml, @"电话</li>([\s\S]*?)</li>");
                        Match a7 = Regex.Match(strhtml, @"手机</li>([\s\S]*?)</li>");
                        Match a8 = Regex.Match(strhtml, @"QQ</li>([\s\S]*?)</li>");
                        Match a9 = Regex.Match(strhtml, @"data-email=""([\s\S]*?)""");
                        Match a10 = Regex.Match(strhtml, @"地址</li>([\s\S]*?)<a");

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(Regex.Replace(a1.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a2.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a3.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a4.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(NCRtoString(Regex.Replace(a5.Groups[1].Value, "<[^>]+>", "")).Trim());
                        lv1.SubItems.Add(Regex.Replace(a6.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a7.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a8.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a9.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(NCRtoString(Regex.Replace(a10.Groups[1].Value, "<[^>]+>", "")).Trim());


                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)

                        {
                            return;
                        }
                        Thread.Sleep(2000);
                    }

                }
            }

            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        private void 医疗网_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                typeid = "1";
            }
            else if (radioButton2.Checked == true)
            {
                typeid = "3";
            }
            else if (radioButton3.Checked == true)
            {
                typeid = "8";
            }
            else if (radioButton4.Checked == true)
            {
                typeid = "5";
            }
            else if (radioButton5.Checked == true)
            {
                typeid = "26";
            }
            else if (radioButton6.Checked == true)
            {
                typeid = "2000";
            }

            status = true;
           

            #region 通用导出

            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php", "utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == "3.3.3.3")
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {

                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            }
            else
            {
                MessageBox.Show("请登录您的账号！");
                System.Diagnostics.Process.Start("http://www.acaiji.com");
                return;
            }
            #endregion
          


        }

        private void Button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            status = false;
        }
    }
}
