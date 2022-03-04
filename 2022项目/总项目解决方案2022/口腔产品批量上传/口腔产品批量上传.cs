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

namespace 口腔产品批量上传
{
    public partial class 口腔产品批量上传 : Form
    {
        public 口腔产品批量上传()
        {
            InitializeComponent();
        }
        //http://auth.dentsplysirona-cn.cn/Account/SubmitLogin?username=C7351631&usertype=0&password=182163&returnUrl=%2Faccount%2FFactoryLogin

        private void MakeRequests()
        {
            HttpWebResponse response;

            if (Request_auth_dentsplysirona_cn_cn(out response))
            {
                response.Close();
            }
        }

        private static void WriteMultipartBodyToRequest(HttpWebRequest request, string body)
        {
            string[] multiparts = Regex.Split(body, @"<!>");
            byte[] bytes;
            using (MemoryStream ms = new MemoryStream())
            {
                foreach (string part in multiparts)
                {
                    if (File.Exists(part))
                    {
                        bytes = File.ReadAllBytes(part);
                    }
                    else
                    {
                        bytes = System.Text.Encoding.UTF8.GetBytes(part.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n"));
                    }

                    ms.Write(bytes, 0, bytes.Length);
                }

                request.ContentLength = ms.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    ms.WriteTo(stream);
                }
            }
        }

        private bool Request_auth_dentsplysirona_cn_cn(out HttpWebResponse response)
        {
            response = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://auth.dentsplysirona-cn.cn/ProFactory/ProYhg/ImportXls");

                request.KeepAlive = true;
                request.Headers.Set(HttpRequestHeader.CacheControl, "max-age=0");
                request.Headers.Add("Upgrade-Insecure-Requests", @"1");
                request.Headers.Add("Origin", @"http://auth.dentsplysirona-cn.cn");
                request.ContentType = "multipart/form-data; boundary=----WebKitFormBoundaryVB4ZO7NqyCjNWHze";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                request.Referer = "http://auth.dentsplysirona-cn.cn/ProFactory/ProYhg";
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "zh,sq;q=0.9,zh-CN;q=0.8,oc;q=0.7,de;q=0.6,en;q=0.5");
                request.Headers.Set(HttpRequestHeader.Cookie, @"ASP.NET_SessionId=zy1lj42df41j2ht2etxlny0r; AspNetWebAuth=C91EE6AD246018739CEDEC4622A875E62D450D45E362DF4A41020667A7CF9B87F11F62B5BCEB6C7DB216881105C4A87EE168258CD773C979CDF0B4EB1C80A1708C2495576A73F8F7C541820B2F14101E416685A9C74712620CB9296202CE103F577D8450A8F3A08331045E0856C4EB53ED6FA814375345028255BF4425807C7193B126430825EE3592D6E9998907B5326AA84B45FAA447F0A267423C13F850ACED8D7204D2DBE9B16706CAD31DEAEFB4213B1DD812893EE96FC335DE536E553F25013A053C182E61E83C1B277EBC68ED757A72142A684C523AFEEC9BB6C97449F0E163CDC4D1F0C395D2C5A240B698E0B3197192BDD52AB3C1361F27B583A9F0DA4AB5A7A2F5C4FED8C338E1CC0E75479319B979C74D3E22726734CF9CD9AF5BA815313123F515B18464FE22255A4CC4956B19EEF415D23FB94742EE9BCDD5DE972B9144EF0615F687747F180C1B081DA1971F4FE12DD9296527159B65736D6B2DD6974D4BC2D8B712AD3E0644703A32AE2FE8B720EF539B55B2623349128CF47DB8BF8466637C94647AE4EB3EAFC9F7D2DDA3A6E9D8FD991ED19A546C0025167C9E85AC845A2773C955F0D8B92C0070299236C7B762711E2426C64B8F77BE1F4EE69F649F896B8A14C75BF80F328DDF53F9F0F5139057C3D47B308236FDD0256A7928DACA5FFB9EB8F4A5EF6AFC4842EE20F469A9DB3160299F34580522C1D2E71900A6B810235B5D9F3531C862EE951D14A474E9FECC469FAF43945BBB2AA784F05FC83073EA7120833C8238FA71A6FE5483606B21F807DC8E398D726311C6C0B174D3DB6B1F277EA45C1646E5FDDDB7A626A0DF12312AB6DB230E3049C4327E6C9F661083A695E4849E2E99DF87B2929FCC2639E1729403121C6835285872D9269526AA8908FB6609FC0B63CE8072C9F2933AC2A394DCE5FB5E614D7AAD8C952DE8E7E6B24D913E588C14A5DC9B2D8B97134E17280FF38C0783677298DA96A5742433C18D7DE769C5B4D78E78F1EC2D2C56CF930A77ABCC346E7C79B22B6EFF739CD585795BA32AA7BB4BB0722691968917663472C017C6E708FBE2A81606C463F3D103F3B9BF9606740F32DBDA593AD474552294DF38DB890B19B6DE298830B9960D1E443B1B0710E821D1FD8EE6");

                request.Method = "POST";
                request.ServicePoint.Expect100Continue = false;

                string body = @"------WebKitFormBoundaryVB4ZO7NqyCjNWHze
Content-Disposition: form-data; name=""filedata""; filename=""上传测试.xls""
Content-Type: application/vnd.ms-excel

<!>C:\Users\zhou\AppData\Local\Temp\10\sp3hoqka\上传测试.xls<!>
------WebKitFormBoundaryVB4ZO7NqyCjNWHze--
";
                WriteMultipartBodyToRequest(request, body);

                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
                else return false;
            }
            catch (Exception)
            {
                if (response != null) response.Close();
                return false;
            }

            return true;
        }

        private void 口腔产品批量上传_Load(object sender, EventArgs e)
        {
            MakeRequests();
        }
    }
}
