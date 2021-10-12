using gregn6Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 体育打票软件
{
    public partial class 体育打票软件 : Form
    {
        public 体育打票软件()
        {
            InitializeComponent();
        }



        #region ini读取
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.inipath);
        }

        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
            return temp.ToString();
        }

        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public bool ExistINIFile()
        {
            return File.Exists(inipath);
        }

        #endregion








        string path = AppDomain.CurrentDomain.BaseDirectory;
        private GridppReport Report = new GridppReport();
        private void 体育打票软件_Load(object sender, EventArgs e)
        {
           

            if (ExistINIFile())
            {
                address_txt.Text = IniReadValue("values", "address");
                haoma_txt.Text = IniReadValue("values", "haoma");
                bianma_txt.Text = IniReadValue("values", "bianma");
            }



            //this.tabControl1.Region = new Region(new RectangleF(this.tabPage1.Left, this.tabPage1.Top, this.tabPage1.Width, this.tabPage1.Height));
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("https://www.sporttery.cn/jc/jsq/zqhhgg/");
            //webBrowser1.Navigate("https://trade.500.com/jczq/");


            //Report.LoadFromFile(@"C:\Grid++Report 6\Samples\Reports\1a.简单表格.grf");
            //Report.DetailGrid.Recordset.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;" +
            //    @"User ID=Admin;Data Source=C:\Grid++Report 6\\Samples\Data\Northwind.mdb";
        }


        function fc = new function();

        public static string html;
        public static string ahtml;
        public static string suiji;

        public void gethtml()
        {
            //var btns = webBrowser1.Document.GetElementsByTagName("input");
            //foreach (HtmlElement btn in btns)
            //{
            //    if (btn.GetAttribute("id") == "detailBtn")
            //    {
            //        btn.InvokeMember("click");
            //    }
            //}

            
        
            html = webBrowser1.Document.Body.OuterHtml;
            ahtml = webBrowser1.DocumentText;
            if (webBrowser1.Url.ToString().Contains("500.com"))
            {
                StreamReader sr = new StreamReader(webBrowser1.DocumentStream, Encoding.GetEncoding(("gb2312")));
                ahtml = sr.ReadToEnd();
            }
           // textBox1.Text = html;
            html=fc.getfullname(html);
        }


   

        private void button1_Click(object sender, EventArgs e)
        {
            button2.BackColor = Color.LightGray;
            button1.BackColor = Color.White;
            tabControl1.SelectedIndex = 0;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.BackColor = Color.LightGray;
            button2.BackColor=Color.White;
            tabControl1.SelectedIndex = 1;

        }




        public void getvalue_click(string zhou, string value,string beishu)
        {

            int a = 0;
            string html = 体育打票软件.html;
            MatchCollection zhous = Regex.Matches(html, @"<td class=""mCodeCls""([\s\S]*?)>([\s\S]*?)</td>");
            for (int i = 0; i < zhous.Count; i++)
            {
                if (zhous[i].Groups[2].Value == zhou)
                {
                    a = i;
                    break;
                }

            }


            string avalue = "a" + value + "a";


            string maohao = Regex.Match(avalue, @":").Groups[0].Value;
            string bifen_qita = Regex.Match(avalue, @"其他").Groups[0].Value;


            string shuzi = Regex.Match(avalue, @"a\da").Groups[0].Value;
            string shuzi_7jia = Regex.Match(avalue, @"7+").Groups[0].Value;

            string sheng = Regex.Match(avalue, @"a胜a|a平a|a负a").Groups[0].Value;
            string rangsheng = Regex.Match(avalue, @"a让胜a|a让平a|a让负a").Groups[0].Value;
            string bansheng = Regex.Match(avalue, @"a胜胜a|a胜平a|a胜负a|a平胜a|a平平a|a平负a|a负胜a|a负平a|a负负a").Groups[0].Value;

            string type = "半全场胜平负";





            if (bansheng != "" && sheng == "" && rangsheng == "" && shuzi == "" && maohao == "")
            {
                type = "半全场胜平负";
            }

            else if (bansheng == "" && sheng == "" && rangsheng != "" && shuzi == "" && maohao == "")
            {
                type = "胜平负";
            }
            else if (bansheng == "" && sheng != "" && rangsheng == "" && shuzi == "" && maohao == "")
            {
                type = "胜平负";
            }
            else if (bansheng == "" && sheng == "" && rangsheng == "" && shuzi != "" && maohao == "")
            {
                type = "进球数";
            }
            else if (bansheng == "" && sheng == "" && rangsheng == "" && shuzi_7jia != "" && maohao == "")
            {
                type = "进球数";
            }



            else if (bansheng == "" && sheng == "" && rangsheng == "" && shuzi == "" && maohao != "")
            {
                type = "比分";
            }
            else if (bansheng == "" && sheng == "" && rangsheng == "" && shuzi == "" && bifen_qita != "")
            {
                type = "比分";
            }

            string id = "d_0_4";

            switch (type)
            {
                case "胜平负":
                    id = "d_" + a + "_1";
                    break;
                case "半全场胜平负":
                    id = "d_" + a + "_4";
                    break;
                case "进球数":
                    id = "d_" + a + "_3";
                    break;
                case "比分":
                    id = "d_" + a + "_2";
                    break;



            }



            HtmlElement element = webBrowser1.Document.GetElementById(id);
            HtmlElementCollection element0 = element.Children;

            if (type == "胜平负")
            {
                HtmlElementCollection elementchild = element0[0].Children[0].Children;


                switch (value)
                {
                    case "胜":
                        elementchild[5].Children[1].InvokeMember("click");
                        break;
                    case "平":
                        elementchild[5].Children[2].InvokeMember("click");
                        break;
                    case "负":
                        elementchild[5].Children[3].InvokeMember("click");
                        break;
                    case "让胜":

                        elementchild[5].Children[6].InvokeMember("click");
                        break;

                    case "让平":
                        elementchild[5].Children[7].InvokeMember("click");
                        break;
                    case "让负":
                        elementchild[5].Children[8].InvokeMember("click");
                        break;





                }
            }

            if (type == "半全场胜平负")
            {
                HtmlElementCollection elementchild = element0[1].Children;


                switch (value)
                {
                    case "胜胜":
                        elementchild[0].Children[0].InvokeMember("click");
                        break;
                    case "胜平":
                        elementchild[1].Children[0].InvokeMember("click");
                        break;
                    case "胜负":
                        elementchild[2].Children[0].InvokeMember("click");
                        break;
                    case "平胜":

                        elementchild[3].Children[0].InvokeMember("click");
                        break;

                    case "平平":
                        elementchild[4].Children[0].InvokeMember("click");
                        break;
                    case "平负":
                        elementchild[5].Children[0].InvokeMember("click");
                        break;

                    case "负胜":

                        elementchild[6].Children[0].InvokeMember("click");
                        break;

                    case "负平":
                        elementchild[7].Children[0].InvokeMember("click");
                        break;
                    case "负负":
                        elementchild[8].Children[0].InvokeMember("click");
                        break;




                }
            }

            if (type == "进球数")
            {
                HtmlElementCollection elementchild = element0[1].Children;


                switch (value)
                {
                    case "0":
                        elementchild[0].Children[0].InvokeMember("click");
                        break;
                    case "1":
                        elementchild[1].Children[0].InvokeMember("click");
                        break;
                    case "2":
                        elementchild[2].Children[0].InvokeMember("click");
                        break;
                    case "3":
                        elementchild[3].Children[0].InvokeMember("click");
                        break;

                    case "4":
                        elementchild[4].Children[0].InvokeMember("click");
                        break;
                    case "5":
                        elementchild[5].Children[0].InvokeMember("click");
                        break;
                    case "6":
                        elementchild[6].Children[0].InvokeMember("click");
                        break;
                    case "7+":
                        elementchild[7].Children[0].InvokeMember("click");
                        break;




                }
            }


            if (type == "比分")
            {
                HtmlElementCollection elementchild = element0[0].Children;
                HtmlElementCollection elementchild1 = element0[1].Children;
                HtmlElementCollection elementchild2 = element0[2].Children;
                switch (value)
                {
                    case "1:0":
                        elementchild[1].Children[1].InvokeMember("click");
                        break;
                    case "2:0":
                        elementchild[2].Children[1].InvokeMember("click");
                        break;
                    case "2:1":
                        elementchild[3].Children[1].InvokeMember("click");
                        break;

                    case "3:0":
                        elementchild[4].Children[1].InvokeMember("click");
                        break;
                    case "3:1":
                        elementchild[5].Children[1].InvokeMember("click");
                        break;
                    case "3:2":
                        elementchild[6].Children[1].InvokeMember("click");
                        break;
                    case "4:0":
                        elementchild[7].Children[1].InvokeMember("click");
                        break;
                    case "4:1":
                        elementchild[8].Children[1].InvokeMember("click");
                        break;
                    case "4:2":
                        elementchild[8].Children[1].InvokeMember("click");
                        break;
                    case "5:0":
                        elementchild[10].Children[1].InvokeMember("click");
                        break;
                    case "5:1":
                        elementchild[11].Children[1].InvokeMember("click");
                        break;
                    case "5:2":
                        elementchild[12].Children[1].InvokeMember("click");
                        break;
                    case "胜其他":
                        elementchild[13].Children[1].InvokeMember("click");
                        break;
                    case "0:0":
                        elementchild1[0].Children[1].InvokeMember("click");
                        break;
                    case "1:1":
                        elementchild1[1].Children[1].InvokeMember("click");
                        break;
                    case "2:2":

                        elementchild1[2].Children[1].InvokeMember("click");
                        break;
                    case "3:3":
                        elementchild1[3].Children[1].InvokeMember("click");
                        break;
                    case "平其他":
                        elementchild1[4].Children[1].InvokeMember("click");
                        break;
                    case "0:1":
                        elementchild2[0].Children[1].InvokeMember("click");
                        break;
                    case "0:2":
                        elementchild2[1].Children[1].InvokeMember("click");
                        break;
                    case "1:2":
                        elementchild2[2].Children[1].InvokeMember("click");
                        break;
                    case "0:3":
                        elementchild2[3].Children[1].InvokeMember("click");
                        break;
                    case "1:3":
                        elementchild2[4].Children[1].InvokeMember("click");
                        break;
                    case "2:3":
                        elementchild2[5].Children[1].InvokeMember("click");
                        break;
                    case "0:4":
                        elementchild2[6].Children[1].InvokeMember("click");
                        break;
                    case "1:4":
                        elementchild2[7].Children[1].InvokeMember("click");
                        break;
                    case "2:4":
                        elementchild2[8].Children[1].InvokeMember("click");
                        break;
                    case "0:5":
                        elementchild2[9].Children[1].InvokeMember("click");
                        break;
                    case "1:5":
                        elementchild2[10].Children[1].InvokeMember("click");
                        break;
                    case "2:5":
                        elementchild2[11].Children[1].InvokeMember("click");
                        break;
                    case "负其他":
                        elementchild2[12].Children[1].InvokeMember("click");
                        break;




                }
            }


           
            //  webBrowser1.Refresh();

          
        }



        /// <summary>
        /// 模拟解析
        /// </summary>
       
        public void getdata(string txt)
        {
            //HtmlElement bifen = webBrowser1.Document.GetElementById("crsChk");
            //HtmlElement jinqiu = webBrowser1.Document.GetElementById("ttgChk");
            //HtmlElement banquanchang = webBrowser1.Document.GetElementById("hafuChk");

            //bifen.InvokeMember("click");
            //jinqiu.InvokeMember("click");
            //banquanchang.InvokeMember("click");
            //Application.DoEvents();

            try
            {



                StringBuilder textsb = new StringBuilder();
                string[] text = txt.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                foreach (string item in text)
                {

                    if (item != "")
                    {
                        if (item.Contains("/"))
                        {
                            textsb.Append("\r\n" + item);
                        }
                        else
                        {
                            textsb.Append("&" + item + "&");
                        }
                    }

                }

               // textBox1.Text = textsb.ToString();

                string[] text0 = textsb.ToString().Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int a = 0; a < text0.Length; a++)
                {



                    string[] text1 = text0[a].ToString().Split(new string[] { "	" }, StringSplitOptions.None);


                    string beishu = Regex.Replace(text1[2], ".*/", "");
                    string guoguan= text1[4].Replace("串", "x");
                   
                   


                    MatchCollection values = Regex.Matches(text0[a].ToString().Replace(",", ">"), @"周([\s\S]*?)&");
                    for (int i = 0; i < values.Count; i++)  //循环周
                    {

                        string zhou = "周" + Regex.Match("周" + values[i].Groups[1].Value, @"周([\s\S]*?)>").Groups[1].Value;



                        MatchCollection results = Regex.Matches(values[i].Groups[1].Value, @">([\s\S]*?)\|");
                        foreach (Match result in results)  //循环结果胜、1：0
                        {
                            getvalue_click(zhou, result.Groups[1].Value, beishu);
                        }


                    }


                    //点击过关
                    foreach (HtmlElement element in webBrowser1.Document.GetElementsByTagName("span"))
                    {
                        if (element.InnerText == guoguan)
                        {
                            element.FirstChild.InvokeMember("click");
                            break;
                        }
                    }


                    //点击倍数
                    for (int i = 1; i < Convert.ToInt32(beishu); i++)
                    {
                        var button = this.webBrowser1.Document.GetElementById("addBtn");
                        button.InvokeMember("click");
                    }


                    var btn = webBrowser1.Document.GetElementById("detailBtn");
                    btn.InvokeMember("click");

                    html = webBrowser1.Document.Body.OuterHtml;
                    ahtml = webBrowser1.DocumentText;


                    string fangshi = Regex.Match(体育打票软件.ahtml, @"<title>([\s\S]*?)</title>").Groups[1].Value;

                    fc.getdata(Report, 体育打票软件.html, 体育打票软件.ahtml, guoguan);

                    //PreviewForm theForm = new PreviewForm();
                    //theForm.AttachReport(Report);
                    //theForm.ShowDialog();
                    Report.Print(false);



                    //取消点击

                    for (int i = 0; i < values.Count; i++)  //循环周
                    {

                        string zhou = "周" + Regex.Match("周" + values[i].Groups[1].Value, @"周([\s\S]*?)>").Groups[1].Value;



                        MatchCollection results = Regex.Matches(values[i].Groups[1].Value, @">([\s\S]*?)\|");
                        foreach (Match result in results)  //循环结果胜、1：0
                        {
                            getvalue_click(zhou, result.Groups[1].Value, beishu);
                        }


                    }

                    for (int i = 1; i < Convert.ToInt32(beishu); i++)
                    {
                        var button = this.webBrowser1.Document.GetElementById("subBtn");
                        button.InvokeMember("click");
                    }

                    //取消点击结束
                }
            }


            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {

           
            //foreach (HtmlElement element in webBrowser1.Document.GetElementsByTagName("span"))
            //{
            //foreach (HtmlElement elementchild in element.Parent)
            //{
            //if (elementchild.InnerText == "35.00")
            //{
            //   element.InvokeMember("click");
            //    break;
            //}
            //}

            //if (element.InnerText=="35.00")
            //{
            //    MessageBox.Show(element..InnerText);
            //}
            //    }



            //List<string> list = new List<string>();
            //list.Add("3.13");
            //list.Add("3.23");
            //list.Add("3.33");
            //list.Add("3.43");

            //List<List<string>> list2= new List<List<string>>();
            //list2= function.GetCombinationList(list,3);
            //MessageBox.Show(list2.Count.ToString());
            //for (int i = 0; i < list2.Count; i++)
            //{

            //    for (int j = 0; j < list2[i].Count; j++)
            //    {
            //        textBox1.Text += list2[i][j];
            //    }
            //    textBox1.Text += "\r\n";
            //}
            webBrowser1.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://www.sporttery.cn/jc/jsq/zqhhgg/");

            //List<Task> TaskList = new List<Task>();

            //for (int i = 0; i < 100; i++)
            //{
            //    TaskList.Add(
            //        Task.Factory.StartNew(() =>
            //        {
            //            BeginInvoke(new Action(() =>
            //            {
            //                textBox1.Text += i + "\r\n";

            //            }));
            //        })
            //    );
            //}
          
            
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://trade.500.com/jczq/");
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://trade.500.com/jclq/index.php?playid=274&g=2");
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://www.sporttery.cn/jc/jsq/zqhhgg/");
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://www.sporttery.cn/jc/jsq/lqsf/");
        }


        int zhankai = 0;
        private void button6_Click(object sender, EventArgs e)
        {
           
            #region 通用检测

            if (!function.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"FCUoF"))
            {
                return;
            }

            #endregion
            suiji = bianma_txt.Text + function.getsuijima();
            gethtml();
            if (webBrowser1.Url.ToString().Contains("500"))
            {
                string fangshi = "解析模式：500网";
                jiexi jx = new jiexi();
                jx.Show();
                jx.Text = fangshi;
               
                   
                
            }
            else
            {

                string fangshi = "解析模式：竞彩" + Regex.Match(ahtml, @"<title>([\s\S]*?)</title>").Groups[1].Value;
                if (!fangshi.Contains("混合过关"))
                {
                    MessageBox.Show("请将页面切换至混合过关");
                }
                else
                {
                    if (zhankai == 0)
                    {
                        HtmlElement bifen = webBrowser1.Document.GetElementById("crsChk");
                        HtmlElement jinqiu = webBrowser1.Document.GetElementById("ttgChk");
                        HtmlElement banquanchang = webBrowser1.Document.GetElementById("hafuChk");

                        bifen.InvokeMember("click");
                        jinqiu.InvokeMember("click");
                        banquanchang.InvokeMember("click");
                        Application.DoEvents();
                        zhankai = 1;
                    }
                    jiexi jx = new jiexi();
                    
                    jx.getdata_jingcai += new jiexi.GetData_jingcai(getdata);
                    jx.Text = fangshi;
                    jx.Show();
                }
            }

        }

        public void cishi(string value)
        {
            MessageBox.Show(value);
        }

        private void button5_Click(object sender, EventArgs e)
        { // Report.Print(true);
            //Report.PrintPreview(true);

            #region 通用检测

            if (!function.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"FCUoF"))
            {
                return;
            }

            #endregion

           

            gethtml();
            if (webBrowser1.Url.ToString().Contains("500"))
            {
                try
                {
                    HtmlElement element2 = webBrowser1.Document.CreateElement("script");
                    element2.SetAttribute("type", "text/javascript");
                    element2.SetAttribute("text", "function _func(){return document.getElementById('buy_bs').value}");   //这里写JS代码
                    HtmlElement head = webBrowser1.Document.Body.AppendChild(element2);
                    fc.beishu_500 = webBrowser1.Document.InvokeScript("_func").ToString();
                }
                catch (Exception)
                {
                    HtmlElement element2 = webBrowser1.Document.CreateElement("script");
                    element2.SetAttribute("type", "text/javascript");
                    element2.SetAttribute("text", "function _func(){k=document.getElementsByTagName('input');for (l = 0; l < k.length; l++){if (k[l].type == 'text') { return k[l].value; }}}");   //这里写JS代码
                    HtmlElement head = webBrowser1.Document.Body.AppendChild(element2);
                    fc.beishu_500 = webBrowser1.Document.InvokeScript("_func").ToString();

                   
                }
                fc.getdata_500(Report, html, ahtml);
            }

            else
            {
                string fangshi = Regex.Match(ahtml, @"<title>([\s\S]*?)</title>").Groups[1].Value;
                //if (fangshi == "足球混合过关")
                //{
                //    fc.getdata(Report, html, ahtml);
                //}
                //if (fangshi == "足球胜平负" || fangshi == "足球半全场胜平负" || fangshi == "足球总进球数" || fangshi == "足球比分")
                //{
                //    fc.getdata_shengpingfu(Report, html, ahtml);
                //}

                fc.getdata(Report, html, ahtml,"");
            }

           




            PreviewForm theForm = new PreviewForm();
            
            theForm.AttachReport(Report);
            theForm.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            IniWriteValue("values", "address", address_txt.Text.ToString());
            IniWriteValue("values", "haoma", haoma_txt.Text.ToString());
            IniWriteValue("values", "bianma", bianma_txt.Text.ToString());
            MessageBox.Show("保存成功","保存提示");

        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void button5_MouseHover(object sender, EventArgs e)
        {
           
        }

        private void button5_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                var btn = webBrowser1.Document.GetElementById("detailBtn");
                btn.InvokeMember("click");

              
            }
            catch (Exception)
            {
                var btn2 = webBrowser1.Document.GetElementById("panelSelectBtn");
                btn2.InvokeMember("click");
            }
        }
    }
}
