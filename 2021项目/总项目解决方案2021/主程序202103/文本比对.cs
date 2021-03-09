using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 主程序202103
{
    public partial class 文本比对 : Form
    {
        public 文本比对()
        {
            InitializeComponent();
        }

        string path = AppDomain.CurrentDomain.BaseDirectory;
  
        List<string> valueslist1 = new List<string>();
       
        public void run()
        {
            if (textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("请选择文本AB");
                return;
            }
            
         
            valueslist1.Clear();

            try
            {

               string txt1path = textBox2.Text;
               string txt2path = textBox3.Text;
                StreamReader sr = new StreamReader(txt1path, method.EncodingType.GetTxtType(txt1path));
                //一次性读取完 
                string texts = sr.ReadToEnd();

                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {
                    label1.Text = DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss") + "：正在读取.." + "\r\n";
                    valueslist1.Add(text[i].Trim());
                }
                sr.Close();
                label1.Text = DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss") + "：正在比对.." + "\r\n";
                bidui(valueslist1);


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }


        public void bidui(List<string> list)
        {
            //string name = DateTime.Now.ToString("HHMMss") + ".txt";
            //FileStream fs1 = new FileStream(path+"新文本//" + name, FileMode.Create, FileAccess.Write);//创建写入文件 
            //StreamWriter sw = new StreamWriter(fs1);
            try
            {
                StreamReader sr = new StreamReader(textBox3.Text.Trim(), method.EncodingType.GetTxtType(textBox3.Text.Trim()));
                //一次性读取完 
                string texts = sr.ReadToEnd();

                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {
                    if (valueslist1.Contains(text[i].Trim()) && text[i].Trim()!="")
                    {
                        label1.Text = text[i]+"\r\n";
                        // sw.WriteLine(text[i]);
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(text[i]);
                    }
                }
                sr.Close();
                //sw.Close();
                //fs1.Close();
                //sw.Dispose();
                label1.Text = "【已完成】";

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }


        Thread t;

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"N2nhD6"))
            {
                MessageBox.Show("");
                return;
            }



            #endregion

            
            if (t == null || !t.IsAlive)
            {
                t = new Thread(run);
                t.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void 文本比对_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.ListviewToTxt(listView1,1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
           OpenFileDialog sfd = new OpenFileDialog();
            sfd.Filter = "txt|*.txt";
            sfd.Title = "txt文件导出";
           

            if (sfd.ShowDialog() == DialogResult.OK)
            {
               textBox2.Text= sfd.FileName;
            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Filter = "txt|*.txt";
            sfd.Title = "txt文件导出";
          

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = sfd.FileName;
            }
        }
    }
}
