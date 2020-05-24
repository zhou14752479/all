using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 保罗看水
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }
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
                Cookie = "",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36",//用户的浏览器类型，版本，操作系统     可选项有默认值  
                Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值  
                ContentType = "text/html",//返回类型    可选项有默认值  
                Referer = "https://www.hga030.com/app/member/FT_browse/index.php?rtype=r&uid=dwcho1vfm22747894l289778&langx=zh-cn&mtype=3&showtype=&hot_game=&hot_game=&league_id=",//来源URL     可选项  
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
            string cookie = result.Cookie;


            return html;

        }

        #endregion
        

        #region 主程序
        public void run()
        {

           
            try
            {

                listView1.Items.Clear();
                string url = "https://www.hga030.com/app/member/FT_browse/body_var.php?uid=dwcho1vfm22747894l289778&rtype=r_main&langx=zh-cn&mtype=3&page_no=0&league_id=&hot_game=&isie11=%27N%27";
                string html = gethtml(url);
               
                MatchCollection ahtmls = Regex.Matches(html, @"\(\['([\s\S]*?)\]\);");

               // toolStripProgressBar1.Maximum = ahtmls.Count-1;
                for (int j = 0; j < ahtmls.Count; j++)
                {
                    try
                    {
                        string[] values = ahtmls[j].Groups[1].Value.Split(new string[] { "," }, StringSplitOptions.None);

                        string aurl = "https://www.hga030.com/app/member/FT_order/FT_order_all.php?gid="+values[0]+"&uid=x3verwdxm22747894l300147&odd_f_type=H&type=H&gnum="+values[3]+"&strong=C&langx=zh-cn&ptype=&imp=N&rtype=RH&wtype=R";

                        string ahtml = gethtml(aurl);
                        Match  shishipeilv = Regex.Match(ahtml, @"redFatWord"">([\s\S]*?)<");
                       
                        if (shishipeilv.Groups[1].Value != values[9].Replace("'", "") && shishipeilv.Groups[1].Value+"0" != values[9].Replace("'", "") && shishipeilv.Groups[1].Value!="")
                        {
                            bofang();
                            ListViewItem lv1 = listView1.Items.Add(Regex.Replace(values[1].Replace("<br>", " "), "<[^>]+>", "").Replace("'", "").Replace("Running Ball", "")); //使用Listview展示数据   

                            lv1.SubItems.Add(Regex.Replace(values[2], "<[^>]+>", "").Replace("'", ""));


                            lv1.SubItems.Add(Regex.Replace(values[5], "<[^>]+>", "").Replace("'", ""));  //主队开始数据

                            lv1.SubItems.Add(values[9].Replace("'", ""));
                            lv1.SubItems.Add(values[14].Replace("'", ""));



                            ListViewItem lv2 = listView1.Items.Add("    "); //使用Listview展示数据 

                            lv2.SubItems.Add("   ");
                            lv2.SubItems.Add(Regex.Replace(values[6], "<[^>]+>", "").Replace("'", ""));  //客队开始数据
                            lv2.SubItems.Add(values[10].Replace("'", ""));
                            lv2.SubItems.Add(values[13].Replace("'", ""));

                        }
                    }
                    catch
                    {

                        continue;
                    }
                    //toolStripProgressBar1.Value = j;
                    //label8.Text = (j * 100 / toolStripProgressBar1.Maximum).ToString() + "%";//显示百分比

                }

              
            }


            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


        }

        #endregion
        string path = AppDomain.CurrentDomain.BaseDirectory;
        public void bofang()
        {
           
            System.Media.SoundPlayer sp = new SoundPlayer();
            sp.SoundLocation = @path + "resource/tixing.wav";
            sp.Play();
           
          

        }
        private void main_Load(object sender, EventArgs e)
        {
            button1.Image = Image.FromFile(@path + "resource/start.jpg");
        }



    
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            run();
        }


        private void button1_Click(object sender, EventArgs e)
        {

            #region 通用检测

            string html = gethtml("http://www.acaiji.com/index/index/vip.html");

            if (!html.Contains(@"baoluokanshui"))
            {

                MessageBox.Show("验证失败");
                return;


            }

            #endregion
            if (timer1.Enabled == false)
            {
                timer1.Start();
                button1.Image = null;
                button1.Text = "停止运行";
                button1.BackColor = Color.Red;
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            }

            else
            {
                button1.Text = "";
                button1.Image = Image.FromFile(@path + "resource/start.jpg");
               
                timer1.Stop();
            }
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            
           
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                Clipboard.SetDataObject(listView1.SelectedItems[0].SubItems[2].Text);
            }
        }
    }
}
