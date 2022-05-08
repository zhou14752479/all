using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 阿里巴巴采集软件
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
                formChild.FormBorderStyle = FormBorderStyle.None;
                formChild.Dock = DockStyle.Fill;
                formChild.Show();

            }
        }



        private void 商品采集ToolStripMenuItem_Click(object sender, EventArgs e)
        {
         
            阿里巴巴商品采集 aligood = new 阿里巴巴商品采集();
            aligood.Name = aligood.GetType().FullName;
            OpenChildForm(aligood);
        }

        private void 评论采集ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            阿里巴巴评论采集 aligood = new 阿里巴巴评论采集();
            aligood.Name = aligood.GetType().FullName;
            OpenChildForm(aligood);
        }

        private void 主界面_Load(object sender, EventArgs e)
        {

        }
    }
}
