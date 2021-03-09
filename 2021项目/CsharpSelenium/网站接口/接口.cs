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

        public void ceshi()
        {
            method md = new method();
            // string fileName = @"C:/Users/zhou/Desktop/1/"  + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xlsx";
            
            // string o = "39,84,二手车,"+fileName;
            //md.exportExcel(o);
            string o = "39,84,%e5%ae%bf%e8%bf%81,%e7%be%8e%e5%ae%b9";
            md.Amap(o);

        }
        private void button3_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(ceshi);
            t.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
