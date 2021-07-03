using BAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class datalist : System.Web.UI.Page
    {
        public int PageIndex { get; set; }
        public int PageCount { get; set; }

        public List<dataList> Datas { get; set; }  //定义一个list让aspx继承的页面可以获取到。

        protected void Page_Load(object sender, EventArgs e)
        {
            int pageSize = 10;
            int pageIndex;
            if (!int.TryParse(Request.QueryString["pageIndex"], out pageIndex))
            {
                pageIndex = 1;
            }

            dataListBal datalistbal = new dataListBal();
            int pagecount = datalistbal.GetPageCount(pageSize);//获取总页数
            PageCount = pagecount;
            //对当前页码值范围进行判断
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            pageIndex = pageIndex > pagecount ? pagecount : pageIndex;
            PageIndex = pageIndex;
            //List<dataList> list = datalistbal.GetList();//获取所有数据
            List<dataList> list = datalistbal.GetPageList(pageIndex, pageSize);//获取分页数据
            Datas = list;                  //将list传给公共集合Lists
        }
    
    }
}