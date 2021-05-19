using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
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

namespace 地图采集器
{
    public partial class 地图采集器 : Form
    {
        public 地图采集器()
        {
            InitializeComponent();
        }

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                 request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                string content = reader.ReadToEnd();


                reader.Close();
                response.Close();
                return content;



            }
            catch (System.Exception ex)
            {
                ex.ToString();

            }
            return "";
        }
        #endregion

        bool zanting = true;
        private Point mPoint = new Point();
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
            }
        }


        Dictionary<string, string> areadics = new Dictionary<string, string>();
        ///// <summary>
        ///// 获取经纬度
        ///// </summary>
        ///// <param name="city"></param>
        ///// <returns></returns>
        //public ArrayList getlat(string city)
        //{
        //    ArrayList areas = new ArrayList();
        //    string url = "http://www.jsons.cn/lngcode/?keyword="+ System.Web.HttpUtility.UrlEncode(city) + "&txtflag=0";
        //    string html = GetUrl(url);

        //    Match ahtml = Regex.Match(html, @"<table class=""table table-bordered table-hover"">([\s\S]*?)</table>");

        //    MatchCollection values = Regex.Matches(ahtml.Groups[1].Value, @"<td>([\s\S]*?)</td>");

        //    for (int i = 0; i < values.Count; i++)
        //    {
        //        if (values[i].Groups[1].Value.Contains("1"))
        //        {
        //            areas.Add(values[i].Groups[1].Value.Replace("，","%2C").Trim());
        //        }

        //    }
        //    return areas;
        //}


        Dictionary<string, string> dics = new Dictionary<string, string>();
        /// <summary>
        /// 获取地区
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public ArrayList getarea(string city)
        {
            ArrayList areas = new ArrayList();
            string url = "http://www.jsons.cn/lngcode/?keyword=" + System.Web.HttpUtility.UrlEncode(city) + "&txtflag=0";
            string html = GetUrl(url);

            Match ahtml = Regex.Match(html, @"<table class=""table table-bordered table-hover"">([\s\S]*?)</table>");

            MatchCollection values = Regex.Matches(ahtml.Groups[1].Value, @"<td>([\s\S]*?)</td>");

            for (int i = 0; i < values.Count; i++)
            {
                //MessageBox.Show(values[i].Groups[1].Value);
                if (values[i].Groups[1].Value.Contains("区") || values[i].Groups[1].Value.Contains("县"))
                {
                    if (!comboBox4.Items.Contains(values[i].Groups[1].Value.Trim()))
                    {
                        comboBox4.Items.Add(values[i].Groups[1].Value.Trim());
                        dics.Add(values[i].Groups[1].Value.Trim(), values[i + 1].Groups[1].Value.Replace("，", "%2C").Trim());
                    }
                }

            }
            return areas;
        }


       
      
        /// <summary>
        /// 获取关键词
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public ArrayList getkeywords()
        {
            ArrayList keywords = new ArrayList();
            for (int i = 0; i < listView3.Items.Count; i++)
            {
                keywords.Add(listView3.Items[i].SubItems[0].Text);
            }
            return keywords;
        }
       
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
            ArrayList keywords = getkeywords();
            if (keywords.Count == 0)
            {
                MessageBox.Show("请添加关键字");
                return;
            }


            try
            {
              
              

                foreach (string keyword in keywords)
                {



                    for (int a = 0; a < listView2.Items.Count; a++)
                    {
                        string lat = dics[listView2.Items[a].Text];

                            for (int page = 1; page < 100; page++)
                            {


                                string url = "https://restapi.amap.com/v3/place/around?appname=1e3bb24ab8f75ba78a7cf8a9cc4734c6&key=1e3bb24ab8f75ba78a7cf8a9cc4734c6&keywords=" + System.Web.HttpUtility.UrlEncode(keyword) + "&location=" + lat + "&logversion=2.0&page=" + page + "&platform=WXJS&s=rsx&sdkversion=1.2.0";
                                string html = GetUrl(url);



                                MatchCollection names = Regex.Matches(html, @"""name"":""([\s\S]*?)""");
                                MatchCollection tels = Regex.Matches(html, @"""tel"":([\s\S]*?),");
                                MatchCollection address = Regex.Matches(html, @"""address"":([\s\S]*?),");
                                MatchCollection pros = Regex.Matches(html, @"""pname"":([\s\S]*?),");
                                MatchCollection citynames = Regex.Matches(html, @"""cityname"":([\s\S]*?),");
                                MatchCollection areas = Regex.Matches(html, @"""adname"":([\s\S]*?),");
                                MatchCollection types = Regex.Matches(html, @"""type"":([\s\S]*?),");

                                if (names.Count == 0)
                                    break;

                                for (int i = 0; i < names.Count; i++)
                                {
                                  

                                    if (comboBox1.Text == "全部采集")
                                    {

                                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                                        lv1.SubItems.Add(names[i].Groups[1].Value);
                                        lv1.SubItems.Add(tels[i].Groups[1].Value.Replace("\"", ""));
                                        lv1.SubItems.Add(address[i].Groups[1].Value.Replace("\"", ""));
                                        lv1.SubItems.Add(pros[i].Groups[1].Value.Replace("\"", ""));
                                        lv1.SubItems.Add(citynames[i].Groups[1].Value.Replace("\"", ""));
                                        lv1.SubItems.Add(areas[i].Groups[1].Value.Replace("\"", ""));
                                        lv1.SubItems.Add(types[i].Groups[1].Value.Replace("\"", ""));
                                        lv1.SubItems.Add(keyword);
                                        while (this.zanting == false)
                                        {
                                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                        }
                                    }
                                    else if (comboBox1.Text == "只采集有联系方式")
                                    {
                                        if (tels[i].Groups[1].Value.Replace("\"", "") != "[]")
                                        {
                                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                                            lv1.SubItems.Add(names[i].Groups[1].Value);
                                            lv1.SubItems.Add(tels[i].Groups[1].Value.Replace("\"", ""));
                                            lv1.SubItems.Add(address[i].Groups[1].Value.Replace("\"", ""));
                                            lv1.SubItems.Add(pros[i].Groups[1].Value.Replace("\"", ""));
                                            lv1.SubItems.Add(citynames[i].Groups[1].Value.Replace("\"", ""));
                                            lv1.SubItems.Add(areas[i].Groups[1].Value.Replace("\"", ""));
                                            lv1.SubItems.Add(types[i].Groups[1].Value.Replace("\"", ""));
                                            lv1.SubItems.Add(keyword);
                                            while (this.zanting == false)
                                            {
                                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                            }
                                        }
                                    }
                                   
                                }
                                Thread.Sleep(1000);
                            }
                        }
                    }
                
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void close_btn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
               
            }
            else
            {
               
            }
        }

        private void zuixiaohua_btn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void addKey_Click(object sender, EventArgs e)
        {
            string key = key_tbox.Text.Replace(" ", "");
            if (key != "")
            {
                for (int i = 0; i < listView3.Items.Count; i++)
                {
                    if (listView3.Items[i].SubItems[0].Text.Contains(key))
                    {
                        MessageBox.Show(key+"：重复输入");
                        return;
                    }

                }
                listView3.Items.Add(key);
            }
            else
            {
                MessageBox.Show("输入为空");
            }
            key_tbox.Text = "";
        }
        public static string  username="";
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html");

            if (!html.Contains(@"Xzn0x"))
            {
                MessageBox.Show("");
                return;
            }

            #endregion

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

       

        private void 清空所有ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            listView3.Items.Clear();
        }

        private void 删除此项ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = listView3.SelectedItems.Count - 1; i >= 0; i--)
            {
                ListViewItem item = listView3.SelectedItems[i];
                listView3.Items.Remove(item);
            }
        }

      

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
        }

        private void 删除此项ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            for (int i = listView2.SelectedItems.Count - 1; i >= 0; i--)
            {
                ListViewItem item = listView2.SelectedItems[i];
                listView2.Items.Remove(item);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataTableToExcel(listViewToDataTable(listView1), "Sheet1", true);
        }

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
            //lv.Columns.Count
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
                    try
                    {
                        dr[j] = lv.Items[i].SubItems[j].Text.Trim();

                    }
                    catch
                    {

                        continue;
                    }

                }
                dt.Rows.Add(dr);
            }

            return dt;
        }
        #endregion

        #region NPOI导出表格
        public static int DataTableToExcel(DataTable data, string sheetName, bool isColumnWritten)
        {
            int i = 0;
            int j = 0;
            int count = 0;
            ISheet sheet = null;
            IWorkbook workbook = null;
            FileStream fs = null;
            // bool disposed;
            SaveFileDialog sfd = new SaveFileDialog();
            // sfd.Filter = "xlsx|*.xls|xlsx|*.xlsx";
            sfd.Filter = "xlsx|*.xlsx";
            sfd.Title = "Excel文件导出";
            string fileName = "";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                fileName = sfd.FileName;
            }
            else
                return -1;

            fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                workbook = new XSSFWorkbook();
            else if (fileName.IndexOf(".xls") > 0) // 2003版本
                workbook = new HSSFWorkbook();

            try
            {
                if (workbook != null)
                {
                    sheet = workbook.CreateSheet(sheetName);
                    ICellStyle style = workbook.CreateCellStyle();
                    style.FillPattern = FillPattern.SolidForeground;

                }
                else
                {
                    return -1;
                }

                if (isColumnWritten == true) //写入DataTable的列名
                {
                    IRow row = sheet.CreateRow(0);
                    for (j = 0; j < data.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data.Columns[j].ColumnName);

                    }
                    count = 1;
                }
                else
                {
                    count = 0;
                }

                for (i = 0; i < data.Rows.Count; ++i)
                {
                    IRow row = sheet.CreateRow(count);
                    for (j = 0; j < data.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
                    }
                    ++count;
                }
                workbook.Write(fs); //写入到excel
                workbook.Close();
                fs.Close();
                System.Diagnostics.Process[] Proc = System.Diagnostics.Process.GetProcessesByName("");
                MessageBox.Show("数据导出完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return -1;
            }
        }

        #endregion

        #region  listview导出文本TXT
        public static void ListviewToTxt(ListView listview, int i)
        {
            if (listview.Items.Count == 0)
            {
                MessageBox.Show("列表为空!");
            }
            else
            {
                List<string> list = new List<string>();
                foreach (ListViewItem item in listview.Items)
                {

                    list.Add(item.SubItems[i].Text + "," + item.SubItems[i + 1].Text + "," + item.SubItems[i + 2].Text + "," + item.SubItems[i + 3].Text + "," + item.SubItems[i + 4].Text);


                }
                Thread thexp = new Thread(() => export(list)) { IsBackground = true };
                thexp.Start();
            }
        }


        private static void export(List<string> list)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "导出_" + Guid.NewGuid().ToString() + ".txt";

            StringBuilder sb = new StringBuilder();
            foreach (string tel in list)
            {
                sb.AppendLine(tel);
            }
            System.IO.File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            MessageBox.Show("文件导出成功!文件地址:" + path);
        }



        #endregion

       

        private void 地图采集器_Load(object sender, EventArgs e)
        {
            ProvinceCity.ProvinceCity.BindProvince(comboBox2);
            label3.Text = username;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProvinceCity.ProvinceCity.BindCity(comboBox2, comboBox3);
           
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
            thread.Abort();
        }

        private void 清空数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ListviewToTxt(listView1,0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < comboBox4.Items.Count; i++)
            {
                listView2.Items.Add(comboBox4.Items[i].ToString());
            }

         
           
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            getarea(comboBox3.SelectedItem.ToString().Replace("市",""));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            listView2.Items.Add(comboBox4.Text);


        }
    }
}
