using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 基鹿工具箱
{
    public partial class 后台数据导入 : Form
    {
        public 后台数据导入()
        {
            InitializeComponent();
        }
        
        DataTable dt;
       

        public void insertdata()
        {
            if(dt==null )
            {
                MessageBox.Show("请选择表格");
                return;
            }
           
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                string ci = dt.Rows[i][0].ToString().Trim();
                string ss_zs = dt.Rows[i][1].ToString().Trim();
                string fd = dt.Rows[i][2].ToString().Trim();
                string good_zs = dt.Rows[i][3].ToString().Trim();
                string gx_zs = dt.Rows[i][4].ToString().Trim();
                bool status= Util.insert(ci,ss_zs,fd,good_zs,gx_zs);
                if(status==true)
                {
                    toolStripStatusLabel1 .Text = "导入数据："+ci+" 成功";
                }
                else
                {
                    toolStripStatusLabel1.Text = "导入数据：" + ci + " 失败";
                    break;
                }
            }
            MessageBox.Show("完成");
        }


        Thread thread;
     
       

      

        private void 查询数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Util.getdata("Select ci,ss_zs,fd,good_zs,gx_zs,pp,copy From datas ");
            dataGridView1.Columns["ci"].HeaderText = "相关搜索词";
            dataGridView1.Columns["ss_zs"].HeaderText = "搜索指数";
            dataGridView1.Columns["fd"].HeaderText = "搜索增长幅度";
            dataGridView1.Columns["good_zs"].HeaderText = "商品指数";
            dataGridView1.Columns["gx_zs"].HeaderText = "供需指数";
            dataGridView1.Columns["pp"].HeaderText = "是否匹配";
            dataGridView1.Columns["copy"].HeaderText = "复制关键词";
        }

        private void 清空数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要清空吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                string sql = "delete from datas";
                bool status = Util.SQL(sql);
                Util.SQL("VACUUM");
                Util.SQL("DELETE FROM sqlite_sequence WHERE name = 'datas';");
                MessageBox.Show("清空成功");
            }
            else
            {
               
            }
        }

        private void 开始导入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(insertdata);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void 选择表格ToolStripMenuItem_Click(object sender, EventArgs e)
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

                dt = Util.ExcelToDataTable(openFileDialog1.FileName, true);

            }
        }

        private void 后台数据导入_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }

        private void 导入数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
