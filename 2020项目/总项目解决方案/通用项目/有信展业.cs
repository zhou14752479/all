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

namespace 通用项目
{
    public partial class 有信展业 : Form
    {
        public 有信展业()
        {
            InitializeComponent();
        }
        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData,string charset)
        {
            
            try
            {
               // System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                 request.ContentType = "application/x-www-form-urlencoded";  
                request.ContentLength = postData.Length;
                request.KeepAlive = true;
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("platform:E05-BEE-CLIENT-WEB");
                headers.Add("sign:ec399b33be3de5f73d1519d0be47a15e");
                headers.Add("userKey:9F0C062CBE259C67B5205C5982005D79052C1173D499B59E");
                headers.Add("userToken:555861F3A6C5FD6D15E39376E8183205121785CDD49A4B1F51E30861D33512B32DD90B67552CA236@");
                //添加头部
                request.UserAgent = "YXZhanYe/2.4.4 (iPhone; iOS 12.3.1; Scale/3.00)";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈    
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return html;
            }
            catch (Exception ex)
            {

                return ex.ToString();

            }

        }

        #endregion


        public void run()
        {
            try
            {
                    
                        string url = "https://beeweb.youxinsign.com:16081/e05-bee-client/youka/user-flow-order/selectRobedList";
                        string postdata = "handleStatus=100&pageNum=2&pageSize=20";
                       

                    string html = PostUrl(url, postdata, "utf-8");
                     textBox1.Text = html;
                     //Match key = Regex.Match(html, @"""balance"":([\s\S]*?)\}");
                     //if (key.Groups[1].Value == "")
                     //{
                     //    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                     //    lv1.SubItems.Add(value[0]);
                     //    lv1.SubItems.Add(value[1]);
                     //    lv1.SubItems.Add("inv");
                     //}



                     Thread.Sleep(1000);
                    
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
