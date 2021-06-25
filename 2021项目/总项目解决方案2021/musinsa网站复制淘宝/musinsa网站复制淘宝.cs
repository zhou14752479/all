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
    public partial class musinsa网站复制淘宝 : Form
    {
        public musinsa网站复制淘宝()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Filter = "txt|*.txt";
            sfd.Title = "txt文件导出";


            if (sfd.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = sfd.FileName;
            }
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
        //    dialog.Description = "请选择所在文件夹";
        //    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //    {
        //        if (string.IsNullOrEmpty(dialog.SelectedPath))
        //        {
        //            MessageBox.Show(this, "文件夹路径不能为空", "提示");
        //            return;
        //        }

        //        textBox1.Text = dialog.SelectedPath;
        //    }
        //}


        public string getminprice(string id)
        {
            string url = "https://store.musinsa.com/app/svc/member_price_new/"+id+"/0?1622708406067&type=detail";
            string html = method.GetUrlWithCookie(url, "", "utf-8");
            MatchCollection prices = Regex.Matches(html, @"list_price"">([\s\S]*?)</span>");
            if (prices.Count > 0)
            {
                return prices[prices.Count - 1].Groups[1].Value.Replace("원","").Replace(",","");
            }
            else
            {
                return "空";
            }
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;
        public void run()
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("请导入文本");
                return;
            }

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
                

                StreamReader sr = new StreamReader(textBox2.Text, method.EncodingType.GetTxtType(textBox2.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();

                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {
                    string uid= text[i];
                    textBox3.Text += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"正在抓取" +uid+"\r\n";
                    string url = "https://store.musinsa.com/app/goods/" + uid;
                    string html = method.GetUrlWithCookie(url, "", "utf-8");
                    string xqhtml = Regex.Match(html, @"id=""detail_view"">([\s\S]*?) <div class=""btn-more"">").Groups[1].Value;
                    string chimahtml = Regex.Match(html, @"<!--사이즈추천 개선-->([\s\S]*?)<ul class=""n-info-txt"">").Groups[1].Value;

                    string title = Regex.Match(html, @"<p class=""product_article_contents""><strong>([\s\S]*?)</a>").Groups[1].Value;
                    title = Regex.Replace(title, "<[^>]+>", "");
                    string price = getminprice(uid);
                    string mainpicurl = "https:"+Regex.Match(html, @"<div class=""product-img"">([\s\S]*?)<img src=""([\s\S]*?)""").Groups[2].Value;
                    MatchCollection xqpicurls = Regex.Matches(xqhtml, @"src=""([\s\S]*?)""");


                    StringBuilder miaoshu_sb = new StringBuilder();
                   // string picpath = textBox1.Text + "\\" + uid;
                    //if (!Directory.Exists(picpath))
                    //{
                    //    Directory.CreateDirectory(picpath); //创建文件夹
                    //}

                    //下载图片开始
                    if (mainpicurl != "https:")
                    {
                        textBox3.Text += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"正在下载" + uid + "主图\r\n";
                      
                        method.downloadFile(mainpicurl, zhutupath, uid+"zhu.tbi", "");
                    }


                    for (int a = 0; a < xqpicurls.Count; a++)
                    {
                        string xqpicurl = xqpicurls[a].Groups[1].Value;
                        if (!xqpicurls[a].Groups[1].Value.Contains("https"))
                        {
                            xqpicurl = "https:" +xqpicurls[a].Groups[1].Value;
                        }

                        miaoshu_sb.Append("<P><IMG src=\""+xqpicurl+"\" align=middle></P>");
                        textBox3.Text += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+ "正在下载" + uid + "详情图"+a+"\r\n";
                        //miaoshu_sb.Append("<p><IMG src=\"FILE:///"+ picpath + "xq" + a + ".jpg"+ "\" align=middle></P>");
                        // method.downloadFile(xqpicurl, picpath, "xq" + a + ".jpg", "");
                    }

                    chimahtml = Regex.Replace(chimahtml, "<a (.*?)>", "", RegexOptions.Compiled);
                    chimahtml = Regex.Replace(chimahtml, "<input (.*?)>", "", RegexOptions.Compiled);
                    chimahtml = chimahtml.Replace("위 구매 내역의 사이즈를 저장하시겠습니까?","");
                    chimahtml = chimahtml.Trim().Replace("\t", "");
                    chimahtml = chimahtml.Replace("\r", "");
                    chimahtml = chimahtml.Replace("\n", "");

                    chimahtml = chimahtml.Replace("<table id=\"size_table\" class=\"table_th_grey\">", "<TABLE id=size_table class=table_th_grey style=\"FONT-SIZE: 12px; BORDER-TOP: 0px; FONT-FAMILY: 돋움, Dotum, Arial, Helvetica, sans-serif; BORDER-RIGHT: 0px; WIDTH: 520px; VERTICAL-ALIGN: top; BACKGROUND: rgb(255, 255, 255); WHITE-SPACE: normal; WORD-SPACING: 0px; BORDER-COLLAPSE: collapse; MIN-WIDTH: 100%; BORDER-BOTTOM: 0px; TEXT-TRANSFORM: none; FONT-WEIGHT:400; COLOR: rgb(0, 0, 0); OUTLINE-WIDTH: 0px; PADDING-BOTTOM: 0px; FONT-STYLE: normal; TEXT-ALIGN: center; PADDING-TOP: 0px; OUTLINE-STYLE: none; PADDING-LEFT: 0px; BORDER-SPACING: 0px; BORDER-LEFT: 0px; ORPHANS: 2; WIDOWS: 2; MARGIN: 18px 0px 15px; LETTER-SPACING: normal; OUTLINE-COLOR: invert; PADDING-RIGHT: 0px; font-variant-ligatures: normal; font-variant-caps: normal; -webkit-text-stroke-width:0px;text-decoration-thickness:initial; text-decoration-style:initial; text-decoration-color:initial\">");
                    chimahtml = chimahtml.Replace("<thead>", "<THEAD style=\"BORDER-TOP: 0px; BORDER-RIGHT: 0px; VERTICAL-ALIGN: top; BACKGROUND: none transparent scroll repeat 0% 0%; BORDER-BOTTOM: 0px; OUTLINE-WIDTH: 0px; PADDING-BOTTOM: 0px; PADDING-TOP: 0px; OUTLINE-STYLE: none; PADDING-LEFT: 0px; BORDER-LEFT:0px; MARGIN: 0px; OUTLINE-COLOR: invert; PADDING-RIGHT: 0px\">");
                    chimahtml = chimahtml.Replace("<tr>", "<TR style=\"BORDER-TOP: 0px; BORDER-RIGHT: 0px; VERTICAL-ALIGN: top; BACKGROUND: none transparent scroll repeat 0% 0%; BORDER-BOTTOM: 0px; OUTLINE-WIDTH: 0px; PADDING-BOTTOM: 0px; PADDING-TOP: 0px; OUTLINE-STYLE: none; PADDING-LEFT: 0px; BORDER-LEFT:0px; MARGIN: 0px; OUTLINE-COLOR: invert; PADDING-RIGHT: 0px\">");
                    chimahtml = chimahtml.Replace("<th>", "<TH style=\"BOX-SIZING: border-box; BORDER-TOP: rgb(238, 238, 238) 1px solid; HEIGHT: 34px; BORDER-RIGHT: rgb(238, 238, 238) 1px solid; VERTICAL-ALIGN: top; BACKGROUND: rgb(245, 245, 245); BORDER-BOTTOM: rgb(238, 238, 238) 1px solid; FONT-WEIGHT: normal; OUTLINE-WIDTH: 0px; PADDING-BOTTOM: 8px; PADDING-TOP: 10px; OUTLINE-STYLE: none; PADDING-LEFT: 0px; BORDER-SPACING: 0px; BORDER-LEFT: rgb(238, 238, 238) 1px solid; MARGIN: 0px; OUTLINE-COLOR: invert; PADDING-RIGHT: 0px\">");
                    chimahtml = chimahtml.Replace("<th class=\"item_val\">", "<TH class=item_val style=\"BOX-SIZING: border-box; BORDER-TOP: rgb(238, 238, 238) 1px solid; HEIGHT: 34px; BORDER-RIGHT: rgb(238, 238, 238) 1px solid; VERTICAL-ALIGN: top; BACKGROUND: rgb(245, 245, 245); BORDER-BOTTOM: rgb(238, 238, 238) 1px solid; FONT-WEIGHT: normal; OUTLINE-WIDTH: 0px; PADDING-BOTTOM: 8px; PADDING-TOP: 10px; OUTLINE-STYLE: none; PADDING-LEFT: 0px; BORDER-SPACING: 0px; BORDER-LEFT: rgb(238, 238, 238) 1px solid; MARGIN: 0px; OUTLINE-COLOR: invert; PADDING-RIGHT: 0px\">");
                    chimahtml = chimahtml.Replace("<input type=\"hidden\" name=\"diff_range_0\" value=\"5\" />", "");
                    chimahtml = chimahtml.Replace("<tbody>", "<TBODY style=\"BORDER-TOP: 0px; BORDER-RIGHT: 0px; VERTICAL-ALIGN: top; BACKGROUND: none transparent scroll repeat 0% 0%; BORDER-BOTTOM: 0px; OUTLINE-WIDTH: 0px; PADDING-BOTTOM: 0px; PADDING-TOP: 0px; OUTLINE-STYLE: none; PADDING-LEFT: 0px; BORDER-LEFT: 0px; MARGIN: 0px; OUTLINE-COLOR: invert; PADDING-RIGHT: 0px\">");
                    chimahtml = chimahtml.Replace("<tr id=\"mysize\">", "<TR id=mysize style=\"BORDER-TOP: 0px; BORDER-RIGHT: 0px; VERTICAL-ALIGN: top; BACKGROUND: none transparent scroll repeat 0% 0%; BORDER-BOTTOM: 0px; OUTLINE-WIDTH: 0px; PADDING-BOTTOM: 0px; PADDING-TOP: 0px; OUTLINE-STYLE: none; PADDING-LEFT: 0px; BORDER-LEFT: 0px; MARGIN: 0px; OUTLINE-COLOR: invert; PADDING-RIGHT: 0px\">");
                    //chimahtml = chimahtml.Replace("", "");
                    //chimahtml = chimahtml.Replace("", "");
                    //chimahtml = chimahtml.Replace("", "");
                    //chimahtml = chimahtml.Replace("", "");
                    chimahtml = chimahtml.Replace("<input type=\"hidden\" name=\"size_info\" value=\"\"/>", "");
                    chimahtml = chimahtml.Replace("<td class=\"goods_size_val\">", "<TD class=goods_size_val style=\"BOX-SIZING: border-box; BORDER-TOP: rgb(238, 238, 238) 1px solid; HEIGHT: 34px; BORDER-RIGHT: rgb(238, 238, 238) 1px solid; VERTICAL-ALIGN: top; BACKGROUND: none transparent scroll repeat 0% 0%; BORDER-BOTTOM: rgb(238, 238, 238) 1px solid; COLOR: rgb(119, 119, 119); OUTLINE-WIDTH: 0px; PADDING-BOTTOM: 8px; PADDING-TOP: 10px; OUTLINE-STYLE: none; PADDING-LEFT: 0px; BORDER-SPACING: 0px; BORDER-LEFT: rgb(238, 238, 238) 1px solid; MARGIN: 0px; OUTLINE-COLOR: invert; LINE-HEIGHT: 12px; PADDING-RIGHT: 0px\">");
                    chimahtml = "<p>" + chimahtml + "</p>";


                   

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

                        +chimahtml+ miaoshu_sb.ToString() + "\t"  //宝贝描述
                        + "" + "\t"  //宝贝属性
                        + "1163639570" + "\t" //邮费模板
                        + "0" + "\t"
                        + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t"//修改时间
                        + "200" + "\t"  //Z列


                        + "2;" + "\t"// AA
                        + "0" + "\t"
                        + ""+ uid + "zhu:1:0:|;" + "\t"
                        + "" + "\t"
                        + ""+price+":10::20509:28383;1627207:28320;" + "\t"//销售属性
                        + "" + "\t"
                        + "" + "\t"//
                        +  uid + "\t"  //AH

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

                }
               textBox3.Text += "抓取结束，生成CSV成功" + "\r\n";
                MessageBox.Show("抓取结束，生成CSV成功");
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }
        }
        private void musinsa网站复制淘宝_Load(object sender, EventArgs e)
        {

        }
        Thread thread;
        private void button3_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html","utf-8");

            if (!html.Contains(@"cMBfRdu"))
            {
                MessageBox.Show("");
                return;
            }



            #endregion
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            this.textBox3.SelectionStart = this.textBox3.Text.Length;
            this.textBox3.SelectionLength = 0;
            this.textBox3.ScrollToCaret();
        }
    }
}
