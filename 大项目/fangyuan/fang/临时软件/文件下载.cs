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
            ArrayList ids = new ArrayList();
            string path = AppDomain.CurrentDomain.BaseDirectory;
            foreach (string id in ids)
            {

 
               
                    if (!Directory.Exists(path + id))
                    {
                        Directory.CreateDirectory(path + id); //创建文件夹
                    }

                    for (int j = 1; j < 7; j++)
                    {

                    string url = "http://mall.plap.cn/npc/products.html?utf8=%E2%9C%93&q%5Bcatalog_id_eq%5D="+id+"&q%5Bname_or_products_my_sku_cont%5D=&q%5Bproducts_price_gteq%5D=&q%5Bproducts_price_lteq%5D=&commit=%E5%AF%BC%E5%87%BAEXCEL";

                        if (method.GetUrl(url, "utf-8") != "")   //判断请求图片的网址响应是否为空，如果为空表示没有图片，下载会报错！
                        {
                            method.downloadFile(url, path + i, "//" + j + ".xlsx");

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
