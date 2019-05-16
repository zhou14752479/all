using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;

namespace CsharpHttpHelper.Helper
{
    internal class ImageHelper
    {
        /// <summary>
        /// 将字节数组转为图片
        /// </summary>
        /// <param name=" b">字节数组</param>
        /// <returns>返回图片</returns>
        internal static System.Drawing.Image ByteToImage(byte[] b)
        {
            try
            {
                MemoryStream ms = new MemoryStream(b);
                return Bitmap.FromStream(ms, true);
            }
            catch { return null; }
        }
    }
}
