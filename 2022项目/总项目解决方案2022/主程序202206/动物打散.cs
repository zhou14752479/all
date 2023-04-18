using System;
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

namespace 主程序202206
{
    public partial class 动物打散 : Form
    {
        public 动物打散()
        {
            InitializeComponent();
        }
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            //if (thread == null || !thread.IsAlive)
            //{
            //    thread = new Thread(run);
            //    thread.Start();
            //    Control.CheckForIllegalCrossThreadCalls = false;
            //}
            run();
        }
        public static List<string> ListRandom(List<string> sources)
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            var resultList = new List<string>();
            foreach (var item in sources)
            {
                resultList.Insert(random.Next(resultList.Count+1), item);
            }
            return resultList;
        }

        public void run()
        {
            try
            {
                string input=  Regex.Replace(textBox1.Text.Replace("各", ""), @"\d{1,}", "");
                string v = Regex.Match(textBox1.Text, @"\d{1,}").Groups[0].Value;
                string[] arr= input.Split(new string[] { "，" }, StringSplitOptions.None);


                int sum = arr.Length * Convert.ToInt32(v);



               // int all = 0;
                List<string> lists = new List<string>();
                for (int i = 0; i < arr.Length; i++)
                {

                    Random random = new Random(Guid.NewGuid().GetHashCode());
                    int value = random.Next(10, Convert.ToInt32(v) +5);


                    if (i == arr.Length - 1)
                    {
                        value = 20;
                    }
                    else if(i == arr.Length - 2)
                    {
                        value = sum-20;
                    }
                    else
                    {
                        sum = sum - value;
                    }


                    lists.Add(arr[i] + value);


                    //all = all + value;
                    //textBox1.Text = all.ToString();

                }
                List<string> resultlist = ListRandom(lists);


              

               StringBuilder sb = new StringBuilder();
              

                foreach (string s in resultlist)
                {
                   
                    sb.Append(s+",");    
                }

                textBox2.Text = sb.ToString();

                System.Windows.Forms.Clipboard.SetText(sb.ToString()); //复制

            }
            catch (Exception ex)
            {
                //textBox2.Text == "输入错误";
                textBox2.Text= ex.ToString();
            }
        }


    }
}
