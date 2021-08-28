using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 表格合并
{
    public partial class 表格合并 : Form
    {
        public 表格合并()
        {
            InitializeComponent();
        }
        internal class Table
        {
          
            public string 关键词 { get; set; }

            
            public string 搜索人气 { get; set; }

           
            public string 搜索指数 { get; set; }
            public string 点击人气 { get; set; }
            public string 点击指数 { get; set; }

            public string 点击率 { get; set; }

            public string 成交金额指数 { get; set; }

           
            public string 成交单量指数 { get; set; }

         
            public string 成交转化率 { get; set; }

          
            public string 全网商品数 { get; set; }
            public string 在线商品数 { get; set; }
            public string 蓝海值{ get; set; }

            public string 潜力值 { get; set; }

  
            public string 最优品类名称 { get; set; }


            public string 快车参考价 { get; set; }


            public string 计算列 { get; set; }

            public string X { get; set; }
        }

        private static string sourcPath = "source";
        public  void run()
        {
            string[] fileNames = Directory.GetFiles(sourcPath);
            List<Table> list = new List<Table>();
            foreach (string fileName in fileNames)
            {
                body_text.Text+=("开始合并:" + fileName)+"\r\n";
                using (NPExcelHelper helper = new NPExcelHelper(fileName))
                {
                    for (int i = 1; i < 100000; i++)
                    {
                        try
                        {
                            bool flag = string.IsNullOrEmpty(helper.ReadCell(0, i, 0));
                            if (flag)
                            {
                                break;
                            }
                        }
                        catch
                        {
                            break;
                        }
                        Table table = new Table();
                        table.关键词 = helper.ReadCell(0, i, 0);
                        table.搜索人气 = helper.ReadCell(0, i, 1);
                        table.搜索指数 = helper.ReadCell(0, i, 2);
                        table.点击人气 = helper.ReadCell(0, i, 3);
                        table.点击指数 = helper.ReadCell(0, i, 4);
                        table.点击率 = helper.ReadCell(0, i, 5);
                        table.成交金额指数 = helper.ReadCell(0, i, 6);
                        table.成交单量指数 = helper.ReadCell(0, i, 7);
                        table.成交转化率 = helper.ReadCell(0, i, 8);
                        table.全网商品数 = helper.ReadCell(0, i, 9);
                        table.在线商品数 = helper.ReadCell(0, i, 10);
                        table.蓝海值 = helper.ReadCell(0, i, 11);
                        table.潜力值 = helper.ReadCell(0, i, 12);
                        table.快车参考价 = helper.ReadCell(0, i, 13);
                        table.最优品类名称 = helper.ReadCell(0, i, 14);
                       
                     
                        list.Add(table);
                    }
                }
            }
            NPExcelHelper.CreateExcel("result/result.xls");
            using (NPExcelHelper helper2 = new NPExcelHelper("result/result.xls"))
            {
                int j = 0;
                helper2.SetCell(0, 0, 0, "关键词");
                helper2.SetCell(0, 0, 1, "搜索人气");
                helper2.SetCell(0, 0, 2, "搜索指数");
                helper2.SetCell(0, 0, 3, "点击人气");
                helper2.SetCell(0, 0, 4, "点击指数");
                helper2.SetCell(0, 0, 5, "点击率");
                helper2.SetCell(0, 0, 6, "成交金额指数");
                helper2.SetCell(0, 0, 7, "成交单量指数");
                helper2.SetCell(0, 0, 8, "成交转化率");
                helper2.SetCell(0, 0, 9, "全网商品数");
                helper2.SetCell(0, 0, 10, "在线商品数");
                helper2.SetCell(0, 0, 11, "蓝海值");
                helper2.SetCell(0, 0, 12, "潜力值");
                helper2.SetCell(0, 0, 13, "快车参考价");
                helper2.SetCell(0, 0, 14, "最优品类名称");
              
                foreach (Table item in list)
                {
                    j++;
                    helper2.SetCell(0, j, 0, item.关键词);
                    helper2.SetCell(0, j, 1, item.搜索人气.ToString());
                    helper2.SetCell(0, j, 2, item.搜索指数.ToString());
                    helper2.SetCell(0, j, 3, item.点击人气.ToString());
                    helper2.SetCell(0, j, 4, item.点击指数.ToString());
                    helper2.SetCell(0, j, 5, item.点击率);
                    helper2.SetCell(0, j, 6, item.成交金额指数.ToString());
                    helper2.SetCell(0, j, 7, item.成交单量指数.ToString());
                    helper2.SetCell(0, j, 8, item.成交转化率);
                    helper2.SetCell(0, j, 9, item.全网商品数.ToString());
                    helper2.SetCell(0, j, 10, item.在线商品数.ToString());
                    helper2.SetCell(0, j, 11, item.蓝海值.ToString());
                    helper2.SetCell(0, j, 12, item.潜力值.ToString());
                    helper2.SetCell(0, j, 13, item.快车参考价);
                    helper2.SetCell(0, j, 14, item.最优品类名称);
                  
                }
                helper2.Write();
            }
            body_text.Text+=("完成")+"\r\n";
            

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (DateTime.Now > Convert.ToDateTime("2021-09-09"))
            {
                return;
            }
            run();
        }
    }
}
