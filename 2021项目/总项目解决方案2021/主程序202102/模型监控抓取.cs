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

namespace 主程序202102
{
    public partial class 模型监控抓取 : Form
    {
        public 模型监控抓取()
        {
            InitializeComponent();
        }


        public bool panduan(string id)
        {
            if (checkBox2.Checked == true)
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (listView1.Items[i].SubItems[2].Text == id)
                    {
                        return true;
                    }

                }
                return false;
            }
            else
            {
                return false;
            }
        }

        #region 主程序
        public void run()
        {
            string[] types = { "3dmoxing_s47p1", "mf3dmx_p1",  "3dmoxing_s5p1" };
            foreach (var item in types)
            {
                string fufei = "免费";
                if (item != "mf3dmx_p1")
                {
                    fufei = "付费";
                }

                string firstdate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                for (int i = 1; i < 51; i++)
                {

                    //if (firstdate != DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"))
                    //{

                    //    break;
                    //}
                    string url = "https://3d.znzmo.com/"+item+"/p_"+i+".html";

                    string html = method.GetUrl(url, "utf-8");

                    MatchCollection uids = Regex.Matches(html, @"{""skuId"":([\s\S]*?),");


                    for (int j = 0; j < uids.Count; j++)
                    {
                        if (!panduan(uids[j].Groups[1].Value))
                        {
                            string aurl = "https://3d.znzmo.com/3dmoxing/" + uids[j].Groups[1].Value + ".html";
                            string ahtml = method.GetUrl(aurl, "utf-8");
                            try
                            {

                                Match title = Regex.Match(ahtml, @"<title class=""next-head"">([\s\S]*?)_");
                                Match aid = Regex.Match(ahtml, @"{""skuId"":""([\s\S]*?)""");
                                Match a1 = Regex.Match(ahtml, @"更新时间：</span><span>([\s\S]*?)</span>");
                                Match a2 = Regex.Match(ahtml, @"文件大小：</span><span>([\s\S]*?)<");
                                Match a3 = Regex.Match(ahtml, @"风格<!-- -->:</span><span>([\s\S]*?)</span>");
                                Match a4 = Regex.Match(ahtml, @"贴图<!-- -->:</span><span>([\s\S]*?)</span>");
                                Match a5 = Regex.Match(ahtml, @"VR/CR<!-- -->:</span><span>([\s\S]*?)</span>");
                                Match a6 = Regex.Match(ahtml, @"灯光详情<!-- -->:</span><span>([\s\S]*?)</span>");
                                Match a7 = Regex.Match(ahtml, @"<span>MAX版本<!-- -->:</span>([\s\S]*?)</span>");

                                //if (a1.Groups[1].Value != DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"))
                                //{

                                //    firstdate = a1.Groups[1].Value;
                                //    break;
                                //}
                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(title.Groups[1].Value);
                                lv1.SubItems.Add(aid.Groups[1].Value);
                                lv1.SubItems.Add(a1.Groups[1].Value);
                                lv1.SubItems.Add(a2.Groups[1].Value);
                                lv1.SubItems.Add(a3.Groups[1].Value);
                                lv1.SubItems.Add(a4.Groups[1].Value);
                                lv1.SubItems.Add(a5.Groups[1].Value);
                                lv1.SubItems.Add(a6.Groups[1].Value);
                                lv1.SubItems.Add(Regex.Replace(a7.Groups[1].Value, "<[^>]+>", ""));
                                lv1.SubItems.Add(fufei);

                                Thread.Sleep(1000);
                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                                if (status == false)
                                    return;

                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }

                    }
                    Thread.Sleep(1000);
                }
            }
        }

        #endregion

        Thread thread;
        bool zanting = true;
        bool status = true;
        private void 模型监控抓取_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"QUpuM7"))
            {
                MessageBox.Show("");
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

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
        #region listview转datable
        /// <summary>
        /// listview转datable
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        public  DataTable listViewToDataTable(ListView lv)
        {
            int i, j;
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Clear();
            dt.Columns.Clear();
            //lv.Columns.Count
            //生成DataTable列头
            for (i = 0; i < lv.Columns.Count; i++)
            {
                dt.Columns.Add(lv.Columns[i].Text.Trim(), typeof(String));
            }
            //每行内容
            for (i = 0; i < lv.Items.Count; i++)
            {
                if (panduan(i))
                {
                    dr = dt.NewRow();
                    for (j = 0; j < lv.Columns.Count; j++)
                    {
                        try
                        {


                            dr[j] = lv.Items[i].SubItems[j].Text.Trim();



                        }
                        catch
                        {

                            continue;
                        }

                    }
                    dt.Rows.Add(dr);
                }
            }
                

            return dt;
        }
        #endregion


        public bool panduan(int i)
        {
            if (listView1.Items[i].SubItems[3].Text.Trim() != dateTimePicker1.Value.ToString("yyyy-MM-dd"))
            {
                return false;

            }
            else
            {
                if (listView1.Items[i].SubItems[7].Text.Trim() != comboBox1.Text && comboBox1.Text != "全部")
                {
                    return false;
                }
                else
                {
                    if (listView1.Items[i].SubItems[10].Text.Trim() != comboBox2.Text && comboBox2.Text != "全部")
                    {
                        return false;
                    }
                    else
                    {
                        if (listView1.Items[i].SubItems[5].Text.Trim() != comboBox3.Text && comboBox3.Text != "全部")
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }

            }

        }
        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                timer1.Start();
            }
            else
            {
                timer1.Stop();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void 模型监控抓取_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
