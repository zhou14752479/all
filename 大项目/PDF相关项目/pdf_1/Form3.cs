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

namespace pdf_1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        int rownum = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择所在文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = dialog.SelectedPath;
                string[] files = Directory.GetFiles(dialog.SelectedPath + @"\", "*.pdf");
                foreach (string file in files)
                {
                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(file);
                    lv1.SubItems.Add("未开始");
                }
            }
        }

        public void run()
        {
            try
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    listView1.Items[i].SubItems[2].Text = "正在读取...";
                    string pdfpath = listView1.Items[i].SubItems[1].Text;
                    string str = method.PDFtoStr(pdfpath);
                   textBox2.Text = str;
                    string lie25= Regex.Match(str, @"AD-[A-Z]{3}-\d{6}").Groups[0].Value;
                    string lie3 = Regex.Match(str, @"BP-\d{3,}-\d{7,}").Groups[0].Value;
                    string lie13 = Regex.Match(str, @"HASBRO \d{1,}").Groups[0].Value.Replace("HASBRO","").Trim();

                    if (lie13 == "")
                    {
                        lie13 = Regex.Match(str, @"HASBRO TOYS \d{1,}").Groups[0].Value.Replace("HASBRO TOYS ", "").Trim();
                    }


                    string lie38 = Regex.Match(str, @"CODE: ([\s\S]*?) ([\s\S]*?) ([\s\S]*?)CRE").Groups[1].Value.Trim();

                    string lie17 = Regex.Match(str, @"CODE: ([\s\S]*?) ([\s\S]*?) ([\s\S]*?)CRE").Groups[3].Value.Trim();
                    string lie18 = Regex.Match(str, @"CODE: ([\s\S]*?) ([\s\S]*?) ([\s\S]*?)CRE").Groups[2].Value.Trim();

                    if (lie25 == "")
                    {
                        lie25 = Regex.Match(str, @"AD-[A-Z]{3} -\d{6}").Groups[0].Value;
                    }


                    if (lie38 == "")
                    {
                        lie38 = Regex.Match(str, @"VENDOR CODE:([\s\S]*?)NER").Groups[1].Value.Trim();
                    }
                    if (lie38 == "")
                    {
                        lie38 = Regex.Match(str, @"VENDOR CODE([\s\S]*?)NER").Groups[1].Value.Trim();
                    }
                    if (lie38 == "")
                    {
                        lie38 = Regex.Match(str, @"Vendor code ([\s\S]*?) ").Groups[1].Value.Trim();
                    }





                    string lie23 = Regex.Match(str, @"SHIP TO CODE:\d{5,}").Groups[0].Value.Replace("SHIP TO CODE:", "").Trim();
                    if (lie23 == "")
                    {
                        lie23 = Regex.Match(str, @"SHIP TO CODE: \d{5,}").Groups[0].Value.Replace("SHIP TO CODE:", "").Trim();
                    }
                    if (lie23 == "")

                    {
                        lie23 = Regex.Match(str, @"SHIP TO CODE \d{5,}").Groups[0].Value.Replace("SHIP TO CODE", "").Trim();
                    }
                    if (lie23 == "")

                    {
                        lie23 = Regex.Match(str, @"Ship to code \d{5,}").Groups[0].Value.Replace("Ship to code", "").Trim();
                    }

                    MatchCollection lie10s = Regex.Matches(str, @"HFE PO#: ([\s\S]*?)Customer");
                    MatchCollection lie9s = Regex.Matches(str, @"Customer PO#: \d{7,}");
                    MatchCollection lie11s = Regex.Matches(str, @"Material #: [A-Z0-9]{9}");
                    MatchCollection lie54s = Regex.Matches(str, @"Material Desc: ([\s\S]*?)Quantity");
                    MatchCollection lie12s = Regex.Matches(str, @"Quantity: \d{1,6}");
                    MatchCollection lie14s = Regex.Matches(str, @"Dimension: ([\s\S]*?) x ([\s\S]*?) x ([\s\S]*?) CM");


                    string lie21 = Regex.Match(str, @"Ready Date: ([\s\S]*?)2021").Groups[1].Value.Trim()+" 2021";

                    for (int a = 0; a < lie10s.Count; a++)
                    {

                        try
                        {
                            string lie10a = Regex.Replace(lie10s[a].Groups[1].Value.Trim(), "/.*", "");
                            string lie10 = Regex.Match(lie10a, @"\d{9,}").Groups[0].Value;
                            string lie9 = lie9s[a].Groups[0].Value.Replace("Customer PO#:", "").Trim();
                            string lie11 = lie11s[a].Groups[0].Value.Replace("Material #:", "").Trim();
                            string lie54 = lie54s[a].Groups[1].Value.Replace("PD CHOMPIN MONSTER TRUCK", "").Trim();
                            string lie12 = lie12s[a].Groups[0].Value.Replace("Quantity:","").Trim();
                            string lie14 = lie14s[a].Groups[1].Value;
                            string lie15 = lie14s[a].Groups[2].Value;
                            string lie16 = lie14s[a].Groups[3].Value;


                            listView2.Items[rownum].SubItems[24].Text = lie25;
                            listView2.Items[rownum].SubItems[2].Text = lie3;
                            listView2.Items[rownum].SubItems[12].Text = lie13;
                            listView2.Items[rownum].SubItems[37].Text = lie38;
                            listView2.Items[rownum].SubItems[17].Text = lie18;
                            listView2.Items[rownum].SubItems[16].Text = lie17;
                            listView2.Items[rownum].SubItems[22].Text = lie23;
                            listView2.Items[rownum].SubItems[9].Text = lie10;
                            listView2.Items[rownum].SubItems[8].Text = lie9;
                            listView2.Items[rownum].SubItems[10].Text = lie11;
                            listView2.Items[rownum].SubItems[53].Text = lie54;
                            listView2.Items[rownum].SubItems[11].Text = lie12;
                            listView2.Items[rownum].SubItems[13].Text = lie14;
                            listView2.Items[rownum].SubItems[14].Text = lie15;
                            listView2.Items[rownum].SubItems[15].Text = lie16;
                            listView2.Items[rownum].SubItems[20].Text = lie21;

                            rownum = rownum + 1;

                        }
                        catch (Exception ex)
                        {

                            //textBox2.Text=ex.ToString();
                            continue;
                        }
                    }


    
                    if (status == false)
                        return;


                    listView1.Items[i].SubItems[2].Text = "已完成";
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        Thread thread;
        bool zanting = true;
        bool status = true;
        private void button6_Click(object sender, EventArgs e)
        {
            #region 通用检测

            if (DateTime.Now > Convert.ToDateTime("2021-06-06"))
            {
                MessageBox.Show("时间错误");
                return;
            }


            #endregion
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
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

        public void qingkong()
        {
            for (int i = 0; i < listView2.Items.Count; i++)
            {
                for (int j = 0; j < listView2.Columns.Count; j++)
                {
                    listView2.Items[i].SubItems[j].Text = "";
                }

            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
          Thread  thread = new Thread(qingkong);
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;

       
        private void Form3_Load(object sender, EventArgs e)
        {

            DataTable dt1 = myDLL.method.ExcelToDataTable(path + "Sample.xlsx", true);
            myDLL.method.ShowDataInListView(dt1, listView2);

           

        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
        }
    }
}
