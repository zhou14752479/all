using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace 主程序202011
{
    public partial class 表格日期监控1 : Form
    {
        public 表格日期监控1()
        {
            InitializeComponent();
        }
        #region  listView导出CSV
        /// <summary>
        /// 导出CSV
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="includeHidden"></param>
        public static void ListViewToCSV(ListView listView, bool includeHidden)
        {
            //make header string
            SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "xlsx|*.xls|xlsx|*.xlsx";

            //sfd.Title = "Excel文件导出";
            string filePath = "";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                filePath = sfd.FileName + ".csv";
            }
            StringBuilder result = new StringBuilder();
            WriteCSVRow(result, listView.Columns.Count, i => includeHidden || listView.Columns[i].Width > 0, i => listView.Columns[i].Text);

            //export data rows
            foreach (ListViewItem listItem in listView.Items)
                WriteCSVRow(result, listView.Columns.Count, i => includeHidden || listView.Columns[i].Width > 0, i => listItem.SubItems[i].Text);

            File.WriteAllText(filePath, result.ToString());
            MessageBox.Show("导出成功");
        }

        private static void WriteCSVRow(StringBuilder result, int itemsCount, Func<int, bool> isColumnNeeded, Func<int, string> columnValue)
        {
            bool isFirstTime = true;
            for (int i = 0; i < itemsCount; i++)
            {
                try
                {

                    if (!isColumnNeeded(i))
                        continue;

                    if (!isFirstTime)
                        result.Append(",");
                    isFirstTime = false;

                    result.Append(String.Format("\"{0}\"", columnValue(i)));
                }
                catch
                {
                    continue;
                }
            }

            result.AppendLine();
        }

        #endregion
        string path = AppDomain.CurrentDomain.BaseDirectory;

        #region NPOI读取表格导入
        public  void ReadFromExcelFile(string filePath, ListView listView,int biao)
        {
            //using (OpenFileDialog openFileDialog1 = new OpenFileDialog() { Filter = "Microsoft Excel files(*.xls)|*.xls;*.xlsx" })
            //    if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //    {
            //        ReadFromExcelFile(openFileDialog1.FileName);
            //    }
            IWorkbook wk = null;
            string extension = System.IO.Path.GetExtension(filePath);
            try
            {
                FileStream fs = File.OpenRead(filePath);
                if (extension.Equals(".xls"))
                {
                    //把xls文件中的数据写入wk中
                    wk = new HSSFWorkbook(fs);
                }
                else
                {
                    //把xlsx文件中的数据写入wk中
                    wk = new XSSFWorkbook(fs);
                }

                fs.Close();
                //读取当前表数据
                ISheet sheet = wk.GetSheetAt(biao);

                IRow row = sheet.GetRow(0);  //读取当前行数据
                                             //LastRowNum 是当前表的总行数-1（注意）
                                             // int offset = 0;
                string wupin = "";
                int youxiao = 0;
                int guoqi = 0;
                int linqi = 0;

                int youxiao2 = 0;
                int guoqi2 = 0;
                int linqi2 = 0;
                for (int i = 2; i <= sheet.LastRowNum; i++)
                {
                    row = sheet.GetRow(i);  //读取当前行数据
                    if (row != null)
                    {

                       

                        try
                        {
                            //读取该行的第j列数据
                            string aid = row.GetCell(0).ToString().Trim();

                            string value = row.GetCell(2).ToString().Trim();
                            string value1 = row.GetCell(6).ToString().Trim();
                            string value2 = row.GetCell(11).ToString().Trim();

                            //textBox1.Text+=(value.ToString() + " ");
                            if (value1 != "")
                            {
                                wupin = value;
                                value1 = "20"+value1.Substring(value1.Length - 2, 2) +"/"+ value1.Substring(0, value1.Length - 3);
                              
                                ListViewItem lv1 = listView.Items.Add((listView.Items.Count + 1).ToString());
                                lv1.SubItems.Add(value.ToString());
                                lv1.SubItems.Add(value1);
                                lv1.SubItems.Add(value2);
                                if (Convert.ToDateTime(value1.ToString()) > DateTime.Now.Date.AddDays(19))
                                {
                                    //  lv1.SubItems.Add("有效");
                                    if (value2 == "在用")
                                    {
                                        youxiao = youxiao + 1;
                                    }
                                    if (value2 == "备用")
                                    {
                                        youxiao2 = youxiao2 + 1;
                                    }
                                }
                               else if (Convert.ToDateTime(value1.ToString())< DateTime.Now.Date)
                                {
                                    lv1.SubItems.Add("过期");
                                    lv1.BackColor = Color.Red;
                                   
                                    if (value2 == "在用")
                                    {
                                        guoqi = guoqi + 1;
                                        MessageBox.Show(wupin+"："+aid);
                                        
                                    }
                                   
                                    if (value2 == "备用")
                                    {
                                        guoqi2 = guoqi2 + 1;
                                    }
                                }
                                else 
                                {
                                    lv1.SubItems.Add("临期");
                                    lv1.BackColor = Color.Yellow;
                                   
                                    if (value2 == "在用")
                                    {
                                        linqi = linqi + 1;
                                    }

                                    if (value2 == "备用")
                                    {
                                        linqi2 = linqi2 + 1;
                                    }
                                }

                            }
                           
                        }
                        catch (Exception)
                        {

                            continue;
                        }


                        // textBox1.Text += "\r\n";
                    }
                }
                ListViewItem lv = listView6.Items.Add(wupin);
                lv.SubItems.Add(youxiao.ToString());
                lv.SubItems.Add(guoqi.ToString());
                lv.SubItems.Add(linqi.ToString());

                ListViewItem lv2 = listView7.Items.Add(wupin);
                lv2.SubItems.Add(youxiao2.ToString());
                lv2.SubItems.Add(guoqi2.ToString());
                lv2.SubItems.Add(linqi2.ToString());


            }

            catch (Exception e)
            {
                //只在Debug模式下才输出
                MessageBox.Show(e.ToString());
            }
        }

        #endregion

        public void run()
        {
            try
            {
                

                    string[] files = Directory.GetFiles(path + @"\", "*.xls");
                    foreach (string file in files)
                    {

                        ReadFromExcelFile(file, listView1,0);
                        ReadFromExcelFile(file, listView2, 1);
                        ReadFromExcelFile(file, listView3, 2);
                        ReadFromExcelFile(file, listView4, 3);
                        ReadFromExcelFile(file, listView5, 4);


                    }
               
                
            }
            catch (Exception)
            {

                throw;
            }
        }


        Thread thread;

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                //headers.Add("sec-fetch-site:same-origin");
                //headers.Add("sec-fetch-user:?1");
                //headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                // request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
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

        private void button2_Click(object sender, EventArgs e)
        {
            listView6.Items.Clear();
            listView7.Items.Clear();

            listView1.Items.Clear();
            listView2.Items.Clear();
            listView3.Items.Clear();
            listView4.Items.Clear();
            listView5.Items.Clear();

            if (DateTime.Now.Date > Convert.ToDateTime("2020-12-15"))
            {
                MessageBox.Show("系统错误");
                return;
            }
          
            if (thread == null || !thread.IsAlive)
            {

                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
           
        }

        private void 表格日期监控1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ListViewToCSV(listView6,true);
            ListViewToCSV(listView7, true);
        }
    }
}
