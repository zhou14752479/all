using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 天猫店铺采集
{
    public partial class 虚拟娱乐 : Form
    {
        public 虚拟娱乐()
        {
            InitializeComponent();
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;
        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double total = 0;
            StreamReader sr = new StreamReader(path + "2.txt", method.EncodingType.GetTxtType(path + "2.txt"));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            //MessageBox.Show(text.Length.ToString());
            for (int i = 0; i < text.Length; i++)
            {
                ListViewItem lv1 = listView2.Items.Add(listView2.Items.Count.ToString()); //使用Listview展示数据
                string[] v = text[i].Split(new string[] { "#" }, StringSplitOptions.None);

                foreach (string item in v)
                {
                    lv1.SubItems.Add(item.Trim());
                }
                string je = Regex.Match(v[3],@"现金：([\s\S]*?)元").Groups[1].Value;
                total = total + Convert.ToDouble(je);
                label2.Text=total.ToString("0.00");   
            }


            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存
        }


    }
}
