using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 冷宝分析
{
    public partial class 主界面 : Form
    {
        public 主界面()
        {
            InitializeComponent();
        }
        private void OpenChildForm(Form formChild)//formChild只是是实例化的但既没有设置为父窗体的子窗体也没有显示
        {
            bool isOpened = false;
            foreach (Form form in this.MdiChildren)
            {
                //如果要显示的子窗体已经在父窗体的子窗体数组数组中，我们就把新建的多余的formChild销毁

                if (formChild.Name == form.Name)
                {
                    form.Activate();//既然我们想新建但已经有了，那就把之前存在的激活并调到最前边来
                    form.WindowState = FormWindowState.Normal;//窗口大小  为窗口模式
                    formChild.Dispose();
                    isOpened = true;//表示窗口已经打开

                    break;
                }
            }

            if (!isOpened)//如果没打开
            {
                formChild.MdiParent = this;//设置为子窗体
                formChild.ShowIcon = false;//标题栏不显示Icon
                                           //formChild.FormBorderStyle = FormBorderStyle.None;
                                           // formChild.Dock = DockStyle.Fill;
                formChild.Show();

            }
        }
        private void 场次管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            场次管理 title = new 场次管理();
            title.Name = title.GetType().FullName;
            OpenChildForm(title);
        }

        private void 开宝记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            开宝记录 title = new 开宝记录();
            title.Name = title.GetType().FullName;
            OpenChildForm(title);
        }

        private void 未开分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            未开分析 title = new 未开分析();
            title.Name = title.GetType().FullName;
            OpenChildForm(title);
        }

        private void 主界面_Load(object sender, EventArgs e)
        {
           if(Convert.ToDateTime("2022-12-12")<DateTime.Now)
            {
                MessageBox.Show(" ");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            function.getsoft();
            function.getcolor();
        }

        private void 基本设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            基本设置 title = new 基本设置();
            title.Name = title.GetType().FullName;
            OpenChildForm(title);
        }
    }
}
