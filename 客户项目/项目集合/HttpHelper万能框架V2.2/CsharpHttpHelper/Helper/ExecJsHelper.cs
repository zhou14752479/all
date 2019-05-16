using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace CsharpHttpHelper.Helper
{
    /// <summary>
    /// 用户执行JS的方法  Copyright：http://www.httphelper.com/
    /// </summary>
    internal class ExecJsHelper
    {
        #region 私有
        private static Type type = Type.GetTypeFromProgID("ScriptControl");
        #endregion

        #region Internal
        /// <summary>
        /// 直接调用JS方法并获取返回的值
        /// </summary>
        /// <param name="strJs">要执行的JS代码</param>
        /// <param name="main">要调用的方法名</param>
        /// <returns>执行结果</returns>
        internal static string JavaScriptEval(string strJs, string main)
        {
            //Type
            object obj = GetScriptControl();
            //设置JS代码
            SetScriptControlType(strJs, obj);
            //执行JS
            return type.InvokeMember("Eval", BindingFlags.InvokeMethod, null, obj, new object[] { main }).ToString();
        }
        #endregion

        #region Private Main
        /// <summary>
        /// 获取ScriptControl接口类
        /// </summary>
        /// <param name="strJs">JS</param>
        /// <param name="obj">对象</param>
        /// <returns>Type</returns>
        private static Type SetScriptControlType(string strJs, object obj)
        {
            //设置语言类型
            type.InvokeMember("Language", BindingFlags.SetProperty, null, obj, new object[] { "JScript" });
            //添加JS代码
            type.InvokeMember("AddCode", BindingFlags.InvokeMethod, null, obj, new object[] { strJs });
            return type;
        }
        /// <summary>
        /// 获取ScriptControl接口对象
        /// </summary>
        /// <returns>ScriptControl对象</returns>
        private static object GetScriptControl()
        {
            return Activator.CreateInstance(type);
        }
        #endregion
    }
}
