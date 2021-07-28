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

namespace 主程序202104
{
    public partial class 模型图片替换 : Form
    {
        public 模型图片替换()
        {
            InitializeComponent();
        }

        static string ip = "";


        public  void getip()
        {
            ip = GetUrl2(textBox3.Text,"utf-8").Trim();
            label3.Text = ip;
        }


        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public  string GetUrl2(string Url, string charset)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "www_search_history=%5B%221233566%22%2C%221233757%22%5D; gr_user_id=981fc49f-afca-4720-9e70-ab4abf91e850; user_behavior_id=BDBB5EC62B26249CE3B950F8B34EC58B0; UM_distinctid=17a3cf5c7f2e-0c2200a6cc2a88-d7e163f-1fa400-17a3cf5c7f3c83; PHPSESSID=mh5tikcofpsmh32mm3le76n2q5; openWinFocus=showed; sotuTip=showed; Hm_lvt_bh_ud=F311CBE6F988857FD958EEE8E552151B; login_token=b60c0a80015d4bb25b5d11c6b0452167; login_sign=402d19e50fff44c827a4f3b608bd5812%3A4Yfba9aJrpnidZe-v3rh_7PcbgA%3D%3AeyJpcCI6IjE4My4xMzEuMjM4LjE3IiwidGltZSI6MTYyNDUyMDQ3MCwiYnJvd3NlciI6IkNocm9tZSIsImRlYWRsaW5lIjoxNjI0NTIwNDcwfQ%3D%3D; get_cookie_time=1624563855; www_search_history=%5B%221233566%22%5D; Hm_lvt_de9a43418888d1e2c4d93d2fc3aef899=1624520378,1624521598; key_name=; 95ea25105d564481_gr_session_id=7cba25dc-ed05-4754-a3fb-c677f69df56b; 95ea25105d564481_gr_session_id_7cba25dc-ed05-4754-a3fb-c677f69df56b=true; CNZZDATA1263507971=2095459576-1624517148-https%253A%252F%252Fwww.baidu.com%252F%7C1624522548; userBindPhone=; last_login_type=2; userCookieData=%7B%22is_star%22%3A0%2C%22user_id%22%3A176059003%2C%22user_name%22%3A%22%5Cu601d%5Cu5fc6%5Cu8f6f%5Cu4ef6%22%2C%22is_vip%22%3A0%2C%22vip_status%22%3A0%2C%22user_img%22%3A%22https%3A%5C%2F%5C%2Fthirdwx.qlogo.cn%5C%2Fmmopen%5C%2FM96D3RLIE0l719Mrm4Hysaibskf7NcULwicTpQPpSLlLJagia7QibbCnrDK7epUdnlos2FI5JSZMOKD0PMXiadogL4SoBZbM5m2uh%5C%2F132%22%2C%22star_level%22%3A0%2C%22user_lb%22%3A0%2C%22xing_dian%22%3A0%2C%22zeng_dian%22%3A0%2C%22xuan_dian%22%3A0%2C%22cash_lb%22%3A0%2C%22vip_icon%22%3A%22%5C%2Fuser%5C%2Fimages%5C%2Fvipicon%5C%2Fvip0.png%22%2C%22vip_icon_expire%22%3A%22%5C%2Fuser%5C%2Fimages%5C%2Fvipicon%5C%2Fvip0.png%22%2C%22vip_next_vip_icon%22%3A%22%5C%2Fuser%5C%2Fimages%5C%2Fvipicon%5C%2Fvip01.png%22%2C%22vip_next_vip_icon_expire%22%3A%22%5C%2Fuser%5C%2Fimages%5C%2Fvipicon%5C%2Fvip11.png%22%2C%22star_icon%22%3A%22%5C%2Fuser%5C%2Fimages%5C%2Fstaricon%5C%2Fstar0.png%22%2C%22is_xuanran_vip%22%3A0%2C%22is_need_phone%22%3A0%2C%22is_vr_vip%22%3A0%2C%22is_soft_vip%22%3A0%2C%22is_sc_vip%22%3A0%2C%22all_vip_end_day%22%3A0%2C%22sc_end_day%22%3A0%2C%22is_one_end%22%3A0%2C%22vr_end_day%22%3A0%2C%22xr_end_day%22%3A0%2C%22soft_end_day%22%3A0%2C%22vip_rank%22%3A0%2C%22reg_time%22%3A1624525489%2C%22is_home_page%22%3A0%7D; last_user_name=%E6%80%9D%E5%BF%86%E8%BD%AF%E4%BB%B6; Hm_lpvt_de9a43418888d1e2c4d93d2fc3aef899=1624525492";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
            
                request.Referer = "https://www.justeasy.cn/model/getmodeldetails?id=546544";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);

                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
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
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public string GetUrl(string Url, string charset)
        {


            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "www_search_history=%5B%221233566%22%2C%221233757%22%5D; gr_user_id=981fc49f-afca-4720-9e70-ab4abf91e850; user_behavior_id=BDBB5EC62B26249CE3B950F8B34EC58B0; UM_distinctid=17a3cf5c7f2e-0c2200a6cc2a88-d7e163f-1fa400-17a3cf5c7f3c83; PHPSESSID=mh5tikcofpsmh32mm3le76n2q5; openWinFocus=showed; sotuTip=showed; Hm_lvt_bh_ud=F311CBE6F988857FD958EEE8E552151B; login_token=b60c0a80015d4bb25b5d11c6b0452167; login_sign=402d19e50fff44c827a4f3b608bd5812%3A4Yfba9aJrpnidZe-v3rh_7PcbgA%3D%3AeyJpcCI6IjE4My4xMzEuMjM4LjE3IiwidGltZSI6MTYyNDUyMDQ3MCwiYnJvd3NlciI6IkNocm9tZSIsImRlYWRsaW5lIjoxNjI0NTIwNDcwfQ%3D%3D; get_cookie_time=1624563855; www_search_history=%5B%221233566%22%5D; Hm_lvt_de9a43418888d1e2c4d93d2fc3aef899=1624520378,1624521598; key_name=; 95ea25105d564481_gr_session_id=7cba25dc-ed05-4754-a3fb-c677f69df56b; 95ea25105d564481_gr_session_id_7cba25dc-ed05-4754-a3fb-c677f69df56b=true; CNZZDATA1263507971=2095459576-1624517148-https%253A%252F%252Fwww.baidu.com%252F%7C1624522548; userBindPhone=; last_login_type=2; userCookieData=%7B%22is_star%22%3A0%2C%22user_id%22%3A176059003%2C%22user_name%22%3A%22%5Cu601d%5Cu5fc6%5Cu8f6f%5Cu4ef6%22%2C%22is_vip%22%3A0%2C%22vip_status%22%3A0%2C%22user_img%22%3A%22https%3A%5C%2F%5C%2Fthirdwx.qlogo.cn%5C%2Fmmopen%5C%2FM96D3RLIE0l719Mrm4Hysaibskf7NcULwicTpQPpSLlLJagia7QibbCnrDK7epUdnlos2FI5JSZMOKD0PMXiadogL4SoBZbM5m2uh%5C%2F132%22%2C%22star_level%22%3A0%2C%22user_lb%22%3A0%2C%22xing_dian%22%3A0%2C%22zeng_dian%22%3A0%2C%22xuan_dian%22%3A0%2C%22cash_lb%22%3A0%2C%22vip_icon%22%3A%22%5C%2Fuser%5C%2Fimages%5C%2Fvipicon%5C%2Fvip0.png%22%2C%22vip_icon_expire%22%3A%22%5C%2Fuser%5C%2Fimages%5C%2Fvipicon%5C%2Fvip0.png%22%2C%22vip_next_vip_icon%22%3A%22%5C%2Fuser%5C%2Fimages%5C%2Fvipicon%5C%2Fvip01.png%22%2C%22vip_next_vip_icon_expire%22%3A%22%5C%2Fuser%5C%2Fimages%5C%2Fvipicon%5C%2Fvip11.png%22%2C%22star_icon%22%3A%22%5C%2Fuser%5C%2Fimages%5C%2Fstaricon%5C%2Fstar0.png%22%2C%22is_xuanran_vip%22%3A0%2C%22is_need_phone%22%3A0%2C%22is_vr_vip%22%3A0%2C%22is_soft_vip%22%3A0%2C%22is_sc_vip%22%3A0%2C%22all_vip_end_day%22%3A0%2C%22sc_end_day%22%3A0%2C%22is_one_end%22%3A0%2C%22vr_end_day%22%3A0%2C%22xr_end_day%22%3A0%2C%22soft_end_day%22%3A0%2C%22vip_rank%22%3A0%2C%22reg_time%22%3A1624525489%2C%22is_home_page%22%3A0%7D; last_user_name=%E6%80%9D%E5%BF%86%E8%BD%AF%E4%BB%B6; Hm_lpvt_de9a43418888d1e2c4d93d2fc3aef899=1624525492";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                WebProxy proxy = new WebProxy(ip);
                request.Proxy = proxy;
                request.Referer = "https://www.justeasy.cn/model/getmodeldetails?id=546544";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                //request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/7.0.10(0x17000a21) NetType/4G Language/zh_CN";
                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
              
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                string content = reader.ReadToEnd();


                reader.Close();
                response.Close();
                return content;



            }
            catch (System.Exception ex)
            {
                Thread.Sleep(5000);
                getip();
                ex.ToString();

            }
            return "";
        }
        #endregion

        #region 下载文件新
        public void GetImage(string url, string path)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            req.ServicePoint.Expect100Continue = false;
            req.Method = "GET";
            req.KeepAlive = true;

            req.ContentType = "image/jpg";
            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();

            System.IO.Stream stream = null;

            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                Image.FromStream(stream).Save(path);
            }
            finally
            {
                // 释放资源
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }
        }

        #endregion

        #region  3d.znzmo.com（知末）

        public string run1(string id)
        {
            try
            {
                string url = "https://3d.znzmo.com/3dmoxing/" + id + ".html";
                string html = GetUrl(url, "utf-8");
               
                string picurl = Regex.Match(html, @"""listImageUrl"":""([\s\S]*?)""").Groups[1].Value;
                return picurl;

            }
            catch (Exception ex)
            { 
                textBox2.Text=ex.ToString();
                return "";
            }

        }

        #endregion

        #region  www.3d66.com（3D溜溜）

        public string run2(string id)
        {
            try
            {
                string url = "https://so.3d66.com/res/"+id+"_1_1.html";
                string html = GetUrl(url, "utf-8");
                string picurl = Regex.Match(html, @"<div class=""swiper-slide""> <a href=""([\s\S]*?)""").Groups[1].Value.Replace("full","324");
                return picurl;

            }
            catch (Exception ex)
            {
                ex.ToString();
                return "";
            }

        }

        #endregion

        #region  www.om.cn（欧模网）

        public string run3(string id)
        {
            try
            {
                string url = "https://www.om.cn/v55/model/image/search?group=model&page=1&num=1&mid=" + id;
                string html = GetUrl(url, "utf-8");
                string picurl = "https://down.om.cn/v3"+Regex.Match(html, @"cover"":""([\s\S]*?)""").Groups[1].Value;
                return picurl;

            }
            catch (Exception ex)
            {
                 ex.ToString();
                return "";
            }

        }

        #endregion

        #region  www.justeasy.cn（建易设计网）

        public string run4(string id)
        {
            try
            {
                string url = "https://www.justeasy.cn/3d/id-" + id + ".html";
                string html = GetUrl(url, "utf-8");
                string picurl = Regex.Match(html, @"<li data=""0"">([\s\S]*?)src=""([\s\S]*?)""").Groups[2].Value;
                return picurl;

            }
            catch (Exception ex)
            {
                textBox2.Text = ex.ToString();
                return "";
            }

        }

        #endregion



        public string getlarge(string id,int large)
        {

            try
            {
                string large1 = Regex.Match(GetUrl("https://3d.znzmo.com/3dmoxing/" + id + ".html", "utf-8"), @"""fileLength"":([\s\S]*?),").Groups[1].Value.Trim();

                string large2 = Regex.Match(GetUrl("https://www.3d66.com/reshtml/sketchup/" + id.Substring(0, 4) + "/" + id + ".html", "utf-8"), @"大小：</span> <span>([\s\S]*?)MB").Groups[1].Value.Trim();

                string large3 = Regex.Match(GetUrl("https://www.om.cn/models/detail/" + id + ".html", "utf-8"), @"</span> <span>([\s\S]*?)MB").Groups[1].Value.Trim();

                string large4 = Regex.Match(GetUrl("https://www.justeasy.cn/model/getmodeldetails?id=" + id, "utf-8"), @"""size"":""([\s\S]*?)MB").Groups[1].Value.Trim();

              //  MessageBox.Show(large1+"#"+ large2 + "#"+ large3 + "#"+ large4 + "#");
                if (large1 != "")
                {
                    if (Math.Abs(Convert.ToInt32(Convert.ToDouble(large1)) - large) <= 1)
                    {

                        return "1";
                    }
                }
                if (large2 != "")
                {
                    if (Math.Abs(Convert.ToInt32(Convert.ToDouble(large2)) - large) <= 1)
                    {

                        return "2";
                    }
                }
                if (large3 != "")
                {
                    if (Math.Abs(Convert.ToInt32(Convert.ToDouble(large3)) - large) <= 1)
                    {
                        return "3";
                    }
                }
                if (large4 != "")
                {
                    if (Math.Abs(Convert.ToInt32(Convert.ToDouble(large4)) - large) <= 1)
                    {
                        return "4";
                    }
                }

                if (large1 != "")
                {
                    return "1";
                }
                if (large2 != "")
                {
                    return "2";
                }
                if (large3 != "")
                {
                    return "3";
                }
                if (large4 != "")
                {
                    return "4";
                }
                return "未匹配到";
            }
            catch (Exception ex)
            {

                return "异常"; ;
            }
          
        }




        public string getimg(string zipfile)
        {
            string imgpath = "";
            for (int i = 0; i < lists.Count; i++)
            {
                string file = lists[i].ToString();

                FileInfo fi = new FileInfo(file);//---使用FileInfo类进行操作
                if (fi.Extension == ".jpg" || fi.Extension == ".png" || fi.Extension == ".jpeg")
                {
                    string imgid = Regex.Match(Path.GetFileNameWithoutExtension(file), @"\d{5,}").Groups[0].Value;
                    string zipid = Regex.Match(Path.GetFileNameWithoutExtension(zipfile), @"\d{5,}").Groups[0].Value;
                    if (imgid == zipid)
                    {
                        imgpath= file;
                        break;
                    }
                }
            }


            return imgpath;
        }

        public void readzip()
        {
            for (int i = 0; i < lists.Count; i++)
            {
                string file = lists[i].ToString();
                FileInfo fi = new FileInfo(file);//---使用FileInfo类进行操作

                if (fi.Extension == ".7z" || fi.Extension == ".zip" || fi.Extension == ".rar")
                {
                    int large = Convert.ToInt32(fi.Length /(1024*1024));
                    string zipid = Regex.Match(Path.GetFileNameWithoutExtension(file), @"\d{5,}").Groups[0].Value;
                    if (zipid != "")
                    {
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(file);
                        lv1.SubItems.Add(large.ToString());
                        lv1.SubItems.Add(getimg(file));
                        lv1.SubItems.Add(zipid);
                        lv1.SubItems.Add("");
                        lv1.SubItems.Add("");
                    }
                }
            }
        }


        public void run()
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                try
                {
                    string id = listView1.Items[i].SubItems[4].Text;
                    string picpath = listView1.Items[i].SubItems[3].Text;
                    string zippath = listView1.Items[i].SubItems[1].Text;
                    string picurl = "";
                    string pingtainame = "";
                    if (radioButton1.Checked == true)
                    {
                        picurl = run1(id);
                        pingtainame = "知末";
                    }
                    if (radioButton2.Checked == true)
                    {
                        picurl = run2(id);
                        pingtainame = "3D66";
                    }
                    if (radioButton3.Checked == true)
                    {
                        picurl = run3(id);
                        pingtainame = "欧模";
                    }
                    if (radioButton4.Checked == true)
                    {
                        picurl = run4(id);
                        pingtainame = "建易";
                    }

                    if (picurl != "")
                    {
                        if (picpath != "")
                        {
                            GetImage(picurl, picpath);
                        }
                        else
                        {
                            GetImage(picurl, zippath.Replace(".zip", ".jpg").Replace(".7z", ".jpg").Replace(".rar", ".jpg"));
                        }
                        listView1.Items[i].SubItems[5].Text = "替换成功";
                        listView1.Items[i].SubItems[6].Text = pingtainame;
                    }
                    else
                    {
                        listView1.Items[i].SubItems[5].Text = "图片地址为空";
                        listView1.Items[i].SubItems[6].Text = pingtainame;
                    }

                }
                catch (Exception ex)
                {

                   continue;
                }
               
               
               
            }

        }



        public void run2()
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                try
                {
                    string id = listView1.Items[i].SubItems[4].Text;
                    string picpath = listView1.Items[i].SubItems[3].Text;
                    string zippath = listView1.Items[i].SubItems[1].Text;
                    int large = Convert.ToInt32(listView1.Items[i].SubItems[2].Text);
                    string picurl = "";

                    string pingtai = getlarge(id, large);
                 
                    string pingtainame = "";
                    if (pingtai == "1")
                    {
                        picurl = run1(id);
                        pingtainame = "知末";
                    }
                    if (pingtai == "2")
                    {
                        picurl = run2(id);
                        pingtainame = "3D66";
                    }
                    if (pingtai == "3")
                    {
                        picurl = run3(id);
                        pingtainame = "欧模";
                    }
                    if (pingtai == "4")
                    {
                        picurl = run4(id);
                        pingtainame = "建易";
                    }
                  
                    if (picurl != "")
                    {

                        if (picpath != "")
                        {
                            GetImage(picurl, picpath);
                        }
                        else
                        {
                            GetImage(picurl, zippath.Replace(".zip", ".jpg").Replace(".7z", ".jpg").Replace(".rar", ".jpg"));
                        }


                        listView1.Items[i].SubItems[5].Text = "替换成功";
                        listView1.Items[i].SubItems[6].Text = pingtainame;
                    }
                    else
                    {
                        listView1.Items[i].SubItems[5].Text = "图片地址为空";
                        listView1.Items[i].SubItems[6].Text = pingtainame;
                    }
                }
                catch (Exception ex)
                {

                 ex.ToString();
                }

            }

        }

        private void 模型图片替换_Load(object sender, EventArgs e)
        {
            radioButton5.Checked = true;
        }


        Thread thread;
        Thread thread1;
        private void button1_Click(object sender, EventArgs e)
        {
            getip();


            if (radioButton5.Checked == true)
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(run2);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
            else
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(run);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
            
        }


        ArrayList lists = new ArrayList();

        private void button2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择所在文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = dialog.SelectedPath;
                string[] files = Directory.GetFiles(dialog.SelectedPath);
                foreach (string file in files)
                {
                    lists.Add(file);

                }
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
           
          
            if (textBox1.Text == "")
            {
                MessageBox.Show("请选择目录");
                return;
            }
            if (thread1 == null || !thread1.IsAlive)
            {
                thread1 = new Thread(readzip);
                thread1.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 模型图片替换_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            getip();
        }
    }
}
