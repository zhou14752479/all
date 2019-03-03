using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taobao
{
    public partial class Form1 : Form
    {
        public string token { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")

            {
                MessageBox.Show("请输入员工账号！");
                return;
            }

            if (this.startdate==null)

            {
                MessageBox.Show("请重新选择日期！");
                return;
            }
            if (this.enddate == null)

            {
                MessageBox.Show("请重新选择日期！");
                return;
            }

            getCookieValue(webbrowser.cookie);
            Thread thread = new Thread(new ThreadStart(liaoT));
            thread.Start();

           



        }
        #region GET请求带COOKIE
        /// <summary>
        /// GET请求带COOKIE
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlWithCookie(string Url, string charset, string COOKIE)
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";

                request.Headers.Add("Cookie", COOKIE);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

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




        public static void insertData( string sql)
        {
            string Nowpath = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
            string path = Nowpath + "\\data\\" ;
            SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\Content.db");
            mycon.Open();

            SQLiteCommand cmd = new SQLiteCommand(sql, mycon);

            cmd.ExecuteNonQuery();  //执行sql语句
            mycon.Close();
        }


        public void getCookieValue(string cookie)
        {
            string[] cookieContainer = cookie.Split(';');

            foreach (string str in cookieContainer)
            {
              
                    if (str.Contains("token"))

                    {
                    this.token = str.Replace("_tb_token_=", "").Trim();
                    }
 
              
               
            }

        }


        public void liaoT()

        {
            try
            {
 

                string user = System.Web.HttpUtility.UrlEncode(comboBox1.Text.Trim(), System.Text.Encoding.GetEncoding("utf-8"));
                    string Url = "https://zizhanghao.taobao.com/subaccount/monitor/chatRecordJson.htm?action=/subaccount/monitor/ChatRecordQueryAction&eventSubmitDoQueryChatRealtion=anything&_tb_token_=" + this.token + "&_input_charset=utf-8&chatRelationQuery=%7B%22employeeNick%22%3A%22" + user + "%22%2C%22customerNick%22%3A%22%22%2C%22start%22%3A%22" + startdate + "%22%2C%22end%22%3A%22" + enddate+"%22%2C%22beginKey%22%3Anull%2C%22endKey%22%3Anull%2C%22employeeAll%22%3Afalse%2C%22customerAll%22%3Atrue%7D&site=0&_=1536033905990";
                
                    string cookie = webbrowser.cookie;

                    string html = GetUrlWithCookie(Url, "GBK", cookie);
                    MatchCollection match = Regex.Matches(html, @"cntaobao([\s\S]*?)""");



                for (int i = 1; i < match.Count; i++)
                {

                   

   
                                               string userid = System.Web.HttpUtility.UrlEncode(match[i].Groups[1].Value.Trim(), System.Text.Encoding.GetEncoding("utf-8"));


                        string url2 = "https://zizhanghao.taobao.com/subaccount/monitor/chatRecordHtml.htm?action=/subaccount/monitor/ChatRecordQueryAction&eventSubmitDoQueryChatContent=anything&_tb_token_=" + this.token + "&_input_charset=utf-8&chatContentQuery=%7B%22employeeUserNick%22%3A%5B%22cntaobao" + user + "%22%5D%2C%22customerUserNick%22%3A%5B%22cntaobao" + userid + "%22%5D%2C%22employeeAll%22%3Afalse%2C%22customerAll%22%3Afalse%2C%22start%22%3A%22" + startdate + "%22%2C%22end%22%3A%22" + enddate + "%22%2C%22beginKey%22%3Anull%2C%22endKey%22%3Anull%7D&site=0&_=1536033906045";

                        string body = GetUrlWithCookie(url2, "GBK", cookie);


                        string temp = Regex.Replace(body, "<[^>]*>", "");

                        string Temp = temp.Replace("15822280648", "").Replace("13516251600", "").Replace("18722432957", "").Replace("18222091486", "").Replace("17822193390", "").Replace("15022111121", "").Replace("15202226301", "").Replace("18222680889", "").Replace("13820232505", "").Replace("463035598", "").Replace("18222210493", "").Replace("17694803059", "");

                        string temp2 = Temp.Replace("您好，欢迎光临 聚成网络！本公司专业提供微信公众号、微信小程序的功能开发服务。工作时间：9:00-22:00服务电话：18222680889 欢迎致电！（微信:13820232505）", "");
                        string temp3 = temp2.Replace("可以的亲，您有什么具体的要求吗，或者参考案例吗", "");
                        string temp4 = temp3.Replace("我们有两种套餐，一种半包服务300元：提供后台功能搭建服务及客户操作视频及文档学习资料，由您自己操作上传；另一种全包服务600元：帮助注册认证、支付开通、功能搭建、页面设置、内容上传、代码审核、一对一操作指导等一次服务，由您提供所需的图片、视频及文字，服务人员上传页面及文章量之和不得超过30，超过部分须客户自行上传或另行付费（注意：腾讯收取300元认证费用，需要您另行缴纳给腾讯", "");
                        string temp5 = temp4.Replace("亲，我们官网小程序模板能够满足您的需求。官网小程序功能：页面可拖拽自定义，支持图片、文字、视频、商品展示、图文列表、链接、一键拨号、地图导航、表单、客服等功能，满足企业官网企业介绍、服务项目展示、产品宣传、在线预约、线上联系等要求。", "");
                        string temp6 = temp5.Replace("亲您有营业执照吗，做这个需要营业执照的", "");
                        string temp7 = temp6.Replace("亲，好久没有收到您的消息，请问还有什么可以帮助您的吗  方便的时候可以加我微信 s1798120121（电话18222680889）或者您的微信发我加您，具体看您的需求。", "");


                        Match tells = Regex.Match(temp7, @"1\d{10}", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    if (body.Contains(comboBox2.Text))
                    {
                        ListViewItem lv1 = listView1.Items.Add(match[i].Groups[1].Value.Trim()); //使用Listview展示数据
                        lv1.SubItems.Add(match[0].Groups[1].Value.Trim());
                        lv1.SubItems.Add(temp7.Trim().Substring(0,10));
                        lv1.SubItems.Add(tells.Value);
                        lv1.SubItems.Add(temp7.Trim());
                        label7.Text = listView1.Items.Count.ToString();

                        string[] values = { match[i].Groups[1].Value.Trim(),
                               match[0].Groups[1].Value.Trim(),
                                temp7.Trim().Substring(0,10),
                                tells.Value,
                                temp7.Trim()
                            };

                        string sql = "INSERT INTO result VALUES( '" + values[0] + "','" + values[1] + "','" + values[2] + "','" + values[3] + "','" + values[4] + "')";

                        insertData(sql);

                        Thread.Sleep(1000);

                    }


                    

                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

           
            




        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataTableToExcel(listViewToDataTable(this.listView1), "Sheet1", true);
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
                    dr[j] = lv.Items[i].SubItems[j].Text.Trim();
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
            sfd.Filter = "xls|*.xls|xlsx|*.xlsx";
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

        private void button3_Click(object sender, EventArgs e)
        {
            webbrowser web = new webbrowser("https://zizhanghao.taobao.com/subaccount/monitor/chatRecordQuery.htm?spm=a211ki.11005395.0.0.5c3361c4QipeGm&query=empty");
            web.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

            if (File.Exists(path + "//users.txt"))
            {

                StreamReader sr = new StreamReader(path + "//users.txt");
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] != "")
                    {
                        comboBox1.Items.Add(text[i]);
                    }

                }
                sr.Close();
            }
            if (File.Exists(path + "//keywords.txt"))
            {

                StreamReader sr = new StreamReader(path + "//keywords.txt");
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {

                    if (text[i] != "")
                    {
                        comboBox2.Items.Add(text[i]);

                    }
                }
                sr.Close();
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
           this.dateTimePicker1.CustomFormat = "yyyy-MM-dd";

            this.startdate = dateTimePicker1.Text;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageBox.Show(comboBox1.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.startdate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            this.enddate = DateTime.Now.ToString("yyyy-MM-dd");

            if (comboBox1.Text == "")

            {
                MessageBox.Show("请输入员工账号！");
                return;
            }

            if (this.startdate == null)

            {
                MessageBox.Show("请重新选择日期！");
                return;
            }

            getCookieValue(webbrowser.cookie);
            Thread thread = new Thread(new ThreadStart(liaoT));
            thread.Start();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            comboBox1.Items.Add(textBox1.Text.Trim());
           

            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

            StreamWriter sw = File.AppendText(path + "//users.txt");
            sw.WriteLine(textBox1.Text);

            sw.Flush();
            sw.Close();
            textBox1.Text = "";
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            comboBox2.Items.Add(textBox2.Text.Trim());
           
            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

            StreamWriter sw = File.AppendText(path + "//keywords.txt");
            sw.WriteLine(textBox2.Text);

            sw.Flush();
            sw.Close();
            textBox2.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            this.dateTimePicker2.CustomFormat = "yyyy-MM-dd";

            this.enddate = dateTimePicker2.Text;
        }
    }
}
