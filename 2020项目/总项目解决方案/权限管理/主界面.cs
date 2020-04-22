using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 权限管理
{
    public partial class 主界面 : Form
    {
        public 主界面()
        {
            InitializeComponent();
        }

        private void 主界面_Load(object sender, EventArgs e)
        {
            if (登录.qid == "0")
            {
                tabPage2.Parent = null;
                tabPage3.Parent = null;
            }
            if (登录.qid == "1")
            {
                tabPage1.Parent = null;
                tabPage3.Parent = null;
            }
            if (登录.qid == "2")
            {
                tabPage1.Parent = null;
                tabPage2.Parent = null;
            }


        }
    }
}
