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
using helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace 模拟采集1
{
    public partial class 企查查 : Form
    {
        public 企查查()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

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
        public static string PostUrl(string url, string postData)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
              
                 request.ContentType = "application/json";
               // request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", cookie);

                request.Referer = "https://amr.sz.gov.cn/aicmerout/jsp/gcloud/giapout/industry/aicmer/processpage/step_one.jsp?ywType=30";
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


        public string getyzm(byte[] VImage)
        {
            // 识别

            // 图鉴平台的账号，使用时请更换为自己的
            string UserName = textBox1.Text.Trim();
            // 图鉴平台账号对应的密码，使用时请更换为自己的
            string Password = textBox2.Text.Trim();
            // 16:汉字 
            // 14:图片旋转
            // 11:计算题 
            // 7:无感学习
            // 4:闪动GIF
            // 3:数英混合
            // 2:纯英文
            // 1:纯数字.
            string TypeId = "3";
            // 超时等待，单位：秒
            string TimeOut = "60";
            // 待识别图片的二进制流，需要自行获取带识别图片的二进制流，本处设置为 null 仅为演示使用，实际场景需要获取
           // byte[] VImage = null;

            var param = new Dictionary<object, object>{
                {"username",UserName},
                {"password",Password},
                {"typeid",TypeId},
                {"timeout",TimeOut}
            };

            string PostResult = TTHttp.Post("http://api.ttshitu.com/create.json", param, VImage);

            //json
            string joSuccess = "";
            string joMessage = "";
            //
            string joId = "";
            string joResult = "";
            JObject jo = null;

            try
            {
                jo = (JObject)JsonConvert.DeserializeObject(PostResult);

                // 识别结果
                joSuccess = jo["success"].ToString().ToLower();
                // 关联提示
                joMessage = jo["message"].ToString();

            }
            catch { }
            //识别成功
            if (joSuccess == "true")
            {
                //平台返回的图片ID
                joId = jo["data"]["id"].ToString();
                // 平台返回的识别结果
                joResult = jo["data"]["result"].ToString();


                // 可以按需要继续写入您的业务逻辑
            }
            else if (joSuccess == "false")
            {
                // 识别失败

                // 可以按需要继续写入您的业务逻辑
            }
            else
            {
                // 未知异常，一般不会出现
            }

            return joResult;
        }
        /// <summary>
        /// 获取key
        /// </summary>
        /// <returns></returns>
        public string getKey(string code,string name,string yzm)
        {
            string postdata = "{\"params\":{\"javaClass\":\"ParameterSet\",\"map\":{\"bgContentType\":\"\",\"ywtype\":\"30\",\"regno\":\"91440300MA5FWE0L46\",\"yzymType\":\"2\",\"entName\":\"深圳南山区宝诚汽车销售服务有限公司\",\"isDivOrMer\":\"0\",\"fddbrName\":\"\",\"fddbrCertNo\":\"\",\"fddbrPhoneNum\":\"\",\"fddbrPhoneValidCode\":\"\",\"isSecond\":\"0\",\"validCode\":\""+yzm+"\"},\"length\":13},\"context\":{\"javaClass\":\"HashMap\",\"map\":{},\"length\":0}}";
            string url = "https://amr.sz.gov.cn/aicmerout/command/ajax/com.inspur.gcloud.giapbase.aicmer.qydj.cmd.QydjUrlCxCmd/queryOtherUrl";
           string html= PostUrl(url,postdata);
            return html;
        }

        /// <summary>
        /// 获取No
        /// </summary>
        /// <returns></returns>
        public string getNo(string key)
        {
            string postdata = "{\"params\":{\"javaClass\":\"ParameterSet\",\"map\":{\"formId\":\"SCZT_NZGS_BG_NW\",\"primaryKey\":\""+key+"\",\"ywType\":\"30\"},\"length\":3},\"context\":{\"javaClass\":\"HashMap\",\"map\":{},\"length\":0}}";
            string url = "https://amr.sz.gov.cn/aicmerout/command/ajax/com.inspur.gcloud.giapbase.aicmer.formmgr.datamgr.cmd.AicmerBaseFormDataInitCmd/loadData";
            string html = PostUrl(url, postdata);
            return html;
        }







        public string html = "";
        bool status = false;

        private void 企查查_Load(object sender, EventArgs e)
        {
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("https://www.qcc.com/user_login");
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WB_DocumentCompleted);
        }

        private void WB_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                return;
            if (e.Url.ToString() != webBrowser1.Url.ToString())
                return;

            if (webBrowser1.DocumentText.Contains("</html>"))
            {

                html = webBrowser1.Document.Body.OuterHtml;
                run();
              
                status = true;
            }
            else
            {
                Application.DoEvents();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string url = comboBox1.Text.Trim();

            webBrowser1.Navigate(url);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        public void run()
        {

            MatchCollection names = Regex.Matches(html, @"addSearchIndex([\s\S]*?)',([\s\S]*?),'([\s\S]*?)'");
            for (int i = 0; i < names.Count; i++)
            {
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据   
                lv1.SubItems.Add(System.Web.HttpUtility.UrlDecode(names[i].Groups[3].ToString()).Replace("<em>", "").Replace("</em>", ""));
            }

        }


        public static string cookie;

        
        private void button1_Click(object sender, EventArgs e)
        {
            cookie = "__jsluid_s=7a1ae6ed3fe23ef03c6dc5c7d5ab7a9b; __jsluid_h=1006c695331ed4f6e246187cfc776caf; pgv_pvi=6021011456; Hm_lvt_f89f708d1e989e02c93927bcee99fb29=1599550357; sangfor_cookie=28150568; isCaUser=true; isSameUser=C8938041764A631C1A211195D89326F0F39FD20BD5C4941F777554BC3C98E2A500AB1E71BDB7F79F; psout_sso_token=9QL-g2YRAbyxoos2D0Oxtcm; gdbsTokenId=AQIC5wM2LY4SfczFGIdJtLKIihCuBp-yuUWcQ4eF8xLlff8.*AAJTSQACMDIAAlNLABQtNjc1MTczMDA2MDE5MjI3MTc5OQ..*@node5; accessToken=88b222c0-0748-4f62-96e1-db9fc2882d39@node5; userType=1; ishelp=false; insert_cookie=19819662; JSESSIONID=0000MQH-mTykR7ugwYbQWEAe9f6:-1";
            //for (int i = 1; i < 251; i++)
            //{


            //    status = false;
            //    webBrowser1.Navigate("https://www.qcc.com/search?key=%E5%8D%97%E5%B1%B1%E5%8C%BA#p:"+i+"&");
            //    webBrowser1.Navigate("https://www.qcc.com/search?key=%E5%8D%97%E5%B1%B1%E5%8C%BA#p:" + i + "&");


            //    while (status == false)
            //    {
            //        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
            //    }
            //    Thread.Sleep(1000);
            //}
            //MessageBox.Show("抓取结束");

            byte[] imgbyte = method.Getbyte("https://amr.sz.gov.cn/aicmerout/validImageServlet", cookie);

            string yzm = getyzm(imgbyte);
            string keyhtml = getKey("", "", yzm.Trim());
            MessageBox.Show(keyhtml);
            Match key = Regex.Match(keyhtml, @"""id"":""([\s\S]*?)"""); ;
            MessageBox.Show(key.Groups[1].Value);
            textBox1.Text = getNo(key.Groups[1].Value);

        }


    }
}
