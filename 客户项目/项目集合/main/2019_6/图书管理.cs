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

namespace main._2019_6
{
    public partial class 图书管理 : Form
    {
        public 图书管理()
        {
            InitializeComponent();
        }


        private void 图书管理_Load(object sender, EventArgs e)
        {
             this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        }
        public static string COOKIE = "BAIDUID=D5049657ADDEADD6C4D60D7802DC22B4:FG=1; PSTM=1561528830; BIDUPSID=76109653D77CDD2ACB1BF156ACA5E346; BDORZ=B490B5EBF6F3CD402E515D22BCDA1598; PANWEB=1; Hm_lvt_7a3960b6f067eb0085b7f96ff5e660b0=1561013992,1561447770,1562142535,1562142895; pan_login_way=1; BDUSS=mdWVE54dmVHbFRWcTM5VlNJV35oSU13TjdjQ2REUHRyRFhHS0ZBM3BGZVEtRU5kSVFBQUFBJCQAAAAAAAAAAAEAAAD0LrFyxMfE48TYtcTKx87SAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAJBrHF2QaxxdQ; STOKEN=0d994607324554cdf3bd84bdb2cce4ea68376ca41feed890d39423e94444f3bd; SCRC=8124da518718ac5acbbf73b4e1c4b87f; Hm_lpvt_7a3960b6f067eb0085b7f96ff5e660b0=1562201122; cflag=13%3A3; PANPSC=6744369120179718171%3AJfOZln5JN3fTqiNgSmuCE1cS2d9ns3O51IbqOmZzQP%2BuaBheoMl5yWT%2FMK3iZvOoBm6rqQ5QdYIJJKiYQ97r2Xt7IirdPrszMRmYrlFHgmqMjJFSZNE06ws09VennxmMwFxgyqx8xpLWHyYIIPcWScfcHruOK58T%2BnFPyQFrcwzoLm2lh875yTE9tZRkdTU29S30DBbLU%2FEXmNzb72JUKM33oYTWEKi85TRpgrZIkdiOKBFUoinHwimZHSg9lqILrnvLlvX43na%2FzVpii0tk4dPc2BCrIhgZkrFWWqsgPEw%3D";
        public string getfid(string SS)
        {
           
            string url = "https://pan.baidu.com/api/search?recursion=1&order=time&desc=1&showempty=0&web=1&page=1&num=100&key="+SS+"&t=0.44952105920289886&channel=chunlei&web=1&app_id=250528&bdstoken=48404fb5de446913308370558e82e71a&logid=MTU2MjIwMTE1NTE5NzAuNzAzNjIzMDg2Mzg5MTYxOQ==&clienttype=0";
            string html = method.gethtml(url, COOKIE, "utf-8");
            Match fid = Regex.Match(html, @"fs_id"":([\s\S]*?),");
            return fid.Groups[1].Value;
        }

        public void getLink()
        {
            
            string ss = textBox2.Text;
          
                string url = "https://pan.baidu.com/share/set?channel=chunlei&clienttype=0&web=1&channel=chunlei&web=1&app_id=250528&bdstoken=48404fb5de446913308370558e82e71a&logid=MTU2MjIwMjE5MjI4MjAuNzAxMDA4NjYxNDk1NzM4Mw==&clienttype=0";
            string postdata = "schannel=4&channel_list=%5B%5D&period=1&pwd=qazx&fid_list=%5B"+getfid(ss)+"%5D";


            string html = method.PostUrl(url,postdata,COOKIE,"UTF-8");
            
            Match link = Regex.Match(html, @"link"":""([\s\S]*?)""");
            ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
            listViewItem.SubItems.Add(ss);
            listViewItem.SubItems.Add("未下载");
            listViewItem.SubItems.Add(link.Groups[1].Value.Replace("\\", ""));
           
            listViewItem.SubItems.Add(DateTime.Now.ToString());
        }



        public void  run()
        {
            try
            {
                string cookie = "__dxca=2bf12bd4-691d-41d6-83c8-09cffad21c63; msign_dsr=1561165647141; UM_distinctid=16b7cb94c4c1d1-096e50efb17b03-e353165-1fa400-16b7cb94c4e43; JSESSIONID=3539B7B776F70ED80924EC5553F91E21.tomcat1; route=376cb6de2e4984b327cce781377f6d41; DSSTASH_LOG=C%5f35%2dUN%5f%2d1%2dUS%5f%2d1%2dT%5f1561165647141; userIPType_abo=1; userName_dsr=\"\"; enc_abo=A8A4B0A033E2E8462153267F85A75FB0; groupId=431; nopubuser_abo=0; groupenctype_abo=1; duxiu=userName%5fdsr%2c%3d0%2c%21userid%5fdsr%2c%3d%2d1%2c%21char%5fdsr%2c%3d%2c%21metaType%2c%3d0%2c%21logo%5fdsr%2c%3dareas%2fucdrs%2fimages%2flogo%2ejpg%2c%21logosmall%5fdsr%2c%3darea%2fucdrs%2flogosmall%2ejpg%2c%21title%5fdsr%2c%3d%u5168%u56fd%u56fe%u4e66%u9986%u53c2%u8003%u54a8%u8be2%u8054%u76df%2c%21url%5fdsr%2c%3d%2c%21compcode%5fdsr%2c%3d%2c%21province%5fdsr%2c%3d%2c%21isdomain%2c%3d0%2c%21showcol%2c%3d0%2c%21isfirst%2c%3d0%2c%21og%2c%3d0%2c%21ogvalue%2c%3d0%2c%21cdb%2c%3d0%2c%21userIPType%2c%3d1%2c%21lt%2c%3d0%2c%21enc%5fdsr%2c%3dDFF50C5FEFE4DB34AF818E35A0DECC24; AID_dsr=689; userId_abo=%2d1; schoolid_abo=689; user_enc_abo=B5AB06A2BD20A0BC011FBFEE2B9757CB; idxdom=www%2eucdrs%2esuperlib%2enet; CNZZDATA2088844=cnzz_eid%3D1687723352-1561160529-%26ntime%3D1563612631";
                string html = method.GetUrlWithCookie(textBox1.Text,cookie, "utf-8");
                 Match title = Regex.Match(html, @"bookname=""([\s\S]*?)""");
                Match zuozhe = Regex.Match(html, @"<dd>【作　者】([\s\S]*?)</dd>");
                Match chubanshe = Regex.Match(html, @"<dd>【出版项】([\s\S]*?)</dd>");
                Match date = Regex.Match(html, @"publishdate"" value = ""([\s\S]*?)""");
                Match ISBN = Regex.Match(html, @"<dd>【ISBN号】([\s\S]*?)</dd>");
                Match page = Regex.Match(html, @"<dd>【形态项】([\s\S]*?)</dd>");

                textBox3.Text = "书名："+title.Groups[1].Value.Trim() + "\r\n";
                textBox3.Text += "\r\n";
                textBox3.Text += "作者：" + zuozhe.Groups[1].Value.Trim() + "\r\n";
                textBox3.Text += "\r\n";
                textBox3.Text += "出版社：" + chubanshe.Groups[1].Value.Trim() + "\r\n";
                textBox3.Text += "\r\n";
                textBox3.Text += "出版时间：" + date.Groups[1].Value.Trim() + "\r\n";
                textBox3.Text += "\r\n";
                textBox3.Text += "ISBN：" + ISBN.Groups[1].Value.Trim()+ "\r\n";
                textBox3.Text += "\r\n";
                textBox3.Text += "页码：" + page.Groups[1].Value.Trim() + "\r\n";


                Match SSN = Regex.Match(html, @"ssn=([\s\S]*?)&");
                string ssn = SSN.Groups[1].Value;
                textBox2.Text =ssn ;

                if (getfid(ssn) == "")
                {
                    label11.Text = "0";
                    return;
                }
                string path = AppDomain.CurrentDomain.BaseDirectory;
                StreamReader sr = new StreamReader(path+ "BDK\\熊猫软件可下载书单.txt", Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                sr.Close();
                if (texts.Contains(ssn))
                {
                    label9.Text = "有";
                    label10.Text = "存在熊猫软件可下载书单"+"\r\n";
                    label11.Text = "1";
                }

                StreamReader sr1 = new StreamReader(path + "BDK\\一元一本QQ.txt", Encoding.Default);
                string texts1 = sr1.ReadToEnd();
                if (texts1.Contains(ssn))
                {
                    label9.Text = "有";
                    label10.Text += "存在一元一本QQ";
                    label11.Text = "1";
                }
                sr1.Close();



            }
            catch (Exception)
            {

                throw;
            }
           
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Button2_Click(object sender, EventArgs e)
        {

        }

        private void Button3_Click(object sender, EventArgs e)
        {
           
            if (textBox2.Text != "")
            {
                Thread thread = new Thread(new ThreadStart(getLink));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();
            }
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("IExplore.exe", "http://www.ucdrs.superlib.net/");
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("IExplore.exe", "http://www.bj.cxstar.cn/bookcd/index/index.do");
        }
    }
}
