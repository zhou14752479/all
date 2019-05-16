using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsharpHttpHelper.Enum;

namespace CsharpHttpHelper.Item
{
    /// <summary>
    /// A连接对象  Copyright：http://www.httphelper.com/
    /// </summary>
    public class AItem
    {
        /// <summary>
        /// 链接地址
        /// </summary>
        public string Href { get; set; }
        /// <summary>
        /// 链接文本
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 链接的图片，如果是文本链接则为空
        /// </summary>
        public ImgItem Img { get; set; }
        /// <summary>
        /// 整个连接Html
        /// </summary>
        public string Html { get; set; }
        /// <summary>
        /// A链接的类型
        /// </summary>
        public AType Type { get; set; } 
    }
}
