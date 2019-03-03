using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _58.临时软件
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false; // 设置线程之间可以操作
        }

        bool zanting = true;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        #region  主函数
        public void run()
        {
            Method md = new Method();



            try
            {

                for (int i = 1; i < 314; i++)

                {
                    string cookie = "acw_tc=7b39758815466573274386325e5f2ac38e78158f9ebcfbdc101f6d03e5f886; qimo_seosource_be759b80-2db1-11e8-9292-47bcc2cedaa9=%E7%AB%99%E5%86%85; qimo_seokeywords_be759b80-2db1-11e8-9292-47bcc2cedaa9=; href=https%3A%2F%2Fwww.chinabidding.cn%2Fzbxx%2Fzbgg%2F; accessId=be759b80-2db1-11e8-9292-47bcc2cedaa9; Hm_lvt_0bf7d2e4ce4104fa77e95b012f750771=1546657321; bad_idbe759b80-2db1-11e8-9292-47bcc2cedaa9=49fff201-1096-11e9-b7ca-63b147ed2008; nice_idbe759b80-2db1-11e8-9292-47bcc2cedaa9=49fff202-1096-11e9-b7ca-63b147ed2008; 49BAC005-7D5B-4231-8CEA-96739BEACD67=wopu2018; Hm_lpvt_0bf7d2e4ce4104fa77e95b012f750771=1546657357; pageViewNum=3; SERVERID=415587c0a4474a99267e61243891a333|1546657368|1546657327; CBL_SESSION=35852aa35bbd5049cf824bfee33e5e3b2dc9a16d-___TS=1582657368473&___ID=2310ea41-1fa1-4e98-a74e-1af0a52c7c1e; u_asec=099%23KAFE8EEKEjIEhYTLEEEEEpEQz0yFD6zEDuRoa6gJDXiMW6fHSXiqn6N1DEFETcZdt9wuE7EFL2xhGuGTE1LP%2F3iSlllP%2FcZddn3lluASsyaaolllWMiP%2F36alllzOcZddn3llu8bsYFETrudt%2F95QHGTEELP%2F3kf26XwnoMTEJL5lG14aquYSpXfNVX3LWc6PtuS6Gzp%2Ff8GZiXa3MeWN2sqbs6t1WacVzqB6qd6r61YDw%2BV7JnsLp35HsJqwROyPPdCqYA6bVZnZ0XAxbZnZ9rkbO0p1W2Ir6XaPFDtr6XaZLX9wSWQg9rTqycR9yJKPUxWHda3rfBUE7Tx1qJGEHq0pu5GP8TCk7YWQahv0JmfvhBDyUQAqNf3yUQC%2B1V%2BhNgVN8xA0FEf18Wccrmop7P05c7a%2BwD4D4GV%2BHkGkLHqmCXG5c7CBFt05c7W%2BFMTEEyPP3iSlltXE7EFD67EEE%3D%3D";

                    String Url = "https://www.chinabidding.cn/zbxx/zbgg/" + i + ".html";
                    string strhtml = Method.GetUrlwithCookie(Url, cookie);  //定义的GetRul方法 返回 reader.ReadToEnd()

                    string Rxg = @"href=""/zbgg/([\s\S]*?)""";



                    MatchCollection all = Regex.Matches(strhtml, Rxg);

                    ArrayList lists = new ArrayList();
                    foreach (Match NextMatch in all)
                    {
                        lists.Add("https://www.chinabidding.cn/zbgg/" + NextMatch.Groups[1].Value);

                    }
                    
                    if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;

                    string Rxg0 = @"招标编号：([\s\S]*?)</td>";
                    string Rxg1 = @"开标时间：([\s\S]*?)</td>";
                    string Rxg2 = @"招标代理：([\s\S]*?)</td>";
                    string Rxg3 = @"资金来源：([\s\S]*?)</td>";
                    string Rxg4 = @"招标编码：([\s\S]*?)</td>";
                    string Rxg5 = @"标讯类别：([\s\S]*?)</td>";
                    string Rxg6 = @"招标人：([\s\S]*?)</td>";


                    foreach (string list in lists)


                    {

                        string strhtml1 = Method.GetUrlwithCookie(list, cookie);  //定义的GetRul方法 返回 reader.ReadToEnd()

                        Match a0 = Regex.Match(strhtml1, Rxg0);
                        Match a1 = Regex.Match(strhtml1, Rxg1);
                        Match a2 = Regex.Match(strhtml1, Rxg2);
                        Match a3 = Regex.Match(strhtml1, Rxg3);
                        Match a4 = Regex.Match(strhtml1, Rxg4);
                        Match a5 = Regex.Match(strhtml1, Rxg5);
                        Match a6 = Regex.Match(strhtml1, Rxg6);


                        int index = this.dataGridView1.Rows.Add();    //利用dataGridView1.Rows.Add()事件为DataGridView控件增加新的行，该函数返回添加新行的索引号，即新行的行号，然后可以通过该索引号操作该行的各个单元格，如dataGridView1.Rows[index].Cells[0].Value = "1"。这是很常用也是很简单的方法。

                        this.dataGridView1.Rows[index].Cells[0].Value = index;
                        this.dataGridView1.Rows[index].Cells[1].Value = a0.Groups[1].Value;


                        this.dataGridView1.Rows[index].Cells[2].Value = a1.Groups[1].Value;

                        this.dataGridView1.Rows[index].Cells[3].Value = Regex.Replace(a2.Groups[1].Value, "<.*>", "");

                        this.dataGridView1.Rows[index].Cells[4].Value = a3.Groups[1].Value;
                        this.dataGridView1.Rows[index].Cells[5].Value = a4.Groups[1].Value;
                        this.dataGridView1.Rows[index].Cells[6].Value = a5.Groups[1].Value;

                        this.dataGridView1.Rows[index].Cells[3].Value = a6.Groups[1].Value;


                        this.dataGridView1.CurrentCell = this.dataGridView1.Rows[index].Cells[0];  //让datagridview滚动到当前行


                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }



                        Application.DoEvents();
                        Thread.Sleep(1000);

                    }

                }

            }

            catch (System.Exception ex)
            {
                MessageBox.Show( ex.ToString());
            }
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));

            thread.Start();
        }
    }
}
