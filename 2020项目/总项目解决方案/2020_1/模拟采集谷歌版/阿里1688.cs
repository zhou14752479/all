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
using CefSharp;
using CefSharp.WinForms;
using helper;

namespace 模拟采集谷歌版
{

    public partial class 阿里1688 : Form
    {
        public 阿里1688()
        {
            
          
            InitializeComponent();
        

            //browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(WB_DocumentCompleted);
        }
       

        internal class OpenPageSelf : ILifeSpanHandler
        {
            public bool DoClose(IWebBrowser browserControl, IBrowser browser)
            {
                return false;
            }

            public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
            {

            }

            public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
            {

            }

            public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl,
    string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures,
    IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
            {
                newBrowser = null;
                var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;
                chromiumWebBrowser.Load(targetUrl);
                return true; //Return true to cancel the popup creation copyright by codebye.com.
            }
        }



        string cookie = "__wpkreporterwid_=9192ef62-6c4c-4490-3555-8c73ec902f64; _uab_collina=160029917551410678418585; cna=b2RCFw6lRXICAXnq99rJ24nX; lid=zkg852266010; ali_ab=121.234.247.218.1590897642898.7; ali_apache_track=c_mid=b2b-1052347548|c_lid=zkg852266010|c_ms=1; UM_distinctid=17369d9289b54e-0c0c562ccbba0c-6373664-1fa400-17369d9289da1a; ali_apache_id=11.186.201.43.1595208706311.392756.7; xlly_s=1; hng=CN%7Czh-CN%7CCNY%7C156; ali_beacon_id=121.226.157.8.1600220183186.831557.9; last_mid=b2b-1052347548; cookie2=1bc31d5bee02b9c9c0f969f078e22b33; t=9690c981bf7b16fff7ccf1a3a6d09ef1; _tb_token_=338316b887ebf; alicnweb=touch_tb_at%3D1600298863246%7Clastlogonid%3Dzkg852266010; _m_h5_tk=118e813b0ed3662cf9bdb84447c67cf9_1600307808194; _m_h5_tk_enc=823462f67fa373222822a2a94a423079; cookie1=Vvj8uMJubtxirKFtxaDmWPxYCP5sb7EKtrFe1w68JDk%3D; cookie17=UoH62EAv27BqSg%3D%3D; sg=080; csg=4e53974a; unb=1052347548; uc4=id4=0%40UOnlZ%2FcoxCrIUsehK6wqHaAmXMbE&nk4=0%40GwrkntVPltPB9cR46GncBta4rE%2Bd4zM%3D; __cn_logon__=true; __cn_logon_id__=zkg852266010; ali_apache_tracktmp=c_w_signed=Y; _nk_=zkg852266010; _csrf_token=1600299175342; x5sec=7b227365617263682d776562323b32223a226261366561643563366138666437303830373766613730333939326161383432434b664269767346455037313036544930504c7a42786f4d4d5441314d6a4d304e7a55304f447378227d; h_keys=\" % u6d17 % u8138 % u5dfe#%u673a%u68b0#%u5236%u9020\"; _is_show_loginId_change_block_=b2b-1052347548_false; _show_force_unbind_div_=b2b-1052347548_false; _show_sys_unbind_div_=b2b-1052347548_false; _show_user_unbind_div_=b2b-1052347548_false; __rn_alert__=false; ad_prefer=\"2020/09/17 07:33:35\"; isg=BLy8zW2pzy0ym_rX0NVsAFO6jVputWDfP9F6YpY9mKeKYV3rv8Bob2SXQYkZKZg3; l=eBOx3R_nQZOOFI08BOfwourza77O8IRfguPzaNbMiOCP9pCM5kOlWZr2gJYHCnGVHsCJJ3SxXm94B0YlkyC4_-ooeLIlglwq3dC..; tfstk=cKpABPspJ40cw7jJYIhlfAvFyEROZ0uOPo_gBDFB2MTaogeOiFpHpBvKGGaAe0C..";



       
        public ChromiumWebBrowser browser = new ChromiumWebBrowser("https://www.1688.com");
   
        




        private void 拼多多后台_Load(object sender, EventArgs e)
        {
            
            //var settings = new CefSettings();

            //settings.CachePath = "cache";

            //settings.CefCommandLineArgs.Add("proxy-server", "222.37.125.51:46603");

            //Cef.Initialize(settings);


            browser.LifeSpanHandler = new OpenPageSelf();   //设置在当前窗口打开
          //  browser.Load("https://login.1688.com/member/signin.htm");
            browser.Parent = splitContainer1.Panel2;
            browser.Dock = DockStyle.Fill;
            Control.CheckForIllegalCrossThreadCalls = false;




            // browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(run);
        }
        private void WB_DocumentCompleted(object sender, FrameLoadEndEventArgs e)
        {
         
            if (e.Url.ToString() != browser.Address.ToString())
                return;
            run();
            
            
        }


        bool status = false;
   
        public void run()

        {
            ArrayList urlList = new ArrayList();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("function tempFunction() {");
            //sb.AppendLine(" return document.body.innerHTML; "); 
            sb.AppendLine(" return document.getElementsByTagName('body')[0].outerHTML; ");
            sb.AppendLine("}");
            sb.AppendLine("tempFunction();");
            var task01 = browser.GetBrowser().GetFrame(browser.GetBrowser().GetFrameNames()[0]).EvaluateScriptAsync(sb.ToString());
            task01.ContinueWith(t =>
            {
            if (!t.IsFaulted)
            {
                var response = t.Result;
                if (response.Success == true)
                {
                    if (response.Result != null)
                    {
                        string resultStr = response.Result.ToString();
                      
                        MatchCollection ids = Regex.Matches(resultStr, @"<div class=""company-name"" title=""([\s\S]*?)""><a href=""([\s\S]*?)""");
                          
                                if (ids.Count == 0)
                                {
                                    status = false;
                                }

                                else
                                {
                                for (int j = 0; j < ids.Count; j++)
                                {
                                    string url = ids[j].Groups[2].Value;

                                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                                    lv1.SubItems.Add(url.Replace("?tracelog=p4p", ""));

                                    //if (!textBox1.Text.Contains(url))
                                    //{
                                    //    textBox1.Text += url + "\r\n";
                                    //}
                                }
                                status = true;
                                }
                           






                        }
                    }
                }
            });


            
        }
        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        Thread thread;
        private void button2_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"Wigbkff"))
            {

                return;
            }



            #endregion
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }



        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.expotTxt(listView1,1);
        }


        public void getrun()
        {
            string[] text = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string url in text)
            {
                string html = method.GetUrlWithCookie(url, cookie, "gbk");

                Match lxr = Regex.Match(html, @"class=""membername"" target=""_blank"">([\s\S]*?)</a>");

                Match tel = Regex.Match(html, @"移动电话：</dt>([\s\S]*?)</dd>");
               
                Match tel11= Regex.Match(tel.Groups[1].Value, @"\d{11}");
                if (tel11.Groups[0].Value!="")
                {
                    ListViewItem lv1 = listView1.Items.Add(lxr.Groups[1].Value); //使用Listview展示数据   

                    lv1.SubItems.Add(tel11.Groups[0].Value);
                }
                Thread.Sleep(1000);
            }
          
           

        }

       

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            textBox1.Text = "";
        }

  





       

        private void button5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://dgxtjx168.1688.com/page/contactinfo.htm?spm=a2615.2177701.autotrace-topNav.8.22a764d2iMidpp");
        }
    }


}
