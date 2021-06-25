using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace webkit项目
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

        //在浏览器里执行JS代码,获取返回值
        private void button1_Click(object sender, EventArgs e)
        {
            string strScript = "function _func(){alert('你好')};";
            string result = webKitBrowser1.StringByEvaluatingJavaScriptFromString(strScript);
            MessageBox.Show(result);
        }

        private void button2_Click(object sender, EventArgs e)
        {
           

        }
    }
}
