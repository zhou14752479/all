using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using CsharpHttpHelper.Item;
using System.Net;

namespace CsharpHttpHelper.Helper
{
    /// <summary>
    /// Cookie操作帮助类 Copyright：http://www.httphelper.com/
    /// </summary>
    internal static class HttpCookieHelper
    {
        /// <summary>
        /// 根据字符生成Cookie和精简串，将排除path,expires,domain以及重复项
        /// </summary>
        /// <param name="strcookie">Cookie字符串</param>
        /// <returns>精简串</returns>
        internal static string GetSmallCookie(string strcookie)
        {
            if (string.IsNullOrWhiteSpace(strcookie))
            {
                return string.Empty;
            }
            List<string> cookielist = new List<string>();
            //将Cookie字符串以,;分开，生成一个字符数组，并删除里面的空项
            string[] list = strcookie.ToString().Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in list)
            {
                string itemcookie = item.ToLower().Trim().Replace("\r\n", string.Empty).Replace("\n", string.Empty);
                //排除空字符串
                if (string.IsNullOrWhiteSpace(itemcookie)) continue;
                //排除不存在=号的Cookie项
                if (!itemcookie.Contains("=")) continue;
                //排除path项
                if (itemcookie.Contains("path=")) continue;
                //排除expires项
                if (itemcookie.Contains("expires=")) continue;
                //排除domain项
                if (itemcookie.Contains("domain=")) continue;
                //排除重复项
                if (cookielist.Contains(item)) continue;

                //对接Cookie基本的Key和Value串
                cookielist.Add(string.Format("{0};", item));
            }
            return string.Join(";", cookielist);
        }

        /// <summary>
        /// 将字符串Cookie转为CookieCollection
        /// </summary>
        /// <param name="strcookie">Cookie字符串</param>
        /// <returns>List-CookieItem</returns>
        internal static CookieCollection StrCookieToCookieCollection(string strcookie)
        {
            //排除空字符串
            if (string.IsNullOrWhiteSpace(strcookie))
            {
                return null;
            }
            CookieCollection cookielist = new CookieCollection();
            //先简化Cookie
            strcookie = GetSmallCookie(strcookie);
            //将Cookie字符串以,;分开，生成一个字符数组，并删除里面的空项
            string[] list = strcookie.ToString().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in list)
            {
                string[] cookie = item.ToString().Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                if (cookie.Length == 2)
                {
                    cookielist.Add(new Cookie() { Name = cookie[0].Trim(), Value = cookie[1].Trim() });
                }
            }
            return cookielist;
        }

        /// <summary>
        /// 将CookieCollection转为字符串Cookie
        /// </summary>
        /// <param name="cookie">Cookie字符串</param>
        /// <returns>strcookie</returns>
        internal static string CookieCollectionToStrCookie(CookieCollection cookie)
        {
            if (cookie == null)
            {
                return string.Empty;
            }
            string resultcookie = string.Empty;
            foreach (Cookie item in cookie)
            {
                resultcookie += string.Format("{0}={1};", item.Name, item.Value);
            }
            return resultcookie;
        }

        /// <summary>
        /// 自动合并两个Cookie的值返回更新后结果 
        /// </summary>
        /// <param name="cookie1">Cookie1</param>
        /// <param name="cookie2">Cookie2</param>
        /// <returns>返回更新后的Cookie</returns>
        internal static string GetMergeCookie(string cookie1, string cookie2)
        {
            if (string.IsNullOrWhiteSpace(cookie1))//新的是空的
            {
                return cookie2;//返回老的
            }
            if (string.IsNullOrWhiteSpace(cookie2))//老的是空的
            {
                return cookie1;//返回新的
            }
            List<string> cookielist = new List<string>();//结果
            string[] list_1 = cookie1.ToString().Split(';');
            string[] list_2 = cookie2.ToString().Split(';');
            foreach (string item in list_1)
            {
                //排除重复项
                if (cookielist.Contains(item)) continue;
                //对接Cookie基本的Key和Value串
                cookielist.Add(string.Format("{0} ", item));
            }
            foreach (string item in list_2)
            {
                //排除重复项
                if (cookielist.Contains(item)) continue;
                //对接Cookie基本的Key和Value串
                cookielist.Add(string.Format("{0}", item));
            }
            return string.Join(";", cookielist);
        }
    }
}
