using System;
using System.Collections;
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

namespace 主程序202011
{
    public partial class 新疆继续教育 : Form
    {
        public 新疆继续教育()
        {
            InitializeComponent();
        }
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public  string GetUrl(string Url)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
              
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://servicewechat.com/wx8b422900aa39f849/19/page-frame.html";
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 13_6_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.13(0x17000d2a) NetType/4G Language/zh_CN";        
                request.AllowAutoRedirect = true;
                 request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                string content = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
              textBox3.Text+=DateTime.Now.ToString()+"： "+ ex.ToString()+"\r\n";

            }
            return "";
        }
        #endregion

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";

            
                request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.Referer = "https://servicewechat.com/wx8b422900aa39f849/19/page-frame.html";
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 13_6_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.13(0x17000d2a) NetType/4G Language/zh_CN";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion
        string code = "daa9ed7a7712475f4ff3ccd39fdab4cd";
        string credential = "84363624-5558-4e4c-96f3-f92972f90953";
        /// <summary>
        /// 获取课程
        /// </summary>
        /// 
        public void getlog()
        {
            textBox2.Text = "";
            for (int year = 2014; year < 2021; year++)
            {


                string url = "https://yun.xjrsjxjy.com/API/GetStudyCoursewares.ashx?code=" + code + "&credential=" + credential + "&year=" + year + "&isFree=false";

                string html = GetUrl(url);
                if (html.Contains("登录超时"))
                {
                    MessageBox.Show("账号已掉线");
                    return;
                }
                MatchCollection ids = Regex.Matches(html, @"""Id"":""([\s\S]*?)""");
                MatchCollection names = Regex.Matches(html, @"""Name"":""([\s\S]*?)""");
                MatchCollection Progress = Regex.Matches(html, @"""LearningProgress"":([\s\S]*?),");

                for (int j = 0; j < ids.Count; j++)
                {

                    textBox2.Text += DateTime.Now.ToString("MM-dd HH:mm") +"："+year + "年.." + names[j].Groups[1].Value.ToString() + "【" + (Convert.ToDouble(Progress[j].Groups[1].Value) * 100).ToString() + "%】"+"\r\n";


                }


            }
        }
        public void GetStudyCoursewares()
        {
           





            for (int year = 2014; year < 2021; year++)
            {


                string url = "https://yun.xjrsjxjy.com/API/GetStudyCoursewares.ashx?code=" + code + "&credential=" + credential + "&year=" + year + "&isFree=false";

                string html = GetUrl(url);
                if (html.Contains("登录超时"))
                {
                    MessageBox.Show("账号已掉线");
                    return;
                }
                MatchCollection ids = Regex.Matches(html, @"""Id"":""([\s\S]*?)""");
                MatchCollection names = Regex.Matches(html, @"""Name"":""([\s\S]*?)""");
                MatchCollection Progress = Regex.Matches(html, @"""LearningProgress"":([\s\S]*?),");
                
                for (int j = 0; j < ids.Count; j++)
                {

                  
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据    
                        lv1.SubItems.Add(year.ToString());
                        lv1.SubItems.Add(names[j].Groups[1].Value.ToString());
                        lv1.SubItems.Add((Convert.ToDouble(Progress[j].Groups[1].Value) * 100).ToString() + "%");
                        lv1.SubItems.Add(ids[j].Groups[1].Value.ToString());
                   


                }

               
        }
    }



        /// <summary>
        /// 获取子课程
        /// </summary>
        public ArrayList GetChapters(string year, string coursewareId)
        {
            ArrayList list = new ArrayList();
            string url = "https://yun.xjrsjxjy.com/API/GetChapters.ashx?code=" + code + "&credential=" + credential + "&year=" + year + "&coursewareId=" + coursewareId;

            string html = GetUrl(url);
            if (html.Contains("登录超时"))
            {
                //账号掉线
                return null;
            }
            MatchCollection ids = Regex.Matches(html, @"""Id"":""([\s\S]*?)"",([\s\S]*?)""TotalSeconds"":([\s\S]*?)\}");
            MatchCollection LearningProgress = Regex.Matches(html, @"""LearningProgress"":([\s\S]*?),");

            for (int j = 0; j < ids.Count; j++)
            {
               
                list.Add(ids[j].Groups[1].Value+"#"+ ids[j].Groups[3].Value+"#"+ LearningProgress[j].Groups[1].Value);
               
            }

            return list;

        }

        public void study(object parame)
        {
           
            string[] text = parame.ToString().Split(new string[] { "#" }, StringSplitOptions.None);
            string year = text[0];
            string coursewareId = text[1];
            string chapterId = text[2];
            
            int totalseconds = (Convert.ToInt32(Convert.ToDouble(text[3])))+150;
            
            string url = "https://yun.xjrsjxjy.com/API/SaveLearningProgress.ashx";
            string postdata = "code="+code+"&credential="+credential+"&year="+year+"&coursewareId="+ coursewareId + "&chapterId="+ chapterId + "&type=0&progress=0";
            
            string html = PostUrl(url,postdata);
            for (int i = 1; i < totalseconds; i++)
            {
                string postdata1 = "code=" + code + "&credential=" + credential + "&year=" + year + "&coursewareId=" + coursewareId + "&chapterId=" + chapterId + "&type=1&progress="+i;
            
                if (i % 60 == 0)
                {
                    PostUrl(url, postdata1);
                    getlog();
                    //button2.PerformClick();
                }
                Thread.Sleep(1011);
            }

        }
        
        public void run()
        {
            if (listView1.CheckedItems.Count == 0)
            {
                MessageBox.Show("请勾选需要学习的课程");
                button1.Text = "开始学习";
                return;
            }


            try
            {
                for (int i = 0; i < listView1.CheckedItems.Count; i++)
                {
                    string year = listView1.CheckedItems[i].SubItems[1].Text;
                    string coursewareId = listView1.CheckedItems[i].SubItems[4].Text;
                    ArrayList chapterIds = GetChapters(year,coursewareId);
                    foreach (string values in chapterIds)
                    {
                        string[] value = values.Split(new string[] { "#" }, StringSplitOptions.None);
                        
                        string chapterId = value[0];
                        string seconds = value[1];
                        string LearningProgress = value[2];
                        //Thread thread = new Thread(new ParameterizedThreadStart(study));
                        //string o = year+","+coursewareId+","+chapterId;
                        //thread.Start((object)o);
                        //Control.CheckForIllegalCrossThreadCalls = false;
                        
                        if (LearningProgress.Trim() != "1")
                        {
                            string o = year + "#" + coursewareId + "#" + chapterId + "#" + seconds;
                            study((object)o);
                        }
                        
                       
                       
                    }

                }

                button1.Text = "学习完毕";
                MessageBox.Show("已全部学习完毕");

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void 新疆继续教育_Load(object sender, EventArgs e)
        {

        }
        Thread t;
        private void button1_Click(object sender, EventArgs e)
        {
            Match cod = Regex.Match("", @"code=([\s\S]*?)&");
            Match cre = Regex.Match("", @"credential=([\s\S]*?)&");

            code = cod.Groups[1].Value;
            credential = cre.Groups[1].Value;

            listView1.Items.Clear();
            GetStudyCoursewares();
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Checked = true;
            }



            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html");

            if (!html.Contains(@"xinjiang"))
            {
                MessageBox.Show("");
                return;
            }

            #endregion

            if (t == null || !t.IsAlive)
            {
                t = new Thread(run);
                t.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
                button1.Text = "正在学习...";
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
           

           
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           
        }

        private void 新疆继续教育_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
                //System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }

        private void listView2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }

        private void listView2_DragDrop(object sender, DragEventArgs e)
        {
            string[] filePath = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in filePath)
            {
                StreamReader sr = new StreamReader(file, Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {
                    string[] value = text[i].Split(new string[] { "----" }, StringSplitOptions.None);
                    ListViewItem lv2 = listView2.Items.Add(listView2.Items.Count.ToString()); //使用Listview展示数据
                    lv2.SubItems.Add(value[0].Trim());
                    lv2.SubItems.Add(value[1].Trim());


                }
            }




        }
    }
}
