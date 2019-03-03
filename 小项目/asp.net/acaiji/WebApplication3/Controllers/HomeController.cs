
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

                 
            
            return View();

          

        }

        public ActionResult List()
        {



            return View();



        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            string sql = "select * from vip_user";
            DataTable da = method.GetDataTable(sql, CommandType.Text);
            List<Home> lists = new List<Home>();

            if (da.Rows.Count > 0)
            {
                Home home = null;
                foreach (DataRow row in da.Rows)
                {
                    home = new Home();

                    home.Id = Convert.ToInt32(row["id"]);
                    home.UserName = row["user"].ToString();

                    home.PassWord = row["password"].ToString();

                    home.Mac = row["mac"].ToString();
                    home.Ip = row["ip"].ToString();

                    lists.Add(home);
                }

                ViewData["datas"] = lists;
            }


            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Register()
        {
            

            return View();
        }
        public ActionResult Login()
        {


            return View();
        }

        public ActionResult doRegister()

        {
            string username = Request["username"];  //直接拿值
            string password = Request["password"];
            string code = Request["code"];
            string mac = method.GetMacAddress();
            string ip = method.GetIp();
            if (username != string.Empty && password != string.Empty && code==method.Random())

            {

                string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {

                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "insert into vip_user(user,password,time,ip,mac)values(@username,@password,@time,@ip,@mac)";

                        MySqlParameter[] pars = {
                                      new MySqlParameter("@username",MySqlDbType.VarChar,64),
                                       new MySqlParameter("@password",MySqlDbType.VarChar,64),
                                       new MySqlParameter("@time",MySqlDbType.VarChar,100),
                                       new MySqlParameter("@ip",MySqlDbType.VarChar,100),
                                       new MySqlParameter("@mac",MySqlDbType.VarChar,100),

                                      };
                        pars[0].Value = username;
                        pars[1].Value = password;
                        pars[2].Value = DateTime.Now;
                        pars[3].Value = ip;
                        pars[4].Value = mac;

                        cmd.Parameters.AddRange(pars);

                        conn.Open();
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            // return Content("注册成功");

                            ViewBag.Message = "注册成功";
                            return View();

                        }
                        else
                        {
                            
                            return Content("注册失败");
                           


                        }
                    }
                }

            }

            else
            {
                // return Content("您的注册码错误！");
                ViewBag.Message = "注册码错误！";
                return View();

            }

          

        }
        public ActionResult doLogin()
        {
            //return Content(id);上方括号内需要填入id
            string username = Request["username"];  //直接拿值
            string password = Request["password"];


            HttpCookie cookie = new HttpCookie("logmessages");
          
            //cookie.Name = "Name";
            cookie.Expires = DateTime.Today.AddDays(365);
            cookie.Values.Add("username",username);
            Response.Cookies.Add(cookie);

            string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            //根据接收到ID查询数据表中相应的记录.
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();

            MySqlCommand cmd = new MySqlCommand("select * from vip_user where user= '" + username + "'", conn);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

            //MySqlParameter[] pars = { new MySqlParameter("@username", MySqlDbType.VarChar, 64) };

            //pars[0].Value = username;

            MySqlParameter par = new MySqlParameter("@username", MySqlDbType.VarChar);
            par.Value = username;

            MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源




            if (reader.Read())

            {

                string pass = reader["password"].ToString().Trim(); //获取数据库对应的password

                if (pass == password)

                {
                    
                     return Content("登陆成功!");
                    


                }
                else
                {

                    return Content("密码错误!");
                   // return RedirectToAction("Index", "Home");

                }

            }

            else
            {
               return Content("您输入的账户不存在!!");
               
            }


        }

        public ActionResult news()
        {
            string sql = "select count(*) from news";            
            int allcount = Convert.ToInt32(method.ExecuteScalar(sql, CommandType.Text));  //获取数据总数


            
            int pageCount = Convert.ToInt32(Math.Ceiling((double)allcount / 10));  //获取总页数


            int pageIndex;   //当前页码
            if (!int.TryParse(Request.QueryString["pageIndex"], out pageIndex))
            {
                pageIndex = 1;
            }

            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            pageIndex = pageIndex > pageCount ? pageCount : pageIndex;
            int start = (pageIndex - 1) * 10 + 1;



            string sql2 = "select * from news limit @startnum , @num";                    // limit m,n --m：表示从哪一行开始查，n:查询多少条
            MySqlParameter[] pars = {
                                  new MySqlParameter("@startnum",MySqlDbType.Int32),
                                  new MySqlParameter("@num",MySqlDbType.Int32)

                                  };
            pars[0].Value = start;
            pars[1].Value = 10; //每页的数量

            DataTable da = method.GetDataTable(sql2, CommandType.Text, pars);
            List<News> lists = new List<News>();

            if (da.Rows.Count > 0)
            {
                News news = null;
                foreach (DataRow row in da.Rows)
                {
                    news = new News();

                    news.Id = Convert.ToInt32(row["id"]);
                    news.Title = row["title"].ToString();

                    news.Body = row["body"].ToString();

                    news.Time = row["time"].ToString();
                  

                    lists.Add(news);
                }

                
                ViewData["datas"] = lists;
                ViewData["pageIndex"] = pageIndex;
                ViewData["pageCount"] = pageCount;

            }


            return View();

            
        }
        public ActionResult showDetail(int id)
        {
            string sql = "select * from news where id =" +id;
            DataTable da = method.GetDataTable(sql, CommandType.Text);
            List<News> lists = new List<News>();

            if (da.Rows.Count > 0)
            {
                News news = null;
                foreach (DataRow row in da.Rows)
                {
                    news = new News();

                    news.Id = Convert.ToInt32(row["id"]);
                    news.Title = row["title"].ToString();

                    news.Body = row["body"].ToString();

                    news.Time = row["time"].ToString();

                    lists.Add(news);
                }

                ViewData["datas"] = lists;
            }


            return View();
        }

    }
}