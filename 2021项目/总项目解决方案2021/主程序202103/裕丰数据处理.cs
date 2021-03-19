using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 主程序202103
{
    public partial class 裕丰数据处理 : Form
    {
        public 裕丰数据处理()
        {
            InitializeComponent();
        }

        DataTable dt=null;
        public void run()
        {
            dt = method.ExcelToDataTable(textBox1.Text, true);
            try
            {

                 // method.ShowDataInListView(dt, listView1);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                   DataRow dr = dt.Rows[i];
                    string address = dr[1].ToString();
                    string a1 = Regex.Replace(address, @"\d{1,}楼\d{1,}房", "");
                    a1= Regex.Replace(a1, @"\d{1,}栋", "");
                    a1 = Regex.Replace(a1, @"\d{1,}座", "");
                    a1 = Regex.Replace(a1, @"[A-Za-z]{1,}座", "");
                    string a2 = Regex.Match(address, @"\d{1,}栋").Groups[0].Value;
                    if (a2 == "")
                    {
                        a2 = Regex.Match(address, @"\d{1,}座").Groups[0].Value;
                    }
                    if (a2 == "")
                    {
                        a2 = Regex.Match(address, @"[A-Za-z]{1,}座").Groups[0].Value;
                    }

                    string a3 = Regex.Match(address, @"\d{1,}楼").Groups[0].Value;
                    string a4= Regex.Match(address, @"\d{1,}房").Groups[0].Value;



                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    
                    lv1.SubItems.Add(a1);
                    lv1.SubItems.Add(a2);
                    lv1.SubItems.Add(a3);
                    lv1.SubItems.Add(a4);

                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void 裕丰数据处理_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog  openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            // openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
               textBox1.Text = openFileDialog1.FileName;
               
              

            }
        }
        Thread thread;
        private void button2_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
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
                textBox2.Text = openFileDialog1.FileName;



            }
        }

        private void button4_Click(object sender, EventArgs e)
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
                textBox3.Text = openFileDialog1.FileName;



            }
        }

        public void createNewExcel()
        {


        }
        private void button5_Click(object sender, EventArgs e)
        {
           
           DataTable lastdaydt = method.ExcelToDataTable(textBox1.Text, true);
            DataTable todaydt = method.ExcelToDataTable(textBox1.Text, true);

        }
    }
}
