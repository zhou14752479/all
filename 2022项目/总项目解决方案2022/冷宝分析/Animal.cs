using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 冷宝分析
{
    public class Animal
    {
       

        public string Name
        {
            get;
            set;
        }
  

        public int WeiKaiChangShu
        {
            get;
            set;
        }

        public int WeiKaiChangShuPianYi
        {
            get;
            set;
        }

        public int WeiKaiTianShu
        {
            get;
            set;
        }

        public int WeiKaiTianShuPianYi
        {
            get;
            set;
        }

        public DateTime Createtime
        {
            get;
            set;
        } = DateTime.Now;

    }
}
