using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

namespace _58二手房
{
    public partial class 二手房 : Form
    {
        public 二手房()
        {
            InitializeComponent();
        }

        private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void 二手房_Load(object sender, EventArgs e)
        {

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
                MessageBox.Show(ex.ToString());

            }
            return "";
        }
        #endregion
        public string getcityId(string city)
        {
            string html = GetUrl("https://" + city + ".58.com/");
            Match value = Regex.Match(html, @"'area':'([\s\S]*?)'");
            return value.Groups[1].Value;
        }

        public string getcityname(string city)
        {
            string html = GetUrl("https://" + city + ".58.com/");
            Match value = Regex.Match(html, @"content=""58同城([\s\S]*?)分类");
            return value.Groups[1].Value;
        }


        ArrayList citys = new ArrayList();

        bool zanting = true;

       
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
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"58huiyuan"))
            {
               
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
        /// <summary>
        /// 手机端列表直接
        /// </summary>
        public void mobilerun()
        {
          

            getnodes();


            foreach (string city in citys)
            {
                string cityId = getcityId(city);
                string cityname = getcityname(city);

                for (int i = 1; i < Convert.ToInt32(textBox1.Text); i++)

                {
                    try
                    {
                        string url = "https://miniapp.58.com/sale/property/list?cid=" + cityId + "&from=58_ershoufang&app=i-wb&platform=ios&b=iPhone&s=iOS12.3.1&t=1585296563&cv=5.0&wcv=5.0&wv=7.0.12&sv=2.10.3&batteryLevel=69&muid=33369ab43c140f725624e8ed4aa4ccaf&weapp_version=1.0.0&user_id=&oid=oIArb4tHXwSbAOMiJpA7LwxGVlY0&udid=oIArb4tHXwSbAOMiJpA7LwxGVlY0&page=" + i + "&page_size=25&open_id=&union_id=&token=&source_id=2&orderby=6&entry=1003&city_id=" + cityId;

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

                  

                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add(titles[j].Groups[1].Value);
                                lv1.SubItems.Add(names[j].Groups[2].Value);
                                lv1.SubItems.Add(tels[j].Groups[1].Value);
                                lv1.SubItems.Add("正在抓取" + cityname + "第" + i + "页");
                               
                                //lv1.SubItems.Add(ConvertStringToDateTime(times[j].Groups[1].Value).ToString());
                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                                if (listView1.Items.Count > 2)
                                {
                                    listView1.EnsureVisible(listView1.Items.Count - 1);
                                }

                                Thread.Sleep(100);



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
        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }
    }
}
