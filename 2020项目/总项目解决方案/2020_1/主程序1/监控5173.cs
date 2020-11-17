using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序1
{
    public partial class 监控5173 : Form
    {
        public 监控5173()
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


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "session-id=144-7450304-7580635; session-id-time=2082787201l; ubid-main=133-9411273-8184314; x-wl-uid=1AR+eCj1iY57TRhM7A2m5KF9SEb1ho13Om87l60jAFJAp10qHX8GNgnZcOFTknCbmNkftPnMho/k=; aws-priv=eyJ2IjoxLCJldSI6MCwic3QiOjB9; aws-target-static-id=1536650638823-915613; s_fid=16BD3861C3483809-386224FB67B4E94E; regStatus=pre-register; s_dslv=1536656308918; i18n-prefs=USD; lc-main=zh_CN; sp-cdn=\"L5Z9: CN\"; session-token=/8/yst6nJSzUghSOya1omO6MEhQ/Moyyq2FsFStf5zcm4cZPhl38RIpfC+UZyiw//J9HubG+McoZMSB4hRyykQZ0SH1X07eSi5nxcOjmHQshqSmCJD6tL8cgFOFCByRnF1EJMjmxRfVwTkZZ/4yLqjzBQ2Ik6WclU4tG1u7+4UCFeGDYa//WLb3fCGfB6RuU; csm-hit=tb:DT2JH7KAE9BTWY50PJA8+s-DT2JH7KAE9BTWY50PJA8|1585472314824&t:1585472314824&adb:adblk_no";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://www.amazon.com/s?k=6Q0+959+856&__mk_zh_CN=%E4%BA%9A%E9%A9%AC%E9%80%8A%E7%BD%91%E7%AB%99&ref=nb_sb_noss";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("sec-fetch-mode:navigate");
                headers.Add("sec-fetch-site:same-origin");
                headers.Add("sec-fetch-user:?1");
                headers.Add("upgrade-insecure-requests: 1");
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

        string constr = "";
        public void insertData(string value)
        {
           
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();



            MySqlCommand cmd = new MySqlCommand("INSERT INTO data (infos,date)VALUES('" + value + " ','" + DateTime.Now.ToString() + " ')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


            int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
            if (count > 0)
            {
               

                mycon.Close();

            }
            else
            {

                mycon.Close();
            }

        }

        string path = AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {


            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {

                try
                {


                    string infos = dataGridView1.Rows[i].Cells[1].Value.ToString().Trim() + dataGridView1.Rows[i].Cells[2].Value.ToString().Trim();


                    string inurl = dataGridView1.Rows[i].Cells[0].Value.ToString().Trim();
                    string inprice = dataGridView1.Rows[i].Cells[3].Value.ToString().Trim();
                    string incount = dataGridView1.Rows[i].Cells[4].Value.ToString().Trim();
                    if (inurl != "")
                    {

                        string url = "http://yxbmall.5173.com/gamegold-facade-frontend/services/goods/QueryAllCategoryGoods?t=1588512625251&callback=jQuery1520847470474957774_1588512624480&currentUrl=" + System.Web.HttpUtility.UrlEncode(inurl) + "&_=1588512625252";
                        string html = GetUrl(url, "utf-8");

                        Match price = Regex.Match(html, @"""unitPrice"":([\s\S]*?),");
                        Match count = Regex.Match(html, @"""sellableCount"":([\s\S]*?),");

                        if (Convert.ToDouble(price.Groups[1].Value.Trim()) != Convert.ToDouble(inprice) || count.Groups[1].Value.Trim() != incount)
                        {
                            string ahtml = GetUrl(inurl, "gb2312");

                            Match qu = Regex.Match(ahtml, @"urchinTracker\(""\/search\/([\s\S]*?)""");
                            Match aprice = Regex.Match(ahtml, @"<ul class=""pdlist_unitprice"">([\s\S]*?)</ul>");


                            Match bprice = Regex.Match(aprice.Groups[1].Value, @"</li><li>([\s\S]*?)元");

                            double bili = Convert.ToDouble(bprice.Groups[1].Value) / Convert.ToDouble(price.Groups[1].Value);
                            string message = DateTime.Now.ToString() + " " + qu.Groups[1].Value + "  " + price.Groups[1].Value + "  " + count.Groups[1].Value + "  非商城：" + Regex.Replace(aprice.Groups[1].Value, "<[^>]+>", "").Trim() + "  比例" + bili.ToString();
                            label1.Text = "正在监控..." + infos;
                            insertData(System.Web.HttpUtility.UrlEncode(message.Replace("?", "")));
                            //send(GetUnicode(message));
                        }
                    }


                    Thread.Sleep(Convert.ToInt32(textBox1.Text) * 1000);

                }
                catch
                {
                    continue;
                }

            }






        }
        private void 监控5173_Load(object sender, EventArgs e)
        {

            for (int i = 0; i < 100; i++)
            {
                dataGridView1.Rows.Add();
            }






            string path = AppDomain.CurrentDomain.BaseDirectory;
            StreamReader srs = new StreamReader(path + "ip.txt", Encoding.Default);
            //一次性读取完 
            string ip = srs.ReadToEnd().Trim();
            constr = "Host ="+ip+";Database=jiagejiankong;Username=jiagejiankong;Password=rsFFARtWZ27jWPhz";


            foreach (Control ctr in this.Controls)
            {

                if (ctr is TextBox)
                {

                    
                    if (File.Exists(path + ctr.Name + ".txt"))
                    {

                        StreamReader sr = new StreamReader(path + ctr.Name + ".txt", Encoding.GetEncoding("utf-8"));
                        //一次性读取完 
                        string texts = sr.ReadToEnd();
                        ctr.Text = texts;
                        sr.Close();
                    }
                }
            }

            StreamReader srqu = new StreamReader(path + "value.txt", Encoding.GetEncoding("utf-8"));
            //一次性读取完 
            string qus = srqu.ReadToEnd().Trim();
            string[] text = qus.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                string[] value = text[i].Split(new string[] { "#" }, StringSplitOptions.None);
                dataGridView1.Rows[i].Cells[0].Value = value[0].ToString();
                dataGridView1.Rows[i].Cells[1].Value = value[1].ToString();
                dataGridView1.Rows[i].Cells[2].Value = value[2].ToString();
                dataGridView1.Rows[i].Cells[3].Value = value[3].ToString();
                dataGridView1.Rows[i].Cells[4].Value = value[4].ToString();
            }
            srqu.Close();

        }
        protected string GetUnicode(string text)
        {
            string result = "";
            for (int i = 0; i < text.Length; i++)
            {
                if ((int)text[i] > 32 && (int)text[i] < 127)
                {
                    result += text[i].ToString();
                }
                else
                    result += string.Format("\\u{0:x4}", (int)text[i]);
            }
            return result;
        }
        private void button1_Click(object sender, EventArgs e)
        {
       
                
                Thread thread1 = new Thread(new ThreadStart(run));
                thread1.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
                timer1.Start();
                timer1.Interval = Convert.ToInt32(textBox4.Text)*1000;
           

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            run();
        }
        TcpClient tcpclnt = new TcpClient();
      

        public void send(String str)
        {

            Stream stm = tcpclnt.GetStream();
            // 发送字符串
            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(str);
            //textBox1.Text += "传输中.....";
            stm.Write(ba, 0, ba.Length);


            // 关闭客户端连接
            //tcpclnt.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            label1.Text = "停止监控...";
            timer1.Stop();
        }

       

        private void 监控5173_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确认退出吗？", "退出询问"
           , MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK)
            {
                e.Cancel = true;//告诉窗体关闭这个任务取消

            }
            else
            {
                foreach (Control ctr in this.Controls)
                {
                    if (ctr is TextBox)
                    {


                        string path = AppDomain.CurrentDomain.BaseDirectory ;
                        FileStream fs1 = new FileStream(path + ctr.Name + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                        StreamWriter sw = new StreamWriter(fs1);
                        sw.WriteLine(ctr.Text.Trim());
                        sw.Close();
                        fs1.Close();

                    }
                }
                string value = "";
                for (int i = 0; i < dataGridView1.Rows.Count-1; i++)//得到总行数并在之内循环  
                {
                    
                    value += dataGridView1.Rows[i].Cells[0].Value + "#" + dataGridView1.Rows[i].Cells[1].Value + "#" + dataGridView1.Rows[i].Cells[2].Value + "#" + dataGridView1.Rows[i].Cells[3].Value + "#" + dataGridView1.Rows[i].Cells[4].Value+"\r\n";//定位到相同的单元格  
                        
                    
                }

                FileStream fs11 = new FileStream(path + "value.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                StreamWriter sw1 = new StreamWriter(fs11);
                sw1.WriteLine(value);
                sw1.Close();
                fs11.Close();

                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
    }
}
