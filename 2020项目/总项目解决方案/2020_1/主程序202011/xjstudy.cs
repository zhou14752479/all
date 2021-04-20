using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序202011
{
    class xjstudy
    {
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public string GetUrl(string Url)
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
              return( ex.ToString());

            }
           
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
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
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


        public delegate void GetLogs(string log);
        public event GetLogs getlogs;


        public string getcode(string credential)
        {
            string code = GetMD5(credential + "Xj@Jx#Jy$123");
            return code;

        }


        public string credential = "";
        public string username = "";

        public void newt()
        {
            Thread t = new Thread(run);
            t.Start();
        }

        public void run()
        {
            string code = getcode(credential);

            for (int year = 2014; year <= 2021; year++)
            {


                string url = "https://yun.xjrsjxjy.com/API/GetStudyCoursewares.ashx?code=" + code + "&credential=" + credential + "&year=" + year + "&isFree=false";

                string html = GetUrl(url);
                if (html.Contains("登录超时"))
                {
                    MessageBox.Show("账号已掉线");
                    
                }
                MatchCollection ids = Regex.Matches(html, @"""Id"":""([\s\S]*?)""");
                MatchCollection names = Regex.Matches(html, @"""Name"":""([\s\S]*?)""");
                MatchCollection Progress = Regex.Matches(html, @"""LearningProgress"":([\s\S]*?),");



                for (int j = 0; j < ids.Count; j++)
                {
                   
                    string coursewareId = ids[j].Groups[1].Value.ToString();
                    string aurl = "https://yun.xjrsjxjy.com/API/GetChapters.ashx?code=" + code + "&credential=" + credential + "&year=" + year + "&coursewareId=" + coursewareId;
                   
                    string ahtml = GetUrl(aurl);
                   
                   MatchCollection aids = Regex.Matches(ahtml, @"""Id"":""([\s\S]*?)"",([\s\S]*?)""TotalSeconds"":([\s\S]*?)\}");
                    MatchCollection LearningProgress = Regex.Matches(ahtml, @"""LearningProgress"":([\s\S]*?),");


                    for (int z = 0; z < aids.Count; z++)
                    {
                        string jindu = (Convert.ToDouble(LearningProgress[z].Groups[1].Value) * 100).ToString() + "%";

                        getlogs(username + "：" + names[j].Groups[1].Value.ToString() + jindu);
                        if (LearningProgress[z].Groups[1].Value != "1")
                        {
                            
                            string o = year + "#" + coursewareId + "#" + aids[z].Groups[1].Value + "#" + aids[z].Groups[3].Value;
                            study((object)o);
                           
                        }

                    }

                }

            }

            getlogs(username + "  ：全部学习结束");
        }


        public void study(object parame)
        {

            string code = getcode(credential);
            string[] text = parame.ToString().Split(new string[] { "#" }, StringSplitOptions.None);
            string year = text[0];
            string coursewareId = text[1];
            string chapterId = text[2];

            int totalseconds = (Convert.ToInt32(Convert.ToDouble(text[3]))) + 150;

            string url = "https://yun.xjrsjxjy.com/API/SaveLearningProgress.ashx";
            string postdata = "code=" + code + "&credential=" + credential + "&year=" + year + "&coursewareId=" + coursewareId + "&chapterId=" + chapterId + "&type=0&progress=0";

            string html = PostUrl(url, postdata);
            for (int i = 1; i < totalseconds; i++)
            {
                string postdata1 = "code=" + code + "&credential=" + credential + "&year=" + year + "&coursewareId=" + coursewareId + "&chapterId=" + chapterId + "&type=1&progress=" + i;

                if (i % 60 == 0)
                {
                    PostUrl(url, postdata1);
                    getlogs("【"+username + "】正在学习...当前课程进度："+i);

                }
                Thread.Sleep(1011);
            }

        }








    }
}
