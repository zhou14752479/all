using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 主程序202106
{
    public partial class 知网邮箱采集 : Form
    {
        public 知网邮箱采集()
        {
            InitializeComponent();
        }


        Thread thread;
        bool zanting = true;
        bool status = true;
        string cookie = "";
     




        public void getzazhis()
        {
            try
            {
                status = true;
                for (int i = Convert.ToInt32(numericUpDown6.Value); i < Convert.ToInt32(numericUpDown5.Value); i++)
                {
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                 
                        string url = "https://navi.cnki.net/knavi/Common/Search/Journal";
                      
                        string postdata = "SearchStateJson=%7B%22StateID%22%3A%22%22%2C%22Platfrom%22%3A%22%22%2C%22QueryTime%22%3A%22%22%2C%22Account%22%3A%22knavi%22%2C%22ClientToken%22%3A%22%22%2C%22Language%22%3A%22%22%2C%22CNode%22%3A%7B%22PCode%22%3A%22CJFD%22%2C%22SMode%22%3A%22%22%2C%22OperateT%22%3A%22%22%7D%2C%22QNode%22%3A%7B%22SelectT%22%3A%22%22%2C%22Select_Fields%22%3A%22%22%2C%22S_DBCodes%22%3A%22%22%2C%22QGroup%22%3A%5B%7B%22Key%22%3A%22Navi%22%2C%22Logic%22%3A1%2C%22Items%22%3A%5B%5D%2C%22ChildItems%22%3A%5B%7B%22Key%22%3A%22Journal%22%2C%22Logic%22%3A1%2C%22Items%22%3A%5B%7B%22Key%22%3A1%2C%22Title%22%3A%22%22%2C%22Logic%22%3A1%2C%22Name%22%3A%22168%E4%B8%93%E9%A2%98%E4%BB%A3%E7%A0%81%22%2C%22Operate%22%3A%22%22%2C%22Value%22%3A%22"+textBox3.Text.Trim()+"%3F%22%2C%22ExtendType%22%3A0%2C%22ExtendValue%22%3A%22%22%2C%22Value2%22%3A%22%22%7D%5D%2C%22ChildItems%22%3A%5B%5D%7D%5D%7D%5D%2C%22OrderBy%22%3A%22OTA%7CDESC%22%2C%22GroupBy%22%3A%22%22%2C%22Additon%22%3A%22%22%7D%7D&displaymode=2&pageindex="+i+"&pagecount=20&index=1&random=0.7702059787437423";

                        string html = method.PostUrl(url, postdata, cookie,"utf-8", "application/x-www-form-urlencoded", "https://navi.cnki.net/knavi/All.html");
                  
                    MatchCollection ahtmls = Regex.Matches(html, @"<h2>([\s\S]*?)</div>");


                  
                    for (int j = 0; j < ahtmls.Count; j++)
                    {
                        string ahtml = ahtmls[j].Groups[1].Value;
                        string uid = Regex.Match(ahtml, @"pcode=([\s\S]*?)""").Groups[1].Value.Replace("amp;","");
                        string title = Regex.Match(ahtml, @"baseid=([\s\S]*?)"">([\s\S]*?)</a>").Groups[2].Value;
                        string zhuban = Regex.Match(ahtml, @"tab_2"">([\s\S]*?)</span>").Groups[1].Value;
                        string yin = Regex.Match(ahtml, @"tab_3"">([\s\S]*?)</span>").Groups[1].Value;
                        string down = Regex.Match(ahtml, @"tab_4"">([\s\S]*?)</span>").Groups[1].Value;
                        string beiyin = Regex.Match(ahtml, @"tab_5"">([\s\S]*?)</span>").Groups[1].Value;
                      ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        try
                        {
                            lv1.SubItems.Add(uid);
                            lv1.SubItems.Add(title);
                            lv1.SubItems.Add(zhuban);
                            lv1.SubItems.Add(yin);
                            lv1.SubItems.Add(down);
                            lv1.SubItems.Add(beiyin);
                            if (status == false)
                                return;
                        }
                        catch (Exception)
                        {

                            lv1.SubItems.Add("");
                        }
                       
                       
                    }
                       

                        Thread.Sleep(1000);

                      
                    }

              
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        Random ra = new Random();
        public void getmails()
        {
            status = true;
            for (int i = 0; i < listView1.CheckedItems.Count; i++)
            {

            string aid = listView1.CheckedItems[i].SubItems[1].Text;
                for (int a = 0; a< listView1.CheckedItems.Count; a++)
                {
                    listView1.CheckedItems[a].BackColor = Color.White;
                }
                    listView1.CheckedItems[i].BackColor = Color.Yellow;


                aid = aid.Replace("baseid", "pykm");
            log_txtbox.Text += DateTime.Now.ToLongTimeString() + "开始采集" +aid+"\r\n";

            for (int year = Convert.ToInt32(numericUpDown2.Value); year >= Convert.ToInt32(numericUpDown1.Value); year--)
            {
                    for (int qikan = 1; qikan < 7; qikan++)
                    {
                        log_txtbox.Text += DateTime.Now.ToLongTimeString() + "开始采集" + year + "年  " + qikan + "期刊" + "\r\n";
                        try
                        {
                            string url = "https://navi.cnki.net/knavi/JournalDetail/GetArticleList?year=" + year + "&issue=0" + qikan + "&pcode=" + aid + "&pageIdx=0";
                            string html = method.GetUrl(url, "utf-8");
                            MatchCollection aurls = Regex.Matches(html, @"<a class=""journalshares"" href=""([\s\S]*?)filename=([\s\S]*?)&");

                            if (aurls.Count == 0)  //期刊内不存在文章
                            {
                                log_txtbox.Text += year + "  " + qikan + "期刊内不存在文章" + "\r\n";
                                continue;
                            }
                            for (int j = 0; j < aurls.Count; j++)
                            {
                              
                                this.textBox5.Focus();
                                this.textBox5.Select(this.textBox5.TextLength, 0);
                                this.textBox5.ScrollToCaret();
                                this.log_txtbox.SelectionStart = this.log_txtbox.Text.Length;
                                this.log_txtbox.ScrollToCaret();



                                log_txtbox.Text += DateTime.Now.ToLongTimeString() + "开始采集" + aurls[j].Groups[2].Value.Trim() + "\r\n";
                                string aurl = "https://kns.cnki.net/KXReader/Detail?FileName=" + aurls[j].Groups[2].Value.Trim();

                                string ahtml = method.GetUrlWithCookie(aurl, cookie, "utf-8");
                                if (ahtml.Contains("中国知网-登录"))
                                {
                                    log_txtbox.Text += DateTime.Now.ToLongTimeString() + "登录状态过期" + "\r\n";
                                    zanting = false;
                                    MessageBox.Show("登录状态过期,请重新登录后，然后点击【暂停/继续】");
                                }
                                if (ahtml.Contains("操作太过频繁"))
                                {
                                    log_txtbox.Text += DateTime.Now.ToLongTimeString() + "操作太过频繁,请重新登录" + "\r\n";
                                    zanting = false;
                                    MessageBox.Show("操作太过频繁,请重新登录后，然后点击【暂停/继续】");
                                }
                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                                //textBox5.Text = ahtml;
                                //MessageBox.Show("1");

                                string mail = Regex.Match(ahtml, @"mail:([\s\S]*?)<").Groups[1].Value.Replace("amp;", "").Trim();
                                mail = Regex.Replace(mail, "phone.*", "");
                                mail = Regex.Replace(mail, "。.*", "");
                                if (mail != "")
                                {
                                    textBox5.Text +=  mail+ "\r\n";
                                }
                                else
                                {
                                   
                                    log_txtbox.Text +=DateTime.Now.ToLongTimeString()+  "邮箱地址为空跳过" + "\r\n";
                                }

                                int suiji = ra.Next(Convert.ToInt32(numericUpDown4.Value), Convert.ToInt32(numericUpDown3.Value));
                                Thread.Sleep(suiji*1000);
                                if (status == false)
                                    return;
                            }




                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.ToString());
                        }

                    }
                }
            }
           
        }

        Dictionary<string, string> catedics = new Dictionary<string, string>();
        //public void getcates()
        //{

        //    string url = "https://navi.cnki.net/knavi/Common/LeftNavi/All?productcode=SCDB&index=1&random=0.8109649386952973";
        //    string html = method.GetUrl(url, "utf-8");
        //    MatchCollection names = Regex.Matches(html, @"专题子栏目代码','([\s\S]*?)','([\s\S]*?)'");
        //    for (int i = 0; i < names.Count; i++)
        //    {
        //        if (names[i].Groups[1].Value.Length == 1)
        //        {
        //            catedics.Add(names[i].Groups[2].Value, names[i].Groups[1].Value);
        //            comboBox1.Items.Add(names[i].Groups[2].Value);
        //            textBox5.Text += "catedics.Add(\"" + names[i].Groups[2].Value+"\""+","+ "\""+ names[i].Groups[1].Value + "\");" + "\r\n";
        //        }
        //    }
        //}
        private void 知网邮箱采集_Load(object sender, EventArgs e)
        {
            catedics.Add("基础科学", "A");
            catedics.Add("工程科技Ⅰ辑", "B");
            catedics.Add("工程科技Ⅱ辑", "C");
            catedics.Add("农业科技", "D");
            catedics.Add("医药卫生科技", "E");
            catedics.Add("哲学与人文科学", "F");
            catedics.Add("社会科学Ⅰ辑", "G");
            catedics.Add("社会科学Ⅱ辑", "H");
            catedics.Add("信息科技", "I");
            catedics.Add("经济与管理科学", "J");


            //getcates();
        }

    
        private void start_btn_Click(object sender, EventArgs e)
        {
            #region 通用检测


            string html = method.GetUrl("http://www.acaiji.com:8080/api/vip.html", "utf-8");

            if (!html.Contains(@"YOaO6Wx"))
            {

                return;
            }

            #endregion
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getzazhis);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

       

        private void stop_btn_Click(object sender, EventArgs e)
        {
            status = false;
        }


        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            //for (int i = 0; i <listView1.CheckedItems.Count; i++)
            //{
            //    textBox4.Text += listView1.CheckedItems[i].SubItems[1].Text+",";
            //}
            for (int i = 0; i < listView1.CheckedItems.Count; i++)
            {
                textBox4.Text = listView1.CheckedItems[i].SubItems[1].Text;
            }
        }


        Thread thread1;
        public void getnowcookie()
        {
            try
            {

                StreamReader sr = new StreamReader(path + "cookie.txt", method.EncodingType.GetTxtType(path + "cookie.txt"));
                //一次性读取完 
                string texts = sr.ReadToEnd();

                cookie = Regex.Match(texts, @"cookie=([\s\S]*?)&").Groups[1].Value;
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            getnowcookie();

            status = true;
            if (thread1 == null || !thread1.IsAlive)
            {
                thread1 = new Thread(getmails);
                thread1.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;
        private void login_btn_Click(object sender, EventArgs e)
        {
           
            Process.Start(path + "helper.exe");
        }
        #region  txtbox导出文本TXT
        public static void ListviewToTxt(string txt)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            // string path = AppDomain.CurrentDomain.BaseDirectory + "导出_" + Guid.NewGuid().ToString() + ".txt";
            string path = "";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                path = sfd.FileName + ".txt";
            }

      

            System.IO.File.WriteAllText(path, txt, Encoding.UTF8);
            MessageBox.Show("文件导出成功!文件地址:" + path);


        }






        #endregion
        private void button3_Click(object sender, EventArgs e)
        {
            ListviewToTxt(textBox5.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
            getnowcookie();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox3.Text = catedics[comboBox1.Text];
            
        }

        private void log_txtbox_TextChanged(object sender, EventArgs e)
        {
            this.log_txtbox.SelectionStart = this.log_txtbox.Text.Length;
            this.log_txtbox.SelectionLength = 0;
            this.log_txtbox.ScrollToCaret();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            this.textBox5.SelectionStart = this.textBox5.Text.Length;
            this.textBox5.SelectionLength = 0;
            this.textBox5.ScrollToCaret();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            for (int a = 0; a < listView1.Items.Count; a++)
            {
                listView1.Items[a].Checked=true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            for (int a = 0; a < listView1.Items.Count; a++)
            {
                listView1.Items[a].Checked = false;
            }
        }
    }
}
