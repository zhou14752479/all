using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 基鹿工具箱
{
    public partial class 图标设置 : Form
    {
        public 图标设置()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string url = "http://47.96.189.55/jilusoft/getpic.php";
                //string url = "http://localhost/getpic.php";
                string picpath=textBox1.Text.Trim();
                WebClient client = new WebClient();
                client.Credentials = CredentialCache.DefaultCredentials;
                client.Headers.Add("Content-Type", "application/form-data");//注意头部必须是form-data
                string filename = System.IO.Path.GetFileName(picpath);
                client.QueryString["file_name"] = filename;
                byte[] fileb = client.UploadFile(new Uri(url), "POST", picpath);
                string res = Encoding.UTF8.GetString(fileb);
               if(res=="1")
                {
                    MessageBox.Show("上传成功");
                }

                string openurl = textBox2.Text;
                string sort = "1";
                string sql = "INSERT INTO icon(name,url,sort) VALUES('" + filename + " ', '" + openurl + " ', '" + sort + " ') ON DUPLICATE KEY UPDATE name = '" + filename + " ',url= '" + openurl + " ',sort= '" + sort + " '";
                Util.SQL(sql);
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "选择图片";
            openFileDialog1.Filter = "图片|*.gif;*.jpg;*.jpeg;*.bmp;*.jfif;*.png;";//限制只能选择这几种图片格式
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox1.Text = openFileDialog1.FileName;



            }
        }
        Util util = new Util();

        
        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            imageList1.Images.Clear();
            List<string> icons= util.geticons();
            for (int i = 0; i < icons.Count; i++)
            {
               
                imageList1.Images.Add(Util.GetImage(icons[i].ToString()));
                listView1.Items.Add(System.IO.Path.GetFileName(icons[i].ToString()), i);
                listView1.Items[i].ImageIndex = i;
                listView1.Items[i].Name = icons[i].ToString();
            }
           
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;
           
          string html=  util.delicon(listView1.SelectedItems[0].Text);
            MessageBox.Show(html);
            button3.PerformClick();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                string url = "http://47.96.189.55/jilusoft/gethaibao.php";
                string picpath = textBox1.Text.Trim();
                WebClient client = new WebClient();
                client.Credentials = CredentialCache.DefaultCredentials;
                client.Headers.Add("Content-Type", "application/form-data");//注意头部必须是form-data
                string filename = System.IO.Path.GetFileName(picpath);
                client.QueryString["file_name"] = filename;
                byte[] fileb = client.UploadFile(new Uri(url), "POST", picpath);
                string res = Encoding.UTF8.GetString(fileb);
                if (res == "1")
                {
                    MessageBox.Show("上传成功");
                }

                string openurl = textBox2.Text;
                string sort = "1";
                string sql = "INSERT INTO icon(name,url,sort) VALUES('" + filename + " ', '" + openurl + " ', '" + sort + " ') ON DUPLICATE KEY UPDATE name = '" + filename + " ',url= '" + openurl + " ',sort= '" + sort + " '";
                Util.SQL(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
