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

        string cookie = "3AB9D23F7A4B3C9B=V7O7MI6JN2WBIOKWOYR23BUXOMNM753VTKYJNJXH6D3S62EJTR5YAVWAWQBY7RSORJ5JK2365MRWQVIHWCQZNKCUKM; __stu=GGCHRWa6OMDn5uBU; 3AB9D23F7A4B3C9B=V7O7MI6JN2WBIOKWOYR23BUXOMNM753VTKYJNJXH6D3S62EJTR5YAVWAWQBY7RSORJ5JK2365MRWQVIHWCQZNKCUKM; __jdv=197855408|wl.jdwl.com|-|referral|-|1597890166477; thor=CB0E91001C227862421C0FEFB0C2A6FE015DC7B083BE07BC2BADD487A1C4D68DEFAF11B294EF7B94C7740734F2674ACAFF5678553968859D77CB706A78ACCE62B52DC32B719441A097EE8EF4548BCA5D1BB178D839450C445D5CF012027F3975B1281997014A25E4C1251B667ABE76E7C9BDE65FF73319BA228749F03476E42F25CB01D8589A1E4431643D145AFDDC55177C7FD4693E6AB6625DC62498E4C954; pin=jd_7582fdf3af9ec; unick=%E6%A1%93%E5%8F%B0%E5%95%A4%E9%85%92%E4%BB%A3%E7%90%86; __sts=GGCHRWa6OMDn5uBU|Wa7O9gwCanx; __jda=234118157.15955965935011543226538.1595596593.1597890166.1598233379.6; __jdc=234118157; __jdb=234118157.11.15955965935011543226538|6.1598233379; JSESSIONID=A8BCE67F1DECD92C755D338996055D15.s1";

       

        public double getjiekouzl(string code)
        {
            string html = method.GetUrl("http://tk.xufu.live/test.php?code="+code,"utf-8");
            Match value = Regex.Match(html, @"\d{1,}");

            return Convert.ToDouble(value.Groups[0].Value);
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
                MatchCollection sjrs = Regex.Matches(html, @"收件人：</span>([\s\S]*?)</em>");
                MatchCollection sjrTels = Regex.Matches(html, @"收件人联系方式：</span>([\s\S]*?)</em>");
                MatchCollection zhuangtais = Regex.Matches(html, @"<span class=""first color333"">([\s\S]*?)</span>");




                if (uids.Count == 0)
                {
                    MessageBox.Show("抓取结束");
                    label3.Text = "抓取结束";
                    break;
                }

                for (int j = 0; j < uids.Count; j++)
                {
                    label3.Text = "正在查询......." + uids[j].Groups[1].Value;
                    string aurl = "https://biz-wb.jdwl.com/business/waybillmanage/toDeliveryDetail?orderStatusId=&orderState=&cancelKeys=&deliveryI_h=&detailDeliveryId=" + uids[j].Groups[1].Value;
                    string ahtml = method.GetUrlWithCookie(aurl, cookie, "utf-8");


                    

                    Match zhongliang = Regex.Match(ahtml, @"重量：([\s\S]*?)kg");
                    Match jjr = Regex.Match(ahtml, @"寄件人信息</div>([\s\S]*?)<p>([\s\S]*?)<b>");
                    Match jjrtel = Regex.Match(ahtml, @"寄件人信息</div>([\s\S]*?)1([\s\S]*?)&nbsp");
                    Match leixing = Regex.Match(ahtml, @"附加业务类型</div>([\s\S]*?)<p>");
                    Match baojia = Regex.Match(ahtml, @"保价：¥([\s\S]*?)</p>");
                    Match shuliang = Regex.Match(ahtml, @"包裹数：([\s\S]*?)<");
                    Match mingcheng = Regex.Match(ahtml, @"id=""spGoods"">([\s\S]*?)</span>");



                    double jiekouzl = getjiekouzl(uids[j].Groups[1].Value);
                    double caijizl = Convert.ToDouble(zhongliang.Groups[1].Value.Trim());


                   double jisuanzl = jiekouzl > caijizl ? jiekouzl : caijizl;
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                     lv1.SubItems.Add(uids[j].Groups[1].Value);
                    lv1.SubItems.Add(times[j].Groups[1].Value.Replace("<em>","").Trim());
                    lv1.SubItems.Add(xdrs[j].Groups[1].Value.Replace("<em>", "").Trim());
                    lv1.SubItems.Add(sjrs[j].Groups[1].Value.Replace("<em>", "").Trim());
                    lv1.SubItems.Add(sjrTels[j].Groups[1].Value.Replace("<em>", "").Trim());
                    lv1.SubItems.Add(jjr.Groups[2].Value.Trim());
                    lv1.SubItems.Add("1"+jjrtel.Groups[2].Value.Trim());
                   
                    lv1.SubItems.Add(caijizl.ToString()); //采集
                    lv1.SubItems.Add(jiekouzl.ToString());  //接口
                    lv1.SubItems.Add(Math.Ceiling(jisuanzl).ToString());  //计算
                    lv1.SubItems.Add(leixing.Groups[1].Value.Replace("<div class=\"info\">", "").Trim());
                    lv1.SubItems.Add(zhongliang.Groups[1].Value.Trim());
                    lv1.SubItems.Add(shuliang.Groups[1].Value);
                    lv1.SubItems.Add(mingcheng.Groups[1].Value);
                    lv1.SubItems.Add(zhuangtais[j].Groups[1].Value.Trim());

                    Thread.Sleep(500);
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                }


            }




        }
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

            

           // cookie = helper.Form1.cookie;
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
