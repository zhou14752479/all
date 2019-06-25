using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main._2019_6
{
    public partial class 证券所 : Form
    {
        public 证券所()
        {
            InitializeComponent();
        }



        public static string Path=AppDomain.CurrentDomain.BaseDirectory;

        private void 证券所_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            
        }
        #region  获取文件夹内的所有.txt文件
        public static ArrayList GetFiles(string filePath)
        {
            ArrayList lists = new ArrayList();
            if (!Directory.Exists(filePath))
            {

                return lists;
            }

            //创建一个DirectoryInfo的类
            DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
            //获取当前的目录的文件
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            foreach (FileInfo info in fileInfos)
            {
                //获取文件的名称(包括扩展名)
                string fullName = info.FullName;
                //获取文件的扩展名
                string extension = info.Extension.ToLower();
                if (extension == ".txt")
                {
                    //获取文件的大小
                    long length = info.Length;
                    lists.Add(fullName);

                }
            }

            return lists;
        }
        #endregion

        /// <summary>
        /// 上海
        /// </summary>
        public void run()
        {
            DateTime dt = DateTime.Parse(dateTimePicker1.Text);
            try
            {


                string Url = "http://acaiji.com/sse.php?date="+ dt.ToString("yyyyMMdd");

                string html = method.gethtml(Url,"", "utf-8");

                MatchCollection codes = Regex.Matches(html, @"""secCode"":""([\s\S]*?)""");
                MatchCollection jianchengs = Regex.Matches(html, @"""secAbbr"":""([\s\S]*?)""");
                MatchCollection mairus = Regex.Matches(html, @"""branchNameB"":""([\s\S]*?)""");    
                MatchCollection maichus = Regex.Matches(html, @"""branchNameS"":""([\s\S]*?)""");
                
                for (int i = 0; i < codes.Count; i++)
                {
                    string buy = mairus[i].Groups[1].Value.Replace("有限责任公司", "").Replace("股份有限公司", "").Replace("有限公司", "").Replace("证券营业部", "").Replace("营业部", "");
                   string sell= maichus[i].Groups[1].Value.Replace("有限责任公司", "").Replace("股份有限公司", "").Replace("有限公司", "").Replace("证券营业部", "").Replace("营业部", "");
                    string[] mairu = buy.Split(new string[] { "," }, StringSplitOptions.None);
                    string[] maichu = sell.Split(new string[] { "," }, StringSplitOptions.None);

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据         
                    lv1.SubItems.Add(codes[i].Groups[1].Value);
                    lv1.SubItems.Add(jianchengs[i].Groups[1].Value);
                    for (int j = 0; j < mairu.Length; j++)
                    {    
                        lv1.SubItems.Add(mairu[j]);                  
                    }
                    for (int j = 0; j < maichu.Length; j++)
                    {
                        lv1.SubItems.Add(maichu[j]);
                    }

                }

                label3.Text = "获取完成";

            }


            catch (System.Exception ex)
            {

               ex.ToString();
            }

        }


        public void run1()
        {
            DateTime dt = DateTime.Parse(dateTimePicker1.Text);
            try
            {
                for (int j = 1; j < 20; j++)
                {



                    string Url = "http://www.szse.cn/api/report/ShowReport/data?SHOWTYPE=JSON&CATALOGID=1842_xxpl&TABKEY=tab1&PAGENO=" + j + "&txtStart=" + dt.ToString("yy-MM-dd") + "&txtEnd=" + dt.ToString("yy-MM-dd");

                    string strhtml = method.GetUrl(Url, "gb2312");
                    MatchCollection urls = Regex.Matches(strhtml, @"param='([\s\S]*?)'");
                    if (urls.Count == 0)
                    {
                        return;
                    }


                    for (int i = 0; i < urls.Count; i++)
                    {

                        string url2 = "http://www.szse.cn/api/report" + urls[i].Groups[1].Value;
                        string html = method.GetUrl(url2, "utf-8");

                        Match names = Regex.Match(html, @"""data"":([\s\S]*?)""zqjc"":""([\s\S]*?)&nbsp;");
                        Match code = Regex.Match(html, @"&nbsp;\(([\s\S]*?)\)");

                        Match mai1 = Regex.Match(html, @"买1"",""zsmc"":""([\s\S]*?)""");
                        Match mai2 = Regex.Match(html, @"买2"",""zsmc"":""([\s\S]*?)""");
                        Match mai3 = Regex.Match(html, @"买3"",""zsmc"":""([\s\S]*?)""");
                        Match mai4 = Regex.Match(html, @"买4"",""zsmc"":""([\s\S]*?)""");
                        Match mai5 = Regex.Match(html, @"买5"",""zsmc"":""([\s\S]*?)""");
                        Match sell1 = Regex.Match(html, @"卖1"",""zsmc"":""([\s\S]*?)""");
                        Match sell2 = Regex.Match(html, @"卖2"",""zsmc"":""([\s\S]*?)""");
                        Match sell3 = Regex.Match(html, @"卖3"",""zsmc"":""([\s\S]*?)""");
                        Match sell4 = Regex.Match(html, @"卖4"",""zsmc"":""([\s\S]*?)""");
                        Match sell5 = Regex.Match(html, @"卖5"",""zsmc"":""([\s\S]*?)""");


                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据      

                        lv1.SubItems.Add(code.Groups[1].Value);
                        lv1.SubItems.Add(names.Groups[2].Value);
                        lv1.SubItems.Add(mai1.Groups[1].Value.Replace("有限责任公司", "").Replace("股份有限公司", "").Replace("有限公司", "").Replace("证券营业部", "").Replace("营业部", ""));
                        lv1.SubItems.Add(mai2.Groups[1].Value.Replace("有限责任公司", "").Replace("股份有限公司", "").Replace("有限公司", "").Replace("证券营业部", "").Replace("营业部", ""));
                        lv1.SubItems.Add(mai3.Groups[1].Value.Replace("有限责任公司", "").Replace("股份有限公司", "").Replace("有限公司", "").Replace("证券营业部", "").Replace("营业部", ""));
                        lv1.SubItems.Add(mai4.Groups[1].Value.Replace("有限责任公司", "").Replace("股份有限公司", "").Replace("有限公司", "").Replace("证券营业部", "").Replace("营业部", ""));
                        lv1.SubItems.Add(mai5.Groups[1].Value.Replace("有限责任公司", "").Replace("股份有限公司", "").Replace("有限公司", "").Replace("证券营业部", "").Replace("营业部", ""));
                        lv1.SubItems.Add(sell1.Groups[1].Value.Replace("有限责任公司", "").Replace("股份有限公司", "").Replace("有限公司", "").Replace("证券营业部", "").Replace("营业部", ""));
                        lv1.SubItems.Add(sell2.Groups[1].Value.Replace("有限责任公司", "").Replace("股份有限公司", "").Replace("有限公司", "").Replace("证券营业部", "").Replace("营业部", ""));
                        lv1.SubItems.Add(sell3.Groups[1].Value.Replace("有限责任公司", "").Replace("股份有限公司", "").Replace("有限公司", "").Replace("证券营业部", "").Replace("营业部", ""));
                        lv1.SubItems.Add(sell4.Groups[1].Value.Replace("有限责任公司", "").Replace("股份有限公司", "").Replace("有限公司", "").Replace("证券营业部", "").Replace("营业部", ""));
                        lv1.SubItems.Add(sell5.Groups[1].Value.Replace("有限责任公司", "").Replace("股份有限公司", "").Replace("有限公司", "").Replace("证券营业部", "").Replace("营业部", ""));






                    }
                    label3.Text = "获取完成";

                }
            }
            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        
        }

        public void run2()
        {
           



            DateTime dt = DateTime.Parse(dateTimePicker1.Text);
            try
            {


                string Url = "http://acaiji.com/sse.php?date=" + dt.ToString("yyyyMMdd");

                string html = method.gethtml(Url, "", "utf-8");

                MatchCollection codes = Regex.Matches(html, @"""secCode"":""([\s\S]*?)""");
                MatchCollection jianchengs = Regex.Matches(html, @"""secAbbr"":""([\s\S]*?)""");
                MatchCollection mairus = Regex.Matches(html, @"""branchNameB"":""([\s\S]*?)""");
                MatchCollection maichus = Regex.Matches(html, @"""branchNameS"":""([\s\S]*?)""");

                for (int i = 0; i < codes.Count; i++)
                {
                    ArrayList lists = GetFiles(Path);
                    int geshu = 0;

                    string buy = mairus[i].Groups[1].Value.Replace("有限", "").Replace("责任", "").Replace("公司", "").Replace("股份", "").Replace("第一", "").Replace("第二", "").Replace("证券营业部", "");
                    string sell = maichus[i].Groups[1].Value.Replace("有限", "").Replace("责任", "").Replace("公司", "").Replace("股份", "").Replace("第一", "").Replace("第二", "").Replace("证券营业部", "");
                    string[] mairu = buy.Split(new string[] { "," }, StringSplitOptions.None);
                    string[] maichu = sell.Split(new string[] { "," }, StringSplitOptions.None);

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据         
                    lv1.SubItems.Add(codes[i].Groups[1].Value);
                    lv1.SubItems.Add(jianchengs[i].Groups[1].Value);
                    for (int a = 0; a < lists.Count; a++)
                    {
                        StreamReader sr = new StreamReader(lists[a].ToString(), Encoding.Default);
                        //一次性读取完 
                        string texts = sr.ReadToEnd();
                        
                        for (int j = 0; j < mairu.Length; j++)
                        {
                            lv1.SubItems.Add(mairu[j]);
                            if (texts.Contains(mairu[j]))
                            {
                             
                                geshu = geshu + 1;
                            }
                        }
                        for (int j = 0; j < maichu.Length; j++)
                        {
                            lv1.SubItems.Add(maichu[j]);
                            if (texts.Contains(maichu[j]))
                            {
                                geshu = geshu + 1;
                            }
                        }
                    }

                    if (geshu >1)
                    {
                       textBox1.Text += jianchengs[i].Groups[1].Value+"\r\n";
                    }

                }


                string aUrl = "http://reportdocs.static.szse.cn/files/text/jy/jy" + dt.ToString("yyMMdd") + ".txt";

                string ahtml = method.GetUrl(aUrl, "gb2312");
                MatchCollection names = Regex.Matches(ahtml, @".*\(代码");
                MatchCollection acodes = Regex.Matches(ahtml, @"\(代码.*\)");
                MatchCollection amairus = Regex.Matches(ahtml, @"买入金额最大的前5名([\s\S]*?)卖出金额最大的前5名");
                MatchCollection amaichus = Regex.Matches(ahtml, @"卖出金额最大的前5名([\s\S]*?)代码");

                for (int i = 0; i < acodes.Count; i++)
                {

                    ArrayList lists = GetFiles(Path);
                    int geshu = 0;
                 

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据      
                    lv1.SubItems.Add(acodes[i].Groups[0].Value.Replace("(代码", "").Replace(")", ""));
                    lv1.SubItems.Add(names[i].Groups[0].Value.Replace("(代码", ""));

                    MatchCollection buys = Regex.Matches(amairus[i].Groups[1].Value, @".*证券|.*专用");

                    for (int a = 0; a < lists.Count; a++)
                    {

                        StreamReader sr = new StreamReader(lists[a].ToString(), Encoding.Default);
                        //一次性读取完 
                        string texts = sr.ReadToEnd();
                        for (int j = 0; j < buys.Count; j++)
                        {
                            string buy = buys[j].Groups[0].Value.Remove(buys[j].Groups[0].Value.Length - 2, 2).Replace("有限", "").Replace("责任", "").Replace("公司", "").Replace("股份", "").Replace("第一", "").Replace("第二", "");
                            lv1.SubItems.Add(buy);

                            if (texts.Contains(buy))
                            {

                                geshu = geshu + 1;
                            }
                        }

                        if (i != acodes.Count - 1)

                        {
                            MatchCollection sells = Regex.Matches(amaichus[i].Groups[1].Value, @".*证券|.*专用");
                           
                            for (int j = 0; j < sells.Count; j++)
                            {
                                string sell = sells[j].Groups[0].Value.Remove(sells[j].Groups[0].Value.Length - 2, 2).Replace("有限", "").Replace("责任", "").Replace("公司", "").Replace("股份", "").Replace("第一", "").Replace("第二", "");
                                lv1.SubItems.Add(sell);
                                if (texts.Contains(sell))
                                {

                                    geshu = geshu + 1;
                                }
                            }
                        }
                    }

                    if (geshu > 1)
                    {
                        textBox1.Text += jianchengs[i].Groups[1].Value + "\r\n";
                    }

                }
                label3.Text = "获取完成";

            }


            catch (System.Exception ex)
            {

                ex.ToString();
            }

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "上海证券交易所")
            {
                label3.Text = "正在获取...";
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
            }
            else
            {
                label3.Text = "正在获取...";
                Thread thread = new Thread(new ThreadStart(run1));
                thread.Start();

            }



        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }
    }
}
