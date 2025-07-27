using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace 拼多多邮寄
{
    public partial class 拼多多邮寄 : Form
    {
        public 拼多多邮寄()
        {
            InitializeComponent();
        }
        
        DataTable dt = null;
        Thread thread;
        string cookie = "";
        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
          
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox2.Text = openFileDialog1.FileName;
               

            }
        }

       
        public void run()
        {

            try
            {
                dt = method.ExcelToDataTable(textBox2.Text, false);

                var tableGroups = method.SplitDataTable(dt, 10);

               
                for (int i = 0; i < tableGroups.Count; i++)
                {
                    status = false;

                    StringBuilder sb = new StringBuilder();
                    sb.Append("[");

                    for (int a= 0; a < tableGroups[i].Rows.Count; a++)
                    {

                        DataRow dr = tableGroups[i].Rows[a];
                        sb.Append("\"" + dr[0].ToString() + "\",");

                    }
                    sb.Append("]");

                    string postdata = "{\"exportType\":92,\"tnSeconds\":" + sb.ToString().Replace(",]", "]") + "}";


                    string html = method.PostUrl("https://mms.pinduoduo.com/mangosteen/consolidated/express/export/refund/proof", postdata, cookie, "utf-8", "application/json", "");


                    string taskid = Regex.Match(html, @"""taskId"":""([\s\S]*?)""").Groups[1].Value;
                   if(taskid!="")
                    {
                        label2.Text = "获取任务ID成功";
                    }
                   else
                    {

                        label2.Text = "获取任务ID失败"+html;
                        return;
                    }
                    while (true)
                    {
                        Thread.Sleep(5000);
                        run2(taskid);
                        if (status == true)
                            break;
                    }


                }


               

              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }

           
        }

        bool status = false;
        public void run2(string taskid)
        {

            try
            {
               
                string url = "https://mms.pinduoduo.com/mangosteen/consolidated/export/result?taskId=" + taskid;
                string html = method.GetUrl(url, "utf-8", cookie);

                
              
                MatchCollection trckNo = Regex.Matches(html, @"secondTrckList"":\[\{""trckNo"":""([\s\S]*?)""");
                MatchCollection goodsName = Regex.Matches(html, @"""goodsName"":""([\s\S]*?)""");
                MatchCollection spec = Regex.Matches(html, @"""spec"":""([\s\S]*?)""");
                MatchCollection goodsPrice = Regex.Matches(html, @"""goodsPrice"":([\s\S]*?),");


                if(goodsName.Count>0)
                {
                    status = true;  
                }
                else
                {
                    label2.Text = html;
                }
                for (int i = 0; i < goodsName.Count; i++)
                {
                    
                    double price = Convert.ToDouble(goodsPrice[i].Groups[1].Value) / 100;

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(trckNo[i].Groups[1].Value);
                    lv1.SubItems.Add(goodsName[i].Groups[1].Value);
                    lv1.SubItems.Add(spec[i].Groups[1].Value);
                    lv1.SubItems.Add(price.ToString());

                    if (listView1.Items.Count > 2)
                    {
                        this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                    }

                    label2.Text = "已生成："+ listView1.Items.Count;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
            button1.Enabled = true;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox2.Text=="")
            {
                MessageBox.Show("请导入表格");
                return;
            }
            cookie = textBox1.Text.Trim();
            button1.Enabled = false;
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

     

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void 拼多多邮寄_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}
