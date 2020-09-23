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
        public static string shoufawen = "";
        string path = AppDomain.CurrentDomain.BaseDirectory + "data\\";
        string weiPath = AppDomain.CurrentDomain.BaseDirectory + "datas.txt";
        #region  主程序
        public void run()
        {
            try
            
{
               

               
                for (int a = 0; a < richTextBox1.Lines.Length; a++)
                {
                    linkLabel1.Text = (a+1).ToString();
                   toolStripLabel1.Text = "正在抓取"+ richTextBox1.Lines[a]+ "........";
                    string url = "https://api.ipr.kuaifawu.com/xcx/tmsearch/index";


                    string postdata = textBox2.Text;

                    postdata = Regex.Replace(postdata, @"\d{6,}", richTextBox1.Lines[a].Trim());

                    string html = method.PostUrl(url, postdata, "", "utf-8");




                    Match a1 = Regex.Match(html, @"""MarkName"":""([\s\S]*?)""");
                    Match a2 = Regex.Match(html, @"UnionTypeCode"":([\s\S]*?),");
                    //Match a3 = Regex.Match(html, @"""StateDate2017"":""([\s\S]*?)""");
                    Match a4 = Regex.Match(html, @"""AppPerson"":""([\s\S]*?)""");
                    Match a5 = Regex.Match(html, @"""Addr"":""([\s\S]*?)""");  // 地址


                    Match a51 = Regex.Match(html, @"""Addr"":""([\s\S]*?)省");  // 省
                    Match a52 = Regex.Match(html, @"省([\s\S]*?)市");  // 市

                    Match a6 = Regex.Match(html, @"""AppDate"":""([\s\S]*?)""");
                    Match a7 = Regex.Match(html, @"""AgentName"":""([\s\S]*?)""");

                    //通知书发文抓取
                    string aurl = "https://zhiqingzhe.zqz510.com/api/tq/gti?uid=18f0514dacf94e9899136734417b7c83&an=" + richTextBox1.Lines[a].Trim() + "&ic=" + a2.Groups[1].Value.Trim(); ;

                    string ahtml = method.GetUrl(aurl, "utf-8");

                    Match aaa = Regex.Match(ahtml, @"""gglx"":""([\s\S]*?)"",""pd"":""([\s\S]*?)""");

                    //结束
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据      
                    lv1.SubItems.Add(richTextBox1.Lines[a]);
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

        #region listview转datable
        /// <summary>
        /// listview转datable
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        public static DataTable listViewToDataTable(ListView lv)
        {
            int i, j;
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Clear();
            dt.Columns.Clear();
            //生成DataTable列头
            for (i = 0; i < lv.Columns.Count; i++)
            {
                dt.Columns.Add(lv.Columns[i].Text.Trim(), typeof(String));
            }
            //每行内容
            for (i = 0; i < lv.Items.Count; i++)
            {
                dr = dt.NewRow();
                for (j = 0; j < lv.Columns.Count; j++)
                {
                    if (lv.Items[i].SubItems[10].Text.Contains(shangBiao.shoufawen))
                    {
                        dr[j] = lv.Items[i].SubItems[j].Text.Trim();
                    }
                    
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }
        #endregion

        private void shangBiao_Load(object sender, EventArgs e)
        {
            //StreamReader sr = new StreamReader(weiPath, Encoding.Default);
            ////一次性读取完 
            //string texts = sr.ReadToEnd();
            //string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            //richTextBox1.Text = texts;
            //sr.Close();

            //FileSystemWatcher fileSystemWatcher1 = new FileSystemWatcher();

            //this.fileSystemWatcher1.Changed += new FileSystemEventHandler(fileSystemWatcher1_Changed);

            //this.fileSystemWatcher1.EnableRaisingEvents = true;
            //this.fileSystemWatcher1.Path = path ;
            //this.fileSystemWatcher1.Filter = DateTime.Now.ToShortDateString().Replace("/", "-")+".txt";
            //this.fileSystemWatcher1.IncludeSubdirectories = false;//不监视子目录
        }

        private void fileSystemWatcher1_Changed(object sender, System.IO.FileSystemEventArgs e)
        {

            //FileStream fs = File.Open(path + DateTime.Now.ToShortDateString().Replace("/", "-") + ".txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //StreamReader sr = new StreamReader(fs, Encoding.Default);//流读取器
            //string texts = sr.ReadToEnd();
            //string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
          
            

        }

        private void ShangBiao_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0); //点确定的代码
            }
            else
            {
                e.Cancel=true;
            }
        }

     

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
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
            method.DataTableToExcel(listViewToDataTable(this.listView1), "Sheet1", true);
        }

      

        private void Button2_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                shangBiao.shoufawen = checkBox1.Text;
            }
            if (checkBox2.Checked == true)
            {
                shangBiao.shoufawen = checkBox2.Text;
            }
            if (checkBox3.Checked == true)
            {
                shangBiao.shoufawen = checkBox3.Text;
            }
            if (checkBox4.Checked == true)
            {
                shangBiao.shoufawen = checkBox4.Text;
            }











            string date = DateTime.Now.ToShortDateString().Replace("/", "-");
            if (radioButton1.Checked == true)
            {
                date = DateTime.Now.ToShortDateString().Replace("/", "-");
            }
            if (radioButton2.Checked == true)
            {
                date = DateTime.Now.AddDays(-1).ToShortDateString().Replace("/", "-");
            }

          if (radioButton3.Checked == true)
            {
                date = DateTime.Now.AddDays(-2).ToShortDateString().Replace("/", "-");
            }


            if (File.Exists(path + date + ".txt"))
            {
                listView1.Items.Clear();
                StreamReader sr = new StreamReader(path + date + ".txt", Encoding.GetEncoding("utf-8"));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {
                    string[] values = text[i].Split(new string[] { "#" }, StringSplitOptions.None);
                    if (values.Length > 10)
                    {
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(values[0]);
                        lv1.SubItems.Add(values[1]);
                        lv1.SubItems.Add(values[2]);
                        lv1.SubItems.Add(values[3]);
                        lv1.SubItems.Add(values[4]);
                        lv1.SubItems.Add(values[5]);
                        lv1.SubItems.Add(values[6]);
                        lv1.SubItems.Add(values[7]);
                        lv1.SubItems.Add(values[8]);
                        lv1.SubItems.Add(values[9]);
                        lv1.SubItems.Add(values[10]);
                    }

                }
                sr.Close();
                method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
                DialogResult dr = MessageBox.Show("需要删除数据吗？", "删除", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    listView1.Items.Clear();
                }
                else
                {
                  
                }
            }

            else
            {
                MessageBox.Show("未查询到相关数据");
            }

          
        }

        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void SplitContainer1_Panel1_MouseEnter(object sender, EventArgs e)
        {
          
        }

        private void SplitContainer1_Panel1_MouseHover(object sender, EventArgs e)
        {
            FileStream fs1 = new FileStream(weiPath, FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1);
            sw.WriteLine(richTextBox1.Text);
            sw.Close();
            fs1.Close();
           
            linkLabel3.Text = richTextBox1.Lines.Length.ToString();
          
            
        }

        private void ToolStripButton5_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }
    }
}
