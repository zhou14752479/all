using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 主程序202102
{
    public partial class 净水器日期监控 : Form
    {
        public 净水器日期监控()
        {
            InitializeComponent();
        }

      
        string path = AppDomain.CurrentDomain.BaseDirectory;

        bool status = false;
        public void panduan()
        {
            status = false;
            try
            {
                for (int i = 0; i <listView1.Items.Count; i++)
                {


                    string dates = listView1.Items[i].SubItems[6].Text;
                    DateTime now = DateTime.Now;
                    DateTime now3month = now.AddDays(85);
                    DateTime now6month = now.AddDays(180);
                    DateTime now12month = now.AddDays(360);
                    DateTime dt = Convert.ToDateTime(dates);
                    if (dt < now3month)
                    {
                        listView1.Items[i].ForeColor = Color.Red;
                        status = true;
                    }
                    if (dt > now3month && dt < now6month)
                    {
                        listView1.Items[i].ForeColor = Color.Blue;
                        status = true;
                    }
                    if (dt > now6month && dt < now12month)
                    {
                        listView1.Items[i].ForeColor = Color.Yellow;
                        status = true;
                    }
                    
                }

                if (status == true && checkBox1.Checked == true)
                {
                    MessageBox.Show("出现符合日期记录");
                    checkBox1.Checked = false;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void run()
        {
            try
            {
                DirectoryInfo root = new DirectoryInfo(path);
                FileInfo[] files = root.GetFiles();
                foreach (FileInfo fileinfo in files)
                {
                  
                    if (fileinfo.Extension==".xlsx")
                    {
                        DataTable dt = method.ExcelToDataTable(fileinfo.FullName, true);
                        method.ShowDataInListView(dt, listView1);
                        return;
                    }
                }
               
            }
            catch (Exception)
            {

                throw;
            }
        }

        Thread thread;
        Thread t;
        private void button1_Click(object sender, EventArgs e)
        {
            if (t == null || !t.IsAlive)
            {
                t = new Thread(panduan);
                t.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            timer1.Start();
        }

        private void 净水器日期监控_Load(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (t == null || !t.IsAlive)
            {
                t = new Thread(panduan);
                t.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].ForeColor = Color.Black;
            }
                timer1.Stop();
        }
    }
}
