using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class buy : System.Web.UI.Page
    {
        public string username { get; set; }
        public int points { get; set; }
        public int change { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_Click(object sender, EventArgs s)
        {
            string user = Request.Form["username"].ToString();
            this.username = user;
            this.points = method.getPoints(user);
        }

        protected void buy_Click(object sender, EventArgs s)
        {
            string user = Request.Form["username"].ToString();
            string changePoint= Request.Form["docVlGender"].ToString();
            
            method.addPoints(user, method.getPoints(user),Convert.ToInt32(changePoint));
        }
    }
}