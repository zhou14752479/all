using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;



namespace 淘宝商品sku
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static string COOKIE= "t=792ea994957bef8e4a71539f91876594; thw=cn; UM_distinctid=16bde9c6ccb7f7-0183a6c7f99aa8-f353163-1fa400-16bde9c6cccb79; enc=BJiGDZ0SETmb%2BZ1Af%2FLOxZ7Ow%2Fz8B4xQY%2F3CPHkFybDesLHC8XJXgbKIOBMMGVwHTtQxN1Uu1ZSlm%2FWpfRRSTw%3D%3D; ali_ab=49.94.92.171.1563332665663.4; cna=8QJMFUu4DhACATFZv2JYDtwd; hng=CN%7Czh-CN%7CCNY%7C156; uc3=nk2=GcOvCmiKUSBXqZNU&lg2=VT5L2FSpMGV7TQ%3D%3D&vt3=F8dByuK6VGM%2Fj%2BDcu48%3D&id2=UoH62EAv27BqSg%3D%3D; lgc=zkg852266010; uc4=id4=0%40UOnlZ%2FcoxCrIUsehKG1MJKGz9FxR&nk4=0%40GwrkntVPltPB9cR46GnfFERBj601fAU%3D; tracknick=zkg852266010; _cc_=VT5L2FSpdA%3D%3D; tg=0; _m_h5_tk=de749cd01b256ca028e16cae6b69e992_1569225008906; _m_h5_tk_enc=0ed81481b2c8037778aa64c58ca4ec9f; mt=ci=43_1; x=e%3D1%26p%3D*%26s%3D0%26c%3D0%26f%3D0%26g%3D0%26t%3D0; v=0; cookie2=178c99ac00fb0b45511462530bff0247; _tb_token_=5b73e85b3157e; uc1=cookie14=UoTaEcJ15acvYQ%3D%3D; l=cBL47CXIqHssfe2WKOCMquIRGZ7OSIRAguPRwVXXi_5Q56L6ER7Ok1zaFFp6VjWdTgYB40tUd_29-etkmrHG2X2dh-0l.; isg=BGdnSBdP9vR1KHKf4pSmDSPm9pvxRDOgzQuFNjnUg_YdKIfqQbzLHqUuSmgTwBNG";
        bool zanting = true;

        #region 获取店铺内宝贝
        public void run1()
        {

            try
            {
                for (int i = 1; i < 999; i++)
                {


                    Match shopid = Regex.Match(textBox1.Text, @"https:\/\/([\s\S]*?)\.");
                    string Url = "https://" + shopid.Groups[1].Value + ".taobao.com/category.htm";
                    string html = method.gethtml(Url, COOKIE);

                    Match title = Regex.Match(html, @"<span class=""shop-name-title"" title=""([\s\S]*?)""");
                    Match midid = Regex.Match(html, @"mid=w-([\s\S]*?)-");

                    string curl = "https://" + shopid.Groups[1].Value + ".taobao.com/i/asynSearch.htm?_ksTS=1561163488418_160&callback=jsonp161&mid=w-" + midid.Groups[1].Value + "-0&wid=" + midid.Groups[1].Value + "&path=/search.htm&search=y&orderType=newOn_desc&viewType=grid&keyword=null&lowPrice=null&highPrice=null&pageNo=" + i;


                    MatchCollection names = Regex.Matches(method.gethtml(curl,COOKIE), @"<img alt=\\""([\s\S]*?)\\");
                    MatchCollection uids = Regex.Matches(method.gethtml(curl, COOKIE), @"data-id=\\""([\s\S]*?)\\");
                   
                    if (method.gethtml(curl,COOKIE).Contains("很抱歉"))
                        break;

                    for (int j = 0; j < names.Count; j++)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据    
                        lv1.SubItems.Add(names[j].Groups[1].Value);
                        lv1.SubItems.Add("https://item.taobao.com/item.htm?id=" + uids[j].Groups[1].Value);
                    }




                    if (listView1.Items.Count > 2)
                    {
                        listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                    }
                    Thread.Sleep(2000);

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }



                }



                
            }

            catch (System.Exception ex)
            {

                ex.ToString();
            }

        }

        #endregion
        

        #region 获取SKU价格
        public void run(string url)
        {

            try
            {

                    string html = method.gethtml(url, COOKIE);
               
                    MatchCollection  names= Regex.Matches(html, @"<a href=""javascript:void\(0\);"">([\s\S]*?)</span>");
                MatchCollection prices = Regex.Matches(html, @"""price"":""([\s\S]*?)""");



                for (int i = 0; i < names.Count; i++)
                {
                    ListViewItem lv1 = listView2.Items.Add((names[i].Groups[1].Value.Replace("<span>", "").Trim())); //使用Listview展示数据    
                    lv1.SubItems.Add(prices[i].Groups[1].Value);
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

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run1));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

      

        private void Label3_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
                this.Close(); //点确定的代码
            }
            else
            { //点取消的代码 
                return;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            method.ListviewToTxt(listView1);
            listView1.Items.Clear();
        }
        private Point mPoint = new Point();
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
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

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            else
            {
                string url = listView1.SelectedItems[0].SubItems[2].Text;
                MessageBox.Show(listView1.SelectedItems[0].SubItems[2].Text);
                run(url);
            }
           
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length-1; i++)
                {
                    
                    string[] values = text[i].Split(new string[] {"-----" }, StringSplitOptions.None);
                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                   lv1.SubItems.Add(values[0]);
                    lv1.SubItems.Add(values[1]);


                }
            }
        }
    }
}
