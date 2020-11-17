using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 计算
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public double input1;
        public double input2;
        public double input3;
        public double input4;
        public double input5;
        public double sheding;
        public double output1;
        public double output2;
        public double output3;
        public double output4;
        public double output5;

        public void main(int geshu)
        {
            double value1 = input1 * output1;
            double value2 = input2 * output2;
            double value3 = input3 * output3;
            double value4 = input4 * output4;
            double value5 = input5 * output5;
            try
            {
                if (geshu == 5)
                {
                    

                    if (sheding<value1-(value2+ value3+ value4+ value5) || sheding < value2 - (value1 + value3 + value4 + value5) || sheding < value3 - (value2 + value1 + value4 + value5) || sheding < value4 - (value2 + value3 + value1 + value5) || sheding < value5 - (value2 + value3 + value4 + value1))
                    {
                        
                    }

                }

            }
            catch (Exception)
            {

                throw;
            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {
          


        }
    }
}
