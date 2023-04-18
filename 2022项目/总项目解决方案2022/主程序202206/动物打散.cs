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


              

             
                List<string> lists = new List<string>();
                for (int i = 0; i < arr.Length; i++)
                {
                    int sum = Convert.ToInt32(v);

                    for (int j= 0; j < 4; j++)
                    {
                        Random random = new Random(Guid.NewGuid().GetHashCode());
                        int value = random.Next(1, Convert.ToInt32(v));
                       
                        if(j==3)
                        {
                            value = sum;
                        }
                        sum = sum - value;
                        if (sum>0)
                        {
                            lists.Add(arr[i] + value);
                           
                        }
                        
                        if(sum<=0)
                        {
                            sum = sum + value;
                            lists.Add(arr[i] + sum);
                            break;
                        }
                    }


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
