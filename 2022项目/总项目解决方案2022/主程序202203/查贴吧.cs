using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;


namespace 主程序202203
{
    public partial class 查贴吧 : Form
    {
        public 查贴吧()
        {
            InitializeComponent();
        }


       
        bool status = true;
        bool zanting = true;
        Thread thread;

        
        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                textBox1.Text = openFileDialog1.FileName;

            }
        }



        string dingdanhao = "";
        public void run()
        {
            label1.Text = "正在查询";
            try
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, method.EncodingType.GetTxtType(textBox1.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
                for (int i = 0; i < text.Length; i++)
                {
                    if (status == false)
                        return;
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    string url = "https://82cat.com/tieba/forum/"+ System.Web.HttpUtility.UrlEncode(text[i]) + "/1";
                    string html = method.GetUrl(url,"utf-8");
                    MatchCollection a1 = Regex.Matches(html, @"<td>([\s\S]*?)</td>");

                    if (a1.Count != 0)
                    {
                        if (a1[0].Groups[1].Value != dingdanhao)
                        {

                            for (int j = 0; j < a1.Count / 3; j++)
                            {
                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(text[i]);
                                lv1.SubItems.Add(a1[3 * j].Groups[1].Value);
                                lv1.SubItems.Add(Regex.Replace(a1[(3 * j) + 1].Groups[1].Value, "<[^>]+>", ""));
                                lv1.SubItems.Add(Regex.Replace(a1[(3 * j) + 2].Groups[1].Value, "<[^>]+>", ""));


                                FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\"+DateTime.Now.ToString("yyyyMMdd")+".txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                                StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                                sw.WriteLine(a1[3 * j].Groups[1].Value);
                                sw.Close();
                                fs1.Close();
                                sw.Dispose();


                            }


                            dingdanhao = a1[0].Groups[1].Value;
                            label1.Text = text[i] + "：查询到贴吧，已记录";
                            if (listView1.Items.Count > 2)
                            {
                                this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                            }
                        }

                    }
                    else
                    {

                        label1.Text = text[i] + "：未查询到贴吧，跳过";
                    }

                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }



        }

        private void 查贴吧_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        

        
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测


            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"CUoFi"))
            {
                return;
            }

            #endregion
            status = true;

            if (thread == null || !thread.IsAlive)
            {

                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
}
