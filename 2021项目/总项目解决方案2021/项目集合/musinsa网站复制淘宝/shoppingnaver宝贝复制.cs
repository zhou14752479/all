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

namespace musinsa网站复制淘宝
{
    public partial class shoppingnaver宝贝复制 : Form
    {
        public shoppingnaver宝贝复制()
        {
            InitializeComponent();
        }

        public string getxqPics(string proID, string proNo)

        {
            StringBuilder sb = new StringBuilder();
            string url = "https://shopping.naver.com/v1/products/"+proID+"/contents/"+proNo+"/PC?_nc_=1623340800000&usedImageProxy=false";
            string html = method.GetUrlWithCookie(url, "", "utf-8");
            MatchCollection xqpicurls = Regex.Matches(html, @"<img src=\\""([\s\S]*?)\\""");
         
            for (int a = 0; a < xqpicurls.Count; a++)
            {
                string xqpicurl = xqpicurls[a].Groups[1].Value;
                sb.Append("<P><IMG src=\"" + xqpicurls[a].Groups[1].Value + "\" align=middle></P>");
            }
         
            return sb.ToString();
        }

        string path = AppDomain.CurrentDomain.BaseDirectory;

        public void run()
        {

            //if (textBox1.Text == "")
            //{
            //    MessageBox.Show("请选择图片保存文件夹");
            //    return;
            //}

            string nowtime = DateTime.Now.ToString("HH时mm分");

            string pLocalFilePath = path + "Template//Template.csv";//要复制的文件路径
            string pSaveFilePath = path + "data//" + nowtime + ".csv";//指定存储的路径
            string zhutupath = path + "data//" + nowtime;
            if (!Directory.Exists(zhutupath))
            {
                Directory.CreateDirectory(zhutupath); //创建文件夹
            }


            if (File.Exists(pLocalFilePath))//必须判断要复制的文件是否存在
            {
                File.Copy(pLocalFilePath, pSaveFilePath, true);//三个参数分别是源文件路径，存储路径，若存储路径有相同文件是否替换
            }

            try
            {


                string[] urls = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < urls.Length; i++)
                {
                    if (urls[i].Trim() == "")
                    {
                        continue;
                    }

                    string url = urls[i];

                    string channelId = Regex.Match(url, @"stores/([\s\S]*?)/").Groups[1].Value;
                    string productId = Regex.Match(url, @"products/([\s\S]*?)\?").Groups[1].Value;

                    string html = method.GetUrlWithCookie(url, "", "utf-8");
                    string productNo = Regex.Match(html, @"productNo=([\s\S]*?)""").Groups[1].Value;
                    string title = Regex.Match(html, @"data-title=""([\s\S]*?)""").Groups[1].Value;
                    string price = Regex.Match(html, @"""price"":([\s\S]*?),").Groups[1].Value;
                    string zhupicUrl = Regex.Match(html, @"><img style=""height:"" src=""([\s\S]*?)\?").Groups[1].Value;


                    //下载主图开始
                    if (zhupicUrl != "")
                    {
                        label1.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "正在下载" + productId + "主图\r\n";

                        method.downloadFile(zhupicUrl, zhutupath, productId + "zhu.tbi", "");

                    }
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(url);
                    lv1.SubItems.Add("成功");
                    string xqpics = getxqPics(productId, productNo);
                    StringBuilder miaoshu_sb = new StringBuilder();

                    miaoshu_sb.Append(xqpics);

                    string data = title + "\t"
                        + "50010850" + "\t"
                        + "" + "\t"
                        + "1" + "\t"
                        + "海外" + "\t"
                        + "韩国" + "\t"
                        + "1" + "\t"
                        + price + "\t"
                        + "" + "\t"
                        + "10" + "\t"
                        + "0" + "\t"
                        + "2" + "\t"

                        + "0" + "\t"//平邮
                        + "0" + "\t"
                        + "0" + "\t"
                        + "0" + "\t"
                        + "0" + "\t"
                        + "2" + "\t"//放入仓库
                        + "0" + "\t"
                        + "" + "\t"

                        + miaoshu_sb.ToString() + "\t"  //宝贝描述
                        + "" + "\t"  //宝贝属性
                        + "1163639570" + "\t" //邮费模板
                        + "0" + "\t"
                        + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t"//修改时间
                        + "200" + "\t"  //Z列


                        + "2;" + "\t"// AA
                        + "0" + "\t"
                        + "" + productId + "zhu:1:0:|;" + "\t"
                        + "" + "\t"
                        + "" + price + ":10::20509:28383;1627207:28320;" + "\t"//销售属性
                        + "" + "\t"
                        + "" + "\t"//
                        + productId + "\t"  //AH

                        + "" + "\t"//
                        + "0" + "\t"
                        + "0" + "\t"
                        + "-1" + "\t"
                        + "1" + "\t"
                        + "" + "\t"//
                        + "2" + "\t"
                        + "0" + "\t"
                        + "0" + "\t" //AQ

                         + "product_date_end:;product_date_start:;stock_date_end:;stock_date_start:" + "\t"//
                        + "mysize_tp:-1;sizeGroupId:28383;sizeGroupType:women_tops" + "\t"
                        + "1" + "\t"
                        + "2" + "\t"
                        + "韩国" + "\t"
                        + "2" + "\t"//
                        + "bulk:0.000000" + "\t"
                        + "0" + "\t"
                        + "0" + "\t" //AZ


                         + "" + "\t"//BA
                        + "" + "\t"
                        + "" + "\t"
                        + "" + "\t"
                        + "0" + "\t"
                        + "" + "\t"
                         + "" + "\t"
                        + "" + "\t"
                        + "%7B%20%20%7D" + "\t"
                        + "0" + "\t"
                        + "0" + "\t"
                        + "" + "\t"
                        + "" + "\t"
                        + "" + "\t"
                         + "" + "\t"
                        + "" + "\t";//BP 
                    FileStream fs = new FileStream(pSaveFilePath, System.IO.FileMode.Append, System.IO.FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("utf-16"));
                    sw.WriteLine(data);
                    sw.Close();
                    fs.Close();
                    Thread.Sleep(1000);
                }
                label1.Text = "抓取结束，生成CSV成功";
               

            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }
        }

        private void shoppingnaver宝贝复制_Load(object sender, EventArgs e)
        {

        }
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < listView1.SelectedItems.Count; i++)
            {
                try
                {
                    System.Diagnostics.Process.Start((listView1.SelectedItems[i].SubItems[1].Text));
                }
                catch (Exception ex)
                {

                    ;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
