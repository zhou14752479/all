﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 启动程序
{
    public partial class 社保查询 : Form
    {
        public 社保查询()
        {
            InitializeComponent();
        }

        private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        /// <summary>
        /// 获取时间戳毫秒
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalMilliseconds);
            return a.ToString();
        }

        #region  获取32位MD5加密
        public static string GetMD5(string myString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.UTF8.GetBytes(myString);//
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x");
            }

            return byte2String;
        }

        #endregion

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
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "http://app.gjzwfw.gov.cn/jmopen/webapp/html5/unZip/21804951cd294d869e0f92c27ba118a6/qyylbacbrypc/index.html";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("sec-fetch-mode:navigate");
                headers.Add("sec-fetch-site:same-origin");
                headers.Add("sec-fetch-user:?1");
                headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                request.KeepAlive = true;
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

       
        public static string sign = GetMD5("qyylbacbrypc" + time);

        public static string ggsjpt_sign = GetMD5("ada72850-2b2e-11e7-985b-008cfaeb3d74995e00df72f14bbcb7833a9ca063adef"+ time);
        public static string time;

        public void run()
        {
            
         

            
            string url = "http://app.gjzwfw.gov.cn/jmopen/interfaces/wxTransferPort.do?callback=jQuery18309492701749972507_"+time+"&requestUrl=http%3A%2F%2Fapp.gjzwfw.gov.cn%2Fjimps%2Flink.do&datas=dhzkh%22param%22%3A%22dhzkh%5C%22from%5C%22%3A%5C%221%5C%22%2C%5C%22key%5C%22%3A%5C%2291da7d51a42542219852bb3df4399d03%5C%22%2C%5C%22requestTime%5C%22%3A%5C%22" + time+"%5C%22%2C%5C%22sign%5C%22%3A%5C%22"+sign+"%5C%22%2C%5C%22zj_ggsjpt_app_key%5C%22%3A%5C%22ada72850-2b2e-11e7-985b-008cfaeb3d74%5C%22%2C%5C%22zj_ggsjpt_sign%5C%22%3A%5C%22"+ggsjpt_sign+"%5C%22%2C%5C%22zj_ggsjpt_time%5C%22%3A%5C%22"+time+ "%5C%22%2C%5C%22name%5C%22%3A%5C%22%E6%9D%8E%E5%86%AC%E5%BC%BA%5C%22%2C%5C%22cardId%5C%22%3A%5C%22330482199411060619%5C%22%2C%5C%22additional%5C%22%3A%5C%22%5C%22dhykh%22dhykh&heads=&_="+time;
            textBox1.Text = url;
            string html = GetUrl(url,"utf-8");
            MessageBox.Show(html);
        }

        private void 社保查询_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            time = GetTimeStamp();
            run();
        }



    }
}
