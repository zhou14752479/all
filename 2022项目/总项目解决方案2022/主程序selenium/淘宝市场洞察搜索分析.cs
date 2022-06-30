using OpenQA.Selenium;
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
using myDLL;

namespace 主程序selenium
{
    public partial class 淘宝市场洞察搜索分析 : Form
    {
        public 淘宝市场洞察搜索分析()
        {
            InitializeComponent();
        }
        Thread thread;
        private void button2_Click(object sender, EventArgs e)
        {
            status = true;
            if (textBox1.Text == "")
            {
                MessageBox.Show("请导入链接");
                return;
            }
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }


        IWebDriver driver;
        public void run()
        {

            StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == "")
                    continue;
          

            driver = function.getdriver(false, false);

                //string cookies = "thw=cn; hng=CN%7Czh-CN%7CCNY%7C156; _uetvid=993f4bd0d06311ec82d03d81dfb951ad; _ga=GA1.2.1176637355.1652188673; _ga_YFVFB9JLVB=GS1.1.1653461705.3.1.1653461716.0; cc_gray=1; ariaDefaultTheme=undefined; cna=me/qGmJS/VYCAXniuY+bEuiw; mt=ci=-1_0; lgc=zkg852266010; tracknick=zkg852266010; enc=RPrG9Y%2FvqhZaF5a5rvDVIGKEBtOkQASIROxtiBNFEfNXOgriHLiN5bMkTtmUMEzxzSiLVKFZpVDlp%2BM82Iiu3g%3D%3D; xlly_s=1; t=27fc2cb87d769485c71bd38d9dbbcd44; _tb_token_=W8kzgFnMsrh4Fo4jLJ8a; _samesite_flag_=true; cookie2=215092d9563adbacdce5fcddd8466e3d; cancelledSubSites=empty; dnk=zkg852266010; _m_h5_tk=b25a1563665be5cff9b9a61e7d8044aa_1656310233775; _m_h5_tk_enc=dd2ee6c01e52d566517a392603f5460f; v=0; XSRF-TOKEN=411308c1-5ea8-4b12-ab44-333d9ab2f75f; _euacm_ac_l_uid_=1052347548; 1052347548_euacm_ac_c_uid_=1052347548; 1052347548_euacm_ac_rs_uid_=1052347548; sgcookie=E100yo3ZpM3S8luvX%2FhWseLdMrCEz2CHnacSgYvXv32tX4HaiYPh4hFBPTFsVcR59YLz%2Br000y3uaWY6pKRSpbAgvaxgDqURUh0HpDT7tO1R7qcCB1iwn1aR4%2B23BmFZvAUV; unb=1052347548; uc1=cookie15=URm48syIIVrSKA%3D%3D&cookie14=UoexNDhEr2mtrw%3D%3D&pas=0&cookie16=UIHiLt3xCS3yM2h4eKHS9lpEOw%3D%3D&cookie21=URm48syIZJfmZ9wVCtpzEQ%3D%3D&existShop=true; uc3=lg2=U%2BGCWk%2F75gdr5Q%3D%3D&nk2=GcOvCmiKUSBXqZNU&id2=UoH62EAv27BqSg%3D%3D&vt3=F8dCvCIZx5DpmV2qwRU%3D; csg=eade09ff; cookie17=UoH62EAv27BqSg%3D%3D; skt=8d452b304fcbce32; existShop=MTY1NjMwOTE4Ng%3D%3D; uc4=id4=0%40UOnlZ%2FcoxCrIUsehK6kKrm4spu6w&nk4=0%40GwrkntVPltPB9cR46GncA5Lzk4vQfLE%3D; _cc_=URm48syIZQ%3D%3D; _l_g_=Ug%3D%3D; sg=080; _nk_=zkg852266010; cookie1=Vvj8uMJubtxirKFtxaDmWPxYCP5sb7EKtrFe1w68JDk%3D; _portal_version_=new; _euacm_ac_rs_sid_=null; JSESSIONID=E9993C1D82A3A553D0CD8442F90865EE; tfstk=cVCPBV2En7FzoePABQdUPscgPHPfaZsh2j-2Elk8Q1-sxl9kbsbtvExWyE-ruDvl.; l=fBjtFcecLGa6WQOXBO5Cnurza77TgQObzsPzaNbMiIeca1ypeF9c5NCh5ee5DdtjgT52GeKyhayveRnBkzzLRxMpdniWU7R7kj9w8eM3N7AN.; isg=BO_v68bj0d-IwNXu7kci86isfgP5lEO2cY94aQF_kt56UA1SCGIBBn1G0kDuKBsu";
                
                string cookies = textBox2.Text.Trim();
                
                string url = "https://sycm.taobao.com/mc/mq/search_analyze?activeKey=relation&dateRange=2022-06-24%7C2022-06-24&dateType=day&keyword="+ System.Web.HttpUtility.UrlEncode(text[i]) + "&spm=a21ag.11815228.LeftMenu.d588.671a50a5ZznhG1";



              
                driver.Navigate().GoToUrl(url);

              

                string[] cookieall = cookies.Split(new string[] { ";" }, StringSplitOptions.None);

            foreach (var item in cookieall)
            {
                string[] cookie = item.Split(new string[] { "=" }, StringSplitOptions.None);
                if (cookie.Length > 1)
                {
                    Cookie cook = new Cookie(cookie[0].Trim(), cookie[1].Trim(), "", DateTime.Now.AddDays(1));
                    driver.Manage().Cookies.AddCookie(cook);
                }


            }

            driver.Navigate().GoToUrl(url);
             
                Thread.Sleep(1000);
           
           
            try
            {
               

                driver.FindElements(By.TagName("button"))[2].Click(); //30天
                Thread.Sleep(1000);
                driver.FindElements(By.TagName("button"))[3].Click();  //日
                Thread.Sleep(1000);


               
                driver.FindElements(By.TagName("input"))[4].Click();
                driver.FindElements(By.TagName("input"))[5].Click();
                driver.FindElements(By.TagName("input"))[6].Click();
                driver.FindElements(By.TagName("input"))[7].Click();

      
                driver.FindElements(By.TagName("input"))[9].Click();
                driver.FindElements(By.TagName("input"))[10].Click();
                driver.FindElements(By.TagName("input"))[11].Click();
                driver.FindElements(By.TagName("input"))[12].Click();

            }
            catch (Exception ex)
            {

               MessageBox.Show(ex.ToString());
            }
            Thread.Sleep(1000);
                while(true)
                {
                    if (status == false)
                        return;

                    try
                    {
                        string html = driver.PageSource;

                        MatchCollection names = Regex.Matches(html, @"<span class=""ant-table-row-indent indent-level-0"" style=""padding-left: 0px;""></span>([\s\S]*?)</td>");
                        MatchCollection values = Regex.Matches(html, @"<span class=""alife-dt-card-common-table-sortable-value""><span>([\s\S]*?)</span>");

                        for (int j = 0; j < names.Count; j++)
                        {
                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    
                            lv1.SubItems.Add(names[j].Groups[1].Value);
                            lv1.SubItems.Add(values[(5 * j)].Groups[1].Value);
                            lv1.SubItems.Add(values[(5 * j) + 1].Groups[1].Value);
                            lv1.SubItems.Add(values[(5 * j) + 2].Groups[1].Value);
                            lv1.SubItems.Add(values[(5 * j) + 3].Groups[1].Value);
                            lv1.SubItems.Add(values[(5 * j) + 4].Groups[1].Value);

                        }

                        
                        foreach (IWebElement item in driver.FindElements(By.TagName("li")))
                        {
                            if (item.GetAttribute("title") == "下一页")
                            {
                                item.Click();
                                if(item.GetAttribute("aria-disabled") =="true")  //最后一页禁止点击
                                {

                                     html = driver.PageSource;

                                     names = Regex.Matches(html, @"<span class=""ant-table-row-indent indent-level-0"" style=""padding-left: 0px;""></span>([\s\S]*?)</td>");
                                     values = Regex.Matches(html, @"<span class=""alife-dt-card-common-table-sortable-value""><span>([\s\S]*?)</span>");

                                    for (int j = 0; j < names.Count; j++)
                                    {
                                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据

                                        lv1.SubItems.Add(names[j].Groups[1].Value);
                                        lv1.SubItems.Add(values[(5 * j)].Groups[1].Value);
                                        lv1.SubItems.Add(values[(5 * j) + 1].Groups[1].Value);
                                        lv1.SubItems.Add(values[(5 * j) + 2].Groups[1].Value);
                                        lv1.SubItems.Add(values[(5 * j) + 3].Groups[1].Value);
                                        lv1.SubItems.Add(values[(5 * j) + 4].Groups[1].Value);

                                    }
                                    return;
                                }
                            }
                        }

                        Thread.Sleep(500);
                    }
                    catch (Exception)
                    {

                       break;
                    }


                }




                //IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                //js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");

            }

        }

    private void button1_Click(object sender, EventArgs e)
    {
        if (openFileDialog1.ShowDialog() == DialogResult.OK)
        {
            textBox1.Text = openFileDialog1.FileName;
        }
    }

        private void 淘宝市场洞察搜索分析_Load(object sender, EventArgs e)
        {
            #region 通用检测


            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"73N0"))
            {
              
                System.Diagnostics.Process.GetCurrentProcess().Kill();

                return;
            }

            #endregion
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
        bool status = true;
        private void button5_Click(object sender, EventArgs e)
        {
            status = false;
        }
    }
}
