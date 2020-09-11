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
using helper;

namespace 主程序202009
{
    public partial class 币乎查找 : Form
    {
        public 币乎查找()
        {
            InitializeComponent();
        }
        string path = AppDomain.CurrentDomain.BaseDirectory + "\\data\\";
       


        
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
                if (articleIds.Count == 0)
                    break;
                foreach (Match articleId in articleIds)
                {
                    lists.Add(articleId.Groups[1].Value);
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
        public void run(object aaid)
        {
            string aid = aaid.ToString();
            ArrayList bids = getbids(aid);
            for (int i = 0; i < bids.Count; i++)
            {
                ArrayList articleids = getarticleIds(bids[i].ToString());

                for (int j = 0; j < articleids.Count; j++)
                {
                    ArrayList commentPeopleIds= getcommentPeopleIds(articleids[j].ToString());



                    foreach (string commentPeopleId in commentPeopleIds)
                    {
                        string[] text = commentPeopleId.Split(new string[] { "#" }, StringSplitOptions.None);

                        if (text.Length > 2)
                        {
                            if (text[0].Trim() == aid)
                            {
                                ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv2.SubItems.Add(aid);
                                lv2.SubItems.Add(text[0]);
                                lv2.SubItems.Add(ConvertStringToDateTime(text[1]).ToString());
                                lv2.SubItems.Add(articleids[j].ToString());
                                lv2.SubItems.Add(text[2]);

                            }
                            else
                            {

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
           
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(run));
                string o = listView1.Items[i].SubItems[1].Text;
                thread.Start((object)o);
                Control.CheckForIllegalCrossThreadCalls = false;
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
            method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
        }
    }
}
