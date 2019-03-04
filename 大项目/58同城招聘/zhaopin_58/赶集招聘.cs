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

namespace zhaopin_58
{
    public partial class 赶集招聘 : Form
    {
        public 赶集招聘()
        {
            InitializeComponent();
        }

        public ArrayList ganjiCitys()
        {
            ArrayList citys = new ArrayList();
            string html = method.GetUrl("http://www.ganji.com/index.htm");
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

        bool zanting = true;

        #region  获取

        public void zhaopin1()
        {

          
          try
            {

                //ArrayList citys = ganjiCitys();
                string[] keywords = { "zpshichangyingxiao","zpdianhuaxiaoshou", "zpjigongyibangongren", "zpxingzhenghouqin", "zprenliziyuan", "zpjiudiancanyin", "zpkefu", "zptaobao", "zpjiazhenganbao", "zpfangjingjiren", "zpmeirongmeifazhiwei", "zpmaoyiyunshu", "zpruanjianhulianwang" };

                //foreach (string city in citys)
                //{
                    
                   
                    foreach (string keyword in keywords)
                    {

                        if (keyword == "")
                        {
                            MessageBox.Show("请输入采集行业或者关键词！");
                            return;
                        }

                        for (int i = 1; i < 71; i++)
                        {

                            string Url = "http://sh.ganji.com/"+keyword+"/o"+i+"/";
                        string html = method.GetUrl(Url); 

                            MatchCollection TitleMatchs = Regex.Matches(html, @"post_id=([\s\S]*?)puid=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                            ArrayList lists = new ArrayList();

                            foreach (Match NextMatch in TitleMatchs)
                            {

                                lists.Add(NextMatch.Groups[2].Value);
                            }
                            if (lists.Count == 0)

                                break;


                        foreach (string list in lists)

                        {
                            string URL = "https://3g.ganji.com/sh_" + keyword + "/" + list + "x";
                            string strhtml = method.GetUrl(URL); ;  //定义的GetRul方法 返回 reader.ReadToEnd()


                            string rxg = @"class=""title"">([\s\S]*?)</h1>";
                            string rxg1 = @"content=""【([\s\S]*?)】";    //公司                              
                            string rxg2 = @"业</th><td>([\s\S]*?)</a>";
                            string rxg3 = @"地点</th><td>([\s\S]*?)</td>";
                            string rxg4 = @"联系人</th><td>([\s\S]*?)</td>";
                            string rxg5 = @"&phone=([\s\S]*?)&";




                            Match name = Regex.Match(strhtml, rxg);
                            Match company = Regex.Match(strhtml, rxg1);
                            Match hangye = Regex.Match(strhtml, rxg2);
                            Match addr = Regex.Match(strhtml, rxg3);
                            Match lxr = Regex.Match(strhtml, rxg4);
                            Match tel = Regex.Match(strhtml, rxg5);

                                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                                        lv1.SubItems.Add(name.Groups[1].Value.Trim());
                            lv1.SubItems.Add(company.Groups[1].Value.Trim());
                            lv1.SubItems.Add(Regex.Replace(hangye.Groups[1].Value, "<a.*>", ""));
                            lv1.SubItems.Add(addr.Groups[1].Value.Trim());


                            lv1.SubItems.Add(lxr.Groups[1].Value.Trim());
                            lv1.SubItems.Add(tel.Groups[1].Value.Trim());
                           

                            if (listView1.Items.Count - 1 > 1)
                                    {
                                        listView1.EnsureVisible(listView1.Items.Count - 1);
                                    }

                                    while (this.zanting == false)
                                    {
                                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                    }
                                    
                                
                            }

                        }
                    }
                

            }
            catch (System.Exception ex)
            {
              MessageBox.Show(  ex.ToString());
            }
        }


        #endregion

        private void 赶集招聘_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(zhaopin1));
            thread.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
