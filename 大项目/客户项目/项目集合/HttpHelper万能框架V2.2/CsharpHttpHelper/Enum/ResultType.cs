using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsharpHttpHelper.Enum
{
    /// <summary>
    /// 返回类型  Copyright：http://www.httphelper.com/
    /// </summary>
    public enum ResultType
    {
        /// <summary>
        /// 表示只返回字符串 只有Html有数据,ResultByte为空
        /// </summary>
        String,
        /// <summary>
        /// 表示只返回字符串 只有ResultByte有数据,Html为空
        /// </summary>
        Byte,
        /// <summary>
        /// 表示返回字符串和字节流 ResultByte和Html都有数据返回
        /// </summary>
        StringByte
    }
}
