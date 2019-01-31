using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zhaopin_58
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false; // 设置线程之间可以操作
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            pictureBox1.Image = Image.FromStream(System.Net.WebRequest.Create("http://222.187.200.202:8001/ValidateCode/ValidateCode?examid=00181228141531&r=497074510&").GetResponse().GetResponseStream());

        }


        #region  主程序

        public void zhaopin()
        {

            try
            {

                string[] citys = textBox1.Text.Trim().Split(',');
                string[] keywords = textBox2.Text.Trim().Split(',');

                foreach (string city in citys)
                {
                    if (city == "")
                    {
                        MessageBox.Show("请选择城市！");
                        return;
                    }
                    foreach (string keyword in keywords)
                    {


                        for (int i = 1; i < 71; i++)
                        {
                            //String Url = string.Format("http://{0}.58.com/job/pn{1}/?key={2}&final=1&jump=1", city, i, keyword);
                            string Url = "https://sz.58.com/longgang/yewu/pn"+i+"/";
                            string html = method.GetUrl(Url);

                            textBox1.Text = html;
                            MatchCollection TitleMatchs = Regex.Matches(html, @"addition=""0""><a href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                            ArrayList lists = new ArrayList();
                            foreach (Match NextMatch in TitleMatchs)
                            {

                                lists.Add( NextMatch.Groups[1].Value);
                            }

                            Ocr ocr = new Ocr();
                            

                            foreach (string list in lists)

                            {
                                string strhtml = method.GetUrl(list);  //定义的GetRul方法 返回 reader.ReadToEnd()

                                string rxg = @"<span class=""pos_title"">([\s\S]*?)</span>";  
                                string rxg1= @"content='\[([\s\S]*?)\]";    //公司
                                string rxg2 = @"<span class=""pos_area_item"" >([\s\S]*?)</span>";
                                string rxg4 = @"</span><span>([\s\S]*?)</span>";
                                string rxg5 = @"camp_indus"",""V"":""([\s\S]*?)""";
                                string rxg6 = @"linkman:'([\s\S]*?)'";
                                string rxg7 = @"pagenum :""([\s\S]*?)""";



                                string rxg8 = @"pagenum :""([\s\S]*?)_";



                                Match job = Regex.Match(strhtml, rxg);
                                Match company = Regex.Match(strhtml, rxg1);
                                MatchCollection areas= Regex.Matches(strhtml, rxg2);
                                Match addr= Regex.Match(strhtml, rxg4);
                                Match hangye = Regex.Match(strhtml, rxg5);
                                Match lxr = Regex.Match(strhtml, rxg6);
                                Match tel= Regex.Match(strhtml, rxg7);
                                Match Tel = Regex.Match(strhtml, rxg8);


                                string telUrl = "";
                                if (!tel.Groups[1].Value.Trim().Contains("_"))
                                {
                                    telUrl = "http://image.58.com/showphone.aspx?t=v55&v=" + tel.Groups[1].Value.Trim();
                                }
                                else
                                {
                                    telUrl = "http://image.58.com/showphone.aspx?t=v55&v=" + Tel.Groups[1].Value.Trim();
                                }
                                Image telimage= Image.FromStream(System.Net.WebRequest.Create(telUrl).GetResponse().GetResponseStream());
                                pictureBox1.Image = telimage;


                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                                lv1.SubItems.Add(job.Groups[1].Value.Trim());
                                lv1.SubItems.Add(company.Groups[1].Value.Trim());
                                lv1.SubItems.Add(areas[0].Groups[1].Value.Trim());

                                if (areas.Count > 1)
                                {
                                    lv1.SubItems.Add(areas[1].Groups[1].Value.Trim());
                                }
                                else
                                {
                                    lv1.SubItems.Add("");
                                }
                                lv1.SubItems.Add(addr.Groups[1].Value.Trim());
                                lv1.SubItems.Add(hangye.Groups[1].Value.Trim());
                                lv1.SubItems.Add(lxr.Groups[1].Value.Trim());
                                lv1.SubItems.Add(ocr.OCR_sougou(telimage));




                                Application.DoEvents();
                                System.Threading.Thread.Sleep(3000);   //内容获取间隔，可变量

                            }

                        }
                    }
                }

            }
            catch (System.Exception ex)
            {
                textBox1.Text = ex.ToString();
            }
        }









        #endregion



        private void skinTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            textBox1.Text= e.Node.Name +",";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(zhaopin));
            thread.Start();
        }

      
  
        
        private void button2_Click(object sender, EventArgs e)
        {
            Ocr ocr = new Ocr();
            textBox2.Text=ocr.OCR_sougou(this.pictureBox1.Image);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
