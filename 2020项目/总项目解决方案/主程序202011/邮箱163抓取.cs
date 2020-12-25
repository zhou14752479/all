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
using helper;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace 主程序202011
{
    public partial class 邮箱163抓取 : Form
    {
        public 邮箱163抓取()
        {
            InitializeComponent();
        }
        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData, string COOKIE, string charset)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";

                //request.ContentType = "application/json";
                request.ContentLength = postData.Length;
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = "https://mail.163.com/js6/main.jsp?sid=cBZNTotGUjWbvKFHDtGGQKnnrdhvIQXG&df=email163";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public  string GetUrl(string Url)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
              
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
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


        int total = 0;
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
            if (checkBox7.Checked == false)
            {
                dateTimePicker1.Value = DateTime.Now.AddYears(-99);
                dateTimePicker2.Value = DateTime.Now.AddYears(99);
            }


            for (int i = 0; i < Convert.ToInt32(textBox3.Text); i++)
            {
                int start = i * 30;
                string url = "https://mail.163.com/js6/s?sid="+sid+"&func=mbox:listMessages&mbox_pager_next=1";
                string postdata = "var=%3C%3Fxml%20version%3D%221.0%22%3F%3E%3Cobject%3E%3Cint%20name%3D%22fid%22%3E1%3C%2Fint%3E%3Cstring%20name%3D%22order%22%3Edate%3C%2Fstring%3E%3Cboolean%20name%3D%22desc%22%3Etrue%3C%2Fboolean%3E%3Cint%20name%3D%22limit%22%3E30%3C%2Fint%3E%3Cint%20name%3D%22start%22%3E" + start + "%3C%2Fint%3E%3Cboolean%20name%3D%22skipLockedFolders%22%3Efalse%3C%2Fboolean%3E%3Cstring%20name%3D%22topFlag%22%3Etop%3C%2Fstring%3E%3Cboolean%20name%3D%22returnTag%22%3Etrue%3C%2Fboolean%3E%3Cboolean%20name%3D%22returnTotal%22%3Etrue%3C%2Fboolean%3E%3C%2Fobject%3E";
                string html = PostUrl(url, postdata, COOKIE, "utf-8");
              
                MatchCollection aids = Regex.Matches(html, @"<string name=""id"">([\s\S]*?)</string>");
                MatchCollection titles = Regex.Matches(html, @"<string name=""subject"">([\s\S]*?)</string>");
                MatchCollection times = Regex.Matches(html, @"<date name=""receivedDate"">([\s\S]*?)</date>");
                MatchCollection yidus = Regex.Matches(html, @"<object name=""flags([\s\S]*?)</object>");
                if (aids.Count == 0)
                {
                    break;

                }
                


                for (int j = 0; j < aids.Count; j++)
                {
                    bool yiduweidu = true;

                    if (checkBox3.Checked == false && !yidus[j].Groups[1].Value.Contains("read"))
                    {
                        yiduweidu = false;
                    }
                    if (checkBox8.Checked == false && yidus[j].Groups[1].Value.Contains("read"))
                    {
                        yiduweidu = false;
                    }
                    try
                    {
                        if (yiduweidu)
                        {

                            string aurl = "https://mail.163.com/js6/read/readhtml.jsp?mid="+aids[j].Groups[1].Value+"&userType=ud";
                    string ahtml = GetUrl(aurl);

                    Match name = Regex.Match(ahtml, @"font-weight:normal;"">([\s\S]*?)<");
                    Match age = Regex.Match(ahtml, @">（([\s\S]*?)）");
                    Match tel = Regex.Match(ahtml, @"手机号码：([\s\S]*?)</span>");
                            if (name.Groups[1].Value != "")
                            {
                                
                                    string nianling = age.Groups[1].Value.Replace("女", "").Replace("男", "").Replace("，", "").Replace("岁", "");
                                    if (Convert.ToInt32(nianling) > Convert.ToInt32(textBox1.Text) && Convert.ToInt32(nianling) < Convert.ToInt32(textBox2.Text))
                                    {

                                        if (Convert.ToDateTime(times[j].Groups[1].Value) > dateTimePicker1.Value && Convert.ToDateTime(times[j].Groups[1].Value) < dateTimePicker2.Value)
                                        {
                                            if (comboBox1.Text == "全部性别")
                                            {
                                                total = total + 1;
                                                toolStripStatusLabel2.Text = total.ToString();
                                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                                lv1.SubItems.Add(name.Groups[1].Value);
                                                lv1.SubItems.Add(age.Groups[1].Value);
                                                lv1.SubItems.Add(Regex.Replace(tel.Groups[1].Value, "<[^>]+>", ""));
                                                lv1.SubItems.Add(titles[j].Groups[1].Value);
                                                lv1.SubItems.Add(times[j].Groups[1].Value);
                                            }
                                            else
                                            {
                                                if (age.Groups[1].Value.Contains(comboBox1.Text.Trim()))
                                                {
                                                    total = total + 1;
                                                    toolStripStatusLabel2.Text = total.ToString();
                                                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                                    lv1.SubItems.Add(name.Groups[1].Value);
                                                    lv1.SubItems.Add(age.Groups[1].Value);
                                                    lv1.SubItems.Add(Regex.Replace(tel.Groups[1].Value, "<[^>]+>", ""));
                                                    lv1.SubItems.Add(titles[j].Groups[1].Value);
                                                    lv1.SubItems.Add(times[j].Groups[1].Value);

                                                }
                                                else
                                                {
                                                    toolStripStatusLabel2.Text = "不符合性别要求";
                                                }
                                            }

                                        }
                                        else
                                        {
                                            toolStripStatusLabel2.Text = "不符合时间要求";
                                        }
                                    }
                                    else
                                    {
                                        toolStripStatusLabel2.Text = "不符合年龄要求";
                                    }
                                }
                                Thread.Sleep(1000);
                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                                if (status == false)
                                    return;
                            }

                        else
                        {
                            toolStripStatusLabel2.Text = "不符合已读未读筛选";
                        }
                    }
                    catch (Exception)
                    {

                        continue;
                    }


                }
                
            }
            MessageBox.Show("抓取结束");


        }

        bool zanting = true;
        Thread thread;
        string sid = "";
        string COOKIE = "";
        private void button1_Click(object sender, EventArgs e)
        {

            cookieBrowser cb = new cookieBrowser("https://mail.163.com/js6/s?sid=WCMjZlDUiyNqxrkjdXUUVOJCNlostwDI&func=mbox:listMessages&mbox_pager_next=1");
          
            cb.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            COOKIE = cookieBrowser.cookie;
            //COOKIE = "_ntes_nnid=cd122851463bd744e423f21fad397a63,1590834443609; _ga=GA1.2.1033791233.1590834537; _ntes_nuid=cd122851463bd744e423f21fad397a63; UM_distinctid=173f54f7393232-0c87497a2723a2-6373664-1fa400-173f54f7394bb5; vjlast=1598164493.1598164493.30; vjuids=-1bbd9584b.1741a06e3f5.0.222433b9c83f6; locale=; face=js6; mail_psc_fingerprint=f9980414501707bee1035100250b2caf; vinfo_n_f_l_n3=9248c9d9cccd372d.1.6.1590834443622.1603713715708.1605315103536; gdxidpyhxdE=Vif%2F9lItVYjDH85LZC%5CzqsNbtgRRcTqvsryKy7OHb7lTQYhQbfp4zzMOwlhIbyvvjuSXvwyR2nOz%2F4cC5u3PZ5MUpHqYkP3dKNc02%2Fw6j%2Fkm2%2BHOez%2FQZT9u%2BPjjqYO8xMhOShXaVcUXk2D791MN2Dj4NDZN61upjUv2g2moj1SEolCx%3A1605842537545; _9755xjdesxxd_=32; NTES_PASSPORT=LbYompA_wVrwEt9S7VF1iUnyWlyA1QQfEhJPa3VlNnhYNK67NXivTd_PXd.myx5v3q1sDCR2bOMDCxC4kZ2ANKvyNH8L5e4zTiLj9cHi5mGOEnrMtu85dBN2V52BpljeCHzriu2hidmPiU_5hzSV4Lj8K0d5FKtA1qV9aorNjm0UG6tURad9ICYwxX8Htpl8U; P_INFO=aa8898770@163.com|1605873606|1|mail163|00&99|gud&1605873301&mail163#jis&321300#10#0#0|&0|mail163|aa8898770@163.com; nts_mail_user=aa8898770@163.com:-1:1; MAIL_PINFO=aa8898770@163.com|1605873606|1|mail163|00&99|gud&1605873301&mail163#jis&321300#10#0#0|&0|mail163|aa8898770@163.com; MAIL_PASSPORT_INFO=aa8898770@163.com|1605873606|1; starttime=; _autoLogin=1606179062221%7C1; NTES_SESS=8P3cdzrzb9WmzuOj2y96oSPmUPk4ASFMjuTWxpWZfAA2fD7CfFQYXmWuFmaK6_iYVXVP9iv9FY_tUArbTtE0qlWHJGdHQnijq9mmov7qO5RDEiULVL1TeidB8SiEFaqaCIx72RQG7TI6f_Vjgq7Hh22J6SuaBrKts_HrPAjvIgQ3bhKzp55hpC6NoEZVVL1wZhieF5_ijhkwm; S_INFO=1606179062|1|0&60##|aa8898770; ANTICSRF=1152554f147ec80995945a0f499da5fa; mail_upx=t2hz.mail.163.com|t3hz.mail.163.com|t4hz.mail.163.com|t7hz.mail.163.com|t8hz.mail.163.com|t1hz.mail.163.com|t4bj.mail.163.com|t1bj.mail.163.com|t2bj.mail.163.com|t3bj.mail.163.com; mail_upx_nf=; mail_idc=\"\"; Coremail=35bc6e0ca9237%rCLKJJenoYrJaCirrYnnORkMoqGboCYa%g1a127.mail.163.com; cm_last_info=dT1hYTg4OTg3NzAlNDAxNjMuY29tJmQ9aHR0cHMlM0ElMkYlMkZtYWlsLjE2My5jb20lMkZqczYlMkZtYWluLmpzcCUzRnNpZCUzRHJDTEtKSmVub1lySmFDaXJyWW5uT1JrTW9xR2JvQ1lhJnM9ckNMS0pKZW5vWXJKYUNpcnJZbm5PUmtNb3FHYm9DWWEmaD1odHRwcyUzQSUyRiUyRm1haWwuMTYzLmNvbSUyRmpzNiUyRm1haW4uanNwJTNGc2lkJTNEckNMS0pKZW5vWXJKYUNpcnJZbm5PUmtNb3FHYm9DWWEmdz1odHRwcyUzQSUyRiUyRm1haWwuMTYzLmNvbSZsPS0xJnQ9LTEmYXM9dHJ1ZQ==; MAIL_SESS=8P3cdzrzb9WmzuOj2y96oSPmUPk4ASFMjuTWxpWZfAA2fD7CfFQYXmWuFmaK6_iYVXVP9iv9FY_tUArbTtE0qlWHJGdHQnijq9mmov7qO5RDEiULVL1TeidB8SiEFaqaCIx72RQG7TI6f_Vjgq7Hh22J6SuaBrKts_HrPAjvIgQ3bhKzp55hpC6NoEZVVL1wZhieF5_ijhkwm; MAIL_SINFO=1606179062|1|0&60##|aa8898770; secu_info=1; mail_entry_sess=3cc8d9deeb04e3745c495b876a85ca15c804a217353eff4576bef1882ec79b7c2304598b25a5ed147ea1bb259e5426506b71be47158518fc8bb34c0d8532af30f4c3e8c1f2eb539540497e3fd36ade9cf4b440eb3d98a997bb9e01009a23d7188e008e8957414b695d4517c0b5e8f517241a8d90a6b468df245ad50a4f6cd375075f0fec35d59beebfc2b8bd91fc794b13f8548b8a092ec58876715b2523273222b8ba18db63a2ba5a0e6123f0e46d099d1c4d46d412b0c40d2e890b4b76ca34; JSESSIONID=0A0845D12B2AD0A67D9D2CBD735A6265; Coremail.sid=rCLKJJenoYrJaCirrYnnORkMoqGboCYa; mail_style=js6; mail_uid=aa8898770@163.com; mail_host=mail.163.com";
            Match name = Regex.Match(COOKIE, @"Coremail\.sid=([\s\S]*?);");
            sid = name.Groups[1].Value;
         
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html");

            if (!html.Contains(@"163youxiang"))
            {
                MessageBox.Show("");
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
        bool status = true;
        private void button5_Click(object sender, EventArgs e)
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

        private void button6_Click(object sender, EventArgs e)
        {
            
            status = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ArrayList lists = new ArrayList();
            lists.Add(0);
            if (checkBox1.Checked == true)
            {
                lists.Add(1);
            }
            if (checkBox2.Checked == true)
            {
                lists.Add(2);
            }
            if (checkBox4.Checked == true)
            {
                lists.Add(3);
            }
            if (checkBox5.Checked == true)
            {
                lists.Add(4);
            }
            if (checkBox6.Checked == true)
            {
                lists.Add(5);
            }
            DataTableToExcel(listViewToDataTable(this.listView1,lists), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            total = 0;
            listView1.Items.Clear();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ArrayList lists = new ArrayList();
             if (checkBox1.Checked == true)
            {
                lists.Add(1); 
            }
            if (checkBox2.Checked == true)
            {
                lists.Add(2);
            }
            if (checkBox4.Checked == true)
            {
                lists.Add(3);
            }
            if (checkBox5.Checked == true)
            {
                lists.Add(4);
            }
            if (checkBox6.Checked == true)
            {
                lists.Add(5);
            }

            ListviewToTxt(listView1,lists);
        }

        private void 邮箱163抓取_Load(object sender, EventArgs e)
        {

        }

        #region  listview导出文本TXT
        public static void ListviewToTxt(ListView listview, ArrayList alist)
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
                    string daochu = "";
                    foreach (int ali in alist)
                    {
                        daochu+= item.SubItems[ali].Text+",";
                    }
                   
                    list.Add(daochu);

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


        #region listview转datable
        /// <summary>
        /// listview转datable
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        public static DataTable listViewToDataTable(ListView lv,ArrayList lists)
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
                    if (lists.Contains(j))
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
            sfd.Filter = "xlsx|*.xls|xlsx|*.xlsx";
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

        private void 邮箱163抓取_FormClosing(object sender, FormClosingEventArgs e)
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
