using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace imagetotxt
{
    class imagetotxtMethod
    {
        [DllImport("AspriseOCR.dll", EntryPoint = "OCR")]
        public static extern IntPtr OCR(string file, int type);

        [DllImport("AspriseOCR.dll", EntryPoint = "OCRpart")]
        static extern IntPtr OCRpart(string file, int type, int startX, int startY, int width, int height);

        [DllImport("AspriseOCR.dll", EntryPoint = "OCRBarCodes")]
        static extern IntPtr OCRBarCodes(string file, int type);

        [DllImport("AspriseOCR.dll", EntryPoint = "OCRpartBarCodes")]
        static extern IntPtr OCRpartBarCodes(string file, int type, int startX, int startY, int width, int height);
        [DllImport("AspriseOCR.dll")]
        static extern string craboOCR(string file, int type);

        private string GetVeryfyCode(string _imgPath)
        {
            if (File.Exists(_imgPath))//ok now?
            {
                try
                {

                    string _veryfyCode = craboOCR(_imgPath, -1);   //将返回string,并以"\r\n"结尾!!
                    return _veryfyCode;

                }
                catch (Exception e)
                {
                    return e.ToString();
                }
            }
            else
            {
                return "图片路径不存在";
            }
        }


    }
}
