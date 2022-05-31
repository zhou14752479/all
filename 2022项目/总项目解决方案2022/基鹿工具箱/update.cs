using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 基鹿工具箱
{
    public partial class update : Form
    {
        public update()
        {
            InitializeComponent();
        }

        private void update_Load(object sender, EventArgs e)
        {
           
            string fileurl = "http://47.96.189.55/jilusoft/jilu.exe";
            Util.DownloadFile(fileurl, Util.path + "基鹿工具箱.exe", progressBar1, label1);
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string appName = Util.path + "基鹿工具箱.exe";
                Process proc = Process.Start(appName);
               

                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
