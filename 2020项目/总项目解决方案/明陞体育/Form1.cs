using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 明陞体育
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        public void run()
        {
            try
            {
                string url = "https://www.1288945.com/Main/Sports/mSports/nss/Main2Data.aspx";
                string cookie = "ASP.NET_SessionId=3zdxqzteq40g3jshr5jdxswy; userIPCountry=CN; selectedLanguage=zh-CN; m88_cookie2=2248349100.20480.0000; ms_session=%7B%22brand%22%3A%22M88%22%2C%22channel%22%3A%7B%22trafficSource%22%3A%22DIRECT%22%2C%22trafficReferral%22%3A%22%22%2C%22referralURL%22%3A%22%22%2C%22referralFullURL%22%3A%22%22%2C%22registerFrom%22%3A%22www.1288945.com%22%2C%22platform%22%3A%22Desktop%22%2C%22custom_data%22%3A%7B%7D%7D%7D; ms_traffic_source=DIRECT; _pk_ses.1.accf=1; OddsGroup=3; curr1=; lastdt=10~Soccer~2; category=0; categoryMenu=0; lgnum_13=10_999; dtC=2; lgnum_21=999; lgnum_12=10_999; srttyp=0; Lspid=10_11; _pk_id.1.accf=83f39825f4bb460b.1567385568.1.1567385957.1567385568.; M88_COOKIE=!Epr8niwgo50PThM0nKMQGzfoNpOTszUK/9pPF3nPw8ykhdhlOxyGn193K4ir7boeKlNSMsR3NGvVNPI=; M88_NSS=!irpgSJA0VwxkSOE0nKMQGzfoNpOTsynEy83N8ejLI3DxQ2Ij6bJC4VNYJKNdIcgE94W2VzLErSKghw==";
                string datas = "spid=10&ot=2&dt=21&vt=0&vs=0&tf=0&vd=0&lid=ch&lgnum=999&reqpart=0&verpart=undefined&prevParams=undefined&verpartLA=";
                string html = method.PostUrl(url, datas, cookie, "utf-8");

                textBox1.Text = html;
                MatchCollection matches = Regex.Matches(html, @"2019([\s\S]*?)','([\s\S]*?)','([\s\S]*?)'");
                MatchCollection p1s = Regex.Matches(html, @"',,'([\s\S]*?)'");
                for (int i = 0; i < matches.Count; i++)
                {
                    ListViewItem listViewItem = listView1.Items.Add((listView1.Items.Count + 1).ToString());
                    listViewItem.SubItems.Add(matches[i].Groups[2].Value+"-"+ matches[i].Groups[3].Value);
                    listViewItem.SubItems.Add(p1s[i].Groups[1].Value);
                    string[] texts = p1s[i].Groups[1].Value.Split(new string[] { "|" }, StringSplitOptions.None);
                    if (Convert.ToDouble(textBox2.Text) > Convert.ToDouble(texts[0]))
                    {
                       // textBox1.Text += matches[i].Groups[2].Value + "-" + matches[i].Groups[3].Value + "达到设置赔率"+"\r\n";
                    }
                }
            }
            catch (Exception ex)
            {

              MessageBox.Show( ex.ToString());
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
           // timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            timer1.Stop(); 
        }

       
    }
}
