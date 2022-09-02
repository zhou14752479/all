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

        public void getpics()
        {
          
            DirectoryInfo folder = new DirectoryInfo(path+"//images");
            for (int i = 0; i < folder.GetFiles().Count(); i++)
            {
                string picname = Path.GetFileNameWithoutExtension(folder.GetFiles()[i].Name);
              
                pics.Add(picname);
            }
            
        }


        string path = AppDomain.CurrentDomain.BaseDirectory;
        function fc = new function();
   
        List<string> pics = new List<string>();


       

        private void 校友邦_定时签到_Load(object sender, EventArgs e)
        {
            #region 通用检测


            string html = method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"QXTAe"))
            {

                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            #endregion
            webBrowser1.Navigate(path+"static/index.html"); //按照姓名找回 执行加密RSA JS方法
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            getdata();
            getpics();




        }


        int refresh = 0;
        public void run()
        {
            getpics();
            if (DateTime.Now.Hour==2 && refresh==0)
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


                    //判断指定周几
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
                        if (text[2]=="无" ||text[3]=="无")
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

                        string shangchuanmamsg = "";
                        if (fc.status == "2" && !start.Contains("success"))
                        {
                            if (pics.Contains(username))
                            {
                                string myma = fc.getmyma(cookie, traineeid);
                                
                                if (listView1.Items[i].SubItems[0].Text.Contains("集中实习"))
                                {
                                   // MessageBox.Show("1");签到没成功可能是图片文件夹没有用户名图片，然后调用了不上传的签到
                                    shangchuanmamsg = fc.shangchuanmaandtravelCodeImg(cookie, myma);
                                }
                                else
                                {
                                    shangchuanmamsg = fc.shangchuanma(cookie, myma);
                                }
                            }


                            string msg = qiandao(cookie, address, traineeid);
                            //if(msg.Contains("操作异常"))
                            //{
                            //    msg = qiandao2(cookie, address, traineeid);
                            //}

                            listView1.Items[i].SubItems[7].Text = shangchuanmamsg+"  "+msg;
                        }
                        if (fc.status == "1" && !end.Contains("success"))
                        {
                            if (pics.Contains(username))
                            {
                              
                                string myma = fc.getmyma(cookie,traineeid);
                               

                                if (listView1.Items[i].SubItems[0].Text.Contains("集中实习"))
                                {
                                   // MessageBox.Show("2");
                                    shangchuanmamsg = fc.shangchuanmaandtravelCodeImg(cookie, myma);
                                }
                                else
                                {
                                    shangchuanmamsg = fc.shangchuanma(cookie, myma);
                                }

                                
                            }

                            string msg = qiandao(cookie, address, traineeid);
                            //if (msg.Contains("操作异常"))
                            //{
                            //    msg = qiandao2(cookie, address, traineeid);
                            //}
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
            #region 通用检测


            string html = method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"QXTAe"))
            {

                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            #endregion
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













        private delegate string Encrypt(string traineeId, string adcode,string lat,string lng,string address,string clockStatus);

        public string getencrypt(string traineeId,string adcode, string lat, string lng, string address,string clockStatus)
        {

            string result = webBrowser1.Document.InvokeScript("getdata", new object[] { traineeId,adcode,lat,lng,address,clockStatus }).ToString();
            return result;
        }

        //public string getencrypt2(string traineeId, string adcode, string lat, string lng, string address, string clockStatus)
        //{

        //    string result = webBrowser1.Document.InvokeScript("getdata2", new object[] { traineeId, adcode, lat, lng, address, clockStatus }).ToString();
        //    return result;
        //}

        private void button2_Click(object sender, EventArgs e)
        {
    
            timer1.Stop();
           
          
        }



       
        public string qiandao(string cookie, string addr, string traineeId)
        {

            try
            {
                string aurl = "https://xcx.xybsyw.com/student/clock/GetPlan!detail.action";
                string apostdata = "traineeId=" + traineeId;
                string aHtml = function.PostUrl(aurl, apostdata, cookie, "utf-8", "application/x-www-form-urlencoded", "");
                string address = Regex.Match(aHtml, @"""address"":""([\s\S]*?)""").Groups[1].Value.Trim();
                string lat = Regex.Match(aHtml, @"""lat"":([\s\S]*?),").Groups[1].Value.Trim();
                string lng = Regex.Match(aHtml, @"""lng"":([\s\S]*?),").Groups[1].Value.Trim();

                if (address.Trim() == "")
                {
                    address = System.Web.HttpUtility.UrlEncode(addr);
                    string baiduurl = "https://api.map.baidu.com/geocoding/v3/?address=" + address + "&output=json&ak=9DemeyQjUrIX14Fz8uEwVpGyKErUP4Sb&callback=showLocation";
                    string baiduhtml = method.GetUrl(baiduurl, "utf-8");
                    lat = Regex.Match(baiduhtml, @"lat"":([\s\S]*?)}").Groups[1].Value.Trim();
                    lng = Regex.Match(baiduhtml, @"lng"":([\s\S]*?),").Groups[1].Value.Trim();


                }
               
                string adcode = fc.getadcode(lng, lat);

                string url = "https://xcx.xybsyw.com/student/clock/PostNew.action";

              //  string url = "https://xcx.xybsyw.com/student/clock/Post.action";
                string postdata = "model=microsoft&brand=microsoft&platform=windows&" + "traineeId=" + traineeId + "&adcode=" + adcode + "&lat=" + lat + "&lng=" + lng + "&address=" + address + "&deviceName=microsoft&punchInStatus=1&clockStatus=" + fc.status;


                if (address.Contains("%"))
                {
                    address = System.Web.HttpUtility.UrlDecode(address);
                }
                Encrypt aa = new Encrypt(getencrypt);
                IAsyncResult iar = BeginInvoke(aa, new object[] { traineeId, adcode, lat, lng, address, fc.status });
                string crypt = EndInvoke(iar).ToString();
                string[] text = crypt.Split(new string[] { "," }, StringSplitOptions.None);

                string t = text[0];
                string s = text[1];
                string m = text[2];
                string html = jiamipost.PostUrl(url, postdata, cookie, m,t,s);
                MatchCollection msgs = Regex.Matches(html, @"""msg"":""([\s\S]*?)""");
                string msg = msgs[msgs.Count - 1].Groups[1].Value;
                
                if (msg != "")
                {
                    return msg;
                }
                else
                {
                    return "failed";
                }
            }
            catch (Exception ex)
            {

               textBox2.Text=ex.ToString();
                return "failed";
            }
        }






        ///// <summary>
        ///// 包含openId  unionId的签到
        ///// </summary>
        ///// <param name="cookie"></param>
        ///// <param name="addr"></param>
        ///// <param name="traineeId"></param>
        ///// <returns></returns>

        //public string qiandao2(string cookie, string addr, string traineeId)
        //{

        //    try
        //    {
        //        string aurl = "https://xcx.xybsyw.com/student/clock/GetPlan!detail.action";
        //        string apostdata = "traineeId=" + traineeId;
        //        string aHtml = function.PostUrl(aurl, apostdata, cookie, "utf-8", "application/x-www-form-urlencoded", "");
        //        string address = Regex.Match(aHtml, @"""address"":""([\s\S]*?)""").Groups[1].Value.Trim();
        //        string lat = Regex.Match(aHtml, @"""lat"":([\s\S]*?),").Groups[1].Value.Trim();
        //        string lng = Regex.Match(aHtml, @"""lng"":([\s\S]*?),").Groups[1].Value.Trim();

        //        if (address.Trim() == "")
        //        {
        //            address = System.Web.HttpUtility.UrlEncode(addr);
        //            string baiduurl = "https://api.map.baidu.com/geocoding/v3/?address=" + address + "&output=json&ak=9DemeyQjUrIX14Fz8uEwVpGyKErUP4Sb&callback=showLocation";
        //            string baiduhtml = method.GetUrl(baiduurl, "utf-8");
        //            lat = Regex.Match(baiduhtml, @"lat"":([\s\S]*?)}").Groups[1].Value.Trim();
        //            lng = Regex.Match(baiduhtml, @"lng"":([\s\S]*?),").Groups[1].Value.Trim();


        //        }

        //        string adcode = fc.getadcode(lng, lat);

        //        //string url = "https://xcx.xybsyw.com/student/clock/PostNew.action";

        //         string url = "https://xcx.xybsyw.com/student/clock/Post.action";
        //        string postdata = "model=microsoft&brand=microsoft&platform=windows&system=Windows 10 x64&openId=ooru94lH0MDBlYKT4dUwpEkRyAWQ&unionId=oHY-uwfu_6jixOW8l8A1pOhFuvvY&" + "traineeId=" + traineeId + "&adcode=" + adcode + "&lat=" + lat + "&lng=" + lng + "&address=" + address + "&deviceName=microsoft&punchInStatus=0&clockStatus=" + fc.status;

        //        Encrypt aa = new Encrypt(getencrypt2);
        //        IAsyncResult iar = BeginInvoke(aa, new object[] { traineeId, adcode, lat, lng, System.Web.HttpUtility.UrlDecode(address), fc.status });
        //        string crypt = EndInvoke(iar).ToString();
        //        string[] text = crypt.Split(new string[] { "," }, StringSplitOptions.None);

        //        string t = text[0];
        //        string s = text[1];
        //        string m = text[2];
        //        string html = jiamipost.PostUrl(url, postdata, cookie, m, t, s);
        //        MatchCollection msgs = Regex.Matches(html, @"""msg"":""([\s\S]*?)""");
        //        string msg = msgs[msgs.Count - 1].Groups[1].Value;
        //        if (msg != "")
        //        {
        //            return msg;
        //        }
        //        else
        //        {
        //            return "failed";
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        textBox2.Text = ex.ToString();
        //        return "failed";
        //    }
        //}




    }
}
