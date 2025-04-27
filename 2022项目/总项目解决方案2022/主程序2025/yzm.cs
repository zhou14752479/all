using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序2025
{
    internal class yzm
    {

        private static HttpClientHandler handler = new HttpClientHandler
        {
            UseCookies = true,
            CookieContainer = new CookieContainer()
        };

        private static HttpClient httpClient = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        static JObject GetCaptchaConfig(string captchaUrl)
        {
            var response = httpClient.GetAsync(captchaUrl).Result;
            var responseBody = response.Content.ReadAsStringAsync().Result;

            var regex = new Regex(@"window\._config_ = ({.*?});", RegexOptions.Singleline);
            var match = regex.Match(responseBody);
            if (!match.Success)
            {
                throw new Exception($"验证码配置错误：{responseBody}");
            }

            return JObject.Parse(match.Groups[1].Value);
        }

        static string GetX5Sec(string url, ref int cgcs, ref int sbcs)
        {
            try
            {
                // 获取验证码配置
                var config = GetCaptchaConfig(url);

                // 调用外部验证服务
                var postUrl = "http://39.98.67.180:5555/Getali?id=468e981f23aed45611ea5ce75c45e50e";
                var content = new StringContent(config.ToString(), Encoding.UTF8, "application/json");

                var response = httpClient.PostAsync(postUrl, content).Result;
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var ret2Json = JObject.Parse(responseContent);

                if (ret2Json["code"].ToString() != "0")
                {
                    sbcs++;
                    return string.Empty;
                }

                // 准备请求头
                var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    ret2Json["hkurl"].ToString()
                );

                request.Headers.Add("Bx_et", ret2Json["bx_et"].ToString());
                request.Headers.Add("Bx-Pp", ret2Json["bx-pp"].ToString());
                request.Headers.Referrer = new Uri($"https://{config["HOST"].ToString().Replace(":443", "")}");
                request.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.1.0 Safari/537.36");

                // 发送请求
                var getResponse = httpClient.SendAsync(request).Result;
                var responseBody = getResponse.Content.ReadAsStringAsync().Result;
                Console.WriteLine(responseBody);

                // 处理Cookie
                string x5sec = "";
                if (getResponse.Headers.TryGetValues("Set-Cookie", out var cookies))
                {
                    foreach (var cookie in cookies)
                    {
                        var match = Regex.Match(cookie, @"x5sec=([^;]+)");
                        if (match.Success)
                        {
                            x5sec = match.Groups[1].Value;
                            break;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(x5sec))
                {
                    cgcs++;
                    //MessageBox.Show($"协议头:x5sec={x5sec}");
                    return "x5sec="+x5sec;
                }

                sbcs++;
               // MessageBox.Show("获取x5sec失败");
                return string.Empty;
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"发生异常: {ex.Message}");
                sbcs++;
                return string.Empty;
            }
        }

       public static string getx5(string url)
        {
            int cgcs = 0;
            int sbcs = 0;
            var stopwatch = Stopwatch.StartNew();

            //var url = "https://ditu.amap.com/detail/get/detail";
         string x5=   GetX5Sec(url, ref cgcs, ref sbcs);
            return x5;
        }
    }





}
