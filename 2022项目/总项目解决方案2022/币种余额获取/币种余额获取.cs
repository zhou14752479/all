using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using myDLL;

namespace 币种余额获取
{
    public partial class 币种余额获取 : Form
    {
        [DllImport("kernel32.dll")]
        public static extern bool Beep(int freq, int duration);
        public 币种余额获取()
        {
            InitializeComponent();
        }

       // string key = "SRBQMRAYMR3D9E2HJDPXNSBSIA66JTP1ED";



        public void run()
        {

            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)//如果DataGridView中有空的数据，则提示数据输入不完整并退出添加，不包括标题行
            {
                Random rd = new Random(Guid.NewGuid().GetHashCode()); //生成不重复的随机数，默认的话根据时间戳如果太快会相同
                int suiji = rd.Next(1, lists.Count);
                string key = lists[suiji];
                
                dataGridView1.Rows[i].Cells[4].Style.BackColor = Color.White;
                dataGridView1.Rows[i].Cells[5].Style.BackColor = Color.White;
                dataGridView1.Rows[i].Cells[6].Style.BackColor = Color.White;

                string tuandui = dataGridView1.Rows[i].Cells[0].Value.ToString().Trim();
                string yewu = dataGridView1.Rows[i].Cells[1].Value.ToString().Trim();
                string biecheng = dataGridView1.Rows[i].Cells[2].Value.ToString().Trim();
                string addr = dataGridView1.Rows[i].Cells[3].Value.ToString().Trim();
              string eth=dgv.getbalance("ETH",addr,key);
                string usdt = dgv.getbalance("USDT", addr, key);
                string usdc = dgv.getbalance("USDC", addr, key);
                label1.Text = DateTime.Now.ToShortTimeString() +"："+"正在获取---->"+addr;
                try
                {
                    if (eth != dataGridView1.Rows[i].Cells[4].Value.ToString())
                    {
                        double value = Convert.ToDouble(eth) - Convert.ToDouble(dataGridView1.Rows[i].Cells[4].Value.ToString());
                        if (checkBox1.Checked == true)
                        {
                            dgv.sendmsg(tuandui + yewu + biecheng + "ETH变动" + value.ToString("F4"));
                        }
                        dataGridView1.Rows[i].Cells[4].Style.BackColor = Color.Red;
                        Beep(800, 300);
                    }
                    if (usdt != dataGridView1.Rows[i].Cells[5].Value.ToString())
                    {
                        double value = Convert.ToDouble(usdt) - Convert.ToDouble(dataGridView1.Rows[i].Cells[5].Value.ToString());
                        if (checkBox1.Checked == true)
                        {
                            dgv.sendmsg(tuandui + yewu + biecheng + "USDT变动" + value.ToString("F2"));
                        }
                        dataGridView1.Rows[i].Cells[5].Style.BackColor = Color.Red;
                        Beep(800, 300);
                    }
                    if (usdc != dataGridView1.Rows[i].Cells[6].Value.ToString())
                    {
                        double value = Convert.ToDouble(usdc) - Convert.ToDouble(dataGridView1.Rows[i].Cells[6].Value.ToString());
                        if (checkBox1.Checked == true)
                        {
                            dgv.sendmsg(tuandui + yewu + biecheng + "USDC变动" + value.ToString("F2"));
                        }
                        dataGridView1.Rows[i].Cells[6].Style.BackColor = Color.Red;
                        Beep(800, 300);
                    }

                }
                catch (Exception)
                {

                    
                }

                dataGridView1.Rows[i].Cells[4].Value = eth;
                dataGridView1.Rows[i].Cells[5].Value = usdt;
                dataGridView1.Rows[i].Cells[6].Value = usdc;
                dataGridView1.Rows[i].Cells[7].Value = DateTime.Now.ToShortTimeString();

            }
        }

        List<string> lists = new List<string>();
        datagridview_page dgv = new datagridview_page();
        private void 币种余额获取_Load(object sender, EventArgs e)
        {
            dgv.PageSorter(dataGridView1);//分页
            label4.Text = dgv.allpage;
            label5.Text = dgv.nowpage;
            lists = dgv.getkey();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (dgv.currentPage == dgv.pageCount)
            { return; }
            dgv.currentPage++;
            dgv.LoadPage(dataGridView1);
            label4.Text = dgv.allpage;
            label5.Text = dgv.nowpage;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (dgv.currentPage == 1)
            { return; }
            dgv.currentPage--;
            dgv.LoadPage(dataGridView1);
            label4.Text = dgv.allpage;
            label5.Text = dgv.nowpage;
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;
        private void button1_Click(object sender, EventArgs e)
        {
            FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\data.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
            sw.WriteLine(textBox1.Text.Trim()+"#"+ textBox2.Text.Trim() + "#"+textBox3.Text.Trim() + "#"+textBox4.Text.Trim());
            sw.Close();
            fs1.Close();
            sw.Dispose();
            dgv.PageSorter(dataGridView1);//分页
            label4.Text = dgv.allpage;
            label5.Text = dgv.nowpage;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 8)
                {
                  
                    DialogResult dr = MessageBox.Show("确定要删除吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        if (e.RowIndex == -1)
                        {
                            dataGridView1.Rows.Clear();
                            //MessageBox.Show(dataGridView1.Columns[e.ColumnIndex].HeaderText);
                           
                                FileStream fs1 = new FileStream(path + "//data.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                                StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                                sw.WriteLine("");
                                sw.Close();
                                fs1.Close();
                                sw.Dispose();

                                dgv.PageSorter(dataGridView1);
                                label4.Text = dgv.allpage;
                                label5.Text = dgv.nowpage;
                            
                        }
                        else
                        {

                            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                            dataGridView1.Rows.Remove(row);
                            MessageBox.Show("删除成功");
                            FileStream fs1 = new FileStream(path + "//data.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)//如果DataGridView中有空的数据，则提示数据输入不完整并退出添加，不包括标题行
                            {

                                sw.WriteLine(dataGridView1.Rows[i].Cells[0].Value + "#" + dataGridView1.Rows[i].Cells[1].Value + "#" + dataGridView1.Rows[i].Cells[2].Value + "#" + dataGridView1.Rows[i].Cells[3].Value);

                            }
                            sw.Close();
                            fs1.Close();
                            sw.Dispose();

                            dgv.PageSorter(dataGridView1);
                            label4.Text = dgv.allpage;
                            label5.Text = dgv.nowpage;
                        }

                    }
                    else
                    {

                    }
                }
               
            }
            catch (Exception)
            {

                MessageBox.Show("值为空");
            }
        }

        Thread thread;

        private void button2_Click(object sender, EventArgs e)
        {
            
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Start();
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void 币种余额获取_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}
