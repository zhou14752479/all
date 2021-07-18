using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using myDLL;
using System.Threading;

namespace 京东付款二维码生成
{
    public partial class 京东付款二维码生成 : Form
    {
        public 京东付款二维码生成()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <returns></returns>

        public Bitmap creatQcode(string path)
        {
            try
            {
                int width = 232; //图片宽度
                int height = 232;//图片长度
                BarcodeWriter barCodeWriter = new BarcodeWriter();
                barCodeWriter.Format = BarcodeFormat.QR_CODE; // 生成码的方式(这里设置的是二维码),有条形码\二维码\还有中间嵌入图片的二维码等
                barCodeWriter.Options.Hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");// 支持中文字符串
                barCodeWriter.Options.Hints.Add(EncodeHintType.ERROR_CORRECTION, ZXing.QrCode.Internal.ErrorCorrectionLevel.H);
                barCodeWriter.Options.Height = height;
                barCodeWriter.Options.Width = width;
                barCodeWriter.Options.Margin = 0; //设置的白边大小
                ZXing.Common.BitMatrix bm = barCodeWriter.Encode(path);  //DNS为要生成的二维码字符串
                Bitmap result = barCodeWriter.Write(bm);
                Bitmap Qcbmp = result.Clone(new Rectangle(Point.Empty, result.Size), PixelFormat.Format1bppIndexed);//位深度
                                                                                                                    // SaveImg(currentPath, Qcbmp); //图片存储自己写的函数
                                                                                                                    //Qcbmp=WhiteUp(Qcbmp,10);
                return Qcbmp;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                return null;

            }
        }

        string cookie = "";
        string localpath = AppDomain.CurrentDomain.BaseDirectory;

        public void save()
        {
            if (textBox2.Text != "")
            {
                localpath = textBox2.Text;
            }


            cookie = method.GetCookies("https://m.jd.com/");
            try
            {
                string[] urls= textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (var url in urls)
                {
                    //string orderid = Regex.Match(url, @"orderid=([\s\S]*?)&").Groups[1].Value;
                    //if (orderid != "")
                    //{
                    //    string aurl = "https://wq.jd.com/jdpaygw/jdappmpay?dealId="+ orderid;
                    //    string html = method.GetUrlWithCookie(aurl, cookie, "utf-8");
                    //    string payid = Regex.Match(html, @"payId=([\s\S]*?)&").Groups[1].Value;
                    //    if (payid == "")
                    //    {
                    //        payid = Regex.Match(html, @"payId=([\s\S]*?)app").Groups[1].Value;
                    //    }
                    //    if (payid != "")
                    //    {
                    //        string erweimaUrl = "https://pay.m.jd.com/pay/other-pay.html?appId=jd_iphone_app4&payId="+payid ;

                    //        Bitmap bitmap = creatQcode(erweimaUrl);
                    //        string path = localpath + "//"+orderid + ".jpg";
                    //        bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);

                    //        label2.Text =orderid+ "：生成二维码成功！";
                    //    }
                    //    else
                    //    {
                    //        label2.Text = html;

                    //    }
                    //}
                    //else
                    //{
                    //    label2.Text = "订单号识别为空";

                    //}
                    Bitmap bitmap = creatQcode(url);
                    string path = localpath + "//" + DateTime.Now.ToString("HHMMSS") + ".jpg";
                    bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);

                }


                MessageBox.Show("完成");

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }


        private void 京东付款二维码生成_Load(object sender, EventArgs e)
        {
            method.SetFeatures(11000);
            //webBrowser1.ScriptErrorsSuppressed = true;

            //webBrowser1.Navigate("https://plogin.m.jd.com/login/login");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择所在文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }

                textBox2.Text = dialog.SelectedPath;
            }
        }

        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com:8080/api/vip.html", "utf-8");

            if (!html.Contains(@"XTAeic"))
            {

                return;
            }



            #endregion
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(save);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
}
