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
using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using myDLL;

namespace 临时数据抓取
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Thread thread;
        bool zanting = true;
        bool status = true;

        string COOKIE = "preferedstore=3337; RES_TRACKINGID=553328881033867; _gcl_au=1.1.951625230.1624617847; dtm_token=AQEIxp9X_iBRVgE5PlHBAQHyNAE; s_ecid=MCMID%7C71665817602425959411962606342967286693; s_fid=0B287B7D70BFE6E9-25055AD3952EA38A; JSESSIONID=3MwPjxxVsRor2S1qmsJudxi7hN3NhjoFIOu_fAww.pr-camaro2-xl03-EStoreWS03; WWW-WS-ROUTE=ffffffff09c20eb945525d5f4f58455e445a4a421730; TLTSID=7772d917d3f015e1950100e0ed6a79aa; TLTUID=7772d917d3f015e1950100e0ed6a79aa; AKA_A2=A; bm_sz=F2D946A6585938F88ED47D035333B736~YAAQpFw6FwLfl0V6AQAAy0/MTwy77yftJXoAeNCKDJjP+vYpbNlvu0kg7ud8FieKCuG3F3IiJiKEB9QIPipnL2FkOkARYgl/ovwn0O43kiYbNbIuV1Knn2CGUIAau9X72CjIZDzfXVY+xn6RMEWfqm9UGk3bSOG7P7uPbthSe/pFk+zC8QE2ftUHct6rmjgPVoQ=; profileId=19567564934; rewardsId=; cartProductPartIds=; cartProductSkus=; cartProductTitles=; cartProductVendors=; cartUnitPrice=; cartDiscountPriceList=; prevUrlRouteValue=/; ak_bmsc=B9FF13854A5CBE560ABC2C63384EDC8A~000000000000000000000000000000~YAAQpFw6F3ffl0V6AQAADFnMTwyCfjJRinA097+NDQZBS4qvtn0EqN0pAkzqw1xYtYonfSt2qLyT/JbyqEF7xsuy4yPmK7lIUhezLiudf1/Jyq6BxsgEkuZSdfxuScTbKySJPUtwR7uxvQeXhe2urtehcjxu2xkCKighJojcvTmi99hGZ5d+Y8lA50lpN+WdMd1dkkb587+uTwubDBvFdfbhOPgL+sDES0YteCg5mvOePXbm5aqrfnfA+vf3GBJAFLsfmV1efEuN4dyMVO+h5Ri/JpdsukOAeR47JozFK3QXqbV+Bfn1BiGXGJI8L6JGnfzyJZoSNMK6wRPwesxEV4bJt116shYa9fPcT8Wr48DDM1UJNSmgeGvHjtS5fU17wz8hjEmWDlVbeFf54P3gMmEIG2e+Wy5R6R+gMWZZ/m7icBisYMDo2nuC9rI0+mwJ2rRgmF3FUBUltGbdROwvzzIrggMLcks8Iivma1lIyyq2kkzqb5LTIOfzgMg=; _uetsid=319e8e60d79f11ebb418eb5ff48d8548; _uetvid=49a5b400d5a211ebaf32035e455a6bb8; userType=3; nddMarket=OAK, CA; nddHub=4432; nddStore=6913; _scid=06ecf98f-7596-4e5e-add0-ea2c76b29ccd; AMCVS_0E3334FA53DA8C980A490D44%40AdobeOrg=1; AMCV_0E3334FA53DA8C980A490D44%40AdobeOrg=1406116232%7CMCIDTS%7C18806%7CMCMID%7C71665817602425959411962606342967286693%7CMCAAMLH-1625441231%7C7%7CMCAAMB-1625441231%7CRKhpRz8krg2tLO6pguXWp5olkAcUniQYPHaMWWgdJ3xzPWQmdj0y%7CMCOPTOUT-1624843631s%7CNONE%7CMCAID%7CNONE%7CvVersion%7C2.5.0; s_pn=az%3Ahome; sc_nrv=1624836431424-Repeat; sc_lv=1624836431427; sc_lv_s=Less%20than%207%20days; s_vnum=1625068800401%26vn%3D2; s_invisit=true; s_p24=https%3A%2F%2Fwww.autozone.com%2F; s_cc=true; __idcontext=eyJjb29raWVJRCI6IkUyVFFIRlFQQldHTjZCQzYyQVhQWEFLRjRDR0lEWENVWEpVNjRCRFlGRFhRPT09PSIsImRldmljZUlEIjoiRTJUUUhGUVBBRzYyRUgyRVVZNjYzSkRSM1QyMkxRM1JUUklOUUpaQkpHMkE9PT09IiwiaXYiOiJOMzdMTkhERU1IR1VURk1JRUJaRExXVlhZQT09PT09PSIsInYiOjF9; bounceClientVisit3804v=N4IgNgDiBcIBYBcEQM4FIDMBBNAmAYnvgO6kB0AhgK4ID2AXrQHYCmZAxrQLZEgA0IAE4wQ-EChYBzGAG0AugF8gA; _sctr=1|1624809600000; QSI_HistorySession=https%3A%2F%2Fwww.autozone.com%2F~1624836436079; isSearchByPartNumber=true; model=false; userDefinedMake=false; vehicleSetFromSearch=false; s_sq=autozmobilefirstdefdev%3D%2526c.%2526a.%2526activitymap.%2526page%253Dhttps%25253A%25252F%25252Fwww.autozone.com%25252Fsuspension-steering-tire-and-wheel%25252Fsway-bar-bracket-bushing%25252Fduralast-sway-bar-bracket-bushing-fa7247%25252F335009_0_0%2526link%253DLimited-Lifetime%252520Warranty%2526region%253D__next%2526.activitymap%2526.a%2526.c%2526pid%253Dhttps%25253A%25252F%25252Fwww.autozone.com%25252Fsuspension-steering-tire-and-wheel%25252Fsway-bar-bracket-bushing%25252Fduralast-sway-bar-bracket-bushing-fa7247%25252F335009_0_0%2526oid%253DLimited-Lifetime%252520Warranty%2526oidt%253D3%2526ot%253DSUBMIT; utag_main=v_id:017a42c5062e00997bc05b5e396003072001906a00bd0$_sn:2$_se:17$_ss:0$_st:1624838965095$vapi_domain:autozone.com$ses_id:1624836428485%3Bexp-session$_pn:16%3Bexp-session; bm_sv=53158BA913BC06EC0CF3DC0600363889~1Y/uvJuTpDtUUVMwIVY4eDQd2hgbSiwCAHwf1CbXJqi7d5c6HLOFTarqgV25+Bbpdi2XYz8MLwqq2pDLarO4enqFCBKZnKK4HaahZ4h73SLpyCw0QcWLSA3xHQ7QpcP88wrX9urH2nNOm11ugbLddQy6SQ6Ozyin86qYQt4gBw4=; _abck=F5AADFA7CE18F018EB3A77FE5B7853F2~-1~YAAQtFw6F9tg/hp6AQAAlJXXTwZ6RExLW2utf2d+ofJaF31U1l7IjS0J4rC4oYQZ9VJDCnFQieZgwI4GuPSDaZKpxIR6F10C+Q2Qg3gwYxh7E0ovgwOjfFLWsgsdg4THT8xyyp+L3WYxwFL566erqaSFWroNs64XdTikOCWzMWwUEeSnl6TRDBlhSYNPgmjm6beVfM0OZPaiddNUjFZLD8uViHmNc1Y33f5VtR4t3Q5ex7wY4ku7HA4xM7/Tj6WVzgOmxZPrpKb/ZdM/T+IANz6F/pOl2o22KF5Xz0O2JXykLmUee947HzAup+is4HKq9++A6KfQ4976tIpiXDJnHlGtMnPo++za/t6fAZjvCA6WMYgwLAPpyyCj9lzSqISvRk0FbDiubku2IgesH364YC64V+AhrdxlMpC7~-1~-1~-1; RT=\"z=1&dm=autozone.com&si=67f390a8-c24b-4701-b56c-b79a82b3c97a&ss=kqftnm2d&sl=b&tt=6xa&obo=a&rl=1&ld=g0d8&r=4e4pbbgr&ul=g0d9\"";
        string[] agents = {
  "Mozilla/5.0 (Linux; U; Android 2.3.6; en-us; Nexus S Build/GRK39F) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1",
  "Avant Browser/1.2.789rel1 (http://www.avantbrowser.com)",
  "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/532.5 (KHTML, like Gecko) Chrome/4.0.249.0 Safari/532.5",
  "Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/532.9 (KHTML, like Gecko) Chrome/5.0.310.0 Safari/532.9",
  "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/534.7 (KHTML, like Gecko) Chrome/7.0.514.0 Safari/534.7",
  "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US) AppleWebKit/534.14 (KHTML, like Gecko) Chrome/9.0.601.0 Safari/534.14",
  "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.14 (KHTML, like Gecko) Chrome/10.0.601.0 Safari/534.14",
  "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.20 (KHTML, like Gecko) Chrome/11.0.672.2 Safari/534.20",
  "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/534.27 (KHTML, like Gecko) Chrome/12.0.712.0 Safari/534.27",
  "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.24 Safari/535.1",
  "Mozilla/5.0 (Windows NT 6.0) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.120 Safari/535.2",
  "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.7 (KHTML, like Gecko) Chrome/16.0.912.36 Safari/535.7",
  "Mozilla/5.0 (Windows; U; Windows NT 6.0 x64; en-US; rv:1.9pre) Gecko/2008072421 Minefield/3.0.2pre",
  "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.0.10) Gecko/2009042316 Firefox/3.0.10",
  "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-GB; rv:1.9.0.11) Gecko/2009060215 Firefox/3.0.11 (.NET CLR 3.5.30729)",
  "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.1.6) Gecko/20091201 Firefox/3.5.6 GTB5",
  "Mozilla/5.0 (Windows; U; Windows NT 5.1; tr; rv:1.9.2.8) Gecko/20100722 Firefox/3.6.8 ( .NET CLR 3.5.30729; .NET4.0E)",
  "Mozilla/5.0 (Windows NT 6.1; rv:2.0.1) Gecko/20100101 Firefox/4.0.1",
  "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:2.0.1) Gecko/20100101 Firefox/4.0.1",
  "Mozilla/5.0 (Windows NT 5.1; rv:5.0) Gecko/20100101 Firefox/5.0",
  "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:6.0a2) Gecko/20110622 Firefox/6.0a2",
  "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:7.0.1) Gecko/20100101 Firefox/7.0.1",
  "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:2.0b4pre) Gecko/20100815 Minefield/4.0b4pre",
  "Mozilla/4.0 (compatible; MSIE 5.5; Windows NT 5.0 )",
  "Mozilla/4.0 (compatible; MSIE 5.5; Windows 98; Win 9x 4.90)",
  "Mozilla/5.0 (Windows; U; Windows XP) Gecko MultiZilla/1.6.1.0a",
  "Mozilla/2.02E (Win95; U)",
  "Mozilla/3.01Gold (Win95; I)",
  "Mozilla/4.8 [en] (Windows NT 5.1; U)",
  "Mozilla/5.0 (Windows; U; Win98; en-US; rv:1.4) Gecko Netscape/7.1 (ax)",
  "HTC_Dream Mozilla/5.0 (Linux; U; Android 1.5; en-ca; Build/CUPCAKE) AppleWebKit/528.5  (KHTML, like Gecko) Version/3.1.2 Mobile Safari/525.20.1",
  "Mozilla/5.0 (hp-tablet; Linux; hpwOS/3.0.2; U; de-DE) AppleWebKit/534.6 (KHTML, like Gecko) wOSBrowser/234.40.1 Safari/534.6 TouchPad/1.0",
  "Mozilla/5.0 (Linux; U; Android 1.5; en-us; sdk Build/CUPCAKE) AppleWebkit/528.5  (KHTML, like Gecko) Version/3.1.2 Mobile Safari/525.20.1",
  "Mozilla/5.0 (Linux; U; Android 2.1; en-us; Nexus One Build/ERD62) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17",
  "Mozilla/5.0 (Linux; U; Android 2.2; en-us; Nexus One Build/FRF91) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1",
  "Mozilla/5.0 (Linux; U; Android 1.5; en-us; htc_bahamas Build/CRB17) AppleWebKit/528.5  (KHTML, like Gecko) Version/3.1.2 Mobile Safari/525.20.1",
  "Mozilla/5.0 (Linux; U; Android 2.1-update1; de-de; HTC Desire 1.19.161.5 Build/ERE27) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17",
  "Mozilla/5.0 (Linux; U; Android 2.2; en-us; Sprint APA9292KT Build/FRF91) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1",
  "Mozilla/5.0 (Linux; U; Android 1.5; de-ch; HTC Hero Build/CUPCAKE) AppleWebKit/528.5  (KHTML, like Gecko) Version/3.1.2 Mobile Safari/525.20.1",
  "Mozilla/5.0 (Linux; U; Android 2.2; en-us; ADR6300 Build/FRF91) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1",
  "Mozilla/5.0 (Linux; U; Android 2.1; en-us; HTC Legend Build/cupcake) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17",
  "Mozilla/5.0 (Linux; U; Android 1.5; de-de; HTC Magic Build/PLAT-RC33) AppleWebKit/528.5  (KHTML, like Gecko) Version/3.1.2 Mobile Safari/525.20.1 FirePHP/0.3",
  "Mozilla/5.0 (Linux; U; Android 1.6; en-us; HTC_TATTOO_A3288 Build/DRC79) AppleWebKit/528.5  (KHTML, like Gecko) Version/3.1.2 Mobile Safari/525.20.1",
  "Mozilla/5.0 (Linux; U; Android 1.0; en-us; dream) AppleWebKit/525.10  (KHTML, like Gecko) Version/3.0.4 Mobile Safari/523.12.2",
  "Mozilla/5.0 (Linux; U; Android 1.5; en-us; T-Mobile G1 Build/CRB43) AppleWebKit/528.5  (KHTML, like Gecko) Version/3.1.2 Mobile Safari 525.20.1",
  "Mozilla/5.0 (Linux; U; Android 1.5; en-gb; T-Mobile_G2_Touch Build/CUPCAKE) AppleWebKit/528.5  (KHTML, like Gecko) Version/3.1.2 Mobile Safari/525.20.1",
  "Mozilla/5.0 (Linux; U; Android 2.0; en-us; Droid Build/ESD20) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17",
  "Mozilla/5.0 (Linux; U; Android 2.2; en-us; Droid Build/FRG22D) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1",
  "Mozilla/5.0 (Linux; U; Android 2.0; en-us; Milestone Build/ SHOLS_U2_01.03.1) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17",
  "Mozilla/5.0 (Linux; U; Android 2.0.1; de-de; Milestone Build/SHOLS_U2_01.14.0) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17",
  "Mozilla/5.0 (Linux; U; Android 3.0; en-us; Xoom Build/HRI39) AppleWebKit/525.10  (KHTML, like Gecko) Version/3.0.4 Mobile Safari/523.12.2",
  "Mozilla/5.0 (Linux; U; Android 0.5; en-us) AppleWebKit/522  (KHTML, like Gecko) Safari/419.3",
  "Mozilla/5.0 (Linux; U; Android 1.1; en-gb; dream) AppleWebKit/525.10  (KHTML, like Gecko) Version/3.0.4 Mobile Safari/523.12.2",
  "Mozilla/5.0 (Linux; U; Android 2.0; en-us; Droid Build/ESD20) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17",
  "Mozilla/5.0 (Linux; U; Android 2.1; en-us; Nexus One Build/ERD62) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17",
  "Mozilla/5.0 (Linux; U; Android 2.2; en-us; Sprint APA9292KT Build/FRF91) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1",
  "Mozilla/5.0 (Linux; U; Android 2.2; en-us; ADR6300 Build/FRF91) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1",
  "Mozilla/5.0 (Linux; U; Android 2.2; en-ca; GT-P1000M Build/FROYO) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1",
  "Mozilla/5.0 (Linux; U; Android 3.0.1; fr-fr; A500 Build/HRI66) AppleWebKit/534.13 (KHTML, like Gecko) Version/4.0 Safari/534.13",
  "Mozilla/5.0 (Linux; U; Android 3.0; en-us; Xoom Build/HRI39) AppleWebKit/525.10  (KHTML, like Gecko) Version/3.0.4 Mobile Safari/523.12.2",
  "Mozilla/5.0 (Linux; U; Android 1.6; es-es; SonyEricssonX10i Build/R1FA016) AppleWebKit/528.5  (KHTML, like Gecko) Version/3.1.2 Mobile Safari/525.20.1",
  "Mozilla/5.0 (Linux; U; Android 1.6; en-us; SonyEricssonX10i Build/R1AA056) AppleWebKit/528.5  (KHTML, like Gecko) Version/3.1.2 Mobile Safari/525.20.1",
};

        #region GET请求带COOKIE
        /// <summary>
        /// GET请求带COOKIE
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public string GetUrlWithCookie(string Url, string COOKIE, string charset)
        {
            try
            {
              
               System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                                                                                  //添加头部
                WebHeaderCollection headers = request.Headers;
              
               request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.141 Safari/537.36";
             
                request.Referer = "https://www.tecalliance.cn/cn/search/1?q=71433";
                request.Headers.Add("Cookie", COOKIE);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                request.Accept = "*/*";
                request.Timeout = 10000;
                string content = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return "";


            }

        }
        #endregion

        private string GetHttp20210301174102(string url)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = url,//URL     必需项  
                Method = "GET",//URL     可选项 默认为Get  
                Timeout = 100000,//连接超时时间     可选项默认为100000  
                ReadWriteTimeout = 30000,//写入Post数据超时时间     可选项默认为30000  
                IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写  
                Cookie = "",//字符串Cookie     可选项  
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",//用户的浏览器类型，版本，操作系统     可选项有默认值  
                Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值  
                ContentType = "text/html",//返回类型    可选项有默认值  
                Referer = "http://www.sufeinet.com",//来源URL     可选项  
              
                                           //CerPath = "d:\123.cer",//证书绝对路径     可选项不需要证书时可以不写这个参数  
                                           //Connectionlimit = 1024,//最大连接数     可选项 默认为1024  
                Postdata = "",//Post数据     可选项GET时不需要写  
                ProxyIp = "tps115.kdlapi.com:15818",//代理服务器ID     可选项 不需要代理 时可以不设置这三个参数  
                              //ProxyPwd = "123456",//代理服务器密码     可选项  
                              //ProxyUserName = "administrator",//代理服务器账户名     可选项  
                ResultType = ResultType.String,//返回数据类型，是Byte还是String  
            };
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            return html;
        }



        #region tecalliance  检测UA
        public void tecalliance()
        {
            

            for (int i = 0; i < richTextBox1.Lines.Length; i++)
            {
               
                string n = richTextBox1.Lines[i].Trim();
                if (richTextBox1.Lines[i].Trim().Length < 5)
                {
                    n = "0" + richTextBox1.Lines[i].Trim();

                }

                string url = "https://www.tecalliance.cn/cn/search/1?q="+n+ "&lbid=101&base=lbid";
              

               
                string html = GetHttp20210301174102(url);
                if (html.Trim() == "")
                {
                    MessageBox.Show("验证");
                    i = i-1;
                    continue;
                }
                Match num = Regex.Match(html, @"通过产品号查询:([\s\S]*?)</div>");

                Match html1 = Regex.Match(html, @"<div class=""part-detail-item-body"" >([\s\S]*?)<div class=""m-sec"">");
                Match html2= Regex.Match(html, @"<div class=""m-sec"">([\s\S]*?)info-body end");
                Match img = Regex.Match(html, @"class=""brand-img"" src=""([\s\S]*?)""");

                MatchCollection oems = Regex.Matches(html1.Groups[1].Value, @"<a href=""([\s\S]*?)"">([\s\S]*?)</a>");
                MatchCollection canshus = Regex.Matches(html2.Groups[1].Value, @"<span>([\s\S]*?)</span>([\s\S]*?)<strong>([\s\S]*?)</strong>");
                   
                 StringBuilder sb = new StringBuilder();
                for (int j = 0; j < oems.Count; j++)
                {

                    sb.Append(oems[j].Groups[2].Value.Trim()+"/");
                }

                StringBuilder sb1 = new StringBuilder();

                for (int a = 0; a < canshus.Count; a++)
                {

                    sb1.Append(canshus[a].Groups[1].Value.Trim() + canshus[a].Groups[3].Value.Trim() + "/");
                }

                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                lv1.SubItems.Add(n);
                lv1.SubItems.Add(num.Groups[1].Value.Trim());
                lv1.SubItems.Add(sb.ToString());
                lv1.SubItems.Add(sb1.ToString());
                lv1.SubItems.Add(img.Groups[1].Value.Trim());
               
                while (this.zanting == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }
                if (status == false)
                    return;
            }


        }

        #endregion


        #region rockauto.com
        public void rockauto()
        {
            // string ip = getip();
            string ip = "tps115.kdlapi.com:15818";
            for (int i = 0; i < richTextBox1.Lines.Length; i++)
            {
                string n = richTextBox1.Lines[i].Trim();


                string url = "https://www.rockauto.com/en/parts/mevotech," + n;

                //string html = method.GetUrlwithIP(url, ip, "", "utf-8");
                string html = GetHttp20210301174102(url);
     
                //string html = method.GetUrl(url, "utf-8");
               
                Match oem = Regex.Match(html, @"OE Part Numbers"">([\s\S]*?)</span>");
                    Match img = Regex.Match(html, @"info\\\/([\s\S]*?)&");

                //if (oem.Groups[1].Value.Trim()==""|| html.Trim() == "")
                //{
                //    ip = getip();
                //    i = i - 1;
                //    continue;
                //}


                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(n);
                    lv1.SubItems.Add(" ");
                    lv1.SubItems.Add(oem.Groups[1].Value.Trim());
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("http://www.rockauto.com/info/" + img.Groups[1].Value.Replace("__ra_t", "").Replace("\\", "").Trim());
                    
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;
               
            }


        }

        #endregion

        #region rockauto.com进入详情页
        public void rockauto1()
        {


            for (int i = 0; i < richTextBox1.Lines.Length; i++)
            {
                string n = richTextBox1.Lines[i].Trim();
          

                string url = "https://www.rockauto.com/en/parts/mevotech,"+n;



                string html = method.GetUrl(url, "utf-8");

                Match uid = Regex.Match(html, @"moreinfo\.php([\s\S]*?)""");


                if (uid.Groups[1].Value != "")
                {
                    string aurl = "https://www.rockauto.com/en/moreinfo.php" + uid.Groups[1].Value;
                    string ahtml = GetUrlWithCookie(aurl, "", "utf-8");
                    Match oem = Regex.Match(ahtml, @"Number\(s\):([\s\S]*?)</td>");
                    Match img = Regex.Match(ahtml, @"info\\\/([\s\S]*?)&");
                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(n);
                    lv1.SubItems.Add(" ");
                    lv1.SubItems.Add(oem.Groups[1].Value.Trim());
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add("http://www.rockauto.com/info/"+img.Groups[1].Value.Replace("__ra_t","").Replace("\\","").Trim());
                    Thread.Sleep(100);
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;
                }
                else
                {
                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(n);

                }
            }


        }

        #endregion



        public string getip()
        {
            // string html = method.GetUrl("http://47.106.170.4:8081/Index-generate_api_url.html?packid=7&fa=5&groupid=0&fetch_key=&qty=1&port=1&format=txt&ss=1&css=&pro=&city=&usertype=7", "utf-8");
            string html = method.GetUrl("http://47.106.170.4:8081/Index-generate_api_url.html?packid=7&fa=5&groupid=0&fetch_key=&qty=1&port=1&format=txt&ss=1&css=&pro=&city=&usertype=7", "utf-8");
            label1.Text = html;
            return html;
        }


        #region tecalliance去重
        public void tecalliancequchong()
        {

            for (int i = 0; i < richTextBox1.Lines.Length; i++)
            {
                string n = richTextBox1.Lines[i].Trim();
                string[] text = n.Split(new string[] { "/" }, StringSplitOptions.None);
                ArrayList list = new ArrayList();
                StringBuilder sb = new StringBuilder();
                foreach (var item in text)
                {
                    if (!list.Contains(item) &&item!="")
                    {
                        list.Add(item);
                        sb.Append(item+"/");
                    }
                  
                }

                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                lv1.SubItems.Add(sb.ToString());
              




            }


        }

        #endregion

        #region Dorman.com
        public void Dorman()
        {

            string ip = getip();
            for (int i = 0; i < richTextBox1.Lines.Length; i++)
            {
                string n = richTextBox1.Lines[i].Trim();
                

                string url = "https://www.dormanproducts.com/gsearch.aspx?type=oesearch&origin=oesearch&q=" + n;

                string html = method.GetUrlwithIP(url,ip,"","utf-8");

                if (html.Contains("SecurityForm") || html.Trim() == "")
                {
                    ip = getip();
                    i = i - 1;
                    continue;
                }

               MatchCollection dormanpartnumbers = Regex.Matches(html, @"class=""item-name"">([\s\S]*?)</span>");
                Match nums = Regex.Match(html, @"<th scope=""row"">([\s\S]*?)</th>");
                MatchCollection SYDs = Regex.Matches(html, @"<td style=""width:50%"">S([\s\S]*?)</td>");


                Match descriptions = Regex.Match(html, @"<div style=""width:100%; display:block;"">([\s\S]*?)<h4>([\s\S]*?)</h4>");
                Match uid = Regex.Match(html, @"<h2 class=""item-headline"">([\s\S]*?)<a href=""([\s\S]*?)""");
                MatchCollection imgs = Regex.Matches(html, @"<div class=""searchItems-img"">([\s\S]*?)<img src=""([\s\S]*?)""");
                string wutu = " ";
                StringBuilder sb = new StringBuilder();
               // StringBuilder sb1 = new StringBuilder();
                StringBuilder sb2 = new StringBuilder();
                for (int j = 0; j < SYDs.Count; j++)
                {
                     try
                        {
                            sb.Append(dormanpartnumbers[j].Groups[1].Value + ",");
                           // sb1.Append(imgs[j].Groups[2].Value + ",");
                        }
                        catch (Exception)
                        {
                            wutu = "1";
                            continue;
                        }
                      
                    
                }

                //    string aurl = "https://www.dormanproducts.com/" + uid.Groups[2].Value;
                //    string ahtml = method.GetUrlwithIP(aurl, ip, "", "utf-8");
                //if (ahtml.Contains("SecurityForm") ||ahtml.Trim() == "")
                //{
                //    ip = getip();
                //    ahtml = method.GetUrlwithIP(aurl, ip, "", "utf-8");
                //}
                //MatchCollection oems = Regex.Matches(ahtml, @"<th scope=""row"">([\s\S]*?)</th>");

                //for (int a = 0; a < oems.Count; a++)
                //{
                //    sb2.Append(oems[a].Groups[1].Value + ",");

                //}

                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(n);
                lv1.SubItems.Add(nums.Groups[1].Value);
                lv1.SubItems.Add(sb.ToString());
                lv1.SubItems.Add(descriptions.Groups[2].Value);
                    lv1.SubItems.Add(sb2.ToString());
                    lv1.SubItems.Add(wutu);
                  
                    Thread.Sleep(100);
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;
                
               
            }


        }

        #endregion


        #region tecdoc.com
        public void tecdoc()
        {

          
            for (int i = 0; i < richTextBox1.Lines.Length; i++)
            {
                string n = richTextBox1.Lines[i].Trim();
                label1.Text = i.ToString();

                string url = "https://mx.tecdoc.net/search?q="+n+"&lang=en-US";

                string html = method.GetUrl(url, "utf-8");


                MatchCollection aurls = Regex.Matches(html, @"<td class=""text-center text-nowrap hidden-xs hidden-sm""><a href=""([\s\S]*?)""><span class=""highlight"">([\s\S]*?)</span>");
                MatchCollection anames = Regex.Matches(html, @"<h4>([\s\S]*?)</h4>");
                if (aurls.Count == 0)
                {
                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(n);
                    lv1.SubItems.Add("空");
                    lv1.SubItems.Add("空");
                    lv1.SubItems.Add("空");
                    lv1.SubItems.Add("空");
                    lv1.SubItems.Add("空");
                    lv1.SubItems.Add("空");
                    lv1.SubItems.Add("空");
                    lv1.SubItems.Add("空");
                    lv1.SubItems.Add("空");
                    lv1.SubItems.Add("空");
                    continue;
                }
                //MessageBox.Show(aurls.Count.ToString());
                //MessageBox.Show(anames.Count.ToString());
                for (int j = 0; j < aurls.Count; j++)
                {
                    try
                    {
                        string aurl = "https://mx.tecdoc.net"+aurls[j].Groups[1].Value;
                        if (aurls[j].Groups[2].Value == n && anames[j].Groups[1].Value.Contains("SYD"))
                        {
                           
                            string ahtml = method.GetUrl(aurl, "utf-8");
                            string title = Regex.Match(ahtml, @"<h1 itemprop=""name"" class=""media-heading"">([\s\S]*?)<").Groups[1].Value.Trim();
                            string Number = Regex.Match(ahtml, @"<span itemprop=""mpn"">([\s\S]*?)<").Groups[1].Value.Trim();
                            string PartType = Regex.Match(ahtml, @"Part Type:</strong></small>([\s\S]*?)<").Groups[1].Value.Trim();
                            string Position = Regex.Match(ahtml, @"Fitting Position</b></td>([\s\S]*?)</td>").Groups[1].Value.Replace("<td class=\"col-xs-8\">", "").Trim();
                            string Length = Regex.Match(ahtml, @"Length</b></td>([\s\S]*?)</td>").Groups[1].Value.Replace("<td class=\"col-xs-8\">", "").Trim();
                            string Width = Regex.Match(ahtml, @"Width</b></td>([\s\S]*?)</td>").Groups[1].Value.Replace("<td class=\"col-xs-8\">", "").Trim();
                            string Height = Regex.Match(ahtml, @"Height</b></td>([\s\S]*?)</td>").Groups[1].Value.Replace("<td class=\"col-xs-8\">", "").Trim();
                            string Weight = Regex.Match(ahtml, @"Weight</b></td>([\s\S]*?)</td>").Groups[1].Value.Replace("<td class=\"col-xs-8\">", "").Trim();

                            

                            string[] html2s = ahtml.Split(new string[] { "<td rows" }, StringSplitOptions.None); ;
                          
                            if (html2s.Length == 1)
                            {
                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(n);
                                lv1.SubItems.Add(title);
                                lv1.SubItems.Add(Number);
                                lv1.SubItems.Add(PartType);
                                lv1.SubItems.Add(Position);
                                lv1.SubItems.Add(Length);
                                lv1.SubItems.Add(Width);
                                lv1.SubItems.Add(Height);
                                lv1.SubItems.Add(Weight);
                                lv1.SubItems.Add("空");
                                lv1.SubItems.Add("空");

                                continue;
                            }

                            for (int a = 0; a < html2s.Length; a++)
                            {

                                if (html2s[a].Contains("pan=\""))
                                {
                                    string name = Regex.Match(html2s[a], @"<b>([\s\S]*?)</b>").Groups[1].Value.Trim();
                                    MatchCollection values = Regex.Matches(html2s[a], @"<a href=""/search\?q=([\s\S]*?)""");

                                    for (int b = 0; b < values.Count; b++)
                                    {
                                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                                        lv1.SubItems.Add(n);
                                        lv1.SubItems.Add(title);
                                        lv1.SubItems.Add(Number);
                                        lv1.SubItems.Add(PartType);
                                        lv1.SubItems.Add(Position);
                                        lv1.SubItems.Add(Length);
                                        lv1.SubItems.Add(Width);
                                        lv1.SubItems.Add(Height);
                                        lv1.SubItems.Add(Weight);
                                        lv1.SubItems.Add(name);
                                        lv1.SubItems.Add(values[b].Groups[1].Value);

                                        while (this.zanting == false)
                                        {
                                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                        }
                                        if (status == false)
                                            return;
                                    }

                                }

                            }


                        }

                       
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        continue;
                    }


                }
  

            }


        }

        #endregion


        #region autozone.com
        public void autozone()
        {

           
            for (int i = 0; i < richTextBox1.Lines.Length; i++)
            {
                try
                {
                    string n = richTextBox1.Lines[i].Trim();
                    label1.Text = i.ToString();

                    string url = "https://www.autozone.com/searchresult?searchText=" + n ;

                    string html = GetUrlWithCookie(url, COOKIE, "utf-8");


                    Match title = Regex.Match(html, @"<title>([\s\S]*?)</title>");
                    Match price = Regex.Match(html, @"""price"":""([\s\S]*?)""");
                    Match part = Regex.Match(html, @"Part #</span>([\s\S]*?)</span>");
                    Match sku = Regex.Match(html, @"""sku"":""([\s\S]*?)""");
                    Match weight = Regex.Match(html, @"""WEIGHT"":""([\s\S]*?)""");
                    Match img = Regex.Match(html, @"<img src=""([\s\S]*?)""");
                    if (title.Groups[1].Value != "")
                    {
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(n);
                        lv1.SubItems.Add(title.Groups[1].Value.Trim());
                        lv1.SubItems.Add(price.Groups[1].Value.Trim());
                        lv1.SubItems.Add(Regex.Replace(part.Groups[1].Value.Trim(), "<[^>]+>", ""));
                        lv1.SubItems.Add(sku.Groups[1].Value.Trim());
                        lv1.SubItems.Add(weight.Groups[1].Value.Trim());
                        lv1.SubItems.Add(img.Groups[1].Value.Trim());
                    }
                    else
                    {
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(n);
                        lv1.SubItems.Add("空");
                        lv1.SubItems.Add("空");
                        lv1.SubItems.Add("空");
                        lv1.SubItems.Add("空");
                        lv1.SubItems.Add("空");
                        lv1.SubItems.Add("空");

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());

                }

                


            }


        }

        #endregion
        private void button1_Click(object sender, EventArgs e)
        {


            status = true;
            //tecalliance  检测UA
            //if (thread == null || !thread.IsAlive)
            //{
            //    thread = new Thread(tecalliance);
            //    thread.Start();
            //    Control.CheckForIllegalCrossThreadCalls = false;
            //}
            // rockauto
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(autozone);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            //thread = new Thread(tecalliancequchong);
            //thread.Start();
            //Control.CheckForIllegalCrossThreadCalls = false;
            //Dorman
            //if (thread == null || !thread.IsAlive)
            //{
            //    thread = new Thread(Dorman);
            //    thread.Start();
            //    Control.CheckForIllegalCrossThreadCalls = false;
            //}
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            status = false;
        }
    }
}
