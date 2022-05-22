using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace 图片合成
{
    public partial class 图片合成 : Form
    {
        public 图片合成()
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
            string html = "";
            string COOKIE = "";
            try
            {
               
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }

                response.Close();
                return html;



            }
            catch (System.Exception ex)
            {
                return ex.ToString();

            }



        }
        #endregion

        /// <summary>
        /// 上下合成图片
        /// </summary>
        static private void CombinImage2()
        {
            Image img1 = Image.FromFile(Application.StartupPath + "/image/1.jpg");
            Bitmap map1 = new Bitmap(img1);
            Image img2 = Image.FromFile(Application.StartupPath + "/image/2.jpg");
            Bitmap map2 = new Bitmap(img2);

            var width = Math.Max(img1.Width, img2.Width);
            var height = img1.Height + img2.Height  + 10;
            // 初始化画布(最终的拼图画布)并设置宽高
            Bitmap bitMap = new Bitmap(width, height);
            // 初始化画板
            Graphics g1 = Graphics.FromImage(bitMap);
            // 将画布涂为白色(底部颜色可自行设置)
            g1.FillRectangle(Brushes.White, new Rectangle(0, 0, width, height));
            //在x=0，y=0处画上图一
            g1.DrawImage(map1, 0, 0, img1.Width, img1.Height);
            //在x=0，y在图一往下10像素处画上图二
            g1.DrawImage(map2, 0, img1.Height, img2.Width, img2.Height);
          
            map1.Dispose();
            map2.Dispose();
            img1.Dispose();
            img2.Dispose();

            Image img = bitMap;
            //保存
            img.Save(Path.Combine(Application.StartupPath + "/image/result.jpg"));
            img.Dispose();


        }


        /// <summary>
        /// 上下合成图片
        /// </summary>
        static private void CombinImage3()
        {
            Image img1 = Image.FromFile(Application.StartupPath+"/image/1.jpg");
            Bitmap map1 = new Bitmap(img1);
            Image img2 = Image.FromFile(Application.StartupPath + "/image/2.jpg");
            Bitmap map2 = new Bitmap(img2);
            Image img3 = Image.FromFile(Application.StartupPath + "/image/3.jpg");
            Bitmap map3 = new Bitmap(img3);
            var width = Math.Max(img1.Width, img2.Width);
            var height = img1.Height + img2.Height + img2.Height + 10;
            // 初始化画布(最终的拼图画布)并设置宽高
            Bitmap bitMap = new Bitmap(width, height);
            // 初始化画板
            Graphics g1 = Graphics.FromImage(bitMap);
            // 将画布涂为白色(底部颜色可自行设置)
            g1.FillRectangle(Brushes.White, new Rectangle(0, 0, width, height));
            //在x=0，y=0处画上图一
            g1.DrawImage(map1, 0, 0, img1.Width, img1.Height);
            //在x=0，y在图一往下10像素处画上图二
            g1.DrawImage(map2, 0, img1.Height, img2.Width, img2.Height);
            g1.DrawImage(map3, 0, img1.Height + img2.Height, img3.Width, img3.Height);
            map1.Dispose();
            map2.Dispose();
            map3.Dispose();
            img1.Dispose();
            img2.Dispose();
            img3.Dispose();
          

            Image img = bitMap;
            //保存
            img.Save(Path.Combine(Application.StartupPath + "/image/result.jpg"));
            img.Dispose();

         
        }


        /// <summary>
        /// 上下合成图片
        /// </summary>
        static private void CombinImage4()
        {
            Image img1 = Image.FromFile(Application.StartupPath + "/image/1.jpg");
            Bitmap map1 = new Bitmap(img1);
            Image img2 = Image.FromFile(Application.StartupPath + "/image/2.jpg");
            Bitmap map2 = new Bitmap(img2);
            Image img3 = Image.FromFile(Application.StartupPath + "/image/3.jpg");
            Bitmap map3 = new Bitmap(img3);

            Image img4 = Image.FromFile(Application.StartupPath + "/image/4.jpg");
            Bitmap map4 = new Bitmap(img4);


            var width = Math.Max(img1.Width, img2.Width);
            var height = img1.Height + img2.Height + img3.Height + img4.Height + 10;
            // 初始化画布(最终的拼图画布)并设置宽高
            Bitmap bitMap = new Bitmap(width, height);
            // 初始化画板
            Graphics g1 = Graphics.FromImage(bitMap);
            // 将画布涂为白色(底部颜色可自行设置)
            g1.FillRectangle(Brushes.White, new Rectangle(0, 0, width, height));
            //在x=0，y=0处画上图一
            g1.DrawImage(map1, 0, 0, img1.Width, img1.Height);
            //在x=0，y在图一往下10像素处画上图二
            g1.DrawImage(map2, 0, img1.Height, img2.Width, img2.Height);
            g1.DrawImage(map3, 0, img1.Height + img2.Height, img3.Width, img3.Height);
            g1.DrawImage(map4, 0, img1.Height + img2.Height+img3.Height, img4.Width, img4.Height);

            map1.Dispose();
            map2.Dispose();
            map3.Dispose();
            map4.Dispose();
            img1.Dispose();
            img2.Dispose();
            img3.Dispose();
            img4.Dispose();

            Image img = bitMap;
            //保存
            img.Save(Path.Combine(Application.StartupPath + "/image/result.jpg"));
            img.Dispose();


        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 读取本地图片
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Bitmap ReadImageFile(string path)
        {
            if (!File.Exists(path))
            {
                MessageBox.Show("图片路径错误");
                return null;//文件不存在
            }
            FileStream fs = File.OpenRead(path); //OpenRead
            int filelength = 0;
            filelength = (int)fs.Length; //获得文件长度 
            Byte[] image = new Byte[filelength]; //建立一个字节数组 
            fs.Read(image, 0, filelength); //按字节流读取 
            System.Drawing.Image result = System.Drawing.Image.FromStream(fs);
            fs.Close();
            fs.Dispose();
            Bitmap bit = new Bitmap(result);
            return bit;
        }
        private void 图片合成_Load(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"puM7"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion
            //pictureBox1.Image = Image.FromFile(Application.StartupPath+ "/main/宝石.jpg");
        }




        public void pic_hecheng(int w,int h, string path)
        {
           
            Image image = ReadImageFile(path);

            Bitmap  bitmap = new Bitmap(Application.StartupPath + "/main/宝石.jpg");
            Graphics device = Graphics.FromImage(bitmap);
            //如果picturebox1本身有内容，就先画到image上
            device.DrawImage(image, w, h); //用你想要的位置画picturebox2
            pictureBox1.Image = bitmap;
           
        }


        /// <summary>
        /// 指定位置添加图片
        /// </summary>
        /// <param name="changci"></param>
        /// <param name="value"></param>
        public void run(string changci,string value)
        {


            try
            {
                string month = DateTime.Now.Month.ToString();
            
                string day = DateTime.Now.Day.ToString();
              if(Convert.ToInt32(month) <10)
                {
                    month = "0" + month;
                }
                if (Convert.ToInt32(day) < 10)
                {
                    day = "0" + day;
                }
                char[] monthchar = month.ToCharArray();
                
                char[] daychar = day.ToCharArray();



                Image image_changci = ReadImageFile(Application.StartupPath + "/场次/"+changci+".png");



                Image image_month = ReadImageFile(Application.StartupPath + "/date/" + monthchar[0] + ".png");
                Image image_month2 = ReadImageFile(Application.StartupPath + "/date/" + monthchar[1] + ".png");
                Image image_yue = ReadImageFile(Application.StartupPath + "/date/月.png");
                Image image_ri = ReadImageFile(Application.StartupPath + "/date/日.png");
                Image image_day = ReadImageFile(Application.StartupPath + "/date/" + daychar[0] + ".png");
                Image image_day2 = ReadImageFile(Application.StartupPath + "/date/" + daychar[1] + ".png");







                Bitmap bitmap = new Bitmap(Application.StartupPath + "/main/" + value + ".jpg");
                Graphics device = Graphics.FromImage(bitmap);
                //如果picturebox1本身有内容，就先画到image上
                device.DrawImage(image_changci, 383, 83); //用你想要的位置画小图


                device.DrawImage(image_month, 140, 65);
                device.DrawImage(image_month2, 155, 65);

                device.DrawImage(image_day, 195, 65);
                device.DrawImage(image_day2, 210, 65);



                device.DrawImage(image_yue, 95, 26);
                device.DrawImage(image_ri, 165, 26);

             
                bitmap.Save(Application.StartupPath + "/image/"+changci+".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

              
                image_month.Dispose();
                image_month2.Dispose();
                image_yue.Dispose();
                image_ri.Dispose();
                image_day.Dispose();
                image_day2.Dispose();
                image_changci.Dispose();
                bitmap.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
             
            }

        }








        /// <summary>
        /// 在现有画布上添加文字
        /// </summary>
        /// <param name="g">画布对象</param>
        /// <param name="txt">需要添加的文字</param>
        /// <param name="fontName">字体名称</param>
        /// <param name="fontSize">字体大小</param>
        /// <param name="fontStyle">字体风格，是否加粗等</param>
        /// <param name="x">绘画位置X</param>
        /// <param name="y">绘画位置Y</param>
        /// <param name="red">文字颜色R</param>
        /// <param name="green">文字颜色G</param>
        /// <param name="blue">文字颜色B</param>
        public void drawText(string txt, string fontName, int fontSize, FontStyle fontStyle, float x, float y, int red, int green, int blue)
        {
            Bitmap nbmp = new Bitmap(pictureBox1.Image);
            Graphics g = Graphics.FromImage(nbmp);
            //文字绘制
            Font font = new Font(fontName, fontSize, fontStyle, GraphicsUnit.Pixel);
            Brush brush = new SolidBrush(Color.FromArgb(red, green, blue));
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.DrawString(txt, font, brush, x, y, StringFormat.GenericDefault);
            pictureBox1.Image = nbmp;
          
        }



        private void button1_Click(object sender, EventArgs e)
        {
            //drawText("你好","黑体",30,FontStyle.Bold,100,100,0,0,0);
            pictureBox1.Image.Dispose();
            run("1", textBox1.Text.Trim());
           pictureBox1.Image = Image.FromFile(Application.StartupPath + "/image/1.jpg");

        }

        private void button2_Click(object sender, EventArgs e)
        {
           

            pictureBox1.Image.Dispose();
            try
            {
                run("1", textBox1.Text.Trim());

                run("2", textBox2.Text.Trim());



                if (File.Exists(Application.StartupPath + "/image/1.jpg") && File.Exists(Application.StartupPath + "/image/2.jpg"))
                {
                    CombinImage2();
                    pictureBox1.Image = Image.FromFile(Application.StartupPath + "/image/result.jpg");
                }
                else
                {
                    MessageBox.Show("图片不存在");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image.Dispose();
            try
            {
                run("1", textBox1.Text.Trim());

                run("2", textBox2.Text.Trim());

                run("3", textBox3.Text.Trim());


                if (File.Exists(Application.StartupPath + "/image/1.jpg") && File.Exists(Application.StartupPath + "/image/2.jpg") && File.Exists(Application.StartupPath + "/image/3.jpg"))
                {
                    CombinImage3();
                    pictureBox1.Image = Image.FromFile(Application.StartupPath + "/image/result.jpg");
                }
                else
                {
                    MessageBox.Show("图片不存在");
                }
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pictureBox1.Image.Dispose();
            try
            {
                run("1", textBox1.Text.Trim());

                run("2", textBox2.Text.Trim());

                run("3", textBox3.Text.Trim());

                run("4", textBox4.Text.Trim());
                if (File.Exists(Application.StartupPath + "/image/1.jpg") && File.Exists(Application.StartupPath + "/image/2.jpg") && File.Exists(Application.StartupPath + "/image/3.jpg") && File.Exists(Application.StartupPath + "/image/4.jpg"))
                {
                    CombinImage4();
                    pictureBox1.Image = Image.FromFile(Application.StartupPath + "/image/result.jpg");
                }
                else
                {
                    MessageBox.Show("图片不存在");
                }
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.ToString());
            }

        }







    }
}
