using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Net;
using System.Text;
using System.Runtime.InteropServices;


namespace MeiT
{
    public partial class Form1 : Form
    {

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref System.UInt32 pcchCookieData, int dwFlags, IntPtr lpReserved);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int InternetSetCookieEx(string lpszURL, string lpszCookieName, string lpszCookieData, int dwFlags, IntPtr dwReserved);




        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
           // this.pictureBox1.Image = Image.FromStream(System.Net.WebRequest.Create("http://157.122.153.67:9000/khyx/um/umtuser/ValidCode.do").GetResponse().GetResponseStream());

            
            for (int i = 0; i < 100; i++)
            {
                dataGridView1.Rows.Add();
            }

            webBrowser1.ScriptErrorsSuppressed = true;


            // webBrowser1.Url = new Uri("http://157.122.153.67:9000/khyx/um/umtuser/ValidCode.do");
            webBrowser1.Navigate("http://157.122.153.67:9000/khyx/");
        }


        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string COOKIE)
        {
            try
            {
              
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";

                request.Headers.Add("Cookie", COOKIE);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                
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

        

        #region 主方法
        public void run()
        {
            if (textBox1.Text=="")
            {
                MessageBox.Show("请填写查询信息");
                return;
            }

            string cookie = textBox2.Text.Trim();

             string license =textBox1.Text.Trim();

            DateTime time = DateTime.Now;
            // MessageBox.Show(time.AddDays(-30).ToString("yyyy-MM-dd"));
            // MessageBox.Show(time.ToString("yyyy-MM-dd"));

            //time.AddDays(1); //增加一天 dt本身并不改变
            //time.AddDays(-30); //今天前30天 dt本身并不改变


            String Url = "http://157.122.153.67:9000/khyx/qth/proposalsearch/query.do?VersionFourControl=1&uploadImageControl=1&prpallProposalRequestBody.lisenceColorCode=01&payVerifyControl=0&payLinkControl=0&usePlatformPayType=1&totalFee=&comId=34000000&prpallProposalRequestBody.licenseNo="+license+"&prpallProposalRequestBody.policyNo=&prpallProposalRequestBody.insuredName=&prpallProposalRequestBody.frameNo=&prpallProposalRequestBody.operateDateStart="+time.AddDays(-30).ToString("yyyy-MM-dd")+"&prpallProposalRequestBody.operateDate="+time.ToString("yyyy-MM-dd")+"&prpallProposalRequestBody.riskCode=1";

            string strhtml = GetUrl(Url, cookie);  //定义的GetRul方法 返回 reader.ReadToEnd()

            string number = @"total"":([\s\S]*?),";
            string Rxg = @"""policyNo"":""([\s\S]*?)""";
            string Rxg1 = @"""comCode"":""([\s\S]*?)""";
            string Rxg2 = @"""startDate"":""([\s\S]*?)""";
            string Rxg3 = @"""licenseNo"":""([\s\S]*?)""";
            string Rxg4 = @"""frameNo"":""([\s\S]*?)""";
            string Rxg5 = @"""insuredName"":""([\s\S]*?)""";
            string Rxg6 = @"""brandName"":""([\s\S]*?)""";
            string Rxg7 = @"sumPremium"":([\s\S]*?),";
            string Rxg8 = @"""sumtax"":""([\s\S]*?)""";
            string Rxg9 = @"""poliNo"":""([\s\S]*?)""";
            string Rxg10 = @"""operateDate"":""([\s\S]*?)""";


            Match num = Regex.Match(strhtml,number);
            MatchCollection aas = Regex.Matches(strhtml, Rxg);
            MatchCollection bbs = Regex.Matches(strhtml, Rxg1);
            MatchCollection ccs = Regex.Matches(strhtml, Rxg2);
            MatchCollection dds = Regex.Matches(strhtml, Rxg3);
            MatchCollection ees = Regex.Matches(strhtml, Rxg4);
            MatchCollection ffs = Regex.Matches(strhtml, Rxg5);
            MatchCollection ggs = Regex.Matches(strhtml, Rxg6);
            MatchCollection hhs = Regex.Matches(strhtml, Rxg7);
            MatchCollection iis = Regex.Matches(strhtml, Rxg8);
            MatchCollection jjs = Regex.Matches(strhtml, Rxg9);
            MatchCollection kks = Regex.Matches(strhtml, Rxg10);

            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
            if (num.Groups[1].Value != "")
            {
                int nums = Convert.ToInt32(num.Groups[1].Value);


                for (int j = 0; j < nums; j++)
                {

                    this.dataGridView1.Rows[j].Cells[0].Value = j;

                    dataGridView1.Rows[j].Cells[1].Value = aas[j].Groups[1].Value;
                    dataGridView1.Rows[j].Cells[2].Value = bbs[j].Groups[1].Value;
                    dataGridView1.Rows[j].Cells[3].Value = ccs[j].Groups[1].Value;
                    dataGridView1.Rows[j].Cells[4].Value = dds[j].Groups[1].Value;
                    dataGridView1.Rows[j].Cells[5].Value = ees[j].Groups[1].Value;
                    dataGridView1.Rows[j].Cells[6].Value = ffs[j].Groups[1].Value;
                    dataGridView1.Rows[j].Cells[7].Value = ggs[j].Groups[1].Value;
                    dataGridView1.Rows[j].Cells[8].Value = hhs[j].Groups[1].Value;
                    dataGridView1.Rows[j].Cells[9].Value = iis[j].Groups[1].Value;
                    dataGridView1.Rows[j].Cells[10].Value = jjs[j].Groups[1].Value;
                    dataGridView1.Rows[j].Cells[11].Value = kks[j].Groups[1].Value;

                 

                }

            }
            else
            {
                MessageBox.Show("没有查询到相关数据！");
                return;
            }


        }
        #endregion


        #region  获取cookie
        /// <summary>
        /// 获取cookie
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string GetCookies(string url)
        {
            uint datasize = 256;
            StringBuilder cookieData = new StringBuilder((int)datasize);
            if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x2000, IntPtr.Zero))
            {
                if (datasize < 0)
                    return null;


                cookieData = new StringBuilder((int)datasize);
                if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x00002000, IntPtr.Zero))
                    return null;
            }
            return cookieData.ToString();
        }

        #endregion

        #region dataGridView导出CSV，导出分列
        /// <summary>
        /// dataGridView导出CSV，导出的结果分列
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <returns></returns>
        public static bool dataGridViewToCSV(DataGridView dataGridView)
        {
            if (dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("没有数据可导出!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.CreatePrompt = true;
            saveFileDialog.FileName = null;
            saveFileDialog.Title = "保存";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Stream stream = saveFileDialog.OpenFile();
                StreamWriter sw = new StreamWriter(stream, System.Text.Encoding.GetEncoding(-0));
                string strLine = "";
                try
                {
                    //表头
                    for (int i = 0; i < dataGridView.ColumnCount; i++)
                    {
                        if (i > 0)
                            strLine += ",";
                        strLine += dataGridView.Columns[i].HeaderText;
                    }
                    strLine.Remove(strLine.Length - 1);
                    sw.WriteLine(strLine);
                    strLine = "";
                    //表的内容
                    for (int j = 0; j < dataGridView.Rows.Count; j++)
                    {
                        strLine = "";
                        int colCount = dataGridView.Columns.Count;
                        for (int k = 0; k < colCount; k++)
                        {
                            if (k > 0 && k < colCount)
                                strLine += ",";
                            if (dataGridView.Rows[j].Cells[k].Value == null)
                                strLine += "";
                            else
                            {
                                string cell = dataGridView.Rows[j].Cells[k].Value.ToString().Trim();
                                //防止里面含有特殊符号
                                cell = cell.Replace("\"", "\"\"");
                                cell = "\"" + cell + "\"";
                                strLine += cell;
                            }
                        }
                        sw.WriteLine(strLine);
                    }
                    sw.Close();
                    stream.Close();
                    MessageBox.Show("数据被导出到：" + saveFileDialog.FileName.ToString(), "导出完毕", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "导出错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            return true;
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            webBrowser1.Visible = false;

            Thread search_thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            search_thread.Start();


        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            textBox2.Text = GetCookies("http://www.qunzushare.com/detail-558507.html");

            HtmlDocument dc = webBrowser1.Document;

            HtmlElementCollection es = dc.GetElementsByTagName("input");   //GetElementsByTagName返回集合

            

            foreach (HtmlElement e1 in es)
            {
                if (e1.GetAttribute("name").ToLower() == "j_username")
                {
                    e1.SetAttribute("value", textBox3.Text.Trim());
                }
                if (e1.GetAttribute("name").ToLower() == "j_password")
                {
                    e1.SetAttribute("value", textBox4.Text.Trim());
                }
            }

            textBox2.Text = GetCookies("http://157.122.153.67:9000/khyx/");


            ////找到提交按钮
            //foreach (HtmlElement e1 in es)
            //{
            //    if (e1.GetAttribute("type").ToLower() == "submit")
            //    {
            //        //执行提交事件
            //        e1.InvokeMember("Click");
            //        break;
            //    }
            //}


        }


    }
}
