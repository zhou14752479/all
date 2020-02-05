using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace helper
{
    [Serializable]
    public class CodeInfo
    {
        private string _Url;
        /// <summary>
        /// 配置验证码时候的URL 识别并不使用
        /// </summary>
        public string Url
        {
            get { return _Url; }
            set { _Url = value; }
        }

        private Image _ImageTemp;
        /// <summary>
        /// 配置验证码时候的图像 识别并不使用
        /// </summary>
        public Image ImageTemp
        {
            get { return _ImageTemp; }
            set { _ImageTemp = value; }
        }

        private int _CodeCount;
        /// <summary>
        /// 验证码字符的个数
        /// </summary>
        public int CodeCount
        {
            get { return _CodeCount; }
            set { _CodeCount = value; }
        }

        private byte[] _BinaryValues;
        /// <summary>
        /// 验证码二值化阀值
        /// </summary>
        public byte[] BinaryValues
        {
            get { return _BinaryValues; }
            set { _BinaryValues = value; }
        }

        private int _PixelMin;
        /// <summary>
        /// 连通像素的最小像素点个数
        /// </summary>
        public int PixelMin
        {
            get { return _PixelMin; }
            set { _PixelMin = value; }
        }

        private int _PixelMax;
        /// <summary>
        /// 连通像素的最大像素点个数
        /// </summary>
        public int PixelMax
        {
            get { return _PixelMax; }
            set { _PixelMax = value; }
        }

        private Rectangle _RectangleCut;
        /// <summary>
        /// 验证码的裁剪区域
        /// </summary>
        public Rectangle RectangleCut
        {
            get { return _RectangleCut; }
            set { _RectangleCut = value; }
        }

        private bool _IsAutoSelectRect;
        /// <summary>
        /// 是否是自动识别验证码字符区域
        /// </summary>
        public bool IsAutoSelectRect
        {
            get { return _IsAutoSelectRect; }
            set { _IsAutoSelectRect = value; }
        }

        private List<Rectangle> _CodeRectangles;
        /// <summary>
        /// 验证码字符所在的区域 IsAutoSelectRect=true 时无效
        /// </summary>
        public List<Rectangle> CodeRectangles
        {
            get
            {
                if (_CodeRectangles == null) _CodeRectangles = new List<Rectangle>();
                return _CodeRectangles;
            }
        }

        private List<string> _Express;
        /// <summary>
        /// 二值化时去干扰表达式
        /// </summary>
        public List<string> Express
        {
            get
            {
                if (_Express == null) _Express = new List<string>();
                return _Express;
            }
        }

        private Dictionary<char, List<Image>> m_dic_char_img;
        private Dictionary<string, int> m_dic_img_hash = new Dictionary<string, int>();

        public CodeInfo()
        {
            m_dic_char_img = new Dictionary<char, List<Image>>();
        }
        /// <summary>
        /// 添加一个验证码样本
        /// </summary>
        /// <param name="ch">验证码字符</param>
        /// <param name="imgCode">验证码图像</param>
        /// <returns>是否添加成功</returns>
        public bool AddCode(char ch, Image imgCode)
        {
            string strHash = this.GetImageHash(imgCode);
            if (m_dic_img_hash.ContainsKey(strHash)) return false;
            m_dic_img_hash.Add(strHash, 0);
            if (m_dic_char_img.ContainsKey(ch))
            {
                m_dic_char_img[ch].Add(imgCode);
            }
            else
            {
                List<Image> lstBmpCode = new List<Image>();
                lstBmpCode.Add(imgCode);
                m_dic_char_img.Add(ch, lstBmpCode);
            }
            return true;
        }
        /// <summary>
        /// 根据字符获取该字符下的所有样本图像
        /// </summary>
        /// <param name="ch">目标字符</param>
        /// <returns>样本列表</returns>
        public List<Image> GetCodeImages(char ch)
        {
            if (m_dic_char_img.ContainsKey(ch))
            {
                return m_dic_char_img[ch];
            }
            return null;
        }
        /// <summary>
        /// 删除目标字符下的所有样本图像
        /// </summary>
        /// <param name="ch">目标字符</param>
        public void RemoveCode(char ch)
        {
            if (!m_dic_char_img.ContainsKey(ch)) return;
            foreach (var v in m_dic_char_img[ch])
            {
                string strHash = this.GetImageHash(v);
                if (m_dic_img_hash.ContainsKey(strHash))
                    m_dic_img_hash.Remove(strHash);
            }
            m_dic_char_img.Remove(ch);
        }
        /// <summary>
        /// 删除目标字符中指定样本
        /// </summary>
        /// <param name="ch">目标字符</param>
        /// <param name="imgCode">目标图像</param>
        public void RemoveCode(char ch, Image imgCode)
        {
            m_dic_char_img[ch].Remove(imgCode);
            string strHash = this.GetImageHash(imgCode);
            if (m_dic_img_hash.ContainsKey(strHash))
                m_dic_img_hash.Remove(strHash);
        }
        /// <summary>
        /// 清除所有验证码样本
        /// </summary>
        public void ClearCode()
        {
            m_dic_char_img.Clear();
            m_dic_img_hash.Clear();
        }
        /// <summary>
        /// 获取现有的验证码字符
        /// </summary>
        /// <returns></returns>
        public Dictionary<char, List<Image>>.KeyCollection GetChars()
        {
            return m_dic_char_img.Keys;
        }
        /// <summary>
        /// 获取图像hash
        /// </summary>
        /// <param name="img">图像</param>
        /// <returns>hash</returns>
        private string GetImageHash(Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] byImg = new byte[ms.Length];
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(byImg, 0, byImg.Length);
                return BitConverter.ToString(System.Security.Cryptography.MD5.Create().ComputeHash(byImg)).Replace("-", "");
            }
        }
        /// <summary>
        /// 刷行图像hash 反序列化后图像hash可能有所变化
        /// </summary>
        private void RefreshHash()
        {
            m_dic_img_hash.Clear();
            foreach (var v in m_dic_char_img)
            {
                foreach (var i in v.Value)
                {
                    m_dic_img_hash.Add(this.GetImageHash(i), 0);
                }
            }
        }
        /// <summary>
        /// 将目标配置保存到文件中
        /// </summary>
        /// <param name="ci">目标配置</param>
        /// <param name="strFileName">配置文件名(.ci.png后缀以图片形式保存配置 方便查看)</param>
        public static void SaveToFile(CodeInfo ci, string strFileName)
        {
            byte[] byci = null;
            byte[] bylen = null;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, ci);
                byci = ms.ToArray();
            }
            if (System.Text.RegularExpressions.Regex.IsMatch(strFileName.ToLower(), @"\.ci\.png$"))
            {
                ci.ImageTemp.Save(strFileName, System.Drawing.Imaging.ImageFormat.Png);
                bylen = BitConverter.GetBytes(byci.Length);
            }
            using (FileStream fs = new FileStream(strFileName, FileMode.OpenOrCreate))
            {
                fs.Position = fs.Length;
                fs.Write(byci, 0, byci.Length);
                if (bylen != null) fs.Write(bylen, 0, bylen.Length);
            }
        }
        /// <summary>
        /// 从文件中导入配置
        /// </summary>
        /// <param name="strFileName">文件名</param>
        /// <returns>验证码配置</returns>
        public static CodeInfo LoadFromFile(string strFileName)
        {
            byte[] byTemp = File.ReadAllBytes(strFileName);
            byte[] byCi = null;
            if (System.Text.RegularExpressions.Regex.IsMatch(strFileName.ToLower(), @"\.ci\.png$"))
            {
                byCi = new byte[BitConverter.ToInt32(byTemp, byTemp.Length - 4)];
                Array.Copy(byTemp, byTemp.Length - byCi.Length - 4, byCi, 0, byCi.Length);
            }
            using (MemoryStream fs = new MemoryStream(byCi == null ? byTemp : byCi))
            {
                BinaryFormatter bf = new BinaryFormatter();
                CodeInfo ci = (CodeInfo)bf.Deserialize(fs);
                ci.RefreshHash();
                return ci;
            }
        }
    }
}
