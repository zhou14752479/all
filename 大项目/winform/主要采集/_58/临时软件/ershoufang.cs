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
    public partial class ershoufang : Form
    {
        public ershoufang()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }
        #region  58二手房
        public void run()
        {



            try
            {
                string[] citys = {"sh","nj","wx","su","nt","suqian","xz","cz","yancheng","ha","lyg","taizhou","gz","sz",
                    "hz","nb","wz","jh","jx","tz","sx","huzhou","lishui","quzhou","zhoushan"

                };

                foreach (string city in citys)
                {
                    String url = "http://" + city + ".58.com/ershoufang/0/";
                    string strhtml = Method.GetUrl(url);

                    MatchCollection areas = Regex.Matches(strhtml, @"_area_([\s\S]*?)'", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    if (areas.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;
                    foreach (Match area in areas)
                    {

                        for (int i = 1; i < 71; i++)
                        {

                            String Url = "http://" + city + ".58.com/" + area.Groups[1].Value + "/ershoufang/0/pn" + i + "/";
                            string html = Method.GetUrl(Url);
                            MatchCollection TitleMatchs = Regex.Matches(html, @"<li logr=""([\s\S]*?)_0_([\s\S]*?)_([\s\S]*?)_", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                            ArrayList lists = new ArrayList();
                            
                            foreach (Match NextMatch in TitleMatchs)
                            {

                                lists.Add("https://"+city+".58.com/ershoufang/" + NextMatch.Groups[3].Value+ "x.shtml");
                            }


                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;


                            foreach (string list in lists)
                            {

                                string strhtml2 = Method.GetUrl(list);                                                                                //定义的GetRul方法 返回 reader.ReadToEnd()
                                                                             //请求手机端网址
                                string Rxg = @"linkman"":""([\s\S]*?)""";  //手机端正则匹配联系人
                                string Rxg1 = @"<p class='phone-num'>([\s\S]*?)<";   //电话

                                Match contacts = Regex.Match(strhtml2, Rxg);                                                        //手机端正则匹配联系人
                                Match tell = Regex.Match(strhtml2, Rxg1);
                                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                                lv1.SubItems.Add(Unicode2String(contacts.Groups[1].Value));
                                lv1.SubItems.Add(tell.Groups[1].Value);
                                lv1.SubItems.Add(city);
                                Thread.Sleep(1000);

                                if (listView1.Items.Count - 1 > 1)
                                {
                                    listView1.EnsureVisible(listView1.Items.Count - 1);
                                }


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
        private void Ershoufang_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Method.DataTableToExcel(Method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
