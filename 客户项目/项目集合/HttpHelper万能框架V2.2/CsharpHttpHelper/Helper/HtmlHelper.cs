using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsharpHttpHelper.Item;
using System.Text.RegularExpressions;
using System.Web;
using CsharpHttpHelper.Static;
using CsharpHttpHelper.Enum;

namespace CsharpHttpHelper.Helper
{
    /// <summary>
    /// Html操作相关  Copyright：http://www.httphelper.com/
    /// </summary>
    internal class HtmlHelper
    {
        /// <summary>
        /// 获取所有的A链接
        /// </summary>
        /// <param name="html">要分析的Html代码</param>
        /// <returns>返回一个List存储所有的A标签</returns>
        internal static List<AItem> GetAList(string html)
        {
            List<AItem> list = null;
            string ra = RegexString.Alist;
            if (Regex.IsMatch(html, ra, RegexOptions.IgnoreCase))
            {
                list = new List<AItem>();
                foreach (Match item in Regex.Matches(html, ra, RegexOptions.IgnoreCase))
                {
                    AItem a = null;
                    try
                    {
                        a = new AItem()
                        {
                            Href = item.Groups[1].Value,
                            Text = item.Groups[2].Value,
                            Html = item.Value,
                            Type = AType.Text
                        };
                        List<ImgItem> imglist = GetImgList(a.Text);
                        if (imglist != null && imglist.Count > 0)
                        {
                            a.Type = AType.Img;
                            a.Img = imglist[0];
                        }
                    }
                    catch { a = null; }
                    if (a != null)
                    {
                        list.Add(a);
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 获取所有的Img标签
        /// </summary>
        /// <param name="html">要分析的Html代码</param>
        /// <returns>返回一个List存储所有的Img标签</returns>
        internal static List<ImgItem> GetImgList(string html)
        {
            List<ImgItem> list = null;
            string ra = RegexString.ImgList;
            if (Regex.IsMatch(html, ra, RegexOptions.IgnoreCase))
            {
                list = new List<ImgItem>();
                foreach (Match item in Regex.Matches(html, ra, RegexOptions.IgnoreCase))
                {
                    ImgItem a = null;
                    try
                    {
                        a = new ImgItem()
                        {
                            Src = item.Groups[1].Value,
                            Html = item.Value
                        };
                    }
                    catch { a = null; }
                    if (a != null)
                    {
                        list.Add(a);
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 过滤html标签
        /// </summary>
        /// <param name="html">html的内容</param>
        /// <returns>处理后的文本</returns>
        internal static string StripHTML(string html)
        {
            html = Regex.Replace(html, RegexString.Nscript, string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            html = Regex.Replace(html, RegexString.Style, string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            html = Regex.Replace(html, RegexString.Script, string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            html = Regex.Replace(html, RegexString.Html, string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            return html;
        }
        /// <summary>
        /// 过滤html中所有的换行符号
        /// </summary>
        /// <param name="html">html的内容</param>
        /// <returns>处理后的文本</returns>
        internal static string ReplaceNewLine(string html)
        {
            return Regex.Replace(html, RegexString.NewLine, string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }
        /// <summary>
        /// 提取Html字符串中两字符之间的数据
        /// </summary>
        /// <param name="html">源Html</param>
        /// <param name="s">开始字符串</param>
        /// <param name="e">结束字符串</param>
        /// <returns></returns>
        internal static string GetBetweenHtml(string html, string s, string e)
        {
            string rx = string.Format("{0}{1}{2}", s, RegexString.AllHtml, e);
            if (Regex.IsMatch(html, rx, RegexOptions.IgnoreCase))
            {
                Match match = Regex.Match(html, rx, RegexOptions.IgnoreCase);
                if (match != null && match.Groups.Count > 0)
                {
                    return match.Groups[1].Value.Trim();
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// 提取网页Title
        /// </summary>
        /// <param name="html">Html</param>
        /// <returns>返回Title</returns>
        internal static string GetHtmlTitle(string html)
        {
            if (Regex.IsMatch(html, RegexString.HtmlTitle))
            {
                return Regex.Match(html, RegexString.HtmlTitle).Groups[1].Value.Trim();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
