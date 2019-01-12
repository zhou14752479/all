using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Method;
using ClassLibrary1;
using System.IO;
using System.Data.SQLite;
using System.Data;

namespace MeiT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 城市拼音转换
        /// </summary>
        /// <param name="suoxie"></param>
        /// <returns></returns>
        public string getQuanpin(string suoxie)
        {
            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
            SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\area\\city.db");
            mycon.Open();
            string sql = "select quanpin from citys where suoxie ='" + suoxie + "' ";
            SQLiteCommand cmd = new SQLiteCommand(sql, mycon);

            SQLiteDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {

                string quanpin = reader["quanpin"].ToString().Trim();
                return quanpin;
            }

            else
            {
                return "";
            }

                 
        }

        public string areas { get; set; }

        /// <summary>
        /// 获取城市下地区
        /// </summary>
        /// <param name="suoxie"></param>
        /// <returns></returns>

        public string getArea(string citySuoxie)
        {
            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
            SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\area\\city.db");
            mycon.Open();
            string sql = "select * from area where citySuoxie ='" + citySuoxie + "' ";

            SQLiteDataAdapter da = new SQLiteDataAdapter(sql, mycon);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                this.areas += dr[0].ToString().Trim() + ",";
            }

            return this.areas;
        }






            #region 主方法
            /// <summary>
            /// 
            /// </summary>
            public void run()
        {
           
            string[] citys = textBox1.Text.Trim().Split(new string[] { "," }, StringSplitOptions.None);
            
            int page = 32;

           

            foreach (string city in citys)

            {
                string city_areas = getArea(getQuanpin(city));

                string[] areas = city_areas.Split(new string[] { "," }, StringSplitOptions.None);

                foreach(string city_area in areas)
                { 

                for (int i = 1; i <= page; i++)
                {
                        String Url = "http://"+city+".meituan.com/meishi/b"+ city_area + "/pn"+i+"/";

                      
                    string html = method.meituan_GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()

                    MatchCollection TitleMatchs = Regex.Matches(html, @"{""poiId"":([\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    ArrayList lists = new ArrayList();
                    foreach (Match NextMatch in TitleMatchs)
                    {

                        lists.Add("http://www.meituan.com/meishi/" + NextMatch.Groups[1].Value + "/");

                    }

                    if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;

                    Application.DoEvents();
                    Thread.Sleep(1001);

                    foreach (string list in lists)
                    {


                        

                        String Url1 = list;
                        string strhtml = method.meituan_GetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()

                        string a1 = @"{""title"":""([\s\S]*?)""";

                        string Rxg = @"{""poiId"":([\s\S]*?),""name"":""([\s\S]*?)"",""avgScore"":([\s\S]*?),""address"":""([\s\S]*?)"",""phone"":""([\s\S]*?)"",";
                      
                        string Rxg1 = @"{""id"":""([\s\S]*?)"",""name"":""([\s\S]*?)"",""price"":([\s\S]*?),";   //菜品

                        string rxg = @"longitude"":([\s\S]*?),""latitude"":([\s\S]*?),""avgPrice"":([\s\S]*?),""brandId"":([\s\S]*?),""brandName"":""([\s\S]*?)""";

                        string Rxg2 = @"""address"":""([\s\S]*?)区";

                        MatchCollection area = Regex.Matches(strhtml, a1);
                        Match name = Regex.Match(strhtml, Rxg);
                        MatchCollection cai = Regex.Matches(strhtml, Rxg1);
                        Match type = Regex.Match(strhtml, rxg);

                        Match area1 = Regex.Match(strhtml, Rxg2);

                            if (area.Count > 0)
                            {
                                string cityName = area[0].Groups[1].Value.Replace("美团", "");
                                string types = area[2].Groups[1].Value.Replace(cityName, "");

                                string poiId = name.Groups[1].Value;
                                string Name = name.Groups[2].Value.Replace("'","");
                                string avgScore = name.Groups[3].Value;
                                string address = name.Groups[4].Value.Replace("'", "");
                                string phone = name.Groups[5].Value;

                                string longtitude = type.Groups[1].Value;
                                string latitude = type.Groups[2].Value;
                                string avgPrice = type.Groups[3].Value;
                                string brandId = type.Groups[4].Value;
                                string brandName = type.Groups[5].Value;
                                DateTime dt = DateTime.Now;
                                string time = dt.ToShortDateString().ToString();
                                string area_1 = area1.Groups[1].Value;
                                if (area_1.Length > 10)
                                {
                                    area_1 = cityName;
                                }


                                string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹



                                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\data\\meituan.db");
                                mycon.Open();


                                string sql = "INSERT INTO m_infos (city,area,id,averPrice,averScore,commentNums,title,addr,time)VALUES('" + cityName + " ','" + area_1 + "', '" + poiId + " ', '" + avgPrice + " ','" + avgScore + " ','1','" + Name + " ','" + address + " ','" + time + " ');" +
                                    "INSERT INTO m_type (city,area,id,type,phone,latitude,lontitude,brandId,brandName,time)VALUES('" + cityName + " ','" + area_1 + "', '" + poiId + " ','" + types + " ', '" + phone + " ','" + latitude + " ','" + longtitude + " ','" + brandId + " ','" + brandName + " ','" + time + " ')";
                                SQLiteCommand cmd = new SQLiteCommand(sql, mycon);

                                cmd.ExecuteNonQuery();  //执行sql语句

                                label2.Text = "正在采集" + list + "";
                                mycon.Close();



                                foreach (Match match in cai)
                                {
                                    //int index = this.dataGridView1.Rows.Add();
                                    //this.dataGridView1.Rows[index].Cells[0].Value = index;
                                    //this.dataGridView1.Rows[index].Cells[1].Value = match.Groups[2].Value;
                                    //this.dataGridView1.Rows[index].Cells[2].Value = match.Groups[3].Value;

                                    //this.dataGridView1.CurrentCell = this.dataGridView1.Rows[index].Cells[0];  //让datagridview滚动到当前行

                                    SQLiteConnection mycon2 = new SQLiteConnection("Data Source=" + path + "\\data\\meituan.db");
                                    mycon2.Open();


                                    string sql2 = "INSERT INTO m_dishes (city,area,id,dishName,dishPrice,time)VALUES('" + cityName + " ','" + area_1 + "', '" + poiId + " ', '" + match.Groups[2].Value.Replace("'", "") + " ', '" + match.Groups[3].Value + " ','" + time + " ')";

                                    SQLiteCommand cmd2 = new SQLiteCommand(sql2, mycon2);

                                    cmd2.ExecuteNonQuery();  //执行sql语句
                                    mycon2.Close();
                                }

                                int dealsStart = strhtml.IndexOf("\"dealList\"") + 1;
                                int dealsEnd = strhtml.IndexOf("\"vouchers\"");



                                string dealsHtml = strhtml.Substring(dealsStart, dealsEnd - dealsStart + 2);

                                int voucherStart = strhtml.IndexOf("\"vouchers\"") + 1;
                                int voucherEnd = strhtml.IndexOf("</html>");

                                string vouchersHtml = strhtml.Substring(voucherStart, voucherEnd - voucherStart + 2);


                                string taocan = @"g"",""title"":""([\s\S]*?)"",""soldNum"":([\s\S]*?),""price"":([\s\S]*?),""value"":([\s\S]*?)}";

                                MatchCollection deals = Regex.Matches(dealsHtml, taocan);
                                MatchCollection vouchers = Regex.Matches(vouchersHtml, taocan);


                                foreach (Match match in deals)
                                {
                                    //int index = this.dataGridView2.Rows.Add();
                                    //this.dataGridView2.Rows[index].Cells[0].Value = index;
                                    //this.dataGridView2.Rows[index].Cells[1].Value = match.Groups[1].Value;
                                    //this.dataGridView2.Rows[index].Cells[2].Value = match.Groups[2].Value;
                                    //this.dataGridView2.Rows[index].Cells[3].Value = match.Groups[3].Value;
                                    //this.dataGridView2.Rows[index].Cells[4].Value = match.Groups[4].Value;
                                    //this.dataGridView2.CurrentCell = this.dataGridView2.Rows[index].Cells[0];  //让datagridview滚动到当前行



                                    SQLiteConnection mycon3 = new SQLiteConnection("Data Source=" + path + "\\data\\meituan.db");
                                    mycon3.Open();


                                    string sql3 = "INSERT INTO m_cash (city,area,id,name,price,primePrice,nums,time)VALUES( '" + cityName + " ','" + area_1 + "','" + poiId + " ', '" + match.Groups[1].Value.Replace("'", "") + " ', '" + match.Groups[3].Value + " ','" + match.Groups[4].Value + " ', '" + match.Groups[2].Value + " ','" + time + " ')";

                                    SQLiteCommand cmd3 = new SQLiteCommand(sql3, mycon3);

                                    cmd3.ExecuteNonQuery();  //执行sql语句
                                    mycon3.Close();

                                }

                                foreach (Match match in vouchers)
                                {


                                    SQLiteConnection mycon4 = new SQLiteConnection("Data Source=" + path + "\\data\\meituan.db");
                                    mycon4.Open();


                                    string sql3 = "INSERT INTO m_discount (city,area,id,name,price,primePrice,nums,time)VALUES( '" + cityName + " ','" + area_1 + "','" + poiId + " ', '" + match.Groups[1].Value.Replace("'", "") + " ', '" + match.Groups[3].Value + " ','" + match.Groups[4].Value + " ', '" + match.Groups[2].Value + " ','" + time + " ')";

                                    SQLiteCommand cmd4 = new SQLiteCommand(sql3, mycon4);

                                    cmd4.ExecuteNonQuery();  //执行sql语句
                                    mycon4.Close();

                                }




                                if (button4.Text == "已停止")
                                {
                                    return;
                                }




                                Application.DoEvents();
                                System.Threading.Thread.Sleep(2000);
                            }
                    }
                }

             }

            }

        }

        #endregion



        

        #region   dataGridView导出CSV带进度条，导出分列
        public static void csv(DataGridView dgv)
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
                           
                        }
                        sw.Close();
                        myStream.Close();
                        MessageBox.Show("Data has been exported to：" + saveFileDialog.FileName.ToString(), "Exporting Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Exporting Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }

        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            button4.Text = "停止";
            Thread search_thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            search_thread.Start();

        }

   
        private void button4_Click(object sender, EventArgs e)
        {
            button4.Text = "已停止";
        }

    }
}
