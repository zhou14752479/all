using System;
using System.Collections;
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

namespace 通用项目
{
    public partial class 宝宝计划 : Form
    {
        public 宝宝计划()
        {
            InitializeComponent();
        }

        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }

        ArrayList list1 = new ArrayList();
        ArrayList list2 = new ArrayList();
        ArrayList list3 = new ArrayList();
        ArrayList list4 = new ArrayList();
        ArrayList list5 = new ArrayList();

        /// <summary>
        /// 牛仔计划
        /// </summary>
        public void niuzai()
        {
            
            try
            {
                string html = method.GetUrl("https://data.baobaojh8.com/getPlanContent.php?UA=2&id=237&sign=BF4A7FCACBC27DDD5B8EF7F6F421C5D9&token=NzI3OTU1LTI3OGFkOGQzYTU4ZjY3NDIyODVlMzg4NGVlYTZlOTRiLTE1ODE5MTM5NDM%3D", "utf-8");
               
                
                Match value1 = Regex.Match(html, @"""content"":""([\s\S]*?)----------------------------------------");
                string[] a1s = Unicode2String(value1.Groups[1].Value).Split(new string[] { "\\r\\n" }, StringSplitOptions.None);
               
                foreach (string a1 in a1s)
                {
                    string[] value = a1.Split(new string[] { " " }, StringSplitOptions.None);
                    if (value.Length > 4)
                    {
                        if (!list1.Contains(value[value.Length - 3]))
                        {
                            list1.Add(value[value.Length - 3]);
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                            lv1.SubItems.Add(value[value.Length - 4]);
                            lv1.SubItems.Add(value[value.Length - 3]);
                            lv1.SubItems.Add(value[value.Length - 2]);
                            lv1.SubItems.Add(value[value.Length - 1]);
                        }
                    }


                }
            

                Match value2 = Regex.Match(html, @"----------------------------------------([\s\S]*?)----------------------------------------");
                string[] a2s = Unicode2String(value2.Groups[1].Value).Split(new string[] { "\\r\\n" }, StringSplitOptions.None);

                foreach (string a2 in a2s)
                {
                    string[] value = a2.Split(new string[] { " " }, StringSplitOptions.None);
                    if (value.Length > 4)
                    {
                        if (!list2.Contains(value[value.Length - 3]))
                        {
                            list2.Add(value[value.Length - 3]);
                            ListViewItem lv1 = listView2.Items.Add((listView2.Items.Count).ToString()); //使用Listview展示数据   
                            lv1.SubItems.Add(value[value.Length - 4]);
                            lv1.SubItems.Add(value[value.Length - 3]);
                            lv1.SubItems.Add(value[value.Length - 2]);
                            lv1.SubItems.Add(value[value.Length - 1]);
                        }
                    }

                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


        }

        /// <summary>
        /// 五星计划
        /// </summary>
        public void wuxing()
        {

            try
            {
                string html = method.GetUrl("https://data.baobaojh8.com/getPlanContent.php?UA=2&id=255&sign=6656B0ED872D077429A4F68A68A40E91&token=NzI3OTU1LTI3OGFkOGQzYTU4ZjY3NDIyODVlMzg4NGVlYTZlOTRiLTE1ODE5MTM5NDM%3D", "utf-8");


                Match value1 = Regex.Match(html, @"""content"":""([\s\S]*?)----------------------------------------");
                string[] a1s = Unicode2String(value1.Groups[1].Value).Split(new string[] { "\\r\\n" }, StringSplitOptions.None);

                foreach (string a1 in a1s)
                {
                    string[] value = a1.Split(new string[] { " " }, StringSplitOptions.None);
                    if (value.Length > 5)
                    {
                        if (!list3.Contains(value[value.Length - 3]))
                        {
                            list3.Add(value[value.Length - 3]);
                            ListViewItem lv1 = listView3.Items.Add((listView3.Items.Count).ToString()); //使用Listview展示数据   
                            lv1.SubItems.Add(value[value.Length - 5]);
                            lv1.SubItems.Add(value[value.Length - 3]);
                            lv1.SubItems.Add(value[value.Length - 2]);
                            lv1.SubItems.Add(value[value.Length - 1]);
                        }
                    }

                }


                Match value2 = Regex.Match(html, @"----------------------------------------([\s\S]*?)----------------------------------------([\s\S]*?)----------------------------------------");
                string[] a2s = Unicode2String(value2.Groups[1].Value).Split(new string[] { "\\r\\n" }, StringSplitOptions.None);

                foreach (string a2 in a2s)
                {

                    string[] value = a2.Split(new string[] { " " }, StringSplitOptions.None);
                    if (value.Length > 5)
                    {
                        if (!list4.Contains(value[value.Length - 3]))
                        {
                            list4.Add(value[value.Length - 3]);
                            ListViewItem lv1 = listView4.Items.Add((listView4.Items.Count).ToString()); //使用Listview展示数据   
                            lv1.SubItems.Add(value[value.Length - 5]);
                            lv1.SubItems.Add(value[value.Length - 3]);
                            lv1.SubItems.Add(value[value.Length - 2]);
                            lv1.SubItems.Add(value[value.Length - 1]);
                        }
                    }

                }

                string[] a3s = Unicode2String(value2.Groups[2].Value).Split(new string[] { "\\r\\n" }, StringSplitOptions.None);

                foreach (string a3 in a3s)
                {
                    string[] value = a3.Split(new string[] { " " }, StringSplitOptions.None);
                    if (value.Length > 5)
                    {
                        if (!list5.Contains(value[value.Length - 3]))
                        {
                            list5.Add(value[value.Length - 3]);
                            ListViewItem lv1 = listView5.Items.Add((listView5.Items.Count).ToString()); //使用Listview展示数据   
                            lv1.SubItems.Add(value[value.Length - 5]);
                            lv1.SubItems.Add(value[value.Length - 3]);
                            lv1.SubItems.Add(value[value.Length - 2]);
                            lv1.SubItems.Add(value[value.Length - 1]);
                        }
                    }

                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


        }
        private void 宝宝计划_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            timer1.Stop();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            

            //string path = AppDomain.CurrentDomain.BaseDirectory+ DateTime.Now.ToString("m")+"-";
            //method.DataTableToExcelTime(method.listViewToDataTable(this.listView1), true,path+"1.xlsx");
            //method.DataTableToExcelTime(method.listViewToDataTable(this.listView2), true, path + "2.xlsx");
            //method.DataTableToExcelTime(method.listViewToDataTable(this.listView3), true, path + "3.xlsx");
            //method.DataTableToExcelTime(method.listViewToDataTable(this.listView4), true, path + "4.xlsx");
            //method.DataTableToExcelTime(method.listViewToDataTable(this.listView5), true, path + "5.xlsx");

        }

        /// <summary>
        /// 计算次数
        /// </summary>
        public void jisuan(ListView lv,string gua)

        {
            for (int i = 0; i < lv.Items.Count; i++)
            {

                if (i+1 < lv.Items.Count&& lv.Items[i].SubItems[4].Text.Trim() == gua && lv.Items[i+1].SubItems[4].Text.Trim() == "中" )
                {
                    lv.Items[i].SubItems[4].Text = "1";
                }
                if (i + 2 < lv.Items.Count && lv.Items[i].SubItems[4].Text.Trim() == gua && lv.Items[i + 1].SubItems[4].Text.Trim() == gua && lv.Items[i + 2].SubItems[4].Text.Trim() == "中")
                {
                    lv.Items[i].SubItems[4].Text = "1";
                    lv.Items[i+1].SubItems[4].Text = "2";
                }
                if (i + 3 < lv.Items.Count && lv.Items[i].SubItems[4].Text.Trim() == gua && lv.Items[i + 1].SubItems[4].Text.Trim() == gua && lv.Items[i + 2].SubItems[4].Text.Trim() == gua && lv.Items[i + 3].SubItems[4].Text.Trim() == "中")
                {
                    lv.Items[i].SubItems[4].Text = "1";
                    lv.Items[i + 1].SubItems[4].Text = "2";
                    lv.Items[i + 2].SubItems[4].Text = "3";
                }
                if (i +4 < lv.Items.Count && lv.Items[i].SubItems[4].Text.Trim() == gua && lv.Items[i + 1].SubItems[4].Text.Trim() == gua && lv.Items[i + 2].SubItems[4].Text.Trim() == gua && lv.Items[i + 3].SubItems[4].Text.Trim() == gua && lv.Items[i + 4].SubItems[4].Text.Trim() == "中")
                {
                    lv.Items[i].SubItems[4].Text = "1";
                    lv.Items[i + 1].SubItems[4].Text = "2";
                    lv.Items[i + 2].SubItems[4].Text = "3";
                    lv.Items[i + 3].SubItems[4].Text = "4";
                }
                if (i + 5< lv.Items.Count && lv.Items[i].SubItems[4].Text.Trim() == gua && lv.Items[i + 1].SubItems[4].Text.Trim() == gua && lv.Items[i + 2].SubItems[4].Text.Trim() == gua && lv.Items[i + 3].SubItems[4].Text.Trim() == gua && lv.Items[i + 4].SubItems[4].Text.Trim() == gua && lv.Items[i + 5].SubItems[4].Text.Trim() == "中")
                {
                    lv.Items[i].SubItems[4].Text = "1";
                    lv.Items[i + 1].SubItems[4].Text = "2";
                    lv.Items[i + 2].SubItems[4].Text = "3";
                    lv.Items[i + 3].SubItems[4].Text = "4";
                    lv.Items[i + 4].SubItems[4].Text = "5";
                }
                if (i + 6< lv.Items.Count && lv.Items[i].SubItems[4].Text.Trim() == gua && lv.Items[i + 1].SubItems[4].Text.Trim() == gua && lv.Items[i + 2].SubItems[4].Text.Trim() == gua && lv.Items[i + 3].SubItems[4].Text.Trim() == gua && lv.Items[i + 4].SubItems[4].Text.Trim() == gua && lv.Items[i + 5].SubItems[4].Text.Trim() == gua && lv.Items[i + 6].SubItems[4].Text.Trim() == "中")
                {
                    lv.Items[i].SubItems[4].Text = "1";
                    lv.Items[i + 1].SubItems[4].Text = "2";
                    lv.Items[i + 2].SubItems[4].Text = "3";
                    lv.Items[i + 3].SubItems[4].Text = "4";
                    lv.Items[i + 4].SubItems[4].Text = "5";
                    lv.Items[i + 5].SubItems[4].Text = "6";
                }


            }
        }


        public void tongji(ListView lv)

        {
            try
            {
                int yi = 0;
                int er = 0;
                int san = 0;
                int si = 0;
                int wu = 0;
                int liu = 0;
                for (int i = 0; i < lv.Items.Count; i++)
                {
                    if (lv.Items[i].SubItems[4].Text == "1")
                    {
                        yi = yi + 1;
                    }
                    if (lv.Items[i].SubItems[4].Text == "2")
                    {
                        er = er + 1;
                    }
                    if (lv.Items[i].SubItems[4].Text == "3")
                    {
                        san = san + 1;
                    }
                    if (lv.Items[i].SubItems[4].Text == "4")
                    {
                        si = si + 1;
                    }
                    if (lv.Items[i].SubItems[4].Text == "5")
                    {
                        wu = wu + 1;
                    }
                    if (lv.Items[i].SubItems[4].Text == "6")
                    {
                        liu = liu + 1;
                    }

                   

                }

                ListViewItem lv1 = lv.Items.Add((lv.Items.Count).ToString()); //使用Listview展示数据   
                lv1.SubItems.Add("1出现次数");
                lv1.SubItems.Add("2出现次数" );
                lv1.SubItems.Add("3出现次数");
                lv1.SubItems.Add("------------");

                ListViewItem lv11 = lv.Items.Add((lv.Items.Count).ToString()); //使用Listview展示数据   
                lv11.SubItems.Add(yi.ToString());
                lv11.SubItems.Add(er.ToString());
                lv11.SubItems.Add(san.ToString());

                lv11.SubItems.Add("------------");



                ListViewItem lv2 = lv.Items.Add((lv.Items.Count).ToString()); //使用Listview展示数据   
                lv2.SubItems.Add("4出现次数");
                lv2.SubItems.Add("5出现次数");
                lv2.SubItems.Add("6出现次数");
                lv2.SubItems.Add("------------");

                ListViewItem lv22 = lv.Items.Add((lv.Items.Count).ToString()); //使用Listview展示数据   
                lv22.SubItems.Add(si.ToString());
                lv22.SubItems.Add(wu.ToString());
                lv22.SubItems.Add(liu.ToString());

                lv22.SubItems.Add("------------");

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }
        public void buchong(ListView lv, string gua)
        {
            int min = 0;
            int max = 0;
            ArrayList lists = new ArrayList();
            for (int i = 0; i < lv.Items.Count; i++)
            {
                string itemValue = "";
                for (int j = 0; j < lv.Columns.Count; j++)
                {
                    itemValue += lv.Items[i].SubItems[j].Text.Trim()+",";


                }
                min = Convert.ToInt32(lv.Items[0].SubItems[2].Text.Replace("期", "").Trim());
                max = Convert.ToInt32(lv.Items[lv.Items.Count-1].SubItems[2].Text.Replace("期", "").Trim());
                lists.Add(itemValue);

            }

            lv.Items.Clear();
            for (int a = min; a < max+1; a++)
            {
                bool status = false;
                foreach (string item in lists)
                {
                    string[] text = item.Split(new string[] { "," }, StringSplitOptions.None);
                    if (text[2].Contains("期"))
                    {
                        if (Convert.ToInt32(text[2].Replace("期", "")) == a)
                        {
                            ListViewItem lv1 = lv.Items.Add((lv.Items.Count).ToString()); //使用Listview展示数据   
                            lv1.SubItems.Add(text[1]);
                            lv1.SubItems.Add(text[2]);
                            lv1.SubItems.Add(text[3]);
                            lv1.SubItems.Add(text[4]);
                            status = true;
                            break;
                        }
                    }
                  

                }
                if (status == false)
                {
                    ListViewItem lv1 = lv.Items.Add((lv.Items.Count).ToString()); //使用Listview展示数据   
                    lv1.SubItems.Add("-");
                    lv1.SubItems.Add("-");
                    lv1.SubItems.Add("-");
                    lv1.SubItems.Add(gua);
                }
               
               
            }




        }
            private void button5_Click(object sender, EventArgs e)
        {


              jisuan(listView1, "无");
            jisuan(listView2, "无");

            jisuan(listView3, "挂");
            jisuan(listView4, "挂");
            jisuan(listView5, "挂");


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            niuzai();
            wuxing();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            buchong(listView1, "无");
            buchong(listView2, "无");
            buchong(listView3, "挂");
            buchong(listView4, "挂");
            buchong(listView5, "挂");

        }

        private void button6_Click(object sender, EventArgs e)
        {
            tongji(listView1); tongji(listView2); tongji(listView3); tongji(listView4); tongji(listView5);
        }
    }
}
