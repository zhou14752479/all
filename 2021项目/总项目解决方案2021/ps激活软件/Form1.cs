using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace ps激活软件
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region  获取32位MD5加密
        public string GetMD5(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        #endregion
        #region base64加密
        public static string Base64Encode(Encoding encodeType, string source)
        {
            string encode = string.Empty;
            byte[] bytes = encodeType.GetBytes(source);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }
            return encode;
        }
        #endregion


        public void DownloadFile(string URL, string filename, System.Windows.Forms.ProgressBar prog, System.Windows.Forms.Label label1)

        {

            float percent = 0;

            try

            {

                System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(URL);

                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();

                long totalBytes = myrp.ContentLength;

                if (prog != null)

                {

                    prog.Maximum = (int)totalBytes;

                }

                System.IO.Stream st = myrp.GetResponseStream();

                System.IO.Stream so = new System.IO.FileStream(filename, System.IO.FileMode.Create);

                long totalDownloadedByte = 0;

                byte[] by = new byte[1024];

                int osize = st.Read(by, 0, (int)by.Length);

                while (osize > 0)

                {

                    totalDownloadedByte = osize + totalDownloadedByte;

                    System.Windows.Forms.Application.DoEvents();

                    so.Write(by, 0, osize);

                    if (prog != null)

                    {

                        prog.Value = (int)totalDownloadedByte;

                    }
                    osize = st.Read(by, 0, (int)by.Length);
                    percent = (float)totalDownloadedByte / (float)totalBytes * 100;
                    label1.Text = "进度条下载：" + percent.ToString() + "%";

                    System.Windows.Forms.Application.DoEvents();

                    //必须加注这句代码，否则label1将因为循环执行太快而来不及显示信息      

                }

                so.Close();
                st.Close();
            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }
        #region base64解密
        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="encodeType">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(Encoding encodeType, string result)
        {
            string decode = string.Empty;
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encodeType.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }
        #endregion
        string path = AppDomain.CurrentDomain.BaseDirectory;

        #region 获取时间戳  秒
        /// <summary>
        /// 获取时间戳  秒
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalSeconds);
            return a.ToString();
        }
        #endregion
        #region 激活码

        public void jihuo()
        {
            try
            {
                string user = textBox1.Text.Trim();
                string pass = textBox2.Text.Trim();
                if(user=="")
                {
                    MessageBox.Show("激活账户为空");
                    return;
                }
               else if (pass == "")
                {
                    MessageBox.Show("激活序列号为空");
                    return;
                }

                string str = Base64Decode(Encoding.Default, textBox1.Text);
                string time = str.Remove(str.Length-1,1);
                if (Convert.ToDateTime(time).AddMinutes(5)>DateTime.Now)  //5分钟内有效
                {
                    if(textBox2.Text==GetMD5(textBox1.Text+"siyiruanjian"))
                    {
                        MessageBox.Show("激活成功..开始安装");
                        label4.Visible = true;
                        progressBar1.Visible = true;

                        string fileurl = "http://acaiji.com/ps/file/ps_cs6.exe";
                        DownloadFile(fileurl, path+"/download/ps_cs6.exe",progressBar1,label4);
                        //安装软件
                        System.Diagnostics.Process.Start(path + "/download/ps_cs6.exe");
                    }
                    else
                    {
                        MessageBox.Show("激活序号不匹配");
                    }
                }
                else
                {
                    if (textBox2.Text == GetMD5(textBox1.Text + "siyiruanjian"))
                    {
                        MessageBox.Show("激活成功..开始安装");
                        label4.Visible = true;
                        progressBar1.Visible = true;

                        string fileurl = "http://acaiji.com/ps/file/ps_cs6.exe";
                        DownloadFile(fileurl, path + "/download/ps_cs6.exe", progressBar1, label4);
                        //安装软件
                        System.Diagnostics.Process.Start(path + "/download/ps_cs6.exe");
                    }
                    else
                    {
                        MessageBox.Show("激活序号不匹配");
                    }
                    // MessageBox.Show("激活序号已过期");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("激活账户或激活序列号输入错误");
               
              
            }


        }


        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("http://acaiji.com/ps/bj.html");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://acaiji.com/ps/jiaocheng.html");
            }
            catch (Exception)
            {

                System.Diagnostics.Process.Start("explorer.exe", "http://acaiji.com/ps/jiaocheng.html");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
                  
            jihuo();
          
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
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
