using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCP通信
{
    public partial class 客户端 : Form
    {
        public 客户端()
        {
            InitializeComponent();
        }
        TcpClient tcpclnt = new TcpClient();
        private void Button1_Click(object sender, EventArgs e)
        {

            // 新建客户端套接字

            textBox1.Text = "连接.....";

            // 连接服务器
            tcpclnt.Connect("192.168.8.118", 8001);
            
            textBox1.Text += "已连接";
        }


        protected string GetUnicode(string text)
        {
            string result = "";
            for (int i = 0; i < text.Length; i++)
            {
                if ((int)text[i] > 32 && (int)text[i] < 127)
                {
                    result += text[i].ToString();
                }
                else
                    result += string.Format("\\u{0:x4}", (int)text[i]);
            }
            return result;
        }
        public void send()
        {
           
           
            // 读入字符串
            String str = GetUnicode(textBox2.Text);

            // 得到客户端的流

            Stream stm = tcpclnt.GetStream();
            // 发送字符串
            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(str);
           
            textBox1.Text +="传输中.....";
            stm.Write(ba, 0, ba.Length);

            // 接收从服务器返回的信息
            //byte[] bb = new byte[100];
            //int k = stm.Read(bb, 0, 100);

            //// 输出服务器返回信息
            //for (int i = 0; i < k; i++)
            //{
            //    Console.Write(Convert.ToChar(bb[i]));
            //}
            //Console.Read();

            // 关闭客户端连接
            //tcpclnt.Close();
        }
          

        private void Button2_Click(object sender, EventArgs e)
        {
            
            Thread thread = new Thread(new ThreadStart(send));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

        }

        private void 客户端_Load(object sender, EventArgs e)
        {

        }
    }
}
