
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
using System.Text.RegularExpressions;
using System.Threading;

namespace 中国智慧社区服务网
{
    public partial class 智慧社区服务网 : Form
    {
        public 智慧社区服务网()
        {
            InitializeComponent();
        }


        public string jiami(string data)
        {
            Sm4Crypto sm4 = new Sm4Crypto();
            //sm4.Data = "{\"syscode\":\""+ data + "\"}";
            sm4.Data = data;
            string value = Sm4Crypto.EncryptCBC(sm4);
            return value;
        }
        public string jiemi(string data)
        {
            Sm4Crypto sm4 = new Sm4Crypto();
            sm4.Data =data;

            string value = Sm4Crypto.DecryptCBC(sm4);
            return value;
        }

        string cookie = "HMF_CI=09b91a8370e62a6b53d08ac92f9b2c396ac93c8ff3d6a84d7bae2d94cf07b44b00; SF_cookie_25=31761544";
        public void run()
        {
            if(comboBox1.Text=="")
            {
                MessageBox.Show("请选择省份");
                return;
            }
            try
            {
                string url = "https://zqsq.mca.gov.cn/CAFP/restservices/web/cafp_siteSwitching/query";

                string encrypt_a = jiami(dics[comboBox1.Text]);
                string ahtml = method.PostUrlDefault(url, "bean=%7B%22encryData%22%3A%22" + System.Web.HttpUtility.UrlEncode(encrypt_a) + "%22%7D", cookie);
                MatchCollection cnnames_a = Regex.Matches(ahtml, @"""cnname"":""([\s\S]*?)""");
                MatchCollection syscodes_a = Regex.Matches(ahtml, @"""syscode"":""([\s\S]*?)""");

                for (int a= 0; a< cnnames_a.Count; a++) //遍历市
                {
                    
                    if(cnnames_a[a].Groups[1].Value== "农垦总局" || cnnames_a[a].Groups[1].Value == "森林工业总局")
                    {
                        continue;
                    }


                    string encrypt_b = jiami(syscodes_a[a].Groups[1].Value);
                    string bhtml = method.PostUrlDefault(url, "bean=%7B%22encryData%22%3A%22" + System.Web.HttpUtility.UrlEncode(encrypt_b) + "%22%7D", cookie);
                    MatchCollection cnnames_b = Regex.Matches(bhtml, @"""cnname"":""([\s\S]*?)""");
                    MatchCollection syscodes_b = Regex.Matches(bhtml, @"""syscode"":""([\s\S]*?)""");
                    for (int b= 0; b < cnnames_b.Count; b++)//遍历区
                    {
                        string encrypt_c = jiami(syscodes_b[b].Groups[1].Value);
                        string chtml = method.PostUrlDefault(url, "bean=%7B%22encryData%22%3A%22" + System.Web.HttpUtility.UrlEncode(encrypt_c) + "%22%7D", cookie);
                        MatchCollection cnnames_c = Regex.Matches(chtml, @"""cnname"":""([\s\S]*?)""");
                        MatchCollection syscodes_c= Regex.Matches(chtml, @"""syscode"":""([\s\S]*?)""");
                        for (int c =0; c < cnnames_c.Count; c++)//遍历镇
                        {
                            string encrypt_d = jiami(syscodes_c[c].Groups[1].Value);
                            string dhtml = method.PostUrlDefault(url, "bean=%7B%22encryData%22%3A%22" + System.Web.HttpUtility.UrlEncode(encrypt_d) + "%22%7D", cookie);
                            MatchCollection cnnames_d= Regex.Matches(dhtml, @"""cnname"":""([\s\S]*?)""");
                            MatchCollection syscodes_d = Regex.Matches(dhtml, @"""syscode"":""([\s\S]*?)""");
                           // MessageBox.Show(dhtml);


                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (status == false)
                                return;
                            if (cnnames_d.Count==0)
                            {
                                listdata(syscodes_c[c].Groups[1].Value, comboBox1.Text + cnnames_a[a].Groups[1].Value );
                                Thread.Sleep(100);

                            }
                            //MessageBox.Show(ahtml);
                            //MessageBox.Show(bhtml);
                            //MessageBox.Show(chtml);
                            // MessageBox.Show(dhtml);
                            label1.Text = DateTime.Now.ToString() + "： " + cnnames_a[a].Groups[1].Value+"-"+ cnnames_b[b].Groups[1].Value+"-"+ cnnames_c[c].Groups[1].Value;
                            for (int d = 0; d < cnnames_d.Count; d++)//遍历居委会
                            {
                                string url_detail = "https://zqsq.mca.gov.cn/CAFP/restservices/web/cafp_communityProfileList/query";
                                string encrypt_e = jiami(syscodes_d[d].Groups[1].Value);
                                string ehtml = method.PostUrlDefault(url_detail, "bean=%7B%22encryData%22%3A%22" + System.Web.HttpUtility.UrlEncode(encrypt_e) + "%22%7D", cookie);


                                MatchCollection cnnames_e = Regex.Matches(ehtml, @"""cnname"":""([\s\S]*?)""");
                                MatchCollection syscodes_e = Regex.Matches(ehtml, @"""syscode"":""([\s\S]*?)""");

                                listdata(syscodes_d[d].Groups[1].Value, comboBox1.Text + cnnames_a[a].Groups[1].Value);

                                Thread.Sleep(100);
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


            }
            catch (Exception ex)
            {

               MessageBox.Show(ex.ToString());
            }
        }


        public void run_card()
        {
            
            try
            {
                for (int a = 0; a < 10000; a++) //遍历市
                {

                    string url = "https://zqsq.mca.gov.cn/CAFP/restservices/web/checkUsername/query";
              
                string encrypt_a = jiami("{\"username\":\"33038219590825"+a.ToString("D4")+"\"}");
                    //string postdata = "bean=%7B%22encryData%22%3A%222PDxT4rG703QcK%2Bu0N8u498Op7%2BTAv5k8BbxM1yPJIOhKsQKZvIqNQYhTqYNdzuR%22%7D";
                   
                    string postdata = "bean=%7B%22encryData%22%3A%22" + System.Web.HttpUtility.UrlEncode(encrypt_a) + "%22%7D";
                string ahtml = method.PostUrlDefault(url,postdata , cookie);

                    string msg = Regex.Match(ahtml, @"""msg"":""([\s\S]*?)""").Groups[1].Value;

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据    
                    lv1.SubItems.Add(a.ToString());
                    lv1.SubItems.Add(msg);
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;

                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        public void listdata(string uid,string shengshi)
        {
            try
            {
                string url_detail = "https://zqsq.mca.gov.cn/CAFP/restservices/web/cafp_communityProfileList/query";
                string encrypt_e = jiami(uid);
                string ehtml = method.PostUrlDefault(url_detail, "bean=%7B%22encryData%22%3A%22" + System.Web.HttpUtility.UrlEncode(encrypt_e) + "%22%7D", cookie);

                // MessageBox.Show(ehtml);
                string cjname = Regex.Match(ehtml, @"""cjname"":""([\s\S]*?)""").Groups[1].Value;
                string name = Regex.Match(ehtml, @"""communityname"":""([\s\S]*?)""").Groups[1].Value;
                string legalperson = Regex.Match(ehtml, @"""legalperson"":""([\s\S]*?)""").Groups[1].Value;

                string tel = Regex.Match(ehtml, @"""acax595"":""([\s\S]*?)""").Groups[1].Value;
                string time = Regex.Match(ehtml, @"""acax586"":""([\s\S]*?)""").Groups[1].Value;
                if (name != "")
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据    
                    lv1.SubItems.Add(name);
                    lv1.SubItems.Add(shengshi);
                    lv1.SubItems.Add(cjname);
                    lv1.SubItems.Add(legalperson);
                    lv1.SubItems.Add(tel);
                    lv1.SubItems.Add(time);

                }
            }
            catch (Exception ex)
            {

              ex.ToString() ;
            }
        }

        Dictionary<string, string> dics = new Dictionary<string, string>();
        public void getprovince()
        {
            try
            {

                string url = "https://zqsq.mca.gov.cn/CAFP/restservices/web/cafp_siteSwitching/query";
                string html = method.PostUrlDefault(url, "bean=%7B%22encryData%22%3A%22LXpMdr7XOYB9AxMoi3qIguPBDg%2FIFGbp4hv2sjmUZyM%3D%22%7D", cookie);
                MatchCollection cnnames = Regex.Matches(html, @"""cnname"":""([\s\S]*?)""");
                MatchCollection syscodes = Regex.Matches(html, @"""syscode"":""([\s\S]*?)""");
               
                for (int i = 0; i < cnnames.Count; i++)
                {
                    if(!dics.ContainsKey(cnnames[i].Groups[1].Value))
                    {
                        comboBox1.Items.Add(cnnames[i].Groups[1].Value);
                        dics.Add(cnnames[i].Groups[1].Value, syscodes[i].Groups[1].Value);
                    }
                   

                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void 智慧社区服务网_Load(object sender, EventArgs e)
        {
            getprovince();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
        bool status = true;
        bool zanting = true;
        Thread thread;
        private void button7_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run_card);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(jiemi("2PDxT4rG703QcK+u0N8u498Op7+TAv5k8BbxM1yPJIOhKsQKZvIqNQYhTqYNdzuR"));
   
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
