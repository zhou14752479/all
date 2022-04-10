using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using myDLL;

namespace 币种余额获取
{
    class datagridview_page
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
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
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public bool ExistINIFile()
        {
            return File.Exists(inipath);
        }
        string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";

        public static void TopDataGridView(DataGridView dataGridView,int index)
        {
            try
            {

                if (index > 0)//如果该行不是第一行
                {
                    DataGridViewRow dgvr = dataGridView.Rows[index];//获取选中行的上一行
                    dataGridView.Rows.RemoveAt(index);//删除原选中行的上一行
                    dataGridView.Rows.Insert(0, dgvr);//将选中行的上一行插入到选中行的后面                       
                    for (int i = 0; i < dataGridView.Rows.Count; i++)//选中移动后的行
                    {
                        if (i != 0)
                        {
                            dataGridView.Rows[i].Selected = false;
                        }
                        else
                        {
                            dataGridView.Rows[i].Selected = true;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            string html = "";
            string COOKIE = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls; //获取不到加上这一条
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
               
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }

                response.Close();
                return html;



            }
            catch (System.Exception ex)
            {
                return ex.ToString();

            }



        }
        #endregion

        /// <summary>
        /// 每页记录数
        /// </summary>
        public int pageSize = 9999;

        /// <summary>
        /// 总记录数
        /// </summary>
        public int recordCount = 0;

        /// <summary>
        /// 总页数
        /// </summary>
        public int pageCount = 0;

        /// <summary>
        /// 当前页
        /// </summary>
        public int currentPage = 0;


        string path = AppDomain.CurrentDomain.BaseDirectory;
        DataTable table = new DataTable();

        /// <summary>
        /// 分页的方法
        /// </summary>
        /// <param name="str"></param>
        public void PageSorter(DataGridView dataGridView1)
        {
            table.Columns.Clear();
            table.Rows.Clear();
            table.Clear();

           
                DataColumn column1 = new DataColumn("test1", Type.GetType("System.String"));
                DataColumn column2 = new DataColumn("test2", Type.GetType("System.String"));
                DataColumn column3 = new DataColumn("test3", Type.GetType("System.String"));
                DataColumn column4 = new DataColumn("test4", Type.GetType("System.String"));
            DataColumn column5 = new DataColumn("test5", Type.GetType("System.String"));
            DataColumn column6 = new DataColumn("test6", Type.GetType("System.String"));
            DataColumn column7 = new DataColumn("test7", Type.GetType("System.String"));
            DataColumn column8 = new DataColumn("test8", Type.GetType("System.String"));
            DataColumn column9 = new DataColumn("test9", Type.GetType("System.String"));

            table.Columns.Add(column1);             //将列添加到table表中
                table.Columns.Add(column2);
                table.Columns.Add(column3);
                table.Columns.Add(column4);
            table.Columns.Add(column5);
            table.Columns.Add(column6);
            table.Columns.Add(column7);
            table.Columns.Add(column8);
            table.Columns.Add(column9);


            StreamReader sr = new StreamReader(path+"//data.txt", method.EncodingType.GetTxtType(path + "//data.txt"));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                string[] value = text[i].Split(new string[] { "#" }, StringSplitOptions.None);
                if (value.Length > 3)
                {
                    DataRow dr = table.NewRow();            //table表创建行
                    dr["test1"] = value[0];
                    dr["test2"] = value[1];
                    dr["test3"] = value[2];
                    dr["test4"] = value[3];
                    dr["test5"] = "";
                    dr["test6"] = "";
                    dr["test7"] = "";
                    dr["test8"] = DateTime.Now.ToShortTimeString();
                    dr["test9"] = "删除";
                    table.Rows.Add(dr);
                }
            }
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存

           
           
            recordCount = table.Rows.Count;     //记录总行数
            pageCount = (recordCount / pageSize);
            if ((recordCount % pageSize) > 0)
            {
                pageCount++;
            }

            //默认第一页
            currentPage = 1;

            LoadPage(dataGridView1);//调用加载数据的方法

        }

      public  string nowpage = "";
        public string allpage = "";
        public DataTable GetDgvToTable(DataGridView dgv)
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
        /// <summary>
        /// LoadPage方法
        /// </summary>
        public void LoadPage(DataGridView dataGridView1)
        {
            if (currentPage < 1) currentPage = 1;
            if (currentPage > pageCount) currentPage = pageCount;

            int beginRecord;    //开始指针
            int endRecord;      //结束指针
            DataTable dtTemp;
            dtTemp = table.Clone();

            beginRecord = pageSize * (currentPage - 1);
            if (currentPage == 1) beginRecord = 0;
            endRecord = pageSize * currentPage;

            if (currentPage == pageCount) endRecord = recordCount;
            for (int i = beginRecord; i < endRecord; i++)
            {
                dtTemp.ImportRow(table.Rows[i]);
            }

            dataGridView1.Rows.Clear();

            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                dataGridView1.Rows.Add(new object[] { dtTemp.Rows[i][0], dtTemp.Rows[i][1], dtTemp.Rows[i][2], dtTemp.Rows[i][3], dtTemp.Rows[i][4], dtTemp.Rows[i][5], dtTemp.Rows[i][6], dtTemp.Rows[i][7], dtTemp.Rows[i][8] });
            }

            nowpage = "当前页: " + currentPage.ToString() + " / " + pageCount.ToString();//当前页
            allpage = "总行数: " + recordCount.ToString() + " 行";//总记录数


        }



        public string getbalance(string bizhong, string address,string apikey)
        {
            try
            {
                string value = "";
                string url = "https://api.etherscan.io/api?module=account&action=balance&address=" + address + "&tag=latest&apikey=" + apikey;
                if (bizhong == "ETH")
                {
                    url = "https://api.etherscan.io/api?module=account&action=balance&address=" + address + "&tag=latest&apikey=" + apikey;
                    string html = GetUrl(url, "utf-8");
                   
                    string balance = Regex.Match(html, @"""result"":""([\s\S]*?)""").Groups[1].Value;
                  decimal a= Convert.ToDecimal(balance)/ 1000000000000000000;
                    value = a.ToString("F4");
                }
                if (bizhong == "USDT")
                {
                    url = "https://api.etherscan.io/api?module=account&action=tokenbalance&contractaddress=0xdac17f958d2ee523a2206206994597c13d831ec7&address=" + address + "&tag=latest&apikey=" + apikey;
                    string html = GetUrl(url, "utf-8");
                   
                    string balance = Regex.Match(html, @"""result"":""([\s\S]*?)""").Groups[1].Value;
                    decimal a = Convert.ToDecimal(balance) / 1000000;
                    value = a.ToString("F2");
                }
                if (bizhong == "USDC")
                {
                    url = "https://api.etherscan.io/api?module=account&action=tokenbalance&contractaddress=0xa0b86991c6218b36c1d19d4a2e9eb0ce3606eb48&address=" + address + "&tag=latest&apikey=" + apikey;
                    string html = GetUrl(url, "utf-8");
                  
                    string balance = Regex.Match(html, @"""result"":""([\s\S]*?)""").Groups[1].Value;
                    decimal a = Convert.ToDecimal(balance) / 1000000;
                    value = a.ToString("F2");
                }

                return value;
            }
            catch (Exception ex)
            {

                return "";
            }
        
        }


       public string senduser = "push@mail-coinbase.us";
        public string sendpass = "ASdf124578";
        public string revieveaddr = "defi_eth1@126.com";
        public string nicheng = "ASdf124578";
        public void sendmsg(string title,string body)
        {
            try
            {
              
                MailMessage mailMsg = new MailMessage();
                mailMsg.From = new MailAddress(senduser, nicheng);
                mailMsg.To.Add(new MailAddress(revieveaddr));
                //mailMsg.CC.Add("抄送人地址");
                //mailMsg.Bcc.Add("密送人地址");
                //可选，设置回信地址 
               // mailMsg.ReplyToList.Add("***");
                // 邮件主题
                mailMsg.Subject = title;
                // 邮件正文内容
                //string text = "欢迎使用阿里云邮件推送";
                string text = title;
                //string html = @"欢迎使用<a href=""https://dm.console.aliyun.com"">邮件推送</a>";
                string html = body;
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
                // 添加附件
                //string file = "D:\\1.txt";
                //Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
                //mailMsg.Attachments.Add(data);
               
                //邮件推送的SMTP地址和端口
                SmtpClient smtpClient = new SmtpClient("smtpdm.aliyun.com", 25);
                //C#官方文档介绍说明不支持隐式TLS方式，即465端口，需要使用25或者80端口(ECS不支持25端口)，另外需增加一行 smtpClient.EnableSsl = true; 故使用SMTP加密方式需要修改如下：
                //SmtpClient smtpClient = new SmtpClient("smtpdm.aliyun.com", 80);
                //smtpClient.EnableSsl = true;
                // 使用SMTP用户名和密码进行验证
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("push@mail-coinbase.us", "ASdf124578");
                smtpClient.Credentials = credentials;
                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        
       
        public List<string> getkey()
        {
            List<string> lists = new List<string>();
               StreamReader sr = new StreamReader(path+"//key.txt", method.EncodingType.GetTxtType(path + "//key.txt"));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {
               if(text[i].Trim()!="")
                {
                    lists.Add(text[i]);
                }
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            return lists;
        }

    }
}
