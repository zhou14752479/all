using MySql.Data.MySqlClient;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main
{
    class method
    {
        //非常重要获取当前存在浏览器的cookie，可以登陆wbbbrowser更新cookie。

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref System.UInt32 pcchCookieData, int dwFlags, IntPtr lpReserved);



        //浏览器获取源码，亲测好用
        //StreamReader reader = new StreamReader(webBrowser1.DocumentStream, Encoding.GetEncoding(webBrowser1.Document.Encoding));
        //string html = reader.ReadToEnd();
     

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
                string COOKIE = "id58=c5/nn1qC89pTY5VqGmdcAg==; 58tj_uuid=c3d04c48-dd4d-49b3-816c-4633755f76cb; als=0; xxzl_deviceid=H3uwjdpchUrszAFFwEXWI5UaNJCSrPvfwBqXEZzrZl2NJTBICdPpn5Oz382zj99d; wmda_uuid=70c7fd7fd7b308b2e6a6206def584118; wmda_new_uuid=1; gr_user_id=7ec08bb8-eca4-45e4-9423-142f24f58994; Hm_lvt_d32bebe8de17afd6738ef3ad3ffa4be3=1518609564; UM_distinctid=161d0fb312c473-0f0b11be7fbcac-3b60490d-1fa400-161d0fb312d34b; wmda_visited_projects=%3B1731916484865%3B1409632296065%3B1732038237441%3B1732039838209%3B2385390625025; __utma=253535702.1777428119.1523013867.1523013867.1523013867.1; __utmz=253535702.1523013867.1.1.utmcsr=sh.58.com|utmccn=(referral)|utmcmd=referral|utmcct=/shangpuqg/0/; _ga=GA1.2.1777428119.1523013867; Hm_lvt_3bb04d7a4ca3846dcc66a99c3e861511=1526113480,1528285922; Hm_lvt_e15962162366a86a6229038443847be7=1526113481,1528285922; Hm_lvt_e2d6b2d0ec536275bb1e37b421085803=1526113565,1528285939; Hm_lvt_4d4cdf6bc3c5cb0d6306c928369fe42f=1530434006; final_history=33904487894826%2C33904487533008%2C34202955671375%2C34457504313538%2C34457504172715; mcity=sh; mcityName=%E4%B8%8A%E6%B5%B7; nearCity=%5B%7B%22cityName%22%3A%22%E5%8D%97%E4%BA%AC%22%2C%22city%22%3A%22nj%22%7D%2C%7B%22cityName%22%3A%22%E5%AE%BF%E8%BF%81%22%2C%22city%22%3A%22suqian%22%7D%2C%7B%22cityName%22%3A%22%E4%B8%8A%E6%B5%B7%22%2C%22city%22%3A%22sh%22%7D%5D; cookieuid1=mgjwFVtJtsaIVFa/CEaJAg==; Hm_lvt_5a7a7bfd6e7dfd9438b9023d5a6a4a96=1531557573; city=cq; 58home=cq; new_uv=41; utm_source=; spm=; init_refer=; f=n; new_session=0; qz_gdt=; _house_detail_show_time_=2; ppStore_fingerprint=FCCDFD5888AF9A96AB5621ECA8D12A45C2DA9181ABE0A5F0%EF%BC%BF1532790344974";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";

                request.Headers.Add("Cookie", COOKIE);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string content = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                pubVariables.exs = ex.ToString();

            }
            return "";
        }
        #endregion

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData, string COOKIE, string charset)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Post";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postData.Length;

            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.112 Safari/537.36";
            request.Headers.Add("Cookie", COOKIE);

            StreamWriter sw = new StreamWriter(request.GetRequestStream());
            sw.Write(postData);
            sw.Flush();


            WebResponse response = request.GetResponse();
            Stream s = response.GetResponseStream();
            StreamReader sr = new StreamReader(s, Encoding.GetEncoding(charset));
            string html = sr.ReadToEnd();

            sw.Dispose();
            sw.Close();
            sr.Dispose();
            sr.Close();
            s.Dispose();
            s.Close();
            return html;
        }

        #endregion

        #region 美团独立的GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetMtUrl(string Url,string COOKIE)
        {
            try
            {                
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Mobile Safari/537.36";

                request.Headers.Add("Cookie", COOKIE);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string content = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                pubVariables.exs = ex.ToString();

            }
            return "";
        }
        #endregion

        #region 阿里巴巴因为编码问题独立的Get请求
        public static string GetAliUrl(string Url, string COOKIE)
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.AllowAutoRedirect = true;
                request.KeepAlive = true;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gb2312")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string content = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                pubVariables.exs = ex.ToString();

            }
            return "";
        }
        #endregion

        #region 获取数据库美团城市名称
        /// <summary>
        /// 获取数据库美团城市名称
        /// </summary>
        /// <param name="cob">数据绑定的下拉框</param>
        public static void getMeituanCityName(ComboBox cob)
        {
            ArrayList list = new ArrayList();
            try
            {
                string constr = "Host =116.62.62.62;Database=citys;Username=root;Password=zhoukaige";
                string str = "SELECT meituan_city_name from meituan_city ";
                MySqlDataAdapter da = new MySqlDataAdapter(str, constr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString().Trim());
                }
            }
            catch (MySqlException ex)
            {
                pubVariables.exs = ex.ToString();
            }
            cob.DataSource = list;
            
        }
        #endregion

        #region 获取数据库58城市名称
        /// <summary>
        /// 获取数据库美团城市名称
        /// </summary>
        /// <param name="cob">数据绑定的下拉框</param>
        public static void get58CityName(ComboBox cob)
        {
            ArrayList list = new ArrayList();
            try
            {
                string constr = "Host =116.62.62.62;Database=citys;Username=root;Password=zhoukaige";
                string str = "SELECT cityname from city_58 ";
                MySqlDataAdapter da = new MySqlDataAdapter(str, constr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString().Trim());
                }
            }
            catch (MySqlException ex)
            {
                pubVariables.exs = ex.ToString();
            }
            cob.DataSource = list;

        }
        #endregion
       
        #region 获取赶集城市名称
        /// <summary>
        /// 获取数据库美团城市名称
        /// </summary>
        /// <param name="cob">数据绑定的下拉框</param>
        public static void ganjicityName(ComboBox cob)
        {
            ArrayList list = new ArrayList();
            try
            {
                string constr = "Host =116.62.62.62;Database=citys;Username=root;Password=zhoukaige";
                string str = "SELECT cityName from ganji_city ";
                MySqlDataAdapter da = new MySqlDataAdapter(str, constr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString().Trim());
                }
            }
            catch (MySqlException ex)
            {
                pubVariables.exs = ex.ToString();
            }
            cob.DataSource = list;

        }
        #endregion
       
        #region  获取赶集城市拼音

        public static string ganjiCityPinyin(string city)
        {

            try
            {



                string constr = "Host =116.62.62.62;Database=citys;Username=root;Password=zhoukaige";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select citycode from ganji_city where cityName='" + city + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                reader.Read();

                string citypinyin = reader["citycode"].ToString().Trim();
                mycon.Close();
                reader.Close();
                return citypinyin;


            }

            catch (System.Exception ex)
            {
                pubVariables.exs = ex.ToString();
                return "";
            }


        }

        #endregion

        #region 获取赶集本地分类名称
        /// <summary>
        /// 获取赶集本地分类名称
        /// </summary>
        /// <param name="cob"></param>
        public static void ganjiItemName(ComboBox cob)
        {
            ArrayList list = new ArrayList();
            try
            {
                string constr = "Host =116.62.62.62;Database=citys;Username=root;Password=zhoukaige";
                string str = "SELECT name from ganji_item ";
                MySqlDataAdapter da = new MySqlDataAdapter(str, constr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString().Trim());
                }
            }
            catch (MySqlException ex)
            {
                pubVariables.exs = ex.ToString();
            }
            cob.DataSource = list;

        }
        #endregion

        #region  获取赶集本地分类拼音
        /// <summary>
        /// 获取赶集本地分类拼音
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string ganjiitempyin(string item)
        {

            try
            {



                string constr = "Host =116.62.62.62;Database=citys;Username=root;Password=zhoukaige";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select pinyin from ganji_item where name='" + item + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                reader.Read();

                string citypinyin = reader["pinyin"].ToString().Trim();
                mycon.Close();
                reader.Close();
                return citypinyin;


            }

            catch (System.Exception ex)
            {
                pubVariables.exs = ex.ToString();
                return "";
            }


        }

        #endregion

        #region 获取数据库黄页88城市名称
        /// <summary>
        /// 获取数据库美团城市名称
        /// </summary>
        /// <param name="cob">数据绑定的下拉框</param>
        public static void gethy88CityName(ComboBox cob)
        {
            ArrayList list = new ArrayList();
            try
            {
                string constr = "Host =116.62.62.62;Database=citys;Username=root;Password=zhoukaige";
                string str = "SELECT city from hy88 ";
                MySqlDataAdapter da = new MySqlDataAdapter(str, constr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString().Trim());
                }
            }
            catch (MySqlException ex)
            {
                pubVariables.exs = ex.ToString();
            }
            cob.DataSource = list;

        }
        #endregion

        #region  58获取数据库中城市名称对应的拼音

        public static string Get58pinyin(string city)
        {

            try
            {



                string constr = "Host =116.62.62.62;Database=citys;Username=root;Password=zhoukaige";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select citycode from city_58 where cityname='" + city + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                reader.Read();

                string citypinyin = reader["citycode"].ToString().Trim();
                mycon.Close();
                reader.Close();
                return citypinyin;


            }

            catch (System.Exception ex)
            {
                pubVariables.exs = ex.ToString();
                return "";
            }


        }

        #endregion

        #region  美团获取数据库中城市名称对应的拼音

        public static string GetMtpinyin(string city)
        {

            try
            {



                string constr = "Host =116.62.62.62;Database=citys;Username=root;Password=zhoukaige";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select meituan_city_pinyin from meituan_city where meituan_city_name='" + city + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                reader.Read();

                string citypinyin = reader["meituan_city_pinyin"].ToString().Trim();
                mycon.Close();
                reader.Close();
                return citypinyin;


            }

            catch (System.Exception ex)
            {
                pubVariables.exs = ex.ToString();
                return "" ;
            }


        }

        #endregion

        #region  黄页88获取数据库中城市名称对应的拼音

        public static string Gethy88pinyin(string city)
        {

            try
            {



                string constr = "Host =116.62.62.62;Database=citys;Username=root;Password=zhoukaige";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select citypinyin from hy88 where city='" + city + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                reader.Read();

                string citypinyin = reader["citypinyin"].ToString().Trim();
                mycon.Close();
                reader.Close();
                return citypinyin;


            }

            catch (System.Exception ex)
            {
                pubVariables.exs = ex.ToString();
                return "";
            }


        }

        #endregion

        #region 获取百度citycode
        public static int getcityId(string cityName)
        {
            try

            {

                String Url = "https://map.baidu.com/?newmap=1&reqflag=pcmap&biz=1&from=webmap&da_par=direct&pcevaname=pc4.1&qt=s&da_src=searchBox.button&wd=" + cityName + "&c=289&src=0&wd2=&pn=0&sug=0&l=12&b=(13461858.87,3636969.979999999;13584738.87,3670185.979999999)&from=webmap&biz_forward={%22scaler%22:1,%22styles%22:%22pl%22}&sug_forward=&tn=B_NORMAL_MAP&nn=0&u_loc=13166533,3998088&ie=utf-8";

                string html = method.GetUrl(Url);


                MatchCollection Matchs = Regex.Matches(html, @"""code"":([\s\S]*?),", RegexOptions.IgnoreCase);




                int cityId = Convert.ToInt32(Matchs[0].Groups[1].Value);
                return cityId;

            }
            catch (System.Exception ex)
            {
                pubVariables.exs = ex.ToString();
                return 1;
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

        #region 设置表格格式

        public static void setDatagridview(DataGridView dgv, int count, string[] headers)
        {

            dgv.ColumnCount = count;


            for (int i = 0; i < count; i++)
            {
                dgv.Columns[i].HeaderText = headers[i];

            }
        }

        #endregion

        #region datagriview转datatable
        public static DataTable DgvToTable(DataGridView dgv)
        {
            DataTable dt = new DataTable();
            // 列强制转换
            for (int count = 0; count < dgv.Columns.Count; count++)
            {
                DataColumn dc = new DataColumn(dgv.Columns[count].Name.ToString());
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

        #region 获取Mac地址
        /// <summary>
        /// 获取Mac地址
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
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
            catch(SyntaxErrorException ex)
            {
                pubVariables.exs = ex.ToString();
                return "";
            }
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
            sfd.Filter = "xls|*.xls|xlsx|*.xlsx";
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
                pubVariables.exs = ex.ToString();
                return -1;
            }
        }

        #endregion


        #region dataGridView导出CSV，导出分列
        /// <summary>
        /// dataGridView导出CSV，导出的结果分列
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <returns></returns>
        public static bool dataGridViewToCSV(DataGridView dataGridView)
        {
            if (dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("没有数据可导出!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.CreatePrompt = true;
            saveFileDialog.FileName = null;
            saveFileDialog.Title = "保存";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Stream stream = saveFileDialog.OpenFile();
                StreamWriter sw = new StreamWriter(stream, System.Text.Encoding.GetEncoding(-0));
                string strLine = "";
                try
                {
                    //表头
                    for (int i = 0; i < dataGridView.ColumnCount; i++)
                    {
                        if (i > 0)
                            strLine += ",";
                        strLine += dataGridView.Columns[i].HeaderText;
                    }
                    strLine.Remove(strLine.Length - 1);
                    sw.WriteLine(strLine);
                    strLine = "";
                    //表的内容
                    for (int j = 0; j < dataGridView.Rows.Count; j++)
                    {
                        strLine = "";
                        int colCount = dataGridView.Columns.Count;
                        for (int k = 0; k < colCount; k++)
                        {
                            if (k > 0 && k < colCount)
                                strLine += ",";
                            if (dataGridView.Rows[j].Cells[k].Value == null)
                                strLine += "";
                            else
                            {
                                string cell = dataGridView.Rows[j].Cells[k].Value.ToString().Trim();
                                //防止里面含有特殊符号
                                cell = cell.Replace("\"", "\"\"");
                                cell = "\"" + cell + "\"";
                                strLine += cell;
                            }
                        }
                        sw.WriteLine(strLine);
                    }
                    sw.Close();
                    stream.Close();
                    MessageBox.Show("数据被导出到：" + saveFileDialog.FileName.ToString(), "导出完毕", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "导出错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region 积分减少
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="point">当前积分</param>
        /// <param name="change">改动的积分</param>

        public static void decreasePoints(string username, int point, int change)
        {
            try
            {


                string constr = "Host =116.62.62.62;Database=vip;Username=root;Password=zhoukaige";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                DateTime dt = DateTime.Now;
                int points = point - change;

                MySqlCommand cmd = new MySqlCommand("UPDATE vip_points  SET points ='" + points + " ' where username= '" + username + " '", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {
                    MessageBox.Show("本次共消耗" + change + "积分");

                    mycon.Close();

                }

            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        #endregion

        #region 读取积分
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username">用户名</param>
        public static int getPoints(string username)
        {

            string constr = "Host =116.62.62.62;Database=vip;Username=root;Password=zhoukaige";
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();


            MySqlCommand cmd = new MySqlCommand("select * from vip_points where username = '" + username + "'  ", mycon);         //SQL语句读取textbox的值'" + skinTextBox1.Text+"'
            MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源
            if (reader.Read())
            {
                string points = reader["points"].ToString().Trim();

                mycon.Close();
                reader.Close();

                return Convert.ToInt32(points);
            }
            else
            {
                return 0;
            }


        }
        #endregion

        #region 输入地址获取经纬度
        public static void getPoi(string address)
        {

            try

            {

                string html = GetUrl("http://apis.map.qq.com/jsapi?qt=poi&wd=" + address);
                string Rxg1 = @"""pointx"":([\s\S]*?),";
                string Rxg2 = @"""pointy"":([\s\S]*?),";

                

                MatchCollection x = Regex.Matches(html, Rxg1);
                MatchCollection y = Regex.Matches(html, Rxg2);

                foreach (Match match in x)
                {
                   string pointx = match.Groups[1].Value;
                }

                foreach (Match match in y)
                {
                    string pointy = match.Groups[1].Value;
                }


                

            }
            catch (System.Exception ex)
            {
                pubVariables.exs = ex.ToString();
               
            }


        }
        #endregion


        #region 非常重要获取当前存在浏览器的cookie，可以登陆wbbbrowser更新cookie。
        /// <summary>
        /// 非常重要获取当前存在浏览器的cookie，可以登陆wbbbrowser更新cookie。
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetCookies(string url)
        {
            uint datasize = 256;
            StringBuilder cookieData = new StringBuilder((int)datasize);
            if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x2000, IntPtr.Zero))
            {
                if (datasize < 0)
                    return null;


                cookieData = new StringBuilder((int)datasize);
                if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x00002000, IntPtr.Zero))
                    return null;
            }
            return cookieData.ToString();
        }
        #endregion

        /// <summary>  
        /// Unicode字符串转为正常字符串  
        /// </summary>  
        /// <param name="srcText"></param>  
        /// <returns></returns>  
        public static string UnicodeToString(string srcText)
        {
           
                string outStr = "";
                Regex reg = new Regex(@"(?i)\\u([0-9a-f]{4})");
                outStr = reg.Replace(srcText, delegate (Match m1)
                {
                    return ((char)Convert.ToInt32(m1.Groups[1].Value, 16)).ToString();
                });
                return outStr;
            

        }

        /// <summary>
        /// 创建文件夹返回文件夹地址
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string CreateDirectory(string item)
        {
           
            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

            if (!Directory.Exists(path + "\\data\\"+ item))
            {
                Directory.CreateDirectory(path + "\\data\\" + item);
            }
            return path + "\\data\\" + item;
        }


        /// <summary>
        /// 文件夹不存在则执行创建文件夹以及创建表
        /// </summary>
        /// <param name="item"></param>
        /// <param name="columns"></param>
        public static void CreateTable(string item, string[] columns)
        {
            string Nowpath = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

            if (!Directory.Exists(Nowpath + "\\data\\" + item))
            {

                Directory.CreateDirectory(Nowpath + "\\data\\" + item);

                string path = Nowpath + "\\data\\" + item;
                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\Content.db");
                mycon.Open();
                string sql = "CREATE TABLE result( '" + columns[0] + "' varchar(255))";
                SQLiteCommand cmd = new SQLiteCommand(sql, mycon);
                cmd.ExecuteNonQuery();
                mycon.Close();


                for (int i = 1; i < columns.Count(); i++)
                {
                    mycon.Open();
                    string sql2 = "ALTER TABLE result ADD '" + columns[i] + "' varchar(255)";

                    SQLiteCommand cmd2 = new SQLiteCommand(sql2, mycon);
                    cmd2.ExecuteNonQuery();
                    mycon.Close();
                }

            }

         
        }

        /// <summary>
        /// 插入表
        /// </summary>
        /// <param name="item"></param>
        /// <param name="columns"></param>
        /// <param name="values"></param>
        public static void insertData(string item, string sql)
        {
            string Nowpath = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
            string path = Nowpath + "\\data\\" + item;
            SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\Content.db");
            mycon.Open();
          
            SQLiteCommand cmd = new SQLiteCommand(sql, mycon);

            cmd.ExecuteNonQuery();  //执行sql语句
            mycon.Close();
        }

        /// <summary>
        /// 清空表
        /// </summary>
        /// <param name="item"></param>
        public static void clearTable(string item)
        {

            string Nowpath = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
            string path = Nowpath + "\\data\\" + item;
            SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\Content.db");
            mycon.Open();
            string sql = "DELETE FROM result";

            SQLiteCommand cmd = new SQLiteCommand(sql, mycon);

            cmd.ExecuteNonQuery();  //执行sql语句
            
        }


        /// <summary>
        /// 黄页88行业拼音
        /// </summary>
        /// <param name="itemName"></param>
        public static string gethy88itemCode(string itemName)
        {

            string Nowpath = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
            string path = Nowpath + "\\system\\" ;
            SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\items.db");
            mycon.Open();
            string sql = "select itemCode from hy88 where itemName='" + itemName + "'";

            SQLiteCommand cmd = new SQLiteCommand(sql, mycon);

            SQLiteDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

            reader.Read();

            string itemCode = reader["itemCode"].ToString().Trim();
            reader.Close();
            return itemCode;


        }
        /// <summary>
        /// 黄页88行业
        /// </summary>
        /// <param name="cob"></param>
        public static void gethy88itemName(ComboBox cob)
        {
            ArrayList list = new ArrayList();
            try
            {
                string Nowpath = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
                string path = Nowpath + "\\system\\";
                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\items.db");
                string sql = "SELECT itemName from hy88 ";
               SQLiteDataAdapter da = new SQLiteDataAdapter(sql, mycon);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString().Trim());
                }
            }
            catch (MySqlException ex)
            {
                pubVariables.exs = ex.ToString();
            }
            cob.DataSource = list;

        }
        /// <summary>
        /// 慧聪网城市列表
        /// </summary>
        /// <param name="cob"></param>
        public static void getHccityName(ComboBox cob)
        {
            ArrayList list = new ArrayList();
            try
            {
                string Nowpath = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
                string path = Nowpath + "\\system\\";
                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\citys.db");
                string sql = "SELECT cityName from huicong ";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, mycon);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString().Trim());
                }
            }
            catch (MySqlException ex)
            {
                pubVariables.exs = ex.ToString();
            }
            cob.DataSource = list;

        }


        /// <summary>
        /// 顺企网行业Id
        /// </summary>
        /// <param name="cob"></param>
        public static void getshunqiItemName(ComboBox cob)
        {
            ArrayList list = new ArrayList();
            try
            {
                string Nowpath = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
                string path = Nowpath + "\\system\\";
                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\items.db");
                string sql = "SELECT itemName from shunqi ";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, mycon);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString().Trim());
                }
            }
            catch (MySqlException ex)
            {
                pubVariables.exs = ex.ToString();
            }
            cob.DataSource = list;

        }
        /// <summary>
        /// 顺企网行业拼音
        /// </summary>
        /// <param name="itemName"></param>
        public static string getshunqiItemId(string itemName)
        {

            string Nowpath = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
            string path = Nowpath + "\\system\\";
            SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\items.db");
            mycon.Open();
            string sql = "select itemId from shunqi where itemName='" + itemName + "'";

            SQLiteCommand cmd = new SQLiteCommand(sql, mycon);

            SQLiteDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

            reader.Read();

            string itemCode = reader["itemId"].ToString().Trim();
            reader.Close();
            return itemCode;


        }



    }
}
