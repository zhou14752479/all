using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 图片点击显示
{
    public partial class 图片点击显示 : Form
    {
        public 图片点击显示()
        {
            InitializeComponent();
        }

        private void 图片点击显示_Load(object sender, EventArgs e)
        {
            
            foreach (Control item in this.Controls)
            {
                if (item is Button)
                {
                    item.Click+= new EventHandler(btn_clicked);
                }
            
            }
        }

        private void btn_clicked(object sender, EventArgs e)
        {
            图片框 pic = new 图片框(((Button)sender).Text);
            pic.Show();
        }

      
    }
}
