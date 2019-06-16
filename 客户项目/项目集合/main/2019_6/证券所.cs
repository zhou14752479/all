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


        public void jiankong()
        {
            //ArrayList lists = GetFiles(Path);
            //int geshu = 0;
            //for (int i = 0; i < listView1.Items.Count; i++)
            //{
                
            //    for (int j = 0; j < listView1.Columns.Count; j++)
            //    {
            //        if (lists.Contains(listView1.Items[i].SubItems[j].Text.Trim()))

            //        {
            //            geshu = geshu + 1;
            //        }
            //    }

            //    if (geshu > 2)
            //    {
            //        MessageBox.Show(listView1.Items[i].SubItems[1].Text+"符合要求");
            //    }
                
            //}
        }


        public static string Path=AppDomain.CurrentDomain.BaseDirectory;

        private void 证券所_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            timer1.Start();
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
                if (extension == ".htm")
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
                    string buy = mairus[i].Groups[1].Value.Replace("有限", "").Replace("责任", "").Replace("公司", "").Replace("股份", "").Replace("第一", "").Replace("第二", "").Replace("证券营业部", "");
                   string sell= maichus[i].Groups[1].Value.Replace("有限", "").Replace("责任", "").Replace("公司", "").Replace("股份", "").Replace("第一", "").Replace("第二", "").Replace("证券营业部", "");
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


                string Url = "http://reportdocs.static.szse.cn/files/text/jy/jy" + dt.ToString("yyMMdd") + ".txt";

                string html = method.GetUrl(Url, "gb2312");
                MatchCollection names = Regex.Matches(html, @".*\(代码");
                MatchCollection codes = Regex.Matches(html, @"\(代码.*\)");
                MatchCollection mairus = Regex.Matches(html, @"买入金额最大的前5名([\s\S]*?)卖出金额最大的前5名");
                MatchCollection maichus = Regex.Matches(html, @"卖出金额最大的前5名([\s\S]*?)代码");

                for (int i = 0; i < codes.Count; i++)
                {
                    



                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据      
                    lv1.SubItems.Add(codes[i].Groups[0].Value.Replace("(代码","").Replace(")",""));
                    lv1.SubItems.Add(names[i].Groups[0].Value.Replace("(代码", ""));

                    MatchCollection buys = Regex.Matches(mairus[i].Groups[1].Value, @".*证券|.*专用");

                   
                    for (int j = 0; j < buys.Count; j++)
                    {
                        lv1.SubItems.Add(buys[j].Groups[0].Value.Remove(buys[j].Groups[0].Value.Length - 2, 2).Replace("有限", "").Replace("责任", "").Replace("公司", "").Replace("股份", "").Replace("第一", "").Replace("第二", ""));
                    }

                    if (i != codes.Count - 1)

                    {
                        MatchCollection sells = Regex.Matches(maichus[i].Groups[1].Value, @".*证券|.*专用");
                        for (int j = 0; j < sells.Count; j++)
                        {
                            lv1.SubItems.Add(sells[j].Groups[0].Value.Remove(sells[j].Groups[0].Value.Length - 2, 2).Replace("有限", "").Replace("责任", "").Replace("公司", "").Replace("股份", "").Replace("第一", "").Replace("第二", ""));
                        }
                    }
                    //else if (i == codes.Count - 1)
                    //{
                    //    Match maichus1 = Regex.Match(html, @"卖出金额最大的前5名([\s\S]*?)其它异常");
                    //    MatchCollection sells = Regex.Matches(maichus1.Groups[1].Value, @".*证券|.*专用");
                    //    //for (int j = 0; j < sells.Count; j++)
                    //    //{
                    //    //    lv1.SubItems.Add(sells[j].Groups[0].Value.Remove(sells[j].Groups[0].Value.Length - 2, 2).Replace("有限", "").Replace("责任", "").Replace("公司", "").Replace("股份", "").Replace("第一", "").Replace("第二", ""));

                    //    //}

                    //    lv1.SubItems.Add(sells[sells.Count-5].Groups[0].Value.Remove(sells[sells.Count - 5].Groups[0].Value.Length - 2, 2).Replace("有限", "").Replace("责任", "").Replace("公司", "").Replace("股份", "").Replace("第一", "").Replace("第二", ""));
                    //    //lv1.SubItems.Add(sells[sells.Count -4].Groups[0].Value.Remove(sells[j].Groups[0].Value.Length - 2, 2).Replace("有限", "").Replace("责任", "").Replace("公司", "").Replace("股份", "").Replace("第一", "").Replace("第二", ""));
                    //    //lv1.SubItems.Add(sells[sells.Count -3].Groups[0].Value.Remove(sells[j].Groups[0].Value.Length - 2, 2).Replace("有限", "").Replace("责任", "").Replace("公司", "").Replace("股份", "").Replace("第一", "").Replace("第二", ""));
                    //    //lv1.SubItems.Add(sells[sells.Count -2].Groups[0].Value.Remove(sells[j].Groups[0].Value.Length - 2, 2).Replace("有限", "").Replace("责任", "").Replace("公司", "").Replace("股份", "").Replace("第一", "").Replace("第二", ""));
                    //    //lv1.SubItems.Add(sells[sells.Count -1].Groups[0].Value.Remove(sells[j].Groups[0].Value.Length - 2, 2).Replace("有限", "").Replace("责任", "").Replace("公司", "").Replace("股份", "").Replace("第一", "").Replace("第二", ""));
                    //}
                   
                }
                

            }
            catch (System.Exception ex)
            {

              MessageBox.Show  (ex.ToString());
            }

        }

        private void Button1_Click(object sender, EventArgs e)
        {

            if (comboBox1.Text == "上海证券交易所")
            {
                label3.Text = "正在获取上海证券交易所信息";           
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
            }
            else if (comboBox1.Text == "深圳证券交易所")
            {
                label3.Text = "正在获取深圳证券交易所信息";     
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            jiankong();
        }
    }
}
