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
using MySql.Data.MySqlClient;

namespace 模拟采集
{
    public partial class 快3 : Form
    {
        public 快3()
        {
            InitializeComponent();
        }

        private void 测试_Load(object sender, EventArgs e)
        {
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("https://m.500fc99.com/openCenter/pastOpen?lcode=1407");

            
        }
        #region  插入数据

        public void insertData(string qishu, string value)
        {
            string date = qishu.Substring(0, 4);
            try
            {


                string constr = "Host =111.229.244.97;Database=caipiao;Username=root;Password=root";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();




                MySqlCommand cmd = new MySqlCommand("INSERT INTO kuai3 (qishu,haoma,date)VALUES('" + qishu.Substring(4, 4).Replace("\r\n","").Trim() + " ', '" + value.Replace("\r\n", "").Trim() + " ', '" + date.Replace("\r\n", "").Trim() + " ')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {


                    mycon.Close();

                }
                else
                {

                }


            }

            catch (System.Exception ex)
            {
                textBox1.Text = ex.ToString();
            }

            #endregion

        }

        string shangyiqi = "0";
            public void run()
        {
            Invoke(new Action(() =>
            {
                var htmldocument = (mshtml.HTMLDocument)webBrowser1.Document.DomDocument;
          
            string html = htmldocument.documentElement.outerHTML;
                //textBox1.Text = html;
          
            MatchCollection qishus = Regex.Matches(html, @"lottery-name"" data-v-3648a589="""">([\s\S]*?)期");
            MatchCollection values = Regex.Matches(html, @"Dice Dice([\s\S]*?)""");
          
            string he= Convert.ToInt32(values[0].Groups[1].Value) + Convert.ToInt32(values[1].Groups[1].Value) + Convert.ToInt32(values[2].Groups[1].Value) + "\r\n";


                if (shangyiqi != qishus[0].Groups[1].Value)
                {
                    string qishu = qishus[0].Groups[1].Value;

                    ListViewItem lv1 = listView1.Items.Add(qishu.Substring(4, 4)); //使用Listview展示数据   
                    lv1.SubItems.Add(he);


                     insertData(qishu, he);
                    shangyiqi = qishu;
                }
            }));
        }
       

        private void button1_Click(object sender, EventArgs e)
        {

            if (timer1.Enabled == false)
            {
                timer1.Interval = 1;
                timer1.Enabled = true;

            }
            else
            {
                timer1.Enabled = false;

            }

        }
        
     

        private void button4_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
            string url = comboBox1.Text.Trim();

            webBrowser1.Navigate(url);
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            comboBox1.Text = webBrowser1.Url.ToString();
        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            if (webBrowser1.Document.ActiveElement != null)
            {
                webBrowser1.Navigate(webBrowser1.Document.ActiveElement.GetAttribute("href"));
                comboBox1.Text = webBrowser1.Document.ActiveElement.GetAttribute("href");
            }
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        Thread Timerthread;
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 30000;
            if (Timerthread == null || !Timerthread.IsAlive)
            {
                Timerthread = new Thread(run);
                Timerthread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
}
