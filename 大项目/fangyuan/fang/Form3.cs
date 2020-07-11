using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using Microsoft.Win32;
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

namespace fang
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            
        }

        private void Form3_Load(object sender, EventArgs e)
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


                HttpHelper http = new HttpHelper();
                HttpItem item = new HttpItem()
                {
                    URL = Url,
                    Method = "GET",//URL     可选项 默认为Get  
                    Timeout = 100000,//连接超时时间     可选项默认为100000  
                    ReadWriteTimeout = 30000,//写入Post数据超时时间     可选项默认为30000  
                    IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写  
                    Cookie = "",//字符串Cookie     可选项  
                    UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",//用户的浏览器类型，版本，操作系统     可选项有默认值  
                    Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值  
                    ContentType = "text/html",//返回类型    可选项有默认值  
                    Referer = "http://www.sufeinet.com",//来源URL     可选项  
                    Allowautoredirect = false,//是否根据３０１跳转     可选项  
                    AutoRedirectCookie = false,//是否自动处理Cookie     可选项  
                                               //CerPath = "d:\123.cer",//证书绝对路径     可选项不需要证书时可以不写这个参数  
                                               //Connectionlimit = 1024,//最大连接数     可选项 默认为1024  
                    Postdata = "",//Post数据     可选项GET时不需要写  
                                  //ProxyIp = "192.168.1.105：2020",//代理服务器ID     可选项 不需要代理 时可以不设置这三个参数  
                                  //ProxyPwd = "123456",//代理服务器密码     可选项  
                                  //ProxyUserName = "administrator",//代理服务器账户名     可选项  
                    ResultType = ResultType.String,//返回数据类型，是Byte还是String  
                };
                item.Header.Add("nsign", "1000d5b9d5779bbedbee0936eeaae1ba22c4");//设置请求头信息（Header）  
               
                HttpResult result = http.GetHtml(item);
                string html = result.Html;
                string cookie = result.Cookie;
                return html;

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return "";
            }
           
        }
        #endregion

        #region  58经纪人
        public void jingjiren()
        {

            try
            {

                for (int i = 1; i < 3; i++)
                {


                    String Url = "https://appsale.58.com/mobile/v6/broker/list?ajk_city_id=2350&app=i-wb&udid2=bc7859f092322c90d7919f0427f7552e9a07154b&v=12.3.1&city_id=2350&page="+i+"&page_size=25";

                    string html = GetUrl(Url);
                    textBox1.Text = html;

                    MatchCollection names = Regex.Matches(html, @"""brokerId"":""([\s\S]*?)name"":""([\s\S]*?)""");
                    MatchCollection tels = Regex.Matches(html, @"""mobile"":""([\s\S]*?)""");
                    MatchCollection storenames = Regex.Matches(html, @"""companyName"":""([\s\S]*?)""");


                    if (names.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;

                    for (int j = 0; j < names.Count; j++)
                    {


                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(names[j].Groups[2].Value.Trim());
                        lv1.SubItems.Add(tels[j].Groups[1].Value.Trim());
                        lv1.SubItems.Add(storenames[j].Groups[1].Value.Trim());

                        listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置




                    }
                }




            }


            catch (System.Exception ex)
            {

              textBox1.Text=  ex.ToString();
            }

        }

        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button2.Text = "停止";
            Thread thread = new Thread(new ThreadStart(jingjiren));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();



        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Text = "已停止";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

          
        }
    }
}
