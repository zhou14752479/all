using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace fang
{
    class AES
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

            RijndaelManaged rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            //string ss = resultArray.ToHexString();
            //return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            return resultArray.ToHexString();
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Decrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            //byte[] toEncryptArray = Convert.FromBase64String(str);
            byte[] toEncryptArray = str.ToByteArr();
            RijndaelManaged rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cTransform = rm.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Encoding.UTF8.GetString(resultArray);
        }
    }



    public static class Util
    {
        public static string ToHexString(this byte[] bytes)
        {
            string byteStr = string.Empty;
            if (bytes != null || bytes.Length > 0)
            {
                foreach (var item in bytes)
                {
                    byteStr += string.Format("{0:X2}", item);

                }
            }
            return byteStr;
        }
        public static byte[] ToByteArr(this string hex)
        {
            var inputByteArray = new byte[hex.Length / 2];
            for (var x = 0; x < inputByteArray.Length; x++)
            {
                var i = Convert.ToInt32(hex.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            return inputByteArray;
        }


    }



}
