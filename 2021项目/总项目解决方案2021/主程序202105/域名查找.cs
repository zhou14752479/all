using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序202105
{
    public partial class 域名查找 : Form
    {
        public 域名查找()
        {
            InitializeComponent();
        }

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url)
        {


            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
              
                request.AllowAutoRedirect = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 10;
                request.KeepAlive = false;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    response.Close();
                    return "1";
                   
                }
                else
                {
                    response.Close();
                    return "";
                   
                }
              
              



            }
            catch (System.Exception ex)
            {
                ex.ToString();

            }
            return "";
        }
        #endregion

        string path = AppDomain.CurrentDomain.BaseDirectory;
        string[] zimus = {"a","b","c","d","e","f","g","h","i","j","k","L","m","n","o","p","q","r","s","t","u","v","w","x","y","z" };

    

        public void run()
        {
            for (int a = 0; a < 26; a++)
            {
                for (int b = 0; b < 26; b++)
                {
                    for (int c = 0; c < 26; c++)
                    {
                        for (int d = 0; d < 26; d++)
                        {
                            for (int e = 0; e < 26; e++)
                            {
                             
                                    string url1 = "http://www." + zimus[a] + zimus[b] + zimus[c] + zimus[d] + zimus[e] + ".com";

                                    label1.Text = url1;
                                    string html1 = GetUrl(url1);
                                   
                                    if (html1 != "")
                                    {

                                        FileStream fs1 = new FileStream(path + DateTime.Now.ToString("yyyy-MM-dd") + ".txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                                        StreamWriter sw = new StreamWriter(fs1);
                                        sw.WriteLine(url1 + "\r\n");

                                        sw.Close();
                                        sw.Dispose();
                                        fs1.Close();
                                       

                                    }
                                
                                


                            }
                        }
                    }
                }
            }
           


        }
        private void button6_Click(object sender, EventArgs e)
        {
           
         
            Thread  thread = new Thread(run);
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

      
    }
}
