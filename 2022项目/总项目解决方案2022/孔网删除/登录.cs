using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 孔网删除
{
    public partial class 登录 : Form
    {
        public 登录()
        {
            InitializeComponent();
        }

        public static string cookie = "";
        private void 登录_Load(object sender, EventArgs e)
        {
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;

            webBrowser1.Navigate("https://login.kongfz.com/?mustLogin=1&returnUrl=https%3A%2F%2Fseller.kongfz.com%2Fshop%2Fitem.html%3Fhash%3Dunsold_5862");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cookie = method.GetCookies("https://seller.kongfz.com/pc/item/unsoldList?type=unsold&itemName=&author=&press=&itemSn=&isbn=9787565924668&price_min=&price_max=&addTime_begin=&addTime_end=&soldTime_begin=&soldTime_end=&catId=&myCatId=&myCatIdName=&quality=&qualityName=&isDiscount=false&noPic=false&noStock=false&freeShipping=false&noMouldId=false&noItemSn=false&complete=true&catName=&mouldId=&mouldName=&pageShow=50&order_attr=&order_sort=&_=1645519862311");
            if(cookie!="")
            {
                this.Hide();
            }
        }
    }
}
