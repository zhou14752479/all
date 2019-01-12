using System;
using System.Threading;
using System.Windows.Forms;

namespace main
{
   
    public partial class infosForm : Form
    {

       

        public infosForm()
        {
            InitializeComponent();

           
        }


        private void infosForm_Load(object sender, EventArgs e)
        {
           
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text += comboBox1.SelectedItem.ToString() + ",";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pubVariables.citys = this.textBox1.Text;
            pubVariables.keywords = this.textBox2.Text;

            if (radioButton1.Checked)
            {
                pubVariables.fangFrom = 0;
            }
            else if (radioButton2.Checked)
            {
                pubVariables.fangFrom = 1;

            }

            else {
                pubVariables.fangFrom = 0;
            }

            this.Close();

           
        }

        private void infosForm_MouseEnter(object sender, EventArgs e)
        {
 
        }

        private void skinTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void skinTreeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            textBox2.Text += e.Node.Name;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text += comboBox2.SelectedItem.ToString() + ",";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            webBrowser web = new webBrowser("http://sh.meituan.com/meishi/");
            web.Show();
        }
    }
}
