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

namespace fang
{
    public partial class Form5 : Form
    {
        int index { get; set; }
        bool status = true;
        bool zanting = true;

        public Form5()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(run);
            thread.Start();
        }

        private void Form5_Load(object sender, EventArgs e)
        {

        }

        #region  找邮箱
        public void run()
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("请输入网址");
                return;
            }

            try
            {
                String Url = textBox2.Text.Trim();

                string html = method.GetUrl(Url, "utf-8");


                MatchCollection TitleMatchs = Regex.Matches(html, @"journal\/article.aspx\?y=([\s\S]*?)'", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                ArrayList lists = new ArrayList();
                foreach (Match NextMatch in TitleMatchs)
                {
                    lists.Add("http://qikan.cqvip.com/journal/article.aspx?y=" + NextMatch.Groups[1].Value);

                }

                

                foreach (string list in lists)
                {

                    textBox1.Text = "正在采集" + list;
                    string html2 = method.GetUrl(list, "utf-8");

                   
                    MatchCollection Matchs = Regex.Matches(html2, @"<span class=""title""><a href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    ArrayList list2s = new ArrayList();

                    foreach (Match Match in Matchs)
                    {
                        list2s.Add("http://qikan.cqvip.com" + Match.Groups[1].Value);

                    }

                  

                    foreach (string list2 in list2s)
                    {

                        
                        string strhtml = method.GetUrl(list2, "utf-8");                                                                                //定义的GetRul方法 返回 reader.ReadToEnd()
                        

                        string Rxg1 = @"<strong>作者简介：</strong>([\s\S]*?)（";   //姓名
                        string rxg = @"<strong>作者简介：</strong>([\s\S]*?)</div>";  //简介
                        string Rxg = @"[a-z0-9_\.\-]+@[a-z0-9_\-]+\.[a-z0-9_\.]+";

                        Match year = Regex.Match(list, @"y=([\s\S]*?)&n=([\s\S]*?)&");
                        Match name = Regex.Match(strhtml, Rxg1);
                        Match body = Regex.Match(strhtml, rxg);
                        MatchCollection mails = Regex.Matches(body.Groups[1].Value.Trim().Replace("．","."),Rxg, RegexOptions.IgnoreCase | RegexOptions.Multiline);




                        if (body.Groups[1].Value!="" && body.Groups[1].Value.Contains("@"))
                        {
                            this.index = this.dataGridView1.Rows.Add();
                            this.dataGridView1.Rows[index].Cells[0].Value = year.Groups[1].Value+"年"+ year.Groups[2].Value.Trim()+"期";
                            this.dataGridView1.Rows[index].Cells[1].Value=name.Groups[1].Value.Trim();
                            this.dataGridView1.Rows[index].Cells[2].Value=body.Groups[1].Value.Trim();

                            for (int i = 0; i <mails.Count; i++)
                            {
                                this.dataGridView1.Rows[index].Cells[3].Value += mails[i].Groups[0].Value+" ";
                               
                            }

                            this.dataGridView1.CurrentCell = this.dataGridView1.Rows[index].Cells[0];



                        }
                      
                        Application.DoEvents();
                        Thread.Sleep(Convert.ToInt32(1000));   //内容获取间隔，可变量       

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                        if (this.status == false)
                            return;
                    }

                    

                }

            }




            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion


        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.DgvToTable(this.dataGridView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.status = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.zanting = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.zanting = true;
        }
    }
}
