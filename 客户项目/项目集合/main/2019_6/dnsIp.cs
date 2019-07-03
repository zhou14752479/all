using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
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

namespace main._2019_6
{
    public partial class dnsIp : Form
    {
        private void DnsIp_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        }

        bool status = true;
        bool zanting = true;
        
        public dnsIp()
        {
            InitializeComponent();
        }
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
                string COOKIE = "__dxca=2bf12bd4-691d-41d6-83c8-09cffad21c63; JSESSIONID=00B1E3FF849435183810FE59067F1C97.tomcat218; route=7f74d77075feb1edd5827d8afbb814a4; DSSTASH_LOG=C%5f35%2dUN%5f%2d1%2dUS%5f%2d1%2dT%5f1561165647190; userIPType_abo=1; userName_dsr=\"\"; enc_abo=A8A4B0A033E2E8462153267F85A75FB0; groupId=431; nopubuser_abo=0; groupenctype_abo=1; duxiu=userName%5fdsr%2c%3d0%2c%21userid%5fdsr%2c%3d%2d1%2c%21char%5fdsr%2c%3d%2c%21metaType%2c%3d0%2c%21logo%5fdsr%2c%3dareas%2fucdrs%2fimages%2flogo%2ejpg%2c%21logosmall%5fdsr%2c%3darea%2fucdrs%2flogosmall%2ejpg%2c%21title%5fdsr%2c%3d%u5168%u56fd%u56fe%u4e66%u9986%u53c2%u8003%u54a8%u8be2%u8054%u76df%2c%21url%5fdsr%2c%3d%2c%21compcode%5fdsr%2c%3d%2c%21province%5fdsr%2c%3d%2c%21isdomain%2c%3d0%2c%21showcol%2c%3d0%2c%21isfirst%2c%3d0%2c%21og%2c%3d0%2c%21ogvalue%2c%3d0%2c%21cdb%2c%3d0%2c%21userIPType%2c%3d1%2c%21lt%2c%3d0%2c%21enc%5fdsr%2c%3d99B6801E8A942DE1AB7E91EFAB8AE4CF; AID_dsr=689; userId_abo=%2d1; schoolid_abo=689; user_enc_abo=B5AB06A2BD20A0BC011FBFEE2B9757CB; idxdom=www%2eucdrs%2esuperlib%2enet; msign_dsr=1561165647141; UM_distinctid=16b7cb94c4c1d1-096e50efb17b03-e353165-1fa400-16b7cb94c4e43; CNZZDATA2088844=cnzz_eid%3D1687723352-1561160529-%26ntime%3D1561165463";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";

                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

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


        #region 获取状态码
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string Getstatuscode(string url)
        {
            string URL = "http://tool.chinaz.com/pagestatus/?url="+url;
            string html = GetUrl(URL, "utf-8");
                Match code= Regex.Match(html, @"返回状态码</div><div class=""fr zTContrig""><span>([\s\S]*?)</span>");
            return code.Groups[1].Value;
        }
        #endregion

        /// <summary>
        /// 旁站查询
        /// </summary>
        public void run()
        {
            progressBar1.Value = 0;//设置当前值
            progressBar1.Step = 1;//设置没次增长多少
            for (int i = 1; i <99; i++)
            {
                
          
                string URL = "https://dns.aizhan.com/" + textBox1.Text + "/" + i + "/";

                label7.Text = "正在获取第" + i+"页域名信息.....";
                string html = GetUrl(URL, "utf-8");

                MatchCollection  urls = Regex.Matches(html, @"rel=""nofollow"" target=""_blank"">([\s\S]*?)</a>");
                MatchCollection names = Regex.Matches(html, @"rel=""nofollow"" target=""_blank"">([\s\S]*?)<td class=""title"">([\s\S]*?)</td>");
                Match Ipaddress = Regex.Match(html, @"<strong class=""red"">([\s\S]*?)<");
                Match area = Regex.Match(html, @"<strong>([\s\S]*?)<");

                Match count = Regex.Match(html, @"共有 <span class=""red"">([\s\S]*?)</span>"); //总数
                label8.Text = Ipaddress.Groups[1].Value;
                label9.Text = area.Groups[1].Value;
                if (urls.Count == 0)
                    break;
                progressBar1.Maximum = Convert.ToInt32(count.Groups[1].Value);//设置最大值
                for (int j= 0; j< urls.Count ; j++)
                {

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count+1).ToString()); //使用Listview展示数据         
                    lv1.SubItems.Add(urls[j].Groups[1].Value);
                    lv1.SubItems.Add(names[j].Groups[2].Value.Replace("<span>","").Replace("</span>","").Trim());
                    lv1.SubItems.Add(Getstatuscode(urls[j].Groups[1].Value));
                    Thread.Sleep(200);

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    progressBar1.Value += progressBar1.Step; //让进度条增加一次
                }
            }
            //for (int a = 0; a < listView1.Items.Count; a++)
            //{
            //    listView1.Items[a].SubItems[3].Text = Getstatuscode(listView1.Items[a].SubItems[1].Text);
            //    Thread.Sleep(1000);
            //}
            Thread.Sleep((Convert.ToInt32(textBox2.Text)) * 1000);

            label7.Text = textBox1.Text+ "抓取结束";
            
        }

        public void run10()
        {
            progressBar1.Value = 0;//设置当前值
            progressBar1.Step = 1;//设置没次增长多少
            for (int i = 1; i < 99; i++)
            {


                string URL = "https://dns.aizhan.com/" + textBox1.Text + "/" + i + "/";

                label7.Text = "正在获取第" + i + "页域名信息.....";
                string html = GetUrl(URL, "utf-8");

                MatchCollection urls = Regex.Matches(html, @"rel=""nofollow"" target=""_blank"">([\s\S]*?)</a>");
                MatchCollection names = Regex.Matches(html, @"rel=""nofollow"" target=""_blank"">([\s\S]*?)<td class=""title"">([\s\S]*?)</td>");
                Match Ipaddress = Regex.Match(html, @"<strong class=""red"">([\s\S]*?)<");
                Match area = Regex.Match(html, @"<strong>([\s\S]*?)<");

                Match count = Regex.Match(html, @"共有 <span class=""red"">([\s\S]*?)</span>"); //总数
                label8.Text = Ipaddress.Groups[1].Value;
                label9.Text = area.Groups[1].Value;
                if (urls.Count == 0)
                    break;
                progressBar1.Maximum = Convert.ToInt32(count.Groups[1].Value);//设置最大值
                for (int j = 0; j < urls.Count; j++)
                {

                    ListViewItem lv1 = listView3.Items.Add((listView3.Items.Count + 1).ToString()); //使用Listview展示数据         
                    lv1.SubItems.Add(urls[j].Groups[1].Value);
                    lv1.SubItems.Add(names[j].Groups[2].Value.Replace("<span>", "").Replace("</span>", "").Trim());
                    lv1.SubItems.Add(Getstatuscode(urls[j].Groups[1].Value));
                    Thread.Sleep(200);

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    progressBar1.Value += progressBar1.Step; //让进度条增加一次
                }
            }
            //for (int a = 0; a < listView1.Items.Count; a++)
            //{
            //    listView1.Items[a].SubItems[3].Text = Getstatuscode(listView1.Items[a].SubItems[1].Text);
            //    Thread.Sleep(1000);
            //}

            Thread.Sleep((Convert.ToInt32(textBox2.Text)) * 1000);
            label7.Text = textBox1.Text + "抓取结束";

        }



        /// <summary>
        /// C段查询
        /// </summary>
        public void run1()
        {
            try
            {

                progressBar1.Value = 0;//设置当前值
                progressBar1.Step = 1;//设置没次增长多少
                progressBar1.Maximum =255;//设置最大值
                string[] duans = textBox1.Text.Split(new string[] { "." }, StringSplitOptions.None);
                for (int a = 1; a < 256; a++)
                {
                    ListViewItem lv2 = listView2.Items.Add(duans[0] + "." + duans[1] + "." + duans[2] + "." + a); //使用Listview展示数据         
                    progressBar1.Value += progressBar1.Step; //让进度条增加一次
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                }

                for (int i = 0; i < listView2.Items.Count; i++)
                {
                    textBox1.Text = listView2.Items[i].SubItems[0].Text;
                    run();
                    Thread.Sleep((Convert.ToInt32(textBox2.Text))*1000);
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                }
                MessageBox.Show("C段IP抓取完成");

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

         
        }

        /// <summary>
        /// CDN检测
        /// </summary>
        public void cdn()
        {
           
           
                string URL = "http://site.ip138.com/domain/read.do?domain="+textBox1.Text+"&time=1561961104268";

                string html = GetUrl(URL, "utf-8");

                MatchCollection ips = Regex.Matches(html, @"""ip"":""([\s\S]*?)""");

            if (ips.Count > 1)
            {
                MessageBox.Show("该域名为CDN");
            }
            else
                {
                MessageBox.Show("该域名不是CDN");
            }

        }


        private void Button1_Click(object sender, EventArgs e)
        {
            listView3.Visible = false;
            listView1.Visible = true;

            #region 通用验证

            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php", "utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == "2.2.2.2")
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {
                status = false;
                listView1.Items.Clear();
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            }
            else
            {
                MessageBox.Show("IP不符");

            }
            #endregion

          
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            listView3.Visible = false;
            listView1.Visible = true;
            #region 通用验证

            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php", "utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == "2.2.2.2")
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {
                status = true;
                listView1.Items.Clear();
                listView2.Items.Clear();
                Thread thread = new Thread(new ThreadStart(run1));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            }
            else
            {
                MessageBox.Show("IP不符");

            }
            #endregion
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            

            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
           
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Process.Start("IExplore.exe", this.listView1.SelectedItems[0].SubItems[1].Text);
        }

        private void listView2_MouseClick(object sender, MouseEventArgs e)
        {
            status = false;
            listView1.Visible = false;
            listView3.Visible = true;
            textBox1.Text = this.listView2.SelectedItems[0].SubItems[0].Text;
            Thread thread = new Thread(new ThreadStart(run10));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(cdn));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;        

        }

        private void ListView3_MouseClick(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Process.Start("IExplore.exe", this.listView3.SelectedItems[0].SubItems[1].Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            zanting = true;
        }
    }
}
