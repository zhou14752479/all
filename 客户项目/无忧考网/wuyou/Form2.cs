using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wuyou
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
               
                StreamReader sr = new StreamReader(openFileDialog1.FileName, Encoding.Default);

                
                while (sr.ReadLine() != null)
                {
                    
                    textBox1.Text += sr.ReadLine()+"\r\n";
                }
                
                sr.Close();
               
          


            }
        }

       










    }
}
