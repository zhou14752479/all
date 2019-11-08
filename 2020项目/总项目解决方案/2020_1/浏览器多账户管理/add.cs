using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace 浏览器多账户管理
{
    public partial class add : Form
    {
        public add()
        {
            InitializeComponent();
        }

        private void Add_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
           

            Button button1 = new Button();
            button1.Text = "新建店铺";

            button1.Dock = DockStyle.Top;
            button1.Height = 40;
            button1.Font = new Font("Tahoma", 10, FontStyle.Bold);
           // splitContainer1.Panel1.Controls.Add(button1);

           
        }
    }
}
