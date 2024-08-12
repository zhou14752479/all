using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Management;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Eval;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace myDLL
{
	// Token: 0x02000002 RID: 2
	public class method
	{
		
		[DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref uint pcchCookieData, int dwFlags, IntPtr lpReserved);

		[DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int InternetSetCookieEx(string lpszURL, string lpszCookieName, string lpszCookieData, int dwFlags, IntPtr dwReserved);

	
		private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}

        

        #region GET请求
        public static string GetUrl(string Url, string charset)
		{
			string COOKIE = "";
			string result;
			try
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
				request.Proxy = null;
				request.AllowAutoRedirect = true;
				request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
				request.Referer = Url;
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

      
		#region GET请求带COOKIE
		public static string GetUrlWithCookie(string Url, string COOKIE, string charset)
		{
			string result;
			try
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
				request.AllowAutoRedirect = true;
				request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36";
				request.Referer = Url;
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
			catch (WebException ex)
			{
				//result = ex.ToString();
				//400错误也返回内容
				using (var reader = new StreamReader(ex.Response.GetResponseStream()))
				{
					result = reader.ReadToEnd();
				}
			}
			return result;
		}
#endregion

		#region GET请求带IP
		public static string GetUrlwithIP(string Url, string ip, string COOKIE, string charset)
		{
			string result;
			try
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
				request.AllowAutoRedirect = true;
				request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36";
				WebProxy proxy = new WebProxy(ip);
				request.Proxy = proxy;
				request.Referer = Url;
				request.Headers.Add("Cookie", COOKIE);
				request.Headers.Add("Accept-Encoding", "gzip");
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
			catch (WebException ex)
			{
				//result = ex.ToString();
				//400错误也返回内容
				using (var reader = new StreamReader(ex.Response.GetResponseStream()))
				{
					result = reader.ReadToEnd();
				}
			}
			return result;
		}
        #endregion

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

		

		#region POST默认请求
		public static string PostUrlDefault(string url, string postData, string COOKIE)
		{
			string result;
			try
			{
				string charset = "utf-8";
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.Method = "Post";
				request.Proxy = null;
				//WebHeaderCollection headers = request.Headers;
				//headers.Add("version:TYC-XCX-WX");
				request.ContentType = "application/x-www-form-urlencoded";
				// request.ContentType = "application/json";
				request.ContentLength = (long)Encoding.UTF8.GetBytes(postData).Length;
				request.Headers.Add("Accept-Encoding", "gzip");
				request.AllowAutoRedirect = false;
				request.KeepAlive = true;
				request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
				request.Headers.Add("Cookie", COOKIE);
				request.Referer = url;
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
				//result = ex.ToString();
				//400错误也返回内容
				using (var reader = new StreamReader(ex.Response.GetResponseStream()))
				{
					result = reader.ReadToEnd();
				}
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

		#region 获取iE的cookie
		public static string GetCookies(string url)
		{
			uint datasize = 256U;
			StringBuilder cookieData = new StringBuilder((int)datasize);
			bool flag = !method.InternetGetCookieEx(url, null, cookieData, ref datasize, 8192, IntPtr.Zero);
			if (flag)
			{
				bool flag2 = datasize < 0U;
				if (flag2)
				{
					return null;
				}
				cookieData = new StringBuilder((int)datasize);
				bool flag3 = !method.InternetGetCookieEx(url, null, cookieData, ref datasize, 8192, IntPtr.Zero);
				if (flag3)
				{
					return null;
				}
			}
			return cookieData.ToString();
		}

		#endregion

		#region 获取set-cookie
		public static string getSetCookie(string url)
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.Timeout = 10000;
			request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
			request.AllowAutoRedirect = false;
			HttpWebResponse response = request.GetResponse() as HttpWebResponse;
			return response.GetResponseHeader("Set-Cookie");
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

		#region 在ListView展示数据
		public static void ShowDataInListView(DataTable dt, ListView lst)
		{
			lst.Clear();
			lst.AllowColumnReorder = true;
			lst.GridLines = true;
			bool flag = dt == null;
			if (!flag)
			{
				int RowCount = dt.Rows.Count;
				int ColCount = dt.Columns.Count;
				for (int i = 0; i < ColCount; i++)
				{
					lst.Columns.Add(dt.Columns[i].Caption.Trim(), lst.Width / ColCount, System.Windows.Forms.HorizontalAlignment.Left);
				}
				bool flag2 = RowCount == 0;
				if (!flag2)
				{
					for (int i = 0; i < RowCount; i++)
					{
						DataRow dr = dt.Rows[i];
						lst.Items.Add(dr[0].ToString().Trim());
						for (int j = 1; j < ColCount; j++)
						{
							lst.Columns[j].Width = -2;
							lst.Items[i].SubItems.Add(dr[j].ToString().Trim());
						}
					}
				}
			}
		}
		#endregion

		#region  DataGridView转datatable
		public static DataTable DgvToTable(DataGridView dgv)
		{
			DataTable dt = new DataTable();
			for (int count = 0; count < dgv.Columns.Count; count++)
			{
				DataColumn dc = new DataColumn(dgv.Columns[count].HeaderText.ToString());
				dt.Columns.Add(dc);
			}
			for (int count2 = 0; count2 < dgv.Rows.Count; count2++)
			{
				DataRow dr = dt.NewRow();
				for (int countsub = 0; countsub < dgv.Columns.Count; countsub++)
				{
					dr[countsub] = Convert.ToString(dgv.Rows[count2].Cells[countsub].Value);
				}
				dt.Rows.Add(dr);
			}
			return dt;
		}
		#endregion

		#region  Unicode转字符串
		public static string Unicode2String(string source)
		{
			return new Regex("\\\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(source, (Match x) => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)).ToString());
		}
		#endregion

		#region  MD5加密
		public static string GetMD5(string txt)
		{
			string result;
			using (MD5 mi = MD5.Create())
			{
				byte[] buffer = Encoding.Default.GetBytes(txt);
				byte[] newBuffer = mi.ComputeHash(buffer);
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < newBuffer.Length; i++)
				{
					sb.Append(newBuffer[i].ToString("x2"));
				}
				result = sb.ToString();
			}
			return result;
		}
		#endregion

		#region  获取mac地址
		public static string GetMacAddress()
		{
			string result;
			try
			{
				string strMac = string.Empty;
				ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
				ManagementObjectCollection moc = mc.GetInstances();
				foreach (ManagementBaseObject managementBaseObject in moc)
				{
					ManagementObject mo = (ManagementObject)managementBaseObject;
					bool flag = (bool)mo["IPEnabled"];
					if (flag)
					{
						strMac = mo["MacAddress"].ToString();
					}
				}
				result = strMac;
			}
			catch
			{
				result = "unknown";
			}
			return result;
		}
		#endregion

		#region  获取时间戳
		public static string GetTimeStamp()
		{
			return Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString();
		}
		#endregion

		#region  时间戳转时间
		public static DateTime ConvertStringToDateTime(string timeStamp)
		{
			return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(Convert.ToDouble(timeStamp));
		}
		#endregion

		#region  ie浏览器设置
		public static void SetFeatures(uint ieMode)
		{
			bool flag = LicenseManager.UsageMode > LicenseUsageMode.Runtime;
			if (flag)
			{
				throw new ApplicationException();
			}
			string appName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
			string featureControlRegKey = "HKEY_CURRENT_USER\\Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\";
			Registry.SetValue(featureControlRegKey + "FEATURE_BROWSER_EMULATION", appName, ieMode, RegistryValueKind.DWord);
			Registry.SetValue(featureControlRegKey + "FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", appName, 1, RegistryValueKind.DWord);
		}
		#endregion

		#region  DataTable导出表格带文件名
		public static int DataTableToExcelName(DataTable data, string fileName, bool isColumnWritten)
		{
			IWorkbook workbook = null;
			FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			bool flag = fileName.IndexOf(".xlsx") > 0;
			if (flag)
			{
				workbook = new XSSFWorkbook();
			}
			else
			{
				bool flag2 = fileName.IndexOf(".xls") > 0;
				if (flag2)
				{
					workbook = new HSSFWorkbook();
				}
			}
			int result;
			try
			{
				bool flag3 = workbook != null;
				if (flag3)
				{
					ISheet sheet = workbook.CreateSheet("sheet1");
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
			return result;
		}	
		#endregion

		#region  DataTable导出表格
		public static int DataTableToExcelTime(DataTable data, bool isColumnWritten)
		{
			IWorkbook workbook = null;
			string fileName = DateTime.Now.ToString("yyyy-MM-dd-HH") + ".xlsx";
			FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			bool flag = fileName.IndexOf(".xlsx") > 0;
			if (flag)
			{
				workbook = new XSSFWorkbook();
			}
			else
			{
				bool flag2 = fileName.IndexOf(".xls") > 0;
				if (flag2)
				{
					workbook = new HSSFWorkbook();
				}
			}
			int result;
			try
			{
				bool flag3 = workbook != null;
				if (flag3)
				{
					ISheet sheet = workbook.CreateSheet("sheet1");
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
			return result;
		}
		#endregion


		#region  ListView导出csv
		// Token: 0x0600001A RID: 26 RVA: 0x000038C0 File Offset: 0x00001AC0
		//public static void ListViewToCSV(ListView listView, bool includeHidden)
		//{
		//	SaveFileDialog sfd = new SaveFileDialog();
		//	string filePath = "";
		//	bool flag = sfd.ShowDialog() == DialogResult.OK;
		//	if (flag)
		//	{
		//		filePath = sfd.FileName + ".csv";
		//	}
		//	StringBuilder result = new StringBuilder();
		//	method.WriteCSVRow(result, listView.Columns.Count, (int i) => includeHidden || listView.Columns[i].Width > 0, (int i) => listView.Columns[i].Text);
		//	using (IEnumerator enumerator = listView.Items.GetEnumerator())
		//	{

		//		while (enumerator.MoveNext())
		//		{
		//			ListViewItem listItem = (ListViewItem)enumerator.Current;
		//			StringBuilder result2 = result;
		//			int count = listView.Columns.Count;
		//			Func<int, bool> isColumnNeeded;
		//			if (isColumnNeeded == null)
		//			{
		//				isColumnNeeded = (((int i) => includeHidden || listView.Columns[i].Width > 0));
		//			}
		//			method.WriteCSVRow(result2, count, isColumnNeeded, (int i) => listItem.SubItems[i].Text);
		//		}
		//	}
		//	File.WriteAllText(filePath, result.ToString(), Encoding.Default);
		//	MessageBox.Show("导出成功");
		//}

		// Token: 0x0600001B RID: 27 RVA: 0x00003A08 File Offset: 0x00001C08
		private static void WriteCSVRow(StringBuilder result, int itemsCount, Func<int, bool> isColumnNeeded, Func<int, string> columnValue)
		{
			bool isFirstTime = true;
			int i = 0;
			while (i < itemsCount)
			{
				try
				{
					bool flag = !isColumnNeeded(i);
					if (!flag)
					{
						bool flag2 = !isFirstTime;
						if (flag2)
						{
							result.Append(",");
						}
						isFirstTime = false;
						result.Append(string.Format("\"{0}\"", columnValue(i)));
					}
				}
				catch
				{
				}
				IL_4F:
				i++;
				continue;
				goto IL_4F;
			}
			result.AppendLine();
		}
		#endregion

		#region  设置IE类型
		public static void SetWebBrowserFeatures(method.IeVersion ieVersion)
		{
			bool flag = LicenseManager.UsageMode > LicenseUsageMode.Runtime;
			if (!flag)
			{
				string AppName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
				uint ieMode = method.GeoEmulationModee((int)ieVersion);
				string featureControlRegKey = "HKEY_CURRENT_USER\\Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\";
				Registry.SetValue(featureControlRegKey + "FEATURE_BROWSER_EMULATION", AppName, ieMode, RegistryValueKind.DWord);
				Registry.SetValue(featureControlRegKey + "FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", AppName, 1, RegistryValueKind.DWord);
				Registry.SetValue(featureControlRegKey + "FEATURE_AJAX_CONNECTIONEVENTS", AppName, 1, RegistryValueKind.DWord);
				Registry.SetValue(featureControlRegKey + "FEATURE_GPU_RENDERING", AppName, 1, RegistryValueKind.DWord);
				Registry.SetValue(featureControlRegKey + "FEATURE_WEBOC_DOCUMENT_ZOOM", AppName, 1, RegistryValueKind.DWord);
				Registry.SetValue(featureControlRegKey + "FEATURE_NINPUT_LEGACYMODE", AppName, 0, RegistryValueKind.DWord);
			}
		}

		
		private static uint GeoEmulationModee(int browserVersion)
		{
			uint mode = 11000U;
			switch (browserVersion)
			{
			case 7:
				mode = 7000U;
				break;
			case 8:
				mode = 8000U;
				break;
			case 9:
				mode = 9000U;
				break;
			case 10:
				mode = 10000U;
				break;
			case 11:
				mode = 11000U;
				break;
			}
			return mode;
		}
		#endregion

		#region  Listview导出文本
		public static void ListviewToTxt(ListView listview, int i)
		{
			List<string> list = new List<string>();
			foreach (object obj in listview.Items)
			{
				ListViewItem item = (ListViewItem)obj;
				bool flag = item.SubItems[i].Text.Trim() != "";
				if (flag)
				{
					list.Add(item.SubItems[i].Text);
				}
			}
			SaveFileDialog sfd = new SaveFileDialog();
			string path = "";
			bool flag2 = sfd.ShowDialog() == DialogResult.OK;
			if (flag2)
			{
				path = sfd.FileName + ".txt";
			}
			StringBuilder sb = new StringBuilder();
			foreach (string tel in list)
			{
				sb.AppendLine(tel);
			}
			File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
			MessageBox.Show("文件导出成功!文件地址:" + path);
		}
		#endregion

		#region 下载文件带cookie
		public static void downloadFile(string URLAddress, string subPath, string name, string COOKIE)
		{
			try
			{
				string path = Directory.GetCurrentDirectory();
				WebClient client = new WebClient();
				client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
				client.Headers.Add("Cookie", COOKIE);
				client.Headers.Add("Referer", "https://m.mm131.net/chemo/89_5.html");
				bool flag = !Directory.Exists(subPath);
				if (flag)
				{
					Directory.CreateDirectory(subPath);
				}
				client.DownloadFile(URLAddress, subPath + "\\" + name);
			}
			catch (WebException ex)
			{
				ex.ToString();
			}
		}
		#endregion

		#region Base64编码
		public static string Base64Encode(Encoding encodeType, string source)
		{
			string encode = string.Empty;
			byte[] bytes = encodeType.GetBytes(source);
			try
			{
				encode = Convert.ToBase64String(bytes);
			}
			catch
			{
				encode = source;
			}
			return encode;
		}
		#endregion

		#region Base64解码
		public static string Base64Decode(Encoding encodeType, string result)
		{
			string decode = string.Empty;
			byte[] bytes = Convert.FromBase64String(result);
			try
			{
				decode = encodeType.GetString(bytes);
			}
			catch
			{
				decode = result;
			}
			return decode;
		}
		#endregion

		#region ImageToBase64
		public string ImageToBase64(Image image)
		{
			string result;
			try
			{
				Bitmap bmp = new Bitmap(image);
				MemoryStream ms = new MemoryStream();
				bmp.Save(ms, ImageFormat.Jpeg);
				byte[] arr = new byte[ms.Length];
				ms.Position = 0L;
				ms.Read(arr, 0, (int)ms.Length);
				ms.Close();
				result = Convert.ToBase64String(arr);
			}
			catch (Exception ex)
			{
				result = null;
			}
			return result;
		}
		#endregion

		#region Base64ToImage
		public void Base64ToImage(string strbase64, string filename)
		{
			try
			{
				byte[] arr = Convert.FromBase64String(strbase64);
				MemoryStream ms = new MemoryStream(arr);
				Bitmap bmp = new Bitmap(ms);
				ms.Close();
				Bitmap bmpNew = new Bitmap(bmp);
				bmpNew.Save(filename);
				bmpNew.Dispose();
				bmp.Dispose();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}
		#endregion

		#region 发送邮箱邮件
		public static void send(string address, string subject, string body)
		{
			MailMessage mailMessage = new MailMessage();
			mailMessage.From = new MailAddress("1073689549@qq.com");
			mailMessage.To.Add(new MailAddress(address));
			mailMessage.Subject = subject;
			mailMessage.Body = body;
			mailMessage.IsBodyHtml = true;
			new SmtpClient
			{
				Host = "smtp.qq.com",
				EnableSsl = true,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential("1073689549@qq.com", "nlubektsumvmbbdd")
			}.Send(mailMessage);
		}
		#endregion

		#region 获取重定向网址
		public static string GetRedirectUrl(string url)
		{
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
			req.Method = "HEAD";
			req.AllowAutoRedirect = false;
			HttpWebResponse myResp = (HttpWebResponse)req.GetResponse();
			bool flag = myResp.StatusCode == HttpStatusCode.Found;
			if (flag)
			{
				url = myResp.GetResponseHeader("Location");
			}
			return url;
		}
		#endregion

		#region excel转datatable
		public static DataTable ReadExcelToTable(string path)
		{
			DataTable result;
			try
			{
				string connstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";
				using (OleDbConnection conn = new OleDbConnection(connstring))
				{
					conn.Open();
					DataTable sheetsName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[]
					{
						null,
						null,
						null,
						"Table"
					});
					string firstSheetName = sheetsName.Rows[0][2].ToString();
					string sql = string.Format("SELECT * FROM [{0}]", firstSheetName);
					OleDbDataAdapter ada = new OleDbDataAdapter(sql, connstring);
					DataSet set = new DataSet();
					ada.Fill(set);
					conn.Close();
					conn.Dispose();
					result = set.Tables[0];
				}
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		
		public DataSet SplitDataTable(DataTable orgTable, int rowsNum)
		{
			DataSet ds = new DataSet();
			bool flag = rowsNum <= 0 || orgTable.Rows.Count <= 0;
			DataSet result;
			if (flag)
			{
				ds.Tables.Add(orgTable);
				result = ds;
			}
			else
			{
				int tableNum = Convert.ToInt32(Math.Ceiling((double)orgTable.Rows.Count * 1.0 / (double)rowsNum));
				int remainder = orgTable.Rows.Count % rowsNum;
				bool flag2 = tableNum == 1;
				if (flag2)
				{
					ds.Tables.Add(orgTable);
				}
				else
				{
					for (int i = 0; i < tableNum; i++)
					{
						DataTable table = orgTable.Clone();
						bool flag3 = i != tableNum - 1;
						int num;
						if (flag3)
						{
							num = rowsNum;
						}
						else
						{
							num = ((remainder > 0) ? remainder : rowsNum);
						}
						for (int j = 0; j < num; j++)
						{
							DataRow row = orgTable.Rows[i * rowsNum + j];
							table.ImportRow(row);
						}
						ds.Tables.Add(table);
					}
				}
				result = ds;
			}
			return result;
		}
		#endregion

		#region 生成xml文件
		public void CreateXmlFile()
		{
			XmlDocument xmlDoc = new XmlDocument();
			XmlNode node = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "");
			xmlDoc.AppendChild(node);
			XmlNode root = xmlDoc.CreateElement("User");
			xmlDoc.AppendChild(root);
			this.CreateNode(xmlDoc, root, "name", "xuwei");
			this.CreateNode(xmlDoc, root, "sex", "male");
			this.CreateNode(xmlDoc, root, "age", "25");
			try
			{
				xmlDoc.Save("c://data2.xml");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
		#endregion

		#region 生成xmlnode
		public void CreateNode(XmlDocument xmlDoc, XmlNode parentNode, string name, string value)
		{
			XmlNode node = xmlDoc.CreateNode(XmlNodeType.Element, name, null);
			node.InnerText = value;
			parentNode.AppendChild(node);
		}
		#endregion

		#region 关闭程序自身
		public static void TestForKillMyself()
		{
			string bat = "@echo off\r\n                           :tryagain\r\n                           del %1\r\n                           if exist %1 goto tryagain\r\n                           del %0";
			File.WriteAllText("killme.bat", bat);
			Process.Start(new ProcessStartInfo
			{
				FileName = "killme.bat",
				Arguments = "\"" + Environment.GetCommandLineArgs()[0] + "\"",
				WindowStyle = ProcessWindowStyle.Hidden
			});
		}
		#endregion

		#region wxpush消息UID
		public string getuids()
		{
			StringBuilder sb = new StringBuilder();
			string url = "http://wxpusher.zjiecode.com/api/fun/wxuser/v2?appToken=AT_Zwbx5uVZTIpxJ2OPaCGXXOZNuiWHmTKQ&page=1";
			string html = method.GetUrl(url, "utf-8");
			MatchCollection uids = Regex.Matches(html, "\"uid\":\"([\\s\\S]*?)\"");
			foreach (object obj in uids)
			{
				Match item = (Match)obj;
				sb.Append("\"" + item.Groups[1].Value + "\",");
			}
			return sb.ToString().Remove(sb.ToString().Length - 1, 1);
		}
		#endregion

		#region 发送wxpush消息
		public void sendmsg(string title, string neirong)
		{
			bool flag = title.Trim() != "";
			if (flag)
			{
				string uids = this.getuids();
				string url = "http://wxpusher.zjiecode.com/api/send/message";
				string postdata = string.Concat(new string[]
				{
					"{\"appToken\":\"AT_Zwbx5uVZTIpxJ2OPaCGXXOZNuiWHmTKQ\",\"content\":\"",
					title,
					neirong,
					"\",\"contentType\":2,\"uids\":[",
					uids,
					"]}"
				});
				string html = method.PostUrlDefault(url, postdata, "");
			}
		}
		#endregion

		#region 网址转图片
		public static Image GetImage(string url)
		{
			Image result;
			try
			{
				WebRequest request = WebRequest.Create(url);
				Image img;
				using (WebResponse response = request.GetResponse())
				{
					bool flag = response.ContentType.IndexOf("text") != -1;
					if (flag)
					{
						Stream responseStream = response.GetResponseStream();
						StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
						string srcString = reader.ReadToEnd();
						return null;
					}
					img = Image.FromStream(response.GetResponseStream());
				}
				result = img;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				result = null;
			}
			return result;
		}
		#endregion

		#region 获取编码类型
		public class EncodingType
		{
			// Token: 0x0600003D RID: 61 RVA: 0x00004D18 File Offset: 0x00002F18
			public static Encoding GetTxtType(string FILE_NAME)
			{
				FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read);
				Encoding r = method.EncodingType.GetType(fs);
				fs.Close();
				return r;
			}

			// Token: 0x0600003E RID: 62 RVA: 0x00004D44 File Offset: 0x00002F44
			public static Encoding GetType(FileStream fs)
			{
				byte[] array = new byte[]
				{
					byte.MaxValue,
					254,
					65
				};
				byte[] array2 = new byte[3];
				array2[0] = 254;
				array2[1] = byte.MaxValue;
				byte[] array3 = new byte[]
				{
					239,
					187,
					191
				};
				Encoding reVal = Encoding.Default;
				BinaryReader r = new BinaryReader(fs, Encoding.Default);
				int i;
				int.TryParse(fs.Length.ToString(), out i);
				byte[] ss = r.ReadBytes(i);
				bool flag = method.EncodingType.IsUTF8Bytes(ss) || (ss[0] == 239 && ss[1] == 187 && ss[2] == 191);
				if (flag)
				{
					reVal = Encoding.UTF8;
				}
				else
				{
					bool flag2 = ss[0] == 254 && ss[1] == byte.MaxValue && ss[2] == 0;
					if (flag2)
					{
						reVal = Encoding.BigEndianUnicode;
					}
					else
					{
						bool flag3 = ss[0] == byte.MaxValue && ss[1] == 254 && ss[2] == 65;
						if (flag3)
						{
							reVal = Encoding.Unicode;
						}
					}
				}
				r.Close();
				return reVal;
			}

            // Token: 0x0600003F RID: 63 RVA: 0x00004E70 File Offset: 0x00003070
            //private static bool IsUTF8Bytes(byte[] data)
            //{
            //	int charByteCounter = 1;
            //	foreach (byte curByte in data)
            //	{
            //		bool flag = charByteCounter == 1;
            //		if (flag)
            //		{
            //			bool flag2 = curByte >= 128;
            //			if (flag2)
            //			{
            //				while (((curByte = (byte)(curByte << 1)) & 128) > 0)
            //				{
            //					charByteCounter++;
            //				}
            //				bool flag3 = charByteCounter == 1 || charByteCounter > 6;
            //				if (flag3)
            //				{
            //					return false;
            //				}
            //			}
            //		}
            //		else
            //		{
            //			bool flag4 = (curByte & 192) != 128;
            //			if (flag4)
            //			{
            //				return false;
            //			}
            //			charByteCounter--;
            //		}
            //	}
            //	bool flag5 = charByteCounter > 1;
            //	if (flag5)
            //	{
            //		throw new Exception("非预期的byte格式");
            //	}
            //	return true;
            //}

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
                            //标记位首位若为非0 则至少以2个1开始 如:110XXXXX…1111110X
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

		#region  网络图片转Bitmap
		public Bitmap UrlToBitmap(string url, string cookie)
		{
			
			WebClient mywebclient = new WebClient();
			mywebclient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
			mywebclient.Headers.Add("Cookie", cookie);
			byte[] Bytes = mywebclient.DownloadData(url);

			using (MemoryStream ms = new MemoryStream(Bytes))
			{
				Image outputImg = Image.FromStream(ms);

				Bitmap map = new Bitmap(outputImg);
				return map;
			}
		}
        #endregion

		#region 判断浏览器
        public enum IeVersion
		{
			
			IE7 = 7,
			
			IE8,
			
			IE9,
			
			IE10,
			
			IE11
		}

		#endregion

		#region 移除文件非法字符
		public string removeValid(string illegal)
		{
			string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
			string text = invalid;
			for (int i = 0; i < text.Length; i++)
			{
				illegal = illegal.Replace(text[i].ToString(), "");
			}
			return illegal;
		}
		#endregion


		#region 清空ie的COOKIE
		public void clearIeCookie(WebBrowser webBrowser1)
		{
			HtmlDocument document = webBrowser1.Document;
			document.ExecCommand("ClearAuthenticationCache", false, null);
			webBrowser1.Refresh();
		}
		#endregion
	}
}
