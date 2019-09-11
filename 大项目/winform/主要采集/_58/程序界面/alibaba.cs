using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Threading;
using System.Drawing;


namespace _58
{
    public partial class alibaba : Form
    {
        public alibaba()
        {
            InitializeComponent();
        }



        int index { get; set; }
        private bool Condition=true;  //定义数据采集筛选条件，默认为真不筛选
        private Match tell;

        #region Get请求
        public static string GetUrl(string Url, string COOKIE)
        {
            try
            {
                
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);
               
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.AllowAutoRedirect = true;
                request.KeepAlive = true;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gb2312")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string content = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                ex.ToString();



            }
            return "";
        }
        #endregion

        

        #region  主程序

        public void Run()
        {

            visualButton2.Text = "停止采集";
            try
            {

                int page = 100;                
                
                string keywords = System.Web.HttpUtility.UrlEncode(textBox3.Text.Trim(), System.Text.Encoding.GetEncoding("GB2312"));

                string[] citys = textBox2.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string city in citys)

                {
                    string city2312 = System.Web.HttpUtility.UrlEncode(city, System.Text.Encoding.GetEncoding("GB2312"));

                    for (int i = 1; i < page; i++)
                    {

                        String Url = "https://s.1688.com/company/company_search.htm?keywords="+keywords+"&city="+city2312+"&n=y&filt=y&pageSize=30&offset=3&beginPage="+i;

                        string html = GetUrl(Url,textBox1.Text);
                        

                        MatchCollection TitleMatchs = Regex.Matches(html, @"offer-stat=""com"" title=""([\s\S]*?)"" target=""_blank"" href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                        ArrayList lists = new ArrayList();
                        foreach (Match NextMatch in TitleMatchs)
                        {

                            lists.Add(NextMatch.Groups[2].Value);

                        }

                         
                        if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;

                        string tm1 = DateTime.Now.ToString();  //获取系统时间

                        textBox4.Text += tm1 + "-->正在采集" + city + "" + textBox3.Text + "第" + i + "页"+"\r\n";

                        foreach (string list in lists)
                        {
                            

                            

                            String Url1 = list;
                            string strhtml = GetUrl(Url1,textBox1.Text);  //定义的GetRul方法 返回 reader.ReadToEnd()

                            string Rxg = @"compname"">([\s\S]*?)</a>";
                            string Rxg1 = @"data-no=""([\s\S]*?)""";
                            string Rxg2 = @"class=""membername"" target=""_blank"">([\s\S]*?)    ";

                            string Rxg3 = @"所在地区：</label>([\s\S]*?)</span>";
                            string Rxg4 = @"data-encodeid=""([\s\S]*?)""";
                            string Rxg5 = @"经营模式：</label>([\s\S]*?)</span>";
                          




                            Match name = Regex.Match(strhtml, Rxg);
                            this.tell = Regex.Match(strhtml, Rxg1);
                            Match lxr = Regex.Match(strhtml, Rxg2);
                            Match area = Regex.Match(strhtml, Rxg3);
                            Match wangwang = Regex.Match(strhtml, Rxg4);
                            Match moshi = Regex.Match(strhtml, Rxg5);

                            if (radioButton1.Checked == true)
                            {
                                this.Condition = this.tell.Groups[1].Value != "";
                            }


                            if (this.Condition)
                            {
                                this.index = this.skinDataGridView1.Rows.Add();

                                this.skinDataGridView1.Rows[index].Cells[0].Value = name.Groups[1].Value;
                                this.skinDataGridView1.CurrentCell = this.skinDataGridView1.Rows[index].Cells[0];  //让datagridview滚动到当前行
                                this.skinDataGridView1.Rows[index].Cells[1].Value = this.tell.Groups[1].Value;
                                this.skinDataGridView1.Rows[index].Cells[2].Value = lxr.Groups[1].Value.Replace("</a>&nbsp;", "").Replace("&nbsp;", "");

                                string temp = Regex.Replace(area.Groups[1].Value, "<[^>]*>", "");   //去除所有标签去除所有标签去除所有标签去除所有标签去除所有标签去除所有标签去除所有标签

                                string temp1 = Regex.Replace(temp, "\\s+", "");     //去掉空格去掉空格去掉空格去掉空格去掉空格去掉空格去掉空格去掉空格去掉空格
                                this.skinDataGridView1.Rows[index].Cells[3].Value = temp1;



                                this.skinDataGridView1.Rows[index].Cells[4].Value = wangwang.Groups[1].Value;


                            string temp2 = Regex.Replace(moshi.Groups[1].Value, "<[^>]*>", "");   //去除所有标签去除所有标签去除所有标签去除所有标签去除所有标签去除所有标签去除所有标签

                            string temp3 = Regex.Replace(temp2, "\\s+", "");     //去掉空格去掉空格去掉空格去掉空格去掉空格去掉空格去掉空格去掉空格去掉空格

                            this.skinDataGridView1.Rows[index].Cells[5].Value = temp3;
                            

                            this.skinDataGridView1.Rows[index].Cells[6].Value = Url1;

                            }

                            if (visualButton2.Text == "已停止")

                            {
                                return;
                            }




                            Application.DoEvents();
                            System.Threading.Thread.Sleep(1000);   //内容获取间隔，可变量
                            
                        }

                    }

                }
            }


            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

       
        private void alibaba_Load(object sender, EventArgs e)
        {
           
        }


       

        private void visualButton1_Click(object sender, EventArgs e)
        {
          
                Thread thread = new Thread(new ThreadStart(Run));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();
           

        }

        private void visualButton2_Click(object sender, EventArgs e)
        {
            visualButton2.Text = "已停止";

            
        }

        private void visualButton3_Click(object sender, EventArgs e)
        {
            Method.DgvToTable(this.skinDataGridView1);
            Method.DataTableToExcel(Method.DgvToTable(this.skinDataGridView1), "Sheet1", true);

        }

        private void skinTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            textBox2.Text += e.Node.Text + "\r\n";
        }

      
      

        private void alibaba_MouseEnter(object sender, EventArgs e)
        {
            label1.Text= Method.User; //获取Method公共类的静态变量User的值
            textBox1.Text = WebBrowser.cookie;
        }

        private void 查看教程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("点击登陆，使用自己账号登陆，然后获取COOKIE，输入采集参数，点击采集");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("IEXPLORE.EXE", "http://wpa.qq.com/msgrd?v=3&uin=852266010&site=qq&menu=yes");

            System.Diagnostics.Process.Start("IEXPLORE.EXE", "http://www.acaiji.com");
        }

        private void visualButton5_Click(object sender, EventArgs e)
        {
             WebBrowser web = new WebBrowser("https://login.1688.com/member/signin.htm");

            
         
            web.Show();
        }

     

        private void skinTreeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            if (textBox2.Text.Contains("从"))   //删除输入框内提示文本
            {
                textBox2.Text = "";
            }
            textBox2.Text += e.Node.Text + "\r\n";
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            textBox3.Text = "";
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            textBox2.Text = "";
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
                
            }
        }

        #region 鼠标右击事件

        public void clear(object sender, EventArgs e)
        {

            skinDataGridView1.Rows.Clear();

        }



        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            //Method.addPoints(label1.Text, Convert.ToInt32(label3.Text), Convert.ToInt32(textBox5.Text));
            System.Diagnostics.Process.Start("http://acaiji.com/buy.aspx");
        }
    }
}

