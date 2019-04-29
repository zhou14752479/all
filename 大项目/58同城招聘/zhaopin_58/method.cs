using MySql.Data.MySqlClient;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zhaopin_58
{
    class method
    {

        #region 下载文件
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="URLAddress">图片地址</param>
        /// <param name="subPath">图片所在文件夹</param>
        /// <param name="name">图片名称</param>
        public static void downloadFile(string URLAddress, string subPath, string name)
        {
            string path = System.IO.Directory.GetCurrentDirectory();

            WebClient client = new WebClient();

            if (false == System.IO.Directory.Exists(subPath))
            {
                //创建pic文件夹
                System.IO.Directory.CreateDirectory(subPath);
            }
            client.DownloadFile(URLAddress, subPath + "\\" + name);
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
                string COOKIE = "abt=1499071937.0%7CADE; _lxsdk_cuid=15d07a688f6c8-0b50ac5c472b18-333f5902-100200-15d07a688f79c; oc=xrmkO2nLZoY_IrhbS451igIl7hYw7IdDK6N_eVitVxW6WMxwN8yqI07hUj0vwzV57Iemuglzh4HS0E77JWewbxs2LOrDkniIWKv_go8Z8i77EVeAUCUejMdsEHZtdIDxMvg4fR4p53MxVNd2YZr8ZNhk_yZNN_hIE2VChkJKOJI; __mta=54360727.1499071941097.1518360971460.1518584371801.7; iuuid=F457EFC99EEB24DD0A17795BB1F8A91129848721BFF3EE82C45EF7C15E7C210E; _lxsdk=F457EFC99EEB24DD0A17795BB1F8A91129848721BFF3EE82C45EF7C15E7C210E; webp=1; __utmz=74597006.1547084235.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); _hc.v=125767d7-22b6-b361-e802-854d70351873.1547084296; cityname=%E5%AE%BF%E8%BF%81; __utma=74597006.1002021895.1547084235.1547426995.1547429915.3; i_extend=C145095553688078665527034436504084189858_b2_e4339319119865529162_v1084616022904540227_a%e8%bf%90%e5%8a%a8%e5%81%a5%e8%ba%ab_f179411730E015954189128616476612677712756937264803_e7765034795530822391_v1084625786170336172_a%e8%bf%90%e5%8a%a8%e5%81%a5%e8%ba%abGimthomepagesearchH__a100005__b3; uuid=1542127099cf4e0c8245.1550021213.1.0.0; ci=60; rvct=60%2C1%2C184%2C875%2C55%2C40; __mta=54360727.1499071941097.1518584371801.1550470987575.8; _lxsdk_s=168ff45b4ee-4d2-6f6-965%7C%7C6";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Timeout = 10000;
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36";
                request.AllowAutoRedirect = true;
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
                ex.ToString();

            }
            return "";
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
            catch (MySqlException ee)
            {
                ee.Message.ToString();
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
                return ex.ToString();
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
                Console.WriteLine("Exception: " + ex.Message);
                return -1;
            }
        }

        #endregion

        #region 获取IP地区
        /// <summary>
        /// 获取IP地区
        /// </summary>
        /// <returns></returns>
        public static string GetIp()
        {
            try
            {

                string html = GetUrl("https://ip.cn/");

                MatchCollection match = Regex.Matches(html, @"<code>([\s\S]*?)</code>", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                return match[0].Groups[1].Value.Trim();

            }

            catch (Exception ex)
            {
                ex.ToString();

                return "获取IP错误";
            }

        }

        #endregion




        #region 通过API获取IP
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetIp(string Url)
        {
            try
            {         
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.81 Safari/537.36";
                request.AllowAutoRedirect = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 10000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  I
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



        public static string GetUrlWithIp(string URL, string ip,int port)
        {
            try
            {
                string COOKIE = "__utmz=253535702.1530623991.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); city=3144; wmda_visited_projects=%3B6333604277682; xxzl_deviceid=DviWvqAxjqBKPKAAQlHF%2Fu3wlwjT0ubNuBl4jjM2ZwOf4nW7LAg%2B3dpGBIRZvLWF; wmda_new_uuid=1; id58=c5/nn1s7d/lRQJdfbXolAg==; ppStore_fingerprint=6B2AABD021069F53E575C77EDC1494CF3247CEC048B37880%EF%BC%BF1536803833803; __utma=253535702.1105287715.1530623991.1530623991.1530623991.1; new_uv=4; wmda_uuid=303945c42e3068567ada1d9e5881ca2f; als=0; 58tj_uuid=c87bfc4e-5928-4f53-bfe2-1b3190aa6606; cookieuid1=mgjwFVtxWKKpblS6A2QYAg==";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);  //创建一个链接
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.81 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
                WebProxy proxyObject = new WebProxy(ip, port);//str为IP地址 port为端口号 代理类
                request.Proxy = proxyObject; //设置代理 
                request.AllowAutoRedirect = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  I
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
    }
}
