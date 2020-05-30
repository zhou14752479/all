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
using helper;

namespace 主程序202006
{
    public partial class 我的发票 : Form
    {
        public 我的发票()
        {
            InitializeComponent();
        }

        public string token= "ad6ea950-909b-477a-a140-64dd448c8052";

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";

                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("client_type: app-ios");
                headers.Add("version: v1.0.7");
                //添加头部
              
                request.ContentLength = postData.Length;
               
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "myinvoiceappro/1 CFNetwork/978.0.7 Darwin/18.6.0";

                request.Headers.Add("Cookie", "");
              
                request.Referer = "";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

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

        bool zanting = true;
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {

            string url = "https://app.my-invoice.cn/myinvoice-invoice/v1.0.7/secured/getInvoiceList";
            string postdata = "token="+token+"&fplb=&cxrq=&checkStatus=&todo=&userSettingType=0";
                string html = PostUrl(url,postdata);

            MatchCollection name = Regex.Matches(html, @"""xsf_MC"":""([\s\S]*?)""");
           
            MatchCollection price = Regex.Matches(html, @"""jshj"":""([\s\S]*?)""");

            MatchCollection fp_DM = Regex.Matches(html, @"""fp_DM"":""([\s\S]*?)""");
            MatchCollection fp_HM = Regex.Matches(html, @"""fp_HM"":""([\s\S]*?)""");

            for (int i = 0; i < name.Count; i++)
            {
                string ahtml = PostUrl("https://app.my-invoice.cn/myinvoice-invoice/invoice/detail/getMd5FpdmFphm", "token="+token+"&fpdm=" + fp_DM[i].Groups[1].Value+"&fphm=" +fp_HM[i].Groups[1].Value);
                Match fpdm = Regex.Match(ahtml, @"""fpdm"":""([\s\S]*?)""");
                Match fphm = Regex.Match(ahtml, @"""fphm"":""([\s\S]*?)""");


                string bhtml = PostUrl("https://app.my-invoice.cn/myinvoice-invoice/invoice/detail/queryInvoiceInfoByDmHm", "token="+token+"&fpdm="+fpdm.Groups[1].Value+"&fphm="+fphm.Groups[1].Value+"&type=0");
                Match zhanghu = Regex.Match(bhtml, @"""xsf_yhzh"":""([\s\S]*?)""");
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                lv1.SubItems.Add(name[i].Groups[1].Value);
                lv1.SubItems.Add(zhanghu.Groups[1].Value);
                lv1.SubItems.Add(price[i].Groups[1].Value);
            }

    


                if (listView1.Items.Count > 2)
                {
                    listView1.EnsureVisible(listView1.Items.Count - 1);
                }




                while (this.zanting == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }

            
    



        }

        private void button5_Click(object sender, EventArgs e)
        {
            string url = "https://app.my-invoice.cn/myinvoice-account/account/app/login";
            string postdata = "account="+textBox1.Text.Trim()+"&password=" + textBox2.Text.Trim();
            string html = PostUrl(url,postdata);
            Match t = Regex.Match(html,@"""token"":""([\s\S]*?)""");
            token = t.Groups[1].Value;
           
        }

        private void 我的发票_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"weixinyuming"))
            {
                button1.Enabled = false;
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }
    }
}
