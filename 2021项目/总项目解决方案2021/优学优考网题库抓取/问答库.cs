using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 优学优考网题库抓取
{
    public partial class 问答库 : Form
    {
        public 问答库()
        {
            InitializeComponent();
        }




        public void getcates()
        {
            string url = "https://www.asklib.com/";
            string html = yxykw.GetUrl(url,"utf-8","");
            MatchCollection cates = Regex.Matches(html, @"<a href=""/([\s\S]*?)\.html");
            for (int i = 0; i < cates.Count; i++)
            {
                if (!catelist.Contains(cates[i].Groups[1].Value))
                {
                    catelist.Add(cates[i].Groups[1].Value);
                   textBox4.Text+= cates[i].Groups[1].Value+"\r\n";
                }

            }
        }



        优学优考网题库抓取 yxykw = new 优学优考网题库抓取();

        List<string> catelist = new List<string>();
        public void run()
        {

         

            string[] catelist = textBox4.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            try
            {

                foreach (string cate in catelist)
                {
                    textBox7.Text += cate + "\r\n";
                    for (int i = 1; i < 9999; i++)
                    {


                        //kuaiji/chuji
                        //kuaiji/congye

                        string url = "https://www.asklib.com/"+cate+"/p" + i + ".html";
                        string html = yxykw.GetUrl(url, "utf-8","");
                        MatchCollection uids = Regex.Matches(html, @"<a href=""/view/([\s\S]*?)\.html");
                        if (uids.Count == 0)
                        {
                            label8.Text = "当前类目抓取结束";
                            break;
                        }

                        foreach (Match uid in uids)
                        {
                            if (astatus == false)
                                return;
                            string aurl = "https://www.asklib.com/view/" + uid.Groups[1].Value + ".html";
                            string ahtml = yxykw.GetUrl(aurl, "utf-8",textBox8.Text);
                            string title = Regex.Match(ahtml, @"<title>([\s\S]*?)</title>").Groups[1].Value;
                            string qustion = Regex.Match(ahtml, @"<div class=""essaytitle txt_l "">([\s\S]*?)</div>").Groups[1].Value;
                            string anwser = Regex.Match(ahtml, @"<div class=""listbg"">([\s\S]*?)</div>").Groups[1].Value;
                            string jiexi = Regex.Match(ahtml, @"<div style="""">([\s\S]*?)</div>").Groups[1].Value;

                            title = title.Replace("-", "").Replace("_", "").Replace("问答库", "");
                            anwser = anwser + jiexi;


                            if (anwser.Contains("showlogin"))
                            {
                                MessageBox.Show("答案为空，请检查账号是否到期");
                                label8.Text = "已暂停";
                                zanting = false ;
                            }
                            if (title=="")
                            {
                                MessageBox.Show("标题为空");
                                label8.Text = "已暂停";
                                zanting = false;
                            }
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            string status = yxykw.fabu(System.Web.HttpUtility.UrlEncode(title), System.Web.HttpUtility.UrlEncode(qustion), System.Web.HttpUtility.UrlEncode(anwser));
                            if (status == "成功")
                            {
                                ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                listViewItem.SubItems.Add(title);
                                listViewItem.SubItems.Add(status);
                            }
                            else
                            {
                                ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                listViewItem.SubItems.Add(title);
                                listViewItem.SubItems.Add(status);
                                label8.Text = "已暂停";
                                zanting = false;
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        string cookie = "PHPSESSID=al7re7smf6jqm8fo8iuoub0gfb; admin_lang=cn; home_lang=cn; ENV_GOBACK_URL=%2Flogin.php%3Fm%3Dadmin%26c%3DArticle%26a%3Dindex%26typeid%3D86%26lang%3Dcn; ENV_LIST_URL=%2Flogin.php%3Fm%3Dadmin%26c%3DArticle%26a%3Dindex%26lang%3Dcn; workspaceParam=welcome%7CIndex; users_id=1";
        Thread thread;
        bool zanting = true;

        bool astatus = true;


        public void wendaka_login()
        {

            string html = 优学优考网题库抓取.PostUrlDefault("https://www.asklib.com/user.php?act=logincheck", "username=" + textBox5.Text + "&password=" + textBox6.Text, yxykw.zhuaqucookie);
            MessageBox.Show(html);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = yxykw.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8","");

            if (!html.Contains(@"uoQUpu"))
            {

                return;
            }



            #endregion

            if (yxykw.zhuaqucookie == "" || yxykw.fabucookie == "")
            {
                MessageBox.Show("请先登录");
                return;
            }

            astatus = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = yxykw.UrlToBitmap("http://tiku.ucms.club/login.php?m=admin&c=Admin&a=vertify&lang=cn");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string html = 优学优考网题库抓取.PostUrlDefault("http://tiku.ucms.club/login.php?m=admin&c=Admin&a=login&_ajax=1&lang=cn&t=0.9288685268426105", "user_name=" + textBox1.Text.Trim() + "&password=" + textBox2.Text.Trim() + "&vertify=" + textBox3.Text.Trim(), cookie);
            string msg = Regex.Match(html, @"""msg"":""([\s\S]*?)""").Groups[1].Value;
            yxykw.fabucookie = cookie;
            MessageBox.Show(msg);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            wendaka_login();
        }

        private void 问答库_Load(object sender, EventArgs e)
        {
            getcates();
            yxykw.zhuaqucookie = "__gads=ID=6d8984edadb87ce5-22c6ac40aecc0043:T=1634722642:RT=1634722642:S=ALNI_MbWCx0AUM4fEtbiaPH5yT7KknL3sQ; PHPSESSID=t6646e3bqt8qf69oqm7vlfbpe6; Hm_lvt_82cf154d3fe0d815847ee595230f01c1=1635558080,1635576410; Hm_lpvt_82cf154d3fe0d815847ee595230f01c1=1635576778";

          
            pictureBox1.Image = yxykw.UrlToBitmap("http://tiku.ucms.club/login.php?m=admin&c=Admin&a=vertify&lang=cn");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            astatus = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 问答库_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }
    }
}
