using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using CCWin;
using System.Threading;
using System.Collections;

namespace _58
{
    public partial class huicong : Form
    {
        public huicong()
        {
            InitializeComponent();
        }

        private void huicong_Load(object sender, EventArgs e)
        {
            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
            if (File.Exists(path + "\\mac.txt"))

            {
                注册账号ToolStripMenuItem.Enabled = false;
            }
        }
        #region  运行函数
        public void Run()
        {
             
            try
            {


                int page = 100;

                string[] keywords = skinTextBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                string city = label1.Text;

                if (city == "")

                {
                    MessageBox.Show("请选择城市！");
                    return;
                }


                foreach (string keyword in keywords)

                {

                    //string keywordtogb2312 = System.Web.HttpUtility.UrlEncode(keyword, System.Text.Encoding.GetEncoding("gb2312"));

                    for (int i = 1; i <= page; i++)


                    {


                        string Url = "https://s.hc360.com/company/search.html?kwd="+keyword+"&k=0&z="+city+"&pnum="+i;
                        textBox14.Text = Url;
                        //String Url = "https://s.hc360.com/?w=" + keywordtogb2312 + "&mc=enterprise&ee="+i+"&z=" + citycode;
                        string strhtml = Method.GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()

                        string Rxg = @"<h3><a data-exposurelog=""([\s\S]*?)"" href=""([\s\S]*?)""";



                        MatchCollection all = Regex.Matches(strhtml, Rxg);


                        foreach (Match NextMatch in all)
                        {


                            textBox14.Text += NextMatch.Groups[2].Value + "\r\n";

                            textBox14.SelectionStart = textBox14.Text.Length; //设定光标位置
                            textBox14.ScrollToCaret(); //滚动到光标处


                        }
                        if (textBox14.Text == "")  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;

                        string tm1 = DateTime.Now.ToString();  //获取系统时间

                        textBox1.Text += tm1 + "-->正在采集"+label1.Text+"第" + i + "页"+keyword+ "\r\n";

                        textBox1.SelectionStart = textBox1.Text.Length; //设定光标位置
                        textBox1.ScrollToCaret();



                        Application.DoEvents();
                        System.Threading.Thread.Sleep(1000);

                        string[] lines1 = textBox14.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);



                        for (int j = 0; j < lines1.Length - 1; j++)
                        {



                            int index = this.skinDataGridView1.Rows.Add();    //利用dataGridView1.Rows.Add()事件为DataGridView控件增加新的行，该函数返回添加新行的索引号，即新行的行号，然后可以通过该索引号操作该行的各个单元格，如dataGridView1.Rows[index].Cells[0].Value = "1"。这是很常用也是很简单的方法。

                            String Url1 = lines1[j].ToString();
                            string strhtml1 = Method.GetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()



                            string Rxg0 = @"infoname=""([\s\S]*?)""";
                            string Rxg1 = @"联系人</span><span>：([\s\S]*?)</span>";
                            string Rxg2 = @"电话</span><span>：([\s\S]*?)</span>";
                            string Rxg3 = @"手机</span><span>：([\s\S]*?)</span>";

                             string Rxg4 = @"<p>地址：([\s\S]*?)&";



                            MatchCollection name = Regex.Matches(strhtml1, Rxg0);

                            MatchCollection contacts = Regex.Matches(strhtml1, Rxg1);


                            MatchCollection phone = Regex.Matches(strhtml1, Rxg2);

                            MatchCollection tell = Regex.Matches(strhtml1, Rxg3);
                            MatchCollection addr = Regex.Matches(strhtml1, Rxg4);


                            foreach (Match NextMatch in name)
                            {
                                string temp = Regex.Replace(NextMatch.Groups[1].Value, "<[^>]*>", "");   //去除所有标签去除所有标签

                                string temp1 = Regex.Replace(temp, "\\s+", "");   //去除所有标签去除空格

                                this.skinDataGridView1.Rows[index].Cells[0].Value = temp1;







                                this.skinDataGridView1.CurrentCell = this.skinDataGridView1.Rows[index].Cells[0];  //让datagridview滚动到当前行

                            }

                            foreach (Match NextMatch in contacts)
                            {

                                string temp = Regex.Replace(NextMatch.Groups[1].Value, "<[^>]*>", "");   //去除所有标签去除所有标签

                                string temp1 = Regex.Replace(temp, "\\s+", "");   //去除所有标签去除空格

                                this.skinDataGridView1.Rows[index].Cells[1].Value = temp1;



                            }

                            foreach (Match NextMatch in phone)
                            {
                                string temp = Regex.Replace(NextMatch.Groups[1].Value, "<[^>]*>", "");   //去除所有标签去除所有标签

                                string temp1 = Regex.Replace(temp, "\\s+", "");   //去除所有标签去除空格

                                this.skinDataGridView1.Rows[index].Cells[2].Value = temp1;


                            }
                            foreach (Match NextMatch in tell)
                            {
                                string temp = Regex.Replace(NextMatch.Groups[1].Value, "<[^>]*>", "");   //去除所有标签去除所有标签

                                string temp1 = Regex.Replace(temp, "\\s+", "");   //去除所有标签去除空格

                                this.skinDataGridView1.Rows[index].Cells[3].Value = temp1;


                            }
                            foreach (Match NextMatch in addr)
                            {
                               

                                this.skinDataGridView1.Rows[index].Cells[4].Value = NextMatch.Groups[1].Value.Trim();


                            }




                            if (button3.Text == "已停止")  //停止事件触发
                            {
                                button2.Enabled = false;
                                return;


                            }



                            Application.DoEvents();
                            System.Threading.Thread.Sleep(1000);



                        }
                        textBox14.Text = "";


                    }

                }
            }




            catch (System.Exception ex)
            {
                textBox1.Text = ex.ToString();
            }
        }

        #endregion
        private void button2_Click(object sender, EventArgs e)
        {
            if (label15.Text == "测试用户" || label15.Text == "")
            {
                MessageBox.Show("请注册账号登陆！");
                return;
            }
            
            Thread thread = new Thread(new ThreadStart(Run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

       

        private void skinTextBox2_Enter(object sender, EventArgs e)
        {
            skinTextBox2.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Text = "已停止";

           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("IEXPLORE.EXE", "http://wpa.qq.com/msgrd?v=3&uin=852266010&site=qq&menu=yes");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("IEXPLORE.EXE", "https://amos.alicdn.com/getcid.aw?v=3&uid=zkg852266010&site=cnalichn&groupid=0&s=1&charset=gbk");
        }

        private void skinTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Text == "北京" | e.Node.Text == "天津" | e.Node.Text == "重庆" | e.Node.Text == "上海")
            {
                label1.Text = "中国:" + e.Node.Text;
            }

            else if (e.Node.Level == 0)

            {
                label1.Text = "中国:" + e.Node.Text;

            }

            else
            {

                label1.Text = "中国:" + e.Node.Parent.Text + ":" + e.Node.Text + "市";
            }


        }

        

        private void 注册账号ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            register rg = new register();
            rg.Show();
        }

        private void 登陆账号ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login lg = new Login();
            lg.Show();
        }

        private void huicong_MouseEnter(object sender, EventArgs e)
        {
            label15.Text = Method.User;
        }

        private void huicong_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            MessageBox.Show("您确定要关闭吗？");
        }

        private void button5_Click(object sender, EventArgs e)
        {

            Method.DgvToTable(this.skinDataGridView1);
            Method.DataTableToExcel(Method.DgvToTable(this.skinDataGridView1), "Sheet1", true);



            

           

            

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Method.Txt(skinDataGridView1);

           
           

        
                 

        }

      
    }
}
