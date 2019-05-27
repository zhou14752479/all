using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main._2019_5
{
    public partial class 数字组合 : Form
    {
        public 数字组合()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 返回随机数组
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <param name="count">个数</param>
        /// <returns></returns>

        public ArrayList getarray(int max,int n)
        {
            ArrayList arr = new ArrayList();
            
            for (var i = 0; i < 99999; i++)
            {
                Random random = new Random();
                var shuzhi= random.Next(max);
                if (!arr.Contains(shuzhi))
                {
                    arr.Add(shuzhi);
                }

                if (arr.Count == n)
                    break;
            }
            return arr;
        }



        public void run()
        {
            Int32[] arrs = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48 };

            listView1.Items.Clear();

            ArrayList arr = getarray(48, 24);
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < arr.Count; j++)
            {

                sb.Append(arr[j].ToString()+" ");
            }
            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
            lv1.SubItems.Add(sb.ToString());
        }

        private void 数字组合_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;


        }
    }
}
