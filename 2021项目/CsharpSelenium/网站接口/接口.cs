using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace 网站接口
{
    public partial class 接口 : Form
    {
        public 接口()
        {
            InitializeComponent();
        }
        public static string path = AppDomain.CurrentDomain.BaseDirectory;
       
        server sv = new server();

      


        private void 伪原创接口_Load(object sender, EventArgs e)
        {

        }
       
     


        private void button1_Click(object sender, EventArgs e)
        {



        
            textBox1.Text = "http://localhost:8080/api/index.html?userid=123&taskid=456&city=宿迁&keyword=烤肉店&sign=aaa";
            IPAddress ip = IPAddress.Any;
            

           sv.start(ip, 8080, 100, path);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //method md = new method();
            //string o = "1,宿迁鲜花店";
            //textBox1.Text = md.articleslist_get(o);


            sv.stop();
            textBox1.Text = DateTime.Now.ToString("HH:MM:ss") + "API已停止";
        }



        method md = new method();
        public void ceshi()
        {
            //baiduOCR ocr = new baiduOCR();
            //string result = ocr.shibie("http://i.cy1788.com/data/20210625/10538cf20cec45cea595c0b5b3d35475.png");
            try
            {

               
                textBox1.Text = md.getwangwang("tb10879579"); ;
            }
            catch (Exception ex) 
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            md.getwangwang("zkg852266010");
        }
    }
}
