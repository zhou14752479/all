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

namespace 新浪研报
{
    public partial class 新浪研报 : Form
    {
        public 新浪研报()
        {
            InitializeComponent();
        }

        Thread thread;
        bool zanting = true;
        bool status = true;


        //公司研报
        public void run()
        {
            try
            {
               
                    for (int page = 1; page < 2; page++)
                    {


                        string url = "http://stock.finance.sina.com.cn/stock/go.php/vReport_List/kind/search/index.phtml?t1=2&symbol=000001&pubdate=2022-02-18&p="+page;

                    string html = method.GetUrl(url,"gb2312") ;
                        MatchCollection uids = Regex.Matches(html, @"rptid/([\s\S]*?)/");

                    MatchCollection titles = Regex.Matches(html, @"title=""([\s\S]*?)""");
                    MatchCollection types = Regex.Matches(html, @"title=""([\s\S]*?)<td>([\s\S]*?)</td>");
                    MatchCollection dates = Regex.Matches(html, @"<td class=""tal f14"">([\s\S]*?)<td>20([\s\S]*?)</td>");
                    MatchCollection jigous = Regex.Matches(html, @"<div class=""fname05""><span>([\s\S]*?)</span>");
                    MatchCollection yjys = Regex.Matches(html, @"<div class=""fname""><span>([\s\S]*?)</span>");

                    label1.Text += "正在采集：" + page+ "\r\n";

                    if (uids.Count == 0)
                            break;
                        for (int i = 0; i < uids.Count; i++)
                        {
                        string aurl = "http://stock.finance.sina.com.cn/stock/go.php/vReport_Show/kind/search/rptid/"+uids[i].Groups[1].Value+"/index.phtml";
                        string ahtml = method.GetUrl(aurl,"gb2312");
                            string body = Regex.Match(ahtml, @"<div class=""blk_container"">([\s\S]*?)</div>").Groups[1].Value.Replace("&nbsp;", "");
                            

                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据    
                            lv1.SubItems.Add(titles[i].Groups[1].Value.Replace("/t", ""));
                        lv1.SubItems.Add(types[i].Groups[2].Value);
                        lv1.SubItems.Add(Convert.ToDateTime(dates[i].Groups[2].Value).ToString("yyyyMMdd"));
                        lv1.SubItems.Add(jigous[i].Groups[1].Value);
                        lv1.SubItems.Add(yjys[i].Groups[1].Value);
                        lv1.SubItems.Add(Regex.Replace(body, "<[^>]+>", "").Trim());

                        Thread.Sleep(1000);
                        if (status == false)
                            return;

                        }
                    }
                   
                }
                
            
            catch (Exception ex)
            {

               MessageBox.Show(ex.ToString());
            }

        }


     
        //行业研报
        public void run2()
        {
            try
            {

                for (int page = 1; page < 2; page++)
                {


                    string url = "http://stock.finance.sina.com.cn/stock/go.php/vReport_List/kind/search/index.phtml?t1=3&industry=sw2_110300&symbol=&pubdate=2022-02-18&p=" + page;

                    string html = method.GetUrl(url, "gb2312");
                    MatchCollection uids = Regex.Matches(html, @"rptid/([\s\S]*?)/");

                    MatchCollection titles = Regex.Matches(html, @"title=""([\s\S]*?)""");
                    MatchCollection types = Regex.Matches(html, @"title=""([\s\S]*?)<td>([\s\S]*?)</td>");
                    MatchCollection dates = Regex.Matches(html, @"<td class=""tal f14"">([\s\S]*?)<td>20([\s\S]*?)</td>");
                    MatchCollection jigous = Regex.Matches(html, @"<div class=""fname05""><span>([\s\S]*?)</span>");
                    MatchCollection yjys = Regex.Matches(html, @"<div class=""fname""><span>([\s\S]*?)</span>");

                    label1.Text += "正在采集：" + page + "\r\n";

                    if (uids.Count == 0)
                        break;
                    for (int i = 0; i < uids.Count; i++)
                    {
                        string aurl = "http://stock.finance.sina.com.cn/stock/go.php/vReport_Show/kind/search/rptid/" + uids[i].Groups[1].Value + "/index.phtml";
                        string ahtml = method.GetUrl(aurl, "gb2312");
                        string body = Regex.Match(ahtml, @"<div class=""blk_container"">([\s\S]*?)</div>").Groups[1].Value.Replace("&nbsp;","");


                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据    
                        lv1.SubItems.Add(titles[i].Groups[1].Value.Replace("/t", ""));
                        lv1.SubItems.Add(types[i].Groups[2].Value);
                        lv1.SubItems.Add(Convert.ToDateTime(dates[i].Groups[2].Value).ToString("yyyyMMdd"));
                        lv1.SubItems.Add(jigous[i].Groups[1].Value);
                        lv1.SubItems.Add(yjys[i].Groups[1].Value);
                        lv1.SubItems.Add(Regex.Replace(body, "<[^>]+>", "").Trim());
                        if (status == false)
                            return;
                        Thread.Sleep(1000);

                    }
                }

            }


            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }
        private void 新浪研报_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run2);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.ListViewToCSV(listView1,true);
            //method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }
    }
}
