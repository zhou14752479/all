using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DAL;

namespace BAL
{
    public class dataListBal
    {
        
        /// <summary>
        /// 返回数据列表
        /// </summary>
        /// <returns></returns>
        public List<dataList> GetList()
        {
            return dataListDal.GetList();

        }
        /// <summary>
        /// 计算获取数据的访问，完成分页
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示的记录数据</param>
        /// <returns></returns>
        public List<dataList> GetPageList(int pageIndex, int pageSize)
        {
            int start = (pageIndex - 1) * pageSize + 1;
            
            return dataListDal.GetPageList(start, pageSize);

        }
        /// <summary>
        /// 获取总的页数
        /// </summary>
        /// <param name="pageSize">每页显示的记录数</param>
        /// <returns></returns>
        public int GetPageCount(int pageSize)
        {
            int recoredCount = dataListDal.GetRecordCount();//获取总的记录数.
            int pageCount = Convert.ToInt32(Math.Ceiling((double)recoredCount / pageSize));
            return pageCount;
        }


    }
}
