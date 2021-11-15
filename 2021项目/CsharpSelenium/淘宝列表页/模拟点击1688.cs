using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotRas;
using myDLL;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace 淘宝列表页
{
    public partial class 模拟点击1688 : Form
    {
        public 模拟点击1688()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

   
      
    
        public void run()
        {
            try
            {
                ChromeOptions options = new ChromeOptions();
              
                //设置user agent为iPhone5
               // options.AddArgument("--user-agent=Mozilla/5.0 (Linux; U; Android 8.1.0; zh-cn; BLA-AL00 Build/HUAWEIBLA-AL00) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/57.0.2987.132 MQQBrowser/8.9 Mobile Safari/537.36");
                //实例化chrome对象，并加入选项
                WebDriver driver = new ChromeDriver(options);
              
                //driver.Manage().Window.Maximize();
                string loginurl = "https://m.1688.com/search.html";

                driver.Navigate().GoToUrl(loginurl);

                StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Replace("\"","").Split(new string[] { "\r\n" }, StringSplitOptions.None);
                MessageBox.Show(text.Length.ToString());
                for (int i = 1; i < text.Length; i++)
                {
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;

                    string[] value = text[i].Split(new string[] { "," }, StringSplitOptions.None);
                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(value[0]);
                    lv1.SubItems.Add(value[1]);
                    lv1.SubItems.Add(value[2]);
                    if (value[1] != "")
                    {

                        try
                        {


                            string url = "https://m.1688.com/offer_search/-6D7033.html?keywords=" + value[1];
                            driver.Navigate().GoToUrl(url);
                            Thread.Sleep(1000);
                            //IList<IWebElement> listOption = driver.FindElements(By.ClassName("item-link"));
                            //foreach (var item in listOption)
                            //{
                            //    if (item.GetAttribute("href").Contains(value[0]))
                            //    {
                            //        Thread.Sleep(Convert.ToInt32(textBox4.Text)*1000);
                            //        //MessageBox.Show(item.TagName);
                            //        //MessageBox.Show(item.Text);
                            //        // MessageBox.Show(item.GetAttribute("href"));
                            //        // document.getElementById(“test”).scrollIntoView();
                            //        Actions actions = new Actions(driver);
                            //        actions.MoveToElement(item);
                            //        actions.Perform();
                            //        Thread.Sleep(1000);
                            //        item.Click();

                            //    }

                            //}

                            //driver.Navigate().GoToUrl(value[2]);

                            lv1.SubItems.Add("成功");


                            // ADSLHelper.Disconnect("宽带连接");
                            // ADSLHelper.Connect("宽带连接", textBox2.Text.Trim(), textBox3.Text.Trim()

                            Thread.Sleep(Convert.ToInt32(textBox5.Text) * 1000);


                            //ADSL.RASDisplay ras = new ADSL.RASDisplay();
                            //    ras.Disconnect();//断开连接
                            //    Thread.Sleep(3000);
                            //    ras.Connect("ADSL");//重新拨号



                            Thread.Sleep(Convert.ToInt32(textBox6.Text) * 1000);





                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                            Thread.Sleep(3000);
                            continue;
                        }
                    }
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
               
               
            }
            catch (Exception ex)
            {

                ex.ToString();
            }

        }

      


        public static class ADSLHelperNoneedPass
        {
            /// <summary>
            /// 断开连接
            /// </summary>
            public static void Unlink()
            {
                ReadOnlyCollection<RasConnection> conList = RasConnection.GetActiveConnections();
                foreach (RasConnection con in conList)
                {
                    con.HangUp();
                }
            }

            /// <summary>
            /// 宽带连接
            /// </summary>
            /// <returns></returns>
            public static bool boolLink()
            {
                try
                {
                    RasDialer dialer = new RasDialer();
                    dialer.EntryName = "宽带连接";
                    dialer.PhoneNumber = " ";
                    
                    dialer.AllowUseStoredCredentials = true;
                    dialer.PhoneBookPath = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.AllUsers);
                    dialer.Timeout = 2000;
                    dialer.Dial();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return false;
                }
                return true;
            }
        }


        class ADSLHelper
        {
            public static string Connect(string connectionName, string user, string pass)
            {
                string arg = string.Format("rasdial \"{0}\" {1} {2}", connectionName, user, pass);
              string ExitCode= InvokeCmd(arg);
                return ExitCode;
            }
            public static string  Disconnect(string connectionName)
            {
                string arg = string.Format("rasdial \"{0}\" /disconnect", connectionName);
                InvokeCmd(arg);
                string ExitCode = InvokeCmd(arg);
                return ExitCode;
            }
            public  static string InvokeCmd(string cmdArgs)
            {
                Process p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.StandardInput.WriteLine(cmdArgs);
                p.StandardInput.WriteLine("exit");

                // return p.StandardOutput.ReadToEnd();

                //0拨号成功
                return p.ExitCode.ToString();
            }

        }




        private void 模拟点击1688_Load(object sender, EventArgs e)
        {

        }
        Thread thread;
        bool zanting = true;
        bool status = true;
      
        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("未导入文件");
                return;
            }

            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

       

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ADSLHelper.Disconnect("宽带连接");
            ADSLHelper.Connect("宽带连接", textBox2.Text.Trim(), textBox3.Text.Trim());

            //Method.Unlink();
            //Method.boolLink();

        }
    }
}
