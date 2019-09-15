using System;
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

namespace main._2019_9
{
    public partial class 亚马逊 : Form
    {
        public 亚马逊()
        {
            InitializeComponent();
        }

        bool zanting = true;
        #region  公拍网

        public void gp()
        {
            try
            {
                string[] urls = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string URL in urls)
                {

                    for (int i = 0; i < 999; i++)
                    {


                        string strhtml = method.GetUrl(URL+ "&pageNumber="+i, "utf-8");


                        MatchCollection  ids = Regex.Matches(strhtml, @"<div id=""customer_review([\s\S]*?)</span></div></div></ul>");
                 



                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                   
                        listViewItem.SubItems.Add(URL);

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                        Thread.Sleep(1000);
                    }

                }




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        #endregion
        private void 亚马逊_Load(object sender, EventArgs e)
        {

        }
    }
}
