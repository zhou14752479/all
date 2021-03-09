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
using myDLL;

namespace 同城58
{
    public partial class 商铺 : Form
    {
        public 商铺()
        {
            InitializeComponent();
        }
        public List<T> RandomSortList<T>(List<T> ListT)
        {
            Random random = new Random();
            List<T> newList = new List<T>();
            foreach (T item in ListT)
            {
                newList.Insert(random.Next(newList.Count + 1), item);
            }
            return newList;
        }
        public List<string> RandomSort(List<string> list)
        {

            List<string> lists = RandomSortList<string>(list);
            return lists;
        }


        public List<string> getCitys()
        {

            List<string> list = new List<string>();
           
            string url = "http://www.zupuk.com/";
            string html = method.GetUrl(url, "utf-8");
          
            Match ahtml = Regex.Match(html, @"<div class=""part_div3"">([\s\S]*?)</div><br />");
           
            MatchCollection citys = Regex.Matches(ahtml.Groups[1].Value, @"href=""http:\/\/([\s\S]*?)\.");
            for (int i = 0; i < citys.Count; i++)
            {

                list.Add(citys[i].Groups[1].Value);

            }
            return list;
        }


        public List<string> baixinggetCitys()
        {

            List<string> list = new List<string>();

            string url = "https://www.baixing.com/?changeLocation=yes&return=%2Fjingyingzhuanrang%2F%3FsortKey%3DcreatedTime";
            string html = method.GetUrl(url, "utf-8");

         

            MatchCollection citys = Regex.Matches(html, @"data-city='([\s\S]*?)'");
            for (int i = 0; i < citys.Count; i++)
            {

                list.Add(citys[i].Groups[1].Value);

            }
            return list;
        }
        /// <summary>
        /// 租铺客
        /// </summary>
        public void run()
        {
            try
            {
               
                List<string> citys=  RandomSort(getCitys());
                foreach (string city in citys)
                {
                  

                    textBox3.Text += DateTime.Now.ToShortTimeString() + "：正在抓取" + city + "58房产信息" + "\r\n";
                    for (int i = 1; i < 100; i++)
                    {

                        string url = "http://"+city+".zupuk.com/shangpu/p-" + i + "/";
                        
                        string html = method.GetUrl(url, "utf-8");
                       
                        MatchCollection ids = Regex.Matches(html, @"id=""a_([\s\S]*?)""");
                        if (ids.Count == 0)
                            break;

                        for (int j = 0; j < ids.Count; j++)
                        {
                            string aurl = "http://"+city+".zupuk.com/shangpuchuzu/"+ids[j].Groups[1].Value+".html";
                            string ahtml = method.GetUrl(aurl, "utf-8");
                            Match title= Regex.Match(ahtml, @"<title>([\s\S]*?)-");
                            Match name = Regex.Match(ahtml, @"联系人：([\s\S]*?)</p>");
                            Match tel= Regex.Match(ahtml, @"style=""color:red;"">([\s\S]*?)</span>");
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(title.Groups[1].Value);
                            lv1.SubItems.Add(name.Groups[1].Value);
                            lv1.SubItems.Add(tel.Groups[1].Value);

                            Thread.Sleep(1000);
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
            catch (Exception ex)
            {

                textBox3.Text = ex.ToString();
            }

            textBox3.Text = "抓取完成，等待下次更新......";
        }


        public void baixing()
        {
            try
            {
                string cookie = "__admx_track_id=02mQnsR6vXH8lLLEm3JrMQ; __admx_track_id.sig=vLOXgzRibVv8Zl3QRxdTz9SO7uY; __trackId=160852159015034; _ga=GA1.2.1399547366.1608521593; Hm_lvt_5a727f1b4acc5725516637e03b07d3d2=1611887632,1612148061,1612148369,1612501111; _gid=GA1.2.1574081200.1612501111; __city=nanjing; __s=51vqpeubdfbducimmngkknsp05; _auth_redirect=https%3A%2F%2Fnanjing.baixing.com%2Fjingyingzhuanrang%2F; _gat=1; __sense_session_pv=18; Hm_lpvt_5a727f1b4acc5725516637e03b07d3d2=1612501511";
               List<string> citys = baixinggetCitys();
                string[] types = { "jingyingzhuanrang", "shangpuzhuanrang", "shangpuchushou" };
                foreach (string city in citys)
                {

                    foreach (var type in types)
                    {


                        textBox3.Text += DateTime.Now.ToShortTimeString() + "：正在抓取" + city + "58房产信息" + "\r\n";


                        string url = "https://" + city + ".baixing.com/"+type+"/?sortKey=createdTime";

                        string html = method.GetUrlWithCookie(url, cookie, "utf-8");

                        MatchCollection ids = Regex.Matches(html, @"item-regular '><a href='([\s\S]*?)'");


                        for (int j = 0; j < ids.Count; j++)
                        {
                            string aurl = ids[j].Groups[1].Value;
                            string ahtml = method.GetUrlWithCookie(aurl, cookie, "utf-8");
                            Match title = Regex.Match(ahtml, @"<h1>([\s\S]*?)</h1>");
                            Match name = Regex.Match(ahtml, @"class='poster-name'>([\s\S]*?)<");
                            Match tel = Regex.Match(ahtml, @"<strong>([\s\S]*?)</strong>");
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(title.Groups[1].Value);
                            lv1.SubItems.Add(" ");
                            lv1.SubItems.Add(tel.Groups[1].Value);

                            Thread.Sleep(1000);
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
            catch (Exception ex)
            {

                textBox3.Text = ex.ToString();
            }

            textBox3.Text = "抓取完成，等待下次更新......";
        }

        /// <summary>
        /// 财铺网
        /// </summary>

        public void caipucn()
        {
            try
            {
                string cookie = "__admx_track_id=02mQnsR6vXH8lLLEm3JrMQ; __admx_track_id.sig=vLOXgzRibVv8Zl3QRxdTz9SO7uY; __trackId=160852159015034; _ga=GA1.2.1399547366.1608521593; Hm_lvt_5a727f1b4acc5725516637e03b07d3d2=1611887632,1612148061,1612148369,1612501111; _gid=GA1.2.1574081200.1612501111; __city=nanjing; __s=51vqpeubdfbducimmngkknsp05; _auth_redirect=https%3A%2F%2Fnanjing.baixing.com%2Fjingyingzhuanrang%2F; _gat=1; __sense_session_pv=18; Hm_lpvt_5a727f1b4acc5725516637e03b07d3d2=1612501511";
               string[] citys = { "beijing","kunshan","suzhou","shanghai","tianjin","nanjing","wuxi","xuzhou","changzhou","nantong","yangzhou","hangzhou","ningbo","hefei","guangzhou","shenzhen","jinan","changshu"};
                string[] types = { "1", "2", "3" };
                foreach (string city in citys)
                {

                    foreach (var type in types)
                    {


                        textBox3.Text += DateTime.Now.ToShortTimeString() + "：正在抓取" + city + "58房产信息" + "\r\n";

                        for (int i = 1; i < 999; i++)
                        {


                            string url = "https://"+city+".caipucn.cn/shops/shopslist.html?type="+type+"&page="+i;

                            string html = method.GetUrlWithCookie(url, cookie, "utf-8");

                            MatchCollection ids = Regex.Matches(html, @"<div class=""cell-left fl"">([\s\S]*?)id=([\s\S]*?)""");

                            if (ids.Count == 0)
                                break;
                            for (int j = 0; j < ids.Count; j++)
                            {
                                string aurl = "https://hangzhou.caipucn.cn/shops/details.html?id=" + ids[j].Groups[2].Value;
                                string ahtml = method.GetUrlWithCookie(aurl, cookie, "utf-8");
                                Match title = Regex.Match(ahtml, @"<title>([\s\S]*?)-");
                                Match name = Regex.Match(ahtml, @"<p class=""nickname"">([\s\S]*?)</p>");
                                Match tel = Regex.Match(ahtml, @"<br/><b>([\s\S]*?)</b>");
                                ListViewItem lv1 = listView1.Items.Add((31000+listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(title.Groups[1].Value);
                                lv1.SubItems.Add(name.Groups[1].Value.Trim());
                                lv1.SubItems.Add(tel.Groups[1].Value);

                                Thread.Sleep(1000);
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

                textBox3.Text = ex.ToString();
            }

            textBox3.Text = "抓取完成，等待下次更新......";
        }
        Thread thread;
        bool zanting = true;
        bool status = true;
        private void button1_Click(object sender, EventArgs e)
        {
            status = true;

            Thread t = new Thread(run);
            t.Start();
            Control.CheckForIllegalCrossThreadCalls = false;


            if (thread == null || !thread.IsAlive)
            {

                thread = new Thread(baixing);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

            Thread t1 = new Thread(caipucn);
            t1.Start();
            Control.CheckForIllegalCrossThreadCalls = false;


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

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 商铺_Load(object sender, EventArgs e)
        {

        }
    }
}
