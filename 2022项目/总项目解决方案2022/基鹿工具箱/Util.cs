using MySql.Data.MySqlClient;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Eval;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 基鹿工具箱
{
    class Util
    {
       static string COOKIE = "cna=IEEVGmxNb3QCAXnisujqtGPv; taklid=38e78512859846e58e75c42be1833a02; hng=CN%7Czh-CN%7CCNY%7C156; lid=zkg852266010; ali_ab=121.226.159.98.1645245827623.1; ali_apache_track=c_mid=b2b-1052347548|c_lid=zkg852266010|c_ms=1|c_mt=2; _m_h5_tk=3e9666f327a4af69b19b6d7adbb23eb8_1649325439454; _m_h5_tk_enc=d7732f7b2298a8cd29d69f807bcbeccf; xlly_s=1; cookie2=2a6f52413f2e48cd63a5da6bc8191e9e; sgcookie=E100kZBFanIDZNPfcot4VcewKkmUiR9sB7C4rpdjYQP6x25S3n2aTZ8486eH618tvfau6C2Z38RJvIPMt%2BGbTm3%2FCPvaFgnZmBQxYX9X74V0BPEAcMJWWZxMsEutNsxXs7Q5; t=bb42afa080af1dca98fb8d6a52dc1684; _tb_token_=e6859ebee1b17; uc4=nk4=0%40GwrkntVPltPB9cR46GncAmsyVe%2Fk0gQ%3D&id4=0%40UOnlZ%2FcoxCrIUsehK6jr4tbgfrWs; __cn_logon__=false; alicnweb=touch_tb_at%3D1649316804592%7Clastlogonid%3Dzkg852266010%7Cshow_inter_tips%3Dfalse; keywordsHistory=%E9%92%A2%E7%AD%8B%E9%92%A9; _csrf_token=1649319453013; tfstk=c8cRB2aZUnxubUUx7Ypm8uykH_M5aWxLcaZh9Xh0ACwwxaCdOs2isfQ5oLaQbuLA.; l=eBNaJtqgg-gKy7-ABO5ZKurza7791IOfGsPzaNbMiInca6tGXnutLNC3TKQwpdtj_tfjEeKrTan6EdE2SJ438x125OM8ARjSH6v6-; isg=BCgooAfp3oWAkvHrPLKOYWNs-RY6UYxbHQ9mG-JYFKMEPcyni2JB6oc7Nd3NDUQz";
      public  static string mobile= "";
        public static string logintoken = "";

        #region NPOI导出表格
        public static int DataTableToExcel(DataTable data, string sheetName, bool isColumnWritten)
        {
            int i = 0;
            int j = 0;
            int count = 0;
            ISheet sheet = null;
            IWorkbook workbook = null;
            FileStream fs = null;
            // bool disposed;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "xlsx|*.xls|xlsx|*.xlsx";
            //sfd.Filter = "xlsx|*.xlsx";
            sfd.Title = "Excel文件导出";
            string fileName = "";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                fileName = sfd.FileName;
            }
            else
                return -1;

            fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                workbook = new XSSFWorkbook();
            else if (fileName.IndexOf(".xls") > 0) // 2003版本
                workbook = new HSSFWorkbook();

            try
            {
                if (workbook != null)
                {
                    sheet = workbook.CreateSheet(sheetName);
                    ICellStyle style = workbook.CreateCellStyle();
                    style.FillPattern = FillPattern.SolidForeground;

                }
                else
                {
                    return -1;
                }

                if (isColumnWritten == true) //写入DataTable的列名
                {
                    IRow row = sheet.CreateRow(0);
                    for (j = 0; j < data.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data.Columns[j].ColumnName);

                    }
                    count = 1;
                }
                else
                {
                    count = 0;
                }

                for (i = 0; i < data.Rows.Count; ++i)
                {
                    IRow row = sheet.CreateRow(count);
                    for (j = 0; j < data.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
                    }
                    ++count;
                }
                workbook.Write(fs); //写入到excel
                workbook.Close();
                fs.Close();
                System.Diagnostics.Process[] Proc = System.Diagnostics.Process.GetProcessesByName("");
                MessageBox.Show("数据导出完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return -1;
            }
        }

        #endregion

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            string html = "";
            
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36";
                request.Referer = Url;
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
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
                request.AllowAutoRedirect = true;
                // request.Proxy = null;//防止代理抓包
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36";
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                //request.Referer = Url;
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;

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

        #region 获取txt编码
        //调用：EncodingType.GetTxtType(textBox1.Text)
        public class EncodingType
        {
            /// <summary> 
            /// 给定文件的路径，读取文件的二进制数据，判断文件的编码类型 
            /// </summary> 
            /// <param name=“FILE_NAME“>文件路径</param> 
            /// <returns>文件的编码类型</returns> 
            public static System.Text.Encoding GetTxtType(string FILE_NAME)
            {
                FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read);
                Encoding r = GetType(fs);
                fs.Close();
                return r;
            }

            /// <summary> 
            /// 通过给定的文件流，判断文件的编码类型 
            /// </summary> 
            /// <param name=“fs“>文件流</param> 
            /// <returns>文件的编码类型</returns> 
            public static System.Text.Encoding GetType(FileStream fs)
            {
                byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
                byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
                byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM 
                Encoding reVal = Encoding.Default;

                BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
                int i;
                int.TryParse(fs.Length.ToString(), out i);
                byte[] ss = r.ReadBytes(i);
                if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
                {
                    reVal = Encoding.UTF8;
                }
                else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
                {
                    reVal = Encoding.BigEndianUnicode;
                }
                else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
                {
                    reVal = Encoding.Unicode;
                }
                r.Close();
                return reVal;

            }

            /// <summary> 
            /// 判断是否是不带 BOM 的 UTF8 格式 
            /// </summary> 
            /// <param name=“data“></param> 
            /// <returns></returns> 
            private static bool IsUTF8Bytes(byte[] data)
            {
                int charByteCounter = 1; //计算当前正分析的字符应还有的字节数 
                byte curByte; //当前分析的字节. 
                for (int i = 0; i < data.Length; i++)
                {
                    curByte = data[i];
                    if (charByteCounter == 1)
                    {
                        if (curByte >= 0x80)
                        {
                            //判断当前 
                            while (((curByte <<= 1) & 0x80) != 0)
                            {
                                charByteCounter++;
                            }
                            //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X 
                            if (charByteCounter == 1 || charByteCounter > 6)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        //若是UTF-8 此时第一位必须为1 
                        if ((curByte & 0xC0) != 0x80)
                        {
                            return false;
                        }
                        charByteCounter--;
                    }
                }
                if (charByteCounter > 1)
                {
                    throw new Exception("非预期的byte格式");
                }
                return true;
            }
        }

        #endregion

        #region POST默认请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrlDefault(string url, string postData, string COOKIE)
        {
            try
            {
                string time = DateTime.Now.ToString("yyyyMMddHHmmss");
                string sign = Sha1(Md5(key+time));
                string charset = "utf-8";
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.Proxy = null;//防止代理抓包
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("ASINLU-TIME:"+time);
                headers.Add("ASINLU-SIGN:"+sign);
                //headers.Add("sec-fetch-user:?1");
                //headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                //request.ContentType = "application/x-www-form-urlencoded";
                request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                // request.ContentLength = postData.Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = url;
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");

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
                //return key+"   "+time+"   "+ Md5(key + time)+"   "+sign+"   "+html;
                return html;
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion

        #region GET请求获取Set-cookie
        public static string getSetCookie(string url)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);  //创建一个链接
            request.Timeout = 10000;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
            request.AllowAutoRedirect = false;

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

            string content = response.GetResponseHeader("Set-Cookie"); ;
            return content;


        }
        #endregion

        public static string Md5(string str)
        {
            //将输入字符串转换成字节数组
            var buffer = Encoding.Default.GetBytes(str);
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

        /// <summary>
        /// 基于Sha1的自定义加密字符串方法：输入一个字符串，返回一个由40个字符组成的十六进制的哈希散列（字符串）。
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>加密后的十六进制的哈希散列（字符串）</returns>
        public static string Sha1(string str)
        {
            Encoding encode = Encoding.GetEncoding("utf-8");
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytes_in = encode.GetBytes(str);
            byte[] bytes_out = sha1.ComputeHash(bytes_in);
            sha1.Dispose();
            string result = BitConverter.ToString(bytes_out);
            result = result.Replace("-", "");
            return result.ToLower();

        }

        public static string key = "Hie345)(^@#34g**-+534hdF4~`13";
        public static string domain = "www.asinlu.com";

        public static string register()
        {
            string url = "http://"+domain+"/Api/register";
            string postdata = "{\"username\":\"123\",\"mobile\":\"12345678900\",\"password\":\"123\"}";
            string html = PostUrlDefault(url,postdata,"");
            return html;

        }

        public static string expiretime = "2022-01-01";
        #region unicode转中文
        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }

        #endregion;
        public static string login(string mobile,string password)
        {
            string url = "http://" + domain + "/Api/login";
            string postdata = "{\"mobile\":\""+mobile+"\",\"password\":\""+password+"\"}";
            string html = PostUrlDefault(url, postdata, "");
            return html;

        }

        public static string getuser(string mobile, string token)
        {
            string url = "http://" + domain + "/Api/service";
            string postdata = "{\"mobile\":\"" + mobile + "\",\"token\":\"" + token + "\"}";
            string html = PostUrlDefault(url, postdata, "");
            return html;

        }


        #region listview转datable
        /// <summary>
        /// listview转datable
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        public static DataTable listViewToDataTable(ListView lv)
        {
            int i, j;
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Clear();
            dt.Columns.Clear();
            //lv.Columns.Count
            //生成DataTable列头
            for (i = 0; i < lv.Columns.Count; i++)
            {
                dt.Columns.Add(lv.Columns[i].Text.Trim(), typeof(String));
            }
            //每行内容
            for (i = 0; i < lv.Items.Count; i++)
            {
                dr = dt.NewRow();
                for (j = 0; j < lv.Columns.Count; j++)
                {
                    try
                    {
                        dr[j] = lv.Items[i].SubItems[j].Text.Trim();

                    }
                    catch
                    {

                        continue;
                    }

                }
                dt.Rows.Add(dr);
            }

            return dt;
        }
        #endregion

        

        #region NPOI读取表格导入datatable 
        /// <summary>  
        /// 将excel导入到datatable  
        /// </summary>  
        /// <param name="filePath">excel路径</param>  
        /// <param name="isColumnName">第一行是否是列名</param>  
        /// <returns>返回datatable</returns>  
        public static DataTable ExcelToDataTable(string filePath, bool isColumnName)
        {
            DataTable dataTable = null;
            FileStream fs = null;
            DataColumn column = null;
            DataRow dataRow = null;
            IWorkbook workbook = null;
            ISheet sheet = null;
            IRow row = null;
            ICell cell = null;
            int startRow = 0;
            try
            {
                using (fs = File.OpenRead(filePath))
                {
                    // 2007版本  
                    if (filePath.IndexOf(".xlsx") > 0)
                        workbook = new XSSFWorkbook(fs);
                    // 2003版本  
                    else if (filePath.IndexOf(".xls") > 0)
                        workbook = new HSSFWorkbook(fs);

                    if (workbook != null)
                    {
                        sheet = workbook.GetSheetAt(0);//读取第一个sheet，当然也可以循环读取每个sheet  
                        dataTable = new DataTable();
                        if (sheet != null)
                        {
                            int rowCount = sheet.LastRowNum;//总行数  
                            if (rowCount > 0)
                            {
                                IRow firstRow = sheet.GetRow(0);//第一行  
                                int cellCount = firstRow.LastCellNum;//列数  

                                //构建datatable的列  
                                if (isColumnName)
                                {
                                    startRow = 1;//如果第一行是列名，则从第二行开始读取  
                                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                    {
                                        cell = firstRow.GetCell(i);
                                        if (cell != null)
                                        {
                                            if (cell.StringCellValue != null)
                                            {
                                                column = new DataColumn(cell.StringCellValue);
                                                dataTable.Columns.Add(column);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                    {
                                        column = new DataColumn("column" + (i + 1));
                                        dataTable.Columns.Add(column);
                                    }
                                }

                                //填充行  
                                for (int i = startRow; i <= rowCount; ++i)
                                {
                                    row = sheet.GetRow(i);
                                    if (row == null) continue;

                                    dataRow = dataTable.NewRow();
                                    for (int j = row.FirstCellNum; j < cellCount; ++j)
                                    {

                                        ICell RCells = row.GetCell(j);
                                        if (RCells != null)
                                        {
                                            try
                                            {
                                                switch (RCells.CellType)  //注意按单元格格式分类取值
                                                {
                                                    case CellType.Numeric:    //用于取出数值和公式类型的数据 
                                                        if (DateUtil.IsCellDateFormatted(RCells)) { dataRow[j] = RCells.DateCellValue.ToString("yyyy/MM/dd HH:mm:ss"); }
                                                        else { dataRow[j] = RCells.NumericCellValue; }


                                                        break;
                                                    case CellType.Error:
                                                        dataRow[j] = ErrorEval.GetText(row.GetCell(j).ErrorCellValue);
                                                        break;
                                                    case CellType.Formula:
                                                        switch (row.GetCell(j).CachedFormulaResultType)
                                                        {
                                                            case CellType.String:
                                                                string strFORMULA = row.GetCell(j).StringCellValue;
                                                                if (strFORMULA != null && strFORMULA.Length > 0)
                                                                {
                                                                    dataRow[j] = strFORMULA.ToString();
                                                                }
                                                                else
                                                                {
                                                                    dataRow[j] = null;
                                                                }
                                                                break;
                                                            case CellType.Numeric:
                                                                dataRow[j] = Convert.ToString(row.GetCell(j).NumericCellValue);
                                                                break;
                                                            case CellType.Boolean:
                                                                dataRow[j] = Convert.ToString(row.GetCell(j).BooleanCellValue);
                                                                break;
                                                            case CellType.Error:
                                                                dataRow[j] = ErrorEval.GetText(row.GetCell(j).ErrorCellValue);
                                                                break;
                                                            default:
                                                                dataRow[j] = "";
                                                                break;
                                                        }
                                                        break;
                                                    case CellType.Boolean:
                                                        // Boolean type
                                                        dataRow[j] = RCells.BooleanCellValue.ToString();
                                                        break;

                                                    case CellType.Blank:
                                                        break;

                                                    default:
                                                        // String type
                                                        dataRow[j] = RCells.StringCellValue.Trim();
                                                        break;
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                //MessageBox.Show(e.Message);
                                                continue;
                                                //MessageBox.Show(e.ToString());
                                            }
                                        }
                                        else { dataRow[j] = ""; }


                                    }
                                    dataTable.Rows.Add(dataRow);
                                }
                            }
                        }
                    }
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                if (fs != null)
                {
                    fs.Close();
                }
                return null;
            }
        }

        #endregion
        static string conn = "Host =47.96.189.55;Database=titledb;Username=root;Password=root";
        #region 绑定数据
        public static DataTable getdata(string sql)
        {
            
            
            MySqlDataAdapter sda = new MySqlDataAdapter(sql, conn);
            DataSet Ds = new DataSet();
            sda.Fill(Ds, "T_Class");

            DataTable dt = Ds.Tables["T_Class"];
            return dt;
        }

        #endregion


        public static bool SQL(string sql)
        {
            try
            {

                MySqlConnection mycon = new MySqlConnection(conn);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand(sql, mycon);

                int status = cmd.ExecuteNonQuery();  //执行sql语句
                if (status > 0)
                {
                    return true;

                }

                mycon.Close();
                return false;
            }
            catch (Exception)
            {


                return false;
            }



        }
        public static bool insert(string a,string b,string c,string d,string e)
        {
           
            MySqlConnection mycon = new MySqlConnection(conn);
            mycon.Open();
           // string sql = "INSERT INTO datas (ci,ss_zs,fd,good_zs,gx_zs)VALUES('" + a + " ', '" + b + " ', '" + c + " ', '" + d + " ', '" + e + " ')";
           string sql = "INSERT INTO datas(ci,ss_zs,fd,good_zs,gx_zs) VALUES('" + a + " ', '" + b + " ', '" + c + " ', '" + d + " ', '" + e + " ') ON DUPLICATE KEY UPDATE ss_zs = '" + b + " ',fd = '" + c + " ',good_zs = '" + d + " ',gx_zs = '" + e+ " '"; 
            MySqlCommand cmd = new MySqlCommand(sql, mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


            int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
            if (count > 0)
            {
                mycon.Close();
                return true;
               

            }
            else
            {
                mycon.Close();
                return false;
            }
            
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

        public static List<string> keys_list = new List<string>();

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

        public int saledCount =0;
        public int saledCount2 = 0;

        public string sellerLoginId = "";
        public string payOrder30DayStr = "";
        public  Dictionary<DateTime, int> reviewtimedic = new Dictionary<DateTime, int>();
        public Dictionary<string, int> reviewcountdic = new Dictionary<string, int>();
        public bool stop = false;

        string reviewcookie = "_m_h5_tk=92db0fa413f671f2890f3f01433abdb1_1650879394197;_m_h5_tk_enc=d85cbacbe17135f56c54809f7f00c70f; ";
        string token = "";






        public void getreview(object o)
        {

            saledCount = 0;
            saledCount2 = 0;
            try
            {
               
                // string[] text = o.ToString().Split(new string[] { "," }, StringSplitOptions.None);
                //string itemid = text[0];
                //string logid = text[1];
                string itemid = o.ToString().Trim();

                getsalecount(itemid);



                for (int page = 1; page < 101; page++)
                {
                    string time = Util.GetTimeStamp();
                    string str = token+"&" + time + "&12574478&{\"page\":"+page+",\"pageSize\":10,\"tagType\":\"common\",\"tagName\":\"全部\",\"scene\":\"item\",\"itemId\":\"" + itemid + "\",\"loginId\":\"" + sellerLoginId + "\"}";
                  
                    string sign = Md5_utf8(str);

                    string url = "https://h5api.m.1688.com/h5/mtop.1688.trade.service.mtoprateservice.queryitemratedlistv2/1.0/?jsv=2.4.11&appKey=12574478&t=" + time + "&sign=" + sign + "&api=mtop.1688.trade.service.MtopRateService.queryItemRatedListV2&v=1.0&type=jsonp&isSec=0&timeout=20000&dataType=jsonp&callback=mtopjsonp21&data=%7B%22page%22%3A"+page+"%2C%22pageSize%22%3A10%2C%22tagType%22%3A%22common%22%2C%22tagName%22%3A%22%E5%85%A8%E9%83%A8%22%2C%22scene%22%3A%22item%22%2C%22itemId%22%3A%22" + itemid + "%22%2C%22loginId%22%3A%22" + sellerLoginId + "%22%7D";
                   
                    
                    string html = GetUrlWithCookie (url, reviewcookie, "utf-8");
                  
                    if(html.Contains("令牌过期"))
                    {
                        string cookiestr = getSetCookie(url);
                        string _m_h5_tk = "_m_h5_tk="+Regex.Match(cookiestr, @"_m_h5_tk=([\s\S]*?);").Groups[1].Value;
                        string _m_h5_tk_enc = "_m_h5_tk_enc="+Regex.Match(cookiestr, @"_m_h5_tk_enc=([\s\S]*?);").Groups[1].Value;
                        reviewcookie = _m_h5_tk + ";" + _m_h5_tk_enc + ";";
                        token = Regex.Match(reviewcookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;
                        page = page - 1;
                        continue;
                        
                    }
        
                    MatchCollection skunames = Regex.Matches(html, @"""specInfo"":""([\s\S]*?)""");
                    MatchCollection times = Regex.Matches(html, @"""gmtCreate"":""([\s\S]*?)""");
                    MatchCollection quantitys = Regex.Matches(html, @"""quantity"":""([\s\S]*?)""");

                    if (skunames.Count == 0)
                        break;
                    for (int i = 0; i < skunames.Count; i++)
                    {
                        
                        string skuname="";
                        DateTime skutime =Convert.ToDateTime( Convert.ToDateTime(times[i].Groups[1].Value).ToShortDateString());
                        int quantity = Convert.ToInt32(quantitys[i].Groups[1].Value.Replace(".0",""));

                        saledCount2 = saledCount2 + 1;
                        if (!reviewtimedic.ContainsKey(skutime))
                        {
                            reviewtimedic.Add(skutime, 1);
                        }
                        else
                        {
                            reviewtimedic[skutime] = reviewtimedic[skutime] + 1;
                        }


                        if (!skunames[i].Groups[1].Value.Contains("7C"))
                        {
                          
                            saledCount = saledCount + quantity;
                         


                            string[] skus = skunames[i].Groups[1].Value.Split(new string[] { "#" }, StringSplitOptions.None);
                            for (int a = 0; a < skus.Length; a++)
                            {
                                if (skus[a].Contains("3B"))
                                {
                                    skuname = skuname + skus[a].Replace("3B","");
                                   
                                }

                            }


                            if (!reviewcountdic.ContainsKey(skuname))
                            {
                                reviewcountdic.Add(skuname, quantity);
                            }
                            else
                            {
                                reviewcountdic[skuname] = reviewcountdic[skuname] + quantity;
                            }

                            


                        }



                    }

                   

                }
                stop = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            
        }




        public void getsalecount(string itemid)
        {
           

            token = Regex.Match(reviewcookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;
           string time = Util.GetTimeStamp();
            string str = token + "&" + time + "&12574478&{\"cid\":\"offerdetailGetShopInfo:offerdetailGetShopInfo\",\"methodName\":\"execute\",\"params\":\"{\\\"offerId\\\":" + itemid + ",\\\"businessType\\\":\\\"default\\\"}\"}";
            string sign = Md5_utf8(str);

            string aurl = "https://h5api.m.1688.com/h5/mtop.taobao.widgetservice.getjsoncomponent/1.0/?jsv=2.4.8&appKey=12574478&t=" + time + "&sign=" + sign + "&api=mtop.taobao.widgetService.getJsonComponent&v=1.0&type=jsonp&isSec=0&timeout=20000&dataType=jsonp&callback=mtopjsonp9&data=%7B%22cid%22%3A%22offerdetailGetShopInfo%3AofferdetailGetShopInfo%22%2C%22methodName%22%3A%22execute%22%2C%22params%22%3A%22%7B%5C%22offerId%5C%22%3A" + itemid + "%2C%5C%22businessType%5C%22%3A%5C%22default%5C%22%7D%22%7D";

            string html = GetUrlWithCookie(aurl, reviewcookie, "utf-8");

         
            if(html.Contains("令牌过期"))
            {
                string cookiestr = getSetCookie(aurl);
                string _m_h5_tk = "_m_h5_tk=" + Regex.Match(cookiestr, @"_m_h5_tk=([\s\S]*?);").Groups[1].Value;
                string _m_h5_tk_enc = "_m_h5_tk_enc=" + Regex.Match(cookiestr, @"_m_h5_tk_enc=([\s\S]*?);").Groups[1].Value;
                reviewcookie = _m_h5_tk + ";" + _m_h5_tk_enc + ";";

                token = Regex.Match(reviewcookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;
                 time = Util.GetTimeStamp();
                str = token + "&" + time + "&12574478&{\"cid\":\"offerdetailGetShopInfo:offerdetailGetShopInfo\",\"methodName\":\"execute\",\"params\":\"{\\\"offerId\\\":" + itemid + ",\\\"businessType\\\":\\\"default\\\"}\"}";

                sign = Md5_utf8(str);

                aurl = "https://h5api.m.1688.com/h5/mtop.taobao.widgetservice.getjsoncomponent/1.0/?jsv=2.4.8&appKey=12574478&t=" + time + "&sign=" + sign + "&api=mtop.taobao.widgetService.getJsonComponent&v=1.0&type=jsonp&isSec=0&timeout=20000&dataType=jsonp&callback=mtopjsonp9&data=%7B%22cid%22%3A%22offerdetailGetShopInfo%3AofferdetailGetShopInfo%22%2C%22methodName%22%3A%22execute%22%2C%22params%22%3A%22%7B%5C%22offerId%5C%22%3A" + itemid + "%2C%5C%22businessType%5C%22%3A%5C%22default%5C%22%7D%22%7D";

               
                html = GetUrlWithCookie(aurl, reviewcookie, "utf-8");
            }
          
            sellerLoginId = Regex.Match(html, @"""loginId"":""([\s\S]*?)""").Groups[1].Value;










            try
            {
              
                //string url = "https://detail.1688.com/offer/"+itemid+".html";
                //string html = GetUrlWithCookie(url, "", "utf-8");
              
                // saledCount = Regex.Match(html, @"""saledCount"":""([\s\S]*?)""").Groups[1].Value;
                // sellerLoginId = Regex.Match(html, @"""sellerLoginId"":""([\s\S]*?)""").Groups[1].Value;
                // payOrder30DayStr = Regex.Match(html, @"payOrder30DayStr\\"":\\""([\s\S]*?)\\""").Groups[1].Value;

               


            }
            catch (Exception ex)
            {
               
                MessageBox.Show(ex.ToString());
               
            }

        }











    }
}
