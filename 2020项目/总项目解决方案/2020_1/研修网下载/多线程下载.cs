using System;
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
                illegal = illegal.Replace(c.ToString(), "");
            }
            return illegal;
        }

        #endregion
        bool zanting = true;
        
        string path = AppDomain.CurrentDomain.BaseDirectory;
        bool status = true;
        #region  主程序
        public void run(object cook)
        {

            try
            {

                string cookie = cook.ToString();

                for (int i = Convert.ToInt32(textBox1.Text); i <= Convert.ToInt32(textBox2.Text); i = i + 1)

                {

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



                                method.downloadFile(downUrl, path + "下载文件\\", removeValid(bt) + "." + gs, cookie);

                                textBox6.Text += DateTime.Now.ToString() + "  " + i + "成功：" + bt + "\r\n";







                            }
                            else
                            {
                                //textBox6.Text += DateTime.Now.ToString() + "格式不符合跳过下载" + "\r\n";
                            }

                        }
                        else
                        {
                            textBox6.Text += DateTime.Now.ToString() + "格式不符合跳过下载" + "\r\n";
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
            catch (System.Exception ex)
            {
                textBox6.Text = ex.ToString();
            }


        }




        #endregion


        public void ceshi(object value)
        {
            for (int i = 0; i < 10; i++)
            {
                textBox6.Text += Thread.CurrentThread.Name + "  "+value + i + "\r\n";
                Thread.Sleep(2000);
            }
        }
            
        private void 多线程下载_Load(object sender, EventArgs e)
        {

        }

        Dictionary<string, string> dic = new Dictionary<string, string>();

        private void button2_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            StreamReader sr = new StreamReader(path+"账号.txt", Encoding.Default);
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int i = 0; i < text.Length; i++)
            {
                string[] zhanghao=text[i].Split(new string[] { "#" }, StringSplitOptions.None);
                dic.Add(zhanghao[0], zhanghao[1]);



            }







            for (int i = 1; i < 3; i++)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(ceshi));
                string o = "参数"+i;
                thread.Start((object)o);
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Name = "线程"+i;
            }
            
        }
    }
}
