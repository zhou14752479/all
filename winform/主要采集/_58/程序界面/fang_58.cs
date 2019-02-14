using System;
using System.Collections;
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
using System.Net.Mail;
using System.Timers;


namespace _58
{
    public partial class fang_58 : Form
    {
        int index { get; set; }
        public fang_58()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        
        private void button5_Click(object sender, EventArgs e)
        {
          
            
        }
        private void skinTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            label6.Text = e.Node.Name;
            label14.Text = e.Node.Text;
            return;
        }
        int time = 1000;  //获取内容间隔
        private void Form1_Load(object sender, EventArgs e)
        {
            
            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
            if (File.Exists(path + "\\mac.txt"))

            {
                注册账号ToolStripMenuItem.Enabled = false;
            }

                foreach (Control c in this.groupBox6.Controls)
            {

                if (c is RadioButton)
                {
                    if (((RadioButton)c).Checked == true)
                    {
                        label8.Text = ((RadioButton)c).Text;
                    }
                }
            }
        }
        int page = 70;


        //string temp = Regex.Replace(NextMatch.Groups[1].Value, "<[^>]*>", "");   //去除所有标签去除所有标签



        #region  生意转让、商铺出租、商铺出售
        public void shangpu(object item)
        {
            string[] headers = { "标题", "联系人", "电话", "地区", "网址" };

            setDatagridview(skinDataGridView1, 5, headers);

            try
            {
                
                string city = label6.Text;

                if (city == "")
                {
                    MessageBox.Show("请选择城市！");
                    return;
                }

                for (int i = 1; i <= page; i++)
                {
                    String Url = "http://" + city + ".58.com/"+item.ToString()+"/0/pn" + i + "/";
                    string html = Method.GetUrl(Url);


                    MatchCollection TitleMatchs = Regex.Matches(html, @"https://[a-z]+.58.com/[a-z]+/[0-9]+x.shtml", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                    ArrayList lists = new ArrayList();

                    foreach (Match NextMatch in TitleMatchs)
                    {

                        if (!lists.Contains(NextMatch.Groups[0].Value))
                        {
                            lists.Add(NextMatch.Groups[0].Value);
                        }


                    }
                    string tm1 = DateTime.Now.ToString();  //获取系统时间

                    textBox3.Text += tm1 + "-->正在采集第" + i + "页\r\n";

                    textBox3.SelectionStart = textBox3.Text.Length; //设定光标位置
                    textBox3.ScrollToCaret();

                    Application.DoEvents();
                    Thread.Sleep(500);  //网址获取时间间隔 固定不变

                

                    foreach (string list in lists)
                    {

                        this.index = this.skinDataGridView1.Rows.Add();
                        String Url1 =list;
                        string strhtml = Method.GetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()


                        string title = @"<h1 class=""c_000 f20"">([\s\S]*?)</h1>";
                        string Rxg = @"<a class=""c_000 agent-name-txt""([\s\S]*?)>([\s\S]*?)</a>";
                        string Rxg1 = @"<p class='phone-num'>([\s\S]*?)</p>";
                        string Rxg2 = @"详细地址:</span>([\s\S]*?)</span>";
                        


                        Match titles = Regex.Match(strhtml, title);
                        Match contacts = Regex.Match(strhtml, Rxg);
                        Match tell = Regex.Match(strhtml, Rxg1);
                        Match region = Regex.Match(strhtml, Rxg2);
                     


                        this.skinDataGridView1.Rows[index].Cells[0].Value = titles.Groups[1].Value.Trim();
                        
                        this.skinDataGridView1.Rows[index].Cells[1].Value = contacts.Groups[2].Value.Trim();
                        this.skinDataGridView1.Rows[index].Cells[2].Value = tell.Groups[1].Value;

                        string temp = Regex.Replace(region.Groups[1].Value, "<[^>]*>", "");
                        this.skinDataGridView1.Rows[index].Cells[3].Value = temp.Trim().Replace(" ","").Replace("&nbsp;","");

                        this.skinDataGridView1.Rows[index].Cells[4].Value = list;


                        this.skinDataGridView1.CurrentCell = this.skinDataGridView1.Rows[index].Cells[0];  //让datagridview滚动到当前行

                       

                        if (skinButton13.Text == "已停止")
                        {
                            
                            return;


                        }
                        Application.DoEvents();
                        Thread.Sleep(time);   //内容获取间隔，可变量



                    }
                    textBox14.Text = "";

                }




            }

            catch (System.Exception ex)
            {

                ex.ToString();
            }


        }


        #endregion

        #region 设置表格格式

        public void setDatagridview(DataGridView dgv,int count,string[] headers)
        {

            dgv.ColumnCount = count;


            for (int i = 0; i < count; i++)
            {
                dgv.Columns[i].HeaderText = headers[i];
                
            }
        }

        #endregion


        #region 写字楼

        public void xiezilou()
        {


            try
            {
                
                string city = label6.Text;

                if (city == "")
                {
                    MessageBox.Show("请选择城市！");
                    return;
                }

                for (int i = 1; i <= page; i++)
                {
                    String Url = "http://" + city + ".58.com/zhaozu/0/pn" + i + "/";
                    string html = Method.GetUrl(Url);

                    MatchCollection TitleMatchs = Regex.Matches(html, @"<h2 class='title'>([\s\S]*?)<a href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                    foreach (Match NextMatch in TitleMatchs)
                    {

                        textBox14.Text += NextMatch.Groups[2].Value + "\r\n";

                        textBox14.SelectionStart = textBox14.Text.Length; //设定光标位置
                        textBox14.ScrollToCaret(); //滚动到光标处


                    }
                    string tm1 = DateTime.Now.ToString();  //获取系统时间

                    textBox3.Text += tm1 + "-->正在采集第" + i + "页\r\n";

                    textBox3.SelectionStart = textBox3.Text.Length; //设定光标位置
                    textBox3.ScrollToCaret();

                    Application.DoEvents();
                    System.Threading.Thread.Sleep(500);  //网址获取时间间隔 固定不变

                    string[] lines = textBox14.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                    for (int j = 0; j < lines.Length - 1; j++)
                    {

                        this.index = this.skinDataGridView1.Rows.Add();

                        String Url1 = lines[j];
                        string strhtml = Method.GetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()


                        string title = @"<title>([\s\S]*?)</title>";
                        string Rxg = @"<span class='f14 c_333 jjrsay'>([\s\S]*?)</span>";
                        string Rxg1 = @"<p class='phone-num'>([\s\S]*?)</p>";
                        string Rxg2 = @"province=([\s\S]*?);";


                        Match titles = Regex.Match(strhtml, title);
                        Match contacts = Regex.Match(strhtml, Rxg);
                        Match tell = Regex.Match(strhtml, Rxg1);
                        Match region = Regex.Match(strhtml, Rxg2);


                        this.skinDataGridView1.Rows[index].Cells[0].Value = titles.Groups[1].Value;

                        this.skinDataGridView1.Rows[index].Cells[1].Value = contacts.Groups[1].Value;

                        this.skinDataGridView1.Rows[index].Cells[2].Value = tell.Groups[1].Value;

                        this.skinDataGridView1.Rows[index].Cells[3].Value = region.Groups[1].Value;

                        this.skinDataGridView1.Rows[index].Cells[4].Value = lines[j];

                        this.skinDataGridView1.Columns[2].FillWeight = 40;   //设置列宽

                        this.skinDataGridView1.CurrentCell = this.skinDataGridView1.Rows[index].Cells[0];  //让datagridview滚动到当前行

                        if (skinButton13.Text == "已停止")
                        {
                            
                            return;


                        }
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(time);   //内容获取间隔，可变量

                    }
                    textBox14.Text = "";

                }


               
            }
            catch (System.Exception ex)
            {
                textBox3.Text = ex.ToString();
                //MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region 厂房

        public void changfang()
        {


            try
            {
                
                string city = label6.Text;

                if (city == "")
                {
                    MessageBox.Show("请选择城市！");
                    return;
                }

                for (int i = 1; i <= page; i++)
                {
                    String Url = "http://" + city + ".58.com/changfang/0/pn" + i + "/";
                    string html = Method.GetUrl(Url);

                    MatchCollection TitleMatchs = Regex.Matches(html, @"<h2 class='title'>([\s\S]*?)<a href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                    foreach (Match NextMatch in TitleMatchs)
                    {

                        textBox14.Text += NextMatch.Groups[2].Value + "\r\n";

                        textBox14.SelectionStart = textBox14.Text.Length; //设定光标位置
                        textBox14.ScrollToCaret(); //滚动到光标处


                    }
                    string tm1 = DateTime.Now.ToString();  //获取系统时间

                    textBox3.Text += tm1 + "-->正在采集第" + i + "页\r\n";

                    textBox3.SelectionStart = textBox3.Text.Length; //设定光标位置
                    textBox3.ScrollToCaret();

                    Application.DoEvents();
                    System.Threading.Thread.Sleep(500);  //网址获取时间间隔 固定不变

                    string[] lines = textBox14.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                    for (int j = 0; j < lines.Length - 1; j++)
                    {

                        this.index = this.skinDataGridView1.Rows.Add();

                        String Url1 = lines[j];
                        string strhtml = Method.GetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()


                        string title = @"<title>([\s\S]*?)</title>";
                        string Rxg = @"<span class='f14 c_333 jjrsay'>([\s\S]*?)</span>";
                        string Rxg1 = @"<p class='phone-num'>([\s\S]*?)</p>";
                        string Rxg2 = @"province=([\s\S]*?);";


                        Match titles = Regex.Match(strhtml, title);
                        Match contacts = Regex.Match(strhtml, Rxg);
                        Match tell = Regex.Match(strhtml, Rxg1);
                        Match region = Regex.Match(strhtml, Rxg2);


                        this.skinDataGridView1.Rows[index].Cells[0].Value = titles.Groups[1].Value;


                        this.skinDataGridView1.Rows[index].Cells[1].Value = contacts.Groups[1].Value;


                        this.skinDataGridView1.Rows[index].Cells[2].Value = tell.Groups[1].Value;



                        this.skinDataGridView1.Rows[index].Cells[3].Value = region.Groups[1].Value;


                        this.skinDataGridView1.Rows[index].Cells[4].Value = lines[j];
                     
                        this.skinDataGridView1.CurrentCell = this.skinDataGridView1.Rows[index].Cells[0];  //让datagridview滚动到当前行



                        if (skinButton13.Text == "已停止")
                        {
                           
                            return;


                        }
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(time);   //内容获取间隔，可变量

                    }
                    textBox14.Text = "";

                }



            }
            catch (System.Exception ex)
            {
                textBox3.Text = ex.ToString();
                //MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region 车位

        public void chewei()
        {


            try
            {
                
                string city = label6.Text;

                if (city == "")
                {
                    MessageBox.Show("请选择城市！");
                    return;
                }

                for (int i = 1; i <= page; i++)
                {
                    String Url = "http://" + city + ".58.com/cheku/0/pn" + i + "/";
                    string html = Method.GetUrl(Url);

                    MatchCollection TitleMatchs = Regex.Matches(html, @"<h2 class='title'>([\s\S]*?)<a href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                    foreach (Match NextMatch in TitleMatchs)
                    {

                        textBox14.Text += NextMatch.Groups[2].Value + "\r\n";

                        textBox14.SelectionStart = textBox14.Text.Length; //设定光标位置
                        textBox14.ScrollToCaret(); //滚动到光标处


                    }
                    string tm1 = DateTime.Now.ToString();  //获取系统时间

                    textBox3.Text += tm1 + "-->正在采集第" + i + "页\r\n";

                    textBox3.SelectionStart = textBox3.Text.Length; //设定光标位置
                    textBox3.ScrollToCaret();

                    Application.DoEvents();
                    System.Threading.Thread.Sleep(500);  //网址获取时间间隔 固定不变

                    string[] lines = textBox14.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                    for (int j = 0; j < lines.Length - 1; j++)
                    {

                        this.index = this.skinDataGridView1.Rows.Add();

                        String Url1 = lines[j];
                        string strhtml = Method.GetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()


                        string title = @"<title>([\s\S]*?)</title>";
                        string Rxg = @"<span class='f14 c_333 jjrsay'>([\s\S]*?)</span>";
                        string Rxg1 = @"<p class='phone-num'>([\s\S]*?)</p>";
                        string Rxg2 = @"province=([\s\S]*?);";


                        Match titles = Regex.Match(strhtml, title);
                        Match contacts = Regex.Match(strhtml, Rxg);
                        Match tell = Regex.Match(strhtml, Rxg1);
                        Match region = Regex.Match(strhtml, Rxg2);


                        

                            this.skinDataGridView1.Rows[index].Cells[0].Value = titles.Groups[1].Value;

                      

                            this.skinDataGridView1.Rows[index].Cells[1].Value = contacts.Groups[1].Value;

                    

                            this.skinDataGridView1.Rows[index].Cells[2].Value = tell.Groups[1].Value;

                     

                            this.skinDataGridView1.Rows[index].Cells[3].Value = region.Groups[1].Value;

                        
                        this.skinDataGridView1.Rows[index].Cells[4].Value = lines[j];

                        this.skinDataGridView1.Columns[2].FillWeight = 40;   //设置列宽


                            this.skinDataGridView1.CurrentCell = this.skinDataGridView1.Rows[index].Cells[0];  //让datagridview滚动到当前行

                        if (skinButton13.Text == "已停止")
                        {
                            
                            return;


                        }
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(time);   //内容获取间隔，可变量

                    }
                    textBox14.Text = "";

                }



            }
            catch (System.Exception ex)
            {
                textBox3.Text = ex.ToString();
                //MessageBox.Show(ex.ToString());
            }
        }
        #endregion
        
        #region  58二手房
        public void ershoufang()
        {
           


            try
            {
                string[] headers = {"标题","联系人","电话","地区","小区","面积","价格","网址"};

                setDatagridview(skinDataGridView1,8,headers);

                string city = label6.Text;

                if (city == "")
                {
                    MessageBox.Show("请选择城市！");
                    return;
                }

              

                    for (int i = 1; i <= page; i++)
                    {
                        String Url = "http://" + city + ".58.com/ershoufang/0/pn" + i + "/";

                        string html = Method.GetUrl(Url);


                        MatchCollection TitleMatchs = Regex.Matches(html, @"<li logr=""([\s\S]*?)_0_([\s\S]*?)_([\s\S]*?)_", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                        foreach (Match NextMatch in TitleMatchs)
                        {

                            textBox14.Text += "http://" + city + ".58.com/ershoufang/" + NextMatch.Groups[3].Value + "x.shtml" + "\r\n";
                            textBox14.SelectionStart = textBox14.Text.Length;                                                            //设定光标位置
                            textBox14.ScrollToCaret();                                                                                   //滚动到光标处
                        }

                        string tm1 = DateTime.Now.ToString();                                                                            //获取系统时间
                        textBox3.Text += tm1 + "-->正在采集第" + i + "页\r\n";
                        textBox3.SelectionStart = textBox3.Text.Length;                                                                  //设定光标位置
                        textBox3.ScrollToCaret();


                        string[] lines = textBox14.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);



                        for (int j = 0; j < lines.Length - 1; j++)

                        {

                        this.index = this.skinDataGridView1.Rows.Add();
                        String Url1 = lines[j];
                            // str = str.Substring(str.Length - i) 从右边开始取i个字符

                            string Url2 = "http://m.58.com/" + city + "/ershoufang/" + Url1.Substring(Url1.Length - 21);                       //获取二手房手机端的网址

                            string strhtml = Method.GetUrl(Url1);                                                                                //定义的GetRul方法 返回 reader.ReadToEnd()

                            string strhtml2 = Method.GetUrl(Url2);                                                                               //请求手机端网址

                            string title = @"<h1 class=""c_333 f20"">([\s\S]*?)</h1>";  //标题
                            string Rxg = @"<h2 class=""agent-title"">([\s\S]*?)</h2>";  //手机端正则匹配联系人
                            string Rxg1 = @"<p class='phone-num'>([\s\S]*?)<";   //电话
                            string Rxg2 = @"<li class=""address-info"">([\s\S]*?) -";//手机端地区
                            string Rxg3 = @"小区：([\s\S]*?)</h2>";//手机端小区
                            string Rxg4 = @"面积</p>([\s\S]*?)</p>"; //手机端面积去除标签
                            string Rxg5 = @"售价</p>([\s\S]*?)</p>"; //手机端售价去除标签



                            Match titles = Regex.Match(strhtml, title);
                            Match contacts = Regex.Match(strhtml2, Rxg);                                                        //手机端正则匹配联系人
                            Match tell = Regex.Match(strhtml, Rxg1);
                            Match area = Regex.Match(strhtml2, Rxg2);

                        Match xiaoqu = Regex.Match(strhtml2, Rxg3);
                        Match mianji = Regex.Match(strhtml2, Rxg4);
                        Match price = Regex.Match(strhtml2, Rxg5);


                            this.skinDataGridView1.Rows[index].Cells[0].Value = titles.Groups[1].Value;
                                                       
                            this.skinDataGridView1.Rows[index].Cells[1].Value = contacts.Groups[1].Value;

                        this.skinDataGridView1.Rows[index].Cells[2].Value = tell.Groups[1].Value;


                            this.skinDataGridView1.Rows[index].Cells[3].Value = label14.Text+""+area.Groups[1].Value;

                            
                        this.skinDataGridView1.Rows[index].Cells[4].Value = xiaoqu.Groups[1].Value;


                        string temp = Regex.Replace(mianji.Groups[1].Value, "<[^>]*>", "");
                        this.skinDataGridView1.Rows[index].Cells[5].Value = temp.Trim();

                        string temp1 = Regex.Replace(price.Groups[1].Value, "<[^>]*>", "");
                        this.skinDataGridView1.Rows[index].Cells[6].Value = temp1.Trim();



                        this.skinDataGridView1.Rows[index].Cells[7].Value = lines[j];





                            this.skinDataGridView1.CurrentCell = this.skinDataGridView1.Rows[index].Cells[0];                         //让datagridview滚动到当前行



                            if (skinButton13.Text == "已停止")
                            {

                                return;


                            }
                            Application.DoEvents();
                            System.Threading.Thread.Sleep(time);   //内容获取间隔，可变量



                        }
                        textBox14.Text = "";

                    }
                }
             

           
            catch (System.Exception ex)
            {

                textBox3.Text = ex.ToString();
            }

        }

        #endregion

        #region  发送邮件
        public void SendMessage()
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("852266010@qq.com");//发件人
            message.To.Add(new MailAddress("2927353003@qq.com"));//收件人(可以是多个)
            message.Subject = "第一次尝试发邮件"; //主题
            message.Body = "这是我发的邮件"; //邮件正文

            // mm.SubjectEncoding = System.Text.Encoding.GetEncoding(963);//如果是乱码就需要此转码

            //  mm.SubjectEncoding = System.Text.Encoding.ASCII;

            SmtpClient sc = new SmtpClient();


            sc.Host = "smtp.qq.com";//设置SMTP主机的名称或IP地址

            sc.EnableSsl = true;
            sc.Credentials = new System.Net.NetworkCredential("852266010@qq.com", "pbhninqhupplbeih");//设置用于验证发件人身份的凭据
            sc.Send(message);
            MessageBox.Show("发送成功");


        }


        #endregion

        #region  二手车
        public void Che()
        {
            try
            {

                string city = label6.Text;

                if (city == "")
                {
                    MessageBox.Show("请选择城市！");
                    return;
                }

                for (int i = 1; i <= page; i++)
                {
                    String Url = "http://"+city+".ganji.com/ershouche/a1o"+i+"/";
                    string html = Method.GetUrl(Url);

                    MatchCollection TitleMatchs = Regex.Matches(html, @"id=""puid-([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                    foreach (Match NextMatch in TitleMatchs)
                    {

                        textBox14.Text += "https://3g.ganji.com/"+city+"_ershouche/"+NextMatch.Groups[1].Value + "x"+"\r\n";

                        textBox14.SelectionStart = textBox14.Text.Length; //设定光标位置
                        textBox14.ScrollToCaret(); //滚动到光标处


                    }

                    
                    string tm1 = DateTime.Now.ToString();  //获取系统时间

                    textBox3.Text += tm1 + "-->正在采集第" + i + "页\r\n";

                    textBox3.SelectionStart = textBox3.Text.Length; //设定光标位置
                    textBox3.ScrollToCaret();

                    Application.DoEvents();
                    System.Threading.Thread.Sleep(500);  //网址获取时间间隔 固定不变

                    string[] lines = textBox14.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                    for (int j = 0; j < lines.Length - 1; j++)
                    {

                        this.index = this.skinDataGridView1.Rows.Add();

                        String Url1 = lines[j];
                        string strhtml = Method.GetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()


                        string rxg = @"title>([\s\S]*?)</title>";
                        string Rxg = @"联系人</span>([\s\S]*?)</span>";
                        string Rxg1 = @"data-phone=""([\s\S]*?)""";
                        string Rxg2 = @"province=([\s\S]*?);city=([\s\S]*?);";



                        Match titles = Regex.Match(strhtml, rxg);
                        Match contacts = Regex.Match(strhtml, Rxg);
                        Match tell = Regex.Match(strhtml, Rxg1);
                        Match region = Regex.Match(strhtml, Rxg2);


                       



                               
                                    this.skinDataGridView1.Rows[index].Cells[0].Value = titles.Groups[1].Value;
                                    this.skinDataGridView1.CurrentCell = this.skinDataGridView1.Rows[index].Cells[0];  //让datagridview滚动到当前行
                              
                                    string temp = Regex.Replace(contacts.Groups[1].Value, "<[^>]*>", "");   //去除所有标签去除所有标签

                                    string temp1 = Regex.Replace(temp, "\\s+", "");   //去除所有标签去除空格

                                    this.skinDataGridView1.Rows[index].Cells[1].Value = temp1;
                             

                                    this.skinDataGridView1.Rows[index].Cells[2].Value = tell.Groups[1].Value;

                               

                                    this.skinDataGridView1.Rows[index].Cells[3].Value = region.Groups[2].Value;

                                

                                if (skinButton13.Text == "已停止")
                                {
                                    
                                    return;


                                }
                            
                        

                        Application.DoEvents();
                        System.Threading.Thread.Sleep(time);   //内容获取间隔，可变量

                    }
                    textBox14.Text = "";

                }



            }
            catch (System.Exception ex)
            {
                textBox3.Text = ex.ToString();
                //MessageBox.Show(ex.ToString());
            }
        }

            #endregion

        #region 土地租售
            public void tudi()
        {
            MessageBox.Show("开发中！请选择其他分类");
            return;
        }

        #endregion

        #region 主采集按钮
        private void skinButton1_Click(object sender, EventArgs e)
        {
            
            if (label15.Text == "测试用户" || label15.Text == "")
            {
                MessageBox.Show("请注册账号登陆！");
                return;
            }


            skinButton13.Text = "停止采集";



            if (radioButton1.Checked==true)
            {

                Thread thread = new Thread(new ParameterizedThreadStart(shangpu));
                string o = "shengyizr";
                thread.Start((object)o);
                //创建带参数的线程

                Control.CheckForIllegalCrossThreadCalls = false;                
                label8.Text = radioButton1.Text;

            }
            else if (radioButton5.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(ershoufang));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();
                label8.Text = radioButton5.Text;
            }
            else if (radioButton3.Checked == true)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(shangpu));
                string o = "shangpucz";
                thread.Start((object)o);
                label8.Text = radioButton3.Text;    
            }

            else if (radioButton2.Checked == true)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(shangpu));
                string o = "shangpucs";
                thread.Start((object)o);
                //创建带参数的线程
                label8.Text = radioButton2.Text;
            }
            else if (radioButton4.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(ershoufang));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();

                label8.Text = radioButton4.Text;
            }
            else if (radioButton6.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(Che));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();
                label8.Text = radioButton6.Text;
            }
            else if (radioButton8.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(changfang));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();
                label8.Text = radioButton8.Text;
            }

            else if (radioButton7.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(chewei));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();
                label8.Text = radioButton7.Text;
            }
         
            else if (radioButton9.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(tudi));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();
                label8.Text = radioButton9.Text;
            }



            else
            {
                MessageBox.Show("请选择采集模板！");
            }

           

        }
        #endregion












        private void skinButton14_Click(object sender, EventArgs e)
        {
            SendMessage();
        }


        #region  线程间调用测试
        public void A() {

            for (int i = 0; i < 100; i++) { 

            textBox3.Text  += i.ToString();
                System.Threading.Thread.Sleep(1000);
         }
        }

        public void B()
        {

            for (int i = 0; i < 100; i++)
            {

                textBox14.Text = textBox3.Text;
                System.Threading.Thread.Sleep(1000);
            }
        }

        public void C()
        {
            Thread thread = new Thread(new ThreadStart(A));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();

            Thread thread1 = new Thread(new ThreadStart(B));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread1.Start();


        }

        #endregion

        private void skinButton13_Click(object sender, EventArgs e)
        {
            skinButton13.Text = "已停止";

        } 

        

        private void skinButton2_Click(object sender, EventArgs e)
        {
            //foreach (TreeNode tnn in skinTreeView1.Nodes)
            //{
            //    foreach (TreeNode tn in tnn.Nodes)
            //    {
            //        textBox3.Text += tn.Text + "," + tn.Name + "\r\n";

            //    }


            //}


        }
        public void clear(object sender, EventArgs e)
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

                menu.MenuItems[0].Click += new EventHandler(clear);
                textBox14.Text = "";
            }
        }

        private void 登陆ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 登陆账号ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login lg = new Login();
            lg.Show();
        }

        private void fang_58_MouseEnter(object sender, EventArgs e)
        {
            label15.Text = Method.User; //获取Method公共类的静态变量User的值
        }

        private void 注册账号ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            register rg = new register();
            rg.Show();
        }

        private void 合作模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void 联系我们ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void fang_58_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            MessageBox.Show("您确定要关闭吗？");
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
           

                Method.DgvToTable(this.skinDataGridView1);
                Method.DataTableToExcel(Method.DgvToTable(this.skinDataGridView1), "Sheet1", true);
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            Method.Txt(skinDataGridView1);
        }

        private void fang_58_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show(e.KeyCode.ToString());
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("IEXPLORE.EXE", "http://wpa.qq.com/msgrd?v=3&uin=852266010&site=qq&menu=yes");
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("IEXPLORE.EXE", "http://wpa.qq.com/msgrd?v=3&uin=852266010&site=qq&menu=yes");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://acaiji.com/buy.aspx");
        }
    }

    
}
