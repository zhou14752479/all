using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using myDLL;

namespace 校友邦
{
    class function
    {

       public string status = "2";
        public string login(string username, string password)
        {
            password = method.GetMD5(password);
            string url = "https://xcx.xybsyw.com/login/login.action";
            string postdata = "username=" + username + "&password=" + password + "&openId=ooru94lH0MDBlYKT4dUwpEkRyAWQ&deviceId=";
            string html = method.PostUrl(url, postdata, "", "utf-8", "application/x-www-form-urlencoded", "");

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
            string html = method.PostUrl(url, postdata, cookie, "utf-8", "application/x-www-form-urlencoded", "");

            string planid = Regex.Match(html, @"""planId"":([\s\S]*?),").Groups[1].Value;
            return planid;
        }

        public string gettraineeId(string planid, string cookie)
        {

            string url = "https://xcx.xybsyw.com/student/clock/GetPlan!getDefault.action";
            //string postdata = "planId="+planid;
            string postdata = "";
            string html = method.PostUrl(url, postdata, cookie, "utf-8", "application/x-www-form-urlencoded", "");

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

        public string qiandao(string cookie, string addr, string traineeId)
        {

            string aurl = "https://xcx.xybsyw.com/student/clock/GetPlan!detail.action";
            string apostdata = "traineeId=" + traineeId;
            string aHtml = method.PostUrl(aurl, apostdata, cookie, "utf-8", "application/x-www-form-urlencoded", "");
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

            string url = "https://xcx.xybsyw.com/student/clock/PostNew.action";
            string postdata = "traineeId=" + traineeId + "&adcode=" + adcode + "&lat=" + lat + "&lng=" + lng + "&address=" + address + "&deviceName=microsoft&punchInStatus=1&clockStatus=" + status;

            string html = method.PostUrl(url, postdata, cookie, "utf-8", "application/x-www-form-urlencoded", "");

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
