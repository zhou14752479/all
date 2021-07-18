using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using myDLL;

namespace 地图营销
{
    class map_method
    {
        public bool login(string user,string pass)
        {
          
            string html = method.GetUrl("http://www.acaiji.com:8080/api/mt/login.html?username=" + user + "&password=" + pass + "&code=1&", "utf-8");
            if (html.Contains("成功"))
            {   
              return true;
               
            }
            else
            {
                return false;
            };


        }


    }
}
