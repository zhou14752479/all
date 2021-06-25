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

namespace 主程序202105
{
    public partial class 贴吧用户发言查找 : Form
    {
        public 贴吧用户发言查找()
        {
            InitializeComponent();
        }

        ArrayList lists = new ArrayList();
        ArrayList infos = new ArrayList();
        public void run()
        {
            infos.Add("");
            try
            {
                StreamReader sr = new StreamReader(textBox1.Text.Trim(), method.EncodingType.GetTxtType(textBox1.Text.Trim()));
                //一次性读取完 
                string texts = sr.ReadToEnd();

                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {
                    label2.Text = "正在查询："+i;
                    if (!lists.Contains(text[i]))
                    {
                        lists.Add(text[i]);
                        string url = "https://tieba.baidu.com/f/search/ures?kw=&qw=&rn=1000&un=" + System.Web.HttpUtility.UrlEncode(text[i], Encoding.GetEncoding("GB2312")) + "&only_thread=&sm=1&sd=&ed=&pn=1";

                        string html = method.GetUrl(url, "gb2312");
                        MatchCollection divs = Regex.Matches(html, @"<div class=""p_content"">([\s\S]*?)</div>");

                        foreach (Match div in divs)
                        {
                            string QQ = "";
                            string mail = Regex.Match(div.Groups[1].Value, @"[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*").Groups[0].Value;

                            if (div.Groups[1].Value.Contains("QQ") || div.Groups[1].Value.Contains("qq"))
                            {
                                QQ = Regex.Match(div.Groups[1].Value, @"[1-9][0-9]{7,11}").Groups[0].Value;


                            }

                            string phone = Regex.Match(div.Groups[1].Value, @"1\d{10}").Groups[0].Value;

                            if (!infos.Contains(QQ) && QQ!=phone && QQ.Trim() != "")
                            {
                                infos.Add(QQ);
                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(text[i]);
                                lv1.SubItems.Add(div.Groups[1].Value);
                                lv1.SubItems.Add(QQ);
                            }

                            if (!infos.Contains(mail) && mail.Trim()!="")
                            {
                                infos.Add(mail);
                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(text[i]);
                                lv1.SubItems.Add(div.Groups[1].Value);
                                lv1.SubItems.Add(mail.Replace("~",""));
                                
                            }
                            if (!infos.Contains(phone))
                            {
                                infos.Add(mail);
                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(text[i]);
                                lv1.SubItems.Add(div.Groups[1].Value);
                                lv1.SubItems.Add(phone);
                            }
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)
                                return;


                        }
                      
                    }

                }
                sr.Close();
               
                label2.Text = "【已完成】";

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Filter = "txt|*.txt";
            sfd.Title = "txt文件导出";


            if (sfd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = sfd.FileName;
            }
        }
      
        bool zanting = true;
        bool status = true;

        private void button6_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com:8080/api/vip.html", "utf-8");

            if (!html.Contains(@"XTAeic"))
            {

                return;
            }



            #endregion
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入用户TXT");
                return;
            }

            for (int i = 0; i <Convert.ToInt32(textBox2.Text); i++)
            {
                Thread thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
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



        #region  listview导出文本TXT
        public static void ListviewToTxt(ListView listview)
        {
            if (listview.Items.Count == 0)
            {
                MessageBox.Show("列表为空!");
            }
            else
            {
                List<string> list = new List<string>();
                foreach (ListViewItem item in listview.Items)
                {

                    list.Add(item.SubItems[1].Text+"----"+ item.SubItems[3].Text);


                }
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "txt|*.txt";
                sfd.Title = "txt文件导出";
                string fileName = "";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    fileName = sfd.FileName;
                    StringBuilder sb = new StringBuilder();
                    foreach (string tel in list)
                    {
                        sb.AppendLine(tel);
                    }
                    System.IO.File.WriteAllText(fileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("文件导出成功!");
                }
            }
        }





        #endregion

        #region  listview导出文本TXT
        public static void ListviewToTxt1(ListView listview)
        {
            if (listview.Items.Count == 0)
            {
                MessageBox.Show("列表为空!");
            }
            else
            {
                List<string> list = new List<string>();
                foreach (ListViewItem item in listview.Items)
                {

                    list.Add(item.SubItems[1].Text + "----"+ item.SubItems[2].Text+"----" + item.SubItems[3].Text);


                }
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "txt|*.txt";
                sfd.Title = "txt文件导出";
                string fileName = "";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    fileName = sfd.FileName;
                    StringBuilder sb = new StringBuilder();
                    foreach (string tel in list)
                    {
                        sb.AppendLine(tel);
                    }
                    System.IO.File.WriteAllText(fileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("文件导出成功!");
                }
            }
        }





        #endregion
        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                ListviewToTxt1(listView1);
            }
            if (radioButton2.Checked == true)
            {
                ListviewToTxt(listView1);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 贴吧用户发言查找_Load(object sender, EventArgs e)
        {

        }
    }
}
