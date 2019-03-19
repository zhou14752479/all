using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 链家网
{
    public partial class 教育视频网 : Form
    {
        public 教育视频网()
        {
            InitializeComponent();
            
        }

        private void 教育视频网_Load(object sender, EventArgs e)
        {
                      
        }
        #region 获取网址

        public static void getUrls(ListBox cob)
        {
            ArrayList list = new ArrayList();
            try
            {
                string constr = "Host =116.62.62.62;Database=vip;Username=root;Password=zhoukaige";
                string str = "SELECT url from jiaoyu ";
                MySqlDataAdapter da = new MySqlDataAdapter(str, constr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    //string html = method.GetHtmlSource(dr[0].ToString().Trim());
                    //Match title = Regex.Match(html, @"<h1>([\s\S]*?)</h1>");

                    //list.Add(title.Groups[1].Value);
                    list.Add(dr[0].ToString().Trim());
                }
            }
            catch (MySqlException ee)
            {
                ee.Message.ToString();
            }
            cob.DataSource = list;

        }
        #endregion
        #region 获取名称

        public static void getTitles(ListBox cob)
        {
            ArrayList list = new ArrayList();
            try
            {
                string constr = "Host =116.62.62.62;Database=vip;Username=root;Password=zhoukaige";
                string str = "SELECT url from jiaoyu ";
                MySqlDataAdapter da = new MySqlDataAdapter(str, constr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    string html = method.GetHtmlSource(dr[0].ToString().Trim());
                    Match title = Regex.Match(html, @"<h1>([\s\S]*?)</h1>");

                    list.Add(title.Groups[1].Value);
                    //list.Add(dr[0].ToString().Trim());
                }
            }
            catch (MySqlException ee)
            {
                ee.Message.ToString();
            }
            cob.DataSource = list;

        }
        #endregion

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            getTitles(listBox1);
            getUrls(listBox2);
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
           
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = listBox1.IndexFromPoint(e.X, e.Y);
            listBox1.SelectedIndex = index;
            if (listBox1.SelectedIndex != -1)
            {
                // MessageBox.Show(listBox1.SelectedItem.ToString());

                Match CaseId = Regex.Match(listBox2.Items[listBox1.SelectedIndex].ToString(), @"CaseId=([\s\S]*?)&");
                string html = method.GetHtmlSource("http://1s1k.eduyun.cn/resource/resource/RedesignCaseView/viewCaseBbs1s1k.jspx?date=1552360751106&code=-1&sdResIdCaseId=" + CaseId.Groups[1].Value + "&flags=&guideId=&sk=&sessionKey=A6SXB2v8eURy4yVrbm6O");

                Match value = Regex.Match(html, @"value=""mda-([\s\S]*?)""");
                // System.Diagnostics.Process.Start("http://1s1k.eduyun.cn/resource/redesign/publishCase/vod_player.jsp?resourceCode=mda-" + value.Groups[1].Value + "&divId=playercontainers1&wid=770&hei=530&sessionKey=KTHObD76L5EyEDjLsWWu");
               // this.webKitBrowser1.Navigate("http://1s1k.eduyun.cn/resource/redesign/publishCase/vod_player.jsp?resourceCode=mda-" + value.Groups[1].Value + "&divId=playercontainers1&wid=770&hei=530&sessionKey=KTHObD76L5EyEDjLsWWu");

            }
        }
    }
}
