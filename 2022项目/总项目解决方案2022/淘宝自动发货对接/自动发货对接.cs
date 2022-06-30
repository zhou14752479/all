using Microsoft.VisualBasic;
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

namespace 淘宝自动发货对接
{
    public partial class 自动发货对接 : Form
    {
        public 自动发货对接()
        {
            InitializeComponent();
        }


        public bool autofahuo = false;
        public string tb_seller_nick="pm2咖啡";

        public string starttime = "";
        public string lasttime = "";

        /// <summary>
        /// 获取淘宝订单列表
        /// 接口文档：http://help.fw199.com/docs/h7b/tradelist
        /// </summary>
        public  void GetTradeList()
        {

            if (starttime == "" &&lasttime=="")
            {
                starttime = DateTime.Now.AddDays(-90).ToString("yyyy-MM-dd HH:mm:ss");
                lasttime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                starttime = lasttime;
                lasttime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }



            listView1.Items.Clear();

            if(listView2.CheckedItems.Count ==0)
            {
                MessageBox.Show("请选择店铺");
                return;
            }

            for (int a = 0; a <listView2.CheckedItems.Count; a++)
            {
                tb_seller_nick=listView2.CheckedItems[a].SubItems[1].Text.ToString();
               
                for (int page = 1; page < 9999; page++)
                {
                    if(textBox1.Text.Length>50)
                    {
                        textBox1.Text = "";
                    }
                    textBox1.Text += DateTime.Now.ToString("HH:mm:ss") + "：正在读取第：" +page+"页"+"\r\n";
                    string url = "https://open.fw199.com/gateway/taobao/order/list";
                    long ts = H7BUtil.GetTimeStamp();
                    var requestArgs = new Dictionary<string, string>()
            {

                  {"tb_seller_nick",tb_seller_nick},
                  {"start_created",starttime},
                  {"end_created",lasttime},
                  {"status","WAIT_SELLER_SEND_GOODS"},
                  {"page_no",page.ToString()},
                  {"page_size","30"},
                  {"timestamp", ts.ToString()}

            };
                    string response = H7BUtil.HttpPost(url, requestArgs);
                    MatchCollection oid = Regex.Matches(response, @"""oid"":""([\s\S]*?)""");

                    if (oid.Count == 0)
                    {
                        
                        break;
                    }
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < oid.Count; i++)
                    {
                        sb.Append(oid[i].Groups[1].Value + ",");

                    }

                    getTradeDetail(sb.ToString().Remove(sb.ToString().Length - 1, 1));
                }
            }

            textBox1.Text = DateTime.Now.ToString("HH:mm:ss") + "：本次循环完成";
           
        }


        Dictionary<string ,string> oidwangwang_dic=new Dictionary<string ,string>();

        public void getTradeDetail(string tids)
        {
           
            string url = "https://open.fw199.com/gateway/taobao/order/detail/batch";
            long ts = H7BUtil.GetTimeStamp();
            var requestArgs = new Dictionary<string, string>()
            {

                  {"tb_seller_nick",tb_seller_nick},
                  {"timestamp", ts.ToString()},
                   {"tids", tids}


            };
            string response = H7BUtil.HttpPost(url, requestArgs);
           // textBox1.Text = response;
            MatchCollection oid = Regex.Matches(response, @"""oid"":""([\s\S]*?)""");
            MatchCollection title = Regex.Matches(response, @"tid_str([\s\S]*?)title"":""([\s\S]*?)""");
            MatchCollection created = Regex.Matches(response, @"""created"":""([\s\S]*?)""");
            MatchCollection buyer_nick = Regex.Matches(response, @"""buyer_nick"":""([\s\S]*?)""");
            MatchCollection buyer_message = Regex.Matches(response, @"""buyer_message"":""([\s\S]*?)""");
            MatchCollection status = Regex.Matches(response, @"""status"":""([\s\S]*?)""");
            MatchCollection price = Regex.Matches(response, @"""price"":""([\s\S]*?)""");
            MatchCollection seller_flag = Regex.Matches(response, @"""seller_flag"":([\s\S]*?),");
            MatchCollection seller_memo = Regex.Matches(response, @"""seller_memo"":""([\s\S]*?)""");

            for (int i = 0; i < oid.Count; i++)
            {
                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                lv1.SubItems.Add(oid[i].Groups[1].Value);
                lv1.SubItems.Add(title[i].Groups[2].Value);
                lv1.SubItems.Add(status[i].Groups[1].Value);
                lv1.SubItems.Add(created[i].Groups[1].Value);
                lv1.SubItems.Add(price[i].Groups[1].Value);
                lv1.SubItems.Add(buyer_message[i].Groups[1].Value);
                lv1.SubItems.Add(seller_flag[i].Groups[1].Value+seller_memo[i].Groups[1].Value);
                lv1.SubItems.Add(buyer_nick[i].Groups[1].Value);



                string tid = oid[i].Groups[1].Value; //订单号
                string sid = buyer_message[i].Groups[1].Value;  //备注运单号
                
                
                if(sid=="")
                {
                    if (checkBox3.Checked == true)
                    {
                        if (kuaidi_regex.hasSendList.Contains(tid))
                        {
                            //已发消息
                            continue;

                        }
                        else
                        {
                            //未发消息添加到已发
                            kuaidi_regex.inserthasSend(tid);
                            kuaidi_regex.hasSendList.Add(tid);
                        }

                        sendmsg(buyer_nick[i].Groups[1].Value, 软件设置.msg2);
                    }
                }

                //开启自动发货
                if (autofahuo==true)
                {
                    if (kuaidi_regex.hasSendList.Contains(tid))
                     {
                        //已发消息
                        continue;
                    
                    }
                    else
                    {
                        //未发消息添加到已发
                        kuaidi_regex.inserthasSend(tid);
                        kuaidi_regex.hasSendList.Add(tid);
                    }


                    if (sid != "" && sid.Length>6)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("[");
                       
                        
                        string code = kuaidi_regex.getkdcode(sid);
                        string item = "{\"sender_id\":0,\"feature\":\"\",\"tid\":\"" + tid + "\",\"sub_tid\":\"\",\"consign_pkgs\": [{\"out_sid\":\"" + sid + "\",\"company_code\":\"" + code + "\"}],\"cancel_id\":0},";
                        sb.Append(item.ToString().Remove(item.ToString().Length - 1, 1));
                        sb.Append("]");
                        fahuo(sb.ToString(), buyer_nick[i].Groups[1].Value);
                    }

                }
            }



        }


        public void fahuo(string item,string buyer_nick)
        {
            string url = "https://open.fw199.com/gateway/taobao/logistices/v2/offlinebatchsend";
            long ts = H7BUtil.GetTimeStamp();
            var requestArgs = new Dictionary<string, string>()
            {

                  {"tb_seller_nick",tb_seller_nick},
                  {"timestamp", ts.ToString()},
                   {"items", item}
            };
            string response = H7BUtil.HttpPost(url, requestArgs);
           textBox1.Text = response+"\r\n";

            MatchCollection is_success = Regex.Matches(response, @"""is_success"":([\s\S]*?),");
            for (int i = 0; i < is_success.Count; i++)
            {
                if (is_success[i].Groups[1].Value== "true")
                {
                    if (checkBox1.Checked == true)
                    {
                        sendmsg(buyer_nick, 软件设置.msg1);
                    }
                }
                else if (is_success[i].Groups[1].Value == "false")
                {
                    if (checkBox2.Checked == true)
                    {
                        sendmsg(buyer_nick, 软件设置.msg3);
                    }
                }
            }
        }

        public void sendmsg(string buyer_nick, string msg_content)
        {
          


            string url = "https://open.fw199.com/gateway/taobao/qianniu/sendmsg";
            long ts = H7BUtil.GetTimeStamp();
            var requestArgs = new Dictionary<string, string>()
            {

                  {"tb_seller_nick",tb_seller_nick},
                  {"timestamp", ts.ToString()},
                   {"buyer_nick", buyer_nick},
                {"msg_content", msg_content}
            };
            string response = H7BUtil.HttpPost(url, requestArgs);
            textBox1.Text += response;

        }


        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(GetTradeList);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            autofahuo = false;
            timer1.Start();
            timer1.Interval = 软件设置.time * 1000;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(GetTradeList);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            autofahuo = true;
            timer1.Start();
            timer1.Interval =软件设置.time*1000;

           // getTradeDetail("2717558605147841522,2718363315584841522");
        }

        private void 软件设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            软件设置 set=new 软件设置();
            set.Show();
        }

        private void 自动发货对接_Load(object sender, EventArgs e)
        {
            软件设置.readconfig();
           kuaidi_regex.inittxt();
            kuaidi_regex.gethasSend();


                StreamReader sr = new StreamReader(kuaidi_regex.path + "\\shop.txt", kuaidi_regex.EncodingType.GetTxtType(kuaidi_regex.path + "\\shop.txt"));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {
                if (text[i] != "")
                {
                    ListViewItem lv2 = listView2.Items.Add(listView2.Items.Count.ToString()); //使用Listview展示数据
                    lv2.SubItems.Add(text[i]);
                }
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(GetTradeList);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }


       

        private void button3_Click(object sender, EventArgs e)
        {

            if (listView1.SelectedItems.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("[");

                string tid = listView1.SelectedItems[0].SubItems[1].Text;
                string buyer_nick = listView1.SelectedItems[0].SubItems[8].Text;


                string sid = textBox2.Text;
                string code = kuaidi_regex.getkdcode(sid);
                string item = "{\"sender_id\":0,\"feature\":\"\",\"tid\":\"" + tid + "\",\"sub_tid\":\"\",\"consign_pkgs\": [{\"out_sid\":\"" + sid + "\",\"company_code\":\"" + code + "\"}],\"cancel_id\":0},";
                sb.Append(item.ToString().Remove(item.ToString().Length - 1, 1));
                sb.Append("]");
                fahuo(sb.ToString(), buyer_nick);

                
            }
            else
            {
                MessageBox.Show("请选择一个订单号");
            }





            //textBox2.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
            timer1.Stop();
            autofahuo = false;
        }

        private void 店铺授权ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "https://tb.fw199.com/partner/grant?appid=sv8aqEXPhUJJAIR8&callback=Your_CallBackUrl";
            try
            {
                System.Diagnostics.Process.Start( url);
            }
            catch (Exception)
            {

                System.Diagnostics.Process.Start("explorer.exe", url);
            }
        }

        private void 删除店铺ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选中一个店铺");
            }

            for (int i = 0; i < listView2.SelectedItems.Count; i++)
            {
                listView2.SelectedItems[i].Remove();
            }
           
        }

        private void 添加店铺ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = Interaction.InputBox("淘宝卖家店铺登录账号，非店铺名称，添加后请授权", "添加店铺", "", -1, -1);
           if(str!="")
            {
                ListViewItem lv2 = listView2.Items.Add(listView2.Items.Count.ToString()); //使用Listview展示数据
                lv2.SubItems.Add(str);

                FileStream fs1 = new FileStream(kuaidi_regex.path+ "\\shop.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                sw.WriteLine(str);
                sw.Close();
                fs1.Close();
                sw.Dispose();


            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;
            string sid = listView1.SelectedItems[0].SubItems[6].Text;
            textBox2.Text = sid;
            if (sid == "")
            {
                MessageBox.Show("未备注运单号,请自行填入");
            }
        }

        private void 自动发货对接_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;
            string sid = listView1.SelectedItems[0].SubItems[6].Text;
            textBox2.Text = sid;
            if (sid=="")
            {
                MessageBox.Show("未备注运单号,请自行填入");
            }
        }
    }
}
