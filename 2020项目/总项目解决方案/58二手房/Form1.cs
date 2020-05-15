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
            status = true;
            button1.Enabled = false;
            Thread thread = new Thread(new ThreadStart(mobilerun));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;



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
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.12(0x17000c23) NetType/4G Language/zh_CN";

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
                string COOKIE = "uid=; apn=\"WWAN\"; id58=e87rZV59klVl+fsBEuUpAg==; 58openudid=\"A0659638 - A5A8 - 4068 - 8BF0 - 4F59C28ED81B\"; 58ua=58app; Accept-Encoding=\"deflate,gzip\"; channelid=\"80000\"; charset=\"UTF - 8\"; cid=\"2350\"; cimei=\"0f607264fc6318a92b9e13c65db7cd3c\"; coordinatesystem=GCJ-02; cversion=\"9.7.1\"; dirname=\"suqian\"; f=\"58\"; locationaccuracy=65.000000; ltext=\" % E5 % AE % BF % E8 % BF % 81 -% E5 % AE % BF % E5 % 9F % 8E\"; m=\"0f607264fc6318a92b9e13c65db7cd3c\"; mcity=\"2350\"; netType=4g; openudid=\"bc7859f092322c90d7919f0427f7552e9a07154b\"; os=\"ios\"; osv=\"12.3.1\"; platform=\"iphone\"; productorid=\"3\"; r=\"414_736\"; sid=\"0\"; tokenid=8d0823fddf07e56c07f61c75bced60d1bd57de286ec7aba2e880386fe11321cd; ua=\"iPhone 7P_iOS 12.3.1\"; uploadtime=\"20200327134226\"; uuid=\"4B910935 - EB59 - 4535 - ACBF - CEB9E77C7AE1\"; webviewType=\"wkwebview\"; lat=\"33.965890\"; locationstate=\"1\"; lon=\"118.286450\"; maptype=\"1\"; owner=\"google\"; messId=cdfd51df-2e32-4e81-9fed-c9ee466cf5da";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.1.1 Mobile/15E148 Safari/604.1";
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.12(0x17000c23) NetType/4G Language/zh_CN";
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
            if (checkBox1.Checked == true)
            {
                 getdata();
            }

            getnodes();


            foreach (string city in citys)
            {
                string cityId = getcityId(city);
                string cityname = getcityname(city);
               
                for (int i = 1; i < Convert.ToInt32(textBox1.Text); i++)

                {
                    try
                    {
                        string url = "https://miniapp.58.com/sale/property/list?cid="+cityId+"&from=58_ershoufang&app=i-wb&platform=ios&b=iPhone&s=iOS12.3.1&t=1585296563&cv=5.0&wcv=5.0&wv=7.0.12&sv=2.10.3&batteryLevel=69&muid=33369ab43c140f725624e8ed4aa4ccaf&weapp_version=1.0.0&user_id=&oid=oIArb4tHXwSbAOMiJpA7LwxGVlY0&udid=oIArb4tHXwSbAOMiJpA7LwxGVlY0&page="+i+"&page_size=25&open_id=&union_id=&token=&source_id=2&orderby=6&entry=1003&city_id="+cityId;
                        
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

                            if (!telList.Contains(tels[j].Groups[1].Value))
                            {
                                if (checkBox1.Checked == true)
                                {
                                    insertdata("INSERT INTO tels (tel) VALUES( '" + tels[j].Groups[1].Value + "')");
                                }
                                   
                                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                  //  lv1.SubItems.Add(titles[j].Groups[1].Value);
                                    lv1.SubItems.Add(names[j].Groups[2].Value);
                                    lv1.SubItems.Add(tels[j].Groups[1].Value);
                                lv1.SubItems.Add(cityname);
                                //  lv1.SubItems.Add("正在抓取" + cityname + "第" + i + "页");
                                label2.Text = "正在抓取" + cityname + "第" + i + "页";
                                    //lv1.SubItems.Add(ConvertStringToDateTime(times[j].Groups[1].Value).ToString());
                                    while (this.zanting == false)
                                    {
                                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                    }
                                    if (listView1.Items.Count > 2)
                                    {
                                        listView1.EnsureVisible(listView1.Items.Count - 1);
                                    }

                                    if (status == false)

                                        return;
                                    Thread.Sleep(100);
                                
                            

                            }

                            else
                            {
                                label2.Text = "正在抓取" + cityname + "第" + i + "页,数据库已包含，不添加";
                            }



                        }
                        Thread.Sleep(3500);
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }
    }
}
