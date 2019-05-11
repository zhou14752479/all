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
    public partial class Form10 : Form
    {
        public Form10()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        bool status = true;

        /// <summary>
        /// 获取所有值
        /// </summary>
        /// <returns></returns>
        public ArrayList getListviewValue()

        {
            ArrayList values = new ArrayList();

            for (int i = 0; i < listView2.Items.Count; i++)
            {
                ListViewItem item = listView2.Items[i];
                for (int j = 0; j < item.SubItems.Count; j++)
                {
                    values.Add(item.SubItems[j].Text);

                }


            }

            return values;

        }

        /// <summary>
        /// 获取第二列
        /// </summary>
        /// <returns></returns>
        public ArrayList getListviewValue1(ListView listview)

        {
            ArrayList values = new ArrayList();

            for (int i = 0; i < listview.Items.Count; i++)
            {
                ListViewItem item = listview.Items[i];

                values.Add(item.SubItems[1].Text);


            }

            return values;

        }
        ArrayList finishes = new ArrayList();
      
        #region  主函数
        public void baidu()

        {
            ArrayList lists = getListviewValue1(listView2);

            try

            {

                foreach (string URL in lists)
                {
                    string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹


                    string html = method.GetUrl(URL, "utf-8");


                    string prttern = @"<a[^>]*href=([""'])?(?<href>[^'""]+)\1[^>]*>";
                    MatchCollection aurls = Regex.Matches(html, prttern);


                

                    if (this.status == false)
                        return;




                    for (int i = 0; i < aurls.Count; i++)
                    {

                                                  
                            string html2 = method.GetUrl(aurls[i].Groups["href"].Value, "utf-8");
                            string prttern1 = @"https?://(?:\w+\.)*?(\w*\.(?:com\.cn|cn|com|net))[\\\/]*";
                            MatchCollection mathes = Regex.Matches(html2, prttern1);

                            for (int j = 0; j < mathes.Count; j++)
                            {

                            if (!finishes.Contains(mathes[j].Groups[0].Value))
                            {
                                finishes.Add(mathes[j].Groups[0].Value);
                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(mathes[j].Groups[0].Value);

                                if (listView1.Items.Count - 1 >1)
                                {
                                    listView1.EnsureVisible(listView1.Items.Count - 1);
                                }
                            }


                        }

                        }

                    
                          
                      
                        
                    
                }
            }

            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        #endregion

        
        private void skinButton2_Click(object sender, EventArgs e)
        {
            this.status = true;

            if (listView2.Items.Count < 1)
            {

                MessageBox.Show("请先添加网址");
                return;
            }



            //for (int i = 0; i < Convert.ToInt32(textBox4.Text); i++)
            //{
               
                    Thread thread = new Thread(new ThreadStart(baidu));
                    thread.Start();
               
           // }

        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            this.status = false;

          }


        private void Form10_Load(object sender, EventArgs e)
        {
    
        }

        private void skinButton6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();         
        }


        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {
                 
                        ListViewItem lv2 = listView2.Items.Add(listView2.Items.Count.ToString()); //使用Listview展示数据
                        lv2.SubItems.Add(text[i]);

                    
                }
            }
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void skinButton7_Click(object sender, EventArgs e)
        {
            ListViewItem lv2 = listView2.Items.Add(listView2.Items.Count.ToString()); //使用Listview展示数据
            lv2.SubItems.Add(textBox1.Text);
        }

        private void textBox1_MouseEnter(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
