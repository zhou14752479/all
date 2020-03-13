using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;
using MySql.Data.MySqlClient;

namespace _58二手房
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 插入数据库
        /// </summary>
        public void insertdata(string sql)
        {
            try
            {

                string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\data.db");
                mycon.Open();

                SQLiteCommand cmd = new SQLiteCommand(sql, mycon);

                cmd.ExecuteNonQuery();  //执行sql语句
                mycon.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.ToString());

            }

        }



        ArrayList telList = new ArrayList();

        ArrayList finishes = new ArrayList();
        /// <summary>
        /// 读取数据库
        /// </summary>
        public void getdata()
        {
            try
            {

                string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\data.db");
                mycon.Open();

                SQLiteCommand cmd = new SQLiteCommand("select tel from tels", mycon);
                SQLiteDataReader rdr = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(rdr);
                
                for (int i = 0; i < table.Rows.Count; i++) // 遍历行
                {

                    telList.Add(table.Rows[i]["tel"]);

                }
                mycon.Close();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.ToString());

            }

        }







        private void setChildNodeCheckedState(TreeNode currNode, bool state)
        {
            TreeNodeCollection nodes = currNode.Nodes;
            if (nodes.Count > 0)
            {
                foreach (TreeNode tn in nodes)
                {
                    tn.Checked = state;
                    setChildNodeCheckedState(tn, state);
                }
            }
        }
        private void setParentNodeCheckedState(TreeNode currNode, bool state)
        {
            TreeNode parentNode = currNode.Parent;
            parentNode.Checked = state;
            if (currNode.Parent.Parent != null)
            {
                setParentNodeCheckedState(currNode.Parent, state);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        bool zanting = true;
        bool status = true;
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"18254571301"))
            {
                status = true;
                button1.Enabled = false;
                Thread thread = new Thread(new ThreadStart(mobilerun));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;


            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion



          
            
            
          


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
               string COOKIE = "Hm_lvt_295da9254bbc2518107d846e1641908e=1582933513,1582983468; 58tj_uuid=6af0cab4-7088-4082-b9f8-7c7438d6ab59; new_uv=2; wmda_new_uuid=1; wmda_uuid=81b63df3f3c3e53f507fd2a66fea6a34; wmda_visited_projects=%3B6333604277682; m58comvp=t29v115.159.229.19; als=0; id58=e87rZV5ZpeYPc5QcCxJKAg==";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "";
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.1.1 Mobile/15E148 Safari/604.1";

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
                MessageBox.Show(ex.ToString());

            }
            return "";
        }
        #endregion

        #region GET使用代理IP请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlwithIP(string Url, string ip)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "Hm_lvt_c58e42b54acb40ab70d48af7b1ce0d6a=1563157838,1563157854; ASPSESSIONIDSCRDCDQB=NBHMLEHCKHKNFDMPGPGKFPNP; fikker-vMnk-0qnk=nyMU6OJy8iTIpYhmd5bST9RwBSD9TGV1; fikker-vMnk-0qnk=nyMU6OJy8iTIpYhmd5bST9RwBSD9TGV1; fikker-0epN-KaRa=dGd9KSVkZ7VSnSZSrIPidYtMDe0UVLOA; Hm_lvt_a2f6ee5c5c2efc17b10dc0659462df30=1563161043,1563259279,1563259736,1563259814; Hm_lpvt_a2f6ee5c5c2efc17b10dc0659462df30=1563262152";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.1.1 Mobile/15E148 Safari/604.1";
                WebProxy proxy = new WebProxy(ip);
                request.Proxy = proxy;

                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 8000;
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



        public string getcityId(string city)
        {
            string html = GetUrl("https://"+city+".58.com/");
            Match value = Regex.Match(html, @"'area':'([\s\S]*?)'");
            return value.Groups[1].Value;
        }

        public string getcityname(string city)
        {
            string html = GetUrl("https://" + city + ".58.com/");
            Match value = Regex.Match(html, @"content=""58同城([\s\S]*?)分类");
            return value.Groups[1].Value;
        }

        /// <summary>
        /// 电脑端
        /// </summary>
        public void pcrun()
        {
            getnodes();
            try
            {
              
                foreach (string city in citys)
                {
                    for (int i = 1; i <31; i++)
                    {
                        string url = "https://"+city+".58.com/ershoufang/0/pn"+i+"/";
                        string html = method.GetUrl(url,"utf-8");
                        MatchCollection uids = Regex.Matches(html, @"esf_id:([\s\S]*?),");
                        foreach (Match uid in uids)
                        {
                            string URL = "https://"+city+".58.com/ershoufang/"+uid.Groups[1].Value+"x.shtml";
                            //string ahtml = method.GetUrlwithIP(URL,"tps185.kdlapi.com:15818");
                            string ahtml = method.GetUrl(URL, "utf-8");

                            Match  telUrl= Regex.Match(ahtml, @"privacyCallUrl = '([\s\S]*?)'");
                            // Match tel= Regex.Match(method.GetUrlwithIP(telUrl.Groups[1].Value, "tps185.kdlapi.com:15818"), @"data"":""([\s\S]*?)""");
                            Match tel = Regex.Match(method.GetUrl(telUrl.Groups[1].Value, "utf-8"), @"data"":""([\s\S]*?)""");

                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                            lv1.SubItems.Add(tel.Groups[1].Value);
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            Thread.Sleep(1000);
                            if (status == false)
                               
                                return;
                        }

                    }
                }
               
            }
            catch (Exception)
            {

                throw;
            }
        }
        private DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return dtStart.AddSeconds(Convert.ToDouble(timeStamp));
            
        }


        /// <summary>
        /// 手机端列表直接
        /// </summary>
        public void  mobilerun()
        {
           // getdata();
            getnodes();


            foreach (string city in citys)
            {
                string cityId = getcityId(city);
                string cityname = getcityname(city);
               
                for (int i = 1; i < Convert.ToInt32(textBox1.Text); i++)

                {
                    try
                    {
                        string url = "https://appsale.58.com/mobile/v5/sale/property/list?ajk_city_id="+cityId+ "&app=i-wb&udid2=bc7859f092322c90d7919f0427f7552e9a07154b&v=12.3.1&uuid=bc7859f092322c90d7919f0427f7552e9a07154b&is_ax_partition=0&entry=11&select_type=0&city_id=" + cityId + "&source_id=2&is_struct=1&page=" + i+"&page_size=41";

                       // string html=   GetUrlwithIP(url, "tps185.kdlapi.com:15818");
                        string html = GetUrl(url);
                        MatchCollection titles = Regex.Matches(html, @"""title"":""([\s\S]*?)""");
                        MatchCollection names = Regex.Matches(html, @"brokerId([\s\S]*?)name"":""([\s\S]*?)""");
                        MatchCollection tels = Regex.Matches(html, @"""mobile"":""([\s\S]*?)""");
                       // MatchCollection times = Regex.Matches(html, @"""post_date"":""([\s\S]*?)""");
                        if (tels.Count == 0)
                            break;


                        for (int j = 0; j < tels.Count; j++)
                        {

                            //if (!telList.Contains(tels[j].Groups[1].Value))
                            //{
                            //    if (!finishes.Contains(tels[j].Groups[1].Value))
                            //    {
                                    finishes.Add(tels[j].Groups[1].Value);
                                   // insertdata("INSERT INTO tels (tel) VALUES( '" + tels[j].Groups[1].Value + "')");
                                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                    lv1.SubItems.Add(titles[j].Groups[1].Value);
                                    lv1.SubItems.Add(names[j].Groups[2].Value);
                                    lv1.SubItems.Add(tels[j].Groups[1].Value);
                                    lv1.SubItems.Add("正在抓取" + cityname+ "第" + i + "页");
                                    //lv1.SubItems.Add(ConvertStringToDateTime(times[j].Groups[1].Value).ToString());
                                    while (this.zanting == false)
                                    {
                                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                    }

                                    if (status == false)

                                        return;
                            //    }
                            //}
                        }
                        Thread.Sleep(1000);
                    }
                    catch 
                    {
                        continue;
                        
                    }

                }

            }

            MessageBox.Show("抓取完成");
           
        }

        ArrayList citys = new ArrayList();
        public void getnodes()
        {
            foreach (TreeNode parentNode in skinTreeView1.Nodes)  //江苏省节点
            {
                foreach (TreeNode node in parentNode.Nodes)     //获取江苏省下的节点
                {
                    if (node.Checked)
                    {
                        if (!citys.Contains(node.Name))
                        {
                            citys.Add(node.Name);
                        }
                           
                    }
                }

            }

        }


        private void skinTreeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                
                if (e.Node.Checked == true)
                {
                    //选中节点之后，选中该节点所有的子节点
                    setChildNodeCheckedState(e.Node, true);
                }
                else if (e.Node.Checked == false)
                {
                    //取消节点选中状态之后，取消该节点所有子节点选中状态
                    setChildNodeCheckedState(e.Node, false);
                    //如果节点存在父节点，取消父节点的选中状态
                    if (e.Node.Parent != null)
                    {
                        setParentNodeCheckedState(e.Node, false);
                    }
                }
            }


            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            status = false;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (TreeNode parentNode in skinTreeView1.Nodes)  //江苏省节点
            {
                parentNode.Checked = true;
                foreach (TreeNode node in parentNode.Nodes)     //获取江苏省下的节点
                {
                    node.Checked = true;
                }

                }

        }

        private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
