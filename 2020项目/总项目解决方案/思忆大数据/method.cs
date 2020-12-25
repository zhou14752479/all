using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 思忆大数据
{
    class method
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string address { get; set; }

        public string expiretime { get; set; }
        #region verify 
        public static bool verify = false;
        public static bool forbid = false;
        public static string VerifyGet(string Url, string COOKIE, string project)
        {
            if (forbid == false)
            {
                if (verify == false)
                {

                    string html = GetUrl("http://www.acaiji.com/api/do.php?method=getall");

                    if (html.Contains("\"" + project+ "\"") &&project!="")
                    {
                        verify = true;
                        string strhtml = Getverifyurl(Url, COOKIE);
                        return strhtml;
                    }
                    else
                    {
                        forbid = true;
                        return "";

                    }
                }
                else
                {
                    string strhtml = Getverifyurl(Url, COOKIE);
                    return strhtml;

                }
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region verify geturl

        public static string Getverifyurl(string Url, string COOKIE)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                string content = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                return ex.ToString();

            }
        }

        #endregion

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                string content = reader.ReadToEnd();


                reader.Close();
                response.Close();
                return content;



            }
            catch (System.Exception ex)
            {
                ex.ToString();

            }
            return "";
        }
        #endregion

        /// <summary>
        /// 获取时间戳  秒
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalSeconds);
            return a.ToString();
        }
        #region 时间戳转时间
        private DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(Convert.ToDouble(timeStamp));

        }
        #endregion

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        public string register(string username,string password)
        {
            try
            {
                int shiyongtime = Convert.ToInt32(GetTimeStamp()) +1800;
                string url = "http://acaiji.com/api/do.php?method=add&username="+ username + "&password="+password+"&time="+shiyongtime;
                string html = GetUrl(url);
                return html.Trim();
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public string login(string username, string password)
        {
            try
            {
                string url = "http://acaiji.com/api/do.php?method=login&username=" + username + "&password=" + password;
                string html = GetUrl(url);
                return html.Trim();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #region 获取用户信息
       public string getone(string username)
        {
            try
            {
                string url = "http://acaiji.com/api/do.php?method=getone&username=" + username;
                string html = GetUrl(url);
                Match expiretimestamp = Regex.Match(html, @"""expiretimestamp"":""([\s\S]*?)""");
                expiretime = expiretimestamp.Groups[1].Value.Trim();
                return ConvertStringToDateTime(expiretime).ToString("yyyy-MM-dd HH:mm:ss"); ;
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion


        #region 获取Mac地址
        /// <summary>
        /// 获取Mac地址
        /// </summary>
        /// <returns></returns>
        public string GetMacAddress()
        {
            try
            {
                string strMac = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        strMac = mo["MacAddress"].ToString();
                    }
                }
                moc = null;
                mc = null;
                return strMac;
            }
            catch
            {
                return "unknown";
            }
        }

        #endregion
        /// <summary>
        /// 获取经纬度
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public ArrayList getlat(string city)
        {
            ArrayList areas = new ArrayList();
            string url = "http://www.jsons.cn/lngcode/?keyword=" + System.Web.HttpUtility.UrlEncode(city) + "&txtflag=0";
            string html = GetUrl(url);

            Match ahtml = Regex.Match(html, @"<table class=""table table-bordered table-hover"">([\s\S]*?)</table>");

            MatchCollection values = Regex.Matches(ahtml.Groups[1].Value, @"<td>([\s\S]*?)</td>");

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].Groups[1].Value.Contains("1"))
                {
                    areas.Add(values[i].Groups[1].Value.Replace("，", "%2C").Trim());
                }
            }
            return areas;
        }


        /// <summary>
        /// 获取关键词
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public ArrayList getkeywords(ListView lv)
        {
            ArrayList keywords = new ArrayList();
            for (int i = 0; i < lv.Items.Count; i++)
            {
                keywords.Add(lv.Items[i].SubItems[0].Text);
            }
            return keywords;
        }

        public bool zanting=true;
        public bool status = true;
        public ArrayList citys = new ArrayList();
        public ArrayList keywords = new ArrayList();
        public string telpanduan = "全部采集";
        public static string username = "";
        /// <summary>
        /// 地图主程序
        /// </summary>
        public void ditu(object o)
        {
            ListView LV = (ListView)o;
            if (keywords.Count == 0)
            {
                MessageBox.Show("请添加关键字");
                return;
            }


            try
            {

                if (citys.Count == 0)
                {
                    MessageBox.Show("请添加城市");
                    return;
                }

                foreach (string keyword in keywords)
                {

                    foreach (string city in citys)
                    {
                        ArrayList areaLats = getlat(city);

                        foreach (string lat in areaLats)
                        {


                            for (int page = 1; page < 100; page++)
                            {


                                string url = "https://restapi.amap.com/v3/place/around?appname=1e3bb24ab8f75ba78a7cf8a9cc4734c6&key=1e3bb24ab8f75ba78a7cf8a9cc4734c6&keywords=" + System.Web.HttpUtility.UrlEncode(keyword) + "&location=" + lat + "&logversion=2.0&page=" + page + "&platform=WXJS&s=rsx&sdkversion=1.2.0";
                                string html = VerifyGet(url,"",username);



                                MatchCollection names = Regex.Matches(html, @"""name"":""([\s\S]*?)""");
                                MatchCollection tels = Regex.Matches(html, @"""tel"":([\s\S]*?),");
                                MatchCollection address = Regex.Matches(html, @"""address"":([\s\S]*?),");
                                MatchCollection pros = Regex.Matches(html, @"""pname"":([\s\S]*?),");
                                MatchCollection citynames = Regex.Matches(html, @"""cityname"":([\s\S]*?),");
                                MatchCollection areas = Regex.Matches(html, @"""adname"":([\s\S]*?),");
                                MatchCollection types = Regex.Matches(html, @"""type"":([\s\S]*?),");

                                if (names.Count == 0)
                                    break;

                                for (int i = 0; i < names.Count; i++)
                                {
                                    bool telpd = true;

                                    switch (telpanduan)
                                    {
                                        case "全部采集":
                                            break;
                                        case "只采集有联系方式":
                                            if (tels[i].Groups[1].Value.Replace("\"", "") == "[]")
                                            {
                                                telpd = false;
                                            }
                                            else
                                            {
                                                telpd = true;
                                            }
                                            break;

                                    }

                                    if (telpd)
                                    {

                                        ListViewItem lv1 = LV.Items.Add((LV.Items.Count + 1).ToString()); //使用Listview展示数据

                                        lv1.SubItems.Add(names[i].Groups[1].Value);
                                        lv1.SubItems.Add(tels[i].Groups[1].Value.Replace("\"", ""));
                                        lv1.SubItems.Add(address[i].Groups[1].Value.Replace("\"", ""));
                                        lv1.SubItems.Add(pros[i].Groups[1].Value.Replace("\"", ""));
                                        lv1.SubItems.Add(citynames[i].Groups[1].Value.Replace("\"", ""));
                                        lv1.SubItems.Add(areas[i].Groups[1].Value.Replace("\"", ""));
                                        lv1.SubItems.Add(types[i].Groups[1].Value.Replace("\"", ""));
                                        lv1.SubItems.Add(keyword);
                                        while (this.zanting == false)
                                        {
                                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                        }
                                        if (status == false)
                                            return;
                                    }


                                }
                                Thread.Sleep(1000);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }


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
            //lv.Columns.Count
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
                    try
                    {
                        dr[j] = lv.Items[i].SubItems[j].Text.Trim();

                    }
                    catch
                    {

                        continue;
                    }

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


    }
}
