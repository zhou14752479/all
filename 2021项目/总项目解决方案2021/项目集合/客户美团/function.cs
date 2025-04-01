using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 客户美团
{
    public class function
    {
        public static string GenerateSign(Dictionary<string, string> parameters, string secretKey) // 参数值强制为字符串
        {
            var sortedParams = new SortedDictionary<string, string>(parameters);
            var paramBuilder = new StringBuilder();
            foreach (var pair in sortedParams)
            {
                if (paramBuilder.Length > 0)
                    paramBuilder.Append("&");
                paramBuilder.Append($"{pair.Key}={pair.Value}");
            }
            string fullString = paramBuilder.ToString() + secretKey;
            Console.WriteLine("待签名字符串: " + fullString); // 输出验证

            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(fullString);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder signBuilder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    signBuilder.Append(b.ToString("x2"));
                }
                return signBuilder.ToString();
            }
        }
        public static void ceshi()
        {
            // 所有参数值强制为字符串类型
            var parameters = new Dictionary<string, string>
        {
            { "action", "api_b2b" },
            { "keyword", "装饰" },
            { "page", "1" },        // 使用字符串
            { "source", "1688" },
            { "time", "1739587078" } // 使用字符串
        };

            string secretKey = "secret";
            string sign = GenerateSign(parameters, secretKey);
            MessageBox.Show("生成的 Sign: " + sign);
            // 输出: b6b4e
        }




    }
    }
