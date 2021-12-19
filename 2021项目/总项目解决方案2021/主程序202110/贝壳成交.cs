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
using myDLL;

namespace 主程序202110
{
    public partial class 贝壳成交 : Form
    {
        public 贝壳成交()
        {
            InitializeComponent();
        }
        bool zanting = true;
        Thread thread;
        bool status = true;
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            string html = "";
            string COOKIE = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                //添加头部
                WebHeaderCollection headers = request.Headers;
                headers.Add("Authorization:MjAxODAxMTFfaW9zOjljNjQ4OGJkNzc2MzUxZWMyY2Q0ZDI2YTQ5YjViYjUyMjRiMjZhMzU=");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }

                response.Close();
                return html;



            }
            catch (System.Exception ex)
            {
                return ex.ToString();

            }



        }
        #endregion
        #region 贝壳成交
        public void run()
        {

          
           
            try
            {

                for (int a= 0; a< richTextBox1.Lines.Length; a++)
                {
                    string keyword = richTextBox1.Lines[a].Trim();
                    if(keyword=="")
                    {
                        continue;
                    }
  

                    for (int page = 0; page < 10001; page = page + 100)
                    {

                        string url = "https://app.api.ke.com/Rentplat/v1/rented/list?city_id=310000&condition=rs" + System.Web.HttpUtility.UrlEncode(keyword) + "rs" + System.Web.HttpUtility.UrlEncode(keyword) + "&limit=100&offset=" + page + "&request_ts=1638441809&sign_days=";
                        string html = GetUrl(url, "utf-8");
                        html = method.Unicode2String(html);
                        MatchCollection house_codes = Regex.Matches(html, @"""house_code"":""([\s\S]*?)""");
                        MatchCollection sub_desc = Regex.Matches(html, @"""sub_desc"":""([\s\S]*?)""");
                        MatchCollection house_title = Regex.Matches(html, @"""house_title"":""([\s\S]*?)""");
                      
                        MatchCollection rent_price_trans = Regex.Matches(html, @"""rent_price_trans"":([\s\S]*?),");
                        MatchCollection sign_time = Regex.Matches(html, @"""sign_time"":""([\s\S]*?)""");
                        MatchCollection im_title = Regex.Matches(html, @"""im_title"":""([\s\S]*?)""");
                        if (house_codes.Count == 0)
                            break;

                        for (int i = 0; i < house_codes.Count; i++)
                        {
                            try
                            {
                                string jiagequjian = "0";
                                string xingshi = "";
                                try
                                {
                                    double a1 = Convert.ToDouble(rent_price_trans[i].Groups[1].Value) / 1000;

                                    jiagequjian = Math.Floor(a1) * 1000 + "-" + Math.Ceiling(a1) * 1000;

                                    
                                }
                                catch (Exception)
                                {

                                   ;
                                }


                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(house_codes[i].Groups[1].Value);
                               
                                string[] sub_descs = sub_desc[i].Groups[1].Value.Split(new string[] { "·" }, StringSplitOptions.None);
                                try
                                {
                                    lv1.SubItems.Add(sub_descs[0]);
                                    lv1.SubItems.Add(sub_descs[1]);
                                }
                                catch (Exception)
                                {

                                    lv1.SubItems.Add(sub_desc[i].Groups[1].Value);
                                    lv1.SubItems.Add(sub_desc[i].Groups[1].Value);
                                }
                                
                                string[] house_titles = house_title[i].Groups[1].Value.Split(new string[] { "·" }, StringSplitOptions.None);


                                try
                                {
                                    lv1.SubItems.Add(house_titles[1]);
                                    lv1.SubItems.Add(house_titles[0]);
                                }
                                catch (Exception)
                                {

                                    lv1.SubItems.Add(house_title[i].Groups[1].Value);
                                    lv1.SubItems.Add(house_title[i].Groups[1].Value);
                                }
                             
                                lv1.SubItems.Add(jiagequjian);
                                lv1.SubItems.Add(rent_price_trans[i].Groups[1].Value);
                                lv1.SubItems.Add(sign_time[i].Groups[1].Value);
                                lv1.SubItems.Add(im_title[i].Groups[1].Value);
                                string[] im_titles = im_title[i].Groups[1].Value.Split(new string[] { "，" }, StringSplitOptions.None);
                                try
                                {
                                    lv1.SubItems.Add(im_titles[1]);
                                    lv1.SubItems.Add(im_titles[2]);
                                }
                                catch (Exception)
                                {

                                    lv1.SubItems.Add(im_title[i].Groups[1].Value);
                                    lv1.SubItems.Add(im_title[i].Groups[1].Value);
                                }
                               
                                lv1.SubItems.Add("楼层");
                                lv1.SubItems.Add("标签");

                                if (status == false)
                                    return;
                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }

                                label1.Text = DateTime.Now.ToShortTimeString() + "正在查询：" + keyword;
                            }
                            catch (Exception ex)
                            {
                               // MessageBox.Show(ex.ToString());
                                continue;
                            }
                           
                        }
                     
                    }
                    Thread.Sleep(100);

                }



            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }

        }

        #endregion


        #region 贝壳详情
        public void run2()
        {


            DataTable dt = method.ExcelToDataTable(textBox1.Text, false);
            try
            {

                for (int a = 1; a < dt.Rows.Count; a++)
                {
                    string keyword = dt.Rows[a][1].ToString().Trim();
                    if (keyword == "")
                    {
                        continue;
                    }
                        string url = "https://m.ke.com/chuzu/sh/zufang/"+keyword+".html";
                        string html = GetUrl(url, "utf-8");
                      
                  string louceng = Regex.Match(html, @"楼层：</label>([\s\S]*?)</span>").Groups[1].Value;
                    string tags = Regex.Match(html, @"<p class=""content__item__tag--wrapper"">([\s\S]*?)</p>").Groups[1].Value;
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据


                    lv1.SubItems.Add(dt.Rows[a][1].ToString().Trim());
                    lv1.SubItems.Add(dt.Rows[a][2].ToString().Trim());
                    lv1.SubItems.Add(dt.Rows[a][3].ToString().Trim());
                    lv1.SubItems.Add(dt.Rows[a][4].ToString().Trim());
                    lv1.SubItems.Add(dt.Rows[a][5].ToString().Trim());
                    lv1.SubItems.Add(dt.Rows[a][6].ToString().Trim());
                    lv1.SubItems.Add(dt.Rows[a][7].ToString().Trim());
                    lv1.SubItems.Add(dt.Rows[a][8].ToString().Trim());
                    lv1.SubItems.Add(dt.Rows[a][9].ToString().Trim());
                    lv1.SubItems.Add(dt.Rows[a][10].ToString().Trim());
                    lv1.SubItems.Add(dt.Rows[a][11].ToString().Trim());
                    lv1.SubItems.Add(Regex.Replace(louceng, "<[^>]+>", "").Trim());
                    lv1.SubItems.Add(Regex.Replace(tags.Replace("</i>", ","), "<[^>]+>", "").Trim().Replace(" ",""));
                    if (status == false)
                        return;
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    label1.Text = DateTime.Now.ToShortTimeString() + "正在查询：" + keyword; MatchCollection house_codes = Regex.Matches(html, @"""house_code"":""([\s\S]*?)""");
       
                              
                              
                         

                        

                    }
                   

 


            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }

        }

        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run2);
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

        private void button7_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }


     
        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            // openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox1.Text = openFileDialog1.FileName;



            }
          

        }
    }
}
