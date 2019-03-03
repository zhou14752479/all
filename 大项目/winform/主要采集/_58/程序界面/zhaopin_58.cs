using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections;


namespace _58
{
    public partial class zhaopin_58 : Form
    {
        public zhaopin_58()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
            if (File.Exists(path + "\\mac.txt"))

            {
                注册账号ToolStripMenuItem.Enabled = false;
            }
        }
        
        
        #region 主采集函数
        public void run()
        {
            
            try
            {

                string city = label6.Text;
                string area = label10.Text;

                if (city == "")
                {
                    MessageBox.Show("请选择城市！");
                    return;
                }

                string item = label1.Text;

                string item1 = label5.Text;

                if (item == "")
                {
                    MessageBox.Show("请选择分类！");
                    return;
                }

               
                

                int page = 70;
                for (int i = 1; i <= page; i++)
                {
                    String Url = "http://" + city + ".58.com/"+area+"/" + item + "/pn" + i + "/";
                    string html = Method.GetUrl(Url);

                    MatchCollection TitleMatchs = Regex.Matches(html, @"<a  href=""https://qy.58.com([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    ArrayList lists = new ArrayList();
                    foreach (Match NextMatch in TitleMatchs)
                    {

                        lists.Add("http://qy.m.58.com/m_detail/" + NextMatch.Groups[1].Value);



                    }


                    string tm1 = DateTime.Now.ToString();  //获取系统时间

                    label3.Text = tm1 + "-->正在采集第" + i + "页\r\n";

                    Application.DoEvents();
                    System.Threading.Thread.Sleep(500);  //网址获取时间间隔 固定不变




                    foreach (string list in lists)

                    {
                        int index = this.skinDataGridView1.Rows.Add();

                        String Url1 = list;
                        string strhtml = Method.GetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()


                        string rxg = @"<h1>([\s\S]*?)</h1>";    //公司
                        string Rxg = @"<a href=""tel:([\s\S]*?)""";                                    //电话
                        string Rxg1 = @"</span><span>([\s\S]*?)</span>";                                    //联系人
                        string Rxg2 = @"<dt>公司地址：</dt>([\s\S]*?)</dd>";
                        string Rxg3 = @"<div class=""retTit""><strong>([\s\S]*?)</strong>";




                        Match company = Regex.Match(strhtml, rxg);
                        Match tel = Regex.Match(strhtml, Rxg);
                        Match contacts = Regex.Match(strhtml, Rxg1);
                        Match addr = Regex.Match(strhtml, Rxg2);
                        Match job = Regex.Match(strhtml, Rxg3);



                        this.skinDataGridView1.Rows[index].Cells[0].Value = company.Groups[1].Value;
                        this.skinDataGridView1.CurrentCell = this.skinDataGridView1.Rows[index].Cells[0];  //让datagridview滚动到当前行


                        this.skinDataGridView1.Rows[index].Cells[1].Value = tel.Groups[1].Value;



                        this.skinDataGridView1.Rows[index].Cells[2].Value = contacts.Groups[1].Value;


                        string temp = Regex.Replace(addr.Groups[1].Value, "<[^>]*>", "");   //去除所有标签去除所有标签

                        string temp1 = Regex.Replace(temp, "\\s+", "");   //去除所有标签去除空格
                        this.skinDataGridView1.Rows[index].Cells[3].Value = temp1;
                        this.skinDataGridView1.Rows[index].Cells[4].Value = job.Groups[1].Value;




                        if (button3.Text == "已停止")
                        {

                            return;


                        }


                        this.skinDataGridView1.CurrentCell = this.skinDataGridView1.Rows[index].Cells[0];  //让datagridview滚动到当前行





                        Application.DoEvents();
                        System.Threading.Thread.Sleep(1000);   //内容获取间隔，可变量

                    }
                   
                }

  
            }
            catch (System.Exception ex)
            {
                label3.Text = ex.ToString();
            }

        }
        #endregion
        #region 企业搜索采集
        public void search_job()
        {
           
            try
            {

                string city = label6.Text;

                if (city == "")
                {
                    MessageBox.Show("请选择城市！");
                    return;
                }

                 string keywords = System.Web.HttpUtility.UrlEncode(textBox2.Text, System.Text.Encoding.GetEncoding("utf-8"));

                
                int page = 70;
                for (int i = 1; i <= page; i++)
                {
                    String Url = "http://"+city+".58.com/job/pn"+i+"/?key="+keywords+"&final=1&jump=1";
                    string html = Method.GetUrl(Url);

                    MatchCollection TitleMatchs = Regex.Matches(html, @"<a href=""http://qy.58.com([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    ArrayList lists = new ArrayList();
                    foreach (Match NextMatch in TitleMatchs)
                    {

                        lists.Add( "http://qy.m.58.com/m_detail/" +NextMatch.Groups[1].Value );

                       

                    }

                    
                    string tm1 = DateTime.Now.ToString();  //获取系统时间

                    label3.Text = tm1 + "-->正在采集第" + i + "页\r\n";

                    Application.DoEvents();
                    System.Threading.Thread.Sleep(500);  //网址获取时间间隔 固定不变



                    
                    foreach (string list in lists)

                    { 
                        int index = this.skinDataGridView1.Rows.Add();

                        String Url1 = list;
                        string strhtml = Method.GetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()

                        
                        string rxg = @"<h1>([\s\S]*?)</h1>";    //公司
                        string Rxg = @"<a href=""tel:([\s\S]*?)""";                                    //电话
                        string Rxg1 = @"</span><span>([\s\S]*?)</span>";                                    //联系人
                        string Rxg2 = @"<dt>公司地址：</dt>([\s\S]*?)</dd>";
                        string Rxg3 = @"<div class=""retTit""><strong>([\s\S]*?)</strong>";




                        Match company = Regex.Match(strhtml, rxg);
                        Match tel = Regex.Match(strhtml, Rxg);
                        Match contacts = Regex.Match(strhtml, Rxg1);
                        Match addr = Regex.Match(strhtml, Rxg2);
                        Match job = Regex.Match(strhtml, Rxg3);



                        this.skinDataGridView1.Rows[index].Cells[0].Value = company.Groups[1].Value;
                            this.skinDataGridView1.CurrentCell = this.skinDataGridView1.Rows[index].Cells[0];  //让datagridview滚动到当前行
                      

                            this.skinDataGridView1.Rows[index].Cells[1].Value = tel.Groups[1].Value;

                      

                            this.skinDataGridView1.Rows[index].Cells[2].Value = contacts.Groups[1].Value;


                            string temp = Regex.Replace(addr.Groups[1].Value, "<[^>]*>", "");   //去除所有标签去除所有标签

                            string temp1 = Regex.Replace(temp, "\\s+", "");   //去除所有标签去除空格
                            this.skinDataGridView1.Rows[index].Cells[3].Value = temp1;
                            this.skinDataGridView1.Rows[index].Cells[4].Value = job.Groups[1].Value;




                        if (button3.Text == "已停止")
                        {

                            return;


                        }


                        this.skinDataGridView1.CurrentCell = this.skinDataGridView1.Rows[index].Cells[0];  //让datagridview滚动到当前行

                        
                      


                        Application.DoEvents();
                        System.Threading.Thread.Sleep(1000);   //内容获取间隔，可变量

                    }
                    
                }

            }
            catch (System.Exception ex)
            {
                label3.Text = ex.ToString();
            }

        }
        #endregion
        

        private void skinTreeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            label1.Text = label5.Text= e.Node.Name;
            label8.Text = e.Node.Text;

            if (e.Node.Level == 0)
            {
                MessageBox.Show("请展开选择!");

            }
            else if (e.Node.Parent.Name == "yewu")
            {
                label5.Text = "yewu";
            }
            else if (e.Node.Parent.Name == "meirongjianshen")
            {
                label5.Text = "meirongjianshen";
            }
            else if (e.Node.Parent.Name == "zplvyoujiudian")
            {
                label5.Text = "zplvyoujiudian";
            }



            return;
        }
      


        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (label15.Text == "测试用户" || label15.Text == "")
            {
                MessageBox.Show("请注册账号登陆！");
                return;
            }

            button3.Text = "停止采集";
            

            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Text = "已停止";

            
        }

        

        private void skinTreeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            //e.Node.Checked == true 节点被选中

            label6.Text = e.Node.Name;
            label14.Text = e.Node.Text;

            try { 
            if (e.Node.Parent.Name == "jn")
            {
                label10.Text = e.Node.Name;
                label6.Text = "jn";
            }
            else if (e.Node.Parent.Name == "qd")
            {
                label10.Text = e.Node.Name;
                label6.Text = "qd";
            }
            else if (e.Node.Parent.Name == "yt")
            {
                label10.Text = e.Node.Name;
                label6.Text = "yt";
            }
                else if (e.Node.Parent.Name == "wf")
                {
                    label10.Text = e.Node.Name;
                    label6.Text = "wf";
                }
                return;

            }
            catch (System.Exception ex)
            {
                ex.ToString();
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

        private void zhaopin_58_MouseEnter(object sender, EventArgs e)
        {
            label15.Text = Method.User;
        }

        private void zhaopin_58_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            MessageBox.Show("您确定要关闭吗？");
        }

        private void 合作模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }

       

        private void 联系我们ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("IEXPLORE.EXE", "http://wpa.qq.com/msgrd?v=3&uin=852266010&site=qq&menu=yes");
        }


        public void clear1(object sender, EventArgs e)
        {

            skinDataGridView1.Rows.Clear();

        }
       


        private void skinDataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu menu = new ContextMenu();   //初始化menu
                menu.MenuItems.Add("清空数据");   //添加菜单项c1
                                              //menu.MenuItems.Add("添加城市");   //添加菜单项c2

                menu.Show(skinDataGridView1, new Point(e.X, e.Y));   //在点(e.X, e.Y)处显示menu

                menu.MenuItems[0].Click += new EventHandler(clear1);


            }
        }

    

  

        private void label2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("IEXPLORE.EXE", "http://www.acaiji.com");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button3.Text = "停止采集";
            if (label15.Text == "测试用户" || label15.Text == "")
            {
                MessageBox.Show("请注册账号登陆！");
                return;
            }

            else
            {
                if (radioButton1.Checked == true)
                {
                    Thread thread = new Thread(new ThreadStart(run));
                    Control.CheckForIllegalCrossThreadCalls = false;
                    thread.Start();
                }

                if (radioButton2.Checked == true)
                {
                    if (textBox2.Text == string.Empty)
                    {
                        MessageBox.Show("按照关键词采集，请先输入关键词！");
                        return;
                    }

                    else
                    {
                        Thread thread = new Thread(new ThreadStart(search_job));
                        Control.CheckForIllegalCrossThreadCalls = false;
                        thread.Start();
                    }
                   
                }


            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Method.DgvToTable(this.skinDataGridView1);
            Method.DataTableToExcel(Method.DgvToTable(this.skinDataGridView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Method.Txt(skinDataGridView1);
        }

      
    }
}
