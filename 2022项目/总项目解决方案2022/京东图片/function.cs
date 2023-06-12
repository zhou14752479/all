using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace 京东图片
{
    class function
    {
        string path = AppDomain.CurrentDomain.BaseDirectory;

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
                                                                                              // string COOKIE = "sid=64a4a095f95b472f56b387f81def9f98;USER_FLAG_CHECK=9416a352d1e261a30bc190a337c2a96f;appkey=0a0834aee67bcee63b8a39435300636d;appid=wx862d8e26109609cb;mpChannelId=388635;openid=odI0R5S8UPqCxO5UtR6u6V76SfKQ;wxclient=gxhwx;ie_ai=1;";
                string COOKIE = "3AB9D23F7A4B3C9B=A62ZFW2QMC72IHRF7C7T2KQNKJ7OBXXTO4GQVKXSQOKXBEA42YTNCR7QKTNNUCP77UK557M2VX6TJPO43HXMNCBPNY; 3AB9D23F7A4B3CSS=jdd03A62ZFW2QMC72IHRF7C7T2KQNKJ7OBXXTO4GQVKXSQOKXBEA42YTNCR7QKTNNUCP77UK557M2VX6TJPO43HXMNCBPNYAAAAMIP4QH5XAAAAAADASOSYSCCICYYMX; __jd_ref_cls=MList_Product_Expose; _gia_d=1; mba_muid=1685759975671685082369; mba_sid=16857599773138024028091157017.4; shshshfpa=307d9ca5-c232-23f5-75de-050635eabcdc-1685759985; shshshfpb=tbsR5zcaz8WQHC7uy-eVXNw; PPRD_P=UUID.1685759975671685082369; __jda=123.1685759975671685082369.1685759975.1685759975.1685759977.2; __jdb=123.4.1685759975671685082369|2.1685759977; __jdc=123; __wga=1685760015961.1685759984852.1685759984852.1685759984852.3.1; cid=9; retina=1; wqmnx1=MDEyNjM1MnQvLm9yYWFudyU5RSU2QmFyZSYxMTk5NTh6LyhuUGhPX2tjWHBiNi5LLGVrZW41aTUgcjQ1ZjduMjQyWU9PVSFIJQ%3D%3D; wxa_level=1; cd_eid=jdd03A62ZFW2QMC72IHRF7C7T2KQNKJ7OBXXTO4GQVKXSQOKXBEA42YTNCR7QKTNNUCP77UK557M2VX6TJPO43HXMNCBPNYAAAAMIP4QALOQAAAAACJT66YSL3I2QFEX; shshshfpx=307d9ca5-c232-23f5-75de-050635eabcdc-1685759985; sc_width=390; autoOpenApp_downCloseDate_auto=1685759984936_1800000; sbx_hot_h=null; __jdv=123%7Cm.baidu.com%7Ct_1003608409_%7Ctuiguang%7C0ed7c96d4d264aaa8f4fd5be0e1f22f7%7C1685759977311; appCode=ms0ca95114; jxsid=16857599770354589100; visitkey=8972279493350123880; webp=1; autoOpenApp_downCloseDate_jd_homePage=1685759977761_1; unpl=JF8EALJnNSttWR8BA0kKHEcVHl5TWw1YGx5UajBQUA4MSgAFTAEYRRZ7XlVdXxRKEB9uZRRUXFNJXA4ZBisSEXtdVV9fD0oeBm5vNWRcNkg6dWBkbhFxKBgyVi44SBczblcFU1RcTVANEwQfFRZLWl1aXAFIFQRnVwRkXVBPZDUrBxsTEUpVU19aCXsWM21mBFFZWUxVBhwyUHwRBl1TV1kOTx8LaWMCUl1fQlAEEgEZFRh7XGRd";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                //request.Referer = "https://servicewechat.com/wx91d27dbf599dff74/534/page-frame.html";
                request.Referer = "https://so.m.jd.com/";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                //headers.Add("sec-fetch-site:same-origin");
                //headers.Add("sec-fetch-user:?1");
                //headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                // request.KeepAlive = true;
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
        public void getcates1(ComboBox cob)
        {
            StreamReader sr = new StreamReader(path + "//cates.txt", method.EncodingType.GetTxtType(path + "//cates.txt"));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                string[] values = text[i].Split(new string[] { "," }, StringSplitOptions.None);
                if (values.Length > 2)
                {
                    if (!cob.Items.Contains(values[0]))
                    {
                        cob.Items.Add(values[0]);
                    }

                }

            }
            sr.Close();  //只关闭流
            sr.Dispose();   //

        }

        public void getcates2(ComboBox cob, string cate1)
        {
            StreamReader sr = new StreamReader(path + "//cates.txt", method.EncodingType.GetTxtType(path + "//cates.txt"));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                string[] values = text[i].Split(new string[] { "," }, StringSplitOptions.None);
                if (values.Length > 2)
                {
                    if (!cob.Items.Contains(values[1]) && values[0] == cate1)
                    {
                        cob.Items.Add(values[1]);
                    }

                }

            }
            sr.Close();  //只关闭流
            sr.Dispose();   //

        }


        public void getcates3(ComboBox cob, string cate2)
        {
            StreamReader sr = new StreamReader(path + "//cates.txt", method.EncodingType.GetTxtType(path + "//cates.txt"));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                string[] values = text[i].Split(new string[] { "," }, StringSplitOptions.None);
                if (values.Length > 2)
                {
                    if (!cob.Items.Contains(values[2]) && values[1] == cate2)
                    {
                        cob.Items.Add(values[2]);
                    }

                }

            }
            sr.Close();  //只关闭流
            sr.Dispose();   //

        }
        ///// <summary>
        ///// 查询数据库
        ///// </summary>
        //public DataTable chaxundata(string sql)
        //{
        //    try
        //    {
        //        string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

        //        SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\jddata.db");
        //        mycon.Open();

        //        SQLiteDataAdapter mAdapter = new SQLiteDataAdapter(sql, mycon);
        //        DataTable dt = new DataTable();
        //        mAdapter.Fill(dt);
        //        mycon.Close();
        //        return dt;

        //    }
        //    catch (SQLiteException ex)
        //    {

        //        return null;


        //    }

        //}

        ///// <summary>
        ///// 查询字段
        ///// </summary>
        //public string chaxunvalue(string sql)
        //{
        //    try
        //    {
        //        string value = "";
        //        string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

        //        using (SQLiteConnection con = new SQLiteConnection("Data Source=" + path + "\\jddata.db"))
        //        {
        //            con.Open();
        //            using (SQLiteCommand cmd = new SQLiteCommand())
        //            {
        //                cmd.Connection = con;
        //                cmd.CommandText = string.Format(sql);
        //                using (SQLiteDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
        //                {
        //                    while (dr.Read())
        //                    {
        //                        value = dr["body"].ToString();

        //                    }
        //                }
        //            }
        //            con.Close();
        //        }

        //        return value;

        //    }
        //    catch (SQLiteException ex)
        //    {

        //        return null;


        //    }

        //}

        ///// <summary>
        ///// 插入数据库
        ///// </summary>
        //public bool insertdata(string sql)
        //{
        //    try
        //    {

        //        string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

        //        SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\jddata.db");
        //        mycon.Open();

        //        SQLiteCommand cmd = new SQLiteCommand(sql, mycon);

        //        int status = cmd.ExecuteNonQuery();  //执行sql语句
        //        if (status > 0)
        //        {
        //            return true;

        //        }

        //        mycon.Close();
        //        return false;
        //    }
        //    catch (SQLiteException ex)
        //    {

        //        return false;

        //    }

        //}

       

        public static int DataTableToExcel(DataTable data, string fileName)
        {
            IWorkbook workbook = null;
            //SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "xlsx|*.xls|xlsx|*.xlsx";
            //sfd.Title = "Excel文件导出";
            //bool flag = sfd.ShowDialog() == DialogResult.OK;
            int result;

            bool isColumnWritten = true;
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
                        ISheet sheet = workbook.CreateSheet("Sheet1");
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
                        //MessageBox.Show("数据导出完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
            
           
        
    }
}