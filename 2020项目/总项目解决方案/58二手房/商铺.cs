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

namespace _58二手房
{
    public partial class 商铺 : Form
    {
        public 商铺()
        {
            InitializeComponent();
        }

        private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
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
                // request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.12(0x17000c23) NetType/4G Language/zh_CN";

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
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
        bool zanting = true;
        ArrayList telList = new ArrayList();
        #region  生意转让、商铺出租、商铺出售
        public void shangpu(object item)
        {
            if (checkBox1.Checked == true)
            {
                getdata();
            }
            getnodes();

            try
            {


                foreach (string city in citys)
                {
                    

                    for (int i = 1; i <= Convert.ToInt32(textBox1.Text); i++)
                    {
                        String Url = "https://" + city + ".58.com/" + item.ToString() + "/0/pn" + i + "/";
                       

                        string html = GetUrl(Url);



                        MatchCollection TitleMatchs = Regex.Matches(html, @"https://[a-z]+.58.com/[a-z]+/[0-9]+x.shtml", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                       // MatchCollection TitleMatchs = Regex.Matches(html, @"<div class=""pic"">([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                     
                        ArrayList lists = new ArrayList();

                        foreach (Match NextMatch in TitleMatchs)
                        {
                            if (!lists.Contains(NextMatch.Groups[0].Value))
                            {
                                lists.Add(NextMatch.Groups[0].Value);
                            }
                            
                        }


                        foreach (string list in lists)
                        {

                            try
                            {

                            
                            Match uid = Regex.Match(list, @"\d{10,}");

                            string strhtml = GetUrl("https://miniappfang.58.com/shop/plugin/v1/shopdetail?infoId=" + uid.Groups[0].Value + "&openId=77AA769A2A2C8740ECF1EDB47CD855A04C573D57DAF470CD8AD018A504661F6A");  //定义的GetRul方法 返回 reader.ReadToEnd()

                            Match title = Regex.Match(strhtml, @"""title"":""([\s\S]*?)""");
                            Match contacts = Regex.Match(strhtml, @"""brokerName"":""([\s\S]*?)""");
                            Match tel = Regex.Match(strhtml, @"""phone"":""([\s\S]*?)""");
                           


                            if (!telList.Contains(tel.Groups[1].Value))
                            {
                                if (checkBox1.Checked == true)
                                {
                                    insertdata("INSERT INTO tels (tel) VALUES( '" + tel.Groups[1].Value + "')");
                                }
                                label2.Text = "正在抓取第" + i + "页";
                                ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                listViewItem.SubItems.Add(title.Groups[1].Value);
                                listViewItem.SubItems.Add(contacts.Groups[1].Value);
                                listViewItem.SubItems.Add(tel.Groups[1].Value);
                               


                                Application.DoEvents();
                                Thread.Sleep(1000);   //内容获取间隔，可变量

                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                                }
                            }
                            catch 
                            {

                               continue;
                            }

                        }
                       

                    }
                }




            }

            catch (System.Exception ex)
            {

               ex.ToString();
            }


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


            status = true;
            button1.Enabled = false;
            if (radioButton1.Checked == true)
            {

                Thread thread = new Thread(new ParameterizedThreadStart(shangpu));
                string o = "shangpucz";
                thread.Start((object)o);
                Control.CheckForIllegalCrossThreadCalls = false;


            }
            else if (radioButton2.Checked == true)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(shangpu));
                string o = "shangpucs";
                thread.Start((object)o);
                Control.CheckForIllegalCrossThreadCalls = false;

            }

            else if (radioButton3.Checked == true)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(shangpu));
                string o = "shengyizr";
                thread.Start((object)o);
                Control.CheckForIllegalCrossThreadCalls = false;
                //创建带参数的线程

            }

        }
        bool status = true;
        private void button4_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void skinTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

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

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://suqian.58.com/shangpucz/");
        }
    }
}
