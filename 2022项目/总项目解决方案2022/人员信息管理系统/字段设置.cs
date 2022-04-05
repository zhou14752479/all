using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 人员信息管理系统
{
    public partial class 字段设置 : Form
    {
        public string name;
        public 字段设置(string name)
        {
            InitializeComponent();
            this.name = name;
        }

        function fc = new function();
        private void 字段设置_Load(object sender, EventArgs e)
        {
            this.Text = name;

            if(this.name=="修改字段")
            {
                string texts = fc.readtxt();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i].Trim() != "")
                    {
                        int index = dataGridView1.Rows.Add();
                        string[] value = text[i].Split(new string[] { "#" }, StringSplitOptions.None);
                        if (value.Length > 1)
                        {

                          
                            dataGridView1.Rows[index].Cells[0].Value = value[0];
                            dataGridView1.Rows[index].Cells[1].Value = value[1];
                        }
                    }
                }

               
            }
            


        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.name == "修改字段")
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value.ToString() != "")
                    {
                        sb.Append(dataGridView1.Rows[i].Cells[0].Value + "#" + dataGridView1.Rows[i].Cells[1].Value+"\r\n");
                    }

                }
                fc.xieru(sb.ToString(), FileMode.Create);
            }
            if(this.name=="添加字段")
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    try
                    {
                        string ziduan = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        string ziduanname = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        function.SQL("ALTER TABLE datas ADD "+ziduan+" char(255);");
                        if (dataGridView1.Rows[i].Cells[0].Value.ToString().Trim() != "")
                        {
                            fc.xieru(ziduan + "#" + ziduanname, FileMode.Append);
                        }
                    }
                    catch (Exception)
                    {

                        continue;
                    }

                }
            }


            MessageBox.Show("保存成功");
        }
    }
}
