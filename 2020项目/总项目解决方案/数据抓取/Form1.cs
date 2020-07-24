using CsharpHttpHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 数据抓取
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }




        //public ObjectType getToken(string taskCode, string cookies, WebHeaderCollection heard, string Proxip)
        //{
        //    ObjectType ot = new ObjectType();
        //    HttpHelper hh = new HttpHelper();
        //    HttpItem hi = new HttpItem();
        //    try
        //    {
        //        hi.ProxyIp = Proxip;
        //        hi.Accept = "*/*";
        //        heard.Add("Cache-Control", "no-cache");
        //        heard.Add("Accept-Language", "zh-cn");
        //        hi.Header = heard;
        //        hi.Cookie = cookies.Replace("\\", "");
        //        hi.ContentType = "application/json";
        //        string GetTkkJS = File.ReadAllText(taskCode + "\\getToke.js");
        //        string p = "/swap/im?appId=10023-wb@jN1oMaGj8lk0&source=2&clientType=pcweb&callback=jQuery1124012637464735470672_1544164041815&_=1544164041816";
        //        string tk = GlobalUtil.ExecuteScript("getKey('" + p + "')", GetTkkJS);
        //        string url = "https://ppuswapapi.58.com/swap/im?appId=10023-wb@jN1oMaGj8lk0&source=2&clientType=pcweb&callback=jQuery1124012637464735470672_1544164041815&_=1544164041816&key=" + tk;
        //        hi.URL = url;
        //        hi.Referer = url;
        //        hi.UserAgent = "Mozilla/5.0 (Linux; U; Android 4.4.2; en-us; LGMS323 Build/KOT49I.MS32310c) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/79.0.3945.130 Mobile Safari/537.36";
        //        HttpResult hr = new HttpResult();
        //        hr = hh.GetHtml(hi);
        //        string html = hr.Html;
        //        bool flag = html != "";
        //        if (flag)
        //        {
        //            string token = GlobalUtil.TextGainCenter("token\":\"", "\"", html);
        //            string xxzl_smartid = GlobalUtil.TextGainCenter("xxzl_smartid=", ";", html);
        //            bool flag2 = xxzl_smartid == "";
        //            if (flag2)
        //            {
        //                xxzl_smartid = "86ad30cb50e4e5ee37942fd6752048a7";
        //            }
        //            ot.code = token;
        //            ot.name = xxzl_smartid;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return ot;
        //}
      
        public string getSign(string s)
        {
            SHA1CryptoServiceProvider sha1CryptoServiceProvider = new SHA1CryptoServiceProvider();
            byte[] bytes = Encoding.Default.GetBytes("2f5c8229" + s);
            byte[] value = sha1CryptoServiceProvider.ComputeHash(bytes);
            string text = BitConverter.ToString(value).Replace("-", "");
            return text.ToLower();
        }


       void GetResponse(ParameterModel param, DomainModel dm)
        {

            
            HttpHelper helper = new HttpHelper();
            HttpItem item = new HttpItem();
            try
            {
                //item.ProxyIp = dm.Proxip;
                //Dictionary<string, string> dictionary = new Dictionary<string, string>();
                //dm.Heard.Add("openudid", GlobalUtil.CalcMd5(DateTime.Now.ToString("yyyyMMddHHmmssfff") + "tetete"));
                //dm.Heard.Add("openudid", GlobalUtil.CalcMd5(DateTime.Now.ToString("yyyyMMddHHmmssfff") + "s112121"));
                item.Header = dm.Heard;
                item.Accept = "*/*";
                CookieCollection cookies = new CookieCollection();
                Cookie cookie = new Cookie
                {
                    Name = "cid",
                    Value = "2",
                    Expired = true,
                    Expires = DateTime.Now.AddHours(10.0),
                    Domain = item.Host
                };
                cookies.Add(cookie);
                Cookie cookie2 = new Cookie
                {
                    Name = "os",
                    Value = "ios",
                    Expired = true,
                    Expires = DateTime.Now.AddHours(10.0),
                    Domain = item.Host
                };
                cookies.Add(cookie2);
                item.CookieCollection = cookies;
                item.UserAgent = "Mozilla/5.0 (Linux; U; Android 4.4.2; en-us; LGMS323 Build/KOT49I.MS32310c) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/79.0.3945.130 Mobile Safari/537.36";
                item.Cookie = dm.Cookies;
                int num = (int.Parse(param.Page) / int.Parse(param.PageSize)) + 1;
                string cityId = param.CityId;
                string str2 = cityId.Substring(0, cityId.IndexOf('|'));
                
                string str3 = param.Category.Replace("(?)", num.ToString());
                string str4 = GlobalUtil.CalcMd5(DateTime.Now.ToString("yyyyMMddHHmmssfff")).ToLower();
                object[] objArray1 = new object[11];
                objArray1[0] = "_tdc=";
                TimeSpan span = (TimeSpan)(DateTime.UtcNow - new DateTime(0x7b2, 1, 1, 0, 0, 0, 0));
                objArray1[1] = Convert.ToInt64(span.TotalMilliseconds).ToString();
                objArray1[2] = "&action=getListInfo&circleLat=&circleLon=&localname=";
                objArray1[3] = str2;
                objArray1[4] = "&location=2&openid=";
                objArray1[5] = str4;
                objArray1[6] = "&os=ios&page=";
                objArray1[7] = num;
                objArray1[8] = "&pageIndex=";
                objArray1[9] = num;
                objArray1[10] = "&pageSize=25&tabkey=allcity";
                string s = string.Concat(objArray1);
                string str6 = this.getSign(s);
                s = s + "&filterparams=%7B%22biz%22%3A%220%22%7D&_gxm=" + str6;
                string str7 = "https://miniappfang.58.com/api/list/ershoufang?" + s;
                item.URL = str7;
                HttpResult result = new HttpResult();
                string html = helper.GetHtml(item).Html;
                //textBox1.Text = html;


                string str9 = "42299348767003";

                string[] textArray1 = new string[6];
                textArray1[0] = "_tdc=";
                span = (TimeSpan)(DateTime.UtcNow - new DateTime(0x7b2, 1, 1, 0, 0, 0, 0));
                textArray1[1] = Convert.ToInt64(span.TotalMilliseconds).ToString();
                textArray1[2] = "&infoid=";
                textArray1[3] = str9;
                textArray1[4] = "&openid=";
                textArray1[5] = str4;
                string str11 = string.Concat(textArray1);
                str6 = this.getSign(str11);
                str7 = "https://miniappfang.58.com/api/infodetail?" + str11 + "&_gxm=" + str6;
                item.URL = str7;
                result = new HttpResult();
                // textBox1.Text = helper.GetHtml(item).Html;
                textBox1.Text = str7;
            }
            catch (Exception ex2)
            {
                MessageBox.Show(ex2.ToString());
            }
            
        }


        

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ParameterModel param = new ParameterModel();
            param.Page = "10";
            param.CityId = "sh|2";
          
            param.PageSize = "100";
            param.Category = "/ershoufang/pn(?)/?&source01=0";


            DomainModel dm = new DomainModel();

            dm.Cookies = "";
            
            GetResponse(param, dm);
        }
    }
}
