using System;
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

namespace 表格分类
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            //openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                textBox1.Text = openFileDialog1.FileName;



            }
        }

        List<string> lists = new List<string>();
       
        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text=="")
            {
                MessageBox.Show("请选择原始表格");
                return;
            }
            DataTable dt = method.ExcelToDataTable(textBox1.Text,true);

            for (int i = 0; i < 100; i++)
            {
                run(dt);
            }

            MessageBox.Show("完成");
        }


        public void run(DataTable dt)
        {
            DataTable newdt = new DataTable();
            string zhutaocanname = "";

            newdt.Columns.Add("号码");
            newdt.Columns.Add("主套餐名称");
            newdt.Columns.Add("促销名称");
            newdt.Columns.Add("协议生效时间");
            newdt.Columns.Add("协议失效时间");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = null;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string zhutaocan = dt.Rows[i][2].ToString().Split(new string[] { "\n" }, StringSplitOptions.None)[0];

                    if (zhutaocanname == "")
                    {
                        if (lists.Contains(zhutaocan))
                            break;

                        lists.Add(zhutaocan);
                        zhutaocanname = zhutaocan;

                    }

                    if (zhutaocanname == zhutaocan)
                    {
                        dr = newdt.NewRow(); ;//生成行对象
                        dr["号码"] = dt.Rows[i][1].ToString();
                        dr["主套餐名称"] = dt.Rows[i][2].ToString();
                        dr["促销名称"] = dt.Rows[i][3].ToString();
                        dr["协议生效时间"] = dt.Rows[i][4].ToString();
                        dr["协议失效时间"] = dt.Rows[i][5].ToString();
                    }


                }
                if (dr != null)
                {
                    newdt.Rows.Add(dr);
                }


            }
            method.DataTableToExcelName(newdt, "主套餐/"+removeValid(zhutaocanname)+ ".xlsx", true);
        }


        #region 去掉路径中非法字符
        public string removeValid(string illegal)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                illegal = illegal.Replace(c.ToString(), "");
            }
            return illegal;
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
