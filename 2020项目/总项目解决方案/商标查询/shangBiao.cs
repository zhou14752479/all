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
using helper;

namespace 商标查询
{
    public partial class shangBiao : Form
    {
        public shangBiao()
        {
            InitializeComponent();
        }

        bool zanting = true;

        string path = AppDomain.CurrentDomain.BaseDirectory + "data\\";
        #region  主程序
        public void run()
        {
            try
            
{
               
                string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int a = 0; a < text.Length - 1; a++)
                {
                   toolStrip1.Text = "正在抓取"+text[a]+"........";
                    string url = "https://api.ipr.kuaifawu.com/xcx/tmsearch/index";


                    string postdata = textBox2.Text;

                    postdata = Regex.Replace(postdata, @"\d{6,}", text[a].Trim());

                    string html = method.PostUrl(url, postdata, "", "utf-8");







                    Match a1 = Regex.Match(html, @"""MarkName"":""([\s\S]*?)""");
                    Match a2 = Regex.Match(html, @"UnionTypeCode"":([\s\S]*?),");
                    Match a3 = Regex.Match(html, @"""StateDate2017"":""([\s\S]*?)""");
                    Match a4 = Regex.Match(html, @"""AppPerson"":""([\s\S]*?)""");
                    Match a5 = Regex.Match(html, @"""Addr"":""([\s\S]*?)""");  // 地址


                    Match a51 = Regex.Match(html, @"""Addr"":""([\s\S]*?)省");  // 省
                    Match a52 = Regex.Match(html, @"省([\s\S]*?)市");  // 市

                    Match a6 = Regex.Match(html, @"""AppDate"":""([\s\S]*?)""");
                    Match a7 = Regex.Match(html, @"""AgentName"":""([\s\S]*?)""");

                    //通知书发文抓取
                    string aurl = "https://zhiqingzhe.zqz510.com/api/tq/gti?uid=18f0514dacf94e9899136734417b7c83&an=" + text[a].Trim() + "&ic=" + a2.Groups[1].Value.Trim(); ;

                    string ahtml = method.GetUrl(aurl, "utf-8");

                    Match aaa = Regex.Match(ahtml, @"""gglx"":""([\s\S]*?)"",""pd"":""([\s\S]*?)""");

                    //结束
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据      
                    lv1.SubItems.Add(text[a]);
                    lv1.SubItems.Add(a1.Groups[1].Value);
                    lv1.SubItems.Add(a2.Groups[1].Value);

                    lv1.SubItems.Add(a4.Groups[1].Value);
                    lv1.SubItems.Add("中国");
                    lv1.SubItems.Add(a51.Groups[1].Value);
                    lv1.SubItems.Add(a52.Groups[1].Value);
                    lv1.SubItems.Add(a5.Groups[1].Value);
                    lv1.SubItems.Add(a6.Groups[1].Value);
                    lv1.SubItems.Add(a7.Groups[1].Value);
                    lv1.SubItems.Add(aaa.Groups[2].Value + aaa.Groups[1].Value);//收发文日期

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    Thread.Sleep(1000);



                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion
     


        private void shangBiao_Load(object sender, EventArgs e)
        {

        }

       

        private void ShangBiao_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0); //点确定的代码
            }
            else
            { //点取消的代码 
            }
        }

     

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            #region 通用验证

            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php", "utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == "16.16.16.16")
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
                MessageBox.Show("IP不符");

            }
            #endregion
        }

        private void ToolStripButton2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void ToolStripButton3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void ToolStripButton4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void 新增ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Visible = true;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader(openFileDialog1.FileName, Encoding.Default);
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int i = 0; i < text.Length; i++)
            {

                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                lv1.SubItems.Add(text[i]);


            }
        }
    }
}
