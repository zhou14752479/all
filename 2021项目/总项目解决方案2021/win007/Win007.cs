using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsharpHttpHelper;
using CsharpHttpHelper.Enum;
using myDLL;

namespace win007
{
    public partial class Win007 : Form
    {
        public Win007()
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
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
               
                request.AllowAutoRedirect = true;
                request.UserAgent = "";
                request.Referer = Url;
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
        public void chaxun(TextBox t1, TextBox t2, TextBox t3, TextBox t4, TextBox t5, TextBox t6,Label l1, Label l2, Label l3,ComboBox comb1,DataGridView dgv1, TextBox t9, TextBox t8, TextBox t7, TextBox t12, TextBox t11, TextBox t10)
        {
            int zhusheng_bifen_count = 0;
            int heju_bifen_count = 0;
            int kesheng_bifen_count = 0;

            try
            {
                //if(t1.Text=="" &&t2.Text=="" && t3.Text=="")
                //{
                //    MessageBox.Show("请输入数值");
                //    return;
                //}



                string sql = "select * from datas where";

                
                if(t1.Text!="")
                {
                    sql = sql + (" data1 like '" + t1.Text.Trim() + "' and");
                }
                if (t2.Text != "")
                {
                    sql = sql + (" data2 like '" + t2.Text.Trim() + "' and");
                }
                if (t3.Text != "")
                {
                    sql = sql + (" data3 like '" + t3.Text.Trim() + "' and");
                }




                if (t9.Text != "")
                {
                    sql = sql + (" data4 like '" + t9.Text.Trim() + "' and");
                }
                if (t8.Text != "")
                {
                    sql = sql + (" data5 like '" + t8.Text.Trim() + "' and");
                }
                if (t7.Text != "")
                {
                    sql = sql + (" data6 like '" + t7.Text.Trim() + "' and");
                }



                if (t12.Text != "")
                {
                    sql = sql + (" data7 like '" + t12.Text.Trim() + "' and");
                }
                if (t11.Text != "")
                {
                    sql = sql + (" data8 like '" + t11.Text.Trim() + "' and");
                }
                if (t10.Text != "")
                {
                    sql = sql + (" data9 like '" + t10.Text.Trim() + "' and");
                }







                if (t5.Text != "")
                {
                    sql = sql + (" rangqiudaxiaoqiu like '" + t5.Text.Trim() + "%' and");
                }
                if (t6.Text != "")
                {
                    sql = sql + (" rangqiudaxiaoqiu like '%" + t6.Text.Trim() + "' and");
                }


                if (comb1.Text != "")
                {
                    sql = sql + (" gongsi like '" + comb1.Text.Trim() + "' and");
                }

                if (sql.Substring(sql.Length - 3, 3) == "and")
                {
                    sql = sql.Substring(0, sql.Length - 3);
                }

                //textBox4.Text = sql;
                DataTable dt = fc.chaxundata(sql);
                dgv1.DataSource = dt;
                dgv1.Columns["matchname"].HeaderText = "比赛名";
                dgv1.Columns["zhu"].HeaderText = "主队";
                dgv1.Columns["ke"].HeaderText = "客队";
                dgv1.Columns["time"].HeaderText = "比赛时间";
                dgv1.Columns["gongsi"].HeaderText = "公司名";
               
                dgv1.Columns["bifen"].HeaderText = "比分";
          
                dgv1.Columns["data1"].HeaderText = "数据1";
                dgv1.Columns["data2"].HeaderText = "数据2";
                dgv1.Columns["data3"].HeaderText = "数据3";
                dgv1.Columns["data4"].HeaderText = "数据4";
                dgv1.Columns["data5"].HeaderText = "数据5";
                dgv1.Columns["data6"].HeaderText = "数据6";
                dgv1.Columns["data7"].HeaderText = "数据7";
                dgv1.Columns["data8"].HeaderText = "数据8";
                dgv1.Columns["data9"].HeaderText = "数据9";
                dgv1.Columns["zhu_cj"].HeaderText = "主队成交比例";
                dgv1.Columns["he_cj"].HeaderText = "和交比例";
                dgv1.Columns["ke_cj"].HeaderText = "客队成交比例";

                dgv1.Columns["zhu_yingkui"].HeaderText = "主庄家赢亏";
                dgv1.Columns["he_yingkui"].HeaderText = "和庄家赢亏";
                dgv1.Columns["ke_yingkui"].HeaderText = "客庄家赢亏";

                dgv1.Columns["zhu_yingkuizs"].HeaderText = "主赢亏指数";
                dgv1.Columns["he_yingkuizs"].HeaderText = "和赢亏指数";
                dgv1.Columns["ke_yingkuizs"].HeaderText = "客队赢亏指数";

              
                dgv1.Columns["rangqiudaxiaoqiu"].HeaderText = "让球大小球";
             

                dgv1.Columns[7].HeaderCell.Style.BackColor = Color.Red;
                dgv1.Columns[8].HeaderCell.Style.BackColor = Color.CornflowerBlue;
                dgv1.Columns[9].HeaderCell.Style.BackColor = Color.Green;

                dgv1.Columns[10].HeaderCell.Style.BackColor = Color.Red;
                dgv1.Columns[11].HeaderCell.Style.BackColor = Color.CornflowerBlue;
                dgv1.Columns[12].HeaderCell.Style.BackColor = Color.Green;

                //this.dgv1.Columns[7].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                //this.dgv1.Columns[8].DefaultCellStyle.BackColor = System.Drawing.Color.CornflowerBlue;
                //this.dgv1.Columns[9].DefaultCellStyle.BackColor = System.Drawing.Color.Green;

                //this.dgv1.Columns[10].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                //this.dgv1.Columns[11].DefaultCellStyle.BackColor = System.Drawing.Color.CornflowerBlue;
                //this.dgv1.Columns[12].DefaultCellStyle.BackColor = System.Drawing.Color.Green;

                //this.dgv1.Columns[13].Width = 0;
                //this.dgv1.Columns[14].Width = 0;
                //this.dgv1.Columns[15].Width = 0;

                dgv1.Columns[16].Width = 100;
                dgv1.Columns[17].Width =100;
                dgv1.Columns[18].Width = 50;

                dgv1.Columns[21].Width = 1;
                dgv1.Columns[22].Width = 1;
                dgv1.Columns[23].Width = 1;
                dgv1.Columns[24].Width = 1;
                dgv1.Columns[25].Width = 1;
                dgv1.Columns[26].Width = 1;

                //计算

                for (int i = 0; i < dgv1.Rows.Count - 1; i++)//如果DataGridView中有空的数据，则提示数据输入不完整并退出添加，不包括标题行
                {
                    try
                    {
                        string value1 = dgv1.Rows[i].Cells[7].Value.ToString();
                        string value2 = dgv1.Rows[i].Cells[8].Value.ToString();
                        string value3 = dgv1.Rows[i].Cells[9].Value.ToString();

                        string value4 = dgv1.Rows[i].Cells[10].Value.ToString();
                        string value5 = dgv1.Rows[i].Cells[11].Value.ToString();
                        string value6 = dgv1.Rows[i].Cells[12].Value.ToString();

                        string value7 = dgv1.Rows[i].Cells[13].Value.ToString();
                        string value8 = dgv1.Rows[i].Cells[14].Value.ToString();
                        string value9 = dgv1.Rows[i].Cells[15].Value.ToString();

                        double cha1 = Convert.ToDouble(value1) - Convert.ToDouble(value4);
                        double cha2 = Convert.ToDouble(value2) - Convert.ToDouble(value5);
                        double cha3 = Convert.ToDouble(value3) - Convert.ToDouble(value6);


                        double cha33 = Convert.ToDouble(value6) - Convert.ToDouble(value3);


                        double cha4 = Convert.ToDouble(value7) - Convert.ToDouble(value4);
                        double cha5 = Convert.ToDouble(value8) - Convert.ToDouble(value5);
                        double cha6 = Convert.ToDouble(value9) - Convert.ToDouble(value6);




                        
                        string sjp1 = "平";
                        string sjp2 = "平";
                        string sjp3 = "平";


                        string sjp4 = "平";
                        string sjp5 = "平";
                        string sjp6 = "平";

                        if (cha1>0)
                        {
                            sjp1 = "升";
                        }
                        if (cha1 <0)
                        {
                            sjp1 = "降";
                        }

                        if (cha2 > 0)
                        {
                            sjp2 = "升";
                        }
                        if (cha2 < 0)
                        {
                            sjp2 = "降";
                        }
                        if (cha3> 0)
                        {
                            sjp3 = "升";
                        }
                        if (cha3 < 0)
                        {
                            sjp3 = "降";
                        }


                        if(cha4>0)
                        {
                            sjp4 = "升";
                            dgv1.Rows[i].Cells[10].Style.BackColor = Color.Green;
                        }
                        if (cha5 > 0)
                        {
                            sjp5 = "升";
                            dgv1.Rows[i].Cells[11].Style.BackColor = Color.Green;
                        }
                        if (cha6 > 0)
                        {
                            sjp6= "升";
                            dgv1.Rows[i].Cells[12].Style.BackColor = Color.Green;
                        }
                        if (cha4 < 0)
                        {
                            sjp4 = "降";
                            dgv1.Rows[i].Cells[10].Style.BackColor = Color.Red;
                        }
                        if (cha5 < 0)
                        {
                            sjp5= "降";
                            dgv1.Rows[i].Cells[11].Style.BackColor = Color.Red;
                        }
                        if (cha6 < 0)
                        {
                            sjp6= "降";
                            dgv1.Rows[i].Cells[12].Style.BackColor = Color.Red;
                        }


                        if(textBox1.Text!="")
                        {
                            dgv1.Rows[i].Cells[16].Value = sjp1 + sjp2 + sjp3 + " " + dgv1.Rows[i].Cells[6].Value;
                        }
                         else if(textBox9.Text!="")
                        {
                            dgv1.Rows[i].Cells[16].Value = sjp4 + sjp5 + sjp6 + " " + dgv1.Rows[i].Cells[6].Value;
                        }


                        //差值红绿色
                        if (cha1 > 0)
                        {
                            dgv1.Rows[i].Cells[7].Style.BackColor = Color.Red;
                        }
                        if (cha2 > 0)
                        {
                            dgv1.Rows[i].Cells[8].Style.BackColor = Color.Red;
                        }
                        if (cha3 > 0)
                        {
                            dgv1.Rows[i].Cells[9].Style.BackColor = Color.Red;
                        }
                        if (cha1 < 0)
                        {
                            dgv1.Rows[i].Cells[7].Style.BackColor = Color.Green;
                        }
                        if (cha2 < 0)
                        {
                            dgv1.Rows[i].Cells[8].Style.BackColor = Color.Green;
                        }
                        if (cha3 < 0)
                        {
                            dgv1.Rows[i].Cells[9].Style.BackColor = Color.Green;
                        }


                        ////成交比例颜色黄橙红
                        //if (cha1 > 0.1&& cha1 <= 0.15)
                        //{
                        //    dgv1.Rows[i].Cells[16].Style.BackColor = Color.Yellow;
                        //}
                        //if (cha1 > 0.15 && cha1 <= 0.2)
                        //{
                        //    dgv1.Rows[i].Cells[16].Style.BackColor = Color.Orange;
                        //}
                        //if (cha1 > 0.2)
                        //{
                           
                        //    dgv1.Rows[i].Cells[16].Style.BackColor = Color.Red;
                        //}
                        //if (cha2> 0.1 && cha1 <= 0.15)
                        //{
                        //    dgv1.Rows[i].Cells[16].Style.BackColor = Color.Yellow;
                        //}
                        //if (cha2 > 0.15 && cha1 <= 0.2)
                        //{
                        //    dgv1.Rows[i].Cells[16].Style.BackColor = Color.Orange;
                        //}
                        //if (cha2 > 0.2)
                        //{
                            
                        //    dgv1.Rows[i].Cells[16].Style.BackColor = Color.Red;
                        //}
                        //if (cha3 > 0.1 && cha1 <= 0.15)
                        //{
                        //    dgv1.Rows[i].Cells[16].Style.BackColor = Color.Yellow;
                        //}
                        //if (cha3 > 0.15 && cha1 <= 0.2)
                        //{
                        //    dgv1.Rows[i].Cells[16].Style.BackColor = Color.Orange;
                        //}
                        //if (cha3 > 0.2)
                        //{
                            
                        //    dgv1.Rows[i].Cells[16].Style.BackColor = Color.Red;
                        //}

                        if (cha33 > 0.09 && cha33 <= 0.15)
                        {
                            dgv1.Rows[i].Cells[16].Style.BackColor = Color.Yellow;
                        }
                        if (cha33 > 0.16&& cha33 <= 0.2) 
                        {
                            dgv1.Rows[i].Cells[16].Style.BackColor = Color.Orange;
                        }
                        if (cha33 > 0.21)
                        {

                            
                            dgv1.Rows[i].Cells[16].Style.BackColor = Color.Red;
                        }


                        //成交比例
                        string zhu_cj= dgv1.Rows[i].Cells[18].Value.ToString().Replace("%","");
                        string he_cj = dgv1.Rows[i].Cells[19].Value.ToString().Replace("%", "");
                        string ke_cj = dgv1.Rows[i].Cells[20].Value.ToString().Replace("%", "");

                        if (zhu_cj!="无")
                        {
                            if(Convert.ToDouble(zhu_cj)>=70 & Convert.ToDouble(zhu_cj)<80)
                            {
                                dgv1.Rows[i].Cells[18].Style.BackColor = Color.Yellow;
                            }
                            if (Convert.ToDouble(zhu_cj) >= 80 & Convert.ToDouble(zhu_cj) < 90)
                            {
                                dgv1.Rows[i].Cells[18].Style.BackColor = Color.Orange;
                            }
                            if (Convert.ToDouble(zhu_cj) >90)
                            {
                                dgv1.Rows[i].Cells[18].Style.BackColor = Color.Red;
                            }


                        }
                        if (he_cj != "无")
                        {
                            if (Convert.ToDouble(he_cj) >= 70 & Convert.ToDouble(he_cj) < 80)
                            {
                                dgv1.Rows[i].Cells[19].Style.BackColor = Color.Yellow;
                            }
                            if (Convert.ToDouble(he_cj) >= 80 & Convert.ToDouble(he_cj) < 90)
                            {
                                dgv1.Rows[i].Cells[19].Style.BackColor = Color.Orange;
                            }
                            if (Convert.ToDouble(he_cj) >= 90)
                            {
                                dgv1.Rows[i].Cells[19].Style.BackColor = Color.Red;
                            }
                        }
                        if (ke_cj != "无")
                        {
                            if (Convert.ToDouble(ke_cj) >= 70 & Convert.ToDouble(ke_cj) < 80)
                            {
                                dgv1.Rows[i].Cells[20].Style.BackColor = Color.Yellow;
                            }
                            if (Convert.ToDouble(ke_cj) >= 80 & Convert.ToDouble(ke_cj) <90)
                            {
                                dgv1.Rows[i].Cells[20].Style.BackColor = Color.Orange;
                            }
                            if (Convert.ToDouble(ke_cj) >= 90)
                            {
                                dgv1.Rows[i].Cells[20].Style.BackColor = Color.Red;
                            }
                        }



                       

                    }
                    catch (Exception)
                    {

                        continue;
                    }

                }
              
              

              
                    for (int i = 0; i < dgv1.Rows.Count - 1; i++)//如果DataGridView中有空的数据，则提示数据输入不完整并退出添加，不包括标题行
                    {
                        try
                        {
                            string value = dgv1.Rows[i].Cells[16].Value.ToString();

                            string sjpshaixuan = t4.Text;
                             if (textBox13.Text!="")
                            {
                                sjpshaixuan =  textBox13.Text.Trim();
                            }
                            if (textBox14.Text != "") 
                            {
                                sjpshaixuan = textBox14.Text.Trim();
                            }
                            if (!value.Contains(sjpshaixuan)) 
                            {
                                DataGridViewRow row = dgv1.Rows[i];
                                 dgv1.Rows.Remove(row);
                                  i--; //这句是关键。。
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
               
                //计算比分百分比
                for (int i = 0; i < dgv1.Rows.Count - 1; i++)//如果DataGridView中有空的数据，则提示数据输入不完整并退出添加，不包括标题行
                {
                    //比分
                    string bifen = dgv1.Rows[i].Cells[6].Value.ToString();
                    string[] bifens = bifen.Split(new string[] { "-" }, StringSplitOptions.None);
                    if (bifens.Length == 2)
                    {
                        if (Convert.ToInt32(bifens[0]) > Convert.ToInt32(bifens[1]))
                        {
                            zhusheng_bifen_count = zhusheng_bifen_count + 1;
                        }
                        else if (Convert.ToInt32(bifens[0]) == Convert.ToInt32(bifens[1]))
                        {
                            heju_bifen_count = heju_bifen_count + 1;
                        }
                        else if (Convert.ToInt32(bifens[0]) < Convert.ToInt32(bifens[1]))
                        {
                            kesheng_bifen_count = kesheng_bifen_count + 1;
                        }
                    }
                }
                l1.Text = Convert.ToDouble(Convert.ToDouble(zhusheng_bifen_count) / Convert.ToDouble((zhusheng_bifen_count + heju_bifen_count + kesheng_bifen_count))).ToString("F2");
                l2.Text = Convert.ToDouble(Convert.ToDouble(heju_bifen_count) / Convert.ToDouble((zhusheng_bifen_count + heju_bifen_count + kesheng_bifen_count))).ToString("F2");
               l3.Text = Convert.ToDouble(Convert.ToDouble(kesheng_bifen_count) / Convert.ToDouble((zhusheng_bifen_count + heju_bifen_count + kesheng_bifen_count))).ToString("F2");

                //fc.ShowDataInListView(dt, listView1);
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.ToString());


            }
        }


        private void Win007_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"hGRLg"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion
            dateTimePicker1.Value = DateTime.Now.AddDays(-1);
            dateTimePicker2.Value = DateTime.Now;
        }


        Dictionary<string, string> gongsi_dics = new Dictionary<string, string>();
       
        
        string startdate = "2021-01-01";
        string enddate = "2022-01-06";
        public void getdata()
        {




            for (DateTime dt = Convert.ToDateTime(startdate); dt < Convert.ToDateTime(enddate);dt=dt.AddDays(1))
            {
                try
                {
                    string url = "http://bf.win007.com/football/Over_" + dt.ToString("yyyyMMdd") + ".htm";
                    label7.Text = dt.ToString("yyyyMMdd");

                    string html = method.GetUrl(url, "gb2312");
                    MatchCollection trs = Regex.Matches(html, @"<tr height=18([\s\S]*?)</tr>");
                
                   

                    for (int i = 0; i < trs.Count; i++)
                    {
                        if (trs[i].Groups[1].Value.Contains("display: none"))
                        {
                           // label7.Text = "不显示，跳过..";
                            continue;
                        }
                      string id = Regex.Match(trs[i].Groups[1].Value, @"showgoallist\(([\s\S]*?)\)").Groups[1].Value;
                        string rangqiu = Regex.Match(trs[i].Groups[1].Value, @"<td id='hdp_([\s\S]*?)>([\s\S]*?)<").Groups[2].Value;
                        string daxiaoqiu = Regex.Match(trs[i].Groups[1].Value, @"<td id='ou_([\s\S]*?)>([\s\S]*?)<").Groups[2].Value;
                        if (id.Trim()=="")
                        {
                            continue;
                        }
                        string bifen_zhu= Regex.Match(trs[i].Groups[1].Value, @"showgoallist([\s\S]*?)<font color=([\s\S]*?)>([\s\S]*?)</font>").Groups[3].Value;
                        string bifen_ke = Regex.Match(trs[i].Groups[1].Value, @"showgoallist([\s\S]*?)-<font color=([\s\S]*?)>([\s\S]*?)</font>").Groups[3].Value;
                        string datajsurl = "http://1x2d.win007.com/" + id+ ".js?r=007132848760362108507";
                        string datajs = method.GetUrl(datajsurl, "gb2312");
                        string datajsjs = Regex.Match(datajs, @"game=([\s\S]*?);").Groups[1].Value;

                      
                        //获取成交比例
                        string cjurl = "http://vip.win007.com/betfa/single.aspx?id="+id+"&l=0";
                        //string datacj = method.GetUrlwithIP(cjurl, "tps682.kdlapi.com:15818", "", "utf-8");
                        string datacj = GetUrl(cjurl, "utf-8");
                        
                        MatchCollection cjs = Regex.Matches(datacj, @"<td class=""rb"">([\s\S]*?)</td>");
                        MatchCollection yingkuizss = Regex.Matches(datacj, @"<td>([\s\S]*?)</td>");
                        if (!datacj.Contains("暂无本场比赛的必发数据") && cjs.Count == 0 && datacj.Trim()!="")
                        {
                            datacj = GetUrl(cjurl, "gb2312");
                            MessageBox.Show(datacj);
                            Thread.Sleep(20000);
                            i = i - 1;
                            continue;
                        }
                        // MessageBox.Show(cjs.Count.ToString());
                        string zhu_cj = "无";
                        string he_cj = "无";
                        string ke_cj = "无";


                        string zhu_yingkui = "无";
                        string he_yingkui = "无";
                        string ke_yingkui = "无";
                        string zhu_yingkuizs = "无";
                        string he_yingkuizs = "无";
                        string ke_yingkuizs = "无";

                        if (cjs.Count > 17)
                        {
                            zhu_cj = cjs[3].Groups[1].Value;
                            he_cj = cjs[9].Groups[1].Value;
                            ke_cj = cjs[15].Groups[1].Value;

                            zhu_yingkui = cjs[5].Groups[1].Value;
                            he_yingkui = cjs[11].Groups[1].Value;
                            ke_yingkui = cjs[17].Groups[1].Value;
                            zhu_yingkuizs = yingkuizss[3].Groups[1].Value;
                            he_yingkuizs = yingkuizss[10].Groups[1].Value;
                            ke_yingkuizs = yingkuizss[17].Groups[1].Value;
                        }
                        string matchname_cn = Regex.Match(datajs, @"matchname_cn=""([\s\S]*?)""").Groups[1].Value;
                        string hometeam_cn = Regex.Match(datajs, @"hometeam_cn=""([\s\S]*?)""").Groups[1].Value;
                        string guestteam_cn = Regex.Match(datajs, @"guestteam_cn=""([\s\S]*?)""").Groups[1].Value;
                        string MatchTime = Regex.Match(datajs, @"MatchTime=""([\s\S]*?)""").Groups[1].Value;


                        MatchCollection gongsis = Regex.Matches(datajs, @"\d{1,5}\|\d{8,10}\|([\s\S]*?)\|");
                        for (int a = 0; a < gongsis.Count; a++)
                        {
                            string cid= Regex.Match(gongsis[a].ToString(), @"\d{8,10}").Groups[0].Value;
                            string cname =gongsis[a].Groups[1].ToString();
                          
                            if (!gongsi_dics.ContainsKey(cid))
                            {
                                switch (cname)
                                {
                                    case "SNAI":
                                        gongsi_dics.Add(cid, "SNAI");
                                        break;
                                    case "Titanbet":
                                        gongsi_dics.Add(cid, "Titanbet");
                                        break;
                                    case "Bethard":
                                        gongsi_dics.Add(cid, "Bethard");
                                        break;
                                    case "ComeOn":
                                        gongsi_dics.Add(cid, "ComeOn");
                                        break;
                                    case "Intertops":
                                        gongsi_dics.Add(cid, "Intertops");
                                        break;
                                    case "Bet3000":
                                        gongsi_dics.Add(cid, "Bet3000");
                                        break;
                                    case "Crown":
                                        gongsi_dics.Add(cid, "Crown");
                                        break;
                                    case "William Hill":
                                        gongsi_dics.Add(cid, "William Hill");
                                        break;
                                    case "Bet-at-home":
                                        gongsi_dics.Add(cid, "Bet-at-home");
                                        break;
                                    case "Lottery Official":
                                        gongsi_dics.Add(cid, "Lottery Official");
                                        break;
                                    case "Vcbet":
                                        gongsi_dics.Add(cid, "Vcbet");
                                        break;

                                }
                            }
                          
                        }

                     

                        string datas = Regex.Match(datajs, @"gameDetail=Array\(([\s\S]*?)\)").Groups[1].Value;
                       
                        string[] datastext = datas.Split(new string[] { "\",\"" }, StringSplitOptions.None);


                        
                        for (int j = 0; j < datastext.Length; j++)
                        {

                            string cid= Regex.Match(datastext[j], @"\d{8,10}").Groups[0].Value.Trim();
                           
                            if (gongsi_dics.ContainsKey(cid))
                            {
                                string gongsi_name = gongsi_dics[cid];
                              
                                try
                                {

                                    //ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                                    //lv1.SubItems.Add(matchname_cn);
                                    //lv1.SubItems.Add(hometeam_cn);
                                    //lv1.SubItems.Add(guestteam_cn);
                                    //lv1.SubItems.Add(MatchTime);
                                    //lv1.SubItems.Add(gongsi_dics[cid]);
                                 


                                    string bifen = bifen_zhu+ "-"+ bifen_ke;
                                  
                                    //lv1.SubItems.Add(bifen);

                                    string[] datasresult = datastext[j].Split(new string[] { ";" }, StringSplitOptions.None);

                                    string data1 = "";
                                    string data2 = "";
                                    string data3 = "";
                                    string data4 = "";
                                    string data5 = "";
                                    string data6 = "";
                                    string data7 = "";
                                    string data8 = "";
                                    string data9 = "";
                                    try
                                    {
                                        string[] data_a = datasresult[0].Split(new string[] { "|" }, StringSplitOptions.None);
                                        data1 = data_a[0].Replace(cid,"").Replace("^", "");
                                        data2 = data_a[1];
                                        data3 = data_a[2];


                                        string[] data_b = datasresult[1].Split(new string[] { "|" }, StringSplitOptions.None);
                                        data4 = data_b[0];
                                        data5 = data_b[1];
                                        data6 = data_b[2];


                                        string[] data_c = datasresult[2].Split(new string[] { "|" }, StringSplitOptions.None);
                                        data7 = data_c[0];
                                        data8 = data_c[1];
                                        data9 = data_c[2];
                                    }
                                    catch (Exception)
                                    {

                                        
                                    }




                                    //lv1.SubItems.Add(data1);                             
                                    //    lv1.SubItems.Add(data2);                                 
                                    //    lv1.SubItems.Add(data3);                                
                                    //    lv1.SubItems.Add(data4);
                                    //    lv1.SubItems.Add(data5);                                     
                                    //    lv1.SubItems.Add(data6);
                                    //    lv1.SubItems.Add(data7);
                                    //    lv1.SubItems.Add(data8);
                                    //lv1.SubItems.Add(data9);

                                    string rangqiu_daxiaoqiu = rangqiu +" "+ daxiaoqiu;
                                    fc.insertdata(id,matchname_cn, hometeam_cn, guestteam_cn, MatchTime, gongsi_name, bifen, data1, data2, data3, data4, data5, data6,data7,data8,data9,zhu_cj,he_cj,ke_cj,zhu_yingkui,he_yingkui,ke_yingkui,zhu_yingkuizs,he_yingkuizs,ke_yingkuizs,rangqiu_daxiaoqiu);



                                    if (status == false)
                                        return;
                                   // Thread.Sleep(100);


                                }
                                catch (Exception ex)
                                {
                                    //  MessageBox.Show(ex.ToString());
                                    continue;
                                }
                            }
                        }


                    }
                    
                }
                catch (Exception ex)
                {

                    //MessageBox.Show(ex.ToString());
                }
            }

            MessageBox.Show("完成");
        }
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridView1.ColumnHeadersHeight = 50;
            chaxun(textBox1,textBox2,textBox3,textBox4,textBox5,textBox6,zhusheng_banfenbi_label,heju_banfenbi_label,kesheng_banfenbi_label,comboBox1,dataGridView1,textBox9,textBox8,textBox7,textBox12,textBox11,textBox10);


        }

        private void 清空查询数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                listView1.Items.Clear();
                DataTable dt = (DataTable)dataGridView1.DataSource;
                dt.Rows.Clear();

                dataGridView1.DataSource = dt;
            }
            catch (Exception)
            {

               
            }
        }

        private void 导出查询数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
        bool status = true;

        private void button2_Click(object sender, EventArgs e)
        {
            startdate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            enddate = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getdata);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //startdate = Convert.ToDateTime("2018-07-01").ToString("yyyy-MM-dd");
            startdate = Convert.ToDateTime("2021-02-28").ToString("yyyy-MM-dd");
            enddate = DateTime.Now.ToString("yyyy-MM-dd");
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getdata);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
           // MessageBox.Show("全年数据已抓取");
        }

        //private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    comboBox1.Items.Clear();
        //    ArrayList lists = fc.getsupplyers();
        //    comboBox1.Items.Add("");
        //    foreach (var item in lists)
        //    {
        //        comboBox1.Items.Add(item);
        //    }
        //}

        private void button5_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void 清空数据库全部数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要清空吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                string sql = "delete from datas";
                bool status = fc.SQL(sql);
                fc.SQL("VACUUM");
                fc.SQL("DELETE FROM sqlite_sequence WHERE name = 'datas';");
                MessageBox.Show("清空成功");
            }
            else
            {
               
            }
           
        }

       

        private void Win007_FormClosing(object sender, FormClosingEventArgs e)
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

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
        //    dataGridView2.ColumnHeadersHeight = 50;
        //    chaxun(textBox12, textBox11, textBox10, textBox9, textBox7, textBox8, label15, label14, label3, comboBox2, dataGridView2);
        //}

        //private void button6_Click(object sender, EventArgs e)
        //{
        //    dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
        //    dataGridView3.ColumnHeadersHeight = 50;
        //    chaxun(textBox18, textBox17, textBox16, textBox15, textBox13, textBox14, label28, label27, label26, comboBox3, dataGridView3);
        //}

        //private void button7_Click(object sender, EventArgs e)
        //{
        //    dataGridView4.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
        //    dataGridView4.ColumnHeadersHeight = 50;
        //    chaxun(textBox24, textBox23, textBox22, textBox21, textBox19, textBox20, label41, label40, label39, comboBox4, dataGridView4);
        //}

        //private void button8_Click(object sender, EventArgs e)
        //{
        //    dataGridView5.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
        //    dataGridView5.ColumnHeadersHeight = 50;
        //    chaxun(textBox30, textBox29, textBox28, textBox27, textBox25, textBox26, label54, label53, label52, comboBox5, dataGridView5);
        //}

        private void button9_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            for (int count = 0; count < dataGridView1.Columns.Count; count++)
            {
                DataColumn dc = new DataColumn(dataGridView1.Columns[count].Name.ToString());
                dt.Columns.Add(dc);
            }
            for (int count = 0; count < dataGridView1.Rows.Count; count++)
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    dr[i] = Convert.ToString(dataGridView1.Rows[count].Cells[i].Value);
                }
                dt.Rows.Add(dr);
            }



            dataGridView6.DataSource = dt;

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox4.Text += "升";
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox4.Text += "降";
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox4.Text += "平";
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox4.Text= "";
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox13.Text += "升";

        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox13.Text += "降";
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox13.Text += "平";
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox13.Text = "";
        }

        private void linkLabel12_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox14.Text += "升";
        }

        private void linkLabel11_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox14.Text += "降";
        }

        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox14.Text += "平";
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox14.Text = "";

        }
    }
}
