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
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main._2019_7
{
    public partial class fang58 : Form
    {
        public fang58()
        {
            InitializeComponent();
        }
        bool status = true;
        bool zanting = true;
        int shuliang = 0;
        #region 主程序
        public void run(object url)
        {



            try
            {

                string Url = url.ToString();

                    string html = method.GetUrl(Url, "utf-8");
               
                    Match city= Regex.Match(Url, @"https:\/\/([\s\S]*?)\.");
                MatchCollection ids = Regex.Matches(html, @"esf_id:([\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                

                if (ids.Count == 0)
                        return;
                    ArrayList lists = new ArrayList();

                    foreach (Match id in ids)
                    {
                        lists.Add(id.Groups[1].Value);
                    }

                    foreach (string list in lists)

                    {
                    string purl = "https://" + city.Groups[1].Value + ".58.com/ershoufang/" + list + "x.shtml";
                    string murl = "https://m.58.com/"+ city.Groups[1].Value + "/ershoufang/"+list+"x.shtml";
                    string lurl = "https://jst1.58.com/counter?infoid="+list+"&userid=0&uname=&sid=0&lid=0&px=0&cfpath=";


                    string strhtml = method.GetUrl(purl, "utf-8");
                    string mhtml = method.GetUrl(murl, "utf-8");
                    string lhtml = method.gethtml(lurl,"", "utf-8");
                    
                    Match a0 = Regex.Match(lhtml, @"total=\d*");
                    Match a1 = Regex.Match(strhtml, @"id=''>([\s\S]*?)</span>");
                        Match a2 = Regex.Match(strhtml, @"<title>([\s\S]*?)-");
                        Match a3 = Regex.Match(strhtml, @"<span class='c_000 mr_10'>([\s\S]*?)</span>");
                        Match a4 = Regex.Match(strhtml, @"房本面积</span>([\s\S]*?)</span>");
                        Match a5 = Regex.Match(strhtml, @"售价：([\s\S]*?)\（");
                        Match a6 = Regex.Match(strhtml, @"\（([\s\S]*?)\）"); //单价
                        Match a7 = Regex.Match(strhtml, @"quyu'\)"">([\s\S]*?)</a>");
                        Match a8 = Regex.Match(strhtml, @"房屋户型</span>([\s\S]*?)</span>");
                        Match a9 = Regex.Match(strhtml, @"房屋朝向</span>([\s\S]*?)</span>");
                        Match a10 = Regex.Match(strhtml, @"所在楼层</span>([\s\S]*?)</span>");
                    Match a11 = Regex.Match(strhtml, @"建筑年代([\s\S]*?)</span>");
                    Match a12 = Regex.Match(strhtml, @"装修情况</span>([\s\S]*?)</span>");
                    Match a13 = Regex.Match(strhtml, @"<p class='pic-desc-word'>([\s\S]*?)</p>");
                    Match a14 = Regex.Match(strhtml, @"<p class='phone-num'>([\s\S]*?)</p>");
                    Match a15 = Regex.Match(mhtml, @"<h2 class=""agent-title"">([\s\S]*?)</h2>");



                    if (Convert.ToInt32(a0.Groups[0].Value.Replace("total=", "")) < Convert.ToInt32(textBox25.Text))
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(a0.Groups[0].Value.Replace("total=", "") + "/" + Regex.Replace(a1.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a2.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a3.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a4.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a5.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a6.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a7.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a8.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a9.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a10.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a11.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a12.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a13.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a14.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(a15.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(purl);
                        shuliang = shuliang + 1;
                        label22.Text = shuliang.ToString();

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)

                        {
                            return;
                        }
                        string[] text = textBox5.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        foreach (string mail in text)
                        {
                            send(textBox2.Text, textBox3.Text, textBox4.Text,mail, a2.Groups[1].Value+ purl);
                           
                        }
                        Thread.Sleep(1000);
                    }

                }
            }

            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion


        public static void send(string smtp, string fa,string ma, string address, string body)
        {
            //实例化一个发送邮件类。
            MailMessage mailMessage = new MailMessage();
            //发件人邮箱地址，方法重载不同，可以根据需求自行选择。
            mailMessage.From = new MailAddress(fa);
            //收件人邮箱地址。
            mailMessage.To.Add(new MailAddress(address));
            //邮件标题。
            mailMessage.Subject = "58房产提醒"+DateTime.Now.ToString();
            //邮件内容。
            mailMessage.Body = body;

            //实例化一个SmtpClient类。
            SmtpClient client = new SmtpClient();
            //在这里我使用的是qq邮箱，所以是smtp.qq.com，如果你使用的是126邮箱，那么就是smtp.126.com。
            client.Host = smtp;
            //使用安全加密连接。
            client.EnableSsl = true;
            //不和请求一块发送。
            client.UseDefaultCredentials = false;
            //验证发件人身份(发件人的邮箱，邮箱里的生成授权码);
            client.Credentials = new NetworkCredential(fa, ma);   //这里的密码用授权码
            //发送
            client.Send(mailMessage);
            // MessageBox.Show("发送成功");

        }
        private void fang58_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            Thread thread = new Thread(new ParameterizedThreadStart(run));
            string o = textBox1.Text;
            thread.Start((object)o);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {
                zanting = true;
            }
            else if(zanting==true)
            {
                zanting = false;
            }

           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            Thread thread = new Thread(new ParameterizedThreadStart(run));
            string o = textBox10.Text;
            thread.Start((object)o);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            Thread thread = new Thread(new ParameterizedThreadStart(run));
            string o = textBox15.Text;
            thread.Start((object)o);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            Thread thread = new Thread(new ParameterizedThreadStart(run));
            string o = textBox20.Text;
            thread.Start((object)o);
        }
        #region NPOI导出表格
        public static int DataTableToExcel(DataTable data, string sheetName, bool isColumnWritten)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
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
            string fileName = path + DateTime.Now.ToShortDateString().Replace("/","-")+ ".xlsx";

          

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
               
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return -1;
            }
        }

        #endregion
        private void Timer1_Tick(object sender, EventArgs e)
        {
            int h = Convert.ToInt32(DateTime.Now.Hour); int m = Convert.ToInt32(DateTime.Now.Minute); int s = Convert.ToInt32(DateTime.Now.Second);
            if (Convert.ToInt32(textBox27.Text) == h && Convert.ToInt32(textBox28.Text) == m && Convert.ToInt32(textBox29.Text) == s)
            {
                zanting = false;
                DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
            }

            zanting = true;
            listView1.Items.Clear();
            shuliang = 0;


        }

        private void Button10_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {
                zanting = true;
            }
            else if (zanting == true)
            {
                zanting = false;
            }
        }

        private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {
                zanting = true;
            }
            else if (zanting == true)
            {
                zanting = false;
            }
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {
                zanting = true;
            }
            else if (zanting == true)
            {
                zanting = false;
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {
                zanting = true;
            }
            else if (zanting == true)
            {
                zanting = false;
            }
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            textBox5.Text = textBox32.Text;
            textBox32.Visible = false;
        }

        private void Button12_Click(object sender, EventArgs e)
        {
            textBox6.Text = textBox32.Text;
            textBox32.Visible = false;
        }

        private void Button13_Click(object sender, EventArgs e)
        {
            textBox16.Text = textBox32.Text;
            textBox32.Visible = false;
        }

        private void Button14_Click(object sender, EventArgs e)
        {
            textBox11.Text = textBox32.Text;
            textBox32.Visible = false;
        }

        private void Button11_MouseHover(object sender, EventArgs e)
        {
            textBox32.Visible = true;

        }

        private void Button12_MouseHover(object sender, EventArgs e)
        {
            textBox32.Visible = true;
        }

        private void Button13_MouseHover(object sender, EventArgs e)
        {
            textBox32.Visible = true;
        }

        private void Button14_MouseHover(object sender, EventArgs e)
        {
            textBox32.Visible = true;
        }
    }
}
