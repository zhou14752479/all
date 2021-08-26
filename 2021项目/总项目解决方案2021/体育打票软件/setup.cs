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
    public partial class setup : Form
    {
        public setup()
        {
            InitializeComponent();
        }

        public static string muban="a";
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            muban = "a";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            muban = "a1";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            muban = "b";
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            muban = "b1";
        }

        private void setup_Load(object sender, EventArgs e)
        {

        }
    }
}
