using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 模拟采集1
{
    public partial class 搜索采集文章 : Form
    {
        bool zanting = true;
        bool status = true;
        string path = AppDomain.CurrentDomain.BaseDirectory;
        public 搜索采集文章()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        public void getkeys()
        {
            StreamReader streamReader = new StreamReader(path + "key.txt", Encoding.Default);
            string text = streamReader.ReadToEnd();
            string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < array.Length; i++)
            {
                textBox1.Text += array[i] + "\r\n";

            }
            streamReader.Close();
        }
        private void 搜索采集文章_Load(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(getkeys));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
