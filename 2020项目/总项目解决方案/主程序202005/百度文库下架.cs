using System;
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
using helper;
namespace 主程序202005
{
    public partial class 百度文库下架 : Form
    {
        public 百度文库下架()
        {
            InitializeComponent();
        }
        /// <summary>
        /// VIP专享
        /// </summary>
        public void vipdocs()
        {
            string st = "7";


            try
            {
                for (int i = Convert.ToInt32(textBox1.Text.Trim()); i <= Convert.ToInt32(textBox2.Text.Trim()); i++)
                {
                    textBox3.Text += "正在筛选第" + i + "页" + "\r\n";
                    int pn = (i - 1) * 20;
                    string url = "https://wenku.baidu.com/nduc/api/user/interface/getcontribution?st=" + st + "&pn=" + pn;

                    string html = GetUrl(url);

                    MatchCollection ids = Regex.Matches(html, @"""doc_id"":""([\s\S]*?)""");
                    Match token = Regex.Match(html, @"""new_token"":""([\s\S]*?)""");
                    MatchCollection titles = Regex.Matches(html, @"""title"":""([\s\S]*?)""");

                    this.token = token.Groups[1].Value;
                    for (int j = 0; j < ids.Count; j++)
                    {
                        try
                        {


                            if (checkBox1.Checked == true)
                            {
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add(ids[j].Groups[1].Value);

                                lv1.SubItems.Add(titles[j].Groups[1].Value);
                            }
                            else
                            {
                                if (panduan(titles[j].Groups[1].Value))
                                {
                                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                    lv1.SubItems.Add(ids[j].Groups[1].Value);

                                    lv1.SubItems.Add(titles[j].Groups[1].Value);

                                }
                            }
                        }
                        catch
                        {

                            continue;
                        }




                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                    }
                }

                MessageBox.Show("筛选结束");

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// 付费文档
        /// </summary>
        public void getfufeidocs()
        {



            try
            {
                for (int i = Convert.ToInt32(textBox1.Text.Trim()); i <= Convert.ToInt32(textBox2.Text.Trim()); i++)
                {
                    textBox3.Text += "正在筛选第" + i + "页" + "\r\n";
                    int pn = i-1;
                    string url = "https://wenku.baidu.com/user/interface/shopgetdoclist?sub_tab=2&pn="+pn+"&rn=10";

                    string html = GetUrl(url);

                    MatchCollection ids = Regex.Matches(html, @"""doc_id"":""([\s\S]*?)""");
                    Match token = Regex.Match(html, @"""token"":""([\s\S]*?)""");
                    MatchCollection titles = Regex.Matches(html, @"""title"":""([\s\S]*?)""");
                    this.token = token.Groups[1].Value;
                    
                    for (int j = 0; j < ids.Count; j++)
                    {

                        try
                        {


                            if (checkBox1.Checked == true)
                            {
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                lv1.SubItems.Add(ids[j].Groups[1].Value);

                                lv1.SubItems.Add(Unicode2String(titles[j].Groups[1].Value));
                            }

                            else
                            {

                                if (panduan(Unicode2String(titles[j].Groups[1].Value)))
                                {
                                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                    lv1.SubItems.Add(ids[j].Groups[1].Value);

                                    lv1.SubItems.Add(Unicode2String(titles[j].Groups[1].Value));

                                }
                            }
                        }
                        catch
                        {

                            continue;
                        }

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                    }
                }

                MessageBox.Show("筛选结束");

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        static string COOKIE = "murmur=undefined--Win32; BIDUPSID=A71B424CE88935CF0B85D5E3FE18C45D; PSTM=1589341639; BAIDUID=A71B424CE88935CFBE40D6274B822189:FG=1; BDORZ=B490B5EBF6F3CD402E515D22BCDA1598; _click_param_pc_rec_doc_2017_testid=1; _click_param_reader_query_ab=-1; BDUSS=nlLflBzZkdab0JKNnNoMmppSEtmbUtHUklLVlR-clI1Qmd6akUyOUIzVFR-dTllSVFBQUFBJCQAAAAAAAAAAAEAAADOMhPS1tDQodGnvqvGt73M0~0AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAANNxyF7TccheS; Hm_lvt_d8bfb560f8d03bbefc9bdecafc4a4bf6=1590134983,1590194411,1590221054,1590282665; isJiaoyuVip=1; H_PS_PSSID=1454_31670_21079_31590_31605_31271_31464_31322_30823_22157; delPer=0; PSINO=5; upLayerDoc=true; Hm_lpvt_d8bfb560f8d03bbefc9bdecafc4a4bf6=1590283476";

        string token = "";
  
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

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Referer = "https://ishare.iask.sina.com.cn/ucenter/myuploads?subPage=a&pindex=2";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";

                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("sec-fetch-mode:navigate");
                headers.Add("sec-fetch-site:same-origin");
                headers.Add("sec-fetch-user:?1");
                headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                // request.KeepAlive = true;
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
        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }
        public bool panduan(string title)

        {

            string[] text = textBox8.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != "")
                {
                    if (title.Contains(text[i]))
                    {
                        return true;
                    }
                }

            }
            return false;

        }
        bool zanting = true;

        public bool deletedocs(string doc)
        {

            try
            {
                string url = "https://wenku.baidu.com/user/interface/shopchangedocstatus?operation=2&doc_id=" + doc + "&token=" + token + "&new_token=" + token ;

                
                string html = GetUrl(url);
                
             
                if (html.Contains("code\":0"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {

                return false;
            }
        }
        public void del()
        {
            textBox3.Text = "";
            for (int i = 0; i < listView1.CheckedItems.Count; i++)
            {
                string id = listView1.CheckedItems[i].SubItems[1].Text;


                textBox3.Text = listView1.CheckedItems[i].SubItems[2].Text + "：下架状态" + deletedocs(id) + "\r\n";
                Thread.Sleep(Convert.ToInt32(textBox4.Text) * 1000);
            }
            MessageBox.Show("下架结束");
        }
        private void 百度文库下架_Load(object sender, EventArgs e)
        {
            webBrowser1.Url = new Uri("https://passport.baidu.com/v2/?login&tpl=do&u=https%3A%2F%2Fwenku.baidu.com/nduc/browse/uc?_page=home&_redirect=1#/mydocsupload");
            webBrowser1.ScriptErrorsSuppressed = true;
            method.SetWebBrowserFeatures(method.IeVersion.IE10);
        }

        private void button1_Click(object sender, EventArgs e)
        {
           // COOKIE = "BIDUPSID=A71B424CE88935CF0B85D5E3FE18C45D; PSTM=1589341639; BAIDUID=A71B424CE88935CFBE40D6274B822189:FG=1; BDORZ=B490B5EBF6F3CD402E515D22BCDA1598; _click_param_pc_rec_doc_2017_testid=1; _click_param_reader_query_ab=-1; selfpgcshare=5; pgcshare=9; BDUSS_BFESS=DBENW9vUGVSN0pxM1VCWlBzMX5iZTlhWlpkfkYyOTNsQjJsNVd6bERmUH5nZ1pmSVFBQUFBJCQAAAAAAAAAAAEAAACys-e7cTg1MjI2NjAxMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP~13l7~9d5ea; Hm_lvt_f06186a102b11eb5f7bcfeeab8d86b34=1591353475,1591747020,1591776392,1591865843; layer_show_times_total_1_6831f9420be1265b2aefc54f5280d345=14; layer_show_times_total_3_6831f9420be1265b2aefc54f5280d345=3; isJiaoyuVip=1; murmur=undefined--Win32; Hm_lvt_d8bfb560f8d03bbefc9bdecafc4a4bf6=1592101407,1592109362,1592115500,1592122643; session_name=; H_PS_PSSID=31906_1454_31670_21079_32045_31781_31322_30823_31845_22157; delPer=0; PSINO=5; ZD_ENTRY=baidu; session_id=1592123902383; Hm_lpvt_d8bfb560f8d03bbefc9bdecafc4a4bf6=1592124925; BDUSS=FOUy1kYlJtWDhrN3lxV3Q5RDZhQy1xNUZaN1ZLSFlJeTgtMXRhSG1iY3BjdzFmSVFBQUFBJCQAAAAAAAAAAAEAAADA6eooeHhqODAxMTI0MDEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACnm5V4p5uVeNE";
            
            if (radioButton3.Checked == true)
            {
                textBox3.Text = "";
               COOKIE = method.GetCookies("https://wenku.baidu.com/nduc/browse/uc?_page=home&_redirect=1#/mydocsupload");

                Thread thread = new Thread(new ThreadStart(vipdocs));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            else 
            {
                textBox3.Text = "";

               COOKIE = method.GetCookies("https://wenku.baidu.com/nduc/browse/uc?_page=home&_redirect=1#/mydocsupload");


                Thread thread = new Thread(new ThreadStart(getfufeidocs));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                StreamReader streamReader = new StreamReader(this.openFileDialog1.FileName, Encoding.Default);
                string text = streamReader.ReadToEnd();
                string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < array.Length; i++)
                {
                    textBox8.Text += array[i] + "\r\n";

                }

            }
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = true;
            }
        }

        private void 取消全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 百度文库下架_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            zanting = true;
        }
        #region listview转datable
        /// <summary>
        /// listview转datable
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        public static DataTable listViewToDataTable(ListView lv)
        {
            int i, j;
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Clear();
            dt.Columns.Clear();
            //生成DataTable列头
            for (i = 0; i < lv.Columns.Count; i++)
            {
                dt.Columns.Add(lv.Columns[i].Text.Trim(), typeof(String));
            }
            //每行内容
            for (i = 0; i < lv.CheckedItems.Count; i++)
            {
                dr = dt.NewRow();
                for (j = 0; j < lv.Columns.Count; j++)
                {
                    try
                    {
                        dr[j] = lv.CheckedItems[i].SubItems[j].Text.Trim();

                    }
                    catch
                    {

                        continue;
                    }

                }
                dt.Rows.Add(dr);
            }

            return dt;
        }
        #endregion
        private void button6_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(del));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
