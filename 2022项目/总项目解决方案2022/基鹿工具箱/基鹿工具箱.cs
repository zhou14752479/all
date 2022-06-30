using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace 基鹿工具箱
{
    public partial class 基鹿工具箱 : Form
    {
        public 基鹿工具箱()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            tabControl1.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            imageList1.Images.Clear();  
            tabControl1.SelectedIndex = 1;
            List<string> icons = util.geticons();
            for (int i = 0; i < icons.Count; i++)
            {

                imageList1.Images.Add(util.GetImage(icons[i].ToString()));
                listView2.Items.Add(System.IO.Path.GetFileNameWithoutExtension(icons[i].ToString()), i);
                listView2.Items[i].ImageIndex = i;
                listView2.Items[i].Name = icons[i].ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
           
            tabControl1.SelectedIndex = 2;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 4;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 5;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 6;
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

        private void 基鹿工具箱_Load(object sender, EventArgs e)
        {

            try
            {
                string html = Util.getuser(Util.mobile, Util.logintoken);
               
                //MessageBox.Show(html);
                //textBox6.Text = Util.mobile+Util.logintoken+ html;
                string paytime = Regex.Match(html, @"""pay_time"":""([\s\S]*?)""").Groups[1].Value;
                if (paytime != "")
                {
                    string days = Regex.Match(html, @"""days"":""([\s\S]*?)""").Groups[1].Value;
                    if(days=="")
                    {
                        days = Regex.Match(html, @"""last_days"":([\s\S]*?),").Groups[1].Value;
                    }
                    Util.expiretime = Convert.ToDateTime(paytime).AddDays(Convert.ToInt32(days)).ToString("yyyy-MM-dd");
                }
               else
                {
                    Util.expiretime = "非会员";
                }

                //Util.expiretime = "2023-01-01";
                label4.Text = "有效期至:" + Util.expiretime;
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.ToString());
                
            }
           
          
            
         //   this.tabControl1.Region = new Region(new RectangleF(this.tabPage1.Left, this.tabPage1.Top, this.tabPage1.Width, this.tabPage1.Height));
            button1.Click += new System.EventHandler(btn_Click);
            button2.Click += new System.EventHandler(btn_Click);
            button3.Click += new System.EventHandler(btn_Click);
            button4.Click += new System.EventHandler(btn_Click);
            button5.Click += new System.EventHandler(btn_Click);
            button6.Click += new System.EventHandler(btn_Click);
            button7.Click += new System.EventHandler(btn_Click);
        }
        protected void btn_Click(object sender, EventArgs e)
        {
            foreach (Control ctl in panel2.Controls)
            {
                if (ctl is Button)
                {
                    if(ctl==(Button)sender)
                    {
                        ctl.BackColor = Color.LightGray;
                        ctl.ForeColor = Color.Black;
                    }
                    else
                    {
                       
                        ctl.BackColor = Color.FromArgb(255, 128, 0);
                        ctl.ForeColor = Color.White;
                    }
                   
                }
                

            }
           

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
           
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

       

       

        Thread thread;
        private void button12_Click(object sender, EventArgs e)
        {
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(alibaba);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }


        bool zanting = true;
        bool status = true;
        public void alibaba()
        {
            StreamReader sr = new StreamReader(key_txtbox.Text, Util.EncodingType.GetTxtType(key_txtbox.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] keywords = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存

            foreach (string keyword in keywords)
            {
                for (int page = 1; page < numericUpDown1.Value+1; page++)
                {
                    label6.Text = DateTime.Now.ToString()+ "：正在查询："+keyword+"，第"+page+"页";
                    // string url = "https://search.1688.com/service/marketOfferResultViewService?keywords="+ System.Web.HttpUtility.UrlEncode(keyword, Encoding.GetEncoding("GB2312")) + "&spm=a26352.13672862.searchbox.input&beginPage="+page+"&async=true&asyncCount=20&pageSize=60&startIndex=0&pageName=major&offset=8&sessionId=0201616cb2c84c7491698775cd957fbb&_bx-v=1.1.20";
                    string url = "https://s.1688.com/selloffer/offer_search.htm?keywords=" + System.Web.HttpUtility.UrlEncode(keyword, Encoding.GetEncoding("GB2312")) + "&n=y&netType=16&spm=a260k.dacugeneral.search.0&beginPage="+page+"#sm-filtbar";
                    string html = Util.GetUrl(url, "utf-8");
                    //textBox1.Text = html;
                    MatchCollection uids = Regex.Matches(html, @"""infoId"":([\s\S]*?),");
                    MatchCollection loginIds = Regex.Matches(html, @"""loginId"":""([\s\S]*?)""");
                    MatchCollection subjects = Regex.Matches(html, @"""subject"":""([\s\S]*?)""");

                    for (int i = 0; i < uids.Count; i++)
                    {
                        string uid = uids[i].Groups[1].Value.Trim();
                        string paiming = "无";
                        if(goodlist.Contains(uid))
                        {
                            Random rd = new Random();
                            int suiji = rd.Next(1, 9);
                            paiming = "第" + page + "页,第" + (i + 1) + "名";
                            string mobilepaiming = "第" + page + "页,第" + (i +suiji ) + "名";
                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(keyword);
                            lv1.SubItems.Add(Regex.Replace(subjects[i].Groups[1].Value, "<[^>]+>", ""));
                            lv1.SubItems.Add(paiming);
                            lv1.SubItems.Add(mobilepaiming);
                        }
                        label6.Text = DateTime.Now.ToString() + "：正在查询：" + keyword + "，第" + page + "页，产品"+ Regex.Replace(subjects[i].Groups[1].Value, "<[^>]+>", "") + "不符合";
                       
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;
                    }

                    Thread.Sleep(2000);
                }
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                key_txtbox.Text = openFileDialog1.FileName;
               
            }
        }

        private void button11_Click(object sender, EventArgs e)
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

        private void button14_Click(object sender, EventArgs e)
        {
            Util.DataTableToExcel(Util.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            //if (dr == DialogResult.OK)
            //{
            //    // Environment.Exit(0);
            //    System.Diagnostics.Process.GetCurrentProcess().Kill();
            //}
            //else
            //{
               
            //}
            this.Hide();
            login lg=new login();
            lg.Show();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            erweima ma = new erweima();
            ma.Show();
        }

        List<string> goodlist = new List<string>();
        private void button10_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                good_txtbox.Text = openFileDialog1.FileName;
                StreamReader sr = new StreamReader(good_txtbox.Text, Util.EncodingType.GetTxtType(good_txtbox.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] goods = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (var item in goods)
                {
                    if (item != "")
                    {
                     
                        goodlist.Add(item.Trim());
                    }
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.asinlu.com/index/service_show.html?id=71");
        }

      


        
        private void button21_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                good_txtbox.Text = openFileDialog1.FileName;
                textBox1.Text = openFileDialog1.FileName;
                StreamReader sr = new StreamReader(good_txtbox.Text, Util.EncodingType.GetTxtType(good_txtbox.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] goods = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (var item in goods)
                {
                    if (item != "")
                    {

                        Util.keys_list.Add(item.Trim());
                    }
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            }
        }



        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox4.Text == "")
                {
                    MessageBox.Show("请输入关键词");
                    return;
                }


                StringBuilder sb = new StringBuilder();
                string[] text = textBox4.Text.Trim().Split(new string[] { "," }, StringSplitOptions.None);
                foreach (var item in text)
                {
                    sb.Append(" ci like '%" + item + "%' or");
                }

                string sql2 = sb.ToString();
                if (sb.ToString().Substring(sb.ToString().Length - 2, 2) == "or")
                {
                    sql2 = sb.ToString().Remove(sb.ToString().Length - 2, 2);
                }
                string sql = "Select ci,ss_zs,fd,good_zs,gx_zs,pp,copy From datas where  " + sql2+ "order by ss_zs";

                DataTable dt = Util.getdata(sql);
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font
                    ("宋体", 13, FontStyle.Regular);
                dataGridView1.RowsDefaultCellStyle.Font = new Font
                    ("宋体", 11, FontStyle.Regular);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns["ci"].HeaderText = "关键词";
                dataGridView1.Columns["ss_zs"].HeaderText = "AI搜索值";
                dataGridView1.Columns["fd"].HeaderText = "AI搜索变化率";
                dataGridView1.Columns["good_zs"].HeaderText = "AI商品值";
                dataGridView1.Columns["gx_zs"].HeaderText = "机会指数";
                dataGridView1.Columns["pp"].HeaderText = "是否匹配";
                dataGridView1.Columns["copy"].HeaderText = "复制关键词";

                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)//如果DataGridView中有空的数据，则提示数据输入不完整并退出添加，不包括标题行
                {
                    dataGridView1.Rows[i].Cells[6].Value = "点击复制";
                    //比分
                    string key = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    if (Util.keys_list.Contains(key))
                    {
                        dataGridView1.Rows[i].Cells[5].Value = "匹配";
                    }
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 6)
                {
                    string key = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    System.Windows.Forms.Clipboard.SetText(key); //复制
                    MessageBox.Show("复制成功");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("值为空");
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.Clipboard.SetText(textBox3.Text.Trim()); //复制
                MessageBox.Show("复制成功");
            }
            catch (Exception)
            {

                MessageBox.Show("值为空");
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            string str = "";
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)//如果DataGridView中有空的数据，则提示数据输入不完整并退出添加，不包括标题行
            {
               
                string key = dataGridView1.Rows[i].Cells[5].Value.ToString();
                string keyvalue = dataGridView1.Rows[i].Cells[0].Value.ToString();
                if (key=="匹配")
                {
                    if (str == "")
                    {
                        str = keyvalue;
                    }
                    else
                    {
                        char[] strs = str.ToArray();
                        foreach (var item in strs)
                        {
                            if (keyvalue.Contains(item.ToString()))
                            {
                                keyvalue = keyvalue.Replace(item.ToString(), "");

                            }
                        }
                        str = str + keyvalue;
                    }

                }
            }

            textBox3.Text = str;
        }

      

        private void button23_Click(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.Minimum = DateTime.Now.AddDays(-30).ToOADate();
            
          
        }

       





        Util util = new Util();

        private void button18_Click(object sender, EventArgs e)
        {
            util.stop = false;
            util.reviewcountdic.Clear();
            util.reviewtimedic.Clear();


            Thread thread = new Thread(new ParameterizedThreadStart(util.getreview));
            string o1 = itemidtxt.Text.Trim();
            thread.Start((object)o1);
            Control.CheckForIllegalCrossThreadCalls = false;


            while (true)
            {
                button18.Text = "正在提取数据分析...";
                button18.Enabled = false;
                if(util.saledCount!=0)
                {
                    salecountlabel.Text = util.saledCount.ToString();
                    salecountlabel2.Text = util.saledCount2.ToString();
                }
                if (util.stop == true)
                {
                    button18.Text = "点击分析";
                    button18.Enabled = true;

                    Dictionary<DateTime,int> dic1 = util.reviewtimedic.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);
                   
                    //List<string> xData = new List<string>() { "A", "B", "C", "D", "E" };
                    //List<int> yData = new List<int>() { 10, 20, 30, 40, 100 };
                    chart1.Series[0]["PieLabelStyle"] = "Outside";//将文字移到外侧
                    chart1.Series[0]["PieLineColor"] = "Black";//绘制黑色的连线。
                    chart1.Series[0].Points.DataBindXY(dic1.Keys, dic1.Values);

                   
                    chart1.ChartAreas[0].AxisX.Minimum = Convert.ToDateTime("2022-04-01").ToOADate();


                    break;
                }
            }



        }

       
        private void button26_Click(object sender, EventArgs e)
        {
            util.stop = false;
            util.reviewcountdic.Clear();
            util.reviewtimedic.Clear();


            Thread thread = new Thread(new ParameterizedThreadStart(util.getreview));
            string o = itemidtxt2.Text.Trim();
            thread.Start((object)o);
            Control.CheckForIllegalCrossThreadCalls = false;

         


            while (true)
            {
                button26.Text = "正在提取数据分析...";
                button26.Enabled = false;
                if (util.saledCount != 0)
                {
                    salecountlabel.Text = util.saledCount.ToString();
                    salecountlabel2.Text = util.saledCount.ToString();
                }
                if (util.stop == true)
                {
                    button26.Text = "点击分析";
                    button26.Enabled = true;


                  
                   
                    chart2.Series[0].Points.DataBindXY(util.reviewcountdic.Keys, util.reviewcountdic.Values);

                    Legend legend1= new Legend("#VALX " +
                        "#PERCENT{P} #VALY");

                    chart2.Series["Series1"].Label = "#PERCENT{P}"; //显示占比
                                                                    // chart2.Series["Series1"].LegendText = "#VALX"; 
                    chart2.Series[0]["PieLabelStyle"] = "Outside";//将文字移到外侧
                    chart2.Series[0]["PieLineColor"] = "Black";//绘制黑色的连线。
                    chart2.Series[0].LegendText = legend1.Name;
                    chart2.Legends.Add(legend1);
                    chart2.Series[0].XValueType = ChartValueType.String;  //设置X轴上的值类型

                    chart2.Series[0].IsVisibleInLegend = true;
                  
                    break;
                }
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SKU", typeof(string));
            dt.Columns.Add("销量", typeof(string));
          

            foreach (string key in util.reviewcountdic.Keys)
            {
                DataRow dr = dt.NewRow();
                dr[0] = key;
                dr[1] = util.reviewcountdic[key];
                dt.Rows.Add(dr);
            }

           
            Util.DataTableToExcel(dt,"sheet1",true);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("时间", typeof(string));
            dt.Columns.Add("销量", typeof(string));

            Dictionary<DateTime, int> dic1Asc = util.reviewtimedic.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);

            foreach (DateTime key in dic1Asc.Keys)
            {
                DataRow dr = dt.NewRow();
                dr[0] = key.ToString("yyyy-MM-dd");
                dr[1] = util.reviewtimedic[key].ToString();
                dt.Rows.Add(dr);
            }

          
            Util.DataTableToExcel(dt, "sheet1", true);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

            if(dateTimePicker1.Value>dateTimePicker2.Value)
            {
                MessageBox.Show("错误：起始时间大于结束时间");
                return;
            }

            chart1.ChartAreas[0].AxisX.Minimum = dateTimePicker1.Value.ToOADate();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value > dateTimePicker2.Value)
            {
                MessageBox.Show("错误：起始时间大于结束时间");
                return;
            }
            chart1.ChartAreas[0].AxisX.Maximum = dateTimePicker2.Value.ToOADate();

        }

        private void listView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listView2.SelectedItems.Count == 0)
                return;
            if (Util.expiretime == "")
            {

                MessageBox.Show("请购买套餐");
                return;
            }
            else
            {
                try
                {
                    if (Convert.ToDateTime(Util.expiretime) < DateTime.Now)
                    {
                        MessageBox.Show("请购买套餐");
                        return;
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show("请购买套餐");
                    return;
                }
            }
            if (listView2.SelectedItems[0].Name.Contains("关键词排名查询"))
            {
                tabControl1.SelectedIndex = 7;
            }
           else if (listView2.SelectedItems[0].Name.Contains("标题自动生成"))
            {
                tabControl1.SelectedIndex = 8;
            }
            else if (listView2.SelectedItems[0].Name.Contains("竞品SKU销量分析"))
            {
                tabControl1.SelectedIndex = 9;
            }
            else if (listView2.SelectedItems[0].Name.Contains("竞品SKU占比分析"))
            {
                tabControl1.SelectedIndex = 10;
            }
            else if (listView2.SelectedItems[0].Name.Contains("生意参谋指数"))
            {
                tabControl1.SelectedIndex = 12;
               
                
            }
            else
            {
                string url = util.GeticonUrl(Path.GetFileName(listView2.SelectedItems[0].Name));
                System.Diagnostics.Process.Start(url);
                //System.Diagnostics.Process.Start(listView2.SelectedItems[0].Name);
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }


       public static string gs = "";
        public void zszhuanhua()
        {
            if (gs == "")
            {
                gs = Util.GetZs(zs);
            }
            string formula = gs;
            string[] text = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (var item in text)
            {
                if(item!="")
                {
                    try
                    {
                        // string formula = "(2.118*10)*x+4.03x+(2.207*10)";

                        formula = formula.Replace("x", item);
                        var result = new System.Data.DataTable().Compute(formula, "");
                        textBox5.Text += Convert.ToDouble(result).ToString("0.00") + "\r\n";
                    }
                    catch (Exception)
                    {

                        continue;
                    }
                }

            }
            button19.Enabled = true;
            button19.Text = "点击转换";
        }
        private void button19_Click(object sender, EventArgs e)
        {

            button19.Text = "正在转换...";
            button19.Enabled = false;
            textBox5.Text = "";
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(zszhuanhua);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        public string zs = "jyzs";
        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel7.LinkColor=Color.Red;
            linkLabel8.LinkColor = Color.Black;
            linkLabel9.LinkColor = Color.Black;
            linkLabel10.LinkColor = Color.Black;
            linkLabel11.LinkColor = Color.Black;
            label11.Text = "交易指数";
            label16.Text = "交易金额";
            zs = "jyzs";
            gs = Util.GetZs(zs);
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel7.LinkColor = Color.Black;
            linkLabel8.LinkColor = Color.Red;
            linkLabel9.LinkColor = Color.Black;
            linkLabel10.LinkColor = Color.Black;
            linkLabel11.LinkColor = Color.Black;
            label11.Text = "搜索指数";
            label16.Text = "搜索人数";
            zs = "jyzs";
            gs = Util.GetZs(zs);
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel7.LinkColor = Color.Black;
            linkLabel8.LinkColor = Color.Black;
            linkLabel9.LinkColor = Color.Red;
            linkLabel10.LinkColor = Color.Black;
            linkLabel11.LinkColor = Color.Black;
            label11.Text = "供应商指数";
            label16.Text = "商家数";
            zs = "jyzs";
            gs = Util.GetZs(zs);
        }

        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel7.LinkColor = Color.Black;
            linkLabel8.LinkColor = Color.Black;
            linkLabel9.LinkColor = Color.Black;
            linkLabel10.LinkColor = Color.Red;
            linkLabel11.LinkColor = Color.Black;
            label11.Text = "商品指数";
            label16.Text = "在线商品数";
            zs = "jyzs";
            gs = Util.GetZs(zs);
        }

        private void linkLabel11_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel7.LinkColor = Color.Black;
            linkLabel8.LinkColor = Color.Black;
            linkLabel9.LinkColor = Color.Black;
            linkLabel10.LinkColor = Color.Black;
            linkLabel11.LinkColor = Color.Red;
            label11.Text = "支付转化率指数";
            label16.Text = "支付转化率";
            zs = "zfzs";
            gs = Util.GetZs(zs);
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox2.Text = "";
            
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox5.Text = "";
        }


        //http://47.96.189.55/jilusoft/icon/%E5%85%B3%E9%94%AE%E8%AF%8D%E6%8E%92%E5%90%8D%E6%9F%A5%E8%AF%A2.png
    }
}
