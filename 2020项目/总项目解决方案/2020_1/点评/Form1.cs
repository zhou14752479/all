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
using helper;

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
           
           

        }

        string cate = "";

  

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
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.108 Safari/537.36";

                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", 登录.cookie);
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


                    string Url = "http://www.dianping.com/"+textBox1.Text.Trim()+"/"+cate+"p" + i;

                    string html = GetUrl(Url);

                    MatchCollection all = Regex.Matches(html, @"data-shopid=""([\s\S]*?)""");
                    Match cityId = Regex.Match(html, @"'cityId': '([\s\S]*?)'");

                  

                    ArrayList lists = new ArrayList();
                    foreach (Match NextMatch in all)
                    {
                        

                        if (!lists.Contains(NextMatch.Groups[1].Value))
                        {

                            lists.Add(NextMatch.Groups[1].Value);
                        }

                    }

                   

                    if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;


                    

                    foreach (string list in lists)

                    {

                        string strhtml1 = GetUrl("http://www.dianping.com/shop/"+list);  //定义的GetRul方法 返回 reader.ReadToEnd()


                        Match name = Regex.Match(strhtml1, @"<title>([\s\S]*?)地址");
                        Match tel = Regex.Match(strhtml1, @"itemprop=""tel"">([\s\S]*?)<");
                        Match addr = Regex.Match(strhtml1, @"address"" title=""([\s\S]*?)""");
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
                        Thread.Sleep(1000);


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
            zanting = true;
            //if (comboBox1.Text == "美食")
            //{
            //    cate = "ch10/";
            //}
             if (comboBox1.Text == "休闲娱乐")
            {
                cate = "ch30/";
            }
            else if (comboBox1.Text == "结婚")
            {
                cate = "ch55/";
            }
            else if (comboBox1.Text == "丽人")
            {
                cate = "ch50/";
            }
            else if (comboBox1.Text == "美发")
            {
                cate = "ch50/g157";
            }
            else if (comboBox1.Text == "美容")
            {
                cate = "ch50/g158";
            }
            else if (comboBox1.Text == "亲子")
            {
                cate = "ch70/";
            }
            else if (comboBox1.Text == "运动健身")
            {
                cate = "ch45/";
            }
            else if (comboBox1.Text == "购物")
            {
                cate = "ch20/";
            }
            else if (comboBox1.Text == "家装")
            {
                cate = "ch90/";
            }
            else if (comboBox1.Text == "学习培训")
            {
                cate = "ch75/";
            }
            else if (comboBox1.Text == "生活服务")
            {
                cate = "ch80/";
            }
            else if (comboBox1.Text == "医疗健康")
            {
                cate = "ch85/";
            }
            else if (comboBox1.Text == "爱车")
            {
                cate = "ch65/";
            }
            else if (comboBox1.Text == "宠物")
            {
                cate = "ch95/";
            }


            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            method.ListviewToTxt(listView1,2);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            登录 dl = new 登录("https://account.dianping.com/login?redir=http://www.dianping.com");
            dl.Show();
        }
    }
}
