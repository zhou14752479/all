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

namespace 启动程序
{
    public partial class 微博转赞评 : Form
    {
        public 微博转赞评()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            添加微博 tj = new 添加微博();
            tj.Show();

        }

        public   void add()
        {
            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
            lv1.SubItems.Add(添加微博.beizhu);
           
            lv1.SubItems.Add(添加微博.jishua);
            lv1.SubItems.Add(添加微博.rengong);
            lv1.SubItems.Add(添加微博.cishu);

            添加微博.beizhu = "";
            添加微博.url = "";
            添加微博.jishua = "";
            添加微博.rengong = "";
            添加微博.cishu="";

        }

        private void 微博转赞评_Load(object sender, EventArgs e)
        {

            #region  记录值
            foreach (Control ctr in groupBox1.Controls)
            {

                if (ctr is TextBox)
                {

                    string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
                    if (File.Exists(path + ctr.Name + ".txt"))
                    {

                        StreamReader sr = new StreamReader(path + ctr.Name + ".txt", Encoding.GetEncoding("utf-8"));
                        //一次性读取完 
                        string texts = sr.ReadToEnd();
                        ctr.Text = texts;
                        sr.Close();
                    }
                }
            }

            foreach (Control ctr in groupBox2.Controls)
            {

                if (ctr is TextBox)
                {

                    string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
                    if (File.Exists(path + ctr.Name + ".txt"))
                    {

                        StreamReader sr = new StreamReader(path + ctr.Name + ".txt", Encoding.GetEncoding("utf-8"));
                        //一次性读取完 
                        string texts = sr.ReadToEnd();
                        ctr.Text = texts;
                        sr.Close();
                    }
                }
            }

            foreach (Control ctr in groupBox2.Controls)
            {

                if (ctr is TextBox)
                {

                    string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
                    if (File.Exists(path + ctr.Name + ".txt"))
                    {

                        StreamReader sr = new StreamReader(path + ctr.Name + ".txt", Encoding.GetEncoding("utf-8"));
                        //一次性读取完 
                        string texts = sr.ReadToEnd();
                        ctr.Text = texts;
                        sr.Close();
                    }
                }
            }

            #endregion


        }

        private void 微博转赞评_FormClosing(object sender, FormClosingEventArgs e)
        {
            #region 关闭
            DialogResult result = MessageBox.Show("确认退出吗？", "退出询问"
            , MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK)
            {
                e.Cancel = true;//告诉窗体关闭这个任务取消

            }
            else
            {
                foreach (Control ctr in groupBox1.Controls)
                {
                    if (ctr is TextBox)
                    {


                        string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
                        FileStream fs1 = new FileStream(path + ctr.Name + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                        StreamWriter sw = new StreamWriter(fs1);
                        sw.WriteLine(ctr.Text.Trim());
                        sw.Close();
                        fs1.Close();

                    }
                }

                foreach (Control ctr in groupBox2.Controls)
                {
                    if (ctr is TextBox)
                    {


                        string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
                        FileStream fs1 = new FileStream(path + ctr.Name + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                        StreamWriter sw = new StreamWriter(fs1);
                        sw.WriteLine(ctr.Text.Trim());
                        sw.Close();
                        fs1.Close();

                    }
                }

                foreach (Control ctr in groupBox3.Controls)
                {
                    if (ctr is TextBox)
                    {


                        string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
                        FileStream fs1 = new FileStream(path + ctr.Name + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                        StreamWriter sw = new StreamWriter(fs1);
                        sw.WriteLine(ctr.Text.Trim());
                        sw.Close();
                        fs1.Close();

                    }
                }



                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            #endregion

        }

        private void ListView1_MouseEnter(object sender, EventArgs e)
        {
            if (添加微博.beizhu!= "")
            {
                add();

            }
            
        }
    }
    }
