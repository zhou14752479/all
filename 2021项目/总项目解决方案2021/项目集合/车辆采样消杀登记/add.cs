using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace 车辆采样消杀登记
{
    public partial class add : Form
    {
        public add()
        {
            InitializeComponent();
        }
        function fc = new function();


        #region NPOI读取表格插入数据库大数据高速
        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public void ExcelToDatasql()
        {
            if (textBox9.Text == "")
            {
                MessageBox.Show("请先选择表格文件");
                return;
            }
            string fileName = textBox9.Text;
            string v_supplyer = Path.GetFileNameWithoutExtension(fileName);
            string sheetName = "Sheet1";
            IWorkbook workbook = null;
            ISheet sheet = null;

            int startRow = 0;
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                if (fs == null)
                {
                    MessageBox.Show("");
                }
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
                    int rowCount = sheet.LastRowNum;//最后一列的标号


                    string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
                    using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + path + "\\cardata.db; BinaryGUID=False"))
                    {


                        using (SQLiteCommand insertRngCmd = (SQLiteCommand)conn.CreateCommand())
                        {
                            insertRngCmd.CommandText = @"INSERT INTO datas(ruchang_time,caiji,name,minzu,card,tel,carno,caiyang_time,caiyang_qianzi,xiaosha_time,xiaosha_qianzi) VALUES (@ruchang_time,@caiji,@name,@minzu,@card,@tel,@carno,@caiyang_time,@caiyang_qianzi,@xiaosha_time,@xiaosha_qianzi)";
                            conn.Open();
                            var transaction = conn.BeginTransaction();




                            for (int i = startRow; i <= rowCount; ++i)
                            {

                                IRow row = sheet.GetRow(i);

                                if (row == null) continue; //没有数据的行默认是null
                                try
                                {
                                    string ruchang_time = row.GetCell(1).ToString(); 
                                    string caiji = row.GetCell(2).ToString();
                                    string name = row.GetCell(3).ToString();
                                    string minzu = row.GetCell(4).ToString();
                                    string card = row.GetCell(5).ToString();
                                    string tel = row.GetCell(6).ToString();
                                    string carno = row.GetCell(7).ToString();
                                    string caiyang_time = row.GetCell(8).ToString();
                                    string caiyang_qianzi = row.GetCell(9).ToString();
                                    string xiaosha_time = row.GetCell(10).ToString();
                                    string xiaosha_qianzi = row.GetCell(11).ToString();

                                    insertRngCmd.Parameters.AddWithValue("@ruchang_time", ruchang_time);
                                    insertRngCmd.Parameters.AddWithValue("@caiji", caiji);
                                    insertRngCmd.Parameters.AddWithValue("@name", name);
                                    insertRngCmd.Parameters.AddWithValue("@minzu", minzu);
                                    insertRngCmd.Parameters.AddWithValue("@card", card);
                                    insertRngCmd.Parameters.AddWithValue("@tel", tel);
                                    insertRngCmd.Parameters.AddWithValue("@carno", carno);
                                    insertRngCmd.Parameters.AddWithValue("@caiyang_time", caiyang_time);
                                    insertRngCmd.Parameters.AddWithValue("@caiyang_qianzi", caiyang_qianzi);
                                    insertRngCmd.Parameters.AddWithValue("@xiaosha_time", xiaosha_time);
                                    insertRngCmd.Parameters.AddWithValue("@xiaosha_qianzi", xiaosha_qianzi);

                                    insertRngCmd.ExecuteNonQuery();


                                }
                                catch (Exception)
                                {

                                    continue;
                                }

                            }



                            transaction.Commit();
                        }
                    }


                }

                MessageBox.Show("上传成功");
                button2.Enabled = true;
                log_label.Text = "添加成功";
            }
            catch (Exception ex)
            {

              MessageBox.Show(ex.ToString());
            }
        }


        #endregion


        


        public void inport()
        {
            try
            {
              

                    string ruchang_time = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                    string caiji = textBox1.Text.Trim();
                    string name = textBox2.Text.Trim();
                string minzu = textBox3.Text.Trim();
                string card = textBox4.Text.Trim();
                string tel = textBox5.Text.Trim();
                string carno = textBox6.Text.Trim();
                string caiyang_time = dateTimePicker2.Value.ToString("HH:mm");
                string caiyang_qianzi = textBox7.Text.Trim();
                string xiaosha_time = dateTimePicker3.Value.ToString("HH:mm");
                string xiaosha_qianzi = textBox8.Text.Trim();

                string sql = "INSERT INTO datas(ruchang_time,caiji,name,minzu,card,tel,carno,caiyang_time,caiyang_qianzi,xiaosha_time,xiaosha_qianzi)VALUES("+
                        "'" + ruchang_time + "'," +
                        "'" + caiji + "'," +
                        "'" + name + "'," +
                        "'" + minzu + "'," +
                        "'" + card + "'," +
                        "'" + tel + "'," +
                        "'" + carno + "'," +
                         "'" + caiyang_time + "'," +
                          "'" + caiyang_qianzi + "'," +
                           "'" + xiaosha_time + "'," +
                        "'" + xiaosha_qianzi + "')";
                    fc.insertdata(sql);

                MessageBox.Show("添加成功");
               

            }
            catch (Exception ex)
            {

                log_label.Text = ex.ToString();
            }
        }

        Thread thread;
        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            log_label.Text = "开始导入...";
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(ExcelToDatasql);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            // openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
               textBox9.Text = openFileDialog1.FileName;
              

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(inport);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void add_Load(object sender, EventArgs e)
        {

        }
    }
}
