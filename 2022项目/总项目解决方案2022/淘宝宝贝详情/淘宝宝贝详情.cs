using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
using MySql.Data.MySqlClient;

namespace 淘宝宝贝详情
{
    public partial class 淘宝宝贝详情 : Form
    {
        public 淘宝宝贝详情()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取时间戳毫秒
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalMilliseconds);
            return a.ToString();
        }

        public static string Md5_utf8(string str)
        {
            //将输入字符串转换成字节数组
            var buffer = Encoding.GetEncoding("utf-8").GetBytes(str);
            //接着，创建Md5对象进行散列计算
            var data = MD5.Create().ComputeHash(buffer);

            //创建一个新的Stringbuilder收集字节
            var sb = new StringBuilder();

            //遍历每个字节的散列数据 
            foreach (var t in data)
            {
                //格式每一个十六进制字符串
                sb.Append(t.ToString("X2"));
            }

            //返回十六进制字符串
            return sb.ToString().ToLower();
        }


        #region GET请求带COOKIE
        /// <summary>
        /// GET请求带COOKIE
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlWithCookie(string Url, string COOKIE, string charset)
        {
            string html = "";

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.Proxy = null;//防止代理抓包
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 15_4_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/8.0.20(0x18001435) NetType/WIFI Language/zh_CN ";
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                request.Referer = "https://h5.m.taobao.com/";
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;

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

        static string conn = "Host =47.96.189.55;Database=titledb;Username=root;Password=root;";
        #region 绑定数据
        public static DataTable getdata(string sql)
        {


            MySqlDataAdapter sda = new MySqlDataAdapter(sql, conn);
            DataSet Ds = new DataSet();
            sda.Fill(Ds, "T_Class");

            DataTable dt = Ds.Tables["T_Class"];
            return dt;
        }

        #endregion

        public static bool insert(string itemid, string title, string sellerid, string shop, string wangwang,string cate)
        {
           
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            MySqlConnection mycon = new MySqlConnection(conn);
            mycon.Open();
            string sql = "INSERT INTO tbdata (itemid,title,sellerid,shop,wangwang,cate,time)VALUES('" + itemid + " ', '" + title + " ', '" + sellerid + " ','" + shop + " ',  '" + wangwang + " ', '" + cate + " ', '" + nowtime + " ')";

            MySqlCommand cmd = new MySqlCommand(sql, mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


            int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
            if (count > 0)
            {
                mycon.Close();
                return true;


            }
            else
            {
                mycon.Close();
                return false;
            }
        }

        public string cookie = "";
        public void run()
        {

           
            string cookie = "isg=BNXVAOKYG5EP6T_tamYFJgrS7tWP0onkobYBbVd6kcybrvWgHyKZtON_ejQYtaGc; tfstk=cN6CBON2PuZUptHr2MZaUj0jv9JPZIyWj5YcAPOUfkS-Ga_CiM32lqcCRjdpIh1..; l=eBSLc-HnLKUcCRh5BOfCKurza779ACdbYuPzaNbMiOCP_Wfp5TuhW6f71WL9Cn1Rh6VkR35h-6qyBeYBmQd-nxvOXEzSIxMmn; x5sec=7b226d746f703b32223a223332616361396637653266393432343863386365363031393331646533383465434c2b5a6c355147454944416e712f682f7544663767456144444d354d7a49304f4459784e7a4d374d54446c78722b5a42773d3d227d; WAPFDFDTGFG=%2B4cMKKP%2B8PI%2BKK8WkvO%2BOFO1Q%2FOhBg%3D%3D; _w_app_lg=0; _w_tb_nick=tb855660241; ntm=1; ockeqeudmj=oRssWIU%3D; _cc_=VFC%2FuZ9ajQ%3D%3D; _l_g_=Ug%3D%3D; _nk_=tb855660241; _tb_token_=58673ed8ebfe0; cancelledSubSites=empty; cookie1=BxSpTjKheS%2BNtKqHESDFqb0qgKbZ%2FXYfKGlNvIzjp5Y%3D; cookie17=UNk0wGcFeRAgZg%3D%3D; cookie2=2749faab247c0f530a95524477103375; csg=7efb561b; dnk=tb855660241; lgc=tb855660241; munb=3932486173; sg=136; sgcookie=E1002r5fcZ1D4WimsY%2FqXhuqVEnJ0L8N9TNLyVzKp9bZzI9XuTl%2BLthxBN23y4NYmNloysulirB%2BhtNyjsNVWDokv3zASdsC0%2BtvBbQmkwbhZy4%3D; skt=6017a5dc6502b52b; t=597430916dc5c2dc63d192ef0e521af2; tracknick=tb855660241; uc1=cookie21=UIHiLt3xThH8t7YQoFNq&cookie14=UoexMN0kKitrQg%3D%3D&existShop=false&cookie15=Vq8l%2BKCLz3%2F65A%3D%3D; uc3=vt3=F8dCvC%2B4ZSB2dEhjh5A%3D&lg2=UIHiLt3xD8xYTw%3D%3D&nk2=F5RNYJtQmpouHKU%3D&id2=UNk0wGcFeRAgZg%3D%3D; uc4=id4=0%40Ug4%2B4bIKniW7RNnXZ8cI5eYn4Z%2Fb&nk4=0%40FY4GtwzNHC%2FHY9GiKBbr81yuMcXxSQ%3D%3D; unb=3932486173; _m_h5_tk=c8b634869fe9a7daf032e44af5d62e0c_1652944129233; _m_h5_tk_enc=ff6abeb4f0848d82c664832b12903f37; _samesite_flag_=true; _uetsid=fac9ee40d71f11ecbcec57158d4824d2; _uetvid=faca0f20d71f11ec8ad81db1bbafd024; _gcl_au=1.1.11194650.1652929291; hng=CN%7Czh-CN%7CCNY%7C156; xlly_s=1; thw=cn; cna=HzAJG44cEGkCAXpgIXQET9pA";

           //cookie = method.GetCookies("https://main.m.taobao.com/");
           string token =  Regex.Match(cookie, @"_m_h5_tk=([\s\S]*?)_").Groups[1].Value;

        
            try
            {

                for (long i = Convert.ToInt64(textBox1.Text); i <= Convert.ToInt64(textBox1.Text)+ Convert.ToInt32(textBox3.Text); i=i+ Convert.ToInt32(textBox2.Text))
                {

                    if (status == false)
                        return;
                    Thread.Sleep(1000);
                    //string itemid = "675334675758";
                    string itemid = i.ToString();
                    string time = GetTimeStamp();

                    string str = token + "&" + time + "&12574478&{\"id\":\"" + itemid + "\",\"detail_v\":\"3.5.0\",\"exParams\":\"{\\\"id\\\":\\\"" + itemid + "\\\",\\\"appReqFrom\\\":\\\"detail\\\",\\\"container_type\\\":\\\"xdetail\\\",\\\"dinamic_v3\\\":\\\"true\\\",\\\"supportV7\\\":\\\"true\\\",\\\"ultron2\\\":\\\"true\\\"}\",\"itemNumId\":\"" + itemid + "\",\"pageCode\":\"miniAppDetail\",\"_from_\":\"miniapp\"}";

                    string sign = Md5_utf8(str);

                    string url = "https://h5api.m.taobao.com/h5/mtop.taobao.detail.getdetail/6.0/?jsv=2.6.2&appKey=12574478&t=" + time + "&sign=" + sign + "&api=mtop.taobao.detail.getdetail&v=6.0&ttid=202012%40taobao_h5_9.17.0&isSec=0&ecode=0&AntiFlood=true&AntiCreep=true&H5Request=true&type=jsonp&dataType=jsonp&safariGoLogin=true&mainDomain=taobao.com&subDomain=m&prefix=h5api&getJSONP=true&token=" + token + "&callback=mtopjsonp1&data=%7B%22id%22%3A%22" + itemid + "%22%2C%22detail_v%22%3A%223.5.0%22%2C%22exParams%22%3A%22%7B%5C%22id%5C%22%3A%5C%22" + itemid + "%5C%22%2C%5C%22appReqFrom%5C%22%3A%5C%22detail%5C%22%2C%5C%22container_type%5C%22%3A%5C%22xdetail%5C%22%2C%5C%22dinamic_v3%5C%22%3A%5C%22true%5C%22%2C%5C%22supportV7%5C%22%3A%5C%22true%5C%22%2C%5C%22ultron2%5C%22%3A%5C%22true%5C%22%7D%22%2C%22itemNumId%22%3A%22" + itemid + "%22%2C%22pageCode%22%3A%22miniAppDetail%22%2C%22_from_%22%3A%22miniapp%22%7D";

                    string html = GetUrlWithCookie(url, cookie, "utf-8");

                   // textBox4.Text = html;   
                    //MessageBox.Show(html);
                  
                   string title = Regex.Match(html, @"""itemId""([\s\S]*?)""title"":""([\s\S]*?)""").Groups[2].Value;
                    string sellerId = Regex.Match(html, @"sellerId=([\s\S]*?)&").Groups[1].Value;
                    string shopName = Regex.Match(html, @"""shopName"":""([\s\S]*?)""").Groups[1].Value;
                    string sellerNick = Regex.Match(html, @"""sellerNick"":""([\s\S]*?)""").Groups[1].Value;
                    string rootCategoryId = Regex.Match(html, @"""rootCategoryId"":""([\s\S]*?)""").Groups[1].Value;
                    string categoryId = Regex.Match(html, @"""categoryId"":""([\s\S]*?)""").Groups[1].Value;
                    string shopType = Regex.Match(html, @"""shopType"":""([\s\S]*?)""").Groups[1].Value;  //C店或B店


                    string catename = "";

                    if (shopName == "" || shopType == "B" || html.Contains("下架") || title=="" || html.Contains("交易方式"))
                    {
                        textBox4.Text += itemid + "：宝贝不符合跳过..." + "\r\n";
                        continue;
                    }



                    if (catedics.ContainsKey(categoryId))
                    {
                        catename = catedics[categoryId];
                    }
                    else
                    {
                        textBox4.Text += itemid + "：类目不符合跳过..." + "\r\n";
                        continue;
                    }


                    textBox4.Text += itemid+ "：插入数据成功！" + "\r\n";
                    insert(itemid, title, sellerId, shopName, sellerNick, catename);

                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        Thread thread;
        bool status = true;
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测


            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"ogS6"))
            {
                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();

                return;
            }

            #endregion

            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {
                string[] text = checkedListBox1.CheckedItems[i].ToString().Split(new string[] { "----" }, StringSplitOptions.None);
              if(text.Length>1)
                {
                   
                    if(!catedics.ContainsKey(text[1]))
                    {
                        catedics.Add(text[1], text[0]);
                    }
                }
               
            }
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;
        Dictionary<string,string> catedics=new Dictionary<string,string>();
        public void getcate()
        {
            StreamReader sr = new StreamReader(path+ "//data//cate.txt", method.EncodingType.GetTxtType(path + "//data//cate.txt"));
            //一次性读取完 
            string texts = sr.ReadToEnd();

            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {

                try
                {
                    string cateid = Regex.Match(text[i], @"\d{4,}").Groups[0].Value;
                    string catename = text[i].Replace(cateid, "").Trim();
                    checkedListBox1.Items.Add(catename + "----" + cateid);
                    
                }
                catch (Exception)
                {
                    continue;
                }
            }

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存
        }

        #region  程序关闭删除自身
        public static void TestForKillMyself()
        {
            string bat = @"@echo off
                           :tryagain
                           del %1
                           if exist %1 goto tryagain
                           del %0";
            File.WriteAllText("killme.bat", bat);//写bat文件
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "killme.bat";
            psi.Arguments = "\"" + Environment.GetCommandLineArgs()[0] + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
        }


        #endregion
        private void 淘宝宝贝详情_Load(object sender, EventArgs e)
        {
            #region 通用检测


            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"ogS6"))
            {
                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();

                return;
            }

            #endregion
            getcate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string sql = "select * from tbdata where time > '" + dateTimePicker1.Value.ToString("yyyy-MM-dd 00:00:00") + "' and time <'" + dateTimePicker2.Value.ToString("yyyy-MM-dd 23:59:59") + "'";
           DataTable dt= getdata(sql);
          
            dataGridView1.DataSource = dt;
            dataGridView1.Columns["itemid"].HeaderText = "宝贝";
            dataGridView1.Columns["title"].HeaderText = "标题";
            dataGridView1.Columns["shop"].HeaderText = "店铺";
            dataGridView1.Columns["wangwang"].HeaderText = "旺旺";
            dataGridView1.Columns["cate"].HeaderText = "类目";
            dataGridView1.Columns["time"].HeaderText = "时间";

            dataGridView1.Columns[0].Width = 60;
            dataGridView1.Columns[2].Width = 300;
            label6.Text = dataGridView1.Rows.Count.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.DgvToTable(dataGridView1), "Sheet1", true);
        }

      

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
               
                DataTable dt = (DataTable)dataGridView1.DataSource;
                dt.Rows.Clear();

                dataGridView1.DataSource = dt;
            }
            catch (Exception)
            {


            }
        }

        private void 淘宝宝贝详情_FormClosing(object sender, FormClosingEventArgs e)
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
