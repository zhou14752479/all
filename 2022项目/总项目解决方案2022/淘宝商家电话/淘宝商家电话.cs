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

namespace 淘宝商家电话
{
    public partial class 淘宝商家电话 : Form
    {
        public 淘宝商家电话()
        {
            InitializeComponent();
        }








        //接收信号

            public void setcookie2(string cookie2)
        {
            ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count + 1).ToString()); //使用Listview展示数据    
            lv2.SubItems.Add("11");
            lv2.SubItems.Add(cookie2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            登录 login = new 登录();
            login.Show();

            //声明接收
            login.setcookie = new 登录.SetCookie(setcookie2);
        }
        Thread thread;
        bool zanting = true;
        public void run()
        {
            try
            {
                string cookie = "thw=cn; hng=CN%7Czh-CN%7CCNY%7C156; t=c4361740c8e18715aef03845da1acd12; ucn=unsh; cna=IEEVGmxNb3QCAXnisujqtGPv; lgc=zkg852266010; tracknick=zkg852266010; _m_h5_tk=459a1c2dc5c03e4b0008a68287723506_1644217078548; _m_h5_tk_enc=95e3776b452c52084f438cf6b890ddaf; xlly_s=1; mt=ci=69_1; enc=g73m1nw5Avtc2r85ckZDkSkLSDSfnNXuHuMJGuZPTOLpoZ%2FbLNgSnr8vRm7ogOIR7M5eu1b97FQAWkggSu4gaw%3D%3D; cookie2=20c20bfdcaa0fe04f16fa4ecd40ae453; _tb_token_=e5e8e9e53e733; _samesite_flag_=true; sgcookie=E100WCoNiZPl64OWnGyulNHKEmRplj%2FA8%2Bwiyimm4acC6f%2FsJ8fDBMKY4VbI2ClSzB2usY7v4e1FuSk4CpjS22iqvmGBAFdesj1rnye9nC04fidEAAxgpu2OW%2F%2F3rCAvJbPF; unb=1052347548; uc3=id2=UoH62EAv27BqSg%3D%3D&nk2=GcOvCmiKUSBXqZNU&lg2=VFC%2FuZ9ayeYq2g%3D%3D&vt3=F8dCvU6PKx4EHfidi1A%3D; csg=d0d35746; cancelledSubSites=empty; cookie17=UoH62EAv27BqSg%3D%3D; dnk=zkg852266010; skt=934e55723c04259c; existShop=MTY0NDIyNzAzNA%3D%3D; uc4=nk4=0%40GwrkntVPltPB9cR46GncAmas5jv7MYQ%3D&id4=0%40UOnlZ%2FcoxCrIUsehK6jmZTplGsR2; _cc_=VFC%2FuZ9ajQ%3D%3D; _l_g_=Ug%3D%3D; sg=080; _nk_=zkg852266010; cookie1=Vvj8uMJubtxirKFtxaDmWPxYCP5sb7EKtrFe1w68JDk%3D; uc1=cookie16=UIHiLt3xCS3yM2h4eKHS9lpEOw%3D%3D&cookie14=UoewBGWghfgO4w%3D%3D&existShop=true&cookie15=UtASsssmOIJ0bQ%3D%3D&pas=0&cookie21=W5iHLLyFfXVRCJf5lG0u7A%3D%3D; v=0; tfstk=clPVBuvUgsCqqbh1pjGNCQpyCHuAavU0SQuIn-sA-ZMymTkjYsmDX4mjk4u6zhDc.; l=eBMcj_yIgkAmzySFBO5Zlurza7790IOf1sPzaNbMiInca1rP1nJ-ANCnMHByRdtj_t5xleKrTan6ER3XPzULRxMc7djSi8nBhn96Je1..; isg=BL6-wPw-UGmoKIfjRliySl30D9QA_4J5Z3kwmWjGOYG3C13l0YuciXatg9_HM3qR";

                for (int page = 1; page < 100; page++)
                {
                    string url = "https://buyertrade.taobao.com/trade/itemlist/asyncBought.htm?action=itemlist/BoughtQueryAction&event_submit_do_query=1&_input_charset=utf8";
                    string postdata = "buyerNick=&canGetHistoryCount=false&dateBegin=0&dateEnd=0&historyCount=0&lastStartRow=&logisticsService=&needQueryHistory=false&onlineCount=0&options=0&orderStatus=&pageNum=3&pageSize=15&queryBizType=&queryForV2=false&queryOrder=desc&rateStatus=&refund=&sellerNick=&prePageNo=2";
                    string html = method.PostUrl(url,postdata,cookie,"gb2312", "application/x-www-form-urlencoded", url);
                    MatchCollection ahtmls = Regex.Matches(html, @"""batchGroup""([\s\S]*?)""subOrders""");
                    for (int i = 0; i < ahtmls.Count; i++)
                    {
                        string createDay = Regex.Match(ahtmls[i].Groups[1].Value, @"""createDay"":""([\s\S]*?)""").Groups[1].Value;
                        string id = Regex.Match(ahtmls[i].Groups[1].Value, @"""id"":""([\s\S]*?)""").Groups[1].Value;
                        string tradeStatus = Regex.Match(ahtmls[i].Groups[1].Value, @"""tradeStatus"":""([\s\S]*?)""").Groups[1].Value;
                        string nick = Regex.Match(ahtmls[i].Groups[1].Value, @"""nick"":""([\s\S]*?)""").Groups[1].Value;
                        

                        string tel = "";
                        string jiexi = "";
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据    
                        lv1.SubItems.Add(createDay);
                        lv1.SubItems.Add(id);
                        lv1.SubItems.Add(tradeStatus);
                        lv1.SubItems.Add("账号");
                        lv1.SubItems.Add(nick);
                        lv1.SubItems.Add(tel);
                        lv1.SubItems.Add(jiexi);

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }
                        Thread.Sleep(500);

                    }
                }
            }
            catch (Exception ex)
            {

                textBox1.Text = ex.ToString();
            }

        }

        private void 淘宝商家电话_Load(object sender, EventArgs e)
        {
           
        }

      

        private void button3_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
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
    }
}
