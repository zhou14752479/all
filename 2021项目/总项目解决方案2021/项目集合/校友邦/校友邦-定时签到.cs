using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 校友邦
{
    public partial class 校友邦_定时签到 : Form
    {
        public 校友邦_定时签到()
        {
            InitializeComponent();
        }
        private static int DateDiff(DateTime dateStart, DateTime dateEnd)
        {
            DateTime start = Convert.ToDateTime(dateStart.ToShortDateString());
            DateTime end = Convert.ToDateTime(dateEnd.ToShortDateString());
           
            TimeSpan sp = end-start;

            return sp.Days;
        }
        public void getdata()
        {
            listView1.Items.Clear();
            StreamReader sr = new StreamReader(path + "data.txt", method.EncodingType.GetTxtType(path + "data.txt"));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
              
                string[] value = text[i].Split(new string[] { "#" }, StringSplitOptions.None);
                ListViewItem lv1 = listView1.Items.Add(value[0].Trim()); //使用Listview展示数据    
              
                try
                {

                    listView1.Items[i].BackColor = Color.White;
                    if (value.Length > 2)
                    {

                                                                       
                        lv1.SubItems.Add(value[1].Trim());
                        lv1.SubItems.Add(value[2].Trim());
                        lv1.SubItems.Add(value[3].Trim());
                        lv1.SubItems.Add(value[4].Trim());
                        lv1.SubItems.Add(value[5].Trim());
                        lv1.SubItems.Add(value[6].Trim());


                        lv1.SubItems.Add("");
                        lv1.SubItems.Add("");
                        int shengyu_day = DateDiff(DateTime.Now, Convert.ToDateTime(value[6].Trim()));
                        lv1.SubItems.Add(shengyu_day.ToString());

                        if(value.Length>7)
                        {
                            lv1.SubItems.Add(value[7].Trim());
                        }
                        else
                        {
                            lv1.SubItems.Add("");
                        }
                    }
                }
                catch (Exception ex)
                {
                    lv1.BackColor = Color.Red;
                    textBox2.Text = DateTime.Now.ToString()+ ex.ToString();
                    continue;
                }
            }
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存
        }


        string path = AppDomain.CurrentDomain.BaseDirectory;
        function fc = new function();
        private void 校友邦_定时签到_Load(object sender, EventArgs e)
        {
            #region 通用检测


            string html = method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"QXTAe"))
            {

                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            #endregion
          
          
            getdata();





        }


        int refresh = 0;
        public void run()
        {
            if(DateTime.Now.Hour==2 && refresh==0)
            {
                refresh =1;
                getdata();
            }
            if (DateTime.Now.Hour == 3)
            {
                refresh = 0;
              
            }


            label1.Text = DateTime.Now.ToString()+"：开始启动签到.....";
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                try
                {
                    string username = listView1.Items[i].SubItems[1].Text;
                    string password = listView1.Items[i].SubItems[2].Text;
                    string address = listView1.Items[i].SubItems[3].Text;
                    string timecisu = listView1.Items[i].SubItems[4].Text;
                  
                    string startdate = listView1.Items[i].SubItems[5].Text;
                    string stopdate = listView1.Items[i].SubItems[6].Text;

                    string start= listView1.Items[i].SubItems[7].Text;
                    string end= listView1.Items[i].SubItems[8].Text;


                    string zhouji = listView1.Items[i].SubItems[10].Text.Trim();


                    //判断是否到期
                    if ( DateTime.Now >Convert.ToDateTime(stopdate).AddDays(1))
                    {
                        listView1.Items[i].SubItems[7].Text = "日期超出，不签到";
                        continue;
                    }

                    if (DateTime.Now < Convert.ToDateTime(startdate))
                    {
                        listView1.Items[i].SubItems[7].Text = "日期未到，不签到";
                        continue;
                    }

                   
                   
                
                    string[] text = timecisu.Split(new string[] { "," }, StringSplitOptions.None);


                    //判断指定日期
                    if(zhouji!="")
                    {
                      
                        if(!zhouji.Contains(Convert.ToInt32(DateTime.Now.DayOfWeek).ToString()))
                        {
                            listView1.Items[i].SubItems[7].Text = "指定星期不符合";
                            continue;
                        }
                       

                    }

                    //判断周末
                    if (text[4] == "true") //需要周末签到不需要处理
                    {
                        if(text[5]=="dan")
                        {
                            if (Convert.ToInt32(DateTime.Now.DayOfWeek) == 2 || Convert.ToInt32(DateTime.Now.DayOfWeek) == 4 || Convert.ToInt32(DateTime.Now.DayOfWeek) == 6 || Convert.ToInt32(DateTime.Now.DayOfWeek) == 0)
                            {
                                listView1.Items[i].SubItems[7].Text = "非周一三五不签到";
                                continue;
                            }
                        }

                        if (text[5] == "shuang")
                        {
                            if (Convert.ToInt32(DateTime.Now.DayOfWeek) == 1 || Convert.ToInt32(DateTime.Now.DayOfWeek) ==3 || Convert.ToInt32(DateTime.Now.DayOfWeek) == 5)
                            {
                                listView1.Items[i].SubItems[7].Text = "非周二四六不签到";
                                continue;
                            }
                        }

                    }
                    else
                    {
                        //不需要周末签到
                        if (Convert.ToInt32(DateTime.Now.DayOfWeek)==6 || Convert.ToInt32(DateTime.Now.DayOfWeek)==0)
                        {
                            listView1.Items[i].SubItems[7].Text = "非工作日不签到";
                            continue;
                        }
                    }



                    //判断时间
                    if (DateTime.Now.Hour>15)//结束签到
                    {
                        fc.status = "1";
                      
                        //结束签到时间为空，不需要结束签到
                        if(text[2]=="无" ||text[3]=="无")
                        {
                            listView1.Items[i].SubItems[8].Text = "不需要结束签到";
                            continue;
                        }
                        if(DateTime.Now<Convert.ToDateTime(text[2]) || DateTime.Now > Convert.ToDateTime(text[3]))
                        {
                            if (!end.Contains("success"))
                            {
                                listView1.Items[i].SubItems[8].Text = "结束签到时间未满足";
                                continue;
                            }
                        }

                    }
                    else //开始签到
                    {
                        listView1.Items[i].SubItems[8].Text = "结束签到时间未满足";
                        fc.status = "2";
                        if (DateTime.Now < Convert.ToDateTime(text[0]) || DateTime.Now > Convert.ToDateTime(text[1]))
                        {
                            if (!start.Contains("success"))
                            {
                                listView1.Items[i].SubItems[7].Text = "开始签到时间未满足";
                            }
                            continue;
                        }
                       
                    }





                   
                    string cookie = fc.login(username, password);
                    if (cookie == "")
                    {
                        listView1.Items[i].SubItems[7].Text = "登陆失败";
                        continue;
                    }
                    else
                    {
                        string planid = fc.getplanid(cookie);
                        string traineeid = fc.gettraineeId(planid, cookie);

                       
                        if(fc.status == "2" && !start.Contains("success"))
                        {
                            string shangchuanmamsg = fc.shangchuanma(cookie);
                            string msg = fc.qiandao(cookie, address, traineeid);
                            listView1.Items[i].SubItems[7].Text = shangchuanmamsg+"  "+msg;
                        }
                        if (fc.status == "1" && !end.Contains("success"))
                        {
                            string shangchuanmamsg = fc.shangchuanma(cookie);
                            string msg = fc.qiandao(cookie, address, traineeid);
                            listView1.Items[i].SubItems[8].Text = shangchuanmamsg + "  " + msg;
                        }

                    }

                   
                   

                }
                catch (Exception ex)
                {
                    listView1.Items[i].BackColor = Color.Red;
                    textBox2.Text = DateTime.Now.ToString() + ex.ToString();
                    continue;
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            
            timer1.Interval = Convert.ToInt32(textBox1.Text)*60*1000;
            timer1.Start();
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }


        Thread thread;
      
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            List<string> lines = new List<string>(File.ReadAllLines(path + "data.txt"));//先读取到内存变量

            for (int i = 0; i < lines.Count; i++)
            {
                try
                {
                    string[] value = lines[i].Split(new string[] { "#" }, StringSplitOptions.None);
                    string date = value[value.Length - 1];
                    if(Convert.ToDateTime(date)<DateTime.Now.AddDays(-1))
                    {
                        lines.RemoveAt(i);//指定删除的行
                    }
                    
                }
                catch (Exception)
                {

                    continue;
                }
            }
            File.WriteAllLines(path + "data.txt", lines.ToArray());//在写回硬盤
            getdata();

        }

        private void 校友邦_定时签到_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}
