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
using 类库;


namespace 淘宝商品sku
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static string COOKIE= "t=792ea994957bef8e4a71539f91876594; tg=0; thw=cn; cna=8QJMFUu4DhACATFZv2JYDtwd; hng=CN%7Czh-CN%7CCNY%7C156; UM_distinctid=16bde9c6ccb7f7-0183a6c7f99aa8-f353163-1fa400-16bde9c6cccb79; enc=BJiGDZ0SETmb%2BZ1Af%2FLOxZ7Ow%2Fz8B4xQY%2F3CPHkFybDesLHC8XJXgbKIOBMMGVwHTtQxN1Uu1ZSlm%2FWpfRRSTw%3D%3D; ali_ab=49.94.92.171.1563332665663.4; tracknick=zkg852266010; lgc=zkg852266010; x=e%3D1%26p%3D*%26s%3D0%26c%3D0%26f%3D0%26g%3D0%26t%3D0; _m_h5_tk=202bd7e0b5ef0cb289427c315b979841_1563943103843; _m_h5_tk_enc=4a0c8a5a5e645948f43df4f92cb98265; uc3=vt3=F8dBy3zYc7DsdnXudaU%3D&id2=UoH62EAv27BqSg%3D%3D&nk2=GcOvCmiKUSBXqZNU&lg2=VFC%2FuZ9ayeYq2g%3D%3D; _cc_=VT5L2FSpdA%3D%3D; mt=ci=43_1&np=; v=0; uc1=cookie14=UoTaHPk4PiEuPQ%3D%3D; cookie2=1708a1f964465dc34d93722559800808; _tb_token_=e7be67b0e8ae5; l=cBxhyUZrqScyZL3CBOCMquIRG17OSIRAguPRwVmXi_5Ik6817U7OkVzJVFJ6VjWd9VYB40tUd_v9-etkmPRgTBH8sxAR.; isg=BAIC-lf56-emL_cwAeHe0ZVfUwikew6K4BzAZUwbLnUgn6IZNGNW_YjdS9tGz36F";
        bool zanting = true;

        #region 获取店铺内宝贝
        public void run1()
        {

            try
            {
                for (int i = 0; i < 999; i++)
                {


                    Match shopid = Regex.Match(textBox1.Text, @"https:\/\/([\s\S]*?)\.");
                    string Url = "https://" + shopid.Groups[1].Value + ".taobao.com/category.htm";
                    string html = method.gethtml(Url);

                    Match title = Regex.Match(html, @"<span class=""shop-name-title"" title=""([\s\S]*?)""");
                    Match midid = Regex.Match(html, @"mid=w-([\s\S]*?)-");

                    string curl = "https://" + shopid.Groups[1].Value + ".taobao.com/i/asynSearch.htm?_ksTS=1561163488418_160&callback=jsonp161&mid=w-" + midid.Groups[1].Value + "-0&wid=" + midid.Groups[1].Value + "&path=/search.htm&search=y&orderType=newOn_desc&viewType=grid&keyword=null&lowPrice=null&highPrice=null";


                    MatchCollection names = Regex.Matches(method.gethtml(curl), @"<img alt=\\""([\s\S]*?)\\");
                    MatchCollection uids = Regex.Matches(method.gethtml(curl), @"data-id=\\""([\s\S]*?)\\");
                    textBox2.Text = curl;

                    MessageBox.Show(names.Count.ToString());

                    for (int j = 0; j < names.Count; j++)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据    
                        lv1.SubItems.Add(names[j].Groups[1].Value);
                        lv1.SubItems.Add("https://item.taobao.com/item.htm?id=" + uids[j].Groups[1].Value);
                    }




                    if (listView1.Items.Count > 2)
                    {
                        listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                    }
                    Thread.Sleep(500);

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }



                }



                
            }

            catch (System.Exception ex)
            {

                ex.ToString();
            }

        }

        #endregion

        #region 获取SKU价格
        public void run()
        {

            try
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("请输入商品地址");
                    return;
                }
                string url = textBox1.Text; ;


                    string html = method.gethtml(url);
               
                    MatchCollection  names= Regex.Matches(html, @"<a href=""javascript:void\(0\);"">([\s\S]*?)</span>");
                MatchCollection prices = Regex.Matches(html, @"""price"":""([\s\S]*?)""");



                for (int i = 0; i < names.Count; i++)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据    

                    lv1.SubItems.Add(names[i].Groups[1].Value.Replace("<span>","").Trim());
                    lv1.SubItems.Add(prices[i].Groups[1].Value);


                    if (listView1.Items.Count > 2)
                    {
                        listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                    }


                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。

                    }
                }
               




                }
            

            catch (System.Exception ex)
            {

                ex.ToString();
            }

        }

        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run1));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

      

        private void Label3_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
                this.Close(); //点确定的代码
            }
            else
            { //点取消的代码 
                return;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
