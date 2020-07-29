using Newtonsoft.Json;
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

namespace _58二手房
{
    public partial class 二手房202008更新 : Form
    {

        class areasClass
        {


        }

        public 二手房202008更新()
        {
            InitializeComponent();
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;
        public void getArea()
        {

            //读取自定目录下的json文件
            StreamReader sr = new StreamReader(path+@"area.json");
            string json = sr.ReadToEnd();
            //json文件转为 对象  T 创建的类 字段名 应该和json文件中的保持一致     
            var data = JsonConvert.DeserializeObject<areasClass>(json);
            textBox1.Text = data.ToString();

        }
        private void 二手房202008更新_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            getArea();
        }
    }
}
