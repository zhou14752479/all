using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 主程序202104
{
    public partial class 数据分类 : Form
    {
        public 数据分类()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            // openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.Filter = "文本文件(*.txt)|*.txt";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox1.Text = openFileDialog1.FileName;

                StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                string[] text1 = text[0].Split(new string[] { ";" }, StringSplitOptions.None);

                for (int i = 0; i < text1.Length; i++)
                {
                    string[] value = text1[i].Split(new string[] { "->" }, StringSplitOptions.None);
                    StringBuilder sb = new StringBuilder();
                    if(texts.Contains(value[0]+ "->3"))
                    {
                        sb.Append("3 ");
                     }
                    if (texts.Contains(value[0] + "->1"))
                    {
                        sb.Append("1 ");
                    }
                    if (texts.Contains(value[0] + "->0"))
                    {
                        sb.Append("0 ");
                    }


                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(value[0]);
                    lv1.SubItems.Add(sb.ToString());
                }


                string[] a1 = text[0].Split(new string[] { "->" }, StringSplitOptions.None);
                string a2 = a1[a1.Length - 1].Remove(0, 1);
                string a3 = a2.Remove(a2.Length-2,2);
                StringBuilder sb1 = new StringBuilder();
                if (texts.Contains(a3 + "_3"))
                {
                    sb1.Append("3 ");
                }
                if (texts.Contains(a3 + "_1"))
                {
                    sb1.Append("1 ");
                }
                if (texts.Contains(a3 + "_0"))
                {
                    sb1.Append("0 ");
                }

                ListViewItem lv11 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                lv11.SubItems.Add(a3);
                lv11.SubItems.Add(sb1.ToString());


            }
        }


        //public void run()
        //{
        //    try
        //    {

        //        List<Person> list = new List<Person>();
        //        StringBuilder sb = new StringBuilder();


        //        for (int i = 0; i < listView1.Items.Count; i++)
        //        {
        //            string v1 = Regex.Match(listView1.Items[i].SubItems[1].Text, @"68->\d").Groups[0].Value.Replace("68->",""); ;
        //            string v2 = Regex.Match(listView1.Items[i].SubItems[1].Text, @"71->\d").Groups[0].Value.Replace("71->", ""); ;
        //            string v3 = Regex.Match(listView1.Items[i].SubItems[1].Text, @"73->\d").Groups[0].Value.Replace("73->", ""); ;
        //            string v4 = Regex.Match(listView1.Items[i].SubItems[1].Text, @"75->\d").Groups[0].Value.Replace("75->", ""); ;
        //            string v5 = Regex.Match(listView1.Items[i].SubItems[1].Text, @"76->\d").Groups[0].Value.Replace("76->", ""); ;
        //            string v6 = Regex.Match(listView1.Items[i].SubItems[1].Text, @"77->\d").Groups[0].Value.Replace("77->", ""); ;
        //            string v7 = Regex.Match(listView1.Items[i].SubItems[1].Text, @"79->\d").Groups[0].Value.Replace("79->", ""); ;

        //            Person p = new Person();
        //            p.id =i;
        //            p.value1 = Convert.ToInt32(v1);
        //            p.value2 = Convert.ToInt32(v2);
        //            p.value3 = Convert.ToInt32(v3);
        //            p.value4 = Convert.ToInt32(v4);
        //            p.value5 = Convert.ToInt32(v5);
        //            p.value6 = Convert.ToInt32(v6);
        //            p.value7 = Convert.ToInt32(v7);
        //            list.Add(p);
                    
        //        }


        //        list = list.OrderBy(p => p.value1).ThenBy(p => p.value2).ThenBy(p => p.value3).ThenBy(p => p.value4).ThenBy(p => p.value5).ThenBy(p => p.value6).ThenBy(p => p.value7).ToList();

        //        for (int i = 0; i < list.Count; i++)
        //        {
        //            ListViewItem lv2 = listView2.Items.Add(listView2.Items.Count.ToString()); //使用Listview展示数据
        //            lv2.SubItems.Add(listView1.Items[list[i].id].SubItems[1].Text);
                   
        //        }

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //Thread thread;
       
        //public class Person
        //{
        //    public int id { get; set; }
        //    public int value1 { get; set; }
        //    public int value2 { get; set; }
        //    public int value3 { get; set; }
        //    public int value4 { get; set; }
        //    public int value5 { get; set; }
        //    public int value6 { get; set; }
        //    public int value7 { get; set; }

        //}
       
  


        private void 数据分类_Load(object sender, EventArgs e)
        {
            
        }
    }
}
