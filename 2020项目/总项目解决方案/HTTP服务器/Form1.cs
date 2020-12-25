using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HTTP服务器
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        string path = AppDomain.CurrentDomain.BaseDirectory;

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        server sv = new server();
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text =DateTime.Now.ToString("HH:MM:ss")+ "：API已开启："+"\r\n";
            textBox1.Text += "API地址：" + "\r\n" + "\r\n";
            textBox1.Text += "http://IP地址:8080/api/pdd/index.html?item_id=商品ID";
            IPAddress ip = IPAddress.Any;
            // MessageBox.Show(ip.ToString());
           
            sv.start(ip, 8080, 100, path);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sv.stop();
            textBox1.Text = DateTime.Now.ToString("HH:MM:ss") + "API已停止";
        }
    }
}
