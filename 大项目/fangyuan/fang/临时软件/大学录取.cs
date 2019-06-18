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

namespace fang.临时软件
{
    public partial class 大学录取 : Form
    {
        public 大学录取()
        {
            InitializeComponent();
        }

        private void 大学录取_Load(object sender, EventArgs e)
        {

        }
        public string[] ReadText()
        {
          
            StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.Default);
            string text = streamReader.ReadToEnd();
            return text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
        }
        #region  主函数
        public void run()

        {
            string[] array = this.ReadText();

            try

            {
                foreach (string cid in array)
                {

                   
                    for (int i = 0; i < 2; i = i++)
                    {
                        MessageBox.Show(i.ToString());

                        for (int j = 1; j < 3; j++)
                        {
                            string url = "http://www.diyigaokao.com/college/detail/getMajorData?_csrf=92ef980b-ce0e-463a-8ea9-1e12cac6c0d8";

                            string postdata = "collegeId="+cid+ "&provinceName=%e5%b1%b1%e4%b8%9c&year=2018&liberalScience=" + i+"&phase="+j;
                            string cookie = "mediav=%7B%22eid%22%3A%22254059%22%2C%22ep%22%3A%22%22%2C%22vid%22%3A%22B%3Cb4Nt.xGs%3ApMdDngS5F%22%2C%22ctn%22%3A%22%22%7D; OrderSource=baidu; OrderSourceReferrer=httpswww.baidu.comlinkurlzvx5sOsb-Kht9p0NtGcnLXhqoF7RiNn1LHs8Jg4Jn8n_2dIMnHNTz-95drEmzNeA&wd&eqid8db0727000020f2f000000055d08bfce; OrderSourceSearchKey=; pid=10; JSESSIONID=AE1F820715892F51FCB9E21A8E17353DqTa9Gr; Qs_lvt_160906=1560854481; mediav=%7B%22eid%22%3A%22254059%22%2C%22ep%22%3A%22%22%2C%22vid%22%3A%22B%3Cb4Nt.xGs%3ApMdDngS5F%22%2C%22ctn%22%3A%22%22%7D; Hm_lvt_c03cf607642b008df6b5f827619af522=1560854481; LXB_REFER=www.baidu.com; UserLiberalScience_857611=2; Hm_lpvt_c03cf607642b008df6b5f827619af522=1560854692; Qs_pv_160906=84141441536369570%2C2393693486794656000%2C1418837622324628700%2C2673027950459158000%2C522958230636996740; SERVERID=bc33e5b345c20d4b5f528d285080ad29|1560855367|1560854481";
                            string html = method.PostUrl(url, postdata, cookie, "utf-8");
                           

                            Match  daxue = Regex.Match(html, @"data-liberalscience=""([\s\S]*?)""title=""([\s\S]*?)录取");
                            MatchCollection zhuanyes = Regex.Matches(html, @"]&nbsp;([\s\S]*?)</strong>");
                            MatchCollection zuigaos = Regex.Matches(html, @"score_max s3"">([\s\S]*?)</td>");
                            MatchCollection pingjuns = Regex.Matches(html, @"score_avg s3""><em><strong>([\s\S]*?)</strong>");
                            MatchCollection zuidis = Regex.Matches(html, @"LowestScore s4"">([\s\S]*?)</td>");
                            
                            MatchCollection weicis = Regex.Matches(html, @"<td class=""s4"">([\s\S]*?)</td>");
                            MatchCollection luqus = Regex.Matches(html, @"data-collegeid=""([\s\S]*?)""");

                            
                            


                            for (int z = 0; z < zhuanyes.Count; z++)
                            {

                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                                lv1.SubItems.Add(daxue.Groups[2].Value);
                                lv1.SubItems.Add(zhuanyes[z].Groups[1].Value.Trim());
                                lv1.SubItems.Add(zuigaos[z].Groups[1].Value);
                                lv1.SubItems.Add(zuidis[z].Groups[1].Value);
                                lv1.SubItems.Add(pingjuns[z].Groups[1].Value);
                                lv1.SubItems.Add(weicis[z].Groups[1].Value);
                                lv1.SubItems.Add(luqus[z].Groups[1].Value);

                              
                                if ( i== 0)
                                {
                                    lv1.SubItems.Add("文科");
                                }
                                else if (i==1)
                                {
                                    lv1.SubItems.Add("理科");
                                }
                                if (j == 1)
                                {
                                    lv1.SubItems.Add("本科");
                                }
                                else if (j == 2)
                                {
                                    lv1.SubItems.Add("高职专科");
                                }

                                if (listView1.Items.Count - 1 > 0)
                                {
                                    listView1.EnsureVisible(listView1.Items.Count - 1);
                                }

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

        private void button2_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(this.run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }
    }
}
