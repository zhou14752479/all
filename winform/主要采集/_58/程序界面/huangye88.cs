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



//using NPOI.HSSF.UserModel;
//using NPOI.SS.UserModel;
namespace _58
{
    public partial class huangye88 : Form
    {
        public huangye88()
        {
            InitializeComponent();
        }

        

        
        
        
        #region 信息采集函数
        public void run()
        {
           
            try
            {

                string city = label6.Text;

                if (city == "")
                {
                    MessageBox.Show("请选择城市！");
                    return;
                }

                string item = label1.Text;

                if (item == "")
                {
                    MessageBox.Show("请选择分类！");
                    return;
                }

                

                int page = 50;
                for (int i = 1; i <= page; i++)
                {

                    
                    String Url = "http://b2b.huangye88.com/"+city+"/"+item+"/pn"+i+"/";
                    string html = Method.GetUrl(Url);

                    MatchCollection TitleMatchs = Regex.Matches(html, @"<h4><a href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                    foreach (Match NextMatch in TitleMatchs)
                    {

                        textBox14.Text += NextMatch.Groups[1].Value + "\r\n";

                        textBox14.SelectionStart = textBox14.Text.Length; //设定光标位置
                        textBox14.ScrollToCaret(); //滚动到光标处


                    }
                    string tm1 = DateTime.Now.ToString();  //获取系统时间

                    textBox1.Text += tm1 + "-->正在采集第" + i + "页\r\n";

                    textBox1.SelectionStart = textBox1.Text.Length; //设定光标位置
                    textBox1.ScrollToCaret();
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(500);  //网址获取时间间隔 固定不变
                    string[] lines = textBox14.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                    for (int j = 0; j < lines.Length - 1; j++)
                    {

                        int index = this.skinDataGridView1.Rows.Add();

                        String Url1 = lines[j];
                        string strhtml = Method.GetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()


                        string rxg = @"<dl class=""bottom"">([\s\S]*?)名称：([\s\S]*?)<";            //公司
                        string Rxg = @"<dl class=""bottom"">([\s\S]*?)手机：([\s\S]*?)<";           //电话
                        string Rxg1 = @"联系人：([\s\S]*?)rel=""nofollow"">([\s\S]*?)</a>";          //联系人
                        string Rxg2 = @"<meta name=""Description"" content=""([\s\S]*?)""";          //介绍



                        MatchCollection company = Regex.Matches(strhtml, rxg);
                        MatchCollection tel = Regex.Matches(strhtml, Rxg);
                        MatchCollection contacts = Regex.Matches(strhtml, Rxg1);
                        MatchCollection introduction = Regex.Matches(strhtml, Rxg2);





                        foreach (Match NextMatch in company)
                        {

                            this.skinDataGridView1.Rows[index].Cells[0].Value = NextMatch.Groups[2].Value;


                            this.skinDataGridView1.CurrentCell = this.skinDataGridView1.Rows[index].Cells[0];  //让datagridview滚动到当前行

                        }
                        foreach (Match NextMatch in tel)
                        {

                            this.skinDataGridView1.Rows[index].Cells[1].Value = NextMatch.Groups[2].Value;

                        }
                        foreach (Match NextMatch in contacts)
                        {

                            this.skinDataGridView1.Rows[index].Cells[2].Value = NextMatch.Groups[2].Value;

                        }
                        foreach (Match NextMatch in introduction)
                        {

                            this.skinDataGridView1.Rows[index].Cells[3].Value = NextMatch.Groups[1].Value;

                        }
                        if (skinButton2.Text == "已停止")
                        {
                            skinButton1.Enabled = false;
                            return;


                        }


                        Application.DoEvents();
                        System.Threading.Thread.Sleep(Convert.ToInt32(500));   //内容获取间隔，可变量

                    }
                    textBox14.Text = "";
                }
                

                
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        #endregion
        private void skinTreeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            label1.Text = e.Node.Name;
            label8.Text = e.Node.Text;
            return;
        }

        private void huangye88_Load(object sender, EventArgs e)
        {
            

            
        }

        private void skinTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            label6.Text = e.Node.Name;
            label14.Text = e.Node.Text;
            return;
        }

       

        private void skinButton1_Click(object sender, EventArgs e)
        {

            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

       

        private void skinButton3_Click(object sender, EventArgs e)
        {

        }

        

        private void skinButton2_Click(object sender, EventArgs e)
        {
            //skinButton2.Text = "已停止";
            foreach (TreeNode  tn in skinTreeView2.Nodes)
            {
                textBox1.Text += tn.Text + "\r\n";
                textBox14.Text += tn.Name + "\r\n";
            }
           
        }
    }

    
}
