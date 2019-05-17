using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 上海交易所
        /// </summary>
        /// <param name="time">时间格式：20190516</param>
        /// <returns></returns>
        public string shfe(string time)
        {
            string url = "http://www.shfe.com.cn/data/dailydata/kx/kx"+time+".dat";
            string json = method.GetUrl(url, "utf-8");

            return json;
        }












            /// <summary>
            /// 大连商品交易所
            /// </summary>
            /// <param name="time">日期</param>
            /// <returns></returns>
            public string dce()
        {
            string url = "http://www.dce.com.cn/publicweb/quotesdata/dayQuotesCh.html?dayQuotes.variety=all&dayQuotes.trade_type=0&year=2019&month=4&day=16";
            string html = method.GetUrl(url,"utf-8");
            MatchCollection matches = Regex.Matches(html, @"<tr><td>&nbsp;([\s\S]*?)</td><td>([\s\S]*?)</td><td>([\s\S]*?)</td><td>([\s\S]*?)</td><td>([\s\S]*?)</td><td>([\s\S]*?)</td><td>([\s\S]*?)</td><td>([\s\S]*?)</td><td>([\s\S]*?)</td><td>([\s\S]*?)</td>([\s\S]*?)<td>([\s\S]*?)</td><td>([\s\S]*?)</td><td>([\s\S]*?)</td><td>([\s\S]*?)</td>");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < matches.Count; i++)
            {
                
                QIHUO qihuo = new QIHUO()
                {
                    name = matches[i].Groups[1].Value.Trim(),
                    month = matches[i].Groups[2].Value.Trim(),
                    open = matches[i].Groups[3].Value.Trim(),
                    high = matches[i].Groups[4].Value.Trim(),
                    low = matches[i].Groups[5].Value.Trim(),
                    close = matches[i].Groups[6].Value.Trim(),
                    qjsj = matches[i].Groups[7].Value.Trim(),
                    jsj = matches[i].Groups[8].Value.Trim(),
                    zd = matches[i].Groups[9].Value.Trim(),
                    zd1 = matches[i].Groups[10].Value.Trim(),
                    cjl = matches[i].Groups[12].Value.Trim(),
                    ccl = matches[i].Groups[13].Value.Trim(),
                    change = matches[i].Groups[14].Value.Trim(),
                    cje = matches[i].Groups[15].Value.Trim()

                };
                string jsonStr = JsonConvert.SerializeObject(qihuo);
                
                sb.Append( jsonStr + ",");
            }

            return "{\"data\":[" + sb.ToString().Remove(sb.ToString().Length-1,1) + "]}";
        }
       
       

        /// <summary>
        /// 郑州商品交易所
        /// </summary>
        /// <param name="time">20190516</param>
        /// <returns></returns>
        public string czce(string time)
        {
            string url = "http://www.czce.com.cn/cn/DFSStaticFiles/Future/2019/"+time+"/FutureDataDaily.htm";
            string html = method.GetUrl(url, "utf-8");

            Match shtml = Regex.Match(html, @"<tbody>([\s\S]*?)<td valign");
            string strhtml = shtml.Groups[1].Value;
            MatchCollection lasthtml = Regex.Matches(strhtml, @"<tr>([\s\S]*?)</tr>");
          
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < lasthtml.Count; j++)
            {

                MatchCollection matches = Regex.Matches(lasthtml[j].Groups[1].Value, @"<td>([\s\S]*?)</td>");

              
                    QIHUO qihuo = new QIHUO()
                    {
                        name = matches[0].Groups[1].Value.Trim(),
                        month = matches[0].Groups[1].Value.Trim(),
                        open = matches[2].Groups[1].Value.Trim(),
                        high = matches[3].Groups[1].Value.Trim(),
                        low = matches[4].Groups[1].Value.Trim(),
                        close = matches[5].Groups[1].Value.Trim(),
                        qjsj = matches[1].Groups[1].Value.Trim(),
                        jsj = matches[6].Groups[1].Value.Trim(),
                        zd = matches[7].Groups[1].Value.Trim(),
                        zd1 = matches[8].Groups[1].Value.Trim(),
                        cjl = matches[9].Groups[1].Value.Trim(),
                        ccl = matches[10].Groups[1].Value.Trim(),
                        change = matches[11].Groups[1].Value.Trim(),
                        cje = matches[12].Groups[1].Value.Trim()

                    };
                    string jsonStr = JsonConvert.SerializeObject(qihuo);

                    sb.Append(jsonStr + ",");
                
            }

            return "{\"data\":[" + sb.ToString().Remove(sb.ToString().Length - 1, 1) + "]}";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void Button1_Click(object sender, EventArgs e)
        {
            //for (int i = 0; i < 10; i++)
            //{
            //    QIHUO userDataInfo = new QIHUO()
            //    {
            //        name = "1",
            //        open = 1,
            //        high = 1,
            //        low = 1,

            //    };
            //    string jsonStr = JsonConvert.SerializeObject(userDataInfo);
            //    textBox1.Text += jsonStr+",";

            //}

            //textBox1.Text= "{\"data\":[" + textBox1.Text + "]}";

            //textBox1.Text =dce();

            textBox1.Text= czce("20190516");
        }



    }
}
