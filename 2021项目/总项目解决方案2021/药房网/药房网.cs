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

namespace 药房网
{
    public partial class 药房网 : Form
    {
        public 药房网()
        {
            InitializeComponent();
        }

        private void 药房网_Load(object sender, EventArgs e)
        {

        }
        DataTable dt = null;
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            // openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox1.Text = openFileDialog1.FileName;
                dt = method.ExcelToDataTable(textBox1.Text, true);

            }
        }







        string[] cates = { "10", "11" };
        public void run1()
        {
            try
            {
                foreach (string cate in cates)
                {

                    for (int page = 1; page < 101; page++)
                    {
                        string url = "https://www.yaofangwang.com/catalog-" + cate + "-p" + page + ".html";
                        string html = method.GetUrl(url, "utf-8");
                        MatchCollection ids = Regex.Matches(html, @"<div class=""info medicineInfo"" rel=""([\s\S]*?)""");


                        for (int j = 0; j < ids.Count; j++)
                        {

                            string id = ids[j].Groups[1].Value;
                            textBox2.Text = "正在读取：" + id;
                            string aurl = "https://pub.yaofangwang.com/4000/4000/0/?__cmd=guest.medicine.getMedicineDetail%20as%20goodsInfo%2Cguest.medicine.getShopMedicines%20as%20shopsInfo&__params=%7B%22goodsInfo%22%3A%7B%22mid%22%3A%22" + id + "%22%7D%2C%22shopsInfo%22%3A%7B%22conditions%22%3A%7B%22sort%22%3A%22sprice%22%2C%22sorttype%22%3A%22asc%22%2C%22medicineid%22%3A%22" + id + "%22%7D%2C%22pageSize%22%3A10000%2C%22pageIndex%22%3A1%7D%7D";
                            string ahtml = method.GetUrl(aurl, "utf-8");

                            MatchCollection stores = Regex.Matches(ahtml, @"""store_title"":""([\s\S]*?)""");
                            MatchCollection store_prices = Regex.Matches(ahtml, @"""real_price"":([\s\S]*?),");

                            string title = Regex.Match(ahtml, @"""medicine_name"":""([\s\S]*?)""").Groups[1].Value;
                            string guige = Regex.Match(ahtml, @"""standard"":""([\s\S]*?)""").Groups[1].Value;
                            string changjia = Regex.Match(ahtml, @"生产厂家：([\s\S]*?)""").Groups[1].Value;

                            string guoyaozhunzi = Regex.Match(ahtml, @"""authorized_code"":""([\s\S]*?)""").Groups[1].Value;
                            string price_min = Regex.Match(ahtml, @"""price_min"":([\s\S]*?),").Groups[1].Value;

                            for (int a = 0; a < stores.Count; a++)
                            {
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(title);
                                lv1.SubItems.Add(guige);
                                lv1.SubItems.Add(changjia);
                                lv1.SubItems.Add(guoyaozhunzi);

                                lv1.SubItems.Add(stores[a].Groups[1].Value);
                                lv1.SubItems.Add(store_prices[a].Groups[1].Value);

                                lv1.SubItems.Add(price_min);
                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                                if (status == false)
                                    return;
                            }


                        }

                        Thread.Sleep(500);

                    }
                }

            }
            catch (Exception ex)
            {
                textBox2.Text = ex.ToString();
            }


        }



        public void run2()
        {
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    DataRow dr = dt.Rows[i];
                    //string keyword = System.Web.HttpUtility.UrlEncode(dr[1].ToString());
                    string keyword = dr[1].ToString();
                    textBox2.Text = "正在读取：" + keyword;
                    string url = "https://www.yaofangwang.com/search.html?keyword=" + keyword;
                    string html = method.GetUrl(url, "utf-8");
                    MatchCollection ids = Regex.Matches(html, @"<div class=""info medicineInfo"">([\s\S]*?)medicine\/([\s\S]*?)\/");
                    MessageBox.Show(ids.Count.ToString());

                    for (int j = 0; j < ids.Count; j++)
                    {
                        string id = ids[j].Groups[2].Value;

                        string aurl = "https://pub.yaofangwang.com/4000/4000/0/?__cmd=guest.medicine.getMedicineDetail%20as%20goodsInfo%2Cguest.medicine.getShopMedicines%20as%20shopsInfo&__params=%7B%22goodsInfo%22%3A%7B%22mid%22%3A%22" + id + "%22%7D%2C%22shopsInfo%22%3A%7B%22conditions%22%3A%7B%22sort%22%3A%22sprice%22%2C%22sorttype%22%3A%22asc%22%2C%22medicineid%22%3A%22" + id + "%22%7D%2C%22pageSize%22%3A10000%2C%22pageIndex%22%3A1%7D%7D";
                        string ahtml = method.GetUrl(aurl, "utf-8");

                        MatchCollection stores = Regex.Matches(ahtml, @"""store_title"":""([\s\S]*?)""");
                        MatchCollection store_prices = Regex.Matches(ahtml, @"""real_price"":([\s\S]*?),");

                        string title = Regex.Match(ahtml, @"""medicine_name"":""([\s\S]*?)""").Groups[1].Value;
                        string guige = Regex.Match(ahtml, @"""standard"":""([\s\S]*?)""").Groups[1].Value;
                        string changjia = Regex.Match(ahtml, @"生产厂家：([\s\S]*?)""").Groups[1].Value;

                        string guoyaozhunzi = Regex.Match(ahtml, @"""authorized_code"":""([\s\S]*?)""").Groups[1].Value;
                        string price_min = Regex.Match(ahtml, @"""price_min"":([\s\S]*?),").Groups[1].Value;

                        for (int a = 0; a < stores.Count; a++)
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(title);
                            lv1.SubItems.Add(guige);
                            lv1.SubItems.Add(changjia);
                            lv1.SubItems.Add(guoyaozhunzi);

                            lv1.SubItems.Add(stores[a].Groups[1].Value);
                            lv1.SubItems.Add(store_prices[a].Groups[1].Value);

                            lv1.SubItems.Add(price_min);

                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)
                                return;
                        }


                    }

                    Thread.Sleep(500);



                }
            }
            catch (Exception ex)
            {
                textBox2.Text = ex.ToString();
            }


        }

        bool zanting = true;
        bool status = true;
        Thread thread;

        private void button6_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"kOuseov"))
            {

                return;
            }



            #endregion
            if (radioButton1.Checked == true)
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(run1);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }

            }
            if (radioButton2.Checked == true)
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(run2);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }

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

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
