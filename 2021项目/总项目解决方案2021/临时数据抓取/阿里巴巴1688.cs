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
using myDLL;

namespace 临时数据抓取
{
    public partial class 阿里巴巴1688 : Form
    {
        public 阿里巴巴1688()
        {
            InitializeComponent();
        }
        Thread thread;
        bool status = true;
        bool zanting = true;

        string cookie = "UM_distinctid=177fcc1f9a4520-0230de18f046ee-31346d-1fa400-177fcc1f9a5b35; cna=P4y2GM7SE3kCAXniuV3Hr4CT; ali_apache_id=11.186.201.1.1614853308593.395581.7; hng=CN%7Czh-CN%7CCNY%7C156; lid=zkg852266010; xlly_s=1; ali_ab=121.226.182.199.1616550764897.4; ali_apache_track=c_mid=b2b-1052347548|c_lid=zkg852266010|c_ms=1; taklid=d01db77d42f7431d8993020d1f9347ac; _m_h5_tk=73e0bb96895fdde1be2a895db13b35ed_1616643464508; _m_h5_tk_enc=7a69fb5eff2fcc73a1e196dbcc423ac8; __cn_logon_id__=zkg852266010; __cn_logon__=true; __last_loginid__=zkg852266010; last_mid=b2b-1052347548; cookie1=Vvj8uMJubtxirKFtxaDmWPxYCP5sb7EKtrFe1w68JDk%3D; cookie2=17e5e496467381bfe8ec2dfc3fef6225; cookie17=UoH62EAv27BqSg%3D%3D; t=8d8f3870b48df7f4ec421ab5b51d380c; _tb_token_=eb66143873839; sg=080; csg=53cbbe43; unb=1052347548; uc4=nk4=0%40GwrkntVPltPB9cR46GncB6hzGy3c5sY%3D&id4=0%40UOnlZ%2FcoxCrIUsehK62SDE517isB; ali_apache_tracktmp=c_w_signed=Y; _nk_=zkg852266010; _csrf_token=1616633385335; _is_show_loginId_change_block_=b2b-1052347548_false; _show_force_unbind_div_=b2b-1052347548_false; _show_sys_unbind_div_=b2b-1052347548_false; _show_user_unbind_div_=b2b-1052347548_false; __rn_alert__=false; ad_prefer=\"2021/03/25 08:49:49\"; h_keys=\"%u5973%u88c5#%u5973%u5305#%u5973%u978b#%u7ae5%u88c5#%u673a%u68b0%u8bbe%u5907\"; alicnweb=touch_tb_at%3D1616633389366%7Clastlogonid%3Dzkg852266010%7Cshow_inter_tips%3Dfalse; tfstk=cp3cBuj91mrXmfroAEajGvEp7KzGao_aKVuSzVc5k2k8OUgYgsvd4M-orQ2mnVI1.; l=eBMeE14Vjbg91AnSBO5Zourza77ONIRbzsPzaNbMiInca1y5dFgVtNCQINpkfdtj_tfvhexzhg8L7dF9WkUU-jDDBeYQiD14-xv9-; isg=BA8PQpFZs10O4re6pffl29AvnqMZNGNW0X-8EyEcBn6F8C3yKwZEp4eo9ijOiDvO";
        #region 1688
        public void run()
        {
            string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < text.Length; i++)
            {
              string keyword=  System.Web.HttpUtility.UrlEncode(text[i], Encoding.GetEncoding("GB2312"));
                for (int page = 0; page < 51; page++)
                {
                    label1.Text = page.ToString();

                    string url1 = "https://search.1688.com/service/companySearchBusinessService?spm=a26352.13672862.searchbox.3.209d6039eA3TgI&keywords="+keyword+"&filt=y&province=%E4%B8%8A%E6%B5%B7&beginPage=1&async=true&asyncCount=6&pageSize=20&requestId=dYeGW4byxi3MifMRJGTxpfPamhNHRZAif22cykQ3JKHFx2xa&sessionId=d6f63271486842f0b67bee7ea7ab01cc&startIndex=6&pageName=supplier&_bx-v=1.1.20";
                    string url2 = "https://search.1688.com/service/companySearchBusinessService?spm=a26352.13672862.searchbox.3.209d6039eA3TgI&keywords=" + keyword + "&filt=y&province=%E4%B8%8A%E6%B5%B7&beginPage=1&async=true&asyncCount=14&pageSize=20&requestId=dYeGW4byxi3MifMRJGTxpfPamhNHRZAif22cykQ3JKHFx2xa&sessionId=d6f63271486842f0b67bee7ea7ab01cc&startIndex=6&pageName=supplier&_bx-v=1.1.20";

                    string html1 = method.GetUrlWithCookie(url1, cookie, "utf-8");

                    string html2 = method.GetUrlWithCookie(url2, cookie, "utf-8");

                    string html = html1 + html2;
                  
                    MatchCollection uids = Regex.Matches(html, @"""domainUri"":""([\s\S]*?)""");


                    if (uids.Count == 0)
                        break;
                    for (int j = 0; j < uids.Count; j++)
                    {


                        string aurl = "https://" + uids[j].Groups[1].Value + "/page/contactinfo.htm";
                        string ahtml = method.GetUrlWithCookie(aurl, cookie, "gb2312");

                        Match title = Regex.Match(ahtml, @"<p class=""info"">([\s\S]*?)</span>");
                        Match addr = Regex.Match(ahtml, @"址：</dt>([\s\S]*?)</dd>");
                        Match phone = Regex.Match(ahtml, @"话：</dt>([\s\S]*?)</dd>");
                        Match tel = Regex.Match(ahtml, @"移动电话：</dt>([\s\S]*?)</dd>");
                        Match name = Regex.Match(ahtml, @"class=""membername"" target=""_blank"">([\s\S]*?)<");
                        if (title.Groups[1].Value == "")
                        {
                            MessageBox.Show("1");
                        }
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(Regex.Replace(title.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(addr.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(phone.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(tel.Groups[1].Value, "<[^>]+>", "").Trim());
                        lv1.SubItems.Add(Regex.Replace(name.Groups[1].Value, "<[^>]+>", "").Trim());
                        Random rd = new Random();
                        Thread.Sleep(rd.Next(1,5)*1000);
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;
                    }
                }
            }


        }

        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            cookie = textBox2.Text;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

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

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
