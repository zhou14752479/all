using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;


namespace 资和信
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
       public string COOKIE = "CURRENT_WEB_SITE=17849; AREA_WEB_SITE=17849; CITY_ID=\"140100,350100,130200,130300,110100,330500,330400,450300,450400,450700,450900,451200,451300,150400,150700,150800,330900,331000,340200,340300,340600,340800,341200,341300,341500,341800,350400,350600,350900,360200,360500,360700,360800,360900,361100,370300,370500,370600,370800,370900,371100,371200,371400,371500,371700,330200,130400,130500,130800,130900,140200,140300,140500,140700,140800,141000,520100,520300,520400,522400,522600,540100,542200,542300,542500,542600,620700,620800,621100,621200,623000,632200,632500,632700,450200,450500,450800,451100,451400,141100,330800,331100,340400,340700,341100,341400,341700,350500,350800,360300,360600,11454,450600,451000,460200,340500,341000,341600,350300,350700,360400,361000,370400,370700,371000,371300,371600,330300,130700,131100,140400,140600,140900,520200,522200,522300,522700,542100,542400,620200,620900,622900,632100,632300,632600,632800,64449,340100,620100,620600,620300,620400,360100,630100,370200,370100,330600,330100,330700,620500,621000,130100,130600,131000,460100,450100,652100,84721,650100,650200,652200,652300,652700,652800,652900,653000,653100,653200,654000,654200,654300,659000,640300,640400,640500,640100,640200,84851\"; UM_distinctid=1700f9bb76a71a-09274190cc5ca2-2393f61-1fa400-1700f9bb76b4c2; CNZZDATA1277805780=1244728960-1580808540-%7C1580808540; Hm_lvt_1725624b4d904369c91c3755a88ea7e2=1580809828; JSESSIONID=BE693913EC22B178E3F0976850A8AFF7";
     
        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public  string PostUrl(string url, string postData)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Post";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postData.Length;

            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.112 Safari/537.36";
            request.Headers.Add("Cookie", COOKIE);

            StreamWriter sw = new StreamWriter(request.GetRequestStream());
            sw.Write(postData);
            sw.Flush();


            WebResponse response = request.GetResponse();
            Stream s = response.GetResponseStream();
            StreamReader sr = new StreamReader(s, Encoding.GetEncoding("GBK"));
            string html = sr.ReadToEnd();

            sw.Dispose();
            sw.Close();
            sr.Dispose();
            sr.Close();
            s.Dispose();
            s.Close();
            return html;
        }

        #endregion
        #region GET请求
        public  string getUrl(string url)
        {

            StreamReader reader = new StreamReader(getStream(url), Encoding.GetEncoding("GBK")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

            string content = reader.ReadToEnd();
            return content;

        }
        #endregion
        #region 获取数据流
        public  Stream getStream(string Url)
        {

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
            request.Timeout = 10000;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
            request.AllowAutoRedirect = true;
            request.Headers.Add("Cookie", COOKIE);
            
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
            return response.GetResponseStream();

        }
        #endregion


        


        public void run()
        {
            StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.Default);
            string text = streamReader.ReadToEnd();
            string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != "")
                {
                    Image image = Image.FromStream(getStream("https://www.zihexin.net/Verifycode2.do"));
                   
                    OCR ocr = new OCR();
                    string value = ocr.Shibie("zhou14752479", "zhoukaige00", image);

                   
                    string html = getUrl("https://www.zihexin.net/client/card/inquiry.do?key=&index=index&card_no=" + array[i] + "&verify_code=" + value);

               
                    Match key = Regex.Match(html, @"key"" value=""([\s\S]*?)""");
                   

                    string strhtml = getUrl("https://www.zihexin.net/cardsearch/card/cardCheck.do?card_no=" + array[i] + "&key=" + key.Groups[1].Value);
                    Match jine = Regex.Match(strhtml, @"余额:</h3>([\s\S]*?)</dl>");
                    Match date = Regex.Match(strhtml, @"有效期:</h3>([\s\S]*?)</dl>");
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(array[i]);
                    lv1.SubItems.Add(jine.Groups[1].Value.Replace("<dl>","").Replace("&nbsp;","").Trim());
                    lv1.SubItems.Add(date.Groups[1].Value.Replace("<dl>", "").Replace("&nbsp;", "").Trim());
                }




            }
            

        }


    
        







        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
