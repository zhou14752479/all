﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 京东图片
{
    public partial class 京东图片 : Form
    {
        public 京东图片()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
             //openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
           openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox1.Text = openFileDialog1.FileName;
                DataTable dt = method.ExcelToDataTable(textBox1.Text, true);
                dataGridView1.DataSource= dt;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);

            method.DataTableToExcel(method.DgvToTable(dataGridView1), "Sheet1", true);
           // function.DataTableToExcel(method.DgvToTable(dataGridView1), "京东报备商品信息采集列表.xlsx");
        }


        /// <summary>
        /// 获取详情图
        /// </summary>
        /// <returns></returns>
        public string getxqpic(string aurl)
        {
            string url = "https:"+aurl;
            string html = method.GetUrl(url, "utf-8");

            MatchCollection pics = Regex.Matches(html, @"background-image:url\(([\s\S]*?)\)");
            StringBuilder sb = new StringBuilder();
            foreach (Match item in pics)
            {
                sb.Append("https:" + item.Groups[1].Value + "\r\n");
            }
            if (pics.Count==0)
            {
                pics = Regex.Matches(html, @"http([\s\S]*?)\\");
                foreach (Match item in pics)
                {

                    sb.Append("http" + item.Groups[1].Value + "\r\n");
                }
            }

            if (pics.Count == 0)
            {
                pics = Regex.Matches(html, @"lazyload=\\""([\s\S]*?)\\");
                foreach (Match item in pics)
                {

                    sb.Append("https://" + item.Groups[1].Value + "\r\n");
                }
            }

            //MessageBox.Show(sb.ToString());
            return sb.ToString();
        }


        string path = AppDomain.CurrentDomain.BaseDirectory+"//图片//";

        public void run()
        {
            try
            {
               


               
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {


                    try
                    {
                        string itemid = Regex.Match(dataGridView1.SelectedRows[i].Cells[21].Value.ToString().Trim(), @"\d{7,20}").Groups[0].Value;
                        label1.Text = DateTime.Now.ToString() + "正在获取：" + itemid;

                        string url = "https://item.jd.com/" + itemid + ".html";

                        string html = method.GetUrl(url, "utf-8");
                        string cate = Regex.Match(html, @"catName: \[([\s\S]*?)\]").Groups[1].Value.Trim().Replace("\"", "");
                        string title = Regex.Match(html, @"name: '([\s\S]*?)'").Groups[1].Value.Trim();
                        string pinpai = Regex.Match(html, @"mbNav-5"">([\s\S]*?)</a>").Groups[1].Value.Trim();
                        string xinghao = Regex.Match(html, @"data-value=""([\s\S]*?)""").Groups[1].Value.Trim();

                        string zhupic_a = Regex.Match(html, @"imageList: \[([\s\S]*?)\]").Groups[1].Value.Trim().Replace("\"", "");
                        string zhupic_gs = "jpg";
                        if(zhupic_a.Contains("png"))
                        {
                            zhupic_gs = "png";
                        }

                        string[] cates = cate.Split(new string[] { "," }, StringSplitOptions.None);
                        string[] zhupics = zhupic_a.Split(new string[] { "," }, StringSplitOptions.None);





                        string cate_1= Regex.Match(html, @"mbNav-1"">([\s\S]*?)</a>").Groups[1].Value.Trim();
                        string cate_2 = Regex.Match(html, @"mbNav-2"">([\s\S]*?)</a>").Groups[1].Value.Trim();
                        string cate_3 = Regex.Match(html, @"mbNav-3"">([\s\S]*?)</a>").Groups[1].Value.Trim();
                      

                        //类目识别不到情况
                        if (cate!="")
                        {
                             cate_1 = cates[0];
                             cate_2 = cates[1];
                             cate_3 = cates[2];
                        }
                        if(pinpai=="")
                        {
                            pinpai = Regex.Match(html, @"mbNav-4"">([\s\S]*?)</a>").Groups[1].Value.Trim();
                        }
                        
                       







                        StringBuilder zhupic = new StringBuilder();
                        for (int a = 0; a < zhupics.Length; a++)
                        {


                            //  string picurl = "https://img14.360buyimg.com/n1/" + zhupics[a];  //小图不带码
                            //string picurl = "https://img14.360buyimg.com/n0/"+ zhupics[a];  //大图带码
                            string picurl = "https://img14.360buyimg.com/imgzone/" + zhupics[a];  //大图不带码
                            if (Directory.Exists(path + itemid))
                            {
                                Directory.CreateDirectory(path + itemid);
                            }

                            method.downloadFile(picurl, path + itemid, "主图" + a + "."+zhupic_gs, "");
                        }


                        string descriptionUrl = Regex.Match(html, @"desc: '([\s\S]*?)'").Groups[1].Value.Trim();

                        string xqpic = getxqpic(descriptionUrl);

                        string[] xqpics = xqpic.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        for (int a = 0; a < xqpics.Length; a++)
                        {

                            if (xqpics[a] != "")
                            {
                                //MessageBox.Show(xqpics[a]);
                                try
                                {
                                    method.downloadFile(xqpics[a].Replace("////","//"), path + itemid, "详情图" + a + ".jpg", "");
                                }
                                catch (Exception)
                                {

                                    continue;
                                }
                            }


                        }
                      
                        dataGridView1.SelectedRows[i].Cells[0].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        dataGridView1.SelectedRows[i].Cells[32].Value = title;
                        dataGridView1.SelectedRows[i].Cells[33].Value = cate_1;
                        dataGridView1.SelectedRows[i].Cells[34].Value = cate_2;
                        dataGridView1.SelectedRows[i].Cells[35].Value = cate_3;
                        dataGridView1.SelectedRows[i].Cells[36].Value = pinpai;
                        dataGridView1.SelectedRows[i].Cells[37].Value = xinghao;
                        dataGridView1.SelectedRows[i].Cells[38].Value = itemid;
                        function.DataTableToExcel(method.DgvToTable(dataGridView1), "京东报备商品信息采集列表.xlsx");

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        continue;
                    }

                }

                MessageBox.Show("采集完成");
            }
            catch (Exception ex)
            {

               // MessageBox.Show(ex.ToString());
            }
        }


        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
        
        private void 京东图片_Load(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            DataTable dt = method.ExcelToDataTable(path+ "京东报备商品信息采集列表.xlsx", true);
            dataGridView1.DataSource = dt;
        }

        private void 京东图片_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string url = "https://wxa.jd.com/wqitem.jd.com/itemv3/wxadraw?sku=100057663881";
           textBox2.Text=(function.GetUrl(url));
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
