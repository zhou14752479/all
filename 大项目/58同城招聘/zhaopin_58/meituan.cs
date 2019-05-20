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

        //#region 获取城市名对应的区域ID
        //public ArrayList getAreaId(string city)
        //{

        //    ArrayList areas = new ArrayList();
        //    string cityPinYin = Getpinyin(city);
        //    try
        //    {
        //        //创建Xml文档。
        //        XmlDocument xml = new XmlDocument();
        //        //加载要读取的xml文件。
        //        xml.Load("http://www.acaiji.com/xml/meituan_area.xml");
        //        // 获得文档中的根节点。
        //        XmlElement xmlElement = xml.DocumentElement;
        //        XmlNodeList nodeList = xmlElement.ChildNodes;
        //        foreach (XmlNode item in nodeList)
        //        {

        //            XmlNodeList nodes = item.ChildNodes;

        //            foreach (XmlNode node in nodes)
        //            {
        //                if (node.Name == "meituan_area_citypinyin" && node.InnerText == cityPinYin)
        //                {
        //                    areas.Add(node.PreviousSibling.InnerText);
        //                }
        //            }
        //        }
        //    }
        //    catch (MySqlException ee)
        //    {
        //        ee.Message.ToString();
        //    }
        //    return areas;
        //}

        //#endregion


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

        private void meituan_Load(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(getCityName));
            thread.Start();

        }
        bool zanting = true;
        bool status = true;

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string URL)
        {
            try
            {

                string COOKIE = "abt=1499071937.0%7CADE; _lxsdk_cuid=15d07a688f6c8-0b50ac5c472b18-333f5902-100200-15d07a688f79c; oc=xrmkO2nLZoY_IrhbS451igIl7hYw7IdDK6N_eVitVxW6WMxwN8yqI07hUj0vwzV57Iemuglzh4HS0E77JWewbxs2LOrDkniIWKv_go8Z8i77EVeAUCUejMdsEHZtdIDxMvg4fR4p53MxVNd2YZr8ZNhk_yZNN_hIE2VChkJKOJI; __mta=54360727.1499071941097.1518360971460.1518584371801.7; iuuid=F457EFC99EEB24DD0A17795BB1F8A91129848721BFF3EE82C45EF7C15E7C210E; _lxsdk=F457EFC99EEB24DD0A17795BB1F8A91129848721BFF3EE82C45EF7C15E7C210E; webp=1; __utmz=74597006.1547084235.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); _hc.v=125767d7-22b6-b361-e802-854d70351873.1547084296; __utma=74597006.1002021895.1547084235.1547426995.1547429915.3; i_extend=C145095553688078665527034436504084189858_b2_e4339319119865529162_v1084616022904540227_a%e8%bf%90%e5%8a%a8%e5%81%a5%e8%ba%ab_f179411730E015954189128616476612677712756937264803_e7765034795530822391_v1084625786170336172_a%e8%bf%90%e5%8a%a8%e5%81%a5%e8%ba%abGimthomepagesearchH__a100005__b3; uuid=42434825e14e46769143.1552287125.1.0.0; cityname=%E4%B9%8C%E9%B2%81%E6%9C%A8%E9%BD%90; ci=10; rvct=10%2C387%2C105%2C73%2C60%2C1%2C184%2C875%2C55%2C40";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Timeout = 10000;
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/16B92 MicroMessenger/7.0.3(0x17000321) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
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
                
                string[] keywords = textBox1.Text.Trim().Split(',');

                string city = comboBox1.SelectedItem.ToString();
              

                    ArrayList areaIds = getAreaId(city);
                    string cityId = GetCityId(city);

                    foreach (string keyword in keywords)

                    {

                        foreach (string area in areaIds)
                        {

                            string Url = "https://apimobile.meituan.com/group/v4/poi/pcsearch/"+cityId+"?cateId=-1&sort=default&userid=-1&offset=0&limit=1000&mypos=33.959859%2C118.279675&uuid=C693C857695CAE55399A30C25D9D05F8914E58638F1E750BFB40CACC3AD5AE9F&pcentrance=6&q="+keyword+"&requestType=filter&cityId="+cityId+"&areaId="+area;

                       
                            string html = GetUrl(Url);
                        
                            
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
                                string strhtml = GetUrl("https://apimobile.meituan.com/group/v1/poi/"+list+"?fields=areaName,frontImg,name,avgScore,avgPrice,addr,openInfo,wifi,phone,featureMenus,isWaimai,payInfo,chooseSitting,cates,lat,lng");  //定义的GetRul方法 返回 reader.ReadToEnd()                             

                            
                                Match name = Regex.Match(strhtml, @"poiid([\s\S]*?)""name"":""([\s\S]*?)""");
                                Match addr = Regex.Match(strhtml, @"addr"":""([\s\S]*?)""");
                                Match tel = Regex.Match(strhtml, @"phone"":""([\s\S]*?)""");
                                Match areaName = Regex.Match(strhtml, @"areaName"":""([\s\S]*?)""");

                            if (name.Groups[2].Value != "")
                                {
                                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                                    lv1.SubItems.Add(name.Groups[2].Value.Trim());
                                    lv1.SubItems.Add(addr.Groups[1].Value.Trim());
                                    lv1.SubItems.Add(tel.Groups[1].Value.Trim());
                                    lv1.SubItems.Add(areaName.Groups[1].Value.Trim());
                                    lv1.SubItems.Add(city);

                                
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
            




            catch (System.Exception ex)
            {
              MessageBox.Show(  ex.ToString());
            }
        }

        #endregion



        #region  有所城市

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
                citys.RemoveAt(0);
                foreach (string city in citys)
                {

                ArrayList areaIds = getAreaId(city);
                string cityId = GetCityId(city);

                    foreach (string keyword in keywords)

                    {

                        foreach (string area in areaIds)
                        {

                            string Url = "https://apimobile.meituan.com/group/v4/poi/pcsearch/" + cityId + "?cateId=-1&sort=default&userid=-1&offset=0&limit=1000&mypos=33.959859%2C118.279675&uuid=C693C857695CAE55399A30C25D9D05F8914E58638F1E750BFB40CACC3AD5AE9F&pcentrance=6&q=" + keyword + "&requestType=filter&cityId=" + cityId + "&areaId=" + area;

                            string html = GetUrl(Url);


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
                                string strhtml = GetUrl("https://apimobile.meituan.com/group/v1/poi/" + list + "?fields=areaName,frontImg,name,avgScore,avgPrice,addr,openInfo,wifi,phone,featureMenus,isWaimai,payInfo,chooseSitting,cates,lat,lng");  //定义的GetRul方法 返回 reader.ReadToEnd()                             


                                Match name = Regex.Match(strhtml, @"poiid([\s\S]*?)""name"":""([\s\S]*?)""");
                                Match addr = Regex.Match(strhtml, @"addr"":""([\s\S]*?)""");
                                Match tel = Regex.Match(strhtml, @"phone"":""([\s\S]*?)""");
                                Match areaName = Regex.Match(strhtml, @"areaName"":""([\s\S]*?)""");

                                if (name.Groups[2].Value != "")
                                {
                                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                                    lv1.SubItems.Add(name.Groups[2].Value.Trim());
                                    lv1.SubItems.Add(addr.Groups[1].Value.Trim());
                                    lv1.SubItems.Add(tel.Groups[1].Value.Trim());
                                    lv1.SubItems.Add(areaName.Groups[1].Value.Trim());
                                    lv1.SubItems.Add(city);


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
        {
            status = true;

            #region 通用登录

            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php");
            string localip = GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim()==localip.Trim())
                {
                    value = true;
                    break;
                }
                
            }
            if(value==true)
            {
                //--------登陆函数------------------
                Thread thread = new Thread(new ThreadStart(run1));
                thread.Start();

            }
            else
            {
                MessageBox.Show("请登录您的账号！");
                System.Diagnostics.Process.Start("http://www.acaiji.com");
                return;
            }
            #endregion




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


            status = false;
        }
    }
}
