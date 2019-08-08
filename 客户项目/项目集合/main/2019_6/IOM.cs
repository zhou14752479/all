using System;
using System.Collections;
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

namespace main._2019_6
{
    public partial class IOM : Form
    {
        public IOM()
        {
            InitializeComponent();
        }

        #region  获取文件夹内的所有.html文件
        public static ArrayList GetFiles(string filePath)
        {
            ArrayList lists = new ArrayList();
            if (!Directory.Exists(filePath))
            {

                return lists;
            }

            //创建一个DirectoryInfo的类
            DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
            //获取当前的目录的文件
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            foreach (FileInfo info in fileInfos)
            {
                //获取文件的名称(包括扩展名)
                string fullName = info.FullName;
                //获取文件的扩展名
                string extension = info.Extension.ToLower();
                if (extension == ".html")
                {
                    //获取文件的大小
                    long length = info.Length;
                    lists.Add(fullName);

                }
            }

            return lists;
        }
        #endregion

        private void IOM_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            ArrayList lists = new ArrayList();

            lists = GetFiles(path);

            for (int i = 0; i < lists.Count; i++)
            {
                
                string html = System.IO.File.ReadAllText(lists[i].ToString());
               
                
                Match a1 = Regex.Match(html, @"fieldid=""5"" ismust=""0"">([\s\S]*?)</div>");      //订单编码
                Match a2 = Regex.Match(html, @"fieldid=""383"" ismust=""0"">([\s\S]*?)</div>");     //订单名称
                Match a3 = Regex.Match(html, @"fieldid=""391"" ismust=""0"">([\s\S]*?)</div>");  //订单内容
                Match a4 = Regex.Match(html, @"fieldid=""427"" ismust=""0"">([\s\S]*?)</div>");  //订单地址
               
                Match a5 = Regex.Match(html, @"fieldid=""1274"" ismust=""0"">([\s\S]*?) ");  //要求完成时间
                Match a6 = Regex.Match(html, @"fieldid=""396"" ismust=""0"">([\s\S]*?)</div>"); //受理人
                Match a7 = Regex.Match(html, @"fieldid=""425"" ismust=""0"">([\s\S]*?)</div>");  //客户经理名称
                Match a8 = Regex.Match(html, @"fieldid=""426"" ismust=""0"">([\s\S]*?)</div>");  //客户经理电话
                Match a9 = Regex.Match(html, @"fieldid=""384"" ismust=""0"">([\s\S]*?)</div>");   //客户名以及电话


                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据    
                lv1.SubItems.Add(a1.Groups[1].Value.Trim());
                lv1.SubItems.Add(a2.Groups[1].Value.Trim());
                lv1.SubItems.Add(a3.Groups[1].Value.Trim());
                lv1.SubItems.Add(a4.Groups[1].Value.Trim());
                lv1.SubItems.Add(a5.Groups[1].Value.Trim());
                lv1.SubItems.Add(a6.Groups[1].Value.Trim());
                lv1.SubItems.Add(a7.Groups[1].Value.Trim()+a8.Groups[1].Value.Trim());
                lv1.SubItems.Add(a9.Groups[1].Value.Trim());
            }



        }

      

        private void button3_Click(object sender, EventArgs e)
        {
            label11.Text = (Convert.ToInt32(textBox1.Text) - Convert.ToInt32(textBox3.Text)).ToString();
            label12.Text = (Convert.ToInt32(textBox1.Text)+ Convert.ToInt32(textBox2.Text) - Convert.ToInt32(textBox4.Text)-Convert.ToInt32(label11.Text)).ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
