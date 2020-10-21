using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 主程序202010
{
    public partial class 事考帮 : Form
    {
        [DllImport("shell32.dll")]
        public static extern int ShellExecute(IntPtr hwnd, StringBuilder lpszOp, StringBuilder lpszFile, StringBuilder lpszParams, StringBuilder lpszDir, int FsShowCmd);
        public 事考帮()
        {
            InitializeComponent();
        }
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "appkey=jiangsu; subject=s92; Hm_lvt_16e08538ac8544060b2c6accd6cdc877=1601945291,1601949380,1602667008; province=a11; subject_group=sg3; skb_user_token=YXsrJSdudGZvcGsnPyM2Ozw0PDY2ODs0PCYyJ3VieHd9c3hpJzsnaDg6ODk1Zz03amhrNWgyajs%2Fajw4N2M3amlmZztoZTUmMiZ5anh0bnN0Y29pJzsnNTc3ODw5MjY6NjY8OzwxNzcoMCh6aGppJkAmNzY4Mzw4NyYicTkp; shikaobang_img_code=16026682251618; Hm_lpvt_16e08538ac8544060b2c6accd6cdc877=1602668229";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";

                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                string content = reader.ReadToEnd();


                reader.Close();
                response.Close();
                return content;



            }
            catch (System.Exception ex)
            {
                ex.ToString();

            }
            return "";
        }
        #endregion
        private void 事考帮_Load(object sender, EventArgs e)
        {
            getids();
        }
        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }
        bool zanting = true;
       
        Dictionary<string, string> dic = new Dictionary<string, string>();
        Dictionary<string, string> dic2 = new Dictionary<string, string>();
        public void getids()
        {
            dic.Clear();
           

            string url = "http://www.shikaobang.cn/1.0/jiangsu/practice/point?no_type=1";
            string html = GetUrl(url);
            MatchCollection ahtmls = Regex.Matches(html, @"super-head-icon icon_plus"">([\s\S]*?)</ul>");
            foreach (Match ahtml in ahtmls)
            {
                 Match aname = Regex.Match(ahtml.Groups[1].Value, @"<span class=""tel"">([\s\S]*?)</span>");
                comboBox1.Items.Add(aname.Groups[1].Value);
                MatchCollection ids = Regex.Matches(ahtml.Groups[1].Value, @"<a class=""catalog-link study"" key=""([\s\S]*?)""");
                MatchCollection idnames = Regex.Matches(ahtml.Groups[1].Value, @"<p class=""tel"">([\s\S]*?)</p>");
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < ids.Count; i++)
                {
                    sb.Append(ids[i].Groups[1].Value + "-" + idnames[i].Groups[1].Value+"#");
                    

                }
                dic.Add(aname.Groups[1].Value, sb.ToString());
            }
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            dic2.Clear();
            string value = dic[comboBox1.Text.Trim()];
            string[] text = value.Split(new string[] { "#" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                
                string[] a= text[i].Split(new string[] { "-" }, StringSplitOptions.None);
                if (a.Length > 1)
                {
                    dic2.Add(a[1], a[0]);
                    comboBox2.Items.Add(a[1]);
                }
            }
        }

        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {

           
        
            try
            {
                if (comboBox2.Text == "")
                {
                    MessageBox.Show("请选择分类");
                    return;
                }
               
                    string uid = dic2[comboBox2.Text.Trim()];
                    
                    string url = "http://www.shikaobang.cn/1.0/jiangsu/question/questions?page=1&question_type=1&point="+uid+"&no_type=1&subject=s92&mod=point&appkey=jiangsu&direction=behind";
                    string html = GetUrl(url);


                    MatchCollection ahtmls = Regex.Matches(html, @"""t"":""([\s\S]*?)source");

                    for (int i = 0; i < ahtmls.Count; i++)
                    {

                        MatchCollection cs = Regex.Matches(ahtmls[i].Groups[1].Value, @"""c"":""([\s\S]*?)""");
                        Match a = Regex.Match(ahtmls[i].Groups[1].Value, @"""a"":""([\s\S]*?)""");
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        try
                        {
                        string title = Unicode2String(cs[0].Groups[1].Value).Replace("[img=", "").Replace("\\", "").Replace("]","");
                       title= Regex.Replace(title, @"\(.*?\)", "");


                        string jiexi= Unicode2String(cs[5].Groups[1].Value).Replace("[img=", "").Replace("\\", "").Replace("]", "");
                        jiexi = Regex.Replace(jiexi, @"\(.*?\)", "");

                        lv1.SubItems.Add(title);
                            lv1.SubItems.Add(Unicode2String(cs[1].Groups[1].Value));
                            lv1.SubItems.Add(Unicode2String(cs[2].Groups[1].Value));
                            lv1.SubItems.Add(Unicode2String(cs[3].Groups[1].Value));
                            lv1.SubItems.Add(Unicode2String(cs[4].Groups[1].Value));

                            lv1.SubItems.Add(a.Groups[1].Value);
                            lv1.SubItems.Add(jiexi);
                            lv1.SubItems.Add("职测");
                            lv1.SubItems.Add(comboBox1.Text);
                        lv1.SubItems.Add(comboBox2.Text);
                    }
                        catch (Exception)
                        {
                            lv1.SubItems.Add("");

                        }

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                    }



                }

            
            catch (Exception)
            {

                throw;
            }

        }
        Thread thread;
        
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html");

            if (!html.Contains(@"shikaobang"))
            {
                MessageBox.Show("验证失败");
                return;
            }

            #endregion

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // System.Diagnostics.Process.Start(@"C:\Users\zhou\Desktop\百度上传\zhou14752479.py");
            //需要打开的地方插入此段代码
            // ShellExecute(IntPtr.Zero, new StringBuilder("Open"), new StringBuilder("zhou14752479.py"), new StringBuilder(""), new StringBuilder(@"C:\Users\zhou\Desktop\百度上传"), 1);
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

      
    }
}
