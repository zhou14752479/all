using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 主程序
{
    public partial class 京东 : Form
    {
        public 京东()
        {
            InitializeComponent();
        }
        bool zanting = true;
        ArrayList finishes = new ArrayList();

        /// <summary>
        /// 插入数据库
        /// </summary>
        public void insertdata(string sql)
        {
            try
            {

                string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\datajd.db");
                mycon.Open();

                SQLiteCommand cmd = new SQLiteCommand(sql, mycon);

                cmd.ExecuteNonQuery();  //执行sql语句
                mycon.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.ToString());

            }

        }

        long a = 0;
        #region  主程序
        public void run()
        {
            
            try
            {
                for (long i = Convert.ToInt64(textBox1.Text); i < Convert.ToInt64(textBox1.Text) + Convert.ToInt64(textBox2.Text); i = i + 1)
                {
                    if (!finishes.Contains(i))
                    {
                        finishes.Add(i);
                        string Url = "https://item.m.jd.com/product/" + i + ".html";

                        string html = method.GetUrl(Url, "utf-8"); ;  //定义的GetRul方法 返回 reader.ReadToEnd()
                        Match title = Regex.Match(html, @"<title>([\s\S]*?)</title>");
                        Match price = Regex.Match(html, @"""p"":""([\s\S]*?)""");
                        label4.Text = "已抓取"+a;
                        insertdata("INSERT INTO data (bianma,title,price) VALUES( '" + i.ToString() + "','" + title.Groups[1].Value + "','" + price.Groups[1].Value + "')");
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        a = a + 1;
                    }

                }
                label4.Text = "已完成。。。。。" ;

            }


            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        #endregion
        private void 京东_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"jdsoftware"))
            {
                if (zanting == false)
                {
                    zanting = true;
                }

                else
                {
                    for (int i = 0; i < numericUpDown1.Value; i++)
                    {
                        Thread thread = new Thread(new ThreadStart(run));
                        Control.CheckForIllegalCrossThreadCalls = false;
                        thread.Start();
                    }
                }






            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void 京东_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }
    }
}
