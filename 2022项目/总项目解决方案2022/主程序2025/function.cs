
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using NPOI.SS.UserModel;
using NPOI.SS.Formula.Eval;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;

namespace 主程序2025
{
    internal class function
    {

        public static string date = "2025-05-20";

        #region  程序关闭删除自身
        public static void TestForKillMyself()
        {
            string bat = @"@echo off
                           :tryagain
                           del %1
                           if exist %1 goto tryagain
                           del %0";
            File.WriteAllText("killme.bat", bat);//写bat文件
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "killme.bat";
            psi.Arguments = "\"" + Environment.GetCommandLineArgs()[0] + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
        }

        /// <summary>
        /// 获取时间戳毫秒
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalMilliseconds);
            return a.ToString();
        }

        public static string Md5_utf8(string str)
        {
            //将输入字符串转换成字节数组
            var buffer = Encoding.GetEncoding("utf-8").GetBytes(str);
            //接着，创建Md5对象进行散列计算
            var data = MD5.Create().ComputeHash(buffer);

            //创建一个新的Stringbuilder收集字节
            var sb = new StringBuilder();

            //遍历每个字节的散列数据 
            foreach (var t in data)
            {
                //格式每一个十六进制字符串
                sb.Append(t.ToString("X2"));
            }

            //返回十六进制字符串
            return sb.ToString().ToLower();
        }

        #endregion


        #region GET请求带COOKIE
        /// <summary>
        /// GET请求带COOKIE
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlWithCookie(string Url, string COOKIE, string charset)
        {
            string html = "";

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                // request.Proxy = null;//防止代理抓包
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 17_6_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 AliApp(1688/11.56.1) WindVane/8.7.2 A2U/x 1170x2532 x-i18n/zh-CN WK";
                WebHeaderCollection headers = request.Headers;
                headers.Add("f-pTraceId: WVNet_WV_6-6-124");
                headers.Add("bx-v: 2.5.11, 2.5.11");
                headers.Add("f-refer: wv_h5");
                headers.Add("Sec-Fetch-Site: same-origin");
                headers.Add("Sec-Fetch-Dest: empty");

                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
               
                //request.AllowAutoRedirect = true;
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Referer = "https://search.1688.com/company/wap/factory_search.htm?_wvUseWKWebView=true&__existtitle__=1&__nosearchbox__=1&tabCode=findFactoryTab&key=%E5%A5%B3%E8%A3%85&verticalProductFlag=wapfactory&_layoutMode_=noSort&source=search_input&searchBy=input";
                request.Timeout = 5000;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }

                response.Close();
                return html;



            }
            catch (System.Exception ex)
            {

                return ex.ToString();

            }

        }
        #endregion

        #region GET请求获取Set-cookie
        public static string getSetCookie(string url,string COOKIE)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);  //创建一个链接
            request.Timeout = 10000;
            request.Headers.Add("Cookie", COOKIE);
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36";
            request.AllowAutoRedirect = true;
            request.KeepAlive = true;
            request.Referer = url;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

            string content = response.GetResponseHeader("Set-Cookie"); ;
            return content;


        }
        #endregion



        #region POST默认请求
        public static string PostUrlDefault(string url, string postData, string COOKIE)
        {
            string result;
            try
            {
                string charset = "utf-8";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.Proxy = null;
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("version:TYC-XCX-WX");
                request.ContentType = "application/x-www-form-urlencoded";
                // request.ContentType = "application/json";
                request.ContentLength = (long)Encoding.UTF8.GetBytes(postData).Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = false;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
                request.Referer = url;
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                response.GetResponseHeader("Set-Cookie");
                bool flag = response.Headers["Content-Encoding"] == "gzip";
                string html;
                if (flag)
                {
                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader2 = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset));
                    html = reader2.ReadToEnd();
                    reader2.Close();
                }
                response.Close();
                result = html;
            }
            catch (WebException ex)
            {
                //result = ex.ToString();
                //400错误也返回内容
                using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }
        #endregion



        #region GET请求带COOKIE
        /// <summary>
        /// GET请求带COOKIE
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlWithCookie_ip(string Url, string COOKIE, string ip, string port, string username, string password)
        {
            string html = "";

            try
            {

                string charset = "utf-8";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                // request.Proxy = null;//防止代理抓包
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 17_6_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 AliApp(1688/11.56.1) WindVane/8.7.2 A2U/x 1170x2532 x-i18n/zh-CN WK";
                WebHeaderCollection headers = request.Headers;
                headers.Add("f-pTraceId: WVNet_WV_6-6-124");
                headers.Add("bx-v: 2.5.11, 2.5.11");
                headers.Add("f-refer: wv_h5");
                headers.Add("Sec-Fetch-Site: same-origin");
                headers.Add("Sec-Fetch-Dest: empty");
                // 设置代理 <用户名密码>
                WebProxy proxy = new WebProxy();
                proxy.Address = new Uri(String.Format("http://{0}:{1}", ip, port));
                proxy.Credentials = new NetworkCredential(username, password);
                request.Proxy = proxy;
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");

                //request.AllowAutoRedirect = true;
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Referer = "https://search.1688.com/company/wap/factory_search.htm?_wvUseWKWebView=true&__existtitle__=1&__nosearchbox__=1&tabCode=findFactoryTab&key=%E5%A5%B3%E8%A3%85&verticalProductFlag=wapfactory&_layoutMode_=noSort&source=search_input&searchBy=input";
                request.Timeout = 5000;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }

                response.Close();
                return html;



            }
            catch (System.Exception ex)
            {

                return ex.ToString();

            }

        }
        #endregion


        #region POST代理
        public static string PostUrl_daili(string url, string postData, string COOKIE,string ip,string port,string username,string password)
        {
            string result;
            try
            {
                string charset = "utf-8";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";

                // 设置代理 <用户名密码>
                WebProxy proxy = new WebProxy();
                proxy.Address = new Uri(String.Format("http://{0}:{1}", ip, port));
                proxy.Credentials = new NetworkCredential(username, password);
                request.Proxy = proxy;


                request.ContentType = "application/x-www-form-urlencoded";
                // request.ContentType = "application/json";
                request.ContentLength = (long)Encoding.UTF8.GetBytes(postData).Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = false;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
                request.Referer = url;
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                response.GetResponseHeader("Set-Cookie");
                bool flag = response.Headers["Content-Encoding"] == "gzip";
                string html;
                if (flag)
                {
                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader2 = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset));
                    html = reader2.ReadToEnd();
                    reader2.Close();
                }
                response.Close();
                result = html;
            }
            catch (WebException ex)
            {
                result = ex.ToString();
               
            }
            return result;
        }
        #endregion


        public static string runpython(string url,string pythonScriptPath)
        {
           
            string output = "111";
            string error = "222";
            // Python 脚本的路径，请根据实际情况修改
           

            // 要传递给 Python 脚本的参数
            string argument = url;

            // 创建进程启动信息
            ProcessStartInfo startInfo = new ProcessStartInfo();
            // 假设 Python 已添加到系统环境变量
            startInfo.FileName = "python";
            startInfo.Arguments = $"{pythonScriptPath} {argument}";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;

            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo = startInfo;
                    process.Start();

                    // 获取 Python 脚本的标准输出
                    output = process.StandardOutput.ReadToEnd();
                    // 获取 Python 脚本的错误输出
                    error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        Console.WriteLine("Python 脚本执行成功，输出结果如下：");
                        Console.WriteLine(output);
                    }
                    else
                    {
                        Console.WriteLine("Python 脚本执行失败，错误信息如下：");
                        Console.WriteLine(error);
                    }
                }

                return output;
            }
            catch (Exception ex)
            {
               
                return ex.ToString();
            }
        }



        public static string getx5(string action,string url)
        {
            string apython = AppDomain.CurrentDomain.BaseDirectory + @"a.py";
            string bpython = AppDomain.CurrentDomain.BaseDirectory + @"b.py";
            string x5sec = "";
          
            if (action == "captcha")
            {
                string html = function.runpython(url, apython);
               
                x5sec = "x5sec=" + Regex.Match(html, @"x5sec=([\s\S]*?)=").Groups[1].Value;
            }

            if (action== "captchacapslidev2")
            {
              string html=  function.runpython(url, bpython);
                  x5sec = "x5sec="+Regex.Match(html, @"x5sec=([\s\S]*?)=").Groups[1].Value;
            }

            // MessageBox.Show(function.runpython("https://ditu.amap.com/detail/get/detail",apython));


            //string shuiguoUrl = "https://rest-sig.imaitix.com//api/user/userLogin/_____tmd_____/punish?reqeust=getpunishpage&source=xagent&x5secdata=xcjdo7RF4AYoD1%2fIUrIzu0vTppBIukYTSzK25EjWDbyrA6tGu4DQSGRJnNas9sSvPGRL%2f0Q%2bJTLD3pW2DnBC4wVssE5Qf6w6tJvn9axCXsvUMDGMSkM%2flOA5pVnSE%2feCgFEghYbP7QJRnwSJPKToZ0avJR6zctlHSfyBsnwaEphtY96RU9MlBEqBc08jjo77sHcgfWc25MBa%2f5wlI6562Fy1CcO3hWb69egCzYpt2yo0QAGzrQTlV%2bhfJxg4epc6lL__bx__rest-sig.imaitix.com%2fapi%2fuser%2fuserLogin&x5step=2";


            return x5sec;
        }



        #region Excel转datatable
       



        public static DataTable ExcelToDataTable(string filePath, bool hasHeader = true)
        {
            IWorkbook workbook;
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                if (filePath.EndsWith(".xlsx"))
                {
                    workbook = new XSSFWorkbook(fileStream);
                }
                else if (filePath.EndsWith(".xls"))
                {
                    workbook = new HSSFWorkbook(fileStream);
                }
                else
                {
                    throw new Exception("不支持的文件格式");
                }
            }

            ISheet sheet = workbook.GetSheetAt(0); // 假设读取第一个工作表

            DataTable dataTable = new DataTable();
            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            if (hasHeader)
            {
                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    DataColumn column = new DataColumn(headerRow.GetCell(i)?.ToString());
                    dataTable.Columns.Add(column);
                }

                for (int i = (hasHeader ? 1 : 0); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    DataRow dataRow = dataTable.NewRow();

                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            switch (row.GetCell(j).CellType)
                            {
                                case CellType.String:
                                    dataRow[j] = row.GetCell(j).StringCellValue;
                                    break;
                                case CellType.Numeric:
                                    if (DateUtil.IsCellDateFormatted(row.GetCell(j)))
                                    {
                                        dataRow[j] = row.GetCell(j).DateCellValue;
                                    }
                                    else
                                    {
                                        dataRow[j] = row.GetCell(j).NumericCellValue;
                                    }
                                    break;
                                case CellType.Boolean:
                                    dataRow[j] = row.GetCell(j).BooleanCellValue;
                                    break;
                                case CellType.Formula:
                                    dataRow[j] = row.GetCell(j).CellFormula;
                                    break;
                                case CellType.Blank:
                                    dataRow[j] = "";
                                    break;
                                default:
                                    dataRow[j] = "未知类型";
                                    break;
                            }
                        }
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }
            else
            {
                for (int i = 0; i < cellCount; i++)
                {
                    DataColumn column = new DataColumn($"Column_{i}");
                    dataTable.Columns.Add(column);
                }

                for (int i = 0; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    DataRow dataRow = dataTable.NewRow();

                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            switch (row.GetCell(j).CellType)
                            {
                                case CellType.String:
                                    dataRow[j] = row.GetCell(j).StringCellValue;
                                    break;
                                case CellType.Numeric:
                                    if (DateUtil.IsCellDateFormatted(row.GetCell(j)))
                                    {
                                        dataRow[j] = row.GetCell(j).DateCellValue;
                                    }
                                    else
                                    {
                                        dataRow[j] = row.GetCell(j).NumericCellValue;
                                    }
                                    break;
                                case CellType.Boolean:
                                    dataRow[j] = row.GetCell(j).BooleanCellValue;
                                    break;
                                case CellType.Formula:
                                    dataRow[j] = row.GetCell(j).CellFormula;
                                    break;
                                case CellType.Blank:
                                    dataRow[j] = "";
                                    break;
                                default:
                                    dataRow[j] = "未知类型";
                                    break;
                            }
                        }
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }
    
        #endregion





    }
}
