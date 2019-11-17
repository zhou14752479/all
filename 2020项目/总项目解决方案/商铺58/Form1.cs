using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 商铺58
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string city = "bj";
        string cityName = "北京";

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
                string COOKIE = "id58=Ch8BCFzCmzIkdB4WK5XDAg==; 58tj_uuid=a741f941-0d76-4ad4-aca7-5009322f93ed; _ga=GA1.2.339422915.1556257586; als=0; wmda_uuid=70c7fd7fd7b308b2e6a6206def584118; gr_user_id=18275427-2ddc-4696-8411-a40f183ffda9; xxzl_deviceid=bstJ2gEkRsCFq7pCuBr4SAESHKWXUAyqCxRFcdNKImCFNgUsbEWr9c0wqpe3Lbdy; xxzl_smartid=ec64668edf0cea7ecc1cc570a6df6ee4; Hm_lvt_5a7a7bfd6e7dfd9438b9023d5a6a4a96=1559876130; cookieuid1=e87rjVz52Aq4OEPbBQziAg==; mcity=suqian; mcityName=%E5%AE%BF%E8%BF%81; nearCity=%5B%7B%22cityName%22%3A%22%E4%B8%8A%E6%B5%B7%22%2C%22city%22%3A%22sh%22%7D%2C%7B%22cityName%22%3A%22%E5%AE%BF%E8%BF%81%22%2C%22city%22%3A%22suqian%22%7D%5D; __utma=253535702.339422915.1556257586.1564796955.1564796955.1; __utmz=253535702.1564796955.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); Hm_lvt_295da9254bbc2518107d846e1641908e=1564883078; Hm_lvt_e2d6b2d0ec536275bb1e37b421085803=1565490373; city=suqian; 58home=suqian; wmda_visited_projects=%3B6333604277682%3B1409632296065%3B1732038237441%3B1731916484865%3B2385390625025%3B1732030748417%3B1444510081921%3B6289197098934; __xsptplus8=8.1.1569640714.1569640719.2%234%7C%7C%7C%7C%7C%23%23v9ujfcpY5FwFv_20ewcr1QcMNVIf0l9x%23; Hm_lvt_3bb04d7a4ca3846dcc66a99c3e861511=1569641042; Hm_lvt_e15962162366a86a6229038443847be7=1569641042; xxzl_sid=\"6BxAzb-Sze-LZI-PcD-1aBGNWEeQ\"; xxzl_token=\"OjUrnPxK9PpKSH + MmdW9cafl6rKCJfN0XQsJprWodFu3t4845j386itwD8y0dYuMin35brBb//eSODvMgkQULA==\"; new_uv=30; utm_source=; spm=; init_refer=; new_session=0; xxzl_cid=6924f4ad6a79459fa6729679775b5c15; xzuid=f4c51223-1774-4118-ab41-2a45f9d60044; ppStore_fingerprint=4EB12C202E83C6B4F5E0862F611827369C8C45E085D33781%EF%BC%BF1573896390005";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
           
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
               MessageBox.Show( ex.ToString());

            }
            return "";
        }
        #endregion


        #region  生意转让、商铺出租、商铺出售
        public void shangpu(object item)
        {
           

            try
            {

               

                if (city == "")
                {
                    MessageBox.Show("请选择城市！");
                    return;
                }

                for (int i = 1; i <= 70; i++)
                {
                    String Url = "https://" + city + ".58.com/" + item.ToString() + "/0/pn" + i + "/";
                
                    string html = GetUrl(Url);

                    MatchCollection TitleMatchs = Regex.Matches(html, @"https://[a-z]+.58.com/[a-z]+/[0-9]+x.shtml", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                    ArrayList lists = new ArrayList();

                    foreach (Match NextMatch in TitleMatchs)
                    {
                        if (!lists.Contains(NextMatch.Groups[0].Value))
                        {
                            lists.Add(NextMatch.Groups[0].Value);
                        }
                    }
               

                    foreach (string list in lists)
                    {
                        string tm1 = DateTime.Now.ToString();  //获取系统时间
                        toolStripStatusLabel1.Text = tm1 + "正在采集：" + cityName + list;
                        Match uid = Regex.Match(list, @"\d{10,}");
                      
                        string strhtml = GetUrl("https://miniappfang.58.com/shop/plugin/v1/shopdetail?infoId="+uid.Groups[0].Value+"&openId=77AA769A2A2C8740ECF1EDB47CD855A04C573D57DAF470CD8AD018A504661F6A");  //定义的GetRul方法 返回 reader.ReadToEnd()
                        
                        Match title = Regex.Match(strhtml, @"""title"":""([\s\S]*?)""");
                        Match contacts = Regex.Match(strhtml, @"""brokerName"":""([\s\S]*?)""");
                        Match tel = Regex.Match(strhtml, @"""phone"":""([\s\S]*?)""");
                        Match region = Regex.Match(strhtml, @"""quyu"":""([\s\S]*?)""");
                        Match dizhi = Regex.Match(strhtml, @"""dizhi"":""([\s\S]*?)""");

                        Match date= Regex.Match(strhtml, @"""postDate"":""([\s\S]*?)""");
                        Match description = Regex.Match(strhtml, @"""description"":""([\s\S]*?)""");

                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        listViewItem.SubItems.Add(title.Groups[1].Value);
                        listViewItem.SubItems.Add(contacts.Groups[1].Value);
                        listViewItem.SubItems.Add(tel.Groups[1].Value);
                        listViewItem.SubItems.Add(region.Groups[1].Value);
                        listViewItem.SubItems.Add(dizhi.Groups[1].Value);
                        listViewItem.SubItems.Add(date.Groups[1].Value);
                        listViewItem.SubItems.Add(description.Groups[1].Value);



                        Application.DoEvents();
                        Thread.Sleep(1000);   //内容获取间隔，可变量

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                    }
                   toolStripStatusLabel1.Text = "抓取完成";

                }




            }

            catch (System.Exception ex)
            {

               MessageBox.Show( ex.ToString());
            }


        }

        bool zanting = true;

        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            skinTreeView1.Visible = false;
            button1.Enabled = false;

            if (radioButton1.Checked == true)
            {

                Thread thread = new Thread(new ParameterizedThreadStart(shangpu));
                string o = "shangpucz";
                thread.Start((object)o);
                Control.CheckForIllegalCrossThreadCalls = false;
              

            }
            else if (radioButton3.Checked == true)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(shangpu));
                string o = "shangpucs";
                thread.Start((object)o);
                Control.CheckForIllegalCrossThreadCalls = false;

            }

            else if (radioButton2.Checked == true)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(shangpu));
                string o = "shengyizr";
                thread.Start((object)o);
                Control.CheckForIllegalCrossThreadCalls = false;
                //创建带参数的线程

            }
        }

        private void skinTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            city = e.Node.Name;
            cityName = e.Node.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (skinTreeView1.Visible == false)
            {
                skinTreeView1.Visible = true;
                button5.Text = "关闭城市选择";
                button5.ForeColor = Color.Blue;
            }

            else if (skinTreeView1.Visible == true)
            {
                skinTreeView1.Visible = false;
                button5.Text = "快速选择城市";
                button5.ForeColor = Color.White;
            }

        }
        private Point mPoint = new Point();
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
            }
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            SetWindowRegion();
        }
        public void SetWindowRegion()
        {
            System.Drawing.Drawing2D.GraphicsPath FormPath;
            FormPath = new System.Drawing.Drawing2D.GraphicsPath();
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            FormPath = GetRoundedRectPath(rect, 10);
            this.Region = new Region(FormPath);

        }
        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            int diameter = radius;
            Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));
            GraphicsPath path = new GraphicsPath();

            // 左上角
            path.AddArc(arcRect, 180, 90);

            // 右上角
            arcRect.X = rect.Right - diameter;
            path.AddArc(arcRect, 270, 90);

            // 右下角
            arcRect.Y = rect.Bottom - diameter;
            path.AddArc(arcRect, 0, 90);

            // 左下角
            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90, 90);
            path.CloseFigure();//闭合曲线
            return path;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {
                
            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized; //最小化
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.acaiji.com");
        }
    }
}
