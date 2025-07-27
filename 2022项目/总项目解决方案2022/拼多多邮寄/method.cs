using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Eval;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 拼多多邮寄
{
    public class method
    {
        #region POST请求全参
        public static string PostUrl(string url, string postData, string COOKIE, string charset, string contentType, string refer)
        {
            string result;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("version:TYC-XCX-WX");

                //添加头部
                request.ContentType = contentType;
                request.ContentLength = (long)Encoding.UTF8.GetBytes(postData).Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.131 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
                request.Referer = refer;
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

        #region GET请求
        public static string GetUrl(string Url, string charset, string COOKIE)
        {

            string result;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                //request.Referer = Url;
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("version:TYC-XCX-WX");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
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
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        #endregion

        #region datatable转excel
        public static int DataTableToExcel(DataTable data, string sheetName, bool isColumnWritten)
        {
            IWorkbook workbook = null;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "xlsx|*.xls|xlsx|*.xlsx";
            sfd.Title = "Excel文件导出";
            bool flag = sfd.ShowDialog() == DialogResult.OK;
            int result;
            if (flag)
            {
                string fileName = sfd.FileName;
                FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                bool flag2 = fileName.IndexOf(".xlsx") > 0;
                if (flag2)
                {
                    workbook = new XSSFWorkbook();
                }
                else
                {
                    bool flag3 = fileName.IndexOf(".xls") > 0;
                    if (flag3)
                    {
                        workbook = new HSSFWorkbook();
                    }
                }
                try
                {
                    bool flag4 = workbook != null;
                    if (flag4)
                    {
                        ISheet sheet = workbook.CreateSheet(sheetName);
                        ICellStyle style = workbook.CreateCellStyle();
                        style.FillPattern = FillPattern.SolidForeground;
                        int count;
                        if (isColumnWritten)
                        {
                            IRow row = sheet.CreateRow(0);
                            for (int i = 0; i < data.Columns.Count; i++)
                            {
                                row.CreateCell(i).SetCellValue(data.Columns[i].ColumnName);
                            }
                            count = 1;
                        }
                        else
                        {
                            count = 0;
                        }
                        for (int j = 0; j < data.Rows.Count; j++)
                        {
                            IRow row2 = sheet.CreateRow(count);
                            for (int i = 0; i < data.Columns.Count; i++)
                            {
                                row2.CreateCell(i).SetCellValue(data.Rows[j][i].ToString());
                            }
                            count++;
                        }
                        workbook.Write(fs);
                        workbook.Close();
                        fs.Close();
                        Process[] Proc = Process.GetProcessesByName("");
                        MessageBox.Show("数据导出完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        result = 0;
                    }
                    else
                    {
                        result = -1;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                    result = -1;
                }
            }
            else
            {
                result = -1;
            }
            return result;
        }
        #endregion




        #region listview转datatable
        public static DataTable listViewToDataTable(ListView lv)
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Clear();
            for (int i = 0; i < lv.Columns.Count; i++)
            {
                dt.Columns.Add(lv.Columns[i].Text.Trim(), typeof(string));
            }
            for (int i = 0; i < lv.Items.Count; i++)
            {
                DataRow dr = dt.NewRow();
                int j = 0;
                while (j < lv.Columns.Count)
                {
                    try
                    {
                        dr[j] = lv.Items[i].SubItems[j].Text.Trim();
                    }
                    catch
                    {
                    }
                    IL_A7:
                    j++;
                    continue;
                    goto IL_A7;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        #endregion

        #region Excel转datatable
        public static DataTable ExcelToDataTable(string filePath, bool isColumnName)
        {
            DataTable dataTable = null;
            FileStream fs = null;
            DataRow dataRow = null;
            IWorkbook workbook = null;
            ISheet sheet = null;
            IRow row = null;
            int startRow = 0;
            DataTable result;
            try
            {
                FileStream fileStream;
                fs = (fileStream = File.OpenRead(filePath));
                try
                {
                    bool flag = filePath.IndexOf(".xlsx") > 0;
                    if (flag)
                    {
                        workbook = new XSSFWorkbook(fs);
                    }
                    else
                    {
                        bool flag2 = filePath.IndexOf(".xls") > 0;
                        if (flag2)
                        {
                            workbook = new HSSFWorkbook(fs);
                        }
                    }
                    bool flag3 = workbook != null;
                    if (flag3)
                    {
                        sheet = workbook.GetSheetAt(0);
                        dataTable = new DataTable();
                        bool flag4 = sheet != null;
                        if (flag4)
                        {
                            int rowCount = sheet.LastRowNum;
                            bool flag5 = rowCount > 0;
                            if (flag5)
                            {
                                IRow firstRow = sheet.GetRow(0);
                                int cellCount = (int)firstRow.LastCellNum;
                                if (isColumnName)
                                {
                                    startRow = 1;
                                    for (int i = (int)firstRow.FirstCellNum; i < cellCount; i++)
                                    {
                                        ICell cell = firstRow.GetCell(i);
                                        bool flag6 = cell != null;
                                        if (flag6)
                                        {
                                            bool flag7 = cell.StringCellValue != null;
                                            if (flag7)
                                            {
                                                DataColumn column = new DataColumn(cell.StringCellValue);
                                                dataTable.Columns.Add(column);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    for (int j = (int)firstRow.FirstCellNum; j < cellCount; j++)
                                    {
                                        DataColumn column = new DataColumn("column" + (j + 1).ToString());
                                        dataTable.Columns.Add(column);
                                    }
                                }
                                for (int k = startRow; k <= rowCount; k++)
                                {
                                    row = sheet.GetRow(k);
                                    bool flag8 = row == null;
                                    if (!flag8)
                                    {
                                        dataRow = dataTable.NewRow();
                                        int l = (int)row.FirstCellNum;
                                        while (l < cellCount)
                                        {
                                            ICell RCells = row.GetCell(l);
                                            bool flag9 = RCells != null;
                                            if (flag9)
                                            {
                                                try
                                                {
                                                    switch (RCells.CellType)
                                                    {
                                                        case CellType.Numeric:
                                                            {
                                                                bool flag10 = DateUtil.IsCellDateFormatted(RCells);
                                                                if (flag10)
                                                                {
                                                                    dataRow[l] = RCells.DateCellValue.ToString("yyyy/MM/dd HH:mm:ss");
                                                                }
                                                                else
                                                                {
                                                                    dataRow[l] = RCells.NumericCellValue;
                                                                }
                                                                goto IL_387;
                                                            }
                                                        case CellType.Formula:
                                                            switch (row.GetCell(l).CachedFormulaResultType)
                                                            {
                                                                case CellType.Numeric:
                                                                    dataRow[l] = Convert.ToString(row.GetCell(l).NumericCellValue);
                                                                    goto IL_351;
                                                                case CellType.String:
                                                                    {
                                                                        string strFORMULA = row.GetCell(l).StringCellValue;
                                                                        bool flag11 = strFORMULA != null && strFORMULA.Length > 0;
                                                                        if (flag11)
                                                                        {
                                                                            dataRow[l] = strFORMULA.ToString();
                                                                        }
                                                                        else
                                                                        {
                                                                            dataRow[l] = null;
                                                                        }
                                                                        goto IL_351;
                                                                    }
                                                                case CellType.Boolean:
                                                                    dataRow[l] = Convert.ToString(row.GetCell(l).BooleanCellValue);
                                                                    goto IL_351;
                                                                case CellType.Error:
                                                                    dataRow[l] = ErrorEval.GetText((int)row.GetCell(l).ErrorCellValue);
                                                                    goto IL_351;
                                                            }
                                                            dataRow[l] = "";
                                                            IL_351:
                                                            goto IL_387;
                                                        case CellType.Blank:
                                                            goto IL_387;
                                                        case CellType.Boolean:
                                                            dataRow[l] = RCells.BooleanCellValue.ToString();
                                                            goto IL_387;
                                                        case CellType.Error:
                                                            dataRow[l] = ErrorEval.GetText((int)row.GetCell(l).ErrorCellValue);
                                                            goto IL_387;
                                                    }
                                                    dataRow[l] = RCells.StringCellValue.Trim();
                                                    IL_387:;
                                                }
                                                catch (Exception e)
                                                {
                                                }
                                            }
                                            else
                                            {
                                                dataRow[l] = "";
                                            }
                                            IL_3A3:
                                            l++;
                                            continue;
                                            goto IL_3A3;
                                        }
                                        dataTable.Rows.Add(dataRow);
                                    }
                                }
                            }
                        }
                    }
                }
                finally
                {
                    if (fileStream != null)
                    {
                        ((IDisposable)fileStream).Dispose();
                    }
                }
                result = dataTable;
            }
            catch (Exception ex)
            {
                bool flag12 = fs != null;
                if (flag12)
                {
                    fs.Close();
                }
                result = null;
            }
            return result;
        }
        #endregion


        public static List<DataTable> SplitDataTable(DataTable originalTable, int rowsPerGroup)
        {
            List<DataTable> tableGroups = new List<DataTable>();
            int totalRows = originalTable.Rows.Count;

            if (totalRows <= 0)
            {
                return tableGroups;
            }

            int groupCount = (int)Math.Ceiling((double)totalRows / rowsPerGroup);

            for (int groupIndex = 0; groupIndex < groupCount; groupIndex++)
            {
                DataTable groupTable = originalTable.Clone(); // 仅复制结构

                int startIndex = groupIndex * rowsPerGroup;
                int endIndex = Math.Min(startIndex + rowsPerGroup, totalRows);

                for (int i = startIndex; i < endIndex; i++)
                {
                    groupTable.ImportRow(originalTable.Rows[i]);
                }

                tableGroups.Add(groupTable);
            }

            return tableGroups;
        }




    }
}
