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

namespace 浙江企业基础信息查询
{
    public partial class 浙江企业基础信息查询 : Form
    {
        public 浙江企业基础信息查询()
        {
            InitializeComponent();
        }

        public void run()
        {
            try
            {
                for (int a = 0; a< dt.Rows.Count; a++)
                {

                    
                    DataRow dr = dt.Rows[a];
                    string uid = dr[0].ToString();
                    string url = "http://app.gjzwfw.gov.cn/jimps/link.do";
                    label3.Text = "正在查询：" + uid;
                    string timestr = method.GetTimeStamp() + "000";
                    string sign = method.GetMD5("qyjbxxcxzj" + timestr);
                    string zj_ggsjpt_sign = method.GetMD5("ada72850-2b2e-11e7-985b-008cfaeb3d74" + "995e00df72f14bbcb7833a9ca063adef" + timestr);
                    string postdata = "param=%7B%22from%22%3A%222%22%2C%22key%22%3A%22b4842fe0fadc44398d674c786a583f8e%22%2C%22requestTime%22%3A%22" + timestr + "%22%2C%22sign%22%3A%22" + sign + "%22%2C%22zj_ggsjpt_app_key%22%3A%22ada72850-2b2e-11e7-985b-008cfaeb3d74%22%2C%22zj_ggsjpt_sign%22%3A%22" + zj_ggsjpt_sign + "%22%2C%22zj_ggsjpt_time%22%3A%22" + timestr + "%22%2C%22uniscId%22%3A%22"+uid+"%22%2C%22companyName%22%3A%22%22%2C%22registerNo%22%3A%22%22%2C%22entType%22%3A%22E%22%2C%22additional%22%3A%22%22%7D";
                    string html = method.PostUrlDefault(url, postdata, "");

                    string company = Regex.Match(html, @"""companyName"":""([\s\S]*?)""").Groups[1].Value;

                    string financeInfo = Regex.Match(html, @"financeInfo([\s\S]*?)\]").Groups[1].Value;
                    string liaisonInfo = Regex.Match(html, @"liaisonInfo([\s\S]*?)\]").Groups[1].Value;



                    MatchCollection names = Regex.Matches(html, @"shareholderName='([\s\S]*?)'");
                    MatchCollection cards = Regex.Matches(html, @"paperNo='([\s\S]*?)'");

                    for (int i = 0; i < names.Count; i++)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(uid);
                        lv1.SubItems.Add(company);
                        lv1.SubItems.Add(names[i].Groups[1].Value);
                        lv1.SubItems.Add(cards[i].Groups[1].Value);

                        if (listView1.Items.Count > 2)
                        {
                            this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                        }


                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;
                    }

                    string aname = Regex.Match(financeInfo, @"nAME"":""([\s\S]*?)""").Groups[1].Value;
                    string acard = Regex.Match(financeInfo, @"cERNO"":""([\s\S]*?)""").Groups[1].Value;
                    string atel = Regex.Match(financeInfo, @"mOBTEL"":""([\s\S]*?)""").Groups[1].Value;
                    if(aname!="")
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(uid);
                        lv1.SubItems.Add(company);
                        lv1.SubItems.Add(aname);
                        lv1.SubItems.Add(acard);
                        lv1.SubItems.Add(atel);
                    }


                    string bname = Regex.Match(liaisonInfo, @"nAME"":""([\s\S]*?)""").Groups[1].Value;
                    string bcard = Regex.Match(liaisonInfo, @"cERNO"":""([\s\S]*?)""").Groups[1].Value;
                    string btel = Regex.Match(liaisonInfo, @"mOBTEL"":""([\s\S]*?)""").Groups[1].Value;

                    if (bname != "")
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(uid);
                        lv1.SubItems.Add(company);
                        lv1.SubItems.Add(bname);
                        lv1.SubItems.Add(bcard);
                        lv1.SubItems.Add(btel);
                    }
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }


        bool zanting = true;
        bool status = true;
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
        DataTable dt;
        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            // openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox1.Text = openFileDialog1.FileName;
                dt = method.ExcelToDataTable(textBox1.Text, true);

            }
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

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
