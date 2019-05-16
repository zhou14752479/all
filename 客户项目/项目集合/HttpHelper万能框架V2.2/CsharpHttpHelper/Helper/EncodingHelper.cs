using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsharpHttpHelper.Helper
{
    internal class EncodingHelper
    {
        /// <summary>
        /// 将字节数组转为字符串
        /// </summary>
        /// <param name="b">字节数组</param>
        /// <param name="e">编码，默认为Default</param>
        /// <returns></returns>
        internal static string ByteToString(byte[] b, Encoding e = null)
        {
            if (e == null)
            {
                e = Encoding.Default;
            }
            string result = e.GetString(b);
            return result;
        }

        /// <summary>
        /// 将字符串转为字节数组
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="e">编码，默认为Default</param>
        /// <returns></returns>
        internal static byte[] StringToByte(string s, Encoding e = null)
        {
            if (e == null)
            {
                e = Encoding.Default;
            }
            byte[] b = e.GetBytes(s);
            return b;
        }
    }
}
