using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
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

namespace 竞彩数据查询
{
    public partial class 竞彩数据查询 : Form
    {
        public 竞彩数据查询()
        {
            InitializeComponent();
        }

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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36";
                request.Referer = "http://odds.500.com/fenxi/ouzhi-427496.shtml";
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
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
        function fc = new function();
        public void getdata()
        {




            for (DateTime dt = Convert.ToDateTime("2022-07-20"); dt < Convert.ToDateTime("2023-07-20"); dt = dt.AddDays(1))
            {
                try
                {
                    string url = "https://live.500.com/?e="+ dt.ToString("yyyy-MM-dd");
                    label1.Text = dt.ToString();

                    string html = method.GetUrl(url, "gb2312");
                    MatchCollection trs = Regex.Matches(html, @"<tr id=""([\s\S]*?)</tr>");

                    for (int i = 0; i < trs.Count; i++)
                    {
                        string tr = trs[i].Groups[1].Value;
                        string aid = Regex.Match(tr, @"fid=""([\s\S]*?)""").Groups[1].Value;
                       string date= Regex.Matches(tr, @"<td align=""center""([\s\S]*?)>([\s\S]*?)<")[2].Groups[2].Value;
                        string match = Regex.Match(tr, @"gy=""([\s\S]*?)""").Groups[1].Value;
                        string[] matchs = match.Split(new string[] { "," }, StringSplitOptions.None);

                        string matchname = matchs[0];
                        string zhu= matchs[1];
                        string ke = matchs[2];
                        string bifen = Regex.Match(tr, @"class=""clt1"" >([\s\S]*?)</a>").Groups[1].Value+"-"+ Regex.Match(tr, @"class=""clt3"" >([\s\S]*?)</a>").Groups[1].Value;

                        string result= Regex.Matches(tr, @"<td align=""center"" class=""red"">([\s\S]*?)<")[1].Groups[1].Value.Replace("&nbsp;", "").Trim();

                        string aurl = "http://odds.500.com/fenxi1/json/ouzhi.php?_=1647769994323&fid="+aid+"&cid=1&r=1&time=2014-03-15+09%3A24%3A43&type=europe";
                        string ahtml= GetUrl(aurl, "gb2312").Replace("[[","[").Replace("]]", "]");
                       

                        MatchCollection values = Regex.Matches(ahtml, @"\[([\s\S]*?)\]");
                      
                        for (int j = 0; j < values.Count; j++)
                        {
                            string type = "变化赔率";
                            if(j==0)
                            {
                                type = "最终赔率";
                            }
                            if (j == values.Count-1)
                            {
                                type = "初始赔率";
                            }

                            string[] text = values[j].Groups[1].Value.Split(new string[] { "," }, StringSplitOptions.None);
                        
                            if (text.Length > 4)
                            {
                                fc.insertdata(date, matchname, zhu, ke, bifen, text[0], text[1], text[2], type, text[3], text[4].Replace("\"", ""),result);
                            }
                        }
                   

                    }

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }
            }

            MessageBox.Show("完成");
        }
        Thread thread;
       

        private void button3_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getdata);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
        public void chaxun()
        {
          

            try
            {
                if (textBox1.Text == "" && textBox2.Text == "" && textBox3.Text == "")
                {
                    MessageBox.Show("请输入数值");
                    return;
                }


                #region 查询1
                string sql = "select date,updatetime,match,zhu,ke,bifen,sheng,ping,fu,type,result from datas where";

                if (textBox1.Text != "")
                {
                    sql = sql + (" sheng like '" + textBox1.Text.Trim() + "' and");
                }
                if (textBox2.Text != "")
                {
                    sql = sql + (" ping like '" + textBox2.Text.Trim() + "' and");
                }
                if (textBox3.Text != "")
                {
                    sql = sql + (" fu like '" + textBox3.Text.Trim() + "' and");
                }

                if (sql.Substring(sql.Length - 3, 3) == "and")
                {
                    sql = sql.Substring(0, sql.Length - 3);
                }

                #endregion

                #region 查询2
                string sql2 = "select date,updatetime,match,zhu,ke,bifen,sheng,ping,fu,type,result from datas where";

                if (textBox4.Text != "")
                {
                    sql2 = sql2 + (" sheng like '" + textBox4.Text.Trim() + "' and");
                }
                if (textBox5.Text != "")
                {
                    sql2 = sql2 + (" ping like '" + textBox5.Text.Trim() + "' and");
                }
                if (textBox6.Text != "")
                {
                    sql2 = sql2 + (" fu like '" + textBox6.Text.Trim() + "' and");
                }

                if (sql2.Substring(sql2.Length - 3, 3) == "and")
                {
                    sql2 = sql2.Substring(0, sql2.Length - 3);
                }

                #endregion



                #region 查询3
                string sql3 = "select date,updatetime,match,zhu,ke,bifen,sheng,ping,fu,type,result from datas where";

                if (textBox7.Text != "")
                {
                    sql3 = sql3 + (" sheng like '" + textBox7.Text.Trim() + "' and");
                }
                if (textBox8.Text != "")
                {
                    sql3 = sql3 + (" ping like '" + textBox8.Text.Trim() + "' and");
                }
                if (textBox9.Text != "")
                {
                    sql3 = sql3 + (" fu like '" + textBox9.Text.Trim() + "' and");
                }

                if (sql3.Substring(sql3.Length - 3, 3) == "and")
                {
                    sql3= sql3.Substring(0, sql3.Length - 3);
                }

                #endregion
                DataTable dt = fc.chaxundata(sql);
                DataTable dt2= fc.chaxundata(sql2);
               

                //if (textBox4.Text!="" || textBox5.Text=="" || textBox6.Text=="")
                //{


                //}


                DataTable newdt= dt.Clone();
                // dt.Merge(dt2);
                
               
                Dictionary<string, int> dics = new Dictionary<string, int>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string date = dt.Rows[i][0].ToString().Trim() + dt.Rows[i][3].ToString().Trim();
      
                    if (!dics.ContainsKey(date))
                    {
                        dics.Add(date, i);
                    }
                    
                   
                }
                Dictionary<string, int> dics2 = new Dictionary<string, int>();

                for (int i = 0; i < dt2.Rows.Count; i++)//如果DataGridView中有空的数据，则提示数据输入不完整并退出添加，不包括标题行
                {
                    try
                    {
                        string date = dt2.Rows[i][0].ToString().Trim() + dt2.Rows[i][3].ToString().Trim();

                        if (dics.ContainsKey(date))
                        {
                             if(!dics2.ContainsKey(date))
                            {
                                dics2.Add(date, i);
                            }
                            newdt.Rows.Add(dt.Rows[dics[date]].ItemArray);
                            newdt.Rows.Add(dt2.Rows[i].ItemArray);
                           
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.ToString());
                    }
                }


                //第三种
                if(textBox7.Text!="" || textBox8.Text!="" ||textBox9.Text!="")
                {
                    newdt.Rows.Clear();
                     DataTable dt3 = fc.chaxundata(sql3);
                    for (int i = 0; i < dt3.Rows.Count; i++)//如果DataGridView中有空的数据，则提示数据输入不完整并退出添加，不包括标题行
                    {
                        try
                        {
                            string date = dt3.Rows[i][0].ToString().Trim() + dt3.Rows[i][3].ToString().Trim();

                            if (dics2.ContainsKey(date)) //前两组日期+联赛名包括，则第三组符合要求
                            {

                                newdt.Rows.Add(dt.Rows[dics[date]].ItemArray);
                                newdt.Rows.Add(dt2.Rows[dics2[date]].ItemArray);
                                newdt.Rows.Add(dt3.Rows[i].ItemArray);

                            }
                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.ToString());
                        }
                    }

                }



                dataGridView1.DataSource = newdt;


                dataGridView1.Columns["date"].HeaderText = "比赛时间";
                dataGridView1.Columns["updatetime"].HeaderText = "变化时间";
                dataGridView1.Columns["match"].HeaderText = "比赛名";
                dataGridView1.Columns["zhu"].HeaderText = "主队";
                dataGridView1.Columns["ke"].HeaderText = "客队";
         
                dataGridView1.Columns["bifen"].HeaderText = "比分";

                dataGridView1.Columns["sheng"].HeaderText = "胜";
                dataGridView1.Columns["ping"].HeaderText = "平";
                dataGridView1.Columns["fu"].HeaderText = "负";
                dataGridView1.Columns["type"].HeaderText = "类型";
                dataGridView1.Columns["result"].HeaderText = "赛果";

                double shengcount = 0;
                double pingcount = 0;
                double fucount = 0;

                double ying1count = 0;
                double shu1count = 0;
                //计算

                for (int i = 0; i < (dataGridView1.Rows.Count - 1); i++)//如果DataGridView中有空的数据，则提示数据输入不完整并退出添加，不包括标题行
                {
                    string value = dataGridView1.Rows[i].Cells[10].Value.ToString();
                    string bifen = dataGridView1.Rows[i].Cells[5].Value.ToString();


                    if (value == "胜")
                    {
                        shengcount = shengcount + 1;
                    }
                    if (value == "平")
                    {
                        pingcount = pingcount + 1;
                    }
                    if (value == "负")
                    {
                        fucount = fucount + 1;
                    }


                    //计算一球的场数
                    string[] text = bifen.Split(new string[] { "-" }, StringSplitOptions.None);
                    if (text.Length > 1)
                    {
                        if (text[0].Trim() != "")
                        {
                            if (Convert.ToInt32(text[0]) - Convert.ToInt32(text[1]) == 1)
                            {
                                ying1count = ying1count + 1;
                            }
                            if (Convert.ToInt32(text[1]) - Convert.ToInt32(text[0]) == 1)
                            {
                                shu1count = shu1count + 1;
                            }
                        }
                    }
                }

                try
                {

                    

                    double shenglv = shengcount / (dataGridView1.Rows.Count - 1);
                    double pinglv = pingcount / (dataGridView1.Rows.Count - 1);
                    double fulv = fucount / (dataGridView1.Rows.Count - 1);

                  

                    double ying1lv = ying1count / (dataGridView1.Rows.Count - 1);
                    double shu1lv = shu1count / (dataGridView1.Rows.Count - 1);



                    label4.Text = "共符合：" + (dataGridView1.Rows.Count - 1) + "场比赛，胜率：" + Convert.ToInt32(Convert.ToDouble(shenglv.ToString("F2")) * 100) + "%；平局：" + Convert.ToInt32(Convert.ToDouble(pinglv.ToString("F2")) * 100) + "%；负场：" +
                        Convert.ToInt32(Convert.ToDouble(fulv.ToString("F2")) * 100) + "%；";
                        label7.Text= "赢一场胜率："+ Convert.ToInt32(Convert.ToDouble(ying1lv.ToString("F2")) * 100) + "%；输一场胜率："+ Convert.ToInt32(Convert.ToDouble(shu1lv.ToString("F2")) * 100) + "%";
                }
                catch (Exception)
                {

                    label4.Text = "共符合：" + (dataGridView1.Rows.Count - 1) + "场比赛";
                }
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());


            }
        }


        /// <summary>
        /// 只查询一行，最终赔率
        /// </summary>
        public void chaxun1()
        {


            try
            {
                if (textBox1.Text == "" && textBox2.Text == "" && textBox3.Text == "")
                {
                    MessageBox.Show("请输入数值");
                    return;
                }


                #region 查询1
                string sql = "select date,updatetime,match,zhu,ke,bifen,sheng,ping,fu,type,result from datas where type='最终赔率' and ";

                if (textBox1.Text != "")
                {
                    sql = sql + (" sheng like '" + textBox1.Text.Trim() + "' and");
                }
                if (textBox2.Text != "")
                {
                    sql = sql + (" ping like '" + textBox2.Text.Trim() + "' and");
                }
                if (textBox3.Text != "")
                {
                    sql = sql + (" fu like '" + textBox3.Text.Trim() + "' and");
                }

                if (sql.Substring(sql.Length - 3, 3) == "and")
                {
                    sql = sql.Substring(0, sql.Length - 3);
                }

                #endregion

              
                DataTable dt = fc.chaxundata(sql);
               

                dataGridView1.DataSource = dt;


                dataGridView1.Columns["date"].HeaderText = "比赛时间";
                dataGridView1.Columns["updatetime"].HeaderText = "变化时间";
                dataGridView1.Columns["match"].HeaderText = "比赛名";
                dataGridView1.Columns["zhu"].HeaderText = "主队";
                dataGridView1.Columns["ke"].HeaderText = "客队";

                dataGridView1.Columns["bifen"].HeaderText = "比分";

                dataGridView1.Columns["sheng"].HeaderText = "胜";
                dataGridView1.Columns["ping"].HeaderText = "平";
                dataGridView1.Columns["fu"].HeaderText = "负";
                dataGridView1.Columns["type"].HeaderText = "类型";
                dataGridView1.Columns["result"].HeaderText = "赛果";

                double shengcount = 0;
                double pingcount = 0;
                double fucount = 0;

                double ying1count = 0;
                double shu1count = 0;
                //计算

                for (int i = 0; i < (dataGridView1.Rows.Count - 1); i++)//如果DataGridView中有空的数据，则提示数据输入不完整并退出添加，不包括标题行
                {
                    string value = dataGridView1.Rows[i].Cells[10].Value.ToString();
                    string bifen = dataGridView1.Rows[i].Cells[5].Value.ToString();


                    if (value == "胜")
                    {
                        shengcount = shengcount + 1;
                    }
                    if (value == "平")
                    {
                        pingcount = pingcount + 1;
                    }
                    if (value == "负")
                    {
                        fucount = fucount + 1;
                    }


                    //计算一球的场数
                    string[] text = bifen.Split(new string[] { "-" }, StringSplitOptions.None);
                    if (text.Length > 1)
                    {
                        if (text[0].Trim() != "")
                        {
                            if (Convert.ToInt32(text[0]) - Convert.ToInt32(text[1]) == 1)
                            {
                                ying1count = ying1count + 1;
                            }
                            if (Convert.ToInt32(text[1]) - Convert.ToInt32(text[0]) == 1)
                            {
                                shu1count = shu1count + 1;
                            }
                        }
                    }
                }

                try
                {



                    double shenglv = shengcount / (dataGridView1.Rows.Count - 1);
                    double pinglv = pingcount / (dataGridView1.Rows.Count - 1);
                    double fulv = fucount / (dataGridView1.Rows.Count - 1);



                    double ying1lv = ying1count / (dataGridView1.Rows.Count - 1);
                    double shu1lv = shu1count / (dataGridView1.Rows.Count - 1);



                    label4.Text = "共符合：" + (dataGridView1.Rows.Count - 1) + "场比赛，胜率：" + Convert.ToInt32(Convert.ToDouble(shenglv.ToString("F2")) * 100) + "%；平局：" + Convert.ToInt32(Convert.ToDouble(pinglv.ToString("F2")) * 100) + "%；负场：" +
                        Convert.ToInt32(Convert.ToDouble(fulv.ToString("F2")) * 100) + "%；";
                    label7.Text = "赢一场胜率：" + Convert.ToInt32(Convert.ToDouble(ying1lv.ToString("F2")) * 100) + "%；输一场胜率：" + Convert.ToInt32(Convert.ToDouble(shu1lv.ToString("F2")) * 100) + "%";
                }
                catch (Exception)
                {

                    label4.Text = "共符合：" + (dataGridView1.Rows.Count - 1) + "场比赛";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());


            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            chaxun();
        }

        private void 竞彩数据查询_FormClosing(object sender, FormClosingEventArgs e)
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

        private void button4_Click(object sender, EventArgs e)
        {
            string sql = "delete from datas";
            bool status = fc.SQL(sql);
            fc.SQL("VACUUM");
            fc.SQL("DELETE FROM sqlite_sequence WHERE name = 'datas';");
            MessageBox.Show("清空成功");
        }

        private void 竞彩数据查询_Load(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"tTtuL"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion
        }
        string path = System.Environment.CurrentDirectory + "\\jingcaidata.db";
        private void button2_Click(object sender, EventArgs e)
        {
           
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";

        }
    }
}
