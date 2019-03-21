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

namespace fang._2019
{
    public partial class 淘宝商品上下架 : Form
    {
        public 淘宝商品上下架()
        {
            InitializeComponent();
        }

        private void 淘宝商品上下架_Load(object sender, EventArgs e)
        {

        }
        bool zanting = true;
        public void run()
        {
            try
            {
                int type = 0;
                if (radioButton1.Checked == true)
                {
                    type = 11;
                }

                else if (radioButton1.Checked == true)
                {
                    type = 1;
                }


                for (int i = 1; i < 99999; i++)
                {

                    string url = "https://sell.taobao.com/auction/merchandise/auction_list.htm?type=" + type;
                    string postdata = "_tb_token_=57685d3e0586a&pageName=goodsOnSale&banner=&page=" + i + "&setVal=&orderField=1&orderBy=0&singleId=&singleIdNum=&singleIdMinNum=&distributionIds=&action=goodsmanager%2FGoodsManageAction&event_submit_do_recommend=&event_submit_do_delete=&event_submit_do_off_shelf=&event_submit_do_unrecommend=&event_submit_do_g9_distribute=&event_submit_do_set_lighting_auction=&shopCatName=%C8%AB%B2%BF%B7%D6%C0%E0&searchKeyword=&startPrice=&endPrice=&itemConditionSet=&outId=&startNum=&endNum=&itemStepAudit=&category=&scatid=&itemId=&operate=&canoff%3A589166196631=true&recommend%3A589166196631=false&fittingRoom%3A589166196631=false&canoff%3A589378993132=true&recommend%3A589378993132=false&fittingRoom%3A589378993132=false&canoff%3A589378729533=true&recommend%3A589378729533=false&fittingRoom%3A589378729533=false&canoff%3A589527138215=true&recommend%3A589527138215=false&fittingRoom%3A589527138215=false&canoff%3A589166396372=true&recommend%3A589166396372=false&fittingRoom%3A589166396372=false&canoff%3A589527054270=true&recommend%3A589527054270=false&fittingRoom%3A589527054270=false&canoff%3A589166400362=true&recommend%3A589166400362=false&fittingRoom%3A589166400362=false&canoff%3A589527058238=true&recommend%3A589527058238=false&fittingRoom%3A589527058238=false&canoff%3A589378785389=true&recommend%3A589378785389=false&fittingRoom%3A589378785389=false&canoff%3A589166068792=true&recommend%3A589166068792=false&fittingRoom%3A589166068792=false&canoff%3A589379013067=true&recommend%3A589379013067=false&fittingRoom%3A589379013067=false&canoff%3A589379045007=true&recommend%3A589379045007=false&fittingRoom%3A589379045007=false&canoff%3A589689775205=true&recommend%3A589689775205=false&fittingRoom%3A589689775205=false&canoff%3A589378765339=true&recommend%3A589378765339=false&fittingRoom%3A589378765339=false&canoff%3A589689919017=true&recommend%3A589689919017=false&fittingRoom%3A589689919017=false&canoff%3A589526762665=true&recommend%3A589526762665=false&fittingRoom%3A589526762665=false&canoff%3A589378709360=true&recommend%3A589378709360=false&fittingRoom%3A589378709360=false&canoff%3A589689739178=true&recommend%3A589689739178=false&fittingRoom%3A589689739178=false&canoff%3A589166392178=true&recommend%3A589166392178=false&fittingRoom%3A589166392178=false&canoff%3A589166284283=true&recommend%3A589166284283=false&fittingRoom%3A589166284283=false&operate=&pageNO=";
                    string cookie = textBox1.Text;
                    string html = method.PostUrl(url, postdata, cookie, "gb2312");
                    MatchCollection IDs = Regex.Matches(html, @"data-param=""itemId=([\s\S]*?)&cid=([\s\S]*?)&title=([\s\S]*?)""");

                    if (IDs.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;

                    for (int j = 0; j < IDs.Count; j++)
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(IDs[j].Groups[1].Value);
                        lv1.SubItems.Add(IDs[j].Groups[3].Value);
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (listView1.Items.Count - 1 > 1)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);
                        }
                    }
                    Thread.Sleep(500);
                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void 删除此商品ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            textBox5.Text = "http://gw.api.taobao.com/router/rest?sign=48EB55380FBD5C0B58E4FD265BE5A36F&timestamp=2019-03-21+15%3A04%3A10&v=2.0&app_key=" + textBox2.Text + "&method=taobao.item.delete&sign_method=hmac&partner_id=top-apitools&session=" + textBox4.Text + "&format=json&num_iid=" + this.listView1.SelectedItems[0].SubItems[1].Text + "&force_sensitive_param_fuzzy=true";
            //textBox5.Text=  method.GetUrl("http://gw.api.taobao.com/router/rest?sign=48EB55380FBD5C0B58E4FD265BE5A36F&timestamp=2019-03-21+15%3A04%3A10&v=2.0&app_key="+textBox2.Text+"&method=taobao.item.delete&sign_method=hmac&partner_id=top-apitools&session="+textBox4.Text+"&format=json&num_iid="+ this.listView1.SelectedItems[0].SubItems[1].Text + "&force_sensitive_param_fuzzy=true", "utf-8");
        }
    }
}
