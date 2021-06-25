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

namespace 主程序202105
{
    public partial class 浙江建设市场信息系统 : Form
    {
        public 浙江建设市场信息系统()
        {
            InitializeComponent();
        }

        bool status = false;

        public void run()
        {

            try
            {

                for (int i = 70; i < 192; i++)
                {

                    string url = "http://jssccx.zjt.gov.cn:8532/index/cydw/sg_cydw_list.htm";
                    label1.Text = "正在查询：" + i + "页";

                    string html = method.PostUrl(url, "currentPage=" + i, "JSESSIONID=ABD0A29DDBD61BE7E92577AFFBB62F28; _h_=\"eNrz4MoqTi0uzszP80yRNC7OTLIsLU5OTKnIM85OLMlLLMrKqsgtyOSGqvFOrSyuQdKBLC6Qk1hc4picDBRJTQnJzE3lSS5KTSwByoI4GLI + DAyMle1zT09BUQYXjQIAapo6DA == \"", "utf-8", "application/x-www-form-urlencoded", "");
                    MatchCollection ids = Regex.Matches(html, @"onclick=""jumpDetail\(([\s\S]*?)\)");
                    MatchCollection names = Regex.Matches(html, @"<span class=""a-co-b"">([\s\S]*?)</span>");
                    for (int j = 0; j < ids.Count; j++)
                    {
                        string userid = ids[j].Groups[1].Value;
                        string name = names[j].Groups[1].Value;
                        #region 已完业绩
                        string ahtml = method.PostUrl("http://jssccx.zjt.gov.cn:8532/index/cydw/sg/sg_finisheditems.htm", "sgUserId=" + userid, "JSESSIONID=ABD0A29DDBD61BE7E92577AFFBB62F28; _h_=\"eNrz4MoqTi0uzszP80yRNC7OTLIsLU5OTKnIM85OLMlLLMrKqsgtyOSGqvFOrSyuQdKBLC6Qk1hc4picDBRJTQnJzE3lSS5KTSwByoI4GLI + DAyMle1zT09BUQYXjQIAapo6DA == \"", "utf-8", "application/x-www-form-urlencoded", "");
                        MatchCollection aids = Regex.Matches(ahtml, @"openWin\('([\s\S]*?)'");
                        foreach (Match aid in aids)
                        {
                            try
                            {

                            string ahtmldetail = method.GetUrl("http://jssccx.zjt.gov.cn:8532/index/cydw/sg/sg_finished_detail.htm?sgUserId=" + userid + "&id=" + aid.Groups[1].Value, "utf-8");
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(name);
                            lv1.SubItems.Add("已完业绩");
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(ahtmldetail, @"工程名称：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(ahtmldetail, @"合同段名称：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(ahtmldetail, @"所在省份：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(ahtmldetail, @"开始桩号：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(ahtmldetail, @"结束桩号：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(ahtmldetail, @"工程类别：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(ahtmldetail, @"技术等级：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(ahtmldetail, @"项目经理：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(ahtmldetail, @"技术负责人：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(ahtmldetail, @"项目副经理：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(ahtmldetail, @"合同价：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(ahtmldetail, @"开工时间：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(ahtmldetail, @"竣工时间：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(ahtmldetail, @"建设单位（全称）：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(ahtmldetail, @"承包类型：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());

                            lv1.SubItems.Add(Regex.Replace(Regex.Match(ahtmldetail, @"安全负责人：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(ahtmldetail, @"结算价：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(ahtmldetail, @"交工时间：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(ahtmldetail, @"质量评定结果：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(ahtmldetail, @"交工验收报告：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(ahtmldetail, @"发证机关：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(ahtmldetail, @"竣工验收鉴定书：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Matches(ahtmldetail, @"发证机关：</span>([\s\S]*?)</span>")[1].Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(ahtmldetail, @"竣工验收备案机关：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(ahtmldetail, @"竣工验收备案时间：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(ahtmldetail, @"项目规模：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(ahtmldetail, @"项目获奖情况：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());

                            }
                            catch (Exception ex)
                            {

                            }


                            if (status == false)
                                return;

                        }

                        #endregion


                        #region 在建项目
                        string bhtml = method.PostUrl("http://jssccx.zjt.gov.cn:8532/index/cydw/sg/sg_buildingitems.htm", "sgUserId=" + userid, "JSESSIONID=ABD0A29DDBD61BE7E92577AFFBB62F28; _h_=\"eNrz4MoqTi0uzszP80yRNC7OTLIsLU5OTKnIM85OLMlLLMrKqsgtyOSGqvFOrSyuQdKBLC6Qk1hc4picDBRJTQnJzE3lSS5KTSwByoI4GLI + DAyMle1zT09BUQYXjQIAapo6DA == \"", "utf-8", "application/x-www-form-urlencoded", "");
                        MatchCollection bids = Regex.Matches(bhtml, @"openWin\('([\s\S]*?)'");
                        foreach (Match bid in bids)
                        {
                            string bhtmldetail = method.GetUrl("http://jssccx.zjt.gov.cn:8532/index/cydw/sg/sg_building_detail.htm?sgUserId=" + userid + "&id=" + bid.Groups[1].Value, "utf-8");
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(name);
                            lv1.SubItems.Add("在建项目");
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(bhtmldetail, @"工程名称：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(bhtmldetail, @"合同段名称：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(bhtmldetail, @"所在省份：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(bhtmldetail, @"开始桩号：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(bhtmldetail, @"结束桩号：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(bhtmldetail, @"工程类别：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(bhtmldetail, @"技术等级：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(bhtmldetail, @"项目经理：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(bhtmldetail, @"技术负责人：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(bhtmldetail, @"项目副经理：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(bhtmldetail, @"合同价：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(bhtmldetail, @"开工时间：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(bhtmldetail, @"合同完工时间：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(bhtmldetail, @"建设单位（全称）：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());
                            lv1.SubItems.Add(Regex.Replace(Regex.Match(bhtmldetail, @"承包类型：</span>([\s\S]*?)</span>").Groups[1].Value, "<[^>]+>", "").Trim());

                            if (status == false)
                                return;

                        }

                        #endregion


                    }
                }
            }
            catch (Exception ex)
            {

                label1.Text = ex.ToString();
            }
        }


        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
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
    }
}
