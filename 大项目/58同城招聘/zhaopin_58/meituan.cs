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


        //下载图片

        //Match imgurl = Regex.Match(strhtml, @"frontImg"":""([\s\S]*?)""");

        //if (method.GetUrl(imgurl.Groups[1].Value.Replace("/w.h", ""))!="")   //判断请求图片的网址响应是否为空，如果为空表示没有图片，下载会报错！
        //{
        //    method.downloadFile(imgurl.Groups[1].Value.Replace("/w.h",""), AppDomain.CurrentDomain.BaseDirectory + "图片", name.Groups[2].Value.Trim() + ".jpg");

        //}

        //下载图片结束


        #region  主程序按照单个个城市多个关键词

        public void run()
        {


            try
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("请输入关键字");
                    return;
                }
               // string[] citys = textBox2.Text.Split(new string[] { "," }, StringSplitOptions.None);
                string[] keywords = textBox1.Text.Trim().Split(',');

                string city = comboBox1.SelectedItem.ToString();

                    ArrayList areaIds = getAreaId(city);
                    string cityId = GetCityId(city);


                    foreach (string area in areaIds)
                    {

                        foreach (string keyword in keywords)

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
                                string aurl = "https://apimobile.meituan.com/group/v1/poi/" + list + "?fields=areaName,frontImg,name,avgScore,avgPrice,addr,openInfo,wifi,phone,featureMenus,isWaimai,payInfo,chooseSitting,cates,lat,lng";

                                string strhtml = method.gethtml(aurl);  //定义的GetRul方法 返回 reader.ReadToEnd()                             


                                Match name = Regex.Match(strhtml, @"poiid([\s\S]*?)""name"":""([\s\S]*?)""");
                                // Match name = Regex.Match(strhtml, @"""name"":""([\s\S]*?)""");
                                Match addr = Regex.Match(strhtml, @"addr"":""([\s\S]*?)""");
                                Match tel = Regex.Match(strhtml, @"phone"":""([\s\S]*?)""");
                                Match areaName = Regex.Match(strhtml, @"areaName"":""([\s\S]*?)""");
                                Match opentime = Regex.Match(strhtml, @"openInfo"":""([\s\S]*?)""");




                                if (name.Groups[2].Value != "")
                                {

                                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                                    lv1.SubItems.Add(name.Groups[2].Value.Trim());
                                    lv1.SubItems.Add(addr.Groups[1].Value.Trim());
                                    lv1.SubItems.Add(tel.Groups[1].Value.Trim());
                                    lv1.SubItems.Add(areaName.Groups[1].Value.Trim());
                                    lv1.SubItems.Add(city);
                                    lv1.SubItems.Add(opentime.Groups[1].Value);



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
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();

            }
            else
            {
                MessageBox.Show("请登录您的账号！登陆成功返回软件使用即可");
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
    }
}
