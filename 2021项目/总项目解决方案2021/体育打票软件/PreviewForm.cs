using gregn6Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 体育打票软件
{
    public partial class PreviewForm : Form
    {
        public PreviewForm()
        {
            InitializeComponent();
        }
        public void AttachReport(GridppReport Report)
        {
            //设定查询显示器关联的报表
            axGRPrintViewer1.Report = Report;
        }
        private void PreviewForm_Load(object sender, EventArgs e)
        {
           
            axGRPrintViewer1.Start();
          

        }

        private void PreviewForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            axGRPrintViewer1.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            axGRPrintViewer1.Print(true);
        }
    }
}
