using System;
using System.Collections;
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
using NPOI.HSSF.UserModel;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;

namespace 主程序202009
{
    public partial class 币乎查找 : Form
    {
        public 币乎查找()
        {
            InitializeComponent();
        }
        string path = AppDomain.CurrentDomain.BaseDirectory + "\\data\\";

       
        /// <summary>
        /// 获取用户名
        /// </summary>
        /// <returns></returns>
        public string getName(string id)
        {
           
            string url = "https://gw.bihu.com/api/content/author/"+id+"/list?type=ARTICLE";
            
            string html = method.GetUrl(url,"utf-8");
            Match userName = Regex.Match(html, @"""nickname"":""([\s\S]*?)""");
            return userName.Groups[1].Value;
           
        }


        private void 币乎查找_Load(object sender, EventArgs e)
        {
            ArrayList lists = new ArrayList();


            DirectoryInfo folder = new DirectoryInfo(path);
            for (int i = 0; i < folder.GetFiles("*.txt").Count(); i++)
            {
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                lv1.SubItems.Add(folder.GetFiles("*.txt")[i].Name.Replace(".txt",""));
              
            }
            
           
        }


        /// <summary>
        /// 获取B id
        /// </summary>
        /// <returns></returns>
        public ArrayList getbids(string aid)
        {
           
            ArrayList lists = new ArrayList();
            StreamReader sr = new StreamReader(path + aid.Trim() + ".txt", Encoding.GetEncoding("utf-8"));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);



            for (int i = 0; i < text.Length; i++)
            {
                lists.Add(text[i]);
                
            }
            sr.Close();
            return lists;
        }


        /// <summary>
        /// 获取文章ID
        /// </summary>
        /// <returns></returns>
        public ArrayList getarticleIds(string bid)
        {
            ArrayList lists = new ArrayList();
            for (int i = 1; i < 999; i++)
            {
                string url = "https://gw.bihu.com/api/content/author/"+bid.Trim()+"/list?type=ARTICLE&pageNum="+i;

                string html = method.GetUrl(url,"utf-8");
                MatchCollection articleIds = Regex.Matches(html, @"content"":{""id"":([\s\S]*?),");
                MatchCollection articleTitles = Regex.Matches(html, @"""title"":""([\s\S]*?)""");
                if (articleIds.Count == 0)
                    break;
                for (int j = 0; j < articleIds.Count; j++)
                {

                    lists.Add(articleIds[j].Groups[1].Value+"#"+ articleTitles[j].Groups[1].Value);
                }
            }

            return lists;
        }

        private DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long mTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(mTime);
            return startTime.Add(toNow);

        }


        /// <summary>
        /// 获取评论人ID
        /// </summary>
        /// <returns></returns>
        public ArrayList getcommentPeopleIds(string articleid)
        {
            ArrayList lists = new ArrayList();
            for (int i = 1; i < 999; i++)
            {
                string url = "https://gw.bihu.com/api/comments/"+articleid+"/list?pageNum="+i;

                string html = method.GetUrl(url, "utf-8");
                MatchCollection commentPeopleIds = Regex.Matches(html, @"{""id"":([\s\S]*?),");
                MatchCollection times = Regex.Matches(html, @"""createTime"":([\s\S]*?),");
                MatchCollection contents = Regex.Matches(html, @"""content"":""([\s\S]*?)""");
                if (commentPeopleIds.Count == 0)
                    break;
                for (int j = 0; j < commentPeopleIds.Count; j++)
                {
                    lists.Add(commentPeopleIds[j].Groups[1].Value+"#"+ times[j].Groups[1].Value+ "#"+ contents[j].Groups[1].Value);
                }
            }

            return lists;
        }

        bool zanting = true;
        public void run(object ob)
        {
            ListView listview = (ListView)ob;
           
            string aid = listview.Name.ToString().Trim();
         
            string aname = getName(aid);  //获取a昵称

            ArrayList bids = getbids(aid);
            for (int i = 0; i < bids.Count; i++)
            {
                string bname = getName(bids[i].ToString());  //获取b昵称

                ArrayList articleids = getarticleIds(bids[i].ToString());

                for (int j = 0; j < articleids.Count; j++)
                {
                    string[] articletext = articleids[j].ToString().Split(new string[] { "#" }, StringSplitOptions.None);

                    ArrayList commentPeopleIds= getcommentPeopleIds(articletext[0]);



                    foreach (string commentPeopleId in commentPeopleIds)
                    {
                        string[] text = commentPeopleId.Split(new string[] { "#" }, StringSplitOptions.None);

                        if (text.Length > 2)
                        {
                            if (text[0].Trim() == aid)
                            {
                                ListViewItem lv = listview.Items.Add((listview.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv.SubItems.Add(aname);
                                lv.SubItems.Add(bname);
                                lv.SubItems.Add(ConvertStringToDateTime(text[1]).ToString());
                                lv.SubItems.Add(articletext[1]);
                                lv.SubItems.Add(text[2]);

                            }
                            else
                            {
                                ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv2.SubItems.Add(aname);
                                lv2.SubItems.Add(bname);
                                lv2.SubItems.Add(ConvertStringToDateTime(text[1]).ToString());
                                lv2.SubItems.Add(articletext[1]);
                                lv2.SubItems.Add(text[2]);
                            }

                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                        }
                    }
                }
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
          

            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"bihuchazhao"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion

            timer1.Interval = Convert.ToInt32(textBox1.Text);
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Checked == true)
                {
                    TabPage Page = new TabPage();

                    Page.Text = "符合数据" + (i + 1);
                    tabControl1.Controls.Add(Page);
                    ListView listview = new ListView();
                    listview.Name = listView1.Items[i].SubItems[1].Text.Trim();
                    listview.Dock = DockStyle.Fill;
                    listview.Parent = Page;
                    listview.View = View.Details;
                    listview.GridLines = true;
                    listview.Columns.Add("序号");
                    listview.Columns.Add("用户A");
                    listview.Columns.Add("用户B");
                    listview.Columns.Add("时间");
                    listview.Columns.Add("文章标题");
                    listview.Columns.Add("互动内容");
                    listview.Columns[4].Width = 200;
                    listview.Columns[5].Width = 200;

                    Thread thread = new Thread(new ParameterizedThreadStart(run));
                    object o = listview;
                    thread.Start((object)o);
                    Control.CheckForIllegalCrossThreadCalls = false;

                    //Thread thread = new Thread(new ParameterizedThreadStart(run));
                    //string o = listView1.Items[i].SubItems[1].Text;
                    //thread.Start((object)o);
                    //Control.CheckForIllegalCrossThreadCalls = false;
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
            foreach (Control page in tabControl1.Controls)
            {
                foreach (Control item in page.Controls)
                {
                    if (item is ListView && item.Name == listView1.CheckedItems[0].SubItems[1].Text)
                    {

                        method.DataTableToExcel(method.listViewToDataTable((ListView)item), "Sheet1", true);
                    }
                }
            }
            }
            

        private void timer1_Tick(object sender, EventArgs e)
        {
            listView2.Items.Clear();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            
        }
    }
}
