using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;
using CsharpHttpHelper.Enum;
using System.Net.Security;
using CsharpHttpHelper.Static;

namespace CsharpHttpHelper.Base
{
    /// <summary>
    /// Http连接操作帮助类  Copyright：http://www.httphelper.com/
    /// </summary>
    internal class HttphelperBase
    {
        #region 预定义方变量
        //默认的编码
        private Encoding encoding = Encoding.Default;
        //Post数据编码
        private Encoding postencoding = Encoding.Default;
        //HttpWebRequest对象用来发起请求
        private HttpWebRequest request = null;
        //获取影响流的数据对象
        private HttpWebResponse response = null;
        //设置本地的出口ip和端口
        private IPEndPoint _IPEndPoint = null;
        #endregion
        #region internal

        /// <summary>
        /// 根据相传入的数据，得到相应页面数据
        /// </summary>
        /// <param name="item">参数类对象</param>
        /// <returns>返回HttpResult类型</returns>
        internal HttpResult GetHtml(HttpItem item)
        {
            //返回参数
            HttpResult result = new HttpResult();
            result.item = item;
            try
            {
                //准备参数
                SetRequest(item);
            }
            catch (Exception ex)
            {
                //配置参数时出错
                return new HttpResult() { Cookie = string.Empty, Header = null, Html = ex.Message, StatusDescription = "配置参数时出错：" + ex.Message };
            }
            try
            {
                //请求数据
                using (response = (HttpWebResponse)request.GetResponse())
                {
                    GetData(item, result);
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (response = (HttpWebResponse)ex.Response)
                    {
                        GetData(item, result);
                    }
                }
                else
                {
                    result.Html = ex.Message;
                }
            }
            catch (Exception ex)
            {
                result.Html = ex.Message;
            }
            if (item.IsToLower) result.Html = result.Html.ToLower();
            //重置request，response为空
            if (item.IsReset)
            {
                request = null;
                response = null;
            }
            return result;
        }
        /// <summary>
        /// 快速Post数据这个访求与GetHtml一样，只是不接收返回数据，只做提交。
        /// </summary>
        /// <param name="item">参数类对象</param>
        /// <returns>返回HttpResult类型</returns>
        internal HttpResult FastRequest(HttpItem item)
        {
            //返回参数
            HttpResult result = new HttpResult();
            result.item = item;
            try
            {
                //准备参数
                SetRequest(item);
            }
            catch (Exception ex)
            {
                //配置参数时出错
                return new HttpResult() { Cookie = response.Headers["set-cookie"] != null ? response.Headers["set-cookie"] : string.Empty, Header = null, Html = ex.Message, StatusDescription = "配置参数时出错：" + ex.Message };
            }
            try
            {
                //请求数据
                using (response = (HttpWebResponse)request.GetResponse())
                {
                    //成功 不做处理只回成功状态
                    return new HttpResult() { Cookie = response.Headers["set-cookie"] != null ? response.Headers["set-cookie"] : string.Empty, Header = response.Headers, StatusCode = response.StatusCode, StatusDescription = response.StatusDescription };
                }
            }
            catch (WebException ex)
            {
                using (response = (HttpWebResponse)ex.Response)
                {
                    //不做处理只回成功状态
                    return new HttpResult() { Cookie = response.Headers["set-cookie"] != null ? response.Headers["set-cookie"] : string.Empty, Header = response.Headers, StatusCode = response.StatusCode, StatusDescription = response.StatusDescription };
                }
            }
            catch (Exception ex)
            {
                result.Html = ex.Message;
            }
            if (item.IsToLower) result.Html = result.Html.ToLower();
            return result;
        }
        #endregion

        #region GetData

        /// <summary>
        /// 获取数据的并解析的方法
        /// </summary>
        /// <param name="item"></param>
        /// <param name="result"></param>
        private void GetData(HttpItem item, HttpResult result)
        {
            if (response == null)
            {
                return;
            }
            #region base
            //获取StatusCode
            result.StatusCode = response.StatusCode;
            //获取最后访问的URl
            result.ResponseUri = response.ResponseUri.ToString();
            //获取StatusDescription
            result.StatusDescription = response.StatusDescription;
            //获取Headers
            result.Header = response.Headers;
            //获取CookieCollection
            if (response.Cookies != null) result.CookieCollection = response.Cookies;
            //获取set-cookie
            if (response.Headers["set-cookie"] != null) result.Cookie = response.Headers["set-cookie"];
            //Cookie是否自动更新为请求所获取的新Cookie值 
            if (item.IsUpdateCookie)
            {
                item.Cookie = result.Cookie;
                item.CookieCollection = result.CookieCollection;
            }
            #endregion

            #region 用户设置用编码
            //处理网页Byte
            byte[] ResponseByte = GetByte(item);

            if (ResponseByte != null && ResponseByte.Length > 0)
            {
                //设置编码
                SetEncoding(item, result, ResponseByte);

                //设置返回的Byte
                SetResultByte(item, result, ResponseByte);
            }
            else
            {
                //没有返回任何Html代码
                result.Html = string.Empty;
            }

            #endregion
        }
        /// <summary>
        /// 设置返回的Byte
        /// </summary>
        /// <param name="item">HttpItem</param>
        /// <param name="result">result</param>
        /// <param name="enByte">byte</param>
        private void SetResultByte(HttpItem item, HttpResult result, byte[] enByte)
        {
            //是否返回Byte类型数据
            if (item.ResultType == ResultType.Byte)
            {
                //Byte数据
                result.ResultByte = enByte;
            }
            else if (item.ResultType == ResultType.String)
            {
                //得到返回的HTML
                result.Html = encoding.GetString(enByte);
            }
            else if (item.ResultType == ResultType.StringByte)
            {
                //Byte数据
                result.ResultByte = enByte;
                //得到返回的HTML
                result.Html = encoding.GetString(enByte);
            }
        }
        /// <summary>
        /// 设置编码
        /// </summary>
        /// <param name="item">HttpItem</param>
        /// <param name="result">HttpResult</param>
        /// <param name="ResponseByte">byte[]</param>
        private void SetEncoding(HttpItem item, HttpResult result, byte[] ResponseByte)
        {
            //从这里开始我们要无视编码了
            if (encoding == null)
            {
                Match meta = Regex.Match(Encoding.Default.GetString(ResponseByte), RegexString.Enconding, RegexOptions.IgnoreCase);
                string c = string.Empty;
                if (meta != null && meta.Groups.Count > 0)
                {
                    c = meta.Groups[1].Value.ToLower().Trim();
                }
                string cs = string.Empty;
                if (!string.IsNullOrWhiteSpace(response.CharacterSet))
                {
                    cs = response.CharacterSet.Trim().Replace("\"", "").Replace("\'", "");
                }

                if (c.Length > 2)
                {
                    try
                    {
                        encoding = Encoding.GetEncoding(c.Replace("\"", string.Empty).Replace("'", "").Replace(";", "").Replace("iso-8859-1", "gbk").Trim());
                    }
                    catch
                    {
                        if (string.IsNullOrEmpty(cs))
                        {
                            encoding = Encoding.UTF8;
                        }
                        else
                        {
                            encoding = Encoding.GetEncoding(cs);
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(cs))
                    {
                        encoding = Encoding.UTF8;
                    }
                    else
                    {
                        encoding = Encoding.GetEncoding(cs);
                    }
                }
            }
        }
        /// <summary>
        /// 提取网页Byte
        /// </summary>
        /// <returns></returns>
        private byte[] GetByte(HttpItem item)
        {
            byte[] ResponseByte = null;
            using (MemoryStream _stream = new MemoryStream())
            {
                if (item.IsGzip)
                {
                    //开始读取流并设置编码方式
                    new GZipStream(response.GetResponseStream(), CompressionMode.Decompress).CopyTo(_stream, 10240);
                }
                else
                {
                    //GZIIP处理
                    if (response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //开始读取流并设置编码方式
                        new GZipStream(response.GetResponseStream(), CompressionMode.Decompress).CopyTo(_stream, 10240);
                    }
                    else
                    {
                        //开始读取流并设置编码方式
                        response.GetResponseStream().CopyTo(_stream, 10240);
                    }
                }
                //获取Byte
                ResponseByte = _stream.ToArray();
            }
            return ResponseByte;
        }
        #endregion

        #region SetRequest

        /// <summary>
        /// 为请求准备参数
        /// </summary>
        ///<param name="item">参数列表</param>
        private void SetRequest(HttpItem item)
        {
            if (!string.IsNullOrWhiteSpace(item.CerPath))
            {
                //这一句一定要写在创建连接的前面。使用回调的方法进行证书验证。
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            }
            //初始化对像，并设置请求的URL地址
            request = (HttpWebRequest)WebRequest.Create(item.URL);
            if (item.IPEndPoint != null)
            {
                _IPEndPoint = item.IPEndPoint;
                //设置本地的出口ip和端口
                request.ServicePoint.BindIPEndPointDelegate = new BindIPEndPoint(BindIPEndPointCallback);
            }
            request.AutomaticDecompression = item.AutomaticDecompression;
            // 验证证书
            SetCer(item);
            SetCerList(item);
            //设置Header参数
            if (item.Header != null && item.Header.Count > 0) foreach (string key in item.Header.AllKeys)
                {
                    request.Headers.Add(key, item.Header[key]);
                }
            // 设置代理
            SetProxy(item);
            if (item.ProtocolVersion != null) request.ProtocolVersion = item.ProtocolVersion;
            request.ServicePoint.Expect100Continue = item.Expect100Continue;
            //请求方式Get或者Post
            request.Method = item.Method;
            request.Timeout = item.Timeout;
            request.KeepAlive = item.KeepAlive;
            request.ReadWriteTimeout = item.ReadWriteTimeout;
            if (!string.IsNullOrWhiteSpace(item.Host))
            {
                request.Host = item.Host;
            }
            if (item.IfModifiedSince != null) request.IfModifiedSince = Convert.ToDateTime(item.IfModifiedSince);
            //Accept
            request.Accept = item.Accept;
            //ContentType返回类型
            request.ContentType = item.ContentType;
            //UserAgent客户端的访问类型，包括浏览器版本和操作系统信息
            request.UserAgent = item.UserAgent;
            // 编码
            encoding = item.Encoding;
            //设置安全凭证
            request.Credentials = item.ICredentials;
            //设置Cookie
            SetCookie(item);
            //来源地址
            request.Referer = item.Referer;
            //是否执行跳转功能
            request.AllowAutoRedirect = item.Allowautoredirect;
            if (item.MaximumAutomaticRedirections > 0)
            {
                request.MaximumAutomaticRedirections = item.MaximumAutomaticRedirections;
            }
            //设置最大连接
            if (item.Connectionlimit > 0) request.ServicePoint.ConnectionLimit = item.Connectionlimit;
            //当出现“请求被中止: 未能创建 SSL/TLS 安全通道”时需要配置此属性 
            if (item.SecurityProtocol > 0)
            {
                ServicePointManager.SecurityProtocol = item.SecurityProtocol;
            }
            //设置Post数据
            SetPostData(item);
        }
        /// <summary>
        /// 设置证书
        /// </summary>
        /// <param name="item"></param>
        private void SetCer(HttpItem item)
        {
            if (!string.IsNullOrWhiteSpace(item.CerPath))
            {
                //将证书添加到请求里
                if (!string.IsNullOrWhiteSpace(item.CerPwd))
                {
                    request.ClientCertificates.Add(new X509Certificate(item.CerPath, item.CerPwd));
                }
                else
                {
                    request.ClientCertificates.Add(new X509Certificate(item.CerPath));
                }
            }
        }
        /// <summary>
        /// 设置多个证书
        /// </summary>
        /// <param name="item"></param>
        private void SetCerList(HttpItem item)
        {
            if (item.ClentCertificates != null && item.ClentCertificates.Count > 0)
            {
                foreach (X509Certificate c in item.ClentCertificates)
                {
                    request.ClientCertificates.Add(c);
                }
            }
        }
        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="item">Http参数</param>
        private void SetCookie(HttpItem item)
        {
            if (!string.IsNullOrEmpty(item.Cookie)) request.Headers[HttpRequestHeader.Cookie] = item.Cookie;
            //设置CookieCollection
            if (item.ResultCookieType == ResultCookieType.CookieCollection)
            {
                request.CookieContainer = new CookieContainer();
                if (item.CookieCollection != null && item.CookieCollection.Count > 0)
                    request.CookieContainer.Add(item.CookieCollection);
            }
            else if (item.ResultCookieType == ResultCookieType.CookieContainer)
            {
                request.CookieContainer = item.CookieContainer;
            }
        }
        /// <summary>
        /// 设置Post数据
        /// </summary>
        /// <param name="item">Http参数</param>
        private void SetPostData(HttpItem item)
        {
            //验证在得到结果时是否有传入数据
            if (!request.Method.Trim().ToLower().Contains("get"))
            {
                if (item.PostEncoding != null)
                {
                    postencoding = item.PostEncoding;
                }
                byte[] buffer = null;
                //写入Byte类型
                if (item.PostDataType == PostDataType.Byte && item.PostdataByte != null && item.PostdataByte.Length > 0)
                {
                    //验证在得到结果时是否有传入数据
                    buffer = item.PostdataByte;
                }//写入文件
                else if (item.PostDataType == PostDataType.FilePath && !string.IsNullOrWhiteSpace(item.Postdata))
                {
                    StreamReader r = new StreamReader(item.Postdata, postencoding);
                    buffer = postencoding.GetBytes(r.ReadToEnd());
                    r.Close();
                } //写入字符串
                else if (!string.IsNullOrWhiteSpace(item.Postdata))
                {
                    buffer = postencoding.GetBytes(item.Postdata);
                }
                if (buffer != null)
                {
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }
                else
                {
                    request.ContentLength = 0;
                }
            }
        }
        /// <summary>
        /// 设置代理
        /// </summary>
        /// <param name="item">参数对象</param>
        private void SetProxy(HttpItem item)
        {
            bool isIeProxy = false;
            if (!string.IsNullOrWhiteSpace(item.ProxyIp))
            {
                isIeProxy = item.ProxyIp.ToLower().Contains("ieproxy");
            }
            if (!string.IsNullOrWhiteSpace(item.ProxyIp) && !isIeProxy)
            {
                //设置代理服务器
                if (item.ProxyIp.Contains(":"))
                {
                    string[] plist = item.ProxyIp.Split(':');
                    WebProxy myProxy = new WebProxy(plist[0].Trim(), Convert.ToInt32(plist[1].Trim()));
                    //建议连接
                    myProxy.Credentials = new NetworkCredential(item.ProxyUserName, item.ProxyPwd);
                    //给当前请求对象
                    request.Proxy = myProxy;
                }
                else
                {
                    WebProxy myProxy = new WebProxy(item.ProxyIp, false);
                    //建议连接
                    myProxy.Credentials = new NetworkCredential(item.ProxyUserName, item.ProxyPwd);
                    //给当前请求对象
                    request.Proxy = myProxy;
                }
            }
            else if (isIeProxy)
            {
                //设置为IE代理
            }
            else
            {
                request.Proxy = item.WebProxy;
            }
        }
        #endregion

        #region private main
        /// <summary>
        /// 回调验证证书问题
        /// </summary>
        /// <param name="sender">流对象</param>
        /// <param name="certificate">证书</param>
        /// <param name="chain">X509Chain</param>
        /// <param name="errors">SslPolicyErrors</param>
        /// <returns>bool</returns>
        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) { return true; }

        /// <summary>
        /// 通过设置这个属性，可以在发出连接的时候绑定客户端发出连接所使用的IP地址。 
        /// </summary>
        /// <param name="servicePoint"></param>
        /// <param name="remoteEndPoint"></param>
        /// <param name="retryCount"></param>
        /// <returns></returns>
        public IPEndPoint BindIPEndPointCallback(ServicePoint servicePoint, IPEndPoint remoteEndPoint, int retryCount)
        {
            return _IPEndPoint;//端口号
        }

        #endregion
    }
}
