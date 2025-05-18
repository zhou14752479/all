using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 旺旺发消息
{
    public class function
    {

        #region 获取编码类型
        public class EncodingType
        {
            // Token: 0x0600003D RID: 61 RVA: 0x00004D18 File Offset: 0x00002F18
            public static Encoding GetTxtType(string FILE_NAME)
            {
                FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read);
                Encoding r = EncodingType.GetType(fs);
                fs.Close();
                return r;
            }

            // Token: 0x0600003E RID: 62 RVA: 0x00004D44 File Offset: 0x00002F44
            public static Encoding GetType(FileStream fs)
            {
                byte[] array = new byte[]
                {
                    byte.MaxValue,
                    254,
                    65
                };
                byte[] array2 = new byte[3];
                array2[0] = 254;
                array2[1] = byte.MaxValue;
                byte[] array3 = new byte[]
                {
                    239,
                    187,
                    191
                };
                Encoding reVal = Encoding.Default;
                BinaryReader r = new BinaryReader(fs, Encoding.Default);
                int i;
                int.TryParse(fs.Length.ToString(), out i);
                byte[] ss = r.ReadBytes(i);
                bool flag = EncodingType.IsUTF8Bytes(ss) || (ss[0] == 239 && ss[1] == 187 && ss[2] == 191);
                if (flag)
                {
                    reVal = Encoding.UTF8;
                }
                else
                {
                    bool flag2 = ss[0] == 254 && ss[1] == byte.MaxValue && ss[2] == 0;
                    if (flag2)
                    {
                        reVal = Encoding.BigEndianUnicode;
                    }
                    else
                    {
                        bool flag3 = ss[0] == byte.MaxValue && ss[1] == 254 && ss[2] == 65;
                        if (flag3)
                        {
                            reVal = Encoding.Unicode;
                        }
                    }
                }
                r.Close();
                return reVal;
            }

            // Token: 0x0600003F RID: 63 RVA: 0x00004E70 File Offset: 0x00003070
            //private static bool IsUTF8Bytes(byte[] data)
            //{
            //	int charByteCounter = 1;
            //	foreach (byte curByte in data)
            //	{
            //		bool flag = charByteCounter == 1;
            //		if (flag)
            //		{
            //			bool flag2 = curByte >= 128;
            //			if (flag2)
            //			{
            //				while (((curByte = (byte)(curByte << 1)) & 128) > 0)
            //				{
            //					charByteCounter++;
            //				}
            //				bool flag3 = charByteCounter == 1 || charByteCounter > 6;
            //				if (flag3)
            //				{
            //					return false;
            //				}
            //			}
            //		}
            //		else
            //		{
            //			bool flag4 = (curByte & 192) != 128;
            //			if (flag4)
            //			{
            //				return false;
            //			}
            //			charByteCounter--;
            //		}
            //	}
            //	bool flag5 = charByteCounter > 1;
            //	if (flag5)
            //	{
            //		throw new Exception("非预期的byte格式");
            //	}
            //	return true;
            //}

            private static bool IsUTF8Bytes(byte[] data)
            {
                int charByteCounter = 1; //计算当前正分析的字符应还有的字节数
                byte curByte; //当前分析的字节.
                for (int i = 0; i < data.Length; i++)
                {
                    curByte = data[i];
                    if (charByteCounter == 1)
                    {
                        if (curByte >= 0x80)
                        {
                            //判断当前
                            while (((curByte <<= 1) & 0x80) != 0)
                            {
                                charByteCounter++;
                            }
                            //标记位首位若为非0 则至少以2个1开始 如:110XXXXX…1111110X
                            if (charByteCounter == 1 || charByteCounter > 6)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        //若是UTF-8 此时第一位必须为1
                        if ((curByte & 0xC0) != 0x80)
                        {
                            return false;
                        }
                        charByteCounter--;
                    }
                }
                if (charByteCounter > 1)
                {
                    throw new Exception("非预期的byte格式");
                }
                return true;
            }



        }

        #endregion
    }
}
