using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using 类库;


namespace 淘宝商品sku
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static string COOKIE= "t=db807e67e00d0d1d34419cc5686c31bc; ali_ab=121.234.247.249.1523710505053.9; thw=cn; UM_distinctid=16a5312a75437a-0215e3ab7f0a66-5701631-1fa400-16a5312a755cf6; _m_h5_tk=62b87b345583a58f8366530424905db1_1564200453485; _m_h5_tk_enc=c33e74e65e58f6c35aca17b2c80f04dc; hng=CN%7Czh-CN%7CCNY%7C156; cna=/h4DE4EwWXQCATFGW6Fbj0Jl; uc3=nk2=GcOvCmiKUSBXqZNU&lg2=URm48syIIVrSKA%3D%3D&vt3=F8dBy3za1eXFL98xsVg%3D&id2=UoH62EAv27BqSg%3D%3D; lgc=zkg852266010; uc4=nk4=0%40GwrkntVPltPB9cR46GnfFEki3ZzDLyo%3D&id4=0%40UOnlZ%2FcoxCrIUsehKG1BAZBUjAb5; tracknick=zkg852266010; _cc_=WqG3DMC9EA%3D%3D; tg=0; mt=ci=43_1; enc=1CYl3mYliNWuy%2BDPLhsGnkuO2MysZCOo47GQOZTf%2Bw%2FKvqZQOQrFvw%2FF%2F95bBCZDRWDz0Ush3IC5mw6Pix0XKw%3D%3D; x5sec=7b2273686f7073797374656d3b32223a22316533646536306363323638373162646535623638343238366464613336393743492f6937756b464550726c704a6e55696554344a526f4d4d5441314d6a4d304e7a55304f44737a227d; pnm_cku822=098%23E1hvXvvUvbpvUvCkvvvvvjiPRFFhtj3URLdW6j1VPmPwAjYPPszyAjn8RszZ1jnCvphvC9v9vvCvp8yCvv9vvUv0wxufJOyCvvOUvvVva68tvpvIvvvvvhCvvvvvvUnvphvh5Qvv96CvpC29vvm2phCvhhvvvUnvphvppvyCvhQhew6vC0Er6j7gRbIs7TmxfwpOdeDHD7zpaBwgnZ43IExrVTtiBXxr1WmK5eECkbmxfBuKf3qxs4V9%2Bul1pc7y%2BE7rejvrYExr68g72QhvCvvvMMGtvpvhvvvvv8wCvvpvvUmm; l=cBSQUU-qq_AdS-FhBOCZourza7797IRAguPzaNbMi_5aU6L6xv7OkVfNxFp6cjWdtEYB4k6UXwe9-etkiIhb5lnOEKyF.; isg=BCcnC5XLNgX0krKADCFt6mq9tlujn_uORXM4mfmUV7bd6EeqAX5u3-nuCqhTANMG";
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

                    string curl = "https://" + shopid.Groups[1].Value + ".taobao.com/i/asynSearch.htm?_ksTS=1561163488418_160&callback=jsonp161&mid=w-" + midid.Groups[1].Value + "-0&wid=" + midid.Groups[1].Value + "&path=/search.htm&search=y&orderType=newOn_desc&viewType=grid&keyword=null&lowPrice=null&highPrice=null&pageNo="+i;


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
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
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
    }
}
