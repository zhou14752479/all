using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 家乐园开票
{
    public partial class 家乐园开票 : Form
    {
        public 家乐园开票()
        {
            InitializeComponent();
        }

        bool status = true;

        string cookie = "JSESSIONID=C2D0D5CFE8E78CF06A6C24AF6C88B490; 99692f13925b47cb8a1a7be1e9d04a16=WyI0MTAyMDc2NjE5Il0";
       
        string userinfo = "";
        public void run()
        {

            readtxt1();
            readtxt2();
            try
            {

               
                for (int i = 0; i < jiaoyi.Count; i++)
                {

                    for (int j = 0; j < jine.Count; j++)
                    {
                        
                        if (status == false)
                            return;
                        string jy=jiaoyi[i];
                        string je=jine[j];

                        string p = textBox1.Text.Trim() + "-" + textBox2.Text.Trim() + "-"+jy+"-"+Convert.ToDouble(je)*100;

                        logtxt.Text = "开始下载："+p;
                        //获取基础信息
                        string url = "https://fapiao.subuy.com/bg/wechatInvoiceController.do?readEwm&s=1&dkzbid=null&hbdjbh=null&delhbdj=null&p=" + p + "&userInfo=" + userinfo;
                        string html = method.GetUrlWithCookie(url, cookie, "utf-8");

                        logtxt.Text = html;
                        string zbxx = Regex.Match(html, @"""zbxx"":\[([\s\S]*?)\],""cbxx").Groups[1].Value;
                        zbxx = zbxx.Replace("clientname\":\"\"", "clientname\":\"" + textBox3.Text + "\"");
                        zbxx = zbxx.Replace("clientemail\":\"\"", "clientemail\":\"" + textBox4.Text + "\"");

                      
                        Thread.Sleep(1000);
                        //开始开票
                        string postdata = "zbxx=" + System.Web.HttpUtility.UrlEncode(zbxx);
                        string ahtml = method.PostUrlDefault("https://fapiao.subuy.com/bg/wechatInvoiceController.do?invoiceSave", postdata, cookie);
                      

                        //等待

                        Thread.Sleep(Convert.ToInt32(textBox5.Text) * 1000);

                        //校验，下载
                        string name = System.Web.HttpUtility.UrlEncode(textBox3.Text);
                        string picif_html = creatpic(name, p);

                        if (picif_html.Contains("校验通过"))
                        {


                            string path = AppDomain.CurrentDomain.BaseDirectory + "\\发票\\";
                            string picurl = "https://fapiao.subuy.com/bg//tmp/fpToPic/" + p + "/1.jpg";
                          
                            if(picurl!="")
                            {
                                logtxt.Text = "下载成功..."+picurl;
                            }
                            method.downloadFile(picurl, path, p + ".jpg", "");
                        }

                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {

                logtxt.Text=ex.ToString();
            }
        }
        
        List<string> jiaoyi = new List<string>();
        List<string> jine = new List<string>();


        public void readtxt1()
        {


            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                StreamReader sr = new StreamReader(path + "交易号.txt", method.EncodingType.GetTxtType(path + "交易号.txt"));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {
                    jiaoyi.Add(text[i]);
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            }
            catch (Exception ex)
            {
                MessageBox.Show("交易号文本不存在，请放入软件文件夹");
            }
        }

        public void readtxt2()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                StreamReader sr = new StreamReader(path + "金额.txt", method.EncodingType.GetTxtType(path + "金额.txt"));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {
                    jine.Add(text[i]);
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            }
            catch (Exception ex)
            {

                MessageBox.Show("金额文本不存在，请放入软件文件夹");
            }
        }

        public string creatpic(string name,string djbh)
        {
           
            string postdata = "fptt="+name+"&sjhm=&yx=&djbh="+djbh;
            string ahtml = method.PostUrlDefault("https://fapiao.subuy.com/bg/wechatInvoiceController.do?jyfpxx", postdata, cookie);

            return ahtml;
        }

        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
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
            status = false;
        }
    }
}
