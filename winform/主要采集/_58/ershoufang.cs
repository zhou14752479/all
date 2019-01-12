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

namespace _58
{
    public partial class ershoufang : Form
    {
        public ershoufang()
        {
            InitializeComponent();
        }

        private void ershoufang_Load(object sender, EventArgs e)
        {
            Method.get58CityName(comboBox1);
                

        }
        int index { get; set; }

        bool zanting = true;
        bool status = true;

        public void run() {

            string city = Method.Get58pinyin(comboBox1.SelectedItem.ToString());

            try
            {




                if (city == "")
                {
                    MessageBox.Show("请选择城市！");
                    return;
                }

                for (int i = 1; i < 71; i++)
                {
                    String Url = "http://" + city + ".58.com/ershoufang/pn" + i + "/" + "?key=" + textBox1.Text.Trim();

                    string html = Method.GetUrl(Url);


                    MatchCollection TitleMatchs = Regex.Matches(html, @"<li logr=""([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    

                    ArrayList lists = new ArrayList();
                    foreach (Match NextMatch in TitleMatchs)
                    {

                        lists.Add("http://" + city + ".58.com/ershoufang/" + NextMatch.Groups[4].Value + "x.shtml");
                        //滚动到光标处
                    }

                    if (lists.Count < 0)
                        return;


                    for (int j = 0; j< lists.Count; j++)
                    {

                        this.index = this.dataGridView1.Rows.Add();
                        String Url1 = lists[j].ToString();
                        
                        // str = str.Substring(str.Length - i) 从右边开始取i个字符

                        string Url2 = "http://m.58.com/" + city + "/ershoufang/" + Url1.Substring(Url1.Length - 21);                       //获取二手房手机端的网址
                        textBox2.Text = Url2;
                        string strhtml = Method.GetUrl(Url1);                                                                                //定义的GetRul方法 返回 reader.ReadToEnd()

                       string strhtml2 = Method.GetUrl(Url2);                                                                               //请求手机端网址

                        string rxg = @"<h1 class=""c_333 f20"">([\s\S]*?)</h1>";  //标题
                        string Rxg = @"<h2 class=""agent-title"">([\s\S]*?)</h2>";  //手机端正则匹配联系人
                        string Rxg1 = @"<p class='phone-num'>([\s\S]*?)<";   //电话


                        Match titles = Regex.Match(strhtml, rxg);
                        Match contacts = Regex.Match(strhtml2, Rxg);                                                        //手机端正则匹配联系人
                        Match tell = Regex.Match(strhtml, Rxg1);

                        this.dataGridView1.Rows[index].Cells[0].Value = titles.Groups[1].Value;

                        this.dataGridView1.Rows[index].Cells[1].Value = contacts.Groups[1].Value;

                        this.dataGridView1.Rows[index].Cells[2].Value = tell.Groups[1].Value;


                        this.dataGridView1.CurrentCell = this.dataGridView1.Rows[index].Cells[0];
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                        if (this.status==false)
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

               MessageBox.Show( ex.ToString());
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.zanting = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            this.status = true;
            if (label3.Text == "用户登陆" || label3.Text == "")
            {
                MessageBox.Show("请注册账号登陆！");
                return;
            }
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Method.DataTableToExcel(Method.DgvToTable(this.dataGridView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.zanting = true; ;
        }

        private void 注册ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            register rg = new register();
            rg.Show();
        }

        private void 登陆ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login lg = new Login();
            lg.Show();
        }

        private void ershoufang_MouseEnter(object sender, EventArgs e)
        {
           
        }

        private void splitContainer1_Panel1_MouseEnter(object sender, EventArgs e)
        {
            label3.Text = Method.User; //获取Method公共类的静态变量User的值
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.status = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }
    }
}
