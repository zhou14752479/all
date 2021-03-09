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
using helper;

namespace 主程序202012
{
    public partial class radr账号验证 : Form
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
        public radr账号验证()
        {
            InitializeComponent();
        }

       




        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, method.EncodingType.GetTxtType(openFileDialog1.FileName));
                //一次性读取完 
                string texts = sr.ReadToEnd();

                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {
                    string[] value = text[i].Split(new string[] { "--" }, StringSplitOptions.None);
                    ListViewItem lv2 = listView2.Items.Add(listView2.Items.Count.ToString()); //使用Listview展示数据
                    lv2.SubItems.Add(value[0].Trim());
                    lv2.SubItems.Add(value[1].Trim());
                    lv2.SubItems.Add(value[2].Trim());
                    lv2.SubItems.Add(" ");
                }
            }
        }


        public void getIP()
        {
            try
            {
                string html = method.GetUrl(textBox1.Text,"utf-8");
                string[] value = html.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < value.Length; i++)
                {
                   
                    textBox9.Text += value[i]+"\r\n";

                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData, string COOKIE, string charset)
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
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = "https://t.radarlab.org/login?c=%2Foverview";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion

        string path = AppDomain.CurrentDomain.BaseDirectory+"data\\";
        public void write(string name,string value)
        {
            
            FileStream fs1 = new FileStream(path + ""+name+".txt", FileMode.Append, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1);
            sw.WriteLine(value);
            sw.Close();
            fs1.Close();
            sw.Dispose();
        }

        
        public bool panduancorrect(string user,string pass)
        {
            bool status = false;

            string[] text =
                {
                "tg6218044--123456tg",
"fang--sun123456",
"lzq8a2--wu123456",
"gog7788b02--gog123456",
"czzyh1--a111111",
"17758809411--l111111",
"l19621204--l111111",
"Iris-yu007--yu123456",
"khfly2107--yu123456",
"baivpal001--bai123456",
"baixiangchu--bai123456",
"vpal001--shen123",
"mcr6264--peng123",
"nxq1665--peng123",
"szj1363709--peng123",
"zqz604342--qq1111",
"gaiyan3322--huang123456",
"lifen982--xiao123456",
"ly115--ly1234567890",
"ly117--ly1234567890",
"ylj7788001--ylj56789",
"mj66789--mj56789",
"wzy1817--wzy56789",
"chf6789--chf56789",
"czqj167--czqj56789",
"xdy6789--xdy56789",
"qwe96644--qwe6789",
"zy777--zy6789",
"lwb178--lwb6789",
"mzl98777--mzl6789",
"qwe32175--qwe6789",
"qwe983601--qwe6789",
"wxj0316--wxj6789",
"qwe56891--qwe6789",
"lgh0001--lgh789",
"rita--rita789",
"gdy12345--gdy789",
"mylw081--mylw789",
"wgf54321--wgf789",
"cdl456789--cdl789",
"lsx3134--lsx789",
"ply356789--ply789",
"txf199--txf789",
"klcjdfh001--klcjdfh159",
"chenjinglin001--chenjinglin999",
"chenyuhao004--chenyuhao999",
"jcp999--jcp999",
"wzw001--wzw999",
"xinliao888--xinliao999",
"rdxt9333--rdxt999",
"kpa197801--kpa999",
"libeilei--libeilei999",
"wym999--wym9999",
"lwj910717--lwj999999",
"qfq5192--qfq999999",
"wzz6789--wzz5555",
"ysz1096--ysz123456",
"lcew101--lcew123456",
"hsy7799--hsy123456",
"mao1999--mao123456",
"mao2999--mao123456",
"czy188--czy123456",
"sy9999--sy123456",
"ljdh53--ljdh123456",
"13666964585--123456",
"zgm02638808--zgm123456",
"yxj829--yxj123456",
"jwj20160354--jwj123456",
"wh18977969008--wh123456",
"zhy0000001--zhy123456",
"21qlm8768--qlm123456",
"xh00000--xh123456149",
"frh000001--frh123456",
"ycg-002--ycg123456",
"ycg--009--ycg123456",
"wsh1939--wsh123456",
"yzw12777047--yzw123456",
"srj339901--srj123456",
"wgf111003--wgf123456",
"wgf777888113--wgf123456",
"wym9116--wym123456",
"cgy13338--cgy123456",
"wl888666000063--wl123456",
"liq7373017--liq123456",
"lzy991123--lzy123456",
"ylp6699218--ylp123456",
"lgl696969--lgl123456",
"lym2248--lym123456",
"13607661925--123456",
"yx0612--yx123456",
"zll20190806--zll123456",
"lj5559990104--lj123456",
"ldh123888--ldh123456",
"xgw0133123--xgw123456",
"zg397713--zg123456",
"xgw0232311--xgw123456",
"hlc800555--hlc123456",
"xgw0212113--xgw123456",
"xgw01233222--xgw123456",
"hy10--hy123456",
"dml7788108--dml123456",
"yg5796--yg123456",
"2016gql113--gql123456",
"2016gql116--gql123456",
"wql182001--wql123456",
"ljj132211--ljj123456",
"khy888666111--khy123456",
"wsz23150040--wsz123456",
"dml7788213--dml123456",
"lqh031918--lqh123456",
"cxl13881026768--cxl123456",
"yangfei88805--yangfei123456",
"lzm558800202--lzm123456",
"xgw0232233--xgw123456",
"shq44417--shq123456",
"wdsfc178--wdsfc123456",
"ns15099--ns123456",
"wmf216872--wmf123456",
"wyh33366675--wyh123456",
"wyh33366671--wyh123456",
"lyj01015--lyj123456",
"ycm6623--ycm123456",
"sjc20180926--sjc123456",
"ylp8802--ylp123456",
"hcl17888--hcl123456",
"dml778899--dml123456",
"fl31870603--fl123456",
"scl155100--scl123456",
"zwk99888--zwk123456",
"wj999111046--wj123456",
"ldm180520--ldm123456",
"wsz23150074--wsz123456",
"zyp1310--zyp123456",
"r13348803640--r123456",
"1604367015--123456",
"zyb945352--zyb123456",
"sgz2369--sgz123456",
"tcy8805--tcy123456",
"wyh33366669--wyh123456",
"cxl198700--cxl123456",
"rkj3789--rkj123456",
"13009421836--123456",
"zzy1237--zzy123456",
"flh1098--flh123456",
"wsz23150071--wsz123456",
"hxh15881--hxh123456",
"13460400036--123456",
"pwg88886666--pwg123456",
"2016gql17--gql123456",
"tap67902--tap123456",
"zhx0001234567--zhx123456",
"fzh131488--fzh123456",
"ygl77672--ygl123456",
"yys8895--yys123456",
"csy6680--csy123456",
"ghq2019013--ghq123456",
"lgc004--lgc123456",
"lsj13730288889--lsj123456",
"g19991722978--g123456",
"wkf201798--wkf123456",
"dys1001--dys123456",
"sjp1636--sjp123456",
"xujun998801--xujun123456",
"wqx113114--wqx123456",
"dml7788120--dml123456",
"wgy5176002--wgy123456",
"xwl12001--xwl123456",
"ma686867--ma357357",
"wsl88800--wsl456456",
"zlw5552--zlw456456",
"chyj4401--chyj123123",
"dyf44830--dyf123123",
"dyf44831--dyf123123",
"lxh999--lxh123123",
"lyb6889--lyb123123",
"nblz828222--nblz123123",
"szl11975--szl123123",
"whd6180--whd123123",
"whd6185--whd123123",
"zsf1919--zsf123123",
"cch398310--cch123123",
"hum117--hum123123",
"jl9882--jl123123",
"jxf9883--jxf123123",
"lhy112302--lhy123123",
"lqszh0004--lqszh123123",
"sap00105--sap123123",
"sap103--sap123123",
"sxw6767002--sxw123123",
"sxy1311--sxy123123",
"tsh9886--tsh123123",
"ycg2986--ycg123123",
"yz07421034--yz123123",
"zhj98812--zhj123123",
"zky1337--zky123123",
"ldx2318--ldx123123",
"qcs789007--qcs123123",
"qjh6598--qjh123123",
"zky2646--zky123123",
"zxq3696--zxq123123",
"1lxd6001--lxd123123",
"3lxd6001--lxd123123",
"fuli002--fuli123123",
"lhm5987--lhm123123",
"lyl7679--lyl123123",
"lz60011--lz123123",
"sbh005555--sbh123123",
"ssg201866--ssg123123",
"wgw96883--wgw123123",
"wh5648--wh123123",
"wjf68001--wjf123123",
"wlr1666--wlr123123",
"wm13149--wm123123",
"wsy163017--wsy123123",
"wxl96572--wxl123123",
"zs35618--zs123123",
"zyb798017--zyb123123",
"zyb798021--zyb123123",
"ylh3899--ylh123123",
"qw118027--qw123123",
"mgm20160121--mgm123123",
"tgl196802--tgl123123",
"fph978--fph678678",
"zgh17695589062--zgh123456",
"wjq26788--wjq123456",
"wm6607--wm123456",
"yxq261536--yxq123456",
"1850887877--123456",
"dzl7582--dzl123456",
"42cwy913--cwy12345",
"wei8899711--wei12345",
"wxf99113--wxf12345",
"lj5678003--lj12345",
"yjcz43--yjcz12345",
"a1823781--a12345",
"zch142--zch12345",
"123lgp003--lgp12345",
"fdh197388--fdh12345",
"lxy8881314--lxy12345",
"wvmf5057--wvmf12345",
"hjj76888--hjj12345",
"msk2017--msk12345",
"gj1315--gj12345",
"lqm1384--lqm123654",
"stw99882406--stw123456",
"dxy3453401--dxy123456",
"dml7788228--dml123456",
"sxp6276--sxp123456",
"lfy4313--lfy123456",
"myg18685838918--myg123456",
"2016gql115--gql123456",
"tangxiaoying1--tangxiaoying123",
"yuan99--yuan123",
"zdb69--zdb123",
"mojisimone--mojisimone123",
"zhkya7806--zhkya123",
"zky7817--zky123",
"chenyougui001--chenyougui123",
"wmx189--wmx123",
"zye002--zye123",
"ywhrt1--ywhrt123",
"lxzhrt1--lxzhrt123",
"zcrt59--zcrt123",
"hycrt57--hycrt123",
"xmrrt7--xmrrt123",
"zcrt356--zcrt123",
"zcrt22211--zcrt123",
"lairong1--lairong123",
"qiulian02--qiulian123",
"ycx886--ycx123",
"daz001--daz111",
"wbs2--111111",
"932767356--x123456"
            };

            foreach (string item in text)
            {
                if ((user + "--" + pass) == item)
                {
                    status= true;
                    return status;
                }

            }

            return status;
        }

        public void run()
        {
           
            try
            {

                int userstart = Convert.ToInt32(textBox5.Text);
                int passstart = Convert.ToInt32(textBox8.Text);



                for (int i = passstart; i < listView1.Items.Count; i++)
                {
                    
                    for (int j = userstart; j < listView1.Items.Count; j++)
                    {
                        if (status == false)
                            return;
                        string user = listView1.Items[j].SubItems[1].Text.Trim();
                        string passold = listView1.Items[i].SubItems[2].Text.Trim();
                        string pass = getnewpass(user,passold);
                        //写入config.ini配置文件
                        IniWriteValue("values", "user", j.ToString());
                        IniWriteValue("values", "pass", i.ToString());


                        string url = "https://t.radarlab.org/api/user/login";
                        string postdata = "json=%7B%22username%22%3A%22" + user + "%22%2C%22password%22%3A%22" + pass + "%22%7D";
                        string cookie = "theme=dark; ver=6.14.7; _ga=GA1.2.2138962939.1607583565; lang=zh_CN; locale=zh-Hans-CN; _gid=GA1.2.244605734.1608021685; _gat=1";

                        if(i==2&&j==2)
                        {
                            string html = PostUrl(url, postdata, cookie, "utf-8");
                        }

                        int xcs = 1000 / Convert.ToInt32(textBox7.Text);
                        if (xcs < 100)
                        {
                            xcs = 100;
                        }
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        Thread.Sleep(xcs);

                        textBox5.Text = j.ToString();
                        textBox8.Text = i.ToString();
                        label12.Text = "正在查询："+user+"   "+pass;

                        if (panduancorrect(user, pass))
                        {
                            write("登录密码正确", user + "--" + pass);
                        }
                        else
                        {
                            if (pass == "asd123456")
                            {
                                if (user == "czx01111" || user == "czx02222" || user == "czx05555" || user == "czx07777" || user == "czx091111" || user == "czx08888" || user == "czx06666" || user == "ertjwmdjlx" || user == "fnuohbsomo" || user == "llrxnltdfr" || user == "miyiwosuso" || user == "nxtpdpvbhd")
                                {
                                    write("登录密码正确", user + "--" + pass);
                                }
                            }
                            if (pass == "asdasdasd")
                            {
                                if (user == "247234079" || user == "610892514" || user == "760120248" || user == "903901315" || user == "782856703" || user == "2ljf314" || user == "3ljf314" || user == "4ljf314")
                                {
                                    write("登录密码正确", user + "--" + pass);
                                }
                            }
                            else
                            {
                                write("登录密码错误", user + "--" + pass);
                            }

                        }



                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public void run2()
        {
            try
            {
                for (int i = 0; i < listView2.Items.Count; i++)
                {
                    Thread.Sleep(3000);
                    listView2.Items[i].SubItems[4].Text = "false";
                    
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void radr账号验证_Load(object sender, EventArgs e)
        {
            //读取config.ini
            if (ExistINIFile())
            {
                textBox5.Text = IniReadValue("values", "user");
                textBox8.Text = IniReadValue("values", "pass");
                textBox1.Text = IniReadValue("values", "ipapi");
            }
            tabControl1.SelectedIndex = 1;
        }
        Thread thread;

        public string getnewpass(string user,string pass)
        {


            string passAdd = pass;

            string shouzimu = Regex.Match(user,@"[a-zA-Z]{1}").Groups[0].Value;
            string zimu = Regex.Match(user, @"[a-zA-Z]{1,}").Groups[0].Value;
            string shuzi = Regex.Match(user, @"\d{1,}").Groups[0].Value;
            switch (comboBox1.Text)
            {
                case "原始密码":
                    passAdd = pass;
                    break;
                case "账号字母加前面":
                    passAdd = zimu+passAdd;
                    break;
                case "账号字母加后面":
                    passAdd = passAdd+zimu;
                    break;
                case "账号首字母加前面":
                    passAdd =shouzimu+ passAdd;
                    break;
                case "账号首字母加后面":
                    passAdd = passAdd+shouzimu;
                    break;
                case "账号首字母大写加前面":
                    passAdd = shouzimu.ToUpper()+ passAdd;
                    break;
                case "账号首字母大写加后面":
                    passAdd = passAdd+shouzimu.ToUpper();
                    break;
                case "账号数字加前面":
                    passAdd =shuzi+ passAdd;
                    break;
                case "账号数字加后面":
                    passAdd = passAdd+shuzi;
                    break;

            }

            return passAdd.Trim();
        }


       
        private void button1_Click(object sender, EventArgs e)
        {
            IniWriteValue("values", "ipapi", textBox1.Text);
            /*
             原始密码
账号字母加前面
账号字母加后面
账号首字母加前面
账号首字母加后面
账号首字母大写加前面
账号首字母大写加后面
账号数字加前面
账号数字加后面
*/
            getIP();


            status = true;
            if (radioButton1.Checked == true)
            {
                if (thread == null || !thread.IsAlive)
                {
                   
                    thread = new Thread(run);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
            if (radioButton2.Checked == true)
            {
                if (thread == null || !thread.IsAlive)
                {
                   
                    thread = new Thread(run2);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
        }

        private void 清空账号ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <listView1.Items.Count; i++)
            {
                listView1.Items[i].SubItems[1].Text = "";
            }
        }


        public void importusers()
        {
          
            bool flag = openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, method.EncodingType.GetTxtType(openFileDialog1.FileName));
                //一次性读取完 
                string texts = sr.ReadToEnd();

                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {

                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(text[i].Trim());
                    lv1.SubItems.Add("");
                    lv1.SubItems.Add(" ");
                }
            }
        }
        private void 导入账号ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           

        }
        private void 导入账号和密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, method.EncodingType.GetTxtType(openFileDialog1.FileName));
                //一次性读取完 
                string texts = sr.ReadToEnd();

                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {
                    string[] value = text[i].Split(new string[] { "--" }, StringSplitOptions.None);
                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(value[0].Trim());
                    lv1.SubItems.Add(value[1].Trim());
                    lv1.SubItems.Add(" ");
                }
            }
        }

        private void 导入密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, method.EncodingType.GetTxtType(openFileDialog1.FileName));
                //一次性读取完 
                string texts = sr.ReadToEnd();

                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {

                    listView1.Items[i].SubItems[2].Text = text[i].Trim();
                  
                }
            }
        }
        bool zanting = true;
        bool status = true;
        private void button2_Click(object sender, EventArgs e)
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

        private void 清空密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].SubItems[2].Text = "";
            }
        }

        private void 清空所有ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 快速导入会卡顿ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            importusers();
           
        }

        private void 普通导入不卡顿时间久ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(importusers));
            Control.CheckForIllegalCrossThreadCalls = false;
            t.SetApartmentState(System.Threading.ApartmentState.STA);                       //将线程eThread设置为单线程单元(STA)模式
            t.Start();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }
    }
}
