using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

namespace zj图片保存
{
    public partial class zj图片保存 : Form
    {
       






        public zj图片保存()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void zj图片保存_Load(object sender, EventArgs e)
        {
         
            //webBrowser1.Navigate("https://puser.zjzwfw.gov.cn/sso/newusp.do?action=ssoLogin&servicecode=zjdsjgrbs&goto=https%3A%2F%2Fwww.zjzwfw.gov.cn%2Fucenter%2Ffront2020%2Fmain%2Fpersonal%2Findex.do");
            shoujiusername = textBox1.Text.Trim();
            shoujipassword = textBox2.Text.Trim();
            yzmusername = textBox3.Text.Trim();
            yzmpassword = textBox4.Text.Trim();
        }






        string cookie = "cna=nHxyG3AA8QwCAf////+GB2NJ; zjzwfwloginhx=13262305; ZJZWFWSESSIONID=49bb74ce-2fcd-4e18-bc3e-03fbb5548fd3; amlbcookie=05; PROXY_URL=\"https://esso.zjzwfw.gov.cn/opensso/oauth2c/OAuthProxy.jsp\"; ORIG_URL=\"https://esso.zjzwfw.gov.cn/opensso/UI/Login?goto=https://esso.zjzwfw.gov.cn/opensso/spsaehandler/metaAlias/sp?spappurl=\"; URL_FOR_REG=https%3A%2F%2Fesso.zjzwfw.gov.cn%2Fopensso%2FUI%2FLogin%3Fgoto%3Dhttps%3A%2F%2Fesso.zjzwfw.gov.cn%2Fopensso%2Fspsaehandler%2FmetaAlias%2Fsp%3Fspappurl%3D; iPlanetDirectoryPro=AQIC5wM2LY4SfcwYWpd-dnFapc9HJvrwtqwPn8U2bXi3EpA.*AAJTSQACMDIAAlNLABMxMzIzNDk0MTI3MTQwMzYyNjgwAAJTMQACMDU.*; REG_URL=https%3A%2F%2Fesso.zjzwfw.gov.cn%2Fopensso%2FUI%2FLogin%3Fgoto%3Dhttps%3A%2F%2Fesso.zjzwfw.gov.cn%2Fopensso%2Fspsaehandler%2FmetaAlias%2Fsp%3Fspappurl%3Dhttps%3A%2F%2Fuuser.zjzwfw.gov.cn%2Fexternal%2FdoTimeValeSSOLogin.do; JSESSIONID=5508CD89CDC0155E5C548AAC0DA94576; ssxmod_itna=iqGx97uDgDBDRDfxBcDeun+zGkF9vKwA0QLpQD/jADnqD=GFDK40EEBhK+E54o4=Bm34xDund+x4LW0WRRaNFQmoNtTDCPGnDBI25O4qYA8Dt4DTD34DYDiEKDLDmeD+INKDdjpk5NkuDAQDQ4GyDitDKdiDxG3D0+=dYAY9fkLqqQrDAuASj049D0tDIqGX+0W47qDBt=a49LtNxPGWb2j4deGuDG=KvLLex0pySMU2f6vxbghxIGvb+7PwjAhP30AeI0ANnhqq9xqYl0DdBDqQBr1DG8bQ4u4xD===; ssxmod_itna2=iqGx97uDgDBDRDfxBcDeun+zGkF9vKwA0QLPG9igBjDBLZx7p5+KOd4iHLxOtCCBOrFmGUQYdEKHDIrINd4vOWfd4n0OBfjvF2UWY9f+afpknRu/9ZQFX4oIILy6ehu11O+Q2fvHFFnDGLPxGBrns7rn4=xB1hBRQdSANDn4s9rmph2u4RGos2xWAvx4Ay7cQFfYAoSYF7SILBl+jR95A6+LiyRy4UYLsq0pjq3lOTbuDq4er=qa6btubkPbkf4ezNipE/iKNNxzNPO3iEK5FuxWa9=wu26IAz+uBE/oTmdcrE9EH6doCGtU7x02E81KC1mP1edapezfVGo9c0T7KI0fpjAz26dR2cj2GZD/jI6Gonc05mmtWrRDrl=5Qapi+o0m0AniXmTOjYTSq8/eo4mzKjkG5dETLGjIiQFhweUI1FRGgw7/mwtwESWjKAz256G4DQFT8bxgxFBiqQ+x95O=LPYb8HM/DxxD08DiQ4YD; SAEORGURL=\"\"; SAEORGPOSTPARAMS=\"\"; SESSION=174b7329-fdbd-4cbe-b439-1b66e5a4ed37";


        string shoujiusername = "";
        string shoujipassword = "";

        string yzmusername = "";
        string yzmpassword = "";


        #region  网络图片转Bitmap
        public Bitmap UrlToBitmap(string url)
        {
            WebClient mywebclient = new WebClient();
            mywebclient.Headers.Add("Cookie", cookie);
            byte[] Bytes = mywebclient.DownloadData(url);
            using (MemoryStream ms = new MemoryStream(Bytes))
            {
                Image outputImg = Image.FromStream(ms);

                Bitmap map = new Bitmap(outputImg);
                return map;
            }
        }
        #endregion

        #region POST请求

        public string PostUrl(string url, string postData)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.Headers.Add("Cookie", cookie);
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Referer = "http://www.jszwfw.gov.cn/jsjis/h5/jszfjis/view/perregister.html";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                //MessageBox.Show(ex.ToString());
                return "";
            }


        }

        #endregion

        #region POST请求

        public string PostUrl2(string url, string postData,string appcookie)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/json";
                request.ContentLength = postData.Length;
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.Headers.Add("Cookie", appcookie);
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Referer = "http://www.jszwfw.gov.cn/jsjis/h5/jszfjis/view/perregister.html";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                //MessageBox.Show(ex.ToString());
                return "";
            }


        }

        #endregion

        #region 图片转base64
        public static string ImgToBase64String(Bitmap bmp)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch
            {
                return null;
            }
        }

        #endregion

        public string shibie()
        {
            try
            {
                Bitmap image = UrlToBitmap("https://uuser.zjzwfw.gov.cn/captcha/doReadKaptcha.do");



                string param = "{\"username\":\"" + yzmusername + "\",\"password\":\"" + yzmpassword + "\",\"image\":\"" + ImgToBase64String(image) + "\"}";

                string PostResult = PostUrl("http://api.ttshitu.com/base64", param);

                Match result = Regex.Match(PostResult, @"result"":""([\s\S]*?)""");
                if (result.Groups[1].Value != "")
                {

                    return result.Groups[1].Value;
                }
                else
                {

                    logtxtBox.Text += "图片验证码错误:" + PostResult + "\r\n";
                    // status = false;
                    return "";
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                return "";
            }
        }


        public string token = "";

        public void gettoken()
        {
            string url = "http://api.lx967.com:9090/sms/api/login?username=" + textBox1.Text.Trim() + "&password=" + textBox2.Text.Trim();
            string html = method.GetUrl(url, "utf-8");
            token = Regex.Match(html, @"""token"":""([\s\S]*?)""").Groups[1].Value;
            //MessageBox.Show(html);
        }
        /// <summary>
        /// 获取手机号
        /// </summary>
        /// <returns></returns>
        public string getmobile()
        {
            //167号段
            string url = "http://api.lx967.com:9090/sms/api/getPhone?token=" + token + "&sid=17517&haoduan=";
            string html = method.GetUrl(url, "utf-8");

            string mobileNo = Regex.Match(html, @"phone"":""([\s\S]*?)""").Groups[1].Value;
            string code = Regex.Match(html, @"code"":([\s\S]*?),").Groups[1].Value;
            if (code == "0")
            {
                logtxtBox.Text += DateTime.Now.ToLongTimeString() + "获取手机号成功" +mobileNo+ "\r\n";
                return mobileNo + "&" + code;

            }
            else
            {
                logtxtBox.Text += DateTime.Now.ToLongTimeString() + "获取手机号失败" + html + "\r\n";
                return html;
            }
        }

        int dengdaiduanxinmaseconds = 0;
        /// <summary>
        /// 获取手机短信
        /// </summary>
        /// <returns></returns>
        public string getduanxinma(string mobile)
        {
            Thread.Sleep(5000);
            dengdaiduanxinmaseconds = dengdaiduanxinmaseconds + 5;
            string url = "http://api.lx967.com:9090/sms/api/getMessage?token=" + token + "&sid=17517&phone=" + mobile;
            string html = method.GetUrl(url, "utf-8");
            //MessageBox.Show(html);
            string code = Regex.Match(html, @"\d{6}").Groups[0].Value;
            if (code != "")
            {

                logtxtBox.Text = DateTime.Now.ToLongTimeString() + "获取手机短信验证码码成功" + "\r\n" + html;
                return code;

            }
            else
            {
                logtxtBox.Text = DateTime.Now.ToLongTimeString() + "正在获取手机短信码,还未收到,已等待" + dengdaiduanxinmaseconds + "\r\n" + html;
                return "";
            }

        }

        string fasongmsg = "";

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="imgcode"></param>
        /// <param name="card"></param>
        public bool sendmobile(string mobile, string imgcode)
        {

            string url = "https://uuser.zjzwfw.gov.cn/uuuser/getVerifyCodeByPhone.do";
            string postdata = "phone=" + mobile + "&imgCode=" + imgcode + "&codeType=5";
           // MessageBox.Show(postdata);
            string html = PostUrl(url, postdata);
           // MessageBox.Show(html);
            if (html.Contains("true"))
            {
                logtxtBox.Text += "发送手机短信验证码成功" + "\r\n";
                return true;
            }
            else
            {
               // updatecookie();
                logtxtBox.Text += "发送手机短信验证码失败" + html + "\r\n";
                fasongmsg = html;
                return false;
            }

        }








        bool status = true;


        Thread thread;

        #region 主程序
        public void run()
        {
            
            DataTable dt = method.ExcelToDataTable(textBox5.Text, true);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {


                    string gregegedrgerheh = gdsgdgdgdgdstgfeewrwerw3r23r32rvxsvdsv.rgebgdgdvsdfsdvsdfsdvdsbgdsrt435b515sdfsdf("1", "112233");
                    string sign = gregegedrgerheh.Split(new string[] { "," }, StringSplitOptions.None)[0];
                    string zj_ggsjpt_sign = gregegedrgerheh.Split(new string[] { "," }, StringSplitOptions.None)[1];
                    string expiretime = gregegedrgerheh.Split(new string[] { "," }, StringSplitOptions.None)[2];
                   
                    if (DateTime.Now > Convert.ToDateTime(expiretime))
                    {
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                        return;
                    }


                    if (status == false)
                    {
                        return;
                    }

                    DataRow dr = dt.Rows[i];

                    string name = System.Web.HttpUtility.UrlEncode(dr[0].ToString());
                    string card = dr[1].ToString();


                    string mobilemid = getmobile();
                    string[] text = mobilemid.Split(new string[] { "&" }, StringSplitOptions.None);
                    if (text.Length < 1)
                    {
                        logtxtBox.Text = "获取手机号失败";
                        return;
                    }


                    string mobile = text[0];


                    bool fasongstatus = sendmobile(mobile, shibie());
                    while (!fasongstatus)
                    {
                       

                        Thread.Sleep(5000);
                        if (status == false)
                        {
                            return;
                        }

                        if (fasongmsg.Contains("频繁"))
                        {
                            logtxtBox.Text += "触发发送验证码频繁正在等待120秒.." + "\r\n";
                            Thread.Sleep(120000);
                        }
                       
                        fasongstatus = sendmobile(mobile, shibie());
                       
                    }

                  
                    string mobilecode = getduanxinma(mobile);
                    while (true)
                    {
                        if (status == false)
                        {
                            return;
                        }
                        mobilecode = getduanxinma(mobile);
                        if (dengdaiduanxinmaseconds == Convert.ToInt32(textBox6.Text))
                        {
                            dengdaiduanxinmaseconds = 0;
                            mobile = getmobile();

                          
                            sendmobile(mobile, shibie());
                        }
                        if (mobilecode != "")
                        {
                            dengdaiduanxinmaseconds = 0;
                            break;
                        }



                    }

                    string url = "https://uuser.zjzwfw.gov.cn/uuuser/doModAgent.do";
                    string postdata = "uuUser.id=13262305&codeType=5&uuUser.agentName=" + name + "&uuUser.idType=111&uuUser.idCard=" + card + "&phone=" + mobile + "&imgCode=nnko&verifycode=" + mobilecode;
                    string html = PostUrl(url, postdata);

                    //MessageBox.Show(html);

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add(card);
                    lv1.SubItems.Add(dr[0].ToString());
                    lv1.SubItems.Add(mobile);
                    lv1.SubItems.Add(html);
                    logtxtBox.Text += "成功后等待.." + textBox7.Text + "秒\r\n";
                    Thread.Sleep(Convert.ToInt32(textBox7.Text)*1000);
                  
                }
                catch (Exception ex)
                {
                  
                    logtxtBox.Text = ex.ToString();
                    continue;
                }
            }




        }


        #endregion


        #region GET请求获取Set-cookie
        public  string getSetCookie(string url)
        {

            //可用于网站第一次验证ocokie，不需要用webbrowser获取参考项目【邮箱地址确认】
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);  //创建一个链接
            request.Timeout = 10000;
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
            request.AllowAutoRedirect = false;
            request.Headers.Add("Cookie", cookiewithnotsession);
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

            string content = response.GetResponseHeader("Set-Cookie"); ;
            return content;

          
        }
        #endregion


        string cookiewithnotsession = "cna=nHxyG3AA8QwCAf////+GB2NJ; zjzwfwloginhx=13262305; ZJZWFWSESSIONID=cf9a722f-400c-408b-af88-4f18ea373e92;  ssxmod_itna=QqAhBKY54moh8DzrD2eEDk0eDQqtxDt1hqrQIWDlroxA5D8D6DQeGTrR+CvCQNC41iPiTV00hphm5Tb220ml7ex0aDbqGkqA/Q4GGjxBYDQxAYDGDDPkDj4ibDYSZHDjG96CSAPqAQDKx0kDY5Dwc5sDiPD7hKPCDe821cGZaYDn=iCWix8D75Dux0HNY35AxDCwF35yPvIC4i3fYFGx40OD0K+XpahDBR8g=yH2z4xf0DxrQaYrlDdqirYdB2KrQi4WYRx7e45Fe4q3GxP57hPlmtDi2DafdD==; ssxmod_itna2=QqAhBKY54moh8DzrD2eEDk0eDQqtxDt1hqrQqD6pSC0D0y2xx03WQhCn607NnD8xx6qX0e3Yhe9Kl+RAGWKQ0F2Px1OwuLiWF4lrt8N0BWFW3cAWQqbGonp2OpGkf8ldKfLsEqBGqjEQV7GhMldi/YnK3zxYCF+4EfhhO8CrXSdAsABx5EYox1ldEUqY3lLWgzQatdbYftPwfbxG294GcDiQteD=;";

        public void updatecookie()
        {

            try
            {
                string session = getSetCookie("https://uuser.zjzwfw.gov.cn/captcha/doReadKaptcha.do");
                if (session != "")
                {
                    logtxtBox.Text += DateTime.Now.ToString() + "cookie更新成功：" + "\r\n";
                    cookie = cookiewithnotsession + session;

                   // logtxtBox.Text = cookie;
                }
                else
                {
                    logtxtBox.Text = DateTime.Now.ToString() + "cookie更新失败";
                }

            }
            catch (Exception ex)
            {

                logtxtBox.Text = (ex.Message);
            }
        }

        


        private void button1_Click(object sender, EventArgs e)
        {
            updatecookie();
           // appcookie = textBox10.Text.Trim();

            


            

            gettoken();

            if (textBox5.Text == "")
            {
                MessageBox.Show("请先导入数据表格");
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

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            // openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox5.Text = openFileDialog1.FileName;



            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            status = false;

        }









        #region  APP端函数身份卡片下载

        string appcookie = "SERVERID=1e3429268b3cb0fcfc0f15fef200124b|1662801284|1662801277; cna=f0OjGxXxBVMCAXrAD362MAia; _bl_uid=bnlyh7qnvqXo1Iyaq55dhXbmp1g3; clientUserType=legal; C_zj_accountType=legal; C_zj_gsid=12eb8b72b46a46c3a7b9a3dcf1c24d25-gsid-legal; C_zj_platform=h5";
        //app端聚合调用
        public void run_app()
        {
            string card = "123";
            string seqno = getsequenceNo();
           if(seqno!="")
            {
               string name=  getinfo(seqno);
                if(name!="")
                {
                    sendnfo(seqno);
                    getpic(seqno,card);
                }
            }
        }







        /// <summary>
        /// 获取sequenceNo
        /// </summary>
        /// <returns></returns>
        public string getsequenceNo()
        {
            string url = "https://recept.zjzwfw.gov.cn/matter/reception?matterType=powerDirectory&matterId=101101907&syncUserType=true&endpoint=app";
            string html = method.GetUrlWithCookie(url, appcookie, "utf-8");
             string sequenceNo = Regex.Match(html, @"""sequenceNo"":""([\s\S]*?)""").Groups[1].Value;
            if(sequenceNo!="")
            {
                logtxtBox.Text += "获取seqNo成功.." + "\r\n";
                return sequenceNo;
            }
            else
            {
                logtxtBox.Text += "获取seqNo失败.." + "\r\n";
                return "";
            }
            
        }

        /// <summary>
        /// 获取个人内容详情
        /// </summary>
        /// <returns></returns>
        public string getinfo(string seqno)
        {
           // string url = "https://recept.zjzwfw.gov.cn/matter/reception?matterType=powerDirectory&matterId=101101907&syncUserType=true&endpoint=app&sequenceNo="+seqno+"&config=%7B%22viewId%22%3A%22Material%22%7D&viewId=Material&useDetailNew=true";
            string url = "https://recept.zjzwfw.gov.cn/form/ultronage?matterType=powerDirectory&matterId=101101907&syncUserType=true&endpoint=app&sequenceNo="+seqno+"&config=%7B%22viewId%22%3A%22Form%22%7D&viewId=Form&useDetailNew=true";
            string html = method.GetUrlWithCookie(url, appcookie, "utf-8");
            string name = Regex.Match(html, @"""userName\\"":\\""([\s\S]*?)\\""").Groups[1].Value;
            if (name != "")
            {
                logtxtBox.Text += "获取个人内容成功.." + "\r\n";
                return name;
            }
            else
            {
                logtxtBox.Text += "获取个人内容失败.." + "\r\n";
                return "";
            }

        }


        /// <summary>
        /// 发送个人内容
        /// </summary>
        /// <returns></returns>
        public string sendnfo(string seqno)
        {
          
            string url = "https://recept.zjzwfw.gov.cn/api/reception/execute";

            string sequenceNo = Regex.Match(textBox9.Text.Trim(), @"""sequenceNo"":""([\s\S]*?)""").Groups[1].Value;
            string postdata = textBox9.Text.Trim().Replace(sequenceNo, seqno);
          
            string html = PostUrl2(url, postdata, appcookie);
            
            string sequence = Regex.Match(html, @"""sequenceNo"":""([\s\S]*?)""").Groups[1].Value;
            if (sequence != "")
            {
                logtxtBox.Text += "发送个人内容成功.." + "\r\n";
                return sequence;
            }
            else
            {
                logtxtBox.Text += "发送个人内容失败.." + "\r\n";
                return "";
            }

        }

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <returns></returns>
        public void getpic(string seqno, string card)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory+"\\image\\";


            //https://recept.zjzwfw.gov.cn/matter/reception?t=1664094582404&t=1664094582404&config=%7B%22viewId%22%3A%22Material%22%7D&endpoint=online&matterId=101100129&sequenceNo=b3269a1a272c4dc6990b78a972458a1c&useDetailNew=true&viewId=Material
            string url = "https://recept.zjzwfw.gov.cn/matter/reception?matterType=powerDirectory&matterId=101101907&syncUserType=true&endpoint=app&sequenceNo="+seqno+"&config=%7B%22viewId%22%3A%22Material%22%7D&viewId=Material&useDetailNew=true";

           
            string html = method.GetUrlWithCookie(url, appcookie, "utf-8");
            MatchCollection picurls = Regex.Matches(html, @"""attaUrl"":""([\s\S]*?)""");
           if(picurls.Count>2)
            {
                for (int i = 0; i < 2; i++)
                {
                    logtxtBox.Text += "下载图片" + (i + 1) + "成功.." + "\r\n";
                    method.downloadFile("https:"+picurls[i].Groups[1].Value,path,card+"_"+ (i + 1) + ".jpg","");
                }
            }
           else
            {
                logtxtBox.Text += "获取图片失败.." + "\r\n";
            }

        }

        #endregion

        private void logtxtBox_TextChanged(object sender, EventArgs e)
        {
            if(logtxtBox.Text.Length>3000)
            {
                logtxtBox.Text = "";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            appcookie = textBox10.Text.Trim();
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run_app);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            listView1.Items.Clear();    
        }

        private void zj图片保存_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }

      

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            updatecookie();
        }
    }
}
