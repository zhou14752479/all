using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 主程序202103
{
    public partial class 裕丰数据处理 : Form
    {
        public 裕丰数据处理()
        {
            InitializeComponent();
        }


        /// 判断DataTale中判断某个字段中包含某个数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnName"></param>
        /// <param name="fieldData"></param>
        /// <returns></returns>
        public static Boolean IsIncludeData(DataTable dt, String columnName, string fieldData)
        {
            if (dt == null)
            {
                return false;
            }
            else
            {
                if (dt.Select(columnName + "='" + fieldData + "'").Length > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }


        DataTable dt=null;
        public void run()
        {
           DataTable dt1 = method.ExcelToDataTable(textBox2.Text, true);
            DataTable dt = method.ExcelToDataTable(textBox3.Text, true);
            try
            {

                // method.ShowDataInListView(dt, listView1);
                for (int i = 0; i < dt.Rows.Count; i++)
                {


                    DataRow dr = dt.Rows[i];
                    string xuhao = dr[0].ToString();
                    string address = dr[1].ToString();

                    string csprice = dr[2].ToString();
                    string czprice = dr[3].ToString();
                    string huxing = dr[4].ToString();
                    string mianji = dr[5].ToString();

                    string shi = Regex.Match(huxing,@"\d室").Groups[0].Value.Replace("室","");
                    string ting = Regex.Match(huxing, @"\d厅").Groups[0].Value.Replace("厅", "");
                    string wei= Regex.Match(huxing, @"\d卫").Groups[0].Value.Replace("卫", "");

                    string genjin = dr[9].ToString();
                    string tupian = dr[10].ToString();
                    bool baohan = IsIncludeData(dt1, "地址", address);
                    label3.Text = address+"： "+baohan;
                    if (baohan == false)
                    {
                        string a1 = Regex.Replace(address, @"\d{1,}楼\d{1,}房", "");
                        if (!a1.Contains("散盘"))
                        {
                            a1 = Regex.Replace(a1, @"[A-Z0-9]{1,}栋", "");
                            a1 = Regex.Replace(a1, @"[A-Z0-9]{1,}座", "");
                            a1 = Regex.Replace(a1, @"[A-Za-z]{1,}座", "");
                            a1 = Regex.Replace(a1, @".*路", "");
                            a1 = Regex.Replace(a1, @".*道", "");
                            a1 = Regex.Replace(a1, @"[A-Z0-9]{1,}单元", "");
                            a1 = Regex.Replace(a1, @"[A-Z0-9]{1,}号", "");
                            a1 = Regex.Replace(a1, @"[A-Z0-9]{1,}楼", "");
                            a1 = Regex.Replace(a1, @"[A-Z0-9]{1,}号楼", "");
                            a1 = Regex.Replace(a1, @"独栋", "");
                            a1 = Regex.Replace(a1, @"室", "");
                            a1 = Regex.Replace(a1, @"[0-9]{1,}", "");
                        }
                        string a2 = Regex.Match(address, @"[A-Z0-9]{1,}栋").Groups[0].Value;
                        if (a2 == "")
                        {
                            a2 = Regex.Match(address, @"[A-Z0-9]{1,}座").Groups[0].Value;
                        }
                        if (a2 == "")
                        {
                            a2 = Regex.Match(address, @"[A-Za-z]{1,}座").Groups[0].Value;
                        }


                        string a3 = Regex.Match(address, @"[A-Z0-9]{1,}单元").Groups[0].Value;
                        if (a3 == "")
                        {
                            a3 = Regex.Match(address, @"[A-Z0-9]{1,}号楼").Groups[0].Value;
                        }
                        string a4 = Regex.Match(address, @"\d{1,}楼").Groups[0].Value.Replace("楼", "");
                        string a5 = Regex.Match(address, @"\d{1,}房").Groups[0].Value.Replace("房", "");

                        if (a3 == "")
                        {
                            a3 = "0单元";
                        }

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(a1);
                        lv1.SubItems.Add(a2);
                        lv1.SubItems.Add(a3);
                        lv1.SubItems.Add(a4 + a5);

                        lv1.SubItems.Add(dr[6].ToString());
                        lv1.SubItems.Add(dr[7].ToString());
                        lv1.SubItems.Add(dr[8].ToString());
                        lv1.SubItems.Add(dr[9].ToString());
                        lv1.SubItems.Add(dr[10].ToString());

                        lv1.SubItems.Add(shi);
                        lv1.SubItems.Add(ting);
                        lv1.SubItems.Add(wei);


                        lv1.SubItems.Add(mianji);
                        lv1.SubItems.Add(csprice);
                        lv1.SubItems.Add(czprice);
                        if (genjin != "")
                        {
                            string[] values = genjin.Split(new string[] { "  " }, StringSplitOptions.None);
                            foreach (var value in values)
                            {
                                string[] text = value.Split(new string[] { "：" }, StringSplitOptions.None);
                                if (text.Length > 1)
                                {
                                    ListViewItem lv2 = listView2.Items.Add(xuhao); //使用Listview展示数据
                                    lv2.SubItems.Add("");
                                    lv2.SubItems.Add(text[1]);
                                    lv2.SubItems.Add(text[0]);

                                }
                            }

                        }

                        if (tupian != "")
                        {
                            string[] values = tupian.Split(new string[] { "  " }, StringSplitOptions.None);
                            foreach (var value in values)
                            {

                                ListViewItem lv3 = listView3.Items.Add(address); //使用Listview展示数据
                                lv3.SubItems.Add(value);
                                lv3.SubItems.Add("");
                                lv3.SubItems.Add("");

                            }

                        }

                    }
                }

            }
            catch (Exception)
            {

                
            }
        }

        string path = AppDomain.CurrentDomain.BaseDirectory+"images\\";
        private void 裕丰数据处理_Load(object sender, EventArgs e)
        {

        }

      
        Thread thread;
        private void button2_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
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
                textBox2.Text = openFileDialog1.FileName;



            }
        }

        private void button4_Click(object sender, EventArgs e)
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
                textBox3.Text = openFileDialog1.FileName;



            }
        }

        public void createNewExcel()
        {


        }
       
        #region 下载文件  【好用】
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="URLAddress">图片地址</param>
        /// <param name="subPath">图片所在文件夹</param>
        /// <param name="name">图片名称</param>
        public static void downloadFile(string URLAddress, string subPath, string name, string COOKIE)
        {
            try
            {
                string path = System.IO.Directory.GetCurrentDirectory();

                WebClient client = new WebClient();
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
                client.Headers.Add("Cookie", COOKIE);
                client.Headers.Add("Referer", "");
                if (false == System.IO.Directory.Exists(subPath))
                {
                    //创建pic文件夹
                    System.IO.Directory.CreateDirectory(subPath);
                }

                client.DownloadFile(URLAddress, subPath + "\\" + name);
            }
            catch (Exception ex)
            {

             //  MessageBox.Show(ex.ToString());
            }
        }



        #endregion
        public void downimage()
        {

            for (int i = 0; i < listView3.Items.Count; i++)
            {
               
                string picurls = listView3.Items[i].SubItems[1].Text;
                string newpath = path + listView3.Items[i].SubItems[0].Text;

                if (picurls != "")
                {
                    string[] picurl = picurls.Split(new string[] { "  " }, StringSplitOptions.None);

                    for (int j = 0; j < picurl.Length; j++)
                    {
                        label1.Text = "正在下载："+ picurl[j];

                        downloadFile(picurl[j], newpath, i+j + ".jpg", "");
                    }
                }

            }

            label1.Text = "下载完毕";
        }

        Thread thread1;
        private void button6_Click(object sender, EventArgs e)
        {
            if (thread1 == null || !thread1.IsAlive)
            {
                thread1 = new Thread(downimage);
                thread1.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
            method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
            method.DataTableToExcel(method.listViewToDataTable(this.listView3), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            listView2.Items.Clear();
            listView3.Items.Clear();
        }

        private void button7_Click(object sender, EventArgs e)
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
              
              DataTable dt1=  method.ExcelToDataTable(openFileDialog1.FileName, true);
                method.ShowDataInListView(dt1, listView3);


            }
        }
    }
}
