using SharpCompress.Reader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpCompress.Common;
using System.Threading;
using System.Net;

namespace 主程序202202
{
    public partial class 文件解压缩处理 : Form
    {
        public 文件解压缩处理()
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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("appid:orders");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 100000;

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


        List<string> finishes = new List<string>();
      
        public void unTAR()
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (!finishes.Contains(i.ToString()))
                {
                    finishes.Add(i.ToString());

                    try
                    {
                        DateTime dt1 = DateTime.Now;
                        string tarFilePath = listView1.Items[i].SubItems[1].Text;
                       

                        String directoryPath = AppDomain.CurrentDomain.BaseDirectory + "临时" + "//" + Path.GetFileNameWithoutExtension(tarFilePath) + "//";

                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath); //创建文件夹
                        }
                        SharpCompress.Common.ArchiveEncoding.Default = Encoding.UTF7;

                        using (Stream stream = File.OpenRead(tarFilePath))
                        {
                            var reader = ReaderFactory.Open(stream);

                            while (reader.MoveToNextEntry())
                            {
                                if (!reader.Entry.IsDirectory)
                                    reader.WriteEntryToDirectory(directoryPath,
                                       ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);

                            }
                        }
                        DateTime dt2 = DateTime.Now;
                        TimeSpan span = dt2 - dt1;
                        listView1.Items[i].SubItems[3].Text = span.TotalSeconds.ToString() + "秒";
                        listView1.Items[i].SubItems[4].Text = "完成";

                        shaixaunfile(directoryPath, Path.GetFileNameWithoutExtension(tarFilePath));

                        if (checkBox1.Checked == true)
                        {
                            File.Delete(tarFilePath);
                        }
                    }
                    catch (Exception)
                    {
                       
                        listView1.Items[i].SubItems[4].Text = "格式错误";
                    }
                }
            }
        }


        public void shaixaunfile(string dir,string  zipname)
        {
            try
            {

                string newpath = AppDomain.CurrentDomain.BaseDirectory + "临时" + "//" + zipname + "\\var\\mobile\\Media\\ZORRO";
                DirectoryInfo dd1 = new DirectoryInfo(newpath);
                DirectoryInfo[] directs1 = dd1.GetDirectories();
                newpath = directs1[0].Name;
             

                string lastpath = textBox1.Text + newpath + "//";
                DirectoryInfo d = new DirectoryInfo(dir);
                FileInfo[] files = d.GetFiles();//文件
                DirectoryInfo[] directs = d.GetDirectories();//文件夹
                foreach (FileInfo f in files)
                {

                    if (f.Name == "record_enc.plist")
                    {

                        if (!Directory.Exists(lastpath))
                        {
                            Directory.CreateDirectory(lastpath); //创建文件夹
                        }

                        if (!File.Exists(lastpath + Path.GetFileName(f.FullName)))
                        {
                            File.Copy(f.FullName, lastpath + Path.GetFileName(f.FullName));
                        }
                    }


                    if (f.Name == "JDLoginInfo.plist")
                    {
                        string path1 = lastpath + "com.360buy.jdmobile\\Documents\\";
                        if (!Directory.Exists(path1))
                        {
                            Directory.CreateDirectory(path1); //创建文件夹
                        }
                        if (!File.Exists(path1 + Path.GetFileName(f.FullName)))
                        {
                            File.Copy(f.FullName, path1 + Path.GetFileName(f.FullName));
                        }
                    }


                    if (f.Name == "Cookies.binarycookies")
                    {
                        string path1 = lastpath + "com.360buy.jdmobile\\Library\\Cookies\\";
                        if (!Directory.Exists(path1))
                        {
                            Directory.CreateDirectory(path1); //创建文件夹
                        }
                        if (!File.Exists(path1 + Path.GetFileName(f.FullName)))
                        {
                            File.Copy(f.FullName, path1 + Path.GetFileName(f.FullName));
                        }
                    }
                    if (f.Name == "group.jdmobile.p")
                    {
                        string path1 = lastpath + "com.360buy.jdmobile\\Library\\JDUserDefaults\\";
                        if (!Directory.Exists(path1))
                        {
                            Directory.CreateDirectory(path1); //创建文件夹
                        }
                        if (!File.Exists(path1 + Path.GetFileName(f.FullName)))
                        {
                            File.Copy(f.FullName, path1 + Path.GetFileName(f.FullName));
                        }
                    }


                    if (f.Name.Contains("enc.igri"))
                    {
                        string path1 = textBox1.Text;
                        if (!Directory.Exists(path1))
                        {
                            Directory.CreateDirectory(path1); //创建文件夹
                        }
                        if (!File.Exists(path1 + Path.GetFileName(f.FullName)))
                        {
                            File.Copy(f.FullName, path1 + Path.GetFileName(f.FullName));
                        }
                    }

                }
                //获取子文件夹内的文件列表，递归遍历  
                foreach (DirectoryInfo dd in directs)
                {
                    shaixaunfile(dd.FullName, zipname);
                }

            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.ToString()) ;
            }
        }


        //public void readcookie(string path1,string path2)
        //{
        //    using (FileStream stream = File.OpenRead(path1))
        //    {
        //        byte[] content = new byte[stream.Length];

        //        for (int i = 0; i < content.Length; i++)
        //        {
        //            content[i] = (byte)stream.ReadByte();
                    
        //        }
        //        MessageBox.Show(Encoding.Default.GetString(content));

        //    }
        //}


        private void button1_Click(object sender, EventArgs e)
        {
            //    string cookiepath = @"C:\Users\zhou\Desktop\Cookies.binarycookies";
            //    //FileStream file = new FileStream(cookiepath, FileMode.Open, FileAccess.Read);
            //    //BinaryReader read = new BinaryReader(file);
            //    //int count = (int)file.Length;
            //    //byte[] buffer = new byte[count];
            //    //read.Read(buffer, 0, buffer.Length);
            //    //string msg = Encoding.Default.GetString(buffer);

            //    //textBox1.Text = msg;


            //    readcookie(cookiepath, @"C:\Users\zhou\Desktop\Cookies.txt");

            label3.Text = "启动中...";
            button1.Enabled = false;
            for (int i = 0; i < numericUpDown1.Value; i++)
            {
              Thread  thread = new Thread(unTAR);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
                Thread.Sleep(100);
            }

            button1.Enabled = true;
        }


       


        public void Director(string dir)
        {
            try
            {
                DirectoryInfo d = new DirectoryInfo(dir);
                FileInfo[] files = d.GetFiles();//文件
                DirectoryInfo[] directs = d.GetDirectories();//文件夹
                foreach (FileInfo f in files)
                {
                    ListViewItem item2 = listView1.Items.Add(new ListViewItem(new string[] { (listView1.Items.Count + 1).ToString(), dir+"\\"+f.Name, "", "", "未启动" }));

                }

            }
            catch (Exception)
            {

               MessageBox.Show("请拖入文件夹");
            }
            
        }
        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            //Array filePath = ((Array)e.Data.GetData(DataFormats.FileDrop));

            //foreach (var item in filePath)
            //{
            //    ListViewItem item2 = listView1.Items.Add(new ListViewItem(new string[] { (listView1.Items.Count + 1).ToString(), item.ToString(), "", "", "未启动" }));
            //}
            Array filePath = ((Array)e.Data.GetData(DataFormats.FileDrop));
            Director(filePath.GetValue(0).ToString());

        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }

        private void 文件解压缩处理_Load(object sender, EventArgs e)
        {
            #region 通用检测


            string html = GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"Qg4w"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion
            textBox1.Text = AppDomain.CurrentDomain.BaseDirectory+ "ZORRO\\";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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

                textBox1.Text = dialog.SelectedPath;
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
