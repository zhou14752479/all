using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 外网商城
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {

            try
            {
                string[] ids = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string id in ids)
                {
                    string bhtml = method.GetUrl("https://www.homedepot.com/dynamicrecs/badges?anchor=" +id, "utf-8");  //best seller
                    Match best = Regex.Match(bhtml, @"""label"":""([\s\S]*?)""");

                    string phtml = method.GetUrl("https://www.homedepot.com/p/svcs/frontEndModel/" + id, "utf-8");  //best seller
                    Match price= Regex.Match(phtml, @"""specialPrice"":([\s\S]*?),");
                    Match storeId = Regex.Match(phtml, @"""storeId"":""([\s\S]*?)""");
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   

                    if (best.Groups[1].Value == "")
                    {
                        lv1.SubItems.Add(price.Groups[1].Value);
                        lv1.SubItems.Add("0");
                        lv1.SubItems.Add(storeId.Groups[1].Value);
                    }
                    else
                    {
                        lv1.SubItems.Add(price.Groups[1].Value);
                        lv1.SubItems.Add(best.Groups[1].Value);
                        lv1.SubItems.Add(storeId.Groups[1].Value);
                    }
                   
                    
                }
               


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled=false;
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
