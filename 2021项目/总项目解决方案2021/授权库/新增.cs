using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

        function fc = new function();


        public string uid = "";
        public string type = "";
        public string name = "";
        public string pinpai = "";
        public string cate1 = "";
        public string cate2 = "";


        public string sq_starttime = "";
        public string sq_endtime = "";
        public string yjsq_starttime = "";


        public string is_yuanjian = "";
        public string is_shouhou = "";
        public string is_shangbiao = "";
        public string shangbiao_endtime = "";
        public List<string> filelist = null;


        public void add()
        {

            long uid = fc.GetTimeStamp();
            
            string type = comboBox1.Text;
            string name = textBox1.Text.Trim();
            string pinpai = textBox2.Text.Trim();
            string cate1= textBox3.Text.Trim();
            string cate2 = textBox4.Text.Trim();


           string sq_starttime = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string sq_endtime = dateTimePicker2.Value.ToString("yyyy-MM-dd");

            string yjsq_starttime = dateTimePicker3.Value.ToString("yyyy-MM-dd");
            if (checkBox1.Checked == false)
            {
                yjsq_starttime = "";
            }

            string is_yuanjian = comboBox2.Text;
            string is_shouhou = comboBox3.Text;
            string is_shangbiao = comboBox4.Text;
            string shangbiao_endtime = dateTimePicker4.Value.ToString("yyyy-MM-dd");
            if (checkBox2.Checked == false)
            {
                shangbiao_endtime = "";
            }

            // string img_shouquan = fc.ImageToBase64(Image.FromFile(textBox5.Text));
            // string img_shouhou = fc.ImageToBase64(Image.FromFile(textBox5.Text));
            string sql = "INSERT INTO datas (type,name,pinpai,cate1,cate2,sq_starttime,sq_endtime,yjsq_starttime,is_yuanjian,is_shouhou,is_shangbiao,shangbiao_endtime,uid)VALUES('" + type + " '," +
                " '" + name + " '," +
                " '" + pinpai+ " ', " +
                "'" + cate1 + " '," +
                " '" + cate2 + " '," +
                 " '" + sq_starttime + " '," +
                  " '" + sq_endtime + " '," +
                   " '" + yjsq_starttime + " '," +
                    " '" + is_yuanjian + " '," +
                     " '" + is_shouhou + " '," +
                      " '" + is_shangbiao + " '," +
                       " '" + shangbiao_endtime + " '," +
                        " '" + uid + " ')";
            fc.SQL(sql);

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                string filename = listView1.Items[i].SubItems[1].Text;
                if (filename.Trim() == "")
                    continue;
                fc.insertfile(uid.ToString(), filename);

            }
            MessageBox.Show("文件添加成功！");

        }



        public void xiugai()
        {

          
            string type = comboBox1.Text;
            string name = textBox1.Text.Trim();
            string pinpai = textBox2.Text.Trim();
            string cate1 = textBox3.Text.Trim();
            string cate2 = textBox4.Text.Trim();


            string sq_starttime = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string sq_endtime = dateTimePicker2.Value.ToString("yyyy-MM-dd");

            string yjsq_starttime = dateTimePicker3.Value.ToString("yyyy-MM-dd");
            if (checkBox1.Checked == false)
            {
                yjsq_starttime = "";
            }

            string is_yuanjian = comboBox2.Text;
            string is_shouhou = comboBox3.Text;
            string is_shangbiao = comboBox4.Text;
            string shangbiao_endtime = dateTimePicker4.Value.ToString("yyyy-MM-dd");
            if (checkBox2.Checked == false)
            {
                shangbiao_endtime = "";
            }

            string sql = "update datas set type= '" + type + " ', " +
                "name='" + name + " '," +
                "pinpai='" + pinpai+ " ' ," +
                "cate1='" + cate1 + " '," +
                "cate2='" + cate2+ " '," +
                "sq_starttime='" + sq_starttime + " '," +
                "sq_endtime='" + sq_endtime + " '," +
                "yjsq_starttime='" + yjsq_starttime + " '," +
                "is_yuanjian='" + is_yuanjian + " '," +
                "is_shouhou='" + is_shouhou + " '," +
                "is_shangbiao='" + is_shangbiao + " '," +
                "shangbiao_endtime ='" + shangbiao_endtime + " ' where uid='" + uid + " '";


           
            fc.SQL(sql);

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                string filename = listView1.Items[i].SubItems[1].Text;
                if (filename.Trim() == "")
                    continue;
                fc.insertfile(uid.ToString(), filename);

            }
           

        }

        private void 新增_Load(object sender, EventArgs e)
        {
            if(this.Text=="查看数据")
            {
                button3.Visible=false;

            }

            if (this.Text == "修改数据")
            {
                button3.Visible = true;
                button3.Text = "确定修改";
            }

            if (type != "")
            {
                comboBox1.Text= type ;
                textBox1.Text = name;
                textBox2.Text = pinpai;
                textBox3.Text = cate1;
                textBox4.Text= cate2;


                dateTimePicker1.Value= Convert.ToDateTime(sq_starttime);
                dateTimePicker2.Value= Convert.ToDateTime(sq_endtime);
                if(yjsq_starttime!="" && yjsq_starttime!=null)
                {
                    checkBox1.Checked = true;
                    dateTimePicker3.Value = Convert.ToDateTime(yjsq_starttime);
                }
                


                comboBox2.Text= is_yuanjian;
                comboBox3.Text=is_shouhou ;
                comboBox4.Text= is_shangbiao;
                if (shangbiao_endtime != "" && shangbiao_endtime != null)
                {
                    checkBox2.Checked = true;
                    dateTimePicker4.Value = Convert.ToDateTime(shangbiao_endtime);
                }
              
            }


            if (filelist != null )
            {
                foreach (var item in filelist)
                {

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = true;//等于true表示可以选择多个文件
            //dlg.DefaultExt = ".txt";
           // dlg.Filter = "记事本文件|*.txt";
            if (dlg.ShowDialog() == DialogResult.OK)
            {

                foreach (string file in dlg.FileNames)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(file);
                   
                }

            }



        }

      


        Thread thread;
        private void button3_Click(object sender, EventArgs e)
        {
            if (this.Text == "新增数据")
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(add);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }

            }
            if (this.Text == "修改数据")
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(xiugai);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }

            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
