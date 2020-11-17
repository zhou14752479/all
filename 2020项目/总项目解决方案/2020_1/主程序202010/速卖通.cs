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
using helper;
using static helper.method;

namespace 主程序202010
{
    public partial class 速卖通 : Form
    {

        public 速卖通()
        {
            InitializeComponent();
        }
        bool zanting = true;
        string cookie = "cna=b2RCFw6lRXICAXnq99rJ24nX; aep_common_f=cMK/QoBxhjAIhqaPSJy3lv3pbAA8Th8nw6cvuAzYdtIeYHuKVNY9kg==; ali_apache_id=11.134.216.22.1602414324111.441773.3; _bl_uid=q0k3kgpt5Fy054488n8m30R8348C; _ga=GA1.2.1792401395.1602414347; aeu_cid=11cc1b8136ae4d2288d370deb889a6d7-1602414606578-03097-_ePNSNV; acs_usuc_t=x_csrf=4623l33kl_k&acs_rt=b3645d18681842f69bb8c95f4b38a4e9; xlly_s=1; intl_locale=en_US; _gid=GA1.2.2137405277.1602836596; havana_tgc=eyJjcmVhdGVUaW1lIjoxNjAyODM3MTk5MzExLCJsYW5nIjoiZW5fVVMiLCJwYXRpYWxUZ2MiOnsiYWNjSW5mb3MiOnsiMTMiOnsiYWNjZXNzVHlwZSI6MSwibWVtYmVySWQiOjIyMDczNTk2MDY1NTQsInRndElkIjoiMTVtZDRKbkU2RkhsNWZXZFZJQS1tTmcifX19fQ; _hvn_login=13; xman_us_t=x_lid=cn1529941853uqyo&sign=y&rmb_pp=trc72910@163.com&x_user=/jV48vHrU6ECFkcJt0UwKZpIGk3AV8sg4PkzlT6X7KY=&ctoken=14jnnfyil947w&need_popup=y&l_source=aliexpress; aep_usuc_t=ber_l=A0; xman_f=ow3bz/lzPvqO0gTRWCtKXu11Lk1100dSIiFWmCTuOUa4JVe0E5U05FirORLEl46s3d/MZvSeHLfxED6MFpeBMBsyrSMcKbUAUOkK4wbLvYcSwi4fH1YEeuGI8ZKpJag5pQcBTmpZFHDMYCo9AZnFv9SdzNqs262mMmlAP7aoNhAeiHuHYdgwOSOUNczP0BPCVhRP8XYz6pFi7I0eEFYAJNExSP3eWw5VTSMTA4qXBdTHPz83KERxJNYebHd9Wn+EdrkWnMzsXvd33ihg/+kEegZbIEDgE7VLeV6xAo7H9CARsx31l35+TTDWbu/tOJLqrkrdkqF0UEj0IGbA0OefcG4SSjW0U8RcoX/BB7BEiYNYZ5Xb73J2dMPZKlcjfzs8I2nEyclkTX+KJTpsdMsiu1CTZm4uZZFpAmhbFgwRjpU=; aep_usuc_f=isfm=y&site=glo&c_tp=USD&x_alimid=240118837&iss=y&s_locale=zh_CN&region=CN&b_locale=en_US; ali_apache_tracktmp=W_signed=Y; XSRF-TOKEN=0849a876-932c-48ce-a74d-87523821b21b; ali_apache_track=ms=|mid=cn1529941853uqyo; x5sec=7b2261652d676c6f7365617263682d7765623b32223a223838366130663636373666323162623430353164366434326439343166363030434d653270667746454f5453316336577949627362526f4c4d6a51774d5445344f444d334f7a453d227d; aep_history=keywords%5E%0Akeywords%09%0A%0Aproduct_selloffer%5E%0Aproduct_selloffer%0933012791792%091005001481662213%094001264000858%0932886749876%094000147215784%094001022183590%091005001382937962%094001181175096; _m_h5_tk=f45c6afac0d4d5fde66c879fd9593b6d_1602841510317; _m_h5_tk_enc=0846625ca719ece7d65a8e06960e25e2; intl_common_forever=YTjdFxWCb2PUr6fDHpjVwbafdFXalyfWipBD9twWBxRdH+qQdweBbQ==; JSESSIONID=7FE2B0BE9EFE39AE82C7922025B6807D; xman_us_f=x_lid=cn1529941853uqyo&x_l=1&x_locale=en_US&no_popup_today=n&x_c_chg=0&x_user=CN|null|null|cnfm|240118837&zero_order=y&acs_rt=2366c773b97d4b81a6c4c958e6d296db&last_popup_time=1591700469403&x_as_i=%7B%22aeuCID%22%3A%2211cc1b8136ae4d2288d370deb889a6d7-1602414606578-03097-_ePNSNV%22%2C%22affiliateKey%22%3A%22_ePNSNV%22%2C%22channel%22%3A%22AFFILIATE%22%2C%22cv%22%3A%221%22%2C%22isCookieCache%22%3A%22N%22%2C%22ms%22%3A%221%22%2C%22pid%22%3A%22177275576%22%2C%22tagtime%22%3A1602414606578%7D; xman_t=AJUu0YhD4CgZpeLvhnKyYJgvmNOz5f7S8M2q1/t1UYEZYtOEonyKqRqUv2wv5YLg2VVtJbzy0rGsHbQmgPFrOIM98IrG81CNrDYs0dO/nqE1daDV43raHIMQLciwIVj55KUnA0zKAFLNMnvIxz31ZeJJNsaTu08nlsPGOYdBJK2ugk8fVFEq6PTY8BhVUuyhhk0cNYSbRw/JGNRko2Pm+BQU7jHnL16grDlFGGIekhbUizuJ5zVUGnetH5/1n5Gqnu9P9al4kfg1wrh0eSVFpH3zbEzzhjiUn38s9zjMxewCTFXmrn6lJbW8VariY0u7vwmZsijqfCDuhxV+PdQDf3lV7nRbPyC53unJJG753gekS9YluhB3kpJcfqFDVH0KQBGzVCPQB0Q9++GHQSzKe7wGTbL6VMQWXDFWdo65JlY4Fk4eOkCluqWeRn8E/q0LXZS2cZ8K9z3B0ZEZllMETy+h/gctrdRAZv1OmpScfFYauVJkm3PRMClwVaORzcLuEcxsppLsd/QW9VDGYIZJMdtj0aQ+gZs5oPeN6HUmlMJ4B82CTFEHt3UPx9zk5lctAc3HTVItfNYBPGK6cMVFCPyqi2ds26Z7BhhCp5r2t8rCB20zzTmZ/1LJEZx08V4qZoV6PgTZh2g=; _gat=1; tfstk=c8tFBNgqjDnFsMjSkMszR_xGNKCdZRJHElWf-U-nAxv0gNbhiUPRIvYcQT1xjwf..; isg=BGJi2mQXSTQqCVQ-t6o3eoSus-jEs2bNxT9UMKz7hVWAfwP5lUdT3bG9r7uD795l; l=eBxYxvPqQ9vitLTzBOfZourza77TjIRfguPzaNbMiOCP_3Ce5AblWZ56jiYwCnGVHsI9J3SxXm94BA8R-yCqNudMyXb7Yuis3dC..";
        public string getcatename(string id)
        {
            string url = "https://www.aliexpress.com/af/category/"+id+".html";
            string html = method.GetUrlWithCookie(url, cookie, "utf-8");
            Match catename = Regex.Match(html, @"""catName"":""([\s\S]*?)""", RegexOptions.IgnoreCase);
            return catename.Groups[1].Value;
            
        }
        public void run()
        {
            int page = Convert.ToInt32(textBox6.Text);
        
            string[] array = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int a = 0; a < array.Length; a++)
            {
                string keyword = array[a].ToString();
                string catename = "";
                for (int i = 1; i <=page ; i++)
                {
                    cookie = cookieBrowser.cookie;
                    string url = "https://www.aliexpress.com/af/" + System.Web.HttpUtility.UrlEncode(keyword)+".html?trafficChannel=af&d=y&ltype=affiliate&SortType=default&page="+a+ "&CatId=0&minPrice="+textBox2.Text.Trim()+"&maxPrice="+textBox3.Text.Trim();
                    string html = method.GetUrlWithCookie(url,cookie,"utf-8");
                   
                    if (html.Contains("verify"))
                    {
                        MessageBox.Show("请在打开的网页中拖拉验证");
                        System.Diagnostics.Process.Start(url);
                    }
                    MatchCollection ids = Regex.Matches(html, @"""productid"":([\s\S]*?),", RegexOptions.IgnoreCase);
                    MatchCollection titles = Regex.Matches(html, @"""title"":([\s\S]*?)saleUnit", RegexOptions.IgnoreCase);
                    MatchCollection prices = Regex.Matches(html, @"},""price"":""([\s\S]*?)""", RegexOptions.IgnoreCase);
                    MatchCollection cateids = Regex.Matches(html, @"""displayCategoryId"":""([\s\S]*?)""", RegexOptions.IgnoreCase);
                    if(cateids.Count>0 &&i==1)
                    {
                         catename = getcatename(cateids[0].Groups[1].Value);
                    }
                        
                   
                    
                    for (int j = 0; j < ids.Count; j++)
                    {
                       
                        Match title = Regex.Match(titles[j].Groups[1].Value, @"""([\s\S]*?)""", RegexOptions.IgnoreCase);
                        Match sold = Regex.Match(titles[j].Groups[1].Value, @"""tradeDesc"":""([\s\S]*?)Sold", RegexOptions.IgnoreCase);

                        string yuanp = prices[j].Groups[1].Value.Replace("US", "").Replace("$", "");
                        string minp = yuanp;
                        string maxp = yuanp;

                        string[] price= yuanp.Split(new string[] { "-" }, StringSplitOptions.None);
                        if (price.Length > 1)
                        {
                            minp = price[0].Trim();
                            maxp= price[1].Trim();
                        }
                        if (sold.Groups[1].Value != "")
                        {
                            if (Convert.ToInt32(sold.Groups[1].Value) >= Convert.ToInt32(textBox4.Text) && Convert.ToInt32(sold.Groups[1].Value) <= Convert.ToInt32(textBox5.Text))
                            {
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(title.Groups[1].Value);
                                lv1.SubItems.Add("https://www.aliexpress.com/item/" + ids[j].Groups[1].Value + ".html");
                                lv1.SubItems.Add(minp);
                                lv1.SubItems.Add(maxp);
                                lv1.SubItems.Add(sold.Groups[1].Value);
                                lv1.SubItems.Add(catename);
                                lv1.SubItems.Add(keyword);


                            }
                        }
                        else
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(title.Groups[1].Value);
                            lv1.SubItems.Add("https://www.aliexpress.com/item/" + ids[j].Groups[1].Value + ".html");
                            lv1.SubItems.Add(minp);
                            lv1.SubItems.Add(maxp);
                            lv1.SubItems.Add(sold.Groups[1].Value);
                             lv1.SubItems.Add(cateids[j].Groups[1].Value);

                            
                        }
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;

                    }
                    Thread.Sleep(1000);



                }
            }

        }
        bool status = true;
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"sumaitong"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
              
                StreamReader sr = new StreamReader(this.openFileDialog1.FileName, EncodingType.GetTxtType(this.openFileDialog1.FileName));
                //一次性读取完 
                string texts = sr.ReadToEnd();

                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {

                    textBox1.Text += text[i] + "\r\n";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            cookieBrowser cb = new cookieBrowser("https://www.aliexpress.com/af/wedges+shoes.html?trafficChannel=af&d=y&ltype=affiliate&SortType=default&page=0");
            cb.Show();
        }

        private void 速卖通_Load(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            status = false;
        }
    }
}
