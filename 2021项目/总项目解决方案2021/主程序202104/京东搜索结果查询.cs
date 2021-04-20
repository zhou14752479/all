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

namespace 主程序202104
{
    public partial class 京东搜索结果查询 : Form
    {
        public 京东搜索结果查询()
        {
            InitializeComponent();
        }

        string cookie = "buildtime=20210330;wxapp_version=7.3.300;wxapp_scene=1001;visitkey=50156747055161581922619271;shshshfp=8ef1052498d8c579b6e16b0a06aeb4fc;gender=1;province=Jiangsu;city=Suqian;country=China;wxNickName=%E6%80%9D%E5%BF%86%E8%BD%AF%E4%BB%B6;wxAvatarUrl=https%3A%2F%2Fthirdwx.qlogo.cn%2Fmmopen%2Fvi_32%2FQ0j4TwGTfTK1iaxx2AdrAQGVS9sk57ribM06T7gHBwprBI7ydoeIr8rmtTVXqu92RCVjiaaQTwU7NlslWvJybJouw%2F132;nickName=%E6%80%9D%E5%BF%86%E8%BD%AF%E4%BB%B6;avatarUrl=https%3A%2F%2Fthirdwx.qlogo.cn%2Fmmopen%2Fvi_32%2FQ0j4TwGTfTK1iaxx2AdrAQGVS9sk57ribM06T7gHBwprBI7ydoeIr8rmtTVXqu92RCVjiaaQTwU7NlslWvJybJouw%2F132;cid=5;maxAge=7200;mcossmd=c7eb9353e0b29120db58e24df711b3d1;unionid=oCwKwuB0yA4CUdAQwTBmCxfMQJvs;wid=4043897189;wq_unionid=oCwKwuB0yA4CUdAQwTBmCxfMQJvs;wxapp_openid=oA1P50GfD-1168TtyncBC3sUNneg;skey=zw964886F1D6CB6F5B4559147C43CEAE8CF9E6168C5F580E23006D82C650B18887282F494DD0758FBC2838C95A0F6EBAAC8DD7659CC41BA69CA88A194462B6167B09D7F6289D5E7B2D85BE2D154DAFD9C2550B7C7259B3D5606B87D08C0DED7E3C;ou=EF1D316B9D1ABCE676BCB6979B111FBB698F4B7F75CD2C4E631E6E9A6A84DB7C1B3ABE7D2181E1E40A6DD570748CEFCE3157CDC9057DA09F163388F25B286BADBBB8114BF39ACD9F4C5918404328E826;wxapp_type=1;__jda=122270672.10803b510b8463e10f0186cb477e2082.1617679782983.1617679782983.1617679782983.1;network=wifi;__wga=1617679796220.1617679783724.1617679783724.1617679783724.2.1;__jdv=122270672%7Cdirect%7Ct_1000578828_xcx_1001_fxrk%7Cxcx%7C-%7C1617679796222;PPRD_P=CT.138567.2.2-EA.17078.1.1-LOGID.1617679796300.309508824;shshshfpa=fa8a172f-6a37-fc58-3137-1b6a41aac91e-1617679784;shshshsID=04c8411f8d2281c7cccaa196f8865842_2_1617679796022;shshshfpb=ypvaN4rDbjXbd7A%2BFDuxoBQ%3D%3D;hf_time=1617679784750;jdpin=jd_7a668be69efce;open_id=oTGnpnLfrFPkpF7oqXRSGr-iKHPc;pin=jd_7a668be69efce;pinStatus=4;wq_uin=4043897189;wq_skey=zw964886F1D6CB6F5B4559147C43CEAE8CF9E6168C5F580E23006D82C650B18887282F494DD0758FBC2838C95A0F6EBAAC8DD7659CC41BA69CA88A194462B6167B09D7F6289D5E7B2D85BE2D154DAFD9C2550B7C7259B3D5606B87D08C0DED7E3C;wq_uits=;cartLastOpTime=1617679806;cartNum=4;wq_addr=138453088%7C12_933_3407_40393%7C%E6%B1%9F%E8%8B%8F_%E5%AE%BF%E8%BF%81%E5%B8%82_%E5%AE%BF%E5%9F%8E%E5%8C%BA_%E5%8F%8C%E5%BA%84%E9%95%87%7C%E6%B1%9F%E8%8B%8F%E5%AE%BF%E8%BF%81%E5%B8%82%E5%AE%BF%E5%9F%8E%E5%8C%BA%E5%8F%8C%E5%BA%84%E9%95%87%E5%98%89%E7%9B%9B%E9%BE%99%E5%BA%AD%E5%9B%BD%E9%99%8521%E5%B9%A2%E4%B8%80%E5%8D%95%E5%85%83302%7C118.253%2C33.9411";
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public  string GetUrl(string Url, string charset)
        {
            string html = "";
            string COOKIE = cookie;
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = "https://servicewechat.com/wx91d27dbf599dff74/517/page-frame.html";
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 100000;

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
        DataTable dt =null;
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
             openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
           // openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox2.Text = openFileDialog1.FileName;
                 dt = method.ExcelToDataTable(textBox2.Text, true);
              
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
        }
        Thread thread;

        public string getcount(string keyword)
        {
            try
            {
                string url = "https://wxa.jd.com/wqsou.jd.com/search/searchwxapp?datatype=4&scene=1001&platform=windows&key="+keyword+"&yhyx=1&pagesize=20&longitude=118.24239&latitude=33.96271&g_ty=ls&g_tk=1168521439";
              
                string html =GetUrl(url, "utf-8");
                string count = Regex.Match(html, @"OrgSkuCount"":""([\s\S]*?)""").Groups[1].Value;

                if (count == "")
                {
                    textBox1.Text = html;
                }
                return count;
              
            }
            catch (Exception)
            {
              
                return "";
            }
        }


        public void run()
        {
          
            try
            {

                // method.ShowDataInListView(dt, listView1);
                for (int i = 0; i < dt.Rows.Count; i++)
                {


                    DataRow dr = dt.Rows[i];
                    ListViewItem lv1 = listView2.Items.Add((listView2.Items.Count + 1).ToString()); //使用Listview展示数据
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j == 7)
                        {
                            string count = getcount(dr[0].ToString());
                            lv1.SubItems.Add(count.ToString());
                        }
                        else
                        {
                            lv1.SubItems.Add(dr[j].ToString());
                        }
                       
                      
                    }
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;
                    Thread.Sleep(500);


                }

            }
            catch (Exception)
            {


            }
        }
      
        bool zanting = true;
        bool status = true;

        private void button6_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"6WxcMB"))
            {

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

        private void button1_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
        }
    }
}
