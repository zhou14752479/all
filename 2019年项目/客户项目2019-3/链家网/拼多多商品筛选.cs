using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 链家网
{
    public partial class 拼多多商品筛选 : Form
    {
        public 拼多多商品筛选()
        {
            InitializeComponent();
        }

        static public DataSet ExcelToDataSet(string filename)
        {
             
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                            "Extended Properties=Excel 8.0;" +
                            "data source=" + filename;
            OleDbConnection myConn = new OleDbConnection(strCon);
            string strCom = " SELECT * FROM [Sheet1$]";
            myConn.Open();
            OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn);
            DataSet ds = new DataSet();
            myCommand.Fill(ds);
            myConn.Close();
            return ds;
        }

    
        

        private void 拼多多商品筛选_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = "";
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                 path  = this.openFileDialog1.FileName;

            }

            DataTable dt = ExcelToDataSet(path).Tables[0];
            dataGridView1.DataSource = dt;
        }

      
    }
}
