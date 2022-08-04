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
    public partial class lanqiu : System.Web.UI.Page
    {
       

        public string result = "";
      
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
                if (Request.QueryString["type"] == "2")
                {
                    getokoo();
                    if (Request.QueryString["zhu"] != "" && Request.QueryString["zhu"] != null)
                    {
                        lanqiurangfen();
                    }

                }
                else if (Request.QueryString["type"] == "3")
                {
                   // lanqiudaxiaofen();
                }
                else
                {
                    getokoo();
                }
            }





        }


        function fc = new function();

        #region  篮球让分
        public void lanqiurangfen()
        {
            string zhuname = "阿尔巴";
            string kename = "骑士";
            string zhu_rang_fen = "+2.5";
            string zhu_shourang_fen = "-3.5";

            zhuname = Request.QueryString["zhu"];
            kename = Request.QueryString["ke"];
            zhu_rang_fen = Request.QueryString["rangfen"];

            zhu_shourang_fen = Request.QueryString["rangfen"];





            #region 查询1
            string sql = "select * from datas where";

            if (zhu_rang_fen != "")
            {
                sql = sql + (" zhu_rang_fen like '" + zhu_rang_fen + "' and");
            }
            if (zhuname != "")
            {
                sql = sql + (" zhu like '" + zhuname + "' and");
            }

            if (sql.Substring(sql.Length - 3, 3) == "and")
            {
                sql = sql.Substring(0, sql.Length - 3);
            }

            #endregion


            #region 查询2
            string sql2 = "select * from datas where";

            if (zhu_shourang_fen != "")
            {
                sql2 = sql2 + (" zhu_rang_fen like '" + zhu_shourang_fen + "' and");
            }
            if (kename != "")
            {
                sql2 = sql2 + (" zhu like '" + kename + "' and");
            }
           

            if (sql2.Substring(sql2.Length - 3, 3) == "and")
            {
                sql2 = sql2.Substring(0, sql2.Length - 3);
            }

            #endregion
         
            
            DataTable dt = fc.chaxundata2(sql);
            DataTable dt2 = fc.chaxundata2(sql2);
            dt.Merge(dt2);

         



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

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string value = dt.Rows[i][10].ToString().Trim();


                string xuhao = dt.Rows[i][1].ToString().Trim();
                string time = dt.Rows[i][2].ToString().Trim();
                string liansai = dt.Rows[i][3].ToString().Trim();
                string zhu = dt.Rows[i][4].ToString().Trim();
                string ke = dt.Rows[i][5].ToString().Trim();
                string zhu_fen = dt.Rows[i][6].ToString().Trim();
                string ke_fen = dt.Rows[i][7].ToString().Trim();

                string zhurang_fen = dt.Rows[i][13].ToString().Trim();
                string zhurang_pv = dt.Rows[i][14].ToString().Trim();
            

                sb.Append("<tr>");
                sb.Append("<td><span>" + xuhao + "</span></td>");
                sb.Append("<td><span>" + time + "</span></td>");
                sb.Append("<td><span>" + liansai + "</span></td>");
                sb.Append("<td><span>" + zhu + "</span></td>");
                sb.Append("<td><span>" + ke + "</span></td>");
                sb.Append("<td><span>" + zhu_fen + "</span></td>");
                sb.Append("<td><span>" + ke_fen + "</span></td>");
                sb.Append("<td><span>" + zhurang_fen + "</span></td>");
                sb.Append("<td><span>" + zhurang_pv + "</span></td>");
             
             
                sb.Append("</tr>");




                try
                {
                    if (Convert.ToInt32(zhu_fen) > Convert.ToInt32(ke_fen))
                    {
                        shengcount = shengcount + 1;
                    }
                    if (Convert.ToInt32(zhu_fen) == Convert.ToInt32(ke_fen))
                    {
                        pingcount = pingcount + 1;
                    }
                    if (Convert.ToInt32(zhu_fen) < Convert.ToInt32(ke_fen))
                    {
                        fucount = fucount + 1;
                    }
                }
                catch (Exception)
                {


                }


            }

            result = sb.ToString();
            try
            {

                //变化比例
                double shenglv = shengcount / (dt.Rows.Count);
                double pinglv = pingcount / (dt.Rows.Count);
                double fulv = fucount / (dt.Rows.Count);

                sheng_bili = shenglv.ToString("0.00");
                ping_bili = pinglv.ToString("0.00");
                fu_bili = fulv.ToString("0.00");

              


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
                string url = "https://www.okooo.com/jingcailanqiu/livecenter/";
                string html = function.GetUrl(url, "gb2312");
                StringBuilder sb = new StringBuilder();

                MatchCollection xuhao = Regex.Matches(html, @"type=""checkbox""><span>([\s\S]*?)</span>");
                MatchCollection zhuke = Regex.Matches(html, @"target=""_blank"" title=""([\s\S]*?)""");
                
                MatchCollection rangfen = Regex.Matches(html, @"data-hwl=""([\s\S]*?)""");
                MatchCollection times = Regex.Matches(html, @"<td  class=""graytxt"">([\s\S]*?)</td>");

                for (int i = 0; i < xuhao.Count; i++)
                {

                    string zhu = zhuke[(2 * i)].Groups[1].Value;
                    string ke = zhuke[(2 * i)+1].Groups[1].Value;

                    string time = times[i].Groups[1].Value;

                    sb.Append("<tr>");
                    sb.Append("<td><span>" + xuhao[i].Groups[1].Value + "</span></td>");
                    sb.Append("<td><span>" + time + "</span></td>");
                    sb.Append("<td><span>" + zhu + "</span></td>");
                    sb.Append("<td><span>" + ke + "</span></td>");
                    sb.Append("<td><span>" + rangfen[i].Groups[1].Value + "</span></td>");
                    sb.Append("<td><span><a href=/lanqiu.aspx?type=2&rangfen=" + rangfen[i].Groups[1].Value + "&zhu=" + zhu + "&ke=" + ke + "&time=" + time + ">点击分析</a></span></td>");

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