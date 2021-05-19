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

namespace 主程序202105
{
    public partial class 京喜SKU提取 : Form
    {
        public 京喜SKU提取()
        {
            InitializeComponent();
        }

        private void 京喜SKU提取_Load(object sender, EventArgs e)
        {

        }


        public void run()
        {

            try
            {
                if (textBox2.Text == "")
                {
                    MessageBox.Show("请导入关键字");
                    return;
                }



                StreamReader sr = new StreamReader(textBox2.Text, method.EncodingType.GetTxtType(textBox2.Text));
             
                //一次性读取完 
                string texts = sr.ReadToEnd();

                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {

                    for (int page = 1; page <30; page++)
                    {


                        string url = "https://m.jingxi.com/searchv3/jxjson?client=android&networkType=&screen=1920*1080&body=%7B%22isCorrect%22%3A%221%22%2C%22pvid%22%3A%22f50a21e6cb59b98844814fcd4c7975f2_1620790711544%22%2C%22sort%22%3A%220%22%2C%22JXFWareFilter%22%3A%221%22%2C%22price%22%3A%7B%22min%22%3A%22" + Convert.ToInt32(textBox1.Text.Trim()) + "%22%2C%22max%22%3A%22" + Convert.ToInt32(textBox4.Text.Trim())+"%22%7D%2C%22keyword%22%3A%22"+ System.Web.HttpUtility.UrlEncode(text[i]) + "%22%2C%22pagesize%22%3A%2210%22%2C%22multi_select%22%3A%221%22%2C%22stid%22%3A1%2C%22tabids%22%3A%221%7C2%7C3%7C4%22%2C%22page%22%3A%22"+page+"%22%7D&sceneval=2&g_login_type=1&g_ty=ajax";
                        label3.Text = "正在查询：" + url;
                        if (url != "")
                        {
                            string html = method.GetUrl(url, "utf-8");
                            MatchCollection items = Regex.Matches(html, @"spuId([\s\S]*?)gfdType");
                            for (int j = 0; j < items.Count; j++)
                            {
                                if (!items[j].Groups[1].Value.Contains("起售"))
                                {
                                    string itemid = Regex.Match(items[j].Groups[1].Value, @"""wareId"":""([\s\S]*?)""").Groups[1].Value;
                                    string price = Regex.Match(items[j].Groups[1].Value, @"""jdPrice"":""([\s\S]*?)""").Groups[1].Value;
                                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                    lv1.SubItems.Add(itemid);
                                    lv1.SubItems.Add(price);
                                    if (status == false)
                                        return;
                                }
                            }
                         

                            Thread.Sleep(100);

                        }
                    }
                }
                

            }
            catch (Exception ex)
            {

                label1.Text = ex.ToString();
            }
        }


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

        bool status = true;
        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Filter = "txt|*.txt";
            sfd.Title = "txt文件导出";


            if (sfd.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = sfd.FileName;
             

            }
        }
    }
}
