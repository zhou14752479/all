using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 及时展现
{
    public partial class 客户端 : Form
    {
        public 客户端()
        {
            InitializeComponent();
        }

        private void 客户端_Load(object sender, EventArgs e)
        {
           
          
            
        }

        private void Button1_Click(object sender, EventArgs e)
        {
  


            string conn = "Host =139.159.218.174;Database=data;Username=root;Password=123456";
            MySqlDataAdapter sda = new MySqlDataAdapter("Select aname,bname From datas Where length(aname)=20", conn);
                DataSet Ds = new DataSet();
                sda.Fill(Ds, "T_Class");

                this.dataGridView1.DataSource = Ds.Tables["T_Class"];
          

        }

        private void DataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //增加序号
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
         e.RowBounds.Location.Y,
        dataGridView1.RowHeadersWidth - 4,
        e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics,
            (e.RowIndex + 1).ToString(),
            dataGridView1.RowHeadersDefaultCellStyle.Font,
               rectangle,
                   dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                   TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
    }
}
