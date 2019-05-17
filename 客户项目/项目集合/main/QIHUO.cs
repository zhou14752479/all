using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main
{
    public class QIHUO
    {

        /// <summary>
        /// 商品名称
        /// </summary>
        public string name { set; get; }
        /// <summary>
        /// 交割月份
        /// </summary>
        public string month { set; get; }
       
        /// <summary>
        /// 开盘价
        /// </summary>
        public string open { set; get; }
        /// <summary>
        /// 最高
        /// </summary>
        public string high { set; get; }
        /// <summary>
        /// 最低
        /// </summary>
        public string low { set; get; }
        /// <summary>
        /// 收盘价
        /// </summary>
        public string close { set; get; }
        /// <summary>
        /// 前结算价
        /// </summary>
        public string qjsj{ set; get; }
        /// <summary>
        /// 结算价
        /// </summary>
        public string jsj { set; get; }
        /// <summary>
        /// 涨跌
        /// </summary>
        public string zd { set; get; }
        /// <summary>
        /// 涨跌一
        /// </summary>
        public string zd1 { set; get; }
        /// <summary>
        /// 成交量
        /// </summary>
        public string cjl { set; get; }
        /// <summary>
        /// 持仓量
        /// </summary>
        public string ccl { set; get; }
        /// <summary>
        /// 持仓量变化
        /// </summary>
        public string change { set; get; }

        /// <summary>
        /// 成交额
        /// </summary>
        public string cje { set; get; }
        /// <summary>
        /// 时间
        /// </summary>
        public string time { set; get; }
    }
}
