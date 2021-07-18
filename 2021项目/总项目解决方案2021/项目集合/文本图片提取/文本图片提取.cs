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
using myDLL;

namespace 文本图片提取
{
    public partial class 文本图片提取 : Form
    {
        public 文本图片提取()
        {
            InitializeComponent();
        }
        string path = System.IO.Directory.GetCurrentDirectory();
        #region 下载文件  【好用】
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="URLAddress">图片地址</param>
        /// <param name="subPath">图片所在文件夹</param>
        /// <param name="name">图片名称</param>
        public static void downloadFile(string URLAddress, string subPath, string name,string ip)
        {
            try
            {
                WebClient client = new WebClient();
                string[] ips= ip.Split(new string[] { ":" }, StringSplitOptions.None);
             
                //if(ips.Length>1)
                //{
                //    client.Proxy = new WebProxy(ips[0], Convert.ToInt32(ips[1]));

                //}    
 
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
                client.Headers.Add("Cookie", "");
                client.Headers.Add("Referer", "");
                if (false == System.IO.Directory.Exists(subPath))
                {
                    //创建pic文件夹
                    System.IO.Directory.CreateDirectory(subPath);
                }

                client.DownloadFile(URLAddress, subPath + "\\" + name);
            }
            catch (WebException ex)
            {

                ex.ToString();
            }
        }



        #endregion

        List<string> iplist = new List<string>();
        int iPcount = 0;
        public void getip100()
        {
            iplist.Clear();
            string html = method.GetUrl(textBox3.Text.Trim(), "utf-8");
        
            string[] ips = html.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
          
            foreach (var ip in ips)
            {
               
                iplist.Add(ip);
            }

        }
       
        public string getipone()
        {
            iPcount = iPcount + 1;
            if (iPcount < iplist.Count)
            {
                return iplist[iPcount];
            }
            else
            {

                getip100();
                iPcount = 0;
                return iplist[0];
            }

        }
      


        public string getsuijizimushuzi()
        {
            string zimu = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            Random rd = new Random(Guid.NewGuid().GetHashCode()); //生成不重复的随机数，默认的话根据时间戳如果太快会相同
            string value = "";
            for (int i = 0; i < 5; i++)
            {
                int suiji = rd.Next(0, 10);
                value = value + suiji;

                int suijizimu = rd.Next(0, 52);
                value = value + zimu[suijizimu];
            }

            return value;
        }
        int count = 0;
        public void downimg(object o)
        {
           
            string[] text = o.ToString().Split(new string[] { "#" }, StringSplitOptions.None);
            string url = text[0];
            string imgname = text[1];
            string ip = text[2];

            count = count + 1;
           
            downloadFile(url,path+"//img//",imgname+".jpg",ip);
            if (File.Exists(path + "//img//" + imgname + ".jpg"))
            {
               // return true;
            }

            else
            {
                //return false;
            }
        }

        List<string> txtlist = new List<string>();
      
        public void run()
        {
           
            string path = textBox1.Text;
            DirectoryInfo folder = new DirectoryInfo(path);
            int c = folder.GetFiles("*.txt").Count();
            for (int i = 0; i < c; i++)
            {
                string fullname = folder.GetFiles("*.txt")[i].FullName;
                if (!txtlist.Contains(fullname))
                {
                    string ip = getipone();
                    txtlist.Add(fullname);
                    try
                    {
                        textBox2.Text = "共" + c + "已处理：" + (i + 1);
                       

                        textBox2.Text = "正在处理：" + fullname + "\r\n";

                        StreamReader sr = new StreamReader(fullname, myDLL.method.EncodingType.GetTxtType(fullname));
                        String ReadTxt = sr.ReadToEnd();
                        string txtname = Path.GetFileNameWithoutExtension(fullname); ;

                        MatchCollection picurls = Regex.Matches(ReadTxt, @"src=""([\s\S]*?)""");
                        foreach (Match picurl in picurls)
                        {

                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(txtname);
                            lv1.SubItems.Add(picurl.Groups[1].Value);
                            string imgname = getsuijizimushuzi();
                            lv1.SubItems.Add("../img/" + imgname + ".jpg");

                            if (listView1.Items.Count > 2)
                            {
                                this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                            }

                          
                            Thread thread = new Thread(new ParameterizedThreadStart(downimg));
                            string o = picurl.Groups[1].Value + "#" + imgname + "#" + ip;
                            thread.Start((object)o);
                            Control.CheckForIllegalCrossThreadCalls = false;

                            lv1.SubItems.Add("true");
                            // lv1.SubItems.Add(success.ToString()+ip+Thread.CurrentThread.Name);

                            ReadTxt = ReadTxt.Replace(picurl.Groups[1].Value, "../img/" + imgname + ".jpg");
                        }
                        ReadTxt = Regex.Replace(ReadTxt, @"alt="".*""", "\"");




                        if (checkBox1.Checked == true)
                        {
                            ReadTxt = ReadTxt.Replace("\" >", " title=\"" + txtname + "\" >");
                        }
                        if (checkBox2.Checked == true)
                        {
                            ReadTxt = ReadTxt.Replace("\" >", " \" alt=\"" + txtname + "\" >");
                        }

                        sr.Close();
                        sr.Dispose();

                        StreamWriter sw = new StreamWriter(fullname);
                        sw.Write(ReadTxt);
                        sw.Close();
                        sw.Dispose();

                    }

                    catch (Exception ex)
                    {
                        textBox2.Text=ex.ToString();
                        continue;
                    }
                }
            }


            textBox2.Text = Thread.CurrentThread.Name+"处理结束";

        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请选择文件夹");
                return;
            }
            getip100();
            for (int i = 0; i < numericUpDown1.Value; i++)
            {
                Thread thread = new Thread(run);
                thread.Name = "线程" + i;
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
                
            
            //if (thread == null || !thread.IsAlive)
            //{
            //    thread = new Thread(run);
            //    thread.Start();
            //    Control.CheckForIllegalCrossThreadCalls = false;
            //}
        }

        private void button1_Click(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void 文本图片提取_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
