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
using myDLL;

namespace 淘宝商家电话
{
    public partial class 淘宝商家电话 : Form
    {
        public 淘宝商家电话()
        {
            InitializeComponent();
        }



        public string tb_gettel(string orderid,string cookie,string type)
        {
            try
            {
                string phone = "";
                if (type == "WAIT_BUYER_CONFIRM_GOODS" || type == "TRADE_FINISHED" || type == "WAIT_SELLER_SEND_GOODS")
                {
                    //未退款
                    string url = "https://refund2.taobao.com/dispute/apply.htm?spm=a21we.8289817.0.0.3eff5429drtTtF&bizOrderId=" + orderid + "&type=1";
                    string html = method.GetUrlWithCookie(url, cookie, "utf-8");
                    phone = Regex.Match(html, @"联系电话([\s\S]*?)""text"":""([\s\S]*?)""").Groups[2].Value;
                }
               if(type== "TRADE_CLOSED")
                {
                    string url = "https://refund2.tmall.com/dispute/detail.htm?spm=a1z09.2.0.0.67002e8dbNRs2D&bizOrderId=" + orderid + "&disputeType=1";
                    string html = method.GetUrlWithCookie(url, cookie, "utf-8");
                     phone = Regex.Match(html, @"联系电话([\s\S]*?)""text"":""([\s\S]*?)""").Groups[2].Value;
                }



                return phone;
            }
            catch (Exception ex)
            {

               return "获取错误";
            }

        }
        public string tmall_gettel(string orderid, string cookie, string type)
        {
            try
            {
                string url = "https://trade.tmall.com/detail/orderDetail.htm?spm=a1z09.2.0.0.75532e8d0l3lQG&bizOrderId="+orderid;
                string html = method.GetUrlWithCookie(url, cookie, "gbk");
              string  phone = Regex.Match(html, @"""城市""([\s\S]*?)text"":""([\s\S]*?)""").Groups[2].Value;



                return phone;
            }
            catch (Exception ex)
            {

                return "获取错误";
            }

        }

        public void jiexi()
        {

            try
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    listView1.Items[i].SubItems[7].Text = "正在解析...";

                    string oederid = listView1.Items[i].SubItems[2].Text;
                    string zhanghao = listView1.Items[i].SubItems[4].Text;
                    string shopname = listView1.Items[i].SubItems[5].Text;
                    string type = listView1.Items[i].SubItems[3].Text;

                    string tel = "";
                    if (shopname.Contains("旗舰店") || shopname.Contains("专营店") || shopname.Contains("专卖店"))
                    {
                        tel = tmall_gettel(oederid, dics[zhanghao], type);
                    }
                      else
                    {
                        tel = tb_gettel(oederid, dics[zhanghao],type);
                    }
                   
                 

                    listView1.Items[i].SubItems[6].Text = tel;
                    listView1.Items[i].SubItems[7].Text = "解析完成";
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (listView1.Items.Count > 2)
                    {
                        listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                    }
                    if (status == false)
                        return;
                    Thread.Sleep(1000);
                }

            }
            catch (Exception ex)
            {

                ;
            }
        }


        //接收信号

        public void setcookie2(string zhanghao ,string cookie2)
        {
            ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count + 1).ToString()); //使用Listview展示数据    
            lv2.SubItems.Add(zhanghao);
            lv2.SubItems.Add(cookie2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            登录 login = new 登录();
            login.Show();

            //声明接收
            login.setcookie = new 登录.SetCookie(setcookie2);
        }
        Thread thread;
        bool zanting = true;
        bool status = true;
        public void run()
        {
            try
            {
                for (int a = 0; a < listView2.CheckedItems.Count; a++)
                {
                    string zhanghao = listView2.CheckedItems[a].SubItems[1].Text;
                    string cookie = listView2.CheckedItems[a].SubItems[2].Text;
                    for (int page = 1; page < 100; page++)
                    {
                        string url = "https://buyertrade.taobao.com/trade/itemlist/asyncBought.htm?action=itemlist/BoughtQueryAction&event_submit_do_query=1&_input_charset=utf8";
                        string postdata = "buyerNick=&canGetHistoryCount=false&dateBegin=0&dateEnd=0&historyCount=0&lastStartRow=&logisticsService=&needQueryHistory=false&onlineCount=0&options=0&orderStatus=&pageNum="+page+"&pageSize=100&queryBizType=&queryForV2=false&queryOrder=desc&rateStatus=&refund=&sellerNick=&prePageNo=2";
                        string html = method.PostUrl(url, postdata, cookie, "gb2312", "application/x-www-form-urlencoded", url);
                        MatchCollection ahtmls = Regex.Matches(html, @"""batchGroup""([\s\S]*?)""subOrders""");

                        if (ahtmls.Count == 0)
                            break;
                        for (int i = 0; i < ahtmls.Count; i++)
                        {
                            string createDay = Regex.Match(ahtmls[i].Groups[1].Value, @"""createDay"":""([\s\S]*?)""").Groups[1].Value;
                            string id = Regex.Match(ahtmls[i].Groups[1].Value, @"""id"":""([\s\S]*?)""").Groups[1].Value;
                            string tradeStatus = Regex.Match(ahtmls[i].Groups[1].Value, @"""tradeStatus"":""([\s\S]*?)""").Groups[1].Value;
                            string shopName = Regex.Match(ahtmls[i].Groups[1].Value, @"""shopName"":""([\s\S]*?)""").Groups[1].Value;

                          
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据    
                            lv1.SubItems.Add(createDay);
                            lv1.SubItems.Add(id);
                            lv1.SubItems.Add(tradeStatus);
                            lv1.SubItems.Add(zhanghao);
                            lv1.SubItems.Add(shopName);
                            lv1.SubItems.Add("");
                            lv1.SubItems.Add("");

                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (listView1.Items.Count > 2)
                            {
                                listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                            }
                            if (status == false)
                                return;
                            Thread.Sleep(50);

                        }
                    }
                }
              
            }
            catch (Exception ex)
            {

                textBox1.Text = ex.ToString();
            }

        }



        
        Dictionary<string, string> dics = new Dictionary<string, string>();
        private void 淘宝商家电话_Load(object sender, EventArgs e)
        {
            #region 通用检测


            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"wXzp4"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\cookie.txt"))
            {

                StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\cookie.txt", method.EncodingType.GetTxtType(AppDomain.CurrentDomain.BaseDirectory + "\\cookie.txt"));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {
                    string[] values = text[i].Split(new string[] { "#" }, StringSplitOptions.None);
                    if (values.Length > 1)
                    {
                        
                         ListViewItem lv2 = listView2.Items.Add(listView2.Items.Count.ToString()); //使用Listview展示数据
                        lv2.SubItems.Add(values[0]);
                        lv2.SubItems.Add(values[1]);
                        lv2.Checked = true;
                    }
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            }
        }

      

        private void button3_Click(object sender, EventArgs e)
        {
            status = true;
            for (int i = 0; i < listView2.Items.Count; i++)
            {
                if (!dics.ContainsKey(listView2.Items[i].SubItems[1].Text))
                {
                    dics.Add(listView2.Items[i].SubItems[1].Text, listView2.Items[i].SubItems[2].Text);
                }
            }

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
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

        private void 淘宝商家电话_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                method.DataTableToExcelTime(method.listViewToDataTable(this.listView1),  true);


                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < listView2.Items.Count; i++)
                {
                    sb.Append(listView2.Items[i].SubItems[1].Text + "#" + listView2.Items[i].SubItems[2].Text);
                }


                System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\cookie.txt", sb.ToString(), Encoding.UTF8);
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(jiexi);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            status = false;
        }
    }
}
