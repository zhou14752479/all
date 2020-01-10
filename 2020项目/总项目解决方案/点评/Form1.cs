using System;
using System.Collections;
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

namespace 点评
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            登录 dl = new 登录("http://www.dianping.com/citylist");
            dl.Show();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            登录 dl = new 登录("https://account.dianping.com/login?redir=http://www.dianping.com");
            dl.Show();
           

        }

       public static string COOKIE = "_lxsdk_cuid=16cb6dd4ef5c8-0ab1639fef8078-f353163-1fa400-16cb6dd4ef5c8; _lxsdk=16cb6dd4ef5c8-0ab1639fef8078-f353163-1fa400-16cb6dd4ef5c8; _hc.v=b4c63e9f-f96f-ca4a-2ad0-dfa8208e6726.1566436056; s_ViewType=10; ua=dpuser_5678141658; ctu=90a81cde43e1e0934a456ec54b747c9309c7243af5f8f610acaafc50d303f141; cityid=100; switchcityflashtoast=1; aburl=1; dper=0f2c70b22cd0a4b60f7c81b238310b7f2ddbe2791e360439a7159fb9145520c025dceb4ed31348d733a9a447e52b918da8535b20ceb8ed00d79812b369d0585e711de17fb4fc1b14e34417ab12400cb42c4c16b10177eb47fff35b7d3dff8019; ll=7fd06e815b796be3df069dec7836c3df; uamo=17606117606; cy=100; cye=suqian; _lxsdk_s=16f8839588c-a64-ad8-fae%7C%7C421";
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
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                
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
        #region  点评主程序
        public void run()
        {

            try
            {

                for (int i = 1; i <= 50; i++)

                {


                    string Url = "http://www.dianping.com/suqian/ch70/r3876p" + i;

                    string html = GetUrl(Url);

                    MatchCollection all = Regex.Matches(html, @"dealgrp_id.*\d{5,}");
                    
                    //MatchCollection all = Regex.Matches(html, @"deal\/([\s\S]*?)""");

                    ArrayList lists = new ArrayList();
                    foreach (Match NextMatch in all)
                    {
                        Match uid = Regex.Match(NextMatch.Groups[0].Value, @"\d{5,}");

                        if (!lists.Contains(uid.Groups[0].Value))
                        {

                            lists.Add(uid.Groups[0].Value);
                        }

                    }

                    if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;


                    

                    foreach (string list in lists)

                    {

                        string strhtml1 = GetUrl("http://t.dianping.com/ajax/dealGroupShopDetail?dealGroupId=" + list + "&action=shops&page=1&regionId=0&cityId=100");  //定义的GetRul方法 返回 reader.ReadToEnd()


                        Match name = Regex.Match(strhtml1, @"shopName"":""([\s\S]*?)""");
                        Match tel = Regex.Match(strhtml1, @"contactPhone"":""([\s\S]*?)""");
                        Match addr = Regex.Match(strhtml1, @"address"":""([\s\S]*?)""");
                        if (!tel.Groups[1].Value.Contains("-"))
                        {
                            textBox3.Text += "-->正在采集" + name.Groups[1].Value+"\r\n";
                            
                            ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                            listViewItem.SubItems.Add(name.Groups[1].Value );
                            listViewItem.SubItems.Add(tel.Groups[1].Value);
                            listViewItem.SubItems.Add(addr.Groups[1].Value);
                        }
                        else
                        {
                            textBox3.Text += "-->手机号不符合要求或者重复跳过采集"+"\r\n";
                        }


                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }

                        
                        Application.DoEvents();
                        Thread.Sleep(100);


                    }

                }


            }


            catch (System.Exception ex)
            {
                ex.ToString();
            }
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
