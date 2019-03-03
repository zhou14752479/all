using Newtonsoft.Json;
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

namespace 链家网
{
    public partial class 驾校 : Form
    {
        public 驾校()
        {
            InitializeComponent();
        }
        bool status = true;
        private void 驾校_Load(object sender, EventArgs e)
        {

        }

        public class RootObject
        {
            public List<Result> result { get; set; }

            public string msg { get; set; }
        }


        public class Result
        {
            public List<J1> j1 { get; set; }
        }

        public class J1
        {
            public List<Infolist> infolist { get; set; }
        }


        public class Infolist
        {
            public string name { get; set; }
            public string schoolName { get; set; }
            public string tel { get; set; }
        }

        public void run()
        {
            try
            {


                for (int i = 1; i < 100; i++)
                {
                    string url = "https://businessapi.jxedt.com/list/?cityid=1&channel=80000&version=6.9.0&os=ios&type=jl&productid=3&pageindex="+i+"&pagesize=20";

                    string html = method.GetHtmlSource(url);

                    MatchCollection names = Regex.Matches(html, @"""name"":""([\s\S]*?)""");
                    if (names.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;

                    RootObject rb = JsonConvert.DeserializeObject<RootObject>(html);
                    Result result= JsonConvert.DeserializeObject<Result>(rb.result.ToString());
                    J1 j1 = JsonConvert.DeserializeObject<J1>(result.j1.ToString());
                    

                    foreach (Infolist ep in j1.infolist)
                    {
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                        lv1.SubItems.Add(ep.name);
                    }



                    while (this.status == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
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
            status = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
