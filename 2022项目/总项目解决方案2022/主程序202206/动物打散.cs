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
                string[] arr= input.Split(new string[] { "，" }, StringSplitOptions.None);


                string v = Regex.Match(textBox1.Text, @"\d{1,}").Groups[0].Value;
                List<string> lists = new List<string>();
                foreach (string s in arr)
                {
                    Random random = new Random(Guid.NewGuid().GetHashCode());
                    int value = random.Next(0, Convert.ToInt32(v));
                    lists.Add(s + value);
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

                textBox2.Text="输入错误";
            }
        }


    }
}
