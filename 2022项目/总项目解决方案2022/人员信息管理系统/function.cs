using MySql.Data.MySqlClient;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Eval;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 人员信息管理系统
{
    class function
    {

        public static string bumenid="1";
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
        public static string conn = "Host =121.89.199.37;Database=renyuan;Username=root;Password=Aa123456.";
    
        #region 时间戳
        /// <summary>
        /// 获取时间戳毫秒
        /// </summary>
        /// <returns></returns>
        public long GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalMilliseconds);
            return a;
        }
        #endregion

        #region datagridview转datatable
        public static DataTable DgvToTable(DataGridView dgv)
        {
            DataTable dt = new DataTable();

            // 列强制转换
            for (int count = 0; count < dgv.Columns.Count; count++)
            {
                DataColumn dc = new DataColumn(dgv.Columns[count].HeaderText.ToString());
                dt.Columns.Add(dc);
            }

            // 循环行
            for (int count = 0; count < dgv.Rows.Count; count++)
            {
                DataRow dr = dt.NewRow();
                for (int countsub = 0; countsub < dgv.Columns.Count; countsub++)
                {
                    dr[countsub] = Convert.ToString(dgv.Rows[count].Cells[countsub].Value);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        #endregion

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
        #region 执行SQL
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

                return false;
            }

        }
        #endregion


        string path = AppDomain.CurrentDomain.BaseDirectory +"//data//";

        #region 读取txt
        public string readtxt()
        {
            StreamReader sr = new StreamReader(path + "renyuan.txt", EncodingType.GetTxtType(path + "renyuan.txt"));
            //一次性读取完 
            string texts = sr.ReadToEnd();

            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存
            return texts;
        }
        #endregion

        #region 写入txt

        public void xieru(string value, FileMode mode)
        {

            FileStream fs1 = new FileStream(path + "renyuan.txt", mode, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
            sw.WriteLine(value);
            sw.Close();
            fs1.Close();
            sw.Dispose();
        }
        #endregion

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

        
    }
}
