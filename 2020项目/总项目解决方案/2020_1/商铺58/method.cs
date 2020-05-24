using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 商铺58
{
    public class method
    {
       





            #region 获取公网IP
            public static string GetIP()
            {
                using (var webClient = new WebClient())
                {
                    try
                    {
                        webClient.Credentials = CredentialCache.DefaultCredentials;
                        byte[] pageDate = webClient.DownloadData("http://pv.sohu.com/cityjson?ie=utf-8");
                        String ip = Encoding.UTF8.GetString(pageDate);
                        webClient.Dispose();

                        Match rebool = Regex.Match(ip, @"\d{2,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
                        return rebool.Value;
                    }
                    catch (Exception e)
                    {
                        return e.ToString();
                    }

                }
            }

            #endregion



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
                        dr[j] = lv.Items[i].SubItems[j].Text.Trim();
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

            #region NPOI导出表格默认时间为文件名
            public static int DataTableToExcelTime(DataTable data, string sheetName, bool isColumnWritten, string fileName)
            {
                int i = 0;
                int j = 0;
                int count = 0;
                ISheet sheet = null;
                IWorkbook workbook = null;
                FileStream fs = null;
                // bool disposed;
                //SaveFileDialog sfd = new SaveFileDialog();
                //sfd.Filter = "xlsx|*.xls|xlsx|*.xlsx";
                //sfd.Title = "Excel文件导出";
                //string fileName = "";

                //if (sfd.ShowDialog() == DialogResult.OK)
                //{
                //    fileName = sfd.FileName;
                //}
                //else
                //    return -1;

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

                    return 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                    return -1;
                }
            }

            #endregion

            #region  listview导出文本TXT
            public static void ListviewToTxt(ListView listview)
            {
                if (listview.Items.Count == 0)
                {
                    MessageBox.Show("列表为空!");
                }
                else
                {
                    List<string> list = new List<string>();
                    foreach (ListViewItem item in listview.Items)
                    {
                        for (int i = 0; i < item.SubItems.Count; i++)
                        {
                            list.Add(item.SubItems[i].Text + "，");
                        }
                    }
                    Thread thexp = new Thread(() => export(list)) { IsBackground = true };
                    thexp.Start();
                }
            }


            private static void export(List<string> list)
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "url_" + Guid.NewGuid().ToString() + ".txt";

                StringBuilder sb = new StringBuilder();
                foreach (string tel in list)
                {
                    sb.AppendLine(tel);
                }
                System.IO.File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
                MessageBox.Show("文件导出成功!文件地址:" + path);
            }



            #endregion

         

        
    }
}
