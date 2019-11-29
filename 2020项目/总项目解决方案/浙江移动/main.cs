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

        }

        bool zanting = true;
        #region  主程序
        public void run()
        {
            try

            {



                for (int i = 0; i < 9999; i++)
                {
                   
                    string url = "http://wap.zj.10086.cn/shop/shop/goods/contractNumber/queryIndex.do?cityid=402881ea3286d488013286d756720002&currentPageNum="+i+"&span1=&span2=&span3=&span4=&span5=&fuzzySpan=&span6=&span7=&span8=&span9=&span10=&teleCodePer=&suiteId=8ace47ba6df929ea016e1a6193aa4d6f&priceRangeId=&baseFeeId=&numRuleId=&orderBy=&isNofour=N&pageCount=100";


                    string html = method.GetUrl(url, "utf-8");




                    MatchCollection a1s = Regex.Matches(html, @"""teleCode"":""([\s\S]*?)""");
                    MatchCollection a2s = Regex.Matches(html, @"""teleCode"":""([\s\S]*?)""");
                    MatchCollection a3s = Regex.Matches(html, @"""teleCode"":""([\s\S]*?)""");
                    MatchCollection a4s = Regex.Matches(html, @"""teleCode"":""([\s\S]*?)""");
                    MatchCollection a5s = Regex.Matches(html, @"""teleCode"":""([\s\S]*?)""");



                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据      
              
                    lv1.SubItems.Add(a1.Groups[1].Value);
                  
                 
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
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
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
