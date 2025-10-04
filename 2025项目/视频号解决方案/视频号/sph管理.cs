using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QRCoder;
using System;
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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace 视频号
{
    public partial class sph管理 : Form
    {
        public sph管理()
        {
            InitializeComponent();
        }

        private void sph管理_Load(object sender, EventArgs e)
        {

        }



        private static HttpClient _client;
        private static readonly string _cookieFile = "sphcookies.cookie";
        private static readonly string _qrCodeFile = "login_qrcode.png";
        private static readonly string _baseUri = "https://channels.weixin.qq.com";

        // 二维码登录流程
        private  async Task StartQrLogin(HttpClientHandler handler)
        {
            try
            {
                // 获取登录二维码
                var loginCodeUrl = _baseUri+"/cgi-bin/mmfinderassistant-bin/auth/auth_login_code";
                var data = new
                {
                    timestamp = GetTimestamp(),
                    _log_finder_id = "null",
                    rawKeyBuff = "null"
                };

                // 使用手动序列化代替PostAsJsonAsync
                var json = JsonConvert.SerializeObject(data);
              
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(loginCodeUrl, content);
                
                var result = await response.Content.ReadAsStringAsync();
               
                var jsonResult = JObject.Parse(result);
                var token = jsonResult["data"]["token"].ToString();
                var qrUrl = $"https://channels.weixin.qq.com/mobile/confirm_login.html?token={token}";

                // 生成并保存二维码
                GenerateQrCode(qrUrl);
                 textBox1.Text+=($"📷 二维码已生成：{Path.GetFullPath(_qrCodeFile)}");
                 textBox1.Text+=("请用微信扫描二维码进行登录");

                // 轮询登录状态
                var statusUrl = $"https://channels.weixin.qq.com/cgi-bin/mmfinderassistant-bin/auth/auth_login_status?token={token}&timestamp={GetTimestamp()}";
                while (true)
                {
                    var statusResponse = await _client.PostAsync(statusUrl, null);
                    var statusResult = await statusResponse.Content.ReadAsStringAsync();
                    var statusJson = JObject.Parse(statusResult);
                    var status = statusJson["data"]["status"].ToString();

                    switch (status)
                    {
                        case "0":
                            Console.Write(".");
                            break;
                        case "5":
                             textBox1.Text+=("\n🔍 已扫码，请在手机上确认");
                            break;
                        case "1":
                             textBox1.Text+=("\n✅ 登录成功");
                            // 登录成功后，发送一个请求获取完整Cookie
                            await GetUserInfoAfterLogin();

                            // 延迟一小段时间确保Cookie已被正确保存
                            await Task.Delay(1000);


                            if (statusResponse.Headers.TryGetValues("Set-Cookie", out var cookies))
                            {
                                textBox1.Text = "";
                                foreach (var cookie in cookies)
                                {
                                    textBox1.Text+=(cookie);
                                }
                            }
                            // 保存Cookies
                            SaveCookies(handler);
                            return;
                        default:
                             textBox1.Text+=($"\n❌ 登录失败，状态：{status}");
                            return;
                    }

                    await Task.Delay(2000);
                }
            }
            catch (Exception ex)
            {
                 textBox1.Text+=($"❌ 登录过程出错：{ex.Message}");
            }
        }

        // 生成二维码图片
        private  void GenerateQrCode(string url)
        {
           
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);

            using (var bitmap = qrCode.GetGraphic(10))
            {
              
                bitmap.Save(_qrCodeFile, ImageFormat.Png);
            }
            pictureBox1.Image = Image.FromFile(_qrCodeFile);
        }

        // 保存Cookies - 修复部分
        // 保存Cookies - 增加调试信息
        private void SaveCookies(HttpClientHandler handler)
        {
            try
            {
                // 获取指定Uri的Cookie集合
                var cookieCollection = handler.CookieContainer.GetCookies(new Uri(_baseUri));
                 textBox1.Text+=($"ℹ️ 检测到 {cookieCollection.Count} 个Cookie，正在保存...");

                if (cookieCollection.Count == 0)
                {
                     textBox1.Text+=("⚠️ 警告：未检测到任何Cookie，可能登录过程有问题");
                    return;
                }

                using (var sw = new StreamWriter(_cookieFile))
                {
                    foreach (Cookie cookie in cookieCollection)
                    {
                        // 输出Cookie信息用于调试
                         textBox1.Text+=($"📌 保存Cookie: {cookie.Name}={cookie.Value}");
                        sw.Write($"{cookie.Name}={cookie.Value};");
                    }
                }
                 textBox1.Text+=($"✅ Cookie保存成功，共 {cookieCollection.Count} 个");
            }
            catch (Exception ex)
            {
                 textBox1.Text+=($"❌ 保存Cookies失败：{ex.Message}");
            }
        }


        // 登录成功后获取用户信息，确保Cookie被正确设置
        private static async Task GetUserInfoAfterLogin()
        {
            try
            {
                var url = $"{_baseUri}/cgi-bin/mmfinderassistant-bin/auth/auth_finder_list";
                var data = new
                {
                    timestamp = GetTimestamp(),
                    _log_finder_id = "null",
                    rawKeyBuff = "null"
                };

                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await _client.PostAsync(url, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ 获取用户信息时出错：{ex.Message}");
            }
        }

        // 获取时间戳
        private static long GetTimestamp()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        private async void button1_Click(object sender, EventArgs e)
        {        
          await run();
        }


        async Task run()
        {


            var handler = new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = new CookieContainer(),
                ServerCertificateCustomValidationCallback = (s, cert, chain, err) => true,
                AllowAutoRedirect = true // 允许自动重定向，确保Cookie能正确传递
            };

            _client = new HttpClient(handler);
            _client.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36");
            // 添加Referer头，有些网站会验证
            _client.DefaultRequestHeaders.Referrer = new Uri(_baseUri);




            // 启动二维码登录流程
            await StartQrLogin(handler);

           

            // 清理生成的二维码图片
            if (File.Exists(_qrCodeFile))
            {
                try
                {
                    File.Delete(_qrCodeFile);
                }
                catch { }
            }
        }

      
    }
    }
