using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace helper
{
   public static  class Dc
    {
        [DllImport("dc.dll", EntryPoint = "GetUserInfo")]
        public static extern IntPtr pGetUserInfo(string strVcodeUser, string strVcodePass);

        /// <summary>
        /// 查询剩余验证码点数
        /// </summary>
        /// <param name="strVcodeUser">超人账号</param>
        /// <param name="strVcodePass">超人密码</param>
        /// <returns>成功返回剩余验证码点数;失败返回:-1=网络错误;-5=账户密码错误</returns>
        public static string GetUserInfo(string strVcodeUser, string strVcodePass)
        {
            IntPtr p = pGetUserInfo(strVcodeUser, strVcodePass);
            return Marshal.PtrToStringAnsi(p);
        }

        [DllImport("dc.dll", EntryPoint = "RecByte_A")]
        public static extern IntPtr pRecByte_A(byte[] img, int len, string strVcodeUser, string strVcodePass, string strSoftId);

        /// <summary>
        /// 通过上传验证码图片字节到服务器进行验证码识别
        /// </summary>
        /// <param name="img">图片字节集</param>
        /// <param name="len">图片字节集长度</param>
        /// <param name="strVcodeUser">超人账号</param>
        /// <param name="strVcodePass">超人密码</param>
        /// <param name="strSoftId">软件ID,决定作者分成和验证码位数,为空默认为识别4位验证码</param>
        /// <returns>成功返回->验证码结果|!|验证码图片ID;失败返回->点数不够:Error:No Money!;账户密码错误:Error:No Reg!;上传失败，参数错误或者网络错误:Error:Put Fail!;识别超时:Error:TimeOut!;上传无效验证码:Error:empty picture!;账户或IP被冻结-:Error:Account or Software Bind!;软件被冻结:Error:Software Frozen!;</returns>

        public static string RecByte_A(byte[] img, int len, string strVcodeUser, string strVcodePass, string strSoftId)
        {
            IntPtr p = pRecByte_A(img, len, strVcodeUser, strVcodePass, strSoftId);
            return Marshal.PtrToStringAnsi(p);
        }
        [DllImport("dc.dll", EntryPoint = "RecYZM_A")]
        public static extern IntPtr pRecYZM_A(string strYZMPath, string strVcodeUser, string strVcodePass, string strSoftId);

        /// <summary>
        /// 通过发送验证码本地图片到服务器识别
        /// </summary>
        /// <param name="strYZMPath">验证码图片路径</param>
        /// <param name="strVcodeUser">超人账号</param>
        /// <param name="strVcodePass">超人密码</param>
        /// <param name="strSoftId">软件ID,决定作者分成和验证码位数,为空默认为识别4位验证码</param>
        /// <returns>成功返回->验证码结果|!|验证码图片ID;失败返回->点数不够:Error:No Money!;账户密码错误:Error:No Reg!;上传失败，参数错误或者网络错误:Error:Put Fail!;识别超时:Error:TimeOut!;上传无效验证码:Error:empty picture!;账户或IP被冻结-:Error:Account or Software Bind!;软件被冻结:Error:Software Frozen!;</returns>

        public static string RecYZM_A(string strYZMPath, string strVcodeUser, string strVcodePass, string strSoftId)
        {
            IntPtr p = pRecYZM_A(strYZMPath, strVcodeUser, strVcodePass, strSoftId);
            return Marshal.PtrToStringAnsi(p);
        }
        /// <summary>
        /// 对打错的验证码进行报告
        /// </summary>
        /// <param name="strVcodeUser">超人用户</param>
        /// <param name="strImageId">验证码ID</param>
        [DllImport("dc.dll")]
        public static extern void ReportError(string strVcodeUser, string strImageId);

        /// <summary>
        /// 对打错的验证码进行报告
        /// </summary>
        /// <param name="strVcodeUser">超人用户</param>
        /// <param name="strImageId">验证码ID</param>
        /// <returns>成功返回1，失败返回-1</returns>
        [DllImport("dc.dll")]
        public static extern int ReportError_A(string strVcodeUser, string strImageId);

    }
}
