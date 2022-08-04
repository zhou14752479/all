using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASP足球网站
{
    public partial class index : System.Web.UI.Page
    {
        public string result = "";
        public string sheng= "";
        public string ping = "";
        public string fu = "";

        public string sheng_bili = "";
        public string ping_bili = "";
        public string fu_bili = "";

        public string chusheng_bili = "";
        public string chuping_bili = "";
        public string chufu_bili = "";


        public string matchtime = "";
        public string zhu = "";
        public string ke = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.RequestType == "GET")
            {
                if (Request.QueryString["type"] == "1")
                {
                    getokoo();
                    if (Request.QueryString["matchid"] !="" && Request.QueryString["matchid"] !=null)
                    {
                        zuqiuspf(Request.QueryString["matchid"]);
                    }
                    
                }
               else if (Request.QueryString["type"] == "2")
                {
                   
                }
                else
                {
                    getokoo();
                }
            }





        }


        function fc = new function();

        #region  足球胜平负
        public void zuqiuspf(string matchid)
        {

            zhu = Request.QueryString["zhu"];
            ke = Request.QueryString["ke"];
            matchtime = Request.QueryString["time"];

            string url = "https://www.okooo.com/I/?method=ok.soccer.odds.GetProcess";
            string postdata = "match_id="+ matchid + "&betting_type_id=1&provider_id=2";

            string html = function.PostUrlDefault(url,postdata,"");

            MatchCollection h=Regex.Matches(html, @"""h"":""([\s\S]*?)""");
            MatchCollection d = Regex.Matches(html, @"""d"":""([\s\S]*?)""");
            MatchCollection a = Regex.Matches(html, @"""a"":""([\s\S]*?)""");

            string value1 = "";
            string value2 = "";
            string value3 = "";
            string value4 = "";
            string value5 = "";
            string value6 = "";

            if (h.Count>0)
            {
                 value1 = h[0].Groups[1].Value;
                 value2 = d[0].Groups[1].Value;
                
            }


            if (h.Count > 1)
            {
                 value4 = h[1].Groups[1].Value;
                 value5 = d[1].Groups[1].Value;
               
            }

            sheng = value1;
            ping = value2;
            fu = a[0].Groups[1].Value;
            //string value1 = "2.4";
            //string value2 = "2.8";


            if (h.Count > 0)
            {
                if (value1.Substring(value1.Length - 1, 1) == "0")
                {
                    value1 = value1.Substring(0, value1.Length - 1);
                }
                if (value2.Substring(value2.Length - 1, 1) == "0")
                {
                    value2 = value2.Substring(0, value2.Length - 1);
                }
            }
            if (h.Count > 1)
            {
                if (value4.Substring(value4.Length - 1, 1) == "0")
                {
                    value4 = value4.Substring(0, value4.Length - 1);
                }
                if (value5.Substring(value5.Length - 1, 1) == "0")
                {
                    value5 = value5.Substring(0, value5.Length - 1);
                }
            }



            #region 查询1
            string sql = "select date,updatetime,match,zhu,ke,bifen,sheng,ping,fu,type,result from datas where";

            if (value1 != "")
            {
                sql = sql + (" sheng like '" + value1 + "' and");
            }
            if (value2 != "")
            {
                sql = sql + (" ping like '" + value2 + "' and");
            }
            if (value3 != "")
            {
                sql = sql + (" fu like '" + value3 + "' and");
            }

            if (sql.Substring(sql.Length - 3, 3) == "and")
            {
                sql = sql.Substring(0, sql.Length - 3);
            }

            #endregion


            #region 查询2
            string sql2 = "select date,updatetime,match,zhu,ke,bifen,sheng,ping,fu,type,result from datas where";

            if (value4 != "")
            {
                sql2 = sql2 + (" sheng like '" + value4 + "' and");
            }
            if (value5 != "")
            {
                sql2 = sql2 + (" ping like '" + value5 + "' and");
            }
            if (value6 != "")
            {
                sql2 = sql2 + (" fu like '" + value6 + "' and");
            }

            if (sql2.Substring(sql2.Length - 3, 3) == "and")
            {
                sql2 = sql2.Substring(0, sql2.Length - 3);
            }

            #endregion



            DataTable dt = fc.chaxundata(sql);
            DataTable dt2 = fc.chaxundata(sql2);

            DataTable newdt = dt.Clone();



            //h.count代表是否有两行变化数据
            if (h.Count > 1)
            {
                Dictionary<string, int> dics = new Dictionary<string, int>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string date = dt.Rows[i][0].ToString().Trim() + dt.Rows[i][3].ToString().Trim();

                    if (!dics.ContainsKey(date))
                    {
                        dics.Add(date, i);
                    }


                }
                Dictionary<string, int> dics2 = new Dictionary<string, int>();

                for (int i = 0; i < dt2.Rows.Count; i++)//如果DataGridView中有空的数据，则提示数据输入不完整并退出添加，不包括标题行
                {
                    try
                    {
                        string date = dt2.Rows[i][0].ToString().Trim() + dt2.Rows[i][3].ToString().Trim();

                        if (dics.ContainsKey(date))
                        {
                            if (!dics2.ContainsKey(date))
                            {
                                dics2.Add(date, i);
                            }
                            newdt.Rows.Add(dt.Rows[dics[date]].ItemArray);
                            newdt.Rows.Add(dt2.Rows[i].ItemArray);

                        }
                    }
                    catch (Exception ex)
                    {

                        Response.Write(ex.ToString());
                    }
                }
            }
            //h.count代表是否有两行变化数据，没有则使用第一行
            else
            {
                newdt = dt.Copy();
            }






            //计算初始胜率
            double chushengcount = 0;
            double chupingcount = 0;
            double chufucount = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string value = dt.Rows[i][10].ToString().Trim();
                if (value == "胜")
                {
                    chushengcount = chushengcount + 1;
                }
                if (value == "平")
                {
                    chupingcount = chupingcount + 1;
                }
                if (value == "负")
                {
                    chufucount = chufucount + 1;
                }


            }
            try
            {

                //初始比例
                double chushenglv = chushengcount / (dt.Rows.Count);
                double chupinglv = chupingcount / (dt.Rows.Count);
                double chufulv = chufucount / (dt.Rows.Count);

                chusheng_bili = chushenglv.ToString("0.00");
                chuping_bili = chupinglv.ToString("0.00");
                chufu_bili = chufulv.ToString("0.00");

            }
            catch (Exception)
            {


            }










            //计算变化胜率
            double shengcount = 0;
            double pingcount = 0;
            double fucount = 0;



            StringBuilder sb = new StringBuilder();
           
            for (int i = 0; i < newdt.Rows.Count; i++)
            {
                string value = newdt.Rows[i][10].ToString().Trim();


                string date = newdt.Rows[i][1].ToString().Trim();
                string match = newdt.Rows[i][2].ToString().Trim();
                string zhu = newdt.Rows[i][3].ToString().Trim();
                string ke = newdt.Rows[i][4].ToString().Trim();
                string bifen= newdt.Rows[i][5].ToString().Trim();
                string sheng = newdt.Rows[i][6].ToString().Trim();
                string ping = newdt.Rows[i][7].ToString().Trim();
                string fu = newdt.Rows[i][8].ToString().Trim();

                sb.Append("<tr>");
                sb.Append("<td><span>"+date+"</span></td>");
                sb.Append("<td><span>"+match+"</span></td>");
                sb.Append("<td><span>"+zhu+"</span></td>");
                sb.Append("<td><span>" + ke + "</span></td>");
                sb.Append("<td><span>" + bifen + "</span></td>");
                sb.Append("<td><span>" + sheng + "</span></td>");
                sb.Append("<td><span>" + ping + "</span></td>");
                sb.Append("<td><span>" + fu + "</span></td>");
                sb.Append("<td><span>" + value + "</span></td>");
                sb.Append("</tr>");


       

                if (value == "胜")
                {
                    shengcount = shengcount + 1;
                }
                if (value == "平")
                {
                    pingcount = pingcount + 1;
                }
                if (value == "负")
                {
                    fucount = fucount + 1;
                }


            }

            result = sb.ToString();
            try
            {

                //变化比例
                double shenglv = shengcount / (newdt.Rows.Count);
                double pinglv = pingcount / (newdt.Rows.Count);
                double fulv = fucount / (newdt.Rows.Count);

                sheng_bili = shenglv.ToString("0.00");
                ping_bili= pinglv.ToString("0.00");  
                fu_bili= fulv.ToString("0.00");

                //第二行变化赔率
                if (h.Count <= 1)
                {
                    sheng_bili = "0";
                    ping_bili = "0";    
                    fu_bili = "0";

                }


            }
            catch (Exception)
            {

               
            }

           
        }
        #endregion


        



        #region 获取澳客赔率

        public void getokoo()
        {
            try
            {
                string url = "https://www.okooo.com/livecenter/jingcai/";
                string html = function.GetUrl(url,"gb2312");
                StringBuilder sb = new StringBuilder();

                MatchCollection xuhao = Regex.Matches(html, @"</label><span>([\s\S]*?)</span>");
                MatchCollection zhu = Regex.Matches(html, @"homename([\s\S]*?)>([\s\S]*?)</a>");
                MatchCollection ke = Regex.Matches(html, @"awayname([\s\S]*?)>([\s\S]*?)</a>");
                MatchCollection ids= Regex.Matches(html, @"matchid=""([\s\S]*?)""");
                MatchCollection times = Regex.Matches(html, @"<td class=""ls"">([\s\S]*?)<td class=""ctrl_matchtime");

                for (int i = 0; i < xuhao.Count; i++)
                { 
                    string time = Regex.Replace(times[i].Groups[1].Value, "<[^>]+>", "").Trim().Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ","-");

                    sb.Append("<tr>");
                    sb.Append("<td><span>" + xuhao[i].Groups[1].Value + "</span></td>");
                    sb.Append("<td><span>" + time + "</span></td>");
                    sb.Append("<td><span>" + zhu[i].Groups[2].Value + "</span></td>");
                    sb.Append("<td><span>" + ke[i].Groups[2].Value + "</span></td>");
                    sb.Append("<td><span><a href=/index.aspx?type=1&matchid=" + ids[i].Groups[1].Value + "&zhu="+ zhu[i].Groups[2].Value + "&ke="+ ke[i].Groups[2].Value+ "&time="+ time + ">点击分析</a></span></td>");
                   
                    sb.Append("</tr>");
                    //Response.Write(input);

                }

                Application["okooo_data"] = sb.ToString();

            }
            catch (Exception)
            {
               
                
            }
        }

        #endregion


    }
}