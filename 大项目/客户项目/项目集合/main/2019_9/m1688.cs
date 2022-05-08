using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main._2019_9
{
    public partial class m1688 : Form
    {
        public m1688()
        {
            InitializeComponent();
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
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
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public bool ExistINIFile()
        {
            return File.Exists(inipath);
        }


        #region NPOI读取表格导入DataTable
        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public static DataTable ExcelToDataTable(string fileName, bool isFirstRowColumn)
        {
            string sheetName = "Sheet1";
            IWorkbook workbook = null;
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);

                if (sheetName != null)
                {
                    // sheet = workbook.GetSheet(sheetName); //根据sheet名称
                    sheet = workbook.GetSheetAt(0);//根据sheet序号
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                  
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue+i;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }


        #endregion


        OpenFileDialog Ofile = new OpenFileDialog();

      
        DataSet ds = new DataSet();
        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();


            this.ds.Tables.Clear();
            this.Ofile.FileName = "";
            this.dataGridView1.DataSource = "";
            this.Ofile.ShowDialog();
            string fileName = this.Ofile.FileName;
            textBox1.Text = fileName;

            if (fileName.Trim().ToUpper().EndsWith("XLS")|| fileName.Trim().ToUpper().EndsWith("XLSX"))//判断所要的?展名?型；
            {
              
                dataGridView1.DataSource = ExcelToDataTable(fileName,true);
            }

            if (fileName.Trim().ToUpper().EndsWith("CSV"))//判断所要的?展名?型；
            {
                int ipos = fileName.LastIndexOf("\\");
                string filePath = fileName.Substring(0, ipos + 1);
                string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='text;HDR=YES;FMT=Delimited;'";//有列?的
                string commandText = "select * from " + fileName.Replace(filePath, "");//SQL?句；
                OleDbConnection olconn = new OleDbConnection(connStr);
                olconn.Open();
                OleDbDataAdapter odp = new OleDbDataAdapter(commandText, olconn);
                DataTable dt = new DataTable();
             
              
                odp.Fill(dt);
                dataGridView1.AutoGenerateColumns = true;//有列?的
                dataGridView1.DataSource = dt.DefaultView;//有列?的

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {

                    dataGridView1.Columns[i].HeaderText = dataGridView1.Columns[i].HeaderText + i;
                }

            }


        }
        bool zanting = true;
        #region  1688
        public void run1688()
        {
            try
            {



                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    string title = dataGridView1.Rows[i].Cells[Convert.ToInt32(textBox4.Text.Trim())].Value.ToString();
                    string  taobao = dataGridView1.Rows[i].Cells[Convert.ToInt32(textBox5.Text.Trim())].Value.ToString();


                    string url = "https://m.1688.com/offer_search/-6D7033.html?sortType=booked&filtId=&keywords="+System.Web.HttpUtility.UrlEncode(title)+ "&descendOrder=true";

                    string html = method.GetUrl(url,"utf-8");

                    //MatchCollection aids = Regex.Matches(html, @"data-offer-id=""([\s\S]*?)""");
                    //MatchCollection aids = Regex.Matches(html, @"<span><font color=red>([\s\S]*?)</font>");
                    Match geshu = Regex.Match(html, @"<b id='counter-number'>([\s\S]*?)</b>");
                    if (geshu.Groups[1].Value == "")
                        continue;
                    if (Convert.ToInt32(geshu.Groups[1].Value) <= Convert.ToInt32(textBox2.Text))
                    {
                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());

                        listViewItem.SubItems.Add(title);
                        listViewItem.SubItems.Add(taobao);
                        listViewItem.SubItems.Add(geshu.Groups[1].Value);
                        listViewItem.SubItems.Add("符合条件");
                    }
                    else
                    {
                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());

                        listViewItem.SubItems.Add(title);
                        listViewItem.SubItems.Add(taobao);
                        listViewItem.SubItems.Add(geshu.Groups[1].Value);
                        listViewItem.SubItems.Add("不符合");
                    }
            
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    Thread.Sleep(Convert.ToInt32(textBox3.Text));

                }

                MessageBox.Show("采集完成");

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }





        #endregion

        #region  京东
        public void jd()
        {
            //if (textBox4.Text != "")
            //{
               
            //}


            try
            {




                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    string title = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    string taobao = dataGridView1.Rows[i].Cells[0].Value.ToString();


                    string url = "https://search.jd.com/Search?keyword="+System.Web.HttpUtility.UrlEncode(title)+ "&enc=utf-8&qrst=1&rt=1&stop=1&vt=2&psort=3&click=0";

                    string html = method.GetUrl(url, "utf-8");
                    if (html.Contains("没有找到"))
                    {
                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());

                        listViewItem.SubItems.Add(title);
                        listViewItem.SubItems.Add(taobao);
                        listViewItem.SubItems.Add("0");
                        listViewItem.SubItems.Add("不符合");
                    }
                    else
                    {
                        Match geshu = Regex.Match(html, @"result_count:'([\s\S]*?)'");
                        if (geshu.Groups[1].Value == "")
                            continue;
                        if (Convert.ToInt32(geshu.Groups[1].Value) <= Convert.ToInt32(textBox2.Text))
                        {
                            ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());

                            listViewItem.SubItems.Add(title);
                            listViewItem.SubItems.Add(taobao);
                            listViewItem.SubItems.Add(geshu.Groups[1].Value);
                            listViewItem.SubItems.Add("符合条件");
                        }
                        else
                        {
                            ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());

                            listViewItem.SubItems.Add(title);
                            listViewItem.SubItems.Add(taobao);
                            listViewItem.SubItems.Add(geshu.Groups[1].Value);
                            listViewItem.SubItems.Add("不符合");
                        }

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                        Thread.Sleep(Convert.ToInt32(textBox3.Text));
                    }
                }

                MessageBox.Show("采集完成");

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }





        #endregion

        private void m1688_Load(object sender, EventArgs e)
        {
            //读取config.ini
            if (ExistINIFile())
            {
                textBox1.Text = IniReadValue("values", "a1");
                textBox2.Text = IniReadValue("values", "a2");
                textBox3.Text = IniReadValue("values", "a3");
                textBox4.Text = IniReadValue("values", "a4");
                textBox5.Text = IniReadValue("values", "a5");

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //写入config.ini配置文件
            IniWriteValue("values", "a1", textBox1.Text.Trim());
            IniWriteValue("values", "a2", textBox2.Text.Trim());
            IniWriteValue("values", "a3", textBox3.Text.Trim());
            IniWriteValue("values", "a4", textBox4.Text.Trim());
            IniWriteValue("values", "a5", textBox5.Text.Trim());


            button1.Enabled = false;

            Thread thread = new Thread(new ThreadStart(run1688));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            zanting = true;
            button1.Enabled = true;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel2(method.listViewToDataTable(this.listView1), "Sheet1", true, Path.GetFileNameWithoutExtension(textBox1.Text));
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Process.Start(this.listView1.SelectedItems[0].SubItems[2].Text);
        }

        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void M1688_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel=true;
            }
        }

        private void 复制标题ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(this.listView1.SelectedItems[0].SubItems[1].Text);
        }

    
    }
}
