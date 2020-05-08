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

namespace TCP通信
{
    public partial class 服务端 : Form
    {
        public 服务端()
        {
            InitializeComponent();
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;

        public void connect()
        {
            try
                
            {
                StreamReader sr = new StreamReader(path + "ip.txt", Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);


                // 把IP地址转换为IPAddress的实例
                IPAddress ipAd = IPAddress.Parse(text[0]);

                // 初始化监听器， 端口为8001
                TcpListener myList = new TcpListener(ipAd, Convert.ToInt32(text[1].Trim()));
                // 开始监听服务器端口
                myList.Start();

                // 输出服务器启动信息
                textBox2.Text += "在8001端口启动服务..."+"\r\n";
                textBox2.Text += "本地节点为:" + myList.LocalEndpoint + "\r\n";
                textBox2.Text += "等待连接....." + "\r\n";



                // 等待处理接入连接请求
                // 新建立的连接用套接字s表示
                Socket s = myList.AcceptSocket();
                textBox2.Text += "连接来自 " + s.RemoteEndPoint + "\r\n";

                while (true)
                {
                    // 接收客户端信息
                    byte[] b = new byte[100];
                    int k = s.Receive(b);
                    textBox2.Text += "已接收..." + "\r\n";
                    StringBuilder sb = new StringBuilder();
                    //for (int i = 0; i < k; i++)
                    //{
                    //    //sb.Append (Convert.ToChar(b[i]));
                        
                    //}
                   
                    sb.Append(System.Text.Encoding.UTF8.GetString(b));
                  
                    textBox1.Text+= (sb.ToString()+"\r\n");
                    //// 处理客户端请求，给客户端回应
                    //ASCIIEncoding asen = new ASCIIEncoding();
                    //s.Send(asen.GetBytes("The string was recieved by the server."));
                    //Console.WriteLine("/n已发送回应信息");
                    //Console.Read();

                    //// 善后工作，释放资源
                    // s.Close();
                    //myList.Stop();
                }
            }
            catch (Exception ex)
            {
                textBox1.Text += ex.ToString();
            }
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(connect));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;


        }

        private void 服务端_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }

        private void 服务端_Load(object sender, EventArgs e)
        {

        }
    }
}
