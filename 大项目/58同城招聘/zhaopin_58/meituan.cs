using MySql.Data.MySqlClient;
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
using System.Xml;
using System.Xml.Linq;

namespace zhaopin_58
{
    public partial class meituan : Form
    {
        public meituan()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false; // 设置线程之间可以操作
                                                             
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle; //下一句用来禁止对窗口大小进行拖拽
        }

        string IP = "";
        #region 获取数据库美团城市名称
        public void getCityName()
        {
            ArrayList list = new ArrayList();
            try
            {
                //创建Xml文档。
                XmlDocument xml = new XmlDocument();
                //加载要读取的xml文件。
                xml.Load("http://www.acaiji.com/xml/meituan_province_city.xml");
               // 获得文档中的根节点。
                 XmlElement xmlElement = xml.DocumentElement;
                XmlNodeList nodeList = xmlElement.ChildNodes;
                foreach (XmlNode item in nodeList)
                {

                    XmlNodeList nodes = item.ChildNodes;

                    foreach (XmlNode node in nodes)
                    {
                        if (node.Name == "name")
                        {
                            list.Add(node.InnerText);
                        }
                    }
                }

            }
            catch (MySqlException ee)
            {
                MessageBox.Show(ee.Message.ToString());
            }
            comboBox1.DataSource = list;

        }
        #endregion

        #region 获取XML城市名称返回集合
        public ArrayList getCityNames()
        {
            ArrayList list = new ArrayList();
            try
            {
                //创建Xml文档。
                XmlDocument xml = new XmlDocument();
                //加载要读取的xml文件。
                xml.Load("http://www.acaiji.com/xml/meituan_province_city.xml");
                // 获得文档中的根节点。
                XmlElement xmlElement = xml.DocumentElement;
                XmlNodeList nodeList = xmlElement.ChildNodes;
                foreach (XmlNode item in nodeList)
                {

                    XmlNodeList nodes = item.ChildNodes;

                    foreach (XmlNode node in nodes)
                    {
                        if (node.Name == "name")
                        {
                            list.Add(node.InnerText);
                        }
                    }
                }

            }
            catch (MySqlException ee)
            {
                MessageBox.Show(ee.Message.ToString());
            }
            
            return list;

        }
        #endregion

        #region  获取XML城市名称对应的拼音

        public string Getpinyin(string city)
        {

            try
            {
                //创建Xml文档。
                XmlDocument xml = new XmlDocument();
                //加载要读取的xml文件。
                xml.Load("http://www.acaiji.com/xml/meituan_province_city.xml");
                // 获得文档中的根节点。
                XmlElement xmlElement = xml.DocumentElement;
                XmlNodeList nodeList = xmlElement.ChildNodes;
                foreach (XmlNode item in nodeList)
                {

                    XmlNodeList nodes = item.ChildNodes;

                    foreach (XmlNode node in nodes)
                    {
                        if (node.Name == "name"&&node.InnerText==city)
                        {
                            return node.NextSibling.NextSibling.InnerText; //根据xml排序获取南京节点后第二个节点nanjing
                            
                            
                        }
                    }
                }

                return "";
            }

            catch (System.Exception ex)
            {
                return ex.ToString();
            }


        }

        #endregion

        

        #region  获取XML城市名称对应的Id

        public string GetCityId(string city)
        {

            try
            {
                XmlDocument xml = new XmlDocument();
                //加载要读取的xml文件。
                xml.Load("http://www.acaiji.com/xml/meituan_province_city.xml");
                // 获得文档中的根节点。
                XmlElement xmlElement = xml.DocumentElement;
                XmlNodeList nodeList = xmlElement.ChildNodes;
                foreach (XmlNode item in nodeList)
                {

                    XmlNodeList nodes = item.ChildNodes;

                    foreach (XmlNode node in nodes)
                    {
                        if (node.Name == "name" && node.InnerText == city)
                        {
                            // return node.PreviousSibling.InnerText; //根据xml排序获取南京节点之前的节点值即南京的CityID

                            return node.NextSibling.InnerText;
                        }
                    }
                }

                return "";


            }

            catch (System.Exception ex)
            {
                return ex.ToString();
                
            }


        }

        #endregion




        #region 获取城市名对应的区域ID
        public ArrayList getAreaId(string city)
        {
            //visualComboBox1.SelectedItem.ToString()
            ArrayList areas = new ArrayList();
            string cityPinYin = Getpinyin(city);
            try
            {
                string constr = "Host =47.99.68.92;Database=citys;Username=root;Password=zhoukaige00.@*.";
                string str = "SELECT meituan_area_id from meituan_area Where meituan_area_citypinyin= '" + cityPinYin + "' ";
                MySqlDataAdapter da = new MySqlDataAdapter(str, constr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    areas.Add(dr[0].ToString().Trim());
                }
            }
            catch (MySqlException ee)
            {
                ee.Message.ToString();
            }
            return areas;
        }

        #endregion

        #region  代理iP

        public void getIp()
        {
            string ahtml = method.GetUrl(textBox2.Text);
            this.IP = ahtml.Trim();

        }
        #endregion
        private void meituan_Load(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(getCityName));
            thread.Start();

        }
        bool zanting = true;
        bool status = true;



        #region GET使用代理IP请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrlwithIP(string Url, string ip)
        {
            try
            {

                string COOKIE = "_lxsdk_cuid=1619d225de7ad-0c5551b9982ceb-3b60490d-1fa400-1619d225de8c8; iuuid=EE452628FFED0D7E2E8057602BBF1FB40975D7AC4634E49AA9A0FDF0EEA3FC2F; _lxsdk=EE452628FFED0D7E2E8057602BBF1FB40975D7AC4634E49AA9A0FDF0EEA3FC2F; _ga=GA1.2.1049379255.1526306541; __mta=46562712.1518765142494.1532784659235.1533443717244.73; oc=HHdE1pzMFrdyJAh4aEBuGv_bXE5LSe-aHksZ9tr_sc4-RplxzhG0a9w-vgyjGw4e7nLCFL_rT0P5voF0RmqlIA-s5fsbOkZgu6cxdlreV5nixPpfpB6Z_Xb-Z8LqqLKzcOpgTWVZjJApjKPMcAwtdt3vQlMNkzzEDdwYh0Ks_uE; __utmz=74597006.1537842327.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); a2h=3; _hc.v=b73786d9-299a-936e-97c9-338477597e3b.1556257481; webp=1; __utma=74597006.1049379255.1526306541.1552472120.1566712155.6; cityname=%E5%AE%BF%E8%BF%81; i_extend=C_b1Gimthomepagecategory1394H__a; ci=1; rvct=1%2C70%2C59%2C10%2C101; uuid=d8d2a5a381cb4ca5ab4c.1572670645.1.0.0; lat=33.95258; lng=118.28334";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";
               
                WebProxy proxy = new WebProxy(ip);
                request.Proxy = proxy;
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 8000;
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


        #region  主程序按照单个城市多个关键词

        public void run()
        {


            try
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("请输入关键字");
                    return;
                }
                button1.Enabled = false;
                

                string[] keywords = textBox1.Text.Trim().Split(',');

                string city = comboBox1.Text;

                string cityId = GetCityId(city);


                    foreach (string keyword in keywords)

                    {

                        for (int i = 0; i < 1200; i = i + 15)
                        {

                            string Url = "https://apimobile.meituan.com/group/v4/poi/pcsearch/"+cityId+"?riskLevel=71&optimusCode=10&cateId=-1&sort=defaults&userid=-1&offset="+i+"&limit=15&mypos=33.941062927246094%2C118.24803924560547&uuid=E82ADB4FE4B6D0984D5B1BEA4EE9DE13A16B4B25F8A306260A976B724DF44576&pcentrance=6&cityId="+cityId+"&q="+keyword;


                            string html = GetUrlwithIP(Url,IP);
                     
                      
                        MatchCollection TitleMatchs = Regex.Matches(html, @"false},{""id"":([\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                            ArrayList lists = new ArrayList();

                            foreach (Match NextMatch in TitleMatchs)
                            {

                                lists.Add(NextMatch.Groups[1].Value);
                            }


                            //if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            //    break;
                            foreach (string list in lists)
                            {
                                string aurl = "https://mapi.meituan.com/general/platform/mtshop/poiinfo.json?poiid="+list;
                                //  string aurl = "https://i.meituan.com/wrapapi/allpoiinfo?riskLevel=71&optimusCode=10&poiId=" + list;
                                //string aurl = "https://apimobile.meituan.com/group/v1/poi/" + list;
                                string strhtml = GetUrlwithIP(aurl, IP);  //定义的GetRul方法 返回 reader.ReadToEnd()                             

                           
                            Match name = Regex.Match(strhtml, @"""name"":""([\s\S]*?)""");

                                Match addr = Regex.Match(strhtml, @"""addr"":""([\s\S]*?)""");
                                Match tel = Regex.Match(strhtml, @"""phone"":""([\s\S]*?)""");
                                Match areaName = Regex.Match(strhtml, @"""areaName"":""([\s\S]*?)""");
                                Match score = Regex.Match(strhtml, @"avgScore"":([\s\S]*?),");
                           
                            if (name.Groups[1].Value=="")
                            {
                                getIp();
                                
                            }

                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                                    lv1.SubItems.Add(name.Groups[1].Value.Trim());
                                    lv1.SubItems.Add(addr.Groups[1].Value.Trim());
                                    lv1.SubItems.Add(tel.Groups[1].Value.Trim());
                                    lv1.SubItems.Add(areaName.Groups[1].Value.Trim());
                                    lv1.SubItems.Add(city);
                                   
                                  
                            if (checkBox1.Checked == true)
                            {
                               
                                Match imgurl = Regex.Match(strhtml, @"frontImg"":""([\s\S]*?)""");

                                if (imgurl.Groups[1].Value.Replace("/w.h", "")!= "")   //判断请求图片的网址响应是否为空，如果为空表示没有图片，下载会报错！
                                {
                                    method.downloadFile(imgurl.Groups[1].Value.Replace("/w.h", ""), AppDomain.CurrentDomain.BaseDirectory + "图片", name.Groups[1].Value.Trim()+ tel.Groups[1].Value.Trim() + ".jpg");

                                }

                            }
                              if (listView1.Items.Count - 1 > 1)
                                {
                                    listView1.EnsureVisible(listView1.Items.Count - 1);
                                }
                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }

                                if (status == false)
                                {
                                    return;
                                }

                                Thread.Sleep(1000);   //内容获取间隔，可变量


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



        #region  所有城市

        public void run1()
        {


            try
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("请输入关键字");
                    return;
                }

                string[] keywords = textBox1.Text.Trim().Split(',');

                ArrayList citys = getCityNames();
                
                foreach (string city in citys)
                {

                ArrayList areaIds = getAreaId(city);
                string cityId = GetCityId(city);

                    foreach (string keyword in keywords)

                    {

                        foreach (string area in areaIds)
                        {

                            string Url = "https://apimobile.meituan.com/group/v4/poi/pcsearch/" + cityId + "?cateId=-1&sort=default&userid=-1&offset=0&limit=1000&mypos=33.959859%2C118.279675&uuid=C693C857695CAE55399A30C25D9D05F8914E58638F1E750BFB40CACC3AD5AE9F&pcentrance=6&q=" + keyword + "&requestType=filter&cityId=" + cityId + "&areaId=" + area;

                            string html = method.GetUrl(Url);


                            MatchCollection TitleMatchs = Regex.Matches(html, @"false},{""id"":([\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                            ArrayList lists = new ArrayList();

                            foreach (Match NextMatch in TitleMatchs)
                            {

                                lists.Add(NextMatch.Groups[1].Value);
                            }


                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;
                            foreach (string list in lists)
                            {
                                string strhtml = method.gethtml("https://apimobile.meituan.com/group/v1/poi/" + list + "?fields=areaName,frontImg,name,avgScore,avgPrice,addr,openInfo,wifi,phone,featureMenus,isWaimai,payInfo,chooseSitting,cates,lat,lng");  //定义的GetRul方法 返回 reader.ReadToEnd()                             

                               // string commentHtml = method.gethtml("https://www.meituan.com/meishi/api/poi/getMerchantComment?uuid=f87c45af885944f3a19d.1564549522.1.0.0&platform=1&riskLevel=1&optimusCode=10&id="+list);
                                Match name = Regex.Match(strhtml, @"poiid([\s\S]*?)""name"":""([\s\S]*?)""");
                                Match addr = Regex.Match(strhtml, @"addr"":""([\s\S]*?)""");
                                Match tel = Regex.Match(strhtml, @"phone"":""([\s\S]*?)""");
                                Match areaName = Regex.Match(strhtml, @"areaName"":""([\s\S]*?)""");
                                //Match comment = Regex.Match(commentHtml, @"""total"":([\s\S]*?)\}");

                                if (name.Groups[2].Value != "")
                                {
                                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                                    lv1.SubItems.Add(name.Groups[2].Value.Trim());
                                    lv1.SubItems.Add(addr.Groups[1].Value.Trim());
                                    lv1.SubItems.Add(tel.Groups[1].Value.Trim());
                                    lv1.SubItems.Add(areaName.Groups[1].Value.Trim());
                                    lv1.SubItems.Add(city);
                                   // lv1.SubItems.Add(comment.Groups[1].Value.Trim());





                                    //下载图片

                                    //Match imgurl = Regex.Match(strhtml, @"frontImg"":""([\s\S]*?)""");

                                    //if (method.GetUrl(imgurl.Groups[1].Value.Replace("/w.h", ""))!="")   //判断请求图片的网址响应是否为空，如果为空表示没有图片，下载会报错！
                                    //{
                                    //    method.downloadFile(imgurl.Groups[1].Value.Replace("/w.h",""), AppDomain.CurrentDomain.BaseDirectory + "图片", name.Groups[2].Value.Trim() + ".jpg");

                                    //}

                                    //下载图片结束

                                    if (strhtml.Contains("有外卖"))
                                    {
                                        lv1.SubItems.Add("有外卖");
                                    }
                                    else
                                    {
                                        lv1.SubItems.Add("无外卖");
                                    }


                                    if (listView1.Items.Count - 1 > 1)
                                    {
                                        listView1.EnsureVisible(listView1.Items.Count - 1);
                                    }
                                    while (this.zanting == false)
                                    {
                                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                    }

                                    if (status == false)
                                    {
                                        return;
                                    }

                                    Thread.Sleep(1000);   //内容获取间隔，可变量

                                }

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




        private void 清空数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

      
     

        private void visualComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        public static IPAddress Getlocalipaddress() 
        {
            if (Dns.GetHostAddresses(Dns.GetHostName()).Length > 1)
                return Dns.GetHostAddresses(Dns.GetHostName())[1];
            else
                return Dns.GetHostAddresses(Dns.GetHostName())[0];
        }

        #region 获取公网IP
        public static string GetIP()
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    webClient.Credentials = CredentialCache.DefaultCredentials;
                    byte[] pageDate = webClient.DownloadData("http://pv.sohu.com/cityjson?ie=utf-8");
                    String ip = Encoding.UTF8.GetString(pageDate);
                    webClient.Dispose();

                    Match rebool = Regex.Match(ip, @"\d{2,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
                    return rebool.Value;
                }
                catch (Exception e)
                {
                    return e.ToString();
                }

            }
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {if (textBox2.Text == "")
            {
                MessageBox.Show("请输入代理IP地址,选择网站下方【生成API链接】其他不变，然后复制隧道链接，每天可以领取免费IP");
                
                System.Diagnostics.Process.Start("http://h.zhimaruanjian.com/getapi/");
                return;
            }


            getIp();
            status = true;

            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.zanting = false;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.zanting = true;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            button1.Enabled = true;

            status = false;
        }

        private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].SubItems[3].Text.Contains("-"))
                {
                    listView1.Items.Remove(listView1.Items[i]);
                }
            }
        }

        private void LinkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].SubItems[3].Text=="")
                {
                    listView1.Items.Remove(listView1.Items[i]);
                }
            }
        }

     
        private void ComboBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void Meituan_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }
    }
}
