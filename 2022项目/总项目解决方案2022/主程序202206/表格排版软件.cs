using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 主程序202206
{
    public partial class 表格排版软件 : Form
    {
        public 表格排版软件()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开txt文件";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox1.Text = openFileDialog1.FileName;

            }
        }


        #region txt转datable
        /// <summary>
        /// listview转datable
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        public  DataTable txtToDataTable()
        {
            StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();

            int i, j;
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Clear();
            dt.Columns.Clear();
            //lv.Columns.Count
            //生成DataTable列头

           
            dt.Columns.Add("姓名");
            dt.Columns.Add("身份号");
            dt.Columns.Add("手机号");
            //每行内容

            //MatchCollection names = Regex.Matches(texts, @"""xm"":""([\s\S]*?)""");
            //MatchCollection mobiles = Regex.Matches(texts, @"""mobile"":""([\s\S]*?)""");
            //MatchCollection cards = Regex.Matches(texts, @"----([\s\S]*?)----");
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
          
            for (i = 0; i < text.Length; i++)
            {
                try
                {
                    string[] v = text[i].Split(new string[] { "----" }, StringSplitOptions.None);
                    string mobile = Regex.Match(text[i], @"""mobile"":""([\s\S]*?)""").Groups[1].Value;
                    dr = dt.NewRow();
                
                    dr[0] = v[0];
                    dr[1] = v[1];
                    dr[2] = mobile;
                    dt.Rows.Add(dr);
                }
                catch (Exception)
                {

                    continue;
                }
            }

            return dt;
        }
        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text=="")
            {
                MessageBox.Show("请导入文本");
                return;
            }
            method.DataTableToExcel(txtToDataTable(), "Sheet1", true);
        }
    }
}
