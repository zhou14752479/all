using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using myDLL;

namespace 校友邦
{
    class function
    {

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData, string COOKIE, string charset,string contenttype,string refer)
        {
            try
            {

                refer = "https://servicewechat.com/wxf1c2e0bbdgfggs3c/335/page-frame.html";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
           
                request.ContentType = contenttype;
                //request.ContentLength = postData.Length;
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 15_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/8.0.18(0x18001230) NetType/WIFI Language/zh_CN";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = refer;
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

        public string status = "2";
        public string login(string username, string password)
        {
            password = method.GetMD5(password);
            string url = "https://xcx.xybsyw.com/login/login.action";
            string postdata = "username=" + username + "&password=" + password + "&openId=ooru94lH0MDBlYKT4dUwpEkRyAWQ&deviceId=";
            string html =PostUrl(url, postdata, "", "utf-8", "application/x-www-form-urlencoded", "");

            string sessionId = Regex.Match(html, @"""sessionId"":""([\s\S]*?)""").Groups[1].Value;
            if (sessionId != "")
            {
                return "JSESSIONID=" + sessionId + ";";
            }
            else
            {
                return "";
            }
        }


        public string getplanid(string cookie)
        {

            string url = "https://xcx.xybsyw.com/student/progress/ProjectList.action";
            string postdata = "";
            string html = PostUrl(url, postdata, cookie, "utf-8", "application/x-www-form-urlencoded", "");

            string planid = Regex.Match(html, @"""planId"":([\s\S]*?),").Groups[1].Value;
            return planid;
        }

        public string gettraineeId(string planid, string cookie)
        {

            string url = "https://xcx.xybsyw.com/student/clock/GetPlan!getDefault.action";
            //string postdata = "planId="+planid;
            string postdata = "";
            string html = PostUrl(url, postdata, cookie, "utf-8", "application/x-www-form-urlencoded", "");

            string traineeId = Regex.Match(html, @"""traineeId"":([\s\S]*?)}").Groups[1].Value;
            return traineeId;
        }

        public string getadcode(string lng, string lat)
        {
            string url = "https://restapi.amap.com/v3/geocode/regeo?key=c222383ff12d31b556c3ad6145bb95f4&location=" + lng + "%2C" + lat + "&extensions=all&s=rsx&platform=WXJS&appname=c222383ff12d31b556c3ad6145bb95f4&sdkversion=1.2.0&logversion=2.0";
            string html = method.GetUrl(url, "utf-8");
            string adcode = Regex.Match(html, @"""adcode"":""([\s\S]*?)""").Groups[1].Value.Trim();
            return adcode;
        }

        public string shangchuanma(string cookie)
        {
            string url = "https://xcx.xybsyw.com/student/clock/saveEpidemicSituation.action";
            //string postdata = "planId="+planid;
            string postdata = "healthCodeStatus=0&locationRiskLevel=0&healthCodeImg=https%3A%2F%2Fss0.xybsyw.com%2Ftemp%2F20220228%2Fschool%2F12960%2Fepidemicsituation%2F4238913%2F1646013660242.png";
            string html = PostUrl(url, postdata, cookie, "utf-8", "application/x-www-form-urlencoded", "");

            string msg = Regex.Match(html, @"""msg"":""([\s\S]*?)""").Groups[1].Value;
            return msg;
        }

        public string qiandao(string cookie, string addr, string traineeId)
        {

            string aurl = "https://xcx.xybsyw.com/student/clock/GetPlan!detail.action";
            string apostdata = "traineeId=" + traineeId;
            string aHtml = PostUrl(aurl, apostdata, cookie, "utf-8", "application/x-www-form-urlencoded", "");
            string address = Regex.Match(aHtml, @"""address"":""([\s\S]*?)""").Groups[1].Value.Trim();
            string lat = Regex.Match(aHtml, @"""lat"":([\s\S]*?),").Groups[1].Value.Trim();
            string lng = Regex.Match(aHtml, @"""lng"":([\s\S]*?),").Groups[1].Value.Trim();

            if (address.Trim() == "")
            {
                address = System.Web.HttpUtility.UrlEncode(addr);
                string baiduurl = "https://api.map.baidu.com/geocoding/v3/?address=" + address + "&output=json&ak=9DemeyQjUrIX14Fz8uEwVpGyKErUP4Sb&callback=showLocation";
                string baiduhtml = method.GetUrl(baiduurl, "utf-8");
                lat = Regex.Match(baiduhtml, @"lat"":([\s\S]*?)}").Groups[1].Value.Trim();
                lng = Regex.Match(baiduhtml, @"lng"":([\s\S]*?),").Groups[1].Value.Trim();


            }

            string adcode = getadcode(lng, lat);

            //string url = "https://xcx.xybsyw.com/student/clock/PostNew.action";

            string url = "https://xcx.xybsyw.com/student/clock/Post.action";
            string postdata = "traineeId=" + traineeId + "&adcode=" + adcode + "&lat=" + lat + "&lng=" + lng + "&address=" + address + "&deviceName=microsoft&punchInStatus=1&clockStatus=" + status;

           

            string html = PostUrl(url, postdata, cookie, "utf-8", "application/x-www-form-urlencoded", "");

            //string msg = Regex.Match(html, @"""msg"":""([\s\S]*?)""").Groups[1].Value;
            MatchCollection msgs = Regex.Matches(html, @"""msg"":""([\s\S]*?)""");
            string msg = msgs[msgs.Count - 1].Groups[1].Value;
            if (msg != "")
            {
                return msg;
            }
            else
            {
                return "failed";
            }
        }

    }
}
