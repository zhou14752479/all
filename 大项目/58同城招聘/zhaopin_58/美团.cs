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
                                string strhtml = method.gethtml("https://apimobile.meituan.com/group/v1/poi/" + list + "?fields=areaName,frontImg,name,avgScore,avgPrice,addr,openInfo,wifi,phone,featureMenus,isWaimai,payInfo,chooseSitting,cates,lat,lng");  //定义的GetRul方法 返回 reader.ReadToEnd()                             

                                // string commentHtml = method.gethtml("https://www.meituan.com/meishi/api/poi/getMerchantComment?uuid=f87c45af885944f3a19d.1564549522.1.0.0&platform=1&riskLevel=1&optimusCode=10&id="+list);
                                Match name = Regex.Match(strhtml, @"poiid([\s\S]*?)""name"":""([\s\S]*?)""");
                                Match addr = Regex.Match(strhtml, @"addr"":""([\s\S]*?)""");
                                Match tel = Regex.Match(strhtml, @"phone"":""([\s\S]*?)""");
                                Match areaName = Regex.Match(strhtml, @"areaName"":""([\s\S]*?)""");
                               

                                if (name.Groups[2].Value != "")
                                {
                                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                                    lv1.SubItems.Add(name.Groups[2].Value.Trim());
                                    lv1.SubItems.Add(addr.Groups[1].Value.Trim());
                                    lv1.SubItems.Add(tel.Groups[1].Value.Trim());
                                    lv1.SubItems.Add(areaName.Groups[1].Value.Trim());
                                    lv1.SubItems.Add(city);
                                 

                                    if (strhtml.Contains("有外卖"))
                                    {
                                        lv1.SubItems.Add("有外卖");
                                    }
                                    else
                                    {
                                        lv1.SubItems.Add("无外卖");
                                    }


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
            }





            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
           

            status = true;

            #region 通用登录

            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php");
            string localip = GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == localip.Trim())
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {
                //--------登陆函数------------------
                Thread thread = new Thread(new ThreadStart(run));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();

            }
            else
            {
                MessageBox.Show("请登录您的账号！登陆成功返回软件使用即可");
                System.Diagnostics.Process.Start("http://www.acaiji.com");
                return;
            }
            #endregion

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
    }
}
