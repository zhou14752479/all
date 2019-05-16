using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsharpHttpHelper.Enum
{
    /// <summary>
    /// Cookie返回类型  Copyright：http://www.httphelper.com/
    /// </summary>
    public enum ResultCookieType
    {
        /// <summary>
        /// 只返回字符串类型的Cookie
        /// </summary>
        String,
        /// <summary>
        /// CookieCollection格式的Cookie集合同时也返回String类型的cookie
        /// </summary>
        CookieCollection,
        /// <summary>
        /// CookieContainer 多纬度Cookie
        /// </summary>
        CookieContainer
    }
}
