using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace 主程序202106
{
    public partial class 个税计算 : Form
    {
        public 个税计算()
        {
            InitializeComponent();
        }

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

                                       cell.SetCellType(CellType.String); //Excel导入异常Cannot get a text value from a numeric cell解决

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
                                        cell = row.GetCell(j);
                                        if (cell == null)
                                        {
                                            dataRow[j] = "";
                                        }
                                        else
                                        {
                                            //CellType(Unknown = -1,Numeric = 0,String = 1,Formula = 2,Blank = 3,Boolean = 4,Error = 5,)  
                                            switch (cell.CellType)
                                            {
                                                case CellType.Blank:
                                                    dataRow[j] = "";
                                                    break;
                                                case CellType.Numeric:
                                                    short format = cell.CellStyle.DataFormat;
                                                    //对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理  
                                                    if (format == 14 || format == 31 || format == 57 || format == 58)
                                                        dataRow[j] = cell.DateCellValue;
                                                    else
                                                        dataRow[j] = cell.NumericCellValue;
                                                    break;
                                                case CellType.String:
                                                    dataRow[j] = cell.StringCellValue;
                                                    break;
                                            }
                                        }
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
                MessageBox.Show(ex.ToString());
                if (fs != null)
                {
                    fs.Close();
                }
                return null;
            }
        }

        #endregion

        #region 读取datatable到ListView
        public static void ShowDataInListView(DataTable dt, ListView lst)
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
            for (i = 0; i < ColCount; i++)
            {
                lst.Columns.Add(dt.Columns[i].Caption.Trim(), lst.Width / ColCount, System.Windows.Forms.HorizontalAlignment.Left);
            }

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
        DataTable dt;
      

        public void run()
        {
           
            try
            {
                for (int i = 0; i <listView1.Items.Count; i++)
                {
                    double total = 0;
                    double yinashuie = 0;
                    double shui = 0;
                    for (int j = 2; j < Convert.ToInt32(textBox2.Text)+2; j++)
                    {
                        if (listView1.Items[i].SubItems[j].Text.Trim() != "")
                        {
                        
                            total = total + Convert.ToDouble(listView1.Items[i].SubItems[j].Text);
                            listView1.Items[i].SubItems[14].Text = total.ToString();

                            int month = j - 1;
                           double  zongnashuie =  total - (5000 * month);

                            if (zongnashuie > yinashuie)
                            {

                                double chae = zongnashuie - yinashuie;
                                //if (chae <= 3000 && chae > 0)
                                //{
                                //    shui = shui+ chae * 0.03;

                                //}
                                //else if (3000 < chae && chae <= 12000)
                                //{
                                //    shui = shui + (chae - 210 * month) * 0.1;

                                //}
                                shui = shui + chae * 0.03;


                                yinashuie = zongnashuie;
                            }
                            else
                            {

                            }
                            //MessageBox.Show(zongnashuie.ToString());
                            //MessageBox.Show(yinashuie.ToString());
                            //MessageBox.Show(shui.ToString());
                        }
                    }

                    listView1.Items[i].SubItems[15].Text = shui.ToString();

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }


        public void run1()
        {
            List<string> lists = new List<string>();
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    lists.Add(dr[0].ToString());
                }
                MessageBox.Show(dt.Rows.Count.ToString());
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    if (!lists.Contains(dr[1].ToString()))
                    {
                       // textBox3.Text += dr[1].ToString() + "\r\n";
                    }
                }

               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
           // openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox1.Text = openFileDialog1.FileName;
                dt = ExcelToDataTable(textBox1.Text, true);
                ShowDataInListView(dt, listView1); ;
            }
        }
        Thread thread;
        private void button2_Click(object sender, EventArgs e)
        {

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
