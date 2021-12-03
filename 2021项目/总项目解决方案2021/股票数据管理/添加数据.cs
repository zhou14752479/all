using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 股票数据管理
{
    public partial class 添加数据 : Form
    {
        public 添加数据()
        {
            InitializeComponent();
        }

        function fc = new function();
        private void button3_Click(object sender, EventArgs e)
        {
            bool status = fc.UPDATEdata(codetxt.Text.Trim(),textBox1.Text.Trim(), textBox2.Text.Trim(), textBox3.Text.Trim(), textBox4.Text.Trim(), textBox5.Text.Trim(), textBox6.Text.Trim());
            if(status==true)
            {
                MessageBox.Show("添加今日数据成功");
            }
            else
            {
                bool status2 = fc.INSERTdata(codetxt.Text.Trim(),nametxt.Text.Trim(), textBox1.Text.Trim(), textBox2.Text.Trim(), textBox3.Text.Trim(), textBox4.Text.Trim(), textBox5.Text.Trim(), textBox6.Text.Trim());
              
                if (status2 == true)
                {
                    MessageBox.Show("不存在昨日数据，已添加为新数据");
                }
                else
                {
                    MessageBox.Show("添加为新数据失败");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = function.ExcelToDataTable(xlstxt.Text.Trim(), true);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string xuhao = dt.Rows[i][0].ToString().Trim();
                    string code= dt.Rows[i][1].ToString().Trim();
                    string name = dt.Rows[i][2].ToString().Trim();
                    string b1 = dt.Rows[i][3].ToString().Trim();
                    string b2 = dt.Rows[i][4].ToString().Trim();
                    string b3 = dt.Rows[i][5].ToString().Trim();
                    string b4 = dt.Rows[i][6].ToString().Trim();
                    string b5 = dt.Rows[i][7].ToString().Trim();
                    string b6 = dt.Rows[i][8].ToString().Trim();
                    bool status = fc.UPDATEdata(code,b1,b2,b3,b4,b5,b6);
                    if (status == false)
                    {
                        fc.INSERTdata(code,name, b1, b2, b3, b4, b5, b6);
                    }

                }

                MessageBox.Show("添加成功");
            }
            catch (Exception)
            {

                MessageBox.Show("打开表格失败");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            //openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
             xlstxt.Text=(openFileDialog1.FileName);
            


            }
        }
    }
}
