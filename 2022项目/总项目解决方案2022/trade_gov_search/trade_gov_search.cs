using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace trade_gov_search
{
    public partial class trade_gov_search : Form
    {
        public trade_gov_search()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }



        Thread thread;
        bool zanting = true;
        bool status = true;
        #region 主程序
        public void run()
        {



            StreamReader sr = new StreamReader(openFileDialog1.FileName, method.EncodingType.GetTxtType(textBox1.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存
            for (int i = 0; i < text.Length; i++)
            {
                label2.Text = "正在查询：" + text[i];
                try
                {
                    string aurl = "https://trade.gov.ng/bank-audit-service/formX/getByApplicationNumberOrFormNumber?formNumber=" + text[i];
                    string ahtml = method.GetUrl(aurl, "utf-8");

                    string formNumber = Regex.Match(ahtml, @"""formNumber"":""([\s\S]*?)""").Groups[1].Value;
                    string applicationNumber = Regex.Match(ahtml, @"""applicationNumber"":""([\s\S]*?)""").Groups[1].Value;
                    string applicantTINNumber = Regex.Match(ahtml, @"""applicantTINNumber"":""([\s\S]*?)""").Groups[1].Value;
                    string applicantName = Regex.Match(ahtml, @"""applicantName"":""([\s\S]*?)""").Groups[1].Value;
                    string applicantEndorsementDate = Regex.Match(ahtml, @"""applicantEndorsementDate"":""([\s\S]*?)""").Groups[1].Value;
                    string validUntil = Regex.Match(ahtml, @"""validUntil"":""([\s\S]*?)""").Groups[1].Value;



                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(formNumber);
                    lv1.SubItems.Add(applicationNumber);
                    lv1.SubItems.Add(applicantTINNumber);
                    lv1.SubItems.Add(applicantName);
                    lv1.SubItems.Add(applicantEndorsementDate);
                    lv1.SubItems.Add(validUntil);



                    Thread.Sleep(100);
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;
                }
                catch (Exception)
                {

                    continue;
                }

            }


   
    }

        #endregion


    private void button1_Click(object sender, EventArgs e)
    {
        #region 通用检测


        if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"HPiK"))
        {

            System.Diagnostics.Process.GetCurrentProcess().Kill();

            return;
        }

        #endregion

        status = true;
        if (textBox1.Text == "")
        {
            MessageBox.Show("请导入数据");
            return;
        }
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

    private void button3_Click(object sender, EventArgs e)
    {
        status = false;
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
}
}
