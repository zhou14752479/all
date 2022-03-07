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

        public string cookie = "ASP.NET_SessionId=w1fhhk40ckspqxxndrd0v2lj; AspNetWebAuth=0C59DF5763CE9646DD7995AD5923A80DBF64EF9B2DA61DB43C5EE3E473E3372355BA0EEBE018BA97D55380C6FB5F311DE6DA4B45C83786D38611D9EB1FD09784A7EDA879B7179BA40DE0C4644EA04FB565E9188D24981074FBA077016E1B5DFCF2782E3AEFA2DD1B91CA26E472CD68F0F15902D2E6A4E3AEF1C58B437AB4F3077A0592067F541BD6B7FADFCA7B8C1AE25113B979CF78885612D86CAEA1C9CEA21F52D2374DC4B1DD15B57CE066B2FE918F37F6715A76365F43EE7998A3F6033FA24AE944D4E27ACE0CA558DB57E8BB766AF4D6C093BF7E0DC086E17093374EFBBC45A4977952B63C43CC2C0BAE0152EE5625917C9503AE03CA42A3FB9BC2DA3F204957B254C74338F01DB6406190721464862EA1AB254F98C3EB328699618D86B658622AF5ADE373269FDDAC8B6D3EEE5EB17E2D6185E2D0E30DA8F0902822DB262E09C3B6428C7D3AA501248A36711C326569CB4A89F0A59FF1ED326C591C5252E1E990FD36F3FEBF1C6DF11783C895439B8746F5A3B021458FD14AA5067C193A5E3AE35E0404463B4E3C41082038F7A6E1ACC0F052E8627562D0221E6A98C88B4EDA68F4825716C1BBCD1B0CA3DE55D97C20EA598AA8361654033D65E8D11ADFD10F22F6695076200B67FBB61DBA97B2661D23A6727F2F370E8B42574651A5F5406E1C48EA11BBFA3D305B1C52E80B601AF2836C7CCCE2B3AA8D1F9B477F45E23D905B0E346766A6F31F62A075C110F7AF4E2944495DE379A5F9BA7530CDCC203E19A8F04FA4C6C0B2E4B62EC24DB5AD9A5C081FED5B3FBD67561961E60B52B8FE5F776EB81C8C46E9871673887AC77DDFB6BBF9DAA3D35B9B2EA2E50BDD9A8B878E8E270E938F06A657534D4AF396166BE699B634B0A013B3833C29B99610768680152938524C5140C3C3F18F83E56F6B901ED4600A39D9D847497D7612EF29CBBF97D9545708A47F9FEBCCC0ED13B1DC98AEDC876DC0713B7B2CE6417F4E05760A953C31FD9D4092ECE3C7AE60257D18036EEE397A9ABBE73CA015B6E6AC6BEA6F61B7E719BB5BD17936DBEE3CF979CDF0FDED5AE4F88CAD72097D51120C7A135D951ECF8C0963F4569B399798E1F3507CCE3624D3BA9E01E032D057F923258DC6E25A6F09B08B7EE09B3ACB377F";
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
                request.Headers.Set(HttpRequestHeader.Cookie, cookie);

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
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MakeRequests();
        }
    }
}
