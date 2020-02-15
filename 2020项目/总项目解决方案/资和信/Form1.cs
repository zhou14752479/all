using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;
using tesseract;


namespace 资和信
{
    public partial class Form1 : Form
    {
        [DllImport("AspriseOCR.dll", EntryPoint = "OCR", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr OCR(string file, int type);
        [DllImport("AspriseOCR.dll", EntryPoint = "OCRpart", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr OCRpart(string file, int type, int startX, int startY, int width, int height);
        [DllImport("AspriseOCR.dll", EntryPoint = "OCRBarCodes", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr OCRBarCodes(string file, int type);
        [DllImport("AspriseOCR.dll", EntryPoint = "OCRpartBarCodes", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr OCRpartBarCodes(string file, int type, int startX, int startY, int width, int height);

      

        public Form1()
        {
            InitializeComponent();
            m_tesseract = new TesseractProcessor();
            m_tesseract.Init(m_path, m_lang, (int)TesseractEngineMode.DEFAULT);
            m_tesseract.SetVariable("tessedit_pageseg_mode", TesseractPageSegMode.PSM_AUTO.ToString());
        }


        public enum TesseractEngineMode : int
        {
            /// <summary>
            /// Run Tesseract only - fastest
            /// </summary>
            TESSERACT_ONLY = 0,

            /// <summary>
            /// Run Cube only - better accuracy, but slower
            /// </summary>
            CUBE_ONLY = 1,

            /// <summary>
            /// Run both and combine results - best accuracy
            /// </summary>
            TESSERACT_CUBE_COMBINED = 2,

            /// <summary>
            /// Specify this mode when calling init_*(),
            /// to indicate that any of the above modes
            /// should be automatically inferred from the
            /// variables in the language-specific config,
            /// command-line configs, or if not specified
            /// in any of the above should be set to the
            /// default OEM_TESSERACT_ONLY.
            /// </summary>
            DEFAULT = 3
        }


        public enum TesseractPageSegMode : int
        {
            /// <summary>
            /// Fully automatic page segmentation
            /// </summary>
            PSM_AUTO = 0,

            /// <summary>
            /// Assume a single column of text of variable sizes
            /// </summary>
            PSM_SINGLE_COLUMN = 1,

            /// <summary>
            /// Assume a single uniform block of text (Default)
            /// </summary>
            PSM_SINGLE_BLOCK = 2,

            /// <summary>
            /// Treat the image as a single text line
            /// </summary>
            PSM_SINGLE_LINE = 3,

            /// <summary>
            /// Treat the image as a single word
            /// </summary>
            PSM_SINGLE_WORD = 4,

            /// <summary>
            /// Treat the image as a single character
            /// </summary>
            PSM_SINGLE_CHAR = 5
        }


        private TesseractProcessor m_tesseract = null;
        private string m_path = Application.StartupPath + @"\tessdata\";
        private string m_lang = "eng";

        private string Ocr(Image image)
        {
            m_tesseract.Clear();
            m_tesseract.ClearAdaptiveClassifier();
            return m_tesseract.Apply(image);
        }
        public string imgdo(Bitmap img)
        {
            //去色
            Bitmap btp = img;
            Color c = new Color();
            int rr, gg, bb;
            for (int i = 0; i < btp.Width; i++)
            {
                for (int j = 0; j < btp.Height; j++)
                {
                    //取图片当前的像素点
                    c = btp.GetPixel(i, j);
                    rr = c.R; gg = c.G; bb = c.B;
                    //改变颜色
                    if (rr == 102 && gg == 0 && bb == 0)
                    {
                        //重新设置当前的像素点
                        btp.SetPixel(i, j, Color.FromArgb(255, 255, 255, 255));
                    }
                    if (rr == 153 && gg == 0 && bb == 0)
                    {
                        //重新设置当前的像素点
                        btp.SetPixel(i, j, Color.FromArgb(255, 255, 255, 255));
                    }
                    if (rr == 153 && gg == 0 && bb == 51)
                    {
                        //重新设置当前的像素点
                        btp.SetPixel(i, j, Color.FromArgb(255, 255, 255, 255));
                    }
                    if (rr == 153 && gg == 43 && bb == 51)
                    {
                        //重新设置当前的像素点
                        btp.SetPixel(i, j, Color.FromArgb(255, 255, 255, 255));
                    }
                    if (rr == 255 && gg == 255 && bb == 0)
                    {
                        //重新设置当前的像素点
                        btp.SetPixel(i, j, Color.FromArgb(255, 255, 255, 255));
                    }
                    if (rr == 255 && gg == 255 && bb == 51)
                    {
                        //重新设置当前的像素点
                        btp.SetPixel(i, j, Color.FromArgb(255, 255, 255, 255));
                    }
                }
            }
            // btp.Save("d:\\去除相关颜色.png");

            // pictureBox2.Image = Image.FromFile("d:\\去除相关颜色.png");


            //灰度
            Bitmap bmphd = btp;
            for (int i = 0; i < bmphd.Width; i++)
            {
                for (int j = 0; j < bmphd.Height; j++)
                {
                    //取图片当前的像素点
                    var color = bmphd.GetPixel(i, j);

                    var gray = (int)(color.R * 0.001 + color.G * 0.700 + color.B * 0.250);

                    //重新设置当前的像素点
                    bmphd.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            }
            //  bmphd.Save("d:\\灰度.png");
            // pictureBox27.Image = Image.FromFile("d:\\灰度.png");


            //二值化
            Bitmap erzhi = bmphd;
            Bitmap orcbmp;
            int nn = 3;
            int w = erzhi.Width;
            int h = erzhi.Height;
            BitmapData data = erzhi.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* p = (byte*)data.Scan0;
                byte[,] vSource = new byte[w, h];
                int offset = data.Stride - w * nn;

                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        vSource[x, y] = (byte)(((int)p[0] + (int)p[1] + (int)p[2]) / 3);
                        p += nn;
                    }
                    p += offset;
                }
                erzhi.UnlockBits(data);

                Bitmap bmpDest = new Bitmap(w, h, PixelFormat.Format24bppRgb);
                BitmapData dataDest = bmpDest.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                p = (byte*)dataDest.Scan0;
                offset = dataDest.Stride - w * nn;
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        p[0] = p[1] = p[2] = (int)vSource[x, y] > 161 ? (byte)255 : (byte)0;
                        //p[0] = p[1] = p[2] = (int)GetAverageColor(vSource, x, y, w, h) > 50 ? (byte)255 : (byte)0;
                        p += nn;

                    }
                    p += offset;
                }
                bmpDest.UnlockBits(dataDest);

                orcbmp = bmpDest;
                //  orcbmp.Save("d:\\二值化.png");
                //pictureBox29.Image = Image.FromFile("d:\\二值化.png");
            }

            //OCR的值
            if (orcbmp != null)
            {
                string result = Ocr(orcbmp);
               return result.Replace("\n", "\r\n").Replace(" ", "");
            }

            return "";

        }





        #region 平均分割图片
        /// <summary>
        /// 平均分割图片
        /// </summary>
        /// <param name="RowNum">水平上分割数</param>
        /// <param name="ColNum">垂直上分割数</param>
        /// <returns>分割好的图片数组</returns>
        public Bitmap[] GetSplitPics(Bitmap bmpobj, int RowNum, int ColNum)
        {


            for (int i = 0; i < bmpobj.Height; i++)
            {
                for (int j = 0; j < bmpobj.Width; j++)
                {
                    if (i < 1 || j < 1 || j > bmpobj.Width - 1 - 1 || i > bmpobj.Height - 1 - 1)
                        bmpobj.SetPixel(j, i, Color.FromArgb(255, 255, 255));
                }
            }

            if (RowNum == 0 || ColNum == 0)
                return null;
            int singW = bmpobj.Width / RowNum - 1;
            int singH = bmpobj.Height / ColNum;
            Bitmap[] PicArray = new Bitmap[RowNum * ColNum];

            Rectangle cloneRect;
            for (int i = 0; i < ColNum; i++)      //找有效区
            {
                for (int j = 0; j < RowNum; j++)
                {
                    cloneRect = new Rectangle(j * singW, i * singH, singW, singH);
                    PicArray[i * RowNum + j] = bmpobj.Clone(cloneRect, bmpobj.PixelFormat);//复制小块图
                }
            }
       


            return PicArray;
        }

        #endregion





















        public string COOKIE = "";
     


        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public  string PostUrl(string url, string postData)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Post";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postData.Length;

            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.112 Safari/537.36";
            request.Headers.Add("Cookie", COOKIE);

            StreamWriter sw = new StreamWriter(request.GetRequestStream());
            sw.Write(postData);
            sw.Flush();


            WebResponse response = request.GetResponse();
            Stream s = response.GetResponseStream();
            StreamReader sr = new StreamReader(s, Encoding.GetEncoding("GBK"));
            string html = sr.ReadToEnd();

            sw.Dispose();
            sw.Close();
            sr.Dispose();
            sr.Close();
            s.Dispose();
            s.Close();
            return html;
        }

        #endregion
        #region GET请求
        public  string getUrl(string url)
        {

            StreamReader reader = new StreamReader(getStream(url), Encoding.GetEncoding("GBK")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

            string content = reader.ReadToEnd();
            return content;

        }
        #endregion

        
        #region 获取数据流
        public Stream getStream(string Url)
        {

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
            request.Timeout = 10000;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
            request.AllowAutoRedirect = true;
            request.Headers.Add("Cookie", COOKIE);
            
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
            return response.GetResponseStream();

        }
        #endregion


        

        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
            StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.Default);
            string text = streamReader.ReadToEnd();
            string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != "")
                {
                  
                    string JINE = "";
                    string DATE = "";
                    int a = 0;
                    while (JINE == "")
                    {
                        Image image = Image.FromStream(getStream("https://www.zihexin.net/Verifycode2.do"));

                        //通过超人打码识别
                        //OCR ocr = new OCR();
                        //string value = ocr.Shibie("zhou14752479", "zhoukaige00", image);  //通过超人打码识别

                        //通过C#代码识别
                        //pictureBox1.Image = image;
                        Bitmap bmp = new Bitmap(image);

                        string value = imgdo(bmp);

                        string html = getUrl("https://www.zihexin.net/client/card/inquiry.do?key=&index=index&card_no=" + array[i] + "&verify_code=" + value);


                        Match key = Regex.Match(html, @"key"" value=""([\s\S]*?)""");


                        string strhtml = getUrl("https://www.zihexin.net/cardsearch/card/cardCheck.do?card_no=" + array[i] + "&key=" + key.Groups[1].Value);
                        Match jine = Regex.Match(strhtml, @"余额:</h3>([\s\S]*?)</dl>");
                        Match date = Regex.Match(strhtml, @"有效期:</h3>([\s\S]*?)</dl>");

                        JINE = jine.Groups[1].Value.Replace("<dl>", "").Replace("&nbsp;", "").Trim();
                        DATE = date.Groups[1].Value.Replace("<dl>", "").Replace("&nbsp;", "").Trim();
                        label1.Text = array[i] + "：正在第" + a + "次识别.....";
                            a++;

                        if (a > 20)
                            break;
                    }
                    label1.Text = array[i] + "：识别成功";
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(array[i]);
                    lv1.SubItems.Add(JINE);
                    lv1.SubItems.Add(DATE);
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }


                }




            }
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string pathname = AppDomain.CurrentDomain.BaseDirectory + ts.TotalSeconds.ToString() + ".xlsx";
            method.DataTableToExcelTime(method.listViewToDataTable(this.listView1), true, pathname);
        }


    
       


        private void Form1_Load(object sender, EventArgs e)
        {
            this.COOKIE = method.getUrlCookie("https://www.zihexin.net/Verifycode2.do");
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请导入卡号");
                return;
            }
         
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
        bool zanting = true;
        private void button1_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }
    }
}
