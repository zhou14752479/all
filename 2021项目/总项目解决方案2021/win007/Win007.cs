using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using myDLL;

namespace win007
{
    public partial class Win007 : Form
    {
        public Win007()
        {
            InitializeComponent();
        }
        #region 苏飞请求
        public static string gethtml(string url, string COOKIE)
        {
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = url,//URL     必需项  
                Method = "GET",//URL     可选项 默认为Get  
                Timeout = 100000,//连接超时时间     可选项默认为100000  
                ReadWriteTimeout = 30000,//写入Post数据超时时间     可选项默认为30000  
                IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写  
                Cookie = COOKIE,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36",//用户的浏览器类型，版本，操作系统     可选项有默认值  
                Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值  
                ContentType = "text/html",//返回类型    可选项有默认值  
                Referer = "https://www.google.com/",//来源URL     可选项  
                Allowautoredirect = true,//是否根据３０１跳转     可选项  
                AutoRedirectCookie = true,//是否自动处理Cookie     可选项  
                                          //CerPath = "d:\123.cer",//证书绝对路径     可选项不需要证书时可以不写这个参数  
                                          //Connectionlimit = 1024,//最大连接数     可选项 默认为1024  
                Postdata = "",//Post数据     可选项GET时不需要写  
                              //ProxyIp = "192.168.1.105：2020",//代理服务器ID     可选项 不需要代理 时可以不设置这三个参数  
                              //ProxyPwd = "123456",//代理服务器密码     可选项  
                              //ProxyUserName = "administrator",//代理服务器账户名     可选项  
                ResultType = ResultType.String,//返回数据类型，是Byte还是String  
            };
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            string cookie = result.Cookie;


            return html;

        }

        #endregion

        function fc = new function();
        public void chaxun()
        {
            try
            {


                string sql = "select * from datas where";

                if (comboBox1.Text.Trim() == "")
                {
                    sql = sql + (" gongsi like '_%' and");
                }
                else
                {
                    sql = sql + (" gongsi like '" + comboBox1.Text.Trim() + "' and");
                }

             


                if (sql.Substring(sql.Length - 3, 3) == "and")
                {
                    sql = sql.Substring(0, sql.Length - 3);
                }

                DataTable dt = fc.chaxundata(sql);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns["matchname"].HeaderText = "比赛名";
                dataGridView1.Columns["zhu"].HeaderText = "主队";
                dataGridView1.Columns["ke"].HeaderText = "客队";
                dataGridView1.Columns["time"].HeaderText = "比赛时间";
                dataGridView1.Columns["gongsi"].HeaderText = "公司名";
                dataGridView1.Columns["url"].HeaderText = "数据网址";
                dataGridView1.Columns["data1"].HeaderText = "数据1";
                dataGridView1.Columns["data2"].HeaderText = "数据2";
                dataGridView1.Columns["data3"].HeaderText = "数据3";
                dataGridView1.Columns["data4"].HeaderText = "数据4";
                dataGridView1.Columns["data5"].HeaderText = "数据5";
                dataGridView1.Columns["data6"].HeaderText = "数据6";


                //fc.ShowDataInListView(dt, listView1);
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.ToString());


            }
        }


        private void Win007_Load(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"hGRLg"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion
        }
        public void run()
        {
            for (int day = 20210101; day < 20211225; day++)
            {
                label3.Text = day.ToString();
                fc.getdata(day.ToString());
            }

        }

        string startdate = "2021-01-01";
        string enddate = "2021-12-31";
        public void getdatashihsi()
        {

         


            for (DateTime dt = Convert.ToDateTime(startdate); dt < Convert.ToDateTime(enddate); dt.AddDays(1))
            {
                try
                {
                    string url = "http://bf.win007.com/football/Over_" + dt.ToString("yyyyMMdd") + ".htm";
                    label7.Text = url;
                       
                    string html = method.GetUrl(url, "gb2312");
                    MatchCollection tds = Regex.Matches(html, @"id='ls_([\s\S]*?)'>([\s\S]*?)</tr>");
                    MatchCollection ids = Regex.Matches(html, @"showgoallist\(([\s\S]*?)\)");

                    for (int i = 0; i < tds.Count; i++)
                    {
                        string baseinfo = Regex.Replace(tds[i].Groups[2].Value, "<[^>]+>", "");

                        string datajsurl = "http://1x2d.win007.com/" + ids[i].Groups[1].Value + ".js?r=007132848760362108507";
                        string datajs = method.GetUrl(datajsurl, "gb2312");
                        string datajsjs = Regex.Match(datajs, @"game=([\s\S]*?);").Groups[1].Value;

                        string matchname_cn = Regex.Match(datajs, @"matchname_cn=""([\s\S]*?)""").Groups[1].Value;
                        string hometeam_cn = Regex.Match(datajs, @"hometeam_cn=""([\s\S]*?)""").Groups[1].Value;
                        string guestteam_cn = Regex.Match(datajs, @"guestteam_cn=""([\s\S]*?)""").Groups[1].Value;
                        string MatchTime = Regex.Match(datajs, @"MatchTime=""([\s\S]*?)""").Groups[1].Value;

                        MatchCollection gongsi_names = Regex.Matches(datajsjs, @",00\|([\s\S]*?)\|");
                        MatchCollection cids = Regex.Matches(datajsjs, @"\d{2,5}\|\d{8,10}");

                        for (int j = 0; j < gongsi_names.Count; j++)
                        {

                            try
                            {
                                string[] text = cids[j].Groups[0].Value.Split(new string[] { "|" }, StringSplitOptions.None);
                                string dataurl = "http://op1.win007.com/OddsHistory.aspx?id=" + text[1] + "&sid=" + ids[i].Groups[1].Value + "&cid=" + text[0] + "&l=0";

                                //MessageBox.Show(dataurl);
                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(matchname_cn);
                                lv1.SubItems.Add(hometeam_cn);
                                lv1.SubItems.Add(guestteam_cn);
                                lv1.SubItems.Add(MatchTime);
                                lv1.SubItems.Add(gongsi_names[j].Groups[1].Value);
                                lv1.SubItems.Add(dataurl);




                                string datahtml = gethtml(dataurl, "utf-8");
                                MatchCollection a1s = Regex.Matches(datahtml, @"<b><font color=([\s\S]*?)>([\s\S]*?)</font>");
                                textBox1.Text = datahtml;


                                string data1 = "";
                                string data2 = "";
                                string data3 = "";
                                string data4 = "";
                                string data5 = "";
                                string data6 = "";
                                try
                                {
                                    data1 = a1s[0].Groups[2].Value;
                                    lv1.SubItems.Add(data1);
                                    data2 = a1s[1].Groups[2].Value;
                                    lv1.SubItems.Add(data2);
                                    data3 = a1s[2].Groups[2].Value;
                                    lv1.SubItems.Add(data3);
                                    data4 = a1s[3].Groups[2].Value;
                                    lv1.SubItems.Add(data4);
                                    data5 = a1s[4].Groups[2].Value;
                                    lv1.SubItems.Add(data5);
                                    data6 = a1s[5].Groups[2].Value;
                                    lv1.SubItems.Add(data6);
                                }
                                catch (Exception)
                                {

                                    ;
                                }
                                fc.insertdata(matchname_cn, hometeam_cn, guestteam_cn, MatchTime, gongsi_names[j].Groups[1].Value, dataurl, data1, data2, data3, data4, data5, data6);



                                if (status == false)
                                    return;
                                Thread.Sleep(100);

                            }
                            catch (Exception ex)
                            {
                                //  MessageBox.Show(ex.ToString());
                                continue;
                            }
                        }


                    }

                }
                catch (Exception ex)
                {

                    //MessageBox.Show(ex.ToString());
                }
            }


        }
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            chaxun();


        }

        private void 清空查询数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            dt.Rows.Clear();

            dataGridView1.DataSource = dt;
        }

        private void 导出查询数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
        bool status = true;
     
        private void button2_Click(object sender, EventArgs e)
        {
            startdate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            enddate = DateTime.Now.ToString("yyyy-MM-dd");
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getdatashihsi);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getdatashihsi);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            comboBox1.Items.Clear();
               ArrayList lists = fc.getsupplyers();
            comboBox1.Items.Add("");
            foreach (var item in lists)
            {
                comboBox1.Items.Add(item);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            status = false;
        }
    }
}
