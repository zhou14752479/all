using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 主程序202007
{
    public partial class 京东物流全 : Form
    {
        public 京东物流全()
        {
            InitializeComponent();
        }

       

       
        /// <summary>
        /// 接口重量
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public double getjiekouzl(string code)
        {
            string html = method.GetUrl("http://tk.xufu.live/test.php?code="+code,"utf-8");
            Match value = Regex.Match(html, @"\d{1,}");

            return Convert.ToDouble(value.Groups[0].Value);
        }
        /// <summary>
        /// 收件人信息Json
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string getsjrxx(string code)
        {
            string html = method.PostUrl("https://biz-wb.jdwl.com/business/waybillmanage/queryDesensitizationData", "deliveryId="+code,cookie,"utf-8");
          

            return (html);
        }



        public void run()
        {
            // cookie = Form1.cookie;
       



            label3.Text = "正在查询";
            string begintime = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string endtime = dateTimePicker2.Value.ToString("yyyy-MM-dd");

            for (int i = 1; i < 999; i++)
            {
              


                string url = "https://biz-wb.jdwl.com/business/waybillmanage/index?isFromTab=1&deliveryId=&orderId=&beginTime=" + begintime + "+00%3A00%3A00&endTime=" + endtime + "+23%3A59%3A59&receiveName=&receiveMobile=&receiveCompany=&senderName=&senderMobile=&senderCompany=&boxCode=&orderStatusId=&orderState=&cancelKeys=&deliveryI_h=&printSize=100*113&printType=&goodsType=&waybillType=&isRefuse=&orgId=&preallocationValue=&senderProvinceId=&senderCityId=&senderCountyId=&curPage=" + i + "&pageSize=10";
                string html = method.GetUrlWithCookie(url, cookie, "utf-8");


                MatchCollection uids = Regex.Matches(html, @"data-index=""([\s\S]*?)""");
                MatchCollection times = Regex.Matches(html, @"下单时间：</span>([\s\S]*?)</em>");
                MatchCollection xdrs = Regex.Matches(html, @"下单人：</span>([\s\S]*?)</em>");
           
                MatchCollection zhuangtais = Regex.Matches(html, @"<span class=""first color333"">([\s\S]*?)</span>");




                if (uids.Count == 0)
                {
                    MessageBox.Show("抓取结束");
                    label3.Text = "抓取结束";
                    break;
                }

                for (int j = 0; j < uids.Count; j++)
                {
                    try
                    {

      
               
                    label3.Text = "正在查询......." + uids[j].Groups[1].Value;
                    string aurl = "https://biz-wb.jdwl.com/business/waybillmanage/toDeliveryDetail?orderStatusId=&orderState=&cancelKeys=&deliveryI_h=&detailDeliveryId=" + uids[j].Groups[1].Value;
                    string ahtml = method.GetUrlWithCookie(aurl, cookie, "utf-8");


                    

                    Match zhongliang = Regex.Match(ahtml, @"重量：([\s\S]*?)kg");
                    Match jjr = Regex.Match(ahtml, @"寄件人信息</div>([\s\S]*?)<p>([\s\S]*?)</p>");
                    Match jjrtel = Regex.Match(ahtml, @"寄件人信息</div>([\s\S]*?)&nbsp");
                    Match leixing = Regex.Match(ahtml, @"附加业务类型</div>([\s\S]*?)<p>");
                    Match baojia = Regex.Match(ahtml, @"保价：¥([\s\S]*?)</p>");
                    Match shuliang = Regex.Match(ahtml, @"包裹数：([\s\S]*?)<");
                    Match mingcheng = Regex.Match(ahtml, @"id=""spGoods"">([\s\S]*?)</span>");


                        Match jjrtel2 = Regex.Match(jjrtel.Groups[1].Value, @"\d{11}");


                        double jiekouzl = getjiekouzl(uids[j].Groups[1].Value);
                    double caijizl = Convert.ToDouble(zhongliang.Groups[1].Value.Trim());

                    string sjrhtml = getsjrxx(uids[j].Groups[1].Value);
                    Match sjr= Regex.Match(sjrhtml, @"""receiveName"":""([\s\S]*?)""");
                    Match sjrtel = Regex.Match(sjrhtml, @"""receiveMobile"":""([\s\S]*?)""");


                    double jisuanzl = jiekouzl > caijizl ? jiekouzl : caijizl;
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                     lv1.SubItems.Add(uids[j].Groups[1].Value);
                    lv1.SubItems.Add(times[j].Groups[1].Value.Replace("<em>","").Trim());
                    lv1.SubItems.Add(xdrs[j].Groups[1].Value.Replace("<em>", "").Trim());
                    lv1.SubItems.Add(sjr.Groups[1].Value);
                    lv1.SubItems.Add(sjrtel.Groups[1].Value);
                    lv1.SubItems.Add(jjr.Groups[2].Value.Trim());
                    lv1.SubItems.Add(jjrtel2.Groups[0].Value.Trim());
                   
                    lv1.SubItems.Add(caijizl.ToString()); //采集
                    lv1.SubItems.Add(jiekouzl.ToString());  //接口
                    lv1.SubItems.Add(Math.Ceiling(jisuanzl).ToString());  //计算
                    lv1.SubItems.Add(leixing.Groups[1].Value.Replace("<div class=\"info\">", "").Trim());
                    lv1.SubItems.Add(baojia.Groups[1].Value.Trim());
                    lv1.SubItems.Add(shuliang.Groups[1].Value);
                    lv1.SubItems.Add(mingcheng.Groups[1].Value);
                    lv1.SubItems.Add(zhuangtais[j].Groups[1].Value.Trim());
                   
                        Thread.Sleep(500);
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    }
                    catch (Exception)
                    {

                        continue;
                    }
                }


            }




        }

        string cookie = "";
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"jdwl"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion



            //  cookie = helper.Form1.cookie;
            cookie = "3AB9D23F7A4B3C9B=V7O7MI6JN2WBIOKWOYR23BUXOMNM753VTKYJNJXH6D3S62EJTR5YAVWAWQBY7RSORJ5JK2365MRWQVIHWCQZNKCUKM; __stu=GGCHRWa6OMDn5uBU; 3AB9D23F7A4B3C9B=V7O7MI6JN2WBIOKWOYR23BUXOMNM753VTKYJNJXH6D3S62EJTR5YAVWAWQBY7RSORJ5JK2365MRWQVIHWCQZNKCUKM; __jdv=197855408|wl.jdwl.com|-|referral|-|1597890166477; thor=4175FA636F1E6034913562C0B875DAE96C712D7515069A1954694ADF2A06ADC7D716607498A29B4A6A03E2C3A64DF60CD3BC6448AA32CB873D2F01989BFCDD7C78E73EDF9B4D05CDE3D23674A75C9130B74390E8E512EF710E7718609A1537E0C7AB9A2B20C4260863B6DC49B09BC3B48B6CC360D906A93080ED86942E5572DC7D6309EDF1EFC107D60D31043204FB85632C7E2B5F50C038A97250CD5B60249F; pin=jd_tZJySCmfXCcN; unick=%E6%BB%A8%E5%B7%9E%E7%B2%97%E5%B8%83%E6%89%B9%E5%8F%91; __sts=GGCHRWa6OMDn5uBU|Wa7SBCX2aCE; __jda=234118157.15955965935011543226538.1595596593.1598516736.1598584353.14; __jdc=234118157; __jdb=234118157.8.15955965935011543226538|14.1598584353; JSESSIONID=4CC8DF9B5DB0A0E1B21F25C0A4BD88A0.s1";
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void 京东物流全_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {

                    textBox1.Text += text[i]+ "\r\n";


                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
          
            listView1.Items.Clear();
        }
        bool zanting = true;

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form1 fm1 = new Form1();
            fm1.Show();
        }

        private void 京东物流全_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
                //System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }

        }

        public void chaxun()
        {
            if (textBox1.Text != "")
            {
                string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (var item in text)
                {
                    if (item != "")
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(item);
                        lv1.SubItems.Add("");
                        lv1.SubItems.Add("");
                        lv1.SubItems.Add("");
                        lv1.SubItems.Add("");
                        lv1.SubItems.Add("");
                        lv1.SubItems.Add("");
                        lv1.SubItems.Add("");
                        lv1.SubItems.Add("");
                        lv1.SubItems.Add(Math.Ceiling(getjiekouzl(item.Trim())).ToString());
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                    }
                    Thread.Sleep(1000);
                }

            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"jdwl"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion



     
            Thread thread = new Thread(new ThreadStart(chaxun));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        StringBuilder sb = new StringBuilder();
        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            for (int i = 0; i < listView1.SelectedItems.Count; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    sb.Append(listView1.SelectedItems[i].SubItems[j].Text+"  ");
                    
                }

            }
            Clipboard.SetDataObject(sb.ToString());
            sb.Clear();

        }

    }
}
