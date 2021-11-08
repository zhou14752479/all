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
using Microsoft.VisualBasic;
using myDLL;
using Newtonsoft.Json.Linq;

namespace 工商企业采集
{
    public partial class 工商企业采集 : Form
    {
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

        public 工商企业采集()
        {
            InitializeComponent();
        }

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            string html = "";
         
            string COOKIE = "BDPPN=4c066e75c0ddb0f5b340703df1d3643b; _j54_6ae_=xlTM-TogKuTwJPccNhmEyqbZe5Px5T1rggmd; BDUSS=ctdHM0SGlzNm5GYmtsLTFOZ2l1ZmN-SDdpNGhrYWFQcmlucFdUMnFjQjUxNlZoSVFBQUFBJCQAAAAAAQAAAAEAAABio5cbemhvdTE0NzUyNDc5AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAHlKfmF5Sn5hWE; BAIDUID=C489197E4BB8F24873D9FE13B4F97223:FG=1";
           // COOKIE = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "aiinquiry/2.3.6 (iPhone; iOS 13.6.1; Scale/3.00) aiqicha/2.3.6";
                request.Referer = "https://aiqicha.baidu.com/usercenter";
                //添加头部
                WebHeaderCollection headers = request.Headers;
                //headers.Add("Cuid:72BAF9EA500309A310153C6EB6F523F4F1F0101B1FBGEHHGFGJ");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;

                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }

                response.Close();
                return html;



            }
            catch (System.Exception ex)
            {
                return ex.ToString();

            }



        }
        #endregion

        public string gettel(string id)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                //string url = "https://aiqicha.baidu.com/company_detail_"+ id;
                string url = "https://aiqicha.baidu.com/appcompdata/headinfoAjax?pid=" + id;
                string html = GetUrl(url, "utf-8");

                MatchCollection phones = Regex.Matches(html, @"""phone"":""([\s\S]*?)""");
                foreach (Match item in phones)
                {
                    if (item.Groups[1].Value.Length > 5)
                    {
                        if (radioButton2.Checked == true)
                        {
                            if (!item.Groups[1].Value.Contains("-") && item.Groups[1].Value.Length > 10 && item.Groups[1].Value.Substring(0, 1) == "1")
                            {

                                return item.Groups[1].Value;
                            }
                        }
                        else
                        {
                            sb.Append(item.Groups[1].Value + ",");
                        }

                    }

                }

                if (sb.ToString().Length > 2)
                {
                    return sb.ToString().Remove(sb.ToString().Length - 1, 1);
                }
                else
                {
                    return sb.ToString();
                }
            }
            catch (Exception ex)
            {

                return "" ;
            }


            
        }
       
        public void run()
        {

            StringBuilder sb = new StringBuilder();
            if (comboBox1.Text != "不限")
            {
                sb.Append(string.Format("\"entType\":[\"{0}\"],", dic[comboBox1.Text]));
            }
            if (comboBox2.Text != "不限")
            {
                string[] text = dic[comboBox2.Text].Split(new string[] { "-" }, StringSplitOptions.None);
                sb.Append("\"regCap\":[{\"start\":"+text[0]+ ",\"end\":" + text[1] + "}],");
            }
            if (comboBox3.Text != "不限")
            {
                sb.Append(string.Format("\"openStatus\":[\"{0}\"],", System.Web.HttpUtility.UrlEncode(dic[comboBox3.Text])));
            }
            if (comboBox4.Text != "不限")
            {
                string[] text = dic[comboBox4.Text].Split(new string[] { "-" }, StringSplitOptions.None);
                sb.Append("\"startYear\":[{\"start\":\""+text[0]+"\",\"end\":\""+text[1]+"\"}],");
            }
            if (comboBox5.Text != "不限")
            {
                sb.Append(string.Format("\"industryCode1\":[\"{0}\"],", dic[comboBox5.Text]));
            }

            if (comboBox8.Text != "不限")
            {
                sb.Append(string.Format("\"provinceCode\":[\"{0}\"],", jsondica[comboBox8.Text]));
            }
            else
            {
                if (comboBox7.Text != "不限")
                {
                    sb.Append(string.Format("\"provinceCode\":[\"{0}\"],", jsondicc[comboBox7.Text]));
                }
                else
                {
                    if (comboBox6.Text != "不限")
                    {
                        sb.Append(string.Format("\"provinceCode\":[\"{0}\"],", jsondicp[comboBox6.Text]));
                    }
                    
                }
            }
            

            string filter = "";
            if (sb.ToString().Length > 2)
            {
                filter = sb.ToString().Remove(sb.ToString().Length - 1, 1).Replace("\"", "%22").Replace("{", "%7B").Replace("}", "%7D");
            }
            try
            {
                for (int page= 1; page < 10001; page++)
                {

                    string url = "https://aiqicha.baidu.com/search/advanceFilterAjax?q=" + System.Web.HttpUtility.UrlEncode(textBox2.Text) + "&t=111&p="+page+"&s=10&o=0&f=%7B"+filter+"%7D";
                    //string html = method.GetUrlWithCookie(url, cookie,"utf-8");
                    string html = GetUrl(url,"utf-8");
                   
                    html = method.Unicode2String(html);
                    label7.Text = "正在采集....第"+page;
                    //textBox1.Text = html;
                    MatchCollection uids = Regex.Matches(html, @"""pid"":""([\s\S]*?)""");
                    MatchCollection entName = Regex.Matches(html, @"""titleName"":""([\s\S]*?)""");
                    MatchCollection legalPerson = Regex.Matches(html, @"""legalPerson"":""([\s\S]*?)""");
                    MatchCollection regCap = Regex.Matches(html, @"""regCap"":""([\s\S]*?)""");
                    MatchCollection validityFrom = Regex.Matches(html, @"""validityFrom"":""([\s\S]*?)""");
                    MatchCollection domicile = Regex.Matches(html, @"""domicile"":""([\s\S]*?)""");
                    MatchCollection scope = Regex.Matches(html, @"""scope"":""([\s\S]*?)""");

                    if (uids.Count == 0)
                    {
                        Thread.Sleep(3000);
                        if (page > 2)
                        {
                            page = page - 1;
                        }
                        continue;
                    }
                    for (int i = 0; i < uids.Count; i++)
                    {
                        try
                        {
                            Thread.Sleep(600);
                            label7.Text = DateTime.Now.ToLongTimeString() + "正在提取：" + entName[i].Groups[1].Value;
                            string tel = gettel(uids[i].Groups[1].Value);

                            if (tel.Trim() == "")
                            {
                                label7.Text = DateTime.Now.ToLongTimeString() + "正在提取：" + entName[i].Groups[1].Value + "--无手机号跳过";
                                continue;
                            }


                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString());
                            lv1.SubItems.Add(entName[i].Groups[1].Value);
                            lv1.SubItems.Add(legalPerson[i].Groups[1].Value);
                            lv1.SubItems.Add(regCap[i].Groups[1].Value);
                            lv1.SubItems.Add(validityFrom[i].Groups[1].Value);
                            lv1.SubItems.Add(domicile[i].Groups[1].Value);
                            lv1.SubItems.Add(scope[i].Groups[1].Value);

                            if(jihuo==false)
                            {
                                if(tel.Length>4)
                                {
                                    tel = tel.Substring(0,4)+"*******";
                                }
                               
                            }
                           
                            lv1.SubItems.Add(tel);

                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)
                                return;
                        }
                        catch (Exception  ex)
                        {
                            label7.Text = ex.ToString();
                            continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
              
                label7.Text=ex.ToString();
            }
            label7.Text = "采集完成";
        }

        
        private void 工商企业采集_Load(object sender, EventArgs e)
        {
            

            comboBox1.Items.Add("不限");
            comboBox2.Items.Add("不限");
            comboBox3.Items.Add("不限");
            comboBox4.Items.Add("不限");
            comboBox5.Items.Add("不限");
            comboBox6.Items.Add("不限");
            comboBox7.Items.Add("不限");
            comboBox8.Items.Add("不限");
            getcates("qylx", comboBox1);
            getcates("zczb", comboBox2);
            getcates("qyzt", comboBox3);
            getcates("clnx", comboBox4);
            getcates("hy",comboBox5);
            //getcates("province", comboBox6);


            getprovincefromJson();
        }

        Dictionary<string, string> jsondicp = new Dictionary<string, string>();
        Dictionary<string, string> jsondicc = new Dictionary<string, string>();
        Dictionary<string, string> jsondica = new Dictionary<string, string>();
        public void getprovincefromJson()
        {
            StreamReader sr = new StreamReader(path + "data\\city.json", method.EncodingType.GetTxtType(path + "data\\city.json"));
            //一次性读取完 
            string jsonText = sr.ReadToEnd();
            jsonText = method.Unicode2String(jsonText);
            MatchCollection provinces = Regex.Matches(jsonText, @"""value"":""([\s\S]*?)"",""text"":""([\s\S]*?)""");
            for (int i = 0; i < provinces.Count; i++)
            {
             if(!jsondicp.ContainsKey(provinces[i].Groups[2].Value))
                {
                    jsondicp.Add(provinces[i].Groups[2].Value, provinces[i].Groups[1].Value);
                    if (provinces[i].Groups[1].Value.Substring(provinces[i].Groups[1].Value.Length-4,4)=="0000")
                    {
                        comboBox6.Items.Add(provinces[i].Groups[2].Value);
                        
                    }
                    
                  
                }
                
            }
            sr.Close();
        }


        public void getcityfromJson()
        {
            comboBox7.Items.Clear();
            comboBox7.Items.Add("不限");
            if (comboBox6.Text == "不限")
            {
                return;
            }
            StreamReader sr = new StreamReader(path + "data\\city.json", method.EncodingType.GetTxtType(path + "data\\city.json"));
            //一次性读取完 
            string jsonText = sr.ReadToEnd();
            jsonText = method.Unicode2String(jsonText);
            MatchCollection provinces = Regex.Matches(jsonText, @"""value"":""([\s\S]*?)"",""text"":""([\s\S]*?)""");
            for (int i = 0; i < provinces.Count; i++)
            {
                if (!jsondicc.ContainsKey(provinces[i].Groups[2].Value))
                {
                    
                    if (provinces[i].Groups[1].Value.Substring(0, 2) == jsondicp[comboBox6.Text].Substring(0, 2)  && provinces[i].Groups[1].Value != jsondicp[comboBox6.Text])//添加下级不添加本身
                    {
                        jsondicc.Add(provinces[i].Groups[2].Value, provinces[i].Groups[1].Value);
                        if (provinces[i].Groups[1].Value.Substring(provinces[i].Groups[1].Value.Length - 2, 2) == "00")
                        {
                            comboBox7.Items.Add(provinces[i].Groups[2].Value);
                        }
                 
                       

                    }
          

                }
   

            }
            sr.Close();

        }

        public void getareafromJson()
        {

            comboBox8.Items.Clear();
            comboBox8.Items.Add("不限");
            if (comboBox7.Text == "不限")
            {
                return;
            }
            StreamReader sr = new StreamReader(path + "data\\city.json", method.EncodingType.GetTxtType(path + "data\\city.json"));
            //一次性读取完 
            string jsonText = sr.ReadToEnd();
            jsonText = method.Unicode2String(jsonText);
            MatchCollection provinces = Regex.Matches(jsonText, @"""value"":""([\s\S]*?)"",""text"":""([\s\S]*?)""");
            for (int i = 0; i < provinces.Count; i++)
            {
                if (!jsondica.ContainsKey(provinces[i].Groups[2].Value))
                {

                    if (provinces[i].Groups[1].Value.Substring(0, 4) == jsondicc[comboBox7.Text].Substring(0, 4) && provinces[i].Groups[1].Value!= jsondicc[comboBox7.Text])
                    {
                        jsondica.Add(provinces[i].Groups[2].Value, provinces[i].Groups[1].Value);
                        comboBox8.Items.Add(provinces[i].Groups[2].Value);



                    }


                }


            }
            sr.Close();

        }

        bool zanting = true;
        bool status = true;
        Thread thread;
        private void button7_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"PlSUh"))
            {

                return;
            }



            #endregion
           
            // jiance();
            jihuoma();
            if (textBox2.Text == "")
            {
                MessageBox.Show("请输入关键字");
                return;
            }
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }


        string path = AppDomain.CurrentDomain.BaseDirectory;
        Dictionary<string, string> dic = new Dictionary<string, string>();

      
        public void getcates(string txtname,ComboBox cob)
        {
            StreamReader sr = new StreamReader(path+"data\\"+ txtname + ".txt", method.EncodingType.GetTxtType(path + "data\\"+ txtname + ".txt"));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            MatchCollection cates = Regex.Matches(texts, @"data-log-title=""([\s\S]*?)""([\s\S]*?)>([\s\S]*?)<");
            for (int i = 0; i < cates.Count; i++)
            {
                cob.Items.Add(cates[i].Groups[3].Value.Trim());
                dic.Add(cates[i].Groups[3].Value.Trim(), cates[i].Groups[1].Value.Trim());
            }
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存

        }





       

        public void creatVcf()

        {

            string text = method.GetTimeStamp() + ".vcf";
            if (File.Exists(text))
            {
                if (MessageBox.Show("“" + text + "”已经存在，是否删除它？", "确认", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;
                }
                File.Delete(text);
            }
            UTF8Encoding encoding = new UTF8Encoding(false);
            StreamWriter streamWriter = new StreamWriter(text, false, encoding);
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                string name = listView1.Items[i].SubItems[1].Text.Trim();
                string tel = listView1.Items[i].SubItems[7].Text.Trim();
                if (name != "" && tel != "")
                {
                    streamWriter.WriteLine("BEGIN:VCARD");
                    streamWriter.WriteLine("VERSION:3.0");

                    streamWriter.WriteLine("N;CHARSET=UTF-8:" + name);
                    streamWriter.WriteLine("FN;CHARSET=UTF-8:" + name);

                    streamWriter.WriteLine("TEL;TYPE=CELL:" + tel);



                    streamWriter.WriteLine("END:VCARD");

                }
            }
            streamWriter.Flush();
            streamWriter.Close();
            MessageBox.Show("生成成功！文件名是：" + text);



        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                Thread thread= new Thread(creatVcf);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
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

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            getcityfromJson();
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            getareafromJson();
        }


        bool jihuo = true;

        public void jiance()
        {
            if (ExistINIFile())
            {
                string key = IniReadValue("values", "key");
                string secret = IniReadValue("values", "secret");
                string[] value = secret.Split(new string[] { "asd147" }, StringSplitOptions.None);


                if (Convert.ToInt32(value[1]) < Convert.ToInt32(method.GetTimeStamp()))
                {
                    MessageBox.Show("激活已过期");
                    string str = Interaction.InputBox("您的机器码如下，请复制机器码提供到后台，输入激活码然后激活！", "激活软件", method.GetMD5(method.GetMacAddress()), -1, -1);
                    string[] text = str.Split(new string[] { "asd" }, StringSplitOptions.None);

                    if (text[0] == method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian"))
                    {
                        IniWriteValue("values", "key", method.GetMD5(method.GetMacAddress()));
                        IniWriteValue("values", "secret", str);
                        MessageBox.Show("激活成功");


                    }
                    else
                    {
                        MessageBox.Show("激活码错误");
                        jihuo = false;
                       // System.Diagnostics.Process.GetCurrentProcess().Kill();
                        return;
                    }

                }


                else if (value[0] != method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian") || key != method.GetMD5(method.GetMacAddress()))
                {

                    string str = Interaction.InputBox("您的机器码如下，请复制机器码提供到后台，输入激活码然后激活！", "激活软件", method.GetMD5(method.GetMacAddress()), -1, -1);
                    string[] text = str.Split(new string[] { "asd147" }, StringSplitOptions.None);

                    if (text[0] == method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian"))
                    {
                        IniWriteValue("values", "key", method.GetMD5(method.GetMacAddress()));
                        IniWriteValue("values", "secret", str);
                        MessageBox.Show("激活成功");


                    }
                    else
                    {
                        MessageBox.Show("激活码错误");
                        jihuo = false;
                        //System.Diagnostics.Process.GetCurrentProcess().Kill();
                        return;
                    }
                }


            }
            else
            {

                string str = Interaction.InputBox("您的机器码如下，请复制机器码提供到后台，输入激活码然后激活！", "激活软件", method.GetMD5(method.GetMacAddress()), -1, -1);
                string[] text = str.Split(new string[] { "asd147" }, StringSplitOptions.None);
                if (text[0] == method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian"))
                {
                    IniWriteValue("values", "key", method.GetMD5(method.GetMacAddress()));
                    IniWriteValue("values", "secret", str);
                    MessageBox.Show("激活成功");


                }
                else
                {
                   
                    MessageBox.Show("激活码错误");
                    jihuo = false;
                    // System.Diagnostics.Process.GetCurrentProcess().Kill();
                    return;
                }
            }

        }

        #region 激活码

        public void jihuoma()
        {
            try
            {
                string macmd5 = method.GetMD5(method.GetMacAddress());
                long expiretime = Convert.ToInt64(method.GetTimeStamp()) + 365 * 24 * 3600;
                if (ExistINIFile())
                {
                    string key = IniReadValue("values", "key");
                    string[] value = key.Split(new string[] { "asd147" }, StringSplitOptions.None);


                    if (Convert.ToInt32(value[1]) < Convert.ToInt32(method.GetTimeStamp()))
                    {
                        MessageBox.Show("激活已过期");
                        string str = Interaction.InputBox("请购买激活码,使用正式版软件！", "激活软件", "", -1, -1);


                        if (str.Length > 40)
                        {
                            str = str.Remove(0, 10);
                            str = str.Remove(str.Length - 10, 10);

                            str = method.Base64Decode(Encoding.Default, str);
                            string index = str.Remove(str.Length - 16, 16);
                            string time = str.Substring(str.Length - 10, 10);
                            if (Convert.ToInt64(method.GetTimeStamp()) - Convert.ToInt64(time) < 99999999)  //200秒内有效
                            {
                                if (index == "er" || index=="san")//美团一年
                                {

                                    IniWriteValue("values", "key", macmd5 + "asd147" + expiretime);

                                    MessageBox.Show("激活成功");
                                    return;
                                }
                            }
                            if (index == "si")//试用一天
                            {

                                IniWriteValue("values", "key", macmd5 + "asd147" + 86400);

                                MessageBox.Show("激活成功");
                                return;
                            }
                        }
                        MessageBox.Show("激活码错误");
                        jihuo = false; ;
                    }

                }
                else
                {
                    string str = Interaction.InputBox("请购买激活码,使用正式版软件！", "激活软件", "", -1, -1);

                    if (str.Length > 40)
                    {
                        str = str.Remove(0, 10);
                        str = str.Remove(str.Length - 10, 10);

                        str = method.Base64Decode(Encoding.Default, str);
                        string index = str.Remove(str.Length - 16, 16);
                        string time = str.Substring(str.Length - 10, 10);
                        if (Convert.ToInt64(method.GetTimeStamp()) - Convert.ToInt64(time) < 99999999)  //200秒内有效
                        {
                            if (index == "er" || index == "san")//美团一年
                            {
                                IniWriteValue("values", "key", macmd5 + "asd147" + expiretime);

                                MessageBox.Show("激活成功");
                                return;
                            }
                        }
                        if (index == "si")//试用一天
                        {

                            IniWriteValue("values", "key", macmd5 + "asd147" + 86400);

                            MessageBox.Show("激活成功");
                            return;
                        }
                    }
                    MessageBox.Show("激活码错误");
                    jihuo = false; ;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("激活码错误");
                jihuo = false; ;
            }


        }


        #endregion

    }
}
