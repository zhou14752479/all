using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using helper;


namespace 商标生成
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool zanting = true;
        

        #region 注册人
        public void run()
        {
            try

            {
             
                    
                    string url = "";

                   
                    string html = method.GetUrl(url, "utf-8");

                if (html.Contains("不足"))
                {
                    MessageBox.Show("当前APPKEY剩余次数不足");
                    return;
                }
                   

                    MatchCollection a1 = Regex.Matches(html, @"""appDate"":""([\s\S]*?)""");
                    MatchCollection a2 = Regex.Matches(html, @"""applicantCn"":""([\s\S]*?)""");
                    MatchCollection a3 = Regex.Matches(html, @"""currentStatus"":""([\s\S]*?)""");
                    MatchCollection a4 = Regex.Matches(html, @"""intCls"":""([\s\S]*?)""");
                    MatchCollection a5 = Regex.Matches(html, @"""regNo"":""([\s\S]*?)""");
                    MatchCollection a6 = Regex.Matches(html, @"""tmName"":""([\s\S]*?)""");

                
                for (int i = 0; i < a1.Count; i++)
                    {
                     

                    }


                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    Thread.Sleep(500);

                }


            
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region 注册号
        public void run1()
        {
            try

            {

                string[] text = "".Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {

                    string[] regnos = text[i].Split(new string[] { "," }, StringSplitOptions.None);
                    string url = "http://v.juhe.cn/trademark/detail?regNo=" + regnos[0] + "&intCls=" + regnos[1] + "&key=";
                    string html = method.GetUrl(url, "utf-8");


                    Match a1 = Regex.Match(html, @"""regNo"":""([\s\S]*?)""");
                    Match a2 = Regex.Match(html, @"""tmName"":""([\s\S]*?)""");
                    Match a3 = Regex.Match(html, @"""intCls"":""([\s\S]*?)""");
                    Match a4 = Regex.Match(html, @"""applicantCn"":""([\s\S]*?)""");
                    Match a5 = Regex.Match(html, @"""addressCn"":""([\s\S]*?)""");
                    Match a6 = Regex.Match(html, @"""appDate"":""([\s\S]*?)""");
                    Match a7 = Regex.Match(html, @"""agent"":""([\s\S]*?)""");

                    Match img = Regex.Match(html, @"""tmImg"":""([\s\S]*?)""");

                  



                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    Thread.Sleep(500);

                }
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {

        }

      

        private void button2_Click(object sender, EventArgs e)
        {
           
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run1));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

     
      

      
    }
}
