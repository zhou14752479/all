using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using myDLL;
namespace 校友邦
{
    public partial class 校友邦 : Form
    {
        public 校友邦()
        {
            InitializeComponent();
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.inipath);
        }

        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
            return temp.ToString();
        }

        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public bool ExistINIFile()
        {
            return File.Exists(inipath);
        }

        public void jiance()
        {
            if (ExistINIFile())
            {
                string key = IniReadValue("values", "key");
                string secret = IniReadValue("values", "secret");
                string[] value = secret.Split(new string[] { "asd147" }, StringSplitOptions.None);


                if (Convert.ToInt32(value[1]) < Convert.ToInt32(method.GetTimeStamp()))
                {
                    MessageBox.Show("激活已过期");
                    string str = Interaction.InputBox("您的机器码如下，请复制机器码提供到后台，输入激活码然后激活！", "激活软件", method.GetMD5(method.GetMacAddress()), -1, -1);
                    string[] text = str.Split(new string[] { "asd" }, StringSplitOptions.None);

                    if (text[0] == method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian"))
                    {
                        IniWriteValue("values", "key", method.GetMD5(method.GetMacAddress()));
                        IniWriteValue("values", "secret", str);
                        MessageBox.Show("激活成功");


                    }
                    else
                    {
                        MessageBox.Show("激活码错误");
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                        return;
                    }

                }


                else if (value[0] != method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian") || key != method.GetMD5(method.GetMacAddress()))
                {

                    string str = Interaction.InputBox("您的机器码如下，请复制机器码提供到后台，输入激活码然后激活！", "激活软件", method.GetMD5(method.GetMacAddress()), -1, -1);
                    string[] text = str.Split(new string[] { "asd147" }, StringSplitOptions.None);

                    if (text[0] == method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian"))
                    {
                        IniWriteValue("values", "key", method.GetMD5(method.GetMacAddress()));
                        IniWriteValue("values", "secret", str);
                        MessageBox.Show("激活成功");


                    }
                    else
                    {
                        MessageBox.Show("激活码错误");
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                        return;
                    }
                }


            }
            else
            {

                string str = Interaction.InputBox("您的机器码如下，请复制机器码提供到后台，输入激活码然后激活！", "激活软件", method.GetMD5(method.GetMacAddress()), -1, -1);
                string[] text = str.Split(new string[] { "asd147" }, StringSplitOptions.None);
                if (text[0] == method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian"))
                {
                    IniWriteValue("values", "key", method.GetMD5(method.GetMacAddress()));
                    IniWriteValue("values", "secret", str);
                    MessageBox.Show("激活成功");


                }
                else
                {
                    MessageBox.Show("激活码错误");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    return;
                }
            }

        }
        private void 校友邦_Load(object sender, EventArgs e)
        {
            jiance();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                StreamReader sr = new StreamReader(openFileDialog1.FileName, method.EncodingType.GetTxtType(openFileDialog1.FileName));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {
                    string[] value = text[i].Split(new string[] { "#" }, StringSplitOptions.None);
                    if (value.Length > 2)
                    {
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(value[0]);
                        lv1.SubItems.Add(value[1]);
                        lv1.SubItems.Add(value[2]);
                        lv1.SubItems.Add(value[3]);
                        lv1.SubItems.Add(value[4]);
                     
                        lv1.SubItems.Add("");
                    }
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            }


            for (int i = 0; i < listView1.Items.Count; i++)
            {
                TimeSpan t = Convert.ToDateTime(listView1.Items[i].SubItems[5].Text)-DateTime.Now;
                if (t.Days >= 3)
                {
                    listView1.Items[i].BackColor = Color.Green;
                }
                else
                {
                    listView1.Items[i].BackColor = Color.Red;
                }
            }
        }


        public string login(string username,string password)
        {
            password = method.GetMD5(password);
            string url = "https://xcx.xybsyw.com/login/login.action";
            string postdata = "username="+username+"&password="+password+"&openId=ooru94lH0MDBlYKT4dUwpEkRyAWQ&deviceId=";
            string html = method.PostUrl(url,postdata,"","utf-8", "application/x-www-form-urlencoded", "");
           
            string sessionId = Regex.Match(html, @"""sessionId"":""([\s\S]*?)""").Groups[1].Value;
            if (sessionId != "")
            {
                return "JSESSIONID=" + sessionId + ";";
            }
            else
            {
                return "";
            }
        }

        //public string Duration()
        //{
        //    password = method.GetMD5(password);
        //    string url = "https://xcx.xybsyw.com/login/login.action";
        //    string postdata = "username=" + username + "&password=" + password + "&openId=ooru94lH0MDBlYKT4dUwpEkRyAWQ&deviceId=";
        //    string html = method.PostUrl(url, postdata, "", "utf-8", "application/x-www-form-urlencoded", "");

        //    string sessionId = Regex.Match(html, @"""sessionId"":""([\s\S]*?)""").Groups[1].Value;
        //    if (sessionId != "")
        //    {
        //        return "JSESSIONID=" + sessionId + ";";
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}

        public string getplanid(string cookie)
        {

            string url = "https://xcx.xybsyw.com/student/progress/ProjectList.action";
            string postdata = "";
            string html = method.PostUrl(url, postdata, cookie, "utf-8", "application/x-www-form-urlencoded", "");
           
            string planid = Regex.Match(html, @"""planId"":([\s\S]*?),").Groups[1].Value;
            return planid;
        }

        public string gettraineeId(string planid,string cookie)
        {

            string url = "https://xcx.xybsyw.com/student/clock/GetPlan!getDefault.action";
            string postdata = "planId="+planid;
            string html = method.PostUrl(url, postdata, cookie, "utf-8", "application/x-www-form-urlencoded", "");
         
            string traineeId = Regex.Match(html, @"""traineeId"":([\s\S]*?)}").Groups[1].Value;
            return traineeId;
        }


        public string qiandao(string cookie,string addr, string traineeId)
        {

            string aurl = "https://xcx.xybsyw.com/student/clock/GetPlan!detail.action";
            string apostdata = "traineeId=" + traineeId;
            string aHtml = method.PostUrl(aurl, apostdata, cookie, "utf-8", "application/x-www-form-urlencoded", "");
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


            string url = "https://xcx.xybsyw.com/student/clock/PostNew.action";
            string postdata = "traineeId=" + traineeId + "&adcode=640104&lat=" + lat + "&lng=" + lng + "&address=" + address + "&deviceName=microsoft&punchInStatus=1&clockStatus="+status;
            
            string html = method.PostUrl(url, postdata, cookie, "utf-8", "application/x-www-form-urlencoded", "");

            string msg = Regex.Match(html, @"""msg"":""([\s\S]*?)""").Groups[1].Value;
            if (msg != "")
            {
                return msg;
            }
            else
            {
                return "failed";
            }
        }


        public string chongxinqiandao(string cookie, string addr, string traineeId)
        {
            string aurl = "https://xcx.xybsyw.com/student/clock/GetPlan!detail.action";
            string apostdata = "traineeId=" + traineeId;
            string aHtml = method.PostUrl(aurl, apostdata, cookie, "utf-8", "application/x-www-form-urlencoded", "");
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
           
         

          


            string url = "https://xcx.xybsyw.com/student/clock/Post!updateClock.action";
            string postdata = "traineeId="+ traineeId + "&adcode=640104&lat=" + lat+"&lng="+lng+"&address="+address+"&deviceName=microsoft&punchInStatus=1&clockStatus="+status;
           
            string html = method.PostUrl(url, postdata, cookie, "utf-8", "application/x-www-form-urlencoded", "");
          
            string msg = Regex.Match(html, @"""msg"":""([\s\S]*?)""").Groups[1].Value;
            if (msg != "")
            {
                return msg;
            }
            else
            {
                return "failed";
            }
        }



        public void run()
        {
            if (listView1.CheckedItems.Count == 0)
            {
                MessageBox.Show("请勾选需要签到的数据行");
                return;
            }
            for (int i = 0; i < listView1.CheckedItems.Count; i++)
            {
                try
                {
                    string username = listView1.CheckedItems[i].SubItems[1].Text;
                    string password = listView1.CheckedItems[i].SubItems[2].Text;
                    string address= listView1.CheckedItems[i].SubItems[3].Text;

                    string cookie = login(username,password);
                    if (cookie == "")
                    {
                        listView1.Items[i].SubItems[6].Text = "登陆失败";
                        continue;
                    }
                    else
                    {
                        string planid = getplanid(cookie);
                        string traineeid = gettraineeId(planid,cookie);

                      string msg=  qiandao(cookie, address, traineeid);
                        listView1.CheckedItems[i].SubItems[6].Text = msg;

                    }

                    Thread.Sleep(1000);
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }



        public void chongxin_run()
        {
            if (listView1.CheckedItems.Count == 0)
            {
                MessageBox.Show("请勾选需要签到的数据行");
                return;
            }
            for (int i = 0; i < listView1.CheckedItems.Count; i++)
            {
                try
                {
                    string username = listView1.CheckedItems[i].SubItems[1].Text;
                    string password = listView1.CheckedItems[i].SubItems[2].Text;
                    string address = listView1.CheckedItems[i].SubItems[3].Text;
                    string cookie = login(username, password);
                    if (cookie == "")
                    {
                        listView1.Items[i].SubItems[6].Text = "登陆失败";
                        continue;
                    }
                    else
                    {
                        string planid = getplanid(cookie);
                        string traineeid = gettraineeId(planid, cookie);

                        string msg = chongxinqiandao(cookie, address,traineeid);
                        listView1.CheckedItems[i].SubItems[6].Text = msg;

                    }

                    Thread.Sleep(1000);
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }


       

        Thread thread;
        bool zanting = true;
        private void button2_Click(object sender, EventArgs e)
        {
            #region 通用检测


            string html = method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"QXTAe"))
            {

                return;
            }

            #endregion
            status = "2";
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
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

        private void button4_Click(object sender, EventArgs e)
        {
            #region 通用检测


            string html = method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"QXTAe"))
            {

                return;
            }

            #endregion
            status = "2";
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(chongxin_run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            status = "1";
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        string status = "2";
        private void button6_Click(object sender, EventArgs e)
        {
            status = "1";
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(chongxin_run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Checked = true;
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Checked = false;
            }
        }
    }
}
