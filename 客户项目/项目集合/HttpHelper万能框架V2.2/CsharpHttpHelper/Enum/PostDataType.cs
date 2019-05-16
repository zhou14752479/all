using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsharpHttpHelper.Enum
{
    /// <summary>
    /// Post的数据格式默认为string  Copyright：http://www.httphelper.com/
    /// </summary>
    public enum PostDataType
    {
        /// <summary>
        /// 字符串类型，这时编码Encoding可不设置
        /// </summary>
        String,
        /// <summary>
        /// Byte类型，需要设置PostdataByte参数的值编码Encoding可设置为空
        /// </summary>
        Byte,
        /// <summary>
        /// 传文件，Postdata必须设置为文件的绝对路径，必须设置Encoding的值
        /// </summary>
        FilePath
    }
}
