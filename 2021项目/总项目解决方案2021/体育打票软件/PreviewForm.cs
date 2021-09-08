using gregn6Lib;
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
    public partial class PreviewForm : Form
    {
        public PreviewForm()
        {
            InitializeComponent();
        }

       
        public void AttachReport(GridppReport Report)
        {
            //设定查询显示器关联的报表

            axGRPrintViewer1.Report = Report;
        }
        private void PreviewForm_Load(object sender, EventArgs e)
        {
            textBox2.Text = DateTime.Now.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss");
            axGRPrintViewer1.Start();
            //axGRPrintViewer1.ZoomToWidth();



           
            checkBox5.CheckedChanged += new EventHandler(ziyouguaguan);
            checkBox6.CheckedChanged += new EventHandler(ziyouguaguan);
            checkBox7.CheckedChanged += new EventHandler(ziyouguaguan);
            checkBox8.CheckedChanged += new EventHandler(ziyouguaguan);
            checkBox9.CheckedChanged += new EventHandler(ziyouguaguan);
            checkBox10.CheckedChanged += new EventHandler(ziyouguaguan);
            checkBox11.CheckedChanged += new EventHandler(ziyouguaguan);
        }

        private void PreviewForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            axGRPrintViewer1.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            axGRPrintViewer1.Print(true);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox2.Text =Convert.ToDateTime(textBox2.Text).AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                axGRPrintViewer1.Report.ParameterByName("guoguan").AsString = "过关方式 " + textBox3.Text + "x" + textBox4.Text;
                axGRPrintViewer1.Refresh();
            }
        }


        string zygg = function.a+"场-";
        public void ziyouguaguan(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                zygg = zygg + ((CheckBox)sender).Text.Replace("关", ",");
                axGRPrintViewer1.Report.ParameterByName("guoguan").AsString = zygg.Remove(zygg.Length - 1, 1) + "关";
                axGRPrintViewer1.Refresh();
            }
            else
            {
                zygg = zygg.Replace(((CheckBox)sender).Text.Replace("关", ","),"");
                axGRPrintViewer1.Report.ParameterByName("guoguan").AsString = zygg.Remove(zygg.Length - 1, 1) + "关";
                axGRPrintViewer1.Refresh();
            }
           
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {   
                axGRPrintViewer1.Report.ParameterByName("guoguan").AsString = function.a+"场-单场固定";
                axGRPrintViewer1.Refresh();

            }
        }



    }
}
