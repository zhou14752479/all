using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;
using helper;

namespace 星巴克二维码
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string path = AppDomain.CurrentDomain.BaseDirectory + "image\\";

        /// <summary>
        /// 识别图中二维码 Bitmap格式图片识别
        /// </summary>
        /// <param name="barcodeBitmap"></param>
        /// <returns></returns>
        private string DecodeQrCode(Bitmap barcodeBitmap)
        {
            BarcodeReader reader = new BarcodeReader();
            reader.Options.CharacterSet = "UTF-8";
            var result = reader.Decode(barcodeBitmap);
            return (result == null) ? null : result.Text;
        }

        /// <summary>
        /// base64转换为图像格式Bitmap
        /// </summary>
        /// <param name="strbase64"></param>
        /// <returns></returns>
        public static Bitmap Base64StringToImage(string strbase64)
        {
            try
            {
                byte[] arr = Convert.FromBase64String(strbase64);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);
                ms.Close();
                return bmp;
            }
            catch (Exception ex)
            {
                return null;
            }
        }




        private void Form1_Load(object sender, EventArgs e)
        {

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
                string COOKIE = "usid=07WwkCu3b_78aUPT; IPLOC=CN3213; SUV=00BA2DBC3159B8CD5D2585534E6EA580; CXID=5EA7E0DBFC0F423A95BC1EB511A405C7; SUID=CDB859313118960A000000005D25B077; ssuid=7291915575; pgv_pvi=5970681856; start_time=1562896518693; front_screen_resolution=1920*1080; wuid=AAElSJCaKAAAAAqMCGWoVQEAkwA=; FREQUENCY=1562896843272_13; sg_uuid=6358936283; newsCity=%u5BBF%u8FC1; SNUID=9FB9A0C8F8FC6C9FCB42F1E4F9BFB645; sortcookie=1; sw_uuid=3118318168; ld=3Zllllllll2NrO7hlllllVLmmtGlllllGqOxBkllllwlllllVklll5@@@@@@@@@@; sct=20";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://news.sogou.com/news?query=site%3Asohu.com+%B4%F3%CA%FD%BE%DD&_ast=1571813760&_asf=news.sogou.com&time=0&w=03009900&sort=1&mode=1&manual=&dp=1";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";

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






        bool zanting = true;

        #region  主程序
        public void run()
        {
            try

            {

                StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.Default);
                string text = streamReader.ReadToEnd();
                string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] != "")
                    {
                        string url = fanhuiurl(array[i]);

                        Match uid = Regex.Match(url, @"coupon=.*");
                      
                        string URL = "https://b2bcoupons.starbucks.com.cn/interFace/digitalcoupon.ashx?Action=getCouponInfo&coupon=" + uid.Groups[0].Value.Replace("coupon=", "").Trim();
                       
                        string html = GetUrl(URL);
                        //textBox2.Text = html;
                        Match imagedata = Regex.Match(html, @"""msg"":""([\s\S]*?)""");
                        Match duihuan = Regex.Match(html, @"""status_name"":""([\s\S]*?)""");
                        if (duihuan.Groups[1].Value.Contains("未"))
                        {
                            Bitmap bit = new Bitmap(Base64StringToImage(imagedata.Groups[1].Value));
                            string value = DecodeQrCode(bit);
                            if (value != null)
                            {
                                textBox2.Text += "正在抓取" + array[i] + "\r\n";
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add(array[i]);
                                lv1.SubItems.Add(value);
                            }
                            else
                            {
                                textBox2.Text += "正在抓取" + array[i] + "\r\n";
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add(array[i]);
                                lv1.SubItems.Add("二维码已保存");
                                bit.Save(path + i + ".jpg");
                            }
                        }

                        else
                        {
                            textBox2.Text += "正在抓取" + array[i] + "\r\n";
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                            lv1.SubItems.Add(array[i]);
                            lv1.SubItems.Add("已兑换");
                        }

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        Thread.Sleep(500);
                    }
                }


                method.DataTableToExcelTime(method.listViewToDataTable(this.listView1), true);
               
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion


      
        private void Button2_Click(object sender, EventArgs e)
        {
            


            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

        }
    

    static string fanhuiurl(string cahxunurl)
    {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
            string url = "";
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(cahxunurl);
        req.Method = "HEAD";
        req.AllowAutoRedirect = false;
        HttpWebResponse myResp = (HttpWebResponse)req.GetResponse();
        if (myResp.StatusCode == HttpStatusCode.Redirect)
        { url = myResp.GetResponseHeader("Location"); }
        return url;
    }

    private void Button5_Click(object sender, EventArgs e)
    {
        textBox2.Text = "";
        listView1.Items.Clear();

    }

    private void Button1_Click(object sender, EventArgs e)
    {
        string url = fanhuiurl("http://s1se.cn/rqmyUrRyQB");

        Match uid = Regex.Match(url, @"coupon=.*");
        string URL = "https://b2bcoupons.starbucks.com.cn/interFace/digitalcoupon.ashx?Action=getCouponInfo&coupon=" + uid.Groups[0].Value.Replace("coupon=", "").Trim();

        string html = GetUrl(URL);
        Match imagedata = Regex.Match(html, @"""msg"":""([\s\S]*?)""");

        string value = DecodeQrCode(Base64StringToImage(imagedata.Groups[1].Value));
        MessageBox.Show(value);
        zanting = false;
    }

    private void Button4_Click(object sender, EventArgs e)
    {
        zanting = true;
    }

    private void Button6_Click(object sender, EventArgs e)
    {
        bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
        if (flag)
        {
            this.textBox1.Text = this.openFileDialog1.FileName;
        }
    }

    private void Button3_Click(object sender, EventArgs e)
    {
        method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
    }

    private void 复制网址ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Clipboard.SetDataObject(this.listView1.SelectedItems[0].SubItems[1].Text);
    }

    private void 复制串码ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Clipboard.SetDataObject(this.listView1.SelectedItems[0].SubItems[2].Text);

    }

    private void 重新扫描ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        string url = fanhuiurl(this.listView1.SelectedItems[0].SubItems[1].Text);

        Match uid = Regex.Match(url, @"coupon=.*");
        string URL = "https://b2bcoupons.starbucks.com.cn/interFace/digitalcoupon.ashx?Action=getCouponInfo&coupon=" + uid.Groups[0].Value.Replace("coupon=", "").Trim();

        string html = GetUrl(URL);
        Match imagedata = Regex.Match(html, @"""msg"":""([\s\S]*?)""");

        string value = DecodeQrCode(Base64StringToImage(imagedata.Groups[1].Value));

        this.listView1.SelectedItems[0].SubItems[2].Text = value;
    }

    private void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }


    }
}
