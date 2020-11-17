using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using helper;

namespace 主程序202008
{
    public partial class 天猫店铺宝贝 : Form
    {
        public 天猫店铺宝贝()
        {
            InitializeComponent();
        }

        static string cookie = "cna=b2RCFw6lRXICAXnq99rJ24nX; hng=CN%7Czh-CN%7CCNY%7C156; lid=zkg852266010; enc=xoO7%2BHYNBa0jFGMMnLKZzcoPElaNVz405AW3D17MDCLUtC%2F8eSYq0gdPSdKqruMeTpsJD0HIRv4Q2LostA15Fw%3D%3D; cq=ccp%3D1; pnm_cku822=; sgcookie=EXdUyoXOwotAzCmHvScZw; uc3=lg2=VFC%2FuZ9ayeYq2g%3D%3D&vt3=F8dCufTDD06VG%2BhJIr4%3D&nk2=GcOvCmiKUSBXqZNU&id2=UoH62EAv27BqSg%3D%3D; t=45711f8a47dda0e864b78008d85061ca; tracknick=zkg852266010; uc4=id4=0%40UOnlZ%2FcoxCrIUsehKGIWtJ03TJ4R&nk4=0%40GwrkntVPltPB9cR46GnfG8%2F8S9%2BVUe4%3D; lgc=zkg852266010; _tb_token_=37335e97f4133; cookie2=1813a1b49c7e319f6478da10b121b345; x5sec=7b2273686f7073797374656d3b32223a223633313331343337373635363432653136393562373363633062363463313736434e757469666f46454d6e556c726e6835492b5563426f4d4d5441314d6a4d304e7a55304f447378227d; tfstk=cNM5BP1_Cab5DpdFU0t48aZji22NZfj_AQaoVb1lmJcWWVo5i_6a5uHrtwBY6o1..; l=eBxzLJdcQlCmzg-zBOfZourza77TSIRAguPzaNbMiOCPO3CMJR6hWZPqFGYHCnGVh6j6R3JMwUXJBeYBqBOInxvtr7kH3Kkmn; isg=BNHRBep_aqIIRIdPXWBcfDfb4N1rPkWwSa0NR7NmwRiyWvGs-4z6giKy-C680t3o";

        #region 苏飞请求
        public static string gethtml(string url)
        {
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = url,//URL     必需项  
                Method = "GET",//URL     可选项 默认为Get  
                Timeout = 100000,//连接超时时间     可选项默认为100000  
                ReadWriteTimeout = 30000,//写入Post数据超时时间     可选项默认为30000  
                IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写  
                Cookie = cookie,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36",//用户的浏览器类型，版本，操作系统     可选项有默认值  
                Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值  
                ContentType = "text/html",//返回类型    可选项有默认值  
                Referer = "",//来源URL     可选项  
                Allowautoredirect = true,//是否根据３０１跳转     可选项  
                AutoRedirectCookie = true,//是否自动处理Cookie     可选项  
                                          //CerPath = "d:\123.cer",//证书绝对路径     可选项不需要证书时可以不写这个参数  
                                          //Connectionlimit = 1024,//最大连接数     可选项 默认为1024  
                Postdata = "",//Post数据     可选项GET时不需要写  
                              //ProxyIp = "192.168.1.105：2020",//代理服务器ID     可选项 不需要代理 时可以不设置这三个参数  
                              //ProxyPwd = "123456",//代理服务器密码     可选项  
                              //ProxyUserName = "administrator",//代理服务器账户名     可选项  
                ResultType = ResultType.String,//返回数据类型，是Byte还是String  
            };
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
             //cookie = result.Cookie;


            return html;

        }

        #endregion

        private void 天猫店铺宝贝_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {

            for (int i = 1; i < 99; i++)
            {



                Match shopUrl = Regex.Match(textBox1.Text.Trim(), @"https://([\s\S]*?)/");
                Match cateId = Regex.Match(textBox1.Text.Trim(), @"category-([\s\S]*?)\.");
                Match wid = Regex.Match(textBox1.Text.Trim(), @"\d{11}");

                string url = "https://" + shopUrl.Groups[1].Value + "/i/asynSearch.htm?_ksTS=1597880846993_137&callback=jsonp138&mid=w-" + wid.Groups[0].Value + "-0&wid=" + wid.Groups[0].Value + "&path=/category-" + cateId.Groups[1].Value + ".htm&scene=taobao_shop&catId=" + cateId.Groups[1].Value + "&pageNo="+i+"&scid=" + cateId.Groups[1].Value;
                string html = gethtml(url);
               
                MatchCollection uids = Regex.Matches(html, @"data-id=\\""([\s\S]*?)\\");
              
                foreach (Match uid in uids)
                {
                    try
                    {
                        string URL = "https://detail.tmall.com/item.htm?id=" + uid.Groups[1].Value;

                        string strhtml = gethtml(URL);


                        Match title = Regex.Match(strhtml, @"产品名称：([\s\S]*?)</li>");
                        Match cbs = Regex.Match(strhtml, @"出版社名称:&nbsp;([\s\S]*?)</li>");
                        Match isbn = Regex.Match(strhtml, @"ISBN编号:&nbsp;([\s\S]*?)</li>");
                        Match sj = Regex.Match(strhtml, @"""defaultItemPrice"":""([\s\S]*?)""");
                        Match dj = Regex.Match(strhtml, @"定价:&nbsp;([\s\S]*?)</li>");
                        Match kc = Regex.Match(strhtml, @"""quantity"":([\s\S]*?),");






                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(title.Groups[1].Value);
                        lv1.SubItems.Add(cbs.Groups[1].Value);
                        lv1.SubItems.Add(isbn.Groups[1].Value);
                        lv1.SubItems.Add(sj.Groups[1].Value);
                        lv1.SubItems.Add(dj.Groups[1].Value);
                        lv1.SubItems.Add("全新");
                        lv1.SubItems.Add(kc.Groups[1].Value);

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        Thread.Sleep(1000);
                    }
                    catch (Exception)
                    {

                        continue;
                    }




                }
                Thread.Sleep(2000);
            }


        }

        bool zanting = true;
        private void button1_Click(object sender, EventArgs e)
        {

            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"ceshi1111111"))
            {
                MessageBox.Show("验证失败");
                return;
            }
            #endregion
            //cookie = helper.Form1.cookie;
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
          
        }

        private void 天猫店铺宝贝_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
                //System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
           helper.Form1 fm1 = new helper.Form1();
            fm1.Show();
        }
    }
}
