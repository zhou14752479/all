using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace zhaopin_58
{
    public partial class 美团 : Form
    {
        public 美团()
        {
            InitializeComponent();
        }
   
        #region  获取数据库中城市名称对应的ID

        public string GetcId(string city)
        {

            try
            {
                string constr = "Host =47.99.68.92;Database=citys;Username=root;Password=zhoukaige00.@*.";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select cid from province where name='" + city + "'  ", mycon);         //SQL语句读取textbox的值'"+textBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                reader.Read();

                string cid = reader["cid"].ToString().Trim();
                mycon.Close();
                reader.Close();
                return cid;


            }

            catch (System.Exception ex)
            {
                return ex.ToString();
            }


        }

        #endregion

        private void button5_Click(object sender, EventArgs e)
        {
            城市控件 cs = new 城市控件();
            城市控件.checklists.Clear();
            
            cs.Show();
        }
        bool zanting = true;
        bool status = true;
        #region  多个城市

        public void run()
        {


            try
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("请输入关键字");
                    return;
                }

                string[] keywords = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                ArrayList citys = 城市控件.checklists;
                
                foreach (string city in citys)
                {
                  
                    string cityId = GetcId(city);
                    
                    foreach (string keyword in keywords)

                    {


                            string Url = "https://apimobile.meituan.com/group/v4/poi/pcsearch/" + cityId + "?cateId=-1&sort=default&userid=-1&offset=0&limit=1000&mypos=33.959859%2C118.279675&uuid=C693C857695CAE55399A30C25D9D05F8914E58638F1E750BFB40CACC3AD5AE9F&pcentrance=6&q=" + keyword + "&requestType=filter&cityId=" + cityId;

                            string html = method.GetUrl(Url);


                            MatchCollection TitleMatchs = Regex.Matches(html, @"false},{""id"":([\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                            ArrayList lists = new ArrayList();

                            foreach (Match NextMatch in TitleMatchs)
                            {

                                lists.Add(NextMatch.Groups[1].Value);
                            }


                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;
                            foreach (string list in lists)
                            {
                            string strhtml1 = method.GetUrl("http://i.meituan.com/poi/" + list);  //定义的GetRul方法 返回 reader.ReadToEnd()

                            Match name = Regex.Match(strhtml1, @"<h1 class=""dealcard-brand"">([\s\S]*?)</h1>");
                            Match tell = Regex.Match(strhtml1, @"data-tele=""([\s\S]*?)""");
                            Match addr = Regex.Match(strhtml1, @"addr:([\s\S]*?)&");

                            ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                            listViewItem.SubItems.Add(name.Groups[1].Value);
                            listViewItem.SubItems.Add(tell.Groups[1].Value);
                            listViewItem.SubItems.Add(addr.Groups[1].Value);
                            listViewItem.SubItems.Add(city);


                                    if (listView1.Items.Count - 1 > 1)
                                    {
                                        listView1.EnsureVisible(listView1.Items.Count - 1);
                                    }
                                    while (this.zanting == false)
                                    {
                                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                    }

                                    if (status == false)
                                    {
                                        return;
                                    }

                                    Thread.Sleep(1000);   //内容获取间隔，可变量

                                

                            

                        }
                    }
                }
            }





            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            status = true;
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();


        }
        #region 获取公网IP
        public static string GetIP()
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    webClient.Credentials = CredentialCache.DefaultCredentials;
                    byte[] pageDate = webClient.DownloadData("http://pv.sohu.com/cityjson?ie=utf-8");
                    String ip = Encoding.UTF8.GetString(pageDate);
                    webClient.Dispose();

                    Match rebool = Regex.Match(ip, @"\d{2,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
                    return rebool.Value;
                }
                catch (Exception e)
                {
                    return e.ToString();
                }

            }
        }

        #endregion
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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].SubItems[3].Text.Contains("-"))
                {
                    listView1.Items.Remove(listView1.Items[i]);
                }
            }
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].SubItems[3].Text == "")
                {
                    listView1.Items.Remove(listView1.Items[i]);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void 美团_Load(object sender, EventArgs e)
        {

        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            button1.Enabled = true;
        }
    }
}
