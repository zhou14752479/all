using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 发票有奖查询
{
    public partial class 发票有奖查询 : Form
    {


        //[DllImport("AspriseOCR.dll", EntryPoint = "OCR")]
        //public static extern IntPtr OCR(string file, int type);

        ////识别条形码
        //[DllImport("AspriseOCR.dll", EntryPoint = "OCRBarCodes")]
        //static extern IntPtr OCRBarCodes(string file, int type);
        string yzmusername = "";
        string yzmpassword = "";

        #region  网络图片转Bitmap
        public Bitmap UrlToBitmap(string url)
        {
            WebClient mywebclient = new WebClient();
            mywebclient.Headers.Add("Cookie", cookie);
            byte[] Bytes = mywebclient.DownloadData(url);
            using (MemoryStream ms = new MemoryStream(Bytes))
            {
                Image outputImg = Image.FromStream(ms);

                Bitmap map = new Bitmap(outputImg);
                return map;
            }
        }
        #endregion



        #region POST请求

        public string PostUrl(string url, string postData)
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
                request.Headers.Add("Cookie", cookie);
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Referer = "http://www.jszwfw.gov.cn/jsjis/h5/jszfjis/view/perregister.html";
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

                //MessageBox.Show(ex.ToString());
                return "";
            }


        }

        #endregion

        #region 图片转base64
        public static string ImgToBase64String(Bitmap bmp)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch
            {
                return null;
            }
        }

        #endregion


        public string shibie()
        {
            try
            {
                Bitmap image = UrlToBitmap("https://yjfp.guangdong.chinatax.gov.cn:8453/yzm/get.do");



                string param = "{\"username\":\"" + yzmusername + "\",\"password\":\"" + yzmpassword + "\",\"image\":\"" + ImgToBase64String(image) + "\"}";

                string PostResult = PostUrl("http://api.ttshitu.com/base64", param);

                Match result = Regex.Match(PostResult, @"result"":""([\s\S]*?)""");
                if (result.Groups[1].Value != "")
                {

                    return result.Groups[1].Value;
                }
                else
                {

                   textBox5.Text += "图片验证码错误:" + PostResult + "\r\n";
                    // status = false;
                    return "";
                }

            }
            catch (Exception ex)
            {

                textBox5.Text += (ex.ToString());
                return "";
            }
        }





        public 发票有奖查询()
        {
            InitializeComponent();
        }
        Thread thread;
        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            // openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.Filter = "TXT文本文件|*.txt";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox1.Text = openFileDialog1.FileName;



            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox2.Text=="")
            {
                MessageBox.Show("请输入sessionkey");
                return;
            }

            if (textBox1.Text == "")
            {
                MessageBox.Show("请导入数据");
                return;
            }
            status = true;

            run();
            //if (thread == null || !thread.IsAlive)
            //{
            //    thread = new Thread(run);
            //    thread.Start();
            //    Control.CheckForIllegalCrossThreadCalls = false;
            //}
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ListviewToTxt(listView1, 1);
        }

        #region  listview导出文本TXT
        public void ListviewToTxt(ListView listview, int i)
        {

            if (listview.Items.Count == 0)
            {
                MessageBox.Show("列表为空!");
            }

            List<string> list = new List<string>();




            foreach (ListViewItem item in listview.Items)
            {
                if (item.SubItems[i].Text.Trim() != "")
                {
                    list.Add(item.SubItems[i].Text + item.SubItems[i + 1].Text);

                }


            }
            SaveFileDialog sfd = new SaveFileDialog();

            // string path = AppDomain.CurrentDomain.BaseDirectory + "导出_" + Guid.NewGuid().ToString() + ".txt";
            string path = "";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                path = sfd.FileName + ".txt";
            }

            string[] Mystrings = list.ToArray();

            for (int a = 0; a < Mystrings.Count(); a++) //每个字符串都要参与比较
            {
                for (int j = 1; j < Mystrings.Count(); j++) //字符串长度较长的排在前面
                {
                    if (Mystrings[j - 1].Length < Mystrings[j].Length)
                    {
                        string temp = Mystrings[j - 1];
                        Mystrings[j - 1] = Mystrings[j];
                        Mystrings[j] = temp;
                    }
                }
            }


            StringBuilder sb = new StringBuilder();
            foreach (string tel in Mystrings)
            {
                sb.AppendLine(tel);
            }
            System.IO.File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            MessageBox.Show("文件导出成功!文件地址:" + path);


        }







        #endregion

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
           

            status = false;
        }



        public void run()
        {
            listView1.Items.Clear();
            ;
            if (textBox1.Text == "")
            {

                MessageBox.Show("请导入数据");
                return;

            }

            StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存


            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != "")
                {

                    ListViewItem item = listView1.Items.Add(new ListViewItem(new string[] { (listView1.Items.Count + 1).ToString(), text[i], "准备中", "" }));
                    int id = item.Index;
                    AddDown(id, text[i]);
                }
            }
            StartDown(Convert.ToInt32(numericUpDown1.Value));//开始线程
        }

        public void AddDown(int id, string uid)
        {
            Thread tsk = new Thread(() =>
            {
                download(id, uid);
            });
            list.Add(tsk);
        }

        public void StartDown(int StartNum)
        {

            for (int i2 = 0; i2 < StartNum; i2++)
            {
                lock (list)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].ThreadState == System.Threading.ThreadState.Unstarted || list[i].ThreadState == ThreadState.Suspended)
                        {
                            list[i].Start();
                            break;
                        }
                    }
                }
            }

        }


        string cookie = "";
        string yzm = "";
        string yzmmsg = "";
        private void download(int id, string uid)
        {
            DownMsg msg = new DownMsg();
            try
            {

                msg.Id = id;
                try
                {



                    string[] text = uid.Split(new string[] { "," }, StringSplitOptions.None);

                    if (text.Length < 5)
                    {
                        msg.status = "格式错误";
                        msg.Tag = "end";
                        doSendMsg(msg);
                        return;
                    }

                    if(yzmmsg=="" || yzmmsg.Contains("验证码校验失败"))
                    {
                        yzm = shibie();
                    }
                   
                    string url = "https://yjfp.guangdong.chinatax.gov.cn:8453/xxzcpt/jkgl/jk.do?BEANID=boFpyjYjXCX&APPTYPE=3&BusinessId=app&zflx=0&FORMID=the%20formId%20is%20no%20longer%20available%20in%20develop%20or%20trial%20version%20of%20this%20mini%20program&fpdm="+ text[2].Trim() + "&fphm="+ text[3].Trim() + "&jshj=" + text[4].Trim() + "&thired_sessionkey="+textBox2.Text.Trim()+"&fpyjrkly=0&yzm="+yzm;
                    string html = method.GetUrlWithCookie(url, cookie,"utf-8");
                   
                    int time = 0;
                    while(html.Contains("验证码校验失败"))
                    {
                        textBox5.Text += html + "\r\n";
                        time++;
                        yzm = shibie();
                        url = "https://yjfp.guangdong.chinatax.gov.cn:8453/xxzcpt/jkgl/jk.do?BEANID=boFpyjYjXCX&APPTYPE=3&BusinessId=app&zflx=0&FORMID=the%20formId%20is%20no%20longer%20available%20in%20develop%20or%20trial%20version%20of%20this%20mini%20program&fpdm=" + text[2].Trim() + "&fphm=" + text[3].Trim() + "&jshj=" + text[4].Trim() + "&thired_sessionkey=" + textBox2.Text.Trim() + "&fpyjrkly=0&yzm=" + yzm;
                        html = method.GetUrlWithCookie(url, cookie, "utf-8");
                        Thread.Sleep(1000);
                        if(!html.Contains("验证码校验失败") || time>5)
                        {
                            textBox5.Text += html +"  次数过多"+ "\r\n";
                            break;
                        }

                    }
                    yzmmsg = html;

                    msg.status = html;
                    msg.Tag = "end";
                    doSendMsg(msg);

                }
                catch (Exception ex)
                {

                    msg.Tag = "end";
                    msg.status = ex.ToString();
                }


            }
            catch (Exception ex)
            {

                msg.Tag = "end";
                msg.status = ex.Message;
            }


        }







        private void SendMsgHander(DownMsg msg)
        {

            this.Invoke((MethodInvoker)delegate ()
            {
               MatchCollection msgs = Regex.Matches(msg.status, @"""msg"":""([\s\S]*?)""");

                try
                {
                    listView1.Items[msg.Id].SubItems[2].Text = msgs[msgs.Count - 1].Groups[1].Value.Trim();
                }
                catch (Exception)
                {

                    listView1.Items[msg.Id].SubItems[2].Text = msg.status;
                }

             

                this.listView1.Items[msg.Id].EnsureVisible();
                Application.DoEvents();
            });

        }

        public delegate void dlgSendMsg(DownMsg msg);
        public event dlgSendMsg doSendMsg;


        public class DownMsg
        {
            public int Id;
            public string Tag;
            public string status;

        }

        private void Change(DownMsg msg)
        {
            //按下停止键
            if (status == false)
                return;

            if (msg.Tag == "end")
            {
                StartDown(1);
            }
        }


        List<Thread> list = new List<Thread>();

        bool status = true;



       
        private void 发票有奖查询_Load(object sender, EventArgs e)
        {
            yzmusername = textBox3.Text.Trim();
            yzmpassword = textBox4.Text.Trim();
            cookie = method.getSetCookie("https://yjfp.guangdong.chinatax.gov.cn:8453/yzm/get.do");
          
            doSendMsg += Change;
            doSendMsg += SendMsgHander;//下载过程处理事件
        }

        private void 发票有奖查询_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }
    }
}
