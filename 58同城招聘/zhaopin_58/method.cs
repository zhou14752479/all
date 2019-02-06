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
                string COOKIE = "_lxsdk_cuid=1619d225de7ad-0c5551b9982ceb-3b60490d-1fa400-1619d225de8c8; iuuid=EE452628FFED0D7E2E8057602BBF1FB40975D7AC4634E49AA9A0FDF0EEA3FC2F; _lxsdk=EE452628FFED0D7E2E8057602BBF1FB40975D7AC4634E49AA9A0FDF0EEA3FC2F; webp=1; _hc.v=e24e6fb3-392f-efd0-1faa-23a60600128d.1522554246; _ga=GA1.2.1049379255.1526306541; __mta=46562712.1518765142494.1532784659235.1533443717244.73; UM_distinctid=16556dfabd2f5-0f889f94418bdc-762e6d31-ee67c-16556dfabd316e; oc=HHdE1pzMFrdyJAh4aEBuGv_bXE5LSe-aHksZ9tr_sc4-RplxzhG0a9w-vgyjGw4e7nLCFL_rT0P5voF0RmqlIA-s5fsbOkZgu6cxdlreV5nixPpfpB6Z_Xb-Z8LqqLKzcOpgTWVZjJApjKPMcAwtdt3vQlMNkzzEDdwYh0Ks_uE; __utmz=74597006.1537842327.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); mtcdn=K; ci=1; rvct=1%2C184%2C40%2C66%2C10%2C42%2C59%2C30%2C45%2C50%2C20; cityname=%E5%8C%97%E4%BA%AC; latlng=33.950258,118.277952,1549251093284; __utma=74597006.1049379255.1526306541.1544513677.1549251093.4; i_extend=C_b3GimthomepagesearchH__a100001__b4";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Timeout = 10000;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.81 Safari/537.36";
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
