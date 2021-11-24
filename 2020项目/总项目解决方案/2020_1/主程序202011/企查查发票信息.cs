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
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;


namespace 主程序202011
{
    public partial class 企查查发票信息 : Form
    {
        public 企查查发票信息()
        {
            InitializeComponent();
        }

        #region NPOI读取表格导入
        public static void ReadFromExcelFile(string filePath, ListView listView)
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
                ISheet sheet = wk.GetSheetAt(0);

                IRow row = sheet.GetRow(0);  //读取当前行数据
                                             //LastRowNum 是当前表的总行数-1（注意）
                                             // int offset = 0;
                for (int i = 2; i <= sheet.LastRowNum; i++)
                {
                    row = sheet.GetRow(i);  //读取当前行数据
                    if (row != null)
                    {

                        ListViewItem lv1 = listView.Items.Add((listView.Items.Count + 1).ToString());
                       
                            try
                            {
                                //读取该行的第j列数据
                                string value = row.GetCell(0).ToString();
                            string value1 = row.GetCell(13).ToString();
                            //textBox1.Text+=(value.ToString() + " ");

                            lv1.SubItems.Add(value.ToString());
                            lv1.SubItems.Add(value1.ToString());
                            lv1.SubItems.Add("无");
                        }
                            catch (Exception)
                            {

                                continue;
                            }

                        
                        // textBox1.Text += "\r\n";
                    }
                }
            }

            catch (Exception e)
            {
                //只在Debug模式下才输出
                MessageBox.Show(e.ToString());
            }
        }

        #endregion

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
                request.Referer = "https://www.qcc.com/search?key=%E6%B1%9F%E8%A5%BF%E6%B6%88%E9%98%B2%E8%AE%BE%E5%A4%87%E6%9C%89%E9%99%90%E5%85%AC%E5%8F%B8";
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


        public void gettoken()
        {

            Match code = Regex.Match(GetUrl("http://www.acaiji.com/index/index/vip"), @"code=([\s\S]*?)""");
           
            string url = "https://xcx.qcc.com/mp-alipay/admin/getLastedLoginInfo?code="+code.Groups[1].Value;
            string html = GetUrl(url);
            Match t = Regex.Match(html, @"sessionToken"":""([\s\S]*?)""");
            token = t.Groups[1].Value;
           
        }

        string token = "60a3a599fc03ff983dff5fefa45cc567";
        public void run()
        {
            gettoken();
            zanting = true;
            try
            {

                
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    try
                    {
                        if (!listView1.Items[i].SubItems[2].Text.Contains("-"))
                        {
                            string url = "https://xcx.qcc.com/mp-alipay/forwardApp/v3/base/advancedSearch?token=" + token + "&t=1605660955000&searchKey=" + listView1.Items[i].SubItems[2].Text;
                            textBox1.Text = url;
                            string html = GetUrl(url);
                            Match aid = Regex.Match(html, @"""KeyNo"":""([\s\S]*?)""");
                            if (aid.Groups[1].Value == "")
                            {
                                MessageBox.Show("token过期");
                                zanting = false;
                            }
                            if (aid.Groups[1].Value != "")
                            {
                                string aurl = "https://xcx.qcc.com/mp-alipay/forwardApp/v1/order/getInvoiceInfoByKeyno?token=" + token + "&t=1605664639000&unique=" + aid.Groups[1].Value;
                                string ahtml = GetUrl(aurl);

                                Match bank = Regex.Match(ahtml, @"""bank"":""([\s\S]*?)""");

                                listView1.Items[i].SubItems[3].Text = Regex.Replace(bank.Groups[1].Value, "<[^>]+>", "").Trim();
                                listView1.Items[i].ForeColor = Color.Red;
                                label2.Text = "已查询数量：" + (i + 1);
                            }
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            Thread.Sleep(1000);
                        }
                    }
                    catch (Exception)
                    {

                        continue;
                    }
                 
                }
              



            }
            catch (Exception)
            {

                throw;
            }
        }
        private void 企查查发票信息_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
            ReadFromExcelFile(textBox1.Text, listView1);
        }

        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"qichachafapiao"))
            {
                MessageBox.Show("验证失败");
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
        bool zanting = true;
        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
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

        private void button6_Click(object sender, EventArgs e)
        {
            for (int j = 0; j< 20; j++)
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (!listView1.Items[i].SubItems[3].Text.Contains("招商"))
                    {
                        listView1.Items.RemoveAt(i);
                    }

                }
            }
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //fiddler.fiddlerUse fd = new fiddler.fiddlerUse();
            //fd.Show();

            gettoken();
        }
    }
}
