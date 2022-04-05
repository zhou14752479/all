using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace 人员信息管理系统
{
    public partial class 添加数据 : Form
    {
        public 添加数据()
        {
            InitializeComponent();
        }

        function fc = new function();
        Dictionary<string, string> dics = new Dictionary<string, string>();
        private void 添加数据_Load(object sender, EventArgs e)
        {
            string texts = fc.readtxt();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].Trim() != "")
                {
                  
                    string[] value = text[i].Split(new string[] { "#" }, StringSplitOptions.None);
                    if (value.Length > 1)
                    {
                        dics.Add(value[1], value[0]);
                        DataGridViewTextBoxColumn acCode = new DataGridViewTextBoxColumn();
                        acCode.Name = "acCode";
                        acCode.HeaderText = value[1];
                       dataGridView1.Columns.Add(acCode);

                    }
                }
            }
        }

        private void 清空数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Rows.Clear();
            }
            catch (Exception)
            {
              
                MessageBox.Show("清空失败");
            }
        }

        DataTable dt;
        private void 批量导入模板ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            // openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件

                dt = function.ExcelToDataTable(openFileDialog1.FileName, true);
                
            }
           
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int index = dataGridView1.Rows.Add();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    dataGridView1.Rows[index].Cells[j].Value = dt.Rows[i][j].ToString().Trim();
                 
                }
            }

            MessageBox.Show("添加数据成功");
        }

        #region 插入数据

       
        public void insertdata()
        {
            if (dt == null)
            {
                MessageBox.Show("请选择表格");

            }
            string ziduan = "";
            foreach (var item in dics.Values)
            {
                ziduan = ziduan + item.ToString() + ",";
            }
            if(ziduan.Length>2)
            {
                ziduan = ziduan.Remove(ziduan.Length-1,1);
            }
           
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                try
                {
                    string value = "";
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        if (j < dt.Columns.Count - 1)
                        {
                            value = value + "'" + dt.Rows[i][j].ToString().Trim() + "'" + ",";
                        }
                        else
                        {
                            value = value + "'" + dt.Rows[i][j].ToString().Trim() + "'";
                        }

                    }
                    toolStripStatusLabel1.Text = "正在添加数据----->" + value;
                    string sql = "INSERT INTO datas("+ziduan+") values(" + value + ") ";
                   
                    function.SQL(sql);
                }
                catch (Exception)
                {

                    continue;
                }

            }
            toolStripStatusLabel1.Text = "导入完成";

        }
        #endregion

        Thread thread;
        private void 点击添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要添加吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(insertdata);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
            else
            {
               
            }
        }
    }
}
