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

namespace 竞彩数据查询
{
    public partial class 竞彩篮球 : Form
    {
        public 竞彩篮球()
        {
            InitializeComponent();
        }

        #region GET使用代理IP请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlwithIP(string Url, string ip, string COOKIE, string charset)
        {
            string html = "";

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.75 Safari/537.36";
                WebProxy proxy = new WebProxy(ip);
                request.KeepAlive = false;
                request.Proxy = proxy;
                request.Referer = Url;
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
               
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
        public void getdata()
        {
            //https://www.okooo.com/jingcailanqiu/livecenter/2022-01-11/



            for (DateTime dt = Convert.ToDateTime("2016-01-01"); dt < Convert.ToDateTime("2022-04-20"); dt = dt.AddDays(1))
            {
                try
                {

                    string url = "https://www.okooo.com/jingcailanqiu/livecenter/"+ dt.ToString("yyyy-MM-dd") + "/";
                    label1.Text = dt.ToString();

                    string ahtml = GetUrlwithIP(url, "tps471.kdlapi.com:15818", "", "gb2312");
                    MatchCollection trs = Regex.Matches(ahtml, @"<tr id=""match_detail_([\s\S]*?)float_r");

                    for (int i = 0; i < trs.Count; i++)
                    {
                        string html = trs[i].Groups[1].Value;
                       string xuhao = Regex.Match(html, @"<input id=""([\s\S]*?)""").Groups[1].Value;
                        string time = Regex.Match(html, @"<td  class=""graytxt"">([\s\S]*?)<").Groups[1].Value;
                        string liansai = Regex.Match(html, @"type=""([\s\S]*?)""").Groups[1].Value;
                        string ke = Regex.Matches(html, @"target=""_blank"" title=""([\s\S]*?)""")[0].Groups[1].Value;
                        string zhu = Regex.Matches(html, @"target=""_blank"" title=""([\s\S]*?)""")[1].Groups[1].Value;
                        string zhu_fen = Regex.Match(html, @"ja=""-1"">([\s\S]*?)</b>").Groups[1].Value;
                        string ke_fen = Regex.Match(html, @"jh=""-1"">([\s\S]*?)</b>").Groups[1].Value;

                        string ke_sheng = Regex.Matches(html, @"<td class=""linebg"">([\s\S]*?)</span>")[0].Groups[1].Value;
                        string ke_daxiao = Regex.Matches(html, @"<td class=""linebg"">([\s\S]*?)</span>")[1].Groups[1].Value;
                        string ke_rang = Regex.Matches(html, @"<td class=""linebg"">([\s\S]*?)</td>")[2].Groups[1].Value;


                        string zhu_sheng = Regex.Matches(html, @"<td><span([\s\S]*?)>([\s\S]*?)</span>")[0].Groups[2].Value;
                        string zhu_daxiao = Regex.Matches(html, @"<td><span([\s\S]*?)>([\s\S]*?)</span>")[1].Groups[2].Value;
                        string zhu_rang_fen = Regex.Match(html, @"<td><span class=""fenChaSpan float_l"">([\s\S]*?)</span>").Groups[1].Value;
                        string zhu_rang_peilv = Regex.Match(html, @"<td><span class=""fenChaSpan float_l"">([\s\S]*?)主([\s\S]*?)</span>").Groups[2].Value;


                        zhu_fen = Regex.Replace(zhu_fen, "<[^>]+>", "");
                        ke_fen = Regex.Replace(ke_fen, "<[^>]+>", "");

                        ke_sheng = Regex.Replace(ke_sheng, "<[^>]+>", "").Replace("&nbsp;&nbsp;", "").Replace("&nbsp;", "").Replace("客", "").Trim();
                        ke_daxiao = Regex.Replace(ke_daxiao, "<[^>]+>", "").Replace("&nbsp;&nbsp;", ",").Replace("&nbsp;", "").Replace("客", "").Trim();
                        ke_rang = Regex.Replace(ke_rang, "<[^>]+>", "").Replace("&nbsp;&nbsp;", "").Replace("&nbsp;", "").Replace("客", "").Trim();

                        zhu_sheng = Regex.Replace(zhu_sheng, "<[^>]+>", "").Replace("&nbsp;&nbsp;", "").Replace("&nbsp;", "").Replace("主", "").Trim();
                        zhu_daxiao = Regex.Replace(zhu_daxiao, "<[^>]+>", "").Replace("&nbsp;&nbsp;", ",").Replace("&nbsp;", "").Replace("主", "").Trim();
                        zhu_rang_fen = Regex.Replace(zhu_rang_fen, "<[^>]+>", "").Replace("&nbsp;&nbsp;", ",").Replace("&nbsp;", "").Replace("主", "").Trim();
                        zhu_rang_peilv = Regex.Replace(zhu_rang_peilv, "<[^>]+>", "").Replace("&nbsp;&nbsp;", "").Replace("&nbsp;", "").Replace("主", "").Trim();

                        //string msg =  ke_sheng + "  -" + ke_daxiao + "  -" + ke_rang + "  -" + zhu_sheng + "  -" + zhu_daxiao +"  -" + zhu_rang_fen +"  -" + zhu_rang_peilv;
                        //MessageBox.Show(msg);
                       
                        fc.insertdata_lanqiu(xuhao, time, liansai, zhu, ke, zhu_fen, ke_fen, ke_sheng, ke_daxiao, ke_rang, zhu_sheng, zhu_daxiao, zhu_rang_fen, zhu_rang_peilv, dt.ToString("yyyy-MM-dd"));



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

        function fc=new function();
        private void button4_Click(object sender, EventArgs e)
        {
            string sql = "delete from datas";
            bool status = fc.SQL(sql);
            fc.SQL("VACUUM");
            fc.SQL("DELETE FROM sqlite_sequence WHERE name = 'datas';");
            MessageBox.Show("清空成功");
        }

        private void 竞彩篮球_Load(object sender, EventArgs e)
        {

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
                string sql = "select * from datas where";

                if (textBox1.Text != "")
                {
                    sql = sql + (" zhu_rang_fen like '" + textBox1.Text.Trim() + "' and");
                }
                if (textBox3.Text != "")
                {
                    sql = sql + (" zhu like '" + textBox3.Text.Trim() + "' and");
                }
                if (sql.Substring(sql.Length - 3, 3) == "and")
                {
                    sql = sql.Substring(0, sql.Length - 3);
                }
                #endregion

                #region 查询2
                string sql2 = "select * from datas where";

                if (textBox2.Text != "")
                {
                    sql2 = sql2 + (" zhu_rang_fen like '" + textBox2.Text.Trim() + "' and");
                }
                if (textBox4.Text != "")
                {
                    sql2 = sql2 + (" ke like '" + textBox4.Text.Trim() + "' and");
                }
                if (sql2.Substring(sql2.Length - 3, 3) == "and")
                {
                    sql2 = sql2.Substring(0, sql2.Length - 3);
                }
                #endregion



               
                DataTable dt = fc.chaxundata(sql);
                DataTable dt2 = fc.chaxundata(sql2);
                dt.Merge(dt2);

                //DataTable newdt = dt.Clone();


                //Dictionary<string, int> dics = new Dictionary<string, int>();

                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    string date = dt.Rows[i][2].ToString().Trim();

                //    if (!dics.ContainsKey(date))
                //    {
                //        dics.Add(date, i);
                //    }


                //}
                //Dictionary<string, int> dics2 = new Dictionary<string, int>();

                //for (int i = 0; i < dt2.Rows.Count; i++)//如果DataGridView中有空的数据，则提示数据输入不完整并退出添加，不包括标题行
                //{
                //    try
                //    {
                //        string date = dt2.Rows[i][2].ToString().Trim();

                //        if (dics.ContainsKey(date))
                //        {
                //            if (!dics2.ContainsKey(date))
                //            {
                //                dics2.Add(date, i);
                //            }
                //            newdt.Rows.Add(dt.Rows[dics[date]].ItemArray);
                //            newdt.Rows.Add(dt2.Rows[i].ItemArray);

                //        }
                //    }
                //    catch (Exception ex)
                //    {

                //        MessageBox.Show(ex.ToString());
                //    }
                //}






                dataGridView1.DataSource = dt;


                dataGridView1.Columns["xuhao"].HeaderText = "序号";
                dataGridView1.Columns["time"].HeaderText = "时间";
                dataGridView1.Columns["liansai"].HeaderText = "类型";
                dataGridView1.Columns["zhu"].HeaderText = "主队";
                dataGridView1.Columns["ke"].HeaderText = "客队";

                dataGridView1.Columns["zhu_fen"].HeaderText = "主分";
                dataGridView1.Columns["ke_fen"].HeaderText = "客分";
               
                dataGridView1.Columns["zhu_rang_peilv"].HeaderText = "让分赔率";
                dataGridView1.Columns["zhu_rang_fen"].HeaderText = "让分";

                double shengcount = 0;
                double pingcount = 0;
                double fucount = 0;

                double ying1count = 0;
                double shu1count = 0;
                //计算

                //for (int i = 0; i < (dataGridView1.Rows.Count - 1); i++)//如果DataGridView中有空的数据，则提示数据输入不完整并退出添加，不包括标题行
                //{
                //    string value = dataGridView1.Rows[i].Cells[10].Value.ToString();
                //    string bifen = dataGridView1.Rows[i].Cells[5].Value.ToString();


                //    if (value == "胜")
                //    {
                //        shengcount = shengcount + 1;
                //    }
                //    if (value == "平")
                //    {
                //        pingcount = pingcount + 1;
                //    }
                //    if (value == "负")
                //    {
                //        fucount = fucount + 1;
                //    }


                   
                //}

                //try
                //{



                //    double shenglv = shengcount / (dataGridView1.Rows.Count - 1);
                //    double pinglv = pingcount / (dataGridView1.Rows.Count - 1);
                //    double fulv = fucount / (dataGridView1.Rows.Count - 1);



                //    double ying1lv = ying1count / (dataGridView1.Rows.Count - 1);
                //    double shu1lv = shu1count / (dataGridView1.Rows.Count - 1);



                //    label4.Text = "共符合：" + (dataGridView1.Rows.Count - 1) + "场比赛，胜率：" + Convert.ToInt32(Convert.ToDouble(shenglv.ToString("F2")) * 100) + "%；平局：" + Convert.ToInt32(Convert.ToDouble(pinglv.ToString("F2")) * 100) + "%；负场：" +
                //        Convert.ToInt32(Convert.ToDouble(fulv.ToString("F2")) * 100) + "%；";
                    
                //}
                //catch (Exception)
                //{

                //    label4.Text = "共符合：" + (dataGridView1.Rows.Count - 1) + "场比赛";
                //}

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

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void 竞彩篮球_FormClosing(object sender, FormClosingEventArgs e)
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
