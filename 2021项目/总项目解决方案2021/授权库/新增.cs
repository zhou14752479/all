using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 授权库
{
    public partial class 新增 : Form
    {
        public 新增()
        {
            InitializeComponent();
        }

        public void add()
        {
            string a1 = "";

            string type = comboBox1.Text;
            string name = textBox1.Text.Trim();
            string pinpai = textBox2.Text.Trim();
            string cate1= textBox3.Text.Trim();
            string cate2 = textBox4.Text.Trim();


           string sq_starttime = dateTimePicker1.Value.ToString();
            string sq_endtime = dateTimePicker2.Value.ToString();
            string yjsq_starttime = dateTimePicker3.Value.ToString();


            string is_yuanjian = comboBox2.Text;
            string is_shouhou = comboBox3.Text;
            string is_shangbiao = comboBox4.Text;
            string shangbiao_endtime = dateTimePicker4.Value.ToString();

            string sql = "INSERT INTO datas (username,password,register_t,ip,mac)VALUES('" + a1 + " ', '" + a1 + " ', '" + a1 + " ', '" + a1 + " ', '" + a1 + " ')";
        }
        private void 新增_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            DialogResult result = fileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(fileDialog.FileName);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            DialogResult result = fileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                pictureBox2.Image = Image.FromFile(fileDialog.FileName);
            }
        }
    }
}
