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
using myDLL;

namespace 足球数据处理
{
    public partial class 足球数据处理 : Form
    {
        public 足球数据处理()
        {
            InitializeComponent();
        }

        public ArrayList getFileName()
        {
            ArrayList lists = new ArrayList();

            // string path = AppDomain.CurrentDomain.BaseDirectory;
            string path = textBox1.Text;
            DirectoryInfo folder = new DirectoryInfo(path);
            for (int i = 0; i < folder.GetFiles().Count(); i++)
            {
                lists.Add(folder.GetFiles()[i].FullName);
            }
            return lists;
        }

        public void run()
        {
            ArrayList lists = getFileName();
            for (int i = 0; i < lists.Count; i++)
            {
                string aname = Regex.Match(lists[i].ToString(), @"co_user=([\s\S]*?)&").Groups[1].Value;
                string bname = Regex.Match(lists[i].ToString(), @"su_user=([\s\S]*?)&").Groups[1].Value;
                string cname = Regex.Match(lists[i].ToString(), @"ag_user=([\s\S]*?)&").Groups[1].Value;
                string dname = Regex.Match(lists[i].ToString(), @"mem_user=([\s\S]*?)&").Groups[1].Value;
                int bishu = 0;
                double zongjine = 0;
                double yingshu = 0;
                double shenglv = 0;
                double yingshubi = 0;
                double pingjundanbi= 0;


                double zongshuiwei = 0;
                double pingjunshuiwei = 0;
                int xiaoyu07 = 0;
                int dayu12 = 0;

                double ying = 0;
                double shu = 0;
                double fan180 = 0;
                StreamReader sr = new StreamReader(lists[i].ToString(), method.EncodingType.GetTxtType(lists[i].ToString()));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\n" }, StringSplitOptions.None);

             
                for (int j = 0; j < text.Length; j++)//每一行
                {
                    string[] text1 = text[j].ToString().Split(new string[] { "\t" }, StringSplitOptions.None);

                   
                    //每条数据分割

                    //计算总比数
                    if (text1.Length > 6)
                    {
                        if (text1[4].ToString()==("足球") && text1[6].ToString().Trim() != ("注单平局") && text1[6].ToString().Trim() != ("投注失败"))
                        {

                            if (text1[3].ToString().Contains("让球") || text1[3].ToString().Contains("大 / 小"))
                            {
                                double shuiwei = Convert.ToDouble(Regex.Replace(text1[2].ToString().Replace(",", ""), "<[^>]+>", ""));
                                if (shuiwei > 0)
                                {
                                    if (shuiwei < 0.7)
                                    {
                                        xiaoyu07 = xiaoyu07 + 1;
                                        continue;

                                    }

                                    if (shuiwei > 1.2)
                                    {
                                        dayu12 = dayu12 + 1;
                                        continue;
                                    }
                                }

                                if (shuiwei < 0)
                                {
                                   

                                    if (shuiwei > -0.7)
                                    {
                                        xiaoyu07 = xiaoyu07 + 1;
                                        continue;
                                    }

                                    if (shuiwei < -1.2)
                                    {
                                        dayu12 = dayu12 + 1;
                                        continue;
                                    }
                                }




                                bishu = bishu + 1;
                                double danbijine = Convert.ToDouble(Regex.Replace(text1[5].ToString().Replace(",", ""), "<[^>]+>", ""));
                                zongjine = zongjine + danbijine;  //计算总金额
                                if (text1[6].ToString().Contains(".")) //计算赢输
                                {
                                    double danbiyingshu = Convert.ToDouble(Regex.Replace(text1[6].ToString().Replace(",", ""), "<[^>]+>", ""));
                                   

                                    yingshu =yingshu+ danbiyingshu;
                                    yingshubi = yingshu / zongjine;
                                    if(danbiyingshu>0)
                                    {
                                        ying = ying + 1;
                                        fan180 = fan180 + (danbijine * (-1));
                                      }

                                    if (danbiyingshu < 0)
                                    {
                                        shu = shu + 1;
                                        if (shuiwei>0)
                                        {
                                           
                                            fan180 = fan180 + (danbijine * (1.8 - shuiwei));
                                        }
                                        if (shuiwei < 0)
                                        {
                                            fan180 = fan180 + (danbijine * (1.8 - (shuiwei*-1)));
                                        }


                                    }


                                    if (shuiwei > 0)
                                    {
                                        zongshuiwei = zongshuiwei + shuiwei;
                                      
                                    }

                                    if (shuiwei < 0)
                                    {
                                        zongshuiwei = zongshuiwei - shuiwei;

                                       
                                    }

                                   
                                }


                               

                            }

                          
                        }

                        
                    }
                

                }


                try
                {
                   
                    pingjunshuiwei = Math.Round(zongshuiwei / bishu, 3);

                    shenglv = Math.Round(ying/ (ying + shu), 3); 
                    


                    double a2 = zongjine / bishu;
                    pingjundanbi = Math.Round(a2, 3);
                }
                catch (Exception)
                {
                    continue;
                }

                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                lv1.SubItems.Add(aname);
                lv1.SubItems.Add(bname);
                lv1.SubItems.Add(cname);
                lv1.SubItems.Add(dname);
                lv1.SubItems.Add(bishu.ToString());
                lv1.SubItems.Add(zongjine.ToString());
                lv1.SubItems.Add(yingshu.ToString());
                lv1.SubItems.Add(shenglv.ToString());
                lv1.SubItems.Add(Math.Round(yingshubi,3).ToString());
                lv1.SubItems.Add(pingjundanbi.ToString());
                lv1.SubItems.Add(fan180.ToString());
                lv1.SubItems.Add(pingjunshuiwei.ToString());
                lv1.SubItems.Add(xiaoyu07.ToString());
                lv1.SubItems.Add(dayu12.ToString());
                // MessageBox.Show("");
            }

        }


        public void run1()
        {
            ArrayList lists = getFileName();
            for (int i = 0; i < lists.Count; i++)
            {
                string aname = Regex.Match(lists[i].ToString(), @"co_user=([\s\S]*?)&").Groups[1].Value;
                string bname = Regex.Match(lists[i].ToString(), @"su_user=([\s\S]*?)&").Groups[1].Value;
                string cname = Regex.Match(lists[i].ToString(), @"ag_user=([\s\S]*?)&").Groups[1].Value;
                string dname = Regex.Match(lists[i].ToString(), @"mem_user=([\s\S]*?)&").Groups[1].Value;
                int bishu = 0;
                double zongjine = 0;
                double yingshu = 0;
                double shenglv = 0;
                double yingshubi = 0;
                double pingjundanbi = 0;


                double zongshuiwei = 0;
                double pingjunshuiwei = 0;
                int xiaoyu07 = 0;
                int dayu12 = 0;

                double ying = 0;
                double shu = 0;
                double fan180 = 0;
                StreamReader sr = new StreamReader(lists[i].ToString(), method.EncodingType.GetTxtType(lists[i].ToString()));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\n" }, StringSplitOptions.None);


                for (int j = 0; j < text.Length; j++)//每一行
                {
                    string[] text1 = text[j].ToString().Split(new string[] { "\t" }, StringSplitOptions.None);


                    //每条数据分割

                    //计算总比数
                    if (text1.Length > 6)
                    {
                        if (text1[4].ToString() == ("足球") && text1[6].ToString().Trim() != ("注单平局") && text1[6].ToString().Trim() != ("投注失败"))
                        {

                            if (text1[3].ToString().Contains("让球") || text1[3].ToString().Contains("大 / 小"))
                            {
                                if (text1[3].ToString().Contains("滚球"))
                                {
                                    double shuiwei = Convert.ToDouble(Regex.Replace(text1[2].ToString().Replace(",", ""), "<[^>]+>", ""));
                                    if (shuiwei > 0)
                                    {
                                        if (shuiwei < 0.7)
                                        {
                                            xiaoyu07 = xiaoyu07 + 1;
                                            continue;

                                        }

                                        if (shuiwei > 1.2)
                                        {
                                            dayu12 = dayu12 + 1;
                                            continue;
                                        }
                                    }

                                    if (shuiwei < 0)
                                    {


                                        if (shuiwei > -0.7)
                                        {
                                            xiaoyu07 = xiaoyu07 + 1;
                                            continue;
                                        }

                                        if (shuiwei < -1.2)
                                        {
                                            dayu12 = dayu12 + 1;
                                            continue;
                                        }
                                    }




                                    bishu = bishu + 1;
                                    double danbijine = Convert.ToDouble(Regex.Replace(text1[5].ToString().Replace(",", ""), "<[^>]+>", ""));
                                    zongjine = zongjine + danbijine;  //计算总金额
                                    if (text1[6].ToString().Contains(".")) //计算赢输
                                    {
                                        double danbiyingshu = Convert.ToDouble(Regex.Replace(text1[6].ToString().Replace(",", ""), "<[^>]+>", ""));


                                        yingshu = yingshu + danbiyingshu;
                                        yingshubi = yingshu / zongjine;
                                        if (danbiyingshu > 0)
                                        {
                                            ying = ying + 1;
                                            fan180 = fan180 + (danbijine * (-1));
                                        }

                                        if (danbiyingshu < 0)
                                        {
                                            shu = shu + 1;
                                            if (shuiwei > 0)
                                            {

                                                fan180 = fan180 + (danbijine * (1.8 - shuiwei));
                                            }
                                            if (shuiwei < 0)
                                            {
                                                fan180 = fan180 + (danbijine * (1.8 - (shuiwei * -1)));
                                            }


                                        }


                                        if (shuiwei > 0)
                                        {
                                            zongshuiwei = zongshuiwei + shuiwei;

                                        }

                                        if (shuiwei < 0)
                                        {
                                            zongshuiwei = zongshuiwei - shuiwei;


                                        }


                                    }
                                }



                            }


                        }


                    }


                }


                try
                {

                    pingjunshuiwei = Math.Round(zongshuiwei / bishu, 3);

                    shenglv = Math.Round(ying / (ying + shu), 3);



                    double a2 = zongjine / bishu;
                    pingjundanbi = Math.Round(a2, 3);
                }
                catch (Exception)
                {
                    continue;
                }

                ListViewItem lv1 = listView2.Items.Add(listView2.Items.Count.ToString()); //使用Listview展示数据
                lv1.SubItems.Add(aname);
                lv1.SubItems.Add(bname);
                lv1.SubItems.Add(cname);
                lv1.SubItems.Add(dname);
                lv1.SubItems.Add(bishu.ToString());
                lv1.SubItems.Add(zongjine.ToString());
                lv1.SubItems.Add(yingshu.ToString());
                lv1.SubItems.Add(shenglv.ToString());
                lv1.SubItems.Add(Math.Round(yingshubi, 3).ToString());
                lv1.SubItems.Add(pingjundanbi.ToString());
                lv1.SubItems.Add(fan180.ToString());
                lv1.SubItems.Add(pingjunshuiwei.ToString());
                lv1.SubItems.Add(xiaoyu07.ToString());
                lv1.SubItems.Add(dayu12.ToString());
                // MessageBox.Show("");
            }

        }


        public void run2()
        {
            ArrayList lists = getFileName();
            for (int i = 0; i < lists.Count; i++)
            {
                string aname = Regex.Match(lists[i].ToString(), @"co_user=([\s\S]*?)&").Groups[1].Value;
                string bname = Regex.Match(lists[i].ToString(), @"su_user=([\s\S]*?)&").Groups[1].Value;
                string cname = Regex.Match(lists[i].ToString(), @"ag_user=([\s\S]*?)&").Groups[1].Value;
                string dname = Regex.Match(lists[i].ToString(), @"mem_user=([\s\S]*?)&").Groups[1].Value;
                int bishu = 0;
                double zongjine = 0;
                double yingshu = 0;
                double shenglv = 0;
                double yingshubi = 0;
                double pingjundanbi = 0;


                double zongshuiwei = 0;
                double pingjunshuiwei = 0;
                int xiaoyu07 = 0;
                int dayu12 = 0;

                double ying = 0;
                double shu = 0;
                double fan180 = 0;
                StreamReader sr = new StreamReader(lists[i].ToString(), method.EncodingType.GetTxtType(lists[i].ToString()));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\n" }, StringSplitOptions.None);


                for (int j = 0; j < text.Length; j++)//每一行
                {
                    string[] text1 = text[j].ToString().Split(new string[] { "\t" }, StringSplitOptions.None);


                    //每条数据分割

                    //计算总比数
                    if (text1.Length > 6)
                    {
                        if (text1[4].ToString() == ("足球") && text1[6].ToString().Trim() != ("注单平局") && text1[6].ToString().Trim() != ("投注失败"))
                        {

                            if (text1[3].ToString().Contains("让球") || text1[3].ToString().Contains("大 / 小"))
                            {
                                if (!text1[3].ToString().Contains("滚球"))
                                {
                                    double shuiwei = Convert.ToDouble(Regex.Replace(text1[2].ToString().Replace(",", ""), "<[^>]+>", ""));
                                    if (shuiwei > 0)
                                    {
                                        if (shuiwei < 0.7)
                                        {
                                            xiaoyu07 = xiaoyu07 + 1;
                                            continue;

                                        }

                                        if (shuiwei > 1.2)
                                        {
                                            dayu12 = dayu12 + 1;
                                            continue;
                                        }
                                    }

                                    if (shuiwei < 0)
                                    {


                                        if (shuiwei > -0.7)
                                        {
                                            xiaoyu07 = xiaoyu07 + 1;
                                            continue;
                                        }

                                        if (shuiwei < -1.2)
                                        {
                                            dayu12 = dayu12 + 1;
                                            continue;
                                        }
                                    }




                                    bishu = bishu + 1;
                                    double danbijine = Convert.ToDouble(Regex.Replace(text1[5].ToString().Replace(",", ""), "<[^>]+>", ""));
                                    zongjine = zongjine + danbijine;  //计算总金额
                                    if (text1[6].ToString().Contains(".")) //计算赢输
                                    {
                                        double danbiyingshu = Convert.ToDouble(Regex.Replace(text1[6].ToString().Replace(",", ""), "<[^>]+>", ""));


                                        yingshu = yingshu + danbiyingshu;
                                        yingshubi = yingshu / zongjine;
                                        if (danbiyingshu > 0)
                                        {
                                            ying = ying + 1;
                                            fan180 = fan180 + (danbijine * (-1));
                                        }

                                        if (danbiyingshu < 0)
                                        {
                                            shu = shu + 1;
                                            if (shuiwei > 0)
                                            {

                                                fan180 = fan180 + (danbijine * (1.8 - shuiwei));
                                            }
                                            if (shuiwei < 0)
                                            {
                                                fan180 = fan180 + (danbijine * (1.8 - (shuiwei * -1)));
                                            }


                                        }


                                        if (shuiwei > 0)
                                        {
                                            zongshuiwei = zongshuiwei + shuiwei;

                                        }

                                        if (shuiwei < 0)
                                        {
                                            zongshuiwei = zongshuiwei - shuiwei;


                                        }


                                    }
                                }



                            }


                        }


                    }


                }


                try
                {

                    pingjunshuiwei = Math.Round(zongshuiwei / bishu, 3);

                    shenglv = Math.Round(ying / (ying + shu), 3);



                    double a2 = zongjine / bishu;
                    pingjundanbi = Math.Round(a2, 3);
                }
                catch (Exception)
                {
                    continue;
                }

                ListViewItem lv1 = listView3.Items.Add(listView3.Items.Count.ToString()); //使用Listview展示数据
                lv1.SubItems.Add(aname);
                lv1.SubItems.Add(bname);
                lv1.SubItems.Add(cname);
                lv1.SubItems.Add(dname);
                lv1.SubItems.Add(bishu.ToString());
                lv1.SubItems.Add(zongjine.ToString());
                lv1.SubItems.Add(yingshu.ToString());
                lv1.SubItems.Add(shenglv.ToString());
                lv1.SubItems.Add(Math.Round(yingshubi, 3).ToString());
                lv1.SubItems.Add(pingjundanbi.ToString());
                lv1.SubItems.Add(fan180.ToString());
                lv1.SubItems.Add(pingjunshuiwei.ToString());
                lv1.SubItems.Add(xiaoyu07.ToString());
                lv1.SubItems.Add(dayu12.ToString());
                // MessageBox.Show("");
            }

        }

        private void 足球数据处理_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择所在文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }

                textBox1.Text = dialog.SelectedPath;
            }
        }
        Thread thread;
        private void button2_Click(object sender, EventArgs e)
        {
            if (DateTime.Now > Convert.ToDateTime("2021-08-31"))
            {
                return;
            }
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
            method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
            method.DataTableToExcel(method.listViewToDataTable(this.listView3), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (DateTime.Now > Convert.ToDateTime("2021-08-31"))
            {
                return;
            }
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run1);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

                thread = new Thread(run2);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

        }
    }
}
