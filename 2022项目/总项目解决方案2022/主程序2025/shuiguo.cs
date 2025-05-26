using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;

namespace 主程序2025
{
   public class shuiguo
    {

        private readonly HttpClient _httpClient = new HttpClient();
        private int _success = 0;
        private int _fail = 0;

        public  string GetX5Sec(string url)
        {
            try
            {
                // 获取验证码配置
                var captchaConfig = GetCaptchaConfig("https://rest-sig.imaitix.com//api/user/userLogin/_____tmd_____/punish?reqeust=getpunishpage&source=xagent&x5secdata=xcjdo7RF4AYoD1%2fIUrIzu0vTppBIukYTSzK25EjWDbyrA6tGu4DQSGRJnNas9sSvPGRL%2f0Q%2bJTLD3pW2DnBC4wVssE5Qf6w6tJvn9axCXsvUMDGMSkM%2flOA5pVnSE%2feCgFEghYbP7QJRnwSJPKToZ0avJR6zctlHSfyBsnwaEphtY96RU9MlBEqBc08jjo77sHcgfWc25MBa%2f5wlI6562Fy1CcO3hWb69egCzYpt2yo0QAGzrQTlV%2bhfJxg4epc6lL__bx__rest-sig.imaitix.com%2fapi%2fuser%2fuserLogin&x5step=2");
                Console.WriteLine(JsonConvert.SerializeObject(captchaConfig));
                //Logger.Info($"当前验证码类型:{captchaConfig["action"]}");

                // 构建验证码URL
                var captchaUrl = "https://" + captchaConfig["HOST"].ToString().Replace(":443", "") + captchaConfig["PATH"];

                // 获取水果滑块配置
                var (encryptToken, answerArea, puzzle) = GetFruitConfig(captchaUrl, captchaConfig);

                // 准备提交数据
                var data = new Dictionary<string, object>
            {
                { "captcha_config", captchaConfig },
                { "encryptToken", encryptToken },
                { "imageData", answerArea },
                { "ques", puzzle }
            };

                // 调用API
                 var content = new StringContent(JsonConvert.SerializeObject(data),
                    System.Text.Encoding.UTF8, "application/json");
                var rp = _httpClient.PostAsync("http://139.196.173.147:4200/tb_n", content).Result;
                var result = rp.Content.ReadAsStringAsync().Result;
                dynamic DATA = JsonConvert.DeserializeObject(result);

                // 准备请求头
                var requestUrl = DATA.urlEncode.ToString();
                var headers = new Dictionary<string, string>
            {
                { "bx_et", DATA.bx_et.ToString() },
                { "bx-pp", DATA.bx_pp.ToString() },
                { "referer", DATA.referer.ToString() }
            };


                // 添加User-Agent
                //_httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(DATA.ua.ToString());
              
                //_httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(uaaa);
                // 等待随机时间
                System.Threading.Thread.Sleep((int)(new Random().NextDouble() * 500 + 1000));

                // 禁用SSL验证
                ServicePointManager.ServerCertificateValidationCallback +=
                    (sender, cert, chain, sslPolicyErrors) => true;

                // 发送请求
                 var x5secRequest = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                foreach (var header in headers)
                {
                    x5secRequest.Headers.Add(header.Key, header.Value);
                }

                var x5secResponse = _httpClient.SendAsync(x5secRequest).Result;
                var responseText = x5secResponse.Content.ReadAsStringAsync().Result;
                //Console.WriteLine(responseText);

                // 检查结果
                if (x5secResponse.Headers.TryGetValues("Set-Cookie", out var cookies))
                {
                    foreach (var cookie in cookies)
                    {
                        if (cookie.Contains("x5sec"))
                        {
                            Logger.Success(cookies);
                            _success++;
                            MessageBox.Show(cookie);
                            return cookie;
                        }
                    }
                }

                if (responseText.Contains("\"code\":306"))
                {
                    Logger.Debug("识别距离问题!");
                }
                else
                {
                    Logger.Error("校验未通过!");
                }

                _fail++;
                return null;
            }
            catch (Exception ex)
            {
                Logger.Error($"处理过程中发生错误: {ex.ToString()}");
                _fail++;
                return null;
            }
        }

        public Dictionary<string, object> GetCaptchaConfig(string captchaUrl)
        {
            try
            {
                HttpClient client=new  HttpClient();
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
            catch (AggregateException ex) when (ex.InnerException is TaskCanceledException)
            {
                Console.WriteLine("请求超时");
                throw ex.InnerException;
            }
        }

        public (string encryptToken, string answerArea, string puzzle) GetFruitConfig(string punishUrl, Dictionary<string, object> config)
        {
            // 构建URL
            string url;
            if (config["action"].ToString()  == "captchacapslidev2" || config["action"].ToString() == "captchascene")
            {
                url = punishUrl + "/_____tmd_____/newslidecaptcha?";
            }
            else if (config["action"].ToString() == "captchacappuzzle")
            {
                url = punishUrl + "/_____tmd_____/puzzleCaptchaGet?";
            }
            else
            {
                throw new NotSupportedException($"不支持的验证码类型: {config["action"].ToString()}");
            }

            // 构建参数
            var parameters = new Dictionary<string, string>
        {
            { "token", config["NCTOKENSTR"].ToString() },
            { "appKey", config["NCAPPKEY"].ToString() },
            { "x5secdata", config["SECDATA"].ToString()  },
            { "language", "cn" },
            { "v", Guid.NewGuid().ToString("N") }
        };

            // 编码URL
            var encodedParams = string.Join("&", parameters
                .Select(p => $"{HttpUtility.UrlEncode(p.Key)}={HttpUtility.UrlEncode(p.Value)}"));
            var fullUrl = url + encodedParams;

            // 发送请求
            var response = _httpClient.GetStringAsync(fullUrl).Result;
            dynamic jsonResponse = JsonConvert.DeserializeObject(response);

            // 提取数据
            return (
                jsonResponse.data.encryptToken.ToString(),
                jsonResponse.data.imageData.ToString(),
                jsonResponse.data.ques.ToString()
            );
        }




        // 简单的日志记录器实现
        public static class Logger
        {
            public static void Info(string message) =>function.log($"[INFO] {message}");
            public static void Success(object data) => function.log($"[SUCCESS] {data}");
            public static void Debug(string message) => function.log($"[DEBUG] {message}");
            public static void Error(string message) => function.log($"[ERROR] {message}");
        }

    }
}
