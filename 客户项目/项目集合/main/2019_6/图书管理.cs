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
        
        public void  run()
        {
            try
            {
                string cookie = "__dxca=2bf12bd4-691d-41d6-83c8-09cffad21c63; JSESSIONID=00B1E3FF849435183810FE59067F1C97.tomcat218; route=7f74d77075feb1edd5827d8afbb814a4; DSSTASH_LOG=C%5f35%2dUN%5f%2d1%2dUS%5f%2d1%2dT%5f1561165647190; userIPType_abo=1; userName_dsr=\"\"; enc_abo=A8A4B0A033E2E8462153267F85A75FB0; groupId=431; nopubuser_abo=0; groupenctype_abo=1; duxiu=userName%5fdsr%2c%3d0%2c%21userid%5fdsr%2c%3d%2d1%2c%21char%5fdsr%2c%3d%2c%21metaType%2c%3d0%2c%21logo%5fdsr%2c%3dareas%2fucdrs%2fimages%2flogo%2ejpg%2c%21logosmall%5fdsr%2c%3darea%2fucdrs%2flogosmall%2ejpg%2c%21title%5fdsr%2c%3d%u5168%u56fd%u56fe%u4e66%u9986%u53c2%u8003%u54a8%u8be2%u8054%u76df%2c%21url%5fdsr%2c%3d%2c%21compcode%5fdsr%2c%3d%2c%21province%5fdsr%2c%3d%2c%21isdomain%2c%3d0%2c%21showcol%2c%3d0%2c%21isfirst%2c%3d0%2c%21og%2c%3d0%2c%21ogvalue%2c%3d0%2c%21cdb%2c%3d0%2c%21userIPType%2c%3d1%2c%21lt%2c%3d0%2c%21enc%5fdsr%2c%3d99B6801E8A942DE1AB7E91EFAB8AE4CF; AID_dsr=689; userId_abo=%2d1; schoolid_abo=689; user_enc_abo=B5AB06A2BD20A0BC011FBFEE2B9757CB; idxdom=www%2eucdrs%2esuperlib%2enet; msign_dsr=1561165647141; UM_distinctid=16b7cb94c4c1d1-096e50efb17b03-e353165-1fa400-16b7cb94c4e43; CNZZDATA2088844=cnzz_eid%3D1687723352-1561160529-%26ntime%3D1561165463";
                string html = method.gethtml(textBox1.Text,cookie, "utf-8");
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

                string path = AppDomain.CurrentDomain.BaseDirectory;
                StreamReader sr = new StreamReader(path+ "BDK\\熊猫软件可下载书单.txt", Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                sr.Close();
                if (texts.Contains(ssn))
                {
                    label9.Text = "存在熊猫软件可下载书单"+"\r\n";
                }

                StreamReader sr1 = new StreamReader(path + "BDK\\一元一本QQ.txt", Encoding.Default);
                string texts1 = sr1.ReadToEnd();
                if (texts1.Contains(ssn))
                {
                    label9.Text += "存在一元一本QQ";
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
    }
}
