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
using myDLL;

namespace 网站价格抓取
{
    public partial class 网站价格抓取 : Form
    {
        public 网站价格抓取()
        {
            InitializeComponent();
        }


        string path = AppDomain.CurrentDomain.BaseDirectory+"url/";
        public string readtxt(string name)
        {
            StreamReader sr = new StreamReader(path+name+".txt", method.EncodingType.GetTxtType(path + name + ".txt"));
            //一次性读取完 
            string texts = sr.ReadToEnd();
           
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存
            return texts;
        }


        #region 网站2 tongjungou

        public void tongjungou()
        {
            string txt = readtxt("tongjungou");
            string[] urls = txt.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < urls.Length; i++)
            {
                string url = urls[i];
                string id = Regex.Match(url, @"\d{4,}").Groups[0].Value ;
               
                string html = method.GetUrl(url,"utf-8");

                //有效期
                MatchCollection dates = Regex.Matches(html, @"<span id=""date_([\s\S]*?)""([\s\S]*?)>([\s\S]*?)</span>");

                string dateid = dates[dates.Count - 1].Groups[1].Value.Trim();
                string datename = dates[dates.Count - 1].Groups[3].Value.Trim();

                string ahtml = method.PostUrlDefault("http://www.tongjungou.com/ajaxdatexq", "goods_id="+id+"&wh_id="+ dateid, "");


                //1-6罐
                MatchCollection guans = Regex.Matches(ahtml, @"""id"":""([\s\S]*?)""");


                string bhtml = method.PostUrlDefault("http://www.tongjungou.com/ajaxspec", "goods_id="+id+"&spec_id="+guans[guans.Count-1].Groups[1].Value+"&wh_id="+dateid, "");
                string price = Regex.Match(bhtml, @"""totalprice"":""([\s\S]*?)""").Groups[1].Value;
                string spec_title = Regex.Match(bhtml, @"""spec_title"":""([\s\S]*?)""").Groups[1].Value;

                listView1.Items[i].SubItems[1].Text = method.Unicode2String(spec_title);
                listView1.Items[i].SubItems[4].Text = price;
                listView1.Items[i].SubItems[5].Text = datename;
               
            }


        }

        #endregion


        #region 网站3 saintcos

        public void saintcos()
        {
            string txt = readtxt("saintcos");
            string[] urls = txt.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < urls.Length; i++)
            {
                string url = urls[i];
                string id = Regex.Match(url, @"\d{4,}").Groups[0].Value;

                string html = method.GetUrl(url, "utf-8");

                //有效期
                MatchCollection dates = Regex.Matches(html, @"<span id=""date_([\s\S]*?)""([\s\S]*?)>([\s\S]*?)</span>");

                string dateid = dates[dates.Count - 1].Groups[1].Value.Trim();
                string datename = dates[dates.Count - 1].Groups[3].Value.Trim();

                string ahtml = method.PostUrlDefault("http://www.tongjungou.com/ajaxdatexq", "goods_id=" + id + "&wh_id=" + dateid, "");


                //1-6罐
                MatchCollection guans = Regex.Matches(ahtml, @"""id"":""([\s\S]*?)""");


                string bhtml = method.PostUrlDefault("http://www.tongjungou.com/ajaxspec", "goods_id=" + id + "&spec_id=" + guans[guans.Count - 1].Groups[1].Value + "&wh_id=" + dateid, "");
                string price = Regex.Match(bhtml, @"""totalprice"":""([\s\S]*?)""").Groups[1].Value;
                
              
                listView1.Items[i].SubItems[6].Text = price;
                listView1.Items[i].SubItems[7].Text = datename;

            }


        }

        #endregion


        #region 网站4 goodji

        public void goodji()
        {
            string txt = readtxt("goodji");
            string[] urls = txt.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < urls.Length; i++)
            {
                string url = urls[i];
                string id = Regex.Match(url, @"\d{4,}").Groups[0].Value;

                string html = method.GetUrl(url, "utf-8");



                //有效期  
                MatchCollection dates = Regex.Matches(Regex.Match(html, @"有效期：</span></li>([\s\S]*?)</ul>").Groups[1].Value, @"<li data-id=""([\s\S]*?)""");
               //罐装
                MatchCollection guans = Regex.Matches(Regex.Match(html, @"销售规格：</span></li>([\s\S]*?)</ul>").Groups[1].Value, @"<li data-id=""([\s\S]*?)""");
               
                string datename = dates[dates.Count - 1].Groups[1].Value.Trim();
                string guanname = guans[guans.Count - 1].Groups[1].Value.Trim();

              

                string bhtml = method.PostUrlDefault("http://www.goodji.cn/Home/Goods/ajaxGoodsInfo.html", "is_self=0&goods_id="+id+"&expireDate="+datename+"&item="+guanname+"&shipping_area=", "");
               
                string price = Regex.Match(bhtml, @"""price"":""([\s\S]*?)""").Groups[1].Value;


                listView1.Items[i].SubItems[8].Text = price;
                listView1.Items[i].SubItems[9].Text = datename;

            }


        }

        #endregion
        public void run()
        {
            if(checkBox2.Checked==true)
            {
               Thread thread = new Thread(tongjungou);
                thread.Start();
                
            }
            if (checkBox4.Checked == true)
            {
                Thread thread = new Thread(goodji);
                thread.Start();

            }

        }

        private void 网站价格抓取_Load(object sender, EventArgs e)
        {

        }
        Thread thread;

        bool zanting = true;
        bool status = true;

        private void button3_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            string txt = readtxt("tongjungou");
            string[] urls = txt.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < urls.Length; i++)
            {
                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
            }

            if (urls.Length==0)
            {
                MessageBox.Show("请添加网址");
            }

            run();
        }
    }
}
