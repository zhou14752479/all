using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace helper
{
    public class CodeHelper
    {
        public CodeHelper() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ci">验证码配置信息</param>
        public CodeHelper(CodeInfo ci)
        {
            m_ci = ci;
        }

        private CodeInfo m_ci;              //验证码配置信息

        private BitmapData m_bmpData;       //位图缓存数据
        private byte[] m_byColorInfo;       //位图RGB值缓存

        public void LoadCodeInfo(CodeInfo ci)
        {
            m_ci = ci;
        }

        public void LoadCodeInfo(string strFileName)
        {
            m_ci = CodeInfo.LoadFromFile(strFileName);
        }
        /// <summary>
        /// 传入验证码图片返回验证码字符
        /// </summary>
        /// <param name="imgSrc"></param>
        /// <returns>验证码字符窜</returns>
        public string GetCodeString(Image imgSrc)
        {
            if (m_ci == null) throw new Exception("Please load CodeInfo");
            if (m_ci.GetChars().Count == 0) throw new Exception("Can not found font libary");
            string strCode = string.Empty;
            int nBlackCount = 0, nWhiteCount = 0;
            Image img_dark = null;
            List<Rectangle> lstChar = null;
            if (m_ci.IsAutoSelectRect) m_ci.CodeCount = m_ci.CodeRectangles.Count;
            for (int i = 0, len = m_ci.CodeCount == 0 ? 1 : m_ci.BinaryValues.Length; i < len; i++)
            {
                img_dark = this.GetBinaryImage(imgSrc, m_ci.BinaryValues[i], out nBlackCount, out nWhiteCount, m_ci.Express);
                if (nBlackCount > nWhiteCount) img_dark = this.ReverseColors(img_dark);
                if (m_ci.RectangleCut.Size != img_dark.Size)
                {
                    Bitmap bmp = new Bitmap(m_ci.RectangleCut.Width, m_ci.RectangleCut.Height);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.DrawImage(img_dark, new Rectangle(Point.Empty, bmp.Size), m_ci.RectangleCut, GraphicsUnit.Pixel);
                        img_dark = bmp;
                    }
                }
                lstChar = this.GetCharRect((Bitmap)img_dark, m_ci.PixelMin, m_ci.PixelMax);
                if (lstChar.Count == m_ci.CodeCount) break;
            }
            if (!m_ci.IsAutoSelectRect)
                lstChar = m_ci.CodeRectangles;
            foreach (var v in lstChar)
            {
                strCode += this.GetBestSameChar((Bitmap)img_dark, v);
            }
            return strCode.Replace("-", "");
        }
        /// <summary>
        /// 获取图像的二值化图像(不会操作原图)
        /// </summary>
        /// <param name="imgSrc">原始图像</param>
        /// <param name="byValue">二值化阀值</param>
        /// <param name="nBlackCount">返回二值化后黑点个数</param>
        /// <param name="nWhiteCount">返回二值化后白点个数</param>
        /// <param name="lstExp">条件表达式</param>
        /// <returns>二值化后的图像</returns>
        public Image GetBinaryImage(Image imgSrc, byte byValue, out int nBlackCount, out int nWhiteCount, List<string> lstExp = null)
        {
            nBlackCount = 0;
            nWhiteCount = 0;
            Bitmap b = new Bitmap(imgSrc);
            Bitmap bmp = b.Clone(new Rectangle(0, 0, imgSrc.Width, imgSrc.Height), PixelFormat.Format24bppRgb);
            b.Dispose();
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            byte[] byColorInfo = new byte[bmp.Height * bmpData.Stride];
            Marshal.Copy(bmpData.Scan0, byColorInfo, 0, byColorInfo.Length);
            byte byR, byG, byB;
            for (int x = 0, xLen = bmp.Width; x < xLen; x++)
            {
                for (int y = 0, yLen = bmp.Height; y < yLen; y++)
                {
                    byB = byColorInfo[y * bmpData.Stride + x * 3];
                    byG = byColorInfo[y * bmpData.Stride + x * 3 + 1];
                    byR = byColorInfo[y * bmpData.Stride + x * 3 + 2];
                    byte byV = GetAvg(byR, byG, byB);
                    if (byR == 0 && byG == 0)
                    {
                        int a = 0;
                        a++;
                    }
                    if (lstExp != null)
                    {
                        foreach (var v in lstExp)
                        {
                            string[] strTemps = v.Split('-');
                            if (this.ExecExpress(strTemps[1]
                                .Replace("R", byR.ToString())
                                .Replace("G", byG.ToString())
                                .Replace("B", byB.ToString())
                                .Replace("V", byV.ToString())))
                            {
                                byV = strTemps[0] == "W" ? byV = 255 : byV = 0;
                                break;
                            }
                        }
                    }
                    if (byV >= byValue)
                    {
                        byV = 255;
                        nWhiteCount++;
                    }
                    else
                    {
                        byV = 0;
                        nBlackCount++;
                    }
                    byColorInfo[y * bmpData.Stride + x * 3] =
                        byColorInfo[y * bmpData.Stride + x * 3 + 1] =
                        byColorInfo[y * bmpData.Stride + x * 3 + 2] = byV;
                }
            }
            Marshal.Copy(byColorInfo, 0, bmpData.Scan0, byColorInfo.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }
        /// <summary>
        /// 对图像进行反射图里(操作原图)
        /// </summary>
        /// <param name="imgSrc"></param>
        /// <returns>反色后图像</returns>
        public Image ReverseColors(Image imgSrc)
        {
            Bitmap bmp = (Bitmap)imgSrc;
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte[] byColorInfo = new byte[bmp.Height * bmpData.Stride];
            Marshal.Copy(bmpData.Scan0, byColorInfo, 0, byColorInfo.Length);
            for (int x = 0, xLen = bmp.Width; x < xLen; x++)
            {
                for (int y = 0, yLen = bmp.Height; y < yLen; y++)
                {
                    byColorInfo[y * bmpData.Stride + x * 3] = (byte)(255 - byColorInfo[y * bmpData.Stride + x * 3]);
                    byColorInfo[y * bmpData.Stride + x * 3 + 1] = (byte)(255 - byColorInfo[y * bmpData.Stride + x * 3 + 1]);
                    byColorInfo[y * bmpData.Stride + x * 3 + 2] = (byte)(255 - byColorInfo[y * bmpData.Stride + x * 3 + 2]);
                }
            }
            Marshal.Copy(byColorInfo, 0, bmpData.Scan0, byColorInfo.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }
        /// <summary>
        /// 获取验证码字符所在的区域
        /// </summary>
        /// <param name="imgDark">二值化后的图像</param>
        /// <param name="nPixelMin">连续的最小像素个数 小于此数将被忽略</param>
        /// <param name="nPixelMax">连续的最大像素个数 大于此数将被忽略</param>
        /// <returns></returns>
        public List<Rectangle> GetCharRect(Image imgDark, int nPixelMin, int nPixelMax)
        {
            Bitmap bmp = (Bitmap)imgDark;
            m_bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            m_byColorInfo = new byte[bmp.Height * m_bmpData.Stride];
            Marshal.Copy(m_bmpData.Scan0, m_byColorInfo, 0, m_byColorInfo.Length);
            List<Rectangle> lstRect = new List<Rectangle>();
            for (int y = 0, leny = bmp.Height; y < leny; y++)
            {
                for (int x = 0, lenx = bmp.Width; x < lenx; x++)
                {
                    if (this.GetArgbFormByColor(x, y) == Color.Black.ToArgb())
                    {
                        Rectangle rectTemp = this.GetRectFromPoint(nPixelMin, nPixelMax, new Point(x, y), Color.Black, Color.Blue);
                        if (rectTemp != Rectangle.Empty) lstRect.Add(rectTemp);
                    }
                }
            }
            Marshal.Copy(m_byColorInfo, 0, m_bmpData.Scan0, m_byColorInfo.Length);
            bmp.UnlockBits(m_bmpData);
            //将区域按照left属性排序
            for (int i = 0; i < lstRect.Count; i++)
            {
                for (int j = 1; j < lstRect.Count - i; j++)
                {
                    if (lstRect[j - 1].Left > lstRect[j].Left)
                    {
                        Rectangle rectTemp = lstRect[j];
                        lstRect[j] = lstRect[j - 1];
                        lstRect[j - 1] = rectTemp;
                    }
                }
            }
            return lstRect;
        }
        /// <summary>
        /// 更具起始坐标点获取该点的联通区域
        /// </summary>
        /// <param name="nPixelMin">连续的最小像素个数 小于此数将被忽略</param>
        /// <param name="nPixelMax">连续的最大像素个数 大于此数将被忽略</param>
        /// <param name="ptStart">起始点位置</param>
        /// <param name="clrSrc">点的颜色</param>
        /// <param name="clrSet">标记颜色</param>
        /// <returns>目标点的连通区域</returns>
        private Rectangle GetRectFromPoint(int nPixelMin, int nPixelMax, Point ptStart, Color clrSrc, Color clrSet)
        {
            int nCount = 0;
            int nArgb = clrSrc.ToArgb();
            Rectangle rect = new Rectangle(ptStart.X, ptStart.Y, 0, 0);
            List<Point> ptRegList = new List<Point>();
            List<Point> lstTemp = new List<Point>();
            ptRegList.Add(ptStart);
            lstTemp.Add(ptStart);
            this.SetColorFormByColorInfo(ptStart.X, ptStart.Y, clrSet);
            while (ptRegList.Count != 0)
            {
                Point ptTemp = this.GetNextPoint(ptRegList[ptRegList.Count - 1], nArgb);
                //Point ptTemp = ptRegList[ptRegList.Count - 1];
                if (ptTemp != Point.Empty)
                {
                    ptRegList.Add(ptTemp);
                    lstTemp.Add(ptTemp);
                    this.SetColorFormByColorInfo(ptTemp.X, ptTemp.Y, clrSet);
                    nCount++;
                    if (ptTemp.X < rect.Left) { rect.Width = rect.Right - ptTemp.X; rect.X = ptTemp.X; }
                    if (ptTemp.Y < rect.Top) { rect.Height = rect.Bottom - ptTemp.Y; rect.Y = ptTemp.Y; }
                    if (ptTemp.X > rect.Right) rect.Width = ptTemp.X - rect.Left;
                    if (ptTemp.Y > rect.Bottom) rect.Height = ptTemp.Y - rect.Top;
                }
                else
                    ptRegList.RemoveAt(ptRegList.Count - 1);
            }
            rect.Width += 1; rect.Height += 1;
            if (nCount < nPixelMin || nCount > nPixelMax)
            {
                foreach (var v in lstTemp)
                {
                    this.SetColorFormByColorInfo(v.X, v.Y, Color.White);
                }
                return Rectangle.Empty;
            }
            return rect;
        }
        /// <summary>
        /// 获取下一个连通像素点
        /// </summary>
        /// <param name="ptStart">当前位置</param>
        /// <param name="nArgb">当前像素颜色</param>
        /// <returns>下一个点的坐标</returns>
        private Point GetNextPoint(Point ptStart, int nArgb)
        {
            if (m_bmpData.Height > ptStart.Y + 1 && this.GetArgbFormByColor(ptStart.X, ptStart.Y + 1) == nArgb)
                return new Point(ptStart.X, ptStart.Y + 1);
            if (m_bmpData.Width > ptStart.X + 1 && this.GetArgbFormByColor(ptStart.X + 1, ptStart.Y) == nArgb)
                return new Point(ptStart.X + 1, ptStart.Y);
            if (0 <= ptStart.Y - 1 && this.GetArgbFormByColor(ptStart.X, ptStart.Y - 1) == nArgb)
                return new Point(ptStart.X, ptStart.Y - 1);
            if (0 <= ptStart.X - 1 && this.GetArgbFormByColor(ptStart.X - 1, ptStart.Y) == nArgb)
                return new Point(ptStart.X - 1, ptStart.Y);
            if (0 <= ptStart.X - 1 && m_bmpData.Height > ptStart.Y + 1 && this.GetArgbFormByColor(ptStart.X - 1, ptStart.Y + 1) == nArgb)
                return new Point(ptStart.X - 1, ptStart.Y + 1);
            if (m_bmpData.Width > ptStart.X + 1 && m_bmpData.Height > ptStart.Y + 1 && this.GetArgbFormByColor(ptStart.X + 1, ptStart.Y + 1) == nArgb)
                return new Point(ptStart.X + 1, ptStart.Y + 1);
            if (m_bmpData.Width > ptStart.X + 1 && 0 <= ptStart.Y - 1 && this.GetArgbFormByColor(ptStart.X + 1, ptStart.Y - 1) == nArgb)
                return new Point(ptStart.X + 1, ptStart.Y - 1);
            if (0 <= ptStart.X - 1 && 0 <= ptStart.Y - 1 && this.GetArgbFormByColor(ptStart.X - 1, ptStart.Y - 1) == nArgb)
                return new Point(ptStart.X - 1, ptStart.Y - 1);
            return Point.Empty;
        }
        /// <summary>
        /// 获取r g b平均值
        /// </summary>
        /// <param name="b">blue</param>
        /// <param name="g">green</param>
        /// <param name="r">red</param>
        /// <returns>平均值</returns>
        private byte GetAvg(byte b, byte g, byte r)
        {
            return (byte)((r + g + b) / 3);
        }
        /// <summary>
        /// 从缓存中获取图像指定坐标的ARGB值
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        /// <returns>ARGB值</returns>
        private int GetArgbFormByColor(int x, int y)
        {
            return Color.FromArgb(255,
                 m_byColorInfo[y * m_bmpData.Stride + x * 3],
                 m_byColorInfo[y * m_bmpData.Stride + x * 3 + 1],
                 m_byColorInfo[y * m_bmpData.Stride + x * 3 + 2]).ToArgb();
        }
        /// <summary>
        /// 从缓存设置图像指定坐标的颜色
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y组表</param>
        /// <param name="clr">要设置的颜色</param>
        private void SetColorFormByColorInfo(int x, int y, Color clr)
        {
            m_byColorInfo[y * m_bmpData.Stride + x * 3] = clr.B;
            m_byColorInfo[y * m_bmpData.Stride + x * 3 + 1] = clr.G;
            m_byColorInfo[y * m_bmpData.Stride + x * 3 + 2] = clr.R;
        }
        /// <summary>
        /// 返回文字表达式的执行结果
        /// </summary>
        /// <param name="strExp">表达式字符串</param>
        /// <returns>布尔值</returns>
        private bool ExecExpress(string strExp)
        {
            List<bool> lst = new List<bool>();
            foreach (Match m in Regex.Matches(strExp, @"(\d+)(.)(\d+)"))
            {
                switch (m.Groups[2].Value)
                {
                    case "<": lst.Add(int.Parse(m.Groups[1].Value) < int.Parse(m.Groups[3].Value)); break;
                    case ">": lst.Add(int.Parse(m.Groups[1].Value) > int.Parse(m.Groups[3].Value)); break;
                    case "=": lst.Add(m.Groups[1].Value == m.Groups[3].Value); break;
                }
            }
            int index = 0;
            foreach (Match m in Regex.Matches(strExp, @"&|\|"))
            {
                switch (m.Value)
                {
                    case "|": lst[index + 1] |= lst[index]; break;
                    case "&": lst[index + 1] &= lst[index]; break;
                }
                index++;
            }
            return lst[lst.Count - 1];
        }
        /// <summary>
        /// 从样本中获取最相似的图像并返回字符
        /// </summary>
        /// <param name="imgDarkCodeImage">验证码图像</param>
        /// <param name="rect">字符所在的区域</param>
        /// <returns>最相识的字符</returns>
        private char GetBestSameChar(Image imgDarkCodeImage, Rectangle rect)
        {
            char chCode = '0';
            using (Bitmap bmpChar = new Bitmap(rect.Width, rect.Height))
            {
                using (Graphics g = Graphics.FromImage(bmpChar))
                {
                    g.DrawImage(imgDarkCodeImage, new Rectangle(0, 0, rect.Width, rect.Height), rect, GraphicsUnit.Pixel);
                    int nSameCount = 0;
                    foreach (var v in m_ci.GetChars())
                    {
                        int nTemp = this.GetBestSameCount(bmpChar, v);
                        if (nTemp > nSameCount) { nSameCount = nTemp; chCode = v; }
                    }
                }
            }
            return chCode;
        }
        /// <summary>
        /// 获取验证码字符图在字符ch的模版中最高重叠率的像素
        /// </summary>
        /// <param name="imgDark">验证码字符图像</param>
        /// <param name="ch"></param>
        /// <returns>在ch模版中最高重叠的个数</returns>
        private int GetBestSameCount(Image imgDark, char ch)
        {
            List<Image> lstBmpModel = m_ci.GetCodeImages(ch);
            if (lstBmpModel == null) return 0;
            int nSameCount = 0;
            for (int j = 0, lenj = lstBmpModel.Count; j < lenj; j++)
            {
                int nTemp = this.CmpImage(imgDark, lstBmpModel[j]);
                if (nTemp > nSameCount)
                {
                    nSameCount = nTemp;
                }
            }
            return nSameCount;
        }
        /// <summary>
        /// 将imgB以imgA的大小重叠在一起比对
        /// </summary>
        /// <param name="imgA">目标图片</param>
        /// <param name="imgB">模版图片</param>
        /// <returns>非白色像素点的重叠个数</returns>
        private int CmpImage(Image imgA, Image imgB)
        {
            int nCount = 0;
            using (Bitmap bmpTemp = new Bitmap(imgA.Width, imgA.Height))
            {
                using (Graphics g = Graphics.FromImage(bmpTemp))
                {
                    g.DrawImage(imgB, 0, 0, imgA.Width, imgA.Height);

                    BitmapData bmpDataA = ((Bitmap)imgA).LockBits(new Rectangle(0, 0, imgA.Width, imgA.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                    byte[] byColorInfoA = new byte[imgA.Height * bmpDataA.Stride];
                    Marshal.Copy(bmpDataA.Scan0, byColorInfoA, 0, byColorInfoA.Length);

                    BitmapData bmpDataB = bmpTemp.LockBits(new Rectangle(0, 0, bmpTemp.Width, bmpTemp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                    byte[] byColorInfoB = new byte[bmpTemp.Height * bmpDataB.Stride];
                    Marshal.Copy(bmpDataB.Scan0, byColorInfoB, 0, byColorInfoB.Length);

                    for (int x = 0, xLen = imgA.Width; x < xLen; x++)
                    {
                        for (int y = 0, yLen = imgA.Height; y < yLen; y++)
                        {
                            byte byA = (byte)(GetAvg(
                                byColorInfoA[y * bmpDataA.Stride + x * 3],
                                byColorInfoA[y * bmpDataA.Stride + x * 3 + 1],
                                byColorInfoA[y * bmpDataA.Stride + x * 3 + 2]) != 255 ? 0 : 255);
                            byte byB = (byte)(GetAvg(
                                byColorInfoB[y * bmpDataB.Stride + x * 3],
                                byColorInfoB[y * bmpDataB.Stride + x * 3 + 1],
                                byColorInfoB[y * bmpDataB.Stride + x * 3 + 2]) != 255 ? 0 : 255);
                            if (byA == byB) nCount++;
                        }
                    }
                    Marshal.Copy(byColorInfoA, 0, bmpDataA.Scan0, byColorInfoA.Length);
                    ((Bitmap)imgA).UnlockBits(bmpDataA);
                    Marshal.Copy(byColorInfoB, 0, bmpDataB.Scan0, byColorInfoB.Length);
                    bmpTemp.UnlockBits(bmpDataB);
                }
            }
            return nCount;
        }
    }
}
