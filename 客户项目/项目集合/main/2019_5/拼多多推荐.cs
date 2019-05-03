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

namespace main._2019_5
{
    public partial class 拼多多推荐 : Form
    {
        public 拼多多推荐()
        {
            InitializeComponent();
        }
        ArrayList finishes = new ArrayList();

       static string COOKIE = "api_uid=rBQRpFvJizBVvw8WqeZ3Ag==; _nano_fp=XpdqlpUqnp9xn5ToXo_ke8~lQxmpH7B0sj51EhI7; ua=Mozilla%2F5.0%20(Windows%20NT%2010.0%3B%20Win64%3B%20x64)%20AppleWebKit%2F537.36%20(KHTML%2C%20like%20Gecko)%20Chrome%2F69.0.3497.81%20Safari%2F537.36; webp=1; msec=1800000; rec_list_catgoods=rec_list_catgoods_DSg1TO; pdd_user_uin=7BT7HSLWTMIXDJYZTOHT4Q45MQ_GEXDA; pdd_user_id=7312500755985; PDDAccessToken=N4ADUU4BJHRIWTYK46NGVVW2V4MK2CMYPVUOFEPDOYQGEMOEK7UQ102118b; rec_list_orders=rec_list_orders_K5JiGI; rec_list_index=rec_list_index_UFU4HN; goods_detail=goods_detail_2N7P3v; goods_detail_mall=goods_detail_mall_upiHX7; rec_list_personal=rec_list_personal_Nah6Rp; JSESSIONID=5B0521BB07419CD93E8B777C3C766B46";
       bool zanting =true;
        public void run()
        {
            try
            {
                for (int i = 0; i < 400; i=i+20)
                {

                    string url = "http://mobile.yangkeduo.com/proxy/api/api/barrow/query?app_name=rectab_sim_gyl&support_types=0_1&offset="+i+"&count=20&list_id=6rTfjG3X02&dp_list_id=6rTfjG3X02_dp&pdduid=7312500755985&is_back=1";
                    string html = method.GetUrlWithCookie(url, COOKIE);
                    //textBox3.Text = html;
                    //return;

                    MatchCollection goodids = Regex.Matches(html, @"""goods_id"":([\s\S]*?),");
                    MatchCollection sales = Regex.Matches(html, @"""sales"":([\s\S]*?),");

                    for (int j = 0; j < goodids.Count; j++)
                    {
                        if (Convert.ToInt32(sales[j].Groups[1].Value) < Convert.ToInt32(textBox4.Text))
                        {
                            if (!finishes.Contains(goodids[j].Groups[1].Value.Trim()))
                            {
                                finishes.Add(goodids[j].Groups[1].Value.Trim());
                                string URL = "http://mobile.yangkeduo.com/goods.html?goods_id=" + goodids[j].Groups[1].Value.Trim();


                                ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                listViewItem.SubItems.Add(URL);

                                if (this.listView1.Items.Count > 2)
                                {
                                    this.listView1.EnsureVisible(this.listView1.Items.Count - 1);
                                }



                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                            }
                        }
                    }

                    
                    Application.DoEvents();
                    Thread.Sleep(1000);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(this.run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }
    }
}
