using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
using System.Diagnostics;

namespace 美团
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
       
        public string cookie= "_lxsdk_cuid=178002168e9c8-00e8f9125e0e71-31346d-1fa400-178002168e9c8; _hc.v=952fc3ea-25d0-cec1-62e1-9a56ab2e59a2.1614909934; iuuid=5ABBD748F10D7D43BC85FC9A175AA637BE6D53DCFCAD8E6A9288E639A883BE86; isid=wdDnkGS0cKe4o6R8vWfMZRKlyzwAAAAASg0AAH-mOkBAv1T2k0dxjzBjWO-uBTucKnGdGRrV9dg1cMSoFfV5MiArYB-zINeWnfGXHQ; logintype=normal; cityname=%E5%AE%BF%E8%BF%81; webp=1; _lxsdk=5ABBD748F10D7D43BC85FC9A175AA637BE6D53DCFCAD8E6A9288E639A883BE86; __utma=74597006.1034064235.1618288921.1618288921.1618288921.1; __utmz=74597006.1618288921.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); i_extend=C_b1Gimthomepagecategory11H__a; ci=10; rvct=10%2C184%2C56; wm_order_channel=default; lsu=; uuid=2cf4930eac7d40098af2.1622003918.1.0.0; __mta=55425684.1614909900927.1622167521874.1622512936250.22; client-id=5495ec2e-e905-4693-856d-55cd4a1e5796; mtcdn=K; u=875973616; n=Ffv936639060; lt=XnzgHn320zBj-UR-NxUAdC5ASZkAAAAArQ0AAM3qGbpxqlQ57s3u6nAup6ywraPDROIPbKkS_3g1eRHX0eZJb6vgoJHDE2ncVtRpHg; mt_c_token=XnzgHn320zBj-UR-NxUAdC5ASZkAAAAArQ0AAM3qGbpxqlQ57s3u6nAup6ywraPDROIPbKkS_3g1eRHX0eZJb6vgoJHDE2ncVtRpHg; token=XnzgHn320zBj-UR-NxUAdC5ASZkAAAAArQ0AAM3qGbpxqlQ57s3u6nAup6ywraPDROIPbKkS_3g1eRHX0eZJb6vgoJHDE2ncVtRpHg; token2=XnzgHn320zBj-UR-NxUAdC5ASZkAAAAArQ0AAM3qGbpxqlQ57s3u6nAup6ywraPDROIPbKkS_3g1eRHX0eZJb6vgoJHDE2ncVtRpHg; firstTime=1622512943858; unc=Ffv936639060; _lxsdk_s=179c54ea3e6-631-acd-8cf%7C%7C4";
        bool zanting = true;
        public static string username = "";
        ArrayList tels = new ArrayList();
        private void Form1_Load(object sender, EventArgs e)
        {
            ProvinceCity.ProvinceCity.BindProvince(comboBox1);
         

            label3.Text = username;
           
        }

        bool status = true;
        

        #region GET请求
        public static string meituan_GetUrl(string Url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.97 Safari/537.11";
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 13_6_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.13(0x17000d2a) NetType/4G Language/zh_CN";
                WebHeaderCollection headers = request.Headers;
                headers.Add("uuid: E82ADB4FE4B6D0984D5B1BEA4EE9DE13A16B4B25F8A306260A976B724DF44576");
                headers.Add("open_id: oJVP50IRqKIIshugSqrvYE3OHJKQ");
                headers.Add("token: Vteo9CkJqIGMe30FC3iuvnvTr2YAAAAAygoAAMPHPyLNO16W1eYLn1hWsLhD40r-KnDdB70rrl9LN9OHUfVBGbTDt4PCDHH72xKkDA");
                
                request.Referer = "https://servicewechat.com/wxde8ac0a21135c07d/328/page-frame.html";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

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

        #region GET请求2
        public string GetUrl(string Url)
        {
            try
            {


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                string COOKIE = cookie;
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.11(0x17000b21) NetType/4G Language/zh_CN";
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("openIdCipher", "AwQAAABJAgAAAAEAAAAyAAAAPLgC95WH3MyqngAoyM/hf1hEoKrGdo0pJ5DI44e1wGF9AT3PH7Wes03actC2n/GVnwfURonD78PewMUppAAAADhS4d+zREPZw1PQNF/0Zp8SLSbtYsmCKZFYbIjL5Ty7FJZwQ/bkMIGOGHHGqk1Nld5+rcxtuifNmA==");
                request.Referer = "https://servicewechat.com/wxde8ac0a21135c07d/350/page-frame.html";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

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

        #region  获取城市ID

        public string GetcityId(string city)
        {

            try
            {
                string url = "https://apimobile.meituan.com/group/v1/area/search/"+ System.Web.HttpUtility.UrlEncode(city.Replace("市",""));
                string html = GetUrl(url);
                string cityId = Regex.Match(html, @"""cityId"":([\s\S]*?),").Groups[1].Value;
               
                return cityId;
            }

            catch (System.Exception ex)
            {
                return ex.ToString();
            }

        }

        #endregion

        #region  获取城市拼音缩写

        public string Getsuoxie(string city)
        {

            try
            {
                string url = "https://apimobile.meituan.com/group/v1/area/search/"+System.Web.HttpUtility.UrlEncode(city);
                string html = GetUrl(url);
                string suoxie = Regex.Match(html, @"""cityAcronym"":""([\s\S]*?)""").Groups[1].Value;
              
                return suoxie;
            }

            catch (System.Exception ex)
            {
                return ex.ToString();
            }


        }

        #endregion


        #region 获取区域
        public ArrayList getareas(string city)
        {
            string Url = "https://"+city+".meituan.com/meishi/";
          
            string html = GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()

            MatchCollection areas = Regex.Matches(html, @"""subAreas"":\[\{""id"":([\s\S]*?),");
            ArrayList lists = new ArrayList();

            if (areas.Count == 0)
            {
               toolStripStatusLabel1.Text=("获取区域失败");
                Process.Start(path + "helper.exe");
            }
            foreach (Match item in areas)
            {
               
               
                lists.Add(item.Groups[1].Value);
            }
          
            return lists;
        }

        #endregion


        #region  获取所有城市

        public ArrayList GetAllcityName()
        {
            ArrayList lists = new ArrayList();
            try
            {
                string url = "https://www.meituan.com/ptapi/getprovincecityinfo/";
                string html = GetUrl(url);
                MatchCollection cityId = Regex.Matches(html, @"""name"":""([\s\S]*?)""");
                foreach (Match match in cityId)
                {
                    lists.Add(match.Groups[1].Value);
                }
                return lists;
            }

            catch (System.Exception ex)
            {
                return lists;
            }

        }

        #endregion


        #region  主程序
        public void run()
        {

            try
            {
                 string[] citys = textBox1.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
               
                if (textBox1.Text.Trim() == "")
                {
                    MessageBox.Show("请输入城市！");
                    return;
                }

             

                string[] keywords = textBox2.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);


                foreach (string city in citys)
                {

                    ArrayList areas = getareas(Getsuoxie(city));

                    string cityId = GetcityId(city);


                    foreach (string keyword in keywords)

                    {

                        foreach (string areaId in areas)
                        {


                            for (int i = 0; i < 1000; i = i + 15)

                            {
                                toolStripStatusLabel1.Text = "正在抓取：" + city + "," + keyword;

                                string Url = "https://apimobile.meituan.com/group/v4/poi/search/miniprogram/" + cityId + "?riskLevel=71&optimusCode=10&cateId=-1&sort=defaults&userid=-1&offset=" + i + "&limit=15&mypos=33.94123077392578%2C118.24799346923828&uuid=178c0a12928c8-18eda8e20c814f-0-0-178c0a12928c8&version_name=10.4.200&supportDisplayTemplates=itemA%2CitemB%2CitemJ%2CitemP%2CitemS%2CitemM%2CitemY%2CitemL&supportTemplates=default%2Chotel%2Cblock%2Cnofilter%2Ccinema&searchSource=miniprogram&ste=_b100000&openId=oJVP50IRqKIIshugSqrvYE3OHJKQ&cityId=" + cityId + "&q=" + keyword + "&areaId=" + areaId;

                                string html = GetUrl(Url); ;  //定义的GetRul方法 返回 reader.ReadToEnd()

                                MatchCollection all = Regex.Matches(html, @"\{""poiid"":([\s\S]*?),");

                                ArrayList lists = new ArrayList();
                                foreach (Match NextMatch in all)
                                {

                                    //https://apimobile.meituan.com/group/v1/poi/194905459?fields=areaName,frontImg,name,avgScore,avgPrice,addr,openInfo,wifi,phone,featureMenus,isWaimai,payInfo,chooseSitting,cates,lat,lng
                                    //lists.Add("https://mapi.meituan.com/general/platform/mtshop/poiinfo.json?poiid=" + NextMatch.Groups[1].Value);
                                    //lists.Add("http://i.meituan.com/poi/" + NextMatch.Groups[1].Value);
                                    lists.Add("https://i.meituan.com/wrapapi/poiinfo?poiId=" + NextMatch.Groups[1].Value);
                                    //lists.Add("https://i.meituan.com/wrapapi/allpoiinfo?riskLevel=71&optimusCode=10&poiId=" + NextMatch.Groups[1].Value + "&isDaoZong=true");  
                                }

                             

                                if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                    break;

                                string tm1 = DateTime.Now.ToString();  //获取系统时间

                                toolStripStatusLabel1.Text = tm1 + "-->正在采集" + city + areaId + keyword + "第" + i + "页";

                                foreach (string list in lists)

                                {

                                    string strhtml1 = meituan_GetUrl(list);  //定义的GetRul方法 返回 reader.ReadToEnd()
                                  
                                    Match name = Regex.Match(strhtml1, @"name"":""([\s\S]*?)""");
                                    Match tel = Regex.Match(strhtml1, @"phone"":""([\s\S]*?)""");
                                    Match addr = Regex.Match(strhtml1, @"address"":""([\s\S]*?)""");
                                    Match score = Regex.Match(strhtml1, @"score"":([\s\S]*?),");
                                    if (!tels.Contains(tel.Groups[1].Value))
                                    {
                                        tels.Add(tel.Groups[1].Value);

                                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                        listViewItem.SubItems.Add(name.Groups[1].Value);
                                        listViewItem.SubItems.Add(tel.Groups[1].Value);
                                        listViewItem.SubItems.Add(addr.Groups[1].Value);
                                        listViewItem.SubItems.Add(city);
                                        listViewItem.SubItems.Add(score.Groups[1].Value);


                                        while (this.zanting == false)
                                        {
                                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                        }
                                        if (status == false)
                                        {
                                            return;
                                        }


                                    }

                                    Thread.Sleep(1000);


                                }

                                Thread.Sleep(2000);
                            }




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

        #region  自动全国
        public void run1()
        {

            try
            {
                ArrayList citys = GetAllcityName();

               

                if (textBox2.Text == "")
                {
                    textBox2.Text = "美食, 火锅, 烧烤, 麻辣烫, 面包, 蛋糕, 奶茶, 快餐, 面条, 西餐, 中餐,小吃, 自助餐, 烤鱼, 海鲜, 甜点, 炒菜";
                }

                string[] keywords = textBox2.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);


                foreach (string city in citys)
                {

                    ArrayList areas = getareas(Getsuoxie(city));

                    string cityId = GetcityId(city);
                    foreach (string areaId in areas)
                    {

                        foreach (string keyword in keywords)

                        {

                            for (int i = 0; i < 1000; i = i + 15)

                            {


                                string Url = "https://apimobile.meituan.com/group/v4/poi/search/" + cityId + "?riskLevel=71&optimusCode=10&cateId=-1&sort=default&userid=-1&offset=" + i + "&limit=15&mypos=33.94108581542969%2C118.24807739257812&uuid=E82ADB4FE4B6D0984D5B1BEA4EE9DE13A16B4B25F8A306260A976B724DF44576&version_name=10.4.200&supportDisplayTemplates=itemA%2CitemB%2CitemJ%2CitemP%2CitemS%2CitemM%2CitemY%2CitemL&supportTemplates=default%2Chotel%2Cblock%2Cnofilter%2Ccinema&searchSource=miniprogram&ste=_b100000&q=" + keyword.Trim() + "&requestType=filter&cityId=" + cityId + "&areaId=" + areaId;


                                string html = GetUrl(Url); ;  //定义的GetRul方法 返回 reader.ReadToEnd()

                                MatchCollection all = Regex.Matches(html, @"\{""poiid"":([\s\S]*?),");

                                ArrayList lists = new ArrayList();
                                foreach (Match NextMatch in all)
                                {

                                    //https://apimobile.meituan.com/group/v1/poi/194905459?fields=areaName,frontImg,name,avgScore,avgPrice,addr,openInfo,wifi,phone,featureMenus,isWaimai,payInfo,chooseSitting,cates,lat,lng
                                    //lists.Add("https://mapi.meituan.com/general/platform/mtshop/poiinfo.json?poiid=" + NextMatch.Groups[1].Value);
                                    //lists.Add("http://i.meituan.com/poi/" + NextMatch.Groups[1].Value);
                                    //lists.Add("https://i.meituan.com/wrapapi/poiinfo?poiId=" + NextMatch.Groups[1].Value);
                                    lists.Add("https://i.meituan.com/wrapapi/allpoiinfo?riskLevel=71&optimusCode=10&poiId=" + NextMatch.Groups[1].Value + "&isDaoZong=false");
                                }

                                if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                    break;

                                string tm1 = DateTime.Now.ToString();  //获取系统时间

                                toolStripStatusLabel1.Text = tm1 + "-->正在采集" + city + areaId + keyword + "第" + i + "页";

                                foreach (string list in lists)

                                {

                                    string strhtml1 = meituan_GetUrl(list);  //定义的GetRul方法 返回 reader.ReadToEnd()

                                    Match name = Regex.Match(strhtml1, @"name"":""([\s\S]*?)""");
                                    Match tell = Regex.Match(strhtml1, @"phone"":""([\s\S]*?)""");
                                    Match addr = Regex.Match(strhtml1, @"address"":""([\s\S]*?)""");
                                    Match score = Regex.Match(strhtml1, @"score"":([\s\S]*?),");
                                    if (!tels.Contains(tell.Groups[1].Value))
                                    {


                                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                        listViewItem.SubItems.Add(name.Groups[1].Value);
                                        listViewItem.SubItems.Add(tell.Groups[1].Value);
                                        listViewItem.SubItems.Add(addr.Groups[1].Value);
                                        listViewItem.SubItems.Add(city);
                                        listViewItem.SubItems.Add(score.Groups[1].Value);


                                        while (this.zanting == false)
                                        {
                                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                        }
                                        if (status == false)
                                        {
                                            return;
                                        }

                                        Thread.Sleep(1000);
                                    }




                                }

                                Thread.Sleep(2000);
                            }




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

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {

            }
        }

        private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized; //最小化
        }
        private Point mPoint = new Point();
        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;

        }

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            SetWindowRegion();
        }

        public void SetWindowRegion()
        {
            System.Drawing.Drawing2D.GraphicsPath FormPath;
            FormPath = new System.Drawing.Drawing2D.GraphicsPath();
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            FormPath = GetRoundedRectPath(rect, 10);
            this.Region = new Region(FormPath);

        }
        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            int diameter = radius;
            Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));
            GraphicsPath path = new GraphicsPath();

            // 左上角
            path.AddArc(arcRect, 180, 90);

            // 右上角
            arcRect.X = rect.Right - diameter;
            path.AddArc(arcRect, 270, 90);

            // 右下角
            arcRect.Y = rect.Bottom - diameter;
            path.AddArc(arcRect, 0, 90);

            // 左下角
            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90, 90);
            path.CloseFigure();//闭合曲线
            return path;
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;
        Thread thread;
        
        private void Button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html");

            if (!html.Contains(@"147258369"))
            {
                MessageBox.Show("");
                return;
            }



            #endregion


            try 
            {
          
                if (File.Exists(path + "cookie.txt"))
                {

                    StreamReader sr = new StreamReader(path + "cookie.txt", method.EncodingType.GetTxtType(path + "cookie.txt"));
                    //一次性读取完 
                    string texts = sr.ReadToEnd();

                    cookie = Regex.Match(texts, @"cookie=([\s\S]*?)&").Groups[1].Value;
                    sr.Close();  //只关闭流
                    sr.Dispose();   //销毁流内存
                    if (cookie == "")
                    {

                        Process.Start(path + "helper.exe");
                        return;
                    }
                }
                else
                {
                    Process.Start(path + "helper.exe");
                    return;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            status = true;
            //button1.Enabled = false;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            //status = true;
            //button1.Enabled = false;
            //Thread search_thread = new Thread(new ThreadStart(run1));
            //Control.CheckForIllegalCrossThreadCalls = false;
            //search_thread.Start();


        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

    

      

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //System.Diagnostics.Process.Start("http://www.acaiji.com");
        }

        private void LinkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
        }

       

     

        private void LinkLabel11_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            button1.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            status = false;
            button1.Enabled = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

       

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProvinceCity.ProvinceCity.BindCity(comboBox1, comboBox2);
        }

       

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text.Contains("上海"))
            {
                textBox1.Text += "上海";
                return;
            }


            if (textBox1.Text.Contains(comboBox2.Text))
            {
                MessageBox.Show(comboBox2.Text + "：请勿重复添加", "重复添加错误");
                return;
            }
            textBox1.Text += comboBox2.Text + "\r\n";

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text.Contains("上海"))
            {
                textBox1.Text += "上海";
                return;
            }

            for (int i = 0; i <comboBox2.Items.Count; i++)
            {
                textBox1.Text += comboBox2.Items[i] + "\r\n";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            Process.Start(path + "helper.exe");
        }
    }
}
