using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Devices;

namespace 主程序202009
{
    public partial class excel读取 : Form
    {
        public excel读取()
        {
            InitializeComponent();
        }

    
            public void ReadFromExcelFile(string filePath)
            {

         







            //using (OpenFileDialog openFileDialog1 = new OpenFileDialog() { Filter = "Microsoft Excel files(*.xls)|*.xls;*.xlsx" })
            //    if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //    {
            //        ReadFromExcelFile(openFileDialog1.FileName);
            //    }
            IWorkbook wk = null;
                string extension = System.IO.Path.GetExtension(filePath);
                try
                {
                    FileStream fs = File.OpenRead(filePath);
                    if (extension.Equals(".xls"))
                    {
                        //把xls文件中的数据写入wk中
                        wk = new HSSFWorkbook(fs);
                    }
                    else
                    {
                        //把xlsx文件中的数据写入wk中
                        wk = new XSSFWorkbook(fs);
                    }

                    fs.Close();
                    //读取当前表数据
                    ISheet sheet = wk.GetSheetAt(0);

                    IRow row = sheet.GetRow(0);  //读取当前行数据
                                                 //LastRowNum 是当前表的总行数-1（注意）
                   // int offset = 0;
                    for (int i = 0; i <= sheet.LastRowNum; i++)
                    {
                        row = sheet.GetRow(i);  //读取当前行数据
                        if (row != null)
                        {
                        ListViewItem lv1;
                        try
                        {
                            lv1 = listView1.Items.Add(row.GetCell(0).ToString());
                        }
                        catch (Exception)
                        {

                            return;
                        }
                      


                        for (int j = 1; j < row.LastCellNum; j++) //LastCellNum 是当前行的总列数
                        {
                           var value = "";
                            try
                            {
                                value= row.GetCell(j).ToString();
                                lv1.SubItems.Add(value.ToString());

                            }
                            catch (Exception)
                            {
                                lv1.SubItems.Add(value.ToString());
                                continue;
                                
                            }
                           
                           
                                                                                                            //读取该行的第j列数据
                            
                          
                        }
                               
                        }
                    }
                }

                catch (Exception e)
                {
                //只在Debug模式下才输出
                MessageBox.Show(e.ToString());
                   
                }
            }


        /// <summary>
        /// 核对索引
        /// </summary>
        public void jianchasuoyin()
        {
          
            if (textBox1.Text == "")
            {
                MessageBox.Show("请先选择表格目录");
                return;
            }
            textBox5.Text = "";
            for (int i = 1; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].BackColor = Color.White;
            }
            for (int i = 1; i < listView1.Items.Count; i++)
            {
                try
                {
                    if (listView1.Items[i].SubItems[5].Text == "" && listView1.Items[i].SubItems[0].Text == "")
                    {
                        break;
                    }

                        if (listView1.Items[i].SubItems[5].Text != listView1.Items[i].SubItems[6].Text + "-" + listView1.Items[i].SubItems[0].Text)
                        {
                            listView1.Items[i].BackColor = Color.Red;
                            textBox5.Text += listView1.Items[i].SubItems[5].Text + "【 索引 】错误" + "\r\n";
                            continue;
                        }
                    
                }
                catch (Exception)
                {
                    continue;
                }
            }

        }


        /// <summary>
        /// 核对页数
        /// </summary>
        public void jianchayeshu()
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("请先选择文件夹");
                return;
            }
            if (textBox1.Text == "")
            {
                MessageBox.Show("请先选择表格目录");
                return;
            }
            textBox3.Text = "";
            for (int i = 1; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].BackColor = Color.White;
            }

            int wenjianjiageshu = 0;
            List<String> lists = new List<string>();
            DirectoryInfo root = new DirectoryInfo(textBox2.Text);
            DirectoryInfo[] di = root.GetDirectories();

            for (int i = 0; i < di.Length; i++)
            {

                 wenjianjiageshu=wenjianjiageshu+ di[i].GetDirectories().Length;


            }

            //foreach (string ziDirectory in lists)
            //{
            //    MessageBox.Show(ziDirectory);
            //}

            int tupianzongyeshu = 0;
            int muluzongyeshu = 0;
            for (int i = 1; i < listView1.Items.Count; i++)
            {
                try
                {
                    if (listView1.Items[i].SubItems[5].Text == "" && listView1.Items[i].SubItems[0].Text == "")
                    {
                        break;
                    }
                    string zidicName = listView1.Items[i].SubItems[6].Text + "\\" + listView1.Items[i].SubItems[5].Text;
                    string zidicPath = textBox2.Text + "\\" + zidicName;
                    
                    int geshu = FileOrDirectorygeshu(zidicPath);  //文件夹的图片个数
                    int biaogegeshu = listView1.Items[i].SubItems[3].Text=="" ? 0 : Convert.ToInt32(listView1.Items[i].SubItems[3].Text); //表格的图片个数


                    tupianzongyeshu = tupianzongyeshu + geshu;
                    muluzongyeshu = muluzongyeshu + biaogegeshu;

                    if (geshu != biaogegeshu)
                    {
                        // listView1.Items[i].SubItems[3].Text = geshu.ToString();
                        listView1.Items[i].BackColor = Color.Red;
                        //textBox3.Text += listView1.Items[i].SubItems[5].Text + "【 页数 】错误" +zidicPath+"  "+geshu+"  "+biaogegeshu+ "\r\n";
                        textBox3.Text += listView1.Items[i].SubItems[5].Text + "【 页数 】错误" + "\r\n";
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }




            }

            label2.Text = "图片总页数："+tupianzongyeshu.ToString()+ "  目录总页数：" + muluzongyeshu.ToString()+" 条数共："+ (listView1.Items.Count-1) + " 文件夹共：" + wenjianjiageshu;
            MessageBox.Show("完成");

        }


        //Excel 97-2003 Workbook|*.xls|Excel Workbook|*.xlsx
        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog1 = new OpenFileDialog() { Filter = "Microsoft Excel files(*.xls)|*.xls;*.xlsx" })
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = openFileDialog1.FileName;
                    ReadFromExcelFile(openFileDialog1.FileName);
                }

        }
        /// <summary>
        /// 获取文件夹内文件个数
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
       public static int FileOrDirectorygeshu(string path)
        {
           int count = 0;
            //统计文件的个数
            try
            {
                var files = Directory.GetFiles(path); //String数组类型
                count += files.Length;
                Console.WriteLine(files);

                //遍历文件夹
                var dirs = Directory.GetDirectories(path);
                foreach (var dir in dirs)
                {
                    count += FileOrDirectorygeshu(dir);
                }

            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            return count;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择所在文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }

                textBox2.Text = dialog.SelectedPath;
            }
        }



      

        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < listView1.Items.Count; i++)
            {


                string zidicName = listView1.Items[i].SubItems[5].Text;
                string zidicPath = textBox2.Text + "\\" + zidicName;
           
                int geshu = FileOrDirectorygeshu(zidicPath);
              
                int biaogegeshu = Convert.ToInt32(listView1.Items[i].SubItems[3].Text);
            
                if (geshu != biaogegeshu)
                {
                     listView1.Items[i].SubItems[3].Text = geshu.ToString();
                 
                }


            }
            MessageBox.Show("已完成");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].SubItems[5].Text = listView1.Items[i].SubItems[6].Text + "-" + listView1.Items[i].SubItems[0].Text;
            }
            MessageBox.Show("已完成");
        }

        private void button6_Click(object sender, EventArgs e)
        {
           DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
        /// <summary>
        /// 获取pdf文档的页数
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>-1表示文件不存在</returns>
        public static int GetPDFofPageCount(string filePath)
        {
            int count = -1;//-1表示文件不存在
            if (File.Exists(filePath))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    StreamReader reader = new StreamReader(fs);
                    //从流的当前位置到末尾读取流
                    string pdfText = reader.ReadToEnd();
                    Regex rgx = new Regex(@"/Type\s*/Page[^s]");
                    MatchCollection matches = rgx.Matches(pdfText);
                    count = matches.Count;
                }
            }
            return count;
        }
        private void button7_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(GetPDFofPageCount("C:\\Users\\zhou\\Documents\\Tencent Files\\852266010\\FileRecv\\pdf\\266-RS·0001-0001-10-1.pdf").ToString());
            listView1.Items.Clear();
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

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

                //if (isColumnWritten == true) //写入DataTable的列名
                //{
                //    IRow row = sheet.CreateRow(0);
                //    for (j = 0; j < data.Columns.Count; ++j)
                //    {
                //        row.CreateCell(j).SetCellValue(data.Columns[j].ColumnName);

                //    }
                //    count = 1;
                //}
                //else
                //{
                //    count = 0;
                //}

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
        private void button3_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(jianchayeshu));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

        }
        private void button9_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(jianchasuoyin));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void excel读取_Load(object sender, EventArgs e)
        {

        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
          
            string str = Interaction.InputBox("输入正确索引", "修改索引", listView1.SelectedItems[0].SubItems[5].Text, -1, -1);
            listView1.SelectedItems[0].SubItems[5].Text = str;
        }
        Computer MyComputer = new Computer();

        public void chongmingming()
        {
            DirectoryInfo folder = new DirectoryInfo(textBox2.Text);
            FileInfo[] infos = folder.GetFiles();
            int infocount = infos.Length;
            foreach (FileInfo file in infos)
            {
                string filename = file.FullName;
                string oldname = Path.GetFileNameWithoutExtension(filename);
                string url = "http://m.dianping.com/shop/" + oldname;
                string html = method.GetUrlWithCookie(url, "_lxsdk_cuid=172415f6ef4c8-07c00d4610f257-6373664-1fa400-172415f6ef5c8; _lxsdk=172415f6ef4c8-07c00d4610f257-6373664-1fa400-172415f6ef5c8; _hc.v=189a18fa-f5f0-a2d1-15fa-43abc1680af2.1602832655; s_ViewType=10; ta.uuid=1317006582533660762; isUuidUnion=true; iuuid=172415f6ef4c8-07c00d4610f257-6373664-1fa400-172415f6ef5c8; ua=%E5%91%A8%E5%87%AF%E6%AD%8C; ctu=47f5eba3de69acfd21b70c8ae58e405b7207c5996bee265edc60334adf04114e; ri=1000321300; m_set_info=%7B%22ri%22%3A%221000321300%22%2C%22rv%22%3A%221602833832189%22%2C%22ui%22%3A%22875973616%22%7D; rv=1602833832189; switchcityflashtoast=1; cityid=1; fspop=test; cy=1; cye=shanghai; default_ab=shop%3AA%3A11%7Cindex%3AA%3A3%7CshopList%3AC%3A5; Hm_lvt_602b80cf8079ae6591966cc70a3940e7=1611982670,1611991743,1612057762,1612073576; lgtoken=074b228ee-6ff9-48a1-a8ac-cb19caf6e114; dper=f5d33714d0e321796429cd6a313bb78ae622ca240f1a1302dc7c1713813edd405aba1710f7fcd2a2e8a928cb8481ccdc9d9e46369e3aa5498d4eea419c24116e742fc05981e4fb00174e50a23e6cfe10244005508f6007e0d6e4409400cddd97; ll=7fd06e815b796be3df069dec7836c3df; uamo=17606117606; dplet=f7a695e9118b65db0a3cc5a479b7c132; Hm_lpvt_602b80cf8079ae6591966cc70a3940e7=1612073826; _lxsdk_s=1775712a43a-4ae-fe9-16f%7C%7C175", "utf-8");
                textBox5.Text = html;
                Match name = Regex.Match(html, @"og:title"" content=""([\s\S]*?)""");
                string newname = name.Groups[1].Value + Path.GetExtension(filename);

                try
                {
                    MyComputer.FileSystem.RenameFile(filename, newname.Trim());
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }
               
            }
        }


        public void riqishaixuan()
        {
            DirectoryInfo folder = new DirectoryInfo(textBox2.Text);
            FileInfo[] infos = folder.GetFiles();
            int infocount = infos.Length;
            foreach (FileInfo file in infos)
            {
                string filePath = file.FullName;
                
                IWorkbook wk = null;
                string extension = System.IO.Path.GetExtension(filePath);
                try
                {
                    FileStream fs = File.OpenRead(filePath);
                    if (extension.Equals(".xls"))
                    {
                        //把xls文件中的数据写入wk中
                        wk = new HSSFWorkbook(fs);
                    }
                    else
                    {
                        //把xlsx文件中的数据写入wk中
                        wk = new XSSFWorkbook(fs);
                    }

                    fs.Close();
                    //读取当前表数据
                    ISheet sheet = wk.GetSheetAt(0);

                    IRow row = sheet.GetRow(0);  //读取当前行数据
                                                 //LastRowNum 是当前表的总行数-1（注意）
                                                 // int offset = 0;
                    for (int i = 0; i <= sheet.LastRowNum; i++)
                    {
                        row = sheet.GetRow(i);  //读取当前行数据
                        if (row != null)
                        {
                            ListViewItem lv1;
                            try
                            {
                                lv1 = listView1.Items.Add(row.GetCell(0).ToString());
                            }
                            catch (Exception)
                            {

                                return;
                            }
                            //string value = row.GetCell(6).ToString();
                            // MessageBox.Show(value);

                            for (int j = 1; j < row.LastCellNum; j++) //LastCellNum 是当前行的总列数
                            {
                                var value = "";
                                try
                                {
                                    value = row.GetCell(j).ToString();
                                    lv1.SubItems.Add(value.ToString());

                                }
                                catch (Exception)
                                {
                                    lv1.SubItems.Add(value.ToString());
                                    continue;

                                }


                                //读取该行的第j列数据


                            }

                        }
                    }
                }

                catch (Exception e)
                {
                    //只在Debug模式下才输出
                    MessageBox.Show(e.ToString());

                }



            }
        }
        private void button8_Click_1(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(chongmingming));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

        }

        private void button10_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(riqishaixuan));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
