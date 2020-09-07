using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 研修网下载
{
    public partial class 多线程下载 : Form
    {
        public 多线程下载()
        {
            InitializeComponent();
        }
        #region 去掉路径中非法字符
        public string removeValid(string illegal)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                illegal = illegal.Replace(c.ToString(), "").Replace("&", "");
            }
            return illegal;
        }

        #endregion


       





        bool zanting = true;

        string path = AppDomain.CurrentDomain.BaseDirectory;
        ArrayList finishes = new ArrayList();
        bool status = true;
        #region  主程序
        public void run(object cook)
        {

            // string newpath = path + "下载文件\\" + Thread.CurrentThread.Name + "\\";

            string newpath = path + "下载文件\\";

            if (!Directory.Exists(newpath))
            {
                Directory.CreateDirectory(newpath); //创建文件夹
            }
            

            try
            {

                string cookie = cook.ToString();

                for (int i = Convert.ToInt32(textBox1.Text); i <= Convert.ToInt32(textBox2.Text); i = i + 1)

                {
                    if (!finishes.Contains(i))
                    {
                        finishes.Add(i);
                        try
                        {



                            FileStream fs1 = new FileStream(path + "config.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                            StreamWriter sw = new StreamWriter(fs1);
                            sw.WriteLine(i.ToString());
                            sw.Close();
                            fs1.Close();


                            string Url = "http://q.yanxiu.com/upload/viewResource.tc?resId=" + i;

                            string html = method.GetUrl(Url, "utf-8");  //定义的GetRul方法 返回 reader.ReadToEnd()
                            string downUrl = "http://q.yanxiu.com/uploadResource/DownloadServlet?type=res2&resId=" + i;

                            Match title = Regex.Match(html, @"<h1>([\s\S]*?)</h1>");
                            Match geshi = Regex.Match(html, @"格式：</dt>([\s\S]*?)</dd>");
                            Match daxiao = Regex.Match(html, @"<dd class=""w160"">([\s\S]*?)</dd>");

                            string gs = geshi.Groups[1].Value.Replace("<dd>", "").Trim();
                            string dx = daxiao.Groups[1].Value.Replace(".", "").Replace(" ", "").Trim().Replace("M", "00").Replace("K", "");
                            string bt = title.Groups[1].Value.Replace(".", "").Replace("doc", "").Replace("docx", "").Replace("ppt", "").Replace("pptx", "").Replace(" ", "").Trim();

                            if (dx != "" && dx != null)
                            {

                                if (Convert.ToInt32(dx) > 3)
                                {

                                    if (gs == "doc" || gs == "docx" || gs == "ppt" || gs == "pptx" || gs == "pdf" )
                                    {
                                        if (btPanduan(bt))
                                        {
                                            method.downloadFile(downUrl, newpath, removeValid(bt) + "." + gs, cookie);
                                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                            lv1.SubItems.Add(DateTime.Now.ToString());
                                            lv1.SubItems.Add(bt);
                                            lv1.SubItems.Add(Thread.CurrentThread.Name);
                                            lv1.SubItems.Add(i.ToString());
                                        }
                                        else
                                        {
                                            textBox4.Text = DateTime.Now.ToString() + "标题【" + bt + "】不符" + "\r\n";
                                        }

                                    }
                                    else
                                    {
                                        textBox4.Text = DateTime.Now.ToString() + "格式【"+gs+"】不符" + "\r\n";
                                    }
                                }
                                else
                                {
                                    textBox4.Text = DateTime.Now.ToString() + "大小不符合跳过下载" + "\r\n";
                                }

                            }
                            else
                            {
                                textBox4.Text = DateTime.Now.ToString() + "大小为空跳过下载" + "\r\n";
                            }

                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }

                            Thread.Sleep(13000);
                            if (status == false)
                            {
                                return;
                            }

                        }
                        catch
                        {

                            continue;
                        }
                    }

                }




            }
            catch (System.Exception ex)
            {
               textBox4.Text= ex.ToString();
            }


        }




        #endregion


            
        private void 多线程下载_Load(object sender, EventArgs e)
        {
            foreach (Control ctr in groupBox2.Controls)
            {

                if (ctr is TextBox)
                {

                    string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
                    if (File.Exists(path + ctr.Name + ".txt"))
                    {

                        StreamReader sr1 = new StreamReader(path + ctr.Name + ".txt", Encoding.GetEncoding("utf-8"));
                        //一次性读取完 
                        string texts1= sr1.ReadToEnd();
                        ctr.Text = texts1;
                        sr1.Close();
                    }
                }
            }

            StreamReader sr = new StreamReader(path +  "config.txt", Encoding.GetEncoding("utf-8"));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            textBox1.Text = texts.Replace("\r\n","").Trim();
            sr.Close();
        }

        Dictionary<string, string> dic = new Dictionary<string, string>();


        public string getCookie(int i)
        {

            string path = AppDomain.CurrentDomain.BaseDirectory;
            StreamReader sr = new StreamReader(path +"cookie\\"+ i+".txt", Encoding.Default);
           
            string texts = sr.ReadToEnd();
            return texts.Trim();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            status = true;




            for (int i = 1; i <=20; i++)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(run));
                string o = getCookie(i);
                thread.Start((object)o);
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Name = "线程"+i;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        public bool btPanduan(string bt)
        {

            Match panduan = Regex.Match(bt, @"^[0-9a-zA-Z]+$");

            if (panduan.Groups[0].Value != "")
            {
                return false;

            }
            else if (bt.Contains("党") || bt.Contains("国") || bt.Contains("新建"))
            { 

                return false;
            }

            else
            {
                return true;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(removeValid("789*.&"));
           // MessageBox.Show(btPanduan(textBox1.Text).ToString());
            status = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void 多线程下载_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确认退出吗？", "退出询问"
           , MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK)
            {
                e.Cancel = true;//告诉窗体关闭这个任务取消

            }
            else
            {
                foreach (Control ctr in groupBox2.Controls)
                {
                    if (ctr is TextBox)
                    {


                        string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
                        FileStream fs1 = new FileStream(path + ctr.Name + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                        StreamWriter sw = new StreamWriter(fs1);
                        sw.WriteLine(ctr.Text);
                        sw.Close();
                        fs1.Close();

                    }
                }
            }

            }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
