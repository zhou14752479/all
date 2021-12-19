using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace CEF主程序
{
    public partial class 头条后台 : Form
    {
       


        #region   在自己窗口打开链接
        /// <summary>
        /// 在自己窗口打开链接
        /// </summary>
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

       


        #endregion
        public 头条后台()
        {
          
            InitializeComponent();
        }

        ChromiumWebBrowser browser=new ChromiumWebBrowser("https://mp.toutiao.com/profile_v4/index");
        ChromiumWebBrowser browser_zhuaqu = new ChromiumWebBrowser("https://mp.toutiao.com/profile_v4/index");
        //ChromiumWebBrowser browser_zhuaqu;
        private void 头条后台_Load(object sender, EventArgs e)
        {
            #region 通用检测

            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"yWWc"))
            {
                return;
            }

            #endregion
            tabPage2.Controls.Add(browser_zhuaqu);
            browser_zhuaqu.Dock = DockStyle.Fill;
            browser_zhuaqu.LifeSpanHandler = new OpenPageSelf();
            browser_zhuaqu.FrameLoadEnd += Browser_FrameLoadEnd; //调用加载事件
            Control.CheckForIllegalCrossThreadCalls = false;





            browser.LifeSpanHandler = new OpenPageSelf();
            Control.CheckForIllegalCrossThreadCalls = false;
            panel2.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
        }

        private void button1_Click(object sender, EventArgs e)
        {



            //账号详情页
            browser.GetBrowser().MainFrame.EvaluateScriptAsync("var element = document.getElementsByClassName('auth-avator-name');element[0].textContent = '" + textBox1.Text.Trim() + "'; ");
            browser.GetBrowser().MainFrame.EvaluateScriptAsync("var element = document.getElementsByClassName('content');element[0].textContent = '" + textBox1.Text.Trim() + "'; ");
            browser.GetBrowser().MainFrame.EvaluateScriptAsync("var element = document.getElementsByClassName('content');element[1].textContent = '" + textBox2.Text.Trim() + "'; ");

            //头像
            browser.GetBrowser().MainFrame.EvaluateScriptAsync("var element = document.getElementsByClassName('imagewrapper-image-url')[0];element.src =  '" + touxiangtxt.Text.Trim() + "'; ");

            browser.GetBrowser().MainFrame.EvaluateScriptAsync("var element = document.getElementsByClassName('content');element[3].textContent = '" + textBox3.Text.Trim() + "'; ");
            browser.GetBrowser().MainFrame.EvaluateScriptAsync("var element = document.getElementsByClassName('content');element[4].textContent = '" + textBox4.Text.Trim() + "'; ");
            //二维码
            browser.GetBrowser().MainFrame.EvaluateScriptAsync("var element = document.getElementsByClassName('qrcodewrapper-img')[0];element.src =  '" + erweimatxt.Text.Trim() + "'; ");

            browser.GetBrowser().MainFrame.EvaluateScriptAsync("var element = document.getElementsByClassName('content');element[6].textContent = '" + textBox5.Text.Trim() + "'; ");
            browser.GetBrowser().MainFrame.EvaluateScriptAsync("var element = document.getElementsByClassName('content');element[7].textContent = '" + textBox6.Text.Trim() + "'; ");
            browser.GetBrowser().MainFrame.EvaluateScriptAsync("var element = document.getElementsByClassName('content');element[8].textContent = '" + textBox7.Text.Trim() + "'; ");
            //账号详情页结束




            //主页
            browser.GetBrowser().MainFrame.EvaluateScriptAsync("var element = document.getElementsByClassName('data-board-item-primary');element[0].textContent = '"+textBox8.Text.Trim()+"'; ");
            browser.GetBrowser().MainFrame.EvaluateScriptAsync("var element = document.getElementsByClassName('data-board-item-primary');element[1].textContent = '" + textBox9.Text.Trim() + "'; ");
            browser.GetBrowser().MainFrame.EvaluateScriptAsync("var element = document.getElementsByClassName('data-board-item-primary');element[2].textContent = '" + textBox10.Text.Trim() + "'; ");
            //主页结束


            //作品页


            for (int i = 0; i <dics.Keys.Count; i++)
            {
              
                browser.GetBrowser().MainFrame.EvaluateScriptAsync("var element = document.getElementsByClassName('title')["+i+ "];element.textContent =  '" + dics.ElementAt(i).Key+ "'; ");
                browser.GetBrowser().MainFrame.EvaluateScriptAsync("var element = document.getElementsByClassName('cover-img')["+i+"];element.src =  '" + dics.ElementAt(i).Value + "'; ");
                browser.GetBrowser().MainFrame.EvaluateScriptAsync("var element = document.getElementsByClassName('time-info')[" + i + "];element.textContent =  '" + DateTime.Now.AddDays(i).ToString("MM:dd HH:mm")+ "'; ");
                browser.GetBrowser().MainFrame.EvaluateScriptAsync("var element = document.getElementsByByTagNam('li')[" + i + "];element.textContent =  '" + i+ "'; ");
            }
          
           
            //作品页结束
        }


        Dictionary<string, string> dics = new Dictionary<string, string>();

        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("function tempFunction() {");
            //sb.AppendLine(" return document.body.innerHTML; "); 
            sb.AppendLine(" return document.getElementsByTagName('body')[0].outerHTML; ");
            sb.AppendLine("}");
            sb.AppendLine("tempFunction();");
            var task01 = browser_zhuaqu.GetBrowser().GetFrame(browser.GetBrowser().GetFrameNames()[0]).EvaluateScriptAsync(sb.ToString());
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
                            MatchCollection nums = Regex.Matches(resultStr, @"<span class=""num"">([\s\S]*?)</span>");

                            MatchCollection titles = Regex.Matches(resultStr, @"<h2>([\s\S]*?)</h2>");
                            MatchCollection covers = Regex.Matches(resultStr, @"<div class=""cover""([\s\S]*?)https:([\s\S]*?)\)");
                            if (titles.Count==0)
                            {
                                titles = Regex.Matches(resultStr, @"<h2 title=""([\s\S]*?)""");
                                covers = Regex.Matches(resultStr, @"<div class=""feed-card-cover""([\s\S]*?)<img src=""([\s\S]*?)""");
                            }

                            //textBox11.Text = resultStr;
                           
                            try
                            {
                                if (titles.Count > 0)
                                {
                                    dics.Clear();
                                    textBox1.Text = Regex.Match(resultStr, @"<span class=""name"">([\s\S]*?)</span>").Groups[1].Value.Trim();
                                    textBox2.Text = Regex.Match(resultStr, @"简介：</span>([\s\S]*?)</p>").Groups[1].Value.Trim();

                                    touxiangtxt.Text = Regex.Match(resultStr, @"<div class=""ttp-avatar auth-0 auth"">([\s\S]*?)src=""([\s\S]*?)""").Groups[2].Value.Trim();


                                    textBox8.Text = Regex.Replace(nums[0].Groups[1].Value, "<[^>]+>", "");
                                    textBox9.Text = Regex.Replace(nums[1].Groups[1].Value, "<[^>]+>", "");
                                    textBox10.Text = Regex.Replace(nums[2].Groups[1].Value, "<[^>]+>", "");

                                
                                    for (int i = 0; i < titles.Count; i++)
                                    {
                                       
                                        dics.Add(titles[i].Groups[1].Value, "https:" + covers[i].Groups[2].Value.Replace("&quot;",""));
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                                
                            }
                         
                        }
                    }
                }
            });
            label13.Text = "抓取完毕";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            label13.Text = "正在抓取......";
            browser_zhuaqu.Load(linktxt.Text);
        
          


        }
    }
}
