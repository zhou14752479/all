using System;
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
using helper;

namespace 浙江移动
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        bool zanting = true;
        #region  主程序
        public void run()
        {
            try

            {
                StreamReader sr = new StreamReader(textBox4.Text, Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                sr.Close();
                foreach (string  urls in text)
                {
                    string[] url = urls.Split(new string[] { "," }, StringSplitOptions.None);

                    Match suiteId = Regex.Match(url[0], @"prepayInfoId=.*");

                  
                    for (int i = 0; i < 9999; i++)
                    {

                        string URL = "http://wap.zj.10086.cn/shop/shop/goods/contractNumber/queryIndex.do?cityid=402881ea3286d488013286d756720002&currentPageNum=" + i + "&span1=&span2=&span3=&span4=&span5=&fuzzySpan=&span6=&span7=&span8=&span9=&span10=&teleCodePer=&suiteId="+ suiteId.Groups[0].Value .Replace("prepayInfoId=","").Trim() + "&priceRangeId=&baseFeeId=&numRuleId=&orderBy=&isNofour=N&pageCount=100";
                        textBox2.Text = URL;
                        MessageBox.Show("1");

                        string html = method.GetUrl(URL, "utf-8");




                        MatchCollection a1s = Regex.Matches(html, @"""teleCode"":([\s\S]*?),");  //手机号
                        MatchCollection a2s = Regex.Matches(html, @"""deposits"":([\s\S]*?),");  //预存
                        MatchCollection a3s = Regex.Matches(html, @"""rulePrice"":([\s\S]*?),");   //卡费
                        MatchCollection a4s = Regex.Matches(html, @"""ruleBaseFee"":([\s\S]*?),");  //保底
                        MatchCollection a5s = Regex.Matches(html, @"""inLen"":([\s\S]*?),");

                        if (a1s.Count == 0)
                            break;


                        for (int j = 0; j < a1s.Count; j++)
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据      
                            lv1.SubItems.Add(a1s[j].Groups[1].Value.Replace("\"", ""));
                            lv1.SubItems.Add(a2s[j].Groups[1].Value.Replace("\"", ""));
                            lv1.SubItems.Add(a3s[j].Groups[1].Value.Replace("\"", ""));
                            lv1.SubItems.Add(a4s[j].Groups[1].Value.Replace("\"", ""));
                            lv1.SubItems.Add(a5s[j].Groups[1].Value.Replace("\"", ""));
                            lv1.SubItems.Add(url[1]);

                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }

                        }

                    }



                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion
        private void Button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox4.Text = openFileDialog1.FileName;
            }
        }
    }
}
