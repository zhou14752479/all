using MySql.Data.MySqlClient;
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

namespace fang.临时软件
{
    public partial class IC交易网 : Form
    {
        public IC交易网()
        {
            InitializeComponent();
        }

        bool zanting = true;
        bool status = true;

        #region 获取网址

        public static void getUrls(ListBox cob)
        {
            ArrayList list = new ArrayList();
            try
            {
                string constr = "Host =116.62.62.62;Database=vip;Username=root;Password=zhoukaige";
                string str = "SELECT url from jiaoyu ";
                MySqlDataAdapter da = new MySqlDataAdapter(str, constr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    //string html = method.GetHtmlSource(dr[0].ToString().Trim());
                    //Match title = Regex.Match(html, @"<h1>([\s\S]*?)</h1>");

                    //list.Add(title.Groups[1].Value);
                    list.Add(dr[0].ToString().Trim());
                }
            }
            catch (MySqlException ee)
            {
                ee.Message.ToString();
            }
            cob.DataSource = list;

        }
        #endregion
        /// <summary>
        /// 获取第二列
        /// </summary>
        /// <returns></returns>
        public ArrayList getListviewValue1(ListView listview,int a)

        {
            ArrayList values = new ArrayList();

            for (int i = 0; i < listview.Items.Count; i++)
            {
                ListViewItem item = listview.Items[i];

                values.Add(item.SubItems[a].Text);


            }

            return values;

        }

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            try
            {


                string COOKIE = "ICNet[Domain]=ED4p2yQItgBdokn41qREbpyAlAO%2FUbpi%2Fd0uJSFbP2U; PHPSESSID=o4md2j6nkqnm959da1c29g4hh6; Hm_lvt_01dd801871f8a184f89f8e6863636c12=1552701572; ICNet[Guest]=B%2B1%2BwMrrYGwwIpGk%2BWHN9TNBMkgP5Qc36uH4BVatcDY; ICNet[CompanyId]=B%2B1%2BwMrrYGwwIpGk%2BWHN9TNBMkgP5Qc36uH4BVatcDY; ICNet[CompanyName]=%E5%AE%8F%E6%B4%BE%E7%A7%91%E6%8A%80; ICNet[MemberType]=uJmmN%2Bu9RkPeUPaX%2BqhfJALHWTRTe%2BzI4RsZZt1oj%2Fo; ICNet[ActiveFlag]=1PHJqa5wGJMAlS8IgRLYiZ0Cehp4WWBUjw1BUeIF5bE; ICNet[MemLevel]=1PHJqa5wGJMAlS8IgRLYiZ0Cehp4WWBUjw1BUeIF5bE; ICNet[AutoLogin]=2yF6LpClDFq8uByePtJwfX0ndutUqh4gNxqmVbE7fpQ; ICNet[LoginType_og]=m5NYsiAXZZrOgIwJ8BmcTCu4Mwxw4YR7ScVCZ8fGTuE; ICNet[ValidDate]=nm34sS0pwcsmGYJEU6AFdKZltDqbot3QB2MhfFbxPnA; ICNet[Version]=L2Ft58bk%2B58QlZRBPSRGZ747%2FQgmEpCmjyNP4VQ0RqQ; ICNet[LoginType]=n8TryyZnbUOYSL%2FBP8wbQ3Ayz0Ek0klgwIqjBjL9WME; ICNet[Collect_Key]=%5B%2250765d1fc192e08360a4ad71975cf3786c448e3a%22%5D; Hm_lpvt_01dd801871f8a184f89f8e6863636c12=1552702363; ICNet[sct]=C5PN8RidAI2QxSwX";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.81 Safari/537.36";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                request.KeepAlive = false;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

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

        #region  主函数
        public void run()

        {
            string str = System.Environment.CurrentDirectory;

            StreamReader sr = new StreamReader(str + "/partIndex.txt", Encoding.Default);
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            ArrayList lists = new ArrayList();
            for (int i = 0; i < text.Length; i++)
            {
                lists.Add(text[i]);
            }
            string[]companys = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            try

            {
                foreach (string company in companys)
                {

                    foreach (string key in lists)
                    {
                        label3.Text = company+""+key;
                        

                        string html = GetUrl("https://" + company + ".ic.net.cn/userHomePage/search.php?key=" + key, "utf-8");

                        MatchCollection xinghaos = Regex.Matches(html, @" id=""partNo"" value=""([\s\S]*?)""");
                        MatchCollection mfgs = Regex.Matches(html, @" id=""mfg"" value=""([\s\S]*?)""");
                        MatchCollection dcs = Regex.Matches(html, @" id=""dc"" value=""([\s\S]*?)""");
                        MatchCollection qtys = Regex.Matches(html, @" id=""qty"" value=""([\s\S]*?)""");
                        MatchCollection packages = Regex.Matches(html, @" id=""pack"" value=""([\s\S]*?)""");
                        MatchCollection des = Regex.Matches(html, @" id=""description"" value=""([\s\S]*?)""");






                        for (int i = 0; i < xinghaos.Count; i++)
                        {
                            ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv2.SubItems.Add(xinghaos[i].Groups[1].Value);
                            lv2.SubItems.Add(mfgs[i].Groups[1].Value);
                            lv2.SubItems.Add(dcs[i].Groups[1].Value);
                            lv2.SubItems.Add(qtys[i].Groups[1].Value);
                            lv2.SubItems.Add(packages[i].Groups[1].Value);
                            lv2.SubItems.Add(des[i].Groups[1].Value);

                            lv2.SubItems.Add(company);
                            lv2.SubItems.Add(key);
                            lv2.SubItems.Add(DateTime.Now.ToString());

                        }





                        if (listView2.Items.Count - 1 > 1)
                        {
                            listView2.EnsureVisible(listView2.Items.Count - 1);
                        }

                        if (status == false)

                        {
                            return;
                        }
                        while (this.zanting == false)
                        {
                            Application.DoEvents();
                        }
                        Thread.Sleep(1000);

                    }
                }
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        private void skinButton2_Click(object sender, EventArgs e)
        {
            status = true;
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void IC交易网_Load(object sender, EventArgs e)
        {
           
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void skinButton5_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

     
        private void skinButton4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView2.Items.Clear();
        }

        private void skinButton7_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
