using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace 学科网下载
{
    public partial class api : System.Web.UI.Page
    {
        

        public static string cookie = "";
       
        protected void Page_Load(object sender, EventArgs e)
        {


            try
            {
                cookie = File.ReadAllText(Server.MapPath("~/") + "cookie.txt").Trim();
                
                string method = Request["method"];
                string key = Request["key"];
                string link = Request["link"];

                string cishu = Request["cishu"];
                string day = Request["day"];
                string vip = Request["vip"];
                string page = Request["page"];


                if (method == "getfile" && link != "" && key != "")
                {
                    if(link.Contains("zxxk") || link.Contains("xkw"))
                    {
                        getfile(key, link);
                    }
                    else
                    {
                        Response.Write("{\"status\":\"0\",\"msg\":\"网址输入有误，请联系客服\"}");
                    }
                    
                }
                else
                {
                    Response.Write("{\"status\":\"0\",\"msg\":\"网址输入有误，请联系客服\"}");
                }
            }
            catch (Exception ex)
            {
                Response.Write("{\"status\":\"0\",\"msg\":\"服务异常，请联系客服\"}"+ex.ToString());
            }
            
           

        }





        #region 下载文件
        public void getfile(string key, string link)
        {
            string userIp = Request.UserHostAddress;

            MySqlConnection mycon = new MySqlConnection(method.constr);
            mycon.Open();
            string query = "SELECT * FROM mykeys where mykey= '" + key + "' ";
            MySqlCommand command = new MySqlCommand(query, mycon);
            MySqlDataReader reader = command.ExecuteReader();



            if (reader.Read())
            {
                string cishu = reader["cishu"].ToString().Trim();
                string extime = reader["extime"].ToString().Trim();
                string day = reader["day"].ToString().Trim();
                string isvip = reader["isvip"].ToString().Trim();
                string fileid = Regex.Match(link, @"\d{5,}").Groups[0].Value;

                mycon.Close();
                reader.Close();


                if(fileid=="")
                {
                    Response.Write("{\"status\":\"0\",\"msg\":\"资料地址错误，请复制一篇资料网址，而不是专辑\"}");
                    return;
                }

                if (Convert.ToInt32(cishu) <= 0)
                {
                    Response.Write("{\"status\":\"0\",\"msg\":\"剩余次数不足，请联系客服\"}");
                    return;
                }

                if (extime!="")
                {
                    if (Convert.ToDateTime(extime) < DateTime.Now)
                    {
                        Response.Write("{\"status\":\"0\",\"msg\":\"当前购买的秘钥已过期！\"}");
                        return;
                    }
                   
                }

                //第一次筛选中职
                if (isvip == "0" || isvip == "")
                {
                    if (link.Contains("zhijiao"))
                    {
                        Response.Write("{\"status\":\"0\",\"msg\":\"当前购买的权限不能下载教辅及中职资料，请拍下对应选项的秘钥！（当前账户可下：精品、特供、普通、≤5储值的资料）\"}");
                        return;
                    }
                }

                //第一次下载
                if (extime == "")
                {
                    extime =  DateTime.Now.AddDays(Convert.ToInt32(day)).ToString("yyyy-MM-dd HH:mm:ss");
                    method.editetime(key, extime);  //第一次登录

                  string area=  method.addiplog(key,userIp);  //获取IP信息

                    if (area.Contains("北京市") || area.Contains("学科"))
                    {

                        Response.Write("{\"status\":\"0\",\"msg\":\"服务暂不支持，请咨询客服！\"}");

                        //设置过期
                        extime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                        method.editetime(key, extime); 
                        return;
                    }
                }


                string url = "http://115.190.166.221/xk.aspx?method=kd0ph8fynbhj&id=" + fileid;
               
                
                string html = method.GetUrlWithCookie(url, cookie);
                if(!html.Contains("fileUrl"))
                {
                    url = method.methodurl + fileid;
                    html = method.GetUrlWithCookie(url, cookie);
                }

                string fileurl = Regex.Match(html, @"""fileUrl"":""([\s\S]*?)""").Groups[1].Value.Trim();
                string filename = Regex.Match(html, @"""fileName"":""([\s\S]*?)""").Groups[1].Value.Trim();
                string fileSize = Regex.Match(html, @"""fileSize"":([\s\S]*?),").Groups[1].Value.Trim();
                string provider = Regex.Match(html, @"""provider"":""([\s\S]*?)""").Groups[1].Value.Trim();
                string price = Regex.Match(html, @"""price"":([\s\S]*?),").Groups[1].Value.Trim();
                string shopId = Regex.Match(html, @"""shopId"":([\s\S]*?),").Groups[1].Value.Trim();
                string commercialLevel = Regex.Match(html, @"""commercialLevel"":""([\s\S]*?)""").Groups[1].Value.Trim();
                string gradeId = Regex.Match(html, @"""gradeId"":([\s\S]*?),").Groups[1].Value.Trim();
                string courseId = Regex.Match(html, @"""courseId"":([\s\S]*?),").Groups[1].Value.Trim();
                string stageId = Regex.Match(html, @"""stageId"":([\s\S]*?),").Groups[1].Value.Trim();

                /*"1202","普通"
                "1203","精品"
                "1204","特供"
                "1205","中职"
                "1205","教辅"
                */
                //筛选教辅、中职
                if (commercialLevel=="1205" || stageId=="6")
                {
                    //账号不符合的跳过
                    if (isvip == "0" || isvip == "")
                    {
                        Response.Write("{\"status\":\"0\",\"msg\":\"当前购买的权限不能下载教辅及中职，请拍对应选项（当前账户可下：精品、特供、普通、≤5储值的资料）\"}");
                        return;
                    }
                    //教辅的不超过5块
                    if (price != "")
                    {
                        if (Convert.ToDouble(price) > 5)
                        {
                            Response.Write("{\"status\":\"0\",\"msg\":\"无法下载大于5储值的教辅，请咨询客服\"}");
                            return;
                        }
                    }

                }

               

                //普通的不超过20
                if (price != "")
                {
                    if (Convert.ToDouble(price) > 19)
                    {
                        Response.Write("{\"status\":\"0\",\"msg\":\"当前购买的权限无法下载价格过高资料，请咨询客服\"}");
                        return;
                    }
                }




                if (fileurl == "")
                {
                    Response.Write("{\"status\":\"0\",\"msg\":\"下载失败，登录失效，请联系客服\"}");
                    return;
                }
                else
                {
                    int cishu_new = Convert.ToInt32(cishu) - 1;

                    //原网址下载
                    string protocol = Request.Url.Scheme; // "http" 或 "https"
                    string host = Request.Url.Host;       // 域名或IP
                    int port = Request.Url.Port;
                    string suiji =method.GenerateRandomString(10);
                   
                    string jiamifilekey = method.Base64Encode(Encoding.GetEncoding("utf-8"), fileurl).Replace("a",suiji);
                    string jiamifileurl = protocol + "://" + host + ":" + port + "/download.aspx?filekey=" + jiamifilekey+"&suiji="+suiji;
                    Response.Write("{\"status\":\"1\",  \"cishu\":\"" + cishu_new + "\", \"extime\":\"" + extime + "\",   \"filename\":\"" + filename + "\",\"fileurl\":\"" + jiamifileurl + "\",\"fileSize\":\"" + fileSize + "\",\"msg\":\"下载成功,请查看浏览器下载列表\"}");



                    ////下载文件下载
                    //filename = method.CleanUrlKeepChinese(filename);
                    //filename = method.AddIDToFileName(filename, fileid);

                    //string oss = @"C:\Users\Administrator\Desktop\xueke\oss\";
                    //string ex = method.downloadFile(fileurl, oss, filename);

                    //Response.Write("{\"status\":\"1\",  \"cishu\":\"" + cishu_new + "\", \"extime\":\"" + extime + "\",   \"filename\":\"" + filename + "\",\"fileurl\":\"" + filepath + "\",\"fileSize\":\"" + ex.ToString() + "\",\"msg\":\"下载成功,请查看浏览器下载列表\"}");
                  
                   
                    method.adddownlog(key,link,isvip,cishu,day,price, commercialLevel, gradeId, courseId, stageId); //下载成功  添加下载记录
                   
                    method.editekey(key); //下载成功  减去次数
                }

             


            }
            else
            {
                Response.Write("{\"status\":\"0\",\"msg\":\"秘钥错误或不存在，请联系客服！\"}");
                mycon.Close();
                reader.Close();

            }



        }
        #endregion

       


        


        

        

        

        

        






        






    }
}