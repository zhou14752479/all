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

namespace 孔网删除
{
    public partial class 孔网删除 : Form
    {
        public 孔网删除()
        {
            InitializeComponent();
        }

        DataTable dt;
        string cookie = "shoppingCartSessionId=c15a1f2bd0a25905477a3b398b35f43f; kfz_uuid=6b41c6b7-a714-4283-8414-6652bf1201cf; reciever_area=24003000000; leftMenuStatus=-,-,-,-,+,+,+,+,+,+,+,+,+,+,+,+; kfz-tid=bf43ca1baa9b9312ebbdc305318bb7db; PHPSESSID=5a31f1a6f2758c7ce17a90cdcf42fede2e008369; kfz_trace=6b41c6b7-a714-4283-8414-6652bf1201cf|8007927|0478b5aab1c9d03d|";
        public string delete(string postdata)
        {
            try
            {
                string url = "https://seller.kongfz.com/pc/item/update";
                string html = method.PostUrlDefault(url,postdata,cookie);
                return html;
            }
            catch (Exception ex)
            {

                return "删除错误";
            }
        }
        public void run()
        {

            try
            {

               
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    DataRow dr = dt.Rows[i];
                    string isbn = dr[0].ToString();
                    string price = dr[7].ToString();
                    if(isbn.Trim()=="")
                    {
                        break;
                    }
                    string url = "https://seller.kongfz.com/pc/item/unsoldList?type=unsold&itemName=&author=&press=&itemSn=&isbn="+isbn+"&price_min=&price_max=&addTime_begin=&addTime_end=&soldTime_begin=&soldTime_end=&catId=&myCatId=&myCatIdName=&quality=&qualityName=&isDiscount=false&noPic=false&noStock=false&freeShipping=false&noMouldId=false&noItemSn=false&complete=true&catName=&mouldId=&mouldName=&pageShow=50&order_attr=&order_sort=&_=1645519862311";
                    string html = method.GetUrlWithCookie(url,cookie,"utf-8");

                    MatchCollection uids = Regex.Matches(html, @"""itemId"":([\s\S]*?),");
                    MatchCollection ps = Regex.Matches(html, @"""price"":""([\s\S]*?)""");
                    label1.Text = "正在删除："+isbn;

                    double bjprice = 0;
                    for (int j = 0; j < uids.Count; j++)
                    {
                       
                        try
                        {
                            string uid = uids[j].Groups[1].Value;
                            string p = ps[j].Groups[1].Value;

                            if (bjprice == 0)
                            {
                                bjprice = Convert.ToDouble(p);
                            }
                            else
                            {
                                if (Convert.ToDouble(p) < Convert.ToDouble(bjprice))
                                {
                                    bjprice = Convert.ToDouble(p);
                                }
                            }
                            


                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.ToString());
                            continue;
                        }
                    }
                    StringBuilder sb = new StringBuilder();
                    sb.Append("fields%5BisDelete%5D=1");
                    for (int j = 0; j < uids.Count; j++)
                    {

                        try
                        {
                            string uid = uids[j].Groups[1].Value;
                            string p = ps[j].Groups[1].Value;

                            if(Convert.ToDouble(p) != bjprice)
                            {
                                sb.Append("&itemIds%5B%5D="+uid);
                            }


                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.ToString());
                            continue;
                        }
                    }
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(isbn);
                    lv1.SubItems.Add(bjprice.ToString());

                    if(uids.Count>1)
                    {
                        string msg = delete(sb.ToString());
                        if (msg.Contains("status\":1,\"data\":true"))
                        {
                            lv1.SubItems.Add("删除成功，剩余最低价");
                        }
                        else
                        {
                            lv1.SubItems.Add("删除失败");
                        }
                    }
                    else
                    {
                        lv1.SubItems.Add("单个商品，无需删除");
                    }



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
                    Thread.Sleep(1000);


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

        private void button1_Click(object sender, EventArgs e)
        {
            if(登录.cookie=="")
            {
                MessageBox.Show("请先登录");
                return;
            }
            cookie = 登录.cookie;
            if (textBox1.Text=="")
            {
                MessageBox.Show("请导入表格");
                return;
            }

            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
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

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;

        }

        private void button7_Click(object sender, EventArgs e)
        {
            登录 login = new 登录();
            login.Show();
        }

        private void 孔网删除_Load(object sender, EventArgs e)
        {

        }
    }
}
