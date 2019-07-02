using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main._2019_7
{
    public partial class books : Form
    {
        public books()
        {
            InitializeComponent();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            getUrlbyWeb get = new getUrlbyWeb();
            get.Show();
        }

        public void fang1()
        {

            try
            {

                for (int i = 1; i <51; i++)
                {

             

                    string Url = "https://www.vitalsource.com/textbooks/law?page="+i;


                    string html = method.gethtml(Url, "", "gb2312");
                    if (html == null)
                        break;
                    MatchCollection titles = Regex.Matches(html, @"<span title=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection authers = Regex.Matches(html, @"<div class='product-search-result__author'>([\s\S]*?)</div>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection banbens = Regex.Matches(html, @"<ul class='horizontal-dictionary'>([\s\S]*?)<li class='product-search-result__price'>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection prices = Regex.Matches(html, @"<span title=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    MatchCollection pics = Regex.Matches(html, @"<img class=""product-cover__image"" alt="""" src=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    for (int j = 0; j < titles.Count; j++)

                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                        lv1.SubItems.Add(titles[j].Groups[1].Value.Trim());



                    }



                }
            }

            catch (System.Exception ex)
            {

                ex.ToString();
            }

        }
    }
}
