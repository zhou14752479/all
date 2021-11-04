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

namespace 思忆美团
{
    public partial class 思忆美团 : Form
    {
        class ListViewNF : System.Windows.Forms.ListView
        {
            public ListViewNF()
            {
                // 开启双缓冲
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

                // Enable the OnNotifyMessage event so we get a chance to filter out 
                // Windows messages before they get to the form's WndProc
                this.SetStyle(ControlStyles.EnableNotifyMessage, true);
            }

            protected override void OnNotifyMessage(Message m)
            {
                //Filter out the WM_ERASEBKGND message
                if (m.Msg != 0x14)
                {
                    base.OnNotifyMessage(m);
                }
            }
        }




        public 思忆美团()
        {
            InitializeComponent();
        }

        bool zanting = true;
        bool status = true;
        Thread thread;
       
        functions fc = new functions();
        private void button3_Click(object sender, EventArgs e)
        {
          
            tabControl1.SelectedIndex = 2;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void 思忆美团_Load(object sender, EventArgs e)
        {
            ProvinceCity.ProvinceCity.BindProvince(comboBox2);
            this.tabControl1.Region = new Region(new RectangleF(this.tabPage1.Left, this.tabPage1.Top, this.tabPage1.Width, this.tabPage1.Height));



            //fc.Getcates(comboBox1);



            if (fc.ExistINIFile())
            {
               user_text.Text = fc.IniReadValue("values", "username");
                pass_text.Text = fc.IniReadValue("values", "password");
              
            }


        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {

            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private Point mPoint = new Point();

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
            }
        }

        private void exportExl_btn_Click(object sender, EventArgs e)
        {
            functions.DataTableToExcel(functions.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void clear_btn_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text.Contains("上海"))
            {
                textBox1.Text += "上海";
                return;
            }
            if (comboBox2.Text.Contains("北京"))
            {
                textBox1.Text += "北京";
                return;
            }
            if (comboBox2.Text.Contains("重庆"))
            {
                textBox1.Text += "重庆";
                return;
            }
            if (comboBox2.Text.Contains("天津"))
            {
                textBox1.Text += "天津";
                return;
            }
            ProvinceCity.ProvinceCity.BindCity(comboBox2, comboBox3);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text += "\r\n"+ comboBox3.Text ;
        }

       

        private void start_btn_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = functions.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"NtTtu"))
            {
                MessageBox.Show("");
                return;
            }



            #endregion
            functions.viptime = "2022-06-06";
            functions.username = "222222";
            if (functions.username == "")
            {
                infolabel.Text = "请登录账号！";
                MessageBox.Show("请登录账号！");
                tabControl1.SelectedIndex = 2;
                return;
            }

            if (textBox1.Text == "")
            {
                infolabel.Text = "请选择城市！";
                MessageBox.Show("请选择城市！");
                return;
            }

            if (functions.catename_selected == "")
            {
                infolabel.Text = "请选择分类！";
                MessageBox.Show("请选择分类！");
                return;
            }
            status = true;
           
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
        ArrayList finishes = new ArrayList();

        public bool shaixuan()
        {

            return true;
        }


        public string getip()
        {
            string html = functions.GetUrl("http://47.106.170.4:8081/Index-generate_api_url.html?packid=7&fa=5&groupid=0&fetch_key=&qty=1&port=1&format=txt&ss=1&css=&pro=&city=&usertype=7","utf-8");
          
            return html;
        }

        #region  主程序
        public void run()
        {
           // string ip = getip();
            try
            {
                string[] citys = textBox1.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string city in citys)
                {
                    if (city == "")
                    {
                        continue;
                    }
                    string cityid = fc.GetcityId(city.Replace("市",""));
                    Dictionary<string, string> areadics = fc.getareas2(cityid);
                    foreach (string areaid in areadics.Values)
                    {
                        string[] catenames = functions.catename_selected.Trim().Split(new string[] { "," }, StringSplitOptions.None);
                        foreach (string catename in catenames)
                        {
                            if(catename=="")
                            {
                                continue;
                            }
                            string cateid = functions.catedic[catename];
                           
                            for (int page = 0; page < 1001; page = page + 100)
                            {
                                if (functions.viptime == "" || Convert.ToDateTime(functions.viptime) < DateTime.Now)
                                {
                                    infolabel.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"：账号已过期，请充值，若已充值，请重新登录！";
                                    return;
                                }
                               
                                string url = "https://m.dianping.com/mtbeauty/index/ajax/shoplist?token=&cityid=" + cityid + "&cateid=22&categoryids=" + cateid + "&lat=&lng=&userid=&uuid=&utm_source=meituan-wxapp&utmmedium=&utmterm=&utmcontent=&versionname=&utmcampaign=&mock=0&openid=&mtlite=false&start=" + page + "&limit=100&areaid=" + areaid + "&distance=&subwaylineid=&subwaystationid=&sort=2";

                                string html = functions.GetUrl(url, "utf-8");  //定义的GetRul方法 返回 reader.ReadToEnd()

                               if(html.Contains("已禁止") && html.Contains("Exception"))
                                {
                                    MessageBox.Show("当前IP被美团禁止访问，请更换IP，或者等待恢复");
                                    status = false;
                                    return;
                                }
                                // string html = functions.GetUrlwithIP(url, ip,"","utf-8");
                                //if (html == "" || html == null)
                                //{
                                //    ip = getip();
                                //    infolabel.Text = ip;
                                //    page = page - 1;
                                //    continue;
                                //}
                                MatchCollection names = Regex.Matches(html, @"""shopName"":""([\s\S]*?)""");
                                MatchCollection address = Regex.Matches(html, @"""address"":""([\s\S]*?)""");
                                MatchCollection phone = Regex.Matches(html, @"""phone"":""([\s\S]*?)""");
                                MatchCollection cate = Regex.Matches(html, @"mainCategoryName"":""([\s\S]*?)""");
                                MatchCollection shangquan = Regex.Matches(html, @"""areaName"":""([\s\S]*?)""");

                                infolabel.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：正在采集" + city + "-" + areaid + "-" + catename + "-页码：" +((page/100)+1);
                              
                                for (int j = 0; j < names.Count; j++)
                                {
                                    try
                                    {
                                        string newphone = shaixuan(phone[j].Groups[1].Value);
                                        if (newphone != "")
                                        {
                                            if (!finishes.Contains(newphone))
                                            {
                                                ListViewItem listViewItem = listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                                listViewItem.SubItems.Add(fc.Unicode2String(names[j].Groups[1].Value));
                                                listViewItem.SubItems.Add(fc.Unicode2String(address[j].Groups[1].Value));
                                                listViewItem.SubItems.Add(newphone);
                                                listViewItem.SubItems.Add(cate[j].Groups[1].Value);
                                                listViewItem.SubItems.Add(shangquan[j].Groups[1].Value);
                                                listViewItem.SubItems.Add(city);
                                                Thread.Sleep(200);
                                                if (listView1.Items.Count > 2)
                                                {
                                                    this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                                                }
                                                while (this.zanting == false)
                                                {
                                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                                }
                                                if (status == false)
                                                    return;
                                            }
                                        }

                                    }
                                    catch (Exception)
                                    {

                                        continue;
                                    }
                                }

                                if (names.Count == 0 || names.Count < 100)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集
                                {
                                    break;
                                }



                                Thread.Sleep(1000);
                            }
                        }

                    }
                }

                infolabel.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"：采集完成！";
            }
            catch (System.Exception ex)
            {
             
                infolabel.Text=(ex.ToString());
            }



        }



        #endregion


        #region 筛选
        public string shaixuan(string tel)
        {
            try
            {
                string haoma = tel;
                string[] tels = tel.Split(new string[] { "/" }, StringSplitOptions.None);

                if (checkBox1.Checked == true)
                {
                    if (tels.Length == 0)
                    {
                        haoma = "";
                        return haoma;
                    }

                }
                if (checkBox2.Checked == true)
                {
                    if (tels.Length == 1)
                    {
                        if (!tel.Contains("-") && tels[0].Length > 10)
                        {
                            haoma = tel;
                            return haoma;
                        }
                        else
                        {
                            return "";
                        }

                    }

                    if (tels.Length == 2)
                    {
                        if (!tels[0].Contains("-") && tels[0].Length > 10)
                        {
                            haoma = tels[0];
                        }

                        else if (!tels[1].Contains("-") && tels[1].Length > 10)
                        {
                            haoma = tels[1];
                        }
                        else
                        {
                            haoma = "";
                        }
                    }
                }
                if (checkBox3.Checked == true)
                {
                    finishes.Add(haoma);
                }
                return haoma;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(tel+"   " +ex.ToString());
                return "";
            }


        }
        #endregion
        private void pause_btn_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void stop_btn_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void exportTxt_btn_Click(object sender, EventArgs e)
        {
            functions.ListviewToTxt(listView1,3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.acaiji.com/alipay/");

            }
            catch (Exception)
            {

                MessageBox.Show("打开失败，请复制网址到浏览器打开") ;
            }
        }

        private void register_btn_Click(object sender, EventArgs e)
        {
            string isregister = fc.IniReadValue("values", "register");
            if(isregister=="1")
            {
                MessageBox.Show("请勿重复注册");
                return;

            }
            if (user_text.Text == "" || pass_text.Text == "")
            {
                MessageBox.Show("请输入账号和密码");
                return;
            }
            bool msg = fc.register(user_text.Text.Trim(), pass_text.Text.Trim());
            if (msg == true)
            {
                functions.username = user_text.Text.Trim();
                fc.IniWriteValue("values", "username", user_text.Text.Trim());
                fc.IniWriteValue("values", "password", pass_text.Text.Trim());
                fc.IniWriteValue("values", "register", "1");
                tabControl1.SelectedIndex = 0;
            }
          

        }

        private void login_btn_Click(object sender, EventArgs e)
        {
            if (user_text.Text == "" || pass_text.Text == "")
            {
                MessageBox.Show("请输入账号和密码");
                return;
            }
            bool msg = fc.login(user_text.Text.Trim(), pass_text.Text.Trim());
            if (msg == true)
            {
                functions.username = user_text.Text.Trim();
                fc.IniWriteValue("values", "username", user_text.Text.Trim());
                fc.IniWriteValue("values", "password", pass_text.Text.Trim());
                tabControl1.SelectedIndex = 0;
            }
           
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.acaiji.com/");

            }
            catch (Exception)
            {

                MessageBox.Show("打开失败，请复制网址到浏览器打开");
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://wpa.qq.com/msgrd?v=3&uin=852266010&site=qq&menu=yes&from=message&isappinstalled=0");

            }
            catch (Exception)
            {

                MessageBox.Show("打开失败，请复制网址到浏览器打开");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            选择分类 cate = new 选择分类();
            cate.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
