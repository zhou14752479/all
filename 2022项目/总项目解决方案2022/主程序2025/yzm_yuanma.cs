using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web.UI.WebControls;
using System.Security.Policy;

namespace 主程序2025
{
    public class yzm_yuanma
    {



        static int success = 0;
        static int fail = 0;

        static Dictionary<string, object> GetCaptchaConfig(string captchaUrl, HttpClient client)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                var request = new HttpRequestMessage(HttpMethod.Get, captchaUrl);
                var response = client.SendAsync(request).Result;
                var responseContent = response.Content.ReadAsStringAsync().Result;

                var match = Regex.Match(responseContent, @"window._config_ = (\{.*?\});", RegexOptions.Singleline);
                if (match.Success)
                {
                    var configJson = match.Groups[1].Value;
                    var config = JsonConvert.DeserializeObject<Dictionary<string, object>>(configJson);
                    //MessageBox.Show($"获取验证码配置信息config：{JsonConvert.SerializeObject(config)}");
                    return config;
                }
                else
                {
                   // MessageBox.Show($"验证码配置错误：{responseContent}");
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                function.log($"获取验证码配置信息时出错：{ex.Message}");
                throw;
            }
        }

        static HttpClientHandler GetProxyHandler()
        {
            return new HttpClientHandler();
        }

        static void GetX5sec(string url, HttpClient client)
        {
            try
            {
                var captchaConfig = GetCaptchaConfig(url, client);
               // MessageBox.Show($"当前验证码类型:{captchaConfig["action"]}");
                if (captchaConfig["action"].ToString() != "captcha")
                {
                    return;
                }

                var data = new Dictionary<string, object>
                {
                    { "captcha_config", captchaConfig },
                    { "key", "" }
                };

                var jsonData = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                //var postRequest = new HttpRequestMessage(HttpMethod.Post, "http://127.0.0.1:3888/tb_n");
                //var postRequest = new HttpRequestMessage(HttpMethod.Post, "http://8.155.4.205:3888/tb_n");
        
                var postRequest = new HttpRequestMessage(HttpMethod.Post, "http://8.153.165.134:3888/tb_n");
                postRequest.Content = content;
                var rp = client.SendAsync(postRequest).Result;
                var responseContent = rp.Content.ReadAsStringAsync().Result;
                var DATA = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);

                var newUrl = DATA["urlEncode"].ToString();
                var getRequest = new HttpRequestMessage(HttpMethod.Get, newUrl);
                getRequest.Headers.Add("bx_et", DATA["bx_et"].ToString());
                getRequest.Headers.Add("bx-pp", DATA["bx_pp"].ToString());
                getRequest.Headers.Add("Referer", DATA["referer"].ToString());

                var x5secResponse = client.SendAsync(getRequest).Result;
                var x5secResponseContent = x5secResponse.Content.ReadAsStringAsync().Result;
                //MessageBox.Show(x5secResponseContent);

                IEnumerable<string> cookies;
                if (x5secResponse.Headers.TryGetValues("Set-Cookie", out cookies))
                {
                    string[] cookieArray = new List<string>(cookies).ToArray();
                    if (Array.Find<string>(cookieArray, c => c.Contains("x5sec")) != null)
                    {
                        // MessageBox.Show($"成功: {string.Join("; ", cookies)}");

                       
                        x5sec = Regex.Match(string.Join("; ", cookies), @"x5sec=([\s\S]*?);").Groups[1].Value;
                        x5sec = "x5sec="+x5sec;
                        success++;
                    }
                    else
                    {
                        function.log($"失败: {x5secResponseContent}");
                        fail++;
                    }
                }
                else
                {
                    function.log($"失败: 未获取到 Set-Cookie 头");
                    fail++;
                }

                function.log($"当前成功率:{Math.Round((double)success / (success + fail) * 100, 2)} 次数:{(success + fail)}");
            }
            catch (Exception ex)
            {
                function.log($"处理 X5Sec 时出错：{ex.Message}");
            }
        }


        public static string url = "";
        public static string x5sec = "";

        public static void run()
        {
            var handler = GetProxyHandler();
            using (var client = new HttpClient(handler))
            {
                //https://ditu.amap.com/detail/get/detail?id=B00155L3DHo
                var postRequest = new HttpRequestMessage(HttpMethod.Post, yzm_yuanma.url);
                postRequest.Headers.Add("Authority", "fahuo.cainiao.com");
                postRequest.Headers.Add("Accept", "application/json, text/plain, */*");
                postRequest.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9");
                postRequest.Headers.Add("bx-v", "2.5.3");
                postRequest.Headers.Add("Cache-Control", "no-cache");
                postRequest.Headers.Add("Pragma", "no-cache");
                postRequest.Headers.Add("sec-ch-ua-mobile", "?0");
                postRequest.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
                postRequest.Headers.Add("Sec-Fetch-Dest", "empty");
                postRequest.Headers.Add("Sec-Fetch-Mode", "cors");
                postRequest.Headers.Add("Sec-Fetch-Site", "same-origin");
                postRequest.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/129.0.0.0 Safari/537.36");

                //var response = client.SendAsync(postRequest).Result;
                //var result = response.Content.ReadAsStringAsync().Result;
                //var jsonResult = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
                //var url = jsonResult["data"].ToString();

                //var aurl = JsonConvert.DeserializeObject<Dictionary<string, object>>(url);
                //var burl = aurl["url"].ToString();

                GetX5sec(url, client);
            }
        }






    }
}
