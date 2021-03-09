using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 主程序202011
{
    public partial class 移动办公抓取 : Form
    {
        public 移动办公抓取()
        {
            InitializeComponent();
        }

        string COOKIE = "";

        /// <summary>
        /// 获取oid
        /// </summary>
        public string getoid()
        {
            string o = "";
            Match uid = Regex.Match(COOKIE, @"userId=([\s\S]*?);");

            string url = "https://web.duanmatong.cn/access/IMLogin/getValidOrgIds";
                string postdata = "{\"uid\":\""+uid.Groups[1].Value+"\",\"orgType\":6}";
                string html = method.PostUrl(url, postdata, COOKIE, "utf-8");

                Match oid = Regex.Match(html, @"validOrgIds"":\[([\s\S]*?)\]");
            if (oid.Groups[1].Value.Contains(","))
            {
                string[] text = oid.Groups[1].Value.Split(new string[] { "," }, StringSplitOptions.None);
                o = text[Convert.ToInt32(textBox1.Text)-1].Trim();
            }
            else
            {
                o = oid.Groups[1].Value.Trim();
            }
            return o;
            }


        /// <summary>
        /// 获取部门ID
        /// </summary>
        public ArrayList getdeptId()
        {
            ArrayList lists = new ArrayList();
            string oid = getoid();

            string url = "https://web.duanmatong.cn/access/Contacts/getDepts";
            string postdata = "{\"updateFlag\":\"\",\"mobile\":\"\",\"orgId\":\""+ oid + "\"}";
            string html = method.PostUrl(url, postdata, COOKIE, "utf-8");

            MatchCollection ids = Regex.Matches(html, @"""id"":([\s\S]*?),");

            for (int i = 0; i <ids.Count; i++)
            {
                lists.Add(ids[i].Groups[1].Value);
            }
            return lists;
        }


        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
          
            string oid = getoid();
            ArrayList lists = getdeptId();
            foreach (string depid in lists)
            {

                for (int page = 0; page < 99999; page = page + 100)
                {
                    string url = "https://web.duanmatong.cn/access/Contacts/getUsersSegment";
                    string postdata = "{\"count\":100,\"mobile\":\"\",\"updateFlag\":,\"deptId\":"+depid+",\"orgId\":" + oid + ",\"startIdx\":" + page + "}";
                    string html = method.PostUrl(url, postdata, COOKIE, "utf-8");

                    MatchCollection names = Regex.Matches(html, @"""name"":""([\s\S]*?)""");
                    MatchCollection mobiles = Regex.Matches(html, @"""mobile"":""([\s\S]*?)""");
                    if (names.Count == 0)
                    {
                        break;
                     
                    }

                    for (int j = 0; j < names.Count; j++)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据


                        lv1.SubItems.Add(names[j].Groups[1].Value);
                        lv1.SubItems.Add(mobiles[j].Groups[1].Value);


                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }



                    }
                    Thread.Sleep(1000);
                }
            }
           MessageBox.Show("抓取结束");


        }

        bool zanting = true;
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            COOKIE = cookieBrowser.cookie;
           
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html","utf-8");

            if (!html.Contains(@"yidongbangong"))
            {
                MessageBox.Show("");
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

        private void button6_Click(object sender, EventArgs e)
        {
            cookieBrowser cb = new cookieBrowser("https://web.duanmatong.cn/#/contact");
            cb.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button2_Click(object sender, EventArgs e)
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

        private void 移动办公抓取_Load(object sender, EventArgs e)
        {

        }
    }
}
