using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace 搜图匹配
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static string COOKIE = "";

        

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
                string outStr = "";
                string tmpStr = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
               
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                request.KeepAlive = true;
               
                // request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip");
                // request.Headers.Set(HttpRequestHeader.AcceptLanguage, "zh-cn,zh,en");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
               // request.Accept = "*/*";
               
                try
                {//循环获取
                    while ((tmpStr = reader.ReadLine()) != null)
                    {
                        outStr += tmpStr;
                    }
                }
                catch
                {

                }
                reader.Close();
                response.Close();
                return outStr;



            }
            catch (System.Exception ex)
            {
              ex.ToString();

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
        public static string PostUrl(string url, string postData, string COOKIE, string charset)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                // request.ContentType = "application/x-www-form-urlencoded";

                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("appid:orders");
                //headers.Add("x-nike-visitid:5");
                //headers.Add("x-nike-visitorid:d03393ee-e42c-463e-9235-3ca0491475b4");
                //添加头部
                request.ContentType = "application/json";
                request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = "https://web.duanmatong.cn/";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

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


        

        public static string APIKey = "4r4r9Y9GMNL9tqvOQlpoOmBt";
        public static string SecretKey = "92SwzvYSrwodcuy3fmP779kvD62DsMmg";
        public static string access_token = "";
        public string domain;
        public string token = "";
        /// <summary>
        /// 获取accesstoken
        /// </summary>
        /// <returns></returns>
        public static String getAccessToken()
        {
            String authHost = "https://aip.baidubce.com/oauth/2.0/token";
            HttpClient client = new HttpClient();
            List<KeyValuePair<String, String>> paraList = new List<KeyValuePair<string, string>>();
            paraList.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            paraList.Add(new KeyValuePair<string, string>("client_id", APIKey));
            paraList.Add(new KeyValuePair<string, string>("client_secret", SecretKey));

            HttpResponseMessage response = client.PostAsync(authHost, new FormUrlEncodedContent(paraList)).Result;
            String result = response.Content.ReadAsStringAsync().Result;
            Match AccessToken = Regex.Match(result, @"""access_token"":""([\s\S]*?)""");
            return AccessToken.Groups[1].Value;
        }
    

        #region  网络图片转Bitmap
        public static Bitmap UrlToBitmap(string url)
        {

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.Accept = "*/*";
                request.Referer = url;
                request.KeepAlive = true;
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "zh-cn,zh,en");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK)
                    return null;
                Stream resStream = response.GetResponseStream();
                Bitmap bmp = new Bitmap(resStream);
                response.Close();
                request.Abort();
                return bmp;
            }
            catch (Exception ex)
            {

               //MessageBox.Show(ex.ToString());
               return null;
            }
        }
        #endregion

        #region  bitmap转base64
        public static string BitmapToBase64(Bitmap bmp)
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
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region unicode转中文
        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }

        #endregion;

        public static System.Text.Encoding GetTxtType(string FILE_NAME)
        {
            FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read);
            Encoding r = GetType(fs);
            fs.Close();
            return r;
        }
        public static System.Text.Encoding GetType(FileStream fs)
        {
            byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
            byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
            byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM 
            Encoding reVal = Encoding.Default;

            BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
            int i;
            int.TryParse(fs.Length.ToString(), out i);
            byte[] ss = r.ReadBytes(i);
            if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
            {
                reVal = Encoding.UTF8;
            }
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = Encoding.Unicode;
            }
            r.Close();
            return reVal;

        }
        /// <summary> 
        /// 判断是否是不带 BOM 的 UTF8 格式 
        /// </summary> 
        /// <param name=“data“></param> 
        /// <returns></returns> 
        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1; //计算当前正分析的字符应还有的字节数 
            byte curByte; //当前分析的字节. 
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前 
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X 
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1 
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("非预期的byte格式");
            }
            return true;
        }
    


        /// <summary>
        /// 获取源图片
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
    public string getSourcepic(string key)
        {


            string url = "https://"+domain+"/search?q="+key;
            string html = GetUrl(url);

            Match pic = Regex.Match(html, @"<img class=""list-view-item__image"" src=""([\s\S]*?)""");
            Match price = Regex.Match(html, @"price-item--sale"">([\s\S]*?)</span>");
            if (pic.Groups[1].Value != "" && price.Groups[1].Value != "")
            {
                return "https:"+pic.Groups[1].Value.Replace("_95x95","")+"#"+ price.Groups[1].Value.Trim();
            }
            else
            {
                return "";
            }
       

        }


        /// <summary>
        /// 谷歌搜索
        /// </summary>
        /// <returns></returns>
        public ArrayList google(string keyword)
        {

            try
            {
                //string keyword = "Masked+Santa+Ornament+2020+For+Christmas+Tree";
               
                ArrayList lists = new ArrayList();
                string url = "https://www.google.com/search?q=" + keyword.Trim() + "&source=lnms&tbm=isch&sa=X&ved=2ahUKEwiZ3Mr0-IbuAhXRfXAKHQyuAP4Q_AUoAnoECA8QBA&biw=1920&bih=936";
               // textBox1.Text = url;
                string html = GetUrl(url);
               // textBox3.Text = html;
                MatchCollection pics = Regex.Matches(html, @"<img class=""VzLYRc"" data-src=""([\s\S]*?)""");
                MatchCollection companys = Regex.Matches(html, @"<div class=""kaNpvd nLaBQd X7oSNe""><span aria-label=""by([\s\S]*?)""");
                MatchCollection prices = Regex.Matches(html, @"<div class=""t3Ss7 hLMyCc"">([\s\S]*?)</div>");
                for (int i = 0; i < pics.Count; i++)
                {

                    lists.Add(pics[i].Groups[1].Value + "*" + companys[i].Groups[1].Value + "*" + prices[i].Groups[1].Value);

                }

                return lists;
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.ToString());
                return null;
            }
        }


        /// <summary>
        /// lazada搜索
        /// </summary>
        /// <returns></returns>
        public ArrayList lazada(string keyword)
        {

            try
            {
                //string keyword = "Masked+Santa+Ornament+2020+For+Christmas+Tree";

                ArrayList lists = new ArrayList();
                string url = "https://www.lazada.com.my/catalog/?q="+keyword+"&_keyori=ss&from=input&spm=a2o4k.home.search.go.75f82e7eVKyf52";
                // textBox1.Text = url;
                string html = GetUrl(url);
                // textBox3.Text = html;
                MatchCollection pics = Regex.Matches(html, @"\[{""image"":""([\s\S]*?)""");
              
                MatchCollection prices = Regex.Matches(html, @"""price"":""([\s\S]*?)""");
                for (int i = 0; i < pics.Count; i++)
                {

                    lists.Add(pics[i].Groups[1].Value + "*" +"lazada" + "*" + prices[i].Groups[1].Value);

                }

                return lists;
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.ToString());
                return null;
            }
        }



        // 商品检索—入库
        public  string productAdd(string brief, string url)
        {
          
            string host = "https://aip.baidubce.com/rest/2.0/image-classify/v1/realtime_search/product/add?access_token=" + token;
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.KeepAlive = true;
            string base64 = BitmapToBase64(UrlToBitmap(url));
            String str = "brief="+ brief + "&image=" + HttpUtility.UrlEncode(base64);
            //String str = "brief=" + brief + "&url=" +base64;
            byte[] buffer = encoding.GetBytes(str);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string result = reader.ReadToEnd();
            //textBox3.Text+=("商品检索—入库:")+"\r\n";
            //textBox3.Text += result + "\r\n";
            return result;
        }

        // 商品检索—检索
        public  string productSearch(string url)
        {
           
            string host = "https://aip.baidubce.com/rest/2.0/image-classify/v1/realtime_search/product/search?access_token=" + token;
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.KeepAlive = true;
            // 图片的base64编码
            string base64 = BitmapToBase64(UrlToBitmap(url));
            String str = "image=" + HttpUtility.UrlEncode(base64);
            byte[] buffer = encoding.GetBytes(str);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string result = reader.ReadToEnd();
            //textBox3.Text += ("商品检索—搜索:") + "\r\n";
            //textBox3.Text += result + "\r\n";
            return result;
        }


        // 商品检索—删除
        public string productDelete(string url)
        {
            
            string host = "https://aip.baidubce.com/rest/2.0/image-classify/v1/realtime_search/product/delete?access_token=" + token;
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.KeepAlive = true;
            string base64 = BitmapToBase64(UrlToBitmap(url));
            String str = "image=" + HttpUtility.UrlEncode(base64);
            byte[] buffer = encoding.GetBytes(str);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string result = reader.ReadToEnd();
            Console.WriteLine("商品检索—删除:");
            Console.WriteLine(result);
            return result;
        }

        ArrayList imagelists = new ArrayList();



        /// <summary>
        /// 入库
        /// </summary>
        public void ruku()
        {
            textBox3.Text += "正在入库" + "\r\n";
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入主域名");
                return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("请导入csv");
                return;
            }
            textBox3.Text += ("程序已启动:") + "\r\n";
            domain = textBox1.Text.Trim();
            token = getAccessToken();

            StreamReader sr = new StreamReader(textBox2.Text, GetTxtType(textBox2.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();

            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int i = 1; i < text.Length; i++)
            {
                string keyword = text[i].Replace(" ", "+");
                ArrayList picsList = new ArrayList();
                if (radioButton1.Checked == true || radioButton2.Checked == true)
                {
                    picsList = google(keyword);
                }
               if(radioButton3.Checked==true)

                {
                    picsList = lazada(keyword);
                }
                textBox3.Text += ("获取搜索图片成功:")+ "\r\n";
                foreach (string item in picsList)
                {
                    try
                    {
                        string[] value = item.Split(new string[] { "*" }, StringSplitOptions.None);

                        string picurl = value[0];
                        string company = value[1];
                        string price = value[2];

                       
                        string brief =   price+"#"+company+"#"+picurl;
                       
                        textBox3.Text += ("正在入库图片序号:") +picurl+ "\r\n";

                        string result = productAdd(brief, picurl);
                      
                        if (result.Contains("cont_sign"))
                        {
                            textBox3.Text += ("入库智能云成功:") + brief + "\r\n";
                            imagelists.Add(picurl);
                        }

                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(keyword);
                        lv1.SubItems.Add("");
                        lv1.SubItems.Add("");
                        lv1.SubItems.Add(domain);
                        lv1.SubItems.Add("");
                        lv1.SubItems.Add(value[1]);
                        lv1.SubItems.Add(value[0]);
                        lv1.SubItems.Add("");
                        lv1.SubItems.Add(value[2]);
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;

                    }
                    catch (Exception)
                    {

                        continue;
                    }
                }


                
            }

            textBox3.Text = "入库结束";
        }


   
        /// <summary>
        /// 查找
        /// </summary>
        public void chazhao()
        {
            textBox3.Text += "正在查找"+"\r\n";
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入主域名");
                return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("请导入csv");
                return;
            }
            textBox3.Text += ("程序已启动:") + "\r\n";
            domain = textBox1.Text.Trim();
            token = getAccessToken();

            StreamReader sr = new StreamReader(textBox2.Text, GetTxtType(textBox2.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();

            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int i = 1; i < text.Length; i++)
            {
                string keyword = text[i].Replace(" ", "+");
                string[] sourcevalue = getSourcepic(keyword).Split(new string[] { "#" }, StringSplitOptions.None);
               


                
                if (sourcevalue.Length > 1)
                {
                    string sourcePicUrl = sourcevalue[0];
                    string sourceprice = sourcevalue[1];
                    textBox3.Text += ("正在搜索图片序号:") + sourcePicUrl + "\r\n";
                    string result = productSearch(sourcePicUrl);

                    if (result.Contains("brief"))
                    {
                        MatchCollection briefs = Regex.Matches(result, @"brief"": ""([\s\S]*?)""");
                        MatchCollection scores = Regex.Matches(result, @"score"":([\s\S]*?),");


                        int count = 0;
                        int max = 0;
                        if (briefs.Count > 50)
                        {
                            max = 50;
                        }
                        else
                        {
                            max = briefs.Count;
                        }

                        for (int z = 0; z < max; z++)
                        {
                            if (Convert.ToDouble(scores[z].Groups[1].Value) > 0.69)

                            {
                                try
                                {
                                    string[] value = briefs[z].Groups[1].Value.Split(new string[] { "#" }, StringSplitOptions.None);
                                    if (value.Length > 1)
                                    {
                                        count = count + 1;
                                    }
                                }
                                catch (Exception)
                                {

                                    continue;
                                }
                            }
                        }
                        if (count == 0)
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(keyword.Replace("+", " "));
                            lv1.SubItems.Add(count.ToString());
                            lv1.SubItems.Add(sourcePicUrl);
                            lv1.SubItems.Add(domain);
                            lv1.SubItems.Add(sourceprice);
                            lv1.SubItems.Add(" ");
                            lv1.SubItems.Add(" ");
                            lv1.SubItems.Add(" ");
                            lv1.SubItems.Add(" ");
                        }

                        else
                        {

                            for (int j = 0; j < max; j++)
                            {
                                if (Convert.ToDouble(scores[j].Groups[1].Value) > 0.69)
                                {
                                    try
                                    {
                                        string[] value = briefs[j].Groups[1].Value.Split(new string[] { "#" }, StringSplitOptions.None);
                                        if (value.Length > 1)
                                        {
                                            textBox3.Text += ("搜索成功:") + briefs[j].Groups[1].Value + "\r\n";

                                            if (j == 0)
                                            {
                                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                                lv1.SubItems.Add(keyword.Replace("+", " "));
                                                lv1.SubItems.Add(count.ToString());
                                                lv1.SubItems.Add(sourcePicUrl);
                                                lv1.SubItems.Add(domain);
                                                lv1.SubItems.Add(sourceprice);
                                                lv1.SubItems.Add(value[1]);
                                                lv1.SubItems.Add(value[2]);
                                                lv1.SubItems.Add(scores[j].Groups[1].Value);
                                                lv1.SubItems.Add(value[0]);
                                            }
                                            else
                                            {
                                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                                lv1.SubItems.Add(" ");
                                                lv1.SubItems.Add(" ");
                                                lv1.SubItems.Add(" ");
                                                lv1.SubItems.Add(" ");
                                                lv1.SubItems.Add(" ");
                                                lv1.SubItems.Add(value[1]);
                                                lv1.SubItems.Add(value[2]);
                                                lv1.SubItems.Add(scores[j].Groups[1].Value);
                                                lv1.SubItems.Add(value[0]);

                                            }

                                        }
                                    }
                                    catch (Exception)
                                    {

                                        continue;
                                    }
                                }
                                else
                                {
                                    textBox3.Text += ("图像匹配分值未达到设置值") + "\r\n";

                                }
                            }
                        }



                    }
                    else
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(keyword.Replace("+", " "));
                        lv1.SubItems.Add("0");
                        lv1.SubItems.Add(sourcePicUrl);
                        lv1.SubItems.Add(domain);
                        lv1.SubItems.Add("空");
                        lv1.SubItems.Add(" ");
                        lv1.SubItems.Add(" ");
                        lv1.SubItems.Add(" ");
                        lv1.SubItems.Add(" ");
                    }
   

                }
                else
                {
                    textBox3.Text += ("源图片为空跳过:") +  "\r\n";
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(keyword.Replace("+", " "));
                    lv1.SubItems.Add("0");
                    lv1.SubItems.Add("空");
                    lv1.SubItems.Add(domain);
                    lv1.SubItems.Add("空");
                    lv1.SubItems.Add(" ");
                    lv1.SubItems.Add(" ");
                    lv1.SubItems.Add(" ");
                    lv1.SubItems.Add(" ");
                }



                while (this.zanting == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }
                if (status == false)
                    return;




            }

            MessageBox.Show("搜索结束");
        }




        Thread thread;
    bool zanting = true;
    bool status = true;

        #region  listView导出CSV
        /// <summary>
        /// 导出CSV
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="includeHidden"></param>
        /// 
        public static void ListViewToCSV(ListView listView, bool includeHidden)
        {
            //make header string
            SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "xlsx|*.xls|xlsx|*.xlsx";

            //sfd.Title = "Excel文件导出";
            string filePath = "";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                filePath = sfd.FileName + ".csv";
            }
            StringBuilder result = new StringBuilder();
            WriteCSVRow(result, listView.Columns.Count, i => includeHidden || listView.Columns[i].Width > 0, i => listView.Columns[i].Text);

            //export data rows
            foreach (ListViewItem listItem in listView.Items)
                WriteCSVRow(result, listView.Columns.Count, i => includeHidden || listView.Columns[i].Width > 0, i => listItem.SubItems[i].Text);

            File.WriteAllText(filePath, result.ToString(),Encoding.Default);
            MessageBox.Show("导出成功");
        }


        private static void WriteCSVRow(StringBuilder result, int itemsCount, Func<int, bool> isColumnNeeded, Func<int, string> columnValue)
        {
            bool isFirstTime = true;
            for (int i = 0; i < itemsCount; i++)
            {
                try
                {

                    if (!isColumnNeeded(i))
                        continue;

                    if (!isFirstTime)
                        result.Append(",");
                    isFirstTime = false;

                    result.Append(String.Format("\"{0}\"", columnValue(i)));
                }
                catch
                {
                    continue;
                }
            }

            result.AppendLine();
        }

        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            ListViewToCSV(listView1, true);
        }

        private void button6_Click(object sender, EventArgs e)
        {

            status = true;
            if (thread == null || !thread.IsAlive)
            {
               
                thread = new Thread(ruku);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            timer1.Start();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {
                zanting = true;
                button1.Text = "暂停";
            }
            else
            {
                zanting = false;
                button1.Text = "继续";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox2.Text = this.openFileDialog1.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
            status = false;
            textBox3.Text = "已停止";
        }

        public void delete()
        {
            token = getAccessToken();
            foreach (string item in imagelists)
            {
                textBox3.Text = "正在删除："+productDelete(item);

            }

            textBox3.Text = "全部删除成功";
            listView1.Items.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
          Thread thread = new Thread(delete);
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        long GetBkn(string skey)
        {
            var hash = 5381;
            for (int i = 0, len = skey.Length; i < len; ++i)
                hash += (hash << 5) + (int)skey[i];
            return hash & 2147483647;
        }
        private void button7_Click(object sender, EventArgs e)
        {
          //  MessageBox.Show(GetBkn("@vfY6NdjCv").ToString());
            status = true;
            listView1.Items.Clear();
            if (thread == null || !thread.IsAlive)
            {

                thread = new Thread(chazhao);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
              
                textBox3.Text = listView1.SelectedItems[0].SubItems[4].Text;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(thread == null || !thread.IsAlive)
            {
                listView1.Items.Clear();
                thread = new Thread(chazhao);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
                timer1.Stop();
            }

        }
    }
}
