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
        string pro = "";
        bool status = true;
        bool zanting = true;
        #region 主程序
        public void run()
        {
            if (this.comboBox1.Text == "全国")
            {

                pro = "";
            }
            else if (this.comboBox1.Text == "北京")
            {
                pro = "15";
            }
            else if (this.comboBox1.Text == "天津")
            {
                pro = "13";
            }
            else if (this.comboBox1.Text == "重庆")
            {
                pro = "27";
            }
            else if (this.comboBox1.Text == "上海")
            {
                pro = "8";
            }
            else if (this.comboBox1.Text == "河北省")
            {
                pro = "29";
            }
            else if (this.comboBox1.Text == "山西省")
            {
                pro = "28";
            }
            else if (this.comboBox1.Text == "内蒙古自治区")
            {
                pro = "33";
            }
            else if (this.comboBox1.Text == "辽宁省")
            {
                pro = "14";
            }
            else if (this.comboBox1.Text == "吉林省")
            {
                pro = "23";

            }
            else if (this.comboBox1.Text == "黑龙江省")
            {
                pro = "5";
            }
            else if (this.comboBox1.Text == "江苏省")
            {
                pro = "30";
            }
            else if (this.comboBox1.Text == "浙江省")
            {
                pro = "19";
            }
            else if (this.comboBox1.Text == "安徽省")
            {
                pro = "31";
            }
            else if (this.comboBox1.Text == "福建省")
            {
                pro = "11";
            }
            else if (this.comboBox1.Text == "江西省")
            {
                pro = "20";
            }
            else if (this.comboBox1.Text == "山东省")
            {
                pro = "18";
            }
            else if (this.comboBox1.Text == "河南省")
            {
                pro = "17";
            }
            else if (this.comboBox1.Text == "湖北省")
            {
                pro = "16";
            }
            else if (this.comboBox1.Text == "湖南省")
            {
                pro = "6";
            }
            else if (this.comboBox1.Text == "广东省")
            {
                pro = "10";
            }
            else if (this.comboBox1.Text == "广西省")
            {
                pro = "22";
            }
            else if (this.comboBox1.Text == "海南省")
            {
                pro = "12";
            }
            else if (this.comboBox1.Text == "四川省")
            {
                pro = "3";
            }
            else if (this.comboBox1.Text == "贵州省")
            {
                pro = "9";
            }
            else if (this.comboBox1.Text == "云南省")
            {
                pro = "4";
            }
            else if (this.comboBox1.Text == "西藏自治区")
            {
                pro = "24";
            }
            else if (this.comboBox1.Text == "陕西省")
            {
                pro = "26";
            }
            else if (this.comboBox1.Text == "甘肃省")
            {
                pro = "25";
            }

            else if (this.comboBox1.Text == "青海省")
            {
                pro = "1";
            }
            else if (this.comboBox1.Text == "宁夏回族自治区")
            {
                pro = "32";
            }
            else if (this.comboBox1.Text == "新疆维吾尔自治区")
            {
                pro = "2";
            }







            try
            {
                for (int i = 0; i < 9999; i++)
                {

                    string Url = "https://3g.kq36.cn/m/companylist.aspx?keyw=&typeid="+typeid+"&proviceid="+this.pro+"&cityid=&areaid=&minying=&guimo=&xingzhi=&offerid=&ddlmedicare=&pageindex="+i;

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
