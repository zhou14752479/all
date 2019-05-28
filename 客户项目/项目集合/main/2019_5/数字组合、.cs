using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            Control.CheckForIllegalCrossThreadCalls = false;
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
            
            for (var i = 0; i < 999999; i++)
            {


                if (arr.Count == n)
                {
                    break;
                }
                else
                {
                    Random random = new Random();
                    var shuzhi = random.Next(max);
                    if (!arr.Contains(shuzhi)&& shuzhi !=0)
                    {
                        arr.Add(shuzhi);
                    }
                }
            }
            return arr;
        }



        public void run()
        {
            //Int32[] arrs = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48 };

            listView1.Items.Clear();

          
            for (int i= 0; i < 400; i++)
            {
                ArrayList arr = getarray(48, 24);

                StringBuilder sb = new StringBuilder();

                for (int j = 0; j < arr.Count; j++)
                {

                    sb.Append(","+arr[j].ToString());
                }
                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                lv1.SubItems.Add(sb.ToString());
                if (this.listView1.Items.Count > 2)
                {
                    this.listView1.EnsureVisible(this.listView1.Items.Count - 1);
                }
            }
        }

       

        private void 数字组合_Load(object sender, EventArgs e)
        {
          
        }

        private void Button1_Click(object sender, EventArgs e)
        {
           
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
           

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }


     

        private void Button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(listView1.Items.Count.ToString());
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].BackColor = Color.White;
            }



            for (int i =0; i < listView1.Items.Count; i++)
            {
               
                
               
                    if (listView1.Items[i].SubItems[1].Text .Contains(","+textBox1.Text.Trim()+","))

                    {
                        listView1.Items[i].BackColor = Color.Red;
                    }
             
            }

         //   foreach (int item in list)
         //   {
         //       for (int i = 0; i < listView1.Items.Count; i++)
         //       {
         //           if (listView1.Items[i].SubItems[0].Text.Trim() == item.ToString().Trim())
         //           {
         //               listView1.Items.RemoveAt(i);
         //           }

         //       }
         //}
               
           




        }

        private void Button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].BackColor!=Color.Red)
                    {
                       listView1.Items.RemoveAt(i);
                  }
            }

        }

       
    }
}
