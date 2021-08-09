using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Eval;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace 主程序202104
{
    public partial class 数据处理excel : Form
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.inipath);
        }

        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
            return temp.ToString();
        }

        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public bool ExistINIFile()
        {
            return File.Exists(inipath);
        }


        public 数据处理excel()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
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
            }
            textBox1.Text = dialog.SelectedPath;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择所在文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] files = Directory.GetFiles(dialog.SelectedPath + @"\", "*.xlsx");
                foreach (string file in files)
                {
                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(file);
                    lv1.SubItems.Add("未开始");
                }
            }
        }

        bool xuanze = false;

        private void button7_Click(object sender, EventArgs e)
        {
            if (xuanze == false)
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    listView1.Items[i].Checked = true;
                }
                xuanze = true;
            }
            else
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    listView1.Items[i].Checked = false;
                }
                xuanze = false;
            }
        }
        /// <summary>
        /// 获取时间戳毫秒
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalMilliseconds);
            return a.ToString();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                System.Diagnostics.Process.Start(textBox1.Text);
            }
        }
        ArrayList lists = new ArrayList();


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
                                    for (int i = firstRow.FirstCellNum; i < cellCount-1; ++i)
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
                                    for (int j = row.FirstCellNum; j < cellCount-1; ++j)
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
                                               
                                                MessageBox.Show(e.ToString());
                                                continue;
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
            for (int i = 0; i < listView1.CheckedItems.Count; i++)
            {
                string filename = listView1.CheckedItems[i].SubItems[1].Text;
                if (!lists.Contains(filename))
                {
                    lists.Add(filename);
                    listView1.CheckedItems[i].SubItems[2].Text = "正在读取...";
                    try
                    {
                        DataTable dt = ExcelToDataTable(filename, true);
                        //MessageBox.Show(dt.Rows.Count.ToString());
                        if (checkBox1.Checked == true)
                        {
                            string txtname = textBox1.Text + "\\" +textBox2.Text+i+ ".txt";
                            FileStream fs1 = new FileStream(txtname, FileMode.Append, FileAccess.Write);//创建写入文件 
                            StreamWriter sw = new StreamWriter(fs1);
                            for (int j = 2; j < dt.Rows.Count; j++)
                            {
                                string value = "";
                                if (filename.Contains("高级"))
                                {
                                    
                                    value = dt.Rows[j][21].ToString().Replace(";", "\r\n").Trim();
                                  
                                }
                                else
                                {
                                    value = dt.Rows[j][14].ToString().Replace(";", "\r\n").Trim();
                                }


                                if (value != "" && value != "-")
                                {
                                    sw.WriteLine(value);
                                }
                            }
                            sw.Close();
                            fs1.Close();
                            sw.Dispose();

                            listView1.CheckedItems[i].SubItems[2].Text = "已完成";

                        }
                        if (checkBox2.Checked == true)
                        {
                            string txtname = textBox1.Text + "\\"+ textBox2.Text+i + ".txt";
                            FileStream fs1 = new FileStream(txtname, FileMode.Append, FileAccess.Write);//创建写入文件 
                            StreamWriter sw = new StreamWriter(fs1);
                            for (int j = 2; j < dt.Rows.Count; j++)
                            {
                                string value = "";
                                if (filename.Contains("高级"))
                                {
                                    value = dt.Rows[j][24].ToString().Replace(";", "\r\n").Trim();
                                }
                                else
                                {
                                    value = dt.Rows[j][19].ToString().Replace(";", "\r\n").Trim();
                                }
                                if (value != "" && value !="-")
                                {
                                    sw.WriteLine(value);
                                }
                            }
                            sw.Close();
                            fs1.Close();
                            sw.Dispose();

                            listView1.CheckedItems[i].SubItems[2].Text = "已完成";
                        }

                    }
                    catch (Exception ex)
                    {

                        ex.ToString();
                    }
                }
            }
            
        }

       
        private void button3_Click(object sender, EventArgs e)
        {
            IniWriteValue("values", "path", textBox1.Text.Trim());

            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"Qpsly5V"))
            {

                return;
            }



            #endregion

            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;






        }

        private void 数据处理excel_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }

        private void 数据处理excel_Load(object sender, EventArgs e)
        {
            if (ExistINIFile())
            {
                textBox1.Text = IniReadValue("values", "path");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
