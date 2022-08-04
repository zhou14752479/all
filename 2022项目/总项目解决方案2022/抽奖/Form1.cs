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

namespace 抽奖
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        Thread thread;
        private void Form1_Load(object sender, EventArgs e)
        {

        }





        bool status = true;

        string[] value = {"5","6","7","8","9","10","11", "12", "13", "14", "15", "16" };


        public void run()
        {

            try
            {
                while(true)
                {
                    foreach (Control control in panel1.Controls)
                    {
                        if (control is Button &&control.Name!= "button17" && control.Name != "button18")
                        {
                            control.BackColor = Color.Red;
                            Thread.Sleep(100);
                        }
                        control.BackColor = Color.White;
                        if (status == false)
                            break;
                    }

                    if (status == false)
                        break;
                }

                Random rd = new Random(Guid.NewGuid().GetHashCode());
                int suijizimu = rd.Next(0, value.Length);
              
                switch (value[suijizimu])
                {
                    case "5":
                        button5.BackColor = Color.Red;
                        break;
                    case "6":
                        button6.BackColor = Color.Red;
                        break;
                    case "7":
                        button7.BackColor = Color.Red;
                        break;
                    case "8":
                        button8.BackColor = Color.Red;
                        break;
                    case "9":
                        button9.BackColor = Color.Red;
                        break;
                    case "10":
                        button10.BackColor = Color.Red;
                        break;
                    case "11":
                        button11.BackColor = Color.Red;
                        break;
                    case "12":
                        button12.BackColor = Color.Red;
                        break;
                    case "13":
                        button13.BackColor = Color.Red;
                        break;
                    case "14":
                        button14.BackColor = Color.Red;
                        break;
                    case "15":
                        button15.BackColor = Color.Red;
                        break;
                    case "16":
                        button16.BackColor = Color.Red;
                        break;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("七夕快乐");
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            foreach (Control control in panel1.Controls)
            {
                control.BackColor = Color.White;

            }
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            status = false;
        }
    }
}
