using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace 文件二维码
{
    public partial class 二维码识别 : Form
    {
        public 二维码识别()
        {
            InitializeComponent();
        }

        #region 识别图中二维码 Bitmap格式图片识别无二维码为空
        /// <summary>
        /// 识别图中二维码 Bitmap格式图片识别无二维码为空
        /// </summary>
        /// <param name="barcodeBitmap"></param>
        /// <returns></returns>
        public static string DecodeQrCode(Bitmap barcodeBitmap)
        {
            BarcodeReader reader = new BarcodeReader();
            reader.Options.CharacterSet = "UTF-8";
            var result = reader.Decode(barcodeBitmap);
            return (result == null) ? null : result.Text;
        }

        #endregion

       
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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                //headers.Add("sec-fetch-site:same-origin");
                //headers.Add("sec-fetch-user:?1");
                //headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                // request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
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


        private void btnFullScreen_Click(object sender, EventArgs e)
        {
            
        }

        private void SaveImage(Image image)
        {
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                string fileName = saveFileDialog1.FileName;
                string extension = Path.GetExtension(fileName);
                if (extension == ".jpg")
                {
                    image.Save(fileName, ImageFormat.Jpeg);
                }
                else
                {
                    image.Save(fileName, ImageFormat.Bmp);
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            
            //创建图象，保存将来截取的图象
            Bitmap image = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics imgGraphics = Graphics.FromImage(image);
            //设置截屏区域 柯乐义
            imgGraphics.CopyFromScreen(0, 0, 0, 0, new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));
            //保存
            SaveImage(image);
            listView1.Items.Clear();
            //Bitmap bmp = new Bitmap();
           // string url = DecodeQrCode(bmp);
           // System.Diagnostics.Process.Start(url);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in ofd.FileNames)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(file);
                    lv1.SubItems.Add(" ");
                }
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                二维码读取 er = new 二维码读取();
                二维码读取.filepath = listView1.SelectedItems[0].SubItems[1].Text;
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    er.lists.Add(listView1.Items[i].SubItems[1].Text);
                }
                er.Show();
            }
        }

        private void 二维码识别_Load(object sender, EventArgs e)
        {
            
        }
    }
}
