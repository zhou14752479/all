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
    public partial class 校友邦 : Form
    {
        public 校友邦()
        {
            InitializeComponent();
        }

        

        private void 校友邦_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
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
                        lv1.SubItems.Add("");
                    }
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
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

        public string qiandao(string cookie, string address,string traineeId)
        {
            address = System.Web.HttpUtility.UrlEncode(address);
            string baidumapUrl = "https://api.map.baidu.com/geocoding/v3/?address="+address+"&output=json&ak=9DemeyQjUrIX14Fz8uEwVpGyKErUP4Sb&callback=showLocation";
            string baidumapHtml = method.GetUrl(baidumapUrl,"utf-8");
            string lat = Regex.Match(baidumapHtml, @"lat"":([\s\S]*?)}").Groups[1].Value.Trim();
            string lng = Regex.Match(baidumapHtml, @"lng"":([\s\S]*?),").Groups[1].Value.Trim();


           
            string url = "https://xcx.xybsyw.com/student/clock/Post!updateClock.action";
            string postdata = "traineeId="+ traineeId + "&adcode=640104&lat=" + lat+"&lng="+lng+"&address="+address+"&deviceName=microsoft&punchInStatus=0&clockStatus=2";
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
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                try
                {
                    string username = listView1.Items[i].SubItems[1].Text;
                    string password = listView1.Items[i].SubItems[2].Text;
                    string address = listView1.Items[i].SubItems[3].Text;
                    string cookie = login(username,password);
                    if (cookie == "")
                    {
                        listView1.Items[i].SubItems[4].Text = "登陆失败";
                        continue;
                    }
                    else
                    {
                        string planid = getplanid(cookie);
                        string traineeid = gettraineeId(planid,cookie);
                      string msg=  qiandao(cookie,address, traineeid);
                        listView1.Items[i].SubItems[4].Text = msg;

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
    }
}
