using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Spire.Xls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 图书管理
{
    class function
    {

        string path = System.Environment.CurrentDirectory + "\\bookdata.db"; //获取当前程序运行文件夹
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

        /// <summary>
        /// 查询字段
        /// </summary>
        public string chaxunvalue(string sql)
        {
            try
            {
                string value = "";
               
                using (SQLiteConnection con = new SQLiteConnection("Data Source=" + path))
                {
                    con.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = string.Format(sql);
                        using (SQLiteDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (dr.Read())
                            {
                                value = dr["body"].ToString();

                            }
                        }
                    }
                    con.Close();
                }

                return value;

            }
            catch (SQLiteException ex)
            {
               
                return ex.ToString();


            }

        }


        /// <summary>
        /// 获取供货商
        /// </summary>
        public ArrayList getsupplyers()
        {
            try
            {
                ArrayList lists=new ArrayList();
                string sql = "select supplyer from datas";
                using (SQLiteConnection con = new SQLiteConnection("Data Source=" + path))
                {
                    con.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = string.Format(sql);
                        using (SQLiteDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            while (dr.Read())
                            {
                                string supplyer = dr["supplyer"].ToString();
                                if (!lists.Contains(supplyer))
                                {
                                    lists.Add(supplyer);
                                }
                               

                            }
                        }
                    }
                    con.Close();
                }

                return lists;

            }
            catch (SQLiteException ex)
            {

                return null;


            }

        }

        /// <summary>
        /// 插入数据库
        /// </summary>
        public bool insertdata(string supplyer, string isbn, string name, string cbs, string price, string kucun, string kuwei, string zhekou)
        {
            try
            {
                string sql = "INSERT INTO datas(supplyer,isbn,name,cbs,price,kucun,kuwei,zhekou)VALUES('" + supplyer + "'," +
                    "'" + isbn + "'," +
                    "'" + name + "'," +
                    "'" + cbs + "'," +
                    "'" + price + "'," +
                    "'" + kucun + "'," +
                    "'" + kuwei + "'," +
                    "'" + zhekou + "')";
               
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



        #region NPOI读取表格插入数据库大数据高速
        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public string ExcelToDataTable(string fileName)
        {
          
            string v_supplyer = Path.GetFileNameWithoutExtension(fileName);
            string sheetName = "Sheet1";
            IWorkbook workbook = null;
            ISheet sheet = null;
           
            int startRow = 0;
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                if(fs==null)
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


                
                int isbn = 99;
                int name = 99;
                int cbs = 99;
                int price = 99;
                int kucun = 99;
                int kuwei = 99;
                int zhekou = 99;





                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数                                       
                    int rowCount = sheet.LastRowNum;//最后一列的标号

                    for (int i = startRow; i <= rowCount; ++i)
                    {

                        IRow row = sheet.GetRow(i);

                        if (row == null) continue; //没有数据的行默认是null　　　　　　　
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null)
                            {
                                string value = row.GetCell(j).ToString();
                                if (value.Contains("条码") || value.Contains("isbn") || value.Contains("条形码"))
                                {
                                    isbn = j;
                                }
                                else if (value.Contains("书名"))
                                {
                                    name = j;
                                }
                                else if (value.Contains("出版社")|| value.Contains("版别"))
                                {
                                    cbs = j;
                                }
                                else if (value.Contains("定价") || value.Contains("价格"))
                                {
                                    price = j;
                                }
                                else if (value.Contains("库存")|| value.Contains("数量"))
                                {
                                    kucun = j;
                                }
                                else if (value.Contains("库位"))
                                {
                                    kuwei= j;
                                }
                                else if (value.Contains("折扣"))
                                {
                                    zhekou = j;
                                }


                            }

                                 
                        }
                        int panduan = isbn + name + cbs;
                        if (panduan != 0)
                        {
                            startRow = i + 1;
                            //MessageBox.Show(startRow.ToString());
                            //MessageBox.Show(isbn.ToString());
                            break;
                        }
                    }


                    using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + path+"; BinaryGUID=False"))
                    {


                        using (SQLiteCommand insertRngCmd = (SQLiteCommand)conn.CreateCommand())
                        {
                            insertRngCmd.CommandText = @"INSERT INTO datas(supplyer, isbn, name, cbs, price, kucun, kuwei, zhekou) VALUES (@supplyer, @isbn, @name, @cbs, @price, @kucun, @kuwei, @zhekou)";
                            conn.Open();
                            var transaction = conn.BeginTransaction();

                           
                      

                    for (int i = startRow; i <= rowCount; ++i)
                    {
                      
                        IRow row = sheet.GetRow(i);

                        if (row == null) continue; //没有数据的行默认是null
                                try
                                {
                                    string v_isbn = isbn == 99 ? "" : row.GetCell(isbn).ToString();
                                    string v_name = name == 99 ? "" : row.GetCell(name).ToString();
                                    string v_cbs = cbs == 99 ? "" : row.GetCell(cbs).ToString();
                                    string v_price = price == 99 ? "" : row.GetCell(price).ToString();
                                    string v_kucun = kucun == 99 ? "" : row.GetCell(kucun).ToString();
                                    string v_kuwei = kuwei == 99 ? "" : row.GetCell(kuwei).ToString();
                                    string v_zhekou = zhekou == 99 ? "" : row.GetCell(zhekou).ToString();

                                    insertRngCmd.Parameters.AddWithValue("@supplyer", v_supplyer);
                                    insertRngCmd.Parameters.AddWithValue("@isbn", v_isbn);
                                    insertRngCmd.Parameters.AddWithValue("@name", v_name);
                                    insertRngCmd.Parameters.AddWithValue("@cbs", v_cbs);
                                    insertRngCmd.Parameters.AddWithValue("@price", v_price);
                                    insertRngCmd.Parameters.AddWithValue("@kucun", v_kucun);
                                    insertRngCmd.Parameters.AddWithValue("@kuwei", v_kuwei);
                                    insertRngCmd.Parameters.AddWithValue("@zhekou", v_zhekou);

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
              
                return "上传失败，请取消表格数据的超链接,重新上传！"+ ex.Message;
            }
        }


        #endregion

        #region 通过JET.OLEDB读取excel表格到数据库
        //根据excle的路径把第一个sheel中的内容放入datatable
        public  string ReadExcelToTable_OLEDB(string fileName)//excel存放的路径
        {
            try
            {
                string v_supplyer = Path.GetFileNameWithoutExtension(fileName);
                DataTable dt = null;

                //连接字符串
                //string connstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1';"; // Office 07及以上版本 不能出现多余的空格 而且分号注意
                string connstring = "Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1';"; //Office 07以下版本 
                using (OleDbConnection conn = new OleDbConnection(connstring))
                {
                    conn.Open();
                    DataTable sheetsName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" }); //得到所有sheet的名字
                    string firstSheetName = sheetsName.Rows[0][2].ToString(); //得到第一个sheet的名字
                   
                    string sql = string.Format("SELECT * FROM [{0}]", firstSheetName); //查询字符串                    //string sql = string.Format("SELECT * FROM [{0}] WHERE [日期] is not null", firstSheetName); //查询字符串
                    OleDbDataAdapter ada = new OleDbDataAdapter(sql, connstring);
                    DataSet set = new DataSet();
                    ada.Fill(set);
                    dt = set.Tables[0];
                    conn.Close();
                    conn.Dispose();
                }

                int isbn = 99;
                int name = 99;
                int cbs = 99;
                int price = 99;
                int kucun = 99;
                int kuwei = 99;
                int zhekou = 99;

                int startRow = 0;
                for (int i = 0; i <= dt.Rows.Count; ++i)
                {

                   　　　
                    for (int j = 0; j <dt.Columns.Count; ++j)
                    {
                        if (dt.Rows[i][j] != null)
                        {
                            string value = dt.Rows[i][j].ToString();
                            if (value.Contains("条码") || value.Contains("isbn") || value.Contains("条形码"))
                            {
                                isbn = j;
                            }
                            else if (value.Contains("书名"))
                            {
                                name = j;
                            }
                            else if (value.Contains("出版社") || value.Contains("版别"))
                            {
                                cbs = j;
                            }
                            else if (value.Contains("定价") || value.Contains("价格"))
                            {
                                price = j;
                            }
                            else if (value.Contains("库存") || value.Contains("数量"))
                            {
                                kucun = j;
                            }
                            else if (value.Contains("库位"))
                            {
                                kuwei = j;
                            }
                            else if (value.Contains("折扣"))
                            {
                                zhekou = j;
                            }


                        }

                    }
                    int panduan = isbn + name + cbs;
                    if (panduan != 0)
                    {
                        startRow = i + 1;
                        //MessageBox.Show(startRow.ToString());
                        //MessageBox.Show(isbn.ToString());
                        break;
                    }
                }


                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + path + "; BinaryGUID=False"))
                {


                    using (SQLiteCommand insertRngCmd = (SQLiteCommand)conn.CreateCommand())
                    {
                        insertRngCmd.CommandText = @"INSERT INTO datas(supplyer, isbn, name, cbs, price, kucun, kuwei, zhekou) VALUES (@supplyer, @isbn, @name, @cbs, @price, @kucun, @kuwei, @zhekou)";
                        conn.Open();
                        var transaction = conn.BeginTransaction();




                        for (int i = startRow; i <= dt.Rows.Count; ++i)
                        {

                            try
                            {
                                string v_isbn = isbn == 99 ? "" : dt.Rows[i][isbn].ToString();
                                string v_name = name == 99 ? "" : dt.Rows[i][name].ToString();
                                string v_cbs = cbs == 99 ? "" : dt.Rows[i][cbs].ToString();
                                string v_price = price == 99 ? "" : dt.Rows[i][price].ToString();
                                string v_kucun = kucun == 99 ? "" : dt.Rows[i][kucun].ToString();
                                string v_kuwei = kuwei == 99 ? "" : dt.Rows[i][kuwei].ToString();
                                string v_zhekou = zhekou == 99 ? "" : dt.Rows[i][zhekou].ToString();

                                insertRngCmd.Parameters.AddWithValue("@supplyer", v_supplyer);
                                insertRngCmd.Parameters.AddWithValue("@isbn", v_isbn);
                                insertRngCmd.Parameters.AddWithValue("@name", v_name);
                                insertRngCmd.Parameters.AddWithValue("@cbs", v_cbs);
                                insertRngCmd.Parameters.AddWithValue("@price", v_price);
                                insertRngCmd.Parameters.AddWithValue("@kucun", v_kucun);
                                insertRngCmd.Parameters.AddWithValue("@kuwei", v_kuwei);
                                insertRngCmd.Parameters.AddWithValue("@zhekou", v_zhekou);

                                insertRngCmd.ExecuteNonQuery();


                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                                continue;
                            }

                        }


                        transaction.Commit();
                    }
                }


                return "上传成功";
        }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return "上传失败";
            }

        }
        #endregion


        #region 读取datatable到ListView
        public void ShowDataInListView(DataTable dt, ListView lst)
        {
            lst.Clear();
            //   lst.View=System.Windows.Forms.View.Details;
            lst.AllowColumnReorder = true;//用户可以调整列的位置
            lst.GridLines = true;

            int RowCount, ColCount, i, j;
            DataRow dr = null;

            if (dt == null) return;
            RowCount = dt.Rows.Count;
            ColCount = dt.Columns.Count;
            //添加列标题名
            //for (i = 0; i < ColCount; i++)
            //{
            //    lst.Columns.Add(dt.Columns[i].Caption.Trim(), lst.Width / ColCount, System.Windows.Forms.HorizontalAlignment.Left);
            //}
            lst.Columns.Add("id", 100, System.Windows.Forms.HorizontalAlignment.Center);
            lst.Columns.Add("供应商", 100, System.Windows.Forms.HorizontalAlignment.Center);
            lst.Columns.Add("书号", 100, System.Windows.Forms.HorizontalAlignment.Center);
            lst.Columns.Add("书名", 100, System.Windows.Forms.HorizontalAlignment.Center);
            lst.Columns.Add("出版社", 100, System.Windows.Forms.HorizontalAlignment.Center);
            lst.Columns.Add("定价", 100, System.Windows.Forms.HorizontalAlignment.Center);
            lst.Columns.Add("库存", 100, System.Windows.Forms.HorizontalAlignment.Center);
            lst.Columns.Add("库位", 100, System.Windows.Forms.HorizontalAlignment.Center);
            lst.Columns.Add("折扣", 100, System.Windows.Forms.HorizontalAlignment.Center);
            lst.Columns.Add("定数", 100, System.Windows.Forms.HorizontalAlignment.Center);
           

            if (RowCount == 0) return;
            for (i = 0; i < RowCount; i++)
            {
                dr = dt.Rows[i];
                lst.Items.Add(dr[0].ToString().Trim());
                for (j = 1; j < ColCount; j++)
                {
                    lst.Columns[j].Width = -2;
                    lst.Items[i].SubItems.Add((string)dr[j].ToString().Trim());
                }
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



        #region xls转存xlsx 用于无法读取xls文件
        
        public string xlstoxlsx(string filename)
        {
            try
            {
                Workbook workbook = new Workbook();
                if (workbook != null)
                {
                    workbook.LoadFromFile(filename);
                    workbook.SaveToFile(filename.Replace("xls", "xlsx"), ExcelVersion.Version2013);
                }
                return filename.Replace("xls", "xlsx");
            }
            catch (System.NullReferenceException ex)
            {
              
              
                return filename;
            }
            finally

            {

               

            }

        }


        #endregion
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

        public void ExportCSV(DataTable table)
        {
            try
            {
                string path = "";
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "csv|*.csv";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    path = sfd.FileName;
                }

               table.Columns["supplyer"].ColumnName = "供应商";
               table.Columns["name"].ColumnName = "书名";
               table.Columns["cbs"].ColumnName = "出版社";
               table.Columns["isbn"].ColumnName = "书号";
               table.Columns["price"].ColumnName = "定价";
               table.Columns["kucun"].ColumnName = "库存";
               table.Columns["kuwei"].ColumnName = "库位";
               table.Columns["zhekou"].ColumnName = "折扣";
                table.Columns["dingshu"].ColumnName = "定数";
                StreamWriter writer;
                bool comma = false;
                int columns = table.Columns.Count;

                using (writer = new StreamWriter(path, false))
                {
                    foreach (DataColumn col in table.Columns)
                    {
                        if (!comma) comma = true;
                        else writer.Write(',');

                        writer.Write(col.ColumnName);
                    }
                    writer.WriteLine();

                    foreach (DataRow row in table.Rows)
                    {
                        comma = false;
                        for (int c = 0; c < columns; c++)
                        {
                            try
                            {
                                if (!comma) comma = true;
                                else writer.Write(',');

                                writer.Write(row[c].ToString().Replace("\"","").Replace(",", " "));
                            }
                            catch (Exception ex)
                            {

                                MessageBox.Show(ex.ToString());
                            }
                        }
                        writer.WriteLine();
                    }

                    MessageBox.Show("数据导出完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }
            catch (Exception)
            {

                ;
            }

        }

        #region datatabel分解成多个datatable
        /// <summary>
        /// 分解数据表遍历dataset获取datatable
        /// </summary>
        /// <param name="orgTable">需要分解的表</param>
        /// <param name="rowsNum">每个表包含的数据量</param>
        /// <returns></returns>
        public DataSet SplitDataTable(DataTable orgTable, int rowsNum)
        {
            //遍历获取每个datatable
            // for (int i = 0; i < dataset.Tables.Count; i++)
           // {
                //fc.ExportCSV(dataset.Tables[i]);
           // }
            var ds = new DataSet();
            if (rowsNum <= 0 || orgTable.Rows.Count <= 0)
            {
                ds.Tables.Add(orgTable);
                return ds;
            }
            var tableNum = Convert.ToInt32(Math.Ceiling(orgTable.Rows.Count * 1.0 / rowsNum));
            var remainder = orgTable.Rows.Count % rowsNum;
            if (tableNum == 1)
            {
                ds.Tables.Add(orgTable);
            }
            else
            {
                for (var i = 0; i < tableNum; i++)
                {
                    var table = orgTable.Clone();
                    int num;
                    if (i != tableNum - 1)
                        num = rowsNum;
                    else
                        num = remainder > 0 ? remainder : rowsNum;
                    for (var j = 0; j < num; j++)
                    {
                        var row = orgTable.Rows[i * rowsNum + j];
                        table.ImportRow(row);
                    }
                    ds.Tables.Add(table);
                }
            }
            return ds;
        }
        #endregion

    }
}
