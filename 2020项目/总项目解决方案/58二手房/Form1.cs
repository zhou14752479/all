using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

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
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
            //string a1 = "测试";
            //string sql = "INSERT INTO tels (tel) VALUES( '" + a1 + "')";
            //insertdata(sql);


        }

        public void run()
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
            status = false;
        }
    }
}
