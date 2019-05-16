using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsharpHttpHelper.Base;
using System.Drawing;
using System.IO;
using CsharpHttpHelper.Enum;
using CsharpHttpHelper.Helper;

namespace CsharpHttpHelper.BaseBll
{
    /// <summary>
    /// 具体实现方法  Copyright：http://www.httphelper.com/
    /// </summary>
    internal class HttpHelperBll
    {
        /// <summary>
        /// Httphelper原始访问类对象
        /// </summary>
        private HttphelperBase httpbase = new HttphelperBase();
        /// <summary>
        /// 根据相传入的数据，得到相应页面数据
        /// </summary>
        /// <param name="item">参数类对象</param>
        /// <returns>返回HttpResult类型</returns>
        internal HttpResult GetHtml(HttpItem item)
        {
            if (item.Allowautoredirect && item.AutoRedirectCookie)
            {
                HttpResult result = null;
                for (int i = 0; i < 100; i++)
                {
                    item.Allowautoredirect = false;
                    result = httpbase.GetHtml(item);
                    if (string.IsNullOrWhiteSpace(result.RedirectUrl))
                    {
                        break;
                    }
                    else
                    {
                        item.URL = result.RedirectUrl;
                        item.Method = "GET";
                        if (item.ResultCookieType == ResultCookieType.String)
                        {
                            item.Cookie += result.Cookie;
                        }
                        else
                        {
                            item.CookieCollection.Add(result.CookieCollection);
                        }
                    }
                }
                return result;
            }
            return httpbase.GetHtml(item);
        }
        /// <summary>
        /// 根据Url获取图片
        /// </summary>
        /// <param name="item">参数类对象</param>
        /// <returns>返回图片</returns>
        internal Image GetImage(HttpItem item)
        {
            item.ResultType = ResultType.Byte;
            return ImageHelper.ByteToImage(GetHtml(item).ResultByte);
        }
        /// <summary>
        /// 快速Post数据这个访求与GetHtml一样，只是不接收返回数据，只做提交。
        /// </summary>
        /// <param name="item">参数类对象</param>
        /// <returns>返回HttpResult类型</returns>
        internal HttpResult FastRequest(HttpItem item)
        {
            return httpbase.FastRequest(item);
        }
    }
}
