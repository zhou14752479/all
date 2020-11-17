using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序202005
{
    public partial class 库存监控 : Form
    {
        public 库存监控()
        {
            InitializeComponent();
        }
        #region 苏飞请求
        public static string gethtml(string url)
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
                Cookie = "GeoLocationZipCode=90001; internationalshippref=preferredcountry=US&preferredcurrency=USD&preferredcountryname=United%20States; no-track=ccpa=false; nui=firstVisit=2020-05-21T01%3A17%3A00.015Z&geoLocation=&isModified=false&lme=false; shoppertoken=shopperToken=eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJlOThmZTE3MzA4ZTU0MTU5OTI1OWIyOGI0ODgyODcwZSIsImF1ZCI6Imd1ZXN0IiwiaXNzIjoibm9yZHN0cm9tLWd1ZXN0LWF1dGgiLCJleHAiOjE5MDU1NTY2MjAsInJlZnJlc2giOjE1OTAwMzgyMjAsImp0aSI6ImJkNTczNDdiLWE5MGUtNGIwNi1iNjBjLWM5NDIxZjFhY2IxZiIsImlhdCI6MTU5MDAyMzgyMH0.zoXFIcOPGqfnoW-daceDsyYTya2vmaub7VbX3WI0BvpXejEPdSexXhlwqXgvpWfxTUNavm5IgfbWpvtFeWOZ4kpuRmRX59uB6UeFmfEcBhvia-xCg-k_fqD9U5yfu-TmF9lbrdRHY3y5iWk6cPvs6Six_LX8K8tfGqeVMdrfbcuuidoUEnR3KOnDCMSnFbhrjds0ERsS-UxwxH7najaTVK7e7rIpFyo8Pj26cKzj7UmgmvoIKHbs6uXtjXbO5eIm4mI9t4xHRfRvYBX8XKbzaRLQ5F_DNkDSXQR2MLJ0EOQjLiwZaHYfBAWO_08b_elynca4PbZj8xNiRlspkEaePw; experiments=ExperimentId=33a9eb2a-1587-4ea8-9e03-670f4d200858; Ad34bsY56=AsITzjRyAQAAnRBXGCd4mkYoJMKE-5p5DBxIXZYxI3d9kRXerAAAAXI0zhPCAQ-EpiE|1|1|8e34939b7f24f50c467f295e20e47d6b546dad7e; shopping-bag-migration=shopperId=e98fe17308e541599259b28b4882870e&isMigrated=true; _gcl_au=1.1.1188902204.1590023828; storemode=postalCode=90001&selectedStoreIds=&storesById=&localMarketId=&localMarketsById=; client=viewport=5_XLARGE; shopping-bag-migration=shopperId=e98fe17308e541599259b28b4882870e&isMigrated=true; ftr_ncd=6; _fbp=fb.1.1590023853323.972303518; rfx-forex-rate=currencyCode=USD&exchangeRate=1&quoteId=0; session=FILTERSTATE=&RESULTBACK=&RETURNURL=http%3A%2F%2Fshop.nordstrom.com&SEARCHRETURNURL=http%3A%2F%2Fshop.nordstrom.com&FLSEmployeeNumber=&FLSRegisterNumber=&FLSStoreNumber=&FLSPOSType=&gctoken=&CookieDomain=&IsStoreModeActive=0; usersession=CookieDomain=nordstrom.com&SessionId=d90a3904-ec61-43eb-9af8-b7e52bb1ccf5; _sp_id.c229=33a9eb2a-1587-4ea8-9e03-670f4d200858.1590023831514.2.1590023831514.1590023831514.af6b4cb8-6edc-47e6-955c-213a80cda2d3; _sp_ses.c229=*; nordstrom=bagcount=1&firstname=&ispinned=False&isSocial=False&shopperattr=||0|False|-1&shopperid=e98fe17308e541599259b28b4882870e&USERNAME=; _gid=GA1.2.56238419.1590222641; _4c_mc_=24f600c2-9b9a-48b5-ab15-42e6ee27fb8d; _ga_XWLT9WQ1YB=GS1.1.1590222638.2.1.1590222702.0; _ga=GA1.2.1335495830.1590023833; recentsearches=value=5381779%2C5984134; Bd34bsY56=AnvJrkByAQAABicm4gcmOpuoG99IOFKfBzC7iw-aMyYAsbXGmgAAAXJArsl7Abhwqzo=; storeprefs=90001|100|||2020-05-23T08:47:22.143Z; minibag=MiniBagHash=1590223648690; _uetsid=d6808563-7462-36c3-18c2-86aa7f648e7e; rkglsid=h-2e474f789e8add63735afa3117234ce5_t-1590223649; Ad34bsY56_dc=%7B%22c%22%3A%20%22YlBjVGZRZDFXclhUaHluRg%3D%3Dry7kP6GwRwkOlUkZjlG-tz_QYMW5lo0bbE7cGBPxGNWulgaKpK4hczhxqART5c-905bbfbh-zbSQTz7CC8zLmTGN8vljSw%3D%3D%22%2C%20%22dc%22%3A%200%2C%20%22mf%22%3A%200%7D; forterToken=c194ed0af12d460d8e13621875ac802b_1590223641705__UDF43_6",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36",//用户的浏览器类型，版本，操作系统     可选项有默认值  
                Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值  
                ContentType = "text/html",//返回类型    可选项有默认值  
                Referer = "https://live.500.com/wanchang.php",//来源URL     可选项  
                Allowautoredirect = true,//是否根据３０１跳转     可选项  
                AutoRedirectCookie = true,//是否自动处理Cookie     可选项  
                                          //CerPath = "d:\123.cer",//证书绝对路径     可选项不需要证书时可以不写这个参数  
                                          //Connectionlimit = 1024,//最大连接数     可选项 默认为1024  
                Postdata = "",//Post数据     可选项GET时不需要写  
                              //ProxyIp = "192.168.1.105：2020",//代理服务器ID     可选项 不需要代理 时可以不设置这三个参数  
                              //ProxyPwd = "123456",//代理服务器密码     可选项  
                              //ProxyUserName = "administrator",//代理服务器账户名     可选项  
                ResultType = ResultType.String,//返回数据类型，是Byte还是String  
            };
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            string cookie = result.Cookie;


            return html;

        }

        #endregion

        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
            

            try
            {
                string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i =0; i < text.Length; i++)
                {

                    string url = "https://shop.nordstrom.com/sr?origin=keywordsearch&keyword="+text[i];

                    string html = gethtml(url);

                    
                    Match uid = Regex.Match(html, @"class=""_1av3_"" href=""([\s\S]*?)""");

                    string aurl = "https://shop.nordstrom.com" + uid.Groups[1].Value;
                    string ahtml = gethtml(aurl);

                    Match title = Regex.Match(ahtml, @"""productName"":""([\s\S]*?)""");
                    MessageBox.Show(title.Groups[1].Value);
                    if (ahtml.Contains("SOLD OUT"))
                    {
                        MessageBox.Show(DateTime.Now.ToString()+"\r\n"+title.Groups[1].Value+"\r\n"+"货号："+text[i]+"\r\n"+ "无库存");
                    }
                  


                }
                

              

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void 库存监控_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            run();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                StreamReader streamReader = new StreamReader(this.openFileDialog1.FileName, Encoding.Default);
                string text = streamReader.ReadToEnd();
                string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    textBox1.Text += array[i] + "\r\n";

                }

            }
        }
    }
}
