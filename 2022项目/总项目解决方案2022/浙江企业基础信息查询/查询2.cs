using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
using Spire.Xls;

namespace 浙江企业基础信息查询
{
    public partial class 查询2 : Form
    {
        public 查询2()
        {
            InitializeComponent();
        }

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            string html = "";
            string COOKIE = "HWWAFSESID=cc9147f4aa41fc86ee; HWWAFSESTIME=1618565738420; route=0f1040e0778720d344b64fd91ee406cf; _monitor_sessionid=tCy7Ys6iRe1626459960928; _monitor_idx=5; JMOPENSESSIONID=1b9e1ff0-e9f4-4dc0-9a49-3720b58f83d9";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Linux; Android 10; M2007J3SC Build/QKQ1.200419.002; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/83.0.4103.101 Mobile Safari/537.36 zgzwfwpt_iOS_hanweb_1.4.4_gjzwfw_hanweb_1.4.2";
                request.Referer = Url;
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }

                response.Close();
                return html;



            }
            catch (System.Exception ex)
            {
                return ex.ToString();

            }



        }
        #endregion
        public string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalMilliseconds);
            return a.ToString();
        }
        public void run()
        {
            try
            {
                for (int a = 0; a < dt.Rows.Count; a++)
                {
                    if (DateTime.Now > Convert.ToDateTime("2022-04-07"))
                    {
                        MessageBox.Show("{\"msg\":\"非法请求\"}");
                        return;
                    }

                    DataRow dr = dt.Rows[a];
                    string uid = dr[0].ToString();
                    string timestr = GetTimeStamp();
                    string sign = method.GetMD5("rkkpcxxcxjtapp" + timestr);
                    string zj_ggsjpt_sign = method.GetMD5("ada72850-2b2e-11e7-985b-008cfaeb3d74" + "995e00df72f14bbcb7833a9ca063adef" + timestr);
                    string url = "http://app.gjzwfw.gov.cn/jimps/link.do?param=%7B%22from%22%3A%222%22%2C%22key%22%3A%223b8d18a7d9b4482caf1cbc39b4404d06%22%2C%22requestTime%22%3A%22"+timestr+"%22%2C%22sign%22%3A%22"+sign+"%22%2C%22zj_ggsjpt_app_key%22%3A%22ada72850-2b2e-11e7-985b-008cfaeb3d74%22%2C%22zj_ggsjpt_sign%22%3A%22"+ zj_ggsjpt_sign + "%22%2C%22zj_ggsjpt_time%22%3A%22"+timestr+"%22%2C%22gmsfhm%22%3A%22"+uid+"%22%2C%22additional%22%3A%22%22%7D";
                    label3.Text = "正在查询：" + uid;
                  
                    
                    string html = GetUrl(url,"utf-8");

                    textBox2.Text = html;
                    MatchCollection dwxxmc = Regex.Matches(html, @"""dwxxmc"":""([\s\S]*?)""");
                    MatchCollection xywcqk = Regex.Matches(html, @"""xywcqk"":""([\s\S]*?)""");
                    MatchCollection gmsfhm = Regex.Matches(html, @"""gmsfhm"":""([\s\S]*?)""");
                    MatchCollection sfsz = Regex.Matches(html, @"""sfsz"":""([\s\S]*?)""");
                    MatchCollection hkdjd = Regex.Matches(html, @"""hkdjd"":""([\s\S]*?)""");
                    MatchCollection mz = Regex.Matches(html, @"""mz"":""([\s\S]*?)""");
                    MatchCollection xm = Regex.Matches(html, @"""xm"":""([\s\S]*?)""");
                    MatchCollection hyzk = Regex.Matches(html, @"""hyzk"":""([\s\S]*?)""");
                    MatchCollection jnsyzk = Regex.Matches(html, @"""jnsyzk"":""([\s\S]*?)""");

                    if (jiami == true)
                    {
                        uid = method.Base64Encode(Encoding.GetEncoding("utf-8"), uid);
                       
                    }
                    StringBuilder sb = new StringBuilder();
                    sb.Append(uid + ",");
                    for (int i = 0; i < dwxxmc.Count; i++)
                    {
                        string dwxxmc1 = dwxxmc[i].Groups[1].Value;
                        string xywcqk1 = xywcqk[i].Groups[1].Value;
                        string gmsfhm1 = gmsfhm[i].Groups[1].Value;
                        string sfsz1 = sfsz[i].Groups[1].Value;
                        string hkdjd1 = hkdjd[i].Groups[1].Value;
                        string mz1=mz[i].Groups[1].Value;
                        string xm1 = xm[i].Groups[1].Value;
                        string hyzk1 = hyzk[i].Groups[1].Value;
                        string jnsyzk1 = jnsyzk[i].Groups[1].Value;

                        if (jiami == true)
                        {
                            dwxxmc1 = method.Base64Encode(Encoding.GetEncoding("utf-8"), dwxxmc[i].Groups[1].Value);
                            xywcqk1 = method.Base64Encode(Encoding.GetEncoding("utf-8"), xywcqk[i].Groups[1].Value);
                            gmsfhm1 = method.Base64Encode(Encoding.GetEncoding("utf-8"), gmsfhm[i].Groups[1].Value);
                            sfsz1 = method.Base64Encode(Encoding.GetEncoding("utf-8"), sfsz[i].Groups[1].Value);
                            hkdjd1 = method.Base64Encode(Encoding.GetEncoding("utf-8"), hkdjd[i].Groups[1].Value);
                            mz1 = method.Base64Encode(Encoding.GetEncoding("utf-8"), mz[i].Groups[1].Value);
                            xm1 = method.Base64Encode(Encoding.GetEncoding("utf-8"), xm[i].Groups[1].Value);
                            hyzk1 = method.Base64Encode(Encoding.GetEncoding("utf-8"), hyzk[i].Groups[1].Value);
                            jnsyzk1 = method.Base64Encode(Encoding.GetEncoding("utf-8"), jnsyzk[i].Groups[1].Value);

                        }
                        sb.Append(dwxxmc1 + ","+ xywcqk1 + "," + gmsfhm1 + "," + sfsz1 + "," + hkdjd1 + "," + mz1 + "," + xm1 + "," + hyzk1 + "," + jnsyzk1);
                      

                    }
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(sb.ToString());
                    //Thread.Sleep(1000);

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
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        bool jiami = true;
        bool zanting = true;
        bool status = false;
        Thread thread;
        DataTable dt;
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请先导入表格");
                return;
            }

            if (status == true)
            {
                status = false;
                label3.Text = "已停止";
            }
            else
            {
                status = true;
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(run);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
        }

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

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "14752479")
            {
                MessageBox.Show("密码错误");
                return;
            }


            zanting = false;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                for (int j = 1; j < listView1.Columns.Count; j++)
                {
                    try
                    {

                        if (jiami == false)
                        {
                            if (j == 1)
                            {

                                string[] text = listView1.Items[i].SubItems[j].Text.Split(new string[] { "," }, StringSplitOptions.None);
                                listView1.Items[i].SubItems[j].Text = "";
                                foreach (var item in text)
                                {
                                    listView1.Items[i].SubItems[j].Text += method.Base64Encode(Encoding.GetEncoding("utf-8"), item) + ",";
                                }
                            }

                        }
                        else
                        {
                            if (j == 1)
                            {
                                string[] text = listView1.Items[i].SubItems[j].Text.Split(new string[] { "," }, StringSplitOptions.None);
                                listView1.Items[i].SubItems[j].Text = "";
                                foreach (var item in text)
                                {

                                    listView1.Items[i].SubItems[j].Text += method.Base64Decode(Encoding.GetEncoding("utf-8"), item) + ",";
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {

                        continue;
                    }
                }
            }


            zanting = true;

            if (jiami == false)
            {
                jiami = true;
            }
            else
            {
                jiami = false;
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

        private void 查询2_Load(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcelName(listViewToDataTable(this.listView1), "sample.xlsx", true);
            excel_fenlie();
            //method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
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
            for (i = 1; i < lv.Columns.Count; i++)
            {
                dt.Columns.Add(lv.Columns[i].Text.Trim(), typeof(String));
            }
            //每行内容
            for (i = 0; i < lv.Items.Count; i++)
            {
                dr = dt.NewRow();
                dr[0] = lv.Items[i].SubItems[1].Text.Trim();
                dt.Rows.Add(dr);
            }

            return dt;
        }
        #endregion
        public void excel_fenlie()
        {
            //创建Workbook，加载Excel测试文档
            Workbook book = new Workbook();
            book.LoadFromFile("sample.xlsx");
            //获取第一个工作表
            Worksheet sheet = book.Worksheets[0];

            //遍历数据（从第2行到最后一行）
            string[] splitText = null;
            string text = null;
            for (int i = 1; i < sheet.LastRow; i++)
            {
                text = sheet.Range[i + 1, 1].Text;
                //分割按逗号作为分隔符的数据列
                splitText = text.Split(',');
                //保存被分割的数据到数组，数组项写入列
                for (int j = 0; j < splitText.Length; j++)
                {
                    sheet.Range[i + 1, 1 + j + 1].Text = splitText[j];
                }
            }
            //保存并打开文档
            string time = GetTimeStamp();
            book.SaveToFile("结果" + time + ".xlsx", ExcelVersion.Version2010);
            File.Delete("sample.xlsx");
            MessageBox.Show("导出成功文件位于软件根目录：" + time + ".xlsx");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
