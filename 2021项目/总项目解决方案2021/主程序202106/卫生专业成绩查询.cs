using CsharpHttpHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
using System.Text.RegularExpressions;

namespace 主程序202106
{
    public partial class 卫生专业成绩查询 : Form
    {
        public 卫生专业成绩查询()
        {
            InitializeComponent();
        }
        private string GetHttp(string url)
        {
           HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = url,
                Method = "GET",
                Accept = "application/json",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.85 Safari/537.36",
                ContentType = "application/json;charset=UTF-8",
                Referer = "https://erquery.21wecan.com/LHTdyjej5z/",
                Cookie = "JSESSIONID=2734004F6299F65082972B2CBCE1556D; SERVER_ID=vserver-jp3kmlo3-backend-cq2um1uc",
                Host = "erquery.21wecan.com",
            };
           // item.Header.Add("sec-ch-ua", "" Not A; Brand";v="99", "Chromium";v="90", "Google Chrome";v="90"");
            item.Header.Add("sec-ch-ua-mobile", "?0");
            item.Header.Add("Sec-Fetch-Site", "same-origin");
            item.Header.Add("Sec-Fetch-Mode", "cors");
            item.Header.Add("Sec-Fetch-Dest", "empty");
            item.Header.Add("Accept-Encoding", "gzip, deflate, br");
            item.Header.Add("Accept-Language", "zh,sq;q=0.9,zh-CN;q=0.8,oc;q=0.7,de;q=0.6,en;q=0.5");
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            return html;
        }

        private void 卫生专业成绩查询_Load(object sender, EventArgs e)
        {

        }


        bool status = true;
        bool zanting = true;
        string cookie = "";
        Thread thread;

        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
   
            if (textBox1.Text == "")
            {
                MessageBox.Show("请导入账号");
                return;
            }

            //if (cookie == "")
            //{
            //    MessageBox.Show("请先点击获取COOKIE");
            //    return;
            //}
            DataTable dt = method.ExcelToDataTable(textBox1.Text, true);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                string name = dr[0].ToString().Trim();
                string card= dr[1].ToString().Trim();
                try
                {
                    label1.Text = "正在查询："+name;
                    if (status == false)
                    {
                        return;
                    }


                
                    string url = string.Format("https://erquery.21wecan.com/LHTdyjej5z/query?cardNo={0}&name={1}&testcardNo=&validcode=",card,System.Web.HttpUtility.UrlEncode(name));
                    string html = GetHttp(url);
               
                   string km1 = Regex.Match(html, @"""km1"":""([\s\S]*?)""").Groups[1].Value;
                    string km2 = Regex.Match(html, @"""km2"":""([\s\S]*?)""").Groups[1].Value;
                    string km3 = Regex.Match(html, @"""km3"":""([\s\S]*?)""").Groups[1].Value;
                    string km4 = Regex.Match(html, @"""km4"":""([\s\S]*?)""").Groups[1].Value;
                    if (km1 != "")
                    {
                        string zong = km1 + "," + km2 + "," + km3 + "," + km4;
                        string tongguo = "通过";
                        string[] text = zong.Split(new string[] { "," }, StringSplitOptions.None);
                        foreach (string item in text)
                        {
                            if (item != "null")
                            {
                                if (Convert.ToInt32(item) < 60)
                                {
                                    tongguo = "未通过";
                                }
                            }

                        }

                        if (tongguo == "通过")
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                            lv1.SubItems.Add(dr[0].ToString().Trim());
                            lv1.SubItems.Add(dr[1].ToString().Trim());
                            lv1.SubItems.Add(dr[2].ToString().Trim());
                            lv1.SubItems.Add(dr[3].ToString().Trim());
                            lv1.SubItems.Add(dr[4].ToString().Trim());
                            lv1.SubItems.Add(dr[5].ToString().Trim());
                            lv1.SubItems.Add(dr[6].ToString().Trim());
                            lv1.SubItems.Add(km1 + "," + km2 + "," + km3 + "," + km4 + "   " + tongguo);

                        }
                        else
                        {
                            ListViewItem lv1 = listView2.Items.Add((listView2.Items.Count).ToString()); //使用Listview展示数据   
                            lv1.SubItems.Add(dr[0].ToString().Trim());
                            lv1.SubItems.Add(dr[1].ToString().Trim());
                            lv1.SubItems.Add(dr[2].ToString().Trim());
                            lv1.SubItems.Add(dr[3].ToString().Trim());
                            lv1.SubItems.Add(dr[4].ToString().Trim());
                            lv1.SubItems.Add(dr[5].ToString().Trim());
                            lv1.SubItems.Add(dr[6].ToString().Trim());
                            lv1.SubItems.Add(km1 + "," + km2 + "," + km3 + "," + km4 + "   " + tongguo);
                        }
                       
                    }
                    else
                    {
                        ListViewItem lv1 = listView2.Items.Add((listView2.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(dr[0].ToString().Trim());
                        lv1.SubItems.Add(dr[1].ToString().Trim());
                        lv1.SubItems.Add(dr[2].ToString().Trim());
                        lv1.SubItems.Add(dr[3].ToString().Trim());
                        lv1.SubItems.Add(dr[4].ToString().Trim());
                        lv1.SubItems.Add(dr[5].ToString().Trim());
                        lv1.SubItems.Add(dr[6].ToString().Trim());
                        lv1.SubItems.Add("无");
                    }
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;


                    //if (Convert.ToInt32(grade1) >= 60 && Convert.ToInt32(grade2) >= 60)
                    //{
                    //    //ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    //    //lv1.SubItems.Add(card);
                    //    //lv1.SubItems.Add(name);
                    //    //lv1.SubItems.Add(tel);

                    //}


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    continue;
                }

                Thread.Sleep(500);
            }
            MessageBox.Show("查询完成");



        }

        private void button6_Click(object sender, EventArgs e)
        {
            #region 通用检测


            string html = method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"DZkGmf"))
            {

                return;
            }

            #endregion
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            //  openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox1.Text = openFileDialog1.FileName;
             
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
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

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
        }
    }
}
