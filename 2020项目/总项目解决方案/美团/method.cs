using Microsoft.Win32;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 美团
{
    public class method
    {

        public enum IeVersion
        {
            IE7 = 7,
            IE8 = 8,
            IE9 = 9,
            IE10 = 10,
            IE11 = 11
        };

        /// <summary>  
        /// 修改注册表信息来兼容当前程序
        /// </summary>  
        public static void SetWebBrowserFeatures(IeVersion ieVersion)
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime) return;
            //获取程序及名称  
            string AppName = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

            //得到浏览器的模式的值  
            UInt32 ieMode = GeoEmulationModee((int)ieVersion);

            string featureControlRegKey = @"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\";
            //设置浏览器对应用程序（appName）以什么模式（ieMode）运行  

            Registry.SetValue(featureControlRegKey + "FEATURE_BROWSER_EMULATION", AppName, ieMode, RegistryValueKind.DWord);

            Registry.SetValue(featureControlRegKey + "FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", AppName, 1, RegistryValueKind.DWord);
            Registry.SetValue(featureControlRegKey + "FEATURE_AJAX_CONNECTIONEVENTS", AppName, 1, RegistryValueKind.DWord);
            Registry.SetValue(featureControlRegKey + "FEATURE_GPU_RENDERING", AppName, 1, RegistryValueKind.DWord);
            Registry.SetValue(featureControlRegKey + "FEATURE_WEBOC_DOCUMENT_ZOOM", AppName, 1, RegistryValueKind.DWord);
            Registry.SetValue(featureControlRegKey + "FEATURE_NINPUT_LEGACYMODE", AppName, 0, RegistryValueKind.DWord);
        }

        /// <summary>  
        /// 通过版本得到浏览器模式的值  
        /// </summary>  
        /// <param name="browserVersion"></param>  
        /// <returns></returns>  
        private static UInt32 GeoEmulationModee(int browserVersion)
        {
            UInt32 mode = 11000; // Internet Explorer 11. Webpages containing standards-based !DOCTYPE directives are displayed in IE11 Standards mode.   
            switch (browserVersion)
            {
                case 7:
                    mode = 7000; // Webpages containing standards-based !DOCTYPE directives are displayed in IE7 Standards mode.   
                    break;
                case 8:
                    mode = 8000; // Webpages containing standards-based !DOCTYPE directives are displayed in IE8 mode.   
                    break;
                case 9:
                    mode = 9000; // Internet Explorer 9. Webpages containing standards-based !DOCTYPE directives are displayed in IE9 mode.                      
                    break;
                case 10:
                    mode = 10000; // Internet Explorer 10.  
                    break;
                case 11:
                    mode = 11000; // Internet Explorer 11  
                    break;
            }
            return mode;
        }




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
