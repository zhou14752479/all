using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 淘宝问答
{
    public partial class 淘宝问答 : Form
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
        public 淘宝问答()
        {
            InitializeComponent();
        }

        public string allcookie { get; set; }
        public string loginname { get; set; }

        
        private void button1_Click(object sender, EventArgs e)
        {
           login login = new login();
            login.ShowDialog();
          
          
          
        }
        #region  程序关闭删除自身
        public static void TestForKillMyself()
        {
            string bat = @"@echo off
                           :tryagain
                           del %1
                           if exist %1 goto tryagain
                           del %0";
            File.WriteAllText("killme.bat", bat);//写bat文件
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "killme.bat";
            psi.Arguments = "\"" + Environment.GetCommandLineArgs()[0] + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
        }


        #endregion


       
        public string linkcount = "0";
        private void 淘宝问答_Load(object sender, EventArgs e)
        {
            string jiqima = IniReadValue("value", "key");
            string value2 = method.Base64Decode(Encoding.GetEncoding("utf-8"), jiqima);
            string value = method.Base64Decode(Encoding.GetEncoding("utf-8"), value2);
            string[] text = value.Split(new string[] { "*" }, StringSplitOptions.None);
            string time = method.ConvertStringToDateTime(text[1]).ToString("yyyy-MM-dd");
            this.Text = "到期时间："+time+"  链接个数："+text[2];
           
            linkcount= text[2]; 


            #region 通用检测


            if (!method.GetUrlWithCookie("http://acaiji.com/index/index/vip.html","", "utf-8").Contains(@"sej0N"))
            {
                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();

                return;
            }

            #endregion
            login.sendData += ShowInfo;

            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void ShowInfo(string name,string cookie)
        {
            this.BeginInvoke((Action)(() =>
            {
              label5.Text = "账号: " + name+ " 登陆成功！";
               
                loginname = name;
                allcookie = cookie; 
               
            }));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //allcookie = "thw=cn; enc=HYxeMUeTzCthhdPIdLk0WU7%2Fs2GTWjmKTCc5HJfIS%2BbcjvBNH6zITVSd2Ld0w8jbIvuevTcpWUHeEY2DSUG6pw%3D%3D; useNativeIM=false; t=fafab523e1e10e43a754fee738329ec8; _tb_token_=f37e57a41456e; xlly_s=1; cookie2=973d2bcb7e84fd4a8fc2b7e12dc8f740; _samesite_flag_=true; cna=42NtGwhsrnACAXnit7rR4XOG; sgcookie=E100LJ0siFmvsmBAchPSzPlk1Mt9FVoXchvKKZdbDeB51Jg7gwDZha%2F6%2F2xN0ZeWRXo8E9%2B0TTQhShi6mrjpxokdLsCbxYDcHhBO80IUGvvadNk%3D; unb=2209546201082; uc3=lg2=VFC%2FuZ9ayeYq2g%3D%3D&nk2=rOrPqG7pTwQ%3D&id2=UUphw2BBUoAdThjIpw%3D%3D&vt3=F8dCvjESzLnWpM60Fbg%3D; csg=6e8c6550; lgc=%5Cu76F8%5Cu8317%5Cu6C34%5Cu4EA7; cancelledSubSites=empty; cookie17=UUphw2BBUoAdThjIpw%3D%3D; dnk=%5Cu76F8%5Cu8317%5Cu6C34%5Cu4EA7; skt=c8176230baa750d9; existShop=MTY3MjMwMTE2Ng%3D%3D; uc4=nk4=0%40rsQTqDbhIf9finOOQMGJTYjRzA%3D%3D&id4=0%40U2grGN9DEVUYAflxDbRDtn4wr%2FolMzoZ; tracknick=%5Cu76F8%5Cu8317%5Cu6C34%5Cu4EA7; _cc_=VFC%2FuZ9ajQ%3D%3D; _l_g_=Ug%3D%3D; sg=%E4%BA%A72c; _nk_=%5Cu76F8%5Cu8317%5Cu6C34%5Cu4EA7; cookie1=Uoe2yNKkZmH3pUqfoam9E5vpHGsvP4NJjcjuwL0Swlw%3D; mt=ci=0_1; uc1=cookie21=UIHiLt3xTwwM1Oej1w%3D%3D&pas=0&cookie15=UIHiLt3xD8xYTw%3D%3D&cookie14=UoezTU0XYr1adw%3D%3D&cookie16=UIHiLt3xCS3yM2h4eKHS9lpEOw%3D%3D&existShop=true; _m_h5_tk=351623608668bd6725405b5543c508f4_1672315733325; _m_h5_tk_enc=90cd40a59badeceb51000bb12593c0c6; tfstk=c-KfBFXIxSVX0yngmsMr3-Y9vU7OZvP5Qq69hj37sRdK71pfiJZFOjkHSWjNBT1..; l=fBIrrPgITnyRL7pkXOfwPurza77OSIRAguPzaNbMi9fPOM1D550hW6SBdpLkC3GVF6PBR3z1UrspBeYBqIVI5iWc_EleaQkmnmOk-Wf..; isg=BGdnWufTyNP1oUxfY2WWXeDR9psx7DvOkS_hyTnUg_YdKIfqQbzLHqUqTii2wBNG";

            //textBox1.Text = allcookie;
            if(textBox1.Text.Trim()=="")
            {
                MessageBox.Show("请输入链接");
                return;
            }

            MatchCollection links = Regex.Matches(textBox1.Text, @"http([\s\S]*?) ");

            if(links.Count>Convert.ToInt32(linkcount))
            {
                MessageBox.Show("当前设备绑定数量不足");
                return;
            }
            for (int i = 0; i < links.Count; i++)
            {
                string uid = md.getitemidbylink("http" + links[i].Groups[1].Value);


                ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                lv1.SubItems.Add(loginname);
                lv1.SubItems.Add("http" + links[i].Groups[1].Value);
                lv1.SubItems.Add(textBox2.Text.Trim());
                lv1.SubItems.Add("0/"+textBox3.Text.Trim());
                lv1.SubItems.Add(uid);
                lv1.SubItems.Add(allcookie);
            }


            //textBox1.Text = allcookie;
        }




        public void  jishi()
        {
            try
            {

                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    string name = listView1.Items[i].SubItems[1].Text;
                    string link = listView1.Items[i].SubItems[2].Text;
                    string time = listView1.Items[i].SubItems[3].Text;
                    string cishu = listView1.Items[i].SubItems[4].Text;
                    string itemid = listView1.Items[i].SubItems[5].Text;
                    string cookie= listView1.Items[i].SubItems[6].Text;
                    int value = (Convert.ToInt32(time) - 1);
                    listView1.Items[i].SubItems[3].Text =value.ToString();
                    if(value<=0)
                    {

                        string[] text = cishu.Split(new string[] { "/" }, StringSplitOptions.None);
                        label5.Text= DateTime.Now.ToString("HH:mm:ss")+"：  "+  md.answer(itemid,cookie);
                        listView1.Items[i].SubItems[3].Text = textBox2.Text;
                        listView1.Items[i].SubItems[4].Text = (Convert.ToInt32(text[0]) + 1).ToString()+"/"+text[1];
                    }
                }

            }
            catch (Exception ex)
            {

                label5.Text=ex.ToString();
            }
        }
        method md = new method();
        private void button3_Click(object sender, EventArgs e)
        {
            //string cookie = "intl_locale=zh_CN;arms_uid=cfc4f083-54aa-4ed9-8f85-b02374dff0a3;SameSite=none;XSRF-TOKEN=ed7e2dfe-e286-4541-b504-dce13999785f;_samesite_flag_=true;cookie2=1c8cf5f7bac36084096681259b239dca;t=68062735b62110a9b7f82c9dda43ab36;_tb_token_=3358ab3e77ab1;cna=TO02HDqTGWcCAXLv5QL3k2dS;cna=TO02HDqTGWcCAXLv5QL3k2dS;sca=b54aa994;tbsa=03edb894b304b0421946ec76_1672478540_1;cbc=GB0685181BD363903468F2154581FD4E7EC38111DE139CB29ED;umdata_=G993B14084DE608BAC308A87CB690D0BA057B3E8F3B03132B13;xlly_s=1;sgcookie=E100WXtso7bk1F2g3MoHSeVcarNfea2ZPZpjQu2ljuA998OoqDTHi4uEVzkSfy9LRBuYNFQkGJhSkHOiftpwCZSentKVAG246DDPgftSSqYDiCA%2F2lLuC9rVUu37rs3%2BDZNd;unb=402168116;uc1=cookie14=UoezTUqELCNnTQ%3D%3D&cookie21=VT5L2FSpdeCjwGS%2FFqZpWg%3D%3D&cookie16=V32FPkk%2FxXMk5UvIbNtImtMfJQ%3D%3D&existShop=true&pas=0&cookie15=U%2BGCWk%2F75gdr5Q%3D%3D;uc3=id2=VyyTmPqoEuVk&lg2=UIHiLt3xD8xYTw%3D%3D&nk2=r7pv0Nh%2F&vt3=F8dCvjAZradTIHeSjcs%3D;csg=0f2955c3;lgc=%5Cu7530%5Cu53E4%5Cu9633;cancelledSubSites=empty;cookie17=VyyTmPqoEuVk;dnk=%5Cu7530%5Cu53E4%5Cu9633;skt=94e8d43b92410b7d;existShop=MTY3MjQ3ODU5Mw%3D%3D;uc4=nk4=0%40rVjjcNJZTTHRpe5SSnuYkFc%3D&id4=0%40VXtcNHCfaECA6FZ1wpTl19bIwiw%3D;tracknick=%5Cu7530%5Cu53E4%5Cu9633;lc=V36%2FOFRuK7Jwcko%3D;_cc_=W5iHLLyFfA%3D%3D;lid=%E7%94%B0%E5%8F%A4%E9%98%B3;_l_g_=Ug%3D%3D;sg=%E9%98%B361;_nk_=%5Cu7530%5Cu53E4%5Cu9633;cookie1=ACjlyUjTzdNGuH2O6X92jz8O1aN06Hjb2QDQQdwIYAA%3D;XSRF-TOKEN=a6e00df5-e267-418a-8ba0-873291f85029;cookie1=ACjlyUjTzdNGuH2O6X92jz8O1aN06Hjb2QDQQdwIYAA%3D;cookie2=1c8cf5f7bac36084096681259b239dca;cookie17=VyyTmPqoEuVk;sgcookie=E100WXtso7bk1F2g3MoHSeVcarNfea2ZPZpjQu2ljuA998OoqDTHi4uEVzkSfy9LRBuYNFQkGJhSkHOiftpwCZSentKVAG246DDPgftSSqYDiCA%2F2lLuC9rVUu37rs3%2BDZNd;t=68062735b62110a9b7f82c9dda43ab36;_tb_token_=3358ab3e77ab1;sg=%E9%98%B361;csg=0f2955c3;lid=%E7%94%B0%E5%8F%A4%E9%98%B3;unb=402168116;uc4=nk4=0%40rVjjcNJZTTHRpe5SSnuYkFc%3D&id4=0%40VXtcNHCfaECA6FZ1wpTl19bIwiw%3D;__cn_logon__=true;__cn_logon_id__=%E7%94%B0%E5%8F%A4%E9%98%B3;ali_apache_track=c_mid=b2b-402168116|c_lid=%E7%94%B0%E5%8F%A4%E9%98%B3|c_ms=1;ali_apache_tracktmp=c_w_signed=Y;tfstk=c2SGBsYYeN86Lw8hPlt_2_aO5vLNaZ0wwi7PYJWdSp0VZ0spQscK84WY1JvYN5uf.;isg=BKmptoPh7neImNLDRAPtSmwfuFUDdp2oylf1xUue8xDPEsskksf9eI7L0L4kijXg;l=fBOJwO9nTk94lRfGBO5aPurza77tUIRbzsPzaNbMiIEGa6MPJFapENCFx-hH6dtj_TCxbeKPk6NoQdnH-e4KAxDDBejeokOsnxvOFzBj4;cna=TO02HDqTGWcCAXLv5QL3k2dS;cnaui=402168116;aui=402168116;x5secdata=xb1047e3c406b1c0660067f2a51ba588bf1672478594a-717315356a1993109894abazc2cj402168116a__bx__fourier.taobao.com%3A443%2Frp;xlly_s=1;atpsida=601c16feafa8f22c2e658376_1672478594_2;__mbox_csrf_token=zsDcAQz8rO38r50T_1672480394534;_csrf_token=1672478594568;_m_h5_tk=2503b7cd061b56473d76c69892dd160c_1672486874653;_m_h5_tk_enc=0a468804059d00e43eabe5def57762f9;EGG_SESS=wWjCFFOVNEMbggEBluLj9fFE5qT1FZFtlBUoM9jmNcKok6VwYW8f-vyeEPNXApF15OPxk7RmbxIBQa2mZwkW8eYtoOKF02vFnyXLw2pnF07nCJ_GKQC0MFJh5Btmk5EQYlEGwUZeiPaNN-GfV3pgpILvH5IvUo5fKa0P2QFSvAKObqphUzav6sdvYLjppPs_37hMTpoB2radYOzaBFNsQg7GVOyDkJwtC2vbgymMlE5y5DCOz4zHZloFIofNDLmqJlFz2tfnooajZSMauUJvPMR1358DQsR-Mo9zEHK_tE0WuPdOxUYw5Nky3NKrwkVaJXZqtq90GJH3-kTEhzePwN1QFPf-QGvP8qgeRONuaFq6HmRddDxb8C0llTYxm87M6L2sWGg3rlkKFvHAzUb5pPzOSJNd4_a8Kz_PWZ5m1YXknodXEyrFytPUJFGQRxm-EcJ6elSiHy5RoSrYy54GsUFnyYGoCPRKJ9k4326xzRepzhP1ytING72ZuOv2GUDyjFPFrkkSXkj7YdyrvqD4ww4Bo3HfIEuJzzjVB60lryx6hDu9fP9aPzJj6TA6mSS02pE3ce0fXsdmXNd01_Lw5gyHXWE0kjBaHa5QKKACpTpjGmJW-4A-I7d2gge2uVTwd_tGXsIdJkjSlCDatZgTKKQQnHOE9uQHMv9yc42m2O3X9L8lxN0Cp6xICqpGsG13PfsrA2cV83PDlb0e2vt6OiJqCMCNuMTPkiH2eDyLie_N_CUj_KC-FYAfsncG0X4VFM2-4ZJPaW-yPLi8CHHsOEscVLw_ZIukA5qWV423g376vbhEA46MznxiPVR0Tux2BDx0q24saXOlKIe6YmDFnJiCE9jIsF4nx5urS2XA5MZJcmgBFVLyq7JLkzbruSuGuJUxuCv457H2gdZYDAXPv68MICdwdnOqdAkntZ_-u6EwhsoPFdGkCLgSuBICAn-3odN-EVCDxAdJ7OF3Iq5mI1eHbY9imPnzsCgCXEzQSBCsMsWzgFGBAlDCW340zuKAtfPGbRzj8w5pLGnepsjLS8-rSwkogiXRmU3UvzB332rJ7cYAo4ElAVc6WN82_NqKeFwSxVTIuARPn7JckQLWM05TAdgFTuFKdQqAwAhMdmhcYSEsTZGmvNIIBrS9RwaxUfRX5uDyIWlXsYk_SeZb2-wbswqxHtTKqDNr-h3ASn_etus9HXBY_m9xF8McdPkZ9-AprirUYWb2DZzitEbceefu0bMiNpQxF_D98Xmq7em2gSd0SBQDrL2dYJnuy3sev3O4x7oM9g5J8JNq-X5FOZq6jprUtX2uabF_IDqAU_99Dg8Dtips_oWE4PB0Zuh1OrLeH1bm_rHRsQfi4v1jAQ2Jqyb4ZKuehVEhCGEgfBQbdY-rBwVTTmmM8AYV9FlKjRfqoUWjxTPzxqDNDJ5UXRrYJXJPag2DTAoZEZcYRCCjHHLweHz_8AFW_Cu22WQZ2qPqbs7CKdV3FSdFfoCCvq2gzR5PQBhoZ0qMDr3DIXQ-vG6q2eP9qUvBcbNNt01X9JGMx1ogjfKFlcKb7ZiOt873ZYPKd6lDoheR9i3aYp4PPBzFQFluGzv9a4Hf5jniO7lYYXjm71wqr8KN0-Q1irn4qbuNU8xYo2CfWm14Af5mA2Y_lKAF0DnmTO32z_3T2lTFRWHbdWzFY7t81Ezk9xBf3eadWEEQChn23-9Ub7fwkyEIwvv3pz1_FyUSykqVgAB51hJKGlHHIuEKIOSNnlKEBB3OY8oP1kXogVa-Pjeh-JKlYNf5D-dzjO9uNkHFGVHslnTNh8n2ZpAWmNhmN3RLIB_E0Vjv6XsEBtzori3qLP73VZHYxYwn9__WsUYFK-VD_JJZcGE2t2tUPkcWlConTcWxZGe4bb9nqM9rRXAjbrb3NDzG0zGc5gKy-SPmmGCN9IN6A6IasHUnTN_fln0IBjk2-K5jkHYPZyIuwgs=;isg=BNzcTyrC00BNbaccmU43sKpmrfqOVYB_z8Sgurbd6EeqAXyL3mVQD1IzZWH5ibjX;l=fBTAzsvITk9opwGLKOfwPurza77OSIRAguPzaNbMi9fPOwSe5WXNW6S3Hf9wC3GNFsEJR37AO7DwBeYBqSVgba4CAKVnP4MmnmOk-Wf..;tfstk=c81cBovTJtJfT2FOViOb9MwtjfrGZrk2v1WFaz7RHGJHBpfPi8HrLNEbrFEQ0A1..;";
            //method md = new method();
            // string html = md.answer("391694064180", cookie);
            //MessageBox.Show(html);

            label5.Text = DateTime.Now.ToString("HH:mm:ss") + "：  " + "已启动";
            timer1.Start();

        }

        Thread thread;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(jishi);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void 淘宝问答_FormClosing(object sender, FormClosingEventArgs e)
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

        private void button4_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            label5.Text = DateTime.Now.ToString("HH:mm:ss") + "：  " + "已停止";
        }

        private void 删除此行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                listView1.Items.RemoveAt(listView1.SelectedItems[0].Index);
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;
          
            MessageBox.Show(listView1.SelectedItems[0].SubItems[6].Text);
        }
    }
}
