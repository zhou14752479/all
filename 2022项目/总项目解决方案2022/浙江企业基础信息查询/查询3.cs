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
    public partial class 查询3 : Form
    {
        public 查询3()
        {
            InitializeComponent();
        }

        bool jiami = true;
        bool zanting = true;
        bool status = false;
        Thread thread;
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

       
        int total;
        public void run()
        {

            try
            {
                for (int a = 0; a < dt.Rows.Count; a++)
                {
                    total = total + 1;
                   

                    DataRow dr = dt.Rows[a];
                    string uid = dr[0].ToString();
                    string timestr = 查询2.GetTimeStamp();
                   string gregegedrgerheh= gdsgdgdgdgdstgfeewrwerw3r23r32rvxsvdsv.rgebgdgdvsdfsdvsdfsdvdsbgdsrt435b515sdfsdf("3",timestr);
                    string sign = gregegedrgerheh.Split(new string[] { "," }, StringSplitOptions.None)[0];
                    string zj_ggsjpt_sign = gregegedrgerheh.Split(new string[] { "," }, StringSplitOptions.None)[1];

                    string expiretime = gregegedrgerheh.Split(new string[] { "," }, StringSplitOptions.None)[2];

                    if (DateTime.Now > Convert.ToDateTime(expiretime))
                    {
                        MessageBox.Show("{\"msg\":\"非法请求\"}");
                        return;
                    }
                    string url = "http://app.gjzwfw.gov.cn/jimps/link.do";
                    string postdata = "param={\"from\":\"1\",\"key\":\"de95ac616f4d4d069545fe8587faa7b2\",\"requestTime\":\"" + timestr + "\",\"sign\":\"" + sign + "\",\"zj_ggsjpt_app_key\":\"ada72850-2b2e-11e7-985b-008cfaeb3d74\",\"zj_ggsjpt_sign\":\"" + zj_ggsjpt_sign + "\",\"zj_ggsjpt_time\":\"" + timestr + "\",\"AAB004\":\"" + uid + "\",\"AAE140\":\"110\",\"additional\":\"\"}";
                    
                    string html = 查询2.PostUrlDefault(url, postdata, "");
                    MessageBox.Show(html);
                    // textBox2.Text = html;
                    label3.Text = "正在查询：" + uid;
                    MatchCollection bAB010s = Regex.Matches(html, @"""bAB010"":""([\s\S]*?)""");
                    MatchCollection bAZ159s = Regex.Matches(html, @"""bAZ159"":""([\s\S]*?)""");
                    MatchCollection aAC147s = Regex.Matches(html, @"""aAC147"":""([\s\S]*?)""");
                    MatchCollection aAC058s = Regex.Matches(html, @"""aAC058"":""([\s\S]*?)""");
                    MatchCollection aAB004s = Regex.Matches(html, @"""aAB004"":""([\s\S]*?)""");
                    MatchCollection aAB301s = Regex.Matches(html, @"""aAB301"":""([\s\S]*?)""");
                    MatchCollection aAE030s = Regex.Matches(html, @"""aAE030"":""([\s\S]*?)""");
                    MatchCollection aAE140s = Regex.Matches(html, @"""aAE140"":""([\s\S]*?)""");
                    MatchCollection aAB001s = Regex.Matches(html, @"""aAB001"":""([\s\S]*?)""");
                    MatchCollection aAC001s = Regex.Matches(html, @"""aAC001"":""([\s\S]*?)""");
                    MatchCollection aAC002s = Regex.Matches(html, @"""aAC002"":""([\s\S]*?)""");
                    MatchCollection aac003s = Regex.Matches(html, @"""aac003"":""([\s\S]*?)""");
                    MatchCollection aAE031s = Regex.Matches(html, @"""aAE031"":([\s\S]*?),");
                    if (jiami == true)
                    {
                        uid = method.Base64Encode(Encoding.GetEncoding("utf-8"), uid);

                    }
                   
                    for (int i = 0; i < bAB010s.Count; i++)
                    {
                        string bAB010 = bAB010s[i].Groups[1].Value;
                        string bAZ159 = bAB010s[i].Groups[1].Value;
                        string aAC147 = aAC147s[i].Groups[1].Value;
                        string aAC058 = aAC058s[i].Groups[1].Value;
                        string aAB004 = aAB004s[i].Groups[1].Value;
                        string aAB301 = aAB301s[i].Groups[1].Value;
                        string aAE030 = aAE030s[i].Groups[1].Value;
                        string aAE140 = aAE140s[i].Groups[1].Value;
                        string aAB001 = bAB010s[i].Groups[1].Value;
                        string aAC001 = aAC001s[i].Groups[1].Value;
                        string aAC002 = aAC002s[i].Groups[1].Value;
                        string aac003 = aac003s[i].Groups[1].Value;
                        string aAE031 = aAE031s[i].Groups[1].Value.Replace("\"","");

                        if (jiami == true)
                        {
                            bAB010 = method.Base64Encode(Encoding.GetEncoding("utf-8"), bAB010s[i].Groups[1].Value);
                            bAZ159 = method.Base64Encode(Encoding.GetEncoding("utf-8"), bAZ159s[i].Groups[1].Value);
                            aAC147 = method.Base64Encode(Encoding.GetEncoding("utf-8"), aAC147s[i].Groups[1].Value);
                            aAC058 = method.Base64Encode(Encoding.GetEncoding("utf-8"), aAC058s[i].Groups[1].Value);
                            aAB004 = method.Base64Encode(Encoding.GetEncoding("utf-8"), aAB004s[i].Groups[1].Value);
                            aAB301 = method.Base64Encode(Encoding.GetEncoding("utf-8"), aAB301s[i].Groups[1].Value);
                            aAE030 = method.Base64Encode(Encoding.GetEncoding("utf-8"), aAE030s[i].Groups[1].Value);
                            aAE140 = method.Base64Encode(Encoding.GetEncoding("utf-8"), aAE140s[i].Groups[1].Value);
                            aAB001 = method.Base64Encode(Encoding.GetEncoding("utf-8"), aAB001s[i].Groups[1].Value);
                            bAB010 = method.Base64Encode(Encoding.GetEncoding("utf-8"), bAB010s[i].Groups[1].Value);
                            aAC001 = method.Base64Encode(Encoding.GetEncoding("utf-8"), aAC001s[i].Groups[1].Value);
                            aAC002 = method.Base64Encode(Encoding.GetEncoding("utf-8"), aAC002s[i].Groups[1].Value);
                            aac003 = method.Base64Encode(Encoding.GetEncoding("utf-8"), aac003s[i].Groups[1].Value);
                            aAE031 = method.Base64Encode(Encoding.GetEncoding("utf-8"), aAE031s[i].Groups[1].Value);

                        }

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(aac003);
                        lv1.SubItems.Add(bAB010);
                        lv1.SubItems.Add(aAE140);
                        lv1.SubItems.Add(bAZ159);
                        lv1.SubItems.Add(aAC058);
                        lv1.SubItems.Add(aAC147);
                        lv1.SubItems.Add(aAE030);
                        lv1.SubItems.Add(aAE031);
                        lv1.SubItems.Add(aAB001);
                        lv1.SubItems.Add(aAB004);
                        lv1.SubItems.Add(aAB301);
                        lv1.SubItems.Add(aAC001);
                        lv1.SubItems.Add(aAC002);
                        Thread.Sleep(100);
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

                    
                   
                   

                   

                }
                MessageBox.Show("完成");

            
            }
            catch (Exception ex)
            {


                MessageBox.Show(ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请先导入表格");
                return;
            }

            if (status == true)
            {
                status = false;
                label3.Text = "已停止";
            }
            else
            {
                status = true;
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(run);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
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
            //查询2.DataTableToExcelName(查询2.listViewToDataTable(this.listView1), "sample.xlsx", true);
            // 查询2.excel_fenlie();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "14752479")
            {
                MessageBox.Show("密码错误");
                return;
            }


            zanting = false;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                for (int j = 1; j < listView1.Columns.Count; j++)
                {
                    try
                    {

                        if (jiami == false)
                        {
                            if (j != 0)
                            {
                                listView1.Items[i].SubItems[j].Text = method.Base64Encode(Encoding.GetEncoding("utf-8"), listView1.Items[i].SubItems[j].Text);
                            }

                        }
                        else
                        {
                            if (j != 0)
                            {
                                listView1.Items[i].SubItems[j].Text = method.Base64Decode(Encoding.GetEncoding("utf-8"), listView1.Items[i].SubItems[j].Text) ;
                            }
                        }

                    }
                    catch (Exception ex)
                    {

                        continue;
                    }
                }
            }


            zanting = true;

            if (jiami == false)
            {
                jiami = true;
            }
            else
            {
                jiami = false;
            }
        }

        private void 查询3_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 查询3_Load(object sender, EventArgs e)
        {

        }
    }
}
