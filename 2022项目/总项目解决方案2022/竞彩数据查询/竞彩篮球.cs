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
using myDLL;

namespace 竞彩数据查询
{
    public partial class 竞彩篮球 : Form
    {
        public 竞彩篮球()
        {
            InitializeComponent();
        }
        public void getdata()
        {
            //https://www.okooo.com/jingcailanqiu/livecenter/2022-01-11/



            for (DateTime dt = Convert.ToDateTime("2014-01-01"); dt < Convert.ToDateTime("2022-04-10"); dt = dt.AddDays(1))
            {
                try
                {

                    string url = "https://live.500.com/lq.php?c=jc&e=" + dt.ToString("yyyy-MM-dd");
                    label1.Text = dt.ToString();

                    string html = method.GetUrl(url, "gb2312");
                    MatchCollection trs = Regex.Matches(html, @"<tr id=""([\s\S]*?)</tr>");

                    for (int i = 0; i < trs.Count; i++)
                    {
                    
                        MatchCollection values = Regex.Matches(html, @"\[([\s\S]*?)\]");

                        //string[] text = values[j].Groups[1].Value.Split(new string[] { "," }, StringSplitOptions.None);

                        //if (text.Length > 4)
                        //{
                        //    fc.insertdata(date, matchname, zhu, ke, bifen, text[0], text[1], text[2], type, text[3], text[4].Replace("\"", ""), result);
                        //}


                    }

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }
            }

            MessageBox.Show("完成");
        }
        Thread thread;
        private void button3_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getdata);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        function fc;
        private void button4_Click(object sender, EventArgs e)
        {
            string sql = "delete from datas";
            bool status = fc.SQL(sql);
            fc.SQL("VACUUM");
            fc.SQL("DELETE FROM sqlite_sequence WHERE name = 'datas';");
            MessageBox.Show("清空成功");
        }

        private void 竞彩篮球_Load(object sender, EventArgs e)
        {

        }
    }
}
