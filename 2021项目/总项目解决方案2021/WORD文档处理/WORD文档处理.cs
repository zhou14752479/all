using Spire.Doc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WORD文档处理
{
    public partial class WORD文档处理 : Form
    {
        public WORD文档处理()
        {
            InitializeComponent();
        }

        public void docreplace()
        {
            try
            {
                var doc = new Document();
                doc.LoadFromFile(@"C:\Users\zhou\Desktop\B1常规固体加入.docx");

                doc.Replace("数据", "数据抓取", false, false);


                var guid = Guid.NewGuid().ToString();
                doc.SaveToFile("s" + guid + ".docx", FileFormat.Docx);
                doc.Close();

                Process.Start("s" + guid + ".docx");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 文档合成
        /// </summary>
        /// <param name="sourcePath">原文档</param>
        /// <param name="insertPath">插入到第二页的文档</param>
        /// <param name="saveToPath">保存路径</param>
        private void SaveToFileInsertText(string sourcePath, string insertPath, string saveToPath)
        {
            Document doc = new Document(sourcePath);
            doc.InsertTextFromFile(insertPath, FileFormat.Docx);
            doc.SaveToFile(saveToPath, FileFormat.Docx);
            doc.Close();
        }
        string path = AppDomain.CurrentDomain.BaseDirectory + "/通用单元操作模板/";
        public void run()
        {
            try
            {



                Document doc=null;



                DirectoryInfo d = new DirectoryInfo(path);
                DirectoryInfo[] directs = d.GetDirectories();//文件夹
                string[] text = textBox1.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i].Trim() != "")
                    {
                        string zimu = text[i].Trim().Substring(0, 1);
                        string zimushuzi = text[i].Trim().Substring(0, 2);
                        foreach (DirectoryInfo dd in directs)//遍历文件夹
                        {
                            if (Path.GetFileName(dd.FullName).Substring(0, 1) == zimu)
                            {
                                FileInfo[] files = dd.GetFiles();//文件
                                foreach (FileInfo f in files)
                                {
                                    if (f.Name.Substring(0, 2) == zimushuzi)
                                    {
                                        //获取到模板文件
                                        //MessageBox.Show(f.FullName);
                                       if(i==0)
                                        {
                                           doc = new Document(f.FullName);
                                        }
                                       else
                                        {
                                            doc.InsertTextFromFile(f.FullName, FileFormat.Docx);
                                        }
                                        
                                    }
                                }
                            }
                        }

                    }
                    //SaveToFileInsertText(@"C:\Users\zhou\Desktop\B1常规固体加入.docx", @"C:\Users\zhou\Desktop\A1反应罐检查.docx", "1.docx");
                }



                String fileName = "";
                using (SaveFileDialog fileDialog = new SaveFileDialog())
                {
                    fileDialog.Filter = "WORD文件(*.docx)|*.docx";
                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        fileName = fileDialog.FileName;
                        if (!String.IsNullOrEmpty(fileName))
                        {
                            doc.SaveToFile(fileName, FileFormat.Docx);
                        }
                    }
                }
                MessageBox.Show(String.Format("文件成功保存到{0}", fileName));

                doc.Close();

            }
            catch (Exception ex)
            {

              MessageBox.Show(ex.ToString());
            }
        }

       
        private void WORD文档处理_Load(object sender, EventArgs e)
        {
            //时间校验
            if(DateTime.Now>Convert.ToDateTime("2022-02-15"))
            {
                MessageBox.Show("时间校验失败");
            }
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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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


        private void button1_Click(object sender, EventArgs e)
        {
            //if (thread == null || !thread.IsAlive)
            //{
            //    thread = new Thread(run);
            //    thread.Start();
            //    Control.CheckForIllegalCrossThreadCalls = false;
            //}
            run();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }
    }
}
