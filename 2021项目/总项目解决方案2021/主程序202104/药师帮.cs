using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 主程序202104
{
    public partial class 药师帮 : Form
    {
        public 药师帮()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
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


        public bool duibi(string guige,string danwei,string changjia,string aguige,string adanwei,string achangjia)
        {
           
            bool status = false;
            if (radioButton2.Checked == true)
            {
                if (guige == aguige && danwei == adanwei && changjia == achangjia)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }
            }
            else
            {
                status = true;
            }

        

            return status;
        }




        DataTable dt = null;
        string cookie = "sajssdk_2015_cross_new_user=1; rcfp=5e26e85da46947c48cb4004bce9728a2dd27; Captcha=84E7391CDF4F42F8AC6657CA83ECECEA; Token=6d3c3c08759248198ca45b22dae1dc27; sensorsdata2015jssdkcross=%7B%22distinct_id%22%3A%221067688%22%2C%22first_id%22%3A%2217c6d1cf412ae8-0bceff3c68e1a3-4343363-2073600-17c6d1cf41382d%22%2C%22props%22%3A%7B%22%24latest_traffic_source_type%22%3A%22%E7%9B%B4%E6%8E%A5%E6%B5%81%E9%87%8F%22%2C%22%24latest_search_keyword%22%3A%22%E6%9C%AA%E5%8F%96%E5%88%B0%E5%80%BC_%E7%9B%B4%E6%8E%A5%E6%89%93%E5%BC%80%22%2C%22%24latest_referrer%22%3A%22%22%7D%2C%22%24device_id%22%3A%2217c6d1cf412ae8-0bceff3c68e1a3-4343363-2073600-17c6d1cf41382d%22%7D";



        /// <summary>
        /// 全部价格不导入直接搜索
        /// </summary>
        public void run_all()
        {

          
                for (int page = 1; page <36000; page++)
                {
                    try
                    {

                    try
                    {
                        string path = AppDomain.CurrentDomain.BaseDirectory;
                        StreamReader sr = new StreamReader(path + "ex1.txt", method.EncodingType.GetTxtType(path + "ex1.txt"));
                        //一次性读取完 
                        string texts = sr.ReadToEnd();

                        ex1 = texts.Trim();
                        sr.Close();  //只关闭流
                        sr.Dispose();   //销毁流内存

                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.ToString());
                    }


                    string url = "https://dian.ysbang.cn/wholesale-drug/sales/getWholesaleList/v4270";
                    //string postdata = "{\"platform\":\"pc\",\"version\":\"4.39.6\",\"ua\":\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.90 Safari/537.36 Chrome 89\",\"ex\":\"2021-4-7 18:42 indexContent\",\"page\":" + page + ",\"pagesize\":\"60\",\"classify_id\":\"\",\"searchkey\":\"\",\"sort\":\"\",\"operationtype\":1,\"provider_filter\":\"\",\"activityTypes\":[],\"qualifiedLoanee\":0,\"factoryNames\":\"\",\"specs\":\"\",\"drugId\":-1,\"showRecentlyPurchasedFlag\":true,\"onlyShowRecentlyPurchased\":false,\"token\":\"f41d7da82aef43e583e7a94fa579d93d\"}";

                    string postdata = "{\"platform\":\"pc\",\"version\":\"5.7.0\",\"ua\":\"Chrome 92\",\"ex\":\"2021-9-24 19:32 indexContent 10-11 10:09:46 10-11 10:12:17\",\"ex1\":\""+ex1+"\",\"page\":"+page+",\"pagesize\":\"60\",\"classify_id\":\"\",\"searchkey\":\"\",\"sort\":\"\",\"operationtype\":1,\"provider_filter\":\"\",\"activityTypes\":[],\"qualifiedLoanee\":0,\"factoryNames\":\"\",\"specs\":\"\",\"drugId\":-1,\"showRecentlyPurchasedFlag\":true,\"onlyShowRecentlyPurchased\":false,\"token\":\"6d3c3c08759248198ca45b22dae1dc27\"}";
                   
                    string html = method.PostUrl(url, postdata, cookie, "utf-8", "application/json", "https://dian.ysbang.cn/");
                   if(html.Contains("你的页面访问有问题"))
                    {
                        MessageBox.Show("登录失效，请重新搜索");
                        page = page - 1;
                        continue;
                    }
                    MatchCollection cn_names = Regex.Matches(html, @"""cn_name"":""([\s\S]*?)""");
                        MatchCollection specifications = Regex.Matches(html, @"""specification"":""([\s\S]*?)""");
                        MatchCollection chainPrices = Regex.Matches(html, @"""chainPrice"":""([\s\S]*?)""");
                        MatchCollection manufacturers = Regex.Matches(html, @"""manufacturer"":""([\s\S]*?)""");
                        MatchCollection units = Regex.Matches(html, @"""unit"":""([\s\S]*?)""");

                    //MessageBox.Show(cn_names.Count.ToString());
                        if (cn_names.Count == 0)
                            break;

                        for (int j = 0; j < cn_names.Count; j++)
                        {
                          
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(cn_names[j].Groups[1].Value.Trim());
                                lv1.SubItems.Add(specifications[j].Groups[1].Value.Trim());
                                lv1.SubItems.Add(chainPrices[j].Groups[1].Value.Trim());
                                lv1.SubItems.Add(manufacturers[j].Groups[1].Value.Trim());
                                lv1.SubItems.Add(units[j].Groups[1].Value.Trim());
                                lv1.SubItems.Add("");
                            
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)
                                return;


                        }
                        Thread.Sleep(1000);

                    }
                    catch (Exception ex)
                    {

                        ex.ToString();
                    }

                }


            }




        /// <summary>
        /// 全部价格
        /// </summary>
        public void run()
        {
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int page = 1; page < 101; page++)
                {
                    try
                    {
                        DataRow dr = dt.Rows[i];
                        //string keyword = System.Web.HttpUtility.UrlEncode(dr[1].ToString());
                       string keyword = dr[1].ToString();

                        string guige = dr[2].ToString();
                        string danwei = dr[3].ToString();
                        string changjia = dr[4].ToString();



                        string url = "https://dian.ysbang.cn/wholesale-drug/sales/getWholesaleList/v4270";
                        string postdata = "{\"platform\":\"pc\",\"version\":\"5.7.0\",\"ua\":\"Chrome 92\",\"ex\":\"2021-9-24 19:32 indexContent 10-11 10:09:46 10-11 10:12:17\",\"ex1\":\"" + ex1 + "\",\"page\":" + page + ",\"pagesize\":\"60\",\"classify_id\":\"\",\"searchkey\":\""+keyword+"\",\"sort\":\"\",\"operationtype\":1,\"provider_filter\":\"\",\"activityTypes\":[],\"qualifiedLoanee\":0,\"factoryNames\":\"\",\"specs\":\"\",\"drugId\":-1,\"showRecentlyPurchasedFlag\":true,\"onlyShowRecentlyPurchased\":false,\"token\":\"6d3c3c08759248198ca45b22dae1dc27\"}";

                        string html = method.PostUrl(url, postdata, cookie, "utf-8", "application/json", "https://dian.ysbang.cn/index.html");
                      
                        MatchCollection cn_names = Regex.Matches(html, @"""cn_name"":""([\s\S]*?)""");
                        MatchCollection specifications = Regex.Matches(html, @"""specification"":""([\s\S]*?)""");
                        MatchCollection chainPrices = Regex.Matches(html, @"""chainPrice"":""([\s\S]*?)""");
                        MatchCollection manufacturers = Regex.Matches(html, @"""manufacturer"":""([\s\S]*?)""");
                        MatchCollection units = Regex.Matches(html, @"""unit"":""([\s\S]*?)""");
                      

                        if (cn_names.Count == 0)
                            break;

                        for (int j = 0; j < cn_names.Count; j++)
                        {
                            if (duibi(guige, danwei, changjia, specifications[j].Groups[1].Value.Trim(), units[j].Groups[1].Value.Trim(), manufacturers[j].Groups[1].Value.Trim()))
                            {
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(cn_names[j].Groups[1].Value.Trim());
                                lv1.SubItems.Add(specifications[j].Groups[1].Value.Trim());
                                lv1.SubItems.Add(chainPrices[j].Groups[1].Value.Trim());
                                lv1.SubItems.Add(manufacturers[j].Groups[1].Value.Trim());
                                lv1.SubItems.Add(units[j].Groups[1].Value.Trim());
                                lv1.SubItems.Add(keyword);
                            }


                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)
                                return;

                            
                        }
                        Thread.Sleep(1000);

                    }
                    catch (Exception ex)
                    {

                      ex.ToString();
                    }

                }


            }



        }

     

        /// <summary>
        /// 最低价格
        /// </summary>
        public void run1()
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int page = 1; page < 2; page++)
                {
                    try
                    {
                        DataRow dr = dt.Rows[i];
                        //string keyword = System.Web.HttpUtility.UrlEncode(dr[1].ToString());
                        string keyword = dr[1].ToString();

                        string guige = dr[2].ToString();
                        string danwei = dr[3].ToString();
                        string changjia = dr[4].ToString();



                        string url = "https://dian.ysbang.cn/wholesale-drug/sales/getWholesaleList/v4270";
                        string postdata = "{\"platform\":\"pc\",\"version\":\"5.7.0\",\"ua\":\"Chrome 92\",\"ex\":\"2021-9-24 19:32 indexContent 10-11 10:09:46 10-11 10:12:17\",\"ex1\":\"" + ex1 + "\",\"page\":" + page + ",\"pagesize\":\"60\",\"classify_id\":\"\",\"searchkey\":\""+keyword+"\",\"sort\":\"\",\"operationtype\":1,\"provider_filter\":\"\",\"activityTypes\":[],\"qualifiedLoanee\":0,\"factoryNames\":\"\",\"specs\":\"\",\"drugId\":-1,\"showRecentlyPurchasedFlag\":true,\"onlyShowRecentlyPurchased\":false,\"token\":\"6d3c3c08759248198ca45b22dae1dc27\"}";

                        string html = method.PostUrl(url, postdata, cookie, "utf-8", "application/json", "https://dian.ysbang.cn/index.html");

                        MatchCollection cn_names = Regex.Matches(html, @"""cn_name"":""([\s\S]*?)""");
                        MatchCollection specifications = Regex.Matches(html, @"""specification"":""([\s\S]*?)""");
                        MatchCollection chainPrices = Regex.Matches(html, @"""chainPrice"":""([\s\S]*?)""");
                        MatchCollection manufacturers = Regex.Matches(html, @"""manufacturer"":""([\s\S]*?)""");
                        MatchCollection units = Regex.Matches(html, @"""unit"":""([\s\S]*?)""");


                        if (cn_names.Count == 0)
                            break;

                        int index = 0;
                        Dictionary<int,double> dics = new Dictionary<int , double>();
                       
                        for (int j = 0; j < cn_names.Count; j++)
                        {
                            if (duibi(guige, danwei, changjia, specifications[j].Groups[1].Value.Trim(), units[j].Groups[1].Value.Trim(), manufacturers[j].Groups[1].Value.Trim()))
                            {
                                if (dics.Count == 0)
                                {
                                    dics.Add(j, Convert.ToDouble(chainPrices[j].Groups[1].Value.Trim()));
                                }
                                else
                                {
                                    if (dics[dics.ElementAt(0).Key] > Convert.ToDouble(chainPrices[j].Groups[1].Value.Trim()))
                                    {
                                        dics.Clear();
                                        dics.Add(j, Convert.ToDouble(chainPrices[j].Groups[1].Value.Trim()));
                                    }
                                }
                            }
                        
                        }
                        index = dics.ElementAt(0).Key;

                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(cn_names[index].Groups[1].Value.Trim());
                        lv1.SubItems.Add(specifications[index].Groups[1].Value.Trim());
                        lv1.SubItems.Add(chainPrices[index].Groups[1].Value.Trim());
                        lv1.SubItems.Add(manufacturers[index].Groups[1].Value.Trim());
                        lv1.SubItems.Add(units[index].Groups[1].Value.Trim());
                        lv1.SubItems.Add(keyword);


                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;

                        Thread.Sleep(1000);

                    }
                    catch (Exception ex)
                    {

                       ex.ToString();
                    }

                }


            }



        }

        Thread thread;
        bool zanting = true;
        bool status = true;

        string ex1 = "";
        private void button6_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"kOuseov"))
            {

                return;
            }



            #endregion
            string path = AppDomain.CurrentDomain.BaseDirectory;
            try
            {
                StreamReader sr = new StreamReader(path + "cookie.txt", method.EncodingType.GetTxtType(path + "cookie.txt"));
                //一次性读取完 
               cookie= sr.ReadToEnd();

             
                textBox2.Text = cookie;
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


            try
            {
                StreamReader sr = new StreamReader(path + "ex1.txt", method.EncodingType.GetTxtType(path + "ex1.txt"));
                //一次性读取完 
                string texts = sr.ReadToEnd();

                ex1 = texts.Trim();
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }



            if (checkBox3.Checked == true)
            {
                dt = new DataTable();
                if (thread == null || !thread.IsAlive)
                {

                    thread = new Thread(run_all);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }

            else
            {

                if (textBox1.Text == "")
                {
                    MessageBox.Show("请导入表格");
                    return;
                }
                dt = method.ExcelToDataTable(textBox1.Text, true);
            }
            status = true;


          

           


            if (checkBox1.Checked == true)
            {
                if (thread == null || !thread.IsAlive)
                {
                   
                    thread = new Thread(run);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
            if (checkBox2.Checked == true)
            {
             
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(run1);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }

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

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"O6WxcM"))
            {
                MessageBox.Show("");
                return;
            }



            #endregion


            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                checkBox1.Checked = false;
            }
            else
            {
                checkBox1.Checked = true;
            }
        }

        private void 药师帮_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            Process.Start(path + "CEF主程序.exe");
        }

        private void 药师帮_Load(object sender, EventArgs e)
        {

        }
    }
}
