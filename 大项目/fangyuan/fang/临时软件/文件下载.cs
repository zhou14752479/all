using System;
using System.Collections;
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

namespace fang.临时软件
{
    public partial class 文件下载 : Form
    {
        public 文件下载()
        {
            InitializeComponent();
        }

        private void 文件下载_Load(object sender, EventArgs e)
        {

        }
        ArrayList finishes = new ArrayList();
        public void run()

        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            for (int i = 1001; i < 3002; i++)
            {
               
                    if (!Directory.Exists(path + i))
                    {
                        Directory.CreateDirectory(path + i); //创建文件夹
                    }

                    for (int j = 1; j < 7; j++)
                    {

                        string url = "http://pic.shanshi123.com/img/" + i + "/" + j + ".jpg";

                        if (method.GetUrl(url, "utf-8") != "")   //判断请求图片的网址响应是否为空，如果为空表示没有图片，下载会报错！
                        {
                            method.downloadFile(url, path + i, "//" + j + ".jpg");

                        }

                        label2.Text = url;
                    
                }
            }

        }


        private void Button1_Click(object sender, EventArgs e)
        {
          
                Thread thread = new Thread(new ThreadStart(run));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();
          

        }
    }

}
