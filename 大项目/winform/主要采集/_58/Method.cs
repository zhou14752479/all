using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Management;
using System.Timers;
using MySql.Data.MySqlClient;
using System.Xml;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data.OleDb;
using System.Data.SQLite;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Collections;

namespace _58
{
    public class Method
    {
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
                string constr = "Host =47.99.68.92;Database=citys;Username=root;Password=zhoukaige00.@*.";
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
                MessageBox.Show(ee.Message.ToString());
            }
            cob.DataSource = list;

        }
        #endregion

        #region 获取数据库58城市名称到LIST集合
        /// <summary>
        /// 获取数据库美团城市名称
        /// </summary>
        /// <param name="cob">数据绑定的下拉框</param>
        public static ArrayList get58CityNames()
        {
            ArrayList list = new ArrayList();
            try
            {
                string constr = "Host =47.99.68.92;Database=citys;Username=root;Password=zhoukaige00.@*.";
                string str = "SELECT citycode from city_58 ";
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
                MessageBox.Show(ee.Message.ToString());
            }
            return list;

        }
        #endregion

        #region  58获取数据库中城市名称对应的拼音

        public static string Get58pinyin(string city)
        {

            try
            {



                string constr = "Host =47.99.68.92;Database=citys;Username=root;Password=zhoukaige00.@*.";
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

        public static string User; //当前登陆用户
        #region 导出文本文件
        /// <summary>
        /// 导出文本文件
        /// </summary>
        /// <param name="dgv">需要导出的表格</param>
        public static void Txt(DataGridView dgv) //另存新档按钮   导出成.txt文件
        {


            

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "txt文件 (*.txt)|*.txt";
            //saveFileDialog.Filter = "词库文件 (*.ys)|*.ys";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.FileName = "采集器手机号码";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.CreatePrompt = true;
            saveFileDialog.Title = "导出txt文件到";
            //saveFileDialog.ShowDialog();
            if (saveFileDialog.ShowDialog() != DialogResult.Cancel)
            {
                Stream myStream;
                myStream = saveFileDialog.OpenFile();
                StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));
                try
                {
                    //写内容
                    for (int j = 0; j < dgv.Rows.Count; j++)
                    {
                        string tempStr = string.Empty;
                        tempStr += dgv.Rows[j].Cells[2].Value + "\r\n";  //导出第二列
                        sw.Write(tempStr);
                    }
                    sw.Close();
                    myStream.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
                finally
                {
                    sw.Close();
                    myStream.Close();
                }
            }
        }

        #endregion

        //非常重要获取当前存在浏览器的cookie，可以登陆wbbbrowser更新cookie。

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref System.UInt32 pcchCookieData, int dwFlags, IntPtr lpReserved);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int InternetSetCookieEx(string lpszURL, string lpszCookieName, string lpszCookieData, int dwFlags, IntPtr dwReserved);


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


        #region 写入与读取Text文本
        /// <summary>
        /// 写入与读取Text文本
        /// </summary>
        /// <param name="text"></param>
        public static void WrightText(string text)
        {
            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

            if (!File.Exists(path + "\\mac.txt") && text != "")

            {
                FileStream fs1 = new FileStream(path + "//mac.txt", FileMode.Create, FileAccess.Write);//在当前程序运行文件夹内创建文件 
                StreamWriter sw = new StreamWriter(fs1);
                sw.WriteLine(text.Trim());//开始写入值
                sw.Close();
                fs1.Close();
            }



        }
        public void ReadText()
        {

            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
            StreamReader sr = new StreamReader(path + "\\username.txt");
            //一次性读取完 
            string Password = sr.ReadToEnd();

            



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
            catch
            {
                return "unknown";
            }
        }

        #endregion

        #region  定时器

        public  void timer()
        {
            System.Timers.Timer aTimer = new System.Timers.Timer(); //实例化Timer类  

            aTimer.Elapsed += new ElapsedEventHandler(timerevent);    //到时间的时候执行事件  需要using System.Timers; 

            aTimer.Interval = 5000;

            aTimer.AutoReset = false;    //执行一次 false，一直执行true  

            aTimer.Enabled = true;      //是否执行System.Timers.Timer.Elapsed事件
        }

        private void timerevent(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {


                string dns = Dns.GetHostName();
                string constr = "Host =116.62.62.62;Database=datas;Username=root;Password=zhoukaige";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select * from gonggao ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                while (reader.Read())
                {


                    string gz = reader["gonggao"].ToString();

                    MessageBox.Show(gz);


                }



            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
                ex.ToString();
                


            }
            return "";
        }
        #endregion

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlwithCookie(string Url,string cookie)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";

                request.Headers.Add("Cookie", cookie);
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

        #region 获取百度citycode
        public int getcityId(string cityName)
        {
            try

            {

                String Url = "https://map.baidu.com/?newmap=1&reqflag=pcmap&biz=1&from=webmap&da_par=direct&pcevaname=pc4.1&qt=s&da_src=searchBox.button&wd=" + cityName + "&c=289&src=0&wd2=&pn=0&sug=0&l=12&b=(13461858.87,3636969.979999999;13584738.87,3670185.979999999)&from=webmap&biz_forward={%22scaler%22:1,%22styles%22:%22pl%22}&sug_forward=&tn=B_NORMAL_MAP&nn=0&u_loc=13166533,3998088&ie=utf-8";

                string html = Method.GetUrl(Url);


                MatchCollection Matchs = Regex.Matches(html, @"""code"":([\s\S]*?),", RegexOptions.IgnoreCase);




                int cityId = Convert.ToInt32(Matchs[0].Groups[1].Value);
                return cityId;

            }
            catch (System.Exception ex)
            {
                ex.ToString();
                return 1;
            }




        }
        #endregion

        

        #region 注册码随机生成函数
        /// <summary>
        /// 注册码随机生成函数
        /// </summary>
        /// <returns></returns>
        public static string RandomKey()

        {
            //string[] array = {"A","B","C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};
            string Hour = DateTime.Now.Hour.ToString();                    //获取当前小时
            string Hour2 = Math.Pow(Convert.ToDouble(Hour),2).ToString();  //获取当前小时的平方
            //string zimu = array[DateTime.Now.Hour];                        //获取当前小时作为数组索引的字母
            string key = Hour + "1475" +(Hour2) + "2479" + Hour;
            
            return key;
            
           
            
        }


        #endregion

        #region 输入地址获取经纬度
        public string getPoi(string address)
        {

            try

            {
               
                string html = GetUrl("http://apis.map.qq.com/jsapi?qt=poi&wd=" + address);
                string Rxg1 = @"pinfo([\s\S]*?)geo";
                
                Match point = Regex.Match(html, Rxg1);
                           
                    string x = point.Groups[1].Value;
                    string y = point.Groups[3].Value;

                return "latitude="+y+"&longitude="+x;
                
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
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

        #region 通过datatable 导入数据到数据库

            public static void importtodatabase(DataGridView dgv)
        {

            try
            {



                string constr = "Host =116.62.62.62;Database=datas;Username=root;Password=zhoukaige";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();
                for (int i = 0; i < dgv.RowCount - 1; i++)   //dgv.RowCount： 行数   RowColumn ：列数
                {
                    
                        string name = dgv.Rows[i].Cells[0].Value.ToString().Trim();
                        string tell = dgv.Rows[i].Cells[1].Value.ToString().Trim();
                        string address = dgv.Rows[i].Cells[2].Value.ToString().Trim();
                        string area = dgv.Rows[i].Cells[3].Value.ToString().Trim();

                        string sql = "INSERT INTO meituan_datas (meituan_name,meituan_tell,meituan_address,meituan_area)VALUES('" + name + " ', '" + tell + " ', '" + address + " ','" + area + " ')";
                        MySqlCommand cmd = new MySqlCommand(sql, mycon);

                        cmd.ExecuteNonQuery();  //执行sql语句


                        //MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                        //reader.Read();
                    

                }
                mycon.Close();


            }

            catch (System.Exception ex)
            {
                ex.ToString();
            }

        }


        #endregion

        #region 一条一条导入数据库
        /// <summary>
        /// 一条一条导入数据库
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="index"></param>

        public static void EachToData(DataGridView dgv,int index)
        {

            try
            {



                string constr = "Host =116.62.62.62;Database=datas;Username=root;Password=zhoukaige";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();
                

                    string name = dgv.Rows[index].Cells[1].Value.ToString().Trim();
                    string tell = dgv.Rows[index].Cells[2].Value.ToString().Trim();
                    string address = dgv.Rows[index].Cells[3].Value.ToString().Trim();
                    string area = dgv.Rows[index].Cells[4].Value.ToString().Trim();

                    string sql = "INSERT INTO meituan_datas (meituan_name,meituan_tell,meituan_address,meituan_area)VALUES('" + name + " ', '" + tell + " ', '" + address + " ','" + area + " ')";
                    MySqlCommand cmd = new MySqlCommand(sql, mycon);

                    cmd.ExecuteNonQuery();  //执行sql语句
                
                
                mycon.Close();


            }

            catch (System.Exception ex)
            {
                ex.ToString();
            }

        }


        #endregion

        #region listview转datatable
        public static DataTable ListViewToDataTable(ListView lv)
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Clear();
            dt.Columns.Clear();
            for (int k = 0; k < lv.Columns.Count; k++)
            {
                dt.Columns.Add(lv.Columns[k].Text.Trim().ToString());//生成DataTable列头
            }
            for (int i = 0; i < lv.Items.Count; i++)
            {
                dr = dt.NewRow();
                for (int j = 0; j < lv.Columns.Count; j++)
                {
                    dr[j] = lv.Items[i].SubItems[j].Text.Trim();
                }
                dt.Rows.Add(dr);//每行内容
            }

            return dt;
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

        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url,string postData,string COOKIE,string charset)
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
        string html =sr.ReadToEnd();

            sw.Dispose();
            sw.Close();
            sr.Dispose();
            sr.Close();
            s.Dispose();
            s.Close();
            return html;
        }

        #endregion

        #region 读取xml文件


        #endregion


        //打开对话框，需要先拖Dialog对话框，然后获取文件
        //var result = openFileDialog1.ShowDialog();
        //    if (result == DialogResult.OK)
        //    {
        //        txtSourceFile.Text = openFileDialog1.FileName;
        //    }

        #region 读取excel值,返回ds
        /// <summary>
        /// 读取excel值,返回ds
        /// </summary>
        /// <returns></returns>

        //打开文件
        public static DataSet getData() { 
                    OpenFileDialog file = new OpenFileDialog();
                    file.Filter = "Excel(*.xlsx)|*.xlsx|Excel(*.xls)|*.xls";
                file.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                file.Multiselect = false;
                if (file.ShowDialog() == DialogResult.Cancel) 
                    return null;
                //判断文件后缀
                var path = file.FileName;
                string fileSuffix = System.IO.Path.GetExtension(path);
                if (string.IsNullOrEmpty(fileSuffix)) 
                    return null;
                using (DataSet ds = new DataSet())
                {
                    //判断Excel文件是2003版本还是2007版本
                    string connString = "";
                    if (fileSuffix == ".xls")
                        connString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + path + ";" + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
                    else
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + path + ";" + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";
                    //读取文件
                    string sql_select = " SELECT * FROM [Sheet1$]";
                    using (OleDbConnection conn = new OleDbConnection(connString))
                    using (OleDbDataAdapter cmd = new OleDbDataAdapter(sql_select, conn))
                    {
                        conn.Open();
                        cmd.Fill(ds);
                    }
                    if (ds == null || ds.Tables.Count <= 0) return null;
                    return ds;
            }
    }
        #endregion

        #region  创建sqlite数据库
        /// <summary>
        /// 创建sqlite数据库
        /// </summary>
        public static void Sqlite()

        {
            string time = DateTime.Now.ToString("yyyy-MM-dd");
            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
            DirectoryInfo dinfo = new DirectoryInfo(path + "\\data");
            if (!dinfo.Exists)
            {

                dinfo.Create();

            }
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + path + "\\data\\" + time + ".db3");
            conn.Open();
            string sql = "create table meituan (id INTEGER  primary key AUTOINCREMENT,meituan_name VarChar(30) NOT NULL,meituan_tell VarChar(30) NOT NULL,meituan_addr VarChar(30),meituan_area VarChar(10))";

            SQLiteCommand com = new SQLiteCommand(sql);
            com.Connection = conn;
            com.ExecuteNonQuery();

        }

        #endregion


        #region  去除固话

        public static void RemoveTell(DataGridView dgv)

        {

            try
            {
                for (int i = 0; i <= dgv.Rows.Count; i++)
                {
                    string Lpv = dgv.Rows[i].Cells[1].Value.ToString();

                    if (Lpv.Contains("-")&& !Lpv.Contains("/"))                //Contains()包含, indexof()返回字符在字符串中首次出现的位置，若没有返回 -1
                    {
                        dgv.Rows.RemoveAt(i);
                    }
                }
            }

            catch (System.Exception ex)
            {
                ex.ToString();
            }
        }

        #endregion

        #region 导入数据到SQLite
        /// <summary>
        /// 导入数据到SQLite数据库
        /// </summary>
        /// <param name="dgv"></param>

        public static void DataToSqlite(DataGridView dgv)

        {
            try
            {
                string time = DateTime.Now.ToString("yyyy-MM-dd");
                string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹



                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\data\\" + time + ".db3");
                mycon.Open();
                for (int i = 0; i < dgv.RowCount - 1; i++)   //dgv.RowCount： 行数   RowColumn ：列数
                {

                    string name = dgv.Rows[i].Cells[0].Value.ToString().Trim();
                    string tell = dgv.Rows[i].Cells[1].Value.ToString().Trim();
                    string address = dgv.Rows[i].Cells[2].Value.ToString().Trim();
                    string area = dgv.Rows[i].Cells[3].Value.ToString().Trim();

                    string sql = "INSERT INTO meituan (meituan_name,meituan_tell,meituan_addr,meituan_area)VALUES('" + name + " ', '" + tell + " ', '" + address + " ','" + area + " ')";
                    SQLiteCommand com = new SQLiteCommand(sql);
                    com.Connection = mycon;
                    com.ExecuteNonQuery();



                }
                mycon.Close();


            }

            catch (System.Exception ex)
            {
                ex.ToString();
            }

        }
        #endregion

        #region  读取SQLite当天的数据库

        public static void ReadSqlite(DataGridView dgv)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd");
            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

            if (File.Exists(path + "\\data\\" + time + ".db3"))
            {



                SQLiteConnection connection = new SQLiteConnection("Data Source=" + path + "\\data\\" + time + ".db3");
                connection.Open();
                string sql = "select * from meituan";
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, connection);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dgv.DataSource = ds.Tables[0];



            }

            else
            {
                MessageBox.Show("没有");
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


        #region   dataGridView导出CSV带进度条，导出分列
        /// <summary>
        /// dataGridView导出CSV带进度条，导出结果分列
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="progressbar"></param>
        public static void csv(DataGridView dgv,ProgressBar progressbar)
        {

            if (dgv.Rows.Count == 0)
            {
                MessageBox.Show("No data available!", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                saveFileDialog.FilterIndex = 0;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.CreatePrompt = true;
                saveFileDialog.FileName = null;
                saveFileDialog.Title = "Save path of the file to be exported";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Stream myStream = saveFileDialog.OpenFile();
                    StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));
                    string strLine = "";
                    try
                    {
                        //Write in the headers of the columns.
                        for (int i = 0; i < dgv.ColumnCount; i++)
                        {
                            if (i > 0)
                                strLine += ",";
                            strLine += dgv.Columns[i].HeaderText;
                        }
                        strLine.Remove(strLine.Length - 1);
                        sw.WriteLine(strLine);
                        strLine = "";
                        //Write in the content of the columns.
                        for (int j = 0; j < dgv.Rows.Count; j++)
                        {
                            strLine = "";
                            for (int k = 0; k < dgv.Columns.Count; k++)
                            {
                                if (k > 0)
                                    strLine += ",";
                                if (dgv.Rows[j].Cells[k].Value == null)
                                    strLine += "";
                                else
                                {
                                    string m = dgv.Rows[j].Cells[k].Value.ToString().Trim();
                                    strLine += m.Replace(",", "，");
                                }
                            }
                            strLine.Remove(strLine.Length - 1);
                            sw.WriteLine(strLine);
                            //Update the Progess Bar.
                            progressbar.Value = 100 * (j + 1) / dgv.Rows.Count;
                        }
                        sw.Close();
                        myStream.Close();
                        MessageBox.Show("Data has been exported to：" + saveFileDialog.FileName.ToString(), "Exporting Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        progressbar.Value = 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Exporting Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            
        }
        #endregion

        #region 下载文件
        /// <summary>
        /// 下载单个文件
        /// </summary>
        /// <param name="URLAddress">文件地址</param>
        public void downloadFile(string URLAddress)
        {
            WebClient client = new WebClient();
        
            string receivePath = @"D:\";

            client.DownloadFile(URLAddress, receivePath + Path.GetFileName(URLAddress));
        }

        #endregion

        #region 通过注册表记住账号密码
        public static void writeKey(string setKey1,string setKey2)
        {
            RegistryKey regkey = Registry.CurrentUser.CreateSubKey("cc");//"cc"要创建一个特殊的字符串，防止与系统或其他程序写注册表名称相同，


            regkey.SetValue("UserName", setKey1);


            regkey.SetValue("PassWord", setKey2);

            // …………多个value值………………

            regkey.Close();

        }

        public static string[] readKey()
        {
               RegistryKey regkey = Registry.CurrentUser.OpenSubKey("cc"); //打开cc注册表

            //  Registry.CurrentUser.DeleteSubKey("cc"); //删除cc注册表


            if (regkey != null)

            {
                string getKey1 = regkey.GetValue("UserName").ToString();
                string getKey2 = regkey.GetValue("PassWord").ToString();
                regkey.Close();
                string[] key = { getKey1, getKey2 };

                return key;

            }

            else
            {
                return null;
            }
        }


        public static void clearkey()
        {

            Registry.CurrentUser.DeleteSubKey("cc"); //删除cc注册表
        }

        #endregion




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






        //<a\b[^>]+\bhref="([^"]*)"[^>]*>([\s\S]*?)</a>

        //string temp = Regex.Replace(match.Groups[1].Value, "<[^>]*>", "");   //去除所有标签去除所有标签
    }

}
