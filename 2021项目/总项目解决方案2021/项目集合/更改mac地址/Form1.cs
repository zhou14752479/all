using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(DateTime.Now<Convert.ToDateTime("2021-08-27 21:00:00"))
            {
                string mac = method.SetMac.ExtractModifyMac();
                MessageBox.Show(mac);
            }
         
        }
    }
}
