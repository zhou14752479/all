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

namespace 币种价格对比
{
    public partial class 币种价格对比 : Form
    {
        public 币种价格对比()
        {
            InitializeComponent();
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            skinEngine1.SkinFile = path + "//Skins//" + comboBox1.Text;
        }

        private void 币种价格对比_Load(object sender, EventArgs e)
        {
           
            DirectoryInfo d = new DirectoryInfo(path+"//Skins//");
            FileInfo[] files = d.GetFiles();//文件
            foreach (FileInfo f in files)
            {
               comboBox1.Items.Add(f.Name);//添加文件名到列表中  
            }
        }
    }
}
