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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 模拟采集
{
    public partial class 亚马逊账户管理 : Form
    {
        public 亚马逊账户管理()
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

        public void getall()
        {
            listView1.Items.Clear();
            string url = " http://111.229.244.97/do.php?method=getall&username=&password=";
            string html = GetUrl(url);
           
            MatchCollection a1s = Regex.Matches(html, @"username:([\s\S]*?) ");
            MatchCollection a2s = Regex.Matches(html, @"password:([\s\S]*?) ");
            MatchCollection a3s = Regex.Matches(html, @"expiretimestamp:([\s\S]*?) ");
           
            for (int i = 0; i < a1s.Count; i++)
            {
                
                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据   
                lv1.SubItems.Add(a1s[i].Groups[1].Value.Trim());
                lv1.SubItems.Add(a2s[i].Groups[1].Value.Trim());
                lv1.SubItems.Add(ConvertStringToDateTime(a3s[i].Groups[1].Value.Trim()).ToString());
            }
        }
        private DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(Convert.ToDouble(timeStamp));

        }
        public long GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalSeconds);
            return a;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("账户或密码为空");
                return;
            }

            long timestamp = GetTimeStamp() + (86400 * 30);

            switch (comboBox1.Text.Trim())
            {
                case ("一天"):
                    timestamp = GetTimeStamp() + 86400;
                    break;
                case ("一周"):
                    timestamp = GetTimeStamp() + (86400 * 7);
                    break;
                case ("一个月"):
                    timestamp = GetTimeStamp() + (86400 * 30);
                    break;
                case ("半年"):
                    timestamp = GetTimeStamp() + (86400 * 180);
                    break;
                case ("一年"):
                    timestamp = GetTimeStamp() + (86400 * 365);
                    break;

            }
            
            label3.Text = GetUrl("http://111.229.244.97/do.php?method=add&username="+textBox1.Text.Trim()+"&password="+textBox2.Text.Trim()+"&time=" + timestamp).Trim();
           
            textBox1.Text = "";
            textBox2.Text = "";
          
            getall();
        }

        private void 亚马逊账户管理_Load(object sender, EventArgs e)
        {
            getall();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.CheckedItems.Count; i++)
            {
                label3.Text = GetUrl("http://111.229.244.97/do.php?method=del&username=" + listView1.CheckedItems[i].SubItems[1].Text.Trim() + "&password=").Trim();
                getall();
            }
            
        }
    }
}
