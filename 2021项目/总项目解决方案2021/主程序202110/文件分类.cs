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
using NPOI.SS.Formula.Eval;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace 主程序202110
{
    public partial class 文件分类 : Form
    {
        public 文件分类()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择所在文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }

                textBox2.Text = dialog.SelectedPath;
            }
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
                        // sheet = workbook.GetSheetAt(1);//读取第一个sheet，当然也可以循环读取每个sheet  
                        sheet = workbook.GetSheet("目录"); //根据sheet名称
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

        public void run()
        {
            try
            {
                DataTable dt = ExcelToDataTable(textBox3.Text,true);
              
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                   string namevalue = dt.Rows[i][8].ToString().Trim();
                    string namevalue2 ="";
                    if (!namevalue.Contains("-"))
                    {
                        namevalue2 = dt.Rows[i + 1][8].ToString().Trim();
                    }
                    //倒数第一个
                    else
                    {
                        string odlnamevalue = namevalue;
                        namevalue = odlnamevalue.Split(new string[] { "-" }, StringSplitOptions.None)[0];
                        namevalue2 = odlnamevalue.Split(new string[] { "-" }, StringSplitOptions.None)[1];
                        if(namevalue2==namevalue)
                        {
                            namevalue2 = (Convert.ToInt32(namevalue) + 1).ToString();
                        }
                    }
                   

                    //倒数第二个
                    if(namevalue2.Contains("-"))
                    {
                      namevalue2 = namevalue2.Split(new string[] { "-" }, StringSplitOptions.None)[0];
                    }
                  

                    string dir = textBox2.Text + "/" + dt.Rows[i][9].ToString().Trim();
                    string newdic = textBox2.Text + "/" + dt.Rows[i][9].ToString().Trim() + "/" + dt.Rows[i][10].ToString().Trim();
                    if (!Directory.Exists(newdic))
                    {
                        Directory.CreateDirectory(newdic); //创建文件夹
                    }
                    DirectoryInfo d = new DirectoryInfo(dir);
                    FileInfo[] files = d.GetFiles();//文件
                    foreach (FileInfo f in files)
                    {
                        if (namevalue2!="")
                        {
                            if (Convert.ToInt32(Path.GetFileNameWithoutExtension(f.FullName)) >= Convert.ToInt32(namevalue)  && Convert.ToInt32(Path.GetFileNameWithoutExtension(f.FullName)) < Convert.ToInt32(namevalue2))
                            {
                                File.Copy(f.FullName, newdic + "/" + Path.GetFileName(f.FullName));
                                File.Delete(f.FullName);
                                textBox1.Text += "分类文件："+ Path.GetFileNameWithoutExtension(f.FullName)+"成功\r\n";
                            }
                        }
                        
                    }

                    //File.Copy(f.FullName, newdic + "/" + Path.GetFileName(f.FullName));
                    //File.Delete(f.FullName);

                }

                //  method.ShowDataInListView(dt,listView1);

                //目录封面等

                DirectoryInfo d1 = new DirectoryInfo(textBox2.Text);     
                DirectoryInfo[] directs = d1.GetDirectories();//文件夹
               

                foreach (DirectoryInfo dd in directs)
                {
                    string newdic = dd.FullName + "/"+Path.GetFileName(dd.FullName)+"-0000/";
                    if (!Directory.Exists(newdic))
                    {
                        Directory.CreateDirectory(newdic); //创建文件夹
                    }

                    FileInfo[] files = dd.GetFiles();//文件
                    foreach (FileInfo f in files)
                    {
                        File.Copy(f.FullName, newdic + "/" + Path.GetFileName(f.FullName));
                        File.Delete(f.FullName);

                    }
                }

                MessageBox.Show("完成");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
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

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            //openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox3.Text = openFileDialog1.FileName;

              

            }
        }

        private void 文件分类_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
