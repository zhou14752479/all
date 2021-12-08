using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 股票数据管理
{
    class function
    {

        string path = System.Environment.CurrentDirectory + "\\data.db"; //获取当前程序运行文件夹


        public DataTable chaxun(string isbn)
        {
            try
            {

                string sql = "select * from datas where isbn = '" + isbn.Trim() + "'";
                DataTable dt = chaxundata(sql);
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;

            }
        }

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            string html = "";
            string COOKIE = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 100000;

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

        #region NPOI读取表格导入DataTable
        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public static DataTable ExcelToDataTable(string fileName, bool isFirstRowColumn)
        {
            string sheetName = "Sheet1";
            IWorkbook workbook = null;
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);

                if (sheetName != null)
                {
                    // sheet = workbook.GetSheet(sheetName); //根据sheet名称
                    sheet = workbook.GetSheetAt(0);//根据sheet序号
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }


        #endregion

        #region datagridview转datatable
        public DataTable DgvToTable(DataGridView dgv)
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
            //sfd.Filter = "xlsx|*.xls|xlsx|*.xlsx";
            sfd.Filter = "xlsx|*.xlsx";
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

        /// <summary>
        /// 执行SQL
        /// </summary>
        public bool SQL(string sql)
        {
            try
            {

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path);
                mycon.Open();

                SQLiteCommand cmd = new SQLiteCommand(sql, mycon);

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


        #region NPOI读取表格插入数据库大数据高速【插入昨日数据】
        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public string ExcelToData(string fileName)
        {

            string v_supplyer = Path.GetFileNameWithoutExtension(fileName);
            string sheetName = "Sheet1";
            IWorkbook workbook = null;
            ISheet sheet = null;

            int startRow = 1; //不包含标题
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                if (fs == null)
                {
                    MessageBox.Show("");
                }
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);

                if (sheetName != null)
                {
                    // sheet = workbook.GetSheet(sheetName); //根据sheet名称
                    sheet = workbook.GetSheetAt(0);//根据sheet序号
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }






                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数        
                    if (cellCount < 5)
                    {

                        cellCount = sheet.GetRow(5).LastCellNum;
                    }

                    // MessageBox.Show(cellCount.ToString());
                    int rowCount = sheet.LastRowNum;//最后一列的标号

                 

                    using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + path + "; BinaryGUID=False"))
                    {


                        using (SQLiteCommand insertRngCmd = (SQLiteCommand)conn.CreateCommand())
                        {
                            insertRngCmd.CommandText = @"INSERT INTO datas(code,name, a1, a2, a3, a4, a5,a6) VALUES (@code, @name, @a1, @a2, @a3, @a4, @a5, @a6)";
                            conn.Open();
                            var transaction = conn.BeginTransaction();




                            for (int i = startRow; i <= rowCount; ++i)
                            {

                                IRow row = sheet.GetRow(i);

                                if (row == null) continue; //没有数据的行默认是null
                                try
                                {
    
                                    insertRngCmd.Parameters.AddWithValue("@code", row.GetCell(1).ToString());
                                    insertRngCmd.Parameters.AddWithValue("@name", row.GetCell(2).ToString());
                                    insertRngCmd.Parameters.AddWithValue("@a1", row.GetCell(3).ToString());
                                    insertRngCmd.Parameters.AddWithValue("@a2", row.GetCell(4).ToString());
                                    insertRngCmd.Parameters.AddWithValue("@a3", row.GetCell(5).ToString());
                                    insertRngCmd.Parameters.AddWithValue("@a4", row.GetCell(6).ToString());
                                    insertRngCmd.Parameters.AddWithValue("@a5", row.GetCell(7).ToString());
                                    insertRngCmd.Parameters.AddWithValue("@a6", row.GetCell(8).ToString());

                                    insertRngCmd.ExecuteNonQuery();


                                }
                                catch (Exception)
                                {

                                    continue;
                                }

                            }



                            transaction.Commit();
                        }
                    }


                }

                return "上传成功";
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.ToString());
                return "上传失败，请取消表格数据的超链接,重新上传！" + ex.Message;
            }
        }


        #endregion

        /// <summary>
        /// 插入数据库
        /// </summary>
        public bool INSERTdata(string code,string name, string a1, string a2, string a3, string a4, string a5, string a6)
        {
            try
            {
                string sql = "INSERT INTO datas(a1,a2,a3,a4,a5,a6,code,name)VALUES('" + a1+ "'," +
                    "'" + a2 + "'," +
                    "'" + a3 + "'," +
                    "'" +a4 + "'," +
                    "'" + a5 + "'," +
                    "'" + a6 + "'," +
                    "'" + code + "'," +
                    "'" + name + "')";

            
                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path);
                mycon.Open();

                SQLiteCommand cmd = new SQLiteCommand(sql, mycon);

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
        /// <summary>
        /// 更新数据库
        /// </summary>
        public bool UPDATEdata(string code,string b1, string b2, string b3, string b4, string b5, string b6)
        {
            try
            {
                string sql = "UPDATE datas set b1='" + b1 + "',b2='" + b2 + "',b3='" + b3 + "',b4='" + b4 + "',b5='" + b5 + "',b6='" + b6 + "' where code='" + code+ "'";

              
                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path);
                mycon.Open();

                SQLiteCommand cmd = new SQLiteCommand(sql, mycon);

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

        /// <summary>
        /// 查询数据库
        /// </summary>
        public DataTable chaxundata(string sql)
        {
            try
            {


                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path);
                mycon.Open();

                SQLiteDataAdapter mAdapter = new SQLiteDataAdapter(sql, mycon);
                DataTable dt = new DataTable();
                mAdapter.Fill(dt);
                mycon.Close();
                return dt;

            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.ToString());
                return null;


            }

        }
    }
}
