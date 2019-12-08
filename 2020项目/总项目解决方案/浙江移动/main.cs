using System;
using System.Collections;
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
using MySql.Data.MySqlClient;

namespace 浙江移动
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            StreamReader sr = new StreamReader(path + "url.txt", Encoding.Default);
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (string urls in text)
            {
                string[] url = urls.Split(new string[] { "," }, StringSplitOptions.None);
                checkedListBox2.Items.Add(url[0]);
                checkedListBox3.Items.Add(url[1]);

            }
                sr.Close();
        }
        ArrayList finishes = new ArrayList();
        bool zanting = true;
        string cityId = "402881ea3286d488013286d756720002";
        ArrayList urllists = new ArrayList();


        #region  多个号码提取
        public void tiqu()
        {
            try

            {
                foreach (string  urls in urllists)
                {
                    if (!finishes.Contains(urls))
                    {
                        finishes.Add(urls);
                        string[] url = urls.Split(new string[] { "," }, StringSplitOptions.None);
                        string ahtml = method.GetUrl(url[1], "utf-8");
                        Match suiteId = Regex.Match(ahtml, @"suiteId"" value=""([\s\S]*?)""");

                    
                        for (int i = 0; i < 9999; i++)
                        {

                            string URL = "http://wap.zj.10086.cn/shop/shop/goods/contractNumber/queryIndex.do?cityid=402881ea3286d488013286d756720002&currentPageNum=" + i + "&span1=&span2=&span3=&span4=&span5=&fuzzySpan=&span6=&span7=&span8=&span9=&span10=&teleCodePer=&suiteId=" + suiteId.Groups[1].Value.Replace("prepayInfoId=", "").Trim() + "&priceRangeId=&baseFeeId=&numRuleId=&orderBy=&isNofour=N&pageCount=100";

                            string html = method.GetUrl(URL, "utf-8");




                            MatchCollection a1s = Regex.Matches(html, @"""teleCode"":([\s\S]*?),");  //手机号
                            MatchCollection a2s = Regex.Matches(html, @"""deposits"":([\s\S]*?),");  //预存
                            MatchCollection a3s = Regex.Matches(html, @"""rulePrice"":([\s\S]*?),");   //卡费
                            MatchCollection a4s = Regex.Matches(html, @"""ruleBaseFee"":([\s\S]*?),");  //保底
                            MatchCollection a5s = Regex.Matches(html, @"""inLen"":([\s\S]*?),");

                            if (a1s.Count == 0)
                                continue;


                            for (int j = 0; j < a1s.Count; j++)
                            {
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据      
                                lv1.SubItems.Add(a1s[j].Groups[1].Value.Replace("\"", ""));
                                lv1.SubItems.Add(a2s[j].Groups[1].Value.Replace("\"", ""));
                                lv1.SubItems.Add(a3s[j].Groups[1].Value.Replace("\"", ""));
                                lv1.SubItems.Add(a4s[j].Groups[1].Value.Replace("\"", ""));
                                lv1.SubItems.Add(a5s[j].Groups[1].Value.Replace("\"", ""));
                                lv1.SubItems.Add(url[0]);

                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }

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
        #endregion

        #region  模糊查询
        public void mohuchaxun()
        {
            if (checkBox1.Checked == true)
            {
                cityId = "Hangzhou";
            }
            else if (checkBox2.Checked == true)
            {
                cityId = "Ningbo";
            }
            else if (checkBox3.Checked == true)
            {
                cityId = "Wenzhou";
            }
            else if (checkBox4.Checked == true)
            {
                cityId = "Shaoxing";
            }
            else if (checkBox5.Checked == true)
            {
                cityId = "Quzhou";
            }
            else if (checkBox6.Checked == true)
            {
                cityId = "Jinhua";
            }
            else if (checkBox7.Checked == true)
            {
                cityId = "Zhoushan";
            }
            else if (checkBox8.Checked == true)
            {
                cityId = "Jiaxing";
            }
            else if (checkBox9.Checked == true)
            {
                cityId = "Huzhou";
            }
            else if (checkBox10.Checked == true)
            {
                cityId = "Lishui";
            }
            else if (checkBox11.Checked == true)
            {
                cityId = "Taizhou";
            }






            try
            {
                string a1 = textBox5.Text.Trim();
                string a2 = textBox6.Text.Trim();
                string a3 = textBox7.Text.Trim();
                string a4 = textBox8.Text.Trim();
                string a5 = textBox9.Text.Trim();
                string a6 = textBox10.Text.Trim();
                string a7 = textBox11.Text.Trim();
                string a8 = textBox12.Text.Trim();
                string a9 = textBox13.Text.Trim();
                string a10 = textBox14.Text.Trim();
                string a11 = textBox15.Text.Trim();
                string URL = "http://www.zj.10086.cn/shop/shop/sales/ajaxNumberInfos.do?citycd=" + cityId + "&numId=&priceRangeId=&numRuleId=&teleCodePer=&teleCode=" + a1+"A"+a2+"A"+a3+"A"+a4+"A"+a5+"A"+a6+"A"+a7+"A"+a8+"A"+a9+"A"+a10+"A"+a11+"A&suiteTypeId=&suiteDetailId=&proId=&isLoverTeleCode=&regionid=&officeId=&baseFeeId=&noFourNumber=false&stuTag=&orderBy=";

                string html = method.GetUrl(URL, "utf-8");

                MatchCollection a1s = Regex.Matches(html, @"<h6>([\s\S]*?)</h6>");  //手机号
                MatchCollection a2s = Regex.Matches(html, @"<u>([\s\S]*?)</u>");  //预存
              


                for (int j = 0; j < a1s.Count; j++)
                {
                    ListViewItem lv1 = listView2.Items.Add((listView2.Items.Count).ToString()); //使用Listview展示数据      
                    lv1.SubItems.Add(a1s[j].Groups[1].Value.Replace("\"", ""));
                    lv1.SubItems.Add(a2s[j].Groups[1].Value.Replace("\"", ""));
                  
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region  精确查询
        public void jingquechaxun()
        {
            if (checkBox1.Checked == true)
            {
                cityId = "Hangzhou";
            }
            else if (checkBox2.Checked == true)
            {
                cityId = "Ningbo";
            }
            else if (checkBox3.Checked == true)
            {
                cityId = "Wenzhou";
            }
            else if (checkBox4.Checked == true)
            {
                cityId = "Shaoxing";
            }
            else if (checkBox5.Checked == true)
            {
                cityId = "Quzhou";
            }
            else if (checkBox6.Checked == true)
            {
                cityId = "Jinhua";
            }
            else if (checkBox7.Checked == true)
            {
                cityId = "Zhoushan";
            }
            else if (checkBox8.Checked == true)
            {
                cityId = "Jiaxing";
            }
            else if (checkBox9.Checked == true)
            {
                cityId = "Huzhou";
            }
            else if (checkBox10.Checked == true)
            {
                cityId = "Lishui";
            }
            else if (checkBox11.Checked == true)
            {
                cityId = "Taizhou";
            }
            try
            {
                string URL = "http://www.zj.10086.cn/shop/shop/sales/ajaxNumberInfos.do?citycd="+cityId+"&numId=&priceRangeId=&numRuleId=&teleCodePer=&teleCode="+textBox1.Text.Trim()+"&suiteTypeId=&suiteDetailId=&proId=&isLoverTeleCode=&regionid=&officeId=&baseFeeId=&noFourNumber=false&stuTag=&orderBy=";

                string html = method.GetUrl(URL, "utf-8");

                MatchCollection a1s = Regex.Matches(html, @"<h6>([\s\S]*?)</h6>");  //手机号
                MatchCollection a2s = Regex.Matches(html, @"<u>([\s\S]*?)</u>");  //预存



                for (int j = 0; j < a1s.Count; j++)
                {
                    ListViewItem lv1 = listView2.Items.Add((listView2.Items.Count).ToString()); //使用Listview展示数据      
                    lv1.SubItems.Add(a1s[j].Groups[1].Value.Replace("\"", ""));
                    lv1.SubItems.Add(a2s[j].Groups[1].Value.Replace("\"", ""));

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion


        #region  查找
        public void chahzao()
        {
          
            try
            {
                string a1 = textBox3.Text.Trim();
                string a2 = textBox4.Text.Trim();
                string a3 = textBox16.Text.Trim();
                string a4 = textBox21.Text.Trim();
                string a5 = textBox22.Text.Trim();
                string a6 = textBox23.Text.Trim();
                string a7 = textBox24.Text.Trim();
                string a8 = textBox25.Text.Trim();
                string a9 = textBox26.Text.Trim();
                string a10 = textBox27.Text.Trim();
                foreach (string urls in urllists)
                {
                    string[] url = urls.Split(new string[] { "," }, StringSplitOptions.None);
                    string ahtml = method.GetUrl(url[1], "utf-8");
                    Match suiteId = Regex.Match(ahtml, @"suiteId"" value=""([\s\S]*?)""");
                    string URL = "http://wap.zj.10086.cn/shop/shop/goods/contractNumber/queryIndex.do?cityid=402881ea3286d488013286d756720002&span1=" + a1 + "&span2=" + a2 + "&span3=" + a3 + "&span4=" + a4 + "&span5=" + a5 + "&span6=" + a6 + "&span7=" + a7 + "&span8=" + a8 + "&span9=" + a9 + "&span10=" + a10 + "&suiteId="+ suiteId + "&fuzzySpan=&teleCodePer=&priceRangeId=&baseFeeId=&numRuleId=&orderBy=&isNofour=N&pageCount=100";

                    string html = method.GetUrl(URL, "utf-8");

                    MatchCollection a1s = Regex.Matches(html, @"""teleCode"":([\s\S]*?),");  //手机号
                    MatchCollection a2s = Regex.Matches(html, @"""deposits"":([\s\S]*?),");  //预存
                    MatchCollection a3s = Regex.Matches(html, @"""rulePrice"":([\s\S]*?),");   //卡费
                    MatchCollection a4s = Regex.Matches(html, @"""ruleBaseFee"":([\s\S]*?),");  //保底
                    MatchCollection a5s = Regex.Matches(html, @"""inLen"":([\s\S]*?),");


                    for (int j = 0; j < a1s.Count; j++)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据      
                        lv1.SubItems.Add(a1s[j].Groups[1].Value.Replace("\"", ""));
                        lv1.SubItems.Add(a2s[j].Groups[1].Value.Replace("\"", ""));
                        lv1.SubItems.Add(a3s[j].Groups[1].Value.Replace("\"", ""));
                        lv1.SubItems.Add(a4s[j].Groups[1].Value.Replace("\"", ""));
                        lv1.SubItems.Add(a5s[j].Groups[1].Value.Replace("\"", ""));
                        lv1.SubItems.Add("");

                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        private void Button2_Click(object sender, EventArgs e)
        {
            string constr = "Host =47.99.68.92;Database=vip_database;Username=root;Password=zhoukaige00.@*.";
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();

            MySqlCommand cmd = new MySqlCommand("select * from vip where username='浙江移动'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

            MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

            if (reader.Read())
            {

                string password = reader["password"].ToString().Trim();

                if (password != "浙江移动")

                {
                    MessageBox.Show("验证失败");

                    Environment.Exit(0);
                }


                for (int i = 0; i < 20; i++)
                {
                    Thread thread = new Thread(new ThreadStart(tiqu));
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }



        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

     

        private void Button9_Click(object sender, EventArgs e)
        {
            
        }

   

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

     

        private void Button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(mohuchaxun));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(jingquechaxun));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(chahzao));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void CheckedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void CheckedListBox2_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //string selectedItem = string.Empty;
            //for (int i = 0; i < checkedListBox1.Items.Count; i++)
            //{
            //    if (checkedListBox1.GetItemChecked(i))
            //    { selectedItem = selectedItem + " " + checkedListBox1.Items[i].ToString(); }
            //}

            urllists.Add(checkedListBox2.Items[e.Index].ToString()+","+checkedListBox3.Items[e.Index].ToString());
        }
    }
}
