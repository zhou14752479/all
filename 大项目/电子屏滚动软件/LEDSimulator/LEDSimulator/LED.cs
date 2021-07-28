using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace LEDSimulator
{
    class LED
    {
        Bitmap m_Srcmap;
        Bitmap m_Desmap;
        int m_iCellSize;
        int m_iBorderSize;
         
        public LED()
        {
            m_Srcmap = new Bitmap(200, 80);
        }

        //设置文字
        public void SetText(string strText, int Desmapwidth, int Desmapheight)
        {
           
            m_Desmap = new Bitmap(Desmapwidth, Desmapheight);
            m_iCellSize = 3;
            m_iBorderSize = 1;

            Graphics g = Graphics.FromImage(this.m_Srcmap);
            g.FillRectangle(Brushes.DarkGreen, 0, 0, this.m_Srcmap.Width, this.m_Srcmap.Height);
            SolidBrush brush = new SolidBrush(Color.FromArgb(20, 255, 20));
            g.DrawString(strText, new Font("Arial", 9f), brush, 1, 1);
            g.Dispose();

            //根据srcBitmap绘制desBitmap
            BitmapData desData = m_Desmap.LockBits(
                new Rectangle(0, 0, m_Desmap.Width, m_Desmap.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);
            

            //获取源图的bmdata
            BitmapData srcData = m_Srcmap.LockBits(
                new Rectangle(0, 0, m_Srcmap.Width, m_Srcmap.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb
                );
            //扫描行宽度
            int desStride = desData.Stride;

            int srcStride = srcData.Stride;

            unsafe
            {

                byte* pDest = (byte*)(void*)desData.Scan0;
                byte* pSrc = (byte*)(void*)srcData.Scan0;	//源图

                for (int i = 0; i < m_Srcmap.Width; ++i)
                {
                    for (int j = 0; j < m_Srcmap.Height; ++j)
                    {
                        int widthStart = i * (m_iCellSize + m_iBorderSize);
                        int widthEnd = i * (m_iCellSize + m_iBorderSize) + m_iCellSize;
                        int heightStart = j * (m_iCellSize + m_iBorderSize);
                        int heightEnd = j * (m_iCellSize + m_iBorderSize) + m_iCellSize;

                        for (int l = heightStart; (l < heightEnd && l < m_Desmap.Height); ++l)
                        {
                            for (int k = widthStart; (k < widthEnd && k < m_Desmap.Width); ++k)
                            {
                                pDest[desStride * l + k * 3] = pSrc[srcStride * j + i * 3];		//B
                                pDest[desStride * l + k * 3 + 1] = pSrc[srcStride * j + i * 3 + 1];	//G
                                pDest[desStride * l + k * 3 + 2] = pSrc[srcStride * j + i * 3 + 2];	//R
                            }
                        }
                    }
                } 
                
                m_Srcmap.UnlockBits(srcData);
                m_Desmap.UnlockBits(desData);
            }

           

        }

        //循环移动
        public void MoveStep()
        {
            //根据srcBitmap绘制desBitmap
            BitmapData desData = m_Desmap.LockBits(
                new Rectangle(0, 0, m_Desmap.Width, m_Desmap.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);


            //获取源图的bmdata
            BitmapData srcData = m_Srcmap.LockBits(
                new Rectangle(0, 0, m_Srcmap.Width, m_Srcmap.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb
                );
            //扫描行宽度
            int desStride = desData.Stride;

            int srcStride = srcData.Stride;

            unsafe
            {

                byte* pDest = (byte*)(void*)desData.Scan0;
                byte* pSrc = (byte*)(void*)srcData.Scan0;	//源图

                //循环向前移动
                for (int j = 0; j < m_Srcmap.Height; ++j)
                {                    
                    for (int i = 0; i < m_Srcmap.Width; ++i)
                    {
                        pSrc[srcStride * j + i * 3]     = pSrc[srcStride * j + ((i + 1) % m_Srcmap.Width) * 3];		//B
                        pSrc[srcStride * j + i * 3 + 1] = pSrc[srcStride * j + ((i + 1) % m_Srcmap.Width) * 3 + 1];	//G
                        pSrc[srcStride * j + i * 3 + 2] = pSrc[srcStride * j + ((i + 1) % m_Srcmap.Width) * 3 + 2];	//R
                    }
                }

                for (int i = 0; i < m_Srcmap.Width; ++i)
                {
                    for (int j = 0; j < m_Srcmap.Height; ++j)
                    {
                        int widthStart = i * (m_iCellSize + m_iBorderSize);
                        int widthEnd = i * (m_iCellSize + m_iBorderSize) + m_iCellSize;
                        int heightStart = j * (m_iCellSize + m_iBorderSize);
                        int heightEnd = j * (m_iCellSize + m_iBorderSize) + m_iCellSize;

                        for (int l = heightStart; (l < heightEnd && l < m_Desmap.Height); ++l)
                        {
                            for (int k = widthStart; (k < widthEnd && k < m_Desmap.Width); ++k)
                            {
                                pDest[desStride * l + k * 3] = pSrc[srcStride * j + i * 3];		//B
                                pDest[desStride * l + k * 3 + 1] = pSrc[srcStride * j + i * 3 + 1];	//G
                                pDest[desStride * l + k * 3 + 2] = pSrc[srcStride * j + i * 3 + 2];	//R
                            }
                        }
                    }
                }

                m_Srcmap.UnlockBits(srcData);
                m_Desmap.UnlockBits(desData);
            }
        }

        public void GetBitmap(ref PictureBox picBox)
        {
            picBox.Image = m_Desmap;            
            //picBox.Image = m_Srcmap;            
        }

    }
}
