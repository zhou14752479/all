using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CSharp.RuntimeBinder;
using MSScriptControl;


namespace 数据抓取
{
    class GlobalUtil
    {

        public static string TextGainCenter(string left, string right, string text)
        {
            bool flag = string.IsNullOrEmpty(left);
            string result;
            if (flag)
            {
                result = "";
            }
            else
            {
                bool flag2 = string.IsNullOrEmpty(right);
                if (flag2)
                {
                    result = "";
                }
                else
                {
                    bool flag3 = string.IsNullOrEmpty(text);
                    if (flag3)
                    {
                        result = "";
                    }
                    else
                    {
                        int Lindex = text.IndexOf(left);
                        bool flag4 = Lindex == -1;
                        if (flag4)
                        {
                            result = "";
                        }
                        else
                        {
                            Lindex += left.Length;
                            int Rindex = text.IndexOf(right, Lindex);
                            bool flag5 = Rindex == -1;
                            if (flag5)
                            {
                                result = "";
                            }
                            else
                            {
                                result = text.Substring(Lindex, Rindex - Lindex);
                            }
                        }
                    }
                }
            }
            return result;
        }

        public static string CalcMd5(string text)
        {
            string md5 = "";
            MD5 md5_text = MD5.Create();
            byte[] temp = md5_text.ComputeHash(Encoding.ASCII.GetBytes(text));
            for (int i = 0; i < temp.Length; i++)
            {
                md5 += temp[i].ToString("x2");
            }
            return md5;
        }

        //public static string ExecuteScript(string sExpression, string sCode)
        //{
        //    ScriptControl scriptControl = (ScriptControl)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("0E59F1D5-1FBE-11D0-8FF2-00A0D10038BC")));
        //    scriptControl.UseSafeSubset = true;
        //    scriptControl.Language = "JScript";
        //    scriptControl.AddCode(sCode);
        //    try
        //    {
        //        if (GlobalUtil.<> o__32.<> p__1 == null)
        //        {
        //            GlobalUtil.<> o__32.<> p__1 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(GlobalUtil)));
        //        }
        //        Func<CallSite, object, string> target = GlobalUtil.<> o__32.<> p__1.Target;
        //        CallSite<> p__ = GlobalUtil.<> o__32.<> p__1;
        //        if (GlobalUtil.<> o__32.<> p__0 == null)
        //        {
        //            GlobalUtil.<> o__32.<> p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(GlobalUtil), new CSharpArgumentInfo[]
        //            {
        //                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
        //            }));
        //        }
        //        return target(<> p__, GlobalUtil.<> o__32.<> p__0.Target(GlobalUtil.<> o__32.<> p__0, scriptControl.Eval(sExpression)));
        //    }
        //    catch (Exception ex)
        //    {
        //        string str = ex.Message;
        //    }
        //    return null;
        //}


    }
}
