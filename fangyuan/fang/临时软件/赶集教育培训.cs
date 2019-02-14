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

namespace fang.临时软件
{
    public partial class 赶集教育培训 : Form
    {
        public 赶集教育培训()
        {
            InitializeComponent();
        }

        bool status = true;

        private void 赶集教育培训_Load(object sender, EventArgs e)
        {

        }


        public ArrayList ganjiCitys()
        {
            ArrayList citys = new ArrayList();
            string html = method.GetUrl("http://www.ganji.com/index.htm", "utf-8");
            MatchCollection matchs = Regex.Matches(html, @"<a href=""http://([\s\S]*?)\.", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            foreach (Match match in matchs)
            {
                if (!citys.Contains(match.Groups[1].Value))
                {
                    citys.Add(match.Groups[1].Value);
                }
               
            }

            return citys;

        }


        #region  赶集本地服务
        public void run()
        {


            ArrayList citys = ganjiCitys();


            string cate = "";


            switch (comboBox1.SelectedItem.ToString())
            {
                case "继续教育":
                    cate = "jixujiaoyurenzheng";
                    break;
                case "学历认证":
                    cate = "xuelirenzheng";
                    break;
                case "成人教育":
                    cate = "chengrenjiaoyu";
                    break;
                case "成人高考":
                    cate = "chengrengaokao";
                    break;
                case "专升本":
                    cate = "zhuanshengben";
                    break;
                case "远程教育":
                    cate = "yuanchengjiaoyu";
                    break;
                
                case "自考":
                    cate = "zikao";
                    break;


            }
                 
                  


            try
            {
                foreach (string city in citys)
                {

                    for (int i = 1; i < 63; i++)
                    {


                        string url = "http://"+city+".ganji.com/"+cate+"/o"+i+"/";

                        MessageBox.Show(url);
                        string html = method.GetUrl(url, "utf-8");

                        MatchCollection urls = Regex.Matches(html, @"<p class=""t clearfix"">([\s\S]*?)<a href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                       
                        ArrayList lists = new ArrayList();
                       
                        foreach (Match NextMatch in urls)
                        {
                            lists.Add(NextMatch.Groups[2].Value);

                        }

                        if (lists.Count == 0)

                            break;

                        foreach (string list in lists)
                        {
                            textBox1.Text = list;                        
                            string strhtml = method.GetUrl(list, "utf-8");
                          
                            Match title = Regex.Match(strhtml, @"【([\s\S]*?)】");
                            Match tel = Regex.Match(strhtml, @"@phone=([\s\S]*?)@");
                            Match lxr = Regex.Match(strhtml, @"联系人：</i>([\s\S]*?)</span>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                            Match company = Regex.Match(strhtml, @"<h1>([\s\S]*?)</h1>");

                            if (title.Groups[1].Value != "")
                            {
                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                                lv1.SubItems.Add(title.Groups[1].Value.Trim());
                                lv1.SubItems.Add(Regex.Replace(lxr.Groups[1].Value, "<[^>]*>", "").Trim());
                                lv1.SubItems.Add(tel.Groups[1].Value.Trim());

                                lv1.SubItems.Add(company.Groups[1].Value.Trim());

                                Application.DoEvents();
                                Thread.Sleep(Convert.ToInt32(1000));

                                if (listView1.Items.Count > 1)
                                {
                                    listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                                }

                                while (this.status == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                            }

                        }

                       

                    }

                }
            }


            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.status = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    
    }
}
