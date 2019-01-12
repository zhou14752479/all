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

namespace fang.临时软件
{
    public partial class anjuke : Form
    {
        public anjuke()
        {
            InitializeComponent();
        }
        int index { get; set; }

        bool zanting = true;
        bool status = true;
       
        #region 安居客房源
        public void run()
        {

            try
            {

                if (textBox2.Text == "")
                {
                    MessageBox.Show("请输入城市！");
                    return;
                }

                string city = textBox2.Text.Trim(); ;

                for (int i = 1; i < 100; i++)
                {
                    String Url = "https://"+city+".anjuke.com/sale/p"+i+ "/?kw="+textBox3.Text;
                    

                    string html = method.GetUrl(Url,"utf-8");


                    MatchCollection TitleMatchs = Regex.Matches(html, @"data-company=""""([\s\S]*?)href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    ArrayList lists = new ArrayList();
                    foreach (Match NextMatch in TitleMatchs)
                    {
                        lists.Add(NextMatch.Groups[2].Value);

                    }

                    if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;

                    foreach (string list in lists)

                    {

                        textBox1.Text = list;
                        this.index = this.dataGridView1.Rows.Add();

                        
                        string strhtml = method.GetUrl(list, "utf-8");

                        string rxg = @"<em class=""houseTitle"">([\s\S]*?)</em>";
                        string Rxg = @"brokercard-name"">([\s\S]*?)</div>";

                        string Rxg1 = @"url=([\s\S]*?)""";



                        Match titles = Regex.Match(strhtml, rxg);
                        Match contacts = Regex.Match(strhtml, Rxg);



                        Match m_url = Regex.Match(strhtml, Rxg1);
                     
                        string html2 = method.GetUrl(m_url.Groups[1].Value, "utf-8");


                        Match tel = Regex.Match(html2, @"data-phone=""([\s\S]*?)""");

                        this.dataGridView1.Rows[index].Cells[0].Value = titles.Groups[1].Value.Trim();

                        this.dataGridView1.Rows[index].Cells[1].Value = contacts.Groups[1].Value.Trim();

                        this.dataGridView1.Rows[index].Cells[2].Value = tel.Groups[1].Value.Trim();


                        this.dataGridView1.CurrentCell = this.dataGridView1.Rows[index].Cells[0];
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                        if (this.status == false)
                        {
                            return;
                        }

                        Application.DoEvents();
                        System.Threading.Thread.Sleep(1000);   //内容获取间隔，可变量





                    }


                }

            }


            catch (System.Exception ex)
            {

                textBox1.Text = ex.ToString();
            }

        }

        #endregion

        private void anjuke_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.status = true;

            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.DgvToTable(this.dataGridView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.zanting = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.zanting = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.status = false;

           textBox1.Text=    method.GetPYString("北京");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }
    }
}
