using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 爱采集网站
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }
        string biaoti = "";
        string neirong = "";
        string guanjianci = "";
        public void run()
        {
            


            string url = "http://www.acaiji.com/admin/Index/write.html";
            string postdata = "biaoti="+ biaoti + "&neirong="+ neirong + "&guanjianci="+ guanjianci + "&laiyuan=&zhaiyao="+biaoti+"&verification=8563c07a621ea96d3b640c1f2e71f4cb&suolvetu=&fabushijian=2019-11-21+16%3A30%3A33&zhiding=0&tuijian=0&pinglun=1&xingshi=0";
            string cookie = textBox1.Text;
            string html = method.PostUrl(url,postdata,cookie,"utf-8");
        }
    }
}
