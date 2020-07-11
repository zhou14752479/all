using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace http03
{
    public partial class Form1 : Form
    {
        ArrayList list = new ArrayList();
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string url = @"https://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=1&rsv_idx=2&ch=&tn=baiduhome_pg&bar=&wd=C%23+%E5%AD%97%E7%AC%A6%E4%B8%B2%E8%BD%ACCookieCollection&rsv_spt=1&oq=%2526lt%253B%2523%2520%25E5%25AD%2597%25E7%25AC%25A6%25E4%25B8%25B2%25E8%25BD%25ACcookie&rsv_pq=f5d07276000b8840&rsv_t=8e6dxAPw%2B54Q5GWCF1k98T7s068Ctkqr5cJhwWfda0kJ06KTFSPNR%2BQB0h5hxrOtEOVk&rqlang=cn&rsv_enter=0&rsv_btype=t&rsv_dl=tb&inputT=933";

            var response = Program.get(url);

            var html = Program.getText(response);
            ////Console.WriteLine(html);
            //Console.WriteLine(response.Cookies.Count);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string url = "https://www.baidu.com/s?wd=c%23%20string%20%E8%BD%AC%20CookieContainer&pn=40&oq=c%23%20string%20%E8%BD%AC%20CookieContainer&tn=baiduhome_pg&ie=utf-8&rsv_idx=2&rsv_pq=8e8f1230001710e7&rsv_t=e7bc31hqf4PCo2KEJNOy7R%2Bw4Tp8OGRVx9lRUAUyhUivnthtCjAZswfsjAwADQYd77L1&rsv_page=1";
            CookieContainer cookies = new CookieContainer();
            string html = Program.getwithCookies(url, "", ref cookies);
            Console.WriteLine(cookies.GetCookieHeader(new Uri(url)));
        }
        /// <summary>
        /// 主要执行部分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            string s = "手机";
            string start_url = "https://s.taobao.com/search?q=" + s;
            Console.WriteLine(start_url);
            int count = 10;
            for (int i = 0; i < count; i++)
            {
                int p = count * 44;
                string url = start_url + "&s=" + p.ToString();
                string html = getHtmlpages(url);
                textBox1.Text = html;
                getGoodsinfo(html, list);
            }
            printGoodsinfo(list);
            
        }
        /// <summary>
        /// 根据定义请求头header发送get请求，获取到页面的html，其中包含了成功登录的cookie
        /// </summary>
        /// <param name="url"></param>
        /// <returns>返回html</returns>
        public static string getHtmlpages(string url)
        {
            Dictionary<string, string> kv = new Dictionary<string, string>();
            kv.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.186 Safari/537.36");
            //kv.Add("Cookie", "cna=ecr8FaGj4BkCAXQBA/ISif7h; t=74708015a4c8f195ffecf6910094518a; cookie2=1ba81d62075785c1aba0fef97e44a639; v=0; _tb_token_=ee75efeb6a6f3; _samesite_flag_=true; sgcookie=Ee1UjSL5Xy%2FoGCIy2YTej; unb=2567975303; uc3=nk2=BM4UqWtIwrLmo%2FTS&id2=UU20srK2IG68%2FA%3D%3D&lg2=Vq8l%2BKCLz3%2F65A%3D%3D&vt3=F8dBxGXFryZCZN77wes%3D; csg=ded34206; lgc=g812cm%5Cu7684%5Cu73AE%5Cu54E5; cookie17=UU20srK2IG68%2FA%3D%3D; dnk=g812cm%5Cu7684%5Cu73AE%5Cu54E5; skt=ff3aa2e9fdb99acd; existShop=MTU4OTM3NDg0Ng%3D%3D; uc4=id4=0%40U2%2Fz993QMiMBcqer33PZH8r8U2VT&nk4=0%40BsogzKLUdUAKNt4RccUT3lfRsxtfiYY%3D; tracknick=g812cm%5Cu7684%5Cu73AE%5Cu54E5; _cc_=WqG3DMC9EA%3D%3D; _l_g_=Ug%3D%3D; sg=%E5%93%A535; _nk_=g812cm%5Cu7684%5Cu73AE%5Cu54E5; cookie1=BxY5GoxuA9R6Jz%2FjbN3SW2nHhWhRwOZ7xqnSsqpvp6E%3D; enc=ueaGTkz%2FLBTfJlmU57xXHLpBRvG8gMuUQ1vbsr%2FC7%2BznvJM9wz9CcNW9oZJziPT5aGuke9p6l6uOqtAPluKTkg%3D%3D; tfstk=c8UOBww_q9XiuqNZ4rIncloQSHClZc_qddMXkszqEo5AFo8Ainzuyu4R5jvtJ6C..; hng=CN%7Czh-CN%7CCNY%7C156; thw=cn; mt=ci=112_1; uc1=cookie14=UoTUM2M25mx%2F5g%3D%3D&cookie16=UtASsssmPlP%2Ff1IHDsDaPRu%2BPw%3D%3D&existShop=false&cookie21=UtASsssmeW6lpyd%2BB%2B3t&cookie15=W5iHLLyFOGW7aA%3D%3D&pas=0; JSESSIONID=31174B96F0394FF0592B8156FBA4E94D; l=eBEwguePQlV4qScBBOfwPurza77OSIRAguPzaNbMiT5P9Hfp5khhWZbg1u89C3GVh6D9R3ykIQI_BeYBqIv4n5U62j-la_kmn; isg=BObmTB7XhMrekFBu76YpHChkN1xoxyqB1oftY9CP0onkU4ZtOFd6kcwhq09feyKZ");
            kv.Add("Cookie", "t=c9a6e5e62ca2c120600d8dcfaad4b45e; thw=cn; enc=ute7RgjnKNuWOLGvghOzHfWqQ%2Bda%2B8ztZ5sUbEMRoIt6Zyu4hnlnOniJR%2BCmKTF%2BRoN9coNBszSwMaBD4u3evQ%3D%3D; cna=nkhGF1vUIysCASrth2w3e468; v=0; cookie2=706a594478cd359f69d1e12e33b67581; _tb_token_=098750736364; _samesite_flag_=true; mt=ci=7_1; alitrackid=www.taobao.com; lastalitrackid=www.taobao.com; _m_h5_tk=7ea85b73cb3231eac90e9dc8508b4bf8_1594222212753; _m_h5_tk_enc=aebae86eef347f8828d6a6b7b45177a4; sgcookie=EFxUDEjvQ0waWYEYZfNho; uc3=nk2=qAa8HFm95q%2Fd0A%3D%3D&id2=UNX%2FQTb0NzMHoQ%3D%3D&lg2=W5iHLLyFOGW7aA%3D%3D&vt3=F8dBxGJklM302EReif8%3D; csg=57492921; lgc=%5Cu6C34%5Cu4E2D%5Cu7684%5Cu60A0%5Cu9C7C; dnk=%5Cu6C34%5Cu4E2D%5Cu7684%5Cu60A0%5Cu9C7C; skt=ca4d4ea3053d3044; existShop=MTU5NDMwMjM0Mw%3D%3D; uc4=nk4=0%40qj2lm0SW741TWX3CoxPeQKWyNIjT&id4=0%40UgJ6wqPI2HNj3h91yworhlt2facd; tracknick=%5Cu6C34%5Cu4E2D%5Cu7684%5Cu60A0%5Cu9C7C; _cc_=UIHiLt3xSw%3D%3D; tfstk=cIBdIVgqeP4hWvxO06FMPFM2v79bs_lovUmByYEljcLWtPlnYDfDWtXv9CVB43U3X; uc1=cookie14=UoTV6OIqJvBSmA%3D%3D&pas=0&existShop=false&cookie21=VT5L2FSpccLuJBreKQgf&cookie16=V32FPkk%2FxXMk5UvIbNtImtMfJQ%3D%3D; JSESSIONID=A31B50CF82FC524ABF1565ABB22B21E0; l=eB_6_1EeQ9YKC2pbBOfwourza77O7IRAguPzaNbMiOCPO9fHWCXhWZlugQLMCnGVh6UMR3PVQHf9BeYBqIv4n5U62j-lasMmn; isg=BHp6kyqV8fLlVnx4OlzzWSmXy6CcK_4FT3e-zoRzNo3SdxqxbLmSFbaFwwOrZ3ad");
            HttpWebRequest myrequest = (HttpWebRequest)WebRequest.Create(url);
            myrequest.Method = "GET";
            myrequest.UserAgent = kv["user-agent"];
            myrequest.Timeout = 3000;
            myrequest.KeepAlive = true;
            myrequest.Headers["Cookie"] = kv["Cookie"];

            Console.WriteLine("headers is:\n\n\tName\t\tValue\n{0}", myrequest.Headers);
            HttpWebResponse Web_Response = (HttpWebResponse)myrequest.GetResponse();
            Console.WriteLine(((HttpWebResponse)Web_Response).StatusDescription);
            string html = getText(Web_Response);
            return html;
        }
        /// <summary>
        /// 正则筛选出所需内容
        /// </summary>
        /// <param name="html"></param>
        /// <param name="list">定义在上方的公共Arraylist变量，用于储存变量</param>
        public static void getGoodsinfo(string html,ArrayList list)
        {
            
            Regex rx_title = new Regex(@"\""raw_title\""\:\"".*?\""", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex rx_price = new Regex(@"\""view_price\""\:\""[\d.]*\""", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex rx_sales = new Regex(@"\""view_sales\""\:\""[\d\.]*[\u4e00-\u9fa5]?\+?人付款\""", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var title = rx_title.Matches(html);
            var price = rx_price.Matches(html);
            var sales = rx_sales.Matches(html);

            for (int i = 0; i < title.Count; i++)
            {
                var tit = title[i].ToString().Split(':')[1];
                var pri = price[i].ToString().Split(':')[1];
                var sal = sales[i].ToString().Split(':')[1];
                string[] item = new string[] {tit,pri,sal};
                list.Add(item);
            }

        }
        /// <summary>
        /// 输出商品信息
        /// </summary>
        /// <param name="list"></param>
        public static void printGoodsinfo(ArrayList list)
        {
            int count = 1;
            foreach (string[] item in list)
            {
                Console.WriteLine($"{count}\t{item[0]}\t{item[1]}\t{item[2]}");
                count += 1;
            }
        }
        /// <summary>
        /// 从response中获取页面信息，根据返回头自动分析页面格式，选择相应的格式进行解析，支持gzip和原格式
        /// </summary>
        /// <param name="Web_Response"></param>
        /// <returns>返回html</returns>
        public static string getText(HttpWebResponse Web_Response)
        {
            string html = "";
            foreach (Cookie cook in Web_Response.Cookies)
            {
                Console.WriteLine("Cookie:");
                Console.WriteLine($"{cook.Name} = {cook.Value}");
                Console.WriteLine($"Domain: {cook.Domain}");
                Console.WriteLine($"Path: {cook.Path}");
                Console.WriteLine($"Port: {cook.Port}");
                Console.WriteLine($"Secure: {cook.Secure}");

                Console.WriteLine($"When issued: {cook.TimeStamp}");
                Console.WriteLine($"Expires: {cook.Expires} (expired? {cook.Expired})");
                Console.WriteLine($"Don't save: {cook.Discard}");
                Console.WriteLine($"Comment: {cook.Comment}");
                Console.WriteLine($"Uri for comments: {cook.CommentUri}");
                Console.WriteLine($"Version: RFC {(cook.Version == 1 ? 2109 : 2965)}");

                // Show the string representation of the cookie.
                Console.WriteLine($"String: {cook}");
            }
            if (Web_Response.ContentEncoding.ToLower() == "gzip")  // 如果使用了GZip则先解压
            {
                using (Stream Stream_Receive = Web_Response.GetResponseStream())
                {
                    using (var Zip_Stream = new GZipStream(Stream_Receive, CompressionMode.Decompress))
                    {
                        using (StreamReader Stream_Reader = new StreamReader(Zip_Stream, Encoding.UTF8))
                        {
                            html = Stream_Reader.ReadToEnd();


                        }
                    }
                }
            }
            else
            {
                using (Stream Stream_Receive = Web_Response.GetResponseStream())
                {
                    using (StreamReader Stream_Reader = new StreamReader(Stream_Receive, Encoding.UTF8))
                    {
                        html = Stream_Reader.ReadToEnd();
                    }
                }
            }

            return html;
        }

    }
}
