using Spire.Xls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace 文件转XML
{
   
        /// <summary>
        　/// 这个示例演示如何把Office文件编码为xml文件以及如何把生成的xml文件转换成Office文件
        　/// 把文件转换成xml格式,然后就可以用web服务,.NET Remoting,WinSock等传送了(其中后两者可以不转换也可以传送)
        　/// xml解决了在多层架构中数据传输的问题,比如说在客户端可以用Web服务获取服务器端的office文件,修改后再回传给服务器
        　/// 只要把文件转换成xml格式,便有好多方案可以使用了,而xml具有平台无关性,你可以在服务端用.net用发布web服务,然后客户端用
        　/// Java写一段applit小程序来处理发送过来的文件,当然我举的例子几乎没有任何显示意义,它却给了我们不少的启示.
        　/// 另外如果你的解决方案是基于多平台的,那么他们之间的交互最好不要用远程应用程序接口调用(RPC),应该尽量用基于文档的交互,
        　/// 比如说.net下的MSMQ,j2ee的JMQ.
        　/// 
        　/// 示例中设计到好多的类,我并没有在所有的地方做过多注释,有不明白的地方请参阅MSDN,这是偶第一个windows程序,有不对的地方
        　/// 欢迎各位指导 
        　/// </summary>
        partial class 文件转XML : System.Windows.Forms.Form
        {
            public 文件转XML()
            {
                InitializeComponent();
            }

            private void 文件转XML_Load(object sender, EventArgs e)
            {

            }

   

       
            private void button1_Click(object sender, System.EventArgs e)
            {
                saveFileDialog1.Filter = "xml 文件|*.xml";//设置打开对话框的文件过滤条件
                saveFileDialog1.Title = "保存成 xml 文件";//设置打开对话框的标题
                saveFileDialog1.FileName = "";
                saveFileDialog1.ShowDialog();//打开对话框
                if (saveFileDialog1.FileName != "")//检测用户是否输入了保存文件名
                {
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(strFileName);
                workbook.SaveAsXml(saveFileDialog1.FileName);
           
                    MessageBox.Show("保存成功");
                }
            }
         
         
            public void Form1_Load(object sender, System.EventArgs e)
            {
                MessageBox.Show("欢迎使用蛙蛙牌文档转换器");
            }
            /// <summary>
            　　/// 卸载窗体时把临时变量全部释放
            　　/// </summary>
            　　/// <param name="sender"></param>
            　　/// <param name="e"></param>
            public void Form1_Closed(object sender, System.EventArgs e)
            {
               
            }

        string strFileName;
        /// <summary>
        　　/// 加载office文件并编码序列花为一个XmlDocument变量
        　　/// </summary>
        　　/// <param name="sender"></param>
        　　/// <param name="e"></param>
        private void button3_Click(object sender, System.EventArgs e)
            {
              
              //  openFileDialog1.Filter = "Office Documents(*.doc, *.xls, *.ppt)|*.doc;*.xls;*.ppt";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.FileName = "";
                openFileDialog1.ShowDialog();
                strFileName = openFileDialog1.FileName;
                if (strFileName.Length != 0)
                {
                   
                    MessageBox.Show("加载成功");
                }
            }
           


    }
    }

