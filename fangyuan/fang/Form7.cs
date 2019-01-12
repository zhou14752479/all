using MySql.Data.MySqlClient;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fang
{
    public partial class Form7 : Form
    {

        bool status = true;
        int page;
        public Form7()
        {
            InitializeComponent();
        }
        public string[] ReadText()
        {

            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
            StreamReader sr = new StreamReader(textBox1.Text);
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            return text;


        }
        
       

      
        #region  赶集本地服务
        public void run()
        {

            page = Convert.ToInt32(textBox3.Text);
            if (textBox2.Text=="")
            {
                MessageBox.Show("请输入二级网址！");
                return;
            }

            try
            {

                string[] URLs = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                foreach (string URL  in URLs)
                {



                    for (int i = 1; i < page; i++)
                    {



                        string url = URL + "o"+i + "/";
                        
                        Match citymatch = Regex.Match(url, @"//([\s\S]*?)\.");
                        string city = citymatch.Groups[1].Value;
                       
                        string html = method.GetUrl(url, "utf-8");

                         MatchCollection urls = Regex.Matches(html, @"<a class=""f14 list-info-title"" target=""_blank"" href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        MatchCollection urls2 = Regex.Matches(html, @"<a class=""f14 list-info-title js_wuba_stas"" target=""_blank"" href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        ArrayList lists = new ArrayList();
                        ArrayList lists2 = new ArrayList();
                        foreach (Match NextMatch in urls2)
                        {
                            lists.Add("http:" + NextMatch.Groups[1].Value);

                        }


                        foreach (Match match in urls)
                        {
                            lists2.Add("http://" + city + ".ganji.com" + match.Groups[1].Value);

                        }

                        if (lists2.Count == 0 && lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;


                        foreach (string list in lists)
                        {
                           textBox1.Text = list;

                            //联系人网址与其他网址不同
                            string strhtml = method.GetUrl(list, "utf-8");
                            string strhtmlTel = method.GetUrl(list + "contactus/", "utf-8");
                            Match tel = Regex.Match(strhtml, @"@phone=([\s\S]*?)@");
                            Match addr = Regex.Match(strhtml, @"<p class=""fl"">([\s\S]*?)</p>");
                            Match lxr = Regex.Match(strhtmlTel, @"人：</span>([\s\S]*?)</p>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                            Match company = Regex.Match(strhtmlTel, @"<h1>([\s\S]*?)</h1>");
                            Match infos = Regex.Match(strhtml, @"<div class=""txt"" id=""real_service_about"">([\s\S]*?)</div>");


                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                            lv1.SubItems.Add(Regex.Replace(lxr.Groups[1].Value, "<[^>]*>", "").Trim());
                            lv1.SubItems.Add(tel.Groups[1].Value.Trim());
                            lv1.SubItems.Add(Regex.Replace(addr.Groups[1].Value.Trim(), "<[^>]*>", ""));
                            lv1.SubItems.Add(company.Groups[1].Value.Trim());
                            lv1.SubItems.Add(Regex.Replace(infos.Groups[1].Value.Trim(), "<[^>]*>", ""));


                            Application.DoEvents();
                            Thread.Sleep(Convert.ToInt32(1000));



                            if (listView1.Items.Count > 1)
                            {
                                listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                            }

                            while (this.status == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }

                        }

                        foreach (string list2 in lists2)
                        {
                            textBox1.Text = list2;
                            string strhtml = method.GetUrl(list2, "utf-8");


                            Match tel = Regex.Match(strhtml, @"@phone=([\s\S]*?)@");
                            Match addr = Regex.Match(strhtml, @"<p class=""fl"">([\s\S]*?)</p>");
                            Match lxr = Regex.Match(strhtml, @"人：</span>([\s\S]*?)<!--webim start-->", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                            Match company = Regex.Match(strhtml, @"<li class=""fb"">([\s\S]*?)</li>");
                            Match infos = Regex.Match(strhtml, @"<div class=""txt"" id=""real_service_about"">([\s\S]*?)</div>");

                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                            lv1.SubItems.Add(Regex.Replace(lxr.Groups[1].Value, "<[^>]*>", "").Trim());
                            lv1.SubItems.Add(tel.Groups[1].Value.Trim());
                            lv1.SubItems.Add(Regex.Replace(addr.Groups[1].Value.Trim(), "<[^>]*>", ""));
                            lv1.SubItems.Add(company.Groups[1].Value.Trim());
                            lv1.SubItems.Add(Regex.Replace(infos.Groups[1].Value.Trim(), "<[^>]*>", ""));


                            Application.DoEvents();
                            Thread.Sleep(Convert.ToInt32(1000));

                            while (this.status == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                        }

                    }

                }
            }


            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion


        #region  百姓本地服务
        public void run1()
        {

            page = Convert.ToInt32(textBox3.Text);

            if (textBox2.Text == "")
            {
                MessageBox.Show("请输入二级网址！");
                return;
            }

            try
            {
                string[] URLs = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                foreach (string URL in URLs)
                {


                    for (int i = 0; i < page; i++)
                    {



                        string url = URL + "?page=" + i;


                        string html = method.GetUrl(url, "utf-8");
                        textBox1.Text = html;
                        MatchCollection urls = Regex.Matches(html, @"</button></span><a href='([\s\S]*?)'", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                        ArrayList lists = new ArrayList();

                        foreach (Match NextMatch in urls)
                        {
                            lists.Add(NextMatch.Groups[1].Value);

                        }



                        if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;


                        foreach (string list in lists)
                        {
                            textBox1.Text = list;

                            //联系人网址与其他网址不同
                            string strhtml = method.GetUrl(list, "utf-8");

                            Match tel = Regex.Match(strhtml, @"<strong>([\s\S]*?)</strong>");
                            Match addr = Regex.Match(strhtml, @"所在地：</label><div class='content'><span>([\s\S]*?)</span>");
                            Match lxr = Regex.Match(strhtml, @"联系人：</label><div class='content'><span>([\s\S]*?)</span>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                            Match infos = Regex.Match(strhtml, @"<div class='viewad-text'>([\s\S]*?)</div>");

                            ListViewItem lv1 = listView1.Items.Add(Regex.Replace(lxr.Groups[1].Value, "<[^>]*>", "").Trim());
                            lv1.SubItems.Add(tel.Groups[1].Value.Trim());
                            lv1.SubItems.Add(Regex.Replace(addr.Groups[1].Value.Trim(), "<[^>]*>", ""));
                            lv1.SubItems.Add(Regex.Replace(infos.Groups[1].Value.Trim().Replace("&zwnj;", "").Replace("&nbsp;", ""), "<[^>]*>", ""));


                            Application.DoEvents();
                            Thread.Sleep(Convert.ToInt32(1000));

                            if (listView1.Items.Count > 1)
                            {
                                listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                            }

                            while (this.status == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }

                        }



                    }

                }
            }


            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion
        private void Form7_Load(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.status = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(run));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();
            }

            else if (radioButton2.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(run1));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();
            }

            else {

                MessageBox.Show("请选择百姓或赶集？");
                return;
            }
         
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.status =true;
        }

        private void 删除此行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                listView1.Items.Remove(item);
            }
        }

        private void 保存此行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
      



                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "txt文件 (*.txt)|*.txt";
               
                saveFileDialog.FilterIndex = 0;
                saveFileDialog.FileName = "数据";
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.CreatePrompt = true;
                saveFileDialog.Title = "导出txt文件到";
                if (saveFileDialog.ShowDialog() != DialogResult.Cancel)
                {
               
                    Stream myStream;
                    myStream = saveFileDialog.OpenFile();
                    StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));
                    try
                    {
                    string tempStr = "";
                    foreach (ListViewItem item in listView1.SelectedItems)
                    {

                        
                         tempStr += item.SubItems[0].Text + "---" + item.SubItems[1].Text + "---" + item.SubItems[2].Text + "---" + item.SubItems[3].Text + "---" + "\r\n";  //导出第二列
                       
                    }

                    sw.Write(tempStr);
                    sw.Close();
                    myStream.Close();
                }

                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                    finally
                    {
                        sw.Close();
                        myStream.Close();
                    }
                
                }
       
            

        }
    }
}
