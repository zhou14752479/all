using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fang.临时软件
{
    public partial class 物通网 : Form
    {
        public 物通网()
        {
            InitializeComponent();
        }

        bool status = true;

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            try
            {
              
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.UserAgent = "387084CFFF0AD93DF880F4BA042190E5493B8A1B5006C518363D39972896F36E846A1A184B93207E";
                request.AllowAutoRedirect = true;         
                request.KeepAlive = false;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string content = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                ex.ToString();

            }
            return "";
        }
        #endregion


     //   ArrayList finishes = new ArrayList();

        #region  物通网
        public void run()
        {

           
            try
            {

                string province = System.Web.HttpUtility.UrlEncode(visualComboBox1.Text); 


                        string url = "http://android.chinawutong.com/PostData.ashx?chechang=&infotype=1&condition=1&tsheng=&txian=&chexing=&huiyuan_id=2264195&fsheng="+province+"&type=GetGood_new&fshi=&tshi=&pid=1&fxian=&ver_version=1&r_20717=37619";
                      
                        string html = GetUrl(url, "utf-8");

                    MatchCollection goods_names = Regex.Matches(html, @"goods_name"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                   
                    MatchCollection zaizhongs = Regex.Matches(html, @"zaizhong"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection trans_modes = Regex.Matches(html, @"trans_mode"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection huo_phones = Regex.Matches(html, @"huo_phone"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection huo_contacts = Regex.Matches(html, @"huo_contact"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    MatchCollection company_names = Regex.Matches(html, @"company_name"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                   
                    
                    MatchCollection times = Regex.Matches(html, @"data_time"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                MatchCollection from_areas = Regex.Matches(html, @"from_area"":""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                MatchCollection to_areas = Regex.Matches(html, @"to_area"":([\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                for (int j = 0; j < goods_names.Count; j++)
                    {

                    //string strhtml = GetUrl("http://www.chinawutong.com/203/ab"+ from_areas[j].Groups[1].Value.Trim() + "k"+ from_areas[j].Groups[1].Value.Trim() + "l-1m-1n-1j-1/", "gb2312");
                    //string strhtml1 = GetUrl("http://www.chinawutong.com/203/ab" + to_areas[j].Groups[1].Value.Trim() + "k" + to_areas[j].Groups[1].Value.Trim() + "l-1m-1n-1j-1/", "gb2312");

                    //Match from_area = Regex.Match(strhtml, @"<a>-([\s\S]*?)<");
                    //Match to_area = Regex.Match(strhtml1, @"<a>-([\s\S]*?)<");
                        if (goods_names.Count > 0)
                        {
                        //  ListViewItem lv1 = listView1.Items.Add(from_area.Groups[1].Value + "→" + to_area.Groups[1].Value);
                        ListViewItem lv1 = listView1.Items.Add("");
                        lv1.SubItems.Add(goods_names[j].Groups[1].Value.Trim());
                            lv1.SubItems.Add(zaizhongs[j].Groups[1].Value.Trim() + "公斤");
                            lv1.SubItems.Add(trans_modes[j].Groups[1].Value.Trim());
                            lv1.SubItems.Add(huo_phones[j].Groups[1].Value.Trim());
                            lv1.SubItems.Add(huo_contacts[j].Groups[1].Value.Trim());
                            lv1.SubItems.Add(company_names[j].Groups[1].Value.Trim());

                            lv1.SubItems.Add(times[j].Groups[1].Value.Trim());

                           
                            while (this.status == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
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

        private void 物通网_Load(object sender, EventArgs e)
        {
           
        }

        private void visualButton1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            this.status = true;

            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();


        }

        private void visualButton2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void visualButton3_Click(object sender, EventArgs e)
        {
            this.status = false;
            
        }

        private void visualButton4_Click(object sender, EventArgs e)
        {
            this.status = true;
        }

        private void 清空数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
