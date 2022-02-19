using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 中国智慧社区服务网
{
    class Sm4Crypto
    {
        public Sm4Crypto()
        {
            Key = "iuas4dk8fa6s67p2";
            Iv = "longrise12345678";
            HexString = false;
            CryptoMode = Sm4CryptoEnum.CBC;
        }
        /// <summary>
        /// 数据
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// 秘钥
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 向量
        /// </summary>
        public string Iv { get; set; }
        /// <summary>
        /// 明文是否是十六进制
        /// </summary>
        public bool HexString { get; set; }
        /// <summary>
        /// 加密模式(默认ECB)
        /// </summary>
        public Sm4CryptoEnum CryptoMode { get; set; }

        #region 加密
        public string Encrypt(Sm4Crypto entity)
        {
            return entity.CryptoMode == Sm4CryptoEnum.CBC ? EncryptCBC(entity) : EncryptECB(entity);
        }
        /// <summary>
        /// ECB加密
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string EncryptECB(Sm4Crypto entity)
        {
            Sm4Context ctx = new Sm4Context
            {
                IsPadding = true
            };

            byte[] keyBytes = entity.HexString ? Hex.Decode(entity.Key) : Encoding.Default.GetBytes(entity.Key);

            SM4 sm4 = new SM4();
            sm4.SetKeyEnc(ctx, keyBytes);
            byte[] encrypted = sm4.Sm4CryptEcb(ctx, Encoding.Default.GetBytes(entity.Data));

            return Encoding.Default.GetString(Hex.Encode(encrypted));
        }
        /// <summary>
        /// CBC加密
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string EncryptCBC(Sm4Crypto entity)
        {
            Sm4Context ctx = new Sm4Context
            {
                IsPadding = true
            };

            byte[] keyBytes = entity.HexString ? Hex.Decode(entity.Key) : Encoding.Default.GetBytes(entity.Key);
            byte[] ivBytes = entity.HexString ? Hex.Decode(entity.Iv) : Encoding.Default.GetBytes(entity.Iv);

            SM4 sm4 = new SM4();
            sm4.SetKeyEnc(ctx, keyBytes);
            byte[] encrypted = sm4.Sm4CryptCbc(ctx, ivBytes, Encoding.Default.GetBytes(entity.Data));

            //return encrypted.ToBase64();
            return Convert.ToBase64String(encrypted);
        }
        #endregion


        #region 解密
        public object Decrypt(Sm4Crypto entity)
        {
            return entity.CryptoMode == Sm4CryptoEnum.CBC ? DecryptCBC(entity) : DecryptECB(entity);
        }
        /// <summary>
        ///  ECB解密
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string DecryptECB(Sm4Crypto entity)
        {
            Sm4Context ctx = new Sm4Context
            {
                IsPadding = true,
                Mode = 0
            };

            byte[] keyBytes = entity.HexString ? Hex.Decode(entity.Key) : Encoding.Default.GetBytes(entity.Key);

            SM4 sm4 = new SM4();
            sm4.Sm4SetKeyDec(ctx, keyBytes);
            byte[] decrypted = sm4.Sm4CryptEcb(ctx, Hex.Decode(entity.Data));
            return Encoding.Default.GetString(decrypted);
        }
        /// <summary>
        /// CBC解密
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public static string DecryptCBC(Sm4Crypto entity)
        {
            Sm4Context ctx = new Sm4Context
            {
                IsPadding = true,
                Mode = 0
            };

            byte[] keyBytes = entity.HexString ? Hex.Decode(entity.Key) : Encoding.Default.GetBytes(entity.Key);
            byte[] ivBytes = entity.HexString ? Hex.Decode(entity.Iv) : Encoding.Default.GetBytes(entity.Iv);

            SM4 sm4 = new SM4();
            sm4.Sm4SetKeyDec(ctx, keyBytes);
            byte[] decrypted = sm4.Sm4CryptCbc(ctx, ivBytes, Convert.FromBase64String(entity.Data));
            return Encoding.Default.GetString(decrypted);
        }
        #endregion

        /// <summary>
        /// 加密类型
        /// </summary>
        public enum Sm4CryptoEnum
        {
            /// <summary>
            /// ECB(电码本模式)
            /// </summary>
            [Description("ECB模式")]
            ECB = 0,
            /// <summary>
            /// CBC(密码分组链接模式)
            /// </summary>
            [Description("CBC模式")]
            CBC = 1
        }

    }
}
